using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Minigram.Application.Features.Users.Interface.Dtos;

namespace Minigram.Application.Features.Users.Interface.Services
{
    public interface IUserService
    {
        Task<List<UserListDto>> ListUsersAsync(string searchText, CancellationToken cancellationToken = default);
    }
}