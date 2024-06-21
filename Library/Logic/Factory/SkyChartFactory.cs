using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gif.Components;

namespace VedAstro.Library
{
	/// <summary>
	/// Logic to create Sky Chart, simple chart with zodiac and planets in it
	/// </summary>
	public static class SkyChartFactory
	{

		public static async Task<byte[]> GenerateChartGif(Time time, double width, double height)
		{
			//STAGE 1: get charts as SVG list frames
			var startTime = time.SubtractHours(Tools.DaysToHours(10));
			var endTime = time.AddHours(Tools.DaysToHours(10));
			var timeSliceList = Time.GetTimeListFromRange(startTime, endTime, 24); //should be 30 frames\
			var xxList = new List<object>();
			foreach (var time1 in timeSliceList)
			{
				xxList.Add(new { Time = time1, Height = height, Width = width });
			}
			var chartSvgList = new List<string>();
			foreach (var timesSlice in timeSliceList)
			{
				var xss = await SkyChartFactory.GenerateChart(timesSlice, width, height);
				chartSvgList.Add(xss);
			}


			//STAGE 2: Convert SVG to PNG frames
			var pngFrameListByteTransparent = chartSvgList.Select(x => Tools.Svg2Png(x, (int)width, (int)height)).ToList();
			var pngFrameLisWhite = pngFrameListByteTransparent.Select(x => TransparencyToWhite((Bitmap)x, ImageFormat.Png)).ToList();
			var pngFrameList = pngFrameLisWhite.Select(x => ByteArrayToImage(x)).ToList();


			//STAGE 3: Make GIF from PNGs
			AnimatedGifEncoder e = new AnimatedGifEncoder();
			var memStream = new MemoryStream();
			e.Start(memStream);
			e.SetDelay(500);
			//-1:no repeat,0:always repeat
			e.SetRepeat(0);
			//make gif frame by frame
			foreach (var pngFrame in pngFrameList) { e.AddFrame(pngFrame); }

			//take the gif out as binary data
			var x = e.Output();
			e.Finish();
			return x.ToArray();

			//--------------------------------

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


				var planetList = Calculate.AllPlanetLongitude(time);

				var renderWidth = widthPx - 30; // 750 -> 720
				angleRuler = GenerateAngleRuler(renderWidth, 10);

				zodiacRuler = await GenerateZodiacRuler(renderWidth, 15);

				houseRuler = GenerateHouseRuler(time, widthPx, 95);

				dateTimeLocation = GetDateTimeHeader(time);
				locationHeader = GetLocationHeader(time);

				border = GetBorderSvg((int)widthPx, (int)heightPx);

				content = await GetAllPlanetLineIcons(planetList, widthPx, 120, time); //130px from top is planet icon

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




		//----------PRIVATE

		private static async Task<string?> GenerateZodiacRuler(double widthPx, int yAxis)
		{
			var svgFileUrl = $"{URL.WebStable}/images/SkyChart/zodiac-360.svg";

			//no file return nothing
			//makes it ready to be injected into another SVG
			var svgIconHttp = await Tools.GetSvgIconHttp(svgFileUrl, 360, 30);

			var ratio = widthPx / 360;
			//var startOfStar = zodiacEvent.StartX / ratio; //position in final x where zodiac starts
			var final = $@"
                           <g id=""ZodiacRuler"" transform=""scale({ratio}) translate(0,{yAxis})"">
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
			var allPositions = Calculate.AllHouseMiddleLongitudes(time);
			var hse1 = allPositions[0];

			//store entire row as names of signs to process after into icons (begin and end)
			HouseName[] rowData = new HouseName[(int)widthPx]; //represent degree as sign name 
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
						rowData[xAxis] = house.GetHouseName();
						break; //once found stop looking
					}
				}

			}


