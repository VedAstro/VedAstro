using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Diagnostics;
using System.Management.Automation;
using Azure.Storage.Blobs.Models;
using ByteSizeLib;
using Microsoft.CodeAnalysis;
using MimeTypes;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using Azure.Storage;
using System.Security.Cryptography;
using System.IO;
using Azure;
using Azure.Core.Pipeline;
using Azure.Storage.Sas;
using System.Security.Principal;

namespace Publisher
{
    internal class Program
    {
        private static string projectPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website";
        private static string projectBuildPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\bin\\Release\\net7.0\\publish\\wwwroot";
        private static string projectBuildPath2 = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\bin\\Release\\net7.0\\wwwroot";
        private static string? _nukeOld;
        private static IConfigurationRoot _config;
        private const string _lastmodifiedticks = "lastmodifiedticks";


        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine("Choose wisely");
                Console.WriteLine("1:Build & Upload All");
                Console.WriteLine("2:Build & Upload AOT only");
                Console.WriteLine("3:Build > Delete Minor Files > Sync Upload");
                Console.WriteLine("4:Upload AOT only");
                Console.WriteLine("5:Upload Normal only");
                Console.WriteLine("6:Upload AOT & Normal");
                Console.WriteLine("7:Build AOT & Normal");
                Console.WriteLine("8:Build Normal Only");
                Console.WriteLine("9:Delete Old Website Files Normal (not images)");
                Console.WriteLine("10:Build > Sync Upload");
                Console.WriteLine("11:Sync Upload");
                Console.WriteLine("12:Sync JS WWWROOT");

                var choice = Console.ReadLine();
                //Console.WriteLine("Nuke old website? Y/N");
                //_nukeOld = Console.ReadLine()?.ToLower();

                //init config to get app secrets, like upload string
                _config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();


                switch (choice)
                {
                    case "1":
                        //build the website project
                        BuildProject();

                        //upload to azure
                        UploadToAzureAzCopySync();

                        BuildProjectAOT();

                        //upload to azure
                        await UploadToAzure("vedastrowebsitestorage2");  //NOTE:AOT USES WEBSITE2
                        goto END;
                    case "2": BuildProjectAOT(); await UploadToAzure("vedastrowebsitestorage2"); goto END;
                    case "3": BuildProject(); DeleteWebsiteFiles(); UploadToAzureAzCopySync(); goto END;
                    case "4": await UploadToAzure("vedastrowebsitestorage2"); goto END;
                    case "5": UploadToAzureAzCopySync(); goto END;
                    case "6":
                        Console.WriteLine("NORMAL BUILD UPLOAD");
                        UploadToAzureAzCopySync();
                        Console.WriteLine("AOT BUILD UPLOAD");
                        await UploadToAzure("vedastrowebsitestorage2");  //NOTE:AOT USES WEBSITE2
                        goto END;
                    case "7": BuildProject(); BuildProjectAOT(); goto END;
                    case "8": BuildProject(); goto END;
                    case "9": DeleteWebsiteFiles(); goto END;
                    case "10": BuildProject(); UploadToAzureAzCopySync(); goto END;
                    case "11": UploadToAzureAzCopySync(); goto END;
                    case "12": AzCopySyncJS(); goto END;

                }



            END:
                Console.WriteLine("Done! You can go fly kites now.");

                //hold your horses
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }

