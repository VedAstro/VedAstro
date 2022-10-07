using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Diagnostics;
using System.Management.Automation;
using Azure.Storage.Blobs.Models;
using ByteSizeLib;
using Microsoft.CodeAnalysis;
using MimeTypes;
using Microsoft.Extensions.Configuration;

namespace Publisher
{
    internal class Program
    {
        private static string projectPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website";
        private static string projectFilePath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\Website.csproj";
        private static string projectBuildPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\bin\\Release\\net7.0\\publish\\wwwroot";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Choose wisely");
            Console.WriteLine("1:Build & Upload All");
            Console.WriteLine("2:Build & Upload AOT only");
            Console.WriteLine("3:Upload AOT only (Upload All)");
            Console.WriteLine("4:Upload AOT only (Upload Changes Only)");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "2": goto AOT;
                case "3": goto AOT_UPLOAD;
                case "4": goto AOT_UPLOAD;
            }

            //NORMAL BUILD
            //build the website project
            BuildProject();

            //upload to azure
            await UploadToAzure();

            //AOT BUILD
            AOT:
            Console.WriteLine("Now we do AOT");
            BuildProjectAOT();

            AOT_UPLOAD:
            //upload to azure
            await UploadToAzureAOT();

            Console.WriteLine("DONE!!!");

            //hold your horses
            Console.ReadLine();
        }


        private static async Task UploadToAzure()
        {
            string containerName = "$web";

            //get secret connection string from secrets.json (security)
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            string connectionString = config["vedastrowebsitestorage"];

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

                Console.Write($"\r{i}/{files.Length} Uploaded:{blobName}"); //space to delete extra prev
            }

            Console.WriteLine("Upload Complete");
        }
        private static async Task UploadToAzureAOT()
        {
            string containerName = "$web";

            //get secret connection string from secrets.json (security)
            var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            string connectionString = config["vedastrowebsitestorage2"]; //NOTE:AOT USES WEBSITE2

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

                Console.Write($"\r{i}/{files.Length} Uploaded:{blobName}"); //space to delete extra prev
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
            ps.AddScript(@"dotnet publish -c Release -p:Flavor=AOT_OFF");
            ps.Invoke();

            foreach (PSObject o in ps.Invoke())
            {
                Console.WriteLine(o.ToString());
            }


            Console.WriteLine("Build Complete");
            PrintBuildSize();
        }
        
        private static void BuildProjectAOT()
        {
            Console.WriteLine("Birthing new AOT website");

            //delete all existing builds
            CleanBuildFolder();

            //set project path to build
            System.Environment.CurrentDirectory = projectPath;

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            ps.AddScript(@"dotnet publish -c Release -p:Flavor=AOT_ON");
            ps.Invoke();

            foreach (PSObject o in ps.Invoke())
            {
                Console.WriteLine(o.ToString());
            }

            Console.WriteLine("Build Complete");
            PrintBuildSize();
        }
        
        public static long PrintFolderSizeMB(DirectoryInfo d)
        {
            long sizeBytes = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                sizeBytes += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                sizeBytes += PrintFolderSizeMB(di);
            }

            var megaByte = ByteSize.FromBytes(sizeBytes).MegaBytes;
            Console.WriteLine($"Build Size : {megaByte} MB");

            return (long)megaByte;
        }

        private static void PrintBuildSize()
        {
            double sizeBytes = DirSize(new DirectoryInfo(projectBuildPath));
            var megaByte = ByteSize.FromBytes(sizeBytes).MegaBytes;
            Console.WriteLine($"Build Size : {megaByte} MB");
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

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

    }
}