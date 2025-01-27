using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace VedAstro.Library
{
    public enum LifeAspect
    {
        Finance,
        Romance,
        Education,
        Health
    }

    public class NumerologyPrediction(PlanetName planet, int number, string prediction, Dictionary<LifeAspect, int> predictionSummary) : IToJson
    {
        public PlanetName Planet { get; set; } = planet;
        public int Number { get; set; } = number;
        public string Prediction { get; set; } = prediction;
        
        /// <summary>
        /// Score range 0 to 100
        /// </summary>
        public Dictionary<LifeAspect, int> PredictionSummary { get; set; } = predictionSummary;

        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject
            {
                ["Planet"] = Planet.ToString(),
                ["Number"] = Number,
                ["Prediction"] = Prediction,
                ["PredictionSummary"] = new JObject()
            };

            foreach (var item in PredictionSummary)
            {
                temp["PredictionSummary"][item.Key.ToString()] = item.Value;
            }

            return temp;

        }


        #endregion

    }

}
