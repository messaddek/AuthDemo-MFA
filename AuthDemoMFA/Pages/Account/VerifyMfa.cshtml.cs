using System.Security.Claims;
using AuthDemo.Repositories;
using AuthDemoMFA.Models;
using AuthDemoMFA.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemoMFA.Pages.Account
{
    public class VerifyMfaModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IMfaService _mfaService;

        public VerifyMfaModel(IUserRepository userRepository, IMfaService mfaService)
        {
            _userRepository = userRepository;
            _mfaService = mfaService;
        }

        [BindProperty]
        public VerifyMfaViewModel Input { get; set; } = new();

        [TempData]
        public string? Username { get; set; }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(Username))
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(Username))
                return Page();

            var user = _userRepository.GetByUsername(Username);
            if (user == null || !user.IsMfaEnabled || string.IsNullOrEmpty(user.MfaSecretKey))
                return RedirectToPage("/Account/Login");

            if (!_mfaService.VerifyCode(user.MfaSecretKey, Input.Code))
            {
                ModelState.AddModelError(string.Empty, "Invalid verification code");
                return Page();
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
}
