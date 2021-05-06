using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Minigram.IdentityProvider.Pages
{
    public class Consent : PageModel
    {
        private readonly IIdentityServerInteractionService interactionService;

        public class ApiScopeResponse
        {
            public bool Consented { get; set; }
            public ApiScope ApiScope { get; set; }
        }
        
        public string ClientName { get; set; }
        
        [BindProperty]
        public List<ApiScopeResponse> ApiScopes { get; set; }
        
        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public bool Remember { get; set; }

        public Consent(IIdentityServerInteractionService interactionService)
        {
            this.interactionService = interactionService;
        }

        public async Task OnGetAsync(string returnUrl)
        {
            ReturnUrl = returnUrl;
            var context = await interactionService.GetAuthorizationContextAsync(ReturnUrl);

            ClientName = context.Client.ClientName;

            ApiScopes = context.ValidatedResources.ParsedScopes
                .Select(x => new ApiScopeResponse
                {
                    Consented = true,
                    ApiScope = context.ValidatedResources.Resources.FindApiScope(x.ParsedName)
                })
                .Where(x => x.ApiScope != null)
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            var context = await interactionService.GetAuthorizationContextAsync(ReturnUrl);

            ConsentResponse response = null;
            if (action == "consent")
            {
                var scopesConsented = ApiScopes.Where(x => x.Consented)
                    .Select(x => x.ApiScope.Name)
                    .Append(IdentityServerConstants.StandardScopes.OpenId);
                
                response = new ConsentResponse
                {
                    RememberConsent = Remember,
                    ScopesValuesConsented = scopesConsented
                };
            }
            else
            {
                response = new ConsentResponse
                {
                    Error = AuthorizationError.AccessDenied
                };
            }

            await interactionService.GrantConsentAsync(context, response);
            if (interactionService.IsValidReturnUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return Page();
        }
    }
}