namespace GitHubAppToken
{
    public static class GitHubApp
    {
        public static GitHubAccessToken CreateAccessToken(string pemRsaKey, long appId, string owner, string repo, string userAgent,
            string baseUrl = "https://api.github.com", int expMinutes = 10)
        {
            var jwt = GitHubJwt.Create(pemRsaKey, appId, expMinutes);    
            return  new GitHubHttpClient().GetAccessToken(jwt, owner, repo, userAgent, baseUrl).Result;
        }
    }
}