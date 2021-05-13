using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Minigram.Application.Features.Users.Interface.Services;
using Minigram.Application.Features.Users.Services;
using Minigram.Dal;
using Minigram.Dal.Entities;
using Xunit;

namespace Minigram.Tests.Application
{
    public class UserServiceTests : IClassFixture<AppDependencyFixture>
    {
        private readonly IUserService userService;
        private readonly MinigramDbContext context;

        public UserServiceTests(AppDependencyFixture fixture)
        {
            context = fixture.MinigramDbContext;
            userService = new UserService(fixture.MinigramDbContext, fixture.ConfigurationProvider);
        }

        [Fact]
        public async Task UserServiceListsUsers()
        {
            context.Users.Add(new MinigramUser
            {
                Email = "teszt@teszt.hu",
                UserName = "teszt"
            });
            await context.SaveChangesAsync();

            var users = await userService.ListUsersAsync("tes");

            users.Count.Should().Be(1);
            users.Single().UserName.Should().Be("teszt");
        }

        [Fact]
        public async Task UserServiceDoesNotListNotContaining()
        {
            context.Users.Add(new MinigramUser
            {
                Email = "teszt@teszt.hu",
                UserName = "teszt"
            });
            await context.SaveChangesAsync();

            var users = await userService.ListUsersAsync("xyz");

            users.Count.Should().Be(0);
        }
    }
}