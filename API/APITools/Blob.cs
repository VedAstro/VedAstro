using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.Xml.Linq;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {

        /// <summary>
        /// Converts a blob client of a file to string
        /// </summary>
        public static async Task<string> BlobClientToString(BlobClient blobClient)
        {
            try
            {
                var xmlFileString = await DownloadToText(blobClient);

                return xmlFileString;
            }
            catch (Exception e)
            {
                await APILogger.Error(e); //log it
                throw new Exception($"Azure Storage Failure : {blobClient.Name}");
            }

            //Console.WriteLine(blobClient.Name);
            //Console.WriteLine(blobClient.AccountName);
            //Console.WriteLine(blobClient.BlobContainerName);
            //Console.WriteLine(blobClient.Uri);
            //Console.WriteLine(blobClient.CanGenerateSasUri);

            //if does not exist raise alarm
            if (!await blobClient.ExistsAsync())
            {
                Console.WriteLine("NO FILE!");
            }

            //parse string as xml doc
            //var valueContent = blobClient.Download().Value.Content;
            //Console.WriteLine("Text:"+Tools.StreamToString(valueContent));
        }

        /// <summary>
        /// Method from Azure Website
        /// </summary>
        public static async Task<string> DownloadToText(BlobClient blobClient)
        {
            var isFileExist = (await blobClient.ExistsAsync()).Value;

            if (isFileExist)
            {
                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                string downloadedData = downloadResult.Content.ToString();

                return downloadedData;
            }
            else
            {
                //will be logged by caller
                throw new Exception($"No File in Cloud : {blobClient.Name}");
            }

        }


    }
}
