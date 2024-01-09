using System.Collections.Generic;
using System.Linq;
using static VedAstro.Library.PlanetName;

namespace VedAstro.Library
{
    /// <summary>
    /// Calculators for all Ashtakavarga related caluculations
    /// </summary>
    public static class Ashtakavarga
    {

        //NOTE: below table is default shortcut table, used to make final Sarvashtakavarga Chart
        //without using Bhinnashtaka chart, also outputs in popularly accepted format (with Lagna)
        static readonly Dictionary<string, int[]> ShortcutTable = new Dictionary<string, int[]>
        {
            { Sun.ToString(), new[] { 3, 3, 3, 3, 2, 3, 4, 5, 3, 5, 7, 2 } },
            { Moon.ToString(), new[] { 2, 3, 5, 2, 2, 5, 2, 2, 2, 3, 7, 1 } },
            { Mars.ToString(), new[] { 4, 5, 3, 4, 3, 3, 4, 4, 4, 6, 7, 2 } },
            { Mercury.ToString(), new[] { 3, 1, 5, 2, 6, 6, 1, 2, 5, 5, 7, 3 } },
            { Jupiter.ToString(), new[] { 2, 1, 1, 2, 3, 4, 2, 4, 2, 4, 7, 4 } },
            { Venus.ToString(), new[] { 2, 3, 3, 3, 4, 4, 2, 3, 4, 3, 6, 3 } },
            { Saturn.ToString(), new[] { 3, 2, 4, 4, 4, 3, 3, 4, 4, 4, 6, 1 } },
            { "Ascendant", new[] { 5, 3, 5, 5, 2, 6, 1, 2, 2, 6, 7, 1 } },
        };


        /// <summary>
        /// Tabulating the Bhinnashtaka of each planet. This would give us a 'scattered' picture known 
        /// as the "Prastaraka Ashtakavarga". From this can be derived the Bhinnashtaka varga of each planet.
        /// Note: Rahu & Ketu not supported
        /// </summary>
        public static Prastaraka PlanetPrastaraka(PlanetName inputPlanet, Time birthTime)
        {
            //8 cardinal points including ascendant
            var minorPlanetList = All7Planets.Select(e => e.ToString()).ToList();
            minorPlanetList.Add("Ascendant"); //add special case Ascendant for Ashtakvarga calculation

            //load the benefic places for all the minor planets
            var allPlanetBeneficList = new Dictionary<string, int[]>();
            foreach (var minorPlanet in minorPlanetList)
            {
                var mainPlanetBeneficList = PlanetBeneficAshtakaHouse(inputPlanet.Name.ToString(), minorPlanet);
                allPlanetBeneficList.Add(minorPlanet, mainPlanetBeneficList);
            }

            //create place to pack the data
            var returnVal = new Prastaraka();

            //go through benefic house list and add points to each sign (make row)
            foreach (var minorPlanet in minorPlanetList)
            {
                //parse minor planet type, if ascendant get sign of 1st house
                //start sign can be from planet or 1st house (Ascendant)
                var isAscendant = minorPlanet == "Ascendant";
                var minorPlanetStartSign = isAscendant
                    ? Calculate.HouseSignName(HouseName.House1, birthTime)
                    : Calculate.PlanetZodiacSign(Parse(minorPlanet), birthTime).GetSignName();

                var prastarakaRow = ZodiacNameExtensions.AllZodiacSignsDictionary(0);

                //add the points together, add 1 for a benefic sign
                foreach (var houseCount in allPlanetBeneficList[minorPlanet])
                {
                    var signXFromPlanet = Calculate.SignCountedFromInputSign(minorPlanetStartSign, houseCount);
                    prastarakaRow[signXFromPlanet] = 1;
                }
                returnVal[minorPlanet] = prastarakaRow;

            }

            return returnVal;

        }