			//STAGE 2 : Process data
			//create compiled list of zodiac signs like events with start and end times
			var startPosition = 0;
			var endPosition = 0;
			var previousHouse = HouseName.Empty;
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

<svg version=""1.1""  xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px""
	 width=""30px"" height=""8.9px"" viewBox=""0 0 30 8.9"" enable-background=""new 0 0 30 8.9"" xml:space=""preserve"">
<font horiz-adv-x=""2048"">
<!-- Gill Sans(R) is a trademark of The Monotype Corporation, Inc. which may be registered in certain jurisdictions. -->
<!-- Copyright: Copyright 2023 Adobe System Incorporated. All rights reserved. -->
<font-face font-family=""GillSansMT-Bold"" units-per-em=""2048"" underline-position=""-154"" underline-thickness=""102""/>
<missing-glyph horiz-adv-x=""2048"" d=""M256,0l0,1536l1536,0l0,-1536M384,128l1280,0l0,1280l-1280,0z""/>
<glyph unicode="" "" horiz-adv-x=""569""/>
<glyph unicode=""!"" horiz-adv-x=""555"" d=""M449,1194C449,1101 433,963 402,780C370,597 348,491 336,464C324,436 305,422 279,422C251,422 232,434 222,457C212,480 190,578 157,751C123,924 106,1072 106,1194C106,1341 164,1415 279,1415C392,1415 449,1341 449,1194M100,160C100,209 118,251 153,286C188,321 230,338 279,338C328,338 370,321 405,286C440,251 457,209 457,160C457,111 440,69 405,34C370,-1 328,-18 279,-18C230,-18 188,-1 153,34C118,69 100,111 100,160z""/>
<glyph unicode=""&quot;"" horiz-adv-x=""981"" d=""M823,944l-178,0l-49,275l0,247l276,0l0,-247M109,1466l276,0l0,-247l-49,-275l-178,0l-49,275z""/>
<glyph unicode=""#"" horiz-adv-x=""1196"" d=""M-12,1026l297,0l77,370l232,0l-78,-370l315,0l78,370l228,0l-78,-370l151,0l0,-224l-196,0l-56,-268l252,0l0,-223l-301,0l-78,-371l-227,0l76,371l-313,0l-76,-371l-234,0l78,371l-147,0l0,223l194,0l56,268l-250,0M469,802l-55,-268l313,0l55,268z""/>
<glyph unicode=""$"" horiz-adv-x=""1110"" d=""M956,1237l0,-252C858,1060 752,1105 637,1120l0,-332C762,749 859,692 927,617C994,541 1028,460 1028,373C1028,273 994,188 927,119C859,50 762,4 637,-18l0,-226l-172,0l0,226C342,1 222,48 106,123l0,242C231,270 350,212 465,193l0,393C216,644 92,774 92,977C92,1070 126,1151 193,1219C260,1287 350,1328 465,1341l0,74l172,0l0,-74C736,1337 842,1302 956,1237M465,850l0,270C374,1097 328,1054 328,989C328,926 374,879 465,850M637,532l0,-335C742,221 795,275 795,358C795,437 742,495 637,532z""/>
<glyph unicode=""%"" horiz-adv-x=""1473"" d=""M379,1415C470,1415 547,1383 612,1319C677,1255 709,1177 709,1085C709,995 677,918 612,853C547,788 469,756 379,756C288,756 211,788 146,853C81,918 49,995 49,1085C49,1177 81,1255 146,1319C210,1383 288,1415 379,1415M379,932C420,932 454,947 482,977C510,1006 524,1042 524,1085C524,1127 510,1163 481,1194C452,1224 418,1239 379,1239C338,1239 303,1224 275,1195C247,1166 233,1129 233,1085C233,1044 248,1009 277,978C306,947 340,932 379,932M1307,1415l-977,-1431l-170,0l983,1431M1092,641C1183,641 1260,609 1325,545C1389,480 1421,402 1421,311C1421,221 1389,144 1324,79C1259,14 1182,-18 1092,-18C1001,-18 924,14 859,79C794,144 762,221 762,311C762,402 794,480 859,545C923,609 1001,641 1092,641M1092,158C1133,158 1167,173 1195,203C1223,232 1237,268 1237,311C1237,353 1223,389 1194,420C1165,450 1131,465 1092,465C1051,465 1016,450 988,421C960,392 946,355 946,311C946,270 961,235 990,204C1019,173 1053,158 1092,158z""/>
<glyph unicode=""&amp;"" horiz-adv-x=""1536"" d=""M1188,725l299,0C1408,541 1314,402 1206,307l281,-307l-424,0l-102,111C826,25 679,-18 522,-18C395,-18 290,17 206,88C122,158 80,245 80,350C80,507 184,639 393,745C308,861 266,964 266,1055C266,1158 306,1244 387,1313C468,1381 570,1415 694,1415C813,1415 910,1384 985,1322C1060,1259 1098,1180 1098,1083C1098,947 1011,828 838,725l190,-219C1088,566 1141,639 1188,725M686,891C779,945 825,1004 825,1067C825,1101 813,1129 789,1151C765,1173 734,1184 696,1184C659,1184 628,1174 604,1154C579,1133 567,1108 567,1077C567,1024 607,962 686,891M799,291l-248,274C456,504 408,441 408,377C408,332 425,294 460,262C495,229 536,213 584,213C651,213 723,239 799,291z""/>
<glyph unicode=""'"" horiz-adv-x=""492"" d=""M336,944l-178,0l-49,275l0,247l276,0l0,-247z""/>
<glyph unicode=""("" horiz-adv-x=""788"" d=""M776,1415C533,1148 412,834 412,473C412,116 533,-198 776,-471l-211,0C417,-352 301,-208 218,-39C134,130 92,301 92,473C92,644 134,815 219,986C303,1156 418,1299 565,1415z""/>
<glyph unicode="")"" horiz-adv-x=""788"" d=""M12,-471C255,-197 377,118 377,473C377,834 255,1148 12,1415l211,0C370,1299 485,1156 570,986C654,815 696,644 696,473C696,302 654,132 571,-37C488,-206 372,-350 223,-471z""/>
<glyph unicode=""*"" horiz-adv-x=""961"" d=""M607,616C611,604 613,593 613,584C613,555 593,541 554,541l-125,0C386,541 365,555 365,582C365,592 367,603 371,616l91,291l-25,23l-203,-209C204,690 182,675 168,675C156,675 144,686 132,709l-57,108C66,834 62,848 62,861C62,886 76,901 105,907l312,68l0,4l-312,70C76,1056 61,1072 61,1096C61,1107 64,1119 70,1130l62,117C145,1271 156,1283 167,1283C180,1283 203,1267 234,1235l203,-209l25,23l-91,290C367,1352 365,1363 365,1372C365,1401 386,1415 429,1415l119,0C591,1415 613,1401 613,1373C613,1364 611,1352 607,1339l-90,-290l24,-23l203,209C775,1266 797,1282 808,1282C819,1282 832,1268 847,1241l51,-92C911,1125 917,1106 917,1091C917,1069 902,1055 873,1049l-311,-70l0,-4l311,-68C902,901 917,886 917,861C917,847 912,831 902,813l-53,-96C832,688 818,673 807,673C796,673 781,684 761,705l-220,225l-24,-23z""/>
<glyph unicode=""+"" horiz-adv-x=""1196"" d=""M86,592l0,262l381,0l0,381l262,0l0,-381l381,0l0,-262l-381,0l0,-381l-262,0l0,381z""/>
<glyph unicode="","" horiz-adv-x=""555"" d=""M102,-346l0,63C165,-222 197,-145 197,-52C197,-41 196,-29 195,-18C125,9 90,58 90,131C90,178 106,216 138,247C170,278 210,293 258,293C318,293 367,271 406,228C444,184 463,128 463,59C463,-40 429,-129 362,-208C294,-287 207,-333 102,-346z""/>
<glyph unicode=""-"" horiz-adv-x=""682"" d=""M608,610l0,-272l-534,0l0,272z""/>
<glyph unicode=""."" horiz-adv-x=""555"" d=""M98,160C98,209 116,251 151,286C186,321 227,338 276,338C325,338 368,321 403,286C438,251 455,209 455,160C455,111 438,69 403,34C368,-1 325,-18 276,-18C227,-18 186,-1 151,34C116,69 98,111 98,160z""/>
<glyph unicode=""/"" horiz-adv-x=""575"" d=""M0,-18l346,1433l211,0l-350,-1433z""/>
<glyph unicode=""0"" horiz-adv-x=""1130"" d=""M567,1415C715,1415 831,1351 914,1224C997,1097 1038,924 1038,705C1038,478 998,300 917,173C836,46 721,-18 571,-18C419,-18 301,45 217,172C132,299 90,474 90,698C90,925 133,1101 219,1227C304,1352 420,1415 567,1415M567,244C623,244 664,278 689,346C714,413 727,540 727,727C727,882 715,992 690,1057C665,1121 623,1153 565,1153C508,1153 467,1122 441,1059C414,996 401,871 401,684C401,527 414,415 441,347C467,278 509,244 567,244z""/>
<glyph unicode=""1"" horiz-adv-x=""1130"" d=""M715,1397l0,-1397l-309,0l0,1397z""/>
<glyph unicode=""2"" horiz-adv-x=""1130"" d=""M1034,285l0,-285l-923,0l0,39C196,142 293,273 402,432C511,590 579,702 608,769C637,836 651,900 651,963C651,1018 633,1065 597,1104C560,1143 516,1163 465,1163C363,1163 251,1105 129,989l0,293C266,1371 396,1415 520,1415C657,1415 766,1380 845,1309C924,1238 963,1141 963,1018C963,849 837,605 584,285z""/>
<glyph unicode=""3"" horiz-adv-x=""1130"" d=""M416,602l0,246C474,848 521,864 556,897C591,930 608,969 608,1014C608,1061 589,1099 552,1130C515,1161 467,1176 410,1176C332,1176 247,1153 154,1108l0,246C247,1395 353,1415 473,1415C606,1415 713,1383 796,1320C879,1256 920,1174 920,1073C920,934 851,826 713,750C876,686 958,566 958,391C958,267 913,168 823,94C733,19 612,-18 461,-18C338,-18 224,8 119,61l0,271C224,258 323,221 414,221C479,221 531,238 572,273C613,307 633,351 633,406C633,461 614,506 575,543C536,580 483,599 416,602z""/>
<glyph unicode=""4"" horiz-adv-x=""1130"" d=""M938,1415l0,-848l94,0l0,-223l-94,0l0,-344l-309,0l0,344l-545,0l0,223l657,848M330,567l299,0l0,381z""/>
<glyph unicode=""5"" horiz-adv-x=""1130"" d=""M928,1397l0,-281l-447,0l0,-260C630,866 750,829 840,745C930,661 975,555 975,426C975,291 929,184 838,103C746,22 623,-18 469,-18C334,-18 221,7 131,57l0,254C238,251 337,221 428,221C492,221 545,240 587,277C628,314 649,360 649,416C649,478 624,527 573,563C522,599 452,617 362,617C313,617 260,611 203,600l0,797z""/>
<glyph unicode=""6"" horiz-adv-x=""1130"" d=""M866,1415l-323,-528C574,895 605,899 636,899C741,899 830,859 905,778C979,697 1016,591 1016,459C1016,322 972,209 884,118C796,27 686,-18 553,-18C416,-18 305,28 219,120C133,211 90,330 90,477C90,636 154,819 281,1024l241,391M559,244C607,244 645,263 674,300C703,337 717,387 717,451C717,514 702,564 673,602C644,640 605,659 557,659C509,659 471,640 443,602C415,564 401,512 401,446C401,382 415,332 443,297C471,262 510,244 559,244z""/>
<glyph unicode=""7"" horiz-adv-x=""1130"" d=""M272,88l375,1030l-516,0l0,279l924,0l0,-17l-504,-1398z""/>
<glyph unicode=""8"" horiz-adv-x=""1130"" d=""M309,737C194,809 137,909 137,1036C137,1149 176,1241 254,1311C332,1380 434,1415 561,1415C694,1415 800,1378 878,1304C956,1229 995,1139 995,1034C995,904 938,805 825,737C964,665 1034,552 1034,399C1034,274 991,173 906,97C820,20 706,-18 563,-18C426,-18 314,21 228,98C141,175 98,275 98,397C98,559 168,672 309,737M569,823C609,823 640,841 663,877C685,912 696,961 696,1024C696,1148 652,1210 565,1210C526,1210 494,1193 471,1160C448,1126 436,1079 436,1020C436,959 448,911 472,876C496,841 528,823 569,823M569,186C615,186 652,206 681,245C709,284 723,336 723,401C723,470 709,524 681,565C652,605 614,625 567,625C520,625 482,606 453,567C424,528 410,475 410,410C410,342 425,288 454,247C483,206 522,186 569,186z""/>
<glyph unicode=""9"" horiz-adv-x=""1130"" d=""M256,-18l322,524C546,497 514,493 483,493C379,493 290,536 215,622C140,707 102,811 102,932C102,1069 147,1183 236,1276C325,1369 436,1415 567,1415C699,1415 809,1369 897,1276C984,1183 1028,1067 1028,928C1028,763 967,580 844,381l-246,-399M561,731C606,731 643,751 673,790C702,829 717,879 717,940C717,1006 703,1058 675,1096C647,1134 609,1153 561,1153C515,1153 477,1133 447,1094C416,1054 401,1003 401,942C401,879 416,829 446,790C475,751 514,731 561,731z""/>
<glyph unicode="":"" horiz-adv-x=""555"" d=""M98,766C98,815 116,857 151,892C186,927 227,944 276,944C325,944 368,927 403,892C438,857 455,815 455,766C455,717 438,676 403,641C368,606 325,588 276,588C227,588 186,606 151,641C116,676 98,717 98,766M98,160C98,209 116,251 151,286C186,321 227,338 276,338C325,338 368,321 403,286C438,251 455,209 455,160C455,111 438,69 403,34C368,-1 325,-18 276,-18C227,-18 186,-1 151,34C116,69 98,111 98,160z""/>
<glyph unicode="";"" horiz-adv-x=""555"" d=""M98,766C98,815 116,857 151,892C186,927 227,944 276,944C325,944 368,927 403,892C438,857 455,815 455,766C455,717 438,676 403,641C368,606 325,588 276,588C227,588 186,606 151,641C116,676 98,717 98,766M102,-346l0,63C165,-222 197,-145 197,-52C197,-41 196,-29 195,-18C125,9 90,58 90,131C90,178 106,216 138,247C170,278 210,293 258,293C318,293 367,271 406,228C444,184 463,128 463,59C463,-40 429,-129 362,-208C294,-287 207,-333 102,-346z""/>
<glyph unicode=""&lt;"" horiz-adv-x=""1196"" d=""M96,604l0,246l1006,436l0,-280l-703,-279l703,-278l0,-281z""/>
<glyph unicode=""="" horiz-adv-x=""1196"" d=""M86,815l0,262l1024,0l0,-262M86,369l0,262l1024,0l0,-262z""/>
<glyph unicode=""&gt;"" horiz-adv-x=""1196"" d=""M1100,604l-1006,-436l0,281l703,278l-703,279l0,280l1006,-436z""/>
<glyph unicode=""?"" horiz-adv-x=""768"" d=""M51,1122l51,252C181,1401 260,1415 340,1415C444,1415 526,1389 587,1337C648,1285 678,1215 678,1128C678,1078 669,1026 650,973C631,919 589,851 524,770C459,688 419,628 404,589C389,550 381,512 381,477l0,-24C381,438 377,429 369,426C360,422 343,420 317,420C292,420 275,425 265,436C254,447 242,476 228,523C214,570 207,612 207,649C207,724 238,815 301,922C345,997 367,1053 367,1090C367,1159 327,1194 246,1194C185,1194 127,1170 70,1122M145,160C145,209 163,251 198,286C233,321 275,338 324,338C373,338 415,321 450,286C485,251 502,209 502,160C502,111 485,69 450,34C415,-1 373,-18 324,-18C275,-18 233,-1 198,34C163,69 145,111 145,160z""/>
<glyph unicode=""@"" horiz-adv-x=""2005"" d=""M1778,18l213,0C1918,-128 1805,-239 1654,-315C1503,-392 1322,-430 1112,-430C815,-430 566,-351 364,-192C162,-33 61,188 61,471C61,752 151,993 332,1192C512,1391 764,1491 1087,1491C1339,1491 1544,1420 1701,1279C1858,1137 1937,953 1937,727C1937,535 1874,366 1747,220C1620,73 1463,0 1276,0C1173,0 1112,38 1094,115C1017,38 927,0 823,0C710,0 618,39 546,117C474,195 438,296 438,420C438,587 489,739 590,877C691,1014 819,1083 975,1083C1096,1083 1185,1038 1243,948l25,111l264,0l-160,-758C1369,284 1367,269 1367,256C1367,222 1380,205 1405,205C1468,205 1542,259 1628,366C1714,473 1757,594 1757,731C1757,896 1698,1034 1579,1145C1460,1256 1291,1311 1071,1311C809,1311 605,1230 460,1067C315,904 242,706 242,473C242,248 320,71 477,-57C633,-186 836,-250 1087,-250C1406,-250 1637,-161 1778,18M1001,903C911,903 838,854 781,755C724,656 696,543 696,416C696,345 713,289 746,246C779,203 823,182 877,182C972,182 1047,236 1104,345C1160,454 1188,562 1188,670C1188,741 1171,798 1137,840C1103,882 1058,903 1001,903z""/>
<glyph unicode=""A"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543z""/>
<glyph unicode=""B"" horiz-adv-x=""1430"" d=""M156,1397l604,0C904,1397 1017,1362 1100,1293C1182,1224 1223,1142 1223,1047C1223,926 1159,833 1032,768C1128,743 1202,698 1253,634C1304,570 1329,495 1329,410C1329,289 1287,190 1204,114C1121,38 998,0 836,0l-680,0M504,1141l0,-299l182,0C742,842 786,856 817,885C848,914 864,950 864,993C864,1036 848,1071 817,1099C786,1127 742,1141 686,1141M504,586l0,-330l196,0C793,256 859,269 900,295C941,320 961,365 961,428C961,477 942,515 903,544C864,572 813,586 748,586z""/>
<glyph unicode=""C"" horiz-adv-x=""1579"" d=""M1458,426l0,-305C1283,28 1100,-18 909,-18C661,-18 464,51 317,188C170,325 96,493 96,694C96,895 174,1065 329,1205C484,1345 686,1415 936,1415C1135,1415 1304,1374 1442,1292l0,-311C1273,1076 1112,1124 961,1124C812,1124 691,1084 597,1005C502,925 455,823 455,700C455,576 502,474 595,393C688,312 807,272 952,272C1024,272 1093,282 1160,301C1227,320 1326,361 1458,426z""/>
<glyph unicode=""D"" horiz-adv-x=""1642"" d=""M154,1397l616,0C1008,1397 1196,1332 1335,1202C1473,1071 1542,903 1542,696C1542,479 1472,309 1332,186C1191,62 991,0 731,0l-577,0M502,1141l0,-885l227,0C876,256 989,297 1067,378C1145,459 1184,565 1184,698C1184,835 1145,944 1066,1023C987,1102 873,1141 725,1141z""/>
<glyph unicode=""E"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336z""/>
<glyph unicode=""F"" horiz-adv-x=""1237"" d=""M1118,1397l0,-256l-614,0l0,-312l614,0l0,-256l-614,0l0,-573l-348,0l0,1397z""/>
<glyph unicode=""G"" horiz-adv-x=""1665"" d=""M1489,1300l0,-305C1311,1081 1137,1124 967,1124C803,1124 677,1083 588,1000C499,917 455,812 455,684C455,561 499,462 588,386C677,310 793,272 938,272C1015,272 1092,289 1169,322l0,163l-202,0l0,256l551,0l0,-571C1463,115 1379,70 1266,35C1153,0 1041,-18 930,-18C687,-18 487,49 331,184C174,318 96,489 96,696C96,908 174,1081 331,1215C487,1348 689,1415 938,1415C1143,1415 1327,1377 1489,1300z""/>
<glyph unicode=""H"" horiz-adv-x=""1706"" d=""M1550,1397l0,-1397l-348,0l0,565l-696,0l0,-565l-348,0l0,1397l348,0l0,-561l696,0l0,561z""/>
<glyph unicode=""I"" horiz-adv-x=""682"" d=""M516,1397l0,-1397l-348,0l0,1397z""/>
<glyph unicode=""J"" horiz-adv-x=""682"" d=""M530,1397l0,-1268C530,-229 365,-408 36,-408C5,-408 -29,-406 -66,-403l0,274C-47,-132 -30,-134 -13,-134C117,-134 182,-48 182,123l0,1274z""/>
<glyph unicode=""K"" horiz-adv-x=""1473"" d=""M1329,1397l-455,-656l617,-741l-436,0l-553,655l0,-655l-348,0l0,1397l348,0l0,-629l444,629z""/>
<glyph unicode=""L"" horiz-adv-x=""1260"" d=""M1221,256l0,-256l-1065,0l0,1397l348,0l0,-1141z""/>
<glyph unicode=""M"" horiz-adv-x=""1812"" d=""M907,879l449,518l303,0l0,-1397l-348,0l0,872l-377,-438l-53,0l-377,438l0,-872l-348,0l0,1397l303,0z""/>
<glyph unicode=""N"" horiz-adv-x=""1729"" d=""M1571,1397l0,-1397l-293,0l-772,895l0,-895l-348,0l0,1397l319,0l746,-854l0,854z""/>
<glyph unicode=""O"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272z""/>
<glyph unicode=""P"" horiz-adv-x=""1343"" d=""M170,1397l623,0C934,1397 1047,1356 1130,1274C1213,1192 1255,1095 1255,983C1255,867 1214,768 1132,685C1049,602 937,561 795,561l-277,0l0,-561l-348,0M518,1141l0,-324l162,0C825,817 897,871 897,979C897,1040 878,1082 839,1106C800,1129 734,1141 641,1141z""/>
<glyph unicode=""Q"" horiz-adv-x=""1792"" d=""M1694,-61l22,-29l-272,-232C1369,-317 1292,-302 1212,-279C1137,-257 1060,-228 983,-191C910,-156 837,-114 765,-67C712,-32 648,-1 575,26C514,48 456,79 400,119C342,160 291,209 247,265C200,324 163,388 138,457C110,532 96,610 96,690C96,883 172,1053 324,1198C475,1343 668,1415 901,1415C1138,1415 1330,1343 1477,1198C1624,1053 1698,888 1698,702C1698,551 1651,413 1558,287C1464,161 1337,73 1178,23C1299,-46 1426,-81 1558,-81C1609,-81 1655,-74 1694,-61M897,1124C764,1124 658,1085 577,1006C496,927 455,823 455,694C455,566 496,463 577,384C658,305 764,266 897,266C1028,266 1134,305 1216,383C1298,460 1339,564 1339,694C1339,825 1298,930 1215,1008C1132,1085 1026,1124 897,1124z""/>
<glyph unicode=""R"" horiz-adv-x=""1386"" d=""M156,1397l624,0C915,1397 1025,1358 1110,1280C1195,1201 1237,1106 1237,995C1237,841 1157,727 997,653C1068,622 1135,536 1198,396C1261,256 1321,124 1380,0l-383,0C974,46 935,134 879,263C822,392 774,475 734,512C693,549 650,567 604,567l-100,0l0,-567l-348,0M504,1141l0,-318l184,0C747,823 793,837 828,865C862,892 879,932 879,983C879,1088 813,1141 680,1141z""/>
<glyph unicode=""S"" horiz-adv-x=""1237"" d=""M1104,1307l0,-273C919,1117 771,1159 661,1159C598,1159 549,1149 512,1128C475,1107 456,1079 456,1044C456,1017 469,992 496,968C523,944 588,913 691,875C794,837 877,802 939,769C1001,736 1055,690 1100,631C1145,572 1167,497 1167,406C1167,277 1118,174 1020,97C921,20 792,-18 631,-18C458,-18 290,27 127,117l0,301C220,355 303,310 375,281C447,252 523,238 604,238C741,238 809,281 809,367C809,396 795,424 768,450C741,476 675,508 571,545C466,582 384,617 323,649C262,681 209,727 165,786C120,845 98,921 98,1014C98,1134 146,1231 243,1305C340,1378 469,1415 631,1415C780,1415 938,1379 1104,1307z""/>
<glyph unicode=""T"" horiz-adv-x=""1473"" d=""M1417,1397l0,-256l-506,0l0,-1141l-348,0l0,1141l-506,0l0,256z""/>
<glyph unicode=""U"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618z""/>
<glyph unicode=""V"" horiz-adv-x=""1473"" d=""M1473,1397l-646,-1415l-151,0l-676,1415l379,0l369,-797l350,797z""/>
<glyph unicode=""W"" horiz-adv-x=""2390"" d=""M1194,893l-432,-911l-150,0l-614,1415l369,0l329,-772l359,772l280,0l359,-772l327,772l369,0l-612,-1415l-150,0z""/>
<glyph unicode=""X"" horiz-adv-x=""1665"" d=""M1595,1397l-546,-674l606,-723l-451,0l-379,461l-372,-461l-445,0l592,723l-559,674l451,0l333,-412l328,412z""/>
<glyph unicode=""Y"" horiz-adv-x=""1450"" d=""M551,0l0,653l-551,744l422,0l303,-439l305,439l420,0l-551,-744l0,-653z""/>
<glyph unicode=""Z"" horiz-adv-x=""1430"" d=""M1364,1397l-762,-1141l762,0l0,-256l-1356,0l768,1141l-715,0l0,256z""/>
<glyph unicode=""["" horiz-adv-x=""897"" d=""M858,1397l0,-223l-389,0l0,-1403l389,0l0,-224l-686,0l0,1850z""/>
<glyph unicode=""\"" horiz-adv-x=""575"" d=""M571,-18l-206,0l-351,1433l211,0z""/>
<glyph unicode=""]"" horiz-adv-x=""897"" d=""M39,-453l0,224l389,0l0,1403l-389,0l0,223l686,0l0,-1850z""/>
<glyph unicode=""^"" horiz-adv-x=""1196"" d=""M115,598l377,799l221,0l366,-799l-284,0l-197,489l-197,-489z""/>
<glyph unicode=""_"" horiz-adv-x=""1024"" d=""M-12,-154l1048,0l0,-102l-1048,0z""/>
<glyph unicode=""`"" horiz-adv-x=""682"" d=""M16,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""a"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229z""/>
<glyph unicode=""b"" horiz-adv-x=""1194"" d=""M416,1397l0,-506C494,939 575,963 659,963C795,963 906,918 992,827C1077,736 1120,619 1120,475C1120,330 1079,212 997,120C914,28 809,-18 680,-18C587,-18 499,12 416,72l0,-72l-297,0l0,1397M416,698l0,-438C476,207 535,180 592,180C663,180 718,207 759,260C799,313 819,386 819,479C819,570 798,640 756,690C713,739 657,764 588,764C534,764 477,742 416,698z""/>
<glyph unicode=""c"" horiz-adv-x=""1024"" d=""M948,258l0,-213C832,3 726,-18 629,-18C463,-18 330,27 231,116C132,205 82,322 82,469C82,612 132,731 233,824C333,917 461,963 616,963C716,963 822,942 934,901l0,-221C852,722 771,743 692,743C603,743 532,718 479,669C426,620 399,554 399,471C399,388 425,321 477,270C529,219 597,193 682,193C745,193 833,215 948,258z""/>
<glyph unicode=""d"" horiz-adv-x=""1194"" d=""M1073,1397l0,-1397l-297,0l0,76C700,13 613,-18 514,-18C388,-18 283,29 199,122C114,215 72,332 72,473C72,617 115,735 201,826C286,917 396,963 530,963C613,963 695,943 776,903l0,494M776,258l0,451C722,746 667,764 610,764C535,764 477,738 436,686C394,634 373,561 373,467C373,380 394,311 435,259C476,206 531,180 600,180C661,180 719,206 776,258z""/>
<glyph unicode=""e"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580z""/>
<glyph unicode=""f"" horiz-adv-x=""618"" d=""M463,944l135,0l0,-203l-135,0l0,-741l-297,0l0,741l-135,0l0,203l135,0l0,47C166,1124 203,1228 278,1303C353,1378 453,1415 580,1415C611,1415 671,1409 760,1397l0,-240C698,1165 654,1169 627,1169C518,1169 463,1114 463,1004z""/>
<glyph unicode=""g"" horiz-adv-x=""1110"" d=""M1085,944l0,-199l-135,0C981,692 997,639 997,588C997,515 970,447 915,385C860,323 780,286 673,275C566,263 505,252 491,242C476,231 469,218 469,201C469,184 478,171 497,161C516,150 589,140 718,129C846,118 935,84 986,28C1036,-29 1061,-89 1061,-154C1061,-257 1018,-336 933,-390C847,-444 723,-471 561,-471C396,-471 270,-446 185,-396C100,-346 57,-280 57,-197C57,-82 141,-2 309,43C233,66 195,107 195,166C195,235 252,283 367,309C290,322 227,356 177,409C127,462 102,526 102,600C102,691 138,771 210,840C282,909 387,944 524,944M551,768C502,768 462,751 429,718C396,685 379,644 379,596C379,550 396,510 430,477C464,443 504,426 551,426C598,426 638,444 671,479C704,514 721,557 721,606C721,651 705,689 673,721C640,752 600,768 551,768M575,-295C638,-295 690,-283 729,-260C768,-237 788,-209 788,-176C788,-107 711,-72 557,-72C487,-72 432,-82 392,-102C352,-123 332,-150 332,-184C332,-258 413,-295 575,-295z""/>
<glyph unicode=""h"" horiz-adv-x=""1194"" d=""M424,1397l0,-592l4,0C511,910 610,963 727,963C828,963 911,931 976,868C1041,804 1073,715 1073,602l0,-602l-297,0l0,539C776,676 725,745 623,745C556,745 489,697 424,602l0,-602l-297,0l0,1397z""/>
<glyph unicode=""i"" horiz-adv-x=""555"" d=""M113,1251C113,1296 129,1335 161,1367C193,1399 231,1415 276,1415C321,1415 360,1399 392,1367C424,1335 440,1296 440,1251C440,1206 424,1168 392,1136C360,1103 321,1087 276,1087C231,1087 193,1103 161,1136C129,1168 113,1206 113,1251M426,944l0,-944l-297,0l0,944z""/>
<glyph unicode=""j"" horiz-adv-x=""555"" d=""M119,1251C119,1296 135,1335 167,1367C199,1399 238,1415 283,1415C328,1415 366,1399 398,1367C430,1335 446,1296 446,1251C446,1206 430,1168 398,1136C366,1103 328,1087 283,1087C238,1087 199,1103 167,1136C135,1168 119,1206 119,1251M432,944l0,-971C432,-156 400,-260 336,-337C271,-414 185,-459 76,-471l-94,268C84,-188 135,-105 135,45l0,899z""/>
<glyph unicode=""k"" horiz-adv-x=""1130"" d=""M426,1397l0,-920l4,0l367,467l348,0l-369,-467l385,-477l-364,0l-367,465l-4,0l0,-465l-297,0l0,1397z""/>
<glyph unicode=""l"" horiz-adv-x=""555"" d=""M426,1397l0,-1397l-297,0l0,1397z""/>
<glyph unicode=""m"" horiz-adv-x=""1962"" d=""M426,944l0,-121C527,916 636,963 754,963C916,963 1029,896 1094,762C1208,896 1332,963 1466,963C1575,963 1663,930 1732,864C1801,798 1835,712 1835,606l0,-606l-297,0l0,553C1538,620 1523,672 1492,709C1461,746 1419,764 1364,764C1277,764 1199,711 1130,604l0,-604l-296,0l0,559C834,625 818,676 785,711C752,746 708,764 653,764C575,764 499,720 426,633l0,-633l-297,0l0,944z""/>
<glyph unicode=""n"" horiz-adv-x=""1194"" d=""M420,944l0,-135C507,912 610,963 727,963C825,963 907,932 973,869C1038,806 1071,717 1071,600l0,-600l-297,0l0,575C774,701 725,764 627,764C557,764 488,712 420,608l0,-608l-297,0l0,944z""/>
<glyph unicode=""o"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158z""/>
<glyph unicode=""p"" horiz-adv-x=""1194"" d=""M414,944l0,-82C493,929 583,963 684,963C807,963 910,917 995,824C1080,731 1122,618 1122,483C1122,332 1080,210 995,119C910,28 797,-18 657,-18C570,-18 489,4 414,47l0,-518l-297,0l0,1415M414,676l0,-434C470,201 527,180 586,180C658,180 715,206 758,258C800,309 821,379 821,467C821,557 802,627 763,677C724,727 669,752 598,752C539,752 477,727 414,676z""/>
<glyph unicode=""q"" horiz-adv-x=""1194"" d=""M778,944l297,0l0,-1415l-297,0l0,518C705,4 624,-18 535,-18C396,-18 285,28 200,120C115,212 72,333 72,483C72,620 114,734 198,826C281,917 385,963 510,963C611,963 700,929 778,862M778,242l0,434C717,727 655,752 594,752C524,752 470,727 431,678C392,628 373,558 373,467C373,378 394,308 436,257C477,206 534,180 606,180C665,180 723,201 778,242z""/>
<glyph unicode=""r"" horiz-adv-x=""918"" d=""M428,944l0,-270l4,0C518,867 618,963 733,963C788,963 853,934 930,877l-82,-263C775,661 716,684 672,684C603,684 546,642 499,557C452,472 428,422 428,408l0,-408l-297,0l0,944z""/>
<glyph unicode=""s"" horiz-adv-x=""874"" d=""M772,893l0,-211C659,751 554,786 457,786C386,786 350,762 350,713C350,700 358,688 373,676C388,663 445,637 545,598C645,558 716,512 757,460C798,408 819,351 819,289C819,191 786,115 719,62C652,9 557,-18 434,-18C307,-18 195,5 96,51l0,209C217,207 319,180 401,180C497,180 545,203 545,250C545,268 536,285 519,301C501,317 441,345 340,384C239,423 169,467 132,514C95,561 76,614 76,672C76,758 112,828 184,882C255,936 350,963 467,963C580,963 681,940 772,893z""/>
<glyph unicode=""t"" horiz-adv-x=""831"" d=""M801,944l0,-203l-344,0l0,-389C457,305 470,269 496,242C521,215 557,201 602,201C663,201 730,219 801,256l0,-215C706,2 615,-18 526,-18C410,-18 320,12 256,72C192,132 160,220 160,336l0,405l-135,0l0,58l395,442l37,0l0,-297z""/>
<glyph unicode=""u"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944z""/>
<glyph unicode=""v"" horiz-adv-x=""1044"" d=""M440,-18l-436,962l318,0l200,-520l203,520l315,0l-434,-962z""/>
<glyph unicode=""w"" horiz-adv-x=""1599"" d=""M801,496l-238,-514l-139,0l-426,962l303,0l199,-520l211,520l180,0l209,-520l198,520l304,0l-424,-962l-142,0z""/>
<glyph unicode=""x"" horiz-adv-x=""1130"" d=""M1096,944l-373,-461l401,-483l-344,0l-223,293l-215,-293l-334,0l385,483l-385,461l346,0l203,-272l193,272z""/>
<glyph unicode=""y"" horiz-adv-x=""1044"" d=""M1042,944l-632,-1397l-316,0l277,607l-369,790l317,0l199,-485l213,485z""/>
<glyph unicode=""z"" horiz-adv-x=""1067"" d=""M86,944l922,0l-484,-741l484,0l0,-203l-969,0l485,741l-438,0z""/>
<glyph unicode=""{{"" horiz-adv-x=""788"" d=""M59,643C133,643 188,666 225,711C261,756 279,835 279,948l0,162C279,1239 309,1333 368,1393C427,1453 522,1483 653,1483l90,0l0,-240l-12,0C641,1243 586,1232 567,1209C547,1186 537,1121 537,1016C537,881 527,780 507,711C487,642 439,580 362,526C439,473 487,412 507,343C527,274 537,172 537,37C537,-68 547,-132 567,-155C586,-178 641,-190 731,-190l12,0l0,-240l-90,0C522,-430 427,-400 368,-340C309,-280 279,-186 279,-57l0,161C279,218 261,297 225,342C188,387 133,409 59,410z""/>
<glyph unicode=""|"" horiz-adv-x=""575"" d=""M176,-453l0,1850l223,0l0,-1850z""/>
<glyph unicode=""}}"" horiz-adv-x=""788"" d=""M729,410C655,409 600,387 564,342C528,297 510,218 510,104l0,-161C510,-186 481,-280 422,-340C363,-400 267,-430 135,-430l-90,0l0,240l12,0C148,-190 203,-178 223,-155C242,-132 252,-68 252,37C252,172 262,274 282,343C302,412 350,473 426,526C350,580 302,642 282,711C262,780 252,881 252,1016C252,1121 242,1186 223,1209C203,1232 148,1243 57,1243l-12,0l0,240l90,0C267,1483 363,1453 422,1393C481,1333 510,1239 510,1110l0,-162C510,835 528,756 564,711C600,666 655,643 729,643z""/>
<glyph unicode=""~"" horiz-adv-x=""1196"" d=""M33,358l0,269C151,725 260,774 360,774C421,774 507,747 619,694C731,641 812,614 862,614C945,614 1045,665 1161,766l0,-266C1118,457 1065,422 1004,394C943,366 887,352 838,352C779,352 691,379 576,433C461,487 383,514 344,514C259,514 155,462 33,358z""/>
<glyph unicode=""&#xC4;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M1038,1858C1084,1858 1123,1842 1155,1811C1186,1779 1202,1740 1202,1695C1202,1649 1186,1610 1155,1578C1124,1546 1085,1530 1038,1530C993,1530 954,1546 923,1578C891,1610 875,1649 875,1695C875,1740 891,1779 923,1811C954,1842 993,1858 1038,1858M561,1858C607,1859 646,1843 678,1811C709,1779 725,1740 725,1695C725,1649 709,1610 678,1578C647,1546 608,1530 561,1530C516,1530 477,1546 446,1578C414,1610 398,1649 398,1695C398,1740 414,1779 446,1811C477,1842 516,1858 561,1858z""/>
<glyph unicode=""&#xC5;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397l26,0C573,1430 562,1468 562,1510C562,1567 583,1616 624,1657C665,1697 714,1717 771,1717C828,1717 876,1697 917,1656C958,1615 978,1567 978,1510C978,1468 967,1430 945,1397M771,1620C740,1620 713,1610 692,1589C671,1568 660,1541 660,1510C660,1479 671,1452 693,1430C714,1408 740,1397 771,1397C802,1397 829,1408 851,1430C872,1452 883,1479 883,1510C883,1541 872,1567 850,1588C828,1609 802,1620 771,1620M1001,520l-225,543l-205,-543z""/>
<glyph unicode=""&#xC7;"" horiz-adv-x=""1579"" d=""M1458,426l0,-305C1273,23 1078,-23 875,-17l-12,-30C948,-78 990,-128 990,-197C990,-251 970,-293 929,-322C888,-352 833,-367 766,-367C700,-367 650,-361 615,-348l0,104C718,-247 770,-226 770,-180C770,-142 742,-123 686,-123l66,115C557,18 399,96 278,226C157,356 96,512 96,694C96,909 177,1083 339,1216C501,1349 700,1415 936,1415C1135,1415 1304,1374 1442,1292l0,-311C1273,1076 1112,1124 961,1124C817,1124 697,1085 600,1008C503,931 455,828 455,700C455,572 503,469 598,390C693,311 811,272 952,272C1088,272 1257,323 1458,426z""/>
<glyph unicode=""&#xC9;"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336M977,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xD1;"" horiz-adv-x=""1729"" d=""M1571,1397l0,-1397l-293,0l-772,895l0,-895l-348,0l0,1397l319,0l746,-854l0,854M1125,1821l139,0C1250,1615 1170,1512 1025,1512C983,1512 924,1531 848,1568C772,1605 721,1623 695,1623C643,1623 612,1587 603,1516l-135,0C475,1614 501,1689 544,1742C587,1795 643,1821 711,1821C755,1821 813,1803 886,1766C958,1729 1008,1711 1035,1711C1083,1711 1113,1748 1125,1821z""/>
<glyph unicode=""&#xD6;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M1134,1858C1180,1858 1219,1842 1251,1811C1282,1779 1298,1740 1298,1695C1298,1649 1282,1610 1251,1578C1220,1546 1181,1530 1134,1530C1089,1530 1050,1546 1019,1578C987,1610 971,1649 971,1695C971,1740 987,1779 1019,1811C1050,1842 1089,1858 1134,1858M657,1858C703,1859 742,1843 774,1811C805,1779 821,1740 821,1695C821,1649 805,1610 774,1578C743,1546 704,1530 657,1530C612,1530 573,1546 542,1578C510,1610 494,1649 494,1695C494,1740 510,1779 542,1811C573,1842 612,1858 657,1858z""/>
<glyph unicode=""&#xDC;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M1081,1858C1127,1858 1166,1842 1198,1811C1229,1779 1245,1740 1245,1695C1245,1649 1229,1610 1198,1578C1167,1546 1128,1530 1081,1530C1036,1530 997,1546 966,1578C934,1610 918,1649 918,1695C918,1740 934,1779 966,1811C997,1842 1036,1858 1081,1858M604,1858C650,1859 689,1843 721,1811C752,1779 768,1740 768,1695C768,1649 752,1610 721,1578C690,1546 651,1530 604,1530C559,1530 520,1546 489,1578C457,1610 441,1649 441,1695C441,1740 457,1779 489,1811C520,1842 559,1858 604,1858z""/>
<glyph unicode=""&#xE1;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M871,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xE0;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M219,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xE2;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M684,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xE4;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M782,1411C828,1411 867,1395 899,1364C930,1332 946,1293 946,1248C946,1202 930,1163 899,1131C868,1099 829,1083 782,1083C737,1083 698,1099 667,1131C635,1163 619,1202 619,1248C619,1293 635,1332 667,1364C698,1395 737,1411 782,1411M305,1411C351,1412 390,1396 422,1364C453,1332 469,1293 469,1248C469,1202 453,1163 422,1131C391,1099 352,1083 305,1083C260,1083 221,1099 190,1131C158,1163 142,1202 142,1248C142,1293 158,1332 190,1364C221,1395 260,1411 305,1411z""/>
<glyph unicode=""&#xE3;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M803,1374l139,0C928,1168 848,1065 703,1065C661,1065 602,1084 526,1121C450,1158 399,1176 373,1176C321,1176 290,1140 281,1069l-135,0C153,1167 179,1242 222,1295C265,1348 321,1374 389,1374C433,1374 491,1356 564,1319C636,1282 686,1264 713,1264C761,1264 791,1301 803,1374z""/>
<glyph unicode=""&#xE5;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M336,1249C336,1306 357,1355 398,1396C439,1436 488,1456 545,1456C602,1456 651,1436 692,1396C732,1355 752,1306 752,1249C752,1192 732,1143 691,1102C650,1061 602,1040 545,1040C488,1040 439,1061 398,1102C357,1143 336,1192 336,1249M658,1249C658,1280 647,1306 625,1328C602,1349 576,1360 545,1360C514,1360 488,1349 467,1328C445,1306 434,1280 434,1249C434,1218 445,1192 467,1170C488,1148 514,1137 545,1137C576,1137 602,1148 625,1170C647,1192 658,1218 658,1249z""/>
<glyph unicode=""&#xE7;"" horiz-adv-x=""1024"" d=""M948,258l0,-213C835,3 721,-18 608,-18l-12,-29C681,-78 723,-128 723,-197C723,-248 703,-289 664,-320C625,-351 570,-367 500,-367C433,-367 383,-361 348,-348l0,104C452,-247 504,-226 504,-180C504,-142 476,-123 420,-123l67,119C358,20 259,74 188,159C117,243 82,346 82,469C82,612 132,731 233,824C333,917 461,963 616,963C716,963 822,942 934,901l0,-221C852,722 771,743 692,743C603,743 532,718 479,669C426,620 399,554 399,471C399,388 425,321 477,270C529,219 597,193 682,193C745,193 833,215 948,258z""/>
<glyph unicode=""&#xE9;"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580M893,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xE8;"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580M242,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xEA;"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580M706,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xEB;"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580M804,1411C850,1411 889,1395 921,1364C952,1332 968,1293 968,1248C968,1202 952,1163 921,1131C890,1099 851,1083 804,1083C759,1083 720,1099 689,1131C657,1163 641,1202 641,1248C641,1293 657,1332 689,1364C720,1395 759,1411 804,1411M327,1411C373,1412 412,1396 444,1364C475,1332 491,1293 491,1248C491,1202 475,1163 444,1131C413,1099 374,1083 327,1083C282,1083 243,1099 212,1131C180,1163 164,1202 164,1248C164,1293 180,1332 212,1364C243,1395 282,1411 327,1411z""/>
<glyph unicode=""&#xED;"" horiz-adv-x=""555"" d=""M426,944l0,-944l-297,0l0,944M604,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xEC;"" horiz-adv-x=""555"" d=""M426,944l0,-944l-297,0l0,944M-48,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xEE;"" horiz-adv-x=""555"" d=""M426,944l0,-944l-297,0l0,944M417,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xEF;"" horiz-adv-x=""555"" d=""M426,944l0,-944l-297,0l0,944M516,1411C562,1411 601,1395 633,1364C664,1332 680,1293 680,1248C680,1202 664,1163 633,1131C602,1099 563,1083 516,1083C471,1083 432,1099 401,1131C369,1163 353,1202 353,1248C353,1293 369,1332 401,1364C432,1395 471,1411 516,1411M39,1411C85,1412 124,1396 156,1364C187,1332 203,1293 203,1248C203,1202 187,1163 156,1131C125,1099 86,1083 39,1083C-6,1083 -45,1099 -76,1131C-108,1163 -124,1202 -124,1248C-124,1293 -108,1332 -76,1364C-45,1395 -6,1411 39,1411z""/>
<glyph unicode=""&#xF1;"" horiz-adv-x=""1194"" d=""M420,944l0,-135C507,912 610,963 727,963C825,963 907,932 973,869C1038,806 1071,717 1071,600l0,-600l-297,0l0,575C774,701 725,764 627,764C557,764 488,712 420,608l0,-608l-297,0l0,944M856,1374l139,0C981,1168 901,1065 756,1065C714,1065 655,1084 579,1121C503,1158 452,1176 426,1176C374,1176 343,1140 334,1069l-135,0C206,1167 232,1242 275,1295C318,1348 374,1374 442,1374C486,1374 544,1356 617,1319C689,1282 739,1264 766,1264C814,1264 844,1301 856,1374z""/>
<glyph unicode=""&#xF3;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M936,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xF2;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M285,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xF4;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M749,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xF6;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M847,1411C893,1411 932,1395 964,1364C995,1332 1011,1293 1011,1248C1011,1202 995,1163 964,1131C933,1099 894,1083 847,1083C802,1083 763,1099 732,1131C700,1163 684,1202 684,1248C684,1293 700,1332 732,1364C763,1395 802,1411 847,1411M370,1411C416,1412 455,1396 487,1364C518,1332 534,1293 534,1248C534,1202 518,1163 487,1131C456,1099 417,1083 370,1083C325,1083 286,1099 255,1131C223,1163 207,1202 207,1248C207,1293 223,1332 255,1364C286,1395 325,1411 370,1411z""/>
<glyph unicode=""&#xF5;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M869,1374l139,0C994,1168 914,1065 769,1065C727,1065 668,1084 592,1121C516,1158 465,1176 439,1176C387,1176 356,1140 347,1069l-135,0C219,1167 245,1242 288,1295C331,1348 387,1374 455,1374C499,1374 557,1356 630,1319C702,1282 752,1264 779,1264C827,1264 857,1301 869,1374z""/>
<glyph unicode=""&#xFA;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M924,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xF9;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M272,1384l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xFB;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M737,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xFC;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M835,1411C881,1411 920,1395 952,1364C983,1332 999,1293 999,1248C999,1202 983,1163 952,1131C921,1099 882,1083 835,1083C790,1083 751,1099 720,1131C688,1163 672,1202 672,1248C672,1293 688,1332 720,1364C751,1395 790,1411 835,1411M358,1411C404,1412 443,1396 475,1364C506,1332 522,1293 522,1248C522,1202 506,1163 475,1131C444,1099 405,1083 358,1083C313,1083 274,1099 243,1131C211,1163 195,1202 195,1248C195,1293 211,1332 243,1364C274,1395 313,1411 358,1411z""/>
<glyph unicode=""&#x2020;"" horiz-adv-x=""1044"" d=""M463,780l-357,-78C100,701 95,700 90,700C64,700 51,727 51,780l0,127C51,962 64,989 91,989C95,989 100,988 106,987l357,-80l-74,451C386,1383 390,1399 402,1406C414,1412 434,1415 463,1415l121,0C614,1415 634,1412 645,1405C656,1398 659,1383 655,1360l-71,-453l354,80C976,996 995,969 995,907l0,-127C995,720 977,694 940,702l-356,78l110,-1204C698,-455 676,-471 629,-471l-213,0C369,-471 347,-453 350,-418z""/>
<glyph unicode=""&#xB0;"" horiz-adv-x=""811"" d=""M86,1171C86,1259 117,1334 180,1397C243,1460 318,1491 406,1491C493,1491 568,1460 631,1397C694,1334 725,1259 725,1171C725,1084 694,1009 631,946C568,883 493,852 406,852C318,852 243,883 180,946C117,1009 86,1084 86,1171M549,1171C549,1211 535,1245 507,1273C479,1301 445,1315 406,1315C366,1315 332,1301 304,1273C276,1245 262,1211 262,1171C262,1132 276,1098 304,1070C332,1042 366,1028 406,1028C445,1028 479,1042 507,1070C535,1098 549,1132 549,1171z""/>
<glyph unicode=""&#xA2;"" horiz-adv-x=""1024"" d=""M965,1294l-113,-362C889,921 915,911 932,903l0,-223C889,704 842,723 791,737l-170,-536C640,198 660,197 681,197C767,197 855,219 946,264l0,-219C841,3 710,-18 553,-18l-117,-369l-145,0l123,387C194,75 84,230 84,465C84,609 133,728 231,822C329,915 458,962 618,962C649,962 680,960 713,956l106,338M492,254l155,489C569,737 507,709 462,658C416,607 393,545 393,471C393,380 426,307 492,254z""/>
<glyph unicode=""&#xA3;"" horiz-adv-x=""1067"" d=""M688,809l0,-244l-199,0l-53,-266C457,306 479,309 500,309C543,309 596,302 659,287C722,272 775,264 817,264C878,264 934,276 983,299l0,-276C926,-4 865,-18 801,-18C779,-18 724,-10 637,7C549,23 475,31 416,31C320,31 213,15 96,-18C106,124 142,318 203,565l-92,0l0,244l157,0C339,1060 421,1224 514,1301C607,1377 702,1415 800,1415C817,1415 833,1414 850,1411l119,-268C934,1151 901,1155 870,1155C713,1155 609,1040 557,809z""/>
<glyph unicode=""&#xA7;"" horiz-adv-x=""1067"" d=""M231,891C192,946 166,987 154,1014C141,1041 135,1071 135,1106C135,1201 172,1276 246,1332C320,1387 420,1415 547,1415C668,1415 761,1394 828,1353C895,1312 928,1258 928,1192C928,1150 913,1114 884,1085C854,1055 818,1040 776,1040C713,1040 670,1076 648,1149C639,1180 628,1202 615,1217C602,1232 578,1239 543,1239C509,1239 481,1229 458,1210C435,1190 424,1166 424,1137C424,1115 435,1091 456,1065C477,1038 552,982 679,897C806,811 894,730 942,653C990,576 1014,499 1014,422C1014,347 1000,282 971,227C942,171 896,112 831,51C894,-30 926,-107 926,-180C926,-270 891,-341 820,-393C749,-445 652,-471 528,-471C415,-471 322,-450 249,-408C176,-366 139,-313 139,-250C139,-211 154,-177 185,-147C216,-117 251,-102 291,-102C360,-102 404,-142 425,-222C432,-248 441,-267 453,-278C464,-289 487,-295 522,-295C557,-295 585,-286 607,-268C628,-250 639,-227 639,-199C639,-172 627,-144 604,-114C580,-84 504,-26 377,59C250,144 164,223 119,295C74,367 51,439 51,512C51,647 111,773 231,891M383,756C336,701 313,646 313,592C313,552 328,510 358,466C388,421 497,329 684,190C731,240 754,292 754,346C754,424 709,498 620,569z""/>
<glyph unicode=""&#x2022;"" horiz-adv-x=""725"" d=""M66,698C66,780 95,850 152,907C209,964 279,993 360,993C442,993 512,964 569,907C626,850 655,780 655,698C655,617 626,547 569,490C512,432 442,403 360,403C279,403 209,432 152,490C95,547 66,617 66,698z""/>
<glyph unicode=""&#xB6;"" horiz-adv-x=""1130"" d=""M635,-473l-244,0l0,1046C268,583 171,623 102,694C33,765 -2,860 -2,979C-2,1110 38,1212 118,1286C197,1359 306,1396 444,1396l684,0l0,-262l-112,0l0,-1607l-244,0l0,1607l-137,0z""/>
<glyph unicode=""&#xDF;"" horiz-adv-x=""1237"" d=""M133,0l0,975C133,1118 174,1227 255,1302C336,1377 447,1415 586,1415C734,1415 852,1383 941,1318C1029,1253 1073,1168 1073,1061C1073,935 1013,845 893,791C1051,730 1130,603 1130,410C1130,283 1092,180 1015,101C938,22 839,-18 717,-18C669,-18 623,-12 578,0l0,190C605,181 623,176 633,176C692,176 740,201 776,250C811,299 829,364 829,446C829,632 749,722 590,715l0,147C711,857 772,920 772,1051C772,1110 757,1156 728,1189C698,1222 657,1239 604,1239C488,1239 430,1152 430,979l0,-979z""/>
<glyph unicode=""&#xAE;"" horiz-adv-x=""1516"" d=""M758,1415C970,1415 1151,1340 1300,1191C1449,1042 1524,861 1524,649C1524,437 1449,256 1300,107C1151,-42 970,-117 758,-117C545,-117 365,-42 216,107C67,256 -8,436 -8,649C-8,861 67,1042 216,1191C365,1340 545,1415 758,1415M758,1263C587,1263 442,1203 323,1084C203,965 143,820 143,649C143,478 203,333 323,213C442,93 587,33 758,33C928,33 1073,93 1194,213C1314,333 1374,478 1374,649C1374,820 1314,965 1194,1084C1074,1203 929,1263 758,1263M1118,239l-203,0l-65,134C828,418 803,458 774,493C745,528 720,551 701,560C682,569 650,573 606,573l-31,0l0,-334l-163,0l0,814l401,0C892,1053 954,1033 1000,993C1046,953 1069,897 1069,825C1069,695 996,619 850,598l0,-4C883,582 911,565 932,543C953,520 983,474 1024,403M575,702l170,0C845,702 895,740 895,815C895,889 839,926 727,926l-152,0z""/>
<glyph unicode=""&#xA9;"" horiz-adv-x=""1516"" d=""M758,1415C970,1415 1151,1340 1300,1191C1449,1042 1524,861 1524,649C1524,437 1449,256 1300,107C1151,-42 970,-117 758,-117C545,-117 365,-42 216,107C67,256 -8,436 -8,649C-8,861 67,1042 216,1191C365,1340 545,1415 758,1415M758,1263C587,1263 442,1203 323,1084C203,965 143,820 143,649C143,478 203,333 323,213C442,93 587,33 758,33C928,33 1073,93 1194,213C1314,333 1374,478 1374,649C1374,820 1314,965 1194,1084C1074,1203 929,1263 758,1263M942,536l162,-55C1050,309 938,223 768,223C649,223 554,261 485,338C416,415 381,516 381,643C381,774 416,879 485,958C554,1036 649,1075 770,1075C935,1075 1047,994 1104,831l-162,-39C907,885 848,932 764,932C626,932 557,838 557,649C557,562 574,493 609,442C643,390 692,364 756,364C847,364 909,421 942,536z""/>
<glyph unicode=""&#x2122;"" horiz-adv-x=""2048"" d=""M469,577l0,676l-252,0l0,143l664,0l0,-143l-246,0l0,-676M979,577l0,819l260,0l152,-565l147,565l260,0l0,-819l-158,0l0,653l-178,-653l-149,0l-176,653l0,-653z""/>
<glyph unicode=""&#xB4;"" horiz-adv-x=""682"" d=""M668,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xA8;"" horiz-adv-x=""682"" d=""M579,1411C625,1411 664,1395 696,1364C727,1332 743,1293 743,1248C743,1202 727,1163 696,1131C665,1099 626,1083 579,1083C534,1083 495,1099 464,1131C432,1163 416,1202 416,1248C416,1293 432,1332 464,1364C495,1395 534,1411 579,1411M102,1411C148,1412 187,1396 219,1364C250,1332 266,1293 266,1248C266,1202 250,1163 219,1131C188,1099 149,1083 102,1083C57,1083 18,1099 -13,1131C-45,1163 -61,1202 -61,1248C-61,1293 -45,1332 -13,1364C18,1395 57,1411 102,1411z""/>
<glyph unicode=""&#x2260;"" horiz-adv-x=""1124"" d=""M1106,279l-621,0l-184,-330l-100,57l151,273l-321,0l0,112l385,0l157,285l-542,0l0,112l606,0l186,336l101,-57l-156,-279l338,0l0,-112l-399,0l-160,-285l559,0z""/>
<glyph unicode=""&#xC6;"" horiz-adv-x=""2198"" d=""M2064,1397l0,-256l-651,0l0,-305l623,0l0,-256l-623,0l0,-324l670,0l0,-256l-1018,0l0,580l-373,0l-346,-580l-358,0l835,1397M846,836l219,0l0,378z""/>
<glyph unicode=""&#xD8;"" horiz-adv-x=""1792"" d=""M403,125l-225,-201l-125,156l205,178C150,390 96,537 96,698C96,889 170,1056 319,1200C467,1343 660,1415 899,1415C1084,1415 1248,1367 1391,1270l172,155l141,-141l-170,-147C1642,1006 1696,860 1696,698C1696,504 1622,336 1473,195C1324,53 1132,-18 899,-18C714,-18 548,30 403,125M506,483l645,576C1080,1102 996,1124 899,1124C766,1124 659,1085 578,1007C496,928 455,825 455,698C455,615 472,543 506,483M1286,915l-643,-577C709,294 794,272 899,272C1030,272 1136,311 1217,390C1297,468 1337,571 1337,698C1337,783 1320,856 1286,915z""/>
<glyph unicode=""&#x221E;"" horiz-adv-x=""1460"" d=""M681,843C730,924 781,982 832,1018C883,1053 941,1071 1004,1071C1092,1071 1165,1040 1224,978C1282,915 1311,830 1311,722C1311,614 1282,529 1224,467C1165,404 1092,373 1004,373C941,373 884,391 833,426C782,461 731,520 681,601C641,545 595,502 543,473C491,444 441,429 394,429C327,429 270,455 223,508C176,560 152,631 152,722C152,813 176,884 223,936C270,988 327,1014 394,1014C441,1014 491,999 543,970C595,941 641,898 681,843M774,724C830,638 869,585 892,566C927,536 966,521 1007,521C1058,521 1097,541 1124,580C1151,619 1165,668 1165,726C1165,789 1150,837 1120,872C1090,907 1053,924 1010,924C976,924 942,912 908,888C873,864 829,809 774,724M595,726C528,820 465,867 405,867C374,867 348,855 326,830C303,805 292,771 292,727C292,682 303,646 325,621C347,596 374,583 405,583C431,583 454,590 475,604C511,628 551,669 595,726z""/>
<glyph unicode=""&#xB1;"" horiz-adv-x=""1196"" d=""M86,737l0,262l381,0l0,381l262,0l0,-381l381,0l0,-262l-381,0l0,-381l-262,0l0,381M86,0l0,262l1024,0l0,-262z""/>
<glyph unicode=""&#x2264;"" horiz-adv-x=""1124"" d=""M1077,240l-1018,471l0,127l1018,471l0,-127l-887,-408l887,-407M1077,0l-1018,0l0,112l1018,0z""/>
<glyph unicode=""&#x2265;"" horiz-adv-x=""1124"" d=""M1077,711l-1018,-471l0,127l887,407l-887,408l0,127l1018,-471M1077,0l-1018,0l0,112l1018,0z""/>
<glyph unicode=""&#xA5;"" horiz-adv-x=""1130"" d=""M422,272l-371,0l0,224l371,0l0,118l-371,0l0,224l260,0l-309,628l309,0l254,-567l256,567l309,0l-315,-628l264,0l0,-224l-374,0l0,-118l374,0l0,-224l-374,0l0,-272l-283,0z""/>
<glyph unicode=""&#xB5;"" horiz-adv-x=""1260"" d=""M139,944l297,0l0,-461C436,281 499,180 625,180C685,180 733,204 768,252C803,299 821,364 821,446l0,498l297,0l0,-944l-276,0l0,123l-4,0C822,78 795,43 758,19C720,-6 679,-18 635,-18C544,-18 479,26 440,115l-4,0l0,-475l-297,0z""/>
<glyph unicode=""&#x2202;"" horiz-adv-x=""1012"" d=""M379,1123l-137,84C277,1302 327,1373 393,1420C458,1467 531,1491 612,1491C709,1491 787,1458 846,1391C933,1294 977,1145 977,943C977,769 949,600 894,437C839,274 762,156 664,84C566,11 463,-25 355,-25C263,-25 188,6 131,67C73,128 44,216 44,329C44,434 73,535 131,632C189,729 278,805 397,862C516,919 659,945 826,942C825,1075 801,1174 752,1238C716,1285 668,1309 607,1309C561,1309 521,1296 487,1270C452,1243 416,1194 379,1123M814,758C795,759 780,759 770,759C679,759 594,741 515,704C436,667 376,614 335,545C294,476 273,407 273,338C273,286 287,244 315,213C343,181 376,165 414,165C493,165 566,203 631,280C725,390 786,549 814,758z""/>
<glyph unicode=""&#x2211;"" horiz-adv-x=""1460"" d=""M139,1491l1237,0l0,-164l-988,0l607,-775l-649,-822l1040,0l0,-161l-1264,0l0,187l620,788l-603,772z""/>
<glyph unicode=""&#x220F;"" horiz-adv-x=""1686"" d=""M161,1491l1362,0l0,-1922l-191,0l0,1748l-978,0l0,-1748l-193,0z""/>
<glyph unicode=""&#x3C0;"" horiz-adv-x=""1124"" d=""M0,1063l1124,0l0,-202l-169,0l0,-861l-288,0l0,861l-208,0l0,-861l-292,0l0,861l-167,0z""/>
<glyph unicode=""&#x222B;"" horiz-adv-x=""561"" d=""M602,1760C602,1734 592,1713 573,1696C553,1679 528,1671 497,1671C472,1671 452,1679 437,1696C422,1713 407,1721 393,1721C381,1721 372,1716 366,1707C359,1697 356,1682 356,1663C356,1640 363,1555 376,1407C389,1259 395,1075 395,856C395,519 385,283 365,147C345,10 319,-84 286,-136C269,-163 246,-184 219,-198C191,-212 162,-219 133,-219C100,-219 72,-207 47,-184C22,-161 10,-132 10,-99C10,-74 19,-52 36,-34C53,-16 74,-7 100,-7C123,-7 145,-18 166,-40C187,-62 201,-73 210,-73C227,-73 240,-58 250,-28C259,1 264,52 264,125C264,150 262,215 258,322C254,429 252,616 252,884C252,1205 258,1418 269,1523C280,1628 294,1699 310,1738C329,1785 352,1819 379,1842C406,1865 434,1876 463,1876C501,1876 534,1865 561,1844C588,1822 602,1794 602,1760z""/>
<glyph unicode=""&#xAA;"" horiz-adv-x=""725"" d=""M723,995l0,-116C655,842 591,823 532,823C471,823 433,842 418,879C355,842 290,823 225,823C174,823 131,838 97,868C62,897 45,934 45,977C45,1018 64,1056 103,1089C142,1122 243,1160 406,1202C406,1258 375,1286 313,1286C228,1286 149,1249 78,1176l0,167C161,1391 253,1415 352,1415C435,1415 501,1400 549,1371C597,1341 621,1297 621,1239l0,-244C621,970 627,958 639,958C655,958 683,970 723,995M406,977l0,121C348,1079 310,1063 291,1050C272,1036 262,1018 262,995C262,960 279,942 313,942C344,942 375,954 406,977z""/>
<glyph unicode=""&#xBA;"" horiz-adv-x=""811"" d=""M408,1415C510,1415 594,1387 661,1331C727,1275 760,1205 760,1122C760,1035 727,963 662,907C597,851 512,823 408,823C305,823 220,851 153,906C86,961 53,1031 53,1116C53,1201 87,1273 154,1330C221,1387 306,1415 408,1415M410,944C490,944 530,1005 530,1128C530,1239 488,1294 403,1294C323,1294 283,1234 283,1114C283,1001 325,944 410,944z""/>
<glyph unicode=""&#x2126;"" horiz-adv-x=""1573"" d=""M462,206C385,239 323,276 277,318C210,378 160,450 127,534C94,618 77,710 77,811C77,1018 141,1185 269,1312C396,1439 566,1503 777,1503C932,1503 1062,1477 1167,1424C1271,1371 1354,1290 1417,1180C1479,1069 1510,947 1510,813C1510,678 1479,558 1417,453C1354,347 1258,265 1128,208l330,23l72,0l0,-231l-642,0l0,243C953,274 1004,309 1041,348C1092,401 1130,466 1156,543C1181,619 1194,699 1194,784C1194,949 1156,1073 1080,1158C1003,1242 906,1284 787,1284C674,1284 579,1241 503,1155C426,1068 388,946 388,789C388,698 403,614 432,536C461,458 502,392 556,338C591,303 639,272 700,243l0,-243l-645,0l0,230l98,0z""/>
<glyph unicode=""&#xE6;"" horiz-adv-x=""1599"" d=""M1518,305l0,-194C1403,25 1266,-18 1108,-18C950,-18 822,25 725,111C614,25 486,-18 342,-18C262,-18 197,5 148,50C99,95 74,150 74,217C74,298 107,367 173,423C239,479 396,539 643,604C648,653 634,692 603,721C572,750 529,764 475,764C422,764 361,749 294,720C227,691 167,652 115,604l0,236C255,922 385,963 504,963C615,963 718,928 811,858C889,928 983,963 1092,963C1221,963 1329,913 1416,813C1503,712 1541,583 1532,426l-592,0C936,343 956,277 999,230C1042,182 1100,158 1174,158C1293,158 1407,207 1518,305M940,582l307,0C1236,718 1184,786 1090,786C995,786 945,718 940,582M643,225l0,232C464,408 375,341 375,256C375,228 384,205 402,186C420,167 442,158 469,158C515,158 573,180 643,225z""/>
<glyph unicode=""&#xF8;"" horiz-adv-x=""1217"" d=""M338,43l-133,-154l-105,93l121,141C124,216 76,332 76,473C76,621 127,740 229,829C330,918 457,963 610,963C708,963 798,943 881,903l143,166l109,-104l-127,-150C1096,730 1141,615 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C505,-18 415,2 338,43M406,340l337,399C706,770 662,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,418 397,374 406,340M813,596l-336,-391C511,174 555,158 610,158C752,158 823,265 823,479C823,525 820,564 813,596z""/>
<glyph unicode=""&#xBF;"" horiz-adv-x=""768"" d=""M623,784C623,735 606,694 571,659C536,624 493,606 444,606C395,606 354,624 319,659C284,694 266,735 266,784C266,833 284,876 319,911C354,946 395,963 444,963C493,963 536,946 571,911C606,876 623,833 623,784M717,-178l-51,-252C587,-457 508,-471 428,-471C324,-471 242,-445 181,-393C120,-341 90,-271 90,-184C90,-134 99,-82 118,-28C137,25 179,93 244,175C309,256 350,317 365,356C380,395 387,432 387,467l0,25C387,506 391,515 400,519C408,522 425,524 451,524C476,524 493,519 504,508C514,497 526,468 540,421C554,374 561,332 561,295C561,220 530,129 467,22C423,-53 401,-108 401,-145C401,-215 441,-250 522,-250C583,-250 641,-226 698,-178z""/>
<glyph unicode=""&#xA1;"" horiz-adv-x=""555"" d=""M455,784C455,735 438,694 403,659C368,624 325,606 276,606C227,606 186,624 151,659C116,694 98,735 98,784C98,833 116,876 151,911C186,946 227,963 276,963C325,963 368,946 403,911C438,876 455,833 455,784M106,-250C106,-157 122,-18 154,165C185,348 207,453 219,481C231,508 250,522 276,522C304,522 323,511 333,488C343,465 365,366 399,193C432,20 449,-128 449,-250C449,-397 391,-471 276,-471C163,-471 106,-397 106,-250z""/>
<glyph unicode=""&#xAC;"" horiz-adv-x=""1196"" d=""M846,238l0,436l-760,0l0,262l1022,0l0,-698z""/>
<glyph unicode=""&#x221A;"" horiz-adv-x=""1124"" d=""M1055,1868l-307,-1946l-498,1028l-197,-96l-33,68l299,147l404,-825l260,1634z""/>
<glyph unicode=""&#x192;"" horiz-adv-x=""1130"" d=""M276,850l37,211l201,0l31,170C562,1328 595,1395 646,1434C697,1472 771,1491 870,1491l9,0C969,1491 1057,1477 1143,1450l-56,-225C1026,1242 974,1251 930,1251C892,1251 864,1243 846,1227C828,1210 812,1155 797,1061l207,0l-37,-211l-205,0l-174,-993C569,-252 535,-327 484,-368C433,-409 359,-430 260,-430C161,-430 68,-417 -20,-391l57,221C102,-183 158,-190 207,-190C252,-190 280,-177 293,-152C306,-127 317,-84 328,-23l149,873z""/>
<glyph unicode=""&#x2248;"" horiz-adv-x=""1196"" d=""M33,740l0,269C151,1107 260,1156 360,1156C417,1156 503,1129 617,1076C732,1023 813,996 862,996C945,996 1045,1047 1161,1148l0,-266C1119,841 1068,806 1007,777C946,748 889,734 838,734C781,734 694,761 577,815C460,869 382,896 344,896C259,896 155,844 33,740M33,296l0,269C151,663 260,712 360,712C417,712 503,685 617,632C732,579 813,552 862,552C945,552 1045,603 1161,704l0,-266C1119,397 1068,362 1007,333C946,304 889,290 838,290C781,290 694,317 577,371C460,425 382,452 344,452C259,452 155,400 33,296z""/>
<glyph unicode=""&#x2206;"" horiz-adv-x=""1253"" d=""M668,1409l577,-1409l-1233,0M594,1059l-447,-959l844,0z""/>
<glyph unicode=""&#xAB;"" horiz-adv-x=""1217"" d=""M868,469l326,-455l-293,0l-330,455l330,455l293,0M315,469l326,-455l-293,0l-330,455l330,455l293,0z""/>
<glyph unicode=""&#xBB;"" horiz-adv-x=""1217"" d=""M899,469l-326,455l293,0l330,-455l-330,-455l-293,0M350,469l-325,455l292,0l330,-455l-330,-455l-292,0z""/>
<glyph unicode=""&#x2026;"" horiz-adv-x=""2048"" d=""M1528,160C1528,209 1546,251 1581,286C1616,321 1657,338 1706,338C1755,338 1797,321 1832,286C1867,251 1884,209 1884,160C1884,111 1867,69 1832,34C1797,-1 1755,-18 1706,-18C1657,-18 1616,-1 1581,34C1546,69 1528,111 1528,160M846,160C846,209 864,251 899,286C934,321 975,338 1024,338C1073,338 1115,321 1150,286C1185,251 1202,209 1202,160C1202,111 1185,69 1150,34C1115,-1 1073,-18 1024,-18C975,-18 934,-1 899,34C864,69 846,111 846,160M164,160C164,209 182,251 217,286C252,321 293,338 342,338C391,338 433,321 468,286C503,251 520,209 520,160C520,111 503,69 468,34C433,-1 391,-18 342,-18C293,-18 252,-1 217,34C182,69 164,111 164,160z""/>
<glyph unicode=""&#xA0;"" horiz-adv-x=""569""/>
<glyph unicode=""&#xC0;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M475,1831l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xC3;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M1059,1821l139,0C1184,1615 1104,1512 959,1512C917,1512 858,1531 782,1568C706,1605 655,1623 629,1623C577,1623 546,1587 537,1516l-135,0C409,1614 435,1689 478,1742C521,1795 577,1821 645,1821C689,1821 747,1803 820,1766C892,1729 942,1711 969,1711C1017,1711 1047,1748 1059,1821z""/>
<glyph unicode=""&#xD5;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M1155,1821l139,0C1280,1615 1200,1512 1055,1512C1013,1512 954,1531 878,1568C802,1605 751,1623 725,1623C673,1623 642,1587 633,1516l-135,0C505,1614 531,1689 574,1742C617,1795 673,1821 741,1821C785,1821 843,1803 916,1766C988,1729 1038,1711 1065,1711C1113,1711 1143,1748 1155,1821z""/>
<glyph unicode=""&#x152;"" horiz-adv-x=""2241"" d=""M2118,1397l0,-256l-654,0l0,-293l625,0l0,-256l-625,0l0,-336l668,0l0,-256l-1255,0C641,0 452,63 310,189C167,314 96,485 96,702C96,913 166,1082 306,1208C446,1334 635,1397 874,1397M1116,1141l-168,0C799,1141 679,1099 590,1015C500,930 455,825 455,698C455,571 500,466 590,382C680,298 799,256 948,256l168,0z""/>
<glyph unicode=""&#x153;"" horiz-adv-x=""1792"" d=""M1714,305l0,-203C1596,22 1472,-18 1341,-18C1209,-18 1087,22 975,102C865,22 743,-18 608,-18C457,-18 331,29 228,124C125,219 74,336 74,477C74,616 125,731 228,824C330,917 458,963 612,963C736,963 852,925 961,850C1009,889 1053,918 1093,936C1132,954 1183,963 1245,963C1390,963 1509,915 1602,818C1694,721 1737,591 1731,426l-609,0C1119,347 1142,283 1191,233C1239,183 1299,158 1372,158C1494,158 1608,207 1714,305M1122,582l336,0C1458,641 1442,689 1411,728C1379,767 1337,786 1284,786C1236,786 1197,767 1167,730C1137,693 1122,643 1122,582M608,158C753,158 825,269 825,492C825,586 806,659 768,710C729,761 675,786 604,786C462,786 391,680 391,469C391,262 463,158 608,158z""/>
<glyph unicode=""&#x2013;"" horiz-adv-x=""1024"" d=""M1036,608l0,-270l-1048,0l0,270z""/>
<glyph unicode=""&#x2014;"" horiz-adv-x=""2048"" d=""M2060,608l0,-270l-2072,0l0,270z""/>
<glyph unicode=""&#x201C;"" horiz-adv-x=""1153"" d=""M1047,1415l0,-63C984,1291 952,1214 952,1121C952,1110 953,1098 954,1087C1024,1060 1059,1011 1059,938C1059,891 1043,853 1011,822C979,791 939,776 891,776C831,776 782,798 744,842C705,885 686,941 686,1010C686,1109 720,1198 788,1277C855,1356 942,1402 1047,1415M477,1415l0,-63C414,1291 383,1214 383,1121C383,1110 384,1098 385,1087C454,1060 489,1011 489,938C489,891 473,853 442,822C410,791 370,776 322,776C262,776 213,798 175,842C136,885 117,941 117,1010C117,1109 151,1198 219,1277C286,1356 372,1402 477,1415z""/>
<glyph unicode=""&#x201D;"" horiz-adv-x=""1153"" d=""M676,776l0,64C739,900 770,977 770,1070C770,1082 769,1093 768,1104C699,1131 664,1180 664,1253C664,1300 680,1338 712,1369C743,1400 783,1415 831,1415C891,1415 940,1393 979,1350C1017,1307 1036,1251 1036,1182C1036,1083 1002,993 935,914C867,835 781,789 676,776M106,776l0,64C169,900 201,977 201,1070C201,1082 200,1093 199,1104C129,1131 94,1180 94,1253C94,1300 110,1338 142,1369C174,1400 214,1415 262,1415C322,1415 371,1393 410,1350C448,1307 467,1251 467,1182C467,1083 433,993 366,914C298,835 211,789 106,776z""/>
<glyph unicode=""&#x2018;"" horiz-adv-x=""555"" d=""M453,1415l0,-63C390,1291 358,1214 358,1121C358,1110 359,1098 360,1087C430,1060 465,1011 465,938C465,891 449,853 417,822C385,791 345,776 297,776C237,776 188,798 150,842C111,885 92,941 92,1010C92,1109 126,1198 194,1277C261,1356 348,1402 453,1415z""/>
<glyph unicode=""&#x2019;"" horiz-adv-x=""555"" d=""M102,776l0,64C165,900 197,977 197,1070C197,1082 196,1093 195,1104C125,1131 90,1180 90,1253C90,1300 106,1338 138,1369C170,1400 210,1415 258,1415C318,1415 367,1393 406,1350C444,1307 463,1251 463,1182C463,1083 429,993 362,914C294,835 207,789 102,776z""/>
<glyph unicode=""&#xF7;"" horiz-adv-x=""1196"" d=""M449,1126C449,1167 464,1203 493,1232C522,1261 557,1276 598,1276C639,1276 674,1261 704,1232C733,1203 748,1167 748,1126C748,1085 733,1050 704,1021C674,992 639,977 598,977C557,977 522,992 493,1021C464,1050 449,1085 449,1126M86,592l0,262l1024,0l0,-262M449,319C449,360 464,396 493,425C522,454 557,469 598,469C639,469 674,454 704,425C733,396 748,360 748,319C748,278 733,243 704,214C674,185 639,170 598,170C557,170 522,185 493,214C464,243 449,278 449,319z""/>
<glyph unicode=""&#x25CA;"" horiz-adv-x=""1012"" d=""M962,763l-397,-763l-122,0l-397,763l397,763l122,0M839,763l-335,652l-335,-652l335,-652z""/>
<glyph unicode=""&#xFF;"" horiz-adv-x=""1044"" d=""M1042,944l-632,-1397l-316,0l277,607l-369,790l317,0l199,-485l213,485M761,1411C807,1411 846,1395 878,1364C909,1332 925,1293 925,1248C925,1202 909,1163 878,1131C847,1099 808,1083 761,1083C716,1083 677,1099 646,1131C614,1163 598,1202 598,1248C598,1293 614,1332 646,1364C677,1395 716,1411 761,1411M284,1411C330,1412 369,1396 401,1364C432,1332 448,1293 448,1248C448,1202 432,1163 401,1131C370,1099 331,1083 284,1083C239,1083 200,1099 169,1131C137,1163 121,1202 121,1248C121,1293 137,1332 169,1364C200,1395 239,1411 284,1411z""/>
<glyph unicode=""&#x178;"" horiz-adv-x=""1450"" d=""M551,0l0,653l-551,744l422,0l303,-439l305,439l420,0l-551,-744l0,-653M964,1858C1010,1858 1049,1842 1081,1811C1112,1779 1128,1740 1128,1695C1128,1649 1112,1610 1081,1578C1050,1546 1011,1530 964,1530C919,1530 880,1546 849,1578C817,1610 801,1649 801,1695C801,1740 817,1779 849,1811C880,1842 919,1858 964,1858M487,1858C533,1859 572,1843 604,1811C635,1779 651,1740 651,1695C651,1649 635,1610 604,1578C573,1546 534,1530 487,1530C442,1530 403,1546 372,1578C340,1610 324,1649 324,1695C324,1740 340,1779 372,1811C403,1842 442,1858 487,1858z""/>
<glyph unicode=""&#x2215;"" horiz-adv-x=""449"" d=""M809,1415l-979,-1470l-192,0l980,1470z""/>
<glyph unicode=""&#x20AC;"" horiz-adv-x=""1284"" d=""M530,934l547,0l-33,-168l-552,0C490,745 489,725 489,704C489,684 490,661 492,635l536,0l-33,-168l-471,0C555,390 593,334 638,298C682,262 741,244 815,244C863,244 911,252 959,267C1007,282 1068,306 1141,340l0,-268C1008,12 885,-18 770,-18C624,-18 503,24 406,109C309,193 242,312 205,467l-172,0l33,168l116,0C182,702 183,745 184,766l-151,0l33,168l145,0C252,1086 326,1204 432,1289C537,1373 668,1415 825,1415C922,1415 1035,1401 1165,1372l-43,-260C1017,1139 925,1153 846,1153C689,1153 583,1080 530,934z""/>
<glyph unicode=""&#x2039;"" horiz-adv-x=""662"" d=""M315,469l326,-455l-293,0l-330,455l330,455l293,0z""/>
<glyph unicode=""&#x203A;"" horiz-adv-x=""662"" d=""M346,469l-326,455l293,0l330,-455l-330,-455l-293,0z""/>
<glyph unicode=""&#xFB01;"" horiz-adv-x=""1217"" d=""M719,1163C690,1167 666,1169 645,1169C578,1169 531,1155 504,1126C477,1097 463,1036 463,944l190,0l0,-203l-190,0l0,-741l-297,0l0,741l-125,0l0,203l125,0C166,1258 306,1415 586,1415C636,1415 680,1409 719,1397M780,1255C780,1299 796,1337 828,1368C860,1399 898,1415 942,1415C986,1415 1024,1399 1055,1368C1086,1337 1102,1299 1102,1255C1102,1211 1086,1173 1055,1142C1024,1110 986,1094 942,1094C898,1094 860,1110 828,1142C796,1174 780,1212 780,1255M1090,944l0,-944l-297,0l0,944z""/>
<glyph unicode=""&#xFB02;"" horiz-adv-x=""1217"" d=""M653,944l0,-203l-190,0l0,-741l-297,0l0,741l-123,0l0,203l123,0C166,1258 306,1415 586,1415C636,1415 680,1409 719,1397l0,-234C690,1167 666,1169 645,1169C578,1169 531,1155 504,1126C477,1097 463,1036 463,944M1090,1397l0,-1397l-297,0l0,1397z""/>
<glyph unicode=""&#x2021;"" horiz-adv-x=""1044"" d=""M911,145C920,148 929,149 936,149C974,149 993,105 993,18C993,-75 979,-121 952,-121C948,-121 940,-120 928,-117l-355,80l84,-352C660,-399 661,-408 661,-415C661,-452 615,-471 522,-471C430,-471 384,-452 384,-415C384,-408 385,-399 387,-389l82,352l-352,-80C108,-119 100,-120 94,-120C78,-120 67,-110 61,-90C54,-71 51,-32 51,25C51,108 70,149 107,149C115,149 124,148 133,145l336,-79l-82,407l82,406l-336,-80C122,796 112,794 105,794C69,794 51,839 51,930C51,1027 67,1072 100,1065l369,-84l-82,352C385,1343 384,1352 384,1359C384,1396 430,1415 522,1415C615,1415 662,1396 662,1359C662,1352 661,1346 659,1339l-86,-358l355,80C938,1063 946,1064 951,1064C979,1064 993,1017 993,922C993,837 974,795 937,795C930,795 921,796 911,799l-338,80l84,-406l-84,-407z""/>
<glyph unicode=""&#x2219;"" horiz-adv-x=""555"" d=""M98,707C98,756 116,798 151,833C186,868 227,885 276,885C325,885 368,868 403,833C438,798 455,756 455,707C455,658 438,616 403,581C368,546 325,528 276,528C227,528 186,546 151,581C116,616 98,658 98,707z""/>
<glyph unicode=""&#x201A;"" horiz-adv-x=""555"" d=""M102,-346l0,63C165,-222 197,-145 197,-52C197,-41 196,-29 195,-18C125,9 90,58 90,131C90,178 106,216 138,247C170,278 210,293 258,293C318,293 367,271 406,228C444,184 463,128 463,59C463,-40 429,-129 362,-208C294,-287 207,-333 102,-346z""/>
<glyph unicode=""&#x201E;"" horiz-adv-x=""1153"" d=""M688,-346l0,63C751,-222 782,-145 782,-52C782,-41 781,-29 780,-18C711,9 676,58 676,131C676,178 692,216 724,247C755,278 795,293 844,293C904,293 953,271 992,228C1030,184 1049,128 1049,59C1049,-40 1015,-129 947,-208C879,-287 793,-333 688,-346M117,-346l0,63C180,-222 211,-145 211,-52C211,-41 210,-29 209,-18C139,9 104,58 104,131C104,178 120,216 152,247C184,278 224,293 272,293C332,293 381,271 420,228C458,184 477,128 477,59C477,-40 443,-129 376,-208C308,-287 222,-333 117,-346z""/>
<glyph unicode=""&#x2030;"" horiz-adv-x=""2218"" d=""M375,1415C466,1415 543,1383 608,1319C673,1255 705,1177 705,1085C705,995 673,918 608,853C543,788 465,756 375,756C284,756 207,788 142,853C77,918 45,995 45,1085C45,1177 77,1255 142,1319C206,1383 284,1415 375,1415M375,932C416,932 450,947 478,977C506,1006 520,1042 520,1085C520,1127 506,1163 477,1194C448,1224 414,1239 375,1239C334,1239 299,1224 271,1195C243,1166 229,1129 229,1085C229,1044 244,1009 273,978C302,947 336,932 375,932M1303,1415l-977,-1431l-170,0l983,1431M1843,641C1934,641 2012,609 2077,545C2141,480 2173,402 2173,311C2173,221 2141,144 2076,79C2011,14 1934,-18 1843,-18C1753,-18 1676,14 1611,79C1546,144 1513,221 1513,311C1513,402 1545,480 1610,545C1675,609 1752,641 1843,641M1843,158C1884,158 1918,173 1947,203C1975,232 1989,268 1989,311C1989,353 1975,389 1946,420C1917,450 1883,465 1843,465C1802,465 1768,450 1740,421C1712,392 1698,355 1698,311C1698,270 1712,235 1741,204C1770,173 1804,158 1843,158M1087,641C1178,641 1256,609 1321,545C1385,480 1417,402 1417,311C1417,221 1385,144 1320,79C1255,14 1178,-18 1087,-18C997,-18 920,14 855,79C790,144 758,221 758,311C758,402 790,480 855,545C919,609 996,641 1087,641M1087,158C1128,158 1162,173 1191,203C1219,232 1233,268 1233,311C1233,353 1219,389 1190,420C1161,450 1127,465 1087,465C1046,465 1012,450 984,421C956,392 942,355 942,311C942,270 957,235 986,204C1015,173 1048,158 1087,158z""/>
<glyph unicode=""&#xC2;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M940,1875l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xCA;"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336M790,1875l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xC1;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M1127,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xCB;"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336M888,1858C934,1858 973,1842 1005,1811C1036,1779 1052,1740 1052,1695C1052,1649 1036,1610 1005,1578C974,1546 935,1530 888,1530C843,1530 804,1546 773,1578C741,1610 725,1649 725,1695C725,1740 741,1779 773,1811C804,1842 843,1858 888,1858M411,1858C457,1859 496,1843 528,1811C559,1779 575,1740 575,1695C575,1649 559,1610 528,1578C497,1546 458,1530 411,1530C366,1530 327,1546 296,1578C264,1610 248,1649 248,1695C248,1740 264,1779 296,1811C327,1842 366,1858 411,1858z""/>
<glyph unicode=""&#xC8;"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336M326,1831l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xCD;"" horiz-adv-x=""682"" d=""M516,1397l0,-1397l-348,0l0,1397M668,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xCE;"" horiz-adv-x=""682"" d=""M516,1397l0,-1397l-348,0l0,1397M481,1875l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xCF;"" horiz-adv-x=""682"" d=""M516,1397l0,-1397l-348,0l0,1397M579,1858C625,1858 664,1842 696,1811C727,1779 743,1740 743,1695C743,1649 727,1610 696,1578C665,1546 626,1530 579,1530C534,1530 495,1546 464,1578C432,1610 416,1649 416,1695C416,1740 432,1779 464,1811C495,1842 534,1858 579,1858M102,1858C148,1859 187,1843 219,1811C250,1779 266,1740 266,1695C266,1649 250,1610 219,1578C188,1546 149,1530 102,1530C57,1530 18,1546 -13,1578C-45,1610 -61,1649 -61,1695C-61,1740 -45,1779 -13,1811C18,1842 57,1858 102,1858z""/>
<glyph unicode=""&#xCC;"" horiz-adv-x=""682"" d=""M516,1397l0,-1397l-348,0l0,1397M16,1831l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xD3;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M1223,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xD4;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M1036,1875l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xD2;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M571,1831l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#xDA;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M1170,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xDB;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M983,1875l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#xD9;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M518,1831l338,0l152,-299l-152,0z""/>
<glyph unicode=""&#x131;"" horiz-adv-x=""555"" d=""M426,944l0,-944l-297,0l0,944z""/>
<glyph unicode=""&#x2C6;"" horiz-adv-x=""682"" d=""M481,1428l203,-359l-182,0l-160,179l-164,-179l-180,0l203,359z""/>
<glyph unicode=""&#x2DC;"" horiz-adv-x=""682"" d=""M600,1374l139,0C725,1168 645,1065 500,1065C458,1065 399,1084 323,1121C247,1158 196,1176 170,1176C118,1176 87,1140 78,1069l-135,0C-50,1167 -24,1242 19,1295C62,1348 118,1374 186,1374C230,1374 288,1356 361,1319C433,1282 483,1264 510,1264C558,1264 588,1301 600,1374z""/>
<glyph unicode=""&#x2C9;"" horiz-adv-x=""682"" d=""M737,1317l0,-199l-792,0l0,199z""/>
<glyph unicode=""&#x2D8;"" horiz-adv-x=""682"" d=""M512,1477l203,0C705,1385 665,1304 595,1233C524,1162 441,1126 346,1126C251,1126 170,1156 103,1217C36,1278 -10,1364 -33,1477l205,0C200,1392 256,1350 340,1350C431,1350 488,1392 512,1477z""/>
<glyph unicode=""&#x2D9;"" horiz-adv-x=""682"" d=""M342,1411C388,1411 427,1395 459,1364C490,1332 506,1293 506,1248C506,1202 490,1163 459,1131C428,1099 389,1083 342,1083C297,1083 258,1099 227,1131C195,1163 179,1202 179,1248C179,1293 195,1332 227,1364C258,1395 297,1411 342,1411z""/>
<glyph unicode=""&#x2DA;"" horiz-adv-x=""682"" d=""M133,1249C133,1306 154,1355 195,1396C236,1436 285,1456 342,1456C399,1456 448,1436 489,1396C529,1355 549,1306 549,1249C549,1192 529,1143 488,1102C447,1061 399,1040 342,1040C285,1040 236,1061 195,1102C154,1143 133,1192 133,1249M455,1249C455,1280 444,1306 422,1328C399,1349 373,1360 342,1360C311,1360 285,1349 264,1328C242,1306 231,1280 231,1249C231,1218 242,1192 264,1170C285,1148 311,1137 342,1137C373,1137 399,1148 422,1170C444,1192 455,1218 455,1249z""/>
<glyph unicode=""&#xB8;"" horiz-adv-x=""682"" d=""M479,16l-26,-63C538,-78 580,-128 580,-197C580,-248 560,-289 521,-320C482,-351 427,-367 356,-367C290,-367 240,-361 205,-348l0,104C308,-247 360,-226 360,-180C360,-142 332,-123 276,-123l80,139z""/>
<glyph unicode=""&#x2DD;"" horiz-adv-x=""682"" d=""M774,1384l-268,-299l-146,0l107,299M393,1384l-268,-299l-145,0l106,299z""/>
<glyph unicode=""&#x2DB;"" horiz-adv-x=""682"" d=""M520,-190l0,-164C471,-375 418,-385 362,-385C298,-385 247,-364 210,-323C173,-282 154,-225 154,-154C154,-89 171,-32 205,18l162,0C345,-32 334,-70 334,-96C334,-129 344,-156 365,-177C386,-198 411,-209 442,-209C457,-209 483,-203 520,-190z""/>
<glyph unicode=""&#x2C7;"" horiz-adv-x=""682"" d=""M201,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x141;"" horiz-adv-x=""1260"" d=""M1221,256l0,-256l-1065,0l0,446l-193,-157l0,280l193,158l0,670l348,0l0,-414l287,236l0,-283l-287,-236l0,-444z""/>
<glyph unicode=""&#x142;"" horiz-adv-x=""555"" d=""M426,1397l0,-459l158,131l0,-238l-158,-131l0,-700l-297,0l0,489l-158,-131l0,236l158,131l0,672z""/>
<glyph unicode=""&#x160;"" horiz-adv-x=""1237"" d=""M1104,1307l0,-273C919,1117 771,1159 661,1159C598,1159 549,1149 512,1128C475,1107 456,1079 456,1044C456,1017 469,992 496,968C523,944 588,913 691,875C794,837 877,802 939,769C1001,736 1055,690 1100,631C1145,572 1167,497 1167,406C1167,277 1118,174 1020,97C921,20 792,-18 631,-18C458,-18 290,27 127,117l0,301C220,355 303,310 375,281C447,252 523,238 604,238C741,238 809,281 809,367C809,396 795,424 768,450C741,476 675,508 571,545C466,582 384,617 323,649C262,681 209,727 165,786C120,845 98,921 98,1014C98,1134 146,1231 243,1305C340,1378 469,1415 631,1415C780,1415 938,1379 1104,1307M480,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x161;"" horiz-adv-x=""874"" d=""M772,893l0,-211C659,751 554,786 457,786C386,786 350,762 350,713C350,700 358,688 373,676C388,663 445,637 545,598C645,558 716,512 757,460C798,408 819,351 819,289C819,191 786,115 719,62C652,9 557,-18 434,-18C307,-18 195,5 96,51l0,209C217,207 319,180 401,180C497,180 545,203 545,250C545,268 536,285 519,301C501,317 441,345 340,384C239,423 169,467 132,514C95,561 76,614 76,672C76,758 112,828 184,882C255,936 350,963 467,963C580,963 681,940 772,893M297,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x17D;"" horiz-adv-x=""1430"" d=""M1364,1397l-762,-1141l762,0l0,-256l-1356,0l768,1141l-715,0l0,256M576,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x17E;"" horiz-adv-x=""1067"" d=""M86,944l922,0l-484,-741l484,0l0,-203l-969,0l485,741l-438,0M394,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#xA6;"" horiz-adv-x=""575"" d=""M176,700l0,697l223,0l0,-697M176,-453l0,813l223,0l0,-813z""/>
<glyph unicode=""&#xAD;"" horiz-adv-x=""683"" d=""M608,611l0,-274l-533,0l0,274z""/>
<glyph unicode=""&#xAF;"" horiz-adv-x=""1024"" d=""M-12,1600l1048,0l0,-102l-1048,0z""/>
<glyph unicode=""&#xD0;"" horiz-adv-x=""1642"" d=""M154,1397l616,0C1008,1397 1196,1332 1335,1202C1473,1071 1542,903 1542,696C1542,479 1472,309 1332,186C1191,62 991,0 731,0l-577,0l0,606l-170,0l0,187l170,0M502,1141l0,-348l311,0l0,-187l-311,0l0,-350l227,0C876,256 989,297 1067,378C1145,459 1184,565 1184,698C1184,835 1145,944 1066,1023C987,1102 873,1141 725,1141z""/>
<glyph unicode=""&#xF0;"" horiz-adv-x=""1260"" d=""M1098,1288l-205,-71C1070,1039 1159,818 1159,553C1159,367 1108,225 1007,128C906,31 780,-18 629,-18C472,-18 343,28 244,119C144,210 94,328 94,473C94,610 140,726 232,821C324,916 436,963 569,963C649,963 725,938 797,889l4,2C775,959 716,1035 625,1120l-297,-104l-45,123l225,80C469,1248 433,1269 401,1284l187,131C658,1386 719,1352 772,1315l281,100M629,158C771,158 842,265 842,479C842,684 771,786 629,786C560,786 507,759 469,705C431,651 412,574 412,473C412,263 484,158 629,158z""/>
<glyph unicode=""&#xDD;"" horiz-adv-x=""1450"" d=""M551,0l0,653l-551,744l422,0l303,-439l305,439l420,0l-551,-744l0,-653M1053,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xFD;"" horiz-adv-x=""1044"" d=""M1042,944l-632,-1397l-316,0l277,607l-369,790l317,0l199,-485l213,485M850,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#xDE;"" horiz-adv-x=""1343"" d=""M170,1397l348,0l0,-281l275,0C934,1116 1046,1075 1130,993C1213,911 1255,814 1255,702C1255,586 1214,487 1132,405C1049,322 937,281 795,281l-277,0l0,-281l-348,0M518,860l0,-323l162,0C825,537 897,591 897,698C897,759 878,801 839,825C800,848 734,860 641,860z""/>
<glyph unicode=""&#xFE;"" horiz-adv-x=""1194"" d=""M414,1397l0,-535C493,929 583,963 684,963C807,963 910,917 995,824C1080,731 1122,618 1122,483C1122,332 1080,210 995,119C910,28 797,-18 657,-18C570,-18 489,4 414,47l0,-518l-297,0l0,1868M414,676l0,-434C470,201 527,180 586,180C658,180 715,206 758,258C800,309 821,379 821,467C821,557 802,627 763,677C724,727 669,752 598,752C539,752 477,727 414,676z""/>
<glyph unicode=""&#xD7;"" horiz-adv-x=""1196"" d=""M111,420l303,303l-303,303l184,184l303,-303l303,303l184,-184l-303,-303l303,-303l-184,-184l-303,303l-303,-303z""/>
<glyph unicode=""&#xB9;"" horiz-adv-x=""682"" d=""M449,1415l0,-735l-213,0l0,735z""/>
<glyph unicode=""&#xB2;"" horiz-adv-x=""682"" d=""M637,836l0,-156l-569,0l0,27C186,838 269,937 316,1006C363,1075 387,1132 387,1178C387,1205 378,1228 359,1247C340,1265 317,1274 289,1274C230,1274 159,1239 78,1169l0,174C164,1391 244,1415 319,1415C404,1415 472,1396 521,1359C570,1322 594,1271 594,1208C594,1124 522,1000 377,836z""/>
<glyph unicode=""&#xB3;"" horiz-adv-x=""682"" d=""M242,987l0,137C317,1131 354,1159 354,1206C354,1229 344,1247 325,1262C305,1277 279,1284 248,1284C204,1284 150,1269 86,1239l0,141C145,1403 212,1415 287,1415C368,1415 435,1398 486,1365C537,1331 563,1288 563,1235C563,1165 524,1110 446,1069C539,1032 586,972 586,887C586,823 558,771 501,731C444,690 369,670 276,670C215,670 145,686 66,719l0,155C135,831 198,809 256,809C290,809 317,817 338,832C359,847 369,868 369,893C369,950 327,981 242,987z""/>
<glyph unicode=""&#xBD;"" horiz-adv-x=""1835"" d=""M1378,1415l-979,-1470l-192,0l979,1470M455,1415l0,-735l-213,0l0,735M1669,156l0,-156l-569,0l0,27C1218,158 1301,257 1348,326C1395,395 1419,452 1419,498C1419,525 1410,548 1392,567C1373,585 1350,594 1321,594C1262,594 1191,559 1110,489l0,175C1196,711 1277,735 1352,735C1437,735 1504,716 1553,679C1602,642 1626,591 1626,528C1626,444 1554,320 1409,156z""/>
<glyph unicode=""&#xBC;"" horiz-adv-x=""1835"" d=""M455,1415l0,-735l-213,0l0,735M1378,1415l-979,-1470l-192,0l979,1470M1503,737l0,-434l58,0l0,-127l-58,0l0,-176l-207,0l0,176l-319,0l0,121l393,440M1296,463l-145,-160l145,0z""/>
<glyph unicode=""&#xBE;"" horiz-adv-x=""1835"" d=""M248,987l0,137C323,1131 360,1159 360,1206C360,1229 350,1247 331,1262C311,1277 285,1284 254,1284C210,1284 156,1269 92,1239l0,141C151,1403 218,1415 293,1415C374,1415 441,1398 492,1365C543,1331 569,1288 569,1235C569,1165 530,1110 453,1069C546,1032 592,972 592,887C592,823 564,771 507,731C450,690 375,670 283,670C222,670 151,686 72,719l0,155C141,831 205,809 262,809C296,809 323,817 344,832C365,847 375,868 375,893C375,950 333,981 248,987M1425,1415l-979,-1470l-192,0l979,1470M1550,737l0,-434l58,0l0,-127l-58,0l0,-176l-207,0l0,176l-319,0l0,121l393,440M1343,463l-145,-160l145,0z""/>
<glyph unicode=""&#xB7;"" horiz-adv-x=""683"" d=""M898,710C898,665 882,627 850,595C818,562 779,546 734,546C689,546 650,562 618,594C585,626 569,665 569,710C569,755 585,794 618,827C650,859 689,875 734,875C779,875 817,859 850,827C882,794 898,755 898,710z""/>
<glyph unicode=""&#x102;"" horiz-adv-x=""1599"" d=""M973,1397l620,-1397l-374,0l-115,276l-625,0l-104,-276l-369,0l563,1397M1001,520l-225,543l-205,-543M971,1924l203,0C1164,1832 1124,1751 1054,1680C983,1609 900,1573 805,1573C710,1573 629,1603 562,1664C495,1725 449,1811 426,1924l205,0C659,1839 715,1797 799,1797C890,1797 947,1839 971,1924z""/>
<glyph unicode=""&#x103;"" horiz-adv-x=""1087"" d=""M1087,233l0,-165C988,11 896,-18 809,-18C716,-18 660,17 643,88C540,17 436,-18 332,-18C259,-18 196,5 145,52C94,98 68,152 68,213C68,295 95,361 148,412C201,462 361,527 629,606C636,711 581,764 465,764C339,764 223,711 117,606l0,242C241,925 377,963 526,963C793,963 926,867 926,676l0,-430C926,202 942,180 975,180C1000,180 1037,198 1087,233M629,229l0,228C530,422 462,391 425,364C388,336 369,301 369,258C369,231 378,207 397,188C416,168 438,158 465,158C521,158 576,182 629,229M715,1477l203,0C908,1385 868,1304 798,1233C727,1162 644,1126 549,1126C454,1126 373,1156 306,1217C239,1278 193,1364 170,1477l205,0C403,1392 459,1350 543,1350C634,1350 691,1392 715,1477z""/>
<glyph unicode=""&#x104;"" horiz-adv-x=""1599"" d=""M1595,0l-209,0C1369,-41 1361,-73 1361,-96C1361,-128 1371,-155 1391,-176C1411,-198 1437,-209 1469,-209C1484,-209 1510,-203 1547,-190l0,-164C1498,-375 1445,-385 1389,-385C1324,-385 1273,-364 1236,-322C1199,-281 1181,-225 1181,-154C1181,-97 1194,-45 1221,0l-117,276l-625,0l-104,-276l-369,0l565,1396l402,0M1003,520l-227,541l-205,-541z""/>
<glyph unicode=""&#x105;"" horiz-adv-x=""1088"" d=""M909,-5C894,-42 887,-71 887,-93C887,-127 899,-153 922,-172C945,-191 972,-201 1003,-201C1018,-201 1042,-198 1074,-191l0,-163C1023,-375 970,-386 916,-386C849,-386 797,-365 760,-323C723,-282 704,-228 704,-161C704,-108 716,-58 740,-10C687,3 655,35 643,87C540,17 436,-18 331,-18C257,-18 195,5 144,52C93,98 67,157 67,229C67,272 78,312 99,349C120,386 150,417 189,444C228,470 290,497 374,524l255,83C629,658 617,698 594,725C571,752 528,766 466,766C339,766 223,713 117,607l0,240C242,924 379,962 529,962C625,962 704,948 767,920C830,892 872,858 894,818C916,777 927,717 927,638l0,-375C927,234 931,215 938,204C945,193 957,188 974,188C997,188 1036,203 1089,233l0,-166C1038,36 978,12 909,-5M629,457l-66,-26C488,403 437,377 410,353C383,328 369,297 369,260C369,229 379,205 399,187C419,168 440,159 463,159C523,159 578,182 629,229z""/>
<glyph unicode=""&#x106;"" horiz-adv-x=""1579"" d=""M1458,426l0,-305C1283,28 1100,-18 909,-18C661,-18 464,51 317,188C170,325 96,493 96,694C96,895 174,1065 329,1205C484,1345 686,1415 936,1415C1135,1415 1304,1374 1442,1292l0,-311C1273,1076 1112,1124 961,1124C812,1124 691,1084 597,1005C502,925 455,823 455,700C455,576 502,474 595,393C688,312 807,272 952,272C1024,272 1093,282 1160,301C1227,320 1326,361 1458,426M1230,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x107;"" horiz-adv-x=""1024"" d=""M948,258l0,-213C832,3 726,-18 629,-18C463,-18 330,27 231,116C132,205 82,322 82,469C82,612 132,731 233,824C333,917 461,963 616,963C716,963 822,942 934,901l0,-221C852,722 771,743 692,743C603,743 532,718 479,669C426,620 399,554 399,471C399,388 425,321 477,270C529,219 597,193 682,193C745,193 833,215 948,258M904,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x10C;"" horiz-adv-x=""1579"" d=""M1458,426l0,-305C1283,28 1100,-18 909,-18C661,-18 464,51 317,188C170,325 96,493 96,694C96,895 174,1065 329,1205C484,1345 686,1415 936,1415C1135,1415 1304,1374 1442,1292l0,-311C1273,1076 1112,1124 961,1124C812,1124 691,1084 597,1005C502,925 455,823 455,700C455,576 502,474 595,393C688,312 807,272 952,272C1024,272 1093,282 1160,301C1227,320 1326,361 1458,426M763,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x10D;"" horiz-adv-x=""1024"" d=""M948,258l0,-213C832,3 726,-18 629,-18C463,-18 330,27 231,116C132,205 82,322 82,469C82,612 132,731 233,824C333,917 461,963 616,963C716,963 822,942 934,901l0,-221C852,722 771,743 692,743C603,743 532,718 479,669C426,620 399,554 399,471C399,388 425,321 477,270C529,219 597,193 682,193C745,193 833,215 948,258M437,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x10E;"" horiz-adv-x=""1642"" d=""M154,1397l616,0C1008,1397 1196,1332 1335,1202C1473,1071 1542,903 1542,696C1542,479 1472,309 1332,186C1191,62 991,0 731,0l-577,0M502,1141l0,-885l227,0C876,256 989,297 1067,378C1145,459 1184,565 1184,698C1184,835 1145,944 1066,1023C987,1102 873,1141 725,1141M608,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x10F;"" horiz-adv-x=""1429"" d=""M1073,1397l0,-1397l-297,0l0,76C700,13 613,-18 514,-18C388,-18 283,29 199,122C114,215 72,332 72,473C72,617 115,735 201,826C286,917 396,963 530,963C613,963 695,943 776,903l0,494M776,258l0,451C722,746 667,764 610,764C535,764 477,738 436,686C394,634 373,561 373,467C373,380 394,311 435,259C476,206 531,180 600,180C661,180 719,206 776,258M1151,890C1207,953 1235,1023 1235,1098C1235,1105 1234,1114 1233,1125C1171,1153 1140,1200 1140,1267C1140,1309 1156,1344 1187,1372C1218,1400 1253,1414 1294,1414C1343,1414 1385,1395 1421,1357C1457,1318 1475,1266 1475,1201C1475,1113 1445,1033 1385,961C1325,888 1247,846 1151,834z""/>
<glyph unicode=""&#x110;"" horiz-adv-x=""1643"" d=""M154,605l-171,0l0,187l171,0l0,605l568,0C997,1397 1204,1329 1342,1193C1479,1056 1548,892 1548,701C1548,500 1480,333 1344,200C1207,67 991,0 696,0l-542,0M506,605l0,-340l141,0C756,265 836,271 889,284C941,297 991,322 1039,359C1087,396 1124,444 1149,504C1174,563 1187,630 1187,703C1187,805 1164,891 1119,960C1073,1029 1016,1075 947,1098C878,1120 790,1131 683,1131l-177,0l0,-339l306,0l0,-187z""/>
<glyph unicode=""&#x111;"" horiz-adv-x=""1195"" d=""M1071,1244l113,0l0,-148l-113,0l0,-1096l-294,0l0,77C702,13 614,-19 514,-19C396,-19 293,26 206,115C119,204 75,323 75,473C75,620 118,739 205,828C291,917 399,962 530,962C617,962 699,942 777,902l0,194l-180,0l0,148l180,0l0,153l294,0M777,257l0,451C725,745 670,764 612,764C549,764 494,741 448,696C402,651 379,574 379,466C379,372 401,303 446,258C490,213 542,191 603,191C663,191 721,213 777,257z""/>
<glyph unicode=""&#x118;"" horiz-adv-x=""1300"" d=""M1190,0l-162,0C1011,-41 1003,-73 1003,-96C1003,-128 1013,-155 1033,-176C1053,-198 1079,-209 1111,-209C1126,-209 1152,-203 1189,-190l0,-164C1140,-375 1087,-385 1031,-385C966,-385 915,-364 878,-322C841,-281 823,-225 823,-154C823,-97 836,-45 862,0l-703,0l0,1396l1016,0l0,-260l-663,0l0,-282l633,0l0,-262l-633,0l0,-336l678,0z""/>
<glyph unicode=""&#x119;"" horiz-adv-x=""1130"" d=""M885,24C861,-27 849,-66 849,-95C849,-128 861,-153 885,-172C908,-191 935,-201 965,-201C981,-201 1005,-198 1036,-191l0,-163C985,-375 933,-386 879,-386C816,-386 765,-366 726,-325C687,-284 668,-229 668,-158C668,-107 679,-59 701,-15C678,-17 656,-18 633,-18C528,-18 438,-4 362,25C285,53 219,107 162,186C105,265 76,359 76,470C76,614 125,732 222,824C319,916 436,962 575,962C682,962 776,935 856,880C935,825 989,760 1018,683C1046,606 1060,520 1060,425l-676,0C387,344 416,281 470,235C524,188 599,165 694,165C818,165 935,209 1045,296l0,-190C999,73 946,46 885,24M389,579l409,0C792,640 771,689 735,727C698,764 651,783 593,783C536,783 489,764 453,727C416,689 395,640 389,579z""/>
<glyph unicode=""&#x11A;"" horiz-adv-x=""1300"" d=""M1190,256l0,-256l-1030,0l0,1397l1016,0l0,-256l-668,0l0,-293l637,0l0,-256l-637,0l0,-336M510,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x11B;"" horiz-adv-x=""1130"" d=""M1044,293l0,-187C931,23 790,-18 623,-18C457,-18 324,27 225,116C126,205 76,322 76,469C76,613 123,731 216,824C309,917 428,963 573,963C718,963 838,914 931,817C1024,719 1067,588 1061,424l-678,0C389,339 419,273 474,227C529,181 602,158 694,158C805,158 922,203 1044,293M389,580l410,0C783,717 715,786 594,786C472,786 404,717 389,580M426,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x139;"" horiz-adv-x=""1260"" d=""M1221,256l0,-256l-1065,0l0,1397l348,0l0,-1141M1035,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x13A;"" horiz-adv-x=""555"" d=""M426,1397l0,-1397l-297,0l0,1397M668,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x13D;"" horiz-adv-x=""1260"" d=""M1221,256l0,-256l-1065,0l0,1397l348,0l0,-1141M521,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x13E;"" horiz-adv-x=""555"" d=""M426,1397l0,-1397l-297,0l0,1397M517,890C573,953 601,1023 601,1098C601,1105 600,1114 599,1125C537,1153 506,1200 506,1267C506,1309 522,1344 553,1372C584,1400 619,1414 660,1414C709,1414 751,1395 787,1357C823,1318 841,1266 841,1201C841,1113 811,1033 751,961C691,888 613,846 517,834z""/>
<glyph unicode=""&#x143;"" horiz-adv-x=""1729"" d=""M1571,1397l0,-1397l-293,0l-772,895l0,-895l-348,0l0,1397l319,0l746,-854l0,854M1212,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x144;"" horiz-adv-x=""1194"" d=""M420,944l0,-135C507,912 610,963 727,963C825,963 907,932 973,869C1038,806 1071,717 1071,600l0,-600l-297,0l0,575C774,701 725,764 627,764C557,764 488,712 420,608l0,-608l-297,0l0,944M924,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x147;"" horiz-adv-x=""1729"" d=""M1571,1397l0,-1397l-293,0l-772,895l0,-895l-348,0l0,1397l319,0l746,-854l0,854M725,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x148;"" horiz-adv-x=""1194"" d=""M420,944l0,-135C507,912 610,963 727,963C825,963 907,932 973,869C1038,806 1071,717 1071,600l0,-600l-297,0l0,575C774,701 725,764 627,764C557,764 488,712 420,608l0,-608l-297,0l0,944M459,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x150;"" horiz-adv-x=""1792"" d=""M897,1415C1135,1415 1328,1343 1475,1200C1622,1056 1696,889 1696,698C1696,505 1621,337 1472,195C1323,53 1131,-18 897,-18C660,-18 468,53 319,194C170,335 96,503 96,698C96,889 170,1057 318,1200C465,1343 658,1415 897,1415M897,272C1029,272 1135,311 1216,390C1297,468 1337,571 1337,698C1337,826 1297,929 1216,1007C1135,1085 1029,1124 897,1124C764,1124 658,1085 577,1007C496,929 455,826 455,698C455,571 496,468 577,390C658,311 764,272 897,272M1396,1831l-268,-299l-146,0l107,299M1015,1831l-268,-299l-145,0l106,299z""/>
<glyph unicode=""&#x151;"" horiz-adv-x=""1217"" d=""M610,963C758,963 884,920 987,833C1090,746 1141,626 1141,471C1141,316 1089,195 984,110C879,25 755,-18 610,-18C453,-18 325,28 226,119C126,210 76,328 76,473C76,621 127,740 229,829C330,918 457,963 610,963M610,158C752,158 823,265 823,479C823,684 752,786 610,786C542,786 489,759 451,705C412,651 393,574 393,473C393,263 465,158 610,158M1112,1384l-268,-299l-146,0l107,299M731,1384l-268,-299l-145,0l106,299z""/>
<glyph unicode=""&#x154;"" horiz-adv-x=""1386"" d=""M156,1397l624,0C915,1397 1025,1358 1110,1280C1195,1201 1237,1106 1237,995C1237,841 1157,727 997,653C1068,622 1135,536 1198,396C1261,256 1321,124 1380,0l-383,0C974,46 935,134 879,263C822,392 774,475 734,512C693,549 650,567 604,567l-100,0l0,-567l-348,0M504,1141l0,-318l184,0C747,823 793,837 828,865C862,892 879,932 879,983C879,1088 813,1141 680,1141M1024,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x155;"" horiz-adv-x=""918"" d=""M428,944l0,-270l4,0C518,867 618,963 733,963C788,963 853,934 930,877l-82,-263C775,661 716,684 672,684C603,684 546,642 499,557C452,472 428,422 428,408l0,-408l-297,0l0,944M856,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x158;"" horiz-adv-x=""1386"" d=""M156,1397l624,0C915,1397 1025,1358 1110,1280C1195,1201 1237,1106 1237,995C1237,841 1157,727 997,653C1068,622 1135,536 1198,396C1261,256 1321,124 1380,0l-383,0C974,46 935,134 879,263C822,392 774,475 734,512C693,549 650,567 604,567l-100,0l0,-567l-348,0M504,1141l0,-318l184,0C747,823 793,837 828,865C862,892 879,932 879,983C879,1088 813,1141 680,1141M536,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x159;"" horiz-adv-x=""918"" d=""M428,944l0,-270l4,0C518,867 618,963 733,963C788,963 853,934 930,877l-82,-263C775,661 716,684 672,684C603,684 546,642 499,557C452,472 428,422 428,408l0,-408l-297,0l0,944M392,1069l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x15A;"" horiz-adv-x=""1237"" d=""M1104,1307l0,-273C919,1117 771,1159 661,1159C598,1159 549,1149 512,1128C475,1107 456,1079 456,1044C456,1017 469,992 496,968C523,944 588,913 691,875C794,837 877,802 939,769C1001,736 1055,690 1100,631C1145,572 1167,497 1167,406C1167,277 1118,174 1020,97C921,20 792,-18 631,-18C458,-18 290,27 127,117l0,301C220,355 303,310 375,281C447,252 523,238 604,238C741,238 809,281 809,367C809,396 795,424 768,450C741,476 675,508 571,545C466,582 384,617 323,649C262,681 209,727 165,786C120,845 98,921 98,1014C98,1134 146,1231 243,1305C340,1378 469,1415 631,1415C780,1415 938,1379 1104,1307M973,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x15B;"" horiz-adv-x=""874"" d=""M772,893l0,-211C659,751 554,786 457,786C386,786 350,762 350,713C350,700 358,688 373,676C388,663 445,637 545,598C645,558 716,512 757,460C798,408 819,351 819,289C819,191 786,115 719,62C652,9 557,-18 434,-18C307,-18 195,5 96,51l0,209C217,207 319,180 401,180C497,180 545,203 545,250C545,268 536,285 519,301C501,317 441,345 340,384C239,423 169,467 132,514C95,561 76,614 76,672C76,758 112,828 184,882C255,936 350,963 467,963C580,963 681,940 772,893M775,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x162;"" horiz-adv-x=""1473"" d=""M1417,1397l0,-256l-506,0l0,-1141l-348,0l0,1141l-506,0l0,256M575,-598C631,-535 659,-465 659,-390C659,-383 658,-374 657,-363C595,-335 564,-288 564,-221C564,-179 580,-144 611,-116C642,-88 677,-74 718,-74C767,-74 809,-93 845,-131C881,-170 899,-222 899,-287C899,-375 869,-455 809,-527C749,-600 671,-642 575,-654z""/>
<glyph unicode=""&#x163;"" horiz-adv-x=""831"" d=""M801,944l0,-203l-344,0l0,-389C457,305 470,269 496,242C521,215 557,201 602,201C663,201 730,219 801,256l0,-215C706,2 615,-18 526,-18C410,-18 320,12 256,72C192,132 160,220 160,336l0,405l-135,0l0,58l395,442l37,0l0,-297M331,-598C387,-535 415,-465 415,-390C415,-383 414,-374 413,-363C351,-335 320,-288 320,-221C320,-179 336,-144 367,-116C398,-88 433,-74 474,-74C523,-74 565,-93 601,-131C637,-170 655,-222 655,-287C655,-375 625,-455 565,-527C505,-600 427,-642 331,-654z""/>
<glyph unicode=""&#x164;"" horiz-adv-x=""1473"" d=""M1417,1397l0,-256l-506,0l0,-1141l-348,0l0,1141l-506,0l0,256M594,1516l-203,359l182,0l160,-178l164,178l180,0l-203,-359z""/>
<glyph unicode=""&#x165;"" horiz-adv-x=""1152"" d=""M801,944l0,-203l-344,0l0,-389C457,305 470,269 496,242C521,215 557,201 602,201C663,201 730,219 801,256l0,-215C706,2 615,-18 526,-18C410,-18 320,12 256,72C192,132 160,220 160,336l0,405l-135,0l0,58l395,442l37,0l0,-297M881,890C937,953 965,1023 965,1098C965,1105 964,1114 963,1125C901,1153 870,1200 870,1267C870,1309 886,1344 917,1372C948,1400 983,1414 1024,1414C1073,1414 1115,1395 1151,1357C1187,1318 1205,1266 1205,1201C1205,1113 1175,1033 1115,961C1055,888 977,846 881,834z""/>
<glyph unicode=""&#x16E;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M664,1696C664,1753 685,1802 726,1843C767,1883 816,1903 873,1903C930,1903 979,1883 1020,1843C1060,1802 1080,1753 1080,1696C1080,1639 1060,1590 1019,1549C978,1508 930,1487 873,1487C816,1487 767,1508 726,1549C685,1590 664,1639 664,1696M986,1696C986,1727 975,1753 953,1775C930,1796 904,1807 873,1807C842,1807 816,1796 795,1775C773,1753 762,1727 762,1696C762,1665 773,1639 795,1617C816,1595 842,1584 873,1584C904,1584 930,1595 953,1617C975,1639 986,1665 986,1696z""/>
<glyph unicode=""&#x16F;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M391,1249C391,1306 412,1355 453,1396C494,1436 543,1456 600,1456C657,1456 706,1436 747,1396C787,1355 807,1306 807,1249C807,1192 787,1143 746,1102C705,1061 657,1040 600,1040C543,1040 494,1061 453,1102C412,1143 391,1192 391,1249M713,1249C713,1280 702,1306 680,1328C657,1349 631,1360 600,1360C569,1360 543,1349 522,1328C500,1306 489,1280 489,1249C489,1218 500,1192 522,1170C543,1148 569,1137 600,1137C631,1137 657,1148 680,1170C702,1192 713,1218 713,1249z""/>
<glyph unicode=""&#x170;"" horiz-adv-x=""1686"" d=""M1192,1397l348,0l0,-793C1540,400 1480,245 1359,140C1238,35 1066,-18 842,-18C621,-18 449,35 327,142C204,249 143,402 143,602l0,795l349,0l0,-803C492,497 524,419 588,360C651,301 735,272 838,272C945,272 1031,302 1096,362C1160,422 1192,507 1192,618M1342,1831l-268,-299l-146,0l107,299M961,1831l-268,-299l-145,0l106,299z""/>
<glyph unicode=""&#x171;"" horiz-adv-x=""1194"" d=""M774,0l0,135C687,33 585,-18 469,-18C366,-18 283,14 219,77C155,140 123,228 123,340l0,604l297,0l0,-584C420,240 470,180 569,180C625,180 673,202 713,246C752,289 772,321 772,342l0,602l299,0l0,-944M1103,1384l-268,-299l-146,0l107,299M722,1384l-268,-299l-145,0l106,299z""/>
<glyph unicode=""&#x179;"" horiz-adv-x=""1430"" d=""M1364,1397l-762,-1141l762,0l0,-256l-1356,0l768,1141l-715,0l0,256M1032,1831l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x17A;"" horiz-adv-x=""1067"" d=""M86,944l922,0l-484,-741l484,0l0,-203l-969,0l485,741l-438,0M849,1384l-338,-299l-152,0l152,299z""/>
<glyph unicode=""&#x17B;"" horiz-adv-x=""1430"" d=""M1364,1397l-762,-1141l762,0l0,-256l-1356,0l768,1141l-715,0l0,256M728,1858C774,1858 813,1842 845,1811C876,1779 892,1740 892,1695C892,1649 876,1610 845,1578C814,1546 775,1530 728,1530C683,1530 644,1546 613,1578C581,1610 565,1649 565,1695C565,1740 581,1779 613,1811C644,1842 683,1858 728,1858z""/>
<glyph unicode=""&#x17C;"" horiz-adv-x=""1067"" d=""M86,944l922,0l-484,-741l484,0l0,-203l-969,0l485,741l-438,0M557,1411C603,1411 642,1395 674,1364C705,1332 721,1293 721,1248C721,1202 705,1163 674,1131C643,1099 604,1083 557,1083C512,1083 473,1099 442,1131C410,1163 394,1202 394,1248C394,1293 410,1332 442,1364C473,1395 512,1411 557,1411z""/>
<glyph unicode=""&#x2044;"" horiz-adv-x=""448"" d=""M810,1416l-980,-1471l-192,0l980,1471z""/>
<glyph unicode=""&#xF001;"" horiz-adv-x=""1217"" d=""M719,1163C690,1167 666,1169 645,1169C578,1169 531,1155 504,1126C477,1097 463,1036 463,944l190,0l0,-203l-190,0l0,-741l-297,0l0,741l-125,0l0,203l125,0C166,1258 306,1415 586,1415C636,1415 680,1409 719,1397M780,1255C780,1299 796,1337 828,1368C860,1399 898,1415 942,1415C986,1415 1024,1399 1055,1368C1086,1337 1102,1299 1102,1255C1102,1211 1086,1173 1055,1142C1024,1110 986,1094 942,1094C898,1094 860,1110 828,1142C796,1174 780,1212 780,1255M1090,944l0,-944l-297,0l0,944z""/>
<glyph unicode=""&#xF002;"" horiz-adv-x=""1217"" d=""M653,944l0,-203l-190,0l0,-741l-297,0l0,741l-123,0l0,203l123,0C166,1258 306,1415 586,1415C636,1415 680,1409 719,1397l0,-234C690,1167 666,1169 645,1169C578,1169 531,1155 504,1126C477,1097 463,1036 463,944M1090,1397l0,-1397l-297,0l0,1397z""/>
<glyph unicode=""&#x15E;"" horiz-adv-x=""1237"" d=""M1104,1307l0,-273C919,1117 771,1159 661,1159C598,1159 549,1149 512,1128C475,1107 456,1079 456,1044C456,1017 469,992 496,968C523,944 588,913 691,875C794,837 877,802 939,769C1001,736 1055,690 1100,631C1145,572 1167,497 1167,406C1167,277 1118,174 1020,97C921,20 792,-18 631,-18C458,-18 290,27 127,117l0,301C220,355 303,310 375,281C447,252 523,238 604,238C741,238 809,281 809,367C809,396 795,424 768,450C741,476 675,508 571,545C466,582 384,617 323,649C262,681 209,727 165,786C120,845 98,921 98,1014C98,1134 146,1231 243,1305C340,1378 469,1415 631,1415C780,1415 938,1379 1104,1307M481,-598C537,-535 565,-465 565,-390C565,-383 564,-374 563,-363C501,-335 470,-288 470,-221C470,-179 486,-144 517,-116C548,-88 583,-74 624,-74C673,-74 715,-93 751,-131C787,-170 805,-222 805,-287C805,-375 775,-455 715,-527C655,-600 577,-642 481,-654z""/>
<glyph unicode=""&#x15F;"" horiz-adv-x=""874"" d=""M772,893l0,-211C659,751 554,786 457,786C386,786 350,762 350,713C350,700 358,688 373,676C388,663 445,637 545,598C645,558 716,512 757,460C798,408 819,351 819,289C819,191 786,115 719,62C652,9 557,-18 434,-18C307,-18 195,5 96,51l0,209C217,207 319,180 401,180C497,180 545,203 545,250C545,268 536,285 519,301C501,317 441,345 340,384C239,423 169,467 132,514C95,561 76,614 76,672C76,758 112,828 184,882C255,936 350,963 467,963C580,963 681,940 772,893M311,-598C367,-535 395,-465 395,-390C395,-383 394,-374 393,-363C331,-335 300,-288 300,-221C300,-179 316,-144 347,-116C378,-88 413,-74 454,-74C503,-74 545,-93 581,-131C617,-170 635,-222 635,-287C635,-375 605,-455 545,-527C485,-600 407,-642 311,-654z""/>
<glyph unicode=""&#xA4;"" horiz-adv-x=""1130"" d=""M780,313C717,275 646,256 565,256C486,256 415,274 352,311l-133,-133l-174,174l133,133C141,545 123,617 123,700C123,775 141,846 178,915l-133,134l170,170l131,-132C424,1124 497,1143 565,1143C642,1143 713,1124 780,1087l133,134l174,-174l-133,-134C991,848 1010,777 1010,700C1010,627 991,554 952,481l133,-133l-170,-170M770,700C770,756 750,804 710,844C670,883 622,903 565,903C509,903 461,883 422,844C382,804 362,756 362,700C362,644 382,596 422,556C461,516 509,496 565,496C621,496 669,516 710,556C750,596 770,644 770,700z""/>
</font>

	<font horiz-adv-x=""2048"">
<!-- Gill Sans(R) is a trademark of The Monotype Corporation which may be registered in certain jurisdictions. -->
<!-- Copyright: Copyright 2023 Adobe System Incorporated. All rights reserved. -->
<font-face font-family=""GillSansMT"" units-per-em=""2048"" underline-position=""-154"" underline-thickness=""102""/>
<missing-glyph horiz-adv-x=""2048"" d=""M256,0l0,1536l1536,0l0,-1536M384,128l1280,0l0,1280l-1280,0z""/>
<glyph unicode="" "" horiz-adv-x=""569""/>
<glyph unicode=""!"" horiz-adv-x=""555"" d=""M401,104C401,71 389,42 366,19C342,-4 313,-16 279,-16C243,-16 213,-4 189,19C164,42 152,70 152,104C152,140 164,170 188,195C212,219 241,231 276,231C312,231 342,219 366,195C389,170 401,140 401,104M256,436l-16,51C226,527 210,618 192,760C173,902 164,1023 164,1122C164,1317 202,1415 279,1415C354,1415 391,1320 391,1130C391,972 376,810 346,645l-27,-153l-16,-58C297,413 288,403 276,403C269,403 262,414 256,436z""/>
<glyph unicode=""&quot;"" horiz-adv-x=""725"" d=""M254,946l-111,0l-49,281l0,239l205,0l0,-239M584,946l-109,0l-49,281l0,239l205,0l0,-239z""/>
<glyph unicode=""#"" horiz-adv-x=""1196"" d=""M4,871l0,123l326,0l86,403l121,0l-86,-403l421,0l84,403l123,0l-86,-403l199,0l0,-123l-223,0l-76,-371l299,0l0,-123l-324,0l-84,-399l-122,0l86,399l-424,0l-84,-399l-123,0l84,399l-197,0l0,123l223,0l78,371M848,871l-422,0l-78,-371l424,0z""/>
<glyph unicode=""$"" horiz-adv-x=""1110"" d=""M657,1415l0,-74C790,1321 892,1281 963,1221l0,-230C880,1076 778,1133 657,1161l0,-405C778,720 871,668 934,599C997,530 1028,450 1028,360C1028,270 997,190 935,119C904,84 869,55 828,33C787,11 730,-6 657,-18l0,-222l-168,0l0,222C376,-7 247,40 104,125l0,252C168,319 230,273 290,239C349,205 416,181 489,166l0,440C377,645 298,681 253,714C207,747 171,788 145,835C119,882 106,929 106,977C106,1071 141,1151 211,1218C281,1284 374,1325 489,1341l0,74M489,809l0,352C435,1154 391,1134 357,1103C322,1071 305,1034 305,991C305,952 317,920 340,895C363,870 405,845 464,820C474,816 482,812 489,809M688,522l-31,17l0,-371C772,195 829,253 829,344C829,383 819,416 799,442C778,467 741,494 688,522z""/>
<glyph unicode=""%"" horiz-adv-x=""1384"" d=""M1071,1417l153,0l-919,-1433l-154,0M1021,608C1106,608 1180,578 1241,517C1302,456 1333,382 1333,297C1333,212 1303,139 1242,80C1181,20 1107,-10 1021,-10C934,-10 861,20 800,80C739,139 708,212 708,297C708,383 739,456 800,517C861,578 935,608 1021,608M1019,457C975,457 938,441 907,410C876,379 860,341 860,297C860,252 876,214 907,183C938,151 975,135 1019,135C1064,135 1103,151 1134,182C1165,213 1181,252 1181,297C1181,342 1165,380 1134,411C1103,442 1064,457 1019,457M362,1413C448,1413 521,1382 582,1321C643,1260 673,1186 673,1100C673,1015 643,941 582,880C521,819 447,788 362,788C277,788 204,819 143,880C82,941 51,1015 51,1100C51,1186 81,1260 142,1321C203,1382 276,1413 362,1413M360,1262C315,1262 277,1246 246,1215C214,1183 198,1145 198,1100C198,1056 214,1018 246,987C278,956 316,940 360,940C405,940 443,956 475,987C506,1018 522,1056 522,1100C522,1145 506,1184 475,1215C444,1246 405,1262 360,1262z""/>
<glyph unicode=""&amp;"" horiz-adv-x=""1280"" d=""M1014,0l-162,190C838,176 814,156 780,129l-2,-2C731,88 672,54 603,26C533,-2 467,-16 406,-16C309,-16 227,16 160,79C93,142 59,218 59,309C59,374 76,433 109,486C142,539 195,592 270,647l3,3C317,683 355,708 387,725C282,838 229,946 229,1049C229,1144 261,1222 326,1281C391,1340 475,1370 578,1370C679,1370 763,1341 828,1283C893,1225 926,1151 926,1061C926,939 845,820 682,705l-14,-11l196,-225C921,532 971,607 1014,694l200,0C1166,570 1090,448 987,328l283,-328M684,289l45,41l-227,262l-88,-62C310,460 258,389 258,317C258,278 275,244 309,216C343,188 384,174 432,174C469,174 512,185 559,206C606,227 647,255 684,289M555,829l76,54C702,933 737,991 737,1057C737,1098 722,1132 693,1159C663,1186 625,1200 580,1200C536,1200 500,1187 471,1162C442,1137 428,1105 428,1067C428,1030 439,991 462,948C485,905 516,865 555,829z""/>
<glyph unicode=""'"" horiz-adv-x=""385"" d=""M248,946l-113,0l-45,275l0,245l205,0l0,-245z""/>
<glyph unicode=""("" horiz-adv-x=""662"" d=""M524,1415l113,0C554,1291 493,1185 456,1097C418,1009 390,908 371,795C352,681 342,571 342,465C342,101 440,-210 637,-469l-113,0l-30,37C419,-341 363,-267 327,-209C290,-151 254,-76 218,17C160,165 131,316 131,471C131,628 160,780 218,927C276,1073 378,1236 524,1415z""/>
<glyph unicode="")"" horiz-adv-x=""662"" d=""M137,-469l-112,0C112,-338 173,-227 210,-136C247,-45 274,55 292,164C310,273 319,377 319,477C319,593 309,705 288,814C267,923 235,1026 194,1124C152,1221 96,1318 25,1415l112,0l31,-37C243,1289 298,1216 335,1158C371,1100 407,1024 443,931C501,783 530,630 530,473C530,322 504,177 451,38C425,-31 392,-100 353,-169C313,-238 241,-338 137,-469z""/>
<glyph unicode=""*"" horiz-adv-x=""854"" d=""M444,1102l5,0C463,1102 475,1095 485,1081l172,164C693,1279 725,1296 752,1296C770,1296 785,1289 798,1276C811,1262 817,1245 817,1225C817,1180 791,1150 739,1137l-22,-7l-219,-79C501,1042 502,1034 502,1026C502,1018 501,1011 498,1004l231,-87l45,-22C785,888 795,878 804,865C813,851 817,838 817,827C817,812 811,796 798,781C785,766 769,758 752,758C719,758 680,780 635,825l-150,150C471,961 457,954 444,954l41,-211C491,717 494,686 494,651C494,592 471,563 426,563C379,563 356,595 356,659C356,677 358,694 362,711l52,243C401,955 388,962 375,975l-160,-148C165,782 127,760 102,760C84,760 68,767 55,780C42,793 35,809 35,827C35,871 76,907 158,934l202,70C357,1015 356,1023 356,1028l4,23l-231,86C69,1158 39,1187 39,1223C39,1242 46,1260 60,1275C74,1290 90,1298 109,1298C124,1298 146,1287 174,1266l27,-21l174,-164C386,1094 399,1101 414,1102l-39,205l-10,43C362,1365 360,1380 360,1395C360,1456 383,1487 430,1487C475,1487 498,1460 498,1405C498,1386 494,1354 485,1311z""/>
<glyph unicode=""+"" horiz-adv-x=""1196"" d=""M513,237l0,402l-399,0l0,168l399,0l0,399l170,0l0,-399l399,0l0,-168l-399,0l0,-402z""/>
<glyph unicode="","" horiz-adv-x=""449"" d=""M100,-242l0,37C130,-176 151,-147 163,-118C175,-89 182,-50 184,0C113,21 78,64 78,131C78,167 90,198 115,225C139,251 168,264 201,264C242,264 275,246 301,210C327,174 340,128 340,72C340,-2 319,-67 276,-123C233,-179 175,-219 100,-242z""/>
<glyph unicode=""-"" horiz-adv-x=""662"" d=""M82,551l498,0l0,-195l-498,0z""/>
<glyph unicode=""."" horiz-adv-x=""449"" d=""M223,223C257,223 286,211 310,188C334,165 346,136 346,102C346,69 334,41 311,18C287,-5 258,-16 223,-16C189,-16 160,-5 137,18C114,41 102,69 102,102C102,136 114,165 137,188C160,211 189,223 223,223z""/>
<glyph unicode=""/"" horiz-adv-x=""575"" d=""M5,-18l402,1434l144,0l-401,-1434z""/>
<glyph unicode=""0"" horiz-adv-x=""1024"" d=""M520,1415C648,1415 754,1348 838,1214C921,1079 963,909 963,702C963,490 921,318 838,185C754,52 645,-14 512,-14C377,-14 268,51 186,181C104,311 63,483 63,698C63,910 105,1083 190,1216C275,1349 385,1415 520,1415M522,1225l-10,0C435,1225 375,1178 330,1084C285,989 262,861 262,700C262,535 284,405 328,311C372,217 433,170 510,170C587,170 648,217 693,311C738,405 760,533 760,694C760,856 739,985 696,1081C653,1177 595,1225 522,1225z""/>
<glyph unicode=""1"" horiz-adv-x=""1024"" d=""M414,1397l200,0l0,-1397l-200,0z""/>
<glyph unicode=""2"" horiz-adv-x=""1024"" d=""M422,193l532,0l0,-193l-884,0l0,14l84,99C285,276 390,414 468,527C545,640 596,726 620,784C643,842 655,899 655,956C655,1035 633,1098 588,1146C543,1193 484,1217 410,1217C354,1217 299,1201 245,1168C190,1135 141,1088 98,1028l0,254C207,1371 319,1415 434,1415C556,1415 657,1374 736,1293C815,1212 854,1108 854,983C854,927 844,867 825,804C805,740 770,667 720,584C670,501 585,387 465,244z""/>
<glyph unicode=""3"" horiz-adv-x=""1024"" d=""M369,813l12,0C468,813 535,831 581,867C626,903 649,955 649,1024C649,1086 627,1136 583,1175C538,1214 480,1233 408,1233C335,1233 255,1210 166,1165l0,191C247,1397 336,1417 432,1417C561,1417 662,1383 737,1316C811,1249 848,1157 848,1040C848,969 834,910 805,862C776,813 731,772 668,737C723,716 763,690 788,659C813,628 833,590 847,546C861,502 868,455 868,406C868,284 827,183 746,104C665,24 562,-16 438,-16C332,-16 230,10 131,61l0,218C236,213 339,180 438,180C505,180 560,200 601,240C642,280 662,333 662,399C662,454 645,503 611,544C591,567 569,585 545,596C520,607 468,617 387,627l-18,2z""/>
<glyph unicode=""4"" horiz-adv-x=""1024"" d=""M727,1415l82,0l0,-784l156,0l0,-170l-156,0l0,-461l-195,0l0,461l-591,0l0,86M614,631l0,405l-323,-405z""/>
<glyph unicode=""5"" horiz-adv-x=""1024"" d=""M207,1397l620,0l0,-178l-438,0l0,-353C403,867 417,868 432,868C567,868 679,827 766,745C853,662 897,557 897,430C897,299 854,192 768,110C681,27 569,-14 432,-14C319,-14 209,13 102,68l0,206C203,209 301,176 397,176C481,176 552,201 609,250C666,299 694,359 694,430C694,504 663,566 601,616C539,665 462,690 369,690C290,690 236,683 207,670z""/>
<glyph unicode=""6"" horiz-adv-x=""1024"" d=""M629,1401l20,14l121,-153C663,1193 575,1118 505,1037C434,956 380,860 342,750C416,788 490,807 563,807C673,807 766,768 841,690C916,612 954,516 954,403C954,286 913,187 831,106C749,25 649,-16 532,-16C401,-16 295,30 214,123C133,215 92,336 92,485C92,597 113,709 155,820C196,931 256,1035 335,1132C413,1229 511,1318 629,1401M299,543C294,512 291,477 291,438C291,359 314,295 360,244C405,193 464,168 535,168C599,168 652,190 694,233C735,276 756,331 756,397C756,464 734,519 690,561C645,602 587,623 514,623C477,623 443,617 413,606C382,595 344,574 299,543z""/>
<glyph unicode=""7"" horiz-adv-x=""1024"" d=""M100,1397l904,0l-633,-1413l-178,75l512,1147l-605,0z""/>
<glyph unicode=""8"" horiz-adv-x=""1024"" d=""M98,1022C98,1131 139,1224 221,1301C303,1377 403,1415 522,1415C639,1415 737,1377 818,1302C898,1226 938,1133 938,1022C938,897 880,794 764,713C893,638 958,528 958,383C958,268 916,172 832,97C748,22 642,-16 514,-16C387,-16 281,23 195,100C109,177 66,272 66,385C66,527 132,636 264,713C205,761 162,809 137,857C111,904 98,959 98,1022M739,1016C739,1077 718,1127 677,1166C635,1205 582,1225 518,1225C453,1225 400,1206 359,1167C318,1128 297,1079 297,1018C297,960 319,910 363,869C406,828 459,807 520,807C581,807 632,828 675,869C718,910 739,959 739,1016M756,375l0,20C756,459 733,511 688,552C642,592 583,612 510,612C439,612 381,592 336,551C291,510 268,458 268,393C268,328 291,276 337,235C383,194 443,174 516,174C586,174 644,193 689,230C734,267 756,316 756,375z""/>
<glyph unicode=""9"" horiz-adv-x=""1024"" d=""M383,-16l-123,145C363,194 454,274 531,367C608,460 660,555 686,653C623,614 548,594 463,594C356,594 265,634 189,713C112,792 74,888 74,999C74,1115 115,1213 196,1294C277,1375 377,1415 494,1415C626,1415 734,1367 818,1272C902,1177 944,1054 944,905C944,736 891,565 786,392C681,219 546,83 383,-16M725,852C733,905 737,940 737,956C737,1037 715,1103 670,1154C625,1205 567,1231 496,1231C431,1231 378,1209 336,1166C293,1122 272,1067 272,1001C272,940 295,888 341,844C387,800 442,778 506,778C581,778 654,803 725,852z""/>
<glyph unicode="":"" horiz-adv-x=""449"" d=""M215,227C249,227 278,215 302,191C326,167 338,138 338,104C338,71 326,42 302,19C278,-4 249,-16 215,-16C182,-16 154,-4 130,19C106,42 94,71 94,104C94,138 106,167 129,191C152,215 181,227 215,227M231,938C265,938 294,926 318,902C342,878 354,849 354,815C354,782 342,753 318,730C294,706 265,694 231,694C198,694 170,706 147,730C123,753 111,782 111,815C111,849 123,878 146,902C169,926 198,938 231,938z""/>
<glyph unicode="";"" horiz-adv-x=""469"" d=""M207,-10l2,10C138,21 102,64 102,131C102,167 114,198 139,225C163,251 192,264 225,264C265,264 298,246 325,209C352,172 365,127 365,72C365,-2 344,-67 301,-123C258,-179 200,-219 125,-242l0,37C154,-178 174,-150 186,-121C198,-92 205,-55 207,-10M244,938C278,938 307,926 331,902C355,878 367,849 367,815C367,782 355,753 331,730C307,706 279,694 246,694C212,694 183,706 159,730C135,753 123,782 123,815C123,849 135,878 159,902C182,926 211,938 244,938z""/>
<glyph unicode=""&lt;"" horiz-adv-x=""1196"" d=""M112,641l0,168l971,410l0,-179l-770,-316l770,-319l0,-179z""/>
<glyph unicode=""="" horiz-adv-x=""1196"" d=""M1082,862l-968,0l0,168l968,0M1082,417l-968,0l0,168l968,0z""/>
<glyph unicode=""&gt;"" horiz-adv-x=""1196"" d=""M1083,641l-971,-415l0,179l769,319l-769,316l0,179l971,-410z""/>
<glyph unicode=""?"" horiz-adv-x=""682"" d=""M133,1198l131,219C372,1413 454,1383 510,1327C566,1270 594,1201 594,1118C594,1065 583,1008 562,949C540,889 491,801 416,686C329,551 285,456 285,399C285,390 287,377 291,358C246,379 213,404 194,433C174,462 164,500 164,549C164,596 172,637 188,674C203,711 241,776 301,870C358,961 387,1036 387,1096C387,1137 374,1170 349,1195C323,1219 289,1231 246,1231C214,1231 176,1220 133,1198M438,104C438,70 426,42 402,19C378,-4 348,-16 313,-16C280,-16 252,-5 229,18C206,41 195,69 195,102C195,137 207,166 230,191C253,215 282,227 315,227C350,227 379,215 403,192C426,168 438,139 438,104z""/>
<glyph unicode=""@"" horiz-adv-x=""2068"" d=""M1012,1083C1140,1083 1240,1022 1313,899l35,156l180,0l-160,-742C1359,270 1354,242 1354,229C1354,210 1361,195 1374,182C1387,169 1404,162 1423,162C1466,162 1522,190 1590,246C1657,301 1711,371 1750,455C1789,539 1808,626 1808,717C1808,897 1740,1047 1604,1168C1468,1288 1298,1348 1094,1348C853,1348 654,1267 497,1104C339,941 260,734 260,485C260,242 336,54 488,-80C639,-214 852,-281 1126,-281C1287,-281 1428,-254 1548,-199C1593,-178 1640,-150 1689,-117C1738,-84 1783,-39 1825,18l180,0C1966,-62 1900,-139 1807,-212C1714,-286 1613,-340 1504,-374C1394,-409 1267,-426 1122,-426C808,-426 561,-346 381,-187C200,-28 110,189 110,466C110,657 155,836 244,1002C333,1167 450,1291 597,1372C743,1453 912,1493 1104,1493C1352,1493 1557,1419 1718,1272C1879,1125 1960,938 1960,711C1960,593 1931,479 1874,368C1817,257 1741,167 1647,100C1553,32 1456,-2 1356,-2C1289,-2 1241,10 1213,35C1184,59 1167,102 1161,164C1117,112 1068,72 1015,43C962,14 909,0 856,0C753,0 667,43 598,128C529,213 494,319 494,446C494,551 519,653 568,752C617,851 682,931 763,992C844,1053 927,1083 1012,1083M678,430C678,347 698,279 739,226C780,172 831,145 893,145C986,145 1069,197 1144,300C1218,403 1255,520 1255,649C1255,736 1234,806 1192,859C1149,912 1093,938 1024,938C934,938 854,886 784,783C713,679 678,561 678,430z""/>
<glyph unicode=""A"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506z""/>
<glyph unicode=""B"" horiz-adv-x=""1153"" d=""M627,0l-473,0l0,1397l366,0C635,1397 725,1382 790,1351C855,1320 904,1277 938,1223C972,1168 989,1105 989,1034C989,897 918,798 776,739C878,720 959,678 1020,613C1080,548 1110,471 1110,381C1110,310 1092,246 1055,190C1018,134 965,88 894,53C823,18 734,0 627,0M526,1219l-172,0l0,-420l133,0C594,799 670,819 715,860C760,901 782,953 782,1018C782,1152 697,1219 526,1219M541,621l-187,0l0,-443l197,0C660,178 735,187 774,204C813,221 844,247 868,283C891,319 903,358 903,399C903,442 891,481 866,516C841,551 805,578 759,595C713,612 640,621 541,621z""/>
<glyph unicode=""C"" horiz-adv-x=""1450"" d=""M1358,324l0,-220C1211,24 1042,-16 850,-16C694,-16 562,15 453,78C344,141 258,227 195,337C132,447 100,566 100,694C100,897 173,1068 318,1207C463,1346 641,1415 854,1415C1001,1415 1164,1377 1343,1300l0,-215C1180,1178 1020,1225 864,1225C704,1225 571,1175 466,1074C360,973 307,846 307,694C307,541 359,415 463,316C567,217 700,168 862,168C1031,168 1197,220 1358,324z""/>
<glyph unicode=""D"" horiz-adv-x=""1536"" d=""M156,2l0,1395l471,0C817,1397 967,1369 1078,1312C1189,1255 1277,1171 1342,1060C1407,949 1440,829 1440,698C1440,605 1422,515 1386,430C1350,345 1299,270 1232,205C1164,138 1085,88 995,54C942,33 894,20 850,13C806,6 722,2 598,2M606,1219l-250,0l0,-1039l256,0C712,180 790,187 845,201C900,214 947,232 984,253C1021,274 1054,299 1085,330C1184,430 1233,556 1233,709C1233,859 1182,981 1081,1076C1044,1111 1001,1140 953,1163C904,1186 858,1201 815,1208C772,1215 702,1219 606,1219z""/>
<glyph unicode=""E"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0z""/>
<glyph unicode=""F"" horiz-adv-x=""961"" d=""M156,1397l745,0l0,-178l-545,0l0,-390l545,0l0,-178l-545,0l0,-651l-200,0z""/>
<glyph unicode=""G"" horiz-adv-x=""1516"" d=""M909,688l469,0l0,-592C1206,21 1035,-16 866,-16C635,-16 450,52 312,187C173,322 104,488 104,686C104,895 176,1068 319,1207C462,1346 642,1415 858,1415C937,1415 1011,1407 1082,1390C1153,1373 1242,1341 1350,1296l0,-204C1183,1189 1018,1237 854,1237C701,1237 573,1185 468,1081C363,977 311,849 311,698C311,539 363,410 468,309C573,208 707,158 872,158C952,158 1048,176 1159,213l19,6l0,291l-269,0z""/>
<glyph unicode=""H"" horiz-adv-x=""1493"" d=""M1139,1397l200,0l0,-1397l-200,0l0,608l-785,0l0,-608l-200,0l0,1397l200,0l0,-604l785,0z""/>
<glyph unicode=""I"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1397l-200,0z""/>
<glyph unicode=""J"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1379C356,-111 343,-206 317,-267C290,-329 248,-378 189,-415C130,-452 60,-471 -23,-471C-36,-471 -57,-469 -88,-465l-37,186l51,0C-17,-279 27,-272 58,-258C88,-244 112,-219 130,-184C147,-149 156,-74 156,41z""/>
<glyph unicode=""K"" horiz-adv-x=""1343"" d=""M944,1397l250,0l-606,-662l755,-735l-274,0l-713,690l0,-690l-200,0l0,1397l200,0l0,-639z""/>
<glyph unicode=""L"" horiz-adv-x=""1004"" d=""M154,1397l200,0l0,-1215l629,0l0,-182l-829,0z""/>
<glyph unicode=""M"" horiz-adv-x=""1599"" d=""M1266,1397l186,0l0,-1397l-201,0l0,1087l-430,-540l-37,0l-434,540l0,-1087l-200,0l0,1397l188,0l465,-574z""/>
<glyph unicode=""N"" horiz-adv-x=""1599"" d=""M1264,1397l190,0l0,-1397l-172,0l-934,1075l0,-1075l-188,0l0,1397l162,0l942,-1084z""/>
<glyph unicode=""O"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227z""/>
<glyph unicode=""P"" horiz-adv-x=""1044"" d=""M143,0l0,1399l443,0C719,1399 826,1363 905,1291C984,1219 1024,1122 1024,1001C1024,920 1004,848 963,785C922,722 867,677 796,649C725,620 624,606 492,606l-148,0l0,-606M551,1221l-207,0l0,-437l219,0C644,784 707,803 751,842C795,880 817,935 817,1006C817,1149 728,1221 551,1221z""/>
<glyph unicode=""Q"" horiz-adv-x=""1686"" d=""M1563,-88l-201,-195C1299,-280 1234,-268 1166,-249C1097,-230 1028,-204 958,-171C887,-138 793,-81 676,0C506,31 366,111 256,242C145,373 90,523 90,694C90,903 161,1075 303,1212C445,1349 625,1417 842,1417C1058,1417 1238,1349 1381,1214C1524,1078 1595,907 1595,702C1595,529 1541,378 1433,247C1325,116 1185,34 1014,0l53,-20C1232,-83 1350,-115 1423,-115C1463,-115 1510,-106 1563,-88M842,1227C685,1227 555,1176 452,1075C349,974 298,846 298,692C298,542 350,417 453,318C556,218 686,168 842,168C996,168 1126,219 1231,321C1336,423 1389,549 1389,698C1389,851 1337,977 1234,1077C1131,1177 1000,1227 842,1227z""/>
<glyph unicode=""R"" horiz-adv-x=""1237"" d=""M160,0l0,1397l350,0C651,1397 764,1362 847,1292C930,1222 971,1127 971,1008C971,927 951,856 910,797C869,738 811,693 735,664C780,635 823,595 866,544C909,493 969,405 1046,279C1095,200 1134,140 1163,100l74,-100l-238,0l-61,92C936,95 932,101 926,109l-39,55l-62,102l-67,109C717,432 679,478 645,512C610,546 579,571 552,586C524,601 477,608 412,608l-52,0l0,-608M420,1227l-60,0l0,-441l76,0C537,786 607,795 645,812C682,829 712,856 733,891C754,926 764,965 764,1010C764,1054 752,1094 729,1130C706,1165 673,1190 631,1205C588,1220 518,1227 420,1227z""/>
<glyph unicode=""S"" horiz-adv-x=""938"" d=""M500,586l-152,92C253,736 185,793 145,850C104,906 84,971 84,1044C84,1154 122,1243 199,1312C275,1381 374,1415 496,1415C613,1415 720,1382 817,1317l0,-227C716,1187 608,1235 492,1235C427,1235 373,1220 331,1190C289,1159 268,1120 268,1073C268,1031 283,992 314,955C345,918 394,880 463,840l153,-90C787,649 872,519 872,362C872,250 835,159 760,89C685,19 587,-16 467,-16C329,-16 203,26 90,111l0,254C198,228 323,160 465,160C528,160 580,178 622,213C663,248 684,291 684,344C684,429 623,510 500,586z""/>
<glyph unicode=""T"" horiz-adv-x=""1237"" d=""M35,1399l1167,0l0,-178l-487,0l0,-1221l-201,0l0,1221l-479,0z""/>
<glyph unicode=""U"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604z""/>
<glyph unicode=""V"" horiz-adv-x=""1237"" d=""M1038,1397l199,0l-590,-1403l-45,0l-602,1403l201,0l420,-981z""/>
<glyph unicode=""W"" horiz-adv-x=""2134"" d=""M1933,1397l199,0l-565,-1403l-43,0l-457,1136l-461,-1136l-43,0l-563,1403l201,0l385,-963l387,963l190,0l389,-963z""/>
<glyph unicode=""X"" horiz-adv-x=""1450"" d=""M1165,1397l242,0l-557,-674l596,-723l-242,0l-475,578l-481,-578l-244,0l602,723l-559,674l244,0l438,-533z""/>
<glyph unicode=""Y"" horiz-adv-x=""1237"" d=""M995,1397l242,0l-516,-658l0,-739l-205,0l0,739l-516,658l242,0l374,-482z""/>
<glyph unicode=""Z"" horiz-adv-x=""1323"" d=""M82,1399l1204,0l-893,-1221l893,0l0,-178l-1261,0l895,1221l-838,0z""/>
<glyph unicode=""["" horiz-adv-x=""682"" d=""M176,1397l477,0l0,-164l-280,0l0,-1520l280,0l0,-164l-477,0z""/>
<glyph unicode=""\"" horiz-adv-x=""575"" d=""M426,-18l-402,1434l145,0l401,-1434z""/>
<glyph unicode=""]"" horiz-adv-x=""682"" d=""M506,-451l-475,0l0,164l278,0l0,1520l-278,0l0,164l475,0z""/>
<glyph unicode=""^"" horiz-adv-x=""961"" d=""M479,1194l-241,-598l-185,0l355,801l145,0l354,-801l-182,0z""/>
<glyph unicode=""_"" horiz-adv-x=""1130"" d=""M1161,-406l-1190,0l0,132l1190,0z""/>
<glyph unicode=""`"" horiz-adv-x=""683"" d=""M327,1419l138,-300l-112,0l-230,300z""/>
<glyph unicode=""a"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182z""/>
<glyph unicode=""b"" horiz-adv-x=""1024"" d=""M121,1397l182,0l0,-543C382,911 466,940 555,940C672,940 767,896 841,808C915,720 952,607 952,469C952,321 908,203 820,116C731,28 613,-16 465,-16C406,-16 343,-10 278,3C212,16 160,32 121,51M303,680l0,-498C365,163 429,154 494,154C575,154 641,183 692,240C743,297 768,371 768,461C768,552 744,627 697,684C649,741 587,770 510,770C442,770 373,740 303,680z""/>
<glyph unicode=""c"" horiz-adv-x=""897"" d=""M819,215l0,-180C728,1 638,-16 551,-16C407,-16 292,27 207,112C121,197 78,312 78,455C78,600 120,716 203,805C286,894 396,938 532,938C579,938 622,934 660,925C697,916 744,899 799,874l0,-194C707,739 622,768 543,768C461,768 394,739 341,682C288,624 262,550 262,461C262,367 291,292 348,237C405,182 481,154 578,154C648,154 728,174 819,215z""/>
<glyph unicode=""d"" horiz-adv-x=""1044"" d=""M743,1397l183,0l0,-1397l-389,0C401,0 293,43 212,129C131,215 90,330 90,475C90,610 133,722 218,809C303,896 411,940 543,940C604,940 671,927 743,901M743,156l0,583C686,768 629,782 571,782C480,782 408,752 355,693C301,634 274,554 274,453C274,358 297,285 344,234C372,203 402,183 433,172C464,161 521,156 602,156z""/>
<glyph unicode=""e"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553z""/>
<glyph unicode=""f"" horiz-adv-x=""512"" d=""M150,920l0,55C150,1123 182,1233 245,1306C308,1379 383,1415 471,1415C509,1415 554,1409 606,1397l0,-178C577,1230 549,1235 522,1235C451,1235 401,1216 374,1178C346,1140 332,1072 332,975l0,-55l164,0l0,-164l-164,0l0,-756l-182,0l0,756l-132,0l0,164z""/>
<glyph unicode=""g"" horiz-adv-x=""874"" d=""M82,602C82,700 118,778 190,835C261,892 359,920 483,920l379,0l0,-142l-186,0C712,741 737,708 751,678C765,648 772,614 772,575C772,527 758,480 731,434C704,387 669,352 626,327C583,302 512,283 414,268C345,258 311,234 311,197C311,176 324,158 350,145C375,131 422,117 489,102C602,77 674,58 707,44C739,30 768,10 794,-16C838,-60 860,-115 860,-182C860,-269 821,-339 744,-391C666,-443 562,-469 432,-469C301,-469 196,-443 118,-390C39,-338 0,-268 0,-180C0,-55 77,25 231,61C170,100 139,139 139,178C139,207 152,234 179,258C205,282 240,300 285,311C150,371 82,468 82,602M424,762C375,762 333,745 298,712C263,679 246,639 246,592C246,545 263,506 297,475C331,444 374,428 426,428C477,428 520,444 555,476C589,507 606,547 606,594C606,642 589,682 554,714C519,746 476,762 424,762M381,-43C320,-43 270,-56 231,-82C192,-108 172,-141 172,-182C172,-277 257,-324 428,-324C509,-324 571,-312 616,-288C660,-265 682,-231 682,-188C682,-145 654,-110 598,-83C542,-56 470,-43 381,-43z""/>
<glyph unicode=""h"" horiz-adv-x=""1024"" d=""M125,1397l182,0l0,-598C383,892 477,938 590,938C651,938 706,923 755,892C804,861 840,819 864,765C887,711 899,631 899,524l0,-524l-182,0l0,569C717,636 701,691 668,732C635,773 591,793 537,793C497,793 459,783 424,762C389,741 350,707 307,659l0,-659l-182,0z""/>
<glyph unicode=""i"" horiz-adv-x=""449"" d=""M223,1307C253,1307 279,1297 300,1276C321,1255 332,1230 332,1200C332,1171 321,1145 300,1124C279,1103 253,1092 223,1092C195,1092 170,1103 149,1125C128,1146 117,1171 117,1200C117,1228 128,1253 149,1275C170,1296 195,1307 223,1307M133,920l182,0l0,-920l-182,0z""/>
<glyph unicode=""j"" horiz-adv-x=""449"" d=""M213,1307C243,1307 269,1297 290,1276C311,1255 322,1229 322,1200C322,1169 312,1144 292,1123C271,1102 246,1092 215,1092C186,1092 161,1103 139,1124C117,1145 106,1171 106,1200C106,1228 117,1253 139,1275C160,1296 185,1307 213,1307M127,920l182,0l0,-1051C309,-290 248,-403 125,-469l-135,133C32,-321 65,-296 90,-259C115,-222 127,-180 127,-131z""/>
<glyph unicode=""k"" horiz-adv-x=""981"" d=""M727,920l217,0l-393,-451l473,-469l-244,0l-461,469M129,1397l182,0l0,-1397l-182,0z""/>
<glyph unicode=""l"" horiz-adv-x=""449"" d=""M133,1397l182,0l0,-1397l-182,0z""/>
<glyph unicode=""m"" horiz-adv-x=""1579"" d=""M883,668l0,-668l-183,0l0,512C700,614 686,685 659,726C632,766 584,786 516,786C478,786 443,777 412,760C380,743 344,712 303,668l0,-668l-182,0l0,920l182,0l0,-121C396,892 486,938 575,938C692,938 782,883 846,772C943,884 1045,940 1151,940C1240,940 1314,907 1372,842C1429,777 1458,677 1458,543l0,-543l-182,0l0,545C1276,622 1260,680 1229,721C1198,762 1153,782 1094,782C1019,782 948,744 883,668z""/>
<glyph unicode=""n"" horiz-adv-x=""1024"" d=""M311,920l0,-117C392,893 485,938 588,938C645,938 699,923 748,894C797,864 835,823 861,772C886,720 899,638 899,526l0,-526l-182,0l0,524C717,618 703,685 674,726C645,766 597,786 530,786C444,786 371,743 311,657l0,-657l-186,0l0,920z""/>
<glyph unicode=""o"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758z""/>
<glyph unicode=""p"" horiz-adv-x=""1024"" d=""M117,-469l0,1389l319,0C599,920 727,879 818,798C909,717 954,603 954,457C954,319 911,206 826,117C740,28 631,-16 498,-16C439,-16 374,-3 303,23l0,-492M432,750l-129,0l0,-566C359,155 418,141 479,141C564,141 634,171 689,230C743,289 770,366 770,459C770,519 757,572 732,618C706,664 671,698 627,719C582,740 517,750 432,750z""/>
<glyph unicode=""q"" horiz-adv-x=""1024"" d=""M563,920l344,0l0,-1389l-182,0l0,485C646,-5 579,-16 524,-16C393,-16 285,28 200,116C115,204 72,316 72,451C72,594 116,708 205,793C294,878 413,920 563,920M725,174l0,576l-115,0C531,750 479,747 453,740C426,733 403,723 384,711C365,698 347,683 331,665C281,608 256,538 256,455C256,361 284,284 341,225C397,165 470,135 559,135C580,135 597,136 610,139C623,142 653,151 698,166l14,4z""/>
<glyph unicode=""r"" horiz-adv-x=""811"" d=""M324,920l0,-211l10,16C422,867 510,938 598,938C667,938 738,903 813,834l-96,-160C654,734 595,764 541,764C482,764 432,736 389,680C346,624 324,558 324,481l0,-481l-183,0l0,920z""/>
<glyph unicode=""s"" horiz-adv-x=""788"" d=""M84,66l0,196C135,226 188,197 242,175C295,152 340,141 377,141C415,141 448,150 475,169C502,188 516,210 516,236C516,263 507,285 490,303C472,320 434,346 375,379C258,444 181,500 145,547C108,593 90,643 90,698C90,769 118,826 173,871C228,916 298,938 385,938C475,938 567,913 662,862l0,-180C554,747 466,780 397,780C362,780 333,773 312,758C290,743 279,723 279,698C279,677 289,656 309,637C328,618 363,594 412,567l65,-37C630,443 707,347 707,242C707,167 678,105 619,57C560,8 484,-16 391,-16C336,-16 288,-10 245,2C202,13 149,35 84,66z""/>
<glyph unicode=""t"" horiz-adv-x=""682"" d=""M0,774l342,336l0,-190l291,0l0,-164l-291,0l0,-451C342,200 386,147 473,147C538,147 607,169 680,213l0,-170C610,4 534,-16 451,-16C368,-16 298,8 243,57C226,72 211,88 200,107C189,125 179,149 172,179C164,208 160,265 160,348l0,408l-160,0z""/>
<glyph unicode=""u"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920z""/>
<glyph unicode=""v"" horiz-adv-x=""897"" d=""M692,920l197,0l-406,-936l-61,0l-416,936l199,0l248,-566z""/>
<glyph unicode=""w"" horiz-adv-x=""1473"" d=""M1278,920l195,0l-406,-936l-57,0l-273,641l-268,-641l-59,0l-410,936l195,0l241,-558l234,558l135,0l233,-558z""/>
<glyph unicode=""x"" horiz-adv-x=""1024"" d=""M764,920l233,0l-379,-453l406,-467l-233,0l-289,330l-273,-330l-229,0l387,467l-387,453l229,0l273,-316z""/>
<glyph unicode=""y"" horiz-adv-x=""897"" d=""M692,920l205,0l-651,-1389l-203,0l313,666l-356,723l207,0l248,-519z""/>
<glyph unicode=""z"" horiz-adv-x=""854"" d=""M43,920l786,0l-487,-750l487,0l0,-170l-806,0l483,750l-463,0z""/>
<glyph unicode=""{{"" horiz-adv-x=""682"" d=""M57,449l0,163C105,612 146,624 179,648C212,671 234,706 246,753C257,799 263,881 264,999C265,1140 270,1234 279,1280C287,1325 306,1367 336,1404C362,1437 393,1459 428,1472C463,1485 514,1491 580,1491l57,0l0,-156l-33,0C534,1335 488,1322 467,1296C445,1269 434,1213 434,1128l0,-147C434,892 428,820 415,764C402,707 382,661 355,626C327,590 283,558 223,530C302,497 357,448 388,382C419,316 434,215 434,78l0,-144C434,-151 445,-207 466,-233C487,-259 533,-272 604,-272l33,0l0,-156l-57,0C512,-428 460,-421 425,-408C389,-395 358,-372 333,-339C304,-301 286,-259 278,-214C270,-169 265,-77 264,61C263,181 257,264 245,310C232,356 209,391 174,414C139,437 100,449 57,449z""/>
<glyph unicode=""|"" horiz-adv-x=""532"" d=""M188,-472l0,1869l157,0l0,-1869z""/>
<glyph unicode=""}}"" horiz-adv-x=""682"" d=""M47,1491l55,0C169,1491 221,1485 257,1472C293,1459 323,1435 348,1402C377,1363 396,1321 405,1276C413,1231 418,1138 420,999C420,882 426,800 437,753C448,706 471,671 504,648C537,624 577,612 625,612l0,-163C582,449 542,437 507,414C472,390 448,355 436,308C424,261 418,178 417,61C416,-78 412,-170 404,-215C396,-261 377,-303 348,-341C322,-374 292,-397 257,-409C222,-422 171,-428 102,-428l-55,0l0,156l31,0C149,-272 196,-259 218,-233C239,-207 250,-151 250,-66l0,144C250,214 265,315 296,382C327,448 381,497 459,530C402,557 361,586 334,617C307,648 287,692 272,751C257,809 250,886 250,981l0,147C250,1214 239,1270 217,1296C195,1322 149,1335 78,1335l-31,0z""/>
<glyph unicode=""~"" horiz-adv-x=""1194"" d=""M348,670C302,670 260,661 223,642C186,623 140,587 86,535l0,204C159,820 252,860 365,860C442,860 533,837 639,791l80,-35C760,737 805,727 852,727C940,727 1026,772 1110,862l0,-211C1029,575 937,537 834,537C764,537 682,557 588,598l-62,27C457,655 397,670 348,670z""/>
<glyph unicode=""&#xC4;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M490,1706C516,1706 539,1697 558,1678C577,1659 586,1637 586,1610C586,1583 577,1561 559,1543C541,1524 519,1515 493,1515C466,1515 444,1525 425,1544C406,1563 397,1585 397,1611C397,1638 406,1660 425,1679C443,1697 465,1706 490,1706M875,1706C901,1706 924,1697 943,1678C962,1659 971,1637 971,1610C971,1583 962,1561 944,1543C926,1524 904,1515 878,1515C851,1515 829,1525 810,1544C791,1563 782,1585 782,1611C782,1638 791,1660 810,1679C828,1697 850,1706 875,1706z""/>
<glyph unicode=""&#xC5;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M685,1880C736,1880 781,1862 818,1825C855,1788 873,1744 873,1691C873,1638 855,1593 818,1556C781,1519 736,1500 683,1500C631,1500 587,1519 550,1556C513,1593 494,1637 494,1689C494,1742 513,1788 551,1825C588,1862 633,1880 685,1880M680,1799C646,1799 619,1789 599,1769C578,1748 568,1722 568,1689C568,1657 579,1630 602,1608C624,1585 651,1574 683,1574C715,1574 742,1585 765,1607C788,1629 799,1656 799,1687C799,1718 788,1745 765,1767C742,1788 714,1799 680,1799z""/>
<glyph unicode=""&#xC7;"" horiz-adv-x=""1450"" d=""M846,-16l-5,-17C877,-42 905,-58 926,-83C946,-108 956,-136 956,-167C956,-218 936,-261 895,-296C854,-332 789,-350 700,-350C675,-350 654,-348 635,-345l0,113C669,-235 690,-236 699,-236C737,-236 767,-229 788,-214C801,-205 807,-194 807,-180C807,-164 800,-151 787,-140C773,-129 748,-124 711,-124C704,-124 696,-125 688,-127l42,118C624,3 532,32 453,78C344,141 258,228 195,338C132,447 100,566 100,694C100,897 173,1068 318,1207C463,1346 641,1415 854,1415C1001,1415 1164,1377 1343,1300l0,-215C1180,1178 1020,1225 864,1225C704,1225 571,1175 466,1074C360,973 307,846 307,694C307,541 359,415 463,316C567,217 700,168 862,168C1031,168 1197,220 1358,324l0,-220C1211,24 1042,-16 850,-16z""/>
<glyph unicode=""&#xC9;"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0M736,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xD1;"" horiz-adv-x=""1599"" d=""M1264,1397l190,0l0,-1397l-172,0l-934,1075l0,-1075l-188,0l0,1397l162,0l942,-1084M1126,1777C1125,1696 1107,1633 1070,1588C1042,1553 1007,1535 965,1535C928,1535 879,1548 817,1575C755,1602 708,1615 677,1615C660,1615 645,1608 632,1595C619,1582 612,1562 609,1535l-84,0C536,1629 555,1692 583,1724C611,1755 646,1771 689,1771C728,1771 791,1753 878,1718C923,1700 955,1691 974,1691C993,1691 1009,1698 1022,1711C1035,1724 1043,1746 1048,1777z""/>
<glyph unicode=""&#xD6;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M637,1706C663,1706 686,1697 705,1678C724,1659 733,1637 733,1610C733,1583 724,1561 706,1543C688,1524 666,1515 640,1515C613,1515 591,1525 572,1544C553,1563 544,1585 544,1611C544,1638 553,1660 572,1679C590,1697 612,1706 637,1706M1022,1706C1048,1706 1071,1697 1090,1678C1109,1659 1118,1637 1118,1610C1118,1583 1109,1561 1091,1543C1073,1524 1051,1515 1025,1515C998,1515 976,1525 957,1544C938,1563 929,1585 929,1611C929,1638 938,1660 957,1679C975,1697 997,1706 1022,1706z""/>
<glyph unicode=""&#xDC;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M533,1706C559,1706 582,1697 601,1678C620,1659 629,1637 629,1610C629,1583 620,1561 602,1543C584,1524 562,1515 536,1515C509,1515 487,1525 468,1544C449,1563 440,1585 440,1611C440,1638 449,1660 468,1679C486,1697 508,1706 533,1706M918,1706C944,1706 967,1697 986,1678C1005,1659 1014,1637 1014,1610C1014,1583 1005,1561 987,1543C969,1524 947,1515 921,1515C894,1515 872,1525 853,1544C834,1563 825,1585 825,1611C825,1638 834,1660 853,1679C871,1697 893,1706 918,1706z""/>
<glyph unicode=""&#xE1;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M662,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xE0;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M423,1420l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xE2;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M529,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xE4;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M244,1366C270,1366 293,1357 312,1338C331,1319 340,1297 340,1270C340,1243 331,1221 313,1203C295,1184 273,1175 247,1175C220,1175 198,1185 179,1204C160,1223 151,1245 151,1271C151,1298 160,1320 179,1339C197,1357 219,1366 244,1366M629,1366C655,1366 678,1357 697,1338C716,1319 725,1297 725,1270C725,1243 716,1221 698,1203C680,1184 658,1175 632,1175C605,1175 583,1185 564,1204C545,1223 536,1245 536,1271C536,1298 545,1320 564,1339C582,1357 604,1366 629,1366z""/>
<glyph unicode=""&#xE3;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M738,1397C737,1316 719,1253 682,1208C654,1173 619,1155 577,1155C540,1155 491,1168 429,1195C367,1222 320,1235 289,1235C272,1235 257,1228 244,1215C231,1202 224,1182 221,1155l-84,0C148,1249 167,1312 195,1344C223,1375 258,1391 301,1391C340,1391 403,1373 490,1338C535,1320 567,1311 586,1311C605,1311 621,1318 634,1331C647,1344 655,1366 660,1397z""/>
<glyph unicode=""&#xE5;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M439,1497C490,1497 535,1479 572,1442C609,1405 627,1361 627,1308C627,1255 609,1210 572,1173C535,1136 490,1117 437,1117C385,1117 341,1136 304,1173C267,1210 248,1254 248,1306C248,1359 267,1405 305,1442C342,1479 387,1497 439,1497M434,1416C400,1416 373,1406 353,1386C332,1365 322,1339 322,1306C322,1274 333,1247 356,1225C378,1202 405,1191 437,1191C469,1191 496,1202 519,1224C542,1246 553,1273 553,1304C553,1335 542,1362 519,1384C496,1405 468,1416 434,1416z""/>
<glyph unicode=""&#xE7;"" horiz-adv-x=""897"" d=""M538,-16l-5,-17C569,-42 597,-58 618,-83C638,-108 648,-136 648,-167C648,-218 628,-261 587,-296C546,-332 481,-350 392,-350C367,-350 346,-348 327,-345l0,113C361,-235 382,-236 391,-236C429,-236 459,-229 480,-214C493,-205 499,-194 499,-180C499,-164 492,-151 479,-140C465,-129 440,-124 403,-124C396,-124 388,-125 380,-127l44,123C337,15 258,61 186,133C114,204 78,312 78,455C78,600 120,716 203,805C286,894 396,938 532,938C579,938 622,934 660,925C697,916 744,899 799,874l0,-194C707,739 622,768 543,768C461,768 394,739 341,682C288,624 262,550 262,461C262,367 291,292 348,237C405,182 481,154 578,154C648,154 728,174 819,215l0,-180C728,1 638,-16 551,-16z""/>
<glyph unicode=""&#xE9;"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553M716,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xE8;"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553M477,1419l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xEA;"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553M582,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xEB;"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553M297,1366C323,1366 346,1357 365,1338C384,1319 393,1297 393,1270C393,1243 384,1221 366,1203C348,1184 326,1175 300,1175C273,1175 251,1185 232,1204C213,1223 204,1245 204,1271C204,1298 213,1320 232,1339C250,1357 272,1366 297,1366M682,1366C708,1366 731,1357 750,1338C769,1319 778,1297 778,1270C778,1243 769,1221 751,1203C733,1184 711,1175 685,1175C658,1175 636,1185 617,1204C598,1223 589,1245 589,1271C589,1298 598,1320 617,1339C635,1357 657,1366 682,1366z""/>
<glyph unicode=""&#xED;"" horiz-adv-x=""449"" d=""M133,920l182,0l0,-920l-182,0M522,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xEC;"" horiz-adv-x=""449"" d=""M133,920l182,0l0,-920l-182,0M167,1419l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xEE;"" horiz-adv-x=""449"" d=""M133,920l182,0l0,-920l-182,0M332,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xEF;"" horiz-adv-x=""449"" d=""M133,920l182,0l0,-920l-182,0M31,1366C57,1366 80,1357 99,1338C118,1319 127,1297 127,1270C127,1243 118,1221 100,1203C82,1184 60,1175 34,1175C7,1175 -15,1185 -34,1204C-53,1223 -62,1245 -62,1271C-62,1298 -53,1320 -34,1339C-16,1357 6,1366 31,1366M416,1366C442,1366 465,1357 484,1338C503,1319 512,1297 512,1270C512,1243 503,1221 485,1203C467,1184 445,1175 419,1175C392,1175 370,1185 351,1204C332,1223 323,1245 323,1271C323,1298 332,1320 351,1339C369,1357 391,1366 416,1366z""/>
<glyph unicode=""&#xF1;"" horiz-adv-x=""1024"" d=""M311,920l0,-117C392,893 485,938 588,938C645,938 699,923 748,894C797,864 835,823 861,772C886,720 899,638 899,526l0,-526l-182,0l0,524C717,618 703,685 674,726C645,766 597,786 530,786C444,786 371,743 311,657l0,-657l-186,0l0,920M812,1397C811,1316 793,1253 756,1208C728,1173 693,1155 651,1155C614,1155 565,1168 503,1195C441,1222 394,1235 363,1235C346,1235 331,1228 318,1215C305,1202 298,1182 295,1155l-84,0C222,1249 241,1312 269,1344C297,1375 332,1391 375,1391C414,1391 477,1373 564,1338C609,1320 641,1311 660,1311C679,1311 695,1318 708,1331C721,1344 729,1366 734,1397z""/>
<glyph unicode=""&#xF3;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M771,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xF2;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M489,1419l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xF4;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M641,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xF6;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M344,1366C370,1366 393,1357 412,1338C431,1319 440,1297 440,1270C440,1243 431,1221 413,1203C395,1184 373,1175 347,1175C320,1175 298,1185 279,1204C260,1223 251,1245 251,1271C251,1298 260,1320 279,1339C297,1357 319,1366 344,1366M729,1366C755,1366 778,1357 797,1338C816,1319 825,1297 825,1270C825,1243 816,1221 798,1203C780,1184 758,1175 732,1175C705,1175 683,1185 664,1204C645,1223 636,1245 636,1271C636,1298 645,1320 664,1339C682,1357 704,1366 729,1366z""/>
<glyph unicode=""&#xF5;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M888,1397C887,1316 869,1253 832,1208C804,1173 769,1155 727,1155C690,1155 641,1168 579,1195C517,1222 470,1235 439,1235C422,1235 407,1228 394,1215C381,1202 374,1182 371,1155l-84,0C298,1249 317,1312 345,1344C373,1375 408,1391 451,1391C490,1391 553,1373 640,1338C685,1320 717,1311 736,1311C755,1311 771,1318 784,1331C797,1344 805,1366 810,1397z""/>
<glyph unicode=""&#xFA;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M736,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xF9;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M497,1419l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xFB;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M603,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xFC;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M318,1366C344,1366 367,1357 386,1338C405,1319 414,1297 414,1270C414,1243 405,1221 387,1203C369,1184 347,1175 321,1175C294,1175 272,1185 253,1204C234,1223 225,1245 225,1271C225,1298 234,1320 253,1339C271,1357 293,1366 318,1366M703,1366C729,1366 752,1357 771,1338C790,1319 799,1297 799,1270C799,1243 790,1221 772,1203C754,1184 732,1175 706,1175C679,1175 657,1185 638,1204C619,1223 610,1245 610,1271C610,1298 619,1320 638,1339C656,1357 678,1366 703,1366z""/>
<glyph unicode=""&#x2020;"" horiz-adv-x=""1004"" d=""M506,1415C570,1415 602,1354 602,1231C602,1162 591,1078 569,981l-28,-133C632,883 720,901 805,901C847,901 881,892 908,875C935,857 948,834 948,807C948,781 935,760 909,743C883,726 850,717 811,717C756,717 666,735 541,770l8,-106l10,-117l13,-152C593,169 603,-11 603,-146C603,-266 595,-350 579,-397C562,-445 535,-469 498,-469C458,-469 430,-446 415,-401C399,-356 391,-277 391,-162C391,-98 399,42 416,257C432,472 443,606 449,659l12,111C458,769 449,767 436,763C343,732 264,717 199,717C155,717 120,725 94,741C68,757 55,778 55,805C55,834 68,858 93,875C118,892 153,901 197,901C286,901 374,883 461,848l-25,125C414,1090 403,1175 403,1227C403,1286 412,1333 431,1366C449,1399 474,1415 506,1415z""/>
<glyph unicode=""&#xB0;"" horiz-adv-x=""811"" d=""M127,1214C127,1290 154,1355 209,1410C264,1464 329,1491 406,1491C482,1491 547,1464 601,1410C655,1355 682,1290 682,1214C682,1137 655,1072 601,1018C547,963 482,936 406,936C329,936 264,963 209,1018C154,1072 127,1137 127,1214M236,1214C236,1167 253,1127 286,1094C319,1061 359,1044 406,1044C452,1044 491,1061 524,1094C557,1127 573,1167 573,1214C573,1261 557,1300 525,1333C492,1366 453,1382 406,1382C359,1382 319,1366 286,1333C253,1300 236,1261 236,1214z""/>
<glyph unicode=""&#xA2;"" horiz-adv-x=""897"" d=""M739,1270l95,0l-129,-359C718,906 726,903 729,903C757,893 780,883 797,872l0,-192C755,714 705,739 647,754l-209,-576C487,161 530,152 567,152C610,152 655,158 702,170C749,181 787,196 817,215l0,-176C720,1 628,-18 539,-18C480,-18 426,-12 377,0l-141,-387l-97,0l158,426C229,77 176,132 137,205C98,278 78,358 78,446C78,595 120,715 204,804C287,893 399,938 539,938C556,938 582,936 618,932M362,223l197,545l-18,0C458,768 391,739 340,681C288,622 262,546 262,453C262,359 295,282 362,223z""/>
<glyph unicode=""&#xA3;"" horiz-adv-x=""1087"" d=""M694,-8l-108,16C538,15 489,18 438,18C327,18 222,7 121,-16l162,602l-148,0l0,178l195,0C365,893 410,1010 465,1117C520,1223 580,1299 645,1346C709,1392 776,1415 846,1415C864,1415 889,1412 920,1407l114,-213C995,1215 947,1225 889,1225C816,1225 752,1193 697,1129C642,1065 592,964 547,827l-21,-63l156,0l0,-178l-209,0l-102,-387C391,202 417,203 449,203C522,203 583,199 631,190l80,-14C736,171 768,168 809,168C898,168 967,183 1016,213l0,-195C945,-5 876,-16 811,-16C768,-16 729,-13 694,-8z""/>
<glyph unicode=""&#xA7;"" horiz-adv-x=""831"" d=""M145,1098C145,1189 174,1264 233,1325C292,1385 365,1415 453,1415C524,1415 583,1399 628,1367C673,1334 696,1292 696,1241C696,1208 686,1181 666,1160C646,1139 621,1128 590,1128C567,1128 547,1136 532,1153C516,1169 508,1189 508,1214l0,11C509,1230 510,1236 510,1241C510,1270 489,1284 446,1284C404,1284 371,1272 346,1247C321,1222 309,1188 309,1145C309,1059 369,959 489,846l95,-88C628,717 669,663 708,595C747,526 766,456 766,383C766,323 754,269 731,221C708,172 669,121 614,66C662,0 686,-75 686,-158C686,-245 656,-318 595,-378C534,-439 459,-469 371,-469C301,-469 243,-452 198,-417C152,-383 129,-340 129,-287C129,-252 140,-222 162,-197C184,-172 211,-160 242,-160C271,-160 294,-169 313,-187C331,-206 340,-229 340,-258C340,-269 339,-281 336,-292C335,-297 334,-301 334,-303C334,-312 339,-320 348,-327C357,-334 368,-338 381,-338C421,-338 456,-323 485,-294C514,-265 528,-230 528,-190C528,-148 513,-104 483,-57C453,-10 397,57 315,145C241,219 189,278 160,321C131,364 108,411 91,463C74,515 66,566 66,616C66,724 111,819 201,901C164,965 145,1031 145,1098M434,643l-147,143C244,737 223,686 223,635C223,608 229,578 241,546C252,513 269,482 290,452C311,421 357,368 430,291l100,-107C553,207 571,235 585,267C599,299 606,329 606,356C606,383 602,410 594,436C585,461 573,486 557,509C541,532 500,577 434,643z""/>
<glyph unicode=""&#x2022;"" horiz-adv-x=""725"" d=""M109,698C109,768 134,828 184,878C233,927 293,952 362,952C433,952 493,927 542,878C591,829 616,769 616,698C616,628 591,568 542,519C492,469 432,444 362,444C293,444 233,469 184,519C134,569 109,629 109,698z""/>
<glyph unicode=""&#xB6;"" horiz-adv-x=""1110"" d=""M569,-406l-163,0l0,1043C282,644 184,685 111,759C38,832 2,929 2,1049C2,1175 42,1276 123,1352C158,1385 202,1412 256,1434C310,1455 382,1466 473,1466l633,0l0,-174l-143,0l0,-1698l-170,0l0,1698l-224,0z""/>
<glyph unicode=""&#xDF;"" horiz-adv-x=""1024"" d=""M473,846l27,0C569,846 622,865 660,902C698,939 717,992 717,1059C717,1122 697,1173 658,1214C618,1254 567,1274 506,1274C435,1274 384,1252 352,1209C319,1166 303,1096 303,1001l0,-1001l-182,0l0,999C121,1082 134,1156 161,1222C188,1287 231,1336 290,1368C349,1399 422,1415 510,1415C630,1415 725,1385 794,1326C863,1266 897,1184 897,1081C897,1010 879,947 844,892C809,837 759,797 696,770C779,749 843,706 888,642C933,577 956,497 956,401C956,278 923,179 856,102C789,25 703,-14 598,-14C547,-14 500,-9 457,0l0,178C490,163 524,156 557,156C622,156 675,179 714,226C753,273 772,335 772,414C772,467 762,513 743,552C723,591 694,621 656,644C617,666 556,678 473,680z""/>
<glyph unicode=""&#xAE;"" horiz-adv-x=""1516"" d=""M587,258l-131,0l0,811l281,0C836,1069 907,1061 948,1046C989,1030 1021,1004 1046,967C1071,930 1083,891 1083,850C1083,788 1061,736 1018,693C975,650 919,626 851,621C882,608 911,588 938,559C965,530 999,482 1040,416l98,-158l-160,0l-71,127C856,476 815,536 785,563C755,590 715,603 665,603l-78,0M587,959l0,-244l160,0C815,715 865,725 896,745C927,764 943,795 943,838C943,883 928,914 898,932C867,950 814,959 737,959M758,1417C893,1417 1018,1383 1135,1316C1252,1248 1344,1156 1412,1040C1479,923 1513,798 1513,663C1513,528 1479,402 1412,286C1345,170 1253,78 1136,11C1019,-56 893,-90 758,-90C622,-90 496,-56 379,11C262,78 170,169 103,286C36,402 2,528 2,663C2,798 36,924 104,1041C171,1157 263,1249 380,1316C497,1383 623,1417 758,1417M758,1292C585,1292 437,1231 314,1108C191,985 129,836 129,663C129,489 190,341 313,219C436,96 584,35 758,35C931,35 1079,96 1202,219C1325,342 1386,490 1386,663C1386,836 1325,985 1202,1108C1079,1231 931,1292 758,1292z""/>
<glyph unicode=""&#xA9;"" horiz-adv-x=""1516"" d=""M1108,850l-119,-29C948,924 873,975 762,975C682,975 618,946 571,889C524,831 500,754 500,657C500,561 523,486 568,431C613,376 674,348 752,348C813,348 865,365 910,400C954,434 984,481 999,540l123,-36C1101,419 1059,352 994,303C929,254 851,229 758,229C638,229 542,268 470,347C398,426 362,530 362,661C362,792 398,896 471,972C543,1047 641,1085 766,1085C853,1085 927,1065 986,1024C1045,983 1085,925 1108,850M758,1417C893,1417 1018,1383 1135,1316C1252,1248 1344,1156 1412,1040C1479,923 1513,798 1513,663C1513,528 1479,402 1412,286C1345,170 1253,78 1136,11C1019,-56 893,-90 758,-90C622,-90 496,-56 379,11C262,78 170,169 103,286C36,402 2,528 2,663C2,798 36,924 104,1041C171,1157 263,1249 380,1316C497,1383 623,1417 758,1417M758,1292C585,1292 437,1231 314,1108C191,985 129,836 129,663C129,489 190,341 313,219C436,96 584,35 758,35C931,35 1079,96 1202,219C1325,342 1386,490 1386,663C1386,836 1325,985 1202,1108C1079,1231 931,1292 758,1292z""/>
<glyph unicode=""&#x2122;"" horiz-adv-x=""2048"" d=""M625,582l-136,0l0,695l-264,0l0,120l666,0l0,-120l-266,0M1108,582l-125,0l0,815l201,0l205,-651l198,651l197,0l0,-815l-125,0l0,684l-209,-684l-123,0l-219,695z""/>
<glyph unicode=""&#xB4;"" horiz-adv-x=""683"" d=""M566,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xA8;"" horiz-adv-x=""683"" d=""M148,1366C174,1366 197,1357 216,1338C235,1319 244,1297 244,1270C244,1243 235,1221 217,1203C199,1184 177,1175 151,1175C124,1175 102,1185 83,1204C64,1223 55,1245 55,1271C55,1298 64,1320 83,1339C101,1357 123,1366 148,1366M533,1366C559,1366 582,1357 601,1338C620,1319 629,1297 629,1270C629,1243 620,1221 602,1203C584,1184 562,1175 536,1175C509,1175 487,1185 468,1204C449,1223 440,1245 440,1271C440,1298 449,1320 468,1339C486,1357 508,1366 533,1366z""/>
<glyph unicode=""&#x2260;"" horiz-adv-x=""1195"" d=""M415,417l-301,0l0,168l367,0l109,277l-476,0l0,168l542,0l209,535l125,0l-209,-535l301,0l0,-168l-366,0l-109,-277l475,0l0,-168l-540,0l-209,-536l-127,0z""/>
<glyph unicode=""&#xC6;"" horiz-adv-x=""1835"" d=""M831,1397l912,0l0,-178l-574,0l0,-422l555,0l0,-179l-555,0l0,-438l596,0l0,-178l-796,0l0,616l-381,0l-365,-616l-219,0M969,797l0,465l-277,-465z""/>
<glyph unicode=""&#xD8;"" horiz-adv-x=""1686"" d=""M1374,1419l94,-82l-94,-114C1443,1160 1497,1082 1536,991C1574,900 1593,803 1593,702C1593,496 1522,325 1380,190C1237,54 1058,-14 842,-14C667,-14 523,24 410,100l-101,-118l-96,77l96,119C241,240 188,316 151,407C113,497 94,592 94,692C94,901 165,1074 307,1211C448,1347 627,1415 844,1415C996,1415 1140,1376 1276,1298M1239,1071l-700,-819C635,197 736,170 842,170C999,170 1129,221 1232,322C1335,423 1386,550 1386,705C1386,774 1373,840 1347,905C1321,970 1285,1025 1239,1071M442,328l703,821C1051,1204 951,1231 846,1231C689,1231 559,1181 456,1080C353,979 301,853 301,700C301,559 348,435 442,328z""/>
<glyph unicode=""&#x221E;"" horiz-adv-x=""1460"" d=""M689,812C759,900 814,958 853,986C906,1025 963,1044 1022,1044C1092,1044 1158,1019 1219,968C1280,917 1310,836 1310,723C1310,644 1299,583 1276,538C1253,493 1218,456 1170,429C1122,402 1073,388 1022,388C963,388 906,407 853,445C814,474 759,532 689,620C598,506 502,449 400,449C333,449 275,475 227,526C178,577 154,640 154,716C154,791 178,855 227,906C275,957 333,983 400,983C502,983 598,926 689,812M758,717C816,627 869,565 917,532C948,510 982,499 1020,499C1071,499 1113,518 1148,555C1182,592 1199,645 1199,713C1199,784 1182,838 1147,875C1112,912 1068,931 1016,931C983,931 953,922 924,903C885,877 830,815 758,717M620,716C567,783 524,826 492,845C460,864 429,873 400,873C361,873 329,859 304,832C278,805 265,767 265,719C265,672 278,634 305,606C332,577 364,563 402,563C469,563 542,614 620,716z""/>
<glyph unicode=""&#xB1;"" horiz-adv-x=""1196"" d=""M513,260l0,403l-399,0l0,167l399,0l0,399l170,0l0,-399l399,0l0,-167l-399,0l0,-403M1082,0l-968,0l0,168l968,0z""/>
<glyph unicode=""&#x2264;"" horiz-adv-x=""1195"" d=""M114,762l0,168l968,410l0,-180l-768,-315l768,-319l0,-179M1082,106l-968,0l0,167l968,0z""/>
<glyph unicode=""&#x2265;"" horiz-adv-x=""1195"" d=""M1082,762l-968,-415l0,179l768,319l-768,315l0,180l968,-410M1082,106l-968,0l0,167l968,0z""/>
<glyph unicode=""&#xA5;"" horiz-adv-x=""1130"" d=""M659,0l-182,0l0,326l-416,0l0,137l416,0l0,143l-416,0l0,150l342,0l-403,710l199,0l288,-516C520,893 546,842 563,797C576,825 603,879 645,958l275,508l213,0l-404,-710l340,0l0,-150l-410,0l0,-143l410,0l0,-137l-410,0z""/>
<glyph unicode=""&#xB5;"" horiz-adv-x=""1130"" d=""M793,0l0,109C762,62 730,28 696,9C662,-10 619,-20 567,-20C520,-20 480,-12 449,5C417,21 385,50 352,92l0,-442l-174,0l0,1270l174,0l0,-447C352,376 359,306 373,264C386,221 410,187 443,160C476,133 515,119 559,119C604,119 645,132 681,157C717,182 742,216 757,257C771,298 778,368 778,469l0,451l174,0l0,-920z""/>
<glyph unicode=""&#x2202;"" horiz-adv-x=""1012"" d=""M422,1138l-135,60C334,1303 390,1378 455,1423C520,1468 584,1491 647,1491C698,1491 744,1478 785,1452C826,1425 856,1395 877,1361C908,1308 932,1246 947,1174C962,1102 969,1019 969,926C969,734 940,563 883,414C825,264 747,153 649,82C551,11 455,-25 360,-25C269,-25 195,5 138,66C81,127 52,212 52,322C52,473 103,602 205,709C336,847 553,919 854,924C851,1036 843,1121 830,1180C817,1239 794,1284 762,1316C730,1347 692,1363 649,1363C608,1363 567,1347 528,1314C489,1281 453,1222 422,1138M850,796C687,787 568,764 493,728C417,692 355,633 306,552C257,470 232,387 232,303C232,248 249,202 283,166C317,130 357,112 402,112C452,112 505,130 562,167C640,218 704,296 753,402C802,507 834,639 850,796z""/>
<glyph unicode=""&#x2211;"" horiz-adv-x=""1460"" d=""M139,1491l1237,0l0,-164l-988,0l607,-775l-649,-822l1040,0l0,-161l-1264,0l0,187l620,788l-603,772z""/>
<glyph unicode=""&#x220F;"" horiz-adv-x=""1686"" d=""M161,1491l1362,0l0,-1922l-191,0l0,1748l-978,0l0,-1748l-193,0z""/>
<glyph unicode=""&#x3C0;"" horiz-adv-x=""1124"" d=""M0,1063l1124,0l0,-158l-162,0l0,-905l-189,0l0,905l-426,0l0,-905l-188,0l0,905l-159,0z""/>
<glyph unicode=""&#x222B;"" horiz-adv-x=""561"" d=""M602,1760C602,1734 592,1713 573,1696C553,1679 528,1671 497,1671C472,1671 452,1679 437,1696C422,1713 407,1721 393,1721C381,1721 372,1716 366,1707C359,1697 356,1682 356,1663C356,1640 363,1555 376,1407C389,1259 395,1075 395,856C395,519 385,283 365,147C345,10 319,-84 286,-136C269,-163 246,-184 219,-198C191,-212 162,-219 133,-219C100,-219 72,-207 47,-184C22,-161 10,-132 10,-99C10,-74 19,-52 36,-34C53,-16 74,-7 100,-7C123,-7 145,-18 166,-40C187,-62 201,-73 210,-73C227,-73 240,-58 250,-28C259,1 264,52 264,125C264,150 262,215 258,322C254,429 252,616 252,884C252,1205 258,1418 269,1523C280,1628 294,1699 310,1738C329,1785 352,1819 379,1842C406,1865 434,1876 463,1876C501,1876 534,1865 561,1844C588,1822 602,1794 602,1760z""/>
<glyph unicode=""&#xAA;"" horiz-adv-x=""575"" d=""M473,1182l0,-207C473,945 476,930 483,930C494,930 519,944 559,971l0,-86C508,854 465,838 432,838C381,838 350,854 338,885C289,854 238,838 184,838C141,838 104,852 75,879C46,906 31,940 31,981C31,1053 86,1106 195,1141l139,45l0,24C334,1270 303,1300 242,1300C207,1300 175,1292 145,1277C115,1261 80,1233 41,1194l0,141C90,1388 159,1415 248,1415C328,1415 386,1397 421,1361C456,1324 473,1265 473,1182M334,965l0,147l-64,-22C234,1077 209,1063 194,1049C179,1035 172,1018 172,997C172,951 196,928 244,928C273,928 303,940 334,965z""/>
<glyph unicode=""&#xBA;"" horiz-adv-x=""748"" d=""M360,1415C461,1415 543,1389 606,1336C669,1283 700,1214 700,1130C700,1050 669,983 606,930C543,877 465,850 371,850C279,850 202,877 140,932C78,986 47,1054 47,1135C47,1216 76,1283 135,1336C194,1389 269,1415 360,1415M371,1307C319,1307 276,1290 242,1257C207,1223 190,1181 190,1130C190,1081 207,1042 242,1011C276,980 320,965 375,965C429,965 473,981 508,1013C542,1044 559,1085 559,1135C559,1185 541,1226 506,1259C470,1291 425,1307 371,1307z""/>
<glyph unicode=""&#x2126;"" horiz-adv-x=""1573"" d=""M496,160C424,205 369,248 331,290C273,354 228,429 197,516C166,602 150,697 150,802C150,933 177,1053 230,1163C283,1272 360,1356 461,1415C562,1474 677,1503 808,1503C1032,1503 1204,1418 1324,1249C1411,1125 1455,976 1455,801C1455,689 1437,590 1401,504C1365,417 1318,343 1260,281C1225,242 1175,202 1111,160l364,6l0,-166l-575,0l0,177C953,204 995,230 1024,255C1072,296 1113,344 1146,400C1179,456 1205,516 1222,579C1239,642 1247,708 1247,777C1247,869 1231,962 1198,1055C1165,1148 1115,1218 1048,1267C981,1316 899,1340 804,1340C677,1340 578,1300 508,1220C411,1107 362,961 362,780C362,635 392,511 453,408C514,304 597,228 704,180l0,-180l-577,0l0,168z""/>
<glyph unicode=""&#xE6;"" horiz-adv-x=""1366"" d=""M936,938C1041,938 1126,901 1190,826C1254,751 1286,651 1286,528l0,-28l-567,0C722,410 737,341 763,293C788,244 822,208 863,184C904,159 952,147 1006,147C1093,147 1187,184 1286,258l0,-178C1231,43 1181,18 1137,5C1092,-9 1038,-16 973,-16C910,-16 849,-3 792,24C734,50 691,84 664,125C545,31 430,-16 319,-16C246,-16 186,6 141,49C95,92 72,148 72,217C72,268 88,314 119,355C150,396 199,430 264,459C329,487 377,505 410,512l127,39l0,29C537,649 526,697 503,726C480,754 440,768 385,768C280,768 183,717 92,616l0,220C143,873 191,900 236,915C281,930 331,938 387,938C508,938 600,898 664,819C715,862 761,893 802,911C843,929 887,938 936,938M741,616l363,0C1097,668 1078,708 1048,737C1017,766 978,780 930,780C839,780 776,725 741,616M537,162l0,297l-99,-39C371,394 324,368 295,342C266,316 252,286 252,252C252,212 265,178 291,151C316,123 347,109 383,109C408,109 430,113 449,120C468,127 498,141 537,162z""/>
<glyph unicode=""&#xF8;"" horiz-adv-x=""1130"" d=""M848,856l94,113l68,-62l-90,-106C1013,698 1059,582 1059,455C1059,318 1012,205 917,117C822,28 700,-16 553,-16C456,-16 366,8 283,57l-115,-131l-68,62l111,127C118,210 72,324 72,459C72,596 119,710 213,801C306,892 424,938 565,938C668,938 762,911 848,856M805,670l-410,-480C443,157 499,141 563,141C653,141 729,172 791,233C852,294 883,368 883,457C883,500 877,536 866,567C855,598 834,632 805,670M326,248l407,477C678,758 622,774 565,774C475,774 400,743 339,682C278,621 248,544 248,453C248,375 274,307 326,248z""/>
<glyph unicode=""&#xBF;"" horiz-adv-x=""682"" d=""M594,-172l-133,-217C357,-387 276,-358 219,-302C162,-246 133,-176 133,-92C133,-39 144,18 166,77C187,136 235,223 310,337C398,472 442,568 442,625C442,632 440,647 436,668C481,647 514,621 533,592C552,562 561,523 561,475C561,428 553,387 538,350C522,313 484,248 425,153C368,62 340,-11 340,-68C340,-110 353,-143 378,-168C403,-193 436,-205 479,-205C511,-205 549,-194 594,-172M287,922C287,955 299,984 324,1007C348,1030 377,1042 412,1042C447,1042 476,1031 500,1008C523,985 535,956 535,922C535,887 523,858 499,835C475,811 446,799 412,799C377,799 347,811 323,835C299,858 287,887 287,922z""/>
<glyph unicode=""&#xA1;"" horiz-adv-x=""555"" d=""M152,924C152,957 164,986 189,1011C214,1035 243,1047 276,1047C311,1047 341,1035 365,1012C389,988 401,959 401,924C401,888 389,858 366,834C343,809 314,797 279,797C243,797 213,809 189,834C164,858 152,888 152,924M207,379l14,74C234,522 245,568 252,589C259,610 266,621 274,621C285,621 293,610 299,588C299,584 304,566 315,535C330,492 346,400 363,260C380,120 389,1 389,-98C389,-293 351,-391 276,-391C200,-391 162,-295 162,-104C162,61 177,222 207,379z""/>
<glyph unicode=""&#xAC;"" horiz-adv-x=""1196"" d=""M1081,257l-170,0l0,438l-798,0l0,168l968,0z""/>
<glyph unicode=""&#x221A;"" horiz-adv-x=""1124"" d=""M1055,1868l-307,-1946l-498,1028l-197,-96l-33,68l299,147l404,-825l260,1634z""/>
<glyph unicode=""&#x192;"" horiz-adv-x=""1130"" d=""M721,922l-191,-1090C514,-261 486,-328 446,-368C406,-408 347,-428 270,-428C202,-428 128,-420 47,-403l35,155C148,-263 198,-270 233,-270C272,-270 301,-259 319,-238C336,-217 350,-176 360,-117l179,1039l-201,0l22,139l203,0l25,135C603,1293 627,1362 659,1405C710,1465 782,1495 877,1495C904,1495 943,1488 995,1475l72,-17l18,-4l-34,-154C979,1317 928,1325 897,1325C855,1325 826,1315 809,1294C792,1273 777,1230 766,1165l-18,-104l202,0l-24,-139z""/>
<glyph unicode=""&#x2248;"" horiz-adv-x=""1194"" d=""M348,874C302,874 260,865 223,846C186,827 140,791 86,739l0,204C159,1024 252,1064 365,1064C442,1064 533,1041 639,995l80,-35C760,941 805,931 852,931C940,931 1026,976 1110,1066l0,-211C1029,779 937,741 834,741C764,741 682,761 588,802l-62,27C457,859 397,874 348,874M348,512C302,512 260,503 223,484C186,465 140,429 86,377l0,204C159,662 252,702 365,702C442,702 533,679 639,633l80,-35C760,579 805,569 852,569C940,569 1026,614 1110,704l0,-211C1029,417 937,379 834,379C764,379 682,399 588,440l-62,27C457,497 397,512 348,512z""/>
<glyph unicode=""&#x2206;"" horiz-adv-x=""1253"" d=""M668,1409l577,-1409l-1233,0M594,1059l-447,-959l844,0z""/>
<glyph unicode=""&#xAB;"" horiz-adv-x=""1004"" d=""M653,461l303,-461l-180,0l-301,461l301,459l180,0M238,461l303,-461l-181,0l-303,461l303,459l181,0z""/>
<glyph unicode=""&#xBB;"" horiz-adv-x=""1004"" d=""M350,459l-303,461l178,0l303,-461l-303,-459l-178,0M766,459l-303,461l178,0l303,-461l-303,-459l-178,0z""/>
<glyph unicode=""&#x2026;"" horiz-adv-x=""2048"" d=""M1708,227C1742,227 1771,215 1794,191C1817,167 1829,138 1829,104C1829,71 1817,42 1794,19C1771,-4 1742,-16 1708,-16C1673,-16 1644,-4 1621,19C1597,42 1585,70 1585,104C1585,139 1597,168 1621,192C1644,215 1673,227 1708,227M1024,227C1058,227 1087,215 1111,191C1135,167 1147,138 1147,104C1147,71 1135,42 1111,19C1087,-4 1058,-16 1024,-16C990,-16 961,-4 938,19C915,42 903,71 903,104C903,138 915,167 938,191C961,215 990,227 1024,227M340,225C374,225 403,213 427,188C451,163 463,134 463,100C463,67 451,39 428,16C404,-7 375,-18 340,-18C306,-18 277,-7 254,16C231,39 219,67 219,100C219,135 231,164 255,189C278,213 307,225 340,225z""/>
<glyph unicode=""&#xA0;"" horiz-adv-x=""569""/>
<glyph unicode=""&#xC0;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M637,1800l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xC3;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M964,1777C963,1696 945,1633 908,1588C880,1553 845,1535 803,1535C766,1535 717,1548 655,1575C593,1602 546,1615 515,1615C498,1615 483,1608 470,1595C457,1582 450,1562 447,1535l-84,0C374,1629 393,1692 421,1724C449,1755 484,1771 527,1771C566,1771 629,1753 716,1718C761,1700 793,1691 812,1691C831,1691 847,1698 860,1711C873,1724 881,1746 886,1777z""/>
<glyph unicode=""&#xD5;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M1148,1777C1147,1696 1129,1633 1092,1588C1064,1553 1029,1535 987,1535C950,1535 901,1548 839,1575C777,1602 730,1615 699,1615C682,1615 667,1608 654,1595C641,1582 634,1562 631,1535l-84,0C558,1629 577,1692 605,1724C633,1755 668,1771 711,1771C750,1771 813,1753 900,1718C945,1700 977,1691 996,1691C1015,1691 1031,1698 1044,1711C1057,1724 1065,1746 1070,1777z""/>
<glyph unicode=""&#x152;"" horiz-adv-x=""1878"" d=""M887,1397l905,0l0,-178l-571,0l0,-422l551,0l0,-179l-551,0l0,-438l594,0l0,-178l-861,0C801,2 676,18 581,49C485,80 398,129 321,196C171,325 96,491 96,692C96,902 169,1072 315,1202C461,1332 652,1397 887,1397M881,174l139,0l0,1049l-78,0C787,1223 667,1199 580,1152C493,1105 426,1041 379,961C331,881 307,793 307,698C307,549 362,424 471,324C580,224 717,174 881,174z""/>
<glyph unicode=""&#x153;"" horiz-adv-x=""1706"" d=""M1278,938C1378,938 1461,903 1526,833C1559,798 1584,756 1602,707C1619,658 1630,589 1634,500l-577,0l0,-23C1057,368 1084,287 1138,232C1192,177 1260,150 1343,150C1438,150 1535,188 1634,264l0,-184C1576,44 1524,19 1478,6C1431,-7 1373,-14 1303,-14C1147,-14 1033,45 961,164C863,44 729,-16 559,-16C421,-16 305,30 212,121C119,212 72,324 72,459C72,596 120,710 215,801C310,892 430,938 573,938C734,938 864,875 961,748C998,809 1044,856 1099,889C1154,922 1214,938 1278,938M1075,618l377,0C1429,726 1369,780 1272,780C1225,780 1184,766 1150,738C1115,709 1090,669 1075,618M567,760C479,760 406,731 349,673C291,615 262,542 262,453C262,368 291,298 350,242C408,186 481,158 569,158C657,158 729,186 785,242C840,298 868,370 868,459C868,546 840,618 783,675C726,732 654,760 567,760z""/>
<glyph unicode=""&#x2013;"" horiz-adv-x=""1024"" d=""M-16,584l1058,0l0,-131l-1058,0z""/>
<glyph unicode=""&#x2014;"" horiz-adv-x=""2048"" d=""M-16,584l2082,0l0,-131l-2082,0z""/>
<glyph unicode=""&#x201C;"" horiz-adv-x=""874"" d=""M764,1415l0,-41C740,1358 721,1334 706,1303C691,1271 684,1237 684,1202C684,1185 685,1168 688,1153C719,1151 744,1140 763,1119C782,1098 791,1071 791,1038C791,1005 780,977 757,954C734,931 705,920 670,920C627,920 592,938 565,973C538,1008 524,1054 524,1110C524,1155 534,1198 553,1240C572,1282 597,1316 629,1343C661,1369 706,1393 764,1415M227,1153l9,0C261,1153 283,1141 302,1118C321,1095 330,1068 330,1038C330,1005 319,977 297,954C275,931 248,920 215,920C173,920 138,939 110,976C82,1013 68,1060 68,1116C68,1187 89,1249 131,1302C172,1355 230,1392 303,1415l0,-43C250,1329 223,1273 223,1204z""/>
<glyph unicode=""&#x201D;"" horiz-adv-x=""874"" d=""M651,1145l0,26C580,1180 545,1221 545,1292C545,1328 556,1358 579,1381C601,1404 629,1415 664,1415C707,1415 741,1398 768,1363C794,1328 807,1282 807,1225C807,1152 786,1089 744,1034C701,979 644,941 571,920l0,38C624,1005 651,1067 651,1145M190,1147l0,22C159,1172 134,1184 116,1205C97,1226 88,1252 88,1284C88,1324 99,1356 121,1380C143,1403 172,1415 209,1415C251,1415 285,1397 311,1362C337,1326 350,1279 350,1221C350,1075 270,975 111,920l0,38C164,999 190,1062 190,1147z""/>
<glyph unicode=""&#x2018;"" horiz-adv-x=""449"" d=""M350,1415l0,-31C293,1321 264,1255 264,1186C264,1177 265,1170 266,1163C337,1140 373,1099 373,1038C373,1005 361,977 337,954C312,931 283,920 248,920C206,920 173,937 148,971C123,1004 111,1049 111,1106C111,1260 191,1363 350,1415z""/>
<glyph unicode=""&#x2019;"" horiz-adv-x=""449"" d=""M100,907l0,39C130,975 151,1004 163,1033C175,1062 182,1101 184,1151C113,1174 78,1217 78,1282C78,1319 90,1350 115,1376C139,1402 168,1415 201,1415C242,1415 275,1397 301,1361C327,1325 340,1279 340,1223C340,1148 319,1083 276,1027C233,970 175,930 100,907z""/>
<glyph unicode=""&#xF7;"" horiz-adv-x=""1196"" d=""M604,1131C634,1131 660,1121 681,1100C702,1079 712,1053 712,1023C712,993 702,967 681,946C660,925 634,914 604,914C574,914 548,925 527,946C506,967 495,993 495,1023C495,1053 506,1079 527,1100C548,1121 573,1131 604,1131M1082,639l-968,0l0,168l968,0M605,529C635,529 661,518 682,497C703,476 713,450 713,420C713,390 703,364 682,343C661,322 635,311 605,311C575,311 549,322 528,343C507,364 496,390 496,420C496,450 507,476 528,497C549,518 574,529 605,529z""/>
<glyph unicode=""&#x25CA;"" horiz-adv-x=""1012"" d=""M962,763l-397,-763l-122,0l-397,763l397,763l122,0M839,763l-335,652l-335,-652l335,-652z""/>
<glyph unicode=""&#xFF;"" horiz-adv-x=""897"" d=""M692,920l205,0l-651,-1389l-203,0l313,666l-356,723l207,0l248,-519M254,1366C280,1366 303,1357 322,1338C341,1319 350,1297 350,1270C350,1243 341,1221 323,1203C305,1184 283,1175 257,1175C230,1175 208,1185 189,1204C170,1223 161,1245 161,1271C161,1298 170,1320 189,1339C207,1357 229,1366 254,1366M639,1366C665,1366 688,1357 707,1338C726,1319 735,1297 735,1270C735,1243 726,1221 708,1203C690,1184 668,1175 642,1175C615,1175 593,1185 574,1204C555,1223 546,1245 546,1271C546,1298 555,1320 574,1339C592,1357 614,1366 639,1366z""/>
<glyph unicode=""&#x178;"" horiz-adv-x=""1237"" d=""M995,1397l242,0l-516,-658l0,-739l-205,0l0,739l-516,658l242,0l374,-482M400,1706C426,1706 449,1697 468,1678C487,1659 496,1637 496,1610C496,1583 487,1561 469,1543C451,1524 429,1515 403,1515C376,1515 354,1525 335,1544C316,1563 307,1585 307,1611C307,1638 316,1660 335,1679C353,1697 375,1706 400,1706M785,1706C811,1706 834,1697 853,1678C872,1659 881,1637 881,1610C881,1583 872,1561 854,1543C836,1524 814,1515 788,1515C761,1515 739,1525 720,1544C701,1563 692,1585 692,1611C692,1638 701,1660 720,1679C738,1697 760,1706 785,1706z""/>
<glyph unicode=""&#x2215;"" horiz-adv-x=""406"" d=""M598,1415l139,0l-930,-1468l-137,0z""/>
<glyph unicode=""&#x20AC;"" horiz-adv-x=""1249"" d=""M426,909l594,0l-29,-155l-590,0C400,739 399,722 399,704C399,685 400,665 401,643l570,0l-29,-156l-516,0C487,273 612,166 803,166C924,166 1039,212 1149,303l0,-219C1053,16 936,-18 797,-18C644,-18 517,27 418,116C319,205 252,329 219,487l-162,0l29,156l115,0C201,692 202,729 203,754l-146,0l27,155l141,0C266,1066 338,1189 441,1280C544,1370 666,1415 807,1415C913,1415 1030,1393 1157,1348l-39,-191C1002,1206 896,1231 801,1231C614,1231 489,1124 426,909z""/>
<glyph unicode=""&#x2039;"" horiz-adv-x=""641"" d=""M252,461l303,-461l-180,0l-301,461l301,459l180,0z""/>
<glyph unicode=""&#x203A;"" horiz-adv-x=""641"" d=""M389,459l-303,461l178,0l303,-461l-303,-459l-178,0z""/>
<glyph unicode=""&#xFB01;"" horiz-adv-x=""1024"" d=""M346,922l205,0l0,-156l-205,0l0,-766l-182,0l0,766l-137,0l0,156l137,0l0,30C164,1101 198,1215 265,1295C332,1375 428,1415 553,1415C655,1415 736,1389 795,1337C854,1285 885,1213 887,1122l-172,0C697,1171 675,1205 649,1226C623,1247 589,1257 547,1257C412,1257 344,1157 344,956C344,945 345,934 346,922M715,922l172,0l0,-922l-172,0z""/>
<glyph unicode=""&#xFB02;"" horiz-adv-x=""1024"" d=""M338,922l209,0l0,-156l-209,0l0,-766l-178,0l0,766l-142,0l0,156l142,0l0,45C160,1067 171,1146 194,1204C217,1262 254,1312 307,1353C360,1394 423,1415 498,1415C541,1415 582,1409 621,1397l0,-166C586,1240 559,1245 539,1245C466,1245 414,1224 384,1182C353,1139 338,1066 338,963M713,1397l182,0l0,-1397l-182,0z""/>
<glyph unicode=""&#x2021;"" horiz-adv-x=""1004"" d=""M55,854C55,883 68,907 95,925C122,943 157,952 201,952C268,952 356,935 463,901l-37,168C411,1137 403,1199 403,1255C403,1302 412,1341 431,1371C450,1400 474,1415 504,1415C534,1415 558,1400 576,1369C593,1338 602,1297 602,1245C602,1178 586,1082 555,958l-14,-57C656,935 746,952 811,952C850,952 883,943 908,926C933,909 946,887 946,860C946,833 933,811 907,794C880,777 846,768 805,768C734,768 646,786 541,821C541,774 549,699 564,594C571,542 575,500 575,469C575,453 572,419 565,367l-18,-125C543,216 541,178 541,127C643,161 734,178 815,178C853,178 885,169 910,152C935,135 948,113 948,88C948,59 936,37 912,22C888,6 854,-2 811,-2C730,-2 640,16 541,51C582,-98 602,-215 602,-301C602,-352 592,-392 573,-423C554,-454 528,-469 496,-469C468,-469 446,-453 429,-422C412,-391 403,-351 403,-301C403,-219 423,-102 463,51C351,14 259,-4 188,-4C147,-4 114,4 91,21C67,37 55,59 55,88C55,114 68,136 95,154C122,171 154,180 193,180C254,180 344,162 463,127l-10,119l-15,116C433,417 430,454 430,473C430,502 434,542 443,594C456,676 463,746 463,805l0,16l-33,-8C311,783 231,768 190,768C151,768 118,776 93,793C68,809 55,829 55,854z""/>
<glyph unicode=""&#x2219;"" horiz-adv-x=""532"" d=""M266,819C297,819 323,808 345,786C366,764 377,738 377,707C377,676 366,650 345,629C323,607 297,596 266,596C236,596 210,607 189,629C167,651 156,677 156,707C156,738 167,764 189,786C210,808 236,819 266,819z""/>
<glyph unicode=""&#x201A;"" horiz-adv-x=""449"" d=""M100,-242l0,37C130,-176 151,-147 163,-118C175,-89 182,-50 184,0C113,21 78,64 78,131C78,167 90,198 115,225C139,251 168,264 201,264C242,264 275,246 301,210C327,174 340,128 340,72C340,-2 319,-67 276,-123C233,-179 175,-219 100,-242z""/>
<glyph unicode=""&#x201E;"" horiz-adv-x=""918"" d=""M588,-236l0,37C641,-139 668,-73 668,0C668,7 667,14 666,23C633,27 608,40 589,61C570,82 561,110 561,143C561,179 572,209 595,232C617,255 645,266 680,266C723,266 757,248 784,213C810,177 823,130 823,72C823,0 802,-64 759,-120C716,-176 659,-215 588,-236M125,-238l0,41C181,-140 209,-75 209,-4C209,-3 209,0 208,4C207,10 207,15 207,20C175,23 150,36 131,57C112,78 102,104 102,135C102,175 113,207 135,232C157,256 186,268 223,268C266,268 300,250 326,213C352,176 365,128 365,68C365,25 356,-16 339,-55C321,-95 296,-130 263,-160C230,-191 184,-217 125,-238z""/>
<glyph unicode=""&#x2030;"" horiz-adv-x=""2068"" d=""M1047,1417l153,0l-919,-1433l-156,0M995,608C1081,608 1155,578 1217,517C1278,456 1309,383 1309,297C1309,212 1278,139 1217,80C1156,20 1082,-10 995,-10C908,-10 835,20 775,80C714,139 684,212 684,297C684,383 714,456 775,517C836,578 909,608 995,608M995,457C951,457 914,441 883,410C852,379 836,341 836,297C836,252 852,214 883,183C914,151 951,135 995,135C1040,135 1079,151 1110,182C1141,213 1157,252 1157,297C1157,341 1141,379 1110,410C1078,441 1040,457 995,457M338,1413C424,1413 497,1382 558,1321C619,1260 649,1186 649,1100C649,1015 619,941 558,880C497,819 423,788 338,788C253,788 180,819 119,880C58,941 27,1015 27,1100C27,1186 57,1260 118,1321C179,1382 252,1413 338,1413M336,1262C291,1262 253,1246 222,1215C190,1184 174,1145 174,1100C174,1056 190,1018 222,987C253,956 291,940 336,940C380,940 418,956 450,987C482,1018 498,1056 498,1100C498,1145 482,1183 451,1215C419,1246 381,1262 336,1262M1733,608C1819,608 1892,578 1953,517C2014,456 2044,383 2044,297C2044,212 2014,139 1954,80C1893,20 1820,-10 1733,-10C1646,-10 1573,20 1512,80C1451,139 1421,212 1421,297C1421,383 1452,456 1513,517C1574,578 1647,608 1733,608M1733,457C1688,457 1651,442 1620,411C1589,380 1573,342 1573,297C1573,252 1588,213 1619,182C1650,151 1688,135 1733,135C1777,135 1815,151 1846,183C1877,214 1892,252 1892,297C1892,341 1877,379 1846,410C1815,441 1777,457 1733,457z""/>
<glyph unicode=""&#xC2;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M775,1809l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xCA;"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0M643,1809l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xC1;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M908,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xCB;"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0M318,1706C344,1706 367,1697 386,1678C405,1659 414,1637 414,1610C414,1583 405,1561 387,1543C369,1524 347,1515 321,1515C294,1515 272,1525 253,1544C234,1563 225,1585 225,1611C225,1638 234,1660 253,1679C271,1697 293,1706 318,1706M703,1706C729,1706 752,1697 771,1678C790,1659 799,1637 799,1610C799,1583 790,1561 772,1543C754,1524 732,1515 706,1515C679,1515 657,1525 638,1544C619,1563 610,1585 610,1611C610,1638 619,1660 638,1679C656,1697 678,1706 703,1706z""/>
<glyph unicode=""&#xC8;"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0M497,1800l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xCD;"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1397l-200,0M533,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xCE;"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1397l-200,0M361,1809l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xCF;"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1397l-200,0M70,1706C96,1706 119,1697 138,1678C157,1659 166,1637 166,1610C166,1583 157,1561 139,1543C121,1524 99,1515 73,1515C46,1515 24,1525 5,1544C-14,1563 -23,1585 -23,1611C-23,1638 -14,1660 5,1679C23,1697 45,1706 70,1706M455,1706C481,1706 504,1697 523,1678C542,1659 551,1637 551,1610C551,1583 542,1561 524,1543C506,1524 484,1515 458,1515C431,1515 409,1525 390,1544C371,1563 362,1585 362,1611C362,1638 371,1660 390,1679C408,1697 430,1706 455,1706z""/>
<glyph unicode=""&#xCC;"" horiz-adv-x=""512"" d=""M156,1397l200,0l0,-1397l-200,0M206,1800l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xD3;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M1043,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xD4;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M902,1809l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xD2;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M808,1800l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#xDA;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M962,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xDB;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M809,1809l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#xD9;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M667,1800l138,-300l-112,0l-230,300z""/>
<glyph unicode=""&#x131;"" horiz-adv-x=""449"" d=""M133,920l182,0l0,-920l-182,0z""/>
<glyph unicode=""&#x2C6;"" horiz-adv-x=""683"" d=""M433,1427l193,-309l-112,0l-179,177l-171,-177l-108,0l187,309z""/>
<glyph unicode=""&#x2DC;"" horiz-adv-x=""683"" d=""M642,1397C641,1316 623,1253 586,1208C558,1173 523,1155 481,1155C444,1155 395,1168 333,1195C271,1222 224,1235 193,1235C176,1235 161,1228 148,1215C135,1202 128,1182 125,1155l-84,0C52,1249 71,1312 99,1344C127,1375 162,1391 205,1391C244,1391 307,1373 394,1338C439,1320 471,1311 490,1311C509,1311 525,1318 538,1331C551,1344 559,1366 564,1397z""/>
<glyph unicode=""&#x2C9;"" horiz-adv-x=""683"" d=""M680,1339l0,-149l-677,0l0,149z""/>
<glyph unicode=""&#x2D8;"" horiz-adv-x=""683"" d=""M631,1397C615,1309 582,1243 532,1199C481,1155 417,1133 340,1133C267,1133 204,1156 152,1202C100,1247 66,1312 51,1397l137,0C199,1354 217,1322 243,1301C269,1280 303,1270 345,1270C384,1270 415,1280 440,1300C464,1319 481,1352 492,1397z""/>
<glyph unicode=""&#x2D9;"" horiz-adv-x=""683"" d=""M342,1356C376,1356 405,1344 429,1321C453,1298 465,1269 465,1235C465,1202 453,1174 430,1151C406,1128 377,1117 342,1117C308,1117 279,1128 256,1151C233,1174 221,1202 221,1235C221,1269 233,1298 256,1321C279,1344 308,1356 342,1356z""/>
<glyph unicode=""&#x2DA;"" horiz-adv-x=""683"" d=""M343,1497C394,1497 439,1479 476,1442C513,1405 531,1361 531,1308C531,1255 513,1210 476,1173C439,1136 394,1117 341,1117C289,1117 245,1136 208,1173C171,1210 152,1254 152,1306C152,1359 171,1405 209,1442C246,1479 291,1497 343,1497M338,1416C304,1416 277,1406 257,1386C236,1365 226,1339 226,1306C226,1274 237,1247 260,1225C282,1202 309,1191 341,1191C373,1191 400,1202 423,1224C446,1246 457,1273 457,1304C457,1335 446,1362 423,1384C400,1405 372,1416 338,1416z""/>
<glyph unicode=""&#xB8;"" horiz-adv-x=""683"" d=""M428,16l-15,-49C449,-42 477,-58 498,-83C518,-108 528,-136 528,-167C528,-218 508,-261 467,-296C426,-332 361,-350 272,-350C247,-350 226,-348 207,-345l0,113C241,-235 262,-236 271,-236C309,-236 339,-229 360,-214C373,-205 379,-194 379,-180C379,-164 372,-151 359,-140C345,-129 320,-124 283,-124C276,-124 268,-125 260,-127l51,143z""/>
<glyph unicode=""&#x2DD;"" horiz-adv-x=""683"" d=""M353,1422l-239,-300l-111,0l143,300M680,1422l-237,-300l-113,0l145,300z""/>
<glyph unicode=""&#x2DB;"" horiz-adv-x=""683"" d=""M545,-196l0,-137C498,-354 452,-364 407,-364C339,-364 286,-346 247,-310C208,-274 188,-229 188,-175C188,-110 219,-45 280,19l136,0C368,-46 344,-94 344,-125C344,-149 354,-170 374,-189C393,-208 416,-218 441,-218C459,-218 494,-211 545,-196z""/>
<glyph unicode=""&#x2C7;"" horiz-adv-x=""683"" d=""M662,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x141;"" horiz-adv-x=""1004"" d=""M152,1397l200,0l0,-533l283,232l0,-176l-283,-244l0,-488l631,0l0,-188l-831,0l0,512l-181,-145l0,186l181,149z""/>
<glyph unicode=""&#x142;"" horiz-adv-x=""449"" d=""M135,1397l182,0l0,-539l164,129l0,-162l-164,-133l0,-692l-182,0l0,545l-160,-125l0,162l160,131z""/>
<glyph unicode=""&#x160;"" horiz-adv-x=""938"" d=""M500,586l-152,92C253,736 185,793 145,850C104,906 84,971 84,1044C84,1154 122,1243 199,1312C275,1381 374,1415 496,1415C613,1415 720,1382 817,1317l0,-227C716,1187 608,1235 492,1235C427,1235 373,1220 331,1190C289,1159 268,1120 268,1073C268,1031 283,992 314,955C345,918 394,880 463,840l153,-90C787,649 872,519 872,362C872,250 835,159 760,89C685,19 587,-16 467,-16C329,-16 203,26 90,111l0,254C198,228 323,160 465,160C528,160 580,178 622,213C663,248 684,291 684,344C684,429 623,510 500,586M791,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x161;"" horiz-adv-x=""788"" d=""M84,66l0,196C135,226 188,197 242,175C295,152 340,141 377,141C415,141 448,150 475,169C502,188 516,210 516,236C516,263 507,285 490,303C472,320 434,346 375,379C258,444 181,500 145,547C108,593 90,643 90,698C90,769 118,826 173,871C228,916 298,938 385,938C475,938 567,913 662,862l0,-180C554,747 466,780 397,780C362,780 333,773 312,758C290,743 279,723 279,698C279,677 289,656 309,637C328,618 363,594 412,567l65,-37C630,443 707,347 707,242C707,167 678,105 619,57C560,8 484,-16 391,-16C336,-16 288,-10 245,2C202,13 149,35 84,66M715,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x17D;"" horiz-adv-x=""1323"" d=""M82,1399l1204,0l-893,-1221l893,0l0,-178l-1261,0l895,1221l-838,0M981,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x17E;"" horiz-adv-x=""854"" d=""M43,920l786,0l-487,-750l487,0l0,-170l-806,0l483,750l-463,0M748,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#xA6;"" horiz-adv-x=""532"" d=""M188,701l0,696l157,0l0,-696M188,-472l0,832l157,0l0,-832z""/>
<glyph unicode=""&#xAD;"" horiz-adv-x=""661"" d=""M580,550l0,-194l-498,0l0,194z""/>
<glyph unicode=""&#xAF;"" horiz-adv-x=""1130"" d=""M-35,1496l0,132l1200,0l0,-132z""/>
<glyph unicode=""&#xD0;"" horiz-adv-x=""1536"" d=""M156,2l0,631l-162,0l0,131l162,0l0,633l471,0C817,1397 967,1369 1078,1312C1189,1255 1277,1171 1342,1060C1407,949 1440,829 1440,698C1440,605 1422,515 1386,430C1350,345 1299,270 1232,205C1164,138 1085,88 995,54C942,33 894,20 850,13C806,6 722,2 598,2M752,633l-396,0l0,-453l256,0C712,180 790,187 845,201C900,214 947,232 984,253C1021,274 1054,299 1085,330C1184,430 1233,556 1233,709C1233,859 1182,981 1081,1076C1044,1111 1001,1140 953,1163C904,1186 858,1201 815,1208C772,1215 702,1219 606,1219l-250,0l0,-455l396,0z""/>
<glyph unicode=""&#xF0;"" horiz-adv-x=""1153"" d=""M270,1286l97,127C427,1394 471,1379 500,1368C528,1356 567,1336 618,1309l224,104l49,-110l-154,-76C814,1162 869,1104 904,1055C939,1006 968,954 993,900C1049,781 1077,655 1077,524C1077,357 1032,225 941,129C850,32 726,-16 569,-16C431,-16 315,29 222,119C129,208 82,320 82,453C82,592 130,708 225,800C320,892 440,938 584,938C627,938 662,935 690,929C717,923 753,910 797,891C756,960 723,1008 698,1037C673,1065 630,1102 569,1149l-295,-137l-47,112l215,101C385,1253 328,1273 270,1286M897,463C897,555 868,630 809,688C750,745 674,774 580,774C489,774 413,743 351,681C289,619 258,543 258,453C258,364 289,291 350,234C411,176 489,147 582,147C677,147 754,176 811,234C868,291 897,368 897,463z""/>
<glyph unicode=""&#xDD;"" horiz-adv-x=""1237"" d=""M995,1397l242,0l-516,-658l0,-739l-205,0l0,739l-516,658l242,0l374,-482M842,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xFD;"" horiz-adv-x=""897"" d=""M692,920l205,0l-651,-1389l-203,0l313,666l-356,723l207,0l248,-519M672,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#xDE;"" horiz-adv-x=""1044"" d=""M143,0l0,1397l201,0l0,-301l242,0C720,1096 827,1060 906,988C985,915 1024,818 1024,696C1024,580 984,485 904,412C860,372 807,344 745,328C683,311 599,303 492,303l-148,0l0,-303M551,917l-207,0l0,-436l219,0C644,481 707,500 751,539C795,577 817,631 817,702C817,845 728,917 551,917z""/>
<glyph unicode=""&#xFE;"" horiz-adv-x=""1024"" d=""M117,-469l0,1866l186,0l0,-477l133,0C599,920 727,879 818,798C909,717 954,603 954,457C954,319 911,206 826,117C740,28 631,-16 498,-16C439,-16 374,-3 303,23l0,-492M432,750l-129,0l0,-566C359,155 418,141 479,141C564,141 634,171 689,230C743,289 770,366 770,459C770,519 757,572 732,618C706,664 671,698 627,719C582,740 517,750 432,750z""/>
<glyph unicode=""&#xD7;"" horiz-adv-x=""1196"" d=""M161,410l315,315l-315,313l122,122l314,-314l314,314l121,-122l-313,-313l313,-315l-121,-121l-314,314l-314,-314z""/>
<glyph unicode=""&#xB9;"" horiz-adv-x=""662"" d=""M260,1407l141,0l0,-727l-141,0z""/>
<glyph unicode=""&#xB2;"" horiz-adv-x=""662"" d=""M297,791l307,0l0,-111l-547,0l0,14l6,6C69,707 81,720 100,739C183,824 254,905 313,980C372,1055 401,1119 401,1173C401,1212 389,1243 366,1266C342,1289 310,1300 270,1300C201,1300 137,1261 76,1182l0,159C143,1390 213,1415 285,1415C360,1415 423,1394 472,1351C521,1308 545,1254 545,1188C545,1162 540,1134 529,1104C518,1073 505,1045 488,1020C471,995 412,923 309,805z""/>
<glyph unicode=""&#xB3;"" horiz-adv-x=""662"" d=""M233,1108l13,0C348,1108 399,1142 399,1210C399,1240 387,1264 364,1282C340,1300 308,1309 268,1309C229,1309 178,1295 115,1268l0,112C166,1403 222,1415 283,1415C362,1415 424,1397 470,1362C516,1326 539,1278 539,1217C539,1148 507,1097 442,1063C516,1035 553,978 553,893C553,828 528,775 478,733C427,691 364,670 287,670C224,670 160,685 94,715l0,129C163,804 227,784 287,784C324,784 353,794 374,813C395,832 406,857 406,889C406,924 395,950 372,967C349,984 311,995 258,999l-25,2z""/>
<glyph unicode=""&#xBD;"" horiz-adv-x=""1729"" d=""M258,1407l141,0l0,-727l-141,0M1222,1415l139,0l-930,-1468l-137,0M1366,112l307,0l0,-111l-547,0l0,14l6,6C1138,28 1150,41 1169,60C1252,145 1323,226 1382,301C1441,376 1470,440 1470,494C1470,533 1458,564 1435,587C1411,610 1379,621 1339,621C1270,621 1206,582 1145,503l0,159C1212,711 1282,736 1354,736C1429,736 1492,715 1541,672C1590,629 1614,575 1614,509C1614,483 1609,455 1598,425C1587,394 1574,366 1557,341C1540,316 1481,244 1378,126z""/>
<glyph unicode=""&#xBC;"" horiz-adv-x=""1729"" d=""M258,1407l141,0l0,-727l-141,0M1283,1415l139,0l-930,-1468l-137,0M1520,737l67,0l0,-401l92,0l0,-100l-92,0l0,-236l-139,0l0,236l-348,0l0,51M1448,336l0,172l-160,-172z""/>
<glyph unicode=""&#xBE;"" horiz-adv-x=""1729"" d=""M231,1108l13,0C346,1108 397,1142 397,1210C397,1240 385,1264 362,1282C338,1300 306,1309 266,1309C227,1309 176,1295 113,1268l0,112C164,1403 220,1415 281,1415C360,1415 422,1397 468,1362C514,1326 537,1278 537,1217C537,1148 505,1097 440,1063C514,1035 551,978 551,893C551,828 526,775 476,733C425,691 362,670 285,670C222,670 158,685 92,715l0,129C161,804 225,784 285,784C322,784 351,794 372,813C393,832 404,857 404,889C404,924 393,950 370,967C347,984 309,995 256,999l-25,2M1259,1415l139,0l-930,-1468l-137,0M1520,737l67,0l0,-401l92,0l0,-100l-92,0l0,-236l-139,0l0,236l-348,0l0,51M1448,336l0,172l-160,-172z""/>
<glyph unicode=""&#xB7;"" horiz-adv-x=""683"" d=""M741,720C741,691 731,666 710,645C689,624 664,613 634,613C604,613 579,624 558,645C537,666 526,692 526,722C526,751 537,777 558,798C579,819 604,829 634,829C663,829 689,819 710,798C731,777 741,751 741,720z""/>
<glyph unicode=""&#x102;"" horiz-adv-x=""1366"" d=""M600,1405l141,0l625,-1405l-205,0l-186,414l-594,0l-176,-414l-205,0M893,592l-225,506l-209,-506M972,1764C956,1676 923,1610 873,1566C822,1522 758,1500 681,1500C608,1500 545,1523 493,1569C441,1614 407,1679 392,1764l137,0C540,1721 558,1689 584,1668C610,1647 644,1637 686,1637C725,1637 756,1647 781,1667C805,1686 822,1719 833,1764z""/>
<glyph unicode=""&#x103;"" horiz-adv-x=""874"" d=""M707,553l0,-391C707,131 718,115 739,115C761,115 795,131 842,164l0,-111C801,26 768,8 743,-1C718,-11 691,-16 664,-16C586,-16 540,15 526,76C449,16 366,-14 279,-14C215,-14 162,7 119,50C76,92 55,145 55,209C55,267 76,319 118,365C159,410 218,446 295,473l233,80l0,49C528,713 473,768 362,768C263,768 166,717 72,614l0,199C143,896 244,938 377,938C476,938 556,912 616,860C636,843 654,821 670,794C686,766 696,738 701,711C705,683 707,630 707,553M528,182l0,273l-122,-47C344,383 300,359 275,334C249,309 236,277 236,240C236,202 248,171 273,147C297,123 328,111 367,111C425,111 479,135 528,182M673,1397C657,1309 624,1243 574,1199C523,1155 459,1133 382,1133C309,1133 246,1156 194,1202C142,1247 108,1312 93,1397l137,0C241,1354 259,1322 285,1301C311,1280 345,1270 387,1270C426,1270 457,1280 482,1300C506,1319 523,1352 534,1397z""/>
<glyph unicode=""&#x104;"" horiz-adv-x=""1366"" d=""M1444,-197l0,-137C1395,-355 1345,-365 1294,-365C1234,-365 1185,-347 1146,-310C1107,-274 1087,-228 1087,-172C1087,-113 1112,-56 1161,0l-186,412l-594,0l-176,-412l-205,0l600,1405l141,0l625,-1405l-66,0C1262,-52 1243,-94 1243,-125C1243,-151 1253,-173 1274,-191C1295,-210 1319,-219 1348,-219C1359,-219 1391,-212 1444,-197M668,1098l-209,-506l434,0z""/>
<glyph unicode=""&#x105;"" horiz-adv-x=""874"" d=""M842,164l0,-111C819,37 778,15 719,-12C688,-48 672,-86 672,-125C672,-151 681,-173 699,-191C716,-210 740,-219 770,-219C792,-219 825,-212 870,-197l0,-137C825,-355 780,-365 733,-365C667,-365 614,-347 574,-310C534,-273 514,-227 514,-172C514,-113 538,-57 586,-2C556,15 537,41 528,76C447,13 364,-18 281,-18C218,-18 164,4 121,49C77,94 55,147 55,209C55,261 70,306 99,344C128,381 166,412 215,435C264,458 368,498 528,553l0,51C528,712 474,766 365,766C265,766 168,715 74,614l0,199C145,896 246,938 379,938C446,938 504,927 553,904C601,881 639,847 667,801C695,754 709,672 709,553l0,-391C709,131 720,115 741,115C760,115 794,131 842,164M528,184l0,271C423,414 357,387 331,373C304,358 282,339 265,316C247,293 238,267 238,240C238,203 251,172 276,148C301,123 332,111 369,111C426,111 479,135 528,184z""/>
<glyph unicode=""&#x106;"" horiz-adv-x=""1450"" d=""M1358,324l0,-220C1211,24 1042,-16 850,-16C694,-16 562,15 453,78C344,141 258,227 195,337C132,447 100,566 100,694C100,897 173,1068 318,1207C463,1346 641,1415 854,1415C1001,1415 1164,1377 1343,1300l0,-215C1180,1178 1020,1225 864,1225C704,1225 571,1175 466,1074C360,973 307,846 307,694C307,541 359,415 463,316C567,217 700,168 862,168C1031,168 1197,220 1358,324M1054,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x107;"" horiz-adv-x=""897"" d=""M819,215l0,-180C728,1 638,-16 551,-16C407,-16 292,27 207,112C121,197 78,312 78,455C78,600 120,716 203,805C286,894 396,938 532,938C579,938 622,934 660,925C697,916 744,899 799,874l0,-194C707,739 622,768 543,768C461,768 394,739 341,682C288,624 262,550 262,461C262,367 291,292 348,237C405,182 481,154 578,154C648,154 728,174 819,215M726,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x10C;"" horiz-adv-x=""1450"" d=""M1358,324l0,-220C1211,24 1042,-16 850,-16C694,-16 562,15 453,78C344,141 258,227 195,337C132,447 100,566 100,694C100,897 173,1068 318,1207C463,1346 641,1415 854,1415C1001,1415 1164,1377 1343,1300l0,-215C1180,1178 1020,1225 864,1225C704,1225 571,1175 466,1074C360,973 307,846 307,694C307,541 359,415 463,316C567,217 700,168 862,168C1031,168 1197,220 1358,324M1174,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x10D;"" horiz-adv-x=""897"" d=""M819,215l0,-180C728,1 638,-16 551,-16C407,-16 292,27 207,112C121,197 78,312 78,455C78,600 120,716 203,805C286,894 396,938 532,938C579,938 622,934 660,925C697,916 744,899 799,874l0,-194C707,739 622,768 543,768C461,768 394,739 341,682C288,624 262,550 262,461C262,367 291,292 348,237C405,182 481,154 578,154C648,154 728,174 819,215M839,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x10E;"" horiz-adv-x=""1536"" d=""M156,2l0,1395l471,0C817,1397 967,1369 1078,1312C1189,1255 1277,1171 1342,1060C1407,949 1440,829 1440,698C1440,605 1422,515 1386,430C1350,345 1299,270 1232,205C1164,138 1085,88 995,54C942,33 894,20 850,13C806,6 722,2 598,2M606,1219l-250,0l0,-1039l256,0C712,180 790,187 845,201C900,214 947,232 984,253C1021,274 1054,299 1085,330C1184,430 1233,556 1233,709C1233,859 1182,981 1081,1076C1044,1111 1001,1140 953,1163C904,1186 858,1201 815,1208C772,1215 702,1219 606,1219M1012,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x10F;"" horiz-adv-x=""1216"" d=""M743,1397l183,0l0,-1397l-389,0C401,0 293,43 212,129C131,215 90,330 90,475C90,610 133,722 218,809C303,896 411,940 543,940C604,940 671,927 743,901M743,156l0,583C686,768 629,782 571,782C480,782 408,752 355,693C301,634 274,554 274,453C274,358 297,285 344,234C372,203 402,183 433,172C464,161 521,156 602,156M1046,902l0,38C1102,990 1130,1059 1131,1146C1060,1167 1025,1211 1025,1277C1025,1312 1037,1343 1061,1370C1085,1397 1114,1410 1148,1410C1191,1410 1225,1391 1250,1353C1275,1314 1287,1269 1287,1217C1287,1149 1266,1085 1225,1026C1184,966 1124,925 1046,902z""/>
<glyph unicode=""&#x110;"" horiz-adv-x=""1536"" d=""M155,633l-163,0l0,131l163,0l0,633l472,0C787,1397 923,1376 1034,1333C1145,1290 1240,1211 1320,1095C1400,979 1440,847 1440,699C1440,584 1415,479 1365,384C1315,289 1248,210 1165,148C1082,85 997,45 911,27C825,9 720,0 597,0l-442,0M355,633l0,-452l258,0C775,181 896,203 977,248C1058,292 1121,356 1166,439C1211,522 1233,612 1233,708C1233,796 1214,876 1177,949C1139,1022 1088,1080 1023,1123C958,1166 895,1193 836,1204C776,1214 699,1219 606,1219l-251,0l0,-455l396,0l0,-131z""/>
<glyph unicode=""&#x111;"" horiz-adv-x=""1045"" d=""M926,1208l159,0l0,-106l-159,0l0,-1102l-390,0C391,0 281,46 205,137C128,228 90,340 90,473C90,613 134,726 222,811C309,896 416,938 542,938C607,938 673,926 741,902l0,200l-220,0l0,106l220,0l0,189l185,0M741,740C688,769 631,783 570,783C483,783 412,754 357,696C302,637 274,556 274,453C274,377 289,315 319,268C348,221 383,190 422,176C461,161 521,154 602,154l139,0z""/>
<glyph unicode=""&#x118;"" horiz-adv-x=""1024"" d=""M965,-197l0,-137C916,-355 866,-365 815,-365C755,-365 706,-347 667,-310C628,-274 608,-228 608,-172C608,-113 633,-56 682,0l-528,0l0,1397l792,0l0,-180l-592,0l0,-422l572,0l0,-181l-572,0l0,-434l611,0l0,-180l-144,0C783,-52 764,-94 764,-125C764,-151 774,-173 795,-191C815,-210 839,-219 868,-219C878,-219 910,-212 965,-197z""/>
<glyph unicode=""&#x119;"" horiz-adv-x=""981"" d=""M889,-197l0,-137C844,-355 798,-365 752,-365C686,-365 633,-347 593,-310C552,-274 532,-228 532,-172C532,-121 552,-69 592,-16C588,-16 579,-17 565,-18l-28,0C402,-18 293,28 209,119C124,210 82,325 82,465C82,607 122,721 202,808C282,895 385,938 512,938C632,938 729,895 802,810C875,724 911,610 911,467l0,-23l-647,0C269,359 299,289 354,236C409,183 478,156 563,156C684,156 795,193 895,268l0,-178C870,74 825,50 760,18C712,-39 688,-87 688,-125C688,-152 698,-175 718,-192C738,-210 761,-219 786,-219C822,-219 856,-212 889,-197M268,553l463,0C722,704 648,780 510,780C375,780 294,704 268,553z""/>
<glyph unicode=""&#x11A;"" horiz-adv-x=""1024"" d=""M154,1397l792,0l0,-178l-592,0l0,-426l572,0l0,-179l-572,0l0,-434l611,0l0,-178l-811,0M880,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x11B;"" horiz-adv-x=""981"" d=""M913,444l-647,0C271,356 300,286 355,234C409,182 479,156 565,156C685,156 796,193 897,268l0,-178C841,53 786,26 731,10C676,-6 611,-14 537,-14C436,-14 354,7 291,49C228,91 178,148 141,219C103,290 84,372 84,465C84,605 124,719 203,807C282,894 385,938 512,938C634,938 731,895 804,810C877,725 913,610 913,467M270,553l463,0C728,626 707,682 668,721C629,760 577,780 512,780C447,780 393,760 352,721C310,682 283,626 270,553M840,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x139;"" horiz-adv-x=""1004"" d=""M154,1397l200,0l0,-1215l629,0l0,-182l-829,0M807,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x13A;"" horiz-adv-x=""449"" d=""M133,1397l182,0l0,-1397l-182,0M449,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x13D;"" horiz-adv-x=""1004"" d=""M154,1397l200,0l0,-1215l629,0l0,-182l-829,0M902,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x13E;"" horiz-adv-x=""597"" d=""M133,1397l182,0l0,-1397l-182,0M430,902l0,38C486,990 514,1059 515,1146C444,1167 409,1211 409,1277C409,1312 421,1343 445,1370C469,1397 498,1410 532,1410C575,1410 609,1391 634,1353C659,1314 671,1269 671,1217C671,1149 650,1085 609,1026C568,966 508,925 430,902z""/>
<glyph unicode=""&#x143;"" horiz-adv-x=""1599"" d=""M1264,1397l190,0l0,-1397l-172,0l-934,1075l0,-1075l-188,0l0,1397l162,0l942,-1084M1047,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x144;"" horiz-adv-x=""1024"" d=""M311,920l0,-117C392,893 485,938 588,938C645,938 699,923 748,894C797,864 835,823 861,772C886,720 899,638 899,526l0,-526l-182,0l0,524C717,618 703,685 674,726C645,766 597,786 530,786C444,786 371,743 311,657l0,-657l-186,0l0,920M737,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x147;"" horiz-adv-x=""1599"" d=""M1264,1397l190,0l0,-1397l-172,0l-934,1075l0,-1075l-188,0l0,1397l162,0l942,-1084M1126,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x148;"" horiz-adv-x=""1024"" d=""M311,920l0,-117C392,893 485,938 588,938C645,938 699,923 748,894C797,864 835,823 861,772C886,720 899,638 899,526l0,-526l-182,0l0,524C717,618 703,685 674,726C645,766 597,786 530,786C444,786 371,743 311,657l0,-657l-186,0l0,920M832,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x150;"" horiz-adv-x=""1686"" d=""M840,1417C1056,1417 1236,1349 1380,1212C1523,1075 1595,904 1595,698C1595,492 1523,322 1378,187C1233,52 1050,-16 829,-16C618,-16 443,52 303,187C162,322 92,491 92,694C92,903 163,1075 304,1212C445,1349 624,1417 840,1417M848,1227C688,1227 557,1177 454,1077C351,977 299,849 299,694C299,543 351,418 454,318C557,218 687,168 842,168C998,168 1128,219 1233,321C1337,423 1389,550 1389,702C1389,850 1337,975 1233,1076C1128,1177 1000,1227 848,1227M945,1800l-239,-300l-111,0l143,300M1272,1800l-237,-300l-113,0l145,300z""/>
<glyph unicode=""&#x151;"" horiz-adv-x=""1130"" d=""M569,922C709,922 825,877 918,787C1011,696 1057,583 1057,446C1057,313 1010,203 916,116C822,28 704,-16 561,-16C423,-16 308,29 215,118C122,207 76,318 76,451C76,586 123,698 217,788C310,877 428,922 569,922M559,758C472,758 400,729 344,672C288,615 260,542 260,453C260,365 289,293 346,238C403,182 477,154 567,154C656,154 730,182 787,239C844,295 872,367 872,455C872,542 842,615 783,672C724,729 649,758 559,758M680,1422l-239,-300l-111,0l143,300M1007,1422l-237,-300l-113,0l145,300z""/>
<glyph unicode=""&#x154;"" horiz-adv-x=""1237"" d=""M160,0l0,1397l350,0C651,1397 764,1362 847,1292C930,1222 971,1127 971,1008C971,927 951,856 910,797C869,738 811,693 735,664C780,635 823,595 866,544C909,493 969,405 1046,279C1095,200 1134,140 1163,100l74,-100l-238,0l-61,92C936,95 932,101 926,109l-39,55l-62,102l-67,109C717,432 679,478 645,512C610,546 579,571 552,586C524,601 477,608 412,608l-52,0l0,-608M420,1227l-60,0l0,-441l76,0C537,786 607,795 645,812C682,829 712,856 733,891C754,926 764,965 764,1010C764,1054 752,1094 729,1130C706,1165 673,1190 631,1205C588,1220 518,1227 420,1227M752,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x155;"" horiz-adv-x=""811"" d=""M324,920l0,-211l10,16C422,867 510,938 598,938C667,938 738,903 813,834l-96,-160C654,734 595,764 541,764C482,764 432,736 389,680C346,624 324,558 324,481l0,-481l-183,0l0,920M701,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x158;"" horiz-adv-x=""1237"" d=""M160,0l0,1397l350,0C651,1397 764,1362 847,1292C930,1222 971,1127 971,1008C971,927 951,856 910,797C869,738 811,693 735,664C780,635 823,595 866,544C909,493 969,405 1046,279C1095,200 1134,140 1163,100l74,-100l-238,0l-61,92C936,95 932,101 926,109l-39,55l-62,102l-67,109C717,432 679,478 645,512C610,546 579,571 552,586C524,601 477,608 412,608l-52,0l0,-608M420,1227l-60,0l0,-441l76,0C537,786 607,795 645,812C682,829 712,856 733,891C754,926 764,965 764,1010C764,1054 752,1094 729,1130C706,1165 673,1190 631,1205C588,1220 518,1227 420,1227M876,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x159;"" horiz-adv-x=""811"" d=""M324,920l0,-211l10,16C422,867 510,938 598,938C667,938 738,903 813,834l-96,-160C654,734 595,764 541,764C482,764 432,736 389,680C346,624 324,558 324,481l0,-481l-183,0l0,920M797,1427l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x15A;"" horiz-adv-x=""938"" d=""M500,586l-152,92C253,736 185,793 145,850C104,906 84,971 84,1044C84,1154 122,1243 199,1312C275,1381 374,1415 496,1415C613,1415 720,1382 817,1317l0,-227C716,1187 608,1235 492,1235C427,1235 373,1220 331,1190C289,1159 268,1120 268,1073C268,1031 283,992 314,955C345,918 394,880 463,840l153,-90C787,649 872,519 872,362C872,250 835,159 760,89C685,19 587,-16 467,-16C329,-16 203,26 90,111l0,254C198,228 323,160 465,160C528,160 580,178 622,213C663,248 684,291 684,344C684,429 623,510 500,586M719,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x15B;"" horiz-adv-x=""788"" d=""M84,66l0,196C135,226 188,197 242,175C295,152 340,141 377,141C415,141 448,150 475,169C502,188 516,210 516,236C516,263 507,285 490,303C472,320 434,346 375,379C258,444 181,500 145,547C108,593 90,643 90,698C90,769 118,826 173,871C228,916 298,938 385,938C475,938 567,913 662,862l0,-180C554,747 466,780 397,780C362,780 333,773 312,758C290,743 279,723 279,698C279,677 289,656 309,637C328,618 363,594 412,567l65,-37C630,443 707,347 707,242C707,167 678,105 619,57C560,8 484,-16 391,-16C336,-16 288,-10 245,2C202,13 149,35 84,66M620,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x162;"" horiz-adv-x=""1237"" d=""M35,1399l1167,0l0,-178l-487,0l0,-1221l-201,0l0,1221l-479,0M523,-578l0,38C579,-490 607,-421 608,-334C537,-313 502,-269 502,-203C502,-168 514,-137 538,-110C562,-83 591,-70 625,-70C668,-70 702,-89 727,-127C752,-166 764,-211 764,-263C764,-331 743,-395 702,-454C661,-514 601,-555 523,-578z""/>
<glyph unicode=""&#x163;"" horiz-adv-x=""682"" d=""M0,774l342,336l0,-190l291,0l0,-164l-291,0l0,-451C342,200 386,147 473,147C538,147 607,169 680,213l0,-170C610,4 534,-16 451,-16C368,-16 298,8 243,57C226,72 211,88 200,107C189,125 179,149 172,179C164,208 160,265 160,348l0,408l-160,0M328,-578l0,38C384,-490 412,-421 413,-334C342,-313 307,-269 307,-203C307,-168 319,-137 343,-110C367,-83 396,-70 430,-70C473,-70 507,-89 532,-127C557,-166 569,-211 569,-263C569,-331 548,-395 507,-454C466,-514 406,-555 328,-578z""/>
<glyph unicode=""&#x164;"" horiz-adv-x=""1237"" d=""M35,1399l1167,0l0,-178l-487,0l0,-1221l-201,0l0,1221l-479,0M939,1809l-227,-309l-189,0l-224,309l114,0l206,-173l206,173z""/>
<glyph unicode=""&#x165;"" horiz-adv-x=""896"" d=""M0,774l342,336l0,-190l291,0l0,-164l-291,0l0,-451C342,200 386,147 473,147C538,147 607,169 680,213l0,-170C610,4 534,-16 451,-16C368,-16 298,8 243,57C226,72 211,88 200,107C189,125 179,149 172,179C164,208 160,265 160,348l0,408l-160,0M722,902l0,38C778,990 806,1059 807,1146C736,1167 701,1211 701,1277C701,1312 713,1343 737,1370C761,1397 790,1410 824,1410C867,1410 901,1391 926,1353C951,1314 963,1269 963,1217C963,1149 942,1085 901,1026C860,966 800,925 722,902z""/>
<glyph unicode=""&#x16E;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M727,1880C778,1880 823,1862 860,1825C897,1788 915,1744 915,1691C915,1638 897,1593 860,1556C823,1519 778,1500 725,1500C673,1500 629,1519 592,1556C555,1593 536,1637 536,1689C536,1742 555,1788 593,1825C630,1862 675,1880 727,1880M722,1799C688,1799 661,1789 641,1769C620,1748 610,1722 610,1689C610,1657 621,1630 644,1608C666,1585 693,1574 725,1574C757,1574 784,1585 807,1607C830,1629 841,1656 841,1687C841,1718 830,1745 807,1767C784,1788 756,1799 722,1799z""/>
<glyph unicode=""&#x16F;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M510,1497C561,1497 606,1479 643,1442C680,1405 698,1361 698,1308C698,1255 680,1210 643,1173C606,1136 561,1117 508,1117C456,1117 412,1136 375,1173C338,1210 319,1254 319,1306C319,1359 338,1405 376,1442C413,1479 458,1497 510,1497M505,1416C471,1416 444,1406 424,1386C403,1365 393,1339 393,1306C393,1274 404,1247 427,1225C449,1202 476,1191 508,1191C540,1191 567,1202 590,1224C613,1246 624,1273 624,1304C624,1335 613,1362 590,1384C567,1405 539,1416 505,1416z""/>
<glyph unicode=""&#x170;"" horiz-adv-x=""1450"" d=""M1126,1397l201,0l0,-793C1327,497 1319,416 1304,361C1288,306 1269,261 1246,225C1223,188 1194,156 1161,127C1050,32 906,-16 727,-16C545,-16 399,31 289,126C256,155 228,188 205,225C182,261 163,305 148,358C133,411 125,493 125,606l0,791l201,0l0,-793C326,473 341,381 371,330C401,279 447,238 508,207C569,176 642,160 725,160C844,160 940,191 1015,253C1054,286 1083,326 1100,371C1117,416 1126,494 1126,604M845,1800l-239,-300l-111,0l143,300M1172,1800l-237,-300l-113,0l145,300z""/>
<glyph unicode=""&#x171;"" horiz-adv-x=""1024"" d=""M715,0l0,117C676,75 632,42 583,19C533,-4 483,-16 434,-16C376,-16 323,-1 274,28C225,57 188,96 163,146C138,195 125,278 125,393l0,527l182,0l0,-525C307,298 321,231 349,193C376,154 425,135 494,135C581,135 654,177 715,262l0,658l182,0l0,-920M602,1422l-239,-300l-111,0l143,300M929,1422l-237,-300l-113,0l145,300z""/>
<glyph unicode=""&#x179;"" horiz-adv-x=""1323"" d=""M82,1399l1204,0l-893,-1221l893,0l0,-178l-1261,0l895,1221l-838,0M895,1800l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x17A;"" horiz-adv-x=""854"" d=""M43,920l786,0l-487,-750l487,0l0,-170l-806,0l483,750l-463,0M651,1419l-226,-300l-112,0l135,300z""/>
<glyph unicode=""&#x17B;"" horiz-adv-x=""1323"" d=""M82,1399l1204,0l-893,-1221l893,0l0,-178l-1261,0l895,1221l-838,0M654,1739C688,1739 717,1727 741,1704C765,1681 777,1652 777,1618C777,1585 765,1557 742,1534C718,1511 689,1500 654,1500C620,1500 591,1511 568,1534C545,1557 533,1585 533,1618C533,1652 545,1681 568,1704C591,1727 620,1739 654,1739z""/>
<glyph unicode=""&#x17C;"" horiz-adv-x=""854"" d=""M43,920l786,0l-487,-750l487,0l0,-170l-806,0l483,750l-463,0M450,1356C484,1356 513,1344 537,1321C561,1298 573,1269 573,1235C573,1202 561,1174 538,1151C514,1128 485,1117 450,1117C416,1117 387,1128 364,1151C341,1174 329,1202 329,1235C329,1269 341,1298 364,1321C387,1344 416,1356 450,1356z""/>
<glyph unicode=""&#x2044;"" horiz-adv-x=""405"" d=""M737,1416l-931,-1471l-138,0l931,1471z""/>
<glyph unicode=""&#xF001;"" horiz-adv-x=""1024"" d=""M346,922l205,0l0,-156l-205,0l0,-766l-182,0l0,766l-137,0l0,156l137,0l0,30C164,1101 198,1215 265,1295C332,1375 428,1415 553,1415C655,1415 736,1389 795,1337C854,1285 885,1213 887,1122l-172,0C697,1171 675,1205 649,1226C623,1247 589,1257 547,1257C412,1257 344,1157 344,956C344,945 345,934 346,922M715,922l172,0l0,-922l-172,0z""/>
<glyph unicode=""&#xF002;"" horiz-adv-x=""1024"" d=""M338,922l209,0l0,-156l-209,0l0,-766l-178,0l0,766l-142,0l0,156l142,0l0,45C160,1067 171,1146 194,1204C217,1262 254,1312 307,1353C360,1394 423,1415 498,1415C541,1415 582,1409 621,1397l0,-166C586,1240 559,1245 539,1245C466,1245 414,1224 384,1182C353,1139 338,1066 338,963M713,1397l182,0l0,-1397l-182,0z""/>
<glyph unicode=""&#x15E;"" horiz-adv-x=""938"" d=""M500,586l-152,92C253,736 185,793 145,850C104,906 84,971 84,1044C84,1154 122,1243 199,1312C275,1381 374,1415 496,1415C613,1415 720,1382 817,1317l0,-227C716,1187 608,1235 492,1235C427,1235 373,1220 331,1190C289,1159 268,1120 268,1073C268,1031 283,992 314,955C345,918 394,880 463,840l153,-90C787,649 872,519 872,362C872,250 835,159 760,89C685,19 587,-16 467,-16C329,-16 203,26 90,111l0,254C198,228 323,160 465,160C528,160 580,178 622,213C663,248 684,291 684,344C684,429 623,510 500,586M365,-578l0,38C421,-490 449,-421 450,-334C379,-313 344,-269 344,-203C344,-168 356,-137 380,-110C404,-83 433,-70 467,-70C510,-70 544,-89 569,-127C594,-166 606,-211 606,-263C606,-331 585,-395 544,-454C503,-514 443,-555 365,-578z""/>
<glyph unicode=""&#x15F;"" horiz-adv-x=""788"" d=""M84,66l0,196C135,226 188,197 242,175C295,152 340,141 377,141C415,141 448,150 475,169C502,188 516,210 516,236C516,263 507,285 490,303C472,320 434,346 375,379C258,444 181,500 145,547C108,593 90,643 90,698C90,769 118,826 173,871C228,916 298,938 385,938C475,938 567,913 662,862l0,-180C554,747 466,780 397,780C362,780 333,773 312,758C290,743 279,723 279,698C279,677 289,656 309,637C328,618 363,594 412,567l65,-37C630,443 707,347 707,242C707,167 678,105 619,57C560,8 484,-16 391,-16C336,-16 288,-10 245,2C202,13 149,35 84,66M289,-578l0,38C345,-490 373,-421 374,-334C303,-313 268,-269 268,-203C268,-168 280,-137 304,-110C328,-83 357,-70 391,-70C434,-70 468,-89 493,-127C518,-166 530,-211 530,-263C530,-331 509,-395 468,-454C427,-514 367,-555 289,-578z""/>
<glyph unicode=""&#xA4;"" horiz-adv-x=""1130"" d=""M213,934l-139,136l115,119l139,-140C363,1074 401,1092 441,1104C480,1116 522,1122 565,1122C609,1122 651,1116 691,1104C730,1092 767,1074 802,1049l139,140l116,-119l-139,-136C965,861 989,782 989,699C989,615 965,536 918,463l139,-136l-116,-119l-139,140C767,324 730,306 691,294C651,281 609,275 565,275C522,275 480,281 441,294C401,306 363,324 328,348l-139,-140l-115,119l139,136C166,536 142,615 142,699C142,782 166,861 213,934M305,699C305,627 330,566 381,515C432,464 493,439 564,439C635,439 696,464 747,515C798,566 823,627 823,699C823,770 798,831 748,882C697,933 636,958 564,958C493,958 432,933 381,882C330,831 305,770 305,699z""/>
</font>

