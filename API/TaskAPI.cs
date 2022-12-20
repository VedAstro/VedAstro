using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    public class TaskAPI
    {
        [FunctionName("addtask")]
        public static async Task<IActionResult> AddTask([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get new task data out of incoming request 
                var newTaskXml = APITools.ExtractDataFromRequest(incomingRequest);

                //add new task to main list
                await APITools.AddXElementToXDocumentAzure(newTaskXml, APITools.TaskListFile, APITools.BlobContainerName);

                return APITools.PassMessage();
            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }

        [FunctionName("gettasklist")]
        public static async Task<IActionResult> GetTaskList([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            var responseMessage = "";

            try
            {
                //get task list from storage
                var taskListXml = await APITools.GetXmlFileFromAzureStorage(APITools.TaskListFile, APITools.BlobContainerName);

                //send task list to caller
                responseMessage = taskListXml.ToString();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


            var okObjectResult = new OkObjectResult(responseMessage);

            return okObjectResult;
        }

    }
}

