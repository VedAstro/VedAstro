using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    public record PersonKutaScore(string PersonId, string PersonName, double KutaScore)
    {
        public static List<PersonKutaScore> FromJsonList(JToken personKutaScoreList)
        {
            var returnData = new List<PersonKutaScore>();
            foreach (var personKutaScoreJson in personKutaScoreList)
            {
                var PersonId = personKutaScoreJson["PersonId"].Value<string>();
                var PersonName = personKutaScoreJson["PersonName"].Value<string>();
                var KutaScore = personKutaScoreJson["KutaScore"].Value<double>();
                var temp = new PersonKutaScore(PersonId, PersonName, KutaScore);

                returnData.Add(temp);
            }

            return returnData;
        }



        public static JArray ToJsonList(List<PersonKutaScore> personList)
        {
            var returnJson = new JArray();
            foreach (var personKutaScore in personList)
            {
                //wrap data nicely
                var wrapped = new JObject();
                wrapped["PersonId"] = personKutaScore.PersonId;
                wrapped["PersonName"] = personKutaScore.PersonName;
                wrapped["KutaScore"] = personKutaScore.KutaScore;

                //add to final list
                returnJson.Add(wrapped);
            }

            return returnJson;
        }
    }
}
