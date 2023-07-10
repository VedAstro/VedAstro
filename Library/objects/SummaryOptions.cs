using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    public enum Algorithm
    {
        Empty = 0,
        MK1,
        MK2,
        MK3,
    }
    public record SummaryOptions(Algorithm SelectedAlgorithm, bool IncludeBase)
    {
        public static SummaryOptions Empty = new SummaryOptions(Algorithm.Empty, true);

        public static SummaryOptions FromJson(JToken summaryOptionsJson)
        {
            var selectedAlgorithmString = summaryOptionsJson["SelectedAlgorithm"].Value<string>();// 

            Algorithm algorithm;
            Enum.TryParse(selectedAlgorithmString, out algorithm);

            var includeBase = summaryOptionsJson["IncludeBase"].Value<bool>();// 

            var parsedTime = new SummaryOptions(algorithm, includeBase);

            return parsedTime;

        }
        

        public JObject ToJson()
        {
            var temp = new JObject();
            temp[nameof(SelectedAlgorithm)] = this.SelectedAlgorithm.ToString();
            temp[nameof(IncludeBase)] = this.IncludeBase;

            return temp;
        }

        public static SummaryOptions FromXml(XElement summaryOptionsXml)
        {
            throw new NotImplementedException();
        }
    }
}