	<rect opacity=""0.5"" fill=""#6ABD45"" width=""30"" height=""8.9""/>
<text transform=""matrix(1 0 0 1 7.7085 6.4961)""><tspan x=""0"" y=""0"" font-family=""'GillSansMT'"" font-size=""5.189px"">HOUSE</tspan><tspan x=""16.7"" y=""0"" font-family=""'GillSansMT-Bold'"" font-size=""5.189px""> {houseEvent.HouseNumber}</tspan></text>
<path d=""M1.6,4.4c-0.2,0-0.4,0-0.6,0c-0.1,0-0.2,0-0.2-0.1c0-0.1,0-0.2,0.1-0.3c0.9-0.9,1.8-1.8,2.7-2.8c0.1-0.1,0.2-0.1,0.4,0
	C4.8,2.2,5.8,3.1,6.7,4c0.1,0.1,0.1,0.2,0.1,0.3c0,0.1-0.1,0.1-0.2,0.1c-0.2,0-0.4,0-0.6,0c0,0.1,0,0.1,0,0.2c0,0.7,0,1.5,0,2.2
	C5.9,7,5.9,7,5.7,7C5.2,7,4.8,7,4.4,7C4.2,7,4.2,7,4.2,6.8c0-0.5,0-0.9,0-1.4c0-0.1,0-0.1-0.1-0.1c-0.2,0-0.5,0-0.7,0
	c-0.1,0-0.1,0-0.1,0.1c0,0.5,0,1,0,1.4C3.3,7,3.3,7,3.1,7C2.7,7,2.3,7,1.8,7C1.7,7,1.6,7,1.6,6.8c0-0.7,0-1.5,0-2.2
	C1.6,4.6,1.6,4.5,1.6,4.4z M4.4,6.9c0.4,0,0.9,0,1.3,0c0-0.1,0-0.1,0-0.2c0-0.7,0-1.4,0-2.1c0-0.3,0-0.3,0.3-0.3c0.2,0,0.3,0,0.5,0
	c0,0,0,0,0,0c-0.9-1-1.9-1.9-2.8-2.9c-0.9,1-1.9,1.9-2.8,2.9c0.1,0,0.1,0,0.2,0c0.2,0,0.3,0,0.5,0c0.2,0,0.2,0,0.2,0.2
	c0,0.7,0,1.5,0,2.2c0,0.1,0,0.1,0,0.2c0.5,0,0.9,0,1.3,0c0-0.1,0-0.1,0-0.2c0-0.5,0-0.9,0-1.4c0-0.2,0-0.2,0.2-0.2
	c0.3,0,0.6,0,0.9,0c0.2,0,0.2,0,0.2,0.2C4.4,5.8,4.4,6.3,4.4,6.9z""/>
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

