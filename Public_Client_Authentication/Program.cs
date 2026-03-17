using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using DotNetEnv; // Install via NuGet: DotNetEnv

class Program
{
    // Microsoft Graph scope
    private static readonly string[] Scopes = { "User.Read" };

    static async Task Main()
    {
        // Load environment variables from .env file
        DotNetEnv.Env.Load();

        var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(tenantId))
        {
            Console.WriteLine("❌ CLIENT_ID or TENANT_ID not found in .env file.");
            return;
        }

        var pca = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithRedirectUri("http://localhost")
            .Build();

        try
        {
            // 1. Interactive authentication
            var result = await pca.AcquireTokenInteractive(Scopes).ExecuteAsync();

            Console.WriteLine("✅ Access token acquired!");
            Console.WriteLine($"Token expires on: {result.ExpiresOn}");
            Console.WriteLine($"🔑 Access Token: {result.AccessToken}");

            // 2. Call Microsoft Graph with token
            await CallMicrosoftGraph(result.AccessToken);
        }
        catch (MsalException ex)
        {
            Console.WriteLine($"❌ Authentication failed: {ex.Message}");
        }
    }

    static async Task CallMicrosoftGraph(string accessToken)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var formatted = JsonSerializer.Serialize(
                JsonSerializer.Deserialize<object>(json),
                new JsonSerializerOptions { WriteIndented = true }
            );
            Console.WriteLine("🔎 Microsoft Graph /me response:");
            Console.WriteLine(formatted);
        }
        else
        {
            Console.WriteLine($"❌ Graph call failed: {response.StatusCode}");
        }
    }
}
