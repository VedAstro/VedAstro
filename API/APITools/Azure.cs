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
        /// check if a person's profile image already exist on server
        /// </summary>
        public static async Task<bool> IsCustomPersonImageExist(string personId)
        {

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.API_STORAGE; //place where is image is stored

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

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.API_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            using var ms = new MemoryStream(imageBytes);
            var blobUploadOptions = new BlobUploadOptions();
            blobUploadOptions.AccessTier = AccessTier.Cool; //save money!

            //note no override needed because specifying BlobUploadOptions, is auto override
            await fileBlobClient.UploadAsync(content:ms, options: blobUploadOptions);

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

            var blobContainerName = "vedastro-site-data";

            //get the connection string stored separately (for security reasons)
            //note: dark art secrets are in local.settings.json
            var storageConnectionString = Secrets.API_STORAGE; //place where is image is stored

            //get image from storage
            var blobContainerClient = new BlobContainerClient(storageConnectionString, blobContainerName);

            //get access to file
            var imageFile = $"images/person/{personId}.jpg";
            var fileBlobClient = blobContainerClient.GetBlobClient(imageFile);

            return fileBlobClient;


        }


        /// <summary>
        /// Gets any file from Azure blob storage in string form
        /// </summary>
        public static async Task<string> GetStringFileFromAzureStorage(string fileName, string blobContainerName)
        {
            var fileClient = await Tools.GetBlobClientAzure(fileName, blobContainerName);
            var xmlFile = await BlobClientToString(fileClient);

            return xmlFile;
        }



        /// <summary>
        /// Saves XML file direct to Azure storage
        /// </summary>
        public static async Task SaveXDocumentToAzure(XDocument dataXml, string fileName, string containerName)
        {
            //get file client for file
            var fileClient = await Tools.GetBlobClientAzure(fileName, containerName);

            //upload modified list to storage
            await Tools.OverwriteBlobData(fileClient, dataXml);
        }

    }
}
