using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHubAppToken
{
    public class GitHubHttpClient
    {
        private readonly HttpClient _httpClient;

        public GitHubHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public GitHubHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetInstallation(string jwt, string owner, string repo, string userAgent,
            string baseUrl = "https://api.github.com")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/repos/{owner}/{repo}/installation");
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", $"Bearer {jwt}");
            request.Headers.Add("User-Agent", userAgent);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateToken(string jwt, string userAgent, string accessTokensUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, accessTokensUrl);
            request.Headers.Add("Accept", "application/vnd.github+json");
            request.Headers.Add("Authorization", $"Bearer {jwt}");
            request.Headers.Add("User-Agent", userAgent);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<GitHubAccessToken> GetAccessToken(string jwt, string owner, string repo, string userAgent,
            string baseUrl = "https://api.github.com")
        {
            var installation = await GetInstallation(jwt, owner, repo, userAgent, baseUrl);
            var accessTokensUrl = GetJsonObject(installation, "access_tokens_url");

            var body = await CreateToken(jwt, userAgent, accessTokensUrl);
            var token = GetJsonObject(body, "token");
            var expiresAt = DateTimeOffset.Parse(GetJsonObject(body, "expires_at"));

            return new GitHubAccessToken(token, expiresAt);
        }

        private static string GetJsonObject(string json, string key)
        {
            return Regex.Match(json, $@"""{key}""\s*:\s*""([^""]+)""").Groups[1].Value;
        }
    }
}