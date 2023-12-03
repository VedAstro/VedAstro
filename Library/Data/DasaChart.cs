using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Represents an events report chart
    /// Note: made so that a chart can be saved and accessed later
    /// </summary>
    public class DasaChart
    {

        public static DasaChart Empty = new DasaChart("Empty", "Empty", Person.Empty, TimeRange.Empty, 0, new List<EventTag>(), ChartOptions.Empty);

        public string ChartId { get; set; }
        public string ContentSvg { get; set; }
        public Person Person { get; }
        public double DaysPerPixel { get; }
        public List<EventTag> EventTagList { get; set; }
        public TimeRange TimeRange { get; set; }

        public ChartOptions Options { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public DasaChart(string chartId, string contentSvg, Person person, TimeRange timeRange, double daysPerPixel, List<EventTag> eventTagList, ChartOptions options)
        {
            ChartId = chartId;
            ContentSvg = contentSvg;
            Person = person;
            EventTagList = eventTagList;
            TimeRange = timeRange;
            DaysPerPixel = daysPerPixel;
            Options = options;
        }



        /// <summary>
        /// Gets a nice identifiable name for this chart to show user 
        /// </summary>
        public object GetFormattedName(string personName)
        {
            var startYear = TimeRange.start.StdYear();
            var startMonth = TimeRange.start.StdMonth();
            var endYear = TimeRange.end.StdYear();
            var endMonth = TimeRange.end.StdMonth();
            return $"{personName} - {startMonth}/{startYear} to {endMonth}/{endYear}";
        }

        /// <summary>
        /// create a unique signature to identify all future calls that is exactly alike
        /// name of data, designed for human eyes
        /// </summary>
        public string GetDasaChartSignature()
        {
            //use ticks because can revert back from there
            var endTime = TimeRange.end.StdDateMonthYearText;
            var startTime = TimeRange.start.StdDateMonthYearText;

            //include all data into the name of the cache,
            //so easy to ID en mass and genocide them when needed
            var dataSignature = $"{nameof(DasaChart)}-" +
                                $"{this.Person.Id}-" +
                                $"{startTime}-{endTime}-" +
                                $"{this.DaysPerPixel}-" +
                                $"{(Ayanamsa)Calculate.Ayanamsa}-" +
                                $"{Tools.ListToString(EventTagList, "-")}-" +
                                $"{Options}";

            //needed else cache can't be created 
            var cleaned = new string(dataSignature
                .Where(ch => !Path.GetInvalidFileNameChars().Contains(ch))
                .ToArray());

            return cleaned;
        }

        /// <summary>
        /// Creates empty from json SPEC ONLY
        /// </summary>
        public static async Task<DasaChart> FromJson(JObject? requestJson)
        {
            //get all the data needed out of the incoming request
            var personId = requestJson["PersonId"]?.Value<string>() ?? "101";
            var foundPerson = Tools.GetPersonById(personId);
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
            var newChart = new DasaChart(newChartId, "", foundPerson, new TimeRange(startTime, endTime), daysPerPixel, eventTags, summaryOptions);

            return newChart;
        }

        /// <summary>
        /// From user inputed data make specs for event 
        /// </summary>
        public static DasaChart FromData(Person inputPerson, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth, ChartOptions options)
        {

            //auto calculate precision
            var daysPerPixelRaw = DasaChart.GetDayPerPixel(timeRange, maxWidth);
            //if not defined, use input
            double daysPerPixelInput = 30;
            daysPerPixelInput = daysPerPixelRaw != 0 ? daysPerPixelRaw : daysPerPixelInput;

            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new DasaChart(newChartId, "", inputPerson, timeRange, daysPerPixelInput, inputedEventTags, options);

            return newChart;
        }

        /// <summary>
        /// Convert settings data from URL to instance only,
        /// used by API, only settings no chart here
        /// .../Viknesh1994                     : 0
        /// /Start/00:00/01/01/2011             : 2,3,4,5
        /// /End/00:00/31/12/2024/+08:00        : 7,8,9,10,11
        /// /5.439                              : 12 DaysPerPixel
        /// /PD1,PD2,PD3,PD4,PD5,AshtakvargaGochara,Gochara :13 EventTagList
        /// /GetGeneralScore,GocharaAshtakvargaBindu        :14 SelectedAlgorithm
        ///
        /// Note: format was heavily modified to squeeze as much data into 260 char URL server limit
        /// </summary>
        public static async Task<DasaChart> FromUrl(string url)
        {
            //split URL into data pieces
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            //take out the needed data
            var personId = parts[0];

            //use birth location for chart time (needs testing)
            var person = Tools.GetPersonById(personId);
            var location = person.GetBirthLocation();

            //combine time parts to be parsed easy
            var start = new { hhmmStr = parts[2], dateStr = parts[3], monthStr = parts[4], yearStr = parts[5], offsetStr = parts[11] };
            var startStr = $"{start.hhmmStr} {start.dateStr}/{start.monthStr}/{start.yearStr} {start.offsetStr}";
            var startTime = new Time(startStr, location);

            var end = new { hhmmStr = parts[7], dateStr = parts[8], monthStr = parts[9], yearStr = parts[10], offsetStr = parts[11] };
            var endStr = $"{end.hhmmStr} {end.dateStr}/{end.monthStr}/{end.yearStr} {end.offsetStr}";
            var endTime = new Time(endStr, location);

            var daysPerPixel = double.Parse(parts[12]);
            var eventTags = EventTagExtensions.FromString(parts[13]);
            var summaryOptions = ChartOptions.FromUrl(parts[14]);

            //if custom ayanamsa specified
            var isCustomAya = url.Contains(nameof(Ayanamsa)); //check if ayanamsa specified anywhere
            if (isCustomAya)
            {
                var ayaRaw = $"{parts[^2]}/{parts[^1]}"; //get last since that will be ayanamsa in text or number int
                var enumFromUrl = Tools.EnumFromUrl(ayaRaw);
                Calculate.Ayanamsa = (int)enumFromUrl;
            }


            //a new chart is born
            var newChartId = Tools.GenerateId();
            var newChart = new DasaChart(newChartId, "", person, new TimeRange(startTime, endTime), daysPerPixel, eventTags, summaryOptions);

            return newChart;
        }

        /// <summary>
        /// Converts an instance to URL format for easy transport to API server, only specs here
        /// EXP : 
        /// </summary>
        public string ToUrl()
        {
            var final = "";

            final += $"/{this.Person.Id}";
            var start = this.TimeRange.start;
            final += $"/Start/{start.StdHourMinuteText}/{start.StdDateMonthYearText}"; // 00:00/01/01/2011
            var end = this.TimeRange.end;
            final += $"/End/{end.StdHourMinuteText}/{end.StdDateMonthYearText}/{end.StdTimezoneText}"; // 00:00/01/01/2011/+08:00
            final += $"/{this.DaysPerPixel}";
            final += $"/{string.Join(",", this.EventTagList)}"; // PD1,PD2,PD3,PD4,PD5
            final += $"/{string.Join(",", this.Options.SelectedAlgorithm.Select(func => func.Method.Name))}"; // GetGeneralScore,GocharaAshtakvargaBindu

            return final;
        }

        /// <summary>
        /// Packages the data to send to API to generate the chart
        /// </summary>
        public JObject ToJson()
        {
            var returnPayload = new JObject();

            returnPayload["PersonId"] = this.Person.Id;
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
