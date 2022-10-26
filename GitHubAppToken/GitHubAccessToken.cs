using System;

namespace GitHubAppToken
{
    public class GitHubAccessToken
    {
        public GitHubAccessToken(string token, DateTimeOffset expiresAt)
        {
            Token = token;
            ExpiresAt = expiresAt;
        }
        
        public string Token { get; }
        public DateTimeOffset ExpiresAt { get; }
        
        public override string ToString()
        {
            return $"GHToken[expires_at={ExpiresAt}]";
        }
    }
}