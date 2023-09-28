using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    public record PersonKutaScore(string PersonId, string PersonName, Gender Gender, int Age, double KutaScore)
    {

        public static List<PersonKutaScore> FromJsonList(JToken personKutaScoreList)
        {
            var returnData = new List<PersonKutaScore>();
            foreach (var personKutaScoreJson in personKutaScoreList)
            {
                var personId = personKutaScoreJson["PersonId"]?.Value<string>() ?? "EMPTY"; //mark empty for detection
                var personName = personKutaScoreJson["PersonName"]?.Value<string>() ?? "EMPTY";
                var genderStr = personKutaScoreJson["Gender"]?.Value<string>() ?? "Empty";
                var gender = Enum.Parse<Gender>(genderStr, true);
                var age = personKutaScoreJson["Age"]?.Value<int>() ?? 0;
                var kutaScore = personKutaScoreJson["KutaScore"]?.Value<double>() ?? 0;
                var temp = new PersonKutaScore(personId, personName, gender, age, kutaScore);

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
                wrapped["Gender"] = personKutaScore.Gender.ToString();
                wrapped["Age"] = personKutaScore.Age;
                wrapped["KutaScore"] = personKutaScore.KutaScore;

                //add to final list
                returnJson.Add(wrapped);
            }

            return returnJson;
        }

    }
}