            //hold your horses
            Console.ReadLine();
        }

        private static async Task UploadToAzure(string connectionStringName)
        {
            //place where website root is default saved
            string containerName = "$web";

            //get secret connection string from secrets.json (security)
            string connectionString = _config[connectionStringName];

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            if (_nukeOld is "y")
            {
                Console.WriteLine("Nuking old website");
                //delete all files in website first, so that no problem
                DeleteAllFilesInContainer(containerClient);
            }

            //always delete _framework folder in storage
            //not deleting causes errors on run
            var result = await containerClient.GetBlobClient("_framework").DeleteIfExistsAsync();
            var msgTemp = result.Value ? "PASS" : "FAIL";
            Console.WriteLine($"Old website _framework delete {msgTemp}!");

            //wait little for delete to take effect
            await Task.Delay(3000);

            var selectedBuildPath = projectBuildPath;
            Console.WriteLine(selectedBuildPath);
            Console.WriteLine("Blasting new website into space");
            var files = Directory.GetFiles(selectedBuildPath, "*.*", SearchOption.AllDirectories);
            if (files.Length < 1)
            {
                Console.WriteLine("No files in build path 1, Using build path 2");
                selectedBuildPath = projectBuildPath2; //change once here

                //maintains folder structure
                files = Directory.GetFiles(selectedBuildPath, "*.*", SearchOption.AllDirectories);
            }

            Console.WriteLine($"Files Count:{files.Length}");

            for (var i = 0; i < files.Length; i++)
            {
                var filePath = files[i];

                //maintains folder structure
                var blobName = filePath.Replace(selectedBuildPath, "").Replace("\\", "/");

                //var blobClient = GetNewBlobClientWithMaxRetry(blobName, containerClient);
                var blobClient = containerClient.GetBlobClient(blobName);
                //var x = await blobClient2.ExistsAsync();

                // If the blob already exists, get the last modified tick count in the blobs metadata
                long blobLastModifiedTick = 0;
                try
                {
                    if (await blobClient.ExistsAsync())
                    {
                        var blobProperties = (await blobClient.GetPropertiesAsync()).Value;
                        //this line will fail if lastmodifiedticks was not set before
                        blobLastModifiedTick = long.Parse(blobProperties.Metadata[_lastmodifiedticks]);

                    }
                }
                //if fail, blob last tick will be 0, aka outdated
                catch
                {
                    //Console.WriteLine("Failed to get LastModifiedTick:" + blobName);
                }

                //if local file is latest, then upload else skip
                var localLastModified = File.GetLastWriteTimeUtc(filePath);
                var localIsNewer = blobLastModifiedTick < localLastModified.Ticks;
                if (localIsNewer)
                {
                    //print name 1st to know which gets stuck
                    //extra space to delete extra prev
                    Console.Write($"\r{i}/{files.Length} Uploading:{blobName}");

                    //note will overwrite existing
                    await using var fs = File.Open(filePath, FileMode.Open);
                    await blobClient.UploadAsync(fs, overwrite: true, CancellationToken.None);

                    //auto correct content type from wrongly set "octet/stream"
                    var blobHttpHeaders = new BlobHttpHeaders { ContentType = MimeTypeMap.GetMimeType(filePath) };
                    await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
                    await blobClient.SetMetadataAsync(new Dictionary<string, string>() { { _lastmodifiedticks, localLastModified.Ticks.ToString() } });
                }
                else
                {
                    Console.Write($"\r{i}/{files.Length} Skipped:{blobName}"); //space to delete extra previous line
                }
            }

            Console.WriteLine("\nUpload Complete");
        }
        private static void UploadToAzureAzCopySync()
        {
            Console.WriteLine("UploadToAzureAzCopySync : START");

            //set project path to build
            System.Environment.CurrentDirectory = projectPath;

            Console.WriteLine("Upload From Directory:\n"+ projectBuildPath);

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            var script = $@"./azcopy.exe sync '{projectBuildPath}' '{_config["UploadSasUri"]}' --recursive --delete-destination=true";
            ps.AddScript(script);
            ps.Invoke();

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

            Console.WriteLine("UploadToAzureAzCopySync : DONE");
        }
        private static void AzCopySync(string syncLocalPath)
        {
            Console.WriteLine("Start Sync");
            Console.WriteLine("*****************************************************");

            //set project path to build (for powershell)
            System.Environment.CurrentDirectory = projectPath;

            Console.WriteLine("Upload From Directory:\n"+ syncLocalPath);

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            var script = $@"./azcopy.exe sync '{syncLocalPath}' '{_config["UploadSasUri"]}' --recursive --delete-destination=true";
            ps.AddScript(script);
            ps.Invoke();

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("End Sync");
        }
        private static void AzCopySyncJS()
        {
            var syncLocalPath = "C:\\Users\\vigne\\OneDrive\\Desktop\\Genso.Astrology\\Website\\wwwroot\\js";

            Console.WriteLine("Start Sync");
            Console.WriteLine("*****************************************************");

            //set project path to build (for powershell)
            System.Environment.CurrentDirectory = projectPath;

            Console.WriteLine("Upload From Directory:\n"+ syncLocalPath);

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            var script = $@"./azcopy.exe sync '{syncLocalPath}' '{_config["JsFolder"]}' --recursive --delete-destination=true";
            ps.AddScript(script);
            ps.Invoke();

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

            Console.WriteLine("*****************************************************");
            Console.WriteLine("End Sync");
        }

        /// <summary>
        /// Deletes all files that needs to be updated for site to work
        /// </summary>
        private static void DeleteWebsiteFiles()
        {

            Console.WriteLine("DeleteWebsiteFiles Started");

            //set project path to build
            System.Environment.CurrentDirectory = projectPath;

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            //var script = $@"./azcopy.exe sync '{projectBuildPath}' '{_config["UploadSasUri"]}' --recursive --delete-destination=true --log-level=INFO";
            var script = $@"./azcopy.exe remove '{_config["UploadSasUri"]}' --from-to=BlobTrash --list-of-files ""C:\Users\vigne\OneDrive\Desktop\Genso.Astrology\Publisher\WebFilesToDeleteAlways.txt"" --recursive --log-level=INFO";
            ps.AddScript(script);
            ps.Invoke();

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

            Console.WriteLine("Purged Pesky Old Website Files! (not images)");
        }

        /// <summary>
        /// Makes a new blob client, but with custom retry limit 100
        /// </summary>
        private static BlobClient GetNewBlobClientWithMaxRetry(string blobName, BlobContainerClient containerClient)
        {
            //make temp blob client to get correct URI for this blob
            BlobClient blobClientTemp = containerClient.GetBlobClient(blobName);

            //we create another blob client with increased retry limit, reduces failures in slow connection
            var blobUri = blobClientTemp.Uri;
            var options = new BlobClientOptions();
            options.Diagnostics.IsLoggingEnabled = false;
            options.Diagnostics.IsTelemetryEnabled = false;
            options.Diagnostics.IsDistributedTracingEnabled = false;
            options.Retry.MaxRetries = 100;
            StorageSharedKeyCredential storageSharedKeyCredential =
                new StorageSharedKeyCredential("vedastrowebsitestorage2", _config["VedAstroWebsiteStorage2Key1"]);
            var blobClient = new BlobClient(blobUri: blobUri, options: options, credential: storageSharedKeyCredential);

            return blobClient;
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
            Console.WriteLine("*****************************************************");

            //delete all existing builds
            CleanBuildFolder();

            //set project path to build
            System.Environment.CurrentDirectory = projectPath;

            //run build from power shell since correct dir
            var ps = PowerShell.Create();
            ps.AddScript(@"dotnet publish -c Release -p:Flavor=AOT_OFF");
            ps.Invoke();

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

            Console.WriteLine("*****************************************************");
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

            //print build output
            foreach (PSObject o in ps.Invoke()) { Console.WriteLine(o.ToString()); }

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
            try
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
            catch (Exception e)
            {
                Console.WriteLine("SKIPPED: OLD BUILD DELETE ERROR");
            }

            try
            {
                var di = new DirectoryInfo(projectBuildPath2);
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (var dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("SKIPPED: OLD BUILD DELETE ERROR");
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