using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Minigram.Dal.Entities;

namespace Minigram.IdentityProvider.Pages.Account
{
    public class GetPasswordResetModel : PageModel
    {
        private readonly UserManager<MinigramUser> userManager;

        [Required(ErrorMessage = "Kötelező")]
        [EmailAddress(ErrorMessage = "Nem érvényes e-mail cím")]
        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public bool SendSuccessful { get; set; }

        public GetPasswordResetModel(UserManager<MinigramUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "cancel")
            {
                return Redirect(ReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(Email), "Nem létezik ilyen e-mail című felhasználó");
                }
                else
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.PageLink(pageName: "/Account/PasswordReset", values: new { username = user.UserName, token });

                    // TODO: SendEmail
                    SendSuccessful = true;
                }
            }

            return Page();
        }
    }
}