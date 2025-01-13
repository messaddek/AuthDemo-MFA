// Pages/Account/Login.cshtml.cs
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AuthDemo.Repositories;
using AuthDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginModel(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = _userRepository.GetByUsername(Input.Username);
        if (user == null || !_passwordHasher.VerifyPassword(Input.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return Page();
        }

        if (user.IsMfaEnabled)
        {
            // Store username in TempData for the MFA verification page
            TempData["Username"] = user.Username;
            return RedirectToPage("./VerifyMfa");
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToPage("/Index");
    }
}