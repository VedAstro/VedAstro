using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;


namespace VedAstro.Library
{

    /// <summary>
    /// Central place to for all things events
    /// </summary>
    public static class EventsChartManager
    {

        //px width & height of each slice of time
        //used when generating dasa rows
        //note: changes needed only here
        private const int widthPerSlice = 1;

        private const int maxChartHeightPx = 1000;

        /// <summary>
        /// place to keep track where components are placed
        /// </summary>
        private static bool[] verticalYAxisManifest = new bool[maxChartHeightPx];

        /// <summary>
        /// Filled when first events come out fresh from oven unsorted
        /// declare static, stops GC
        /// </summary>
        public static List<Event> UnsortedEventList { get; set; }

        //index is Y axis px (vertical position)
        //private static string[] SvgConmponentList = new string[];


        //▒█▀▀█ ▒█░▒█ ▒█▀▀█ ▒█░░░ ▀█▀ ▒█▀▀█ 
        //▒█▄▄█ ▒█░▒█ ▒█▀▀▄ ▒█░░░ ▒█░ ▒█░░░ 
        //▒█░░░ ░▀▄▄▀ ▒█▄▄█ ▒█▄▄█ ▄█▄ ▒█▄▄█

        public static async Task<string> GenerateEventsChart(Person inputPerson, TimeRange timeRange, double daysPerPixel, List<EventTag> inputedEventTags)
        {

            //ACT I : declare the components
            string svgHead = null;
            string contentHead = null;
            string timeHeader = null;
            string eventRows = null;
            string nowLine = null;
            string contentTail = null;
            string cursorLine = null;
            string lifeEvents = null;
            string border = null;
            string jsCode = null;
            string svgTail = null;


            //ACT II : fill the components in order

            await GenerateComponents(inputPerson, timeRange.start, timeRange.end, daysPerPixel, inputedEventTags);


            //ACT III : compile in right placement
            var final =
                $@" <!--MADE BY MACHINES FOR HUMAN EYES-->
                    {svgHead}
                        <!--inside border-->
                        {contentHead}
                            {timeHeader}
                            {eventRows} <!-- with summary-->
                            {nowLine}
                        {contentTail}

                        <!--outside border-->
                        {cursorLine}
                        {lifeEvents}
                        {border} <!--border painted last-->
                        {jsCode} <!--place last-->
                    {svgTail}
                ";


            return final;


            async Task GenerateComponents(Person inputPerson, Time startTime, Time endTime, double daysPerPixel, List<EventTag> inputedEventTags)
            {
                //STEP 1: USER INPUT > USABLE DATA
                var svgBackgroundColor = "#f0f9ff"; //not bleach white
                var randomId = Tools.GenerateId();

                // One precision value for generating all dasa components,
                // because misalignment occurs if use different precision
                // note: precision = time slice count, each slice = 1 pixel (zoom level)
                double eventsPrecisionHours = Tools.DaysToHours(daysPerPixel);

                //generate time slice only once for all rows
                var timeSlices = Time.GetTimeListFromRange(startTime, endTime, eventsPrecisionHours);
                var svgWidth = timeSlices.Count; //slice per px
                var svgTotalWidth = svgWidth + 10; //add little for wiggle room

                int verticalYAxis; //last position of element above is set here
                verticalYAxis = 0; //start at 0 y axis

                //rows are dynamically generated as needed, hence the extra logic below
                //list of rows to generate
                EventsChartManager.UnsortedEventList = await EventManager.CalculateEvents(eventsPrecisionHours, startTime, endTime, inputPerson.GetBirthLocation(), inputPerson, inputedEventTags);


                //STEP 2: DATA > SVG COMPONENTS
                timeHeader = GenerateTimeHeaderRow(timeSlices, daysPerPixel, widthPerSlice, ref verticalYAxis);

                //note : avg speed 30s
                eventRows = GenerateEventRows(
                    eventsPrecisionHours,
                    startTime,
                    endTime,
                    inputPerson,
                    timeSlices,
                    inputedEventTags, ref verticalYAxis);

                nowLine = MakeNowLine(startTime, verticalYAxis, timeSlices);

                cursorLine = GetTimeCursorLine(verticalYAxis);

                lifeEvents = GetLifeEventLinesSvg(inputPerson, verticalYAxis, startTime, timeSlices);

                border = GetBorderSvg(timeSlices, verticalYAxis);

                //note: if width & height not hard set, parent div clips it
                var svgTotalHeight = 350;//todo for now hard set, future use: verticalYAxis;
                var svgStyle = $@"width:{svgTotalWidth}px;height:{svgTotalHeight}px;background:{svgBackgroundColor};";//end of style tag
                svgHead = $"<svg class=\"EventChartHolder\" id=\"{randomId}\" style=\"{svgStyle}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">";//much needed for use tags to work

                jsCode = GetJsCodeSvg(randomId);

                //place all content except border & cursor time legend inside group for padding
                const int contentPadding = 2;//todo move to central place
                contentHead = $"<g class=\"EventChartContent\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">";

                svgTail = "</svg>";
                contentTail = "</g>";

            }

        }