        /// <summary>
        /// Calculates Sarvashtakavarga chart using lesser used method Bhinashtaka chart method
        /// </summary>
        public static Sarvashtakavarga SarvashtakavargaChart(Time birthTime)
        {
            var returnVal = new Sarvashtakavarga();

            //get bhinnashtaka chart for all 7 planets
            var bhinnashtakaChart = Calculate.BhinnashtakavargaChart(birthTime);

            var dict1 = ZodiacNameExtensions.AllZodiacSignsDictionary(0);
            foreach (var chart in bhinnashtakaChart.Dictionary)
            {
                var dict2 = chart.Value;

                //combine bhina points for each sign for all planets
                Dictionary<ZodiacName, int> combinedDict = dict1
                    .Concat(dict2)
                    .GroupBy(kvp => kvp.Key)
                    .ToDictionary(group => group.Key, group => group.Sum(kvp => kvp.Value));

                dict1 = combinedDict;
            }

            return returnVal;
        }

        /// <summary>
        /// Calculates Sarvashtakavarga chart using popular shortcut table
        /// </summary>
        public static Sarvashtakavarga SarvashtakavargaChart2(Time birthTime)
        {
            var returnVal = new Sarvashtakavarga();

            //adjust the shortcut table based on the birth time
            //8 cardinal points including ascendant
            foreach (var astakaRow in ShortcutTable)
            {
                //#1 FIND WHERE THE PLANET IS (SIGN)
                //parse minor planet type, if ascendant get sign of 1st house
                //start sign can be from planet or 1st house (Ascendant)
                var isAscendant = astakaRow.Key == "Ascendant";
                var planetStartSign = isAscendant
                    ? Calculate.HouseSignName(HouseName.House1, birthTime)
                    : Calculate.PlanetZodiacSign(Parse(astakaRow.Key), birthTime).GetSignName();

                //#2 MOVE ROW TO RIGHT BY DISTANCE FROM ARIES
                var distanceFromAries = (int)planetStartSign;
                var shortcutRow = astakaRow.Value;
                var arrayIntLength = shortcutRow.Length + 1; //add 1 because 0 based index
                var shiftedArray = shortcutRow.Skip(arrayIntLength - distanceFromAries).Concat(shortcutRow.Take(arrayIntLength - distanceFromAries)).ToArray();

                //#3 ADD ROW TO MODIFIED TABLE
                returnVal[astakaRow.Key] = shiftedArray;
               
            }

            //create final Sarvashtakavarga row based on adjusted shortcut table
            foreach (var row in ZodiacNameExtensions.AllZodiacSignsDictionary(0))
            {
                //add all the points together for current sign
                var currentSignNumber = (int)row.Key;

                var totalForSign = 0;
                foreach (var xx in returnVal.Rows)
                {
                    totalForSign += xx.Value[currentSignNumber - 1];//-1 compensate 0 based index
                }

                //add total to final row
                returnVal.SarvashtakavargaRow[row.Key] = totalForSign;
            }

            return returnVal;

        }

        /// <summary>
        /// Calculates full ashtakvarga chart for a given planet for all 12 signs
        /// Used to for calculating final Ashtakvarga, Rahu & Ketu will return 0
        /// </summary>
        public static Dictionary<ZodiacName, int> BhinnashtakavargaChartForPlanet(PlanetName mainPlanet, Time birthTime)
        {
            //no rahu & ketu, so re
            if (mainPlanet.Name is PlanetNameEnum.Rahu or PlanetNameEnum.Ketu) { return new Dictionary<ZodiacName, int>(); }

            //make the charts compiled from the position of 7 planets plus Ascendant
            var minorPlanetList = All7Planets.Select(e => e.ToString()).ToList();
            minorPlanetList.Add("ascendant"); //add special case Ascendant for Ashtakvarga calculation

            //load the benefic places for all the minor planets
            var allPlanetBeneficList = new Dictionary<string, int[]>();
            foreach (var minorPlanet in minorPlanetList)
            {
                //fixed positions from a given planet that is positive (table data)
                var mainPlanetBeneficList = PlanetBeneficAshtakaHouse(mainPlanet.ToString(), minorPlanet);
                allPlanetBeneficList.Add(minorPlanet, mainPlanetBeneficList);
            }

            //Bhinnashtakavarga chart in array form
            var mainPlanetBhinaAstaChart = ZodiacNameExtensions.AllZodiacSignsDictionary(0);
            foreach (var minorPlanet in minorPlanetList)
            {
                //parse minor planet type, if ascendant get sign of 1st house
                //start sign can be from planet or 1st house (Ascendant)
                var isAscendant = minorPlanet == "ascendant";
                var minorPlanetStartSign = isAscendant
                    ? Calculate.HouseSignName(HouseName.House1, birthTime)
                    : Calculate.PlanetZodiacSign(Parse(minorPlanet), birthTime).GetSignName();

                //add the points together, add 1 for a benefic sign
                foreach (var houseCount in allPlanetBeneficList[minorPlanet])
                {
                    var signXFromPlanet = Calculate.SignCountedFromInputSign(minorPlanetStartSign, houseCount);
                    mainPlanetBhinaAstaChart[signXFromPlanet] += 1; //add 1 to existing score from previous
                }
            }

            //return compiled chart
            return mainPlanetBhinaAstaChart;
        }

