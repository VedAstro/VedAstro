using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Genso.Astrology.Library.Compatibility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    public class MessageAPI
    {

        [FunctionName("getmessagelist")]
        public static async Task<IActionResult> GetMessageList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            var responseMessage = "";

            try
            {
                //get message list from storage
                var messageListXml = await APITools.GetXmlFileFromAzureStorage(APITools.MessageListFile, APITools.BlobContainerName);


                //send task list to caller
                return APITools.PassMessage(messageListXml.Root);


            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

        [FunctionName("addmessage")]
        public static async Task<IActionResult> AddMessage([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get new message data out of incoming request
                //note: inside new person xml already contains user id
                var newMessageXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new message to main list
                await APITools.AddXElementToXDocumentAzure(newMessageXml, APITools.MessageListFile, APITools.BlobContainerName);

                //TODO send email to admin

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
