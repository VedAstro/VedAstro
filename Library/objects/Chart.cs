using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents an events report chart
    /// Note: made so that a chart can be saved and accessed later
    /// </summary>
    public class Chart
    {

        public static Chart Empty = new Chart("Empty", "Empty", "Empty", new XElement("StartTime"), new XElement("EndTime"), 0, new List<EventTag>());

        public string ChartId { get; set; }
        public string ContentSvg { get; set; }
        public string PersonId { get; }
        public XElement StartTimeXml { get; }
        public XElement EndTimeXml { get; }
        public double DaysPerPixel { get; }
        public List<EventTag> EventTagList { get; set; }
        public Time StartTime => Time.FromXml(StartTimeXml);
        public Time EndTime => Time.FromXml(EndTimeXml);


        public Chart(string chartId, string contentSvg, string personId, XElement startTimeXml,
            XElement endTimeXml, double daysPerPixel, List<EventTag> eventTagList)
        {
            ChartId = chartId;
            ContentSvg = contentSvg;
            PersonId = personId;
            EventTagList = eventTagList;
            StartTimeXml = startTimeXml; //root time xml
            EndTimeXml = endTimeXml; //root time xml
            DaysPerPixel = daysPerPixel;
        }


        public XElement ToXml()
        {
            var chartXml = new XElement("Chart");
            var chartIdXml = new XElement("ChartId", ChartId);
            var contentSvg = new XElement("ContentSvg", ContentSvg);
            var personId = new XElement("PersonId", PersonId);
            var daysPerPixel = new XElement("DaysPerPixel", DaysPerPixel);
            var eventTagListXml = EventTagExtensions.ToXmlList(EventTagList);

            chartXml.Add(
                chartIdXml,
                personId,
                daysPerPixel,
                eventTagListXml,
                new XElement("StartTime", StartTimeXml),
                new XElement("EndTime", EndTimeXml),
                contentSvg);

            return chartXml;
        }

        /// <summary>
        /// Data coming in
        /// </summary>
        public static Chart FromXml(XElement personXml)
        {
            var personId = personXml.Element("PersonId")?.Value;
            var chartId = personXml.Element("ChartId")?.Value; //if no id tag generate new one
            var contentSvg = personXml.Element("ContentSvg")?.Value;
            var eventTagListXml = personXml.Element("EventTagList");
            var eventTagList = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = personXml.Element("StartTime").Element("Time");
            var endTimeXml = personXml.Element("EndTime").Element("Time");
            var daysPerPixel = double.Parse(personXml.Element("DaysPerPixel")?.Value);

            var parsedPerson = new Chart(chartId, contentSvg, personId, startTimeXml, endTimeXml, daysPerPixel, eventTagList);

            return parsedPerson;
        }


        /// <summary>
        /// Use CHART ID instead if possible
        /// Gets HASH of chart ID, reliable
        /// </summary>
        public override int GetHashCode()
        {
            return Tools.GetStringHashCode(ChartId); ;

            ////get hash of all the fields & combine them
            //var hash1 = PersonId.GetHashCode();
            //var hash2 = Tools.GetStringHashCode(StartTimeXml.ToString());
            //var hash3 = Tools.GetStringHashCode(EndTimeXml.ToString());
            //var hash4 = Tools.GetStringHashCode(EventTagListXml.ToString());

            ////take out negative before returning
            //var hashCode = Math.Abs(hash1 + hash2 + hash3 + hash4);
            //return hashCode;
        }

        /// <summary>
        /// Gets a nice identifiable name for this chart to show user 
        /// </summary>
        public object GetFormattedName(string personName)
        {
            var startTime = Time.FromXml(StartTimeXml);
            var startYear = startTime.GetStdYear();
            var startMonth = startTime.GetStdMonth();
            var endTime = Time.FromXml(EndTimeXml);
            var endYear = endTime.GetStdYear();
            var endMonth = endTime.GetStdMonth();
            return $"{personName} - {startMonth}/{startYear} to {endMonth}/{endYear}";
        }

        /// <summary>
        /// create a unique signature to identify all future calls that is exactly alike
        /// todo use this for DURABALE ID when caching chart calls
        /// </summary>
        public string GetEventsChartSignature()
        {

            //convert events into 1 signature
            var eventTagsHash = 0;
            foreach (var eventTag in this.EventTagList) { eventTagsHash += (int)eventTag; }

            //convert input data into a signature
            var dataSignature = this.PersonId + this.StartTime.GetHashCode() + this.EndTime.GetHashCode() + this.DaysPerPixel + eventTagsHash;

            return dataSignature;
        }


        public static async Task<Chart> GetDataParsed(JObject? requestJson)
        {


            //get all the data needed out of the incoming request
            //var rootXml = await APITools.ExtractDataFromRequestXml(req);
            var personId = requestJson["PersonId"].Value<string>();
            var eventTagListJson = requestJson["EventTagList"];
            var eventTags = EventTagExtensions.FromJsonList(eventTagListJson);
            var startTimeJson = requestJson["StartTime"];
            var startTime = Time.FromJson(startTimeJson);
            var endTimeJson = requestJson["EndTime"];
            var endTime = Time.FromJson(endTimeJson);
            var daysPerPixel = requestJson["DaysPerPixel"].Value<double>();


            //a new chart is born
            var newChartId = Tools.GenerateId();
            var startTimeXml = startTime.ToXml();
            var endTimeXml = endTime.ToXml();
            var newChart = new Chart(newChartId, "", personId, startTimeXml, endTimeXml, daysPerPixel, eventTags);

            return newChart;
        }

    }
}