        /// <summary>
        /// Returns the specific houses where each of the seven 
        /// planets proves auspicious when considered from each of the eight
        /// cardinal points (the seven planets and the lagna). This gives the
        /// Bhinnashtaka of the inputed planets. 
        /// (this a constant that does not change from Ashtakvarga System pg.9)
        /// </summary>
        public static int[] PlanetBeneficAshtakaHouse(string mainPlanet, string minorPlanet)
        {
            // Code Poetry:
            // This is data that once was stored in human brains,
            // Now only typed by human fingers' strain,
            // Soon only known to be, without a trace.
            //
            // The wisdom of ages, once passed down by word,
            // Now stored in circuits, rarely heard.

            //load the data from ashtakvarga table (Ashtakvarga System pg.9)
            #region LOAD DATA

            var dictionary = new Dictionary<(string, string), int[]>();

            //Below are given the specific houses where each of the seven
            //planets proves auspicious when considered from each of the eight
            //cardinal points(the seven planets and the lagna).This gives the
            //Bhinnashtaka of the seven planets.

            //---------------- SUN --------------

            //Total 48 points.
            //The Sun :- The Sun's benefic places are the
            //1st, 2nd, 4th, 7th, 8th, 9th, 10th and 11th from
            //himself; the same places from Mars and Saturn
            dictionary.Add(("sun", "sun"), new[] { 1, 2, 4, 7, 8, 9, 10, 11 });
            dictionary.Add(("sun", "mars"), new[] { 1, 2, 4, 7, 8, 9, 10, 11 });
            dictionary.Add(("sun", "saturn"), new[] { 1, 2, 4, 7, 8, 9, 10, 11 });

            //the 5th, 6th, 9th and 11th from Jupiter
            dictionary.Add(("sun", "jupiter"), new[] { 5, 6, 9, 11 });

            //he 3rd, 6th, 10th and 11th from the Moon;
            dictionary.Add(("sun", "moon"), new[] { 3, 6, 10, 11 });

            //the 3rd, 5th, 6th, 9th, 10th, 11th and 12th from Mercury;
            dictionary.Add(("sun", "mercury"), new[] { 3, 5, 6, 9, 10, 11, 12 });

            //the 3rd, 4th, 6th, 10th, 11th and 12th from the Ascendant
            dictionary.Add(("sun", "ascendant"), new[] { 3, 4, 6, 10, 11, 12 });

            //nd the 6th, 7th and 12th from Venus
            dictionary.Add(("sun", "venus"), new[] { 6, 7, 12 });

            //---------------- MOON --------------
            //Total 49 points
            //The Moon :- The benefic places of the Moon
            //are the 3rd, 6th, 10th and 11th houses from the Ascendant
            dictionary.Add(("moon", "ascendant"), new[] { 3, 6, 10, 11 });

            //The Moon is auspicious in the 2nd, 3rd, 5th, 6th, 9th, 10th and 11th houses from Mars;
            dictionary.Add(("moon", "mars"), new[] { 2, 3, 5, 6, 9, 10, 11 });

            //the 1st, 3rd, 6th, 7th, 10th and 11th houses from the Moon herself;
            dictionary.Add(("moon", "moon"), new[] { 1, 3, 6, 7, 10, 11 });

            //the 3rd, 6th, 7th, 8th, 10th and 11th from the Sun;
            dictionary.Add(("moon", "sun"), new[] { 3, 6, 7, 8, 10, 11 });

            //the 3rd, 5th, 6th and 11th from Saturn
            dictionary.Add(("moon", "saturn"), new[] { 3, 5, 6, 11 });

            //the 1st, 3rd, 4th, 5th, 7th 8th 10th and 11th from Mercury
            dictionary.Add(("moon", "mercury"), new[] { 1, 3, 4, 5, 7, 8, 10, 11 });

            //the 1st, 4th, 7th, 8th, 10th, 11th ·and 12th from Jupiter
            dictionary.Add(("moon", "jupiter"), new[] { 1, 4, 7, 8, 10, 11, 12 });

            //and the 3rd, 4th, 5th, 7th, 9th, 10th and 11th from Venus:
            dictionary.Add(("moon", "venus"), new[] { 3, 4, 5, 7, 9, 10, 11 });

            //---------------- MARS --------------
            //Total 39 potnts.
            //Mars :- The benefic places of Mars will be the
            //3rd, 5th, 6th, 10th and 11th houses from the Sun;
            dictionary.Add(("mars", "sun"), new[] { 3, 5, 6, 10, 11 });

            //the 1st, 3rd, 6th, 10th and 11th from the Ascendant;
            dictionary.Add(("mars", "ascendant"), new[] { 1, 3, 6, 10, 11 });

            //the 3rd, 6th and 11th from the Moon;
            dictionary.Add(("mars", "moon"), new[] { 3, 6, 11 });

            //the 1st, 2nd, 4th, 7th, 8th, 10th and 11th from himself;
            dictionary.Add(("mars", "mars"), new[] { 1, 2, 4, 7, 8, 10, 11 });

            //in 1, 4, 7, 8, 9, 10 and 11 from Saturn
            dictionary.Add(("mars", "saturn"), new[] { 1, 4, 7, 8, 9, 10, 11 });

            //in 3, 5, 6 and 11 from Mercury
            dictionary.Add(("mars", "mercury"), new[] { 3, 5, 6, 11 });

            //in 6, 8, 11 and 12 from Venus;
            dictionary.Add(("mars", "venus"), new[] { 6, 8, 11, 12 });

            //and in 6, 10, 11 and 12 from Jupiter.
            dictionary.Add(("mars", "jupiter"), new[] { 6, 10, 11, 12 });


            //---------------- MERCURY --------------
            //Total 54 points
            //Mercury :- Mercury produces good in
            //1, 2, 3, 4, 5, 8, 9 and 11 from Venus;
            dictionary.Add(("mercury", "venus"), new[] { 1, 2, 3, 4, 5, 8, 9, 11 });

            //1, 2, 4, 7, 8, 9, 10 and 11 from Mars and Saturn;
            dictionary.Add(("mercury", "mars"), new[] { 1, 2, 4, 7, 8, 9, 10, 11 });
            dictionary.Add(("mercury", "saturn"), new[] { 1, 2, 4, 7, 8, 9, 10, 11 });

            //in 6, 8, 11 and 12 from Jupiter;
            dictionary.Add(("mercury", "jupiter"), new[] { 6, 8, 11, 12 });

            //in 5, 6, 9, 11 and 12 from the Sun;
            dictionary.Add(("mercury", "sun"), new[] { 5, 6, 9, 11, 12 });

            //in 1, 3, 5, 6, 9, 10, 11 and 12 from himself;
            dictionary.Add(("mercury", "mercury"), new[] { 1, 3, 5, 6, 9, 10, 11, 12 });

            //in 2, 4, 6, 8, 10 and 11 from the Moon;
            dictionary.Add(("mercury", "moon"), new[] { 2, 4, 6, 8, 10, 11 });

            //and in 1, 2, 4, 6, 8, 10 and 11 from the Ascendant.
            dictionary.Add(("mercury", "ascendant"), new[] { 1, 2, 4, 6, 8, 10, 11 });

            //---------------- JUPITER --------------
            //Total 56 points;
            //Jupiter :-Jupiter will be auspicious in
            //1, 2, 4, 7, 8, 10 and 11 from Mars;
            dictionary.Add(("jupiter", "mars"), new[] { 1, 2, 4, 7, 8, 10, 11 });

            //in 1, 2, 3, 4, 7, 8, 10 and 11 from himself;
            dictionary.Add(("jupiter", "jupiter"), new[] { 1, 2, 3, 4, 7, 8, 10, 11 });

            //in 1, 2, 3, 4, 7, 8, 9, 10 and 11 from the Sun;
            dictionary.Add(("jupiter", "sun"), new[] { 1, 2, 3, 4, 7, 8, 9, 10, 11 });

            //in 2, 5, 6, 9, 10 and 11 from Venus;
            dictionary.Add(("jupiter", "venus"), new[] { 2, 5, 6, 9, 10, 11 });

            //in 2, 5, 7, 9 and 11 from the Moon;
            dictionary.Add(("jupiter", "moon"), new[] { 2, 5, 7, 9, 11 });

            //in 3, 5, 6 and 12 from Saturn;
            dictionary.Add(("jupiter", "saturn"), new[] { 3, 5, 6, 12 });

            //in 1, 2, 4, 5, 6, 9, 10 and 11 from Mercury;
            dictionary.Add(("jupiter", "mercury"), new[] { 1, 2, 4, 5, 6, 9, 10, 11 });

            //and in 1, 2, 4, 5, 6, 7, 9, 10 and 11 from the Ascendant.
            dictionary.Add(("jupiter", "ascendant"), new[] { 1, 2, 4, 5, 6, 7, 9, 10, 11 });

            //---------------- VENUS --------------
            //Total 52 points
            //Venus :-- Venus produces good in
            //1, 2, 3, 4, 5, 8, 9 and 11 from Lagna;
            dictionary.Add(("venus", "ascendant"), new[] { 1, 2, 3, 4, 5, 8, 9, 11 });

            //in 1, 2, 3, 4, 5, 8, 9, 11 and 12 from the Moon;
            dictionary.Add(("venus", "moon"), new[] { 1, 2, 3, 4, 5, 8, 9, 11, 12 });

            //in 1, 2, 3, 4, 5, 8, 9, 10 and 11 from himself;
            dictionary.Add(("venus", "venus"), new[] { 1, 2, 3, 4, 5, 8, 9, 10, 11 });

            //in 3, 4, 5, 8, 9, 10 and 11 from Saturn;
            dictionary.Add(("venus", "saturn"), new[] { 3, 4, 5, 8, 9, 10, 11 });

            //in 8, 11 and 12 from the Sun ;
            dictionary.Add(("venus", "sun"), new[] { 8, 11, 12 });

            //in 5, 8, 9, 10 and 11 from Jupiter;
            dictionary.Add(("venus", "jupiter"), new[] { 5, 8, 9, 10, 11 });

            //in 3, 5, 6, 9 and 11 from Mercury;
            dictionary.Add(("venus", "mercury"), new[] { 3, 5, 6, 9, 11 });

            //and in 3, 5, 6, 9, 11 and 12 from Mars.
            dictionary.Add(("venus", "mars"), new[] { 3, 5, 6, 9, 11, 12 });

            //---------------- SATURN --------------
            //Total 39 points.
            //Saturn :-Saturn is beneficial in
            //3, 5, 6 and 11 from himself;
            dictionary.Add(("saturn", "saturn"), new[] { 3, 5, 6, 11 });

            //in 3, 5, 6, 10, 11 and 12 from Mars;
            dictionary.Add(("saturn", "mars"), new[] { 3, 5, 6, 10, 11, 12 });

            //in 1, 2, 4, 7, 8, 10 and 11 from the Sun;
            dictionary.Add(("saturn", "sun"), new[] { 1, 2, 4, 7, 8, 10, 11 });

            //in 1, 3, 4, 6, 10 and 11 from the Ascendant;
            dictionary.Add(("saturn", "ascendant"), new[] { 1, 3, 4, 6, 10, 11 });

            //in 6, 8, 9, 10, 11 and 12 from Mercury;
            dictionary.Add(("saturn", "mercury"), new[] { 6, 8, 9, 10, 11, 12 });

            //in 3, 6 and 11 from the Moon;
            dictionary.Add(("saturn", "moon"), new[] { 3, 6, 11 });

            //in 6, 11 and 12 from Venus;
            dictionary.Add(("saturn", "venus"), new[] { 6, 11, 12 });

            //and in 5, 6, 11 and 12 from Jupiter
            dictionary.Add(("saturn", "jupiter"), new[] { 5, 6, 11, 12 });

            #endregion

            //make data small caps for sure check
            mainPlanet = mainPlanet.ToLower();
            minorPlanet = minorPlanet.ToLower();

            //get value from dictionary
            var returnValue = dictionary[(mainPlanet, minorPlanet)];
            return returnValue;
        }

    }
}







