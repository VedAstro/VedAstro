using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace VedAstro.Library
{
    public class Sarvashtakavarga : IToJson
    {
        public Dictionary<string, int[]> Rows => _rows;

        private readonly Dictionary<string, int[]> _rows = new();

        public int[] this[string index]
        {
            get
            {
                if (_rows.ContainsKey(index))
                {
                    return _rows[index];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _rows[index] = value;
            }
        }

        public Dictionary<ZodiacName, int> SarvashtakavargaRow { get; set; } = ZodiacNameExtensions.AllZodiacSignsDictionary(0);


        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var holder = new JObject();

            //add in the rows for each planet & lagna
            foreach (var item in _rows)
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
            var sarvashtakavargaRowData = this.SarvashtakavargaRow;

            //make all zodiacs as 1 to 12, with benefics valued 1
            var sarvashtakavargaRowJson = RowToJArray(sarvashtakavargaRowData, out var sarvashtakavargaRowTotal);

            //package the row
            var sarvashtakavargaRowHolder = new JObject
            {
                ["Total"] = sarvashtakavargaRowTotal,
                ["Rows"] = sarvashtakavargaRowJson
            };

            //put into main holder
            holder["Sarvashtakavarga"] = sarvashtakavargaRowHolder;

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

        private static JArray RowToJArray(int[] item, out int rowTotal)
        {
            JArray zodiacRowPoints = new JArray();
            rowTotal = 0;
            foreach (var value in item)
            {
                zodiacRowPoints.Add(value);
                //add all the points for total (easy validation)
                rowTotal += value;
            }
            return zodiacRowPoints;
        }


        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<Sarvashtakavarga> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Sarvashtakavarga>(); }

            var returnList = new List<Sarvashtakavarga>();

            foreach (var personJson in personList)
            {
                returnList.Add(Sarvashtakavarga.FromJson(personJson));
            }

            return returnList;
        }

        public static Sarvashtakavarga FromJson(JToken personInput)
        {
            throw new NotImplementedException();

        }

        #endregion


    }

}
