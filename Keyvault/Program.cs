using System;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

class Program
{
    static async Task Main(string[] args)
    {
        // 👇 Replace with your vault name
        string keyVaultName = "AKV1514";
        var kvUri = $"https://{keyVaultName}.vault.azure.net/";
        
        string tenantId = ;
        string clientId = ;
        string clientSecret = ;

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        var client = new SecretClient(new Uri(kvUri), credential);

        // 1. Store a secret
        string secretName = "DbPassword";
        string secretValue = "MyConsoleAppSecret123!";
        await client.SetSecretAsync(secretName, secretValue);
        Console.WriteLine($"Secret '{secretName}' created with value: {secretValue}");

        // 2. Retrieve a secret
        KeyVaultSecret secret = await client.GetSecretAsync(secretName);
        Console.WriteLine($"Retrieved secret: {secret.Name} with value: {secret.Value}");
    }
}
