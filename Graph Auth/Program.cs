//using System;
//using System.Threading.Tasks;
//using Azure.Identity;
//using Microsoft.Graph;


using Azure.Identity;
using Microsoft.Graph;

var credential = new ClientSecretCredential(

);

var graphClient = new GraphServiceClient(credential);

var users = await graphClient.Users.GetAsync();

foreach (var user in users.Value)
{
    Console.WriteLine(user.DisplayName);
}


//namespace GraphDemoV5
//{
//    class Program
//    {
//        private static string clientId = ;
//        private static string tenantId = ;
//        private static string[] scopes = ["User.Read"];

//        static async Task Main(string[] args)
//        {
//            // 1. Use InteractiveBrowserCredential from Azure.Identity
//            var interactiveCredential = new InteractiveBrowserCredential(
//                new InteractiveBrowserCredentialOptions
//                {
//                    ClientId = clientId,
//                    TenantId = tenantId
//                });

//            // 2. Initialize Graph client with the credential
//            var graphClient = new GraphServiceClient(interactiveCredential, scopes);

//            // 3. Get user profile
//            try
//            {
//                var me = await graphClient.Me.GetAsync();

//                Console.WriteLine("==== User Profile ====");
//                Console.WriteLine($"Display Name: {me?.DisplayName}");
//                Console.WriteLine($"User Principal Name: {me?.UserPrincipalName}");
//                Console.WriteLine($"ID: {me?.Id}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error fetching user profile: {ex.Message}");
//            }

//        }
//    }
//}
