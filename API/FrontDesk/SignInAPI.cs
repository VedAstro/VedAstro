using System.Text.Json.Nodes;
using Google.Apis.Auth;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;

namespace API
{
    public class SignInAPI
    {

        [Function(nameof(SignInGoogle))]
        public static async Task<HttpResponseData> SignInGoogle([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            //get ID Token/JWT from received request
            var tokenXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
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
                return APITools.PassMessage(userData.ToXml(), incomingRequest);
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessage("Login Failed", incomingRequest);
            }
        }

        [Function(nameof(SignInFacebook))]
        public static async Task<HttpResponseData> SignInFacebook([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {

            try
            {
                //get accessToken from received request
                var tokenXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
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
                return APITools.PassMessage(userData.ToXml(), incomingRequest);
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessage("Login Failed", incomingRequest);
            }
        }

        [Function(nameof(FacebookDeauthorize))]
        public static async Task<HttpResponseData> FacebookDeauthorize([HttpTrigger(AuthorizationLevel.Anonymous, "post,get", Route = null)] HttpRequestData incomingRequest)
        {

            //facebook pings this when user Deauthorize facebook login
            //https://api.vedastro.org/FacebookDeauthorize 

            ApiStatistic.Log(incomingRequest); //logger

            return APITools.PassMessage(incomingRequest);
        }

    }
}
