using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace VedAstro.Library
{
    public class Prastaraka : IToJson
    {

        private readonly Dictionary<string, Dictionary<ZodiacName, int>> _dictionary = new();

        public Dictionary<ZodiacName, int> this[string index]
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
        
        /// <summary>
        /// Calculated from Prastaraka rows
        /// </summary>
        public Dictionary<ZodiacName, int> BhinnashtakaRow()
        {
            //prepare empty row
            var bhinnashtakaRow = ZodiacNameExtensions.AllZodiacSignsDictionary(0);

            //create each bhinnashtaka cell 1 by 1
            foreach (var bhinnashtakaCell in bhinnashtakaRow)
            {
                var totalSum = 0;   
                //add to all the points for each zodiac
                foreach (var prastarakaRow in _dictionary)
                {
                    totalSum += prastarakaRow.Value[bhinnashtakaCell.Key];
                }

                //add the total sum to the bhinnashtaka cell
                bhinnashtakaRow[bhinnashtakaCell.Key] = totalSum;
            }

            return bhinnashtakaRow;
        }


        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var holder = new JObject();

            //add in the rows for each planet & lagna
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
                holder[item.Key] = valueHolder;
            }

            //add in the final Bhinnashtaka row
            var bhinnashtakaRowData = this.BhinnashtakaRow();

            //make all zodiacs as 1 to 12, with benefics valued 1
            var bhinnashtakaRowJson = RowToJArray(bhinnashtakaRowData, out var bhinnashtakaRowTotal);

            //package the row
            var bhinnashtakaRowHolder = new JObject
            {
                ["Total"] = bhinnashtakaRowTotal,
                ["Rows"] = bhinnashtakaRowJson
            };

            //put into main holder
            holder["Bhinnashtaka"] = bhinnashtakaRowHolder;

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
        public static List<Prastaraka> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Prastaraka>(); }

            var returnList = new List<Prastaraka>();

            foreach (var personJson in personList)
            {
                returnList.Add(Prastaraka.FromJson(personJson));
            }

            return returnList;
        }

        public static Prastaraka FromJson(JToken personInput)
        {
            throw new NotImplementedException();

        }

        #endregion


    }

}
