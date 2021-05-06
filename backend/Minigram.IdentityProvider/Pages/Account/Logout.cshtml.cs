using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Minigram.IdentityProvider.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;

        [BindProperty]
        public string LogoutId { get; set; } = "";

        public LogoutModel(IIdentityServerInteractionService interactionService)
        {
            this.interactionService = interactionService;
        }

        public void OnGet(string logoutId)
        {
            LogoutId = logoutId;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "cancel")
            {
                return Redirect("http://localhost:4200");
            }
            else
            {
                var context = await interactionService.GetLogoutContextAsync(LogoutId);
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return Redirect(context.PostLogoutRedirectUri);
            }
        }
    }
}