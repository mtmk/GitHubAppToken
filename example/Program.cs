// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using JWT.Algorithms;
using JWT.Builder;
using Octokit;

Console.WriteLine("Hello, World!");

var key = RSA.Create();
key.ImportFromPem(File.ReadAllText("/Users/ziya/.keys/github/app_242978.key"));
var pub = RSA.Create();
pub.ImportRSAPublicKey(key.ExportRSAPublicKey(), out _);

var jwt = JwtBuilder.Create()
    .WithAlgorithm(new RS256Algorithm(pub, key))
    .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
    .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
    .AddClaim("iss", "242978")
    .Encode();

//Console.WriteLine(token);

var client = new HttpClient();
string accessTokensUrl;
{
    // var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/mtmk/test/installation");
    // request.Headers.Add("Accept", "application/vnd.github+json");
    // request.Headers.Add("Authorization", $"Bearer {jwt}");
    // request.Headers.Add("User-Agent", $"mtmk-client");
    // var response = client.Send(request);
    // var body = await response.Content.ReadAsStringAsync();
    // var json = JsonNode.Parse(body);
    // accessTokensUrl = json["access_tokens_url"]!.GetValue<string>();
}

string token;
{
    // var request = new HttpRequestMessage(HttpMethod.Post, accessTokensUrl);
    // request.Headers.Add("Accept", "application/vnd.github+json");
    // request.Headers.Add("Authorization", $"Bearer {jwt}");
    // request.Headers.Add("User-Agent", $"mtmk-client");
    //
    // var response = client.Send(request);
    // var body = await response.Content.ReadAsStringAsync();
    // var json = JsonNode.Parse(body);
    // token = json["token"]!.GetValue<string>();
}

{
    var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"))
    {
        Credentials = new Credentials(jwt, AuthenticationType.Bearer)
    };
    var installation = await github.GitHubApps.GetRepositoryInstallationForCurrent("mtmk", "test");
    var accessToken = await github.GitHubApps.CreateInstallationToken(installation.Id);
    token = accessToken.Token;
}

Console.WriteLine(token);

{
    var github = new GitHubClient(new ProductHeaderValue("MyAmazingApp"))
    {
        Credentials = new Credentials(token, AuthenticationType.Bearer)
    };
    var readme = await github.Repository.Content.GetReadme("mtmk", "test");
    Console.WriteLine(readme.Content);
}