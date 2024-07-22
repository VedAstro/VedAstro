using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using VedAstro.Library;
using System.Net.Mime;

namespace API
{
    /// <summary>
    /// API with match related stuff
    /// </summary>
    public class MatchAPI
    {
        //NEW



        //------------

        //PUBLIC API

        /// <summary>
        /// Called by API callers
        /// TODO MARKED FOR OBLIVIONS REPLACED BY OPEN API
        /// </summary>
        [Function(nameof(Match))]
        public static async Task<HttpResponseData> Match([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get data out of call
                var rootJson = await APITools.ExtractDataFromRequestJson(incomingRequest);

                //api key to ID the call
                var apiKey = rootJson["APIKey"]?.Value<string>(); //treated here as owner ID also User ID todo if no id handle

                //get person list
                var personListArray = rootJson["PersonList"];

                //convert json list to parsed person list
                var ownerId = apiKey; //every person needs owner
                var personList = await MatchAPIGetPersons(personListArray, ownerId);

                //get 1st and 2nd only for now (todo support more)
                var male = personList[0];
                var female = personList[1];

                //generate compatibility report
                var compatibilityReport = MatchReportFactory.GetNewMatchReport(male, female, ownerId);
                var reportJSON = compatibilityReport.ToJson();
                return APITools.PassMessageJson(reportJSON, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessageJson(e, incomingRequest);
            }
        }

        /// <summary>
        /// called by match report page
        /// </summary>
        [Function("getmatchreport")]
        public static async Task<HttpResponseData> GetMatchReport([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get name of male & female
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var maleId = rootXml.Element("MaleId")?.Value;
                var femaleId = rootXml.Element("FemaleId")?.Value;
                var visitorId = rootXml.Element("VisitorId")?.Value;
                var userId = rootXml.Element("UserId")?.Value;

                var callerId = Tools.GetCallerId(userId, visitorId);

                //generate compatibility report
                var compatibilityReport = await GetNewMatchReport(maleId, femaleId, callerId); //caller id will be unique
                return APITools.PassMessage((XElement)compatibilityReport.ToXml(), incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        [Function("GetSavedMatchReport")]
        public static async Task<HttpResponseData> GetSavedMatchReport([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData incomingRequest)
        {
            try
            {
                //get name of male & female
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var matchId = rootXml.Element("MatchId")?.Value;

                //find match based on match ID
                var MatchReport = await APITools.FindSavedMatchReportById(matchId);
                return APITools.PassMessage((XElement)MatchReport.ToXml(), incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        /// <summary>
        /// Saves match data between people in SavedMatchReportList.xml
        /// </summary>
        [Function("SaveMatchReport")]
        public static async Task<HttpResponseData> SaveMatchReport([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData incomingRequest)
        {
            try
            {
                //get name of male & female
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var maleId = rootXml.Element("MaleId")?.Value;
                var femaleId = rootXml.Element("FemaleId")?.Value;
                var userId = rootXml.Element("UserId")?.Value; //
                var visitorId = rootXml.Element("VisitorId")?.Value; //id of the owner can be visitor id

                var callerId = Tools.GetCallerId(userId, visitorId);

                //generate compatibility report
                var compatibilityReport = await GetNewMatchReport(maleId, femaleId, callerId);
                var chartReadyToInject = compatibilityReport.ToXml();

                //save chart into storage
                //note: todo do not wait to speed things up, beware! failure will go undetected on client
                await Tools.AddXElementToXDocumentAzure(chartReadyToInject, APITools.SavedMatchReportList, Tools.BlobContainerName);

                //let caller know all good
                return APITools.PassMessage(incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        [Function("GetMatchReportList")]
        public static async Task<HttpResponseData> GetMatchReportList([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData incomingRequest)
        {
            AppInstance.IncomingRequest = incomingRequest;

            try
            {
                //STAGE 1 : GET DATA OUT
                //data out of request
                var rootXml = await APITools.ExtractDataFromRequestXml(incomingRequest);
                var userId = rootXml.Element("UserId")?.Value;
                var visitorId = rootXml.Element("VisitorId")?.Value;

                //STAGE 2 : SWAP DATA TODO
                //swap visitor ID with user ID if any (data follows user when log in)
                //await APITools.SwapUserId(new CallerInfo(visitorId, userId), APITools.SavedMatchReportList);

                //STAGE 3 : FILTER
                //get updated all match reports (after swap)
                var savedMatchReportList = await Tools.GetXmlFileFromAzureStorage(APITools.SavedMatchReportList, Tools.BlobContainerName);
                //filter out record by user id
                var userIdList = Tools.FindXmlByUserId(savedMatchReportList, userId);

                //STAGE 4 : SEND XML todo JSON
                //convert list to xml
                var xmlPayload = Tools.AnyTypeToXmlList(userIdList);

                //send filtered list to caller
                return APITools.PassMessage(xmlPayload, incomingRequest);
            }
            catch (Exception e)
            {
                //log error
                APILogger.Error(e, incomingRequest);
                //format error nicely to show user
                return APITools.FailMessage(e, incomingRequest);
            }
        }

        [Function(nameof(FindMatch))]
        public static async Task<HttpResponseData> FindMatch([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FindMatch/PersonId/{personId}")] HttpRequestData incomingRequest, string personId)
        {

            var person = Tools.GetPersonById(personId);

            var personList = await GetAllPersonByMatchStrength(person);

            var returnJson = PersonKutaScore.ToJsonList(personList);


            return APITools.PassMessageJson(returnJson, incomingRequest);
        }



        //PRIVATE

        /// <summary>
        /// special converter for MATCH API anonymous call
        /// when caller asks for match direct with person details
        /// </summary>
        private static async Task<List<Person>> MatchAPIGetPersons(JToken personListArray, string apiKey)
        {
            var returnList = new List<Person>();

            foreach (var personJson in personListArray)
            {
                //NOTE: the person data received here is akin to user entering it via simple editor (so keep it human ready)

                //create new person or use back old person ID

                //1: extract the data out
                var name = personJson["Name"]?.Value<string>() ?? "";
                var genderText = personJson["Gender"]?.Value<string>() ?? "";
                var birthTimeJson = personJson["BirthTime"];

                //we're inside birth time
                var stdTime = birthTimeJson["StdTime"].Value<string>();
                var locationName = birthTimeJson["LocationName"].Value<string>();

                //2: make new person

                //parse data received from user
                var results = await Tools.AddressToGeoLocation(locationName);
                if (!results.IsPass) { throw new Exception($"Failed to get location {locationName}"); }
                var geoLocation = results.Payload;

                //var birthTime = await _timeInput.GetTime(geoLocation);
                var birthTime = new Time(stdTime, geoLocation);

                //each person profile must have a owner, here the unique API key
                var ownerId = apiKey;

                //there is possibility user has put invalid characters, clean it!
                var nameInput = Tools.CleanAndFormatNameText(name);

                //new person ID made from thin air 
                var brandNewHumanReadyId = await PersonManagerTools.GeneratePersonId(ownerId, nameInput, birthTime.StdYear().ToString());

                //get gender from gender string
                var gender = Enum.Parse<Gender>(genderText);

                //create the new complete person profile
                var newPerson = new Person(ownerId, brandNewHumanReadyId, nameInput, birthTime, gender, "");

                //add to list
                returnList.Add(newPerson);

            }

            return returnList;
        }

        /// <summary>
        /// given person id as string will calculate new match report
        /// </summary>
        public static Task<MatchReport> GetNewMatchReport(string maleId, string femaleId, string userId)
        {
            var male = Tools.GetPersonById(maleId);
            var female = Tools.GetPersonById(femaleId);

            //if male & female profile found, make report and return caller
            var notEmpty = !Person.Empty.Equals(male) && !Person.Empty.Equals(female);
            if (notEmpty)
            {
                return Task.FromResult(MatchReportFactory.GetNewMatchReport(male, female, userId));
            }
            else
            {
                throw new Exception(AlertText.PersonProfileNoExist);
            }
        }


        /// <summary>
        /// Gets all people ordered by kuta total strength 0 is highest kuta score
        /// note : chart created to make score is discarded
        /// </summary>
        public static async Task<List<PersonKutaScore>> GetAllPersonByMatchStrength(Person inputPerson)
        {
            var resultList = new List<MatchReport>();

            //set input person in correct gender order
            var inputPersonIsMale = inputPerson.Gender == Gender.Male;

            //get everybody
            var everybody = APITools.GetAllPersonList();

            //this makes sure each person is cross checked against this person correctly
            foreach (var personMatch in everybody)
            {
                //skip own record
                if (personMatch.Equals(inputPerson)) { continue; }

                //add report to list
                MatchReport report;

                //sex orientation depends on input person only
                //in other words input person is always placed in correct sex calculator
                //note : done so that same sex can be checked without to much code
                //       & male can be checked from female position
                if (inputPersonIsMale)
                {
                    report = MatchReportFactory.GetNewMatchReport(inputPerson, personMatch, "101");
                }
                //input person is female
                else
                {
                    report = MatchReportFactory.GetNewMatchReport(personMatch, inputPerson, "101");
                }

                resultList.Add(report);
            }

            //SORT
            //order the list by strength, highest at 0 index
            var resultListOrdered = resultList.OrderByDescending(o => o.KutaScore).ToList();

            //only above 70 should be considered perfect
            var minimumScore = 70;

            //FILTER
            //needs to meets minimum score to make into list
            var finalList =
                from matchReport in resultListOrdered
                where matchReport.KutaScore >= minimumScore
                select matchReport;

            //package together all the needed data
            //get needed details, person name and score to them
            List<PersonKutaScore> personList2;
            personList2 = finalList.Select(matchReport =>
            {
                //if male put in female
                //if female put in male
                var matchPerson = inputPersonIsMale ? matchReport.Female : matchReport.Male;
                var id = matchPerson.Id;
                var name = matchPerson.Name;
                var gender = matchPerson.Gender;
                var age = matchPerson.GetAge();
                return new PersonKutaScore(id, name, gender, age, matchReport.KutaScore);
            }).ToList();

            return personList2;
        }

    }


    public class AppInstance
    {
        public static HttpRequestData IncomingRequest { get; set; }
    }
}