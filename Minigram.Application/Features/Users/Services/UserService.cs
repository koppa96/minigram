using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Minigram.Application.Features.Users.Interface.Dtos;
using Minigram.Application.Features.Users.Interface.Services;
using Minigram.Dal;

namespace Minigram.Application.Features.Users.Services
{
    public class UserService : IUserService
    {
        private readonly MinigramDbContext context;
        private readonly IConfigurationProvider configurationProvider;

        public UserService(MinigramDbContext context, IConfigurationProvider configurationProvider)
        {
            this.context = context;
            this.configurationProvider = configurationProvider;
        }
        
        public Task<List<UserListDto>> ListUsersAsync(string searchText, CancellationToken cancellationToken = default)
        {
            return context.Users.Where(x => x.UserName.ToLower().Contains(searchText.ToLower()))
                .OrderBy(x => x.UserName)
                .Take(25)
                .ProjectTo<UserListDto>(configurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}