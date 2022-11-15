using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace API
{
    public static class EventsChartAPI
    {
        //▄▀█ █▀█ █   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
        //█▀█ █▀▀ █   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


        /// <summary>
        /// Generates a new SVG dasa report given a person id
        /// Exp call:
        /// <Root>
        //      <PersonId>374117709</PersonId>
        //      <StartTime>
        //          <Time>
        //              <StdTime>00:00 01/01/1994 +08:00</StdTime>
        //              <Location>
        //                  <Name>Teluk Intan</Name>
        //                  <Longitude>101.0206</Longitude>
        //                  <Latitude>4.0224</Latitude>
        //              </Location>
        //          </Time>
        //  </StartTime>
        //  <EndTime>
        //      <Time>
        //          <StdTime>11:59 31/12/2024 +08:00</StdTime>
        //          <Location>
        //              <Name>Teluk Intan</Name>
        //              <Longitude>101.0206</Longitude>
        //              <Latitude>4.0224</Latitude>
        //          </Location>
        //      </Time>
        //  </EndTime>
        //  <DaysPerPixel>11</DaysPerPixel>
        //</Root>
        /// </summary>
        [FunctionName("geteventschart")]
        public static async Task<IActionResult> GetEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


        }

        [FunctionName("geteventscharteasy")]
        public static async Task<IActionResult> GetEventsChartEasy(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //get dasa report for sending
                var chart = await GetEventReportSvgForIncomingRequestEasy(incomingRequest);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


        }


        /// <summary>
        /// Gets chart from saved list, needs to be given id to find
        /// </summary>
        [FunctionName("getsavedeventschart")]
        public static async Task<IActionResult> GetSavedEventsChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //send image back to caller
                //convert svg string to stream for sending
                var stream = GenerateStreamFromString(chart.ContentSvg);
                var x = StreamToByteArray(stream);
                return new FileContentResult(x, "image/svg+xml");

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }


        /// <summary>
        /// Special function to access Saved Chart directly without website
        /// </summary>
        [FunctionName("savedchart")]
        public static async Task<IActionResult> GetSavedEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "savedchart/{chartId}")]
            HttpRequestMessage incomingRequest, string chartId)
        {
            try
            {

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);
                var svgString = chart.ContentSvg;

                //get chart index.html and send that to caller
                //get data list from Static Website storage
                var htmlTemplate = await APITools.GetStringFile(APITools.UrlEventsChartViewerHtml);

                //insert person name into page, to show ready page faster
                var personName = (await APITools.GetPersonFromId(chart.PersonId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                htmlTemplate = htmlTemplate.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);

                //insert SVG into html place holder page
                htmlTemplate = htmlTemplate.Replace("<!--INSERT SVG-->", svgString);

                return new ContentResult { Content = htmlTemplate, ContentType = "text/html" };


            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }


        /// <summary>
        /// Special function to generate new Events Chart directly without website
        /// Does not do anything to generate the chart, that is done by geteventscharteasy
        /// </summary>
        [FunctionName("chart")]
        public static async Task<IActionResult> GetEventsChartDirect([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "chart/{personId}/{eventPreset}/{timePreset}")]
            HttpRequestMessage incomingRequest, string personId, string eventPreset, string timePreset)
        {
            try
            {
                //get chart index.html and send that to caller
                var eventsChartViewerHtml = await APITools.GetStringFile(APITools.UrlEventsChartViewerHtml);


                //insert person name into page, to show ready page faster
                var personName = (await APITools.GetPersonFromId(personId)).Name;
                var jsVariables = $@"window.PersonName = ""{personName}"";";
                jsVariables += $@"window.ChartType = ""{"Muhurtha"}"";";
                var finalHtml = eventsChartViewerHtml.Replace("/*INSERT-JS-VAR-HERE*/", jsVariables);


                return new ContentResult { Content = finalHtml, ContentType = "text/html" };

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }


        /// <summary>
        /// Given a chart id it will return the person id for that chart
        /// </summary>
        [FunctionName("getpersonidfromsavedchartid")]
        public static async Task<IActionResult> GetPersonIdFromSavedChartId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get id that will be used find the person
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;

                //get the saved chart record by id
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);
                var foundChartXml = await APITools.FindChartById(savedChartListXml, chartId);
                var chart = Chart.FromXml(foundChartXml);

                //extract out the person id from chart and send it to caller
                var personIdXml = new XElement("PersonId", chart.PersonId);

                return new OkObjectResult(personIdXml.ToString());

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }

        }


        /// <summary>
        /// Saves the chart in Azure Storage
        /// </summary>
        [FunctionName("savepersoneventsreport")]
        public static async Task<IActionResult> SavePersonEventsReport(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {
                //generate report
                var chart = await GetEventReportSvgForIncomingRequest(incomingRequest);

                //save chart into storage
                //note: do not wait to speed things up, beware! failure will go undetected on client
                APITools.AddXElementToXDocument(chart.ToXml(), APITools.SavedChartListFile, APITools.ApiDataStorageContainer);

                //let caller know all good
                return APITools.PassMessage();

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


        }

        /// <summary>
        /// make a name & id only xml list of saved charts
        /// note: done so to not download what is not needed
        ///
        /// client will use data from here to get the actual saved chart
        /// </summary>
        [FunctionName("getsavedchartnamelist")]
        public static async Task<IActionResult> GetSavedChartNameList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {
            try
            {

                //get ful saved chart list
                var savedChartXmlDoc = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);

                //extract out the name & id
                var rootXml = new XElement("Root");

                //make a name & id only xml list of saved charts
                //note: done so to not download what is not needed
                var savedChartXmlList = savedChartXmlDoc?.Root?.Elements().ToList() ?? new List<XElement>(); //empty list so no failure
                foreach (var chartXml in savedChartXmlList)
                {
                    var parsedChart = Chart.FromXml(chartXml);
                    var person = await APITools.GetPersonFromId(parsedChart.PersonId);
                    var chartNameXml = new XElement("ChartName",
                        new XElement("Name",
                            parsedChart.GetFormattedName(person.Name)),
                        new XElement("ChartId",
                            parsedChart.ChartId));

                    //add to final xml
                    rootXml.Add(chartNameXml);
                }


                return new OkObjectResult(rootXml.ToString());

            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }


        }


        [FunctionName("deletesavedchart")]
        public static async Task<IActionResult> DeleteSavedChart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage incomingRequest)
        {

            try
            {
                //get unedited hash & updated person details from incoming request
                var requestData = APITools.ExtractDataFromRequest(incomingRequest);
                var chartId = requestData.Value;


                //get the person record that needs to be deleted
                var savedChartListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);
                var chartToDelete = await APITools.FindChartById(savedChartListXml, chartId);

                //delete the chart record,
                chartToDelete.Remove();

                //upload modified list to storage
                var savedChartListClient = await APITools.GetFileFromContainer(APITools.SavedChartListFile, APITools.ApiDataStorageContainer);
                await APITools.OverwriteBlobData(savedChartListClient, savedChartListXml);

                return APITools.PassMessage();
            }
            catch (Exception e)
            {
                //log error
                await Log.Error(e, incomingRequest);

                //format error nicely to show user
                return APITools.FormatErrorReply(e);
            }
        }





        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█


        /// <summary>
        /// processes incoming xml request and outputs events svg report
        /// </summary>
        private static async Task<Chart> GetEventReportSvgForIncomingRequest(HttpRequestMessage req)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var eventTagListXml = rootXml.Element("EventTagList");
            var eventTags = EventTagExtensions.FromXmlList(eventTagListXml);
            var startTimeXml = rootXml.Element("StartTime")?.Elements().First();
            var startTime = Time.FromXml(startTimeXml);
            var endTimeXml = rootXml.Element("EndTime")?.Elements().First();
            var endTime = Time.FromXml(endTimeXml);
            var daysPerPixel = double.Parse(rootXml.Element("DaysPerPixel")?.Value ?? "0");
           

            //get the person instance by id
            var foundPerson = await APITools.GetPersonFromId(personId);

            //from person get svg report
            var eventsReportSvgString = await GenerateMainEventsReportSvg(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            return new Chart(newChartId, eventsReportSvgString, personId, eventTagListXml, startTimeXml, endTimeXml, daysPerPixel);
        }

        private static async Task<Chart> GetEventReportSvgForIncomingRequestEasy(HttpRequestMessage req)
        {
            //get all the data needed out of the incoming request
            var rootXml = APITools.ExtractDataFromRequest(req);
            var personId = rootXml.Element("PersonId")?.Value;
            var timePreset = rootXml.Element("TimePreset")?.Value;
            var eventPreset = rootXml.Element("EventPreset")?.Value;
            var maxWidth = int.Parse(rootXml.Element("MaxWidth")?.Value); //px
            //client's timezone, not based on birth
            var timezone = Tools.StringToTimezone(rootXml.Element("Timezone")?.Value);//should be "+08:00"

            //hard set maxwidth to 1000px so that no forever calculation created
            maxWidth = maxWidth > 1000 ? 1000 : maxWidth;

            //minus 7% for side padding
            maxWidth = maxWidth - (int)(maxWidth * 0.07); ;


            //EXTRACT & PREPARE DATA

            //get the person instance by id
            var foundPerson = await APITools.GetPersonFromId(personId);

            //TIME
            dynamic x = AutoCalculateTimeRange(timePreset, foundPerson.GetBirthLocation(), timezone);
            Time startTime = x.start;
            Time endTime = x.end;

            //EVENTS
            var eventTags = GetSelectedEventTypesEasy(eventPreset);

            //PRECISION
            //calculate based on max screen width,
            //done for fast calculation only for needed viewability
            var daysPerPixel = GetDayPerPixel(startTime, endTime, maxWidth);

            //from person get svg report
            var eventsReportSvgString = await GenerateMainEventsReportSvg(foundPerson, startTime, endTime, daysPerPixel, eventTags);

            //a new chart is born
            var newChartId = Tools.GenerateId();
            //todo needs update
            return new Chart(newChartId, eventsReportSvgString, personId, EventTagExtensions.ToXmlList(eventTags), startTime.ToXml(), endTime.ToXml(), daysPerPixel);
        }

        /// <summary>
        /// calculates the precision of the events to fit inside 1000px width
        /// </summary>
        public static double GetDayPerPixel(Time start, Time end, int maxWidth)
        {
            //const int maxWidth = 1000; //px

            var daysBetween = end.Subtract(start).TotalDays;
            var daysPerPixel = Math.Round(daysBetween / maxWidth, 3); //small val = higher precision
                                                                      //var daysPerPixel = Math.Round(yearsBetween * 0.4, 3); //small val = higher precision
                                                                      //daysPerPixel = daysPerPixel < 1 ? 1 : daysPerPixel; // minimum 1 day per px

            //if final value is 0 then recalculate without rounding
            daysPerPixel = daysPerPixel <= 0 ? daysBetween / maxWidth : daysPerPixel;

            return daysPerPixel;
        }

        public static List<EventTag> GetSelectedEventTypesEasy(string eventPreset)
        {
            var returnList = new List<EventTag>();

            switch (eventPreset)
            {
                //only general is customized
                case "General":
                    returnList.Add(EventTag.General);
                    returnList.Add(EventTag.Personal);
                    returnList.Add(EventTag.RulingConstellation);
                    break;
                case "Gochara":
                    returnList.Add(EventTag.Personal);
                    returnList.Add(EventTag.Gochara);
                    break;
                case "Travel":
                    returnList.Add(EventTag.Personal);
                    returnList.Add(EventTag.General);
                    returnList.Add(EventTag.Travel);
                    break;

                //others are converted as is
                default:
                    returnList.Add(Enum.Parse<EventTag>(eventPreset));
                    break;

            }

            return returnList;
        }

        /// <summary>
        /// Given a time preset in string like "Today"
        /// a precisise start and end time will be returned
        /// used in dasa/muhurtha easy calculator
        /// </summary>
        public static object AutoCalculateTimeRange(string timePreset, GeoLocation birthLocation, TimeSpan timezone)
        {

            Time start, end;
            //use the inputed user's timezone
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(timezone);
            var today = now.ToString("dd/MM/yyyy zzz");

            var yesterday = now.AddDays(-1).ToString("dd/MM/yyyy zzz");
            switch (timePreset)
            {

                case "Hour":
                    var startHour = now.AddHours(-1);//back 1 hour
                    var endHour = now.AddHours(1);//front 1 hour
                    start = new Time(startHour, birthLocation);
                    end = new Time(endHour, birthLocation);
                    return new { start = start, end = end };
                case "Today":
                case "Day":
                    start = new Time($"00:00 {today}", birthLocation);
                    end = new Time($"23:59 {today}", birthLocation);
                    return new { start = start, end = end };
                case "Week":
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    var weekEnd = now.AddDays(7).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {weekEnd}", birthLocation);
                    return new { start = start, end = end };
                case "Month":
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    var monthEnd = now.AddDays(30).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {monthEnd}", birthLocation);
                    return new { start = start, end = end };
                case "Year":
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    var yearEnd = now.AddDays(365).ToString("dd/MM/yyyy zzz");
                    end = new Time($"23:59 {yearEnd}", birthLocation);
                    return new { start = start, end = end };
                default:
                    return new { start = Time.Empty, end = Time.Empty };
            }
        }

        /// <summary>
        /// Massive method that generates dasa report in SVG
        /// </summary>
        private static async Task<string> GenerateMainEventsReportSvg(Person inputPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> inputedEventTags)
        {
            //1 CALCULATE DATA
            // One precision value for generating all dasa components,
            // because misalignment occurs if use different precision
            // note: precision = time slice count, each slice = 1 pixel (zoom level)
            double eventsPrecisionHours = Tools.DaysToHours(daysPerPixel);

            //generate time slice only once for all rows
            var timeSlices = EventManager.GetTimeListFromRange(startTime, endTime, eventsPrecisionHours);


            //2 MAKE EVENT ROWS
            var compiledRow = GenerateRowsSvg(inputPerson, daysPerPixel, startTime, endTime, timeSlices, eventsPrecisionHours, inputedEventTags, out int totalHeight);


            //3 MAKE NOW LINE
            //the height for all lines, cursor, now & life events line
            //place below the last row
            var padding = 2; //space between rows
            var lineHeight = totalHeight + padding;

            //get now line position
            var clientNowTime = startTime.StdTimeNowAtOffset;//now time at client
            var nowLinePosition = GetLinePosition(timeSlices, clientNowTime);
            var nowLineHeight = lineHeight + 6; //space between icon & last row
            compiledRow += GetNowLine(nowLineHeight, nowLinePosition);

            //5 WRAP IN GROUP
            //place all content except border & cursor time legend inside group for padding
            var contentPadding = 2;
            compiledRow = $"<g class=\"EventChartContent\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{compiledRow}</g>";



            //6 MAKE CURSOR LINE
            //add in the cursor line (moves with cursor via JS)
            //note: - template cursor line is duplicated to dynamically generate legend box
            //      - placed last so that show on top of others
            var svgTotalHeight = totalHeight + 35;//add space for life event icon
            compiledRow += GetTimeCursorLine(svgTotalHeight);

            //4 MAKE LIFE EVENTS
            //wait!, add in life events also
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            //var inputOffset = startTime.GetStdDateTimeOffset().Offset;
            var lifeEventHeight = lineHeight + 6; //space between icon & last row
            var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in
            compiledRow += await GetLifeEventLinesSvg(inputPerson, lifeEventHeight, inputOffset);


            //7 ADD BORDER
            //save a copy of the number of time slices used to calculate the svg total width later
            var dasaSvgWidth = timeSlices.Count;
            //add border around svg element
            //note:compensate for padding, makes border fit nicely around content
            var borderWidth = dasaSvgWidth + contentPadding;
            var roundedBorder = 3;
            compiledRow += $"<rect class=\"EventChartBorder\" rx=\"{roundedBorder}\" width=\"{borderWidth}\" height=\"{svgTotalHeight}\" style=\"stroke-width: 1; fill: none; paint-order: stroke; stroke:#333;\"></rect>";


            //8 DONE!
            //put all stuff in final SVG tag
            var finalSvg = WrapSvgElements(compiledRow, dasaSvgWidth, svgTotalHeight); //little wiggle room
            return finalSvg;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█


            //gets person's life events as lines for the dasa chart
            async Task<string> GetLifeEventLinesSvg(Person person, int lineHeight, TimeSpan inputOffset)
            {
                var compiledLines = "";

                foreach (var lifeEvent in person.LifeEventList)
                {
                    //this is placed here so that if 
                    try
                    {
                        //get timezone at place event happened
                        var lifeEvtTime = await lifeEvent.GetTime();//time at the place of event with correct standard timezone
                        var startTimeInputOffset = lifeEvtTime.ToOffset(inputOffset); //change to output offset, to match chart
                        var positionX = GetLinePosition(timeSlices, startTimeInputOffset);

                        //if line is not in report time range, don't generate it
                        if (positionX == 0) { continue; }

                        //put together icon + line + event data
                        compiledLines += GenerateLifeEventLine(lifeEvent, lineHeight, lifeEvtTime, positionX);
                    }
                    catch (Exception e)
                    {
                        //if fail log it and go on with next life event despite failure
                        await Log.Error(e, null);
                        continue;
                    }
                }


                //wrap in a group so that can be hidden/shown as needed
                //add transform matrix to adjust for border shift
                var wrapperGroup = $"<g id=\"LifeEventLinesHolder\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{compiledLines}</g>";

                return wrapperGroup;


            }
        }

        /// <summary>
        /// Get color based on nature
        /// </summary>
        public static string GetColor(EventNature? eventNature) => GetColor(eventNature.ToString());
        /// <summary>
        /// Get color based on nature
        /// </summary>
        public static string GetColor(string nature)
        {
            switch (nature)
            {
                case "Good": return "#00FF00"; //green
                case "Neutral": return "#D3D3D3"; //grey
                case "Bad": return "#FF0000"; //red
                default: throw new Exception($"Invalid Nature: {nature}");
            }
        }

        /// <summary>
        /// convert value coming in, to a percentage based on min & max
        /// this will make setting color based on value easier & accurate
        /// note, this will cause color to be relative to only what is visible in chart atm!
        /// </summary>
        public static string GetSummaryColor(double totalNature, double minValue, double maxValue)
        {
            string colorHex;
            if (totalNature >= 0) //good
            {
                //note: higher number is lighter color lower is darker
                var color255 = (int)totalNature.Remap(0, maxValue, 0, 255);
                if (color255 <= 10) { return "#FFFFFF"; } //if very close to 0, return greenish white color (lighter)
                if (color255 >= 220) { return "#00DD00"; } //if very close to 255, return darker green (DD) to highlight

                //this is done to invert the color,
                //so that higher score leads to darker green
                color255 = 255 - color255;

                colorHex = $"{color255:X}";//convert from 255 to FF
                var summaryColor = $"#{colorHex}FF{colorHex}";
                return summaryColor; //green
            }
            else //bad
            {
                //note: colors here are rendered opposite compared to good
                //because lower number here is worse, as such needs to be dark red.
                var color255 = (int)totalNature.Remap(minValue, 0, 0, 255);
                if (color255 <= 10) { return "#DD0000"; } //if very close to 255, return darker red (DD) to highlight
                if (color255 >= 220) { return "#FFFFFF"; } //if very close to 0, return white color
                colorHex = $"{color255:X}";//convert from 255 to FF
                var summaryColor = $"#FF{colorHex}{colorHex}";
                return summaryColor; //red
            }

        }

        /// <summary>
        /// Generates individual life event line svg
        /// </summary>
        public static string GenerateLifeEventLine(LifeEvent lifeEvent, int lineHeight, DateTimeOffset lifeEvtTime,
            int positionX)
        {

            //note: this is the icon below the life event line to magnify it
            var iconWidth = 12;
            var iconX = $"-{iconWidth}"; //use negative to move center under line
            var iconSvg = $@"
                                    <g transform=""matrix(2, 0, 0, 2, {iconX}, {lineHeight})"">
                                        <rect style=""fill:{GetColor(lifeEvent.Nature)}; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""{iconWidth}"" height=""9.95"" rx=""2.5"" ry=""2.5""/>
                                        <path d=""M 7.823 5.279 L 6.601 5.279 C 6.377 5.279 6.194 5.456 6.194 5.671 L 6.194 6.846 C 6.194 7.062 6.377 7.238 6.601 7.238 L 7.823 7.238 C 8.046 7.238 8.229 7.062 8.229 6.846 L 8.229 5.671 C 8.229 5.456 8.046 5.279 7.823 5.279 Z M 7.823 1.364 L 7.823 1.756 L 4.566 1.756 L 4.566 1.364 C 4.566 1.148 4.383 0.973 4.158 0.973 C 3.934 0.973 3.751 1.148 3.751 1.364 L 3.751 1.756 L 3.345 1.756 C 2.892 1.756 2.534 2.108 2.534 2.539 L 2.53 8.021 C 2.53 8.454 2.895 8.804 3.345 8.804 L 9.044 8.804 C 9.492 8.804 9.857 8.452 9.857 8.021 L 9.857 2.539 C 9.857 2.108 9.492 1.756 9.044 1.756 L 8.636 1.756 L 8.636 1.364 C 8.636 1.148 8.453 0.973 8.229 0.973 C 8.006 0.973 7.823 1.148 7.823 1.364 Z M 8.636 8.021 L 3.751 8.021 C 3.528 8.021 3.345 7.845 3.345 7.629 L 3.345 3.714 L 9.044 3.714 L 9.044 7.629 C 9.044 7.845 8.86 8.021 8.636 8.021 Z"" />
                                    </g>";

            //put together icon + line + event data
            var lifeEventLine = $"<g" +
                             $" eventName=\"{lifeEvent.Name}\" " +
                             $" class=\"LifeEventLines\" " + //atm used for tooltip logic
                             $" stdTime=\"{lifeEvtTime:dd/MM/yyyy}\" " + //show only date
                             $" transform=\"matrix(1, 0, 0, 1, {positionX}, 0)\"" +
                            $" x=\"0\" y=\"0\" >" +
                                $"<rect" +
                                $" width=\"2\"" +
                                $" height=\"{lineHeight}\"" +
                                $" style=\"" +
                                $" fill:{GetColor(lifeEvent.Nature)};" +
                                //border
                                $" stroke-width:0.5px;" +
                                $" stroke:#000;" +
                                $"\"" +
                                $" />"
                                 + iconSvg +
                            "</g>";

            return lifeEventLine;
        }

        /// <summary>
        /// add in the cursor line (moves with cursor via JS)
        /// note: template cursor line is duplicated to dynamically generate legend box
        /// </summary>
        public static string GetTimeCursorLine(int lineHeight)
        {
            return $@"
                        <g id=""CursorLine"" x=""0"" y=""0"">
                            <!--place where asset icons are stored-->
                            <g id=""CursorLineLegendIcons"" style=""display:none;"">
                                <circle id=""CursorLineBadIcon"" cx=""6.82"" cy=""7.573"" r=""4.907"" fill=""red""></circle>
                                <circle id=""CursorLineGoodIcon"" cx=""6.82"" cy=""7.573"" r=""4.907"" fill=""green""></circle>
                                <g id=""CursorLineClockIcon"" fill=""#fff"" transform=""matrix(0.5, 0, 0, 0.5, 2, 3)"" width=""12"" height=""12"">
				                    <path d=""M10 0a10 10 0 1 0 10 10A10 10 0 0 0 10 0zm2.5 14.5L9 11V4h2v6l3 3z""/>
			                    </g>
                                <path id=""CursorLineSumIcon"" transform=""matrix(0.045, 0, 0, 0.045, -14, -4)"" fill=""#fff"" d=""M437 122c-15 2-26 5-38 10-38 16-67 51-75 91-4 17-4 36 0 54 10 48 47 86 95 98 11 2 15 3 30 3s19-1 30-3c48-12 86-50 96-98 3-18 3-37 0-54-10-47-48-86-95-98-10-2-16-3-29-3h-14zm66 59c2 2 3 3 4 6s1 17 0 20c-2 7-11 9-15 2-1-2-1-3-1-7v-5h-37-37s8 11 18 23l21 25c1 2 1 5 1 7-1 1-10 13-21 26l-19 24c0 1 13 1 37 1h37v-5c0-6 1-9 5-11 5-2 11 1 11 8 1 1 1 6 1 10-1 7-1 8-2 10s-3 4-7 4h-52-50l-2-1c-4-3-5-7-3-11 0 0 11-14 24-29l22-28-22-28c-13-16-24-29-24-30-2-3-1-7 2-9 2-3 2-3 55-3h51l3 1z"" stroke=""none"" fill-rule=""nonzero""/>
                            </g>

                            <g id=""IBeam"">
			                    <rect width=""20"" height=""2"" style=""fill:black;"" x=""-9"" y=""0""></rect>
			                    <rect width=""2"" height=""{lineHeight}"" style=""fill:#000000;"" x=""0"" y=""0""></rect>
			                    <rect width=""20"" height=""2"" style=""fill:black;"" x=""-9"" y=""{lineHeight - 2}""></rect>
		                    </g>
		                    <g id=""CursorLineLegendTemplate"" transform=""matrix(1, 0, 0, 1, 10, 26)"" style=""display:none;"">
                                <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""160"" height=""15"" rx=""2"" ry=""2""></rect>
			                    <text style=""fill:#FFFFFF; font-size:11px; font-weight:400; white-space: pre;"" x=""14"" y=""11"">Template</text>
                                <!--icon set dynamic by JS-->
                                <use xlink:href=""""></use>                                
                            </g>
                            <!--place where dynamic event names are placed by JS-->
                            <g id=""CursorLineLegendHolder"" transform=""matrix(1, 0, 0, 1, 0, 4)""></g>
                            <g id=""CursorLineLegendDescriptionHolder"" transform=""matrix(1, 0, 0, 1, 0, 0)"" style=""display:none;"">
			                    <rect id=""CursorLineLegendDescriptionBackground"" style=""fill:#003e99;"" x=""170"" y=""11.244"" width=""150"" height=""0"" rx=""2"" ry=""2""></rect>
                                <g id=""CursorLineLegendDescription""></g>
		                    </g>
                       </g>
                        ";
        }
        public static string GetNowLine(int lineHeight, int nowLinePosition)
        {
            return $@"
                       <g id=""NowVerticalLine"" x=""0"" y=""0"" transform=""matrix(1, 0, 0, 1, {nowLinePosition}, 0)"">
		                    <rect width=""2"" height=""{lineHeight}"" style="" fill:blue; stroke-width:0.5px; stroke:#000;""></rect>
		                    <g transform=""matrix(2, 0, 0, 2, -12, {lineHeight})"">
			                    <rect style=""fill:blue; stroke:black; stroke-width: 0.5px;"" x=""0"" y=""0"" width=""12"" height=""9.95"" rx=""2.5"" ry=""2.5""></rect>
			                    <text style=""fill: rgb(255, 255, 255); font-size: 4.1px; white-space: pre;"" x=""1.135"" y=""6.367"">NOW</text>
		                    </g>
	                    </g>
                        ";
        }

        /// <summary>
        /// gets line position given a date
        /// finds most closest time slice, else return 0 means none found
        /// note:
        /// - make sure offset in time list and input time matches
        /// - tries to get nearest day first, then tries month to nearest year
        /// </summary>
        public static int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
        {
            //input timezone must be converted output timezone (timeSliceList)
            var outputTimezone = timeSliceList[0].GetStdDateTimeOffset().Offset;
            //translate event timezone to match chart timezone
            inputTime = inputTime.ToOffset(outputTimezone);

#if DEBUG
            if (outputTimezone.Ticks != inputTime.Offset.Ticks) { Console.WriteLine($"Life Event Offset Translated:{outputTimezone}->{inputTime.Offset}"); }
#endif

            var nowHour = inputTime.Hour;
            var nowDay = inputTime.Day;
            var nowYear = inputTime.Year;
            var nowMonth = inputTime.Month;

            //if nearest hour is possible then end here
            var nearestHour = GetNearestHour();
            if (nearestHour != 0) { return nearestHour; }

            //else try get nearest day
            var nearestDay = GetNearestDay();
            if (nearestDay != 0) { return nearestDay; }

            //else try get nearest month
            var nearestMonth = GetNearestMonth();
            if (nearestMonth != 0) { return nearestMonth; }

            //else try get nearest year
            var nearestYear = GetNearestYear();
            if (nearestYear != 0) { return nearestYear; }


            //if control reaches here then now time not found in time slices
            //this is possible when viewing old charts as such set now line to 0
            return 0;

            int GetNearestMonth()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameMonth && sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestDay()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameDay = time.GetStdDate() == nowDay;
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameMonth && sameYear && sameDay)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestHour()
            {
                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameHour = time.GetStdDateTimeOffset().Hour == nowHour;
                    var sameDay = time.GetStdDate() == nowDay;
                    var sameYear = time.GetStdYear() == nowYear;
                    var sameMonth = time.GetStdMonth() == nowMonth;
                    if (sameDay && sameHour && sameMonth && sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }

            int GetNearestYear()
            {

                //go through the list and find where the slice is closest to now
                var slicePosition = 0;
                foreach (var time in timeSliceList)
                {

                    //if same year and same month then send this slice position
                    //as the correct one
                    var sameYear = time.GetStdYear() == nowYear;
                    if (sameYear)
                    {
                        return slicePosition;
                    }

                    //move to next slice position
                    slicePosition++;
                }

                return 0;
            }
        }

        //wraps a list of svg elements inside 1 main svg element
        //if width not set defaults to 1000px, and height to 1000px
        //height is set auto because hard to determine
        public static string WrapSvgElements(string combinedSvgString, int svgWidth, int svgTotalHeight)
        {

            var svgBackgroundColor = "#f0f9ff";

            //create the final svg that will be displayed
            var svgTotalWidth = svgWidth + 10; //add little for wiggle room
            var svgBody = $"<svg id=\"EventChartHolder\"" +
                          //$" width=\"100%\"" +
                          //$" height=\"100%\"" +
                          $" style=\"" +
                          //note: if width & height not hard set, parent div clips it
                          $"width:{svgTotalWidth}px;" +
                          $"height:{svgTotalHeight}px;" +
                          $"background:{svgBackgroundColor};" +
                          $"\" " +//end of style tag
                          $"xmlns=\"http://www.w3.org/2000/svg\" " +
                          $"xmlns:xlink=\"http://www.w3.org/1999/xlink\">" + //much needed for use tags to work
                                                                             //$"<title>{chartTitle}</title>" + //title visible in browser when open direct
                          $"{combinedSvgString}</svg>";

            return svgBody;
        }

        /// <summary>
        /// Generates the event & header part of the dasa report
        /// </summary>
        private static string GenerateRowsSvg(Person inputPerson, double daysPerPixel, Time startTime,
            Time endTime, List<Time> timeSlices, double precisionInHours, List<EventTag> inputedEventTags, out int totalHeight)
        {

            //px width & height of each slice of time
            //used when generating dasa rows
            //note: changes needed only here
            const int widthPerSlice = 1;

            //STEP 1 : GENERATE TIME HEADER ROWS
            var compiledRow = GenerateTimeHeaderRow(timeSlices, daysPerPixel, widthPerSlice, out int headerY);


            //STEP 2 : GENERATE EVENT ROWS
            compiledRow += GenerateEventRows(
                precisionInHours,
                startTime,
                endTime,
                inputPerson,
                headerY,
                timeSlices,
                daysPerPixel,
                inputedEventTags,
                out totalHeight);

            return compiledRow;

        }

        private static string GenerateTimeHeaderRow(List<Time> timeSlices, double daysPerPixel, int _widthPerSlice, out int headerY)
        {
            var dasaSvgWidth = 0; //will be filled when calling row generator
            var compiledRow = "";

            var beginYear = timeSlices[0].GetStdYear();
            var endYear = timeSlices.Last().GetStdYear();
            var difYears = endYear - beginYear;

            //header rows are dynamically generated as needed, hence the extra logic below
            var headerGenerator = new List<Func<List<Time>, int, int, int, string>>();
            var showYearRow = daysPerPixel <= 15;
            if (difYears >= 10 && !showYearRow) { headerGenerator.Add(GenerateDecadeRowSvg); }
            if (difYears is >= 5 and < 10) { headerGenerator.Add(Generate5YearRowSvg); }
            if (showYearRow) { headerGenerator.Add(GenerateYearRowSvg); }
            if (daysPerPixel <= 1.3) { headerGenerator.Add(GenerateMonthRowSvg); }
            if (daysPerPixel <= 0.07) { headerGenerator.Add(GenerateDateRowSvg); }
            if (daysPerPixel <= 0.001) { headerGenerator.Add(GenerateHourRowSvg); }

            var padding = 2;//space between rows
            headerY = 0;
            int headerHeight = 11;
            foreach (var generator in headerGenerator)
            {
                compiledRow += generator(timeSlices, headerY, 0, headerHeight);

                //update for next generator
                headerY = headerY + headerHeight + padding;
            }

            return compiledRow;


            string GenerateYearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousYear == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                                $"<rect " +
                                                    $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                                    $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                                        $" style=\"fill: rgb(255, 255, 255);" +
                                                        $" font-size: 10px;" +
                                                        $" font-weight: 700;" +
                                                        $" text-anchor: middle;" +
                                                        $" white-space: pre;\"" +
                                                        //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                                        $">" +
                                                        $"{previousYear}" + //previous year generate at begin of new year
                                                    $"</text>" +
                                             $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateDecadeRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                const int decade = 10;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + decade; //decade


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year & last month (month for accuracy, otherwise border at jan not december)
                        //todo begging of box is not beginning of year, possible solution month
                        //var isLastMonth = slice.GetStdMonth() is 10 or 11 or 12; //use oct & nov in-case december is not generated at low precision 
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + decade;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string Generate5YearRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousYear = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                const int yearRange = 5;

                var beginYear = timeSlices[0].GetStdYear();
                var endYear = beginYear + yearRange;


                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var yearChanged = previousYear != slice.GetStdYear();
                    if (yearChanged || lastTimeSlice)
                    {
                        //is this slice end year
                        var isEndYear = endYear == slice.GetStdYear();
                        if (isEndYear)
                        {
                            //generate previous year data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{beginYear} - {endYear}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                            //set new begin & end
                            beginYear = endYear + 1;
                            endYear = beginYear + yearRange;

                        }

                    }

                    //update previous year for next slice
                    previousYear = slice.GetStdYear();

                    yearBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;
            }

            string GenerateMonthRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousMonth = 0; //start 0 for first draw
                var yearBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new year box when year changes or at
                    //end of time slices to draw the last year box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var monthChanged = previousMonth != slice.GetStdMonth();
                    if (monthChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousMonth == 0)
                        {
                            yearBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous month data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = yearBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{GetMonthName(previousMonth)}" + //previous year generate at begin of new year
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            yearBoxWidthCount = 0;

                        }
                    }
                    //year same as before
                    else
                    {
                        //update width only, position is same
                        //as when created the year box
                        //yearBoxWidthCount *= _widthPerSlice;

                    }

                    //update previous month for next slice
                    previousMonth = slice.GetStdMonth();

                    yearBoxWidthCount++;


                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

                string GetMonthName(int monthNum)
                {
                    switch (monthNum)
                    {
                        case 1: return "JAN";
                        case 2: return "FEB";
                        case 3: return "MAR";
                        case 4: return "APR";
                        case 5: return "MAY";
                        case 6: return "JUN";
                        case 7: return "JUL";
                        case 8: return "AUG";
                        case 9: return "SEP";
                        case 10: return "OCT";
                        case 11: return "NOV";
                        case 12: return "DEC";
                        default: throw new Exception($"Invalid Month: {monthNum}");
                    }
                }
            }

            string GenerateDateRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousDate = 0; //start 0 for first draw
                var dateBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new date box when date changes or at
                    //end of time slices to draw the last date box
                    var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var dateChanged = previousDate != slice.GetStdDate();
                    if (dateChanged || lastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousDate == 0)
                        {
                            dateBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous date data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = dateBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{previousDate}" + //previous date generate at begin of new date
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            dateBoxWidthCount = 0;

                        }
                    }

                    //update previous date for next slice
                    previousDate = slice.GetStdDate();

                    dateBoxWidthCount++;

                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

            }

            string GenerateHourRowSvg(List<Time> timeSlices, int yAxis, int xAxis, int rowHeight)
            {

                //generate the row for each time slice
                var rowHtml = "";
                var previousHour = -1; //so that hour 0 is counted
                var hourBoxWidthCount = 0;
                int rectWidth = 0;
                int childAxisX = 0;
                //int rowHeight = 11;

                foreach (var slice in timeSlices)
                {

                    //only generate new date box when hour changes or at
                    //end of time slices to draw the last hour box
                    var isLastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
                    var hourChanged = previousHour != slice.GetStdHour();
                    if (hourChanged || isLastTimeSlice)
                    {
                        //and it is in the beginning
                        if (previousHour == -1)
                        {
                            hourBoxWidthCount = 0; //reset width
                        }
                        else
                        {
                            //generate previous hour data first before resetting
                            childAxisX += rectWidth; //use previous rect width to position this
                            rectWidth = hourBoxWidthCount * _widthPerSlice; //calculate new rect width
                            var textX = rectWidth / 2; //center of box divide 2
                            var rect = $"<g transform=\"matrix(1, 0, 0, 1, {childAxisX}, 0)\">" + //y is 0 because already set in parent group
                                       $"<rect " +
                                       $"fill=\"#0d6efd\" x=\"0\" y=\"0\" width=\"{rectWidth}\" height=\"{rowHeight}\" " + $" style=\"paint-order: stroke; stroke: rgb(255, 255, 255); stroke-opacity: 1; stroke-linejoin: round;\"/>" +
                                       $"<text x=\"{textX}\" y=\"{9}\" width=\"{rectWidth}\" fill=\"white\"" +
                                       $" style=\"fill: rgb(255, 255, 255);" +
                                       $" font-size: 10px;" +
                                       $" font-weight: 700;" +
                                       $" text-anchor: middle;" +
                                       $" white-space: pre;\"" +
                                       //$" transform=\"matrix(0.966483, 0, 0, 0.879956, 2, -6.779947)\"" +
                                       $">" +
                                       $"{previousHour}" + //previous hour generate at begin of new hour
                                       $"</text>" +
                                       $"</g>";


                            //add to final return
                            rowHtml += rect;

                            //reset width
                            hourBoxWidthCount = 0;

                        }
                    }

                    //update previous hour for next slice
                    previousHour = slice.GetStdHour();

                    hourBoxWidthCount++;
                }

                //wrap all the rects inside a svg so they can me moved together
                //svg tag here acts as group, svg nesting
                rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

                return rowHtml;

            }

        }

        /// <summary>
        /// Generate rows based of inputed events
        /// </summary>
        private static string GenerateEventRows(double eventsPrecision, Time startTime, Time endTime,
            Person inputPerson, int headerY, List<Time> timeSlices, double daysPerPixel, List<EventTag> inputedEventTags, out int totalHeight)
        {
            //1 GENERATE DATA FOR EVENT ROWS
            const int widthPerSlice = 1;
            const int singleRowHeight = 15;
            var eventDataList = APITools.GetEventDataList().Result;

            //rows are dynamically generated as needed, hence the extra logic below
            //list of rows to generate
            var unsortedEventList = new List<Event>();

            //calculate events for each tag
            foreach (var eventTag in inputedEventTags)
            {
                var tempEventList = APITools.CalculateEvents(eventsPrecision, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, eventTag, eventDataList);
                unsortedEventList.AddRange(tempEventList);
            }

            //sort event by duration, so that events are ordered nicely in chart
            //todo events are breaking up between rows
            var eventList = unsortedEventList.OrderByDescending(x => x.Duration).ToList();


            //2 STACK & GENERATED ROWS FROM ABOVE DATA
            var padding = 1;//space between rows
            var compiledRow = "";

            //note: summary data is filled when generating rows
            var summaryRowData = new Dictionary<int, double>();//x axis & total nature score
            int yAxis = headerY;
            //generate svg for each row & add to final row
            compiledRow += GenerateMultipleRowSvg(eventList, timeSlices, yAxis, 0, out int finalHeight);
            //set y axis (horizontal) for next row
            yAxis = yAxis + finalHeight + padding;

            //4 GENERATE SUMMARY ROW
            //min & max used to calculate color later
            var maxValue = summaryRowData.Values.Max();
            var minValue = summaryRowData.Values.Min();
            compiledRow += GenerateSummaryRow(yAxis);
            yAxis += 15;//add in height of summary row

            //3 LET CALLER KNOW FINAL HEIGHT
            totalHeight = yAxis;

            return compiledRow;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

            string GenerateSummaryRow(int yAxis)
            {
                Console.WriteLine("MAX" + maxValue);
                Console.WriteLine("MIN" + minValue);

                var rowHtml = "";
                //generate color summary
                var colorRow = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    //total nature score is sum of negative & positive 1s of all events
                    //that occurred at this point in time, possible negative number
                    //exp: -4 bad + 5 good = 1 total nature score
                    double totalNatureScore = summarySlice.Value;

                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{singleRowHeight}\" " +
                               $"fill=\"{GetSummaryColor(totalNatureScore, minValue, maxValue)}\" />";

                    //add rect to row
                    colorRow += rect;
                }

                rowHtml += $"<g id=\"ColorRow\">{colorRow}</g>";


                //generate graph summary
                var barChartRow = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    double totalNatureScore = summarySlice.Value; //possible negative
                    var barHeight = (int)totalNatureScore.Remap(minValue, maxValue, 0, 30);
                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{barHeight}\" " +
                               $"fill=\"black\" />";

                    //add rect to row
                    barChartRow += rect;
                }

                //note: chart is flipped 180, to start bar from bottom to top
                //default hidden
                rowHtml += $"<g id=\"BarChartRow\" style=\"display:none;\">{barChartRow}</g>";

                //add in "Summary" label above row
                float aboveRow = yAxis - singleRowHeight - padding;
                rowHtml += $@"
                    <g id=""SummaryLabel"" transform=""matrix(1, 0, 0, 1, 0, {aboveRow})"">
				        <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""68"" height=""15"" rx=""2"" ry=""2""/>
				        <text style=""fill:#FFFFFF; font-size:11px; font-weight:400;"" x=""16"" y=""11"">Summary</text>
				        <path transform=""matrix(0.045, 0, 0, 0.045, -14, -4)"" fill=""#fff"" d=""M437 122c-15 2-26 5-38 10-38 16-67 51-75 91-4 17-4 36 0 54 10 48 47 86 95 98 11 2 15 3 30 3s19-1 30-3c48-12 86-50 96-98 3-18 3-37 0-54-10-47-48-86-95-98-10-2-16-3-29-3h-14zm66 59c2 2 3 3 4 6s1 17 0 20c-2 7-11 9-15 2-1-2-1-3-1-7v-5h-37-37s8 11 18 23l21 25c1 2 1 5 1 7-1 1-10 13-21 26l-19 24c0 1 13 1 37 1h37v-5c0-6 1-9 5-11 5-2 11 1 11 8 1 1 1 6 1 10-1 7-1 8-2 10s-3 4-7 4h-52-50l-2-1c-4-3-5-7-3-11 0 0 11-14 24-29l22-28-22-28c-13-16-24-29-24-30-2-3-1-7 2-9 2-3 2-3 55-3h51l3 1z"" stroke=""none"" fill-rule=""nonzero""/>
			        </g>";

                //return compiled rects as 1 row in a group for easy debugging & edits
                rowHtml = $"<g id=\"SummaryRow\">{rowHtml}</g>";
                return rowHtml;
            }



            //height not known until generated
            //returns the final dynamic height of this event row
            string GenerateMultipleRowSvg(List<Event> eventList, List<Time> timeSlices, int yAxis, int xAxis, out int finalHeight)
            {
                //generate the row for each time slice
                var rowHtml = "";
                var horizontalPosition = 0; //distance from left
                var verticalPosition = 0; //distance from top

                //height of each row
                var spaceBetweenRow = 1;

                //used to determine final height
                var highestTimeSlice = 0;
                var multipleEventCount = 0;

                //start as empty event
                var prevEventList = new Dictionary<int, EventName>();

                //generate 1px (rect) per time slice (horizontal)
                foreach (var slice in timeSlices)
                {
                    //get events that occurred at this time slice
                    var foundEventList = eventList.FindAll(tempEvent => tempEvent.IsOccurredAtTime(slice));

                    //generate rect for each event & stack from top to bottom (vertical)
                    foreach (var foundEvent in foundEventList)
                    {
                        //if current event is different than event has changed, so draw a black line
                        int finalYAxis = yAxis + verticalPosition;
                        var prevExist = prevEventList.TryGetValue(finalYAxis, out var prevEventName);
                        var isNewEvent = prevExist && (prevEventName != foundEvent.Name);
                        var color = isNewEvent ? "black" : GetColor(foundEvent?.Nature);

                        //save current event to previous, to draw border later
                        //border ONLY for top 3 rows (long duration events), lower row borders block color
                        if (finalYAxis <= 29)
                        {
                            prevEventList[finalYAxis] = foundEvent.Name;
                        }

                        //var color = GetColor(foundEvent?.Nature);

                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset().ToString(Time.DateTimeFormat)}\" " +
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{finalYAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                                   $"width=\"{widthPerSlice}\" " +
                                   $"height=\"{singleRowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        //add rect to return list
                        rowHtml += rect;

                        //every time a rect is added, we keep track of it in a list to generate the summary row at last
                        //based on event nature minus or plus 1
                        double natureScore = 0;
                        switch (foundEvent?.Nature)
                        {
                            case EventNature.Good:
                                natureScore = 1;
                                break;
                            case EventNature.Bad:
                                natureScore = -1;
                                break;
                        }

                        //compile nature score for making summary row later (defaults to 0)
                        var previousNatureScoreSum = (summaryRowData.ContainsKey(horizontalPosition) ? summaryRowData[horizontalPosition] : 0);
                        summaryRowData[horizontalPosition] = natureScore + previousNatureScoreSum; //combine current with previous


                        //increment vertical position for next
                        //element to be placed beneath this one
                        verticalPosition += singleRowHeight + spaceBetweenRow;

                        multipleEventCount++; //include this in count
                    }

                    //set position for next element in time slice
                    horizontalPosition += widthPerSlice;

                    //reset vertical position for next time slice
                    verticalPosition = 0;

                    //safe only the highest row (last row in to be added) used for calculating final height
                    var multipleRowH = (multipleEventCount * (singleRowHeight + spaceBetweenRow)) - spaceBetweenRow; //minus 1 to compensate for last row
                    var thisSliceHeight = multipleEventCount > 1 ? multipleRowH : singleRowHeight; //different height calc for multiple & single row
                    highestTimeSlice = thisSliceHeight > highestTimeSlice ? thisSliceHeight : highestTimeSlice;
                    multipleEventCount = 0; //reset

                }

                //wrap all the rects inside a svg so they can be moved together
                //note: use group instead of svg because editing capabilities
                rowHtml = $"<g class=\"EventListHolder\" transform=\"matrix(1, 0, 0, 1, {xAxis}, 0)\">{rowHtml}</g>";

                //send height of tallest time slice aka the
                //final height of this gochara row to caller
                finalHeight = highestTimeSlice;

                return rowHtml;
            }

        }


        private static byte[] StreamToByteArray(Stream input)
        {
            //reset stream position
            input.Position = 0;
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }


}
