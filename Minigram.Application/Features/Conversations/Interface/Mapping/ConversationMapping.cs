using AutoMapper;
using Minigram.Application.Features.Conversations.Interface.Dtos;
using Minigram.Dal.Entities;

namespace Minigram.Application.Features.Conversations.Interface.Mapping
{
    public class ConversationMapping : Profile
    {
        public ConversationMapping()
        {
            CreateMap<Conversation, ConversationListDto>();
            CreateMap<Conversation, ConversationDetailsDto>();
            CreateMap<ConversationMembership, ConversationMembershipDto>();
        }
    }
}