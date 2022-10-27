# GitHub App Access Token Creator

Accessing GitHub securely using git or GitHub APIs from your build services or
automation bots, services and applications can be achieved using a [GitHub App][1].

GitHub App access token creator is a simple .Net library and a command line tool,
that can create temporary access tokens to be used by GitHub APIs or git http
authentication, using a GitHub App private key.

## Using the Library
[![Nuget](https://img.shields.io/nuget/v/GitHubAppToken)](https://www.nuget.org/packages/GitHubAppToken/)

I wanted to avoid having any third party dependencies for anyone simply wanting to
copy paste the code into their projects or even into a Powershell script.

You can simply call the convenience method if you want to get a token
without much ceremony:
```csharp
string key = File.ReadAllText("path/to/app_private_key.pem");
GitHubAccessToken result = GitHubApp.CreateAccessToken(key, appId, owner, repo, "my-user-agent");
DateTimeOffset expiry = result.ExpiresAt;
string token = result.Token;
```

If you want to integrate it into your project using `HttpClient` passed in
by dependency injection, or using custom settings and handlers and take 
advantage of *async* functionality, then you can generate the JWT token
and pass it on to a simple client:
```csharp
string jwt = GitHubJwt.Create(pemRsaKey, appId, expMinutes);    
HttpClient httpClient = new HttpClient();
GitHubHttpClient client = new GitHubHttpClient(httpClient)
GitHubAccessToken result = await client.GetAccessToken(jwt, owner, repo, "my-user-agent");
```

You can also pass an additional optional argument for GitHub Enterprise installations.
```csharp
GitHubApp.CreateAccessToken(key, appId, owner, repo, "my-user-agent", "https://example.com/api/v3");
// or
client.GetAccessToken(jwt, owner, repo, "my-user-agent", "https://example.com/api/v3");
```

## Using Command Line Tool
You can download the command line tool from [releases for your platform][3].
```shell
$ ghtoken
Usage: ghtoken <key-file> <app-id> <owner> <repo> [<base-url>]
$ ghtoken ~/.keys/key.pem 123456 myaccount myrepo
ghs_sKCI9wZz1AVUq3FP5AAsViCbbac2Ib3l6ltQ
```
It returns the token to STDOUT without any line feeds and exit code `0` to
indicate success. Any errors or usage is written to STDERR and the exit code
would be `2` for usage and `3` for all other exceptions.

You can also pass an optional GitHub Enterprise URL as the last argument:
```shell
$ ghtoken ~/.keys/key.pem 123 myaccount myrepo http://example.com/api/v3
```

## How it Works

There are essentially two steps to the process: Creating a JWT string and
calling couple of GitHub REST APIs using the created JWT as Authorization.

For [JWT creation][4], I used a simple routine to create only what's required
by GitHub. (see `GitHubJwt.cs` file)

As for GitHub API calls (see `GitHubHttpClient.cs` file):
* `GET` installation information for a given repository
* `POST` to the URL provided by the first call to create the temporary token.

## Credits

Unfortunately there is no support for PEM decoding in .Net *netstandard-2.0*.
Since I did not want to create any external dependencies, I used the code
generously made available by [JavaScience Consulting](https://www.jensign.com).
`PemKeyDecoder.cs` file contains part of this code that is required to read
PEM encoded private keys used by GitHub Apps.
```
OpenSSLKey
 .NET 2.0  OpenSSL Public & Private Key Parser
 Copyright (C) 2008  	JavaScience Consulting
```
Unfortunately I could not find the original link to the code.
Thanks to these gists keeping a copy:
* https://gist.github.com/stormwild/7887264
* https://gist.github.com/njmube/edc64bb2f7599d33ca5a

## Creating a GitHub App

You need a GitHub App first and generate a private key. Here is how you
can do it in a few simple steps:

* Go to your account or organisation settings
* Select [Developer Settings, then GitHub Apps][2]
* Click on *New GitHub App* (you might need to authenticate after that)
* Fill in the *Register new GitHub App* form.
  For the purpose of only being able to create access tokens, you can ignore
  many of the fields.
  * GitHub App name: this could be anything
  * Homepage URL: Any URL (maybe a link to a README on GitHub)
  * Un-tick *Webhook, Active* so you won't have to provide a webhook url
  * As for permissions, select *Repository permissions, Content*. This would
    give you access to all usual git operations including some API access like
    dealing with releases.
  * Click on *Create GitHub App*
  * Generate a private key (should download automatically)

Once you have your app setup, you then need to *install* the app to your
organisation or user account:

* Go to your account or organisation settings
* Select [Developer Settings, then GitHub Apps][2]
* You should see your app you just created, click *Edit* button
* Select *Install App* from menu
* Select the organisations and optionally repositories to install to.

## Related Projects
* https://github.com/Link-/gh-token
* https://github.com/slawekzachcial/gha-token

[1]: https://docs.github.com/en/developers/apps/getting-started-with-apps/about-apps
[2]: https://github.com/settings/apps
[3]: https://github.com/mtmk/GitHubAppToken/releases
[4]: https://jwt.io/introduction

