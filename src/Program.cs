using System.Text.Json;

if (args.Length != 3)
    throw new ArgumentException("Invalid argument count. Expected 3 but get " + args.Length);

if (args.Any(string.IsNullOrEmpty))
    throw new ArgumentException("Invalid argument. All arguments must be non-empty");

var token = args[0];
var repository = args[1];
var sha = args[2];

const string acceptHeader = "application/vnd.github.v3+json";
var authHeader = $"Bearer {token}";
var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Add("Accept", acceptHeader);
httpClient.DefaultRequestHeaders.Add("Authorization", authHeader);

var commitQueryUrl = $"https://api.github.com/repos/{repository}/commits/{sha}";
Console.WriteLine(JsonSerializer.Serialize(authHeader));
Console.WriteLine(commitQueryUrl);
var commitStr = await httpClient.GetStringAsync(commitQueryUrl, new CancellationToken());
var commitJson = JsonSerializer.Deserialize<dynamic>(commitStr);

var authorLogin = commitJson?["author"]?["login"];
if (string.IsNullOrEmpty(authorLogin == null))
    throw new Exception("Cannot find author login in commit");

var authorName = commitJson?["commit"]?["author"]?["name"];
if (string.IsNullOrEmpty(authorName == null))
    throw new Exception("Cannot find author name in commit");

var commitUrl = commitJson?["html_url"];
if (string.IsNullOrEmpty(commitUrl == null))
    throw new Exception("Cannot find commit url in commit");

var profileQueryUrl = $"https://api.github.com/users/{args[3]}";
Console.WriteLine(profileQueryUrl);
var profileStr = await httpClient.GetStringAsync(profileQueryUrl, new CancellationToken());
var profileJson = JsonSerializer.Deserialize<dynamic>(profileStr);

var profileUrl = profileJson?["html_url"];
if (string.IsNullOrEmpty(profileUrl == null))
    throw new Exception("Cannot find profile url in profile");

ExportOutput("author-name", authorName);
ExportOutput("commit-url", commitUrl);
ExportOutput("profile-url", profileUrl);

void ExportOutput(string key, string value) {
    Console.WriteLine($"\"{key}={value}\" >> $GITHUB_OUTPUT");
}
