using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Features.Users.Interface.Dtos;
using Minigram.Application.Features.Users.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public Task<List<UserListDto>> ListUsersAsync([FromQuery] string searchTerm,
            CancellationToken cancellationToken)
        {
            return userService.ListUsersAsync(searchTerm, cancellationToken);
        }
    }
}