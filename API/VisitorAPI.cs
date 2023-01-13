using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    public class VisitorAPI
    {
        [FunctionName("addvisitor")]
        public static async Task<IActionResult> AddVisitor([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get new visitor data out of incoming request 
                var newVisitorXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new visitor to main list
                await APITools.AddXElementToXDocumentAzure(newVisitorXml, APITools.VisitorLogFile, APITools.BlobContainerName);

                return APITools.PassMessage();
            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        /// <summary>
        /// Gets all the unique visitors to the site
        /// </summary>
        [FunctionName("getvisitorlist")]
        public static async Task<IActionResult> GetVisitorList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {

                //get user id
                var userId = APITools.ExtractDataFromRequest(incomingRequest).Value;

                //get visitor log from storage
                var visitorLogXml = await APITools.GetXmlFileFromAzureStorage(APITools.VisitorLogFile, APITools.BlobContainerName);

                //get all unique visitor elements only
                //var uniqueVisitorList = from visitorXml in visitorLogXml.Root?.Elements()
                //                        where
                //                            //note: location tag only exists for new visitor log,
                //                            //so use that to get unique list
                //                            visitorXml.Element("Location") != null
                //                        select visitorXml;

                //send list to caller
                return APITools.PassMessage(visitorLogXml.Root);

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FailMessage(e);
            }
        }

        [FunctionName("deletevisitorbyuserid")]
        public static async Task<IActionResult> DeleteVisitorByUserId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var userIdXml = APITools.ExtractDataFromRequest(incomingRequest);
                var userId = userIdXml.Value;

                //get all visitor elements that needs to be deleted
                var visitorLogClient = await APITools.GetBlobClientAzure(APITools.VisitorLogFile, APITools.BlobContainerName);
                var visitorListXml = await APITools.BlobClientToXmlDoc(visitorLogClient);
                var visitorLogsToDelete = visitorListXml.Root?.Elements().Where(x => x.Element("UserId")?.Value == userId).ToList();

                //delete each record
                foreach (var visitorXml in visitorLogsToDelete)
                {
                    visitorXml.Remove();
                }

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, visitorListXml);

                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }

        [FunctionName("deletevisitorbyvisitorid")]
        public static async Task<IActionResult> DeleteVisitorByVisitorId([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var visitorIdXml = APITools.ExtractDataFromRequest(incomingRequest);
                var visitorId = visitorIdXml.Value;

                //get all visitor elements that needs to be deleted
                var visitorLogClient = await APITools.GetBlobClientAzure(APITools.VisitorLogFile, APITools.BlobContainerName);
                var visitorListXml = await APITools.BlobClientToXmlDoc(visitorLogClient);
                var visitorLogsToDelete = (from xml in visitorListXml.Root?.Elements()
                                           where xml.Element("VisitorId")?.Value == visitorId
                                           select xml).ToList();

                //delete each record
                foreach (var visitorXml in visitorLogsToDelete)
                {
                    visitorXml.Remove();
                }

                //upload modified list to storage
                await APITools.OverwriteBlobData(visitorLogClient, visitorListXml);

                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }

        }


    }
}
