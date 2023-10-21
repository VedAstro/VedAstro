using System.Net.Mime;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;
using Azure.Storage.Blobs;
using Person = VedAstro.Library.Person;
using Azure.Data.Tables;
using Newtonsoft.Json.Linq;

namespace API
{
    /// <summary>
    /// API Functions related to Person Profiles
    /// </summary>
    public class PersonAPI
    {
        private const string AccountName = "vedastrocentralstorage";
        private static readonly string StorageAccountKey = Secrets.VedAstroCentralStorageKey;

        private const string PersonListName = "PersonList";
        private const string PersonListRecycleBinName = "PersonListRecycleBin";
        private const string LifeEventListName = "LifeEventList";
        private const string PersonListUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListName}";
        private const string PersonListRecycleBinUri = $"https://vedastrocentralstorage.table.core.windows.net/{PersonListRecycleBinName}";
        private const string LifeEventListUri = $"https://vedastrocentralstorage.table.core.windows.net/{LifeEventListName}";
        public static readonly TableClient? PersonListTable = (new TableServiceClient(new Uri(PersonListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListName);
        private static readonly TableClient? PersonListRecycleBinTable = (new TableServiceClient(new Uri(PersonListRecycleBinUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(PersonListRecycleBinName);
        private static readonly TableClient? LifeEventListTable = (new TableServiceClient(new Uri(LifeEventListUri), new TableSharedKeyCredential(AccountName, StorageAccountKey))).GetTableClient(LifeEventListName);


        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [Function(nameof(GetPerson))]
        public static async Task<HttpResponseData> GetPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPerson/OwnerId/{ownerId}/PersonId/{personId}")] HttpRequestData req,
            string personId, string ownerId)
        {

            try
            {
                //query the database
                var foundCalls = PersonListTable.Query<PersonRow>(row => row.PartitionKey == ownerId && row.RowKey == personId);

                //make into readable format
                var personToReturn = Person.FromAzureRow(foundCalls.FirstOrDefault());

                //send person to caller
                return APITools.PassMessageJson(personToReturn.ToJson(), req);

            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //let caller know fail, include exception info for easy debugging
                return APITools.FailMessageJson(e, req);
            }


        }


        /// <summary>
        /// Intelligible gets a person's image
        /// </summary>
        [Function(nameof(GetPersonImage))]
        public static async Task<HttpResponseData> GetPersonImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetPersonImage/PersonId/{personId}")] HttpRequestData req,
            string personId)
        {
            //start with backup person if all fails
            var personToImage = Person.Empty;
            BlobClient imageFile = null;

            try
            {
                //OPTION 1
                //check directly if custom uploaded image exist, if got end here
                var imageFound = await APITools.IsCustomPersonImageExist(personId);

                if (imageFound)
                {
                    imageFile = APITools.GetPersonImage(personId);
                    return APITools.SendFileToCaller(imageFile, req, MediaTypeNames.Image.Jpeg);
                }

                //OPTION 2 : GET AZURE SEARCHED IMAGED
                else
                {
                    //get the person record by ID
                    var foundPersonXml = await Tools.FindPersonXMLById(personId);
                    personToImage = Person.FromXml(foundPersonXml);
                    byte[] foundImage = await APITools.GetSearchImage(personToImage); //gets most probable fitting person image

                    //save copy of image under profile, so future calls don't spend BING search quota
                    await APITools.SaveNewPersonImage(personToImage.Id, foundImage);

                    //return gotten image as is
                    return APITools.SendFileToCaller(foundImage, req, MediaTypeNames.Image.Jpeg);

                }

            }

            //OPTION 3 : USE ANONYMOUS IMAGE
            //used only when bing and saved records fail
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //get default male or female image
                imageFile = personToImage.Gender == Gender.Male ? APITools.GetPersonImage("male") : APITools.GetPersonImage("female");

                //save copy of image under profile, so future calls don't spend BING search quota
                await APITools.SaveNewPersonImage(personToImage.Id, imageFile);

                //send person image to caller
                return APITools.SendFileToCaller(imageFile, req, MediaTypeNames.Image.Jpeg);
            }

        }


        /// <summary>
        /// .../AddPerson/OwnerId/101322/Name/Juliet/Gender/Female/Location/Singapore/Time/14:14/31/12/1985
        /// </summary>
        [Function(nameof(AddPerson))]
        public static async Task<HttpResponseData> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(AddPerson)}/OwnerId/{{ownerId}}/Name/{{inputName}}/Gender/{{inputGender}}/Location/{{inputLocation}}/Time/{{inputMinute}}/{{inputDate}}/{{inputMonth}}/{{inputYear}}")] HttpRequestData req,
            string ownerId, string inputName, string inputGender, string inputLocation, string inputMinute, string inputDate, string inputMonth, string inputYear)
        {

            try
            {

                //special ID made for human brains
                var brandNewHumanReadyId = await APITools.GeneratePersonId(ownerId, inputName, inputYear);


                //parse time given by caller
                var inputBirthTime = await Tools.ParseTime(
                    locationName: inputLocation,
                    hhmmStr: inputMinute,
                    dateStr: inputDate,
                    monthStr: inputMonth,
                    yearStr: inputYear);

                //get gender
                var parsedGender = Enum.Parse<Gender>(inputGender);

                //make new person
                var newPerson = new Person(ownerId, brandNewHumanReadyId, inputName, inputBirthTime, parsedGender, "");

                //possible old cache of person with same id lived, so clear cache if any
                //delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteStuffRelatedToPerson(newPerson);

                //creates record if no exist, update if already there
                PersonListTable.UpsertEntity(newPerson.ToAzureRow());

                //return ID of newly created person so caller can get use it
                return APITools.PassMessageJson(newPerson.Id, req);
            }
            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }

        }

        /// <summary>
        /// Gets person list
        /// </summary>
        [Function(nameof(GetPersonList))]
        public static async Task<HttpResponseData> GetPersonList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(GetPersonList)}/OwnerId/{{ownerId}}")] HttpRequestData req,
            string ownerId)
        {
            try
            {

                var foundCalls = PersonListTable.Query<PersonRow>(call => call.PartitionKey == ownerId);

                var personJsonList = new JArray();
                foreach (var call in foundCalls)
                {
                    personJsonList.Add(Person.FromAzureRow(call).ToJson());
                }

                return APITools.PassMessageJson(personJsonList, req);

            }

            //if any failure, show error in payload
            catch (Exception e)
            {
                APILogger.Error(e, req);
                return APITools.FailMessageJson(e.Message, req);
            }


        }


        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [Function(nameof(UpdatePerson))]
        public static async Task<HttpResponseData> UpdatePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(UpdatePerson))] HttpRequestData req)
        {

            try
            {
                //get data out of call
                var rootJson = await APITools.ExtractDataFromRequestJson(req);

                //api key to ID the call
                var personParsed = Person.FromJson(rootJson);

                //delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteStuffRelatedToPerson(personParsed);

                await PersonListTable?.UpsertEntityAsync(personParsed.ToAzureRow());

                return APITools.PassMessageJson(req);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessageJson(e, req);
            }

        }

        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [Function(nameof(Migrate))]
        public static async Task<HttpResponseData> Migrate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = nameof(Migrate))] HttpRequestData req)
        {

            try
            {



                //get data out of call
                var rootJson = await APITools.ExtractDataFromRequestJson(req);

                //api key to ID the call
                var personParsed = Person.FromJson(rootJson);

                //delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteStuffRelatedToPerson(personParsed);

                await PersonListTable?.UpsertEntityAsync(personParsed.ToAzureRow());

                return APITools.PassMessageJson(req);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessageJson(e, req);
            }

        }


        /// <summary>
        /// Deletes a person's record, uses hash to identify person
        /// Note : user id is not checked here because Person hash
        /// can't even be generated by client side if you don't have access.
        /// Theoretically anybody who gets the hash of the person,
        /// can delete the record by calling this API
        /// </summary>
        [Function(nameof(DeletePerson))]
        public static async Task<HttpResponseData> DeletePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "DeletePerson/OwnerId/{ownerId}/PersonId/{personId}")] HttpRequestData req,
            string ownerId, string personId)
        {
            try
            {
                //# get full person copy to place in recycle bin
                //query the database
                var foundCalls = PersonListTable?.Query<PersonRow>(row => row.PartitionKey == ownerId && row.RowKey == personId);
                //make into readable format
                var personAzureRow = foundCalls?.FirstOrDefault();
                var personToDelete = Person.FromAzureRow(personAzureRow);

                //# delete data related to person (NOT USER, PERSON PROFILE)
                await AzureCache.DeleteStuffRelatedToPerson(personToDelete);

                //# add deleted person to recycle bin
                await PersonListRecycleBinTable.UpsertEntityAsync(personAzureRow);

                //# do final delete from MAIN DATABASE
                await PersonListTable.DeleteEntityAsync(ownerId, personId);

                return APITools.PassMessageJson(req);

            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, req);

                //format error nicely to show user
                return APITools.FailMessageJson(e, req);
            }
        }


    }
}
