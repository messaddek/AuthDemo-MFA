// Pages/Account/Register.cshtml.cs
using System.ComponentModel.DataAnnotations;
using AuthDemo.Models;
using AuthDemo.Repositories;
using AuthDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterModel(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();

    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (_userRepository.GetByUsername(Input.Username) != null)
        {
            ModelState.AddModelError(string.Empty, "Username already taken");
            return Page();
        }

        var user = new User
        {
            Username = Input.Username,
            PasswordHash = _passwordHasher.HashPassword(Input.Password),

        };

        _userRepository.Add(user);

        return RedirectToPage("Login");
    }
}