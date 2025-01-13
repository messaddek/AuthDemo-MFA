// Pages/Account/ManageProfile.cshtml.cs
using AuthDemo.Models;
using AuthDemo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthDemoMFA.Pages.Account
{
    public class ManageProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public ManageProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? CurrentUser { get; set; }

        public IActionResult OnGet()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return RedirectToPage("/Account/Login");

            CurrentUser = _userRepository.GetByUsername(User.Identity!.Name!);
            if (CurrentUser is null)
                return NotFound();

            return Page();
        }
    }
}