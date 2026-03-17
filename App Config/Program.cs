using System;
using Microsoft.Extensions.Configuration;
using Azure.Identity;

class Program
{
    static void Main()
    {
        try
        {
            var builder = new ConfigurationBuilder();

            builder.AddAzureAppConfiguration(options =>
{
    Console.WriteLine("Attempting connection...");
    options.Connect("Endpoint=APPCONFIGCONNECTIONSTRING").Select("*");
    Console.WriteLine("Connect() call finished."); // <-- do we see this?
});


            Console.WriteLine("Connecting to Azure App Configuration...");
            var config = builder.Build();
        Console.WriteLine("Connection successful, building config...");


            // Debug: print all keys
            Console.WriteLine("=== Loaded Keys ===");
            foreach (var kvp in config.AsEnumerable())
            {
                Console.WriteLine($"{kvp.Key} = {kvp.Value}");
            }
            Console.WriteLine("===================");

            string? dbConn = config["AppSettings:DbConnection"];
            string? apiUrl = config["AppSettings:ApiBaseUrl"];

            if (string.IsNullOrEmpty(dbConn))
                Console.WriteLine("Warning: DbConnection is null or empty.");
            else
                Console.WriteLine($"Database Connection: {dbConn}");

            if (string.IsNullOrEmpty(apiUrl))
                Console.WriteLine("Warning: ApiBaseUrl is null or empty.");
            else
                Console.WriteLine($"API Base URL: {apiUrl}");
        }
        catch (Azure.RequestFailedException ex)
        {
            Console.WriteLine($"Error connecting to Azure App Configuration: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
