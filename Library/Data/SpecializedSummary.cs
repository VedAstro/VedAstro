using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Hold data inside each single aspect of life, Money, Body
    /// </summary>
    public class SummaryMetadata
    {
        /// <summary>
        /// Hold data inside each single aspect of life, Money, Body
        /// </summary>
        /// <param name="weight">score is relative to the rest, higher is stronger influence</param>
        /// <param name="nature">overall nature for this aspect</param>
        public SummaryMetadata(double weight, EventNature nature)
        {
            Weight = weight;
            Nature = nature;
        }

        public SummaryMetadata(EventNature nature)
        {
            Weight = 0;
            Nature = nature;
        }

        public double Weight { get; set; }
        public EventNature Nature { get; set; }
    }


    /// <summary>
    /// data summary of the event data & horoscope data description
    /// </summary>
    public class SpecializedSummary : IToJson
    {
        public static SpecializedSummary Empty => new SpecializedSummary();

        public SummaryMetadata Mind { get; set; }
        public SummaryMetadata Studies { get; set; }
        public SummaryMetadata Family { get; set; }
        public SummaryMetadata Money { get; set; }
        public SummaryMetadata Love { get; set; }
        public SummaryMetadata Body { get; set; }



        JObject IToJson.ToJson() => (JObject)this.ToJson();


        public JObject ToJson()
        {
            var json = new JObject
            {
                ["Mind"] = SummaryMetadataToJson(Mind),
                ["Studies"] = SummaryMetadataToJson(Studies),
                ["Family"] = SummaryMetadataToJson(Family),
                ["Money"] = SummaryMetadataToJson(Money),
                ["Love"] = SummaryMetadataToJson(Love),
                ["Body"] = SummaryMetadataToJson(Body)
            };

            return json;
        }

        public static SpecializedSummary FromJson(JToken eventData)
        {
            var summary = new SpecializedSummary
            {
                Mind = eventData["Mind"].ToObject<SummaryMetadata>(),
                Studies = eventData["Studies"].ToObject<SummaryMetadata>(),
                Family = eventData["Family"].ToObject<SummaryMetadata>(),
                Money = eventData["Money"].ToObject<SummaryMetadata>(),
                Love = eventData["Love"].ToObject<SummaryMetadata>(),
                Body = eventData["Body"].ToObject<SummaryMetadata>()
            };

            return summary;
        }

        private static JToken SummaryMetadataToJson(SummaryMetadata metadata)
        {
            if (metadata == null) return null;

            return new JObject
            {
                ["Weight"] = metadata.Weight,
                ["Nature"] = metadata.Nature.ToString()
            };
        }



    }

}
