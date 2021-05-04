using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Minigram.Dal.Entities;

namespace Minigram.IdentityProvider.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IUserClaimsPrincipalFactory<MinigramUser> claimsPrincipalFactory;
        private readonly UserManager<MinigramUser> userManager;
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Kötelező")]
        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public bool RememberMe { get; set; } = false;

        [BindProperty]
        public string ReturnUrl { get; set; } = "";

        public List<string> Errors { get; set; } = new List<string>();

        public LoginModel(
            IIdentityServerInteractionService interactionService,
            IUserClaimsPrincipalFactory<MinigramUser> claimsPrincipalFactory,
            UserManager<MinigramUser> userManager,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            this.interactionService = interactionService;
            this.claimsPrincipalFactory = claimsPrincipalFactory;
            this.userManager = userManager;
            this.environment = environment;
            this.configuration = configuration;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "register")
            {
                return RedirectToPage("/Account/Register", new { returnUrl = Request.GetEncodedUrl() });
            }

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(Username);
                var context = await interactionService.GetAuthorizationContextAsync(ReturnUrl);
                if (user != null && (await userManager.CheckPasswordAsync(user, Password)))
                {
                    if (context.Client.ClientId != configuration.GetValue<string>("AdminAppClientId") || await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        var signInProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                            AllowRefresh = true,
                            RedirectUri = ReturnUrl,
                            IsPersistent = RememberMe
                        };

                        var claimsPrincipal = await claimsPrincipalFactory.CreateAsync(user);
                        await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal, signInProperties);
                        HttpContext.User = claimsPrincipal;

                        if (interactionService.IsValidReturnUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        Errors.Add("Az admin alkalmazásba való bejelentkezéshez adminisztrátori jogosultság szükséges!");
                    }
                }
                else
                {
                    Errors.Add("Hibás felhasználónév vagy jelszó!");
                    return Page();
                }
            }

            return Page();
        }
    }
}