using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace API
{
    public class SignInAPI
    {

        [FunctionName("SignInGoogle")]
        public static async Task<IActionResult> SignInGoogle([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            //get ID Token/JWT from received request
            var tokenXml = APITools.ExtractDataFromRequest(incomingRequest);
            var jwtToken = tokenXml.Value;

            try
            {
                //validate the the token & get data to id the user
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(jwtToken);
                var userId = validPayload.Subject; //Unique Google User ID
                var userName = validPayload.Name;
                var userEmail = validPayload.Email;

                //use the email to get the user's record (or make new one if don't exist)
                var userData = await APITools.GetUserData(userId, userName, userEmail);

                //todo add login to users log (browser, location, time)
                //todo maybe better in client
                //userData.AddLoginLog();
                //save updated user data

                //send user data as xml in with pass status
                //so that client can generate hash and use it
                return APITools.PassMessage(userData.ToXml());
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                await APILogger.Error(e, incomingRequest);
                return APITools.FailMessage("Login Failed");
            }
        }

        [FunctionName("SignInFacebook")]
        public static async Task<IActionResult> SignInFacebook(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get accessToken from received request
                var tokenXml = APITools.ExtractDataFromRequest(incomingRequest);
                var accessToken = tokenXml.Value;

                //validate the the token & get user data
                var url = $"https://graph.facebook.com/me/?fields=id,name,email&access_token={accessToken}";
                var reply = await APITools.GetRequest(url);
                var jsonText = await reply.Content.ReadAsStringAsync();
                var json = JsonNode.Parse(jsonText);
                var userId = json["id"].ToString();
                var userName = json["name"].ToString();
                var userEmail = json["email"].ToString();

                //use the email to get the user's record (or make new one if don't exist)
                var userData = await APITools.GetUserData(userId, userName, userEmail);

                //send user data as xml in with pass status
                //so that client can generate hash and use it
                return APITools.PassMessage(userData.ToXml());
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                await APILogger.Error(e, incomingRequest);
                return APITools.FailMessage("Login Failed");
            }
        }

    }
}
