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
                //todo log the error here
                Console.WriteLine(e);
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
                var downloadResult2 = (await blobClient.DownloadStreamingAsync()).Value.Content;

                var xDoc = await XDocument.LoadAsync(downloadResult2, LoadOptions.None, CancellationToken.None);

                BlobDownloadResult downloadResult = await blobClient.DownloadContentAsync();
                string downloadedData = downloadResult.Content.ToString();
                //Console.WriteLine("Downloaded data:", downloadedData);
                return downloadedData;

            }
            else
            {
                //will be logged by caller
                throw new Exception($"No File in Cloud : {blobClient.Name}");
            }

        }

        /// <summary>
        /// Converts a blob client of a file to an XML document
        /// </summary>
        public static async Task<XDocument> DownloadToXDoc(BlobClient blobClient)
        {
            var isFileExist = (await blobClient.ExistsAsync()).Value;

            if (isFileExist)
            {
                XDocument xDoc;
                await using (var stream = (await blobClient.DownloadStreamingAsync()).Value.Content)
                {
                    xDoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
                }

#if DEBUG
                Console.WriteLine($"Downloaded: {blobClient.Name}");
#endif

                return xDoc;
            }
            else
            {
                //will be logged by caller
                throw new Exception($"No File in Cloud! : {blobClient.Name}");
            }

        }

    }
}
