using System.ComponentModel.DataAnnotations;
using AuthDemo.Repositories;
using AuthDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemoMFA.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public ChangePasswordModel(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public ChangePasswordViewModel Input { get; set; } = new();

        public class ChangePasswordViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current Password")]
            public string CurrentPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "New Password")]
            [StringLength(100, MinimumLength = 6)]
            public string NewPassword { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm New Password")]
            [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return RedirectToPage("/Account/Login");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var user = _userRepository.GetByUsername(User.Identity?.Name!);
            if (user == null)
                return NotFound();

            if (!_passwordHasher.VerifyPassword(Input.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect");
                return Page();
            }

            user.PasswordHash = _passwordHasher.HashPassword(Input.NewPassword);
            _userRepository.Update(user);

            // Add success message
            TempData["StatusMessage"] = "Your password has been changed successfully.";
            return RedirectToPage("./ManageProfile");
        }
    }
}
