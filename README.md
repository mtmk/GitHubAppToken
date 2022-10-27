# GitHub App Access Token Creator

Accessing GitHub securely using git or GitHub APIs from your build services or
automation bots, services and applications can be achieved using a [GitHub App][1].

GitHub App access token creator is a simple .Net library and a command line tool,
that can create temporary access tokens to be used by GitHub APIs or git http
authentication, using a GitHub App private key.

## Using the Library

[![Nuget](https://img.shields.io/nuget/v/GitHubAppToken)](https://www.nuget.org/packages/GitHubAppToken/)


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
