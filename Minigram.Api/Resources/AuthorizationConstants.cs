namespace Minigram.Api.Resources
{
    public static class AuthorizationConstants
    {
        public const string ScopeClaimType = "scope";
        
        public static class Scopes
        {
            public static class Friendships
            {
                public const string Read = "Friendships.Read";
                public const string Manage = "Friendships.Manage";
            }
            
            public static class Conversations
            {
                public const string Read = "Conversations.Read";
                public const string Manage = "Conversations.Manage";
            }
        }
    }
}