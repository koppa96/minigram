using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Minigram.Api.Services;
using Minigram.Application.Abstractions.Services;
using Moq;
using Xunit;

namespace Minigram.Tests.Api
{
    public class IdentityServiceTests
    {
        private readonly IIdentityService identityService;
        private readonly Mock<HttpContext> mockHttpContext;

        public IdentityServiceTests()
        {
            var mockAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContext = new Mock<HttpContext>();
            
            mockAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);

            identityService = new IdentityService(mockAccessor.Object);
        }

        [Fact]
        public void CurrentUserIdIsValid()
        {
            // Arrange
            mockHttpContext.Setup(x => x.User.Claims).Returns(new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, "84EDE270-F795-4923-BD1A-FB235117038A")
            });

            // Act
            var currentUserId = identityService.CurrentUserId;

            // Assert
            currentUserId.Should().Be(Guid.Parse("84EDE270-F795-4923-BD1A-FB235117038A"));
        }

        [Fact]
        public void CurrentUserIdThrowsIfNotLoggedIn()
        {
            // Arrange
            mockHttpContext.Setup(x => x.User).Returns<IEnumerable<Claim>>(null);

            Func<Guid> action = () => identityService.CurrentUserId;

            action.Should().Throw<Exception>();
        }
    }
}