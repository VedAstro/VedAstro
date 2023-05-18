using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// Logic to create Sky Chart, simple chart with zodiac and planets in it
    /// </summary>
    public static class SkyChartManager
    {
        /// <summary>
        /// Sweet heart takes this away!
        /// </summary>
        public static string GenerateChart(Time time)
        {
            //PART I : declare the components
            string svgHead = null;
            string svgTail = null;
            string border = null;
            string contentTail = null;
            string content = null;



            //PART II : fill the components in order

            GenerateComponents();



            //PART III : compile in right placement
            var final =
                $@" <!--MADE BY MACHINES FOR HUMAN EYES-->
                    {svgHead}
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

                var planetList = AstronomicalCalculator.GetAllPlanetLongitude(time);

                content = GetLifeEventLinesSvg(planetList);

                //note: if width & height not hard set, parent div clips it
                var svgTotalHeight = 350;//todo for now hard set, future use: verticalYAxis;
                var svgTotalWidth = 360;//todo for now hard set, future use: verticalYAxis;
                var svgStyle = $@"width:{svgTotalWidth}px;height:{svgTotalHeight}px;background:{svgBackgroundColor};";//end of style tag
                svgHead = $"<svg class=\"SkyChartHolder\" id=\"{randomId}\" style=\"{svgStyle}\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">";//much needed for use tags to work

                svgTail = "</svg>";
                contentTail = "</g>";


            }

        }




        private static string GetLifeEventLinesSvg(List<PlanetLongitude> planetList)
        {
            //use offset of input time, this makes sure life event lines
            //are placed on event chart correctly, since event chart is based on input offset
            var lineHeight = 100;//verticalYAxis + 6; //space between icon & last row
                                 //var inputOffset = startTime.GetStdDateTimeOffset().Offset; //timezone the chart will be in


            //add 1 to offset 0 index
            //each 1 cell is 1 px
            //var maxSlices = timeSlices.Count + 1;
            var maxSlices = 360 + 1;
            var rowList = new List<bool[]>();

            //space smaller than this is set as crowded
            const int minSpaceBetween = 100;//px
            var halfWidth = minSpaceBetween / 2; //icon


            //sort by earliest to latest event
            //var lifeEventList = person.LifeEventList;
            //lifeEventList.Sort((x, y) => x.CompareTo(y));

            var incrementRate = 20; //for overcrowded jump
            var adjustedLineHeight = lineHeight; //keep copy for resetting after overcrowded jum

            var listRowData = new List<string>();
            foreach (var planet in planetList)
            {

                //get timezone at place event happened
                //var lifeEvtTime = lifeEvent.GetDateTimeOffset();//time at the place of event with correct standard timezone
                //var startTimeInputOffset = lifeEvtTime.ToOffset(inputOffset); //change to output offset, to match chart
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
                var generateLifeEventLine = GetLine(planet, adjustedLineHeight, positionX);

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
                endX = endX > (maxSlices - 1) ? (maxSlices - 1) : endX;


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


        private static string GetLine(PlanetLongitude planet, int lineHeight, double positionX)
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