        //wraps a list of svg elements inside 1 main svg element
        //if width not set defaults to 1000px, and height to 1000px
        //todo temp who ever is calling can change
        public static string WrapSvgElements(string svgClass, string combinedSvgString, int svgWidth, int svgTotalHeight, string randomId, string svgBackgroundColor = "#f0f9ff")
        {

            //var svgBackgroundColor = "#f0f9ff";

            //use random id for each chart, done so that js can uniquely
            //manipulate 1 chart in page of multiple charts

            //<? xml version = "1.0" encoding = "utf-8" ?>
            //    < svg viewBox = "5.376 1.192 809.205 1885.759" width = "809.205" height = "1885.759" style = "width:810px;height:1110px;background:#f0f9ff;" xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" >

            //create the final svg that will be displayed
            var svgTotalWidth = svgWidth + 10; //add little for wiggle room
            var svgBody = $"<svg class=\"{svgClass}\" id=\"{randomId}\"" +
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




        //▒█▀▀█ ▒█▀▀█ ▀█▀ ▒█░░▒█ ░█▀▀█ ▀▀█▀▀ ▒█▀▀▀ 
        //▒█▄▄█ ▒█▄▄▀ ▒█░ ░▒█▒█░ ▒█▄▄█ ░▒█░░ ▒█▀▀▀ 
        //▒█░░░ ▒█░▒█ ▄█▄ ░░▀▄▀░ ▒█░▒█ ░▒█░░ ▒█▄▄▄


        /// <summary>
        /// Use this find where to place row in chart, Y axis (vertical position)
        /// scans from last
        /// NOTE : 2 names for 1 methods, yeah! clearer caller code
        /// </summary>
        private static int GetLastOccupiedYAxis() => GetNextFreeYAxis();
        private static int GetNextFreeYAxis()
        {
            //scan from last till you hit something
            for (int i = verticalYAxisManifest.Length; i >= 0; i--)
            {
                //return next position where empty starts
                //if true, means is occupied
                if (verticalYAxisManifest[i]) { return i++; }
            }

            //if control reaches here, complete free, return room 0
            return 0;

        }

        private static string GetJsCodeSvg(string randomId)
        {
            string jsCode = "";

            //add last to load last
            //load order is important
            jsCode += "<script href=\"https://code.jquery.com/jquery-3.6.3.min.js\" />";
            jsCode += "<script href=\"https://cdn.jsdelivr.net/npm/@svgdotjs/svg.js@3.0/dist/svg.min.js\" />";//used in events chart inside js
            jsCode += "<script href=\"https://www.vedastro.org/js/EventsChart.js\" />";
            //compiledRow += "<script href=\"https://www.vedastro.org/js/EventsChartInside.js\" />";

            //random id is created here to link svg element with JS instance
            jsCode += $@"
                            <script>//<![CDATA[

                                new EventsChart($(""#{randomId}""));

                                //animate chart
                                window.EventsChart.animateChart();

                            //]]>
                            </script>
                            ";

            return jsCode;
        }

        private static string GetBorderSvg(List<Time> timeSlices, int svgTotalHeight)
        {
            //save a copy of the number of time slices used to calculate the svg total width later
            var dasaSvgWidth = timeSlices.Count;

            //add border around svg element
            //note:compensate for padding, makes border fit nicely around content
            var borderWidth = dasaSvgWidth + 2; //contentPadding = 2 todo centralize
            var roundedBorder = 3;
            svgTotalHeight += 10; //adjust
            var compiledRow = $"<rect class=\"EventChartBorder\" rx=\"{roundedBorder}\" width=\"{borderWidth}\" height=\"{svgTotalHeight}\" style=\"stroke-width: 2; fill: none; paint-order: stroke; stroke:#333;\"></rect>";

            return compiledRow;
        }

        private static string MakeNowLine(Time startTime, int verticalYAxis, List<Time> timeSlices)
        {
            //the height for all lines, cursor, now & life events line
            //place below the last row
            var nowLineY = verticalYAxis + 2; //space between rows


            //get now line position
            var clientNowTime = startTime.StdTimeNowAtOffset; //now time at client
            var nowLinePosition = GetLinePosition(timeSlices, clientNowTime);
            //skip now line if beyond chart on both sides
            var beyondLimit = nowLinePosition == 0 || nowLinePosition == (timeSlices.Count - 1);
            var nowLineHeight = nowLineY + 6; //space between icon & last row
            var nowLineSvgGroup = GetNowLine(nowLineHeight, nowLinePosition, beyondLimit); //hides if beyond limit


            return nowLineSvgGroup;
        }

        /// <summary>
        /// Given planet list, will return factor for strongest only
        /// </summary>

        private static double GetPlanetPowerFactor(List<PlanetName> inputPlanetList, Time time)
        {
            //if no input planets return 0 power,
            //this makes events with no planets not appear in smart sum row
            //muhurtha events also are stopped here
            if (!inputPlanetList.Any()) { return 0; }

            //get all power factors into a list
            var listx = new List<double>();
            foreach (var planet in inputPlanetList)
            {
                var x = GetPlanetPowerFactor(planet, time);
                listx.Add(x);
            }

            //todo can experiment with average as well,
            //strongest was choose because, it is assumed that strong bhukti can override dasa
            //take the strongest
            var strongest = listx.Max();

            return strongest;

        }

        /// <summary>
        /// Given a list planet names only power factor of strongest planet is returned
        /// convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
        /// </summary>
        private static double GetPlanetPowerFactor(PlanetName valuePlanet, Time time)
        {
            //get all planet strength for given time (horoscope)
            var list = AstronomicalCalculator.GetAllPlanetStrength(time);
            //get the power of the planet inputed
            var planetPwr = list.FirstOrDefault(x => x.Item2 == valuePlanet).Item1;

            //get min & max
            var min = list.Min(x => x.Item1); //weakest planet
            var max = list.Max(x => x.Item1); //strongest planet

            //convert the planets strength into a value over hundred with max & min set by strongest & weakest planet
            //returns as percentage over 100%
            var factor = planetPwr.Remap(min, max, 0, 100);

            //planet power below 60% filtered out
            factor = factor < 60 ? 0 : factor;

            return factor;
        }

        /// <summary>
        /// Get color based on nature
        /// </summary>
        private static string GetColor(EventNature? eventNature) => GetColor(eventNature.ToString());

        /// <summary>
        /// Get color based on nature
        /// </summary>
        private static string GetColor(string nature)
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
        private static string GetSummaryColor(double totalNature, double minValue, double maxValue)
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
        /// Incrementing line height also increments icon position below
        /// </summary>
        private static string GenerateLifeEventLine(LifeEvent lifeEvent, int lineHeight, DateTimeOffset lifeEvtTime, int positionX)
        {
            //shorten the event name if too long & add ellipsis at end
            //else text goes out side box
            var formattedEventName = ShortenName(lifeEvent.Name);

            int iconYAxis = lineHeight + 7; //start icon at end of line + 7 padding
            var iconXAxis = $"-7.5"; //use negative to move center under line
            var nameTextHeight = 15;

            //only show name label for Major and Normal events
            var isMajor = lifeEvent.Weight == "Major";
            var isNormal = lifeEvent.Weight == "Normal";
            var isShowName = isNormal || isMajor; //show name for normal and major only
            var evtNameStyle = isShowName ? "" : "display: none;";

            //display description only for major events
            var descriptionDisplayStyle = isMajor ? "" : "display:none;";

            string descriptionTextSvg = StringToSvgTextBox(lifeEvent.Description, out var boxHeightPx);

            //special space saving background for Normal with desc box underneath
            var nameBackgroundWidth = isMajor ? 82 : GetTextWidthPx(formattedEventName);
            var descriptionBackgroundWidth = 100; //for 24 characters

            var iconSvg = $@"
                                <rect class=""vertical-line"" fill=""#1E1EEA"" width=""2"" height=""{iconYAxis}""></rect>
                                <!-- EVENT ICON LABEL -->
                                <g transform=""translate({iconXAxis},{iconYAxis})"">
                                    <g class=""name-label"" >
                                        <!-- EVENT NAME-->
                                        <g transform=""translate(18,0)"" style=""{evtNameStyle}"">
						                    <rect class=""background"" x=""0"" y=""0"" style=""fill: blue; opacity: 0.8;"" width=""{nameBackgroundWidth}"" height=""{nameTextHeight}"" rx=""2"" ry=""2"" />
						                    <text class=""event-name"" x=""3"" y=""11"" style=""fill:#FFFFFF; font-family:'Calibri'; font-size:12px;"">{formattedEventName}</text>
					                    </g>
                                        <!-- EVENT ICON-->
				                        <g class=""nature-icon"" transform=""translate(8,8)"">
                                            <rect class=""background"" fill=""{GetColor(lifeEvent.Nature)}"" x=""-7.5"" y=""-8"" width=""15"" height=""15"" rx=""2"" ry=""2""/>
					                        <path d=""M2-0.2H0.7C0.5-0.2,0.3,0,0.3,0.2v1.3c0,0.2,0.2,0.4,0.4,0.4H2c0.2,0,0.4-0.2,0.4-0.4V0.2C2.5,0,2.3-0.2,2-0.2z M2-4.5v0.4 h-3.6v-0.4c0-0.2-0.2-0.4-0.4-0.4c-0.2,0-0.4,0.2-0.4,0.4v0.4h-0.4c-0.5,0-0.9,0.4-0.9,0.9v6.1c0,0.5,0.4,0.9,0.9,0.9h6.3 c0.5,0,0.9-0.4,0.9-0.9v-6.1c0-0.5-0.4-0.9-0.9-0.9H3.1v-0.4c0-0.2-0.2-0.4-0.4-0.4S2-4.8,2-4.5z M2.9,2.9h-5.4 c-0.2,0-0.4-0.2-0.4-0.4v-4.4h6.3v4.4C3.4,2.7,3.2,2.9,2.9,2.9z""/>
				                        </g>
                                    </g>
                                    <!-- DESCRIPTION -->
		                            <g class=""description-label"" style=""{descriptionDisplayStyle}"" transform=""translate(0,{17})"">
                                        <rect class=""background"" style=""fill: blue; opacity: 0.8;"" width=""{descriptionBackgroundWidth}"" height=""{boxHeightPx+5}"" rx=""2"" ry=""2""/>
                                        <text transform=""translate(2,11)"">
                                            {descriptionTextSvg}
			                            </text>
		                            </g>
                                </g>
                                ";

            //put together icon + line + event data
            var lifeEventLine = $@"<g eventName=""{lifeEvent.Name}"" class=""LifeEventLines"" stdTime=""{lifeEvtTime:dd/MM/yyyy}"" 
                              transform=""translate({positionX}, 0)"">{iconSvg}</g>";

            return lifeEventLine;

            //LOCAL FUNC
            string ShortenName(string rawInput)
            {
                //if no changes needed return as is (default)
                var returnVal = rawInput;
                const int nameCharLimit = 14; //any number of char above this limit will be replaced with  ellipsis  "..."

                //if input is above limit
                //replace extra chars with ...
                var isAbove = rawInput.Length > nameCharLimit;
                if (isAbove)
                {
                    //cut the extra chars out
                    returnVal = returnVal.Substring(0, nameCharLimit);

                    //add ellipsis 
                    returnVal += "...";
                }

                return returnVal;
            }

        }

        private static double GetTextWidthPx(string textInput)
        {

            SizeF size;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                size = graphics.MeasureString(textInput,
                    new Font(
                        familyName: "Calibri",
                        emSize: 10,
                        style: FontStyle.Regular,
                        unit: GraphicsUnit.Pixel));
            }

            var widthPx = Math.Round(size.Width);

            return widthPx;
        }


        /// <summary>
        /// Input sting to get nicely wrapped text in SVG
        /// Note: this is only an invisible structured text, other styling have to be added separately
        /// </summary>
        /// <returns></returns>
        private static string StringToSvgTextBox(string inputStr, out int boxHeightPx)
        {

            const int characterLimit = 21;
            //chop string to pieces, to wrap text nicely (new line)
            var splitStringRaw = Tools.SplitByCharCount(inputStr, characterLimit);

            //convert raw string to be HTML safe (handles special chars like &,!,%)
            var splitString = splitStringRaw.Select(rawStr => HttpUtility.HtmlEncode(rawStr)).ToList();

            //make a new holder for each line
            var compiledSvgLines = "";
            const int lineHeight = 12;
            var nextYAxis = 0; //start with 0, at top
            foreach (var strLine in splitString)
            {
                var newLineSvg = $@"<tspan x=""0"" y=""{nextYAxis}"" fill=""#FFFFFF"" font-family=""Calibri"" font-size=""10"">{strLine}</tspan>";

                //add together with other lines
                compiledSvgLines += newLineSvg;

                //set next line to come underneath this (increment y axis)
                nextYAxis += lineHeight;
            }

            //save to be used to draw border around
            boxHeightPx = nextYAxis;

            //var finalTextSvg = $@"{compiledSvgLines}";

            return compiledSvgLines;
        }

        /// <summary>
        /// Returns a person's life events in SVG group to be placed inside events chart
        /// gets person's life events as lines for the events chart
        /// </summary>
        private static string GetLifeEventLinesSvg(Person person, int verticalYAxis, Time startTime, List<Time> timeSlices)
        {
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var lineHeight = verticalYAxis + 9; //space between icon & last row
            var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in


            //add 1 to offset 0 index
            //each 1 cell is 1 px
            var maxSlices = timeSlices.Count + 1;
            var rowList = new List<bool[]>();

            //space smaller than this is set as crowded
            const int minSpaceBetween = 80;//px
            //var halfWidth = minSpaceBetween / 2; //icon


            //sort by earliest to latest event
            var lifeEventList = person.LifeEventList;
            lifeEventList.Sort((x, y) => x.CompareTo(y));

            var incrementRate = 20; //for overcrowded jump
            var adjustedLineHeight = lineHeight; //keep copy for resetting after overcrowded jum

            var listRowData = new List<string>();
            foreach (var lifeEvent in lifeEventList)
            {

                //get timezone at place event happened
                var lifeEvtTime = lifeEvent.StartTime.GetStdDateTimeOffset();//time at the place of event with correct standard timezone
                var startTimeInputOffset = lifeEvtTime.ToOffset(inputOffset); //change to output offset, to match chart
                var positionX = GetLinePosition(timeSlices, startTimeInputOffset);

                //if line is not in report time range, don't generate it
                if (positionX == 0) { continue; }

                //no print minor events
                if (lifeEvent.Weight == "Minor") { continue; }

                //get row number, assign row number that is free to occupy
                var rowNumber = GetRowNumber(positionX); //start at 0 index

                //mark as occupied for future ref
                MarkRowNumber(positionX, rowNumber);

                //calculate final event icon height avoiding other icons 
                adjustedLineHeight += rowNumber * incrementRate;

                //put together icon + line + event data
                var generateLifeEventLine = GenerateLifeEventLine(lifeEvent, adjustedLineHeight, lifeEvtTime, positionX);

                //save it under its row with others
                while (rowNumber > (listRowData.Count - 1)) { listRowData.Add(""); } //add empty row if 1st
                listRowData[rowNumber] += generateLifeEventLine;

                //reset line height for next 
                if (rowNumber != 0) { adjustedLineHeight = lineHeight; }
            }

            //place each row in a group and add to final list
            //NOTE:
            //we stack the row from last to first, so that the top most row, is painted last,
            //thus appearing above the lines of the events below
            var finalSvg = "";
            int rowNum = (listRowData.Count - 1); //0 index
            for (; rowNum >= 0; rowNum--)
            {
                var rowEventIcons = listRowData[rowNum];
                var wrap = $@"<g id=""row{rowNum}"">{rowEventIcons}</g>";
                finalSvg += wrap;

            }

            //wrap in a group so that can be hidden/shown as needed
            //add transform matrix to adjust for border shift
            const int contentPadding = 2;
            var wrapperGroup = $"<g id=\"LifeEventLinesHolder\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{finalSvg}</g>";

            return wrapperGroup;

            //-------------------------

            void MarkRowNumber(int startX, int rowNumber)
            {
                //var startX = middleX - halfWidth;
                var endX = startX + minSpaceBetween;

                //set limits
                startX = startX < 0 ? 0 : startX;
                endX = endX > (maxSlices - 1) ? (maxSlices - 1) : endX;


                for (int i = startX; i <= endX; i++)
                {
                    //mark as occupied
                    rowList[rowNumber][i] = true;
                }
            }

            //start at 0 index
            int GetRowNumber(int startX)
            {
                //var startX = middleX - halfWidth;
                var endX = startX + minSpaceBetween;

                //set limits
                startX = startX < 0 ? 0 : startX;
                endX = endX > (maxSlices - 1) ? (maxSlices - 1) : endX;

            TryAgain:
                //check if space is free in rows
                foreach (var row in rowList)
                {
                    var startFree = row[startX] == false;
                    var endFree = row[endX] == false;
                    if (startFree && endFree)
                    {
                        return rowList.IndexOf(row);
                    }
                }

                //if control comes here, than not enough rows so add some
                rowList.Add(new bool[maxSlices]);
                goto TryAgain;

                throw new Exception("Row count exceed!");
            }

            //check if current event icon position will block previous life event icon
            //expects next event to be chronologically next event
            bool IsEventIconSpaceCrowded(int previousX, int currentX)
            {
                //if previous 0, then obviously not crowded
                if (previousX <= 0) { return false; }

                //space smaller than this is set crowded
                const int minSpaceBetween = 110;//px

                //previous X axis should be lower than current
                //difference shows space between these 2 icons
                var difference = currentX - previousX;
                var isOverLapping = difference < minSpaceBetween;
                return isOverLapping;
            }
        }

        /// <summary>
        /// add in the cursor line (moves with cursor via JS)
        /// note: template cursor line is duplicated to dynamically generate legend box
        /// </summary>
        private static string GetTimeCursorLine(int verticalYAxis)
        {
            //add in the cursor line (moves with cursor via JS)
            //note: - template cursor line is duplicated to dynamically generate legend box
            //      - placed last so that show on top of others
            var lineHeight = verticalYAxis + 110;//add space for life event icon


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

        /// <summary>
        /// get now line SVG group
        /// if set hide will set display as hidden
        /// </summary>
        private static string GetNowLine(int lineHeight, int nowLinePosition, bool hideOnLoad = false)
        {

            //add hide style if caller specifies to hide on load
            var extraClass = hideOnLoad ? "style=\"display: none;\"" : "";

            return $@"
                       <g id=""NowVerticalLine"" x=""0"" y=""0"" {extraClass} transform=""matrix(1, 0, 0, 1, {nowLinePosition}, 0)"">
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
        private static int GetLinePosition(List<Time> timeSliceList, DateTimeOffset inputTime)
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

        private static string GenerateTimeHeaderRow(List<Time> timeSlices, double daysPerPixel, int _widthPerSlice, ref int headerY)
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
            Person inputPerson, List<Time> timeSlices, List<EventTag> inputedEventTags, ref int yAxis)
        {
            //1 GENERATE DATA FOR EVENT ROWS
            const int widthPerSlice = 1;
            const int singleRowHeight = 15;
            const int SummaryRowHeight = 22;


            //sort event by duration, so that events are ordered nicely in chart
            //todo events are breaking up between rows
            //todo order by planet strength
            var eventList = EventsChartManager.UnsortedEventList.OrderByDescending(x => x.Duration).ToList();


            //2 STACK & GENERATED ROWS FROM ABOVE DATA
            var padding = 1;//space between rows
            var compiledRow = "";
            double maxValue;
            double minValue;
            Dictionary<int, SumData> summaryRowData;
            if (eventList.Any())
            {

                //note: summary data is filled when generating rows
                //x axis, total nature score, planet name
                summaryRowData = new Dictionary<int, SumData>();
                //generate svg for each row & add to final row
                //compiledRow += GenerateMultipleRowSvg(eventList, timeSlices, yAxis, 0, out int finalHeight);
                compiledRow += GenerateMultipleRowSvg(eventList, timeSlices, yAxis, 0, out int finalHeight);
                //set y axis (horizontal) for next row
                yAxis = yAxis + finalHeight + padding;

                //4 GENERATE SUMMARY ROW
                //min & max used to calculate color later
                maxValue = summaryRowData?.Values?.Max(x => x.NatureScore) ?? 0;
                minValue = summaryRowData?.Values?.Min(x => x.NatureScore) ?? 0;
                compiledRow += GenerateSummaryRow(yAxis);

                //note caller checks final height by checking y axis by ref
                yAxis += 15;//add in height of summary row

            }

            return compiledRow;



            //█░░ █▀█ █▀▀ ▄▀█ █░░   █▀▀ █░█ █▄░█ █▀▀ ▀█▀ █ █▀█ █▄░█ █▀
            //█▄▄ █▄█ █▄▄ █▀█ █▄▄   █▀░ █▄█ █░▀█ █▄▄ ░█░ █ █▄█ █░▀█ ▄█

            string GenerateSummaryRow(int yAxis)
            {
#if DEBUG
                Console.WriteLine($"GenerateSummaryRow : MAX:{maxValue}, MIN:{minValue}");
#endif

                var rowHtml = "";
                //STEP 1 : generate color summary
                var colorRow = "";
                foreach (var summarySlice in summaryRowData)
                {
                    int xAxis = summarySlice.Key;
                    //total nature score is sum of negative & positive 1s of all events
                    //that occurred at this point in time, possible negative number
                    //exp: -4 bad + 5 good = 1 total nature score
                    double totalNatureScore = summarySlice.Value.NatureScore;

                    var rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{SummaryRowHeight}\" " +
                               $"fill=\"{GetSummaryColor(totalNatureScore, minValue, maxValue)}\" />";

                    //add rect to row
                    colorRow += rect;
                }

                rowHtml += $"<g id=\"ColorRow\">{colorRow}</g>";


                //STEP 2 : generate graph summary
                //var barChartRow = "";
                //foreach (var summarySlice in summaryRowData)
                //{
                //    int xAxis = summarySlice.Key;
                //    double totalNatureScore = summarySlice.Value.NatureScore; //possible negative
                //    var barHeight = (int)totalNatureScore.Remap(minValue, maxValue, 0, 30);
                //    var rect = $"<rect " +
                //               $"x=\"{xAxis}\" " +
                //               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                //               $"width=\"{widthPerSlice}\" " +
                //               $"height=\"{barHeight}\" " +
                //               $"fill=\"black\" />";

                //    //add rect to row
                //    barChartRow += rect;
                //}

                ////note: chart is flipped 180, to start bar from bottom to top
                ////default hidden
                //rowHtml += $"<g id=\"BarChartRow\" transform=\"matrix(1, 0, 0, 1, 0, 20)\">{barChartRow}</g>";


                //STEP 3 : generate color summary SMART
                //var colorRowSmart = "";
                //foreach (var summarySlice in summaryRowData)
                //{
                //    int xAxis = summarySlice.Key;
                //    //total nature score is sum of negative & positive 1s of all events
                //    //that occurred at this point in time, possible negative number
                //    //exp: -4 bad + 5 good = 1 total nature score
                //    double totalNatureScore = summarySlice.Value.NatureScore;
                //    var planetPowerFactor = GetPlanetPowerFactor(summarySlice.Value.Planet, summarySlice.Value.BirthTime);
                //    var smartNatureScore = totalNatureScore * planetPowerFactor;
                //    var rect = $"<rect " +
                //               $"x=\"{xAxis}\" " +
                //               $"y=\"{yAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                //               $"width=\"{widthPerSlice}\" " +
                //               $"height=\"{singleRowHeight}\" " +
                //               $"fill=\"{GetSummaryColor(smartNatureScore, -100, 100)}\" />";

                //    //add rect to row
                //    colorRowSmart += rect;
                //}
                ////note: chart is flipped 180, to start bar from bottom to top
                ////default hidden
                //rowHtml += $"<g id=\"BarChartRowSmart\" transform=\"matrix(1, 0, 0, 1, 0, 43)\">{colorRowSmart}</g>";


                //STEP 4 : final wrapper
                //add in "Smart Summary" label above row
                float aboveRow = yAxis - singleRowHeight - padding;
                rowHtml += $@"
                    <g id=""SummaryLabel"" transform=""matrix(1, 0, 0, 1, 0, {aboveRow})"">
				        <rect style=""fill: blue; opacity: 0.80;"" x=""-1"" y=""0"" width=""100"" height=""15"" rx=""2"" ry=""2""/>
				        <text style=""fill:#FFFFFF; font-size:11px; font-weight:400;"" x=""16"" y=""11"">Smart Summary</text>
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

                        //generate and add to row
                        //the hard coded attribute names used here are used in App.js
                        var rect = $"<rect " +
                                   $"eventname=\"{foundEvent?.FormattedName}\" " +
                                   $"eventdescription=\"{foundEvent?.Description}\" " +
                                   $"age=\"{inputPerson.GetAge(slice)}\" " +
                                   $"stdtime=\"{slice.GetStdDateTimeOffset().ToString(Time.DateTimeFormat)}\" " +
                                   $"x=\"{horizontalPosition}\" " +
                                   $"y=\"{finalYAxis}\" " + //y axis placed here instead of parent group, so that auto legend can use the y axis
                                   $"width=\"{widthPerSlice}\" " +
                                   $"height=\"{singleRowHeight}\" " +
                                   $"fill=\"{color}\" />";

                        //add rect to return list
                        rowHtml += rect;


                        //STAGE 2 : SAVE SUMMARY DATA
                        //every time a rect is added, we keep track of it in a list to generate the summary row at last
                        //based on event nature minus or plus 1
                        double natureScore = 0;

                        //calculate accurate nature score
                        natureScore = CalculateNatureScore(foundEvent, inputPerson);

                        //compile nature score for making summary row later (defaults to 0)
                        var previousNatureScoreSum = (summaryRowData.ContainsKey(horizontalPosition) ? summaryRowData[horizontalPosition].NatureScore : 0);

                        var x = new SumData
                        {
                            BirthTime = inputPerson.BirthTime,
                            Planet = Tools.GetPlanetFromName(foundEvent.FormattedName),
                            NatureScore = natureScore + previousNatureScoreSum //combine current with previous
                        };
                        summaryRowData[horizontalPosition] = x;


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

        /// <summary>
        /// Intelligently calculates summary score
        /// </summary>
        private static double CalculateNatureScore(Event foundEvent, Person person)
        {
            //STAGE 1:
            //score from general nature of event
            var generalScore = 0;
            switch (foundEvent?.Nature)
            {
                case EventNature.Good:
                    generalScore = 1;
                    break;
                case EventNature.Bad:
                    generalScore = -1;
                    break;
            }

            //STAGE 2: Special score
            //var eventScore = GetEventScoreFromShadvargaTop3(foundEvent, person);
            var eventScore2 = GetEventScoreFromShadvargaMK3(foundEvent, person);


#if DEBUG
            //Console.WriteLine($"PlanetOrHouse:{eventScore2}");
#endif

            var final = 0;
            final += generalScore;
            final += eventScore2;

            return final;
        }

        private static int GetEventScoreFromShadvargaTop3(Event foundEvent, Person person)
        {
            var finalScore = 0;

            //get all planets used in making event
            var planetInEventList = foundEvent.GetRelatedPlanet();

            //get top 3 planets as good
            var beneficPlanetList = AstronomicalCalculator.GetBeneficPlanetListByShadbala(person.BirthTime);
            //var beneficPlanetList2 = AstronomicalCalculator.GetBeneficPlanetListByShadbala(person.BirthTime, 500);
            var maleficPlanetList = AstronomicalCalculator.GetMaleficPlanetListByShadbala(person.BirthTime);
            //var maleficPlanetList2 = AstronomicalCalculator.GetMaleficPlanetListByShadbala(person.BirthTime, 300);

            //add and remove score based on planet good and bad todo add remove by voting power
            foreach (var planetInEvent in planetInEventList)
            {
                //has good planet plus 1
                var beneficFound = beneficPlanetList.Contains(planetInEvent);
                if (beneficFound) { finalScore += 1; }

                //has good planet plus 1
                //var beneficFound2 = beneficPlanetList2.Contains(planetInEvent);
                //if (beneficFound2) { finalScore += 1; }

                //has bad planet minus 1
                var maleficFound = maleficPlanetList.Contains(planetInEvent);
                if (maleficFound) { finalScore += -1; }

                //has bad planet minus 1
                //var maleficFound2 = maleficPlanetList2.Contains(planetInEvent);
                //if (maleficFound2) { finalScore += -1; }

            }



            var houseInEventList = foundEvent.GetRelatedHouse();

            var beneficHouseList = AstronomicalCalculator.GetBeneficHouseListByShadbala(person.BirthTime);
            // var beneficHouseList2 = AstronomicalCalculator.GetBeneficHouseListByShadbala(person.BirthTime, 550);
            var maleficHouseList = AstronomicalCalculator.GetMaleficHouseListByShadbala(person.BirthTime);
            //var maleficHouseList2 = AstronomicalCalculator.GetMaleficHouseListByShadbala(person.BirthTime, 250);

            foreach (var houseName in houseInEventList)
            {
                //has good planet plus 1
                var beneficFound = beneficHouseList.Contains(houseName);
                if (beneficFound) { finalScore += 1; }

                //has good planet plus 1
                //var beneficFound2 = beneficHouseList2.Contains(houseName);
                //if (beneficFound2) { finalScore += 1; }

                //has bad planet minus 1
                var maleficFound = maleficHouseList.Contains(houseName);
                if (maleficFound) { finalScore += -1; }

                //has bad planet minus 1
                //var maleficFound2 = maleficHouseList2.Contains(houseName);
                //if (maleficFound2) { finalScore += -1; }

            }


            //return the compiled score to caller
            return finalScore;
        }


        private static int GetEventScoreFromShadvargaMK3(Event foundEvent, Person person)
        {
            //get house that the event is related to
            var relatedHouse = foundEvent.GetRelatedHouse().FirstOrDefault(); //for now assume only one
            //get nature of house based on shadbala
            var houseNatureScore = AstronomicalCalculator.GetHouseNatureScore(person.BirthTime, relatedHouse);

            //get houses and planet that the event is related to
            var relatedPlanet = foundEvent.GetRelatedPlanet().FirstOrDefault(); //for now assume only one
            //get nature of planet based on shadbala
            var planetNatureScore = AstronomicalCalculator.GetPlanetNatureScore(person.BirthTime, relatedPlanet);

            var final = 0;
            final += houseNatureScore;
            final += planetNatureScore;

            return final;

        }


        private static int GetEventScoreFromShadvargaPlanetOrHouse(Event foundEvent, Person person)
        {
            var finalScore = 0;

            //get all planets used in making event
            var planetInEventList = foundEvent.GetRelatedPlanet();

            //get top 3 planets as good
            var beneficPlanetList = AstronomicalCalculator.GetBeneficPlanetListByShadbala(person.BirthTime);
            //var beneficPlanetList2 = AstronomicalCalculator.GetBeneficPlanetListByShadbala(person.BirthTime, 500);
            var maleficPlanetList = AstronomicalCalculator.GetMaleficPlanetListByShadbala(person.BirthTime);
            //var maleficPlanetList2 = AstronomicalCalculator.GetMaleficPlanetListByShadbala(person.BirthTime, 300);

            //add and remove score based on planet good and bad todo add remove by voting power
            foreach (var planetInEvent in planetInEventList)
            {
                //has good planet plus 1
                var beneficFound = beneficPlanetList.Contains(planetInEvent);
                if (beneficFound) { finalScore += 1; }

                //has good planet plus 1
                //var beneficFound2 = beneficPlanetList2.Contains(planetInEvent);
                //if (beneficFound2) { finalScore += 1; }

                //has bad planet minus 1
                var maleficFound = maleficPlanetList.Contains(planetInEvent);
                if (maleficFound) { finalScore += -1; }

                //has bad planet minus 1
                //var maleficFound2 = maleficPlanetList2.Contains(planetInEvent);
                //if (maleficFound2) { finalScore += -1; }

            }


            //only use houses when planets empty
            var noPlanets = !planetInEventList.Any();
            if (noPlanets)
            {
                var houseInEventList = foundEvent.GetRelatedHouse();

                var beneficHouseList = AstronomicalCalculator.GetBeneficHouseListByShadbala(person.BirthTime);
                // var beneficHouseList2 = AstronomicalCalculator.GetBeneficHouseListByShadbala(person.BirthTime, 550);
                var maleficHouseList = AstronomicalCalculator.GetMaleficHouseListByShadbala(person.BirthTime);
                //var maleficHouseList2 = AstronomicalCalculator.GetMaleficHouseListByShadbala(person.BirthTime, 250);

                foreach (var houseName in houseInEventList)
                {
                    //has good planet plus 1
                    var beneficFound = beneficHouseList.Contains(houseName);
                    if (beneficFound) { finalScore += 1; }

                    //has good planet plus 1
                    //var beneficFound2 = beneficHouseList2.Contains(houseName);
                    //if (beneficFound2) { finalScore += 1; }

                    //has bad planet minus 1
                    var maleficFound = maleficHouseList.Contains(houseName);
                    if (maleficFound) { finalScore += -1; }

                    //has bad planet minus 1
                    //var maleficFound2 = maleficHouseList2.Contains(houseName);
                    //if (maleficFound2) { finalScore += -1; }

                }

            }


            //return the compiled score to caller
            return finalScore;
        }
    }
}
