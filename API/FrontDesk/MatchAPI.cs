using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// API with match related stuff
    /// </summary>
    public class MatchAPI
    {


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
        /// Gets all people ordered by kuta total strength 0 is highest kuta score
        /// note : chart created to make score is discarded
        /// </summary>
        public static async Task<List<PersonKutaScore>> GetAllPersonByMatchStrength(Person inputPerson)
        {
            var resultList = new List<MatchReport>();

            //set input person in correct gender order
            var inputPersonIsMale = inputPerson.Gender == Gender.Male;

            //get everybody (skip life events, since not needed & faster)
            var everybody = APITools.GetAllPersonList(true);

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

}