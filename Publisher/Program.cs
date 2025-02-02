using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Publisher
{
    class Program
    {
        private static readonly IConfiguration _configuration;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(10, 10); // This will limit the number of parallel uploads to 10

        static Program()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Creating Connection to web storage");

            var connectionString = _configuration["StorageAccountConnectionString"];
            var containerName = _configuration["ContainerName"];
            var localFolderPath = _configuration["LocalFolderPath"];
            var resourceGroup = _configuration["ResourceGroup"];
            var profileName = _configuration["CDNProfileName"];
            var endpointName = _configuration["CDNEndpointName"];

            // Create a BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Generate hash for js/VedAstro.js file in local folder path
            string vedAstroJsPath = Path.Combine(localFolderPath, "js", "VedAstro.js");
            string hash = GenerateFileHash(vedAstroJsPath);
            Console.WriteLine($"Generated hash for VedAstro.js: {hash}");

            // place hash as constant at top of js/app.js file 
            string appJsPath = Path.Combine(localFolderPath, "js", "app.js");
            InsertHashIntoAppJs(appJsPath, hash);

            // Sync local folder to blob storage
            var uploadedFiles = await SyncLocalFolderToBlob(containerClient, localFolderPath);

            // Purge Azure CDN
            await PurgeAzureCDN(resourceGroup, profileName, endpointName, uploadedFiles);
        }

        private static string GenerateFileHash(string filePath)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        private static void InsertHashIntoAppJs(string appJsPath, string hash)
        {
            const string hashConstantName = "const vedAstroJsHash = ";

            string[] lines = File.ReadAllLines(appJsPath);

            // Check if the constant already exists, and update it if so
            bool hashInserted = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith(hashConstantName))
                {
                    lines[i] = $"{hashConstantName}\"{hash}\";";
                    hashInserted = true;
                    break;
                }
            }

            // If the constant does not exist, insert it at the top of the file
            if (!hashInserted)
            {
                List<string> updatedLines = new List<string> { $"{hashConstantName}\"{hash}\";" };
                updatedLines.AddRange(lines);
                lines = updatedLines.ToArray();
            }

            File.WriteAllLines(appJsPath, lines);
        }


        private static async Task<List<string>> SyncLocalFolderToBlob(BlobContainerClient containerClient, string localFolderPath)
        {
            // Get all files in the local folder
            var files = Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories);

            var uploadedFiles = new List<string>();
            var tasks = new List<Task>();
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

                tasks.Add(UploadFile(containerClient, filePath, localFolderPath, uploadedFiles));
            }

            await Task.WhenAll(tasks);
            return uploadedFiles;
        }

        private static async Task UploadFile(BlobContainerClient containerClient, string filePath, string localFolderPath, List<string> uploadedFiles)
        {
            await semaphore.WaitAsync();

            try
            {
                string relativePath = Path.GetRelativePath(localFolderPath, filePath);
                string blobName = relativePath.Replace("\\", "/");

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                // Get the file extension
                string fileExtension = Path.GetExtension(filePath);

                // Set the correct content type based on the file extension
                string contentType;
                switch (fileExtension.ToLower())
                {
                    case ".txt":
                    case ".html":
                        contentType = "text/html";
                        break;
                    case ".css":
                        contentType = "text/css";
                        break;
                    case ".js":
                        contentType = "application/javascript";
                        break;
                    case ".json":
                        contentType = "application/json";
                        break;
                    case ".xml":
                        contentType = "application/xml";
                        break;
                    case ".png":
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".bmp":
                        contentType = "image/" + fileExtension.Substring(1);
                        break;
                    case ".woff":
                    case ".woff2":
                        contentType = "font/woff" + fileExtension.Substring(1);
                        break;
                    case ".ttf":
                        contentType = "font/ttf";
                        break;
                    case ".eot":
                        contentType = "application/vnd.ms-fontobject";
                        break;
                    case ".svg":
                        contentType = "image/svg+xml";
                        break;
                    case ".mp3":
                    case ".wav":
                    case ".aac":
                    case ".ogg":
                        contentType = "audio/" + fileExtension.Substring(1);
                        break;
                    case ".mp4":
                    case ".avi":
                    case ".mov":
                    case ".wmv":
                        contentType = "video/" + fileExtension.Substring(1);
                        break;
                    case ".zip":
                    case ".rar":
                    case ".7z":
                        contentType = "application/zip";
                        break;
                    case ".pdf":
                        contentType = "application/pdf";
                        break;
                    case ".docx":
                    case ".doc":
                        contentType = "application/msword";
                        break;
                    case ".xlsx":
                    case ".xls":
                        contentType = "application/vnd.ms-excel";
                        break;
                    case ".pptx":
                    case ".ppt":
                        contentType = "application/vnd.ms-powerpoint";
                        break;
                    case ".csv":
                        contentType = "text/csv";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        break;
                }

                // Upload the file if it doesn't exist or if the local file is newer than the blob
                if (!await blobClient.ExistsAsync())
                {
                    Console.WriteLine($"Uploading new file: {blobName}");
                    var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
                    await blobClient.UploadAsync(filePath, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
                    uploadedFiles.Add(blobName);
                }
                else
                {
                    var blobProperties = await blobClient.GetPropertiesAsync();
                    DateTimeOffset lastModified = blobProperties.Value.LastModified;

                    // Check if the local file is newer
                    if (File.GetLastWriteTimeUtc(filePath) > lastModified.UtcDateTime)
                    {
                        Console.WriteLine($"Updating file: {blobName}");
                        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
                        await blobClient.UploadAsync(filePath, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
                        uploadedFiles.Add(blobName);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping file (no changes): {blobName}");
                    }
                }
            }
            finally
            {
                semaphore.Release();
            }
        }

        private static async Task PurgeAzureCDN(string resourceGroup, string profileName, string endpointName, List<string> uploadedFiles)
        {
            Console.WriteLine("Purging Azure CDN...");

            foreach (var file in uploadedFiles)
            {
                // Ensure the file path starts with a single `/` and format it correctly
                var purgePath = $"/{file}";

                // Inform the user which file is being purged
                Console.WriteLine($"Purging file: {purgePath}");

                var arguments = $"/c az cdn endpoint purge --resource-group {resourceGroup} --profile-name {profileName} --name {endpointName} --content-paths \"{purgePath}\"";
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                await process.WaitForExitAsync();
            }

            Console.WriteLine("Azure CDN purged successfully.");
        }


    }
}
