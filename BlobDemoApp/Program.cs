using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs; //Azure Sdk namespace to work with Blob Services
using Azure.Storage.Blobs.Models; //Contains Blob-related models like BlobDownloadInfo

class Program
{
    //This is your storage account connection string (copied from Access Keys in the portal).
    //It tells the SDK which account to connect to and how to authenticate.
    private const string connectionString = "";

    static async Task Main(string[] args)
    {
        // 1. Create a BlobServiceClient
        var blobServiceClient = new BlobServiceClient(connectionString);

        // 2. Create a container
        string containerName = "mycontainer" + Guid.NewGuid().ToString("n").Substring(0, 6);
        var containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
        Console.WriteLine($"Created container: {containerName}");

        // 3. Upload a file
        string localPath = "./data";
        Directory.CreateDirectory(localPath);
        string fileName = "example.txt";
        string localFilePath = Path.Combine(localPath, fileName);

        // Write sample text to file
        await File.WriteAllTextAsync(localFilePath, "Hello Azure Blob Storage!");

        BlobClient blobClient = containerClient.Value.GetBlobClient(fileName);
        using FileStream uploadFileStream = File.OpenRead(localFilePath);
        await blobClient.UploadAsync(uploadFileStream, overwrite: true);
        uploadFileStream.Close();

        Console.WriteLine($"Uploaded file to Blob Storage: {fileName}");

        // 4. List blobs in the container
        Console.WriteLine("Listing blobs...");
        await foreach (var blobItem in containerClient.Value.GetBlobsAsync())
        {
            Console.WriteLine($" - {blobItem.Name}");
        }

        // 5. Download the file
        string downloadPath = localFilePath.Replace(".txt", "_DOWNLOADED.txt");
        BlobDownloadInfo download = await blobClient.DownloadAsync();

        using (FileStream downloadFileStream = File.OpenWrite(downloadPath))
        {
            await download.Content.CopyToAsync(downloadFileStream);
            downloadFileStream.Close();
        }

        Console.WriteLine($"Downloaded blob to: {downloadPath}");
    }
}
