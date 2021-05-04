using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Minigram.Dal.Entities;

namespace Minigram.IdentityProvider.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<MinigramUser> userManager;

        [BindProperty]
        [Required(ErrorMessage = "Kötelező")]
        [EmailAddress(ErrorMessage = "Nem érvényes e-mail cím")]
        public string Email { get; set; } = "";

        public bool ConfirmSuccessful { get; set; } = false;
        public bool SendSuccessful { get; set; } = false;

        [BindProperty]
        public bool Resend { get; set; } = false;

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public ConfirmEmailModel(UserManager<MinigramUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnGetAsync(string username, string token, string returnUrl, bool resend = false)
        {
            Resend = resend;
            ReturnUrl = returnUrl ?? "";

            if (!resend && username != null && token != null)
            {
                await CheckDataAsync(token, username);
            }
        }

        private async Task CheckDataAsync(string token, string username)
        {
            var user = await userManager.FindByNameAsync(username);

            if (user != null)
            {
                var confirmResult = await userManager.ConfirmEmailAsync(user, token);
                ConfirmSuccessful = confirmResult.Succeeded;
            }
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
                    ModelState.AddModelError(nameof(Email), "Nem létezik ilyen címmel regisztrált felhasználó!");
                }
                else
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.PageLink(pageName: "/Account/ConfirmEmail", values: new { username = user.UserName, token });

                    // TODO: Send email
                    SendSuccessful = true;
                }
            }

            return Page();
        }
    }
}