using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents an events report chart
    /// Note: made so that a chart can be saved and accessed later
    /// </summary>
    public class EventsChart
    {

        public static EventsChart Empty = new EventsChart("Empty", "Empty", "Empty", TimeRange.Empty, 0, new List<EventTag>(), ChartOptions.Empty);

        public string ChartId { get; set; }
        public string ContentSvg { get; set; }
        public string PersonId { get; }
        public double DaysPerPixel { get; }
        public List<EventTag> EventTagList { get; set; }
        public TimeRange TimeRange { get; set; }

        public ChartOptions Options { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public EventsChart(string chartId, string contentSvg, string personId, TimeRange timeRange, double daysPerPixel, List<EventTag> eventTagList, ChartOptions options)
        {
            ChartId = chartId;
            ContentSvg = contentSvg;
            PersonId = personId;
            EventTagList = eventTagList;
            TimeRange = timeRange;
            DaysPerPixel = daysPerPixel;
            Options = options;
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
            var optionsXml = personXml.Element("StartTime");
            var options = ChartOptions.FromXml(optionsXml);

            var timeRange = new TimeRange(Time.FromXml(startTimeXml), Time.FromXml(endTimeXml));
            var parsedPerson = new EventsChart(chartId, contentSvg, personId, timeRange, daysPerPixel, eventTagList, options);

            return parsedPerson;
        }

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

            //use ticks because can revert back from there
            var endTime = TimeRange.end.GetStdDateTimeOffset().Ticks;
            var startTime = TimeRange.start.GetStdDateTimeOffset().Ticks;
            var dataSignature = $"{this.PersonId}-{startTime}-{endTime}-{this.DaysPerPixel}-{eventTagsHash}";

            return dataSignature;
        }

        /// <summary>
        /// Creates empty from json SPEC ONLY
        /// </summary>
        public static EventsChart FromJson(JObject? requestJson)
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
            var summaryOptionsJson = requestJson["ChartOptions"];
            var summaryOptions = ChartOptions.FromJson(summaryOptionsJson);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new EventsChart(newChartId, "", personId, new TimeRange(startTime, endTime), daysPerPixel, eventTags, summaryOptions);

            return newChart;
        }

        /// <summary>
        /// From user inputed data make specs for event 
        /// </summary>
        public static EventsChart FromData(Person inputPerson, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth, ChartOptions options)
        {

            //auto calculate precision
            var daysPerPixelRaw = EventsChart.GetDayPerPixel(timeRange, maxWidth);
            //if not defined, use input
            double daysPerPixelInput = 30;
            daysPerPixelInput = daysPerPixelRaw != 0 ? daysPerPixelRaw : daysPerPixelInput;

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new EventsChart(newChartId, "", inputPerson.Id, timeRange, daysPerPixelInput, inputedEventTags, options);

            return newChart;
        }

        /// <summary>
        /// Convert settings data from URL to instance only,
        /// used by API, only settings no chart here
        /// .../Viknesh1994   : 0
        /// /StartTime/00:00/01/01/2011/+08:00  : 4 to 8
        /// /EndTime/00:00/31/12/2024/+08:00    : 10 to 14
        /// /DaysPerPixel/5.439                 : 16
        /// /EventTagList/PD1,PD2,PD3,PD4,PD5,AshtakvargaGochara,Gochara :18
        /// /SelectedAlgorithm/GetGeneralScore,GocharaAshtakvargaBindu  :20   
        /// /Location/Teluk Intan               :22
        /// /Longitude/101.0206                 :24
        /// /Latitude/4.0224                    :26
        /// </summary>
        public static EventsChart FromUrl(string url)
        {
            //split URL into data pieces
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //take out the needed data
            var personId = parts[2];
            var location = new GeoLocation(parts[22], double.Parse(parts[24]), double.Parse(parts[26]));

            //combine time parts to be parsed easy
            var startTimeText = new ArraySegment<string>(parts, 4, 5);
            var startTime = Time.FromUrl(string.Join("/", startTimeText), location);

            var endTimeText = new ArraySegment<string>(parts, 10, 5);
            var endTime = Time.FromUrl(string.Join("/", endTimeText), location);

            var eventTags = EventTagExtensions.FromString(parts[18]);
            var daysPerPixel = double.Parse(parts[16]);
            var summaryOptions = ChartOptions.FromUrl(parts[20]);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new EventsChart(newChartId, "", personId, new TimeRange(startTime, endTime), daysPerPixel, eventTags, summaryOptions);

            return newChart;
        }

        /// <summary>
        /// Converts an instance to URL format for easy transport to API server, only specs here
        /// EXP : 
        /// </summary>
        public string ToUrl()
        {
            var final = "";

            final += $"/Settings/PersonId/{this.PersonId}";
            final += $"/StartTime/{this.TimeRange.start.ToUrl()}"; // 00:00/01/01/2011/+08:00
            final += $"/EndTime/{this.TimeRange.end.ToUrl()}";
            final += $"/DaysPerPixel/{this.DaysPerPixel}";
            final += $"/EventTagList/{string.Join(",", this.EventTagList)}"; // PD1,PD2,PD3,PD4,PD5
            final += $"/SelectedAlgorithm/{string.Join(",", this.Options.SelectedAlgorithm.Select(func => func.Method.Name))}"; // GetGeneralScore,GocharaAshtakvargaBindu
            var geoLocation = this.TimeRange.start.GetGeoLocation(); //use start location for both
            final += $"/Location/{geoLocation.Name()}";
            final += $"/Longitude/{geoLocation.Longitude()}";
            final += $"/Latitude/{geoLocation.Latitude()}";

            return final;
        }

        /// <summary>
        /// Packages the data to send to API to generate the chart
        /// </summary>
        public JObject ToJson()
        {
            var returnPayload = new JObject();

            returnPayload["PersonId"] = this.PersonId;
            returnPayload["StartTime"] = this.TimeRange.start.ToJson();
            returnPayload["EndTime"] = this.TimeRange.end.ToJson();
            returnPayload["DaysPerPixel"] = this.DaysPerPixel;
            returnPayload["EventTagList"] = EventTagExtensions.ToJsonList(this.EventTagList);
            returnPayload["ChartOptions"] = this.Options.ToJson();

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

        /// <summary>
        /// Use CHART ID instead if possible
        /// Gets HASH of chart ID, reliable
        /// </summary>
        public override int GetHashCode() => Tools.GetStringHashCode(ChartId);

    }
}
