using System.Collections.Concurrent;
using System.Drawing.Imaging;
using VedAstro.Library;
using System.Drawing;
using Gif.Components;
using Microsoft.AspNetCore.Routing.Template;
using static API.SkyChartManager;
using Svg;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json.Linq;

namespace API
{
    /// <summary>
    /// Logic to create Sky Chart, simple chart with zodiac and planets in it
    /// </summary>
    public static class SkyChartManager
    {


        [Function(nameof(GenerateChartAsync))]
        public static async Task<string> GenerateChartAsync([ActivityTrigger] string payloadString, FunctionContext executionContext)
        {
            var payload = JObject.Parse(payloadString);
            Time time = Time.FromJson(payload["Time"]);
            double widthPx = payload["Width"].Value<double>();
            double heightPx = payload["Height"].Value<double>();

            var x = await GenerateChart(time, widthPx, heightPx);

            return x;
        }

        /// <summary>
        /// Sweet heart takes this away!
        /// Basically generating 1 frame
        /// </summary>
        public static async Task<string> GenerateChart(Time time, double widthPx, double heightPx)
        {
            //PART I : declare the components
            string svgHead = null;
            string svgTail = null;
            string border = null;
            string contentTail = null;
            string content = null;
            string angleRuler = null;
            string signRuler = null;
            string houseRuler = null;
            string zodiacRuler = null;
            string dateTimeLocation = null;
            string locationHeader = null;



            //PART II : fill the components in order
            await GenerateComponents();



            //PART III : compile in right placement
            var final =
                $@" <!--MADE BY MACHINES FOR HUMAN EYES-->
                    {svgHead}
                        {dateTimeLocation}
                        {locationHeader}
                        <!--inside border-->
	                    <g transform=""translate(14, 16)"">
                            {angleRuler}
                            {zodiacRuler}
                            {houseRuler}
                            {content}
	                    </g>

                        <!--outside border-->
                        {border} <!--border painted last-->
                    {svgTail}
                ";


            return final;




            //------------------------LOCALS NEEDED FOR REFS

            async Task GenerateComponents()
            {
                //STEP 1: USER INPUT > USABLE DATA
                var svgBackgroundColor = "#f0f2f5"; //not bleach white
                var randomId = Tools.GenerateId();


                var planetList = AstronomicalCalculator.GetAllPlanetLongitude(time);

                var renderWidth = widthPx - 30; // 750 -> 720
                angleRuler = GenerateAngleRuler(renderWidth, 10);

                zodiacRuler = await GenerateZodiacRuler(renderWidth, 15);

                houseRuler = GenerateHouseRuler(time, widthPx, 95);

                dateTimeLocation = GetDateTimeHeader(time);
                locationHeader = GetLocationHeader(time);

                border = GetBorderSvg((int)widthPx, (int)heightPx);

                content = await GetAllPlanetLineIcons(planetList, widthPx, 120); //130px from top is planet icon

                //note: if width & height not hard set, parent div clips it
                var svgTotalHeight = heightPx;//todo for now hard set, future use: verticalYAxis;
                var svgTotalWidth = widthPx;//todo for now hard set, future use: verticalYAxis;
                var svgStyle = $@"background:{svgBackgroundColor};";//end of style tag
                svgHead = $"<svg viewBox=\"0 0 {svgTotalWidth} {svgTotalHeight}\" width=\"{svgTotalWidth}px\" height=\"{svgTotalHeight}px\" style=\"{svgStyle}\" class=\"SkyChartHolder\" id=\"{randomId}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">";//much needed for use tags to work

                svgTail = "</svg>";
                contentTail = "</g>";


            }

        }

        private static string? GetDateTimeHeader(Time time)
        {
            return $@"<text transform=""translate(290,12)"" style=""font-size:14px;"">{time.GetStdDateTimeOffsetText()}</text>";
        }
        private static string? GetLocationHeader(Time time)
        {
            return $@"<text transform=""translate(580,12)"" style=""font-size:14px;"">{time.GetGeoLocation().ToString()}</text>";
        }

