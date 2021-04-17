using AutoMapper;
using Minigram.Application.Features.FriendManagement.Interface.Dtos;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.FriendManagement.Interface.Mapping
{
    public class FriendRequestMapping : Profile
    {
        public FriendRequestMapping()
        {
            CreateMap<FriendRequest, FriendRequestDto>();
        }
    }
}