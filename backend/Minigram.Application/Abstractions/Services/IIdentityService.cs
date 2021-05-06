using System;

namespace Minigram.Application.Abstractions.Services
{
    public interface IIdentityService
    {
        Guid CurrentUserId { get; }
    }
}