        private static string GetBorderSvg(int svgWidth, int svgTotalHeight)
        {
            //save a copy of the number of time slices used to calculate the svg total width later
            //var dasaSvgWidth = timeSlices.Count;

            //add border around svg element
            //note:compensate for padding, makes border fit nicely around content
            var borderWidth = svgWidth + 2; //contentPadding = 2 todo centralize
            var roundedBorder = 3;
            //var compiledRow = $"<rect class=\"Border\" rx=\"{roundedBorder}\" width=\"{borderWidth}\" height=\"{svgTotalHeight}\" style=\"stroke-width: 1; fill: none; paint-order: stroke; stroke:#333;\"></rect>";
            var compiledRow = $"\t<rect transform=\"translate(10, 15)\" class=\"Border\" rx=\"3\" width=\"{borderWidth - 30}\" height=\"{svgTotalHeight - 30}\" style=\"stroke-width: 1; fill: none; paint-order: stroke; stroke: rgb(51, 51, 51);\"/>";

            return compiledRow;
        }




        public static async Task<byte[]> GenerateChartGif(Time time, double width, double height)
        {
            //STAGE 1: get charts as SVG list frames
            var startTime = time.SubtractHours(Tools.DaysToHours(15));
            var endTime = time.AddHours(Tools.DaysToHours(15));
            var timeSliceList = EventManager.GetTimeListFromRange(startTime, endTime, 24); //should be 30 frames\
            var xxList = new List<object>();
            foreach (var time1 in timeSliceList)
            {
                xxList.Add(new { Time = time1, Height = height, Width = width });
            }

            var chartSvglist = new List<string>();
            await Parallel.ForEachAsync(xxList, Body);



            //foreach (var timesSlice in timeSliceList)
            //{

            //    //create unique id based on params to recognize future calls (caching)
            //    var callerId = $"{timesSlice.GetHashCode()}{width}{height}";

            //    Func<Task<string>> generateChart = () => SkyChartManager.GenerateChart(timesSlice, width, height);

            //    var chart = await APITools.CacheExecuteTask(generateChart, callerId);
            //    chartSvglist.Add(chart);



            //    //var xss = await GenerateChart(timesSlice, width, height);
            //    //chartSvglist.Add(xss);
            //}


            //STAGE 2: Convert SVG to PNG frames
            var pngFrameListByteTransparent = chartSvglist.Select(x => SvgConverter.Svg2Png(x, (int)width, (int)height)).ToList();
            var pngFrameLisWhite = pngFrameListByteTransparent.Select(x => TransparencyToWhite((Bitmap)x, ImageFormat.Png)).ToList();
            var pngFrameList = pngFrameLisWhite.Select(x => ByteArrayToImage(x)).ToList();

            //order correctly according to time
            //because parallel makes it mixed
            //var ordered = pngFrameList.OrderBy(x=>x)

            //STAGE 3: Make GIF from PNGs
            AnimatedGifEncoder e = new AnimatedGifEncoder();
            // read file as memorystream
            var memStream = new MemoryStream();
            e.Start(memStream);
            e.SetDelay(700);
            //-1:no repeat,0:always repeat
            e.SetRepeat(0);
            foreach (var pngFrame in pngFrameList)
            {
                e.AddFrame(pngFrame);
            }

            var x = e.Output();

            e.Finish();

            return x.ToArray();


            async ValueTask Body(dynamic dataObject, CancellationToken arg2)
            {
                //create unique id based on params to recognize future calls (caching)
                var callerId = $"{dataObject.Time.GetHashCode()}{dataObject.Width}{dataObject.Height}";

                Func<Task<string>> generateChart = () => SkyChartManager.GenerateChart(dataObject.Time, dataObject.Width, dataObject.Height);

                var chart = await APITools.CacheExecuteTask(generateChart, callerId);
                chartSvglist.Add(chart);
            }


        }



        //----------PRIVATE

        private static async Task<string?> GenerateZodiacRuler(double widthPx, int yAxis)
        {
            var svgFileUrl = $"{APITools.Url.WebUrl}/images/SkyChart/zodiac-360.svg";

            //no file return nothing
            //makes it ready to be injected into another SVG
            var svgIconHttp = await APITools.GetSvgIconHttp(svgFileUrl, 360, 30);

            var ratio = widthPx / 360;
            //var startOfStar = zodiacEvent.StartX / ratio; //position in final x where zodiac starts
            var final = $@"
                           <g transform=""scale({ratio}) translate(0,{yAxis})"">
                            {svgIconHttp}
                           </g>
                        ";

            return final;

        }


