using System.Text.Json.Nodes;
using Google.Apis.Auth;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using VedAstro.Library;

namespace API
{
    public class SignInAPI
    {

        [Function(nameof(SignInGoogle))]
        public static async Task<HttpResponseData> SignInGoogle([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SignInGoogle/Token/{token}")] HttpRequestData incomingRequest, string token)
        {
            try
            {
                //validate the token & get data to id the user
                var validPayload = await GoogleJsonWebSignature.ValidateAsync(token);
                var userId = validPayload.Subject; //Unique Google User ID
                var userName = validPayload.Name;
                var userEmail = validPayload.Email;

                //using email update or add (if it doesn't exist) NOTE: only updates name and ID
                await AddOrUpdateUserData(userId, userName, userEmail);

                //send back to caller only name and id
                var userDataForWebClient = new JObject();
                userDataForWebClient["Name"] = userName;
                userDataForWebClient["Id"] = userId;

                return APITools.PassMessageJson(userDataForWebClient, incomingRequest);
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson("Login Failed", incomingRequest);
            }
        }

        [Function(nameof(SignInFacebook))]
        public static async Task<HttpResponseData> SignInFacebook([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SignInFacebook/Token/{token}")] HttpRequestData incomingRequest, string token)
        {
            try
            {
                //validate the the token & get user data
                var url = $"https://graph.facebook.com/me/?fields=id,name,email&access_token={token}";
                var reply = await APITools.GetRequest(url);
                var jsonText = await reply.Content.ReadAsStringAsync();
                var json = JsonNode.Parse(jsonText);
                var userId = json["id"].ToString();
                var userName = json["name"].ToString();
                var userEmail = json["email"].ToString();

                //using email update or add (if it doesn't exist) NOTE: only updates name and ID
                await AddOrUpdateUserData(userId, userName, userEmail);

                //send back to caller only name and id
                var userDataForWebClient = new JObject();
                userDataForWebClient["Name"] = userName;
                userDataForWebClient["Id"] = userId;

                return APITools.PassMessageJson(userDataForWebClient, incomingRequest);
            }
            //if any failure, reply as in valid login & log the event
            catch (Exception e)
            {
                APILogger.Error(e, incomingRequest);
                return APITools.FailMessageJson("Login Failed", incomingRequest);
            }
        }

        [Function(nameof(FacebookDeauthorize))]
        public static async Task<HttpResponseData> FacebookDeauthorize([HttpTrigger(AuthorizationLevel.Anonymous, "post,get", Route = null)] HttpRequestData incomingRequest)
        {
            //TODO change URL in FB
            //facebook pings this when user Deauthorize facebook login
            //https://api.vedastro.org/FacebookDeauthorize 

            ApiStatistic.Log(incomingRequest); //logger

            return APITools.PassMessageJson(incomingRequest);
        }



        //--------------------PRIVATE FUNC---------------------------------

        /// <summary>
        /// add or update new user data to DB
        /// </summary>
        private static async Task AddOrUpdateUserData(string userId, string userName, string userEmail)
        {
            // Package data
            var userData = new UserData(userId, userName, userEmail);

            // Add/update user data
            await AzureTable.UserDataList.UpsertEntityAsync(userData.ToAzureRow());
        }


    }
}
