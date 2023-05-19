using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VedAstro.Library;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Gif.Components;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace API
{
    /// <summary>
    /// Logic to create Sky Chart, simple chart with zodiac and planets in it
    /// </summary>
    public static class SkyChartManager
    {
        /// <summary>
        /// Sweet heart takes this away!
        /// Basically generating 1 frame
        /// </summary>
        public static string GenerateChart(Time time)
        {
            //PART I : declare the components
            string svgHead = null;
            string svgTail = null;
            string border = null;
            string contentTail = null;
            string content = null;
            string angleHeader = null;



            //PART II : fill the components in order
            GenerateComponents();



            //PART III : compile in right placement
            var final =
                $@" <!--MADE BY MACHINES FOR HUMAN EYES-->
                    {svgHead}
                        {angleHeader}
                        {content}
                    {svgTail}
                ";


            return final;




            //------------------------LOCALS NEEDED FOR REFS

            void GenerateComponents()
            {
                //STEP 1: USER INPUT > USABLE DATA
                var svgBackgroundColor = "#f0f9ff"; //not bleach white
                var randomId = Tools.GenerateId();

                //angleHeader = GenerateTimeHeaderRow();
                angleHeader = "";

                var planetList = AstronomicalCalculator.GetAllPlanetLongitude(time);

                var widthPx = 720.0;
                var heightPx = 300.0;
                content = GetAllPlanetLineIcons(planetList, widthPx);

                //note: if width & height not hard set, parent div clips it
                var svgTotalHeight = heightPx;//todo for now hard set, future use: verticalYAxis;
                var svgTotalWidth = widthPx;//todo for now hard set, future use: verticalYAxis;
                var svgStyle = $@"width:{svgTotalWidth}px;height:{svgTotalHeight}px;background:{svgBackgroundColor};";//end of style tag
                svgHead = $"<svg class=\"SkyChartHolder\" id=\"{randomId}\" style=\"{svgStyle}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">";//much needed for use tags to work

                svgTail = "</svg>";
                contentTail = "</g>";


            }

        }


        public static byte[] GenerateChartGif(Time time)
        {
            //STAGE 1: get charts as SVG list frames
            var startTime = time.SubtractHours(Tools.DaysToHours(15));
            var endTime = time.AddHours(Tools.DaysToHours(15));
            var timeList = EventManager.GetTimeListFromRange(startTime, endTime, 24); //should be 30 frames
            var chartSvglist = timeList.Select(x => GenerateChart(x)).ToList();


            //STAGE 2: Convert SVG to PNG frames
            var pngFrameListByteTransparent = chartSvglist.Select(x => SvgConverter.Svg2Png(x, 720, 420)).ToList();
            var pngFrameLisWhite = pngFrameListByteTransparent.Select(x => tansparencyToWhite((Bitmap)x, ImageFormat.Png)).ToList();
            var pngFrameList = pngFrameLisWhite.Select(x => byteArrayToImage(x)).ToList();


            //STAGE 3: Make GIF from PNGs
            /* create Gif */
            //you should replace filepath
            AnimatedGifEncoder e = new AnimatedGifEncoder();

            // read file as memorystream
            // byte[] fileBytes = File.ReadAllBytes(outputFilePath);
            var memStream = new MemoryStream();
            e.Start(memStream);
            e.SetDelay(500);
            //-1:no repeat,0:always repeat
            e.SetRepeat(0);
            foreach (var pngFrame in pngFrameList)
            {
                e.AddFrame(pngFrame);
            }

            var x = e.Output();

            e.Finish();

            return x.ToArray();
        }


        public static Image byteArrayToImage(byte[] bytesArr)
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
        private static byte[] tansparencyToWhite(Bitmap input, ImageFormat outputFormat)
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




        private static string GetAllPlanetLineIcons(List<PlanetLongitude> planetList, double widthPx)
        {
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var lineHeight = 100;//verticalYAxis + 6; //space between icon & last row
                                 //var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in


            var maxSlices = widthPx + 1;
            var rowList = new List<bool[]>();

            //space smaller than this is set as crowded
            const int minSpaceBetween = 100;//px
            var halfWidth = minSpaceBetween / 2; //icon


            //sort by earliest to latest event
            var incrementRate = 20; //for overcrowded jump
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
                var generateLifeEventLine = GetPlanetLineIcon(planet, adjustedLineHeight, transformedxAxis);

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


        private static string GetPlanetLineIcon(PlanetLongitude planet, int lineHeight, double positionX)
        {

            //based on length of event name make the background
            //mainly done to shorten background of short names (saving space)
            var planetName = planet.GetPlanetName().ToString();
            var backgroundWidth = APITools.GetTextWidthPx(planetName);


            int iconYAxis = lineHeight; //start icon at end of line
            var iconXAxis = $"-{backgroundWidth / 2}"; //use negative to move center under line
            var nameTextHeight = 15;
            var iconSvg = $@"
                                <rect class=""vertical-line"" fill=""#1E1EEA"" width=""2"" height=""{lineHeight}""></rect>
                                <!-- EVENT ICON LABEL -->
                                <g transform=""translate({iconXAxis},{iconYAxis})"">
                                    <!-- NAME -->
                                    <g class=""name-label"" >
                                        <g transform=""translate(15,0)"">
						                    <rect class=""background"" x=""0"" y=""0"" style=""fill: blue; opacity: 0.8;"" width=""{backgroundWidth}"" height=""{nameTextHeight}"" rx=""2"" ry=""2"" />
						                    <text class=""event-name"" x=""3"" y=""11"" style=""fill:#FFFFFF; font-family:'Calibri'; font-size:12px;"">{planetName}</text>
					                    </g>
				                        <g class=""nature-icon"" transform=""translate(8,8)"">
                                            <rect class=""background"" fill=""{GetColor(planet)}"" x=""-8"" y=""-8"" width=""15"" height=""15"" rx=""0"" ry=""0""/>
					                        <path d=""M2-0.2H0.7C0.5-0.2,0.3,0,0.3,0.2v1.3c0,0.2,0.2,0.4,0.4,0.4H2c0.2,0,0.4-0.2,0.4-0.4V0.2C2.5,0,2.3-0.2,2-0.2z M2-4.5v0.4 h-3.6v-0.4c0-0.2-0.2-0.4-0.4-0.4c-0.2,0-0.4,0.2-0.4,0.4v0.4h-0.4c-0.5,0-0.9,0.4-0.9,0.9v6.1c0,0.5,0.4,0.9,0.9,0.9h6.3 c0.5,0,0.9-0.4,0.9-0.9v-6.1c0-0.5-0.4-0.9-0.9-0.9H3.1v-0.4c0-0.2-0.2-0.4-0.4-0.4S2-4.8,2-4.5z M2.9,2.9h-5.4 c-0.2,0-0.4-0.2-0.4-0.4v-4.4h6.3v4.4C3.4,2.7,3.2,2.9,2.9,2.9z""/>
				                        </g>
                                    </g>
                                </g>
                                ";

            //put together icon + line + event data
            var lifeEventLine = $@"<g eventName=""{planetName}"" class=""PlanetLines"" 
                                          transform=""translate({positionX}, 0)"">{iconSvg}</g>";

            return lifeEventLine;

        }

        private static string GetColor(PlanetLongitude planet)
        {
            return "green";
        }


        //----------PRIVATE



    }
}
