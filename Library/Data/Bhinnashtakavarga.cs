using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace VedAstro.Library
{

    /// <summary>
    /// Data wrapper for Bhinnashtakavarga
    /// Bhinnashtakavarga is a table of 7 rows and 12 columns
    /// </summary>
    public class Bhinnashtakavarga : IToJson
    {

        private readonly Dictionary<PlanetName, Dictionary<ZodiacName, int>> _dictionary = new();

        public Dictionary<ZodiacName, int> this[PlanetName index]
        {
            get
            {
                if (_dictionary.ContainsKey(index))
                {
                    return _dictionary[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _dictionary[index] = value;
            }
        }

        public Dictionary<PlanetName, Dictionary<ZodiacName, int>> Dictionary => _dictionary;


        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var holder = new JObject();

            //add in the rows for each planet (only 7 for Bhinnashtakavarga)
            foreach (var item in _dictionary)
            {
                //make all zodiacs as 1 to 12, with benefics valued 1
                var zodiacRowPoints = RowToJArray(item.Value, out var rowTotal);

                //package the row
                var valueHolder = new JObject
                {
                    ["Total"] = rowTotal,
                    ["Rows"] = zodiacRowPoints
                };

                //put into main holder
                holder[item.Key.ToString()] = valueHolder;
            }


            return holder;
        }

        private static JArray RowToJArray(Dictionary<ZodiacName, int> item, out int rowTotal)
        {
            JArray zodiacRowPoints = new JArray();
            rowTotal = 0;
            foreach (var pair in item)
            {
                zodiacRowPoints.Add(pair.Value);

                //add all the points for total (easy validation)
                rowTotal += pair.Value;
            }

            return zodiacRowPoints;
        }

        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<Bhinnashtakavarga> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Bhinnashtakavarga>(); }

            var returnList = new List<Bhinnashtakavarga>();

            foreach (var personJson in personList)
            {
                returnList.Add(Bhinnashtakavarga.FromJson(personJson));
            }

            return returnList;
        }

        public static Bhinnashtakavarga FromJson(JToken personInput)
        {
            throw new NotImplementedException();

        }

        #endregion


    }

}