        private static string GenerateAngleRuler(double widthPx, int yAxis)
        {

            var barChartRow = "";
            var widthPerSlice = 1;
            var tempY = yAxis;
            for (int xAxis = 0; xAxis < widthPx; xAxis++)
            {
                tempY = yAxis;

                //get back actual angle
                double ratio = widthPx / 360;
                double angleDegree = (double)xAxis / ratio;

                //print for every even number
                var barHeight = 0;
                Int32 lastNumber = (int)(angleDegree % 10);
                Int32 lastNumber2 = (int)(xAxis % 10);
                var lastIsZero = lastNumber == 0 && lastNumber2 == 0;
                var lastIsFive = lastNumber2 == 5;
                //var secondLastIsZero = secondLastNumber == 0;
                if (lastIsZero) { barHeight = 20; }
                else if (lastIsFive) { barHeight = 10; tempY += 10; }
                else if (xAxis % 2 == 0) { barHeight = 5; tempY += 20; }

                //print if above 0
                if (barHeight > 0)
                {
                    string rect;

                    if (barHeight == 20)
                    {
                        //y axis placed here instead of parent group, so that auto legend can use the y axis
                        rect = $@"
                        <g>
                            <text x=""{xAxis - 3}"" y=""{5}"" style=""font-family: Arial, sans-serif; font-size: 6px; font-weight: 700;"" >{angleDegree}°</text>
                            <rect x=""{xAxis}"" y=""{tempY}"" width=""{widthPerSlice}"" height=""{barHeight}"" fill=""black""/>
                        </g>
                                ";

                    }
                    else
                    {
                        //double totalNatureScore = summarySlice.Value.NatureScore; //possible negative
                        rect = $"<rect " +
                               $"x=\"{xAxis}\" " +
                               $"y=\"{yAxis}\" " +
                               $"width=\"{widthPerSlice}\" " +
                               $"height=\"{barHeight}\" " +
                               $"fill=\"black\" />";

                    }


                    //add rect to row
                    barChartRow += rect;
                }

            }

            //note: chart is flipped 180, to start bar from bottom to top
            //default hidden
            var returnVal = $"<g id=\"AngleRuler\" transform=\"matrix(1, 0, 0, 1, 0, 0)\">{barChartRow}</g>";

            return returnVal;
        }

        public record ZodiacEvent(ZodiacName SignName, int StartX, int EndX)
        {
            public override string ToString()
            {
                return $"{{ SignName = {SignName}, StartX = {StartX}, EndX = {EndX} }}";
            }
        }

        private static string? GenerateHouseRuler(Time time, double widthPx, int yAxis)
        {
            //STAGE 1 : CREATE DATA
            var allPositions = AstronomicalCalculator.GetHouses(time);
            var hse1 = allPositions[0];

            //store entire row as names of signs to process after into icons (begin and end)
            int[] rowData = new int[(int)widthPx]; //represent degree as sign name 
            for (int xAxis = 0; xAxis < widthPx; xAxis++)
            {
                //get back actual angle
                var ratio = widthPx / 360;
                var angleDegree = xAxis / ratio;

                foreach (var house in allPositions)
                {
                    var inHouseRange = house.IsLongitudeInHouseRange(Angle.FromDegrees(angleDegree));
                    if (inHouseRange)
                    {
                        //add in house number into list
                        rowData[xAxis] = house.GetHouseNumber();
                        break; //once found stop looking
                    }
                }

            }


            //STAGE 2 : Process data
            //create compiled list of zodiac signs like events with start and end times
            var startPosition = 0;
            var endPosition = 0;
            int previousHouse = 0;
            var houseEventList = new List<HouseEvent>();
            for (int xAxis = 0; xAxis < widthPx; xAxis++)
            {
                var isLast = !((xAxis + 1) < widthPx);

                var currentSign = rowData[xAxis];

                if ((currentSign != previousHouse || isLast) && previousHouse != 0) //than new sign alert
                {
                    //save previous
                    endPosition = xAxis - 1;
                    var temp = new HouseEvent(previousHouse, startPosition, endPosition);
                    houseEventList.Add(temp);

                    //set new
                    startPosition = xAxis;
                }

                previousHouse = currentSign;
            }


            //STAGE 3 :
            var compiledEventSvg = "";
            foreach (var zodiacEvent in houseEventList)
            {
                //also set start and end on x axis
                compiledEventSvg += GetHouseIconSvg(zodiacEvent, yAxis, (int)widthPx);
            }


            //note: chart is flipped 180, to start bar from bottom to top
            //default hidden
            var fromTop = 20;
            var returnVal = $@"<g id=""ZodiacHouseRuler"" transform=""matrix(1, 0, 0, 1, 0, {yAxis})"" >{compiledEventSvg}</g>";


            return returnVal;
        }

