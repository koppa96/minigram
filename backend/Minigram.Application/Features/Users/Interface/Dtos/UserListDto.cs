using System;

namespace Minigram.Application.Features.Users.Interface.Dtos
{
    public class UserListDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}