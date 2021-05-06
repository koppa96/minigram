using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Minigram.Application.Abstractions.Services;

namespace Minigram.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpContext httpContext;

        private Guid? currentUserId;
        public Guid CurrentUserId
        {
            get
            {
                return currentUserId ??=
                    Guid.Parse(httpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
            }
        }

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
    }
}