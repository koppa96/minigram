using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Minigram.Dal.Entities;

namespace Minigram.IdentityProvider.Pages.Account
{
    public class PasswordResetModel : PageModel
    {
        private readonly UserManager<MinigramUser> userManager;

        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Token { get; set; } = "";

        [BindProperty]
        [Required(ErrorMessage = "Kötelező")]
        [MinLength(6, ErrorMessage = "A jelszónak legalább 6 karakterből kell állnia")]
        public string Password { get; set; } = "";

        [BindProperty]
        [Required]
        public string ConfirmPassword { get; set; } = "";

        public bool ResetSuccessful { get; set; } = false;
        public bool OnlyPasswordError { get; set; } = true;

        public PasswordResetModel(UserManager<MinigramUser> userManager)
        {
            this.userManager = userManager;
        }

        public void OnGet(string username, string token)
        {
            Username = username;
            Token = token;
        }

        public async Task OnPostAsync()
        {
            if (ConfirmPassword != Password)
            {
                ModelState.AddModelError(nameof(ConfirmPassword), "A megadott jelszavak nem egyeznek!");
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Username);
                if (user != null)
                {
                    var resetResult = await userManager.ResetPasswordAsync(user, Token, Password);
                    ResetSuccessful = resetResult.Succeeded;
                    OnlyPasswordError = resetResult.Errors.All(x => x.Code.ToLower().Contains(nameof(Password).ToLower()));
                    
                    if (OnlyPasswordError)
                    {
                        foreach (var error in resetResult.Errors)
                        {
                            ModelState.AddModelError(nameof(Password), error.Description);
                        }    
                    }
                }
            }
        }
    }
}