			var beginYear = timeSlices[0].StdYear();
			var endYear = timeSlices.Last().StdYear();
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
					var yearChanged = previousYear != slice.StdYear();
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
					previousYear = slice.StdYear();

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

				var beginYear = timeSlices[0].StdYear();
				var endYear = beginYear + decade; //decade


				foreach (var slice in timeSlices)
				{

					//only generate new year box when year changes or at
					//end of time slices to draw the last year box
					var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
					var yearChanged = previousYear != slice.StdYear();
					if (yearChanged || lastTimeSlice)
					{
						//is this slice end year & last month (month for accuracy, otherwise border at jan not december)
						//todo begging of box is not beginning of year, possible solution month
						//var isLastMonth = slice.GetStdMonth() is 10 or 11 or 12; //use oct & nov in-case december is not generated at low precision 
						var isEndYear = endYear == slice.StdYear();
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
					previousYear = slice.StdYear();

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

				var beginYear = timeSlices[0].StdYear();
				var endYear = beginYear + yearRange;


				foreach (var slice in timeSlices)
				{

					//only generate new year box when year changes or at
					//end of time slices to draw the last year box
					var lastTimeSlice = timeSlices.IndexOf(slice) == timeSlices.Count - 1;
					var yearChanged = previousYear != slice.StdYear();
					if (yearChanged || lastTimeSlice)
					{
						//is this slice end year
						var isEndYear = endYear == slice.StdYear();
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
					previousYear = slice.StdYear();

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
					var monthChanged = previousMonth != slice.StdMonth();
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
					previousMonth = slice.StdMonth();

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
					var dateChanged = previousDate != slice.StdDate();
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
					previousDate = slice.StdDate();

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
					var hourChanged = previousHour != slice.StdHour();
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
					previousHour = slice.StdHour();

					hourBoxWidthCount++;
				}

				//wrap all the rects inside a svg so they can me moved together
				//svg tag here acts as group, svg nesting
				rowHtml = $"<g transform=\"matrix(1, 0, 0, 1, {xAxis}, {yAxis})\">{rowHtml}</g>";

				return rowHtml;

			}

		}

		private static async Task<string> GetAllPlanetLineIcons(List<PlanetLongitude> planetList, double widthPx, int iconStartYAxis, Time time)
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
				var generateLifeEventLine = await GetPlanetLineIcon(planet, adjustedLineHeight, transformedxAxis, time);

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

		private static async Task<string> GetPlanetLineIcon(PlanetLongitude planet, int lineHeight, double positionX, Time time)
		{

			//based on length of event name make the background
			//mainly done to shorten background of short names (saving space)
			var planetName = planet.GetPlanetName().ToString();
			var backgroundWidth = Tools.GetTextWidthPx(planetName);

			var planetIcon = await GetPlanetIcon(planet.GetPlanetName(), time);

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

		private static ConcurrentDictionary<string, string> PlanetIconMemoryCache = new ConcurrentDictionary<string, string>();

		/// <summary>
		/// creates a url to image that should exist in SkyChart folder, auto cached
		/// in memory cache for per instance only
		/// </summary>
		private static async Task<string> GetPlanetIcon(PlanetName planet, Time time)
		{
			string svgIconHttp = "";

			//SPECIAL FOR MOON SINCE SHE CHANGES EVERY DAY
			//for moon special changing icon based on lunar day
			if (planet.Name == PlanetName.PlanetNameEnum.Moon)
			{
				//get moon lunar day to 
				var lunarDay = Calculate.LunarDay(time).GetLunarDateNumber();

				//make url for specific lunar frame
				var svgFileUrl = $"{URL.WebStable}/images/SkyChart/{planet.Name.ToString().ToLower()}-{lunarDay}.svg";

				//check if icon already gotten before
				PlanetIconMemoryCache.TryGetValue(svgFileUrl, out svgIconHttp); //note use with lunar date

				if (string.IsNullOrEmpty(svgIconHttp))
				{

					//makes it ready to be injected into another SVG
					svgIconHttp = await Tools.GetSvgIconHttp(svgFileUrl, 45, 45);

					//place in memory
					PlanetIconMemoryCache[planet.Name.ToString()] = svgIconHttp;
				}
				else
				{
					//Console.WriteLine("PLANET ICON CACHE USED!");
				}

				return svgIconHttp ?? "";


			}

			//ALL OTHER PLANETS
			else
			{

				//check if icon already gotten before
				PlanetIconMemoryCache.TryGetValue(planet.Name.ToString(), out svgIconHttp);

				//if no cache then get new
				if (string.IsNullOrEmpty(svgIconHttp))
				{
					var svgFileUrl = $"{URL.WebStable}/images/SkyChart/{planet.Name.ToString().ToLower()}.svg";

					//makes it ready to be injected into another SVG
					svgIconHttp = await Tools.GetSvgIconHttp(svgFileUrl, 45, 45);

					//place in memory
					PlanetIconMemoryCache[planet.Name.ToString()] = svgIconHttp;
				}
				else
				{
					//Console.WriteLine("PLANET ICON CACHE USED!");
				}

				return svgIconHttp ?? "";
			}


		}

		private static string GetColor(PlanetLongitude planet)
		{
			return "green";
		}


		//----------PRIVATE



	}

	internal record HouseEvent(HouseName HouseNumber, int StartX, int EndX);
}
