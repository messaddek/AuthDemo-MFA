using AuthDemo.Repositories;
using AuthDemoMFA.Models;
using AuthDemoMFA.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AuthDemoMFA.Helpers;

namespace AuthDemoMFA.Pages.Account
{
    public class EnableMfaModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IMfaService _mfaService;

        public EnableMfaModel(IUserRepository userRepository, IMfaService mfaService)
        {
            _userRepository = userRepository;
            _mfaService = mfaService;
        }

        [BindProperty]
        public EnableMfaViewModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return RedirectToPage("/Account/Login");

            var username = User.Identity!.Name;
            var user = _userRepository.GetByUsername(username!);

            if (user == null)
                return NotFound();

            if (user.IsMfaEnabled)
                return RedirectToPage("/Index");

            var secretKey = _mfaService.GenerateSecretKey();
            var qrCodeUri = _mfaService.GenerateQrCodeUri(secretKey, username!);

            Input = new EnableMfaViewModel
            {
                SecretKey = secretKey,
                QrCodeUri = qrCodeUri,
                QrCodeBase64 = QrCodeHelper.GenerateQrCodeBase64(qrCodeUri)
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var username = User.Identity?.Name;
            var user = _userRepository.GetByUsername(username!);

            if (user == null)
                return NotFound();

            if (!_mfaService.VerifyCode(Input.SecretKey, Input.VerificationCode))
            {
                ModelState.AddModelError(string.Empty, "Verification code is invalid");
                return Page();
            }

            user.IsMfaEnabled = true;
            user.MfaSecretKey = Input.SecretKey;
            _userRepository.Update(user);

            return RedirectToPage("/Index");
        }
    }

}
