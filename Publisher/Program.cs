using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Diagnostics;
using System.Management.Automation;
using Azure.Storage.Blobs.Models;
using Microsoft.CodeAnalysis;
using MimeTypes;

namespace Publisher
{
    internal class Program
    {
        private static string projectPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website";
        private static string projectFilePath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\Website.csproj";
        private static string projectBuildPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\bin\\Release\\net6.0\\publish\\wwwroot";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //build the website project
            BuildProject();

            //upload to azure
            await UploadToAzure();

            //hold your horses
            Console.ReadLine();
        }


        private static async Task UploadToAzure()
        {
            string containerName = "$web";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            Console.WriteLine("Nuking old website");
            //delete all files in website first, so that no problem
            DeleteAllFilesInContainer(containerClient);


            Console.WriteLine("Blasting new website into space");
            var files = Directory.GetFiles(projectBuildPath, "*.*", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];
                //maintains folder structure
                var blobName = filePath.Replace(projectBuildPath, "").Replace("\\", "/");

                //upload all new website files
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (var fs = File.Open(filePath, FileMode.Open))
                {
                    //note will not override, folder needs to be cleared before
                    await blobClient.UploadAsync(fs, new BlobHttpHeaders { ContentType = MimeTypeMap.GetMimeType(filePath) });
                    //await blobClient.UploadAsync(fs, overwrite: true);
                }

                Console.WriteLine($"{i}/{files.Length} Uploaded");
            }

            Console.WriteLine("Upload Complete");
        }

        private static void DeleteAllFilesInContainer(BlobContainerClient containerClient)
        {
            var allFiles = containerClient.GetBlobs();
            foreach (var file in allFiles)
            {
                BlobClient blobClient = containerClient.GetBlobClient(file.Name);
                blobClient.Delete();
            }
        }

        private static void BuildProject()
        {

            Console.WriteLine("Birthing new website");

            //delete all existing builds
            CleanBuildFolder();

            //set project path to build
            System.Environment.CurrentDirectory = projectPath;

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            //ps.AddCommand("dotnet").AddParameter("publish").AddParameter(projectFilePath).AddParameter("configuration", "Release"); //build release for publish
            //ps.AddCommand("dotnet").AddArgument("build");
            ps.AddScript(@"dotnet publish -c Release");
            ps.Invoke();

            foreach (PSObject o in ps.Invoke())
            {
                Console.WriteLine(o.ToString());
            }


            Console.WriteLine("Build Complete");
        }

        private static void CleanBuildFolder()
        {
            var di = new DirectoryInfo(projectBuildPath);
            foreach (var file in di.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}