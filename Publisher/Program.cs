using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        private static readonly IConfiguration _configuration;

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        static async Task Main(string[] args)
        {
            var connectionString = _configuration["StorageAccountConnectionString"];
            var containerName = _configuration["ContainerName"];
            var localFolderPath = _configuration["LocalFolderPath"];

            // Create a BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Sync local folder to blob storage
            await SyncLocalFolderToBlob(containerClient, localFolderPath);
        }

        private static async Task SyncLocalFolderToBlob(BlobContainerClient containerClient, string localFolderPath)
        {
            // Get all files in the local folder
            var files = Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                // Exclude specified files and directories
                if (filePath.EndsWith("Website_Mobile.sln") ||
                    filePath.EndsWith("Website_Mobile.csproj") ||
                    filePath.Contains("\\bin\\") ||
                    filePath.Contains("\\Properties\\") ||
                    filePath.Contains("\\obj\\"))
                {
                    continue;
                }

                string relativePath = Path.GetRelativePath(localFolderPath, filePath);
                string blobName = relativePath.Replace("\\", "/");

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Upload the file if it doesn't exist or if the local file is newer than the blob
                if (!await blobClient.ExistsAsync())
                {
                    Console.WriteLine($"Uploading new file: {blobName}");
                    await blobClient.UploadAsync(filePath);
                }
                else
                {
                    var blobProperties = await blobClient.GetPropertiesAsync();
                    DateTimeOffset lastModified = blobProperties.Value.LastModified;

                    // Check if the local file is newer
                    if (File.GetLastWriteTimeUtc(filePath) > lastModified.UtcDateTime)
                    {
                        Console.WriteLine($"Updating file: {blobName}");
                        await blobClient.UploadAsync(filePath, overwrite: true);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping file (no changes): {blobName}");
                    }
                }
            }
        }
    }
}
