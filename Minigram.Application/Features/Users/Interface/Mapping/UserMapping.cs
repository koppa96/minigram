using AutoMapper;
using Minigram.Application.Features.Users.Interface.Dtos;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.Users.Interface.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<MinigramUser, UserListDto>();
        }
    }
}