using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    /// <summary>
    /// API Functions related to Person Profiles
    /// </summary>
    public class PersonAPI
    {
        [FunctionName("addperson")]
        public static async Task<IActionResult> AddPerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var newPersonXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new person to main list
                await APITools.AddXElementToXDocumentAzure(newPersonXml, APITools.PersonListFile, APITools.BlobContainerName);

                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        /// <summary>
        /// API to add current user ID to all people created with current VisitorID
        /// note: this is done to auto move profiles created before login, then user decides to login
        /// but expects all the profiles created before to be there in new account/logged in account
        /// </summary>
        [FunctionName("AddUserIdToVisitorPersons")]
        public static async Task<IActionResult> AddUserIdToVisitorPersons(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get new person data out of incoming request
                //note: inside new person xml already contains user id
                var rootXml = APITools.ExtractDataFromRequest(incomingRequest);
                var userId = rootXml.Element("UserId")?.Value;
                var visitorId = rootXml.Element("VisitorId")?.Value;

                //find all person's with inputed visitor ID
                var personListXmlDoc = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);
                var foundPersonList = APITools.FindPersonByUserId(personListXmlDoc, visitorId);

                //add User ID to each person (if not already added, avoid duplicates)
                foreach (var foundPerson in foundPersonList)
                {
                    var currentOwners = foundPerson?.Element("UserId")?.Value ?? "";
                    var notInList = !currentOwners.Equals(userId);
                    //if not in list, add to current person's owners user ID list
                    if (notInList) { foundPerson.Element("UserId").Value = currentOwners + "," + userId; }
                }

                //upload modified list file to storage
                await APITools.SaveXDocumentToAzure(personListXmlDoc, APITools.PersonListFile, APITools.BlobContainerName);

                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }



        /// <summary>
        /// Gets person all details from only hash
        /// </summary>
        [FunctionName("getperson")]
        public static async Task<IActionResult> GetPerson([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get hash that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var personId = requestData.Value;

                //get the person record by hash
                var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);
                var foundPerson = await APITools.FindPersonById(personListXml, personId);

                //send person to caller
                return APITools.PassMessage(foundPerson);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //let caller know fail, include exception info for easy debugging
                return APITools.FailMessage(e);
            }


        }

        /// <summary>
        /// Updates a person's record, uses hash to identify person to overwrite
        /// </summary>
        [FunctionName("updateperson")]
        public static async Task<IActionResult> UpdatePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var updatedPersonXml = APITools.ExtractDataFromRequest(incomingRequest);

                //save a copy of the original person record in recycle bin, just in-case accidental update
                var personId = Person.FromXml(updatedPersonXml).Id;//does not change
                var originalPerson = await APITools.GetPersonFromId(personId);
                await APITools.AddXElementToXDocumentAzure(originalPerson.ToXml(), APITools.RecycleBinFile, APITools.BlobContainerName);

                //directly updates and saves new person record to main list (does all the work, sleep easy)
                await APITools.UpdatePersonRecord(updatedPersonXml);

                //all is good baby
                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        /// <summary>
        /// Deletes a person's record, uses hash to identify person
        /// Note : user id is not checked here because Person hash
        /// can't even be generated by client side if you don't have access.
        /// Theoretically anybody who gets the hash of the person,
        /// can delete the record by calling this API
        /// </summary>
        [FunctionName("DeletePerson")]
        public static async Task<IActionResult> DeletePerson(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var personId = requestData.Value;

                //get the person record that needs to be deleted
                var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);
                var personToDelete = await APITools.FindPersonById(personListXml, personId);

                //add deleted person to recycle bin 
                await APITools.AddXElementToXDocumentAzure(personToDelete, APITools.RecycleBinFile, APITools.BlobContainerName);

                //delete the person record,
                await APITools.DeleteXElementFromXDocumentAzure(personToDelete, APITools.PersonListFile, APITools.BlobContainerName);

                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }
        }

        /// <summary>
        /// Gets all person profiles owned by User ID & Visitor ID
        /// </summary>
        [FunctionName("GetPersonList")]
        public static async Task<IActionResult> GetPersonList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
        //used when visitor id person were moved to user id, shouldn't happen all the time, obviously adds to the lag (needs speed testing) 
        TryAgain:

            try
            {
                //TODO CHECK HERE FOR SAM ENTRY

                //data out of request
                var rootXml = APITools.ExtractDataFromRequest(incomingRequest);
                var userId = rootXml.Element("UserId")?.Value;
                var visitorId = rootXml.Element("VisitorId")?.Value;

                //get all person list from storage
                var personListXml = await APITools.GetXmlFileFromAzureStorage(APITools.PersonListFile, APITools.BlobContainerName);

                //filter out person by user id
                var filteredList1 = APITools.FindPersonByUserId(personListXml, userId);

                //filter out person by visitor id
                var visitorIdList = APITools.FindPersonByUserId(personListXml, visitorId);

                //before sending to user, clean the data
                //if user made profile while logged out then logs in, transfer the profiles created with visitor id to the new user id
                //if this is not done, then when user loses the visitor ID, they also loose access to the person profile
                var loggedIn = userId != "101" && !(string.IsNullOrEmpty(userId));//already logged in if true
                var visitorProfileExists = visitorIdList.Any();
                if (loggedIn && visitorProfileExists)
                {
                    //transfer to user id
                    foreach (var person in visitorIdList)
                    {
                        //edit data direct to for speed
                        person.Element("UserId").Value = userId;

                        //save to main list
                        await APITools.UpdatePersonRecord(person);
                    }

                    //after the transfer, restart the call as though new, so that user only gets the correct list at all times (though this might be little slow)
                    goto TryAgain;
                }


                //combine and remove duplicates
                if (visitorIdList.Any()) { filteredList1.AddRange(visitorIdList); }
                List<XElement> personListNoDupes = filteredList1.Distinct().ToList();

                //convert list to xml
                var xmlPayload = Tools.AnyTypeToXmlList(personListNoDupes);


                //send filtered list to caller
                return APITools.PassMessage(xmlPayload);

            }
            catch (Exception e)
            {
                //log error
                await APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }


        }


    }
}
