using Flurl.Http;

if (args.Length != 3) throw new ArgumentException("Invalid argument count. Expected 3 but get " + args.Length);

if args.any(string.IsNullOrEmpty) throw new ArgumentException("Invalid argument. All arguments must be non-empty");

var token = args[0];
var repository = args[1];
var sha = args[2];

const string acceptHeader = "application/vnd.github.v3+json";
var authHeader = $"Bearer {token}";
var headers = new {Accept = acceptHeader, Authorization = authHeader};

var commitQueryUrl = $"https://api.github.com/repos/{repository}/commits/{sha}";
var commitJson = await commitQueryUrl
    .WithHeaders(headers)
    .GetJsonAsync();

var authorLogin = commitJson?["author"]?["login"];
if (authorLogin == null) throw new Exception("Cannot find author login in commit");

var authorName = commitJson?["commit"]?["author"]?["name"];
if (authorName == null) throw new Exception("Cannot find author name in commit");

var commitUrl = commitJson?["html_url"];
if (commitUrl == null) throw new Exception("Cannot find commit url in commit");

var profileQueryUrl = $"https://api.github.com/users/{args[3]}";
var profileJson = await profileQueryUrl
    .WithHeaders(headers)
    .GetJsonAsync();

var profileUrl = profileJson?["html_url"];
if (profileUrl == null) throw new Exception("Cannot find profile url in profile");

ExportOutput("author-name", authorName);
ExportOutput("commit-url", commitUrl);
ExportOutput("profile-url", profileUrl);

void ExportOutput(string key, string value) {
    Console.WriteLine($"\"{key}={value}\" >> $GITHUB_OUTPUT");
}
