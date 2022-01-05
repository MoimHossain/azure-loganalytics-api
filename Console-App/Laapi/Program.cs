

using Laapi.Models;
using Laapi.Supports;
using System;

var LogAnalyticsResource = "https://api.loganalytics.io";


var TenantId = Env.GetEnvironmentVariable(Env.Keys.TenantId);
var ClientID = Env.GetEnvironmentVariable(Env.Keys.ClientID);
var ClientSecret = Env.GetEnvironmentVariable(Env.Keys.ClientSecret);
var LogAnalyticsWorkspaceID = Env.GetEnvironmentVariable(Env.Keys.LogAnalyticsWorkspaceID);



var uri = $"https://api.loganalytics.io/v1/workspaces/{LogAnalyticsWorkspaceID}/query";
var httpFactory = new HttpClientFactory(new TokenHelper(TenantId, ClientID, ClientSecret, LogAnalyticsResource));

var loop = false;
do
{
    Console.ResetColor();
    Console.Clear();
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("# Enter KUSTO Query: (e.g. AzureDevOpsAuditing)");

    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("> ");
    var query = Console.ReadLine();
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("# Enter Time span: (e.g. P2D)");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("> ");
    var timespan = Console.ReadLine();

    var logs = await httpFactory.PostAsync<Logs>(uri, new
    {
        Query = query,
        Timespan = timespan
    });

    logs.Dump();

    Console.WriteLine("Type 'yes' to continue...");
    loop = "yes".Equals(Console.ReadLine(), StringComparison.OrdinalIgnoreCase);
} while (loop);

