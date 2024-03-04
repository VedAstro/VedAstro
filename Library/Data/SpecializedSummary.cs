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
    public class SpecializedSummary
    {
        public static SpecializedSummary Empty => new SpecializedSummary();

        public SummaryMetadata Mind { get; set; }
        public SummaryMetadata Studies { get; set; }
        public SummaryMetadata Family { get; set; }
        public SummaryMetadata Money { get; set; }
        public SummaryMetadata Love { get; set; }
        public SummaryMetadata Body { get; set; }


        public static SpecializedSummary FromXml(XElement xmlElement)
        {
            //todo marked for oblivion

            throw new NotImplementedException();

        }

        public XElement ToXml()
        {
            //todo marked for oblivion

            throw new NotImplementedException();
        }


        public static SpecializedSummary FromJson(JObject jsonObject)
        {
            throw new Exception();
        }


        public static JObject ToJson(SpecializedSummary specializedNature)
        {
            throw new Exception();
        }
    }

}
