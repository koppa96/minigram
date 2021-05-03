using AutoMapper;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.Conversations.Mapping
{
    public class MessageMapping : Profile
    {
        public MessageMapping()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(x => x.Sender, o => o.MapFrom(x => x.ConversationMembership.Member));
        }
    }
}