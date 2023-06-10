using Azure.Storage.Blobs;
using System.Xml.Linq;
using Azure.Storage.Blobs.Models;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {
        /// <summary>
        /// Gets file blob client from azure storage by name
        /// </summary>
        public static async Task<BlobClient> GetBlobClientAzure(string fileName, string blobContainerName)
        {
            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.API_STORAGE;

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);
            var fileBlobClient = blobContainerClient.GetBlobClient(fileName);

            return fileBlobClient;

            //var returnStream = new MemoryStream();
            //await fileBlobClient.DownloadToAsync(returnStream);

            //return returnStream;
        }

        /// <summary>
        /// check if file exist in aZure storage
        /// </summary>
        public static async Task<bool> IsCustomPersonImageExist(string personId)
        {

            var blobContainerName = "$web";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.WEB_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            //do the actual code 
            var isCustomPersonImageExist = await fileBlobClient.ExistsAsync();

            var x = isCustomPersonImageExist.Value;
            return x;

        }


        /// <summary>
        /// Given an image in byte form, will save it as Person profile image in correct place with ID as file name
        /// </summary>
        public static async Task SaveNewPersonImage(string personId, byte[] imageBytes)
        {

            var blobContainerName = "$web";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.WEB_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            //BinaryData yy = BinaryData.FromBytes(imageBytes);

            using var ms = new MemoryStream(imageBytes);
            await fileBlobClient.UploadAsync(ms, overwrite: true);

            //            var blobUploadOptions = new BlobUploadOptions();
            //            blobUploadOptions.AccessTier = AccessTier.Cold;
            //            var result = await fileBlobClient.UploadAsync(yy, blobUploadOptions, CancellationToken.None);

            //#if DEBUG
            //            Console.WriteLine(result.Value);
            //#endif


        }

        /// <summary>
        /// Given an image in blob form, will save it as Person profile image in correct place with ID as file name
        /// </summary>
        public static async Task SaveNewPersonImage(string personId, BlobClient blobToUpload)
        {

            var blobContainerName = "$web";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.WEB_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //place to save new image
            var imageFile = $"images/person/{personId}.jpg";
            var oldImageToReplace = blobContainerClient.GetBlobClient(imageFile);

            // assume that if the following doesn't throw an exception, then it is successful.
            CopyFromUriOperation operation = await oldImageToReplace.StartCopyFromUriAsync(blobToUpload.Uri, null, AccessTier.Cool);
            await operation.WaitForCompletionAsync();
        }

        /// <summary>
        /// gets image already stored in Images/Person as blobclient based on image name, without file format
        /// </summary>
        public static BlobClient GetPersonImage(string personId)
        {

            var blobContainerName = "$web";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.WEB_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            return fileBlobClient;


        }

        /// <summary>
        /// Gets XML file from Azure blob storage
        /// </summary>
        public static async Task<XDocument> GetXmlFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await DownloadToXDoc(fileClient);

            return xmlFile;
        }

        /// <summary>
        /// Gets any file from Azure blob storage in string form
        /// </summary>
        public static async Task<string> GetStringFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await BlobClientToString(fileClient);

            return xmlFile;
        }

        /// <summary>
        /// Adds an XML element to XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task AddXElementToXDocumentAzure(XElement dataXml, string fileName, string containerName)
        {
            //get user data list file (UserDataList.xml) Azure storage
            var fileClient = await GetBlobClientAzure(fileName, containerName);

            //add new log to main list
            var updatedListXml = await AddXElementToXDocument(fileClient, dataXml);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, updatedListXml);
        }

        /// <summary>
        /// Deletes an XML element from an XML document in by file & container name
        /// and saves files directly to Azure blob store
        /// </summary>
        public static async Task DeleteXElementFromXDocumentAzure(XElement dataXmlToDelete, string fileName, string containerName)
        {
            //access to file
            var fileClient = await GetBlobClientAzure(fileName, containerName);
            //get xml file
            var xmlDocFile = await DownloadToXDoc(fileClient);

            //check if record to delete exists
            //if not found, raise alarm
            var xmlRecordList = xmlDocFile.Root.Elements();
            var personToDelete = Person.FromXml(dataXmlToDelete);
            var foundRecords = xmlRecordList.Where(x => Person.FromXml(x).Id == personToDelete.Id);
            if (!foundRecords.Any()) { throw new Exception("Could not find XML record to delete in main list!"); }

            //continue with delete
            foundRecords.First().Remove();

            //upload modified list to storage
            await OverwriteBlobData(fileClient, xmlDocFile);
        }

        /// <summary>
        /// Saves XML file direct to Azure storage
        /// </summary>
        public static async Task SaveXDocumentToAzure(XDocument dataXml, string fileName, string containerName)
        {
            //get file client for file
            var fileClient = await GetBlobClientAzure(fileName, containerName);

            //upload modified list to storage
            await OverwriteBlobData(fileClient, dataXml);
        }

    }
}
