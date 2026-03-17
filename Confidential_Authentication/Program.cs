using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using DotNetEnv; // Install via NuGet: DotNetEnv

class Program
{
    // For app-only token, we use .default scope
    private static readonly string[] Scopes = { "https://graph.microsoft.com/.default" };

    static async Task Main()
    {
        // Load environment variables
        DotNetEnv.Env.Load();

        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
        var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientSecret))
        {
            Console.WriteLine("❌ CLIENT_ID, TENANT_ID, or CLIENT_SECRET not found in .env file.");
            return;
        }

        // 1. Build Confidential Client Application
        var cca = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority($"https://login.microsoftonline.com/{tenantId}")
            .Build();

        try
        {
            // 2. Acquire token for client (app-only, no user interaction)
            var result = await cca.AcquireTokenForClient(Scopes).ExecuteAsync();

            Console.WriteLine("✅ Access token acquired (client credentials flow)!");
            Console.WriteLine($"Token expires on: {result.ExpiresOn}");
            Console.WriteLine($"🔑 Access Token: {result.AccessToken}");

        }
        catch (MsalException ex)
        {
            Console.WriteLine($"❌ Authentication failed: {ex.Message}");
        }
    }

}
