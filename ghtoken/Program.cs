using GitHubAppToken;

try
{
    if (args.Length is < 4 or > 5)
    {
        var app = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
        Console.Error.WriteLine($"Usage: {app} <key-file> <app-id> <owner> <repo> [<base-url>]");
        return 2;
    }

    var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    var pem = File.ReadAllText(args[0].Replace("~", home));
    var id = long.Parse(args[1]);
    var owner = args[2];
    var repo = args[3];
    var url = args.Length > 4 ? args[4] : "https://api.github.com";

    var token = GitHubApp.CreateAccessToken(pem, id, owner, repo, "ghtoken", url);

    Console.Write(token.Token);

    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine(e.GetBaseException().Message);
    return 3;
}