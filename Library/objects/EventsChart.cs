using System;
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
    public class EventsChart
    {

        public static EventsChart Empty = new EventsChart("Empty", "Empty", "Empty", TimeRange.Empty,  0, new List<EventTag>());

        public string ChartId { get; set; }
        public string ContentSvg { get; set; }
        public string PersonId { get; }
        public double DaysPerPixel { get; }
        public List<EventTag> EventTagList { get; set; }
        public TimeRange TimeRange { get; set; }

        public EventsChart(string chartId, string contentSvg, string personId, TimeRange timeRange, double daysPerPixel, List<EventTag> eventTagList)
        {
            ChartId = chartId;
            ContentSvg = contentSvg;
            PersonId = personId;
            EventTagList = eventTagList;
            TimeRange = timeRange; 
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
                new XElement("StartTime", TimeRange.start.ToXml()),
                new XElement("EndTime", TimeRange.end.ToXml()),
                contentSvg);

            return chartXml;
        }

        /// <summary>
        /// Data coming in
        /// </summary>
        public static EventsChart FromXml(XElement personXml)
        {
            var personId = personXml.Element("PersonId")?.Value;
            var chartId = personXml.Element("ChartId")?.Value; //if no id tag generate new one
            var contentSvg = personXml.Element("ContentSvg")?.Value;
            var eventTagListXml = personXml.Element("EventTagList");
            var eventTagList = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = personXml.Element("StartTime").Element("Time");
            var endTimeXml = personXml.Element("EndTime").Element("Time");
            var daysPerPixel = double.Parse(personXml.Element("DaysPerPixel")?.Value);

            var timeRange = new TimeRange(Time.FromXml(startTimeXml), Time.FromXml(endTimeXml));
            var parsedPerson = new EventsChart(chartId, contentSvg, personId, timeRange, daysPerPixel, eventTagList);

            return parsedPerson;
        }


        /// <summary>
        /// Use CHART ID instead if possible
        /// Gets HASH of chart ID, reliable
        /// </summary>
        public override int GetHashCode() => Tools.GetStringHashCode(ChartId);

        /// <summary>
        /// Gets a nice identifiable name for this chart to show user 
        /// </summary>
        public object GetFormattedName(string personName)
        {
            var startYear = TimeRange.start.GetStdYear();
            var startMonth = TimeRange.start.GetStdMonth();
            var endYear = TimeRange.end.GetStdYear();
            var endMonth = TimeRange.end.GetStdMonth();
            return $"{personName} - {startMonth}/{startYear} to {endMonth}/{endYear}";
        }

        /// <summary>
        /// create a unique signature to identify all future calls that is exactly alike
        /// </summary>
        public string GetEventsChartSignature()
        {
            //convert events into 1 signature
            var eventTagsHash = 0;
            foreach (var eventTag in this.EventTagList) { eventTagsHash += (int)eventTag; }

            //convert input data into a signature
            //var dataSignature = this.PersonId + this.StartTime.GetHashCode() + this.EndTime.GetHashCode() + this.DaysPerPixel + eventTagsHash;

            //use ticks because can revert back from there
            var endTime = TimeRange.end.GetStdDateTimeOffset().Ticks;
            var startTime = TimeRange.start.GetStdDateTimeOffset().Ticks;
            var dataSignature = $"{this.PersonId}-{startTime}-{endTime}-{this.DaysPerPixel}-{eventTagsHash}";

            return dataSignature;
        }


        /// <summary>
        /// Creates empty from json SPEC ONLY
        /// </summary>
        public static async Task<EventsChart> FromJsonSpecOnly(JObject? requestJson)
        {

            //get all the data needed out of the incoming request
            //var rootXml = await APITools.ExtractDataFromRequestXml(req);
            var personId = requestJson["PersonId"]?.Value<string>() ?? "101";
            var eventTagListJson = requestJson["EventTagList"];
            var eventTags = EventTagExtensions.FromJsonList(eventTagListJson);
            var startTimeJson = requestJson["StartTime"];
            var startTime = Time.FromJson(startTimeJson);
            var endTimeJson = requestJson["EndTime"];
            var endTime = Time.FromJson(endTimeJson);
            var daysPerPixel = requestJson["DaysPerPixel"].Value<double>();


            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new EventsChart(newChartId, "", personId, new TimeRange(startTime, endTime), daysPerPixel, eventTags);

            return newChart;
        }


        /// <summary>
        /// Packages the data to send to API to generate the chart
        /// </summary>
        public static JObject GenerateChartSpecsJson(Person inputPerson, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth)
        {
            //auto calculate precision
            var daysPerPixelRaw = EventsChart.GetDayPerPixel(timeRange, maxWidth);
            //if not defined, use input
            double daysPerPixelInput = 30;
            daysPerPixelInput = daysPerPixelRaw != 0 ? daysPerPixelRaw : daysPerPixelInput;

            var returnPayload = new JObject();

            returnPayload["PersonId"] = inputPerson.Id;
            returnPayload["StartTime"] = timeRange.start.ToJson();
            returnPayload["EndTime"] = timeRange.end.ToJson();
            returnPayload["DaysPerPixel"] = daysPerPixelInput;
            returnPayload["EventTagList"] = EventTagExtensions.ToJsonList(inputedEventTags);

            return returnPayload;


        }

        /// <summary>
        /// calculates the precision of the events to fit inside 1000px width
        /// </summary>
        public static double GetDayPerPixel(TimeRange timeRange, int maxWidth)
        {

            var daysPerPixel = Math.Round(timeRange.daysBetween / maxWidth, 3); //small val = higher precision
            //var daysPerPixel = Math.Round(yearsBetween * 0.4, 3); //small val = higher precision
            //daysPerPixel = daysPerPixel < 1 ? 1 : daysPerPixel; // minimum 1 day per px

            return daysPerPixel;
        }

    }
}