        private static string GetHouseIconSvg(HouseEvent houseEvent, int yAxis, int widthPx)
        {
            var iconSvg = "";

            iconSvg = $@"

<svg version=""1.1""  xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" >
<style type=""text/css"">
	.st0{{fill:#FFFFFF;stroke:#000000;stroke-miterlimit:10;}}
	.st1{{font-family:'MyriadPro-Regular';}}
	.st2{{font-size:4.9451px;}}
</style>
<line class=""st0"" x1=""0.55"" y1=""0"" x2=""0.55"" y2=""10.19""/>
<text transform=""matrix(1 0 0 1 5.7812 6.8604)"" class=""st1 st2"">HOUSE {houseEvent.HouseNumber}</text>
<line class=""st0"" x1=""29.51"" y1=""0"" x2=""29.51"" y2=""10.19""/>
</svg>

          
";

            var ratio = widthPx / 360;
            var startOfStar = houseEvent.StartX / ratio; //position in final x where zodiac starts
            var final = $@"
                           <g transform=""scale({ratio}) translate({startOfStar},0)"">
                            {iconSvg}
                           </g>
                        ";

            return final;
        }


        public static Image ByteArrayToImage(byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

        /// <summary>
        /// Prepares the bitmap to be converted to jpg,
        /// by setting transparency fill color to white
        /// </summary>
        public static byte[] TransparencyToWhite(Bitmap input, ImageFormat outputFormat)
        {
            var stream = new MemoryStream();

            using (var b = new Bitmap(input.Width, input.Height))
            {
                //this will avoid some scaling issues during the conversion
                b.SetResolution(input.HorizontalResolution, input.VerticalResolution);

                using (var g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(input, 0, 0);
                }

                b.Save(stream, outputFormat); ;
            }

            return stream.ToArray();
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
            headerGenerator.Add(GenerateDecadeRowSvg);
            //if (difYears >= 10 && !showYearRow) { headerGenerator.Add(GenerateDecadeRowSvg); }
            //if (difYears is >= 5 and < 10) { headerGenerator.Add(Generate5YearRowSvg); }
            //if (showYearRow) { headerGenerator.Add(GenerateYearRowSvg); }
            //if (daysPerPixel <= 1.3) { headerGenerator.Add(GenerateMonthRowSvg); }
            //if (daysPerPixel <= 0.07) { headerGenerator.Add(GenerateDateRowSvg); }
            //if (daysPerPixel <= 0.001) { headerGenerator.Add(GenerateHourRowSvg); }

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

        private static async Task<string> GetAllPlanetLineIcons(List<PlanetLongitude> planetList, double widthPx, int iconStartYAxis)
        {
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var lineHeight = iconStartYAxis;//verticalYAxis + 6; //space between icon & last row
                                            //var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in


            var maxSlices = widthPx + 1;
            var rowList = new List<bool[]>();

            //space smaller than this is set as crowded
            const int minSpaceBetween = 100;//px
            var halfWidth = minSpaceBetween / 2; //icon


            //sort by earliest to latest event
            var incrementRate = 0; //for overcrowded jump
            var adjustedLineHeight = lineHeight; //keep copy for resetting after overcrowded jum

            var listRowData = new List<string>();
            foreach (var planet in planetList)
            {

                //get timezone at place event happened
                var positionX = (int)planet.GetPlanetLongitude().TotalDegrees;

                //if line is not in report time range, don't generate it
                if (positionX == 0) { continue; }

                //get row number, assign row number that is free to occupy
                var rowNumber = GetRowNumber(positionX); //start at 0 index

                //mark as occupied for future ref
                MarkRowNumber(positionX, rowNumber);

                //calculate final event icon height avoiding other icons 
                adjustedLineHeight += rowNumber * incrementRate;

                //put together icon + line + event data
                double input = widthPx;
                var confirmedOutput = 360.0;
                double yy = input / confirmedOutput;
                var transformedxAxis = positionX * yy;
                var generateLifeEventLine = await GetPlanetLineIcon(planet, adjustedLineHeight, transformedxAxis);

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
            var wrapperGroup = $"<g id=\"PlanetLinesHolder\" transform=\"matrix(1, 0, 0, 1, {contentPadding}, {contentPadding})\">{finalSvg}</g>";

            return wrapperGroup;


            //-------------------------


            //to remember which row is occupied, to implement icon jump
            void MarkRowNumber(int middleX, int rowNumber)
            {

                var startX = middleX - halfWidth;
                var endX = halfWidth + middleX;

                //set limits on width of chart
                startX = startX < 0 ? 0 : startX;
                endX = (int)(endX > (maxSlices - 1) ? (maxSlices - 1) : endX);


                for (int i = startX; i <= endX; i++)
                {
                    //mark as occupied
                    rowList[rowNumber][i] = true;
                }
            }

            //start at 0 index
            int GetRowNumber(int middleX)
            {
                var startX = middleX - halfWidth;
                var endX = halfWidth + middleX;

                //set limits
                startX = startX < 0 ? 0 : startX;
                endX = (int)(endX > (maxSlices - 1) ? (maxSlices - 1) : endX);

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
                rowList.Add(new bool[(int)maxSlices]);
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

        private static async Task<string> GetPlanetLineIcon(PlanetLongitude planet, int lineHeight, double positionX)
        {

            //based on length of event name make the background
            //mainly done to shorten background of short names (saving space)
            var planetName = planet.GetPlanetName().ToString();
            var backgroundWidth = APITools.GetTextWidthPx(planetName);

            var planetIcon = await GetPlanetIcon(planet.GetPlanetName());

            int iconYAxis = lineHeight; //start icon at end of line
            var iconXAxis = $"-{backgroundWidth / 2}"; //use negative to move center under main line
            var iconSvg = $@"
                                <rect class=""vertical-line"" fill=""#1E1EEA"" width=""2"" height=""{lineHeight}""></rect>
                                <!-- {planet.GetPlanetName().Name} ICON LABEL -->
                                <g transform=""translate({iconXAxis},{iconYAxis})"">
                                   {planetIcon}
                                </g>
                                ";

            //put together icon + line + event data
            var lifeEventLine = $@"<g id=""{planetName.ToLower()}-planet-icon""
                                          transform=""translate({positionX}, 0)"">{iconSvg}</g>";

            return lifeEventLine;

        }

        private static ConcurrentDictionary<PlanetName.PlanetNameEnum, string> PlanetIconMemoryCache = new ConcurrentDictionary<PlanetName.PlanetNameEnum, string>();

        /// <summary>
        /// creates a url to image that should exist in SkyChart folder, auto cached
        /// in memory cache for per instance only
        /// </summary>
        private static async Task<string> GetPlanetIcon(PlanetName planet)
        {
            string svgIconHttp = "";
            PlanetIconMemoryCache.TryGetValue(planet.Name, out svgIconHttp);

            //if no cache then get new
            if (string.IsNullOrEmpty(svgIconHttp))
            {
                var svgFileUrl = $"{APITools.Url.WebUrl}/images/SkyChart/{planet.Name.ToString().ToLower()}.svg";

                //makes it ready to be injected into another SVG
                svgIconHttp = await APITools.GetSvgIconHttp(svgFileUrl, 45, 45);

                //place in memory
                PlanetIconMemoryCache[planet.Name] = svgIconHttp;
            }
            else
            {
                Console.WriteLine("PLANET ICON CACHE USED!");
            }

            return svgIconHttp ?? "";

        }

        private static string GetColor(PlanetLongitude planet)
        {
            return "green";
        }


        //----------PRIVATE



    }

    internal record HouseEvent(int HouseNumber, int StartX, int EndX);
}
