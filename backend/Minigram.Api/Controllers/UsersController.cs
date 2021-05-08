using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minigram.Application.Features.Users.Interface.Dtos;
using Minigram.Application.Features.Users.Interface.Services;

namespace Minigram.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Description("Search users by username for autocompletes. Returns the top 25 matches.")]
        public Task<List<UserListDto>> ListUsersAsync(
            [Description("The part of the name of the searched user")] [FromQuery] string searchTerm,
            CancellationToken cancellationToken)
        {
            return userService.ListUsersAsync(searchTerm ?? string.Empty, cancellationToken);
        }
    }
}