using System.Linq;
using AutoMapper;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.Conversations.Mapping
{
    public class ConversationMapping : Profile
    {
        public ConversationMapping()
        {
            CreateMap<Conversation, ConversationListDto>()
                .ForMember(x => x.LastMessage, o => o.MapFrom(x => x.ConversationMemberships
                    .SelectMany(m => m.SentMessages)
                    .OrderByDescending(m => m.CreatedAt)
                    .FirstOrDefault()));
            CreateMap<Conversation, ConversationDetailsDto>();
            CreateMap<ConversationMembership, ConversationMembershipDto>();
        }
    }
}