//--------ARCHIVED CODE BELOW------------

///// <summary>
///// Bhinnashtakavarga or individual Ashtakvarga charts
///// Seven different charts are thus possible for the seven different
///// planets. These are called as Bhinnashtakavargas. The position
///// of each planet in the natal chart is of primary consideration.
///// 
///// List of planets & ascendant with their bindu point
///// </summary>
//public static Dictionary<PlanetName, Dictionary<ZodiacName, int>> Bhinnashtakavarga(Time birthTime)
//{
//    //Made on cold winter morning in July

//    var minorPlanetList = All7Planets.Select(e => e.ToString()).ToList();
//    minorPlanetList.Add("ascendant"); //add special case Ascendant for Ashtakvarga calculation
//    var mainBhinaAstaChart = new Dictionary<PlanetName, Dictionary<ZodiacName, int>>();

//    //make the charts compiled from the position of 7 planets
//    foreach (var mainPlanet in All7Planets)
//    {
//        //load the benefic places for all the minor planets
//        var allPlanetBeneficList = new Dictionary<string, int[]>();
//        foreach (var minorPlanet in minorPlanetList)
//        {
//            var mainPlanetBeneficList = PlanetBeneficAshtakaHouse(mainPlanet.Name.ToString(), minorPlanet);
//            allPlanetBeneficList.Add(minorPlanet, mainPlanetBeneficList);
//        }

//        //Bhinnashtakavarga chart in array form
//        var mainPlanetBhinaAstaChart = ZodiacNameExtensions.AllZodiacSignsDictionary(0);
//        foreach (var minorPlanet in minorPlanetList)
//        {
//            //parse minor planet type, if ascendant get sign of 1st house
//            //start sign can be from planet or 1st house (Ascendant)
//            var isAscendant = minorPlanet == "ascendant";
//            var minorPlanetStartSign = isAscendant
//                                        ? Calculate.HouseSignName(HouseName.House1, birthTime)
//                                        : Calculate.PlanetZodiacSign(Parse(minorPlanet), birthTime).GetSignName();

//            //add the points together, add 1 for a benefic sign
//            foreach (var houseCount in allPlanetBeneficList[minorPlanet])
//            {
//                var signXFromPlanet = Calculate.SignCountedFromInputSign(minorPlanetStartSign, houseCount);
//                mainPlanetBhinaAstaChart[signXFromPlanet] += 1; //add 1 to existing score from previous
//            }
//        }

//        //add the compiled main planet's chart to main list
//        mainBhinaAstaChart.Add(mainPlanet, mainPlanetBhinaAstaChart);
//    }


//    //return compiled charts for all 7 planets
//    return mainBhinaAstaChart;

//}
