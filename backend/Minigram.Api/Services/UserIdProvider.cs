using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.SignalR;

namespace Minigram.Api.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Claims.Single(x => x.Type == JwtClaimTypes.Subject).Value;
        }
    }
}