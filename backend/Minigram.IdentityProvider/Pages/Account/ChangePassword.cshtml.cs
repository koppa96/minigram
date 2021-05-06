using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Minigram.Dal.Entities;

namespace Minigram.IdentityProvider.Pages.Account
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<MinigramUser> userManager;

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string OldPassword { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakterből kell állnia")]
        [BindProperty]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string ConfirmPassword { get; set; } = "";

        public bool ChangeSuccessful { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public ChangePasswordModel(UserManager<MinigramUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "cancel" || action == "finish")
            {
                return Redirect(ReturnUrl);
            }

            if (ConfirmPassword != Password)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var changeResult = await userManager.ChangePasswordAsync(user, OldPassword, Password);
                ChangeSuccessful = changeResult.Succeeded;
                if (!changeResult.Succeeded)
                {
                    if (changeResult.Errors.Any(x => x.Code == "InvalidPassword"))
                    {
                        ModelState.AddModelError(nameof(OldPassword), "Helytelen jelszó");
                    }

                    foreach (var error in changeResult.Errors.Where(x => x.Code != "InvalidPassword" && 
                        x.Code.ToLower().Contains(nameof(Password).ToLower())))
                    {
                        ModelState.AddModelError(nameof(Password), error.Description);
                    }
                }
            }

            return Page();
        }
    }
}