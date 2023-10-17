

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SwissEphNet;
using static VedAstro.Library.PlanetName;
using static VedAstro.Library.HouseName;
using static VedAstro.Library.ZodiacName;
using static VedAstro.Library.Ayanamsa;
using System.Numerics;
using ExCSS;


namespace VedAstro.Library
{

    //█▀▄▀█ █▀▀ ▀▀█▀▀ █░░█ █▀▀█ █▀▀▄ 　 ▀▀█▀▀ █▀▀█ 　 ▀▀█▀▀ █░░█ █▀▀ 　 █▀▄▀█ █▀▀█ █▀▀▄ █▀▀▄ █▀▀ █▀▀ █▀▀ 
    //█░▀░█ █▀▀ ░░█░░ █▀▀█ █░░█ █░░█ 　 ░░█░░ █░░█ 　 ░░█░░ █▀▀█ █▀▀ 　 █░▀░█ █▄▄█ █░░█ █░░█ █▀▀ ▀▀█ ▀▀█ 
    //▀░░░▀ ▀▀▀ ░░▀░░ ▀░░▀ ▀▀▀▀ ▀▀▀░ 　 ░░▀░░ ▀▀▀▀ 　 ░░▀░░ ▀░░▀ ▀▀▀ 　 ▀░░░▀ ▀░░▀ ▀▀▀░ ▀░░▀ ▀▀▀ ▀▀▀ ▀▀▀ 

    //█▀▀█ █▀▀█ █▀▀▄ █▀▀ █▀▀█ 　 ▀▀█▀▀ █▀▀█ 　 ▀▀█▀▀ █░░█ █▀▀ 　 █▀▀ █░░█ █▀▀█ █▀▀█ █▀▀ 
    //█░░█ █▄▄▀ █░░█ █▀▀ █▄▄▀ 　 ░░█░░ █░░█ 　 ░░█░░ █▀▀█ █▀▀ 　 █░░ █▀▀█ █▄▄█ █░░█ ▀▀█ 
    //▀▀▀▀ ▀░▀▀ ▀▀▀░ ▀▀▀ ▀░▀▀ 　 ░░▀░░ ▀▀▀▀ 　 ░░▀░░ ▀░░▀ ▀▀▀ 　 ▀▀▀ ▀░░▀ ▀░░▀ ▀▀▀▀ ▀▀▀

    /// <summary>
    /// Collection of astronomical calculator functions
    /// Note : Many of the functions here use cacheing machanism
    /// </summary>
    public static partial class Calculate
    {

        #region SETTINGS

        /// <summary>
        /// Defaults to RAMAN, but can be set before calling any funcs,
        /// NOTE: remember not to change mid instance, because "GetAyanamsa" & others are cached per instance
        /// </summary>
        public static int Ayanamsa { get; set; } = (int)SimpleAyanamsa.LahiriChitrapaksha;


        /// <summary>
        /// If set true, will not include gochara that was obstructed by "Vedhanka Point" calculation
        /// Enabled by default, recommend only disabled for research & debugging.
        /// Vedhanka needed for accuracy, recommended leave true
        /// </summary>
        public static bool UseVedhankaInGochara { get; set; } = true;

        /// <summary>
        /// Defaults to mean Rahu & Ketu positions for a more even value,
        /// set to false to use true node.
        /// Correlates to Swiss Ephemeris, SE_TRUE_NODE & SE_MEAN_NODE
        /// </summary>
        public static bool UseMeanRahuKetu { get; set; } = true;

        #endregion

        //----------------------------------------CORE CODE---------------------------------------------

        #region AVASTA

        /// <summary>
        /// Gets all the Avastas for a planet, Lajjita, Garvita, Kshudita, etc...
        /// </summary>
		public static List<Avasta> PlanetAvasta(PlanetName planetName, Time time)
        {
            var finalList = new Avasta?[6]; //total 6 avasta

            //add in each avasta that matches
            finalList[0] = IsPlanetInLajjitaAvasta(planetName, time) ? Avasta.LajjitaShamed : null;
            finalList[1] = IsPlanetInGarvitaAvasta(planetName, time) ? Avasta.GarvitaProud : null;
            finalList[2] = IsPlanetInKshuditaAvasta(planetName, time) ? Avasta.KshuditaStarved : null;
            finalList[3] = IsPlanetInTrashitaAvasta(planetName, time) ? Avasta.TrishitaThirst : null;
            finalList[4] = IsPlanetInMuditaAvasta(planetName, time) ? Avasta.MuditaDelighted : null;
            finalList[5] = IsPlanetInKshobhitaAvasta(planetName, time) ? Avasta.KshobitaAgitated : null;

            // Convert array to List<Avasta> and remove nulls
            var resultList = finalList.OfType<Avasta>().ToList();
            return resultList;

        }

        /// <summary>
        /// Lajjita / humiliated : Planet in the 5th house in conjunction with rahu or ketu, Saturn or mars.
        /// </summary>
        public static bool IsPlanetInLajjitaAvasta(PlanetName planetName, Time time)
        {
            //check if input planet is in 5th
            var isPlanetIn5thHouse = IsPlanetInHouse(time, planetName, HouseName.House5);

            //check if any negative planets is in 5th (conjunct)
            var planetNames = new List<PlanetName>() { Rahu, Ketu, Saturn, Mars };
            var rahuKetuSaturnMarsIn5th = IsAllPlanetInHouse(time, planetNames, HouseName.House5);

            //check if all conditions are met Lajjita
            var isLajjita = isPlanetIn5thHouse && rahuKetuSaturnMarsIn5th;

            return isLajjita;

        }


        /// <summary>
        /// Garvita, proud : Planet in exaltation sign or moolatrikona zone, happiness and gains
        /// </summary>
        public static bool IsPlanetInGarvitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in exaltation sign
            var planetExalted = IsPlanetExalted(planetName, time);

            //moolatrikona zone
            var planetInMoolatrikona = IsPlanetInMoolatrikona(planetName, time);

            //check if all conditions are met for Garvita
            var isGarvita = planetExalted || planetInMoolatrikona;

            return isGarvita;
        }

        /// <summary>
        /// Kshudita, hungry : Planet in enemy’s sign or conjoined with enemy or aspected by enemy, Grief
        /// </summary>
        public static bool IsPlanetInKshuditaAvasta(PlanetName planetName, Time time)
        {
            //Planet in enemy’s sign 
            var planetExalted = IsPlanetInEnemyHouse(time, planetName);

            //conjoined with enemy (same house)
            var conjunctWithMalefic = IsPlanetConjunctWithEnemyPlanets(planetName, time);

            //aspected by enemy
            var aspectedByMalefic = IsPlanetAspectedByEnemyPlanets(planetName, time);

            //check if all conditions are met for Kshudita
            var isKshudita = planetExalted || conjunctWithMalefic || aspectedByMalefic;

            return isKshudita;
        }

        /// <summary>
        /// Trashita, thirsty – Planet in a watery sign, aspected by a enemy and is without the aspect of benefic Planets
        /// 
        /// The Planet who being conjoined or aspected by a Malefic or his enemy Planet is situated,
        /// without the aspect of a benefic Planet, in the 4th House is Trashita.
        /// 
        /// Another version
        /// 
        /// If the Planet is situated in a watery sign, is aspected by an enemy Planet and
        /// is without the aspect of benefic Planets he is called Trashita.
        ///
        /// --------
        /// "A planet in a Water Sign and aspected by an enemy planet,
        /// with no auspiscious Graha aspecting is said to be Trishita Avastha/Thirsty State".
        /// 
        /// This state is in effect whenever a planet is in a Water Sign and it gets
        /// aspected by an enemy planet. But if, a Gentle Planet (Mercury/Venus/Moon) aspects here,
        /// it strengthens the planet in Water Sign. This Avastha is only for the aspecting enemy
        /// planet that will cause Trishita/Thirst. This state shows that a planet in a watery
        /// Rasi can still be productive even when aspected by enemies, though it will not be happy.
        /// As the name “Thirsty State” implies, it indicates the lack of emotional fulfillment that a planet experiences.
        /// </summary>
		public static bool IsPlanetInTrashitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in a watery sign
            var planetInWater = IsPlanetInWaterySign(planetName, time);

            //aspected by an enemy
            var aspectedByEnemy = IsPlanetAspectedByEnemyPlanets(planetName, time);

            //no benefic planet aspect
            var noBeneficAspect = false == IsPlanetAspectedByBeneficPlanets(planetName, time);

            //check if all conditions are met for Trashita
            var isTrashita = planetInWater && aspectedByEnemy && noBeneficAspect;

            return isTrashita;
        }

        /// <summary>
        /// The Planet who is in his friend’s sign, is in conjunction with Jupiter,
        /// and is together with or is aspected by a friendly Planet is called Mudita
        /// 
        /// Mudita, sated, happy – Planet in a friend’s sign or aspected by a friend and conjoined with Jupiter, Gains
        ///
        /// If a planet is in a friend’s sign or joined with a friend or aspected by a friend,
        /// or that joined with Jupiter is called Mudita Avastha/Delighted State
        ///
        /// It is clear from explanation itself that a planet will feel delighted when it
        /// is in friendly sign or friendly planet conjuncts/aspects or it is joined by the
        /// biggest benefic planet Jupiter. We can understand planet’s delight in such cases. 
        /// 
        /// Planet in friendly sign - A planet in a friendly sign is productive,
        /// and the stronger that friend planet, the more productive it will be. 
        /// </summary>
        public static bool IsPlanetInMuditaAvasta(PlanetName planetName, Time time)
        {
            //Planet who is in his friend’s sign
            var isInFriendly = IsPlanetInFriendHouse(time, planetName);

            //is in conjunction with Jupiter
            var isConjunctJupiter = IsPlanetConjunctWithPlanet(planetName, Jupiter, time);

            //is together with or is aspected by a friendly (conjunct or aspect)
            var isConjunctWithFriendly = IsPlanetConjunctWithFriendPlanets(planetName, time);
            var isAspectedByFriendly = IsPlanetAspectedByFriendPlanets(planetName, time);
            var accosiatedWithFriendly = isConjunctWithFriendly || isAspectedByFriendly;

            //check if all conditions are met for Mudita
            var isMudita = isInFriendly || isConjunctJupiter || accosiatedWithFriendly;

            return isMudita;
        }

        /// <summary>
        /// If a planet is conjunct by Sun or it is aspected by Enemy Malefic Planets then
        /// it should always be known as Kshobhita Avastha/Agitated State
        /// 
        /// Kshobhita, guilty, repentant – Planet in conjunction with sun and aspected by malefics and an enemy. Penury
        /// </summary>
        public static bool IsPlanetInKshobhitaAvasta(PlanetName planetName, Time time)
        {
            //Planet in conjunction with sun 
            var conjunctWithSun = IsPlanetConjunctWithPlanet(planetName, Sun, time);

            //aspected by an enemy or malefic
            var isAspectedByEnemy = false == IsPlanetAspectedByEnemyPlanets(planetName, time);
            var isAspectedByMalefics = false == IsPlanetAspectedByMaleficPlanets(planetName, time);
            var accosiatedWithBadPlanets = isAspectedByEnemy || isAspectedByMalefics;

            //check if all conditions are met for Kshobhita
            var isKshobhita = conjunctWithSun && accosiatedWithBadPlanets;

            return isKshobhita;
        }

        #endregion

        #region ALL DATA

        /// <summary>
        /// Gets all possible calculations for a Planet at a given Time
        /// </summary>
        public static List<APIFunctionResult> AllPlanetData(PlanetName planetName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, planetName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a House at a given Time
        /// </summary>
		public static List<APIFunctionResult> AllHouseData(HouseName houseName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, houseName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a Planet and House at a given Time
        /// </summary>
		public static List<APIFunctionResult> AllPlanetHouseData(PlanetName planetName, HouseName houseName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, planetName, houseName, time);

            return raw;
        }

        /// <summary>
        /// All possible calculations for a Zodiac Sign at a given Time
        /// </summary>
		public static List<APIFunctionResult> AllZodiacSignData(ZodiacName zodiacName, Time time)
        {
            //exclude this method from getting included in "Find" and Execute below
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodInfo methodToExclude = method as MethodInfo;

            //do calculation
            var raw = AutoCalculator.FindAndExecuteFunctions(methodToExclude, zodiacName, time);

            return raw;
        }


        #endregion

        #region GENERAL

        public static double AyanamsaFinder(PlanetName inputPlanet, ConstellationName expectedConstellation, int expectedPada, Time time)
        {
            var isMatch = false;
            //test each untill found right one
            double ayanamsaYear = 0;
            while (ayanamsaYear < 2000)
            {
                var planetConste = PlanetConstellation(time, inputPlanet);
                var testQuarter = planetConste.GetQuarter();
                var testConstellationName = planetConste.GetConstellationName();
                isMatch = expectedConstellation == testConstellationName && expectedPada == testQuarter;

                if (isMatch)
                {
                    Console.WriteLine($"Ayanamsa : {ayanamsaYear} {planetConste} : FOUND");
                    return ayanamsaYear;
                }
                else
                {
                    //Console.WriteLine($"Ayanamsa : {ayanamsaYear} {planetConste} : FAIL");
                }
                //increase slightly
                ayanamsaYear += 0.005;
            }


            //if control reaches here than not found
            return 0;
        }

        /// <summary>
        /// Calculate Lord of Star (Constellation) given Constellation. Returns Star Lord Name
        /// </summary>
        public static PlanetName LordOfConstellation(ConstellationName constellation)
        {
            var x = constellation;

            switch (constellation)
            {
                case ConstellationName.Aswini:
                case ConstellationName.Makha:
                case ConstellationName.Moola:
                    return VedAstro.Library.PlanetName.Ketu;
                    break;
                case ConstellationName.Bharani:
                case ConstellationName.Pubba:
                case ConstellationName.Poorvashada:
                    return VedAstro.Library.PlanetName.Venus;
                    break;
                case ConstellationName.Krithika:
                case ConstellationName.Uttara:
                case ConstellationName.Uttarashada:
                    return VedAstro.Library.PlanetName.Sun;
                    break;
                case ConstellationName.Rohini:
                case ConstellationName.Hasta:
                case ConstellationName.Sravana:
                    return VedAstro.Library.PlanetName.Moon;
                    break;
                case ConstellationName.Mrigasira:
                case ConstellationName.Chitta:
                case ConstellationName.Dhanishta:
                    return VedAstro.Library.PlanetName.Mars;
                    break;
                case ConstellationName.Aridra:
                case ConstellationName.Swathi:
                case ConstellationName.Satabhisha:
                    return VedAstro.Library.PlanetName.Rahu;
                    break;
                case ConstellationName.Punarvasu:
                case ConstellationName.Vishhaka:
                case ConstellationName.Poorvabhadra:
                    return VedAstro.Library.PlanetName.Jupiter;
                    break;
                case ConstellationName.Pushyami:
                case ConstellationName.Anuradha: 
                case ConstellationName.Uttarabhadra:
                    return VedAstro.Library.PlanetName.Saturn;
                    break;
                case ConstellationName.Aslesha:
                case ConstellationName.Jyesta:
                case ConstellationName.Revathi:
                    return VedAstro.Library.PlanetName.Mercury;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(constellation), constellation, null);
            }

            throw new Exception("End of Line");

        }
    





    /// <summary>
    /// Calculate Fortuna Point for a given birth time & place. Returns Sign Number from Lagna
    /// </summary>
    public static int FortunePoint(ZodiacName ascZodiacSignName, PlanetName moon, PlanetName sun, Time time)
        {
            //Fortune Point is calculated as Asc Degrees + Moon Degrees - Sun Degrees

            //Find Lagna, Moon and Sun longitude degree
            var _asc_Degrees = Calculate.AllHouseLongitudes(time)[0].GetMiddleLongitude().TotalDegrees;
            var _moonDegrees = Calculate.PlanetNirayanaLongitude(time, moon).TotalDegrees;
            var _sunDegrees = Calculate.PlanetNirayanaLongitude(time, sun).TotalDegrees;

            //fortuna point is the point that is same distance from Ascendant
            //as Moon is from Sun

            //first let's compute how far the Moon is from Sun
            var _moon_sun_distance = _moonDegrees - _sunDegrees;

            if (_moon_sun_distance < 0) //moon is behind sun
            {
                _moon_sun_distance = _moon_sun_distance + 360;
            }

            //now lets compute the Fortuna point 
            var _fortunaPointDegrees = _asc_Degrees + _moon_sun_distance;

            if (_fortunaPointDegrees >= 360)
            {
                _fortunaPointDegrees = _fortunaPointDegrees - 360;
            }

            //convert Degrees to Angle
            var _angleAtFortunaPointDegrees = VedAstro.Library.Angle.FromDegrees(_fortunaPointDegrees);

            //find zodiacSignAtFP Longitude
            var _zodiacSignAtFP = Calculate.ZodiacSignAtLongitude(_angleAtFortunaPointDegrees).GetSignName();

            //find how many signs the FP is from Lagna
            var _signCount = Calculate.CountFromSignToSign(ascZodiacSignName, _zodiacSignAtFP);
            return _signCount;
        }

        /// <summary>
        /// Calculate Destiny Point for a given birth time & place. Returns Sign Number from Lagna
        /// </summary>
        public static int DestinyPoint(Time time, ZodiacName ascZodiacSignName, PlanetName rahu, PlanetName moon)
        {
            //destiny point is calculated as follows
            //Difference between Moon and Rahu longitude, Difference divided by 2, the result added to Rahu longitude

            var rahuDegrees = Calculate.PlanetNirayanaLongitude(time, rahu).TotalDegrees;
            var moonDegrees = Calculate.PlanetNirayanaLongitude(time, moon).TotalDegrees;

            var diff = moonDegrees - rahuDegrees;

            // if diff is negative, that means Moon is ahead of Rahu, then add 360 to the number. 
            if (diff < 0)
            {
                diff = diff + 360;
            }

            var mid_point = diff / 2;

            // Add mid_point to Rahu degrees
            var destinyPointDegrees = rahuDegrees + mid_point;

            if (destinyPointDegrees >= 360)
            {
                destinyPointDegrees = destinyPointDegrees - 360;
            }

            var angleAtDestinyPointDegrees = VedAstro.Library.Angle.FromDegrees(destinyPointDegrees);
            var zodiacSignAtDP = Calculate.ZodiacSignAtLongitude(angleAtDestinyPointDegrees).GetSignName();
            var signCount = Calculate.CountFromSignToSign(ascZodiacSignName, zodiacSignAtDP);

            return signCount;
        }

        /// <summary>
        /// Given a person will give yoni kuta animal with sex
        /// </summary>
        public static string YoniKutaAnimal(Person person)
        {
            var finalPrediction = "";

            var birthConst = Calculate.MoonConstellation(person.BirthTime);
            var animal = Calculate.YoniKutaAnimal(birthConst.GetConstellationName());

            finalPrediction += animal.ToString();

            return finalPrediction;
        }

        /// <summary>
        /// Given a constellation will give animal with sex, used for yoni kuta calculations
        /// and body appearance prediction
        /// </summary>
        public static ConstellationAnimal YoniKutaAnimal(ConstellationName sign)
        {
            switch (sign)
            {
                //Horse
                case ConstellationName.Aswini:
                    return new ConstellationAnimal("Male", AnimalName.Horse);
                case ConstellationName.Satabhisha:
                    return new ConstellationAnimal("Female", AnimalName.Horse);

                //Elephant
                case ConstellationName.Bharani:
                    return new ConstellationAnimal("Male", AnimalName.Elephant);
                case ConstellationName.Revathi:
                    return new ConstellationAnimal("Female", AnimalName.Elephant);

                //Sheep
                case ConstellationName.Pushyami:
                    return new ConstellationAnimal("Male", AnimalName.Sheep);
                case ConstellationName.Krithika:
                    return new ConstellationAnimal("Female", AnimalName.Sheep);

                //Serpent
                case ConstellationName.Rohini:
                    return new ConstellationAnimal("Male", AnimalName.Serpent);
                case ConstellationName.Mrigasira:
                    return new ConstellationAnimal("Female", AnimalName.Serpent);

                //Dog
                case ConstellationName.Moola:
                    return new ConstellationAnimal("Male", AnimalName.Dog);
                case ConstellationName.Aridra:
                    return new ConstellationAnimal("Female", AnimalName.Dog);

                //Cat
                case ConstellationName.Aslesha:
                    return new ConstellationAnimal("Male", AnimalName.Cat);
                case ConstellationName.Punarvasu:
                    return new ConstellationAnimal("Female", AnimalName.Cat);

                //Rat
                case ConstellationName.Makha:
                    return new ConstellationAnimal("Male", AnimalName.Rat);
                case ConstellationName.Pubba:
                    return new ConstellationAnimal("Female", AnimalName.Rat);

                //Cow
                case ConstellationName.Uttara:
                    return new ConstellationAnimal("Male", AnimalName.Cow);
                case ConstellationName.Uttarabhadra:
                    return new ConstellationAnimal("Female", AnimalName.Cow);

                //Buffalo
                case ConstellationName.Swathi:
                    return new ConstellationAnimal("Male", AnimalName.Buffalo);
                case ConstellationName.Hasta:
                    return new ConstellationAnimal("Female", AnimalName.Buffalo);

                //Tiger
                case ConstellationName.Vishhaka:
                    return new ConstellationAnimal("Male", AnimalName.Tiger);
                case ConstellationName.Chitta:
                    return new ConstellationAnimal("Female", AnimalName.Tiger);

                //Hare
                case ConstellationName.Jyesta:
                    return new ConstellationAnimal("Male", AnimalName.Hare);
                case ConstellationName.Anuradha:
                    return new ConstellationAnimal("Female", AnimalName.Hare);

                //Monkey
                case ConstellationName.Poorvashada:
                    return new ConstellationAnimal("Male", AnimalName.Monkey);
                case ConstellationName.Sravana:
                    return new ConstellationAnimal("Female", AnimalName.Monkey);

                //
                case ConstellationName.Poorvabhadra:
                    return new ConstellationAnimal("Male", AnimalName.Lion);
                case ConstellationName.Dhanishta:
                    return new ConstellationAnimal("Female", AnimalName.Lion);

                //Mongoose
                case ConstellationName.Uttarashada:
                    return new ConstellationAnimal("Male", AnimalName.Mongoose);

                default: throw new Exception("Yoni Kuta Animal Not Found!");
            }
        }

        /// <summary>
        /// Get sky chart as animated GIF. URL can be used like a image source link
        /// </summary>
        public static async Task<byte[]> SkyChartGIF(Time time) => await SkyChartManager.GenerateChartGif(time, 750, 230);

        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static async Task<string> SkyChart(Time time) => await SkyChartManager.GenerateChart(time, 750, 230);

        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static string SouthIndianChart(Time time)
        {
            var x = SouthChartManager.GenerateChart(time, 1000, 1000);

            return x;
        }

        /// <summary>
        /// Get sky chart at a given time. SVG image file. URL can be used like a image source link
        /// </summary>
        public static string NorthIndianChart(Time time)
        {
            var svgString = NorthChartManager.GenerateChart(time, 1000, 1000);

            return svgString;
        }

        /// <summary>
        /// Checks if a planet is in a Watery or aqua sign
        /// </summary>
        public static bool IsPlanetInWaterySign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //check if sign is watery
            var isWater = IsWaterSign(planetSign.GetSignName());

            return isWater;
        }

        /// <summary>
        /// Strength to judge the exact quantity of effect planet gives in a house
        /// 
        /// Use of Residential Strength --This will
        /// enable us to judge the exact quantity of effect that
        /// a pJanet in a Bhava gives, which may find expression
        /// during its Dasa.Its application and usefulness
        /// will be explained on a subsequent occasion.
        /// This effect will materialize during his Dasa or
        /// Bhukti. This is only a general statement standing
        /// to be modified or qualified in the light of other
        /// important factors such as, the strength or the weakness
        /// of the planets aspecting the Bhavas, the
        /// strength of the Bhava itself and the disposition
        /// of planets towards particular signs, the yogakarakas
        /// and such other factors.
        /// For instance, in the Standard Horoscope Jupiter
        /// gives 0.48 units of the total effects of the 6th Bhava.
        /// </summary>
        public static double ResidentialStrength(PlanetName planetName, Time time)
        {
            return 0;

            //todo from PG15 of Bhava and Graha Balas
            throw new NotImplementedException("");
        }

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        public static Angle TimeToLongitude(TimeSpan time)
        {
            //TODO function is a candidate for caching
            //degrees is equivalent to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }

        //NORMAL FUNCTIONS
        //FUNCTIONS THAT CALL OTHER FUNCTIONS IN THIS CLASS

        /// <summary>
        /// Gets the ephemris time that is consumed by Swiss Ephemeris
        /// Converts normal time to Ephemeris time shown as a number
        /// </summary>
        public static double TimeToEphemerisTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeToEphemerisTime), time, Ayanamsa), _timeToEphemerisTime);


            //UNDERLYING FUNCTION
            double _timeToEphemerisTime()
            {
                SwissEph ephemeris = new();

                //set GREGORIAN CALENDAR
                int gregflag = SwissEph.SE_GREG_CAL;

                //get LMT at UTC (+0:00)
                DateTimeOffset utcDate = LmtToUtc(time);

                //extract details of time
                int year = utcDate.Year;
                int month = utcDate.Month;
                int day = utcDate.Day;
                double hour = (utcDate.TimeOfDay).TotalHours;


                double jul_day_UT;
                double jul_day_ET;

                //do conversion to ephemris time
                jul_day_UT = ephemeris.swe_julday(year, month, day, hour, gregflag); //time to Julian Day
                jul_day_ET = jul_day_UT + ephemeris.swe_deltat(jul_day_UT); //Julian Day to ET

                return jul_day_ET;
            }

        }

        /// <summary>
        /// Gets Moon's position or day in lunar calendar
        /// </summary>
        public static LunarDay LunarDay(Time time)
        {
            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(time, Sun);
            Angle moonLong = PlanetNirayanaLongitude(time, Moon);

            double rawLunarDate;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
            {
                rawLunarDate = (moonLong - sunLong).TotalDegrees / 12.0;
            }
            else
            {
                rawLunarDate = ((moonLong + Angle.Degrees360) - sunLong).TotalDegrees / 12.0;
            }

            //round number to next whole number (ceiling)
            int roundedLunarDateNumber = (int)Math.Ceiling(rawLunarDate);

            //use lunar date number to initialize a lunar day instance
            var lunarDay = new LunarDay(roundedLunarDateNumber);

            //return lunar day to caller
            return lunarDay;


        }

        /// <summary>
        /// Gets name of Constellation behind the moon at a given time
        /// </summary>
        public static PlanetConstellation MoonConstellation(Time time) => PlanetConstellation(time, Moon);

        /// <summary>
        /// Gets the constellation behind a planet at a given time
        /// </summary>
        public static PlanetConstellation PlanetConstellation(Time time, PlanetName planet)
        {
            //get position of planet in longitude
            var planetLongitude = PlanetNirayanaLongitude(time, planet);

            //return the constellation behind the planet
            return ConstellationAtLongitude(planetLongitude);
        }

        /// <summary>
        /// Tarabala or birth ruling constellation strength, used for personal muhurtha
        /// </summary>
        public static Tarabala Tarabala(Time time, Person person)
        {
            int dayRulingConstellationNumber = MoonConstellation(time).GetConstellationNumber();

            int birthRulingConstellationNumber = MoonConstellation(person.BirthTime).GetConstellationNumber();

            int counter = 0;

            int cycle;


            //Need to count from birthRulingConstellationNumber to dayRulingConstellationNumber
            //todo upgrade to "ConstellationCounter", double check validity first
            //If birthRulingConstellationNumber is more than dayRulingConstellationNumber
            if (birthRulingConstellationNumber > dayRulingConstellationNumber)
            {
                //count birthRulingConstellationNumber to last constellation (27)
                int countToLastConstellation = (27 - birthRulingConstellationNumber) + 1; //plus 1 to count it self

                //add dayRulingConstellationNumber to countToLastConstellation(difference)
                counter = dayRulingConstellationNumber + countToLastConstellation;
            }
            else if (birthRulingConstellationNumber == dayRulingConstellationNumber)
            {
                counter = 1;
            }
            else if (birthRulingConstellationNumber < dayRulingConstellationNumber)
            {
                //If dayRulingConstellationNumber is more than or equal to birthRulingConstellationNumber
                counter = (dayRulingConstellationNumber - birthRulingConstellationNumber) + 1; //plus 1 to count it self
            }

            //change to double for division and then round up
            cycle = (int)Math.Ceiling(((double)counter / 9.0));


            //divide the number by 9 if divisible. Otherwise
            //keep it as it is.
            if (counter > 9)
            {
                //get modulos of counter
                counter = counter % 9;
                if (counter == 0)
                    counter = 9;
            }


            //initialize new tarabala from tarabala number & cycle
            var returnValue = new Tarabala(counter, cycle);

            return returnValue;
        }

        /// <summary>
        /// Chandrabala or lunar strength, used for personal muhurtha
        ///
        /// Reference:
        /// Chandrabala. - As we have already said above, the consideration of the
        /// Moon and his position are of much importance in Muhurtha. To be at its
        /// best, the Moon should not occupy in the election chart, a position that
        /// happens to represent the 6th, 8th or 12th from the person's Janma Rasi.
        /// </summary>
        public static int Chandrabala(Time time, Person person)
        {
            //TODO Needs to be updated with count sign from sign for better consistency
            //     also possible to leave it as is for better decoupling since this is working fine

            //initialize chandrabala number as 0
            int chandrabalaNumber = 0;

            //get zodiac name & convert to its number
            var dayMoonSignNumber = (int)MoonSignName(time);
            var birthMoonSignNumber = (int)MoonSignName(person.BirthTime);


            //Need to count from birthMoonSign to dayMoonSign

            //If birthMoonSign is more than dayMoonSign
            if (birthMoonSignNumber > dayMoonSignNumber)
            {
                //count birthMoonSign to last zodiac (12)
                int countToLastZodiac = (12 - birthMoonSignNumber) + 1; //plus 1 to count it self

                //add dayMoonSign to countToLastZodiac
                chandrabalaNumber = dayMoonSignNumber + countToLastZodiac;

            }
            else if (birthMoonSignNumber == dayMoonSignNumber)
            {
                chandrabalaNumber = 1;
            }
            else if (birthMoonSignNumber < dayMoonSignNumber)
            {
                //If dayMoonSign is more than or equal to birthMoonSign
                chandrabalaNumber = (dayMoonSignNumber - birthMoonSignNumber) + 1; //plus 1 to count it self
            }

            return chandrabalaNumber;

        }

        /// <summary>
        /// Zodiac sign behind the Moon at given time
        /// </summary>
        public static ZodiacName MoonSignName(Time time)
        {
            //get zodiac sign behind the moon
            var moonSign = PlanetSignName(Moon, time);

            //return name of zodiac sign
            return moonSign.GetSignName();
        }

        /// <summary>
        /// Zodiac sign at the Lagna/Ascendant at given time
        /// </summary>
        public static ZodiacName LagnaSignName(Time time)
        {
            //get zodiac sign behind the Lagna/Ascendant
            var lagnaSign = HouseSignName(HouseName.House1, time);

            //return name of zodiac sign
            return lagnaSign;
        }

        /// <summary>
        /// Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')
        /// </summary>
        public static NithyaYoga NithyaYoga(Time time)
        {
            //Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')

            //get position of sun & moon in longitude
            Angle sunLongitude = PlanetNirayanaLongitude(time, Sun);
            Angle moonLongitude = PlanetNirayanaLongitude(time, Moon);

            //get joint motion in longitude of the Sun and the Moon
            var jointLongitudeInMinutes = sunLongitude.TotalMinutes + moonLongitude.TotalMinutes;

            //get unrounded nithya yoga number by
            //dividing joint longitude by 800'
            var rawNithyaYogaNumber = jointLongitudeInMinutes / 800;

            //round to ceiling to get whole number
            var nithyaYogaNumber = Math.Ceiling(rawNithyaYogaNumber);

            //convert nithya yoga number to type
            var nithyaYoga = (NithyaYoga)nithyaYogaNumber;

            //return to caller

            return nithyaYoga;
        }

        /// <summary>
        /// Used for auspicious activities, part Panchang like Tithi, Nakshatra, Yoga, etc.
        /// </summary>
        public static Karana Karana(Time time)
        {
            //declare karana as empty first
            Karana? karanaToReturn = null;

            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(time, Sun);
            Angle moonLong = PlanetNirayanaLongitude(time, Moon);

            //get raw lunar date
            double rawlunarDate;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
            {
                rawlunarDate = (moonLong - sunLong).TotalDegrees / 12.0;
            }
            else
            {
                rawlunarDate = ((moonLong + new Angle(degrees: 360)) - sunLong).TotalDegrees / 12.0;
            }

            //round number to next whole number (ceiling)
            int roundedLunarDateNumber = (int)Math.Ceiling(rawlunarDate);

            //get lunar day already traversed
            var lunarDayAlreadyTraversed = rawlunarDate - Math.Floor(rawlunarDate);

            switch (roundedLunarDateNumber)
            {
                //based on lunar date get karana
                case 1:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Kimstughna : Library.Karana.Bava;
                    break;
                case 23:
                case 16:
                case 9:
                case 2:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Balava : Library.Karana.Kaulava;
                    break;
                case 24:
                case 17:
                case 10:
                case 3:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Taitula : Library.Karana.Garija;
                    break;
                case 25:
                case 18:
                case 11:
                case 4:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Vanija : Library.Karana.Visti;
                    break;
                case 26:
                case 19:
                case 12:
                case 5:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Bava : Library.Karana.Balava;
                    break;
                case 27:
                case 20:
                case 13:
                case 6:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Kaulava : Library.Karana.Taitula;
                    break;
                case 28:
                case 21:
                case 14:
                case 7:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Garija : Library.Karana.Vanija;
                    break;
                case 22:
                case 15:
                case 8:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Visti : Library.Karana.Bava;
                    break;
                case 29:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Visti : Library.Karana.Sakuna;
                    break;
                case 30:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Library.Karana.Chatushpada : Library.Karana.Naga;
                    break;

            }

            //if karana not found throw error
            if (karanaToReturn == null)
            {
                throw new Exception("Karana could not be found!");
            }

            return (Karana)karanaToReturn;
        }

        /// <summary>
        /// Zodiac sign behind the Sun at a time
        /// </summary>
        public static ZodiacSign SunSign(Time time)
        {
            //get zodiac sign behind the sun
            var sunSign = PlanetSignName(Sun, time);

            //return zodiac sign behind sun
            return sunSign;
        }

        ///<summary>
        ///Find time when Sun was in 0.001 degrees
        ///in current sign (just entered sign)
        ///</summary>
        public static Time TimeSunEnteredCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeSunEnteredCurrentSign), time, Ayanamsa), _timeSunEnteredCurrentSign);


            //UNDERLYING FUNCTION
            Time _timeSunEnteredCurrentSign()
            {

                //set the maximum accuracy used to calculate time sun will enter the sign
                //once this limit is hit, the previously calculated time will be returned
                double AccuracyLimit = TimePreset.Minute3;

                //set time decrement accuracy at 96 hours (4 days) at first
                double timeDecrementAccuracy = 96;

                //set input time as possible entered time at first
                var possibleEnteredTime = time;
                var previousPossibleEnteredTime = time;


                //get current sun sign
                var currentSunSign = SunSign(time);

                //if entered time not yet found
                while (true)
                {
                    //get the sign at possible entered time
                    var possibleSunSign = SunSign(possibleEnteredTime);

                    //if possible sign name is same as current sign name, then check if sun is about to enter sign
                    var signNameIsSame = possibleSunSign.GetSignName() == currentSunSign.GetSignName();
                    if (signNameIsSame)
                    {
                        //if sun sign is less than 0.001 degrees, entered time found
                        if (possibleSunSign.GetDegreesInSign().TotalDegrees < 0.001) { break; }

                        //else sun not yet torward the start of the sign, so decrement time
                        else
                        {
                            //back up possible entered time before changing
                            previousPossibleEnteredTime = possibleEnteredTime;

                            //decrement entered time, to check next possible time
                            possibleEnteredTime = possibleEnteredTime.SubtractHours(timeDecrementAccuracy);
                        }
                    }
                    //else sun sign is not same, went to far
                    else
                    {
                        //return possible entered time to previous time
                        possibleEnteredTime = previousPossibleEnteredTime;

                        //if accuracy limit is hit, then use previous time as answer, stop looking
                        if (timeDecrementAccuracy <= AccuracyLimit) { break; }

                        //decrease time decrement accuracy by half
                        timeDecrementAccuracy = timeDecrementAccuracy / 2;

                    }
                }

                //return possible entered time
                return possibleEnteredTime;

            }
        }

        ///<summary>
        ///Find time when Sun was in 29 degrees
        ///in current sign (just about to leave sign)
        ///
        /// Note:
        /// -2 possible ways leaving time is calculated
        ///     1. degrees Sun is in sign is more than 29.999 degrees (very very close to leaving sign)
        ///     2. accuracy limit is hit
        ///</summary>
        public static Time TimeSunLeavesCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(TimeSunLeavesCurrentSign), time, Ayanamsa), _getTimeSunLeavesCurrentSign);


            //UNDERLYING FUNCTION
            Time _getTimeSunLeavesCurrentSign()
            {

                //set the maximum accuracy used to calculate time sun will leave the sign
                //once this limit is hit, the previously calculated time will be returned
                double AccuracyLimit = TimePreset.Minute3;

                //set time increment accuracy at 96 hours (4 days) at first
                double timeIncrementAccuracy = 96;

                //set input time as possible leaving time at first
                var possibleLeavingTime = time;
                var previousPossibleLeavingTime = time;

                //get current sun sign
                var currentSunSign = SunSign(time);

                //find leaving time
                while (true)
                {
                    //get the sign at possible leaving time
                    var possibleSunSign = SunSign(possibleLeavingTime);

                    //if possible sign name is same as current sign name, then check if sun is about to leave sign
                    var signNameIsSame = possibleSunSign.GetSignName() == currentSunSign.GetSignName();
                    if (signNameIsSame)
                    {
                        //if sun sign is more than 29.9 degrees, leaving time found
                        if (possibleSunSign.GetDegreesInSign().TotalDegrees > 29.999) { break; }

                        //else sun not yet torward the end of the sign, so increment time
                        else
                        {
                            //back up possible leaving time before changing
                            previousPossibleLeavingTime = possibleLeavingTime;

                            //increment leaving time, to check next possible time
                            possibleLeavingTime = possibleLeavingTime.AddHours(timeIncrementAccuracy);
                        }
                    }
                    //else sun sign is not same, went to far, go back a little in time
                    else
                    {
                        //restore possible leaving time to previous time
                        possibleLeavingTime = previousPossibleLeavingTime;

                        //if accuracy limit is hit, then use previous time as answer, stop looking
                        if (timeIncrementAccuracy <= AccuracyLimit) { break; }

                        //decrease time increment accuracy by half
                        timeIncrementAccuracy = timeIncrementAccuracy / 2;

                    }
                }

                //return possible leaving time
                return possibleLeavingTime;

            }

        }

        /// <summary>
        /// Gets longitudes for houses under Krishnamurti (KP) astrology system
        /// Note: Ayanamsa hard set to Krishnamurti
        /// </summary>
        public static List<House> AllHouseLongitudesKP(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHouseLongitudesKP), time, Ayanamsa), _allHouseLongitudesKP);


            //UNDERLYING FUNCTION

            List<House> _allHouseLongitudesKP()
            {
                //get house positions modified for KP system in raw 
                var swissEphCusps = _houseLongitudesKP();

                //4.0 Initialize houses into list
                var houseList = new List<House>();

                foreach (var house in Library.House.AllHouses)
                {
                    var houseNumber = (int)house;
                    var houseBegin = swissEphCusps[houseNumber];
                    var nextHseNumber = houseNumber + 1;
                    nextHseNumber = nextHseNumber >= 12 ? 1 : nextHseNumber; //goto house 1 once hit house 12
                    var houseEnd = swissEphCusps[nextHseNumber];//start of next house is end of this
                    var houseMid = houseBegin + ((houseEnd - houseBegin) / 2);
                    houseList.Add(new House(house, Angle.FromDegrees(houseBegin), Angle.FromDegrees(houseMid), Angle.FromDegrees(houseEnd)));

                }

                return houseList;

            }

            double[] _houseLongitudesKP()
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                //Convert DOB to Julian Day
                var jul_day_UT = TimeToJulianDay(time);

                SwissEph swissEph = new SwissEph();

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //set ayanamsa
                swissEph.swe_set_sid_mode((int)SimpleAyanamsa.KrishnamurtiKP, 0, 0);

                var iflag = SwissEph.SEFLG_SIDEREAL;

                //NOTE:
                //if you use P which is Placidus there for Krishamurti
                swissEph.swe_houses_ex(jul_day_UT, iflag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);

                //we only return cusps, cause that is what is used for now
                return cusps;
            }


        }

        /// <summary>
        /// Gets all houses with their constelation for KP Krishnamurti system
        /// </summary>
        public static Dictionary<House, PlanetConstellation> AllHouseConstellationKP(Time time)
        {
            //get all house positions
            var housePositions = AllHouseLongitudesKP(time);

            //fill the planet constellations
            var returnList = new Dictionary<House, PlanetConstellation>();
            foreach (var house in housePositions)
            {
                var constellation = Calculate.ConstellationAtLongitude(house.GetBeginLongitude());
                returnList.Add(house, constellation);
            }

            return returnList;
        }

        /// <summary>
        /// special function localized to allow caching
        /// note: there is another version that does caching
        /// </summary>
        public static double TimeToJulianDay(Time time)
        {
            //get lmt time
            var lmtDateTime = time.GetLmtDateTimeOffset();

            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDateTime = lmtDateTime.ToUniversalTime();

            SwissEph swissEph = new SwissEph();

            double jul_day_UT;
            jul_day_UT = swissEph.swe_julday(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day,
                utcDateTime.TimeOfDay.TotalHours, SwissEph.SE_GREG_CAL);
            return jul_day_UT;

        }

        /// <summary>
        /// Calculates & creates all houses as list
        /// </summary>
        public static List<House> AllHouseLongitudes(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHouseLongitudes), time, Ayanamsa), _getHouses);


            //UNDERLYING FUNCTION
            List<House> _getHouses()
            {
                //declare house longitudes
                Angle house1BeginLongitude, house1MiddleLongitude, house1EndLongitude;
                Angle house2BeginLongitude, house2MiddleLongitude, house2EndLongitude;
                Angle house3BeginLongitude, house3MiddleLongitude, house3EndLongitude;
                Angle house4BeginLongitude, house4MiddleLongitude, house4EndLongitude;
                Angle house5BeginLongitude, house5MiddleLongitude, house5EndLongitude;
                Angle house6BeginLongitude, house6MiddleLongitude, house6EndLongitude;
                Angle house7BeginLongitude, house7MiddleLongitude, house7EndLongitude;
                Angle house8BeginLongitude, house8MiddleLongitude, house8EndLongitude;
                Angle house9BeginLongitude, house9MiddleLongitude, house9EndLongitude;
                Angle house10BeginLongitude, house10MiddleLongitude, house10EndLongitude;
                Angle house11BeginLongitude, house11MiddleLongitude, house11EndLongitude;
                Angle house12BeginLongitude, house12MiddleLongitude, house12EndLongitude;


                //1.Get middle longitudes of angular houses

                //1.1 get House 1 & 10

                //Get western 1 & 10 house longitudes
                var cusps = GetHouse1And10Longitudes(time);

                //Get Sayana Long. of cusp of ascend.
                var sayanaCuspOfHouse1 = Angle.FromDegrees(cusps[1]);

                //Get Sayana Long. of cusp of tenth-house
                var sayanaCuspOfHouse10 = Angle.FromDegrees(cusps[10]);

                //Deduct from these two, the Ayanamsa to get the Nirayana longitudes
                // of Udaya Lagna (Ascendant) and the Madhya Lagna (Upper Meridian)
                var ayanamsa = AyanamsaDegree(time);

                var udayaLagna = sayanaCuspOfHouse1 - ayanamsa;
                var madhyaLagna = sayanaCuspOfHouse10 - ayanamsa;

                //Add 180° to each of these two, to get the Nirayana Asta Lagna (Western Horizon)
                //and the Pathala Lagna (Lower Meridian)
                var astaLagna = udayaLagna + Angle.Degrees180;
                var pathalaLagna = madhyaLagna + Angle.Degrees180;

                //if longitude is more than 360°, expunge 360°
                astaLagna = astaLagna.Expunge360();
                pathalaLagna = pathalaLagna.Expunge360();

                //assign angular house middle longitudes, houses 1,4,7,10
                house1MiddleLongitude = udayaLagna + Angle.FromDegrees(15);
                house4MiddleLongitude = pathalaLagna + Angle.FromDegrees(15);
                house7MiddleLongitude = astaLagna + Angle.FromDegrees(15);
                house10MiddleLongitude = madhyaLagna + Angle.FromDegrees(15);

                //2.0 Get middle longitudes of non-angular houses
                //2.1 Calculate arcs
                Angle arcA, arcB, arcC, arcD;

                //calculate Arc A
                if (house4MiddleLongitude < house1MiddleLongitude)
                {
                    arcA = ((house4MiddleLongitude + Angle.Degrees360) - house1MiddleLongitude);
                }
                else
                {
                    arcA = (house4MiddleLongitude - house1MiddleLongitude);
                }

                //calculate Arc B
                if (house7MiddleLongitude < house4MiddleLongitude)
                {
                    arcB = ((house7MiddleLongitude + Angle.Degrees360) - house4MiddleLongitude);
                }
                else
                {
                    arcB = (house7MiddleLongitude - house4MiddleLongitude);
                }

                //calculate Arc C
                if (house10MiddleLongitude < house7MiddleLongitude)
                {
                    arcC = ((house10MiddleLongitude + Angle.Degrees360) - house7MiddleLongitude);
                }
                else
                {
                    arcC = (house10MiddleLongitude - house7MiddleLongitude);
                }

                //calculate Arc D
                if (house1MiddleLongitude < house10MiddleLongitude)
                {
                    arcD = ((house1MiddleLongitude + Angle.Degrees360) - house10MiddleLongitude);
                }
                else
                {
                    arcD = (house1MiddleLongitude - house10MiddleLongitude);
                }

                //2.2 Trisect each arc
                //Cacl House 2 & 3
                house2MiddleLongitude = house1MiddleLongitude + arcA.Divide(3);
                house2MiddleLongitude = house2MiddleLongitude.Expunge360();
                house3MiddleLongitude = house2MiddleLongitude + arcA.Divide(3);
                house3MiddleLongitude = house3MiddleLongitude.Expunge360();

                //Cacl House 5 & 6
                house5MiddleLongitude = house4MiddleLongitude + arcB.Divide(3);
                house5MiddleLongitude = house5MiddleLongitude.Expunge360();
                house6MiddleLongitude = house5MiddleLongitude + arcB.Divide(3);
                house6MiddleLongitude = house6MiddleLongitude.Expunge360();

                //Cacl House 8 & 9
                house8MiddleLongitude = house7MiddleLongitude + arcC.Divide(3);
                house8MiddleLongitude = house8MiddleLongitude.Expunge360();
                house9MiddleLongitude = house8MiddleLongitude + arcC.Divide(3);
                house9MiddleLongitude = house9MiddleLongitude.Expunge360();

                //Cacl House 11 & 12
                house11MiddleLongitude = house10MiddleLongitude + arcD.Divide(3);
                house11MiddleLongitude = house11MiddleLongitude.Expunge360();
                house12MiddleLongitude = house11MiddleLongitude + arcD.Divide(3);
                house12MiddleLongitude = house12MiddleLongitude.Expunge360();

                //3.0 Calculate house begin & end longitudes

                house1EndLongitude = house2BeginLongitude = HouseJunctionPoint(house1MiddleLongitude, house2MiddleLongitude);
                house2EndLongitude = house3BeginLongitude = HouseJunctionPoint(house2MiddleLongitude, house3MiddleLongitude);
                house3EndLongitude = house4BeginLongitude = HouseJunctionPoint(house3MiddleLongitude, house4MiddleLongitude);
                house4EndLongitude = house5BeginLongitude = HouseJunctionPoint(house4MiddleLongitude, house5MiddleLongitude);
                house5EndLongitude = house6BeginLongitude = HouseJunctionPoint(house5MiddleLongitude, house6MiddleLongitude);
                house6EndLongitude = house7BeginLongitude = HouseJunctionPoint(house6MiddleLongitude, house7MiddleLongitude);
                house7EndLongitude = house8BeginLongitude = HouseJunctionPoint(house7MiddleLongitude, house8MiddleLongitude);
                house8EndLongitude = house9BeginLongitude = HouseJunctionPoint(house8MiddleLongitude, house9MiddleLongitude);
                house9EndLongitude = house10BeginLongitude = HouseJunctionPoint(house9MiddleLongitude, house10MiddleLongitude);
                house10EndLongitude = house11BeginLongitude = HouseJunctionPoint(house10MiddleLongitude, house11MiddleLongitude);
                house11EndLongitude = house12BeginLongitude = HouseJunctionPoint(house11MiddleLongitude, house12MiddleLongitude);
                house12EndLongitude = house1BeginLongitude = HouseJunctionPoint(house12MiddleLongitude, house1MiddleLongitude);

                //4.0 Initialize houses into list
                var houseList = new List<House>();

                houseList.Add(new House(HouseName.House1, house1BeginLongitude, house1MiddleLongitude, house1EndLongitude));
                houseList.Add(new House(HouseName.House2, house2BeginLongitude, house2MiddleLongitude, house2EndLongitude));
                houseList.Add(new House(HouseName.House3, house3BeginLongitude, house3MiddleLongitude, house3EndLongitude));
                houseList.Add(new House(HouseName.House4, house4BeginLongitude, house4MiddleLongitude, house4EndLongitude));
                houseList.Add(new House(HouseName.House5, house5BeginLongitude, house5MiddleLongitude, house5EndLongitude));
                houseList.Add(new House(HouseName.House6, house6BeginLongitude, house6MiddleLongitude, house6EndLongitude));
                houseList.Add(new House(HouseName.House7, house7BeginLongitude, house7MiddleLongitude, house7EndLongitude));
                houseList.Add(new House(HouseName.House8, house8BeginLongitude, house8MiddleLongitude, house8EndLongitude));
                houseList.Add(new House(HouseName.House9, house9BeginLongitude, house9MiddleLongitude, house9EndLongitude));
                houseList.Add(new House(HouseName.House10, house10BeginLongitude, house10MiddleLongitude, house10EndLongitude));
                houseList.Add(new House(HouseName.House11, house11BeginLongitude, house11MiddleLongitude, house11EndLongitude));
                houseList.Add(new House(HouseName.House12, house12BeginLongitude, house12MiddleLongitude, house12EndLongitude));


                return houseList;

            }



        }

        /// <summary>
        /// Convert LMT to Julian Days used in Swiss Ephemeris
        /// </summary>
        public static double ConvertLmtToJulian(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConvertLmtToJulian), time, Ayanamsa), _convertLmtToJulian);


            //UNDERLYING FUNCTION
            double _convertLmtToJulian()
            {
                //get lmt time
                DateTimeOffset lmtDateTime = time.GetLmtDateTimeOffset();

                //split lmt time to pieces
                int year = lmtDateTime.Year;
                int month = lmtDateTime.Month;
                int day = lmtDateTime.Day;
                double hour = (lmtDateTime.TimeOfDay).TotalHours;

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //declare output variables
                double localMeanTimeInJulian_UT;

                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //get lmt in julian day in Universal Time (UT)
                localMeanTimeInJulian_UT = ephemeris.swe_julday(year, month, day, hour, gregflag);//time to Julian Day

                return localMeanTimeInJulian_UT;

            }

        }

        /// <summary>
        /// Gets all the planets that are in conjunction with the inputed planet
        ///
        /// Note:
        /// 1.The planet inputed is not included in return list
        /// 
        /// 2. Theory behind conjunction
        /// Conjunction :-Two heavenly bodies in the same longitude.
        ///
        /// "The effect of an aspect is felt even if the planets are not
        /// exactly in the mutual distances mentioned above. Therefore
        /// a so-called orb of aspect, and this varies in each aspect is allowed."
        /// The orbs of aspects are :
        /// Conjunction = 8° degrees
        ///
        /// -Planets can be in same sign but not conjunct :
        /// "There are also other variations
        /// of aspects brought about by two planets remaining in the
        /// same sign and not in conjunction but another planet occupying
        /// a trine in respect of the two."
        /// </summary>
        public static List<PlanetName> PlanetsInConjuction(Time time, PlanetName inputedPlanetName)
        {
            //set 8° degrees as max space around planet where conjunction occurs
            var conjunctionOrbMax = new Angle(8, 0, 0);

            //get longitude of inputed planet
            var inputedPlanet = PlanetNirayanaLongitude(time, inputedPlanetName);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = AllPlanetLongitude(time);

            //a place to store conjunct planets 
            var conjunctPlanets = new List<PlanetName>();

            //loop through each planet
            foreach (var planet in allPlanetLongitudeList)
            {
                //skip the inputed planet
                if (planet.GetPlanetName() == inputedPlanetName) { continue; }

                //get the space between the planet in longitude
                var spaceBetween = DistanceBetweenPlanets(inputedPlanet, planet.GetPlanetLongitude());

                //if space between is from 0° to 8°, then it is conjunct
                if (spaceBetween >= Angle.Zero && spaceBetween <= conjunctionOrbMax)
                {
                    conjunctPlanets.Add(planet.GetPlanetName());
                }

            }

            //return list
            return conjunctPlanets;
        }

        /// <summary>
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for.
        /// - Calculation in Nirayana longitudes
        /// - Calculates longitudes for you
        /// </summary>
        public static Angle DistanceBetweenPlanets(PlanetName planet1, PlanetName planet2, Time time)
        {
            var planet1Longitude = PlanetNirayanaLongitude(time, planet1);
            var planet2Longitude = PlanetNirayanaLongitude(time, planet2);

            var distanceBetweenPlanets = planetDistance(planet1Longitude.TotalDegrees, planet2Longitude.TotalDegrees);

            return Angle.FromDegrees(distanceBetweenPlanets);



            //---------------FUNCTION---------------


            double planetDistance(double len1, double len2)
            {
                double d = red_deg(Math.Abs(len2 - len1));

                if (d > 180) return (360 - d);

                return d;
            }

            //Reduces a given double value modulo 360.
            //The return value is between 0 and 360.
            double red_deg(double input) => a_red(input, 360);

            //Reduces a given double value x modulo the double a(should be positive).
            //The return value is between 0 and a.
            double a_red(double x, double a) => (x - Math.Floor(x / a) * a);

        }

        /// <summary>
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for
        /// - Expects you to calculate longitude
        /// </summary>
        public static Angle DistanceBetweenPlanets(Angle planet1, Angle planet2)
        {

            var distanceBetweenPlanets = planetDistance(planet1.TotalDegrees, planet2.TotalDegrees);

            return Angle.FromDegrees(distanceBetweenPlanets);



            //---------------FUNCTION---------------


            double planetDistance(double len1, double len2)
            {
                double d = red_deg(Math.Abs(len2 - len1));

                if (d > 180) return (360 - d);

                return d;
            }

            //Reduces a given double value modulo 360.
            //The return value is between 0 and 360.
            double red_deg(double input) => a_red(input, 360);

            //Reduces a given double value x modulo the double a(should be positive).
            //The return value is between 0 and a.
            double a_red(double x, double a) => (x - Math.Floor(x / a) * a);

        }

        /// <summary>
        /// Gets list of all planets that's in a house/bhava at a given time
        /// </summary>
        public static List<PlanetName> PlanetsInHouse(HouseName houseNumber, Time time)
        {

            //declare empty list of planets
            var listOfPlanetInHouse = new List<PlanetName>();

            //get all houses
            var houseList = AllHouseLongitudes(time);

            //find house that matches input house number
            var house = houseList.Find(h => h.GetHouseName() == houseNumber);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = AllPlanetLongitude(time);

            //loop through each planet in house
            foreach (var planet in allPlanetLongitudeList)
            {
                //check if planet is in house
                bool planetIsInHouse = house.IsLongitudeInHouseRange(planet.GetPlanetLongitude());

                if (planetIsInHouse)
                {
                    //add to list if planet is in house
                    listOfPlanetInHouse.Add(planet.GetPlanetName());
                }
            }

            //return list
            return listOfPlanetInHouse;
        }

        /// <summary>
        /// Gets list of all planets that's in a sign at a given time
        /// </summary>
        public static List<PlanetName> PlanetsInSign(ZodiacName signName, Time time)
        {
            var returnList = new List<PlanetName>();

            //check each planet if in sign
            foreach (var planet in All9Planets)
            {
                var planetSign = PlanetSignName(planet, time);

                //if correct sign add to return list
                if (planetSign.GetSignName() == signName) { returnList.Add(planet); }
            }

            return returnList;
        }

        /// <summary>
        /// Gets list of all planets and the zodiac signs they are in
        /// </summary>
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetSigns(Time time)
        {
            var returnList = new Dictionary<PlanetName, ZodiacSign>();

            //check each planet if in sign
            foreach (var planet in All9Planets)
            {
                var planetSign = PlanetSignName(planet, time);

                returnList[planet] = planetSign;
            }

            return returnList;
        }

        /// <summary>
        /// Gets list of all planets and the constellation they are in
        /// </summary>
        public static Dictionary<PlanetName, PlanetConstellation> AllPlanetConstellation(Time time)
        {
            var returnList = new Dictionary<PlanetName, PlanetConstellation>();

            //check each planet if in sign
            foreach (var planet in All9Planets)
            {
                var planetSign = PlanetConstellation(time, planet);

                returnList[planet] = planetSign;
            }

            return returnList;
        }

        /// <summary>
        /// Gets the Nirayana longitude of all 9 planets
        /// </summary>
        public static List<PlanetLongitude> AllPlanetLongitude(Time time)
        {
            //get longitudes of all planets
            var sunLongitude = PlanetNirayanaLongitude(time, Sun);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetNirayanaLongitude(time, Moon);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetNirayanaLongitude(time, Mars);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetNirayanaLongitude(time, Mercury);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetNirayanaLongitude(time, Jupiter);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetNirayanaLongitude(time, Venus);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetNirayanaLongitude(time, Saturn);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetNirayanaLongitude(time, Rahu);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetNirayanaLongitude(time, Ketu);
            var ketu = new PlanetLongitude(Ketu, ketuLongitude);


            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        /// <summary>
        /// Gets longitude positions of all planets Sayana / Fixed zodiac 
        /// </summary>
        public static List<PlanetLongitude> AllPlanetFixedLongitude(Time time)
        {
            //get longitudes of all planets
            var sunLongitude = PlanetSayanaLongitude(time, Sun);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetSayanaLongitude(time, Moon);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetSayanaLongitude(time, Mars);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetSayanaLongitude(time, Mercury);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetSayanaLongitude(time, Jupiter);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetSayanaLongitude(time, Venus);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetSayanaLongitude(time, Saturn);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetSayanaLongitude(time, Rahu);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetSayanaLongitude(time, Ketu);
            var ketu = new PlanetLongitude(Ketu, ketuLongitude);


            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        /// <summary>
        /// Gets the House number a given planet is in at a time
        /// </summary>
        public static HouseName HousePlanetIsIn(Time time, PlanetName planetName)
        {

            //get the planets longitude
            var planetLongitude = PlanetNirayanaLongitude(time, planetName);

            //get all houses
            var houseList = AllHouseLongitudes(time);

            //loop through all houses
            foreach (var house in houseList)
            {
                //check if planet is in house's range
                var planetIsInHouse = house.IsLongitudeInHouseRange(planetLongitude);

                //if planet is in house
                if (planetIsInHouse)
                {
                    //return house's number
                    return house.GetHouseName();
                }
            }

            //if planet not found in any house, raise error
            throw new Exception("Planet not in any house, error!");

        }

        /// <summary>
        /// Gets the House number a given planet is in at a time
        /// </summary>
        public static HouseName HousePlanetIsInKP(Time time, PlanetName planetName)
        {

            //get the planets longitude
            var planetLongitude = PlanetNirayanaLongitude(time, planetName);

            //get all houses
            var houseList = AllHouseLongitudesKP(time);

            //loop through all houses
            foreach (var house in houseList)
            {
                //check if planet is in house's range
                var planetIsInHouse = house.IsLongitudeInHouseRange(planetLongitude);

                //if planet is in house
                if (planetIsInHouse)
                {
                    //return house's number
                    return house.GetHouseName();
                }
            }

            //if planet not found in any house, raise error
            throw new Exception("Planet not in any house, error!");

        }

        /// <summary>
        /// List of all planets and the houses they are located in at a given time
        /// </summary>
        public static Dictionary<PlanetName, HouseName> AllPlanetHousePositions(Time time)
        {
            var returnList = new Dictionary<PlanetName, HouseName>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = HousePlanetIsIn(time, planet);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// List of all planets and the zodiac signs they are located in at a given time
        /// </summary>
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetZodiacSigns(Time time)
        {
            var returnList = new Dictionary<PlanetName, ZodiacSign>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = Calculate.PlanetSignName(planet, time);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }


        /// <summary>
        /// List of all planets and the zodiac signs they are located in at a given time
        /// using KP Krishnamurti system, note KP ayanamsa is hard set
        /// </summary>
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetZodiacSignsKP(Time time)
        {

            //hard set KP ayanamsa to match commercial software default output
            Calculate.Ayanamsa = (int)SimpleAyanamsa.KrishnamurtiKP;

            var returnList = new Dictionary<PlanetName, ZodiacSign>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = Calculate.PlanetSignName(planet, time);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// List of all planets and the houses they are located in at a given time
        /// using KP Krishnamurti system, note KP ayanamsa is hard set
        /// </summary>
        public static Dictionary<PlanetName, HouseName> AllPlanetHousePositionsKP(Time time)
        {
            //hard set KP ayanamsa to match commercial software default output
            Calculate.Ayanamsa = (int)SimpleAyanamsa.KrishnamurtiKP;

            var returnList = new Dictionary<PlanetName, HouseName>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = HousePlanetIsInKP(time, planet);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// Gets planet lord of given house at given time
        /// The lord of a bhava is
        /// the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// </summary>
        public static PlanetName LordOfHouse(HouseName houseNumber, Time time)
        {
            //get sign name based on house number //TODO Change to use house name instead of casting to int
            var houseSignName = HouseSignName(houseNumber, time);

            //get the lord of the house sign
            var lordOfHouseSign = LordOfZodiacSign(houseSignName);

            return lordOfHouseSign;
        }

        /// <summary>
        /// The lord of a bhava is
        /// the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// List overload to GetLordOfHouse (above method)
        /// </summary>
        public static List<PlanetName> LordOfHouseList(List<HouseName> houseList, Time time)
        {
            var returnList = new List<PlanetName>();
            foreach (var house in houseList)
            {
                var tempLord = LordOfHouse(house, time);
                returnList.Add(tempLord);
            }

            return returnList;
        }

        /// <summary>
        /// Checks if the inputed sign was the sign of the house during the inputed time
        /// </summary>
        public static bool IsHouseSignName(HouseName house, ZodiacName sign, Time time) => HouseSignName(house, time) == sign;

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        public static ZodiacName HouseSignName(HouseName houseNumber, Time time)
        {
            //if empty return aries, can't give empty because no Empty for ZodiacName
            if (houseNumber == HouseName.Empty) { return ZodiacName.Aries; }

            //get all houses
            var allHouses = AllHouseLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseSignName = ZodiacSignAtLongitude(middleLongitude).GetSignName();

            //return the name of house sign
            return houseSignName;
        }

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        public static PlanetConstellation HouseConstellation(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            //Note :
            //When the middle longitude has just entered a new sign,
            //rounding the longitude shows better accuracy.
            //Example, with middle longitude 90.4694, becomes Cancer (0°28'9"),
            //but predictive results points to Gemini (30°0'0"), so rounding is implemented
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseConstellation = ConstellationAtLongitude(middleLongitude);

            //return the name of house sign
            return houseConstellation;
        }

        /// <summary>
        /// Gets Navamsa sign given a longitude
        /// </summary>
        public static ZodiacName NavamsaSignNameFromLongitude(Angle longitude)
        {
            //1.0 Get ordinary zodiac sign name
            //get ordinary zodiac sign
            var ordinarySign = ZodiacSignAtLongitude(longitude);

            //get name of ordinary sign
            var ordinarySignName = ordinarySign.GetSignName();

            //2.0 Get first navamsa sign
            ZodiacName firstNavamsa;

            switch (ordinarySignName)
            {
                //Aries, Leo, Sagittarius - from Aries.
                case ZodiacName.Aries:
                case ZodiacName.Leo:
                case ZodiacName.Sagittarius:
                    firstNavamsa = ZodiacName.Aries;
                    break;
                //Taurus, Capricorn, Virgo - from Capricorn.
                case ZodiacName.Taurus:
                case ZodiacName.Capricorn:
                case ZodiacName.Virgo:
                    firstNavamsa = ZodiacName.Capricorn;
                    break;
                //Gemini, Libra, Aquarius - from Libra.
                case ZodiacName.Gemini:
                case ZodiacName.Libra:
                case ZodiacName.Aquarius:
                    firstNavamsa = ZodiacName.Libra;
                    break;
                //Cancer, Scorpio, Pisces - from Cancer.
                case ZodiacName.Cancer:
                case ZodiacName.Scorpio:
                case ZodiacName.Pisces:
                    firstNavamsa = ZodiacName.Cancer;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //3.0 Get the number of the navamsa currently in
            //get degrees in ordinary sign
            var degreesInOrdinarySign = ordinarySign.GetDegreesInSign();

            //declare length of a navamsa in the ecliptic arc
            const double navamsaLenghtInDegrees = 3.333333333;

            //divide total degrees in current sign to get raw navamsa number
            var rawNavamsaNumber = degreesInOrdinarySign.TotalDegrees / navamsaLenghtInDegrees;

            //round the raw number to get current navamsa number
            var navamsaNumber = (int)Math.Ceiling(rawNavamsaNumber);

            //4.0 Get navamsa sign
            //count from first navamsa sign
            ZodiacName signAtNavamsa = SignCountedFromInputSign(firstNavamsa, navamsaNumber);

            return signAtNavamsa;

        }

        /// <summary>
        /// Exp : Get 4th sign from Cancer
        /// </summary>
        public static ZodiacName SignCountedFromInputSign(ZodiacName inputSign, int countToNextSign)
        {
            //assign counted to same as starting sign at first
            ZodiacName signCountedTo = inputSign;

            //get the next sign the same number as the count to next sign
            for (int i = 1; i < countToNextSign; i++)
            {
                //get the next zodiac sign from the current counted to sign
                signCountedTo = NextZodiacSign(signCountedTo);
            }

            return signCountedTo;

        }

        /// <summary>
        /// Exp : Get 4th sign from Moon
        /// </summary>
        public static ZodiacName SignCountedFromMoonSign(int countToNextSign, Time inputTime)
        {
            var moonSignName = MoonSignName(inputTime);
            return SignCountedFromInputSign(moonSignName, countToNextSign);
        }

        /// <summary>
        /// Exp : Get 4th sign from Saturn
        /// </summary>
        public static ZodiacName SignCountedFromPlanetSign(int countToNextSign, Time inputTime, PlanetName startPlanet)
        {
            var planetSignName = PlanetSignName(startPlanet, inputTime).GetSignName();
            return SignCountedFromInputSign(planetSignName, countToNextSign);
        }

        /// <summary>
        /// Exp : Get 4th sign from Lagna/Ascendant
        /// </summary>
        public static ZodiacName SignCountedFromLagnaSign(int countToNextSign, Time inputTime) => SignCountedFromInputSign(LagnaSignName(inputTime), countToNextSign);

        /// <summary>
        /// Exp : Get 4th house from 5th house (input house)
        /// </summary>
        public static int HouseCountedFromInputHouse(int inputHouseNumber, int countToNextHouse)
        {
            //assign count to same as starting house at first
            int houseCountedTo = inputHouseNumber;

            //get the next house the same number as the count to next house
            for (int i = 1; i < countToNextHouse; i++)
            {
                //get the next house number from the current counted to house
                houseCountedTo = NextHouseNumber(houseCountedTo);
            }

            return houseCountedTo;

        }

        /// <summary>
        /// Get zodiac sign planet is in.
        /// </summary>
        public static ZodiacSign PlanetSignName(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSignName), planetName, time, Ayanamsa), _getPlanetRasiSign);

            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetRasiSign()
            {
                //get longitude of planet
                var longitudeOfPlanet = PlanetNirayanaLongitude(time, planetName);

                //get sign planet is in
                var signPlanetIsIn = ZodiacSignAtLongitude(longitudeOfPlanet);

                //return
                return signPlanetIsIn;

            }

        }

        /// <summary>
        /// Checks if a given planet is in a given sign at a given time
        /// </summary>
        public static bool IsPlanetInSign(PlanetName planetName, ZodiacName signInput, Time time)
        {
            var currentSign = PlanetSignName(planetName, time).GetSignName();

            //check if sign match
            return currentSign == signInput;
        }

        /// <summary>
        /// Get Navamsa sign of planet at a given time
        /// </summary>
        public static ZodiacName PlanetNavamsaSign(PlanetName planetName, Time time)
        {
            //get planets longitude
            var planetLongitude = PlanetNirayanaLongitude(time, planetName);

            //get navamsa sign at longitude
            var navamsaSignOfPlanet = NavamsaSignNameFromLongitude(planetLongitude);

            return navamsaSignOfPlanet;
        }

        /// <summary>
        /// Gives a list of all zodiac signs a specified planet is aspecting
        /// 
        /// All their location with a quarter sight, the 5th and the
        /// 9th houses with a half sight, the 4th and the 8th houses
        /// with three-quarters of a sight and the 7th house with
        /// a full sight.
        /// </summary>
        public static List<ZodiacName> SignsPlanetIsAspecting(PlanetName planetName, Time time)
        {
            //create empty list of signs
            var planetSignList = new List<ZodiacName>();

            //get zodiac sign name which the planet is currently in
            var planetSignName = PlanetSignName(planetName, time).GetSignName();

            // Saturn powerfully aspects the 3rd and the 10th houses
            if (planetName == Saturn)
            {
                //get signs planet is aspecting
                var sign3FromSaturn = SignCountedFromInputSign(planetSignName, 3);
                var sign10FromSaturn = SignCountedFromInputSign(planetSignName, 10);

                //add signs to return list
                planetSignList.Add(sign3FromSaturn);
                planetSignList.Add(sign10FromSaturn);

            }

            // Jupiter the 5th and the 9th houses
            if (planetName == Jupiter)
            {
                //get signs planet is aspecting
                var sign5FromJupiter = SignCountedFromInputSign(planetSignName, 5);
                var sign9FromJupiter = SignCountedFromInputSign(planetSignName, 9);

                //add signs to return list
                planetSignList.Add(sign5FromJupiter);
                planetSignList.Add(sign9FromJupiter);

            }

            // Mars, the 4th and the 8th houses
            if (planetName == Mars)
            {
                //get signs planet is aspecting
                var sign4FromMars = SignCountedFromInputSign(planetSignName, 4);
                var sign8FromMars = SignCountedFromInputSign(planetSignName, 8);

                //add signs to return list
                planetSignList.Add(sign4FromMars);
                planetSignList.Add(sign8FromMars);

            }

            //All planets aspect 7th house

            //get signs planet is aspecting
            var sign7FromPlanet = SignCountedFromInputSign(planetSignName, 7);

            //add signs to return list
            planetSignList.Add(sign7FromPlanet);


            return planetSignList;

        }

        /// <summary>
        /// Get navamsa sign of house (mid point)
        /// TODO: Checking for correctness needed
        /// </summary>
        public static ZodiacName HouseNavamsaSign(HouseName house, Time time)
        {
            //if empty return Aries
            if (house == HouseName.Empty) { return ZodiacName.Aries; }

            //get all houses
            var allHouseList = AllHouseLongitudes(time);

            //get house mid longitude
            var houseMiddleLongitude = allHouseList.Find(hs => hs.GetHouseName() == house).GetMiddleLongitude();

            //get navamsa house sign at house mid longitude
            var navamsaSign = NavamsaSignNameFromLongitude(houseMiddleLongitude);

            return navamsaSign;
        }

        /// <summary>
        /// Get Thrimsamsa sign of house (mid point)
        /// </summary>
        public static ZodiacName PlanetThrimsamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for Thrimsamsa calculation
            const double maxThrimsamsaDegrees = 1; // 30/1
            const double maxSignDegrees = 30.0;

            //get rough Thrimsamsa number
            double roughThrimsamsaNumber = (degreesInSign % maxSignDegrees) / maxThrimsamsaDegrees;

            //get rounded saptamsa number
            var thrimsamsaNumber = (int)Math.Ceiling(roughThrimsamsaNumber);

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //1,2,3,4,5 - Mars
                if (thrimsamsaNumber >= 0 && thrimsamsaNumber <= 5)
                {
                    //Aries and Scorpio are ruled by Mars
                    return ZodiacName.Scorpio;
                }
                //6,7,8,9,10 - saturn
                if (thrimsamsaNumber >= 6 && thrimsamsaNumber <= 10)
                {
                    //Capricorn and Aquarius by Saturn.
                    return ZodiacName.Capricorn;

                }
                //11,12,13,14,15,16,17,18 - jupiter
                if (thrimsamsaNumber >= 11 && thrimsamsaNumber <= 18)
                {
                    //Sagittarius and Pisces by Jupiter
                    return ZodiacName.Sagittarius;

                }
                //19,20,21,22,23,24,25 - mercury
                if (thrimsamsaNumber >= 19 && thrimsamsaNumber <= 25)
                {
                    //Gemini and Virgo by Mercury
                    return ZodiacName.Gemini;
                }
                //26,27,28,29,30 - venus
                if (thrimsamsaNumber >= 26 && thrimsamsaNumber <= 30)
                {
                    //Taurus and Libra by Venus;
                    return ZodiacName.Taurus;
                }

            }

            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                //1,2,3,4,5 - venus
                if (thrimsamsaNumber >= 0 && thrimsamsaNumber <= 5)
                {
                    //Taurus and Libra by Venus;
                    return ZodiacName.Taurus;
                }
                //6,7,8,9,10,11,12 - mercury
                if (thrimsamsaNumber >= 6 && thrimsamsaNumber <= 12)
                {
                    //Gemini and Virgo by Mercury
                    return ZodiacName.Gemini;
                }
                //13,14,15,16,17,18,19,20 - jupiter
                if (thrimsamsaNumber >= 13 && thrimsamsaNumber <= 20)
                {
                    //Sagittarius and Pisces by Jupiter
                    return ZodiacName.Sagittarius;

                }
                //21,22,23,24,25 - saturn
                if (thrimsamsaNumber >= 21 && thrimsamsaNumber <= 25)
                {
                    //Capricorn and Aquarius by Saturn.
                    return ZodiacName.Capricorn;

                }
                //26,27,28,29,30 - Mars
                if (thrimsamsaNumber >= 26 && thrimsamsaNumber <= 30)
                {
                    //Aries and Scorpio are ruled by Mars
                    return ZodiacName.Scorpio;
                }

            }

            throw new Exception("Thrimsamsa not found, error!");
        }

        /// <summary>
        /// When a sign is divided into 12 equal parts each is called a dwadasamsa and measures 2.5 degrees.
        /// The Bhachakra can thus he said to contain 12x12=144 Dwadasamsas. The lords of the 12
        /// Dwadasamsas in a sign are the lords of the 12 signs from it, i.e.,
        /// the lord of the first Dwadasamsa in Mesha is Kuja, that of the second Sukra and so on.
        /// </summary>
        public static ZodiacName PlanetDwadasamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for Dwadasamsa calculation
            const double maxDwadasamsaDegrees = 2.5; // 30/12
            const double maxSignDegrees = 30.0;

            //get rough Dwadasamsa number
            double roughDwadasamsaNumber = (degreesInSign % maxSignDegrees) / maxDwadasamsaDegrees;

            //get rounded Dwadasamsa number
            var dwadasamsaNumber = (int)Math.Ceiling(roughDwadasamsaNumber);

            //get Dwadasamsa sign from counting with Dwadasamsa number
            var dwadasamsaSign = SignCountedFromInputSign(planetSignName, dwadasamsaNumber);

            return dwadasamsaSign;
        }

        /// <summary>
        /// sign is divided into 7 equal parts each is called a Saptamsa and measures 4.28 degrees
        /// </summary>
        public static ZodiacName PlanetSaptamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for saptamsa calculation
            const double maxSaptamsaDegrees = 4.285714285714286; // 30/7
            const double maxSignDegrees = 30.0;

            //get rough saptamsa number
            double roughSaptamsaNumber = (degreesInSign % maxSignDegrees) / maxSaptamsaDegrees;

            //get rounded saptamsa number
            var saptamsaNumber = (int)Math.Ceiling(roughSaptamsaNumber);

            //2.0 Get even or odd sign

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //convert saptamsa number to zodiac name
                return SignCountedFromInputSign(planetSignName, saptamsaNumber);
            }

            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                var countToNextSign = saptamsaNumber + 6;
                return SignCountedFromInputSign(planetSignName, countToNextSign);
            }


            throw new Exception("Saptamsa not found, error!");
        }

        /// <summary>
        /// Gets the Drekkana sign the planet is in
        /// </summary>
        public static ZodiacName PlanetDrekkanaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //1.0 get the number of the drekkana the planet is in

            //if planet is in 1st drekkana
            if (degreesInSign >= 0 && degreesInSign <= 10)
            {
                //return planet's current sign
                return planetSignName;
            }

            //if planet is in 2nd drekkana
            if (degreesInSign > 10 && degreesInSign <= 20)
            {
                //return 5th sign from planets current sign
                return SignCountedFromInputSign(planetSignName, 5);
            }

            //if planet is in 3rd drekkana
            if (degreesInSign > 20 && degreesInSign <= 30)
            {
                //return 9th sign from planets current sign
                return SignCountedFromInputSign(planetSignName, 9);
            }

            throw new Exception("Planet drekkana not found, error!");
        }

        /// <summary>
        /// Similar to Exaltation but covers a range not just a point
        /// Moolathrikonas, these are positions similar to exaltation.
        /// NOTE:
        /// - No moolatrikone for Rahu & Ketu, no error will be raised
        /// </summary>
        public static bool IsPlanetInMoolatrikona(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetSignName(planetName, time);

            //Sun's Moola Thrikona is Leo (0°-20°);
            if (planetName == Sun)
            {
                if (planetSign.GetSignName() == ZodiacName.Leo)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //Moon-Taurus (4°-30°);
            if (planetName == Moon)
            {
                if (planetSign.GetSignName() == ZodiacName.Taurus)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 4 && degreesInSign <= 30)
                    {
                        return true;
                    }
                }
            }

            //Mercury-Virgo (16°-20°);
            if (planetName == Mercury)
            {
                if (planetSign.GetSignName() == ZodiacName.Virgo)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 16 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //Jupiter-Sagittarius (0°-13°);
            if (planetName == Jupiter)
            {
                if (planetSign.GetSignName() == ZodiacName.Sagittarius)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 13)
                    {
                        return true;
                    }
                }
            }

            // Mars-Aries (0°-18°);
            if (planetName == Mars)
            {
                if (planetSign.GetSignName() == ZodiacName.Aries)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 18)
                    {
                        return true;
                    }
                }
            }

            // Venus-Libra (0°-10°)
            if (planetName == Venus)
            {
                if (planetSign.GetSignName() == ZodiacName.Libra)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 10)
                    {
                        return true;
                    }
                }
            }

            // Saturn-Aquarius (0°-20°).
            if (planetName == Saturn)
            {
                if (planetSign.GetSignName() == ZodiacName.Aquarius)
                {
                    var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

                    if (degreesInSign >= 0 && degreesInSign <= 20)
                    {
                        return true;
                    }
                }
            }

            //if no above conditions met, moolatrikonas not happening 
            return false;
        }

        /// <summary>
        /// Gets a planet's relationship to a sign, based on the relation to the lord
        /// Note :
        /// - Moolatrikona, Debilited & Exalted is not calculated heres
        /// - Rahu & ketu not accounted for
        /// </summary>
        public static PlanetToSignRelationship PlanetRelationshipWithSign(PlanetName planetName, ZodiacName zodiacSignName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return PlanetToSignRelationship.Empty; }


            //types of relationship
            //Swavarga - own varga
            //Samavarga - neutral's varga
            //Mitravarga - friendly varga
            //Adhi Mitravarga - Intimate friend varga
            //Satruvarga - enemy's varga
            //Adhi Satruvarga - Bitter enemy varga


            //Get lord of zodiac sign
            var lordOfSign = LordOfZodiacSign(zodiacSignName);

            //if lord of sign is same as input planet
            if (planetName == lordOfSign)
            {
                //return own varga, swavarga
                return PlanetToSignRelationship.OwnVarga;
            }

            //else, get relationship between input planet and lord of sign
            PlanetToPlanetRelationship relationshipToLordOfSign = PlanetCombinedRelationshipWithPlanet(planetName, lordOfSign, time);

            //return relation ship with sign based on relationship with lord of sign
            switch (relationshipToLordOfSign)
            {
                case PlanetToPlanetRelationship.BestFriend:
                    return PlanetToSignRelationship.BestFriendVarga;
                case PlanetToPlanetRelationship.Friend:
                    return PlanetToSignRelationship.FriendVarga;
                case PlanetToPlanetRelationship.BitterEnemy:
                    return PlanetToSignRelationship.BitterEnemyVarga;
                case PlanetToPlanetRelationship.Enemy:
                    return PlanetToSignRelationship.EnemyVarga;
                case PlanetToPlanetRelationship.Neutral:
                    return PlanetToSignRelationship.NeutralVarga;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// strengths of planets, mix the temporary relations and the permanent
        /// 
        /// In order to find the strengths of planets we have
        /// to mix the temporary relations and the permanent
        /// relations. Thus a temporary enemy plus a permanent
        /// or natural enemy becomes a bitter enemy.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetCombinedRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = mainPlanet.Name == PlanetNameEnum.Rahu;
            var isKetu = mainPlanet.Name == PlanetNameEnum.Ketu;
            var isRahu2 = secondaryPlanet.Name == PlanetNameEnum.Rahu;
            var isKetu2 = secondaryPlanet.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu || isRahu2 || isKetu2;
            if (isRahuKetu) { return PlanetToPlanetRelationship.Empty; }


            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }

            //get planet's permanent relationship
            PlanetToPlanetRelationship planetPermanentRelationship = PlanetPermanentRelationshipWithPlanet(mainPlanet, secondaryPlanet);

            //get planet's temporary relationship
            PlanetToPlanetRelationship planetTemporaryRelationship = PlanetTemporaryRelationshipWithPlanet(mainPlanet, secondaryPlanet, time);

            //Tatkalika Mitra + Naisargika Mitra = Adhi Mitras
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Friend)
            {
                //they both become intimate friends (Adhi Mitras).
                return PlanetToPlanetRelationship.BestFriend;
            }

            //Tatkalika Mitra + Naisargika Satru = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Enemy)
            {
                return PlanetToPlanetRelationship.Neutral;
            }

            //Tatkalika Mitra + Naisargika Sama = Mitra
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Friend && planetPermanentRelationship == PlanetToPlanetRelationship.Neutral)
            {
                return PlanetToPlanetRelationship.Friend;
            }

            //Tatkalika Satru + Naisargika Satru = Adhi Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Enemy)
            {
                return PlanetToPlanetRelationship.BitterEnemy;
            }

            //Tatkalika Satru + Naisargika Mitra = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Friend)
            {
                return PlanetToPlanetRelationship.Neutral;
            }

            //Tatkalika Satru + Naisargika Sama = Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Enemy && planetPermanentRelationship == PlanetToPlanetRelationship.Neutral)
            {
                return PlanetToPlanetRelationship.Enemy;
            }

            throw new Exception("Combined planet relationship not found, error!");
        }

        /// <summary>
        /// Relation between the planet and the lord of the sign of the house
        /// 
        /// Gets a planets relationship with a house,
        /// Based on the relation between the planet and the lord of the sign of the house
        /// Note : needs verification if this is correct
        /// </summary>
        public static PlanetToSignRelationship PlanetRelationshipWithHouse(HouseName house, PlanetName planet, Time time)
        {
            //get sign the house is in
            var houseSign = HouseSignName(house, time);

            //get the planet's relationship with the sign
            var relationship = PlanetRelationshipWithSign(planet, houseSign, time);

            return relationship;
        }

        /// <summary>
        /// Planets found in the certain signs from any other planet becomes temporary friends
        /// 
        /// Temporary Friendship
        /// Planets found in the 2nd, 3rd, 4th, 10th, 11th
        /// and 12th signs from any other planet becomes the
        /// latter's temporary friends. The others are its enemies.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetTemporaryRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {
            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }


            //1.0 get planet's friends
            var friendlyPlanetList = PlanetTemporaryFriendList(mainPlanet, time);

            //check if planet is found in friend list
            var planetFoundInFriendList = friendlyPlanetList.Contains(secondaryPlanet);

            //if found in friend list
            if (planetFoundInFriendList)
            {
                //return relationship as friend
                return PlanetToPlanetRelationship.Friend;
            }

            //if planet is not a friend then it is an enemy
            //return relationship as enemy
            return PlanetToPlanetRelationship.Enemy;
        }


        /// <summary>
        /// Gets all the planets in a sign
        /// </summary>
        public static List<PlanetName> PlanetInSign(ZodiacName signName, Time time)
        {
            //get all planets locations in signs
            var sunSignName = PlanetSignName(Sun, time).GetSignName();
            var moonSignName = PlanetSignName(Moon, time).GetSignName();
            var marsSignName = PlanetSignName(Mars, time).GetSignName();
            var mercurySignName = PlanetSignName(Mercury, time).GetSignName();
            var jupiterSignName = PlanetSignName(Jupiter, time).GetSignName();
            var venusSignName = PlanetSignName(Venus, time).GetSignName();
            var saturnSignName = PlanetSignName(Saturn, time).GetSignName();
            var rahuSignName = PlanetSignName(Rahu, time).GetSignName();
            var ketuSignName = PlanetSignName(Ketu, time).GetSignName();


            //create empty list of planet names to return
            var planetFoundInSign = new List<PlanetName>();

            //if planet is in same sign as input sign add planet to list
            if (sunSignName == signName)
            {
                planetFoundInSign.Add(Sun);
            }
            if (moonSignName == signName)
            {
                planetFoundInSign.Add(Moon);
            }
            if (marsSignName == signName)
            {
                planetFoundInSign.Add(Mars);
            }
            if (mercurySignName == signName)
            {
                planetFoundInSign.Add(Mercury);
            }
            if (jupiterSignName == signName)
            {
                planetFoundInSign.Add(Jupiter);
            }
            if (venusSignName == signName)
            {
                planetFoundInSign.Add(Venus);
            }
            if (saturnSignName == signName)
            {
                planetFoundInSign.Add(Saturn);
            }
            if (rahuSignName == signName)
            {
                planetFoundInSign.Add(Rahu);
            }
            if (ketuSignName == signName)
            {
                planetFoundInSign.Add(Ketu);
            }


            return planetFoundInSign;
        }

        /// <summary>
        /// Get list of Temporary (Tatkalika) Friend for a planet
        /// 
        /// The planets in -the 2nd, 3rd, 4th, 10th, 11th and
        /// 12th signs from any other planet becomes his
        /// (Tatkalika) friend.
        /// </summary>
        public static List<PlanetName> PlanetTemporaryFriendList(PlanetName planetName, Time time)
        {
            //get sign planet is currently in
            var planetSignName = PlanetSignName(planetName, time).GetSignName();

            //Get signs of friends of main planet
            //get planets in 2nd
            var sign2FromMainPlanet = SignCountedFromInputSign(planetSignName, 2);
            //get planets in 3rd
            var sign3FromMainPlanet = SignCountedFromInputSign(planetSignName, 3);
            //get planets in 4th
            var sign4FromMainPlanet = SignCountedFromInputSign(planetSignName, 4);
            //get planets in 10th
            var sign10FromMainPlanet = SignCountedFromInputSign(planetSignName, 10);
            //get planets in 11th
            var sign11FromMainPlanet = SignCountedFromInputSign(planetSignName, 11);
            //get planets in 12th
            var sign12FromMainPlanet = SignCountedFromInputSign(planetSignName, 12);

            //add houses of friendly planets to a list
            var signsOfFriendlyPlanet = new List<ZodiacName>(){sign2FromMainPlanet, sign3FromMainPlanet, sign4FromMainPlanet,
                sign10FromMainPlanet, sign11FromMainPlanet, sign12FromMainPlanet};

            //declare list of friendly planets
            var friendlyPlanetList = new List<PlanetName>();

            //loop through the signs and fill the friendly planet list
            foreach (var sign in signsOfFriendlyPlanet)
            {
                //get the planets in the current sign
                var friendlyPlanetsInThisSign = PlanetInSign(sign, time);

                //add the planets in to the list
                friendlyPlanetList.AddRange(friendlyPlanetsInThisSign);
            }

            //remove rahu & ketu from list
            friendlyPlanetList.Remove(Rahu);
            friendlyPlanetList.Remove(Ketu);


            return friendlyPlanetList;

        }

        /// <summary>
        /// Greenwich Apparent In Julian Days
        /// </summary>
        public static double GreenwichApparentInJulianDays(Time time)
        {
            //convert lmt to julian days, in universal time (UT)
            var localMeanTimeInJulian_UT = GreenwichLmtInJulianDays(time);

            //get longitude of location
            double longitude = time.GetGeoLocation().Longitude();

            //delcare output variables
            double localApparentTimeInJulian;
            string errorString = "";

            //convert lmt to local apparent time (LAT)
            using SwissEph ephemeris = new();
            ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);


            return localApparentTimeInJulian;
        }

        /// <summary>
        /// Shows local apparent time from Swiss Eph
        /// </summary>
        public static DateTime LocalApparentTime(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LocalApparentTime), time, Ayanamsa), _getLocalApparentTime);

            //UNDERLYING FUNCTION
            DateTime _getLocalApparentTime()
            {
                //convert lmt to julian days, in universal time (UT)
                var localMeanTimeInJulian_UT = ConvertLmtToJulian(time);

                //get longitude of location
                double longitude = time.GetGeoLocation().Longitude();

                //delcare output variables
                double localApparentTimeInJulian;
                string errorString = null;

                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //convert lmt to local apparent time (LAT)
                ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);

                var localApparentTime = ConvertJulianTimeToNormalTime(localApparentTimeInJulian);

                return localApparentTime;

            }

        }

        /// <summary>
        /// This method exists mainly for testing internal time calculation of LMT
        /// Important that this method passes the test at all times, so much depends on this
        /// </summary>
        public static DateTimeOffset LocalMeanTime(Time time) => time.GetLmtDateTimeOffset();

        /// <summary>
        /// House start middle and end longitudes
        /// </summary>
        public static House House(HouseName houseNumber, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(House), houseNumber, time, Ayanamsa), _getHouse);

            //UNDERLYING FUNCTION
            House _getHouse()
            {
                //get all house list
                var allHouses = AllHouseLongitudes(time);

                //get required house from list
                var requiredHouse = allHouses.Find(h => h.GetHouseName() == houseNumber);

                return requiredHouse;

            }

        }

        /// <summary>
        /// Gets Panchaka at a given time
        /// </summary>
        public static PanchakaName Panchaka(Time time)
        {
            //If the remainder is 1 (mrityu panchakam), it
            // indicates danger; if 2 (agni panchakam), risk from fire; if 4 (raja
            // panchakam), bad results; if 6 (chora panchakam), evil happenings and if
            // 8 (roga panchakam), disease. If the remainder is 3, 5, 7 or zero then it is
            // good.

            //get the number of the lunar day (from the 1st of the month),
            var lunarDateNumber = LunarDay(time).GetLunarDateNumber();

            //get the number of the constellation (from Aswini)
            var rullingConstellationNumber = MoonConstellation(time).GetConstellationNumber();

            //Number of weekday
            var weekdayNumber = (int)DayOfWeek(time);

            //Number of zodiacal sign, number of the Lagna (from Aries).
            var risingSignNumber = (int)HouseSignName(HouseName.House1, time);

            //add all to get total
            double total = lunarDateNumber + rullingConstellationNumber + weekdayNumber + risingSignNumber;

            //get modulos of 9 to get panchaka number (Remainder From Division)
            var panchakaNumber = total % 9.0;

            //convert panchakam number to name
            switch (panchakaNumber)
            {
                //1 (mrityu panchakam)
                case 1:
                    return PanchakaName.Mrityu;
                //2 (agni panchakam)
                case 2:
                    return PanchakaName.Agni;
                //4 (raja panchakam)
                case 4:
                    return PanchakaName.Raja;
                //6 (chora panchakam)
                case 6:
                    return PanchakaName.Chora;
                //8 (roga panchakam)
                case 8:
                    return PanchakaName.Roga;
                //If the remainder is 3, 5, 7 or 0 then it is good (shubha)
                case 3:
                case 5:
                case 7:
                case 0:
                    return PanchakaName.Shubha;
            }

            //if panchaka number did not match above, throw error
            throw new Exception("Panchaka not found, error!");


        }

        /// <summary>
        /// Planet lord that governs a weekday
        /// </summary>
        public static PlanetName LordOfWeekday(Time time)
        {
            //Sunday Sun
            //Monday Moon
            //Tuesday Mars
            //Wednesday Mercury
            //Thursday Jupiter
            //Friday Venus
            //Saturday Saturn

            //get the weekday
            var weekday = DayOfWeek(time);

            //based on weekday return the planet lord
            switch (weekday)
            {
                case Library.DayOfWeek.Sunday: return Sun;
                case Library.DayOfWeek.Monday: return Moon;
                case Library.DayOfWeek.Tuesday: return Mars;
                case Library.DayOfWeek.Wednesday: return Mercury;
                case Library.DayOfWeek.Thursday: return Jupiter;
                case Library.DayOfWeek.Friday: return Venus;
                case Library.DayOfWeek.Saturday: return Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Planet lord that governs a weekday
        /// </summary>
        public static PlanetName LordOfWeekday(DayOfWeek weekday)
        {
            //Sunday Sun
            //Monday Moon
            //Tuesday Mars
            //Wednesday Mercury
            //Thursday Jupiter
            //Friday Venus
            //Saturday Saturn

            //based on weekday return the planet lord
            switch (weekday)
            {
                case Library.DayOfWeek.Sunday: return Sun;
                case Library.DayOfWeek.Monday: return Moon;
                case Library.DayOfWeek.Tuesday: return Mars;
                case Library.DayOfWeek.Wednesday: return Mercury;
                case Library.DayOfWeek.Thursday: return Jupiter;
                case Library.DayOfWeek.Friday: return Venus;
                case Library.DayOfWeek.Saturday: return Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Convert Local Mean Time (LMT) to Standard Time (STD)
        /// </summary>
        public static DateTimeOffset LmtToStd(DateTimeOffset lmtDateTime, TimeSpan stdOffset)
        {
            //set lmt to offset
            //var tempTime = new DateTimeOffset(lmtDateTime);

            return lmtDateTime.ToOffset(stdOffset);
        }

        /// <summary>
        /// A hora is equal to 1/24th part of
        /// a day. The Hindu day begins with sunrise and continues till
        /// next sunrise. The first hora on any day will be the
        /// first hour after sunrise and the last hora, the hour
        /// before sunrise the next day.
        /// </summary>
        public static int HoraAtBirth(Time time)
        {
            TimeSpan hours;

            var birthTime = time.GetLmtDateTimeOffset();
            var sunriseTime = SunriseTime(time).GetLmtDateTimeOffset();

            //if birth time is after sunrise, then sunrise time is correct 
            if (birthTime >= sunriseTime)
            {
                //get hours (hora) passed since sunrise (start of day)
                hours = birthTime.Subtract(sunriseTime);
            }
            //else birth has occured before sunrise on that day,
            //so have to use sunrise of the previous day
            else
            {
                //get sunrise of the previous day
                var previousDay = new Time(time.GetLmtDateTimeOffset().DateTime.AddDays(-1), time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());
                sunriseTime = SunriseTime(previousDay).GetLmtDateTimeOffset();

                //get hours (hora) passed since sunrise (start of day)
                hours = birthTime.Subtract(sunriseTime);

            }

            //round hours to highest possible (ceiling)
            var hora = Math.Ceiling(hours.TotalHours);

            //if birth time is exactly as sunrise time hora will be zero here, meaning 1st hora
            if (hora == 0) { hora = 1; }


            return (int)hora;

        }

        /// <summary>
        /// Gets hora zodiac sign of a planet
        /// </summary>
        public static ZodiacName PlanetHoraSign(PlanetName planetName, Time time)
        {
            //get planet sign
            var planetSign = PlanetSignName(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get planet degrees in sign
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare flags
            var planetInFirstHora = false;
            var planetInSecondHora = false;

            //1.0 get which hora planet is in
            //if sign in first hora (0 to 15 degrees)
            if (degreesInSign >= 0 && degreesInSign <= 15)
            {
                planetInFirstHora = true;
            }

            //if sign in second hora (15 to 30 degrees)
            if (degreesInSign > 15 && degreesInSign <= 30)
            {
                planetInSecondHora = true;
            }

            //2.0 check which type of sign the planet is in

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //if planet in first hora
                if (planetInFirstHora == true && planetInSecondHora == false)
                {
                    //governed by the Sun (Leo)
                    return ZodiacName.Leo;
                }

                //if planet in second hora
                if (planetInFirstHora == false && planetInSecondHora == true)
                {
                    //governed by the Moon (Cancer)
                    return ZodiacName.Cancer;
                }

            }


            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                //if planet in first hora
                if (planetInFirstHora == true && planetInSecondHora == false)
                {
                    //governed by the Moon (Cancer)
                    return ZodiacName.Cancer;

                }

                //if planet in second hora
                if (planetInFirstHora == false && planetInSecondHora == true)
                {
                    //governed by the Sun (Leo)
                    return ZodiacName.Leo;
                }

            }

            throw new Exception("Planet hora not found, error!");
        }

        /// <summary>
        /// get sunrise time for that day at that place
        /// </summary>
        public static Time SunriseTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(SunriseTime), time, Ayanamsa), _getSunriseTime);


            //UNDERLYING FUNCTION
            Time _getSunriseTime()
            {
                //1. Calculate sunrise time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_RISE | srflag; //set for sunrise
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().Longitude(), time.GetGeoLocation().Latitude(), 0 };
                double riseTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature

                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());


                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = GreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunrise time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref riseTimeRaw, ref errorMsg);


                //2. Convert raw sun rise time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = GreenwichTimeFromJulianDays(riseTimeRaw);

                //return sunrise time at orginal location to caller
                var stdOriginal = sunriseLmtAtGreenwich.ToOffset(time.GetStdDateTimeOffset().Offset);
                var sunriseTime = new Time(stdOriginal, time.GetGeoLocation());
                return sunriseTime;

            }

        }

        /// <summary>
        /// Get actual sunset time for that day at that place
        /// </summary>
        public static Time SunsetTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(SunsetTime), time, Ayanamsa), _getSunsetTime);


            //UNDERLYING FUNCTION
            Time _getSunsetTime()
            {
                //1. Calculate sunset time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_SET | srflag; //set for sunset
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().Longitude(), time.GetGeoLocation().Latitude(), 0 };
                double setTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature


                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());

                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = GreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunset time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref setTimeRaw, ref errorMsg);


                //2. Convert raw sun set time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = GreenwichTimeFromJulianDays(setTimeRaw);

                //return sunset time at orginal location to caller
                var stdOriginal = sunriseLmtAtGreenwich.ToOffset(time.GetStdDateTimeOffset().Offset);
                var sunsetTime = new Time(stdOriginal, time.GetGeoLocation());
                return sunsetTime;

            }

        }

        /// <summary>
        /// Get actual noon time for that day at that place
        /// Returned in apparent time (DateTime)
        /// Note:
        /// This is marked when the centre of the Sun is exactly* on the
        /// meridian of the place. The apparent noon is
        /// almost the same for all places.
        /// *Center of disk is not actually used for now (future implementation)
        /// </summary>
        public static DateTime NoonTime(Time time)
        {
            //get apparent time
            var localApparentTime = LocalApparentTime(time);
            var apparentNoon = new DateTime(localApparentTime.Year, localApparentTime.Month, localApparentTime.Day, 12, 0, 0);

            return apparentNoon;
        }

        /// <summary>
        /// Checks if planet A is in good aspect to planet B
        ///
        /// Note:
        /// A is transmitter, B is receiver
        /// 
        /// An aspect is good or bad according to the relation
        /// between the aspecting and the aspected body
        /// </summary>
        public static bool IsPlanetInGoodAspectToPlanet(PlanetName receivingAspect, PlanetName transmitingAspect, Time time)
        {
            //check if transmitting planet is aspecting receiving planet
            var isAspecting = IsPlanetAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = PlanetCombinedRelationshipWithPlanet(receivingAspect, transmitingAspect, time);
            var isGood = aspectNature == PlanetToPlanetRelationship.BestFriend ||
                         aspectNature == PlanetToPlanetRelationship.Friend;

            //if is aspecting and it is good, then occuring (true)
            return isAspecting && isGood;

        }

        /// <summary>
        ///Checks if a planet is in good aspect to a house
        ///
        /// Note:
        /// An aspect is good or bad according to the relation
        /// between the planet and lord of the house sign
        /// </summary>
        public static bool IsPlanetInGoodAspectToHouse(HouseName receivingAspect, PlanetName transmitingAspect, Time time)
        {
            //check if transmiting planet is aspecting receiving planet
            var isAspecting = IsHouseAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = PlanetRelationshipWithHouse(receivingAspect, transmitingAspect, time);

            var isGood = aspectNature == PlanetToSignRelationship.OwnVarga || //Swavarga - own varga
                         aspectNature == PlanetToSignRelationship.FriendVarga || //Mitravarga - friendly varga
                         aspectNature == PlanetToSignRelationship.BestFriendVarga; //Adhi Mitravarga - Intimate friend varga


            //if is aspecting and it is good, then occuring (true)
            return isAspecting && isGood;

        }

        /// <summary>
        /// To determine if sthana bala is indicating good position or bad position
        /// a neutral point is set, anything above is good & below is bad
        ///
        /// Note:
        /// Neutral point is derived from all possible sthana bala values across
        /// 25 years (2000-2025), with 1 hour granularity
        ///
        /// Formula used = ((max-min)/2)+min
        /// max = hightest possible value
        /// min = lowest possible value
        /// </summary>
        public static double PlanetSthanaBalaNeutralPoint(PlanetName planet)
        {
            //no calculation for rahu and ketu here
            var isRahu = planet.Name == PlanetNameEnum.Rahu;
            var isKetu = planet.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return 0; }


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSthanaBalaNeutralPoint), planet, Ayanamsa), _getPlanetSthanaBalaNeutralPoint);


            double _getPlanetSthanaBalaNeutralPoint()
            {
                int max = 0, min = 0;

                if (planet == Saturn) { max = 297; min = 59; }
                if (planet == Mars) { max = 362; min = 60; }
                if (planet == Jupiter) { max = 296; min = 77; }
                if (planet == Mercury) { max = 295; min = 47; }
                if (planet == Venus) { max = 284; min = 60; }
                if (planet == Sun) { max = 327; min = 52; }
                if (planet == Moon) { max = 311; min = 54; }

                //calculate neutral point
                var neutralPoint = ((max - min) / 2) + min;

                if (neutralPoint <= 0) { throw new Exception("Planet does not have neutral point!"); }

                return neutralPoint;
            }
        }

        /// <summary>
        /// EXPERIMENTAL
        /// To determine if Shadvarga bala is strong or weak
        /// a neutral point is set, anything above is strong & below is weak
        ///
        /// Note:
        /// Neutral point is derived from all possible Shadvarga bala values across
        /// 25 years (2000-2025), with 1 hour granularity
        ///
        /// Formula used = ((max-min)/2)+min (add min to get exact neutral point from 0 range)
        /// max = hightest possible value
        /// min = lowest possible value
        /// </summary>
        public static double PlanetShadvargaBalaNeutralPoint(PlanetName planet)
        {
            //no calculation for rahu and ketu here
            var isRahu = planet.Name == PlanetNameEnum.Rahu;
            var isKetu = planet.Name == PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return 0; }


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetShadvargaBalaNeutralPoint), planet, Ayanamsa), _getPlanetShadvargaBalaNeutralPoint);


            double _getPlanetShadvargaBalaNeutralPoint()
            {
                int max = 0, min = 0;

                if (planet == Saturn) { max = 150; min = 11; }
                if (planet == Mars) { max = 188; min = 21; }
                if (planet == Jupiter) { max = 172; min = 17; }
                if (planet == Mercury) { max = 150; min = 17; }
                if (planet == Venus) { max = 158; min = 15; }
                if (planet == Sun) { max = 180; min = 17; }
                if (planet == Moon) { max = 165; min = 26; }

                //difference between max & min
                var difference = (max - min);

                //divide difference in half to get neutral point
                //add min to get exact neutral point from 0 range
                var neutralPoint = (difference / 2) + min;

                if (neutralPoint <= 0) { throw new Exception("Planet does not have neutral point!"); }


                return neutralPoint;
            }
        }

        /// <summary>
        /// Checks if a planet is in a kendra house (1,4,7,10)
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName planet, Time time)
        {
            //The 4th, the 7th and the 10th are the Kendras
            var planetHouse = HousePlanetIsIn(time, planet);

            //check if planet is in kendra
            var isPlanetInKendra = planetHouse == HouseName.House1 || planetHouse == HouseName.House4 || planetHouse == HouseName.House7 || planetHouse == HouseName.House10;

            return isPlanetInKendra;
        }

        /// <summary>
        /// Checks if any given planet is in a kendra house (1,4,7,10)
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName[] planetList, Time time)
        {
            //default to false
            var isFound = false;

            //if any planet is found, stop loop and return true
            foreach (var planet in planetList)
            {
                var isKendra = IsPlanetInKendra(planet, time);
                if (isKendra) { return true; }
            }

            return isFound;
        }

        /// <summary>
        /// Checks if a planet is in a kendra house (1,4,7,10) from another planet. Exp : Is Jupiter is in a kendra from the Moon
        /// </summary>
        public static bool IsPlanetInKendraFromPlanet(PlanetName kendraFrom, PlanetName kendraTo, Time time)
        {
            //get the number of signs between planets
            var count = SignDistanceFromPlanetToPlanet(kendraTo, kendraFrom, time);

            //check if number is a kendra number
            var isKendra = count == 1 ||
                           count == 4 ||
                           count == 7 ||
                           count == 10;

            return isKendra;
        }


        /// <summary>
        /// Counts number of sign between 2 planets.
        /// </summary>
        public static int SignDistanceFromPlanetToPlanet(PlanetName startPlanet, PlanetName endPlanet, Time time)
        {
            //get position of "kendra to" planet
            var startSign = PlanetSignName(startPlanet, time);

            //get position of "kendra from" planet
            var endSign = PlanetSignName(endPlanet, time);

            //count distance between signs
            var count = CountFromSignToSign(startSign.GetSignName(), endSign.GetSignName());

            return count;
        }

        /// <summary>
        /// Checks if the lord of a house is in the specified house.
        /// Example question : Is Lord of 1st house in 2nd house?
        /// </summary>
        public static bool IsHouseLordInHouse(HouseName lordHouse, HouseName occupiedHouse, Time time)
        {
            //get the house lord
            var houseLord = LordOfHouse(lordHouse, time);

            //get house the lord is in
            var houseIsIn = HousePlanetIsIn(time, houseLord);

            //if it matches then occuring
            return houseIsIn == occupiedHouse;
        }

        /// <summary>
        /// Checks if a planet is conjuct with an evil/malefic planet
        /// </summary>
        public static bool IsPlanetConjunctWithMaleficPlanets(PlanetName planetName, Time time)
        {
            //get all the planets conjuct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(time, planetName);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any conjunct planet is an evil one
            var evilFound = planetsInConjunct.FindAll(planet => evilPlanets.Contains(planet)).Any();
            return evilFound;

        }

        /// <summary>
        /// Checks if a planet is conjunct with an enemy planet by combined relationship
        /// </summary>
        public static bool IsPlanetConjunctWithEnemyPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets conjunct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(time, inputPlanet);

            //check if any conjunct planet is an enemy
            foreach (var planet in planetsInConjunct)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isEnemy = aspectNature == PlanetToPlanetRelationship.Enemy ||
                              aspectNature == PlanetToPlanetRelationship.BitterEnemy;

                //if enemy than end here as true
                if (isEnemy) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;

        }

        /// <summary>
        /// Checks if a planet is conjunct with an Friend planet by combined relationship
        /// </summary>
        public static bool IsPlanetConjunctWithFriendPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets conjunct with inputed planet
            var planetsInConjunct = PlanetsInConjuction(time, inputPlanet);

            //check if any conjunct planet is an Friend
            foreach (var planet in planetsInConjunct)
            {
                //get relationship of the 2 planets
                var conjunctNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isFriend = conjunctNature == PlanetToPlanetRelationship.Friend ||
                               conjunctNature == PlanetToPlanetRelationship.BestFriend;

                //if enemy than end here as true
                if (isFriend) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;

        }

        /// <summary>
        /// Checks if any evil/malefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsMaleficPlanetInHouse(HouseName houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = PlanetsInHouse(houseNumber, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any planet in house is an evil one
            var evilFound = planetsInHouse.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;

        }

        /// <summary>
        /// Checks if any good/benefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsBeneficPlanetInHouse(HouseName houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = PlanetsInHouse(houseNumber, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any planet in house is an good one
            var goodFound = planetsInHouse.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;

        }


        /// <summary>
        /// Checks if any evil/malefic planets are in a sign
        /// </summary>
        public static bool IsMaleficPlanetInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any planet in sign is an evil one
            var evilFound = planetsInSign.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;
        }

        /// <summary>
        /// Gets list of evil/malefic planets in a sign
        /// </summary>
        public static List<PlanetName> MaleficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //get evil planets in sign
            var evilFound = planetsInSign.FindAll(planet => evilPlanets.Contains(planet));

            return evilFound;
        }

        /// <summary>
        /// Checks if any good/benefic planets are in a sign
        /// </summary>
        public static bool IsBeneficPlanetInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any planet in sign is an good one
            var goodFound = planetsInSign.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;
        }

        /// <summary>
        /// Gets any good/benefic planets in a sign
        /// </summary>
        public static List<PlanetName> BeneficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = PlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //gets all good planets in this sign
            var goodFound = planetsInSign.FindAll(planet => goodPlanets.Contains(planet));

            return goodFound;
        }

        /// <summary>
        /// Checks if any evil/malefic planet is transmitting aspect to a house
        /// Note: This does NOT account for bad aspects, where relationship with house lord is checked
        /// TODO relationship aspect should be added get reference for it firsts
        /// </summary>
        public static bool IsMaleficPlanetAspectHouse(HouseName house, Time time)
        {
            //get all evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any evil planet is aspecting the inputed house
            var evilFound = evilPlanets.FindAll(evilPlanet => IsHouseAspectedByPlanet(house, evilPlanet, time)).Any();

            return evilFound;

        }

        /// <summary>
        /// Checks if any good/benefic planet is transmitting aspect to a house
        /// Note: This does NOT account for good aspects, where relationship with house lord is checked
        /// TODO relationship aspect should be added get reference for it firsts
        /// </summary>
        public static bool IsBeneficPlanetAspectHouse(HouseName house, Time time)
        {
            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any good planet is aspecting the inputed house
            var goodFound = goodPlanets.FindAll(goodPlanet => IsHouseAspectedByPlanet(house, goodPlanet, time)).Any();

            return goodFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an evil planet
        /// </summary>
        public static bool IsPlanetAspectedByMaleficPlanets(PlanetName lord, Time time)
        {
            //get list of evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any of the evil planets is aspecting inputed planet
            var evilAspectFound = evilPlanets.FindAll(evilPlanet =>
                IsPlanetAspectedByPlanet(lord, evilPlanet, time)).Any();
            return evilAspectFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an benefic planet
        /// </summary>
        public static bool IsPlanetAspectedByBeneficPlanets(PlanetName lord, Time time)
        {
            //get list of benefic planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any of the benefic planets is aspecting inputed planet
            var goodAspectFound = goodPlanets.FindAll(goodPlanet =>
                IsPlanetAspectedByPlanet(lord, goodPlanet, time)).Any();

            return goodAspectFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an enemy planet based on combined relationship
        /// </summary>
        public static bool IsPlanetAspectedByEnemyPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets aspecting inputed planet
            var planetsAspecting = PlanetsAspectingPlanet(time, inputPlanet);

            //check if any aspecting planet is an enemy
            foreach (var planet in planetsAspecting)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isEnemy = aspectNature == PlanetToPlanetRelationship.Enemy ||
                              aspectNature == PlanetToPlanetRelationship.BitterEnemy;

                //if enemy than end here as true
                if (isEnemy) { return true; }

            }

            //if control reaches here than no enemy planet found
            return false;


        }

        /// <summary>
        /// Checks if a planet is receiving aspects from a Friend planet based on combined relationship
        /// </summary>
        public static bool IsPlanetAspectedByFriendPlanets(PlanetName inputPlanet, Time time)
        {
            //get all the planets aspecting inputed planet
            var planetsAspecting = PlanetsAspectingPlanet(time, inputPlanet);

            //check if any aspecting planet is an Friend
            foreach (var planet in planetsAspecting)
            {
                //get relationship of the 2 planets
                var aspectNature = PlanetCombinedRelationshipWithPlanet(inputPlanet, planet, time);
                var isFriend = aspectNature == PlanetToPlanetRelationship.Friend ||
                               aspectNature == PlanetToPlanetRelationship.BestFriend;

                //if Friend than end here as true
                if (isFriend) { return true; }

            }

            //if control reaches here than no Friend planet found
            return false;


        }

        /// <summary>
        /// Gets the Arudha Lagna Sign 
        /// 
        /// Reference Note:
        /// Arudha Lagna and planetary dispositions in reference to it have a strong bearing on the
        /// financial status of the person. In my own humble experience, Arudha Lagna should be given
        /// as much importance as the usual Janma Lagna. Arudha Lagna is the sign arrived at by counting
        /// as many signs from lord of Lagna as lord of Lagna is removed from Lagna.
        /// Thus if Aquarius is ascendant and its lord Saturn is in the 4th (Taurus)
        /// then the 4th from Taurus, viz., Leo becomes Arudha Lagna.
        /// </summary>
        public static ZodiacName ArudhaLagnaSign(Time time)
        {
            //get janma lagna
            var janmaLagna = HouseSignName(HouseName.House1, time);

            //get sign lord of janma lagna is in
            var lagnaLord = LordOfHouse(HouseName.House1, time);
            var lagnaLordSign = PlanetSignName(lagnaLord, time).GetSignName();

            //count the signs from janma to the sign the lord is in
            var janmaToLordCount = CountFromSignToSign(janmaLagna, lagnaLordSign);

            //use the above count to find arudha sign from lord's sign
            var arudhaSign = SignCountedFromInputSign(lagnaLordSign, janmaToLordCount);

            return arudhaSign;
        }

        /// <summary>
        /// Counts from start sign to end sign
        /// Example : Aquarius to Taurus is 4
        /// </summary>
        public static int CountFromSignToSign(ZodiacName startSign, ZodiacName endSign)
        {
            int count = 0;

            //get zodiac name & convert to its number equivalent
            var startSignNumber = (int)startSign;
            var endSignNumber = (int)endSign;

            //if start sign is more than end sign (meaning lower in the list)
            if (startSignNumber > endSignNumber)
            {
                //minus with 12, as though counting to the end
                int countToLastZodiac = (12 - startSignNumber) + 1; //plus 1 to count it self

                count = endSignNumber + countToLastZodiac;
            }
            else if (startSignNumber == endSignNumber)
            {
                count = 1;
            }
            //if start sign is lesser than end sign (meaning higher in the list)
            //we can minus like normal, and just add 1 to count it self
            else if (startSignNumber < endSignNumber)
            {
                count = (endSignNumber - startSignNumber) + 1; //plus 1 to count it self
            }

            return count;
        }

        /// <summary>
        /// Counts from start Constellation to end Constellation
        /// Example : Aquarius to Taurus is 4
        /// </summary>
        public static int CountFromConstellationToConstellation(PlanetConstellation start, PlanetConstellation end)
        {

            //get the number equivalent of the constellation
            int endConstellationNumber = end.GetConstellationNumber();

            int startConstellationNumber = start.GetConstellationNumber();

            int counter = 0;


            //Need to count from birthRulingConstellationNumber to dayRulingConstellationNumber

            //if start is more than end (meaning lower in the list)
            if (startConstellationNumber > endConstellationNumber)
            {
                //count from start to last constellation (27)
                int countToLastConstellation = (27 - startConstellationNumber) + 1; //plus 1 to count it self

                //to previous count add end constellation number
                counter = endConstellationNumber + countToLastConstellation;
            }
            else if (startConstellationNumber == endConstellationNumber)
            {
                counter = 1;
            }
            else if (startConstellationNumber < endConstellationNumber)
            {
                //if start sign is lesser than end sign (meaning higher in the list)
                //we can minus like normal, and just add 1 to count it self
                counter = (endConstellationNumber - startConstellationNumber) + 1; //plus 1 to count it self
            }


            return counter;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time 
        /// </summary>
        public static bool IsPlanetInHouse(Time time, PlanetName planet, HouseName houseNumber)
        {
            return HousePlanetIsIn(time, planet) == houseNumber;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time 
        /// </summary>
        public static bool IsAllPlanetInHouse(Time time, List<PlanetName> planetList, HouseName houseNumber)
        {
            //calculate each planet, even if 1 planet is out, then return as false
            foreach (var planetName in planetList)
            {
                var tempVal = IsPlanetInHouse(time, planetName, houseNumber);
                if (tempVal == false) { return false; }
            }

            //if control reaches here than all planets in house
            return true;
        }

        /// <summary>
        /// Checks if any planet in list is at a given house at a specified time 
        /// </summary>
        public static bool IsAnyPlanetInHouse(Time time, List<PlanetName> planetList, HouseName houseNumber)
        {
            //calculate each planet, even if 1 planet is out, then return as false
            foreach (var planetName in planetList)
            {
                var tempVal = IsPlanetInHouse(time, planetName, houseNumber);
                if (tempVal == true) { return true; }
            }

            // if control reaches here then no planet is in house
            return false;
        }

        /// <summary>
        /// Checks if a planet is in a longitude where it's in Debilitated
        /// Note : Rahu & ketu accounted for
        /// </summary>
        public static bool IsPlanetDebilitated(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(time, planet);

            //convert planet longitude to zodiac sign
            var planetZodiac = ZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Debilited
            var point = PlanetDebilitationPoint(planet);

            //check if planet is in Debilitation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var sameDegree = planetZodiac.GetDegreesInSign().Degrees == point.GetDegreesInSign().Degrees;
            var planetIsDebilitated = sameSign && sameDegree;

            return planetIsDebilitated;
        }

        /// <summary>
        /// Checks if a planet is in a longitude where it's in Exaltation
        ///
        /// NOTE:
        /// -   Rahu & ketu accounted for
        /// 
        /// -   Exaltation
        ///     Each planet is held to be exalted when it is
        ///     in a particular sign. The power to do good when in
        ///     exaltation is greater than when in its own sign.
        ///     Throughout the sign ascribed,
        ///     the planet is exalted but in a particular degree
        ///     its exaltation is at the maximum level.
        /// </summary>
        public static bool IsPlanetExalted(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(time, planet);

            //convert planet longitude to zodiac sign
            var planetZodiac = ZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Exaltation
            var point = PlanetExaltationPoint(planet);

            //check if planet is in Exaltation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var sameDegree = planetZodiac.GetDegreesInSign().Degrees == point.GetDegreesInSign().Degrees;
            var planetIsExaltation = sameSign && sameDegree;

            return planetIsExaltation;
        }

        /// <summary>
        /// Gets name of vedic month
        /// </summary>
        public static LunarMonth LunarMonth(Time time)
        {
            return Library.LunarMonth.Empty;

            //TODO NEEDS WORK
            throw new NotImplementedException();


            //get this months full moon date
            var fullMoonTime = getFullMoonTime();

            //sunrise
            var x = SunriseTime(time);
            var y = MoonConstellation(x).GetConstellationName();

        Calculate:
            //get the constellation behind the moon
            var constellation = MoonConstellation(fullMoonTime).GetConstellationName();

            //go back one constellation
            //constellation = constellation - 1;

            switch (constellation)
            {
                case ConstellationName.Aswini:
                    return Library.LunarMonth.Aswijam;
                    break;
                case ConstellationName.Bharani:
                    break;
                case ConstellationName.Krithika:
                    return Library.LunarMonth.Karthikam;
                    break;
                case ConstellationName.Rohini:
                    break;
                case ConstellationName.Mrigasira:
                case ConstellationName.Aridra:
                    return Library.LunarMonth.Margasiram;
                    break;
                case ConstellationName.Punarvasu:
                    break;
                case ConstellationName.Pushyami:
                    return Library.LunarMonth.Pooshiam;
                    break;
                case ConstellationName.Aslesha:
                    break;
                case ConstellationName.Makha:
                    return Library.LunarMonth.Magham;
                    break;
                case ConstellationName.Pubba:
                    return Library.LunarMonth.Phalgunam;
                    break;
                case ConstellationName.Uttara:
                    break;
                case ConstellationName.Hasta:
                    break;
                case ConstellationName.Chitta:
                    return Library.LunarMonth.Chitram;
                    break;
                case ConstellationName.Swathi:
                    break;
                case ConstellationName.Vishhaka:
                    return Library.LunarMonth.Visakham;
                    break;
                case ConstellationName.Anuradha:
                    break;
                case ConstellationName.Jyesta:
                    return Library.LunarMonth.Jaistam;
                    break;
                case ConstellationName.Moola:
                    break;
                case ConstellationName.Poorvashada:
                    return Library.LunarMonth.Ashadam;
                    break;
                case ConstellationName.Uttarashada:
                    break;
                case ConstellationName.Sravana:
                    return Library.LunarMonth.Sravanam;
                    break;
                case ConstellationName.Dhanishta:
                    break;
                case ConstellationName.Satabhisha:
                    break;
                case ConstellationName.Poorvabhadra:
                    return Library.LunarMonth.Bhadrapadam;
                case ConstellationName.Uttarabhadra:
                    break;
                case ConstellationName.Revathi:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            throw new ArgumentOutOfRangeException();
            //switch (constellation)
            //{
            //    case ConstellationName.Aswini:
            //        return LunarMonth.Aswijam;
            //        break;
            //    case ConstellationName.Krithika:
            //        return LunarMonth.Karthikam;
            //        break;
            //    case ConstellationName.Mrigasira:
            //        return LunarMonth.Margasiram;
            //        break;
            //    case ConstellationName.Pushyami:
            //        return LunarMonth.Pooshiam;
            //        break;
            //    case ConstellationName.Makha:
            //        return LunarMonth.Magham;
            //        break;
            //    case ConstellationName.Pubba:
            //        return LunarMonth.Phalgunam;
            //        break;
            //    case ConstellationName.Chitta:
            //        return LunarMonth.Chitram;
            //        break;
            //    case ConstellationName.Vishhaka:
            //        return LunarMonth.Visakham;
            //        break;
            //    case ConstellationName.Jyesta:
            //        return LunarMonth.Jaistam;
            //        break;
            //    case ConstellationName.Poorvashada:
            //        return LunarMonth.Ashadam;
            //        break;
            //    case ConstellationName.Sravana:
            //        return LunarMonth.Sravanam;
            //        break;
            //    case ConstellationName.Poorvabhadra:
            //        return LunarMonth.Bhadrapadam;
            //        break;
            //    default:
            //        fullMoonTime = fullMoonTime.AddHours(1);
            //        goto Calculate;
            //}





            //FUNCTIONS

            Time getFullMoonTime()
            {
                //get the lunar date number
                int lunarDayNumber = LunarDay(time).GetLunarDateNumber();

                //start with input time
                var fullMoonTime = time;

                //full moon not yet pass
                if (lunarDayNumber < 15)
                {
                    //go forward in time till find full moon
                    while (!IsFullMoon(fullMoonTime))
                    {
                        fullMoonTime = fullMoonTime.AddHours(1);
                    }

                    return fullMoonTime;
                }
                else
                {
                    //go backward in time till find full moon
                    while (!IsFullMoon(fullMoonTime))
                    {
                        fullMoonTime = fullMoonTime.SubtractHours(1);
                    }

                    return fullMoonTime;

                }

            }
        }

        /// <summary>
        /// Checks if the moon is FULL, moon day 15
        /// </summary>
        public static bool IsFullMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = LunarDay(time).GetLunarDayNumber();

            //if day 15, it is full moon
            return lunarDayNumber == 15;
        }

        /// <summary>
        /// Check if it is a Water / Aquatic sign
        /// Water Signs: this category include Cancer, Scorpio and Pisces.
        /// </summary>
        public static bool IsWaterSign(ZodiacName moonSign) => moonSign is ZodiacName.Cancer or ZodiacName.Scorpio or ZodiacName.Pisces;

        /// <summary>
        /// Check if it is a Fire sign
        /// Fire Signs: this sign encloses Aries, Leo and Sagittarius.
        /// </summary>
        public static bool IsFireSign(ZodiacName moonSign) => moonSign is ZodiacName.Aries or ZodiacName.Leo or ZodiacName.Sagittarius;

        /// <summary>
        /// Check if it is a Earth sign
        /// Earth Signs: it contains Taurus, Virgo and Capricorn.
        /// </summary>
        public static bool IsEarthSign(ZodiacName moonSign) => moonSign is ZodiacName.Taurus or ZodiacName.Virgo or ZodiacName.Capricorn;

        /// <summary>
        /// Check if it is a Air / Windy sign
        /// Air Signs: this sign include Gemini, Libra and Aquarius.
        /// </summary>
        public static bool IsAirSign(ZodiacName moonSign) => moonSign is ZodiacName.Gemini or ZodiacName.Libra or ZodiacName.Aquarius;

        /// <summary>
        /// WARNING! MARKED FOR DELETION : ERONEOUS RESULTS NOT SUITED FOR INTENDED PURPOSE
        /// METHOD NOT VERIFIED
        /// This methods perpose is to define the final good or bad
        /// nature of planet in antaram.
        ///
        /// For now only data from chapter "Key-planets for Each Sign"
        /// If this proves to be inacurate, add more checks in this method.
        /// - bindu points
        /// 
        /// Similar to method GetDasaInfoForAscendant
        /// Data from pg 80 of Key-planets for Each Sign in Hindu Predictive Astrology
        /// TODO meant to determine nature of antram
        /// </summary>
        public static EventNature PlanetAntaramNature(Person person, PlanetName planet)
        {
            //todo account for rahu & ketu
            //rahu & ketu not sure for now, just return neutral
            if (planet == Rahu || planet == Ketu) { return EventNature.Neutral; }

            //get nature from person's lagna
            var planetNature = GetNatureFromLagna();

            //if nature is neutral then use nature of relation to current house
            //assumed that bad relation to sign is bad planet (todo upgrade to bindu points)
            //note: generaly speaking a neutral planet shloud not exist, either good or bad
            if (planetNature == EventNature.Neutral)
            {
                var _planetCurrentHouse = HousePlanetIsIn(person.BirthTime, planet);

                var _currentHouseRelation = PlanetRelationshipWithHouse(_planetCurrentHouse, planet, person.BirthTime);

                switch (_currentHouseRelation)
                {
                    case PlanetToSignRelationship.BestFriendVarga:
                    case PlanetToSignRelationship.FriendVarga:
                    case PlanetToSignRelationship.OwnVarga:
                    case PlanetToSignRelationship.Moolatrikona:
                        return EventNature.Good;
                    case PlanetToSignRelationship.NeutralVarga:
                        return EventNature.Neutral;
                    case PlanetToSignRelationship.EnemyVarga:
                    case PlanetToSignRelationship.BitterEnemyVarga:
                        return EventNature.Bad;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            //else return nature from lagna
            return planetNature;


            //LOCAL FUNCTIONS

            EventNature GetNatureFromLagna()
            {
                var personLagna = HouseSignName(HouseName.House1, person.BirthTime);

                //get list of good and bad planets for a lagna
                dynamic planetData = GetPlanetData(personLagna);
                List<PlanetName> goodPlanets = planetData.Good;
                List<PlanetName> badPlanets = planetData.Bad;

                //check good planets first
                if (goodPlanets.Contains(planet))
                {
                    return EventNature.Good;
                }

                //check bad planets next
                if (badPlanets.Contains(planet))
                {
                    return EventNature.Bad;
                }

                //if control reaches here, then planet not
                //listed as good or bad, so just say neutral
                return EventNature.Neutral;
            }

            // data from chapter "Key-planets for Each Sign"
            object GetPlanetData(ZodiacName lagna)
            {
                List<PlanetName> good = null;
                List<PlanetName> bad = null;

                switch (lagna)
                {
                    //Aries - Saturn, Mercury and Venus are ill-disposed.
                    // Jupiter and the Sun are auspicious. The mere combination
                    // of Jupiler and Saturn produces no beneficial results. Jupiter
                    // is the Yogakaraka or the planet producing success. If Venus
                    // becomes a maraka, he will not kill the native but planets like
                    // Saturn will bring about death to the person.
                    case ZodiacName.Aries:
                        good = new List<PlanetName>() { Jupiter, Sun };
                        bad = new List<PlanetName>() { Saturn, Mercury, Venus };
                        break;
                    //Taurus - Saturn is the most auspicious and powerful
                    // planet. Jupiter, Venus and the Moon are evil planets. Saturn
                    // alone produces Rajayoga. The native will be killed in the
                    // periods and sub-periods of Jupiter, Venus and the Moon if
                    // they get death-inflicting powers.
                    case ZodiacName.Taurus:
                        good = new List<PlanetName>() { Saturn };
                        bad = new List<PlanetName>() { Jupiter, Venus, Moon };
                        break;
                    //Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                    // is most beneficial and in conjunction with Saturn in good signs
                    // produces and excellent career of much fame. Combination
                    // of Saturn and Jupiter produces similar results as in Aries.
                    // Venus and Mercury, when well associated, cause Rajayoga.
                    // The Moon will not kill the person even though possessed of
                    // death-inflicting powers.
                    case ZodiacName.Gemini:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Jupiter, Sun };
                        break;
                    //Cancer - Venus and Mercury are evil. Jupiter and Mars
                    // give beneficial results. Mars is the Rajayogakaraka
                    // (conferor of name and fame). The combination of Mars and Jupiter
                    // also causes Rajayoga (combination for political success). The
                    // Sun does not kill the person although possessed of maraka
                    // powers. Venus and other inauspicious planets kill the native.
                    // Mars in combination with the Moon or Jupiter in favourable
                    // houses especially the 1st, the 5th, the 9th and the 10th
                    // produces much reputation.
                    case ZodiacName.Cancer:
                        good = new List<PlanetName>() { Jupiter, Mars };
                        bad = new List<PlanetName>() { Venus, Mercury };
                        break;
                    //Leo - Mars is the most auspicious and favourable planet.
                    // The combination of Venus and Jupiter does not cause Rajayoga
                    // but the conjunction of Jupiter and Mars in favourable
                    // houses produce Rajayoga. Saturn, Venus and Mercury are
                    // evil. Saturn does not kill the native when he has the maraka
                    // power but Mercury and other evil planets inflict death when
                    // they get maraka powers.
                    case ZodiacName.Leo:
                        good = new List<PlanetName>() { Mars };
                        bad = new List<PlanetName>() { Saturn, Venus, Mercury };
                        break;
                    //Virgo - Venus alone is the most powerful. Mercury and
                    // Venus when combined together cause Rajayoga. Mars and
                    // the Moon are evil. The Sun does not kill the native even if
                    // be becomes a maraka but Venus, the Moon and Jupiter will
                    // inflict death when they are possessed of death-infticting power.
                    case ZodiacName.Virgo:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Moon };
                        break;
                    // Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                    // and Mars are inauspicious. Mercury and Saturn produce good.
                    // The conjunction of the Moon and Mercury produces Rajayoga.
                    // Mars himself will not kill the person. Jupiter, Venus
                    // and Mars when possessed of maraka powers certainly kill the
                    // nalive.
                    case ZodiacName.Libra:
                        good = new List<PlanetName>() { Saturn, Mercury };
                        bad = new List<PlanetName>() { Jupiter, Sun, Mars };
                        break;
                    //Scorpio - Jupiter is beneficial. The Sun and the Moon
                    // produce Rajayoga. Mercury and Venus are evil. Jupiter,
                    // even if be becomes a maraka, does not inflict death. Mercury
                    // and other evil planets, when they get death-inlflicting powers,
                    // do not certainly spare the native.
                    case ZodiacName.Scorpio:
                        good = new List<PlanetName>() { Jupiter };
                        bad = new List<PlanetName>() { Mercury, Venus };
                        break;
                    //Sagittarius - Mars is the best planet and in conjunction
                    // with Jupiter, produces much good. The Sun and Mars also
                    // produce good. Venus is evil. When the Sun and Mars
                    // combine together they produce Rajayoga. Saturn does not
                    // bring about death even when he is a maraka. But Venus
                    // causes death when be gets jurisdiction as a maraka planet.
                    case ZodiacName.Sagittarius:
                        good = new List<PlanetName>() { Mars };
                        bad = new List<PlanetName>() { Venus };
                        break;
                    //Capricorn - Venus is the most powerful planet and in
                    // conjunction with Mercury produces Rajayoga. Mars, Jupiter
                    // and the Moon are evil.
                    case ZodiacName.Capricorn:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Mars, Jupiter, Moon };
                        break;
                    //Aquarius - Venus alone is auspicious. The combination of
                    // Venus and Mars causes Rajayoga. Jupiter and the Moon are
                    // evil.
                    case ZodiacName.Aquarius:
                        good = new List<PlanetName>() { Venus };
                        bad = new List<PlanetName>() { Jupiter, Moon };
                        break;
                    //Pisces - The Moon and Mars are auspicious. Mars is
                    // most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                    // Saturn, Venus, the Sun and Mercury are evil. Mars
                    // himself does not kill the person even if he is a maraka.
                    case ZodiacName.Pisces:
                        good = new List<PlanetName>() { Moon, Mars };
                        bad = new List<PlanetName>() { Saturn, Venus, Sun, Mercury };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                return new { Good = good, Bad = bad };

            }
        }

        /// <summary>
        /// Soumyas
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetBeneficToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Sun || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Taurus:
                    return planetName == Sun || planetName == Mars
                                             || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Gemini:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Cancer:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Leo:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Virgo:
                    return planetName == Venus;
                case ZodiacName.Libra:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Scorpio:
                    return planetName == Jupiter || planetName == Sun || planetName == Moon;
                case ZodiacName.Sagittarius:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Capricorn:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Aquarius:
                    return planetName == Venus || planetName == Mars
                                               || planetName == Sun || planetName == Saturn;
                case ZodiacName.Pisces:
                    return planetName == Mars || planetName == Moon;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Kruras (Malefics)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetMaleficToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Venus || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Taurus:
                    return planetName == Moon || planetName == Jupiter || planetName == Venus;
                case ZodiacName.Gemini:
                    return planetName == Sun || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Cancer:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Leo:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Virgo:
                    return planetName == Mars || planetName == Moon || planetName == Jupiter;
                case ZodiacName.Libra:
                    return planetName == Sun || planetName == Moon || planetName == Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == Mercury || planetName == Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == Saturn || planetName == Venus || planetName == Mercury;
                case ZodiacName.Capricorn:
                    return planetName == Moon || planetName == Mars || planetName == Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == Jupiter || planetName == Moon;
                case ZodiacName.Pisces:
                    return planetName == Sun || planetName == Mercury
                                             || planetName == Venus || planetName == Saturn;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Yogakaraka (Planets indicating prosperity)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetYogakarakaToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Sun;
                case ZodiacName.Taurus:
                    return planetName == Saturn;
                case ZodiacName.Gemini:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Cancer:
                    return planetName == Mars;
                case ZodiacName.Leo:
                    return planetName == Mars;
                case ZodiacName.Virgo:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Libra:
                    return planetName == Moon || planetName == Mercury || planetName == Saturn;
                case ZodiacName.Scorpio:
                    return planetName == Sun || planetName == Moon;
                case ZodiacName.Sagittarius:
                    return planetName == Sun || planetName == Mars;
                case ZodiacName.Capricorn:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Aquarius:
                    return planetName == Mars || planetName == Venus;
                case ZodiacName.Pisces:
                    return planetName == Mars || planetName == Jupiter || planetName == Moon;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Yogakaraka (Planets indicating prosperity)
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetMarakaToLagna(PlanetName planetName, ZodiacName lagna)
        {
            switch (lagna)
            {
                case ZodiacName.Aries:
                    return planetName == Mercury || planetName == Saturn;
                case ZodiacName.Taurus:
                    return planetName == Jupiter || planetName == Venus;
                case ZodiacName.Gemini:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Cancer:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Leo:
                    return planetName == Mercury || planetName == Venus;
                case ZodiacName.Virgo:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Libra:
                    return planetName == Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == Venus || planetName == Saturn;
                case ZodiacName.Capricorn:
                    return planetName == Mars || planetName == Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == Mars;
                case ZodiacName.Pisces:
                    return planetName == Mercury || planetName == Venus || planetName == Saturn;
            }

            //control should not come here
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Checks if planet is placed in own house
        /// meaning house sign owned by planet
        /// note: rahu and ketu return false always
        /// </summary>
        public static bool IsPlanetInOwnHouse(Time time, PlanetName planetName)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetIsIn(time, planetName);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            if (_currentHouseRelation == PlanetToSignRelationship.OwnVarga)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// True if a planet is in a house sign owned by an enemy. Rahu and Ketu is false always
        /// </summary>
        public static bool IsPlanetInEnemyHouse(Time time, PlanetName planetName)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetIsIn(time, planetName);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            var isEnemy = _currentHouseRelation == PlanetToSignRelationship.EnemyVarga;
            var isSuperEnemy = _currentHouseRelation == PlanetToSignRelationship.BitterEnemyVarga;
            if (isEnemy || isSuperEnemy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// True if a planet is in a house sign owned by a friend. Rahu and Ketu is false always
        /// </summary>
        public static bool IsPlanetInFriendHouse(Time time, PlanetName planetName)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetIsIn(time, planetName);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //relation should be own
            var isEnemy = _currentHouseRelation == PlanetToSignRelationship.EnemyVarga;
            var isSuperEnemy = _currentHouseRelation == PlanetToSignRelationship.BitterEnemyVarga;
            if (isEnemy || isSuperEnemy)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get planet's Longitude, Latitude, DistanceAU, SpeedLongitude, SpeedLatitude...
        /// Swiss Ephemeris "swe_calc" wrapper for open API 
        /// </summary>
        public static dynamic SwissEphemeris(Time time, PlanetName planetName)
        {
            //convert planet name, compatible with Swiss Eph
            int swissPlanet = Tools.VedAstroToSwissEph(planetName);

            //do the calculation
            var sweCalcResults = Tools.ephemeris_swe_calc(time, swissPlanet);

            return sweCalcResults;
        }

        /// <summary>
        /// For all planets including Pluto, Neptune, Uranus
        /// Get planet's Longitude, Latitude, DistanceAU, SpeedLongitude, SpeedLatitude...
        /// Uses Swiss Ephemeris directly to get values
        /// </summary>
        public static List<dynamic> SwissEphemerisAll(Time time)
        {
            //for all planets
            var _12Planets = new List<int>
            {
                SwissEph.SE_SUN, SwissEph.SE_MOON, SwissEph.SE_MERCURY, SwissEph.SE_MARS,
                SwissEph.SE_VENUS, SwissEph.SE_JUPITER, SwissEph.SE_SATURN,
                SwissEph.SE_URANUS, SwissEph.SE_NEPTUNE, SwissEph.SE_PLUTO,
                //rahu & ketu
                SwissEph.SE_TRUE_NODE, SwissEph.SE_OSCU_APOG,
            };

            //put all data for all planets in 1 big list
            var bigList = new List<dynamic>();
            foreach (var planet in _12Planets)
            {
                var temp = Tools.ephemeris_swe_calc(time, planet);
                bigList.Add(temp);
            }

            return bigList;
        }

        /// <summary>
        /// Checks if a planet is same house (not nessarly conjunct) with the lord of a certain house
        /// Example : Is Sun joined with lord of 9th?
        /// </summary>
        public static bool IsPlanetSameHouseWithHouseLord(Time birthTime, int houseNumber, PlanetName planet)
        {
            //get house of the lord in question
            var houseLord = LordOfHouse((HouseName)houseNumber, birthTime);
            var houseLordHouse = HousePlanetIsIn(birthTime, houseLord);

            //get house of input planet
            var inputPlanetHouse = HousePlanetIsIn(birthTime, planet);

            //check if both are in same house
            if (inputPlanetHouse == houseLordHouse)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Based on Shadvarga get nature of house for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int HouseNatureScore(Time personBirthTime, HouseName inputHouse)
        {
            //if no house then no score
            if (inputHouse == HouseName.Empty)
            {
                return 0;
            }

            //get house score
            var houseStrength = HouseStrength(inputHouse, personBirthTime).ToDouble();

            //based on score determine final nature
            switch (houseStrength)
            {
                //positive
                case > 550: return 2; //extra for power
                case >= 450: return 1;

                //negative
                case < 250: return -3; //if below is even worse
                case < 350: return -2; //if below is even worse
                case < 450: return -1;
                default:
                    throw new Exception("No Strength Power defined!");
            }
        }

        /// <summary>
        /// Experimental Code
        /// </summary>
        public static double HouseNatureScoreMK4(Time personBirthTime, HouseName inputHouse)
        {
            //if no house then no score
            if (inputHouse == HouseName.Empty)
            {
                return 0;
            }

            //get house score
            var houseStrength = HouseStrength(inputHouse, personBirthTime).ToDouble();

            //weakest planet gives lowest score -2
            //strongest planet gives highest score 2
            //get range
            var highestHouseScore = HouseStrength(AllHousesOrderedByStrength(personBirthTime)[0], personBirthTime).ToDouble();
            var lowestHouseScore = HouseStrength(AllHousesOrderedByStrength(personBirthTime)[11], personBirthTime).ToDouble();

            var rangeBasedScore = houseStrength.Remap(lowestHouseScore, highestHouseScore, -3, 3);


            return rangeBasedScore;
        }

        /// <summary>
        /// Experimental Code, stand back!
        /// </summary>
        public static double PlanetNatureScoreMK4(Time personBirthTime, PlanetName inputPlanet)
        {
            //if no house then no score
            if (inputPlanet == PlanetName.Empty) { return 0; }

            //get house score
            //var planetStrength = GetPlanetShadbalaPinda(inputPlanet, personBirthTime).ToDouble();

            //weakest planet gives lowest score -2
            //strongest planet gives highest score 2
            //get range
            //var highestPlanetScore = GetPlanetShadbalaPinda(GetAllPlanetOrderedByStrength(personBirthTime)[0], personBirthTime).ToDouble();
            //var weakestPlanet = GetAllPlanetOrderedByStrength(personBirthTime)[8];
            //var lowestPlanetScore = GetPlanetShadbalaPinda(weakestPlanet, personBirthTime).ToDouble();

            //find accurate planet strength relative to others
            //if above limit than strong else weak below 0
            var isBenefic = IsPlanetBeneficInShadbala(inputPlanet, personBirthTime);
            //var rangeBasedScore = 0.0;

            var x = isBenefic ? 1 : -1;

            return x;

            //if (isBenefic) //positive number
            //{
            //     rangeBasedScore = planetStrength.Remap(lowestPlanetScore, highestPlanetScore, 0, 2);

            //}
            //else // 0 or below
            //{
            //     rangeBasedScore = planetStrength.Remap(lowestPlanetScore, highestPlanetScore, -2, 0);
            //}

            //return rangeBasedScore;
        }

        /// <summary>
        /// Based on Shadvarga get nature of planet for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int PlanetNatureScore(Time personBirthTime, PlanetName inputPlanet)
        {
            //get house score
            var planetStrength = PlanetShadbalaPinda(inputPlanet, personBirthTime).ToDouble();


            //based on score determine final nature
            switch (planetStrength)
            {
                //positive
                case > 550: return 2; //extra for power
                case >= 450: return 1;

                //negative
                case < 250: return -3; //if below is even worse
                case < 350: return -2; //if below is even worse
                case < 450: return -1;
                default:
                    throw new Exception("No Strength Power defined!");
            }
        }

        /// <summary>
        /// Get a person's varna or color (character)
        /// A person's varna can be observed in real life
        /// </summary>
        public static Varna BirthVarna(Time birthTime)
        {
            //get ruling sign
            var ruleSign = PlanetSignName(Moon, birthTime).GetSignName();

            //get grade
            var maleGrade = GetGrade(ruleSign);

            return maleGrade;

            //higher grade is higher class
            Varna GetGrade(ZodiacName sign)
            {
                switch (sign)
                {   //Pisces, Scorpio and Cancer represent the highest development - Brahmin 
                    case ZodiacName.Pisces:
                    case ZodiacName.Scorpio:
                    case ZodiacName.Cancer:
                        return Varna.BrahminScholar;

                    //Leo, Sagittarius and Libra indicate the second grade - or Kshatriya;
                    case ZodiacName.Leo:
                    case ZodiacName.Sagittarius:
                    case ZodiacName.Libra:
                        return Varna.KshatriyaWarrior;

                    //Aries, Gemini and Aquarius suggest the third or the Vaisya;
                    case ZodiacName.Aries:
                    case ZodiacName.Gemini:
                    case ZodiacName.Aquarius:
                        return Varna.VaisyaWorkmen;

                    //while Taurus, Virgo and Capricorn indicate the last grade, viz., Sudra
                    case ZodiacName.Taurus:
                    case ZodiacName.Virgo:
                    case ZodiacName.Capricorn:
                        return Varna.SudraServant;

                    default: throw new Exception("");
                }
            }


        }

        /// <summary>
        /// Used for judging dasa good or bad, Bala book pg 110
        /// if planet has more Ishta than good = +1
        /// else if more Kashta than bad = -1
        /// </summary>
        public static double PlanetIshtaKashtaScore(PlanetName planet, Time birthTime)
        {
            var ishtaScore = PlanetIshtaScore(planet, birthTime);

            var kashtaScore = PlanetKashtaScore(planet, birthTime);

            //if more than good, else bad
            var ishtaMore = ishtaScore > kashtaScore;

            return ishtaMore ? 1 : -1;
        }

        /// <summary>
        /// Experimental Code, stand back!
        /// Kashta Phala (Bad Strength) of a Planet
        /// </summary>

        public static double PlanetKashtaScore(PlanetName planet, Time birthTime)
        {
            return 0;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ishta Phala (Good Strength) of a Planet
        /// </summary>
        public static double PlanetIshtaScore(PlanetName planet, Time birthTime)
        {
            //The Ochcha Bala (exaltation strength) of a planet
            //is multiplied by its Chesta Bala(motional strength)
            //and then the square root of the product extracted.
            var ochchaBala = PlanetOchchaBala(planet, birthTime).ToDouble();
            var chestaBala = PlanetChestaBala(planet, birthTime, includeSunMoon: true).ToDouble();
            var product = ochchaBala * chestaBala;

            //Square root of the product extracted.
            //the result would represent the Ishta Phala.
            var ishtaScore = Math.Sqrt(product);

            return ishtaScore;
        }

        /// <summary>
        /// Gets all planets in certain sign from the moon. Exp: get planets 3rd from the moon
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromMoon(int signsFromMoon, Time birthTime)
        {
            //get the sign to check
            var moonNthSign = SignCountedFromMoonSign(signsFromMoon, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(moonNthSign, birthTime);

            return planetsIn;
        }


        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromLagna(int signsFromLagna, Time birthTime)
        {
            //get the sign to check
            var lagnaNthSign = SignCountedFromLagnaSign(signsFromLagna, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(lagnaNthSign, birthTime);

            return planetsIn;
        }


        /// <summary>
        /// Gets all planets in certain sign from the moon, given list of signs. Exp: get planets 3rd from the moon
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromMoon(int[] signsFromList, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromMoon(sigsFrom, birthTime);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }

        /// <summary>
        /// Gets all planets in certain sign from a given planet, given list of signs. Exp: get planets 3rd from the Jupiter
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int[] signsFromList, Time birthTime, PlanetName startPlanet)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromPlanet(sigsFrom, birthTime, startPlanet);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }

        /// <summary>
        /// Gets all planets in certain sign from the planet. Exp: get planets 3rd from the Jupiter
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int signsFromMoon, Time birthTime, PlanetName startPlanet)
        {
            //get the sign to check
            var moonNthSign = SignCountedFromPlanetSign(signsFromMoon, birthTime, startPlanet);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(moonNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant, given list of signs. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromLagna(int[] signsFromList, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromLagna(sigsFrom, birthTime);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

        }



        /// <summary>
        /// Checks if a given list of planets are found in any inputed signs from moon
        /// Exp: Is Sun or Moon in 6 or 7th from Moon
        /// </summary>
        public static bool IsPlanetsInSignsFromMoon(int[] signsFromList, PlanetName[] planetList, Time birthTime)
        {
            //get all planets in given list of signs from moon
            var planetsFromMoon = AllPlanetsSignsFromMoon(signsFromList, birthTime);

            var isOccuring = false; //default to false

            //if planet is found will be set by checks below and retured as occuring
            foreach (var planet in planetsFromMoon)
            {
                //check given list if contains planets 
                var isFound = planetList.Contains(planet);
                if (isFound)
                {
                    isOccuring = true;
                    break; //stop looking
                }
            }

            return isOccuring;
        }

        /// <summary>
        /// Checks if a given list of planets are found in any inputed signs from another planet
        /// Exp: Is Sun or Moon in 6 or 7th from Moon
        /// </summary>
        public static bool IsPlanetsInSignsFromPlanet(int[] signsFromList, PlanetName[] planetList, PlanetName startPlanet, Time birthTime)
        {
            //get all planets in given list of signs from planet
            var planetsFromPlanet = AllPlanetsSignsFromPlanet(signsFromList, birthTime, startPlanet);

            var isOccuring = false; //default to false

            //if planet is found will be set by checks below and retured as occuring
            foreach (var planet in planetsFromPlanet)
            {
                //check given list if contains planets 
                var isFound = planetList.Contains(planet);
                if (isFound)
                {
                    isOccuring = true;
                    break; //stop looking
                }
            }

            return isOccuring;
        }



        /// <summary>
        /// Checks if a given list of planets are found in any inputed signs from Lagna/Ascendant
        /// Exp: Is Sun or Moon in 6 or 7th from Lagna
        /// </summary>
        public static bool IsPlanetsInSignsFromLagna(int[] signsFromList, PlanetName[] planetList, Time birthTime)
        {
            //get all planets in given list of signs from Lagna
            var planetsFromLagna = AllPlanetsSignsFromLagna(signsFromList, birthTime);

            var isOccuring = false; //default to false

            //if planet is found will be set by checks below and retured as occuring
            foreach (var planet in planetsFromLagna)
            {
                //check given list if contains planets 
                var isFound = planetList.Contains(planet);
                if (isFound)
                {
                    isOccuring = true;
                    break; //stop looking
                }
            }

            return isOccuring;
        }

        /// <summary>
        /// Checks if benefics are found in any inputed signs from moon
        /// Exp: Is benefics in 6 & 7th from moon
        /// </summary>
        public static bool IsBeneficsInSignsFromMoon(int[] signsFromList, Time birthTime)
        {
            //get all planets that are standard benefics at given time
            var beneficList = BeneficPlanetList(birthTime).ToArray();

            //get all planets in given list of signs from moon
            var isOccuring = IsPlanetsInSignsFromMoon(signsFromList, beneficList, birthTime);

            return isOccuring;
        }

        /// <summary>
        /// Checks if benefics are found in any inputed signs from Lagna/Ascendant
        /// Exp: Is benefics in 6 & 7th from moon
        /// </summary>
        public static bool IsBeneficsInSignsFromLagna(int[] signsFromList, Time birthTime)
        {
            //get all planets that are standard benefics at given time
            var beneficList = BeneficPlanetList(birthTime).ToArray();

            //get all planets in given list of signs from lagna
            var isOccuring = IsPlanetsInSignsFromLagna(signsFromList, beneficList, birthTime);

            return isOccuring;
        }

        #endregion

        #region CACHED FUNCTIONS
        //NOTE : These are functions that don't call other functions from this class
        //       Only functions that don't call other cached functions are allowed to be cached
        //       otherwise, it's erroneous in parallel


        /// <summary>
        /// The distance between the Hindu First Point and the Vernal Equinox, measured at an epoch, is known as the Ayanamsa
        /// in Varahamihira's time, the summer solistice coincided with the first degree of Cancer,
        /// and the winter solistice with the first degree of Capricorn, whereas at one time the summer solistice coincided with the
        /// middle of the Aslesha
        /// </summary>
        public static Angle AyanamsaDegree(Time time)
        {

            //it has been observed and proved mathematically, that each year at the time when the Sun reaches his
            //equinoctial point of Aries 0° when throughout the earth, the day and night are equal in length,
            //the position of the earth in reference to some fixed star is nearly 50.333 of space farther west
            //than the earth was at the same equinoctial moment of the previous year.


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AyanamsaDegree), time, Ayanamsa), _getAyanamsaDegree);


            //UNDERLYING FUNCTION
            Angle _getAyanamsaDegree()
            {
                //This would request sidereal positions calculated using the Swiss Ephemeris.
                int iflag = SwissEph.SEFLG_SIDEREAL;
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //set ayanamsa
                ephemeris.swe_set_sid_mode(Ayanamsa, 0, 0);
                var ayanamsaDegree = ephemeris.swe_get_ayanamsa(jul_day_ET);

                return Angle.FromDegrees(ayanamsaDegree);

            }

        }

        /// <summary>
        /// Get fixed longitude used in western systems, connects SwissEph Library with VedAstro
        /// NOTE This method connects SwissEph Library with VedAstro Library
        /// </summary>
        public static Angle PlanetSayanaLongitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSayanaLongitude), time, planetName, Ayanamsa), _getPlanetSayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLongitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);



                //data in results at index 0 is longitude
                var planetSayanaLongitude = new Angle(degrees: results[0]);

                //if ketu add 180 to rahu
                if (planetName == Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;

            }


        }

        /// <summary>
        /// Planet longitude that has been corrected with Ayanamsa
        /// Gets planet longitude used vedic astrology
        /// Nirayana Longitude = Sayana Longitude corrected to Ayanamsa
        /// Number from 0 to 360, represent the degrees in the zodiac as viewed from earth
        /// Note: Since Nirayana is corrected, in actuality 0 degrees will start at Taurus not Aries
        /// </summary>
        public static Angle PlanetNirayanaLongitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetNirayanaLongitude), time, planetName, Ayanamsa), _getPlanetNirayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetNirayanaLongitude()
            {

                //This would request sidereal positions calculated using the Swiss Ephemeris.
                int iflag = SwissEph.SEFLG_SIDEREAL | SwissEph.SEFLG_SWIEPH; // SEFLG_SIDEREAL | ; //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //NOTE Ayanamsa needs to be set before caling calc
                ephemeris.swe_set_sid_mode(Ayanamsa, 0, 0);

                //do calculation
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

                //data in results at index 0 is longitude
                var planetSayanaLongitude = new Angle(degrees: results[0]);

                //if ketu add 180 to rahu
                if (planetName == Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;

            }


        }

        /// <summary>
        /// find time of next lunar eclipse UTC time
        /// </summary>
        public static DateTime NextLunarEclipse(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextLunarEclipse), time, Ayanamsa), _getNextLunarEclipse);


            //UNDERLYING FUNCTION

            DateTime _getNextLunarEclipse()
            {
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[10];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = Calculate.ConvertLmtToJulian(time);

                //Get planet long
                var eclipseType = 0; /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
                var backward = false; /* TRUE, if backward search */
                int ret_flag = ephemeris.swe_lun_eclipse_when(jul_day_ET, iflag, eclipseType, results, backward, ref err_msg);

                //get raw results out
                var eclipseMaxTime = results[0]; //time of maximum eclipse (Julian day number)

                //convert to UTC Time
                var utcTime = Calculate.ConvertJulianTimeToNormalTime(eclipseMaxTime);

                return utcTime;

            }


        }

        /// <summary>
        /// finds the next solar eclipse globally UTC time
        /// </summary>
        public static DateTime NextSolarEclipse(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextSolarEclipse), time, Ayanamsa), _getNextSolarEclipse);


            //UNDERLYING FUNCTION

            DateTime _getNextSolarEclipse()
            {
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[10];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = Calculate.ConvertLmtToJulian(time);

                //Get planet long
                var eclipseType = 0; /* eclipse type wanted: SE_ECL_TOTAL etc. or 0, if any eclipse type */
                var backward = false; /* TRUE, if backward search */
                int ret_flag = ephemeris.swe_sol_eclipse_when_glob(jul_day_ET, iflag, eclipseType, results, backward, ref err_msg);

                //get raw results out
                var eclipseMaxTime = results[0]; //time of maximum eclipse (Julian day number)

                //convert to UTC Time
                var utcTime = Calculate.ConvertJulianTimeToNormalTime(eclipseMaxTime);

                return utcTime;

            }


        }

        /// <summary>
        /// Get fixed longitude used in western systems aka Sayana longitude
        /// NOTE This method connects SwissEph Library with VedAstro Library
        /// </summary>
        public static Angle PlanetEphemerisLongitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetEphemerisLongitude), time, planetName, Ayanamsa), _getPlanetSayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLongitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);

                //convert planet name, compatible with Swiss Eph
                int swissPlanet = Tools.VedAstroToSwissEph(planetName);

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

                //data in results at index 0 is longitude
                var planetSayanaLongitude = new Angle(degrees: results[0]);

                //if ketu add 180 to rahu
                if (planetName == Library.PlanetName.Ketu)
                {
                    var x = planetSayanaLongitude + Angle.Degrees180;
                    planetSayanaLongitude = x.Expunge360();
                }

                return planetSayanaLongitude;
            }


        }

        /// <summary>
        /// Gets Swiss Ephemeris longitude for a planet
        /// </summary>
        public static Angle PlanetSayanaLatitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSayanaLatitude), time, planetName, Ayanamsa), _getPlanetSayanaLatitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLatitude()
            {
                //Converts LMT to UTC (GMT)
                //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

                int planet = 0;
                int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
                double[] results = new double[6];
                string err_msg = "";
                double jul_day_ET;
                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                jul_day_ET = TimeToEphemerisTime(time);


                //Convert PlanetName to SE_PLANET type
                if (planetName == Library.PlanetName.Sun)
                    planet = SwissEph.SE_SUN;
                else if (planetName == Library.PlanetName.Moon)
                {
                    planet = SwissEph.SE_MOON;
                }
                else if (planetName == Library.PlanetName.Mars)
                {
                    planet = SwissEph.SE_MARS;
                }
                else if (planetName == Library.PlanetName.Mercury)
                {
                    planet = SwissEph.SE_MERCURY;
                }
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    planet = SwissEph.SE_JUPITER;
                }
                else if (planetName == Library.PlanetName.Venus)
                {
                    planet = SwissEph.SE_VENUS;
                }
                else if (planetName == Library.PlanetName.Saturn)
                {
                    planet = SwissEph.SE_SATURN;
                }
                else if (planetName == Library.PlanetName.Rahu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }
                else if (planetName == Library.PlanetName.Ketu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

                //data in results at index 1 is latitude
                return new Angle(degrees: results[1]);

            }


        }

        /// <summary>
        /// Speed of planet from Swiss eph
        /// </summary>
        public static double PlanetSpeed(Time time, PlanetName planetName)
        {
            //Converts LMT to UTC (GMT)
            //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

            int planet = 0;
            int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;
            double[] results = new double[6];
            string err_msg = "";
            double jul_day_ET;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_ET = TimeToEphemerisTime(time);


            //Convert PlanetName to SE_PLANET type
            if (planetName == Library.PlanetName.Sun)
                planet = SwissEph.SE_SUN;
            else if (planetName == Library.PlanetName.Moon)
            {
                planet = SwissEph.SE_MOON;
            }
            else if (planetName == Library.PlanetName.Mars)
            {
                planet = SwissEph.SE_MARS;
            }
            else if (planetName == Library.PlanetName.Mercury)
            {
                planet = SwissEph.SE_MERCURY;
            }
            else if (planetName == Library.PlanetName.Jupiter)
            {
                planet = SwissEph.SE_JUPITER;
            }
            else if (planetName == Library.PlanetName.Venus)
            {
                planet = SwissEph.SE_VENUS;
            }
            else if (planetName == Library.PlanetName.Saturn)
            {
                planet = SwissEph.SE_SATURN;
            }
            else if (planetName == Library.PlanetName.Rahu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }
            else if (planetName == Library.PlanetName.Ketu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

            //data in results at index 3 is speed in right ascension (deg/day)
            return results[3];
        }

        /// <summary>
        /// Converts Planet Longitude to Constellation equivelant
        /// Gets info about the constellation at a given longitude, ie. Constellation Name,
        /// Quarter, Degrees in constellation, etc.
        /// </summary>
        public static PlanetConstellation ConstellationAtLongitude(Angle planetLongitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConstellationAtLongitude), planetLongitude, Ayanamsa), _constellationAtLongitude);


            //UNDERLYING FUNCTION
            PlanetConstellation _constellationAtLongitude()
            {
                if (planetLongitude == null) { return Library.PlanetConstellation.Empty; }

                //if planet longitude is negative means, it before aries at 0, starts back at 360 pieces
                if (planetLongitude.TotalDegrees < 0)
                {
                    planetLongitude = Angle.FromDegrees(360.0 + planetLongitude.TotalDegrees); //use plus because number is already negative
                }

                //get planet's longitude in minutes
                var planetLongitudeInMinutes = planetLongitude.TotalMinutes;

                //The ecliptic is divided into 27 constellations
                //of 13° 20' (800') each. Hence divide 800
                var roughConstellationNumber = planetLongitudeInMinutes / 800.0;

                //get constellation number (rounds up)
                var constellationNumber = (int)Math.Ceiling(roughConstellationNumber);

                //calculate quarter from remainder
                int quarter;

                var remainder = roughConstellationNumber - Math.Floor(roughConstellationNumber);

                if (remainder >= 0 && remainder <= 0.25) quarter = 1;
                else if (remainder > 0.25 && remainder <= 0.5) quarter = 2;
                else if (remainder > 0.5 && remainder <= 0.75) quarter = 3;
                else if (remainder > 0.75 && remainder <= 1) quarter = 4;
                else quarter = 0;

                //calculate "degrees in constellation" from the remainder
                var minutesInConstellation = remainder * 800.0;
                var degreesInConstellation = new Angle(0, minutesInConstellation, 0);


                //put together all the info of this point in the constellation
                var constellation = new PlanetConstellation(constellationNumber, quarter, degreesInConstellation);

                //return constellation value
                return constellation;
            }

        }


        /// <summary>
        /// Converts Planet Longitude to Zodiac Sign equivalent
        /// </summary>
        public static ZodiacSign ZodiacSignAtLongitude(Angle longitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ZodiacSignAtLongitude), longitude, Ayanamsa), _zodiacSignAtLongitude);


            //UNDERLYING FUNCTION
            ZodiacSign _zodiacSignAtLongitude()
            {
                //max degrees of each sign
                const double maxDegreesInSign = 30.0;

                //get rough zodiac number
                double roughZodiacNumber = (longitude.TotalDegrees % 360.0) / maxDegreesInSign;

                //Calculate degrees in zodiac sign
                //get remainder from rough zodiac number
                var roughZodiacNumberRemainder = roughZodiacNumber - Math.Truncate(roughZodiacNumber);

                //convert remainder to degrees in current sign
                var degreesInSignRaw = roughZodiacNumberRemainder * maxDegreesInSign;
                //round number (too high accuracy causes equality mismtach because of minute difference)
                var degreesInSign = Math.Round(degreesInSignRaw, 7);

                //if degrees in sign is 0, it means 30 degrees
                if (degreesInSign == 0)
                {
                    //change value to 30 degrees
                    degreesInSign = 30;
                }

                //Get name of zodiac sign
                //round to ceiling to get integer zodiac number
                var zodiacNumber = (int)Math.Ceiling(roughZodiacNumber);

                //convert zodiac number to zodiac name
                var calculatedZodiac = (ZodiacName)zodiacNumber;

                //if rough zodiac number is less than or equal 0, then return Pisces else return calculated zodiac
                ZodiacName currentSignName = (roughZodiacNumber <= 0) ? ZodiacName.Pisces : calculatedZodiac;

                //return new instance of planet sign
                var degreesAngle = Angle.FromDegrees(Math.Abs(degreesInSign)); //make always positive

                var zodiacSignAtLongitude = new ZodiacSign(currentSignName, degreesAngle);

                return zodiacSignAtLongitude;
            }


        }

        /// <summary>
        /// Converts Zodiac Sign to Planet Longitude equivalent
        /// </summary>
        public static Angle LongitudeAtZodiacSign(ZodiacSign zodiacSign)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LongitudeAtZodiacSign), zodiacSign, Ayanamsa), _getLongitudeAtZodiacSign);


            //UNDERLYING FUNCTION
            Angle _getLongitudeAtZodiacSign()
            {
                //convert zodic name to its number equivelant in order
                var zodiacNumber = (int)zodiacSign.GetSignName();

                //calculate planet longitude to sign just before
                var zodiacBefore = zodiacNumber - 1;
                var maxDegreesInSign = 30.0;
                var longtiudeToBefore = Angle.FromDegrees(maxDegreesInSign * zodiacBefore);

                //add planet longitude from sign just before with
                //degrees already traversed in current sign
                var totalLongitude = longtiudeToBefore + zodiacSign.GetDegreesInSign();

                return totalLongitude;
            }


        }

        #region Longitude Converters



        #endregion

        /// <summary>
        /// Get Day Of Week
        /// </summary>
        public static DayOfWeek DayOfWeek(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(DayOfWeek), time, Ayanamsa), _getDayOfWeek);


            //UNDERLYING FUNCTION
            DayOfWeek _getDayOfWeek()
            {
                //The Hindu day begins with sunrise and continues till
                //next sunrise.The first hora on any day will be the
                //first hour after sunrise and the last hora, the hour
                //before sunrise the next day.

                //TODO Change to new day system
                //TODO make test first

                var sunRise = Calculate.SunriseTime(time);

                //get week day name in string
                var dayOfWeekNameInString = time.GetLmtDateTimeOffset().DayOfWeek.ToString();

                //convert string to day of week type
                Enum.TryParse(dayOfWeekNameInString, out DayOfWeek dayOfWeek);

                //return to caller
                return dayOfWeek;
            }


        }

        /// <summary>
        /// Gets hora lord based on hora number & week day
        /// </summary>
        public static PlanetName LordOfHora(int hora, DayOfWeek day)
        {
            switch (day)
            {
                case Library.DayOfWeek.Sunday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Sun;
                        case 2: return Library.PlanetName.Venus;
                        case 3: return Library.PlanetName.Mercury;
                        case 4: return Library.PlanetName.Moon;
                        case 5: return Library.PlanetName.Saturn;
                        case 6: return Library.PlanetName.Jupiter;
                        case 7: return Library.PlanetName.Mars;
                        case 8: return Library.PlanetName.Sun;
                        case 9: return Library.PlanetName.Venus;
                        case 10: return Library.PlanetName.Mercury;
                        case 11: return Library.PlanetName.Moon;
                        case 12: return Library.PlanetName.Saturn;
                        case 13: return Library.PlanetName.Jupiter;
                        case 14: return Library.PlanetName.Mars;
                        case 15: return Library.PlanetName.Sun;
                        case 16: return Library.PlanetName.Venus;
                        case 17: return Library.PlanetName.Mercury;
                        case 18: return Library.PlanetName.Moon;
                        case 19: return Library.PlanetName.Saturn;
                        case 20: return Library.PlanetName.Jupiter;
                        case 21: return Library.PlanetName.Mars;
                        case 22: return Library.PlanetName.Sun;
                        case 23: return Library.PlanetName.Venus;
                        case 24: return Library.PlanetName.Mercury;
                    }
                    break;
                case Library.DayOfWeek.Monday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Moon;
                        case 2: return Library.PlanetName.Saturn;
                        case 3: return Library.PlanetName.Jupiter;
                        case 4: return Library.PlanetName.Mars;
                        case 5: return Library.PlanetName.Sun;
                        case 6: return Library.PlanetName.Venus;
                        case 7: return Library.PlanetName.Mercury;
                        case 8: return Library.PlanetName.Moon;
                        case 9: return Library.PlanetName.Saturn;
                        case 10: return Library.PlanetName.Jupiter;
                        case 11: return Library.PlanetName.Mars;
                        case 12: return Library.PlanetName.Sun;
                        case 13: return Library.PlanetName.Venus;
                        case 14: return Library.PlanetName.Mercury;
                        case 15: return Library.PlanetName.Moon;
                        case 16: return Library.PlanetName.Saturn;
                        case 17: return Library.PlanetName.Jupiter;
                        case 18: return Library.PlanetName.Mars;
                        case 19: return Library.PlanetName.Sun;
                        case 20: return Library.PlanetName.Venus;
                        case 21: return Library.PlanetName.Mercury;
                        case 22: return Library.PlanetName.Moon;
                        case 23: return Library.PlanetName.Saturn;
                        case 24: return Library.PlanetName.Jupiter;
                    }
                    break;
                case Library.DayOfWeek.Tuesday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Mars;
                        case 2: return Library.PlanetName.Sun;
                        case 3: return Library.PlanetName.Venus;
                        case 4: return Library.PlanetName.Mercury;
                        case 5: return Library.PlanetName.Moon;
                        case 6: return Library.PlanetName.Saturn;
                        case 7: return Library.PlanetName.Jupiter;
                        case 8: return Library.PlanetName.Mars;
                        case 9: return Library.PlanetName.Sun;
                        case 10: return Library.PlanetName.Venus;
                        case 11: return Library.PlanetName.Mercury;
                        case 12: return Library.PlanetName.Moon;
                        case 13: return Library.PlanetName.Saturn;
                        case 14: return Library.PlanetName.Jupiter;
                        case 15: return Library.PlanetName.Mars;
                        case 16: return Library.PlanetName.Sun;
                        case 17: return Library.PlanetName.Venus;
                        case 18: return Library.PlanetName.Mercury;
                        case 19: return Library.PlanetName.Moon;
                        case 20: return Library.PlanetName.Saturn;
                        case 21: return Library.PlanetName.Jupiter;
                        case 22: return Library.PlanetName.Mars;
                        case 23: return Library.PlanetName.Sun;
                        case 24: return Library.PlanetName.Venus;
                    }
                    break;
                case Library.DayOfWeek.Wednesday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Mercury;
                        case 2: return Library.PlanetName.Moon;
                        case 3: return Library.PlanetName.Saturn;
                        case 4: return Library.PlanetName.Jupiter;
                        case 5: return Library.PlanetName.Mars;
                        case 6: return Library.PlanetName.Sun;
                        case 7: return Library.PlanetName.Venus;
                        case 8: return Library.PlanetName.Mercury;
                        case 9: return Library.PlanetName.Moon;
                        case 10: return Library.PlanetName.Saturn;
                        case 11: return Library.PlanetName.Jupiter;
                        case 12: return Library.PlanetName.Mars;
                        case 13: return Library.PlanetName.Sun;
                        case 14: return Library.PlanetName.Venus;
                        case 15: return Library.PlanetName.Mercury;
                        case 16: return Library.PlanetName.Moon;
                        case 17: return Library.PlanetName.Saturn;
                        case 18: return Library.PlanetName.Jupiter;
                        case 19: return Library.PlanetName.Mars;
                        case 20: return Library.PlanetName.Sun;
                        case 21: return Library.PlanetName.Venus;
                        case 22: return Library.PlanetName.Mercury;
                        case 23: return Library.PlanetName.Moon;
                        case 24: return Library.PlanetName.Saturn;
                    }
                    break;
                case Library.DayOfWeek.Thursday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Jupiter;
                        case 2: return Library.PlanetName.Mars;
                        case 3: return Library.PlanetName.Sun;
                        case 4: return Library.PlanetName.Venus;
                        case 5: return Library.PlanetName.Mercury;
                        case 6: return Library.PlanetName.Moon;
                        case 7: return Library.PlanetName.Saturn;
                        case 8: return Library.PlanetName.Jupiter;
                        case 9: return Library.PlanetName.Mars;
                        case 10: return Library.PlanetName.Sun;
                        case 11: return Library.PlanetName.Venus;
                        case 12: return Library.PlanetName.Mercury;
                        case 13: return Library.PlanetName.Moon;
                        case 14: return Library.PlanetName.Saturn;
                        case 15: return Library.PlanetName.Jupiter;
                        case 16: return Library.PlanetName.Mars;
                        case 17: return Library.PlanetName.Sun;
                        case 18: return Library.PlanetName.Venus;
                        case 19: return Library.PlanetName.Mercury;
                        case 20: return Library.PlanetName.Moon;
                        case 21: return Library.PlanetName.Saturn;
                        case 22: return Library.PlanetName.Jupiter;
                        case 23: return Library.PlanetName.Mars;
                        case 24: return Library.PlanetName.Sun;
                    }
                    break;
                case Library.DayOfWeek.Friday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Venus;
                        case 2: return Library.PlanetName.Mercury;
                        case 3: return Library.PlanetName.Moon;
                        case 4: return Library.PlanetName.Saturn;
                        case 5: return Library.PlanetName.Jupiter;
                        case 6: return Library.PlanetName.Mars;
                        case 7: return Library.PlanetName.Sun;
                        case 8: return Library.PlanetName.Venus;
                        case 9: return Library.PlanetName.Mercury;
                        case 10: return Library.PlanetName.Moon;
                        case 11: return Library.PlanetName.Saturn;
                        case 12: return Library.PlanetName.Jupiter;
                        case 13: return Library.PlanetName.Mars;
                        case 14: return Library.PlanetName.Sun;
                        case 15: return Library.PlanetName.Venus;
                        case 16: return Library.PlanetName.Mercury;
                        case 17: return Library.PlanetName.Moon;
                        case 18: return Library.PlanetName.Saturn;
                        case 19: return Library.PlanetName.Jupiter;
                        case 20: return Library.PlanetName.Mars;
                        case 21: return Library.PlanetName.Sun;
                        case 22: return Library.PlanetName.Venus;
                        case 23: return Library.PlanetName.Mercury;
                        case 24: return Library.PlanetName.Moon;
                    }
                    break;
                case Library.DayOfWeek.Saturday:
                    switch (hora)
                    {
                        case 1: return Library.PlanetName.Saturn;
                        case 2: return Library.PlanetName.Jupiter;
                        case 3: return Library.PlanetName.Mars;
                        case 4: return Library.PlanetName.Sun;
                        case 5: return Library.PlanetName.Venus;
                        case 6: return Library.PlanetName.Mercury;
                        case 7: return Library.PlanetName.Moon;
                        case 8: return Library.PlanetName.Saturn;
                        case 9: return Library.PlanetName.Jupiter;
                        case 10: return Library.PlanetName.Mars;
                        case 11: return Library.PlanetName.Sun;
                        case 12: return Library.PlanetName.Venus;
                        case 13: return Library.PlanetName.Mercury;
                        case 14: return Library.PlanetName.Moon;
                        case 15: return Library.PlanetName.Saturn;
                        case 16: return Library.PlanetName.Jupiter;
                        case 17: return Library.PlanetName.Mars;
                        case 18: return Library.PlanetName.Sun;
                        case 19: return Library.PlanetName.Venus;
                        case 20: return Library.PlanetName.Mercury;
                        case 21: return Library.PlanetName.Moon;
                        case 22: return Library.PlanetName.Saturn;
                        case 23: return Library.PlanetName.Jupiter;
                        case 24: return Library.PlanetName.Mars;
                    }
                    break;
            }

            throw new Exception("Did not find hora, something wrong!");

        }

        /// <summary>
        /// Gets the junction point (sandhi) between 2 consecutive
        /// houses, where one house begins and the other ends.
        /// </summary>
        public static Angle HouseJunctionPoint(Angle previousHouse, Angle nextHouse)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseJunctionPoint), previousHouse, nextHouse, Ayanamsa), _getHouseJunctionPoint);


            //UNDERLYING FUNCTION
            Angle _getHouseJunctionPoint()
            {
                //Add the longitudes of two consecutive Bhavas (house)
                //and divide the sum by 2. The result represents sandhi (junction point of houses).

                //get sum of house longitudes
                var longitudeSum = previousHouse + nextHouse;

                Angle junctionPoint;

                //if next house longitude is lower than previous house longitude
                //next house is after 360 degrees
                if (nextHouse < previousHouse)
                {
                    //add 360 to longitude sum
                    longitudeSum = longitudeSum + Angle.Degrees360;

                    //divide sum by 2 to get junction point
                    junctionPoint = longitudeSum.Divide(2);

                    //correct junction point by subtracting 360
                    junctionPoint = junctionPoint - Angle.Degrees360;
                }
                else
                {
                    //divide sum by 2 to get junction point
                    junctionPoint = longitudeSum.Divide(2);
                }

                //return junction point
                return junctionPoint;

            }

        }

        /// <summary>
        /// Gets planet which is the lord of a given sign
        /// </summary>
        public static PlanetName LordOfZodiacSign(ZodiacName signName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LordOfZodiacSign), signName, Ayanamsa), _getLordOfZodiacSign);


            //UNDERLYING FUNCTION
            PlanetName _getLordOfZodiacSign()
            {
                switch (signName)
                {
                    //Aries and Scorpio are ruled by Mars;
                    case ZodiacName.Aries:
                    case ZodiacName.Scorpio:
                        return Library.PlanetName.Mars;

                    //Taurus and Libra by Venus;
                    case ZodiacName.Taurus:
                    case ZodiacName.Libra:
                        return Library.PlanetName.Venus;

                    //Gemini and Virgo by Mercury;
                    case ZodiacName.Gemini:
                    case ZodiacName.Virgo:
                        return Library.PlanetName.Mercury;

                    //Cancer by the Moon;
                    case ZodiacName.Cancer:
                        return Library.PlanetName.Moon;

                    //Leo by the Sun ;
                    case ZodiacName.Leo:
                        return Library.PlanetName.Sun;

                    //Sagittarius and Pisces by Jupiter
                    case ZodiacName.Sagittarius:
                    case ZodiacName.Pisces:
                        return Library.PlanetName.Jupiter;

                    //Capricorn and Aquarius by Saturn.
                    case ZodiacName.Capricorn:
                    case ZodiacName.Aquarius:
                        return Library.PlanetName.Saturn;
                    default:
                        throw new Exception("Lord of sign not found, error!");
                }

            }

        }

        /// <summary>
        /// Gets next zodiac sign after input sign
        /// </summary>
        public static ZodiacName NextZodiacSign(ZodiacName inputSign)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextZodiacSign), inputSign, Ayanamsa), _getNextZodiacSign);


            //UNDERLYING FUNCTION
            ZodiacName _getNextZodiacSign()
            {
                //get number of of input zodiac
                int inputSignNumber = (int)inputSign;

                int nextSignNumber;

                //after pieces (12) is Aries (1)
                if (inputSignNumber == 12)
                {
                    nextSignNumber = 1;
                }
                else
                {
                    //else next sign is input sign plus 1
                    nextSignNumber = inputSignNumber + 1;
                }

                //convert next sign number to its zodiac name
                var nextSignName = (ZodiacName)nextSignNumber;

                return nextSignName;

            }
        }

        /// <summary>
        /// Gets next house number after input house number, goes to  1 after 12
        /// </summary>
        public static int NextHouseNumber(int inputHouseNumber)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(NextHouseNumber), inputHouseNumber, Ayanamsa), _getNextHouseNumber);


            //UNDERLYING FUNCTION
            int _getNextHouseNumber()
            {
                int nextHouseNumber;

                //if input house number is 12
                if (inputHouseNumber == 12)
                {
                    //next house number is 1
                    nextHouseNumber = 1;

                }
                else
                {
                    //else next house number is input number + 1
                    nextHouseNumber = inputHouseNumber + 1;
                }


                return nextHouseNumber;

            }

        }

        /// <summary>
        /// Gets the exact longitude where planet is Exalted/Exaltation
        ///
        /// NOTE:
        /// Rahu & ketu have exaltation points ref : Astroloy for Beginners pg. 12
        /// 
        /// Exaltation
        /// Each planet is held to be exalted when it is
        /// in a particular sign. The power to do good when in
        /// exaltation is greater than when in its own sign.
        /// Throughout the sign ascribed, the planet is exalted
        /// but in a particular degree its exaltation is at the maximum level.
        /// 
        /// </summary>
        public static ZodiacSign PlanetExaltationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetExaltationPoint), planetName, Ayanamsa), _getPlanetExaltationPoint);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetExaltationPoint()
            {
                //Sun in the 10th degree of Aries;
                if (planetName == Library.PlanetName.Sun)
                {
                    return new ZodiacSign(ZodiacName.Aries, Angle.FromDegrees(10));
                }

                // Moon 3rd of Taurus;
                else if (planetName == Library.PlanetName.Moon)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(3));
                }

                // Mars 28th of Capricorn ;
                else if (planetName == Library.PlanetName.Mars)
                {
                    return new ZodiacSign(ZodiacName.Capricorn, Angle.FromDegrees(28));
                }

                // Mercury 15th of Virgo;
                else if (planetName == Library.PlanetName.Mercury)
                {
                    return new ZodiacSign(ZodiacName.Virgo, Angle.FromDegrees(15));
                }

                // Jupiter 5th of Cancer;
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    return new ZodiacSign(ZodiacName.Cancer, Angle.FromDegrees(5));
                }

                // Venus 27th of Pisces and
                else if (planetName == Library.PlanetName.Venus)
                {
                    return new ZodiacSign(ZodiacName.Pisces, Angle.FromDegrees(27));
                }

                // Saturn 20th of Libra.
                else if (planetName == Library.PlanetName.Saturn)
                {
                    return new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(20));
                }

                // Rahu 20th of Taurus.
                else if (planetName == Library.PlanetName.Rahu)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(20));
                }
                // Ketu 20th of Scorpio.
                else if (planetName == Library.PlanetName.Ketu)
                {
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(20));
                }

                throw new Exception("Planet exaltation point not found, error!");

            }

        }

        /// <summary>
        /// Gets the exact longitude where planet is Debilitated/Debility
        /// TODO method needs testing!
        /// Note:
        /// -   Rahu & ketu have debilitation points ref : Astroloy for Beginners pg. 12
        /// -   "planet to sign relationship" is the whole sign, this is just a point
        /// -   The 7th house or the 180th degree from the place of exaltation is the
        ///     place of debilitation or fall. The Sun is debilitated-
        ///     in the 10th degree of Libra, the Moon 3rd
        ///     of Scorpio and so on.
        /// -   The debilitation or depression points are found
        ///     by adding 180° to the maximum points given above.
        ///     While in a state of fall, planets give results contrary
        ///     to those when in exaltation. ref : Astroloy for Beginners pg. 11
        /// </summary>
        public static ZodiacSign PlanetDebilitationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetDebilitationPoint), planetName, Ayanamsa), _getPlanetDebilitationPoint);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetDebilitationPoint()
            {
                //The 7th house or the
                // 180th degree from the place of exaltation is the
                // place of debilitation or fall. The Sun is debilitated-
                // in the 10th degree of Libra, the Moon 3rd
                // of Scorpio and so on.

                //if (planetName == PlanetName.Sun)
                //{
                //    return Angle.FromDegrees(190);
                //}
                //else if (planetName == PlanetName.Moon)
                //{
                //    return Angle.FromDegrees(213);
                //}
                //else if (planetName == PlanetName.Mars)
                //{
                //    return Angle.FromDegrees(118);
                //}
                //else if (planetName == PlanetName.Mercury)
                //{
                //    return Angle.FromDegrees(345);
                //}
                //else if (planetName == PlanetName.Jupiter)
                //{
                //    return Angle.FromDegrees(275);
                //}
                //else if (planetName == PlanetName.Venus)
                //{
                //    return Angle.FromDegrees(177);
                //}
                //else if (planetName == PlanetName.Saturn)
                //{
                //    return Angle.FromDegrees(20);
                //}


                //Sun in the 10th degree of Libra;
                if (planetName == Library.PlanetName.Sun)
                {
                    return new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(10));
                }

                // Moon 0 of Scorpio
                else if (planetName == Library.PlanetName.Moon)
                {
                    //TODO check if 0 degrees exist
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(0));
                }

                // Mars 28th of Cancer ;
                else if (planetName == Library.PlanetName.Mars)
                {
                    return new ZodiacSign(ZodiacName.Cancer, Angle.FromDegrees(28));
                }

                // Mercury 15th of Pisces;
                else if (planetName == Library.PlanetName.Mercury)
                {
                    return new ZodiacSign(ZodiacName.Pisces, Angle.FromDegrees(15));
                }

                // Jupiter 5th of Capricorn;
                else if (planetName == Library.PlanetName.Jupiter)
                {
                    return new ZodiacSign(ZodiacName.Capricorn, Angle.FromDegrees(5));
                }

                // Venus 27th of Virgo and
                else if (planetName == Library.PlanetName.Venus)
                {
                    return new ZodiacSign(ZodiacName.Virgo, Angle.FromDegrees(27));
                }

                // Saturn 20th of Aries.
                else if (planetName == Library.PlanetName.Saturn)
                {
                    return new ZodiacSign(ZodiacName.Aries, Angle.FromDegrees(20));
                }

                // Rahu 20th of Scorpio.
                else if (planetName == Library.PlanetName.Rahu)
                {
                    return new ZodiacSign(ZodiacName.Scorpio, Angle.FromDegrees(20));
                }
                // Ketu 20th of Taurus.
                else if (planetName == Library.PlanetName.Ketu)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(20));
                }


                throw new Exception("Planet debilitation point not found, error!");

            }


        }


        #region SIGN GROUP CALULATORS

        /// <summary>
        /// Returns true if zodiac sign is an Even sign,  Yugma Rasis
        /// </summary>
        public static bool IsEvenSign(ZodiacName planetSignName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsEvenSign), planetSignName, Ayanamsa), _isEvenSign);


            //UNDERLYING FUNCTION
            bool _isEvenSign()
            {
                if (planetSignName == ZodiacName.Taurus || planetSignName == ZodiacName.Cancer || planetSignName == ZodiacName.Virgo ||
                    planetSignName == ZodiacName.Scorpio || planetSignName == ZodiacName.Capricorn || planetSignName == ZodiacName.Pisces)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        /// <summary>
        /// Returns true if zodiac sign is an Odd sign, Oja Rasis
        /// </summary>
        public static bool IsOddSign(ZodiacName planetSignName)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsOddSign), planetSignName, Ayanamsa), _isOddSign);


            //UNDERLYING FUNCTION
            bool _isOddSign()
            {
                if (planetSignName == ZodiacName.Aries || planetSignName == ZodiacName.Gemini || planetSignName == ZodiacName.Leo ||
                    planetSignName == ZodiacName.Libra || planetSignName == ZodiacName.Sagittarius || planetSignName == ZodiacName.Aquarius)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }


        }

        /// <summary>
        /// Fixed signs- Taurus, Leo, Scropio, Aquarius.
        /// </summary>
        public static bool IsFixedSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Taurus:
                case ZodiacName.Leo:
                case ZodiacName.Scorpio:
                case ZodiacName.Aquarius:
                    return true;
                default:
                    return false;
            }

        }

        /// <summary>
        /// Movable signs- Aries, Cancer, Libra, Capricorn.
        /// </summary>
        public static bool IsMovableSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Aries:
                case ZodiacName.Cancer:
                case ZodiacName.Libra:
                case ZodiacName.Capricorn:
                    return true;
                default:
                    return false;
            }

        }

        /// <summary>
        /// Common signs- Gemini, Virgo, Sagitarius, Pisces.
        /// </summary>
        public static bool IsCommonSign(ZodiacName sunSign)
        {
            switch (sunSign)
            {
                case ZodiacName.Gemini:
                case ZodiacName.Virgo:
                case ZodiacName.Sagittarius:
                case ZodiacName.Pisces:
                    return true;
                default:
                    return false;
            }

        }


        #endregion

        /// <summary>
        /// Gets a planets permenant relationship.
        /// Based on : Hindu Predictive Astrology, pg. 21
        /// Note:
        /// - Rahu & Ketu are not mentioned in any permenant relatioship by Raman.
        ///   But some websites do mention this. As such Raman's take is taken as final.
        ///   Since there's so far no explanation by Raman on Rahu & Ketu permenant relation it
        ///   is assumed that such relationship is not needed and to make them up for conveniece sake
        ///   could result in wrong prediction down the line.
        ///   But temporary relationship are mentioned by Raman for Rahu & Ketu, so explicitly use
        ///   Temperary relationship where needed.
        /// </summary>
        public static PlanetToPlanetRelationship PlanetPermanentRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet)
        {

            //no calculation for rahu and ketu here
            var isRahu = mainPlanet.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = mainPlanet.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahu2 = secondaryPlanet.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu2 = secondaryPlanet.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu || isRahu2 || isKetu2;
            if (isRahuKetu) { return PlanetToPlanetRelationship.Empty; }



            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetPermanentRelationshipWithPlanet), mainPlanet, secondaryPlanet, Ayanamsa), _getPlanetPermanentRelationshipWithPlanet);


            //UNDERLYING FUNCTION
            PlanetToPlanetRelationship _getPlanetPermanentRelationshipWithPlanet()
            {
                //if main planet & secondary planet is same, then it is own plant (same planet), end here
                if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }


                bool planetInEnemies = false;
                bool planetInNeutrals = false;
                bool planetInFriends = false;


                //if main planet is sun
                if (mainPlanet == Library.PlanetName.Sun)
                {
                    //List planets friends, neutrals & enemies
                    var sunFriends = new List<PlanetName>() { Library.PlanetName.Moon, Library.PlanetName.Mars, Library.PlanetName.Jupiter };
                    var sunNeutrals = new List<PlanetName>() { Library.PlanetName.Mercury };
                    var sunEnemies = new List<PlanetName>() { Library.PlanetName.Venus, Library.PlanetName.Saturn };

                    //check if planet is found in any of the lists
                    planetInFriends = sunFriends.Contains(secondaryPlanet);
                    planetInNeutrals = sunNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = sunEnemies.Contains(secondaryPlanet);
                }

                //if main planet is moon
                if (mainPlanet == Library.PlanetName.Moon)
                {
                    //List planets friends, neutrals & enemies
                    var moonFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Mercury };
                    var moonNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter, Library.PlanetName.Venus, Library.PlanetName.Saturn };
                    var moonEnemies = new List<PlanetName>() { };

                    //check if planet is found in any of the lists
                    planetInFriends = moonFriends.Contains(secondaryPlanet);
                    planetInNeutrals = moonNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = moonEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mars
                if (mainPlanet == Library.PlanetName.Mars)
                {
                    //List planets friends, neutrals & enemies
                    var marsFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Jupiter };
                    var marsNeutrals = new List<PlanetName>() { Library.PlanetName.Venus, Library.PlanetName.Saturn };
                    var marsEnemies = new List<PlanetName>() { Library.PlanetName.Mercury };

                    //check if planet is found in any of the lists
                    planetInFriends = marsFriends.Contains(secondaryPlanet);
                    planetInNeutrals = marsNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = marsEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mercury
                if (mainPlanet == Library.PlanetName.Mercury)
                {
                    //List planets friends, neutrals & enemies
                    var mercuryFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Venus };
                    var mercuryNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter, Library.PlanetName.Saturn };
                    var mercuryEnemies = new List<PlanetName>() { Library.PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = mercuryFriends.Contains(secondaryPlanet);
                    planetInNeutrals = mercuryNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = mercuryEnemies.Contains(secondaryPlanet);

                }

                //if main planet is jupiter
                if (mainPlanet == Library.PlanetName.Jupiter)
                {
                    //List planets friends, neutrals & enemies
                    var jupiterFriends = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Mars };
                    var jupiterNeutrals = new List<PlanetName>() { Library.PlanetName.Saturn };
                    var jupiterEnemies = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Venus };

                    //check if planet is found in any of the lists
                    planetInFriends = jupiterFriends.Contains(secondaryPlanet);
                    planetInNeutrals = jupiterNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = jupiterEnemies.Contains(secondaryPlanet);

                }

                //if main planet is venus
                if (mainPlanet == Library.PlanetName.Venus)
                {
                    //List planets friends, neutrals & enemies
                    var venusFriends = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Saturn };
                    var venusNeutrals = new List<PlanetName>() { Library.PlanetName.Mars, Library.PlanetName.Jupiter };
                    var venusEnemies = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = venusFriends.Contains(secondaryPlanet);
                    planetInNeutrals = venusNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = venusEnemies.Contains(secondaryPlanet);

                }

                //if main planet is saturn
                if (mainPlanet == Library.PlanetName.Saturn)
                {
                    //List planets friends, neutrals & enemies
                    var saturnFriends = new List<PlanetName>() { Library.PlanetName.Mercury, Library.PlanetName.Venus };
                    var saturnNeutrals = new List<PlanetName>() { Library.PlanetName.Jupiter };
                    var saturnEnemies = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Moon, Library.PlanetName.Mars };

                    //check if planet is found in any of the lists
                    planetInFriends = saturnFriends.Contains(secondaryPlanet);
                    planetInNeutrals = saturnNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = saturnEnemies.Contains(secondaryPlanet);

                }

                //for Rahu & Ketu special exception
                if (mainPlanet == Library.PlanetName.Rahu || mainPlanet == Library.PlanetName.Ketu)
                {
                    throw new Exception("No Permenant Relation for Rahu and Ketu, use Temporary Relation!");
                }




                //return planet relationship based on where planet is found
                if (planetInFriends)
                {
                    return PlanetToPlanetRelationship.Friend;
                }
                if (planetInNeutrals)
                {
                    return PlanetToPlanetRelationship.Neutral;
                }
                if (planetInEnemies)
                {
                    return PlanetToPlanetRelationship.Enemy;
                }


                throw new Exception("planet permanent relationship not found, error!");

            }

        }

        /// <summary>
        /// Converts julian time to normal time, normal time can be lmt, lat, utc
        /// </summary>
        public static DateTime ConvertJulianTimeToNormalTime(double julianTime)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConvertJulianTimeToNormalTime), julianTime, Ayanamsa), _convertJulianTimeToNormalTime);


            //UNDERLYING FUNCTION
            DateTime _convertJulianTimeToNormalTime()
            {
                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //julian time to normal time
                int year = 0, month = 0, day = 0, hour = 0, min = 0;
                double sec = 0;

                // convert julian time to normal time
                ephemeris.swe_jdut1_to_utc(julianTime, gregflag, ref year, ref month, ref day, ref hour, ref min, ref sec);

                //put pieces of time into one type
                var normalUtcTime = new DateTime(year, month, day, hour, min, (int)sec);

                return normalUtcTime;

            }


        }

        /// <summary>
        /// Gets Greenwich time in normal format from Julian days at Greenwich
        /// Note : Inputed time is Julian days at greenwich, callers reponsibility to make sure 
        /// </summary>
        public static DateTimeOffset GreenwichTimeFromJulianDays(double julianTime)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GreenwichTimeFromJulianDays), julianTime, Ayanamsa), _convertJulianTimeToNormalTime);


            //UNDERLYING FUNCTION
            DateTimeOffset _convertJulianTimeToNormalTime()
            {
                //initialize ephemeris
                SwissEph ephemeris = new();

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //prepare a place to receive the time in normal format 
                int year = 0, month = 0, day = 0, hour = 0, min = 0;
                double sec = 0;

                //convert julian time to normal time
                ephemeris.swe_jdut1_to_utc(julianTime, gregflag, ref year, ref month, ref day, ref hour, ref min, ref sec);

                //put pieces of time into one type
                var normalUtcTime = new DateTime(year, month, day, hour, min, (int)sec);

                //set the correct offset (Greenwich = UTC = +0:00)
                var offsetTime = new DateTimeOffset(normalUtcTime, new TimeSpan(0, 0, 0));

                return offsetTime;
            }


        }

        /// <summary>
        /// Gets Local mean time (LMT) at Greenwich (UTC) in Julian days based on the inputed time
        /// </summary>
        public static double GreenwichLmtInJulianDays(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GreenwichLmtInJulianDays), time, Ayanamsa), _getGreenwichLmtInJulianDays);


            //UNDERLYING FUNCTION
            double _getGreenwichLmtInJulianDays()
            {
                //get LMT time at Greenwich (UTC)
                DateTimeOffset lmtDateTime = time.GetLmtDateTimeOffset().ToUniversalTime();

                //split lmt time to pieces
                int year = lmtDateTime.Year;
                int month = lmtDateTime.Month;
                int day = lmtDateTime.Day;
                double hour = (lmtDateTime.TimeOfDay).TotalHours;

                //set calender type
                int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR

                //declare output variables
                double localMeanTimeInJulian_UT;

                //initialize ephemeris
                SwissEph ephemeris = new();

                //get lmt in julian day in Universal Time (UT)
                localMeanTimeInJulian_UT = ephemeris.swe_julday(year, month, day, hour, gregflag);//time to Julian Day

                return localMeanTimeInJulian_UT;

            }

        }

        /// <summary>
        /// Gets the longitude of house 1 and house 10
        /// using Swiss Epehemris swe_houses
        /// </summary>
        public static double[] GetHouse1And10Longitudes(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetHouse1And10Longitudes), time, Ayanamsa), _getHouse1And10Longitudes);


            //UNDERLYING FUNCTION
            double[] _getHouse1And10Longitudes()
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                //Convert DOB to Julian Day
                var jul_day_UT = TimeToJulianDay(time);

                SwissEph swissEph = new SwissEph();

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //set ayanamsa
                swissEph.swe_set_sid_mode(Ayanamsa, 0, 0);

                //NOTE:
                //if you use P which is Placidus there is a high chances you will get unequal houses from the SwissEph library itself...
                // you have to use V - 'V'Vehlow equal (Asc. in middle of house 1)
                swissEph.swe_houses(jul_day_UT, location.Latitude(), location.Longitude(), 'V', cusps, ascmc);

                //we only return cusps, cause that is what is used for now
                return cusps;
            }

        }


        /// <summary>
        /// Converts Local Mean Time (LMT) to Universal Time (UTC)
        /// </summary>
        public static DateTimeOffset LmtToUtc(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(LmtToUtc), time, Ayanamsa), _lmtToUtc);


            //UNDERLYING FUNCTION
            DateTimeOffset _lmtToUtc()
            {
                return time.GetLmtDateTimeOffset().ToUniversalTime();
            }
        }

        #endregion

        #region GOCHARA CALCULATIONS

        /// <summary>
        /// Gets the Gochara House number which is the count from birth Moon sign (janma rasi)
        /// to the sign the planet is at the current time. Gochara == Transits
        /// </summary>
        public static int GocharaZodiacSignCountFromMoon(Time birthTime, Time currentTime, PlanetName planet)
        {
            //get moon sign at birth (janma rasi)
            var janmaSign = Calculate.MoonSignName(birthTime);

            //get planet sign at input time
            var planetSign = Calculate.PlanetSignName(planet, currentTime).GetSignName();

            //count from janma to sign planet is in
            var count = Calculate.CountFromSignToSign(janmaSign, planetSign);

            return count;
        }

        /// <summary>
        /// Check if there is an obstruction to a given Gochara, obstructing house/point (Vedhanka)
        /// </summary>
        public static bool IsGocharaObstructed(PlanetName planet, int gocharaHouse, Time birthTime, Time currentTime)
        {
            //get the obstructing house/point (Vedhanka) for the inputed Gochara house
            var vedhanka = Vedhanka(planet, gocharaHouse);

            //if vedhanka is 0, then end here as no obstruction
            if (vedhanka == 0) { return false; }

            //get all the planets transiting (gochara) in this obstruction point/house (vedhanka)
            var planetList = PlanetsInGocharaHouse(birthTime, currentTime, gocharaHouse);

            //remove the exception planets
            //No Vedha occurs between the Sun and Saturn, and the Moon and Mercury.
            if (planet == Library.PlanetName.Sun || planet == Library.PlanetName.Saturn)
            {
                planetList.Remove(Library.PlanetName.Sun);
                planetList.Remove(Library.PlanetName.Saturn);
            }
            if (planet == Library.PlanetName.Moon || planet == Mercury)
            {
                planetList.Remove(Library.PlanetName.Moon);
                planetList.Remove(Library.PlanetName.Mercury);
            }

            //now if any planet is found in the list, than obstruction is present
            return planetList.Any();

        }

        /// <summary>
        /// Gets all the planets in a given Gochara House
        /// 
        /// Note : Gochara House number is the count from birth Moon sign (janma rasi)
        /// to the sign the planet is at the current time. Gochara == Transits
        /// </summary>
        public static List<PlanetName> PlanetsInGocharaHouse(Time birthTime, Time currentTime, int gocharaHouse)
        {
            //get the gochara house for every planet at current time
            var gocharaSun = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Sun);
            var gocharaMoon = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Moon);
            var gocharaMars = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Mars);
            var gocharaMercury = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Mercury);
            var gocharaJupiter = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Jupiter);
            var gocharaVenus = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Venus);
            var gocharaSaturn = GocharaZodiacSignCountFromMoon(birthTime, currentTime, Library.PlanetName.Saturn);

            //add every planet name to return list that matches input Gochara house number
            var planetList = new List<PlanetName>();
            if (gocharaSun == gocharaHouse) { planetList.Add(Library.PlanetName.Sun); }
            if (gocharaMoon == gocharaHouse) { planetList.Add(Library.PlanetName.Moon); }
            if (gocharaMars == gocharaHouse) { planetList.Add(Library.PlanetName.Mars); }
            if (gocharaMercury == gocharaHouse) { planetList.Add(Library.PlanetName.Mercury); }
            if (gocharaJupiter == gocharaHouse) { planetList.Add(Library.PlanetName.Jupiter); }
            if (gocharaVenus == gocharaHouse) { planetList.Add(Library.PlanetName.Venus); }
            if (gocharaSaturn == gocharaHouse) { planetList.Add(Library.PlanetName.Saturn); }

            return planetList;
        }

        /// <summary>
        /// Gets the Vedhanka (point of obstruction), used for Gohchara calculations.
        /// The data returned comes from a fixed table. 
        /// NOTE: - Planet exceptions are not accounted for here.
        ///       - Return 0 when no obstruction point exists 
        /// Reference : Hindu Predictive Astrology pg. 257
        /// </summary>
        public static int Vedhanka(PlanetName planet, int house)
        {
            //filter based on planet
            if (planet == Library.PlanetName.Sun)
            {
                //good
                if (house == 11) { return 5; }
                if (house == 3) { return 9; }
                if (house == 10) { return 4; }
                if (house == 6) { return 12; }
                //bad
                if (house == 5) { return 11; }
                if (house == 9) { return 3; }
                if (house == 4) { return 10; }
                if (house == 12) { return 6; }
            }

            if (planet == Library.PlanetName.Moon)
            {
                //good
                if (house == 7) { return 2; }
                if (house == 1) { return 5; }
                if (house == 6) { return 12; }
                if (house == 11) { return 8; }
                if (house == 10) { return 4; }
                if (house == 3) { return 9; }
                //bad
                if (house == 2) { return 7; }
                if (house == 5) { return 1; }
                if (house == 12) { return 6; }
                if (house == 8) { return 11; }
                if (house == 4) { return 10; }
                if (house == 9) { return 3; }

            }

            if (planet == Library.PlanetName.Mars)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }
                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }
            }

            if (planet == Library.PlanetName.Mercury)
            {
                //good
                if (house == 2) { return 5; }
                if (house == 4) { return 3; }
                if (house == 6) { return 9; }
                if (house == 8) { return 1; }
                if (house == 10) { return 7; }
                if (house == 11) { return 12; }

                //bad
                if (house == 5) { return 2; }
                if (house == 3) { return 4; }
                if (house == 9) { return 6; }
                if (house == 1) { return 8; }
                if (house == 7) { return 10; }
                if (house == 12) { return 11; }
            }

            if (planet == Library.PlanetName.Jupiter)
            {
                //good
                if (house == 2) { return 12; }
                if (house == 11) { return 8; }
                if (house == 9) { return 10; }
                if (house == 5) { return 4; }
                if (house == 7) { return 3; }

                //bad
                if (house == 12) { return 2; }
                if (house == 8) { return 11; }
                if (house == 10) { return 9; }
                if (house == 4) { return 5; }
                if (house == 3) { return 7; }

            }

            if (planet == Library.PlanetName.Venus)
            {
                //good
                if (house == 1) { return 8; }
                if (house == 2) { return 7; }
                if (house == 3) { return 1; }
                if (house == 4) { return 10; }
                if (house == 5) { return 9; }
                if (house == 8) { return 5; }
                if (house == 9) { return 11; }
                if (house == 11) { return 6; }
                if (house == 12) { return 3; }

                //bad
                if (house == 8) { return 1; }
                if (house == 7) { return 2; }
                if (house == 1) { return 3; }
                if (house == 10) { return 4; }
                if (house == 9) { return 5; }
                if (house == 5) { return 8; }
                if (house == 11) { return 9; }
                if (house == 6) { return 11; }
                if (house == 3) { return 12; }

            }

            if (planet == Library.PlanetName.Saturn)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }
            //copy of saturn & mars
            if (planet == Library.PlanetName.Rahu)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }
            if (planet == Library.PlanetName.Ketu)
            {
                //good
                if (house == 3) { return 12; }
                if (house == 11) { return 5; }
                if (house == 6) { return 9; }

                //bad
                if (house == 12) { return 3; }
                if (house == 5) { return 11; }
                if (house == 9) { return 6; }

            }





            //if no condition above met, then there is no obstruction point
            return 0;
        }

        /// <summary>
        /// Is SunGocharaInHouse1
        /// Checks if a Gochara is occuring for a planet in a given house without any obstructions at a given time
        /// Note : Basically a wrapper method for Gochra event calculations
        /// </summary>
        public static bool IsGocharaOccurring(Time birthTime, Time time, PlanetName planet, int gocharaHouse)
        {
            //check if planet is in the specified gochara house
            var planetGocharaMatch = Calculate.GocharaZodiacSignCountFromMoon(birthTime, time, planet) == gocharaHouse;

            //NOTE: only use Vedha point by default, but allow disable if needed (LONG LEVER DESIGN)
            bool obstructionNotFound = true; //default to true, so if disabled Vedha point will still work
            if (Calculate.UseVedhankaInGochara)
            {
                //check if there is any planet obstructing this transit prediction via Vedhasthana
                obstructionNotFound = !Calculate.IsGocharaObstructed(planet, gocharaHouse, birthTime, time);
            }

            //occuring if all conditions met
            var occuring = planetGocharaMatch && obstructionNotFound;

            return occuring;
        }

        /// <summary>
        /// SunTransit8Bindu
        /// Checks if a given planet's with given number of bindu is transiting now (Gochara)
        /// </summary>
        public static bool IsPlanetGocharaBindu(Time birthTime, Time nowTime, PlanetName planet, int bindu)
        {
            //house the planet is transiting now
            var gocharaHouse = Calculate.GocharaZodiacSignCountFromMoon(birthTime, nowTime, planet);

            //check if there is any planet obstructing this transit prediction via Vedhasthana
            var obstructionFound = Calculate.IsGocharaObstructed(planet, gocharaHouse, birthTime, nowTime);

            //if obstructed end here
            if (obstructionFound) { return false; }

            //gochara ongoing, get sign of house to get planet's bindu score for said transit
            var gocharaSign = HouseSignName((HouseName)gocharaHouse, nowTime);

            //get planet's current bindu
            var planetBindu = Calculate.PlanetAshtakvargaBindu(planet, gocharaSign, nowTime);

            //occuring if bindu is match
            var occuring = planetBindu == bindu;

            return occuring;
        }


        /// <summary>
        /// Give a planet and sign and ashtakvarga bindu can be calculated
        ///
        /// EXP : In the Sun's own Ashtakvarga, there are 5 bindus in Aries
        /// 
        /// NOTE ON USE: Ashtakvarga System pg.128 
        /// For example, in the Standard Horoscope,
        /// the Sun's transit of Aries (3rd from Moon) should
        /// prove favourable. In the Sun's own Ashtakvarga,
        /// there are 5 bindus in Aries. Therefore the
        /// good effects produced should be to the extent
        /// of 62%. The Sun's transit of Capricorn
        /// (12th from the Moon) should prove adverse.
        /// Capricorn, has no bindus.Therefore the evil results
        /// to be produced by this transit are to the brim.
        /// </summary>
        public static int PlanetAshtakvargaBindu(PlanetName planet, ZodiacName signToCheck, Time time)
        {
            //calculates ashtakvarga for given planet 
            var bhinnashtakavargaChart = PlanetBhinnashtakavargaChart(planet, time);

            //get bindu score for given sign
            var bindu = bhinnashtakavargaChart[signToCheck];

            return bindu;
        }

        /// <summary>
        /// Bhinnashtakavarga or individual Ashtakvarga charts
        /// List of planets & ascendant with their their bindu point
        /// </summary>
        public static Dictionary<PlanetName, Dictionary<ZodiacName, int>> AllBhinnashtakavargaChart(Time birthTime)
        {
            //Made on cold winter morning in July

            var minorPlanetList = Library.PlanetName.All7Planets.Select(e => e.ToString()).ToList();
            minorPlanetList.Add("ascendant"); //add special case Ascendant for Ashtakvarga calculation
            var mainBhinaAstaChart = new Dictionary<PlanetName, Dictionary<ZodiacName, int>>();

            //make the charts compiled from the position of 7 planets
            foreach (var mainPlanet in Library.PlanetName.All7Planets)
            {
                //load the benefic places for all the minor planets
                var allPlanetBeneficList = new Dictionary<string, int[]>();
                foreach (var minorPlanet in minorPlanetList)
                {
                    var mainPlanetBeneficList = GetPlanetBeneficHouseAshtakvarga(mainPlanet.ToString(), minorPlanet);
                    allPlanetBeneficList.Add(minorPlanet, mainPlanetBeneficList);
                }

                //Bhinnashtakavarga chart in array form
                var mainPlanetBhinaAstaChart = ZodiacNameExtensions.GetDictionary(0);
                foreach (var minorPlanet in minorPlanetList)
                {
                    //parse minor planet type, if ascendant get sign of 1st house
                    //start sign can be from planet or 1st house (Ascendant)
                    var isAscendant = minorPlanet == "ascendant";
                    var minorPlanetStartSign = isAscendant
                                                ? HouseSignName(HouseName.House1, birthTime)
                                                : PlanetSignName(Library.PlanetName.Parse(minorPlanet), birthTime).GetSignName();

                    //add the points together, add 1 for a benefic sign
                    foreach (var houseCount in allPlanetBeneficList[minorPlanet])
                    {
                        var signXFromPlanet = SignCountedFromInputSign(minorPlanetStartSign, houseCount);
                        mainPlanetBhinaAstaChart[signXFromPlanet] += 1; //add 1 to existing score from previous
                    }
                }

                //add the compiled main planet's chart to main list
                mainBhinaAstaChart.Add(mainPlanet, mainPlanetBhinaAstaChart);
            }


            //return compiled charts for all 7 planets
            return mainBhinaAstaChart;

        }

        /// <summary>
        /// Calculates full ashtakvarga chart for a given planet for all 12 signs
        /// Used to for calculating final Ashtakvarga, Rahu & Ketu will return 0
        /// </summary>
        public static Dictionary<ZodiacName, int> PlanetBhinnashtakavargaChart(PlanetName mainPlanet, Time birthTime)
        {
            //no rahu & ketu, so re
            if (mainPlanet.Name is PlanetNameEnum.Rahu or PlanetNameEnum.Ketu) { return new Dictionary<ZodiacName, int>(); }

            //make the charts compiled from the position of 7 planets plus Ascendant
            var minorPlanetList = Library.PlanetName.All7Planets.Select(e => e.ToString()).ToList();
            minorPlanetList.Add("ascendant"); //add special case Ascendant for Ashtakvarga calculation

            //load the benefic places for all the minor planets
            var allPlanetBeneficList = new Dictionary<string, int[]>();
            foreach (var minorPlanet in minorPlanetList)
            {
                //fixed positions from a given planet that is positive (table data)
                var mainPlanetBeneficList = GetPlanetBeneficHouseAshtakvarga(mainPlanet.ToString(), minorPlanet);
                allPlanetBeneficList.Add(minorPlanet, mainPlanetBeneficList);
            }

            //Bhinnashtakavarga chart in array form
            var mainPlanetBhinaAstaChart = ZodiacNameExtensions.GetDictionary(0);
            foreach (var minorPlanet in minorPlanetList)
            {
                //parse minor planet type, if ascendant get sign of 1st house
                //start sign can be from planet or 1st house (Ascendant)
                var isAscendant = minorPlanet == "ascendant";
                var minorPlanetStartSign = isAscendant
                    ? HouseSignName(HouseName.House1, birthTime)
                    : PlanetSignName(Library.PlanetName.Parse(minorPlanet), birthTime).GetSignName();

                //add the points together, add 1 for a benefic sign
                foreach (var houseCount in allPlanetBeneficList[minorPlanet])
                {
                    var signXFromPlanet = SignCountedFromInputSign(minorPlanetStartSign, houseCount);
                    mainPlanetBhinaAstaChart[signXFromPlanet] += 1; //add 1 to existing score from previous
                }
            }

            //return compiled chart
            return mainPlanetBhinaAstaChart;
        }

        /// <summary>
        /// This a constant that does not change, when calculating ashtakvarga this is the constant count used
        /// data from ashtakvarga table (Ashtakvarga System pg.9)
        /// Code: Once this data was memorized by human minds, today it is programmed by human hands
        /// </summary>
        public static int[] GetPlanetBeneficHouseAshtakvarga(string mainPlanet, string minorPlanet)
        {
            //load the data from ashtakvarga table (Ashtakvarga System pg.9)
            #region LOAD DATA

            var dictionary = new Dictionary<(string, string), int[]>();

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


        #endregion

        #region DASA CALCUATIONS

        /// <summary>
        /// Gets the relationship between a mojor period planet and minor period planet based solely on relationship between
        /// the planets and nothing to do with the signs yet
        /// based on cyclic relationship between planets
        /// </summary>
        public static (EventNature eventNature, string desciption) GetPlanetDasaMajorPlanetAndMinorRelationship(PlanetName majorPlanet, PlanetName minorPlanet)
        {
            //------ Code Poetry ------
            // lets take a moment to appreciate this piece of code
            // it represents mathematically the nueral patern inside the human brain
            // what the brain once did, is now done below
            //-------------------------

            //create place to fill the data in & to detect if null
            var returnVal = (eventNature: EventNature.Empty, desciption: "");

            switch (majorPlanet.Name)
            {
                case Library.PlanetName.PlanetNameEnum.Sun:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Unpleasantness with relatives and superiors, anxieties, headache, pain in the ear, some tendency to urinary or kidney troubles, sickness, fear from rulers and enemies, fear of death, loss of money, danger to father if the Sun is afflicted, stomachache and travels, gains through religious people, mental sufferings, a wandering life in a foreign country.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Winning favour from superiors, increase in business, fresh enterprises, troubles through women, eye troubles, many relatives and friends, indulgence in idle pastimes, jaundice and kindred ailments, new clothes and ornaments, will be happy, healthy, good meals, respect among relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Rheumatic and similar troubles, quarrels, danger of enteric fever, dysentery, troubles to relatives, loss of money by thefts or wasteful expenses, failures, acquisition of wealth in the form of gold and gems, royal favour leading to prosperity, contraction and transmission of bilious and other diseases, mental worries, danger from fire, ill-health, loss of reputation, sorrow.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Gain in money, good reputation, acquisition of new clothes and ornaments, new education, trouble through relatives, mental distress, depression of spirits, waste of money and nervous weakness, no comforts, friends becoming enemies, much anxiety and fears, health bad, children ungrateful, disputes and trouble from ruler or judge, suffer disgrace, many short journeys and wanderings.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Benefits from friends and acquaintances, increase in education, employment in high circles, association with people of high rank, success through obstacles, birth of a child, wealth got through sons (if there is a son), honour to religious people, virtuous acts, good traditional observances, good society and conversations, reputation, gains and court-honours.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Gain of money, respect by rulers and gain of vehicles, likelihood of marriage, increase of property, illness, does many good works, acquisition of pearls or other precious stones, fatigue, addiction to immoral females and profitless discussions.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Constant sickness to family members, new enemies, some loss of property, bodily ailment, much unhappiness, displacement from home accidents, quarrels with relatives, loss of money, disease, lacking in energy, ignoble calls, mental worries, loans, danger from thieve and rulers.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Many troubles, changes according to the position of Rahu, family disputes, journeys, fear of death, trouble from relatives and enemies, loss of peace or mental misery, loss of money, sorrows, unsuccessful in all attempts, fear of thieves and reptiles, scandals.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of money, affliction of mind with troubles, fainting or nervous exhaustion, mind full of misgivings, a long journey to a distant place, change of house due to disputes, troubles among relatives and associates, throat disease, mental anguish, ophthalmia, serious illness, fear from kings or rulers and enemies, diseases, cheated by others.";
                                    return returnVal;
                                }
                            default:
                                {
                                    throw new Exception($"Planet not accounted for! : {minorPlanet}");
                                }
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Moon:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Feverish complaints, pains in the eyes, success or failure according to position of the Sun and the Moon, legal power, free from diseases, decadence of enemies, happiness and prosperity, jaundice, dropsy, fever, loss of money, travels, danger to father and mother, piles, weakness, loss of children and friends.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Devoted attention to learning, love or music, good clothing, company of refined society, sound health, good reputation, journey to holy places, acquisition of abandoned wealth, power, vehicles and lands; marriage, relatives, fortunate deeds, inclination to public life, change of residence, birth of a child, increase of wealth, prosperity to relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Quarrels and litigation among friends and relatives, headlong enterprises, danger of disputes between husband and wife, between lovers or in regard to marital affairs; disease, petulence, loss of money, waste of wealth, trouble from brothers and friends, danger from fever and fire, injury from instruments or stones, loss of blood and disease to household animals.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of wealth from maternal relatives, new clothes and ornaments, settlement of disputes, pleasure through children or lover, increase of wealth, accomplishment of desires, intellectual achievements, new education, honour from rulers, general happiness, enjoyment with females, addiction to betting and drinks.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of property, plenty of food and comforts, prosperous, benefits from superiors such as masters or governors, birth. of a child, vehicles, abundance of clothes and ornaments and success in undertakings, patronage or rulers, gain or property, respect, learned.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Sudden gain from wife, enjoys comforts of agriculture, water products and clothing, suffers from diseases inherited by mother, sickness, pain, loss of property, enmity, gain of houses, good works and good meals, birth of children, expenses due to marriage or other auspicious acts.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Wife's death or separation, much mental anguish, loss of property, loss of friends, ill health, mental trouble due to mother, wind and bilious affections, harsh words, and discussion with unfriendly people, disease due to indigestion, no peace of mind, quarrels with relatives.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Distress of risks from falls and dangerous diseases, waste of wealth and loss of relatives and no ease to body, loss of money, danger of stirring up enemies, sickness, anxiety, enmity of superiors and elders, anxiety and troubles through wife, scandals, change of residence, diseases of skin, danger from thieves and poison, ill-health to father and mother, suffering from hunger.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Illness to wife, loss of relatives, suffering from stomach ache, loss or property, sickness of a feverish nature, danger from fire, subject to swellings or eruptions, eye troubles, mind filled with cares, public criticism or displeasure, dishonor, danger to father, mother and children, scandals among equals, eating of prohibited food, bad acts, bad company, loss of money and memory.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Mars:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Gain of money in bad ways, destruction of enemies, good reputation, long journey to foreign lands and peace of mind, blame, odium of ciders, quarrels with them, sufferings by diseases, heartache occasioned by one's own relatives, fever or other inflammatory affection, danger of fire, troubles through persons in position, many enemies.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Profit, acquisition of wealth, house renovated or some improvements effected in it, comforts of wealth, heavy sleep, ardent passion, enjoyment by the help or women.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Great heat, dislike of friends, annoyance from brothers and sisters, danger from rulers, failure of all undertakings, danger of hurts according to the sign held by Kuja, trouble with superiors and some anxiety through strangers, foreigners or people abroad and through warlike clan. Danger of open violence, quarrel with relations, loss of money, skin disease, consumption, loss of blood, fistula, and fissures in anus, loss of females and brothers, evil doings and boils.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Marriage or inclination to marriage, knowledge and fruits of knowledge, wealth, bodily evils disappear, slander, fear of insects, poisoned by animals and insects, gain of wealth by trade, abundance of houses, trouble from enemies and mental worries, service rendered to friends and relatives, new knowledge, success in litigation.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Loss of wealth, enemies, end the unfortunate period, favour from superiors and persons in position, gain of money, birth of children, auspicious celebrations, acquisition or wealth through holy people, freedom from illness, public reputation, ascendancy and happiness.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of property, gain of money, domestic happiness, successful love affairs, inclination towards religious observances and festivities, favourable associations, influenced by priests, skin eruptions, boils, pleasure from travelling, jewels to wife, clothing, money from relatives and brothers, odium.of females and their society, increase of intelligence, enjoyment of females and gain of money.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of money, diseases, loss of relatives and danger from arms or operation, illness leading to misery, evil threatened by enemies and robbers, disputes with rulers, loss of wealth, quarrel, disputes, litigation, loss of property, cutaneous effects. loss of office or position and much anxiety.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger from rulers and robbers, loss of wealth, success in evil pursuits, suffering from poisonous complaints, loss of relatives, danger from skin diseases, change of residence, some severe kind of cutaneous disease, journey to a foreign country, scandals, loss of cows and buffaloes, illness to wife, loss of memory, fear from insects and thieves, falls into well, fear from ghosts, affection of gonorrhea, fretting and";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Enmity and quarrels with low people, loss of money due to evil works, commission of signs, great sufferings due to troubles from relatives and brothers and opposition of bad people, family disputes, troubles with one's own kindred diseases, poisonous complaints, trouble through women, many enemies.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Mercury:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Pains in bead and stomach, enmity of people, loss of respect, danger of fire, anxieties, sickness to wire, troubles from enemies, many obstacles, troubles through superiors, acquisition of wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Loss of health, some swellings or hurts in the limbs, quarrels and troubles through women, many difficulties, gain of money through ladies and agriculture and trade, success, happiness, diseases, ill-will of enemies, miscarriage of every concern, risk from quadrupeds.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Disappearance of all dangers diseases or enemies, fame derived from acts of charity and beneficence, royal favour, danger from jaundice or bilious fever, affections of the blood, neuralgic pains and headaches, troubles through neighbors, sickness, wounds or hurts, quarrels, addiction to drinks, betting and prostitutes, boils and hurts of arms, travels in forests and villages, sorrows, royal disfavour, imprisonment.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Acquisition of beautiful house and apparel, money through relatives, success in every undertaking, the birth of a brother or sister, increase in family, gain in business, good mind charitable acts, learning of mathematics and arts.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Hatred of friends, relatives and elders, wealth, liable to diseases, acquisition of land and wealth, gain by trade, reputation, good happiness, good credit, benefits from superiors, birth of a child or marriage.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Observance of duty, conformable to religion and morality, acquisition of wealth, clothes and jewels, birth or good children, happiness in married state, relatives prosper, trade increases, knowledge gained, return from a long journey, if not married, betrothal in this period, health, ornaments, vehicles, house, money gained.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Bad luck. stranger to success and happiness, severe reversal, enmity, pain in the part governed by Saturn, downfall or disgrace to relatives, mind full of evil forebodings and distress. rear from diseases, loans, loss of children, destruction of family, scandals, troubles from foreigners, earnings through evil ways, acts of charity and beneficence, acquisition of wealth, material comforts through petty chiefs, failure in agricultural operations.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Intercourse with servants and prostitutes, skin diseases, sufferings from hot diseases, bad company and dirty meals. change or present position, fear and danger through foreigners, disputes concerning property, failure in litigation, evil dreams. headaches, sickness and loss of appetite. wealth from friends and relatives, happiness, new earnings.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Sorrow, disease, loss of work and Dharma, bilious sickness, aimless wandering, loss of property, misfortune to relatives, troubles through doctors, mental anxiety, trouble from relatives, mental agony, loss of comfort, dread of enemy, failure in business.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Jupiter:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Enemies, victory, ease, great diligence, coming in of wealth, royal favour and sound health, gain,good actions or fruits of good action, loss of bodily strength.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of prosperity, gain of fame and fortune, acquisition or property, benefits through children, sexual intercourse with beautiful women, good meals and clothing, success and birth or a female child or marriage to some male member in the family, gain of money.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disappointments and troubles of various kinds, loss by thefts, loss of near and dear relatives, inflammatory disease, transfer or leave, failure in hopes and business, wandering, high fever, great risks, loss of wealth and depression of mind, pilgrimage to temples, acquisition of wealth and fame, adventures.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of wealth, good and auspicious works in the house, communion with relatives, happy, increase of knowledge, acquisition of wealth through trade, favour from rulers, material comforts, perfect practice of hospitality, gain through knowledge in fine arts, birth of a well-favoured child, advantages from superiors.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Increase of property, domestic happiness, benefit from employment or occupation, birth of children, reputation, good meals, good deeds, health. royal favour, great diligence, success in all attempts, travels, dips in sacred rivers, pilgrimage, honour at stake if afflicted.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Appointment, wealth, reputation, gain of money, savings, development of sons and grandsons, jewels, good and delicious meals, marital happiness, auspicious works, reunion of the family, good success in profession or business, gain of land in the month of Taurus or Libra, much enjoyment, relatives, friends, peace or mind, acquisition or valuables, troubles from females and odium of public.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"A feeling of aversion, mental anguish, waste of wealth through sons, failure of business, increase of wealth and prosperity, pain in the body, rheumatic pains in limbs, trouble through wife or partners, failure in profit and credit, sorrows, fears, enmity of friends and relatives, adultery, unrighteous, a witness in court, quarrels in family, mental depression, funeral ceremonies for others,";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Income through low-caste people, apprehension of diseases, possibility of every possible calamity, deprivation of wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Pilgrimages to holy shrines, increase of wealth, suffering for the sake of several seniors and rulers, death of partner if in business, change of residence, separation from relatives and friends, may forsake business, poisonous effects, loss of wealth, destruction of work, illness, boils.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                        break;
                    }
                case Library.PlanetName.PlanetNameEnum.Venus:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Anxious about everything, prosperity collapses, troubles with wife, children, land, family, disputes and quarrels, diseases affecting head, belly and eyes, damage in respect of agriculture.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Gains of females, education, knowledge, money, children and vehicles, worship of God, accomplishments of desires, troubles through wife, domestic happiness afterwards, pain and disease due to inflammation of nervous tissue and from lust and other passions of human nature.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Flow of bile, disease of the eyes, great exertion, much income, acquisition or wealth, marriage, acquisition of lands, venereal diseases, danger from arms, exile in foreign places, atheistic tendencies, increase of property through the influence of females, negligence of duty, bent on pleasure and passion, temporary affection of eyes.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Association with prostitutes, enjoyments, knowledge, mathematical learning, success in litigations, inclination to learn music, piles and other hot ailments, pleasure through wife and children, increase of wealth, gain of knowledge in aru and sciences, wealth, royal favour, prosperity on a large scale and sound health.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Means of livelihood settled, gains from profession, benefits through superiors or employers or persons ruled by Jupiter fame, anxiety, quarrels with saints and religious men, gain of knowledge, end of dependence, worship of certain inferior natural forces, happiness and health, marriages, sexual intercourse, increase of family reputation and good deeds, wealth. ultimate happiness, wife and children suffer in the end.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Success, good servants and good many pleasures, money plentiful. disappearance of enemies, attainment of fame and birth of children.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Sexual intercourse with females advanced in age, accession to lands and wealth, disappearance of enemies, affection of excretory system, piles, etc., rheumatic pains in legs and bands, danger to eye sight, distaste for food, loss of appetite, physical condition poor, loss of money, wanderings, servitude, bolting and gambling, addiction to liquor, bad company, etc., ill-health, loss of memory, impotence.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Meditation, seclusion, quarrels among relatives and his people, entire change of surroundings, schemes of deception. miserliness, acquisition of lost property, dislike of relatives. evil from friends and injury by fire.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Discordance, death of relatives, injury inflicted by enemies, misgivings in heart, deprivation of wealth, troubles through wife, danger from quadrupeds, illness to partner or a member of the family, accidental blood poisoning, delirious fits, weakness in body and mind, gradual loss of wealth, loss of relatives, bad company, abode in seclusion, manifold sorrows, but happiness in the end.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Saturn:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of wife and children, trouble from rulers or robbers. sinking of heart, danger of blood poisoning, haemorrhage of the generative system, chronic poisoning, intestinal swellings, affliction of the eyes, sickness even to healthy children and wife, body full of pain and disorders, danger of death. fear of death to father-in-law.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Increase in cattle, enmity of friends and relatives, cold affections, troubles and sickness, family disputes, loss of money and property, reduction to great need, mortgage of property and its recovery after a lapse of time, death of a near relative, sorrow, dislike of relatives, coming in of money, windy diseases.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Some disgrace, serious enmity, strife, much blame, wanderings from place to place, unsettled life, many enemies, loss of money by fraud or theft, change of residence, serious illness, distress to brothers and friends, hot diseases.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Charitable works, gain of wealth, birth of children, increase of knowledge in some branch, prosperity to children, success to relatives, general prosperity, favours and approbation from superiors, increase of happiness, wealth and fame, benefits occurring from acts of piety and customary religious observances, agriculture and commerce.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Worship of gods and holy people, happiness to family, increase in bodily comforts, accomplishment of intentions by the help of superiors, increase of family circles, attainment of rank.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Auspicious, general happiness, attentions and favours from others, gifts, profits in business, increase of family members, victory over enemies, success in life, goodwill of relatives, accession to wife's property and wealth.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Brings on diseases, troubles and torments, much mental anguish, capacity of kings and free-hooters, loss of wealth, fear or poisonous effect to cattle, much sufferings to family, fever, wind or phlegm, bodily ailments and colic, body languishes, loss of money and children, serious enmities, dispute and troubles from relatives, blood and bilious complaints, quarrels in family, loss of money, mental derangement.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disease in every limb, loss of wealth by rulers, robbers and foes, danger of physical hurts, various physical troubles, fevers, enemies, increase of troubles.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Rheumatism or sickness, danger of poison, danger from sons, loss of money, contentions and quarrels with vile and wicked people, dread of evil dreams, quarrels in family.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Rahu:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Hot fevers, giddiness, fear and enmity of people, quarrels in family, benefits from persons in good position, fear and suspicion in connection with wife, children and relatives, change of position or residence, love of charitable acts, contentment, cessation of all violence and outrage of contagious diseases, success in examinations, private life happy, much reputation and fame, but mental unrest.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Abundance of enjoyments, good crops, coming in of money and communion with kith and kin, loss of relatives, loss of money through wife, pains in the limbs, change of position or residence, danger of personal hurts, unstability of health, sea voyages, gain of lands and money, loss or danger to wife and children.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger from rulers, fires or thieves and by arms, defeat in litigation, loss of money due to cousins, difficulties, sorrows, danger to the person due to malice of enemies, tendency to ease or dissolute habits, disputes and mental anxiety, combination of all possible calamities, bewilderment in every work and culpable failure of memory.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Many friends and relatives, wife and children, accession to wealth or royal favour. In the first 18 months of this period very busy, seriously inclined to marry. In the latter 12 months, enemies increase through his own action, happiness, birth of children, acquisition of vehicles, happiness to relatives and family, enjoyments with prostitutes, showy, gains through trade, fraudulent schemes.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Total disappearance of enemies and sickness, royal favour, acquisition of wealth, birth of children, increase of pleasure, gain through nobles or persons in power, benefits and comforts from superiors, success in all efforts, marriage in the house, increase of enemies, litigations and dips in sacred rivers.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Accession to vehicles and things of foreign land, troubles from foes, relatives and diseases, acquisition of wealth and other advantages, friendly alliances, wife a source of fortune and happiness, benefits from superiors or beads above in office, liable to deception, false friends, gain in land, birth of a child or marriage.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Scandal, danger due to fall of a tree, bad associations, divorce of wife or husband, incessant disputes and contests, rheumatism, biliousness, etc., throughout: disease due to wind and bile, distress of relatives, friends and well-wishers, residence in a remote foreign land.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disturbance in mind, anxieties, quarrels among relatives, death of partner, master or the head of the family, mental anxiety, danger of poisoning, transfer, all sorts of scandals and quarrels, fever, bites of insects or wounds by arms, death of relatives, going to court as witness, quarrels with parents, diseases, illness to wife, failure of intellect, loss of wealth, wandering in far-off countries and distress there.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Danger, disease in the anus, no good and timely meals, epidemic diseases, danger of physical hurts and poison, ill-health to children, some swellings in the body, troubles through wife, danger from superiors, loss of wealth and honour, loss of children, death of cattle and misfortunes of all kinds.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                case Library.PlanetName.PlanetNameEnum.Ketu:
                    {
                        switch (minorPlanet.Name)
                        {
                            case Library.PlanetName.PlanetNameEnum.Sun:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disappointment, physical pain, exile in foreign country, peril and obstruction in every business, increase of knowledge, sickness in family, long journey and return, anxiety about wife's health.";
                                    break;
                                }
                            case Library.PlanetName.PlanetNameEnum.Moon:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Disputes about fair sex, trouble through children, gains and financial success, diseases of biliousness and cold, loss of relatives and money, destruction of wealth and distress of mind.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mars:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Odium of sons, wife and younger brothers, loss of relatives, trouble from diseases, foes and bad rulers, path of progress obstructed, fear and anxiety, disputes and contests of different kinds, enemies arise, danger of disputes and destruction through females, sufferings from fever, fear of robbers, death, imprisonment, urinary diseases, loss and difficulties and surgical operations.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Mercury:
                                {
                                    returnVal.eventNature = EventNature.Neutral;
                                    returnVal.desciption = @"Society of relatives, friends and the like, material gains from knowledge, danger from relatives, anxiety on account of children, failure in plans, deception, jealousy, falsehood, and knowledge.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Jupiter:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Exemption from ailments, acquisition of lands and birth of children, profitable transactions, association with people of good position, danger of poison, wife an object of pleasure, if unmarried marriage takes place.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Venus:
                                {
                                    returnVal.eventNature = EventNature.Good;
                                    returnVal.desciption = @"Wealth and happiness, birth of a child, efforts crowned with success, in the end sickness, wife ill, illness to children, quarrels, loss of relatives and friends, fever and dysentery.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Saturn:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of wife, danger from enemies, imprisonment, loss of wealth, indigestion, property in danger or ruin, heavy loss in different ways, change of residence, some cutaneous diseases. anxiety owing to sickness of partner misgivings in the heart, mental anguish, difference of opinion with relations, exile in foreign countries.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Rahu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Loss of lands, imprisonment, quarrel with friends, danger of blood poisoning, danger of ruin, loss of property, fame and honour, rear of kings and robbers, sorrow, ruin of all business, adultery with mean women.";
                                    return returnVal;
                                }
                            case Library.PlanetName.PlanetNameEnum.Ketu:
                                {
                                    returnVal.eventNature = EventNature.Bad;
                                    returnVal.desciption = @"Fear of death of wife or children, loss of wealth and happiness, mental troubles, separation from relatives, subject to some estrangement, restraint or detention, danger of poison.";
                                    return returnVal;
                                }
                            default:
                                throw new Exception($"Planet not accounted for! : {minorPlanet}");
                        }
                    }
                    break;
                default:
                    throw new Exception($"Planet not accounted for! : {majorPlanet}");
            }

            //return the specialized nature & description packaged together
            return returnVal;
        }

        /// <summary>
        /// Gets dasa counted from birth dasa
        /// </summary>
        public static int CurrentDasaCountFromBirth(Time birthTime, Time currentTime)
        {
            //get dasa planet at birth (birth time = current time)
            var birthDasaPlanet = CurrentDasa8Levels(birthTime, birthTime).PD1;

            var currentDasaPlanet = CurrentDasa8Levels(birthTime, currentTime).PD1;

            //count from birth dasa planet to current dasa planet
            var dasaCount = 1; //minimum 1

            //start with birth dasa planet,
            //incase current & birth dasa planet is same
            PlanetName nextPlanet = birthDasaPlanet;

        TryAgain:
            //planet found, stop counting
            if (nextPlanet == currentDasaPlanet)
            {
                return dasaCount;
            }
            //else planet not found,
            else
            {
                //change to next planet 
                nextPlanet = NextDasaPlanet(nextPlanet);
                //increase count
                dasaCount++;
                //try checking again if it is same planet
                goto TryAgain;
            }

        }

        /// <summary>
        /// The main method that starts all Dasa Calculations
        /// Gets the occuring Planet Dasas (PD1, PD2,...) for a person at the given time
        /// </summary>
        public static Dasas CurrentDasa8Levels(Time birthTime, Time currentTime)
        {
            //todo strength determined by constellation rules not shad or bhava bala
            //lagna lord or moon constellation used based on which is stronger
            //var isLagnaLordStronger = AstronomicalCalculator.LagnaLordVsMoonStrength(birthTime);
            //var finalConstellation = isLagnaLordStronger
            //    ? GetPlanetConstellation(birthTime, AstronomicalCalculator.GetLordOfHouse(HouseName.House1, birthTime)) 
            //    : GetMoonConstellation(birthTime);

            //get dasa planet at birth
            var moonConstellation = PlanetConstellation(birthTime, Library.PlanetName.Moon);
            //var risingConstellation = GetHouseConstellation(1, birthTime);
            var birthDasaPlanetMoon = ConstellationDasaPlanet(moonConstellation.GetConstellationName());
            //var birthDasaPlanet = GetConstellationDasaPlanet(risingConstellation.GetConstellationName());

            //get time traversed in birth dasa 
            var timeTraversedInDasa = YearsTraversedInBirthDasa(birthTime, moonConstellation);

            //get time from birth to current time (converted to Dasa years ie. 365.25 days per year)
            var timeBetween = currentTime.Subtract(birthTime).TotalDays / 365.25;

            //combine years traversed at birth and years to current time
            //this is done to easily calculate to current dasa, bhukti & antaram
            var combinedYears = timeTraversedInDasa + timeBetween;
            var wholeDasa = DasaCountedFromInputDasa(birthDasaPlanetMoon, combinedYears);

            return wholeDasa;
        }


        /// <summary>
        /// Counts from inputed dasa by years to dasa, bhukti & antaram
        /// Inputed planet is taken as birth dasa ("starting dasa" to count from)
        /// Note: It is easier to calculate from start of Dasa,
        ///       so years already traversed at birth must be added into inputed years
        /// Exp: Get dasa, bhukti & antaram planet 3.5 years from start of Sun dasa
        /// </summary>
        public static Dasas DasaCountedFromInputDasa(PlanetName startDasaPlanet, double years)
        {
            double pd1Years = years;
            double pd2Years; //will be filled when getting dasa
            double pd3Years; //will be filled when getting bhukti
            double pd4Years; //will be filled when getting antaram
            double pd5Years; //will be filled when getting prana
            double pd6Years; //will be filled when getting prana
            double pd7Years; //will be filled when getting prana
            double pd8Years; //will be filled when getting prana

            //NOTE: Get Dasa prepares values for Get Bhukti and so on.

            //first get the major dasa planet
            //then based on the pd2 get pd3 and so on in same pattern
            var pd1Planet = GetPD1();
            var pd2Planet = GetPD2();
            var pd3Planet = GetPD3();
            var pd4Planet = GetPD4();
            var pd5Planet = GetPD5();
            var pd6Planet = GetPD6();
            var pd7Planet = GetPD7();
            var pd8Planet = GetPD8();


            return new Dasas()
            {
                PD1 = pd1Planet,
                PD2 = pd2Planet,
                PD3 = pd3Planet,
                PD4 = pd4Planet,
                PD5 = pd5Planet,
                PD6 = pd6Planet,
                PD7 = pd7Planet,
                PD8 = pd8Planet
            };


            //--------------------------------LOCAL FUNCTIONS

            PlanetName GetPD8()
            {
                //first possible PD8 planet is the PD7 planet
                var possiblePD8Planet = pd7Planet;

            //minus the possible PD8 planet's full years
            MinusPD8Years:
                var pd8PlanetFullYears = PD8PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, pd7Planet, possiblePD8Planet);
                pd8Years -= pd8PlanetFullYears;

                //if remaining pd8 time is negative,
                //than current possible PD7 planet is correct
                if (pd8Years <= 0)
                {
                    //return possible planet as correct
                    return possiblePD8Planet;
                }
                //else possible pd8 planet not correct, go to next one 
                else
                {
                    //change to next pd8 planet in order
                    possiblePD8Planet = NextDasaPlanet(possiblePD8Planet);
                    //go back to minus this planet's years
                    goto MinusPD8Years;
                }

            }

            PlanetName GetPD7()
            {
                //first possible PD7 planet is the PD6 planet
                var possiblePD7Planet = pd6Planet;

            //minus the possible PD7 planet's full years
            MinusPD7Years:
                var pd7PlanetFullYears = PD7PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, possiblePD7Planet);
                pd7Years -= pd7PlanetFullYears;

                //if remaining pd7 time is negative,
                //than current possible PD6 planet is correct
                if (pd7Years <= 0)
                {
                    //get back the PD7 time before it becomes negative
                    //this is the time inside the current PD7, aka PD8 time
                    //save it for late use
                    pd8Years = pd7Years + pd7PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD7Planet;
                }
                //else possible pd7 planet not correct, go to next one 
                else
                {
                    //change to next pd7 planet in order
                    possiblePD7Planet = NextDasaPlanet(possiblePD7Planet);
                    //go back to minus this planet's years
                    goto MinusPD7Years;
                }

            }

            PlanetName GetPD6()
            {
                //first possible PD6 planet is the PD4 planet
                var possiblePD6Planet = pd5Planet;

            //minus the possible PD6 planet's full years
            MinusPD6Years:
                var pd6PlanetFullYears = PD6PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, possiblePD6Planet);
                pd6Years -= pd6PlanetFullYears;

                //if remaining pd6 time is negative,
                //than current possible PD5 planet is correct
                if (pd6Years <= 0)
                {
                    //get back the PD6 time before it becomes negative
                    //this is the time inside the current PD6, aka PD7 time
                    //save it for late use
                    pd7Years = pd6Years + pd6PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD6Planet;
                }
                //else possible pd6 planet not correct, go to next one 
                else
                {
                    //change to next pd6 planet in order
                    possiblePD6Planet = NextDasaPlanet(possiblePD6Planet);
                    //go back to minus this planet's years
                    goto MinusPD6Years;
                }

            }

            PlanetName GetPD5()
            {
                //first possible PD5 planet is the PD4 planet
                var possiblePD5Planet = pd4Planet;

            //minus the possible PD5 planet's full years
            MinusPD5Years:
                var pd5PlanetFullYears = PD5PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, possiblePD5Planet);
                pd5Years -= pd5PlanetFullYears;

                //if remaining pd5 time is negative,
                //than current possible PD4 planet is correct
                if (pd5Years <= 0)
                {
                    //get back the PD5 time before it becomes negative
                    //this is the time inside the current PD5, aka PD6 time
                    //save it for late use
                    pd6Years = pd5Years + pd5PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD5Planet;
                }
                //else possible pd5 planet not correct, go to next one 
                else
                {
                    //change to next pd5 planet in order
                    possiblePD5Planet = NextDasaPlanet(possiblePD5Planet);
                    //go back to minus this planet's years
                    goto MinusPD5Years;
                }

            }

            PlanetName GetPD4()
            {
                //first possible pd4 planet is the antaram planet
                var possiblePD4Planet = pd3Planet;

            //minus the possible pd4 planet's full years
            MinusPD4Years:
                var pd4PlanetFullYears = PD4PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, possiblePD4Planet);
                pd4Years -= pd4PlanetFullYears;

                //if remaining pd4 years is negative,
                //than current possible pd4 planet is correct
                if (pd4Years <= 0)
                {
                    //get back the PD4 time before it becomes negative
                    //this is the time inside the current PD4, aka PD5 time
                    //save it for late use
                    pd5Years = pd4Years + pd4PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD4Planet;
                }
                //else possible pd4 planet not correct, go to next one 
                else
                {
                    //change to next pd4 planet in order
                    possiblePD4Planet = NextDasaPlanet(possiblePD4Planet);
                    //go back to minus this planet's years
                    goto MinusPD4Years;
                }

            }

            PlanetName GetPD3()
            {
                //first possible antaram planet is the bhukti planet
                var possibleAntaramPlanet = pd2Planet;

            //minus the possible antaram planet's full years
            MinusPD3Years:
                var antaramPlanetFullYears = PD3PlanetFullYears(pd1Planet, pd2Planet, possibleAntaramPlanet);
                pd3Years -= antaramPlanetFullYears;

                //if remaining antaram years is negative,
                //than current possible antaram planet is correct
                if (pd3Years <= 0)
                {
                    //get back the antaram time before it became negative
                    //this is the time inside the current antaram, aka Sukshma time
                    //save it for late use
                    pd4Years = pd3Years + antaramPlanetFullYears;

                    //return possible planet as correct
                    return possibleAntaramPlanet;
                }
                //else possible antaram planet not correct, go to next one 
                else
                {
                    //change to next antaram planet in order
                    possibleAntaramPlanet = NextDasaPlanet(possibleAntaramPlanet);
                    //go back to minus this planet's years
                    goto MinusPD3Years;
                }


            }

            PlanetName GetPD2()
            {
                //first possible pd2 planet is the major Dasa planet
                var possiblePD2Planet = pd1Planet;

            //minus the possible pd2 planet's full years
            MinusPD2Years:
                var pd2PlanetFullYears = PD2PlanetFullYears(pd1Planet, possiblePD2Planet);
                pd2Years -= pd2PlanetFullYears;

                //if remaining pd2 years is negative,
                //than current possible pd2 planet is correct
                if (pd2Years <= 0)
                {
                    //get back the pd2 years before it became negative
                    //this is the years inside the current pd2, aka antaram years
                    //save it for late use
                    pd3Years = pd2Years + pd2PlanetFullYears;

                    //return possible planet as correct
                    return possiblePD2Planet;
                }
                //else possible pd2 planet not correct, go to next one 
                else
                {
                    //change to next pd2 planet in order
                    possiblePD2Planet = NextDasaPlanet(possiblePD2Planet);
                    //go back to minus this planet's years
                    goto MinusPD2Years;
                }

            }

            PlanetName GetPD1()
            {
                //possible planet starts with the inputed one
                var possibleDasaPlanet = startDasaPlanet;

            //minus planet years
            MinusPD1Years:
                var dasaPlanetFullYears = PD1PlanetFullYears(possibleDasaPlanet);
                pd1Years -= dasaPlanetFullYears;

                //if remaining dasa years is negative,
                //than possible dasa planet is correct
                if (pd1Years <= 0)
                {
                    //get back the dasa years before it became negative
                    //this is the years inside the current dasa, aka bhukti years
                    //save it for late use
                    pd2Years = pd1Years + dasaPlanetFullYears;

                    //return possible planet as correct
                    return possibleDasaPlanet;
                }
                //else possible dasa planet not correct, go to next one 
                else
                {
                    //change to next dasa planet in order
                    possibleDasaPlanet = NextDasaPlanet(possibleDasaPlanet);
                    //go back to minus this planet's years
                    goto MinusPD1Years;
                }
            }

        }


        /// <summary>
        /// Gets next planet in Dasa order
        /// This order is also used for Bhukti & Antaram
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static PlanetName NextDasaPlanet(PlanetName planet)
        {
            if (planet == Library.PlanetName.Sun) { return Library.PlanetName.Moon; }
            if (planet == Library.PlanetName.Moon) { return Library.PlanetName.Mars; }
            if (planet == Library.PlanetName.Mars) { return Library.PlanetName.Rahu; }
            if (planet == Library.PlanetName.Rahu) { return Library.PlanetName.Jupiter; }
            if (planet == Library.PlanetName.Jupiter) { return Library.PlanetName.Saturn; }
            if (planet == Library.PlanetName.Saturn) { return Library.PlanetName.Mercury; }
            if (planet == Library.PlanetName.Mercury) { return Library.PlanetName.Ketu; }
            if (planet == Library.PlanetName.Ketu) { return Library.PlanetName.Venus; }
            if (planet == Library.PlanetName.Venus) { return Library.PlanetName.Sun; }

            //if no plant found something wrong
            throw new Exception("Planet not found!");

        }

        /// <summary>
        ///  Gets years left in birth dasa at birth
        ///  Note : Returned years can only be 0 or above
        ///  Start constellation can be of moon or Lagna
        /// </summary>
        public static double TimeLeftInBirthDasa(Time birthTime, PlanetConstellation startConstellation)
        {
            //get years already passed in birth dasa
            var yearsTraversed = YearsTraversedInBirthDasa(birthTime, startConstellation);

            //get full years of birth dasa planet
            var birthDasaPlanet = ConstellationDasaPlanet(startConstellation.GetConstellationName());
            var fullYears = PD1PlanetFullYears(birthDasaPlanet);

            //calculate the years left in birth dasa at birth
            var yearsLeft = fullYears - yearsTraversed;

            //raise error if years traversed is more than full years
            if (yearsLeft < 0) { throw new Exception("Dasa years traversed is more than full years!"); }

            return yearsLeft;
        }

        /// <summary>
        /// Gets the time in years traversed in Dasa at birth
        /// Start constellation can of Moon's or Lagna lord
        /// </summary>
        public static double YearsTraversedInBirthDasa(Time birthTime, PlanetConstellation startConstellation)
        {
            //get longitude minutes the Moon/Lagna already traveled in the constellation 
            var minutesTraversed = startConstellation.GetDegreesInConstellation().TotalMinutes;

            //get the time period each minute represents
            var timePerMinute = DasaTimePerMinute(startConstellation.GetConstellationName());

            //calculate the years already traversed
            var traversedYears = minutesTraversed * timePerMinute;

            return traversedYears;
        }


        /// <summary>
        /// Gets the Dasa time period each longitude minute in a constellation represents,
        /// based on the planet which is related (lord) to it.
        /// Note: Returns the time in years, exp 0.5 = half year
        /// </summary>
        public static double DasaTimePerMinute(ConstellationName constellationName)
        {
            //maximum longitude minutes of a constellation
            const double maxMinutes = 800.0;

            //get the related (lord) planet for the constellation
            var relatedPlanet = ConstellationDasaPlanet(constellationName);

            //get the full Dasa years for the related planet
            var fullYears = PD1PlanetFullYears(relatedPlanet);

            //calculate the time in years each longitude minute represents
            var timePerMinute = fullYears / maxMinutes;

            return timePerMinute;
        }

        /// <summary>
        /// Gets the related (lord) Dasa planet for a given constellation
        /// Used to find the ruling Dasa Planet
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static PlanetName ConstellationDasaPlanet(ConstellationName constellationName)
        {
            switch (constellationName)
            {
                case ConstellationName.Krithika:
                case ConstellationName.Uttara:
                case ConstellationName.Uttarashada:
                    return Library.PlanetName.Sun;

                case ConstellationName.Rohini:
                case ConstellationName.Hasta:
                case ConstellationName.Sravana:
                    return Library.PlanetName.Moon;

                case ConstellationName.Mrigasira:
                case ConstellationName.Chitta:
                case ConstellationName.Dhanishta:
                    return Library.PlanetName.Mars;

                case ConstellationName.Aridra:
                case ConstellationName.Swathi:
                case ConstellationName.Satabhisha:
                    return Library.PlanetName.Rahu;

                case ConstellationName.Punarvasu:
                case ConstellationName.Vishhaka:
                case ConstellationName.Poorvabhadra:
                    return Library.PlanetName.Jupiter;

                case ConstellationName.Pushyami:
                case ConstellationName.Anuradha:
                case ConstellationName.Uttarabhadra:
                    return Library.PlanetName.Saturn;

                case ConstellationName.Aslesha:
                case ConstellationName.Jyesta:
                case ConstellationName.Revathi:
                    return Library.PlanetName.Mercury;

                case ConstellationName.Makha:
                case ConstellationName.Moola:
                case ConstellationName.Aswini:
                    return Library.PlanetName.Ketu;

                case ConstellationName.Pubba:
                case ConstellationName.Poorvashada:
                case ConstellationName.Bharani:
                    return Library.PlanetName.Venus;
            }

            //if it reaches here something wrong
            throw new Exception("Dasa planet for constellation not found!");
        }




        /// <summary>
        /// Gets the full Dasa years for a given planet
        /// Note: Returns "double" so that division down the road is accurate
        /// Ref:Hindu Predictive Astrology pg. 54
        /// </summary>
        public static double PD1PlanetFullYears(PlanetName planet)
        {

            if (planet == Library.PlanetName.Sun) { return 6.0; }
            if (planet == Library.PlanetName.Moon) { return 10.0; }
            if (planet == Library.PlanetName.Mars) { return 7.0; }
            if (planet == Library.PlanetName.Rahu) { return 18.0; }
            if (planet == Library.PlanetName.Jupiter) { return 16.0; }
            if (planet == Library.PlanetName.Saturn) { return 19.0; }
            if (planet == Library.PlanetName.Mercury) { return 17.0; }
            if (planet == Library.PlanetName.Ketu) { return 7.0; }
            if (planet == Library.PlanetName.Venus) { return 20.0; }

            //if no plant found something wrong
            throw new Exception("Planet not found!");

        }

        /// <summary>
        /// Gets the full years of a bhukti planet in a dasa
        /// </summary>
        public static double PD2PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time a bhukti planet consumes in a dasa is
            //a fixed percentage it consumes in a person's full life
            var bhuktiPlanetPercentage = PD1PlanetFullYears(pd2Planet) / fullHumanLifeYears;

            //bhukti planet's years in a dasa is percentage of the dasa planet's full years
            var bhuktiPlanetFullYears = bhuktiPlanetPercentage * PD1PlanetFullYears(pd1Planet);

            //return the calculated value
            return bhuktiPlanetFullYears;

        }

        /// <summary>
        /// Gets the full years of an antaram planet in a bhukti of a dasa
        /// </summary>
        public static double PD3PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an antaram planet consumes in a bhukti is
            //a fixed percentage it consumes in a person's full life
            var antaramPlanetPercentage = PD1PlanetFullYears(pd3Planet) / fullHumanLifeYears;

            //Antaram planet's full years is a percentage of the Bhukti planet's full years
            var antaramPlanetFullYears = antaramPlanetPercentage * PD2PlanetFullYears(pd1Planet, pd2Planet);

            //return the calculated value
            return antaramPlanetFullYears;

        }

        /// <summary>
        /// Gets the full time of an Sukshma planet 
        /// Sukshma is a Sanskrit word meaning "subtle" or "dormant." The presence of sukshma is felt, but not seen.
        /// </summary>
        public static double PD4PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an sukshma planet consumes in a antaram is
            //a fixed percentage it consumes in a person's full life
            var sukshmaPlanetPercentage = PD1PlanetFullYears(pd4Planet) / fullHumanLifeYears;

            //sukshma planet's full years is a percentage of the Antaram planet's full years
            var sukshmaPlanetFullYears = sukshmaPlanetPercentage * PD3PlanetFullYears(pd1Planet, pd2Planet, pd3Planet);

            //return the calculated value
            return sukshmaPlanetFullYears;

        }

        /// <summary>
        /// Gets the full time of an Prana planet 
        /// </summary>
        public static double PD5PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD5 planet consumes in a PD4 is
            //a fixed percentage it consumes in a person's full life
            var pd5PlanetPercentage = PD1PlanetFullYears(pd5Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd5PlanetFullTime = pd5PlanetPercentage * PD4PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet);

            //return the calculated value
            return pd5PlanetFullTime;

        }

        public static double PD6PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD6 planet consumes in a PD5 is
            //a fixed percentage it consumes in a person's full life
            var pd6PlanetPercentage = PD1PlanetFullYears(pd6Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd6PlanetFullTime = pd6PlanetPercentage * PD5PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet);

            //return the calculated value
            return pd6PlanetFullTime;

        }

        public static double PD7PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet, PlanetName pd7Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD7 planet consumes in a PD6 is
            //a fixed percentage it consumes in a person's full life
            var pd7PlanetPercentage = PD1PlanetFullYears(pd7Planet) / fullHumanLifeYears;

            //Prana planet's full time is a percentage of the Sukshma planet's full time
            var pd7PlanetFullTime = pd7PlanetPercentage * PD6PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet);

            //return the calculated value
            return pd7PlanetFullTime;

        }

        public static double PD8PlanetFullYears(PlanetName pd1Planet, PlanetName pd2Planet, PlanetName pd3Planet, PlanetName pd4Planet, PlanetName pd5Planet, PlanetName pd6Planet, PlanetName pd7Planet, PlanetName pd8Planet)
        {
            //120 years is the total of all the dasa planet's years
            const double fullHumanLifeYears = 120.0;

            //the time an PD8 planet consumes in a PD7 is
            //a fixed percentage it consumes in a person's full life
            var pd8PlanetPercentage = PD1PlanetFullYears(pd8Planet) / fullHumanLifeYears;

            //PD8 planet's full time is a percentage of the Sukshma planet's full time
            var pd8PlanetFullTime = pd8PlanetPercentage * PD7PlanetFullYears(pd1Planet, pd2Planet, pd3Planet, pd4Planet, pd5Planet, pd6Planet, pd7Planet);

            //return the calculated value
            return pd8PlanetFullTime;

        }


        #endregion

        #region PLANET BENEFIC & MALEFIC CALCULATION

        /// <summary>
        /// Whenever an affiiction by way of a malefic occupying
        /// a certain house or joining with a certain planet
        /// is suggested, by implication an aspect is also meant,
        /// though an affliction caused by aspect.is comparatively less malevolent
        ///
        /// Note:
        /// TODO presently not 100% sure, if what is meant by "affliction" is solely only limited to
        /// aspects & conjunction with bad planets. Or
        /// -Located in enemy sign an affliction?
        /// -Low shadbala an affliction?
        /// -Low drikbala an affliction?
        ///
        /// 
        /// At present, malefic aspects & conjunctions are used
        /// becasue it seems based on texts that this is correct.
        /// 
        /// But it seems mercury in enemny sign or position in a house should also play a role.
        /// 
        /// There must be a corelation between shadbala or drikbala to aspects & conjucntion
        /// A more precise way of mesurement it could be via the bala method.
        /// Needs testing for sure, to find out what bala values determine an afflicted mercury
        ///
        /// </summary>
        /// TODO POSSIBLE RENAME TO is IsMercuryMalefic
        public static bool IsMercuryAfflicted(Time time)
        {
            //for now only malefic conjunctions are considered
            return IsMercuryMalefic(time);

        }

        /// <summary>
        /// Check if Mercury is malefic (true), returns false if benefic 
        /// 
        ///
        /// References:
        /// 
        /// "Mercury, by nature, is called sournya or good. And if he is in
        /// conjunction with the Sun, Saturn, Mars, Rahu or Ketu, he will
        /// be a malefic. His conjunction with beneficial planets like Full
        /// Moon, Jupiter or Venus will classify him as a benefic. Benefic
        /// means a good and malefic means an evil planet."
        /// -TODO Does malefic moon make it malefic? (atm malefic moon makes it malefic)
        ///
        /// "Though in the earlier pages Mercury is defined either as a subba
        /// (benefic) or papa (malefic) according to its association is with a benefic or
        /// malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
        /// be deemed as a full benefic. This is in accord with the injunctions of
        /// classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).
        /// "
        ///11. Benefics and Malefics. Among these, Sūrya, Śani, Mangal, decreasing Candr, Rahu and
        /// Ketu (the ascending and the descending nodes of Candr) are malefics, while the rest are
        /// benefics. Budh, however, is a malefic, if he joins a malefic. 
        /// 
        /// Note:
        /// ATM malefic planets override benefic
        /// TODO not sure if malefic planet overrides benefic if both are conjunct
        /// </summary>
        public static bool IsMercuryMalefic(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(IsMercuryMalefic), time, Ayanamsa), _isMercuryMalefic);


            //UNDERLYING FUNCTION
            bool _isMercuryMalefic()
            {
                //if mercury is already with malefics,then not checking if conjunct with benefic (not 100% sure)
                if (conjunctWithMalefic()) { return true; }

                //if conjunct with benefic, then it is benefic
                if (conjunctWithBenefic()) { return false; }

                //if not conjunct with any planet, should be benefic
                //NOTE : Mercury, by nature, is called sournya or good.
                return false; // false means not malefic


                //------------FUNCTIONS-------------

                bool conjunctWithMalefic()
                {
                    //list the planets that will make mercury malefic
                    var evilPlanetNameList = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

                    //if moon is malefic, add to malefic list
                    if (!IsMoonBenefic(time)) { evilPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(time, Library.PlanetName.Mercury);

                    //mark evil planet not in conjunct at first
                    bool evilPlanetFoundInConjunct = false;

                    //check if evil planets are in conjunct
                    foreach (var planetName in evilPlanetNameList)
                    {
                        evilPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one evil planet is found, break loop, stop looking
                        if (evilPlanetFoundInConjunct) { break; }
                    }

                    //return flag of evil planets found in conjunct
                    return evilPlanetFoundInConjunct;

                }

                bool conjunctWithBenefic()
                {
                    var beneficPlanetNameList = new List<PlanetName>() { Library.PlanetName.Jupiter, Library.PlanetName.Venus };

                    //if moon is benefic, add to benefic list
                    if (IsMoonBenefic(time)) { beneficPlanetNameList.Add(Library.PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = Calculate.PlanetsInConjuction(time, Library.PlanetName.Mercury);

                    //mark benefic planet not in conjunct at first
                    bool beneficPlanetFoundInConjunct = false;

                    //check if benefic planets are in conjunct
                    foreach (var planetName in beneficPlanetNameList)
                    {
                        beneficPlanetFoundInConjunct = planetsConjunct.Contains(planetName);

                        //if one good planet is found, break loop, stop looking
                        if (beneficPlanetFoundInConjunct) { break; }
                    }

                    //return flag of benefic planets found in conjunct
                    return beneficPlanetFoundInConjunct;

                }

            }


        }

        /// <summary>
        /// Moon is a benefic from the 8th day of the bright half of the lunar month
        /// to the 8th day of the dark half of the lunar month
        /// and a malefic in the rest of the days.
        /// 
        /// Returns true if benefic & false if malefic
        /// </summary>
        public static bool IsMoonBenefic(Time time)
        {
            //get the lunar date number
            int lunarDateNumber = Calculate.LunarDay(time).GetLunarDateNumber();

            //8th day of the bright half = 8th lunar date
            //8th day of the dark half  = 23rd lunar date
            if (lunarDateNumber >= 8 && lunarDateNumber <= 23)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Checks if a given planet is benefic
        /// </summary>
        public static bool IsPlanetBenefic(PlanetName planetName, Time time)
        {
            //get list of benefic planets
            var beneficPlanetList = BeneficPlanetList(time);

            //check if input planet is in the list
            var planetIsBenefic = beneficPlanetList.Contains(planetName);

            return planetIsBenefic;
        }

        /// <summary>
        /// Gets all planets that are benefics at a given time, since moon & mercury changes
        /// Benefics, on the other hand, tend to do good ; but
        /// sometimes they also become capable of doing harm.
        /// </summary>
        public static List<PlanetName> BeneficPlanetList(Time time)
        {
            //Add permanent good planets to list first
            var listOfGoodPlanets = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Venus };

            //check if moon is benefic
            var moonIsBenefic = IsMoonBenefic(time);

            //if moon is benefic add to benefic list
            if (moonIsBenefic) { listOfGoodPlanets.Add(PlanetName.Moon); }

            //check if mercury is good
            var mercuryIsBenefic = IsMercuryMalefic(time) == false;

            //if mercury is benefic add to benefic list
            if (mercuryIsBenefic) { listOfGoodPlanets.Add(PlanetName.Mercury); }

            return listOfGoodPlanets;
        }

        /// <summary>
        /// Checks if a given planet is Malefic
        /// </summary>
        public static bool IsPlanetMalefic(PlanetName planetName, Time time)
        {
            //get list of malefic planets
            var maleficPlanetList = MaleficPlanetList(time);

            //check if input planet is in the list
            var planetIsMalefic = maleficPlanetList.Contains(planetName);

            return planetIsMalefic;
        }

        /// <summary>
        /// Gets list of permanent malefic planets,
        /// for moon & mercury it is based on changing factors
        ///
        /// Malefics are always inclined to do harm, but under certain
        /// conditions, the intensity of the mischief is tempered.
        /// </summary>
        public static List<PlanetName> MaleficPlanetList(Time time)
        {
            //Add permanent evil planets to list first
            var listOfEvilPlanets = new List<PlanetName>() { Library.PlanetName.Sun, Library.PlanetName.Saturn, Library.PlanetName.Mars, Library.PlanetName.Rahu, Library.PlanetName.Ketu };

            //check if moon is evil
            var moonIsEvil = IsMoonBenefic(time) == false;

            //if moon is evil add to evil list
            if (moonIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Moon);
            }

            //check if mercury is evil
            var mercuryIsEvil = IsMercuryMalefic(time);
            //if mercury is evil add to evil list
            if (mercuryIsEvil)
            {
                listOfEvilPlanets.Add(Library.PlanetName.Mercury);
            }

            return listOfEvilPlanets;
        }

        /// <summary>
        /// Gets all planets the inputed planet is transmitting aspect to
        /// </summary>
        public static List<PlanetName> PlanetsInAspect(PlanetName inputPlanet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(inputPlanet, time);

            //get all the planets located in these signs
            var planetsAspected = new List<PlanetName>();
            foreach (var zodiacSign in signAspecting)
            {
                var planetInSign = Calculate.PlanetInSign(zodiacSign, time);
                //add to list
                planetsAspected.AddRange(planetInSign);
            }


            //return these planets as aspected by input planet
            return planetsAspected;

        }

        /// <summary>
        /// Gets all planets the transmitting aspect to inputed planet
        /// </summary>
        public static List<PlanetName> PlanetsAspectingPlanet(Time time, PlanetName receivingAspect)
        {
            //check if all planets is aspecting inputed planet
            var aspectFound = Library.PlanetName.All9Planets.FindAll(transmitPlanet => IsPlanetAspectedByPlanet(receivingAspect, transmitPlanet, time));

            return aspectFound;
        }

        /// <summary>
        /// Gets houses aspected by the inputed planet
        /// </summary>
        public static List<HouseName> HousesInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = Calculate.SignsPlanetIsAspecting(planet, time);

            //get all the houses located in these signs
            var housesAspected = new List<HouseName>();
            foreach (var house in Library.House.AllHouses)
            {
                //get sign of house
                var houseSign = Calculate.HouseSignName(house, time);

                //add house to list if sign is aspected by planet
                if (signAspecting.Contains(houseSign)) { housesAspected.Add(house); }
            }

            //return the houses aspected by input planet
            return housesAspected;

        }

        /// <summary>
        /// Gets all planets aspecting inputed house
        /// </summary>
        public static List<PlanetName> PlanetsAspectingHouse(HouseName inputHouse, Time time)
        {
            //create empty list
            var returnPlanetList = new List<PlanetName>();

            //check each planet if aspecting house
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get houses
                var housesInAspect = HousesInAspect(planet, time);

                //check if any house is a match
                var houseMatch = housesInAspect.FindAll(house => house == inputHouse).Any();
                if (houseMatch)
                {
                    returnPlanetList.Add(planet);
                }
            }


            return returnPlanetList;
        }

        /// <summary>
        /// Checks if the a planet is aspected by another planet
        /// </summary>
        public static bool IsPlanetAspectedByPlanet(PlanetName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get planets aspected by transmiting planet
            var planetsInAspect = Calculate.PlanetsInAspect(transmitingAspect, time);

            //if receiving planet is in list of currently aspected
            return planetsInAspect.Contains(receiveingAspect);

        }

        /// <summary>
        /// Checks if a house is aspected by a planet
        /// </summary>
        public static bool IsHouseAspectedByPlanet(HouseName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get houses aspected by transmiting planet
            var houseInAspect = Calculate.HousesInAspect(transmitingAspect, time);

            //if receiving house is in list of currently aspected
            return houseInAspect.Contains(receiveingAspect);

        }

        /// <summary>
        /// Checks if the a planet is conjunct with another planet
        ///
        /// Note:
        /// Both planets A & B are checked if they are in conjunct with each other,
        /// performance might be effected mildly, but errors in conjunction calculation would be caught here.
        /// Can be removed once, conjunction calculator is confirmed accurate.
        /// </summary>
        public static bool IsPlanetConjunctWithPlanet(PlanetName planetA, PlanetName planetB, Time time)
        {
            //get planets in conjunt for A & B
            var planetAConjunct = Calculate.PlanetsInConjuction(time, planetA);
            var planetBConjunct = Calculate.PlanetsInConjuction(time, planetB);

            //check that A conjuncts B and B conjuncts A 
            var conjunctFound = planetAConjunct.Contains(planetB) && planetBConjunct.Contains(planetA);

            //erro check, can be removed upon corectness confirmation
            if (planetAConjunct.Contains(planetB) != planetBConjunct.Contains(planetA))
            {
                throw new Exception("Conjunct planet not uniform!");
            }

            return conjunctFound;
        }

        #endregion

        #region PLANET AND HOUSE STRENGHT CALCULATORS

        /// <summary>
        /// Returns an array of all planets sorted by strenght,
        /// 0 index being strongest to 8 index being weakest
        ///
        /// Note:
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// </summary>
        public static List<PlanetName> AllPlanetOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllPlanetOrderedByStrength), time, Ayanamsa), _getAllPlanetOrderedByStrength);


            //UNDERLYING FUNCTION
            List<PlanetName> _getAllPlanetOrderedByStrength()
            {
                var planetStrenghtList = new Dictionary<PlanetName, double>();

                //create a list with planet names & its strength (unsorted)
                foreach (var planet in Library.PlanetName.All9Planets)
                {
                    //get planet strength in rupas
                    var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                    //place in list with planet name
                    planetStrenghtList.Add(planet, strength);
                }


                //sort that list from strongest planet to weakest planet
                var sortedList = planetStrenghtList.OrderByDescending(item => item.Value);
                var nameOnlyList = sortedList.Select(x => x.Key).ToList();

                return nameOnlyList;

                /*--------------FUNCTIONS----------------*/
            }
        }

        /// <summary>
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// Powerful Planets.-Ravi is befd to be
        /// powerful when hi~Shadbala Pinda is 5 or more
        /// rupas. Chandra becomes strong when his Shadbala
        /// Pinda is 6 or more rupas. Kuja becomes powerful
        /// when bis Shadbala Pinda does not fall short of
        /// 5 rupas.Budha becomes potent by having his
        /// Sbadbala Pinda as 7 rupas; Guru, Sukra and Sani
        /// become thoroughly powerful if their Shadbala
        /// Pindas are 6.5, 5.5 and 5 rupas or more respectively.
        /// </summary>
        public static bool IsPlanetBeneficInShadbala(PlanetName planet, Time time)
        {

            var limit = 0.0;
            if (planet == Library.PlanetName.Sun) { limit = 5; }
            else if (planet == Library.PlanetName.Moon) { limit = 6; }
            else if (planet == Library.PlanetName.Mars) { limit = 5; }
            else if (planet == Library.PlanetName.Mercury) { limit = 7; }
            else if (planet == Library.PlanetName.Jupiter) { limit = 6.5; }
            else if (planet == Library.PlanetName.Venus) { limit = 5.5; }
            else if (planet == Library.PlanetName.Saturn) { limit = 5; }
            //todo rahu and ketu added later on based on saturn and mars
            else if (planet == Library.PlanetName.Rahu) { limit = 5; }
            else if (planet == Library.PlanetName.Ketu) { limit = 5; }

            //divide strength by minimum limit of power (based on planet)
            //if above limit than benefic, else return false
            var shadbalaRupa = Calculate.PlanetShadbalaPinda(planet, time);
            var rupa = Math.Round(shadbalaRupa.ToRupa(), 1);
            var strengthAfterLimit = rupa / limit;

            //if 1 or above is positive, below 1 is below limit
            var isBenefic = strengthAfterLimit >= 1.1;

            return isBenefic;
        }

        public static bool IsHouseBeneficInShadbala(HouseName house, Time birthTime, double threshold)
        {
            //get house strength
            var strength = HouseStrength(house, birthTime).ToDouble();

            //if above 450 then good
            var isBenefic = strength > threshold;
            return isBenefic;
        }

        /// <summary>
        /// Gets  strength (shadbala) of all 9 planets
        /// </summary>
        public static List<(double, PlanetName)> AllPlanetStrength(Time time)
        {
            var planetStrenghtList = new List<(double, PlanetName)>();

            //create a list with planet names & its strength (unsorted)
            foreach (var planet in Library.PlanetName.All9Planets)
            {
                //get planet strength in rupas
                var strength = PlanetShadbalaPinda(planet, time).ToDouble();

                //place in list with planet name
                planetStrenghtList.Add((strength, planet));

            }

            return planetStrenghtList;
        }

        /// <summary>
        /// Returns an array of all houses sorted by strength,
        /// 0 index being strongest to 11 index being weakest
        /// </summary>
        public static HouseName[] AllHousesOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHousesOrderedByStrength), time, Ayanamsa), _getAllHousesOrderedByStrength);


            //UNDERLYING FUNCTION
            HouseName[] _getAllHousesOrderedByStrength()
            {
                var houseStrengthList = new Dictionary<double, HouseName>();

                //create a list with planet names & its strength (unsorted)
                foreach (var house in Library.House.AllHouses)
                {
                    //get house strength
                    var strength = HouseStrength(house, time).ToRupa();

                    //place in list with house number
                    houseStrengthList[strength] = house;

                }


                //sort that list from strongest house to weakest house
                var keysSorted = houseStrengthList.Keys.ToList();
                keysSorted.Sort();

                var sortedArray = new HouseName[12];
                var count = 11;
                foreach (var key in keysSorted)
                {
                    sortedArray[count] = houseStrengthList[key];
                    count--;
                }

                return sortedArray;
            }

        }

        /// <summary>
        /// THE FINAL TOTAL STRENGTH
        /// Shadbala :the six sources of strength and weakness the planets
        /// The importance of and the part played by the Shadbalas,
        /// in the science of horoscopy, are manifold
        ///
        /// In order to obtain the total strength of
        /// the Shadbala Pinda of each planet, we have to add
        /// together its Sthana Bala, Dik Bala, Kala Bala.
        /// 'Chesta Bala and Naisargika Bala. And the Graha's
        /// Drik Bala must be added to or subtracted from the
        /// above sum according as it is positive or negative.
        /// The result obtained is the Shadbala Pinda of the
        /// planet in Shashtiamsas.
        ///
        /// Note: Rahu & Ketu supported, via house lord
        /// </summary>
        public static Shashtiamsa PlanetShadbalaPinda(PlanetName planetName, Time time)
        {
            //return 0 if null planet
            if (planetName == null) { return Shashtiamsa.Zero; }

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetShadbalaPinda), planetName, time, Ayanamsa), _getPlanetShadbalaPinda);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadbalaPinda()
            {

                //if planet name is rahu or ketu then replace with house lord's strength
                if (planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu)
                {
                    var houseLord = Calculate.LordOfHousePlanetIsIn(time, planetName);
                    planetName = houseLord;
                }

                //Sthana bala (Positional Strength)
                var sthanaBala = PlanetSthanaBala(planetName, time);

                //Get dik bala (Directional Strength)
                var digBala = PlanetDigBala(planetName, time);

                //Get kala bala (Temporal Strength)
                var kalaBala = PlanetKalaBala(planetName, time);

                //Get Chesta bala (Motional Strength)
                var chestaBala = PlanetChestaBala(planetName, time);

                //Get Naisargika bala (Natural Strength)
                var naisargikaBala = PlanetNaisargikaBala(planetName, time);

                //Get Drik/drug bala (Aspect Strength)
                var drikBala = PlanetDrikBala(planetName, time);


                //Get total Shadbala Pinda
                var total = sthanaBala + digBala + kalaBala + chestaBala + naisargikaBala + drikBala;

                //round it 2 decimal places
                var roundedTotal = new Shashtiamsa(Math.Round(total.ToDouble(), 2));

                return roundedTotal;
            }

        }

        /// <summary>
        /// get total combined strength of the inputed planet
        /// input birth time to get strength in horoscope
        /// note: an alias method to GetPlanetShadbalaPinda ("strength" is easier to remember)
        /// </summary>
        public static Shashtiamsa PlanetStrength(PlanetName planetName, Time time) => PlanetShadbalaPinda(planetName, time);

        /// <summary>
        /// Gets the lord of the house the inputed planet is in
        /// </summary>
        private static PlanetName LordOfHousePlanetIsIn(Time time, PlanetName planetName)
        {
            var currentHouse = Calculate.HousePlanetIsIn(time, planetName);
            var houseLord = Calculate.LordOfHouse((HouseName)currentHouse, time);

            return houseLord;
        }

        /// <summary>
        /// Aspect strength
        ///
        /// This strength is gained by the virtue of the aspect
        /// (Graha Dristi) of different planets on other planet.
        /// The aspect of benefics is considered to be strength and
        /// the aspect of malefics is considered to be weaknesses.
        ///
        /// 
        /// Drik Bala.-This means aspect strength.
        /// The Drik Bala of a Gqaha is one-fourth of the
        /// Drishti Pinda on it. It is positive or negative
        /// according as the Drishti Pinda is positive or
        /// negative.
        ///
        /// 
        /// See the formula given on page 85. There is
        /// special aspect for Jupiter, ,Mars and Saturn on the
        /// 5th and 9th, 4th and 8th and 3rd and 10th signs
        /// respectively.
        /// </summary>
        public static Shashtiamsa PlanetDrikBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            double dk;
            var drishti = new Dictionary<String, double>();
            double vdrishti;
            var sp = new Dictionary<PlanetName, int>();


            foreach (var p in Library.PlanetName.All7Planets)
            {
                if (Calculate.IsPlanetBenefic(p, time))
                {
                    sp[p] = 1;
                }
                else
                {
                    sp[p] = -1;
                }

            }


            foreach (var i in Library.PlanetName.All7Planets)
            {
                foreach (var j in Library.PlanetName.All7Planets)
                {
                    //Finding Drishti Kendra or Aspect Angle
                    var planetNirayanaLongitude = Calculate.PlanetNirayanaLongitude(time, j).TotalDegrees;
                    var nirayanaLongitude = Calculate.PlanetNirayanaLongitude(time, i).TotalDegrees;
                    dk = planetNirayanaLongitude - nirayanaLongitude;

                    if (dk < 0) { dk += 360; }

                    //get special aspect if any
                    vdrishti = FindViseshaDrishti(dk, i);

                    drishti[i.ToString() + j.ToString()] = FindDrishtiValue(dk) + vdrishti;

                }
            }

            double bala = 0;

            var DrikBala = new Dictionary<PlanetName, double>();

            foreach (var i in Library.PlanetName.All7Planets)
            {
                bala = 0;

                foreach (var j in All7Planets)
                {
                    bala = bala + (sp[j] * drishti[j.ToString() + i.ToString()]);

                }

                DrikBala[i] = bala / 4;
            }



            return new Shashtiamsa(DrikBala[planetName]);



        }

        /// <summary>
        /// Get special aspect if any of Kuja, Guru and Sani
        /// </summary>
        public static double FindViseshaDrishti(double dk, PlanetName p)
        {
            double vdrishti = 0;

            if (p == Library.PlanetName.Saturn)
            {
                if (((dk >= 60) && (dk <= 90)) || ((dk >= 270) && (dk <= 300)))
                {
                    vdrishti = 45;
                }

            }
            else if (p == Library.PlanetName.Jupiter)
            {

                if (((dk >= 120) && (dk <= 150))
                    || ((dk >= 240) && (dk <= 270)))
                {
                    vdrishti = 30;
                }

            }
            else if (p == Library.PlanetName.Mars)
            {
                if (((dk >= 90) && (dk <= 120)) || ((dk >= 210) && (dk <= 240)))
                {
                    vdrishti = 15;
                }

            }
            else
            {
                vdrishti = 0;
            }


            return vdrishti;

        }

        public static double FindDrishtiValue(double dk)
        {

            double drishti = 0;

            if ((dk >= 30.0) && (dk <= 60))
            {
                drishti = (dk - 30) / 2;
            }
            else if ((dk > 60.0) && (dk <= 90))
            {
                drishti = (dk - 60) + 15;
            }
            else if ((dk > 90.0) && (dk <= 120))
            {
                drishti = ((120 - dk) / 2) + 30;
            }
            else if ((dk > 120.0) && (dk <= 150))
            {
                drishti = (150 - dk);
            }
            else if ((dk > 150.0) && (dk <= 180))
            {
                drishti = (dk - 150) * 2;
            }
            else if ((dk > 180.0) && (dk <= 300))
            {
                drishti = (300 - dk) / 2;
            }

            return drishti;

        }

        /// <summary>
        /// Nalsargika Bala.-This is the natural
        /// strength that each Graha possesses. The value
        /// assigned to each depends upon its luminosity.
        /// Ravi, the brightest of all planets, has the greatest
        /// Naisargika strength while Sani, the darkest, has
        /// the least Naisargika Bala.
        ///
        /// This is the natural or inherent strength of a planet.
        /// </summary>
        public static Shashtiamsa PlanetNaisargikaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            if (planetName == Library.PlanetName.Sun) { return new Shashtiamsa(60); }
            else if (planetName == Library.PlanetName.Moon) { return new Shashtiamsa(51.43); }
            else if (planetName == Library.PlanetName.Venus) { return new Shashtiamsa(42.85); }
            else if (planetName == Library.PlanetName.Jupiter) { return new Shashtiamsa(34.28); }
            else if (planetName == Library.PlanetName.Mercury) { return new Shashtiamsa(25.70); }
            else if (planetName == Library.PlanetName.Mars) { return new Shashtiamsa(17.14); }
            else if (planetName == Library.PlanetName.Saturn) { return new Shashtiamsa(8.57); }

            throw new Exception("Planet not specified!");
        }

        /// <summary>
        /// NOTE: sun, moon get score for ISHTA/KESHA calculation TODO
        /// MOTIONAL STRENGTH
        /// Chesta here means Vakra Chesta or act of retrogression. Each planet, except the Sun and the Moon,
        /// and shadowy planets get into the state of Vakra or retrogression
        /// when its distance from the Sun exceeds a particular limit.
        /// And the strength or potency due to the planet on account of the arc of the retrogression is
        /// termed as Chesta Bala
        ///
        /// Deduct from the Seeghrocbcha, half the sum
        /// of the True and Mean Longitudes of planets and
        /// divide the difference by 3. The quotient is the
        /// Chestabala.
        /// Max 60, meaning Retrograde/Vakra
        /// When the distance of any one planet from
        /// the Sun exceeds a particular limit, it becomes
        /// retrograde, i.e., when the planet goes from
        /// perihelion (the point in a planet's orbit nearest
        /// to the Sun) to aphelion (the part of a planet's
        /// oroit most distant from the Sun) as it recedes
        /// from the Sun, it gradually loses the power
        /// of the Sun's gravitation and consequently, 
        /// </summary>
        public static Shashtiamsa PlanetChestaBala(PlanetName planetName, Time time, bool includeSunMoon = false)
        {
            //if include Sun/Moon specified, then use special function (used for Ishta/Kashta score)
            var isSunMoon = planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon;
            if (isSunMoon && includeSunMoon)
            {
                return SunMoonChestaBala(planetName);
            }

            //the Sun,Moon,Rahu and Ketu does not not retrograde, so 0 chesta bala
            if (isSunMoon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return Shashtiamsa.Zero; }

            //get the interval between birth date and the date of the epoch (1900)
            //verified standard horoscope = 6862.579
            //NOTE: dates before 1900 give negative values
            var interval = EpochInterval(time);

            //get the mean/average longitudes of all planets
            var madhya = Madhya(interval, time);

            //get the apogee of all planets (apogee=earth, aphelion=sun)
            // aphelion (the part of a planet's orbit most distant from the Sun) 
            var seegh = GetSeeghrochcha(madhya, interval, time);


            //calculate chesta kendra, also called Seeghra kendra
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName).TotalDegrees;
            //This is the Arc of retrogression.
            var planetAphelion = seegh[planetName]; //fixed most distant point from sun
            var planetMeanCircle = madhya[planetName]; //planet average distant point from sun (CIRCLE ORBIT)
            //Chesta kendra = 180 degrees = Retrograde
            //Because the orbits are elliptical
            //and not circular, equations are applied to the mean positions to get the true longitudes.
            var trueLongitude = ((planetMeanCircle + planetLongitude) / 2.0);
            //distance from stationary point, if less than 0 than add 360 
            var chestaKendra = (planetAphelion - trueLongitude);


            //If the Chesta kendra exceeds 180° (maximum retrograde), subtract it from 360, otherwise
            //keep it as it is. The remainder represents the reduced Chesta kendra
            //NOTE: done to reduce value of direct motion, only value relative to retro motion
            if (chestaKendra < 360.00)
            {
                chestaKendra = chestaKendra + 360;
            }
            chestaKendra = chestaKendra % 360;
            if (chestaKendra > 180.00) { chestaKendra = 360 - chestaKendra; }


            //The Chesta Bala is zero when the Chesta kendra is also zero. When it is
            //180° the Chesta Bala is 60 Shashtiamsas. In intermediate position, the
            //Bala is found by proportion (devide by 3)
            var chestaBala = (chestaKendra / 3.00);

            return new Shashtiamsa(chestaBala);



            //------------------------FUNCTIONS--------------


            //Seeghrochcha is the aphelion of the planet
            //It is required to find the Chesta kendra.
            //NOTE:aphelion (the part of a planet's orbit most distant from the Sun)
            Dictionary<PlanetName, double> GetSeeghrochcha(Dictionary<PlanetName, double> mean, double epochToBirthDays, Time time1)
            {
                int _birthYear = time1.GetLmtDateTimeOffset().Year;
                var seegh = new Dictionary<PlanetName, double>();
                double correction;

                //The mean longitude of the Sun will be the Seeghrochcha of Kuja, Guru and Sani.
                seegh[Library.PlanetName.Mars] = seegh[Library.PlanetName.Jupiter] = seegh[Library.PlanetName.Saturn] = mean[Library.PlanetName.Sun];

                correction = 6.670 + (0.00133 * (_birthYear - 1900));
                double changeDuringIntervalMercury = (epochToBirthDays * 4.092385);
                const double aphelionAtEpochMercury = 164.00; // The Seeghrochcha of Budha at the epoch
                double mercuryAphelion = changeDuringIntervalMercury < 0 ? aphelionAtEpochMercury - changeDuringIntervalMercury : aphelionAtEpochMercury + changeDuringIntervalMercury;
                mercuryAphelion -= correction; //further correction of +6.670-0133
                seegh[Library.PlanetName.Mercury] = (mercuryAphelion + correction) % 360;

                correction = 5 + (0.0001 * (_birthYear - 1900));
                double changeDuringIntervalVenus = (epochToBirthDays * 1.602159);
                const double aphelionAtEpochVenus = 328.51; // The Seeghrochcha of Budha at the epoch
                double venusAphelion = changeDuringIntervalVenus < 0 ? aphelionAtEpochVenus - changeDuringIntervalVenus : aphelionAtEpochVenus + changeDuringIntervalVenus;
                venusAphelion -= correction; //diminish the sum by 5 + 0.001*t (where t = birth year - 1900)
                seegh[Library.PlanetName.Venus] = venusAphelion % 360;

                return seegh;
            }

        }


        /// <summary>
        /// special function to get chesta score for Ishta/Kashta score
        /// Bala book pg. 108
        /// </summary>
        public static Shashtiamsa SunMoonChestaBala(PlanetName planetName)
        {
            return Shashtiamsa.Zero;
            throw new NotImplementedException();
        }

        /// <summary>
        /// The mean position of a planet is the position which it would have attained at a uniform
        /// rate of motion and the corrections to be applied in respect of the eccentricity of the orbit are not considered
        /// </summary>
        public static Dictionary<PlanetName, double> Madhya(double epochToBirthDays, Time time1)
        {
            int _birthYear = time1.GetLmtDateTimeOffset().Year;

            var madhya = new Dictionary<PlanetName, double>();

            //calculate chesta kendra, also called Seeghra kendra

            //SUN 
            //Start from the epoch. Calculate the time of interval from the epoch to the day of birth
            //and multiply the same by the daily motion of the planet, and the change during the interval is obtained.
            var sunEpochMean = 257.4568; //epoch the mean position
            double changeDuringIntervalSun = (epochToBirthDays * 0.9855931);

            //This change being added to or subtracted from the mean position at the
            //time of epoch as the date is posterior or anterior to the epoch day, the mean position is arrived at.
            double meanPositionSun = changeDuringIntervalSun < 0 ? sunEpochMean - changeDuringIntervalSun : sunEpochMean + changeDuringIntervalSun;
            meanPositionSun = meanPositionSun % 360; //expunge
            madhya[Library.PlanetName.Sun] = meanPositionSun;

            //Mean Longitudes of -Inferior Planets.-The mean longitudes of Budba and Sukra are the same as that of the Sun.
            //same for venus & mercury because closer to sun than earth it self
            madhya[Library.PlanetName.Mercury] = madhya[Library.PlanetName.Venus] = madhya[Library.PlanetName.Sun];

            //MARS
            var marsEpochMean = 270.22;
            double changeDuringIntervalMars = (epochToBirthDays * 0.5240218);
            double meanPositionMars = changeDuringIntervalMars < 0 ? marsEpochMean - changeDuringIntervalMars : marsEpochMean + changeDuringIntervalMars;
            meanPositionMars = meanPositionMars % 360; //expunge
            madhya[Library.PlanetName.Mars] = meanPositionMars;

            //JUPITER
            var jupiterEpochMean = 220.04;
            double changeDuringIntervalJupiter = (epochToBirthDays * 0.08310024);
            double meanPositionJupiter = changeDuringIntervalJupiter < 0 ? jupiterEpochMean - changeDuringIntervalJupiter : jupiterEpochMean + changeDuringIntervalJupiter;
            var correction1 = 3.33 + (0.0067 * (_birthYear - 1900));
            meanPositionJupiter -= correction1; //deduct from the total 3.33 + 0.0067*t (where t=birth year-1900).
            meanPositionJupiter %= 360; //expunge
            madhya[Library.PlanetName.Jupiter] = meanPositionJupiter;

            //SATURN
            var saturnEpochMean = 220.04;
            double changeDuringIntervalSaturn = (epochToBirthDays * 0.03333857);
            double meanPositionSaturn = changeDuringIntervalSaturn < 0 ? saturnEpochMean - changeDuringIntervalSaturn : saturnEpochMean + changeDuringIntervalSaturn;
            var correction2 = 5 + (0.001 * (_birthYear - 1900));
            meanPositionSaturn += correction2; //add 5°+0.001*t (where t = birth year - 1900)
            meanPositionSaturn %= 360; //expunge
            madhya[Library.PlanetName.Saturn] = meanPositionSaturn;

            //raise alarm if negative, since that is clearly an error, no negative mean longitude
            if (madhya.Any(x => x.Value < 0)) { throw new Exception("Madya/Mean can't be negative!"); }

            return madhya;
        }


        /// <summary>
        /// Get interval from the epoch to the birth date in days
        /// The result represents the interval from the epoch to the birth date.
        /// </summary>
        public static double EpochInterval(Time time1)
        {
            //Determine the interval between birth date and the date of the epoch thus.

            int birthYear = time1.GetLmtDateTimeOffset().Year;
            int birthMonth = time1.GetLmtDateTimeOffset().Month;
            int birthDate = time1.GetLmtDateTimeOffset().Day;

            //month ends in days
            int[] monthEnds = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };

            //Deduct 1900 from the Christian Era. The difference will be past
            //years when positive and coming years when negative.
            int yrdiff = birthYear - 1900;

            //Multiply the same by 365 and to the product add the intervening bi-sextile days.
            var epochDays = ((yrdiff * 365) + (yrdiff / 4) + monthEnds[birthMonth - 1]) - 1 + birthDate;


            int hour = time1.GetLmtDateTimeOffset().Hour;
            int minute = time1.GetLmtDateTimeOffset().Minute;
            double offsetHours = time1.GetLmtDateTimeOffset().Offset.TotalHours;
            double utime = new TimeSpan(hour, minute, 0).TotalHours + ((5 + (double)(4.00 / 60.00)) - offsetHours);

            //The result represents the interval from the epoch to the birth date.
            double interval = epochDays + (double)(utime / 24.00);
            interval = Math.Round(interval, 3);//round to 3 places decimal

            return interval;
        }


        /// <summary>
        /// Gets the planets motion name, can be Retrograde, Direct, Stationary
        /// a name version of Chesta Bala
        /// </summary>
        public static PlanetMotion PlanetMotionName(PlanetName planetName, Time time)
        {
            //sun, moon, rahu & ketu don' have retrograde so always direct
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Rahu || planetName == Library.PlanetName.Ketu) { return PlanetMotion.Direct; }

            //get chestaBala
            var chestaBala = Calculate.PlanetChestaBala(planetName, time).ToDouble();

            //based on chesta bala assign name to it
            //Chesta kendra = 180 degrees = Retrograde
            switch (chestaBala)
            {
                case <= 60 and > 45: return PlanetMotion.Retrograde;
                case <= 45 and > 15: return PlanetMotion.Direct;
                case <= 15 and >= 0: return PlanetMotion.Stationary;
                default:
                    throw new Exception($"Error in GetPlanetMotionName : {chestaBala}");
            }

        }


        /// <summary>
        /// circulation time of the objects in years, used by cheshta bala calculation
        /// </summary>
        public static double PlanetCirculationTime(PlanetName planetName)
        {

            if (planetName == Library.PlanetName.Sun) { return 1.0; }
            if (planetName == Library.PlanetName.Moon) { return .082; }
            if (planetName == Library.PlanetName.Mars) { return 1.88; }
            if (planetName == Library.PlanetName.Mercury) { return .24; }
            if (planetName == Library.PlanetName.Jupiter) { return 11.86; }
            if (planetName == Library.PlanetName.Venus) { return .62; }
            if (planetName == Library.PlanetName.Saturn) { return 29.46; }

            throw new Exception("Planet circulation time not defined!");

        }

        /// <summary>
        /// Sapthavargajabala: This is the strength of a
        /// planet due to its residence in the seven sub-divisions
        /// according to its relation with the dispositor.
        ///
        /// Saptavargaja bala means the strength a
        /// planet gets by virtue of its disposition in a friendly,
        /// neutral or inimical Rasi, Hora, Drekkana, Sapthamsa,
        /// Navamsa, Dwadasamsa and Thrimsamsa.
        /// </summary>
        public static Shashtiamsa PlanetSaptavargajaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSaptavargajaBala), planetName, time, Ayanamsa), _getPlanetSaptavargajaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSaptavargajaBala()
            {
                //declare total value
                double totalSaptavargajaBalaInShashtiamsa = 0;

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();


                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = Calculate.IsPlanetInMoolatrikona(planetName, time);

                //if planet is in moolatrikona
                if (planetInMoolatrikona)
                {
                    //add the relationship to the list
                    planetSignRelationshipList.Add(PlanetToSignRelationship.Moolatrikona);
                }
                else
                //else get planet's normal relationship with rasi
                {
                    //get planet rasi
                    var planetRasi = Calculate.PlanetSignName(planetName, time).GetSignName();
                    var rasiSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //get planet hora
                var planetHora = Calculate.PlanetHoraSign(planetName, time);
                var horaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);


                //get planet drekkana
                var planetDrekkana = Calculate.PlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //get planet saptamsa
                var planetSaptamsa = Calculate.PlanetSaptamsaSign(planetName, time);
                var saptamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetSaptamsa, time);
                //add planet saptamsa relationship to list
                planetSignRelationshipList.Add(saptamsaSignRelationship);


                //get planet navamsa
                var planetNavamsa = Calculate.PlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //get planet dwadasamsa
                var planetDwadasamsa = Calculate.PlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //get planet thrimsamsa
                var planetThrimsamsa = Calculate.PlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
                //add planet thrimsamsa relationship to list
                planetSignRelationshipList.Add(thrimsamsaSignRelationship);


                //calculate total Saptavargaja Bala

                //loop through all the relationship
                foreach (var planetToSignRelationship in planetSignRelationshipList)
                {
                    //add relationship point accordingly

                    //A planet in its Moolatrikona is assigned a value of 45 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Moolatrikona)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 45;
                    }

                    //in Swavarga 30 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.OwnVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.BestFriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.FriendVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.NeutralVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.EnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.BitterEnemyVarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalSaptavargajaBalaInShashtiamsa);

            }

        }


        /// <summary>
        /// Shadvarga bala: This is the strength of a
        /// planet due to its residence in the 6 sub-divisions
        /// according to its relation with the dispositor.
        ///
        /// They are (1) Rasi, {2) Hora, (3) Drekkana, (4) Navamsa, (5)
        /// Dwadasamsa and (6) Trimsamsa.
        /// </summary>
        public static Shashtiamsa PlanetShadvargaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetShadvargaBala), planetName, time, Ayanamsa), _getPlanetShadvargaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadvargaBala()
            {

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();

                //1.
                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = Calculate.IsPlanetInMoolatrikona(planetName, time);

                //if planet is in moolatrikona
                if (planetInMoolatrikona)
                {
                    //add the relationship to the list
                    planetSignRelationshipList.Add(PlanetToSignRelationship.Moolatrikona);
                }
                else
                //else get planet's normal relationship with rasi
                {
                    //get planet rasi
                    var planetRasi = Calculate.PlanetSignName(planetName, time).GetSignName();
                    var rasiSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //2.
                //get planet hora
                var planetHora = Calculate.PlanetHoraSign(planetName, time);
                var horaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);

                //3.
                //get planet drekkana
                var planetDrekkana = Calculate.PlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //4.
                //get planet navamsa
                var planetNavamsa = Calculate.PlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //5.
                //get planet dwadasamsa
                var planetDwadasamsa = Calculate.PlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //6.
                //get planet thrimsamsa
                var planetThrimsamsa = Calculate.PlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = Calculate.PlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
                //add planet thrimsamsa relationship to list
                planetSignRelationshipList.Add(thrimsamsaSignRelationship);


                //calculate total Saptavargaja Bala

                //a place to store total value
                double totalShadvargaBalaInShashtiamsa = 0;

                //loop through all the relationship
                foreach (var planetToSignRelationship in planetSignRelationshipList)
                {
                    //add relationship point accordingly

                    //A planet in its Moolatrikona is assigned a value of 45 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Moolatrikona)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 45;
                    }

                    //in Swavarga 30 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.OwnVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.BestFriendVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.FriendVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.NeutralVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.EnemyVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.BitterEnemyVarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalShadvargaBalaInShashtiamsa);

            }

        }

        /// <summary>
        /// TODO : use of shadvarga bala might be wrong here, needs clarification
        /// problem is too much of time goes under bad, doesnt seem right
        /// for now we put it 140 threhold so guarenteed to be strong
        /// and doesn not occur all the time
        /// </summary>
        public static bool IsPlanetStrongInShadvarga(PlanetName planet, Time time)
        {
            //get planet shadvarga bala
            var planetBala = Calculate.PlanetShadvargaBala(planet, time).ToDouble();

            //Note: To determine if shadvarga bala value is strong or weak
            //a neutral point is set, anything above is strong & below is weak
            var neutralPoint = Calculate.PlanetShadvargaBalaNeutralPoint(planet);

            //if above neutral number, is strong else it is weak
            return planetBala > 140;
        }


        /// <summary>
        /// residence of the planet and as such a certain degree of strength or weakness attends on it
        /// 
        /// Positonal strength
        /// 
        /// A planet occupies a
        /// certain sign in a Rasi and friendly, neutrai or
        /// inimical varga~. It is either exalted or debilitated·
        /// lt ocupies its Moolathrikona or it has its own
        /// varga. All these states refer to the position or
        /// residence of the planet and as such a certain degree
        /// of strength or weakness attends on it. This strength
        /// or potency is known as the Sthanabala.
        /// 
        /// 
        /// 1.Uccha Bala:
        /// Uccha means exaltation. When a planet is placed in its highest exaltation point,
        /// it is of full strength and when it is in its deepest debilitation point, it is devoid of any strength.
        /// When in between the strength is calculated proportionately dependent on the distance these planets are
        /// placed from the highest exaltation or deepest debilitation point.
        ///
        /// 2.Sapta Vargiya Bala:
        /// Rashi, Hora, Drekkana, Saptamsha, Navamsha, Dwadasamsha and Trimsamsha constitute the Sapta Varga.
        /// The strength of the planets in these seven divisional charts based on their placements in Mulatrikona,
        /// own sign, friendly sign etc. constitute the Sapta vargiya bala.
        /// 
        /// 3.Oja-Yugma Rashi-Amsha Bala:
        /// Oja means odd signs and Yugma means even signs. Thus, as the name imply, this strength is derived from
        /// a planet’s placement in the odd or even signs in the Rashi and Navamsha.
        /// 
        /// 4.Kendradi Bala:
        /// The name itself implies how to compute this strength. A planet in a Kendra (1-4-7-10) gets full strength,
        /// while one in Panapara (2-5-8-11) gets half and the one in Apoklimas (12-3-6-9) gets quarter strength.
        ///
        /// 5.Drekkana Bala:
        /// Due to placement in first, second, or third Drekkana of a sign, male, female and hermaphrodite planets respectively,
        /// get a quarter strength according to placements in the first, second and third Drekkana.
        /// </summary>
        public static Shashtiamsa PlanetSthanaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetSthanaBala), planetName, time, Ayanamsa), _getPlanetSthanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSthanaBala()
            {
                //Get Ochcha Bala (exaltation strength)
                var ochchaBala = PlanetOchchaBala(planetName, time);

                //Get Saptavargaja Bala
                var saptavargajaBala = PlanetSaptavargajaBala(planetName, time);

                //Get Ojayugmarasyamsa Bala
                var ojayugmarasymsaBala = PlanetOjayugmarasyamsaBala(planetName, time);

                //Get Kendra Bala
                var kendraBala = PlanetKendraBala(planetName, time);

                //Drekkana Bala
                var drekkanaBala = PlanetDrekkanaBala(planetName, time);

                //Total Sthana Bala
                var totalSthanaBala = ochchaBala + saptavargajaBala + ojayugmarasymsaBala + kendraBala + drekkanaBala;

                return totalSthanaBala;

            }

        }

        /// <summary>
        /// Drekkanabala: The Sun, Jupiter and Mars
        /// in the lst ; Saturn and Mercury in the 2nd ; and
        /// the Moon and Venus in the last Drekkana, get full
        /// strength of 60 shashtiamsas.
        /// </summary>
        public static Shashtiamsa PlanetDrekkanaBala(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = Calculate.PlanetSignName(planetName, time);

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //if male planet -Ravi, Guru and Kuja.
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mars)
            {
                //if planet is in 1st drekkana
                if (degreesInSign >= 0 && degreesInSign <= 10.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Hermaphrodite Planets.-Sani and Budba
            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Mercury)
            {
                //if planet is in 2nd drekkana
                if (degreesInSign > 10 && degreesInSign <= 20.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Female Planets.-Chandra and Sukra
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if planet is in 3rd drekkana
                if (degreesInSign > 20 && degreesInSign <= 30.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }
            }

            //if none above conditions met return 0 bala
            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Kendrtzbala: Planets in Kendras get 60
        /// shashtiamsas; in Panapara 30, and in Apoklima 15.
        /// </summary>
        public static Shashtiamsa PlanetKendraBala(PlanetName planetName, Time time)
        {
            //get number of the sign planet is in
            var planetSignNumber = (int)Calculate.PlanetSignName(planetName, time).GetSignName();

            //A planet in a kendra sign  gets 60 Shashtiamsas as its strength ;
            //Quadrants.-Kendras-1 (Ar, 4, 7 and 10.
            if (planetSignNumber == 1 || planetSignNumber == 4 || planetSignNumber == 7 || planetSignNumber == 10)
            {
                return new Shashtiamsa(60);
            }

            //in a Panapara sign 30 Shashtiamsas;
            //-Panaparas-2, 5, 8 and 11.
            if (planetSignNumber == 2 || planetSignNumber == 5 || planetSignNumber == 8 || planetSignNumber == 11)
            {
                return new Shashtiamsa(30);
            }


            //and in an Apoklima sign 15 Shashtiamsas.
            //Apoklimas-3, 6, 9 and 12 {9th being a trikona must be omitted).
            if (planetSignNumber == 3 || planetSignNumber == 6 || planetSignNumber == 9 || planetSignNumber == 12)
            {
                return new Shashtiamsa(15);
            }


            throw new Exception("Kendra Bala not found, error");
        }

        /// <summary>
        /// Ojayugmarasyamsa: In odd Rasi and Navamsa,
        /// the Sun, Mars, Jupiter, Mercury and Saturn
        /// get strength and the rest in even signs
        /// </summary>
        public static Shashtiamsa PlanetOjayugmarasyamsaBala(PlanetName planetName, Time time)
        {
            //get planet rasi sign
            var planetRasiSign = Calculate.PlanetSignName(planetName, time).GetSignName();

            //get planet navamsa sign
            var planetNavamsaSign = Calculate.PlanetNavamsaSign(planetName, time);

            //declare total Ojayugmarasyamsa Bala as 0 first
            double totalOjayugmarasyamsaBalaInShashtiamsas = 0;

            //if planet is the moon or venus
            if (planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Venus)
            {
                //if rasi sign is an even sign
                if (Calculate.IsEvenSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an even sign
                if (Calculate.IsEvenSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }
            //if planet is Sun, Mars, Jupiter, Mercury and Saturn
            else if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars ||
                     planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mercury || planetName == Library.PlanetName.Saturn)
            {
                //if rasi sign is an odd sign
                if (Calculate.IsOddSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an odd sign
                if (Calculate.IsOddSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }

            return new Shashtiamsa(totalOjayugmarasyamsaBalaInShashtiamsas);
        }

        /// <summary>
        /// Gets a planet's Kala Bala or Temporal strength
        /// </summary>
        public static Shashtiamsa PlanetKalaBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }



            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetKalaBala), planetName, time, Ayanamsa), _getPlanetKalaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetKalaBala()
            {
                //place to store pre kala bala values
                var kalaBalaList = new Dictionary<PlanetName, Shashtiamsa>();

                //Yuddha Bala requires all planet's pre kala bala
                //so calculate pre kala bala for all planets first
                foreach (var planet in Library.PlanetName.All7Planets)
                {
                    //calculate pre kala bala
                    var preKalaBala = GetPreKalaBala(planet, time);

                    //store in a list sorted by planet name, to be accessed later
                    kalaBalaList.Add(planet, preKalaBala);
                }

                //calculate Yuddha Bala
                var yuddhaBala = PlanetYuddhaBala(planetName, kalaBalaList, time);

                //Total Kala Bala
                var total = kalaBalaList[planetName] + yuddhaBala;

                return total;

                //---------------FUNCTIONS--------------
                Shashtiamsa GetPreKalaBala(PlanetName planetName, Time time)
                {
                    //Nathonnatha Bala
                    var nathonnathaBala = PlanetNathonnathaBala(planetName, time);

                    //Paksha Bala
                    var pakshaBala = PlanetPakshaBala(planetName, time);

                    //Tribhaga Bala
                    var tribhagaBala = PlanetTribhagaBala(planetName, time);

                    //Abda Bala
                    var abdaBala = PlanetAbdaBala(planetName, time);

                    //Masa Bala
                    var masaBala = PlanetMasaBala(planetName, time);

                    //Vara Bala
                    var varaBala = PlanetVaraBala(planetName, time);

                    //Hora Bala
                    var horaBala = PlanetHoraBala(planetName, time);

                    //Ayana Bala
                    var ayanaBala = PlanetAyanaBala(planetName, time);

                    //combine all the kala bala calculated so far, and return the value
                    var preKalaBala = nathonnathaBala + pakshaBala + tribhagaBala + abdaBala + masaBala + varaBala + horaBala +
                                      ayanaBala;

                    return preKalaBala;
                }
            }

        }

        /// <summary>
        /// Two planets are said to be in Yuddha or fight when they are in
        /// conjunction and the distance between them is less than one degree.
        /// TODO Not fully tested
        ///
        /// Yuddhabala : All planets excepting the Sun
        /// and the Moon enter into war when two planets are
        /// in the same degree. The pJanet having the lesser
        /// longitude is the winner. Find out the sum total of
        /// the SthanabaJa, Kalabala and Digbala of these two'
        /// planets. Difference between the two, divided by
        /// the difference of their diameters of its disc, gives
        /// the Yuddhabala. Add this to the victorious planet
        /// and dedu_ct it from the vanquished.
        /// </summary>
        public static Shashtiamsa PlanetYuddhaBala(PlanetName inputedPlanet, Dictionary<PlanetName, Shashtiamsa> preKalaBalaValues, Time time)
        {
            //All the planets excepting Sun and Moon may enter into war (Yuddha)
            if (inputedPlanet == Library.PlanetName.Moon || inputedPlanet == Library.PlanetName.Sun) { return Shashtiamsa.Zero; }


            //place to store winner & loser balas
            var yudhdhabala = new Dictionary<PlanetName, Shashtiamsa>();


            //get all planets that are conjunct with inputed planet
            var conjunctPlanetList = Calculate.PlanetsInConjuction(time, inputedPlanet);

            //remove rahu & kethu if present, they are not included in Yuddha Bala calculations
            conjunctPlanetList.RemoveAll(pl => pl == Library.PlanetName.Rahu || pl == Library.PlanetName.Ketu);


            foreach (var checkingPlanet in conjunctPlanetList)
            {

                //All the planets excepting Sun and Moon may enter into war (Yuddha)
                //no need to calculate Yuddha, move to next planet, if sun or moon
                if (checkingPlanet == Library.PlanetName.Moon || checkingPlanet == Library.PlanetName.Sun) { continue; }


                //get distance between conjunct planet & inputed planet
                var inputedPlanetLong = Calculate.PlanetNirayanaLongitude(time, inputedPlanet);
                var checkingPlanetLong = Calculate.PlanetNirayanaLongitude(time, checkingPlanet);
                var distance = Calculate.DistanceBetweenPlanets(inputedPlanetLong, checkingPlanetLong);


                //if the distance between them is less than one degree
                if (distance < Angle.FromDegrees(1))
                {
                    PlanetName winnerPlanet = null;
                    PlanetName losserPlanet = null;

                    //The conquering planet is the one whose longitude is less.
                    if (inputedPlanetLong < checkingPlanetLong) { winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet; } //inputed planet won
                    else if (inputedPlanetLong > checkingPlanetLong) { winnerPlanet = checkingPlanet; losserPlanet = inputedPlanet; } //checking planet won
                    else if (inputedPlanetLong == checkingPlanetLong)
                    {
                        //unlikely chance, log error & set inputed planet as winner (random)
                        LogManager.Error($"Planets same longitude! Not expected, random result used!");
                        winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet;
                    }

                    //When two planets are in war, get the sum of the various Balas, viv., Sthanabala, the
                    // Dikbala and the Kalabala (up to Horabala) described hitherto of the fighting planets. Find out the
                    // difference between these two sums.
                    var shadbaladiff = Math.Abs(preKalaBalaValues[inputedPlanet].ToDouble() - preKalaBalaValues[checkingPlanet].ToDouble());


                    //Divide shadbala difference by the difference between the diameters of the discs of the two fighting planets
                    var diameterDifference = PlanetDiscDiameter(inputedPlanet).GetDifference(PlanetDiscDiameter(checkingPlanet));


                    //And the resulting quotient which is the Yuddhabala (Shashtiamsa) must be added to the total of the Kalabala (detailed
                    // hitherto) of the victorious planet and must be subtracted from the total Kalabala of the vanquished planet.
                    var shadbala = diameterDifference.TotalDegrees / shadbaladiff;

                    yudhdhabala[winnerPlanet] = new Shashtiamsa(shadbala);
                    yudhdhabala[losserPlanet] = new Shashtiamsa(-shadbala);

                }


            }


            //return yudhdhabala if it was calculated else, return 0 
            var found = yudhdhabala.TryGetValue(inputedPlanet, out var bala);
            return found ? bala : Shashtiamsa.Zero;




            //-----------FUNCTIONS----------------


        }

        /// <summary>
        /// Bimba Parimanas -This means the diameters of the discs of the planets.
        /// </summary>
        static Angle PlanetDiscDiameter(PlanetName planet)
        {
            if (planet == Library.PlanetName.Mars) { return new Angle(0, 9, 4); }
            if (planet == Library.PlanetName.Mercury) { return new Angle(0, 6, 6); }
            if (planet == Library.PlanetName.Jupiter) { return new Angle(0, 190, 4); }
            if (planet == Library.PlanetName.Venus) { return new Angle(0, 16, 6); }
            if (planet == Library.PlanetName.Saturn) { return new Angle(0, 158, 0); }

            //control should not come here, report error
            throw new Exception("Disc diameter now found!");
        }


        /// <summary>
        /// Ayanabala : All planets get 30 shasbtiamsas
        /// at the equator. For the Sun, Jupiter, Mars
        /// and Venus add proportionately when they are in
        /// northern course and for the Moon and Saturn when
        /// in southern course. Deduct proportionately when
        /// they are in the opposite direction. Unit of strength
        /// is 60 shashtiamsas.
        ///
        /// 
        /// TODO some values for calculation with standard hooscope out of whack,
        /// it seems small differences in longitude seem magnified at final value,
        /// not 100% sure, need further testing for confirmation, but final values seem ok so far
        /// </summary>
        public static Shashtiamsa PlanetAyanaBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAyanaBala), planetName, time, Ayanamsa), _getPlanetAyanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAyanaBala()
            {
                double ayanaBala = 0;

                //get plant kranti (negative south, positive north)
                var kranti = PlanetDeclination(planetName, time);

                //prepare values for calculation of ayanabala
                var x = Angle.FromDegrees(24);
                var isNorthDeclination = kranti < 0 ? false : true;

                //get declination without negative (absolute value), easier for calculation
                var absKranti = Math.Abs((double)kranti);

                //In case of Sukra, Ravi, Kuja and Guru their north declinations are
                //additive and south declinations are subtractive
                if (planetName == Library.PlanetName.Venus || planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars || planetName == Library.PlanetName.Jupiter)
                {
                    //additive
                    if (isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }

                    //And double the Ayanabala in the case of the Sun
                    if (planetName == Library.PlanetName.Sun) { ayanaBala = ayanaBala * 2; }

                }
                //In case of Sani and Chandra, their south declinations are additive while their
                //north declinations are subtractive.
                else if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon)
                {
                    //additive
                    if (!isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }
                }
                //For Budha the declination, north or south, is always additive.
                else if (planetName == Library.PlanetName.Mercury)
                {
                    ayanaBala = ((24 + absKranti) / 48) * 60;
                }


                return new Shashtiamsa(ayanaBala);

            }


        }

        /// <summary>
        /// A heavenly body moves northwards the equator for sometime and
        /// then gets southwards. This angular distance from
        /// the equinoctial or celestial equator is Kranti or the
        /// declination.
        ///
        /// Declinations are reckoned plus or minus according as the planet
        /// is situated in the northern or southern celestial hemisphere
        /// </summary>
        public static double PlanetDeclination(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetDeclination), planetName, time, Ayanamsa), _getPlanetDeclination);


            //UNDERLYING FUNCTION
            double _getPlanetDeclination()
            {
                //for degree to radian conversion
                const double DEG2RAD = 0.0174532925199433;

                var eps = EclipticObliquity(time);

                var tlen = Calculate.PlanetSayanaLongitude(time, planetName);
                var lat = Calculate.PlanetSayanaLatitude(time, planetName);

                //if kranti (declination), is a negative number, it means south, else north of equator
                var kranti = lat.TotalDegrees + eps * Math.Sin(DEG2RAD * tlen.TotalDegrees);

                return kranti;
            }

        }

        /// <summary>
        /// true obliquity of the Ecliptic (includes nutation)
        /// </summary>
        public static double EclipticObliquity(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(EclipticObliquity), time, Ayanamsa), _getPlanetEps);


            //UNDERLYING FUNCTION
            double _getPlanetEps()
            {
                double eps;

                string err = "";
                double[] x = new double[6];

                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                var jul_day_ET = Calculate.TimeToEphemerisTime(time);

                ephemeris.swe_calc(jul_day_ET, SwissEph.SE_ECL_NUT, 0, x, ref err);

                eps = x[0];

                return eps;
            }

        }

        /// <summary>
        /// Hora Bala AKA Horadhipathi Bala
        /// </summary>
        public static Shashtiamsa PlanetHoraBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetHoraBala), planetName, time, Ayanamsa), _getPlanetHoraBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetHoraBala()
            {
                //first ascertain the weekday of birth
                var birthWeekday = Calculate.DayOfWeek(time);

                //ascertain the number of hours elapsed from sunrise to birth
                //This shows the number of horas passed.
                var hora = Calculate.HoraAtBirth(time);

                //get lord of hora (hour)
                var lord = Calculate.LordOfHora(hora, birthWeekday);

                //planet inputed is lord of hora, then 60 shashtiamsas
                if (lord == planetName)
                {
                    return new Shashtiamsa(60);
                }
                else
                {
                    return Shashtiamsa.Zero;
                }

            }



        }

        /// <summary>
        /// The planet who is the king of
        /// the year of birth is assigned a value of 15 Shashtiamsas
        /// as his Abdabala.
        /// </summary>
        public static Shashtiamsa PlanetAbdaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(PlanetAbdaBala), planetName, time, Ayanamsa), _getPlanetAbdaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAbdaBala()
            {
                //calculate year lord
                dynamic yearAndMonthLord = YearAndMonthLord(time);
                PlanetName yearLord = yearAndMonthLord.YearLord;

                //if inputed planet is year lord than 15 Shashtiamsas
                if (yearLord == planetName) { return new Shashtiamsa(15); }

                //not year lord, 0 Shashtiamsas
                return Shashtiamsa.Zero;
            }


        }

        /// <summary>
        /// Gets a planet's masa bala
        /// the lord of the month of birth is assigned a value of 30 Shashtiamsas as his Masabala
        /// </summary>
        public static Shashtiamsa PlanetMasaBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the month of birth is assigned a value of 30 Shashtiamsas
            //as his Masabala.

            //calculate month lord
            dynamic yearAndMonthLord = YearAndMonthLord(time);
            PlanetName monthLord = yearAndMonthLord.MonthLord;

            //if inputed planet is month lord than 30 Shashtiamsas
            if (monthLord == planetName) { return new Shashtiamsa(30); }

            //not month lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;
        }

        public static Shashtiamsa PlanetVaraBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the day of birth is assigned a value of 45 Shashtiamsas
            //as his Varabala.

            //calculate day lord
            PlanetName dayLord = Calculate.LordOfWeekday(time);

            //if inputed planet is day lord than 45 Shashtiamsas
            if (dayLord == planetName) { return new Shashtiamsa(45); }

            //not day lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        /// <summary>
        /// Gets year & month lord at inputed time
        /// </summary>
        public static object YearAndMonthLord(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(YearAndMonthLord), time, Ayanamsa), _getYearAndMonthLord);


            //UNDERLYING FUNCTION
            object _getYearAndMonthLord()
            {
                //set default
                var yearLord = Library.PlanetName.Sun;
                var monthLord = Library.PlanetName.Sun;

                //initialize ephemeris
                using SwissEph ephemeris = new SwissEph();

                double ut_arghana = ephemeris.swe_julday(1827, 5, 2, 0, SwissEph.SE_GREG_CAL);
                double ut_noon = Calculate.GreenwichLmtInJulianDays(time);

                double diff = ut_noon - ut_arghana;
                if (diff >= 0)
                {
                    double quo = Math.Floor(diff / 360.0);
                    diff -= quo * 360.0;
                }
                else
                {
                    double pdiff = -diff;
                    double quo = Math.Ceiling(pdiff / 360.0);
                    diff += quo * 360.0;
                }

                double diff_year = diff;
                while (diff > 30.0) diff -= 30.0;
                double diff_month = diff;
                while (diff > 7) diff -= 7.0;

                var yearLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_year);
                var monthLordRaw = ephemeris.swe_day_of_week(ut_noon - diff_month);

                //parse raw weekday
                var yearWeekday = swissEphWeekDayToMuhurthaDay(yearLordRaw);
                var monthWeekday = swissEphWeekDayToMuhurthaDay(monthLordRaw);


                //Abdadbipat : the planet that rules over the weekday on which the year begins (hindu year)
                yearLord = Calculate.LordOfWeekday(yearWeekday);

                //Masadhipath : The planet that rules the weekday of the commencement of the month of the birth
                monthLord = Calculate.LordOfWeekday(monthWeekday);

                //package year & month lord together & return
                return new { YearLord = yearLord, MonthLord = monthLord };


                //---------------------FUNCTION--------------------

                //converts swiss epehmeris weekday numbering to muhurtha weekday numbering
                DayOfWeek swissEphWeekDayToMuhurthaDay(int dayNumber)
                {
                    switch (dayNumber)
                    {
                        case 0: return Library.DayOfWeek.Monday;
                        case 1: return Library.DayOfWeek.Tuesday;
                        case 2: return Library.DayOfWeek.Wednesday;
                        case 3: return Library.DayOfWeek.Thursday;
                        case 4: return Library.DayOfWeek.Friday;
                        case 5: return Library.DayOfWeek.Saturday;
                        case 6: return Library.DayOfWeek.Sunday;
                        default: throw new Exception("Invalid day number!");
                    }
                }

            }

        }

        /// <summary>
        /// Thribhagabala : Mercury, the Sun and
        /// Saturn get 60 shashtiamsas each, during the lst,
        /// 2nd and 3rd one-third positions of the day, respectively.
        /// The Moon, Venus and Mars govern the lst, 2nd and 3rd one-third portion of the night
        /// respectively. Jupiter is always strong and gets 60
        /// shashtiamsas of strength.
        /// </summary>
        public static Shashtiamsa PlanetTribhagaBala(PlanetName planetName, Time time)
        {
            PlanetName ret = Library.PlanetName.Jupiter;

            var sunsetTime = Calculate.SunsetTime(time);

            if (IsDayBirth(time))
            {
                //find out which part of the day birth took place

                var sunriseTime = Calculate.SunriseTime(time);

                //substraction should always return a positive number, since sunset is always after sunrise
                double lengthHours = (sunsetTime.Subtract(sunriseTime).TotalHours) / 3;
                double offset = time.Subtract(sunriseTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Mercury; break;
                    case 1: ret = Library.PlanetName.Sun; break;
                    case 2: ret = Library.PlanetName.Saturn; break;
                }
            }
            else
            {
                //get sunrise time at on next day to get duration of the night
                var nextDayTime = time.AddHours(24);
                var nextDaySunrise = Calculate.SunriseTime(nextDayTime);

                double lengthHours = (nextDaySunrise.Subtract(sunsetTime).TotalHours) / 3;
                double offset = time.Subtract(sunsetTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = Library.PlanetName.Moon; break;
                    case 1: ret = Library.PlanetName.Venus; break;
                    case 2: ret = Library.PlanetName.Mars; break;
                }
            }

            //Always assign a value of 60 Shashtiamsas
            //to Guru irrespective of whether birth is during
            //night or during day.
            if (planetName == Library.PlanetName.Jupiter || planetName == ret) { return new Shashtiamsa(60); }

            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Oochchabala : The distance between the
        /// planet's longitude and its debilitation point, divided
        /// by 3, gives its exaltation strength or oochchabaJa.
        /// </summary>
        public static Shashtiamsa PlanetOchchaBala(PlanetName planetName, Time time)
        {
            //1.0 Get Planet longitude
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName);

            //2.0 Get planet debilitation point
            var planetDebilitationPoint = Calculate.PlanetDebilitationPoint(planetName);
            //convert to planet longitude
            var debilitationLongitude = LongitudeAtZodiacSign(planetDebilitationPoint);

            //3.0 Get difference between planet longitude & debilitation point
            //var difference = planetLongitude.GetDifference(planetDebilitationPoint); //todo need checking
            var difference = DistanceBetweenPlanets(planetLongitude, debilitationLongitude);

            //4.0 If difference is more than 180 degrees
            if (difference.TotalDegrees > 180)
            {
                //get the difference of it with 360 degrees
                //difference = difference.GetDifference(Angle.Degrees360);
                difference = Calculate.DistanceBetweenPlanets(difference, Angle.Degrees360);

            }

            //5.0 Divide difference with 3 to get ochchabala
            var ochchabalaInShashtiamsa = difference.TotalDegrees / 3;

            //return value in shashtiamsa type
            return new Shashtiamsa(ochchabalaInShashtiamsa);
        }



        /// <summary>
        /// Determines if the input time is day during day, used for birth times
        /// if day returns true
        /// </summary>
        public static bool IsDayBirth(Time time)
        {
            //get sunrise & sunset times
            var sunrise = Calculate.SunriseTime(time).GetLmtDateTimeOffset();
            var sunset = Calculate.SunsetTime(time).GetLmtDateTimeOffset();
            var checkingTime = time.GetLmtDateTimeOffset();

            //if time is after sunrise & before sunset, than it is during the day
            if (checkingTime >= sunrise && checkingTime <= sunset)
            {
                return true;
            }
            //else during night
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Pakshabala : When the Moon is waxing,
        /// take the distance from the Sun to the Moon and
        /// divide it by 3. The quotient is the Pakshabala.
        /// When the Moon is waning, take the distance from
        /// the Moon to the· Sun, and divide it by 3 for assessing
        /// Pakshabala. Moon, Jupiter, Venus and
        /// Mercury are strong in Sukla Paksba and the others
        /// in Krishna Paksha.
        ///
        /// Note: Mercury is benefic or malefic based on planets conjunct with it
        /// </summary>
        public static Shashtiamsa PlanetPakshaBala(PlanetName planetName, Time time)
        {
            double pakshaBala = 0;

            //get moon phase
            var moonPhase = Calculate.LunarDay(time).GetMoonPhase();

            var sunLongitude = Calculate.PlanetNirayanaLongitude(time, Library.PlanetName.Sun);
            var moonLongitude = Calculate.PlanetNirayanaLongitude(time, Library.PlanetName.Moon);

            //var differenceBetweenMoonSun = moonLongitude.GetDifference(sunLongitude);
            var differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(moonLongitude, sunLongitude);

            //When Moon's Long.-Sun's Long. exceeds 180, deduct it from 360°
            if (differenceBetweenMoonSun.TotalDegrees > 180)
            {
                differenceBetweenMoonSun = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360);
            }

            double pakshaBalaOfSubhas = 0;

            //get paksha Bala Of Subhas
            if (moonPhase == MoonPhase.BrightHalf)
            {
                //If birth has occurred during Sukla Paksha subtract the Sun's longitude from that of the Moon· Divide the
                // balance by 3. The result represents the Paksha Bala of Subhas.
                pakshaBalaOfSubhas = differenceBetweenMoonSun.TotalDegrees / 3.0;
            }
            else if (moonPhase == MoonPhase.DarkHalf)
            {
                //Subtract the remainder again from 360 degrees and divide the balance divide 3
                var totalDegrees = Calculate.DistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360).TotalDegrees;
                pakshaBalaOfSubhas = totalDegrees / 3.0;
            }

            //60 Shashtiamsas diminished by paksha Bala Of Subhas gives the Paksha Bala of Papas
            var pakshaBalaOfPapas = 60.0 - pakshaBalaOfSubhas;

            //if planet is malefic
            var planetIsMalefic = Calculate.IsPlanetMalefic(planetName, time);
            var planesIsBenefic = Calculate.IsPlanetBenefic(planetName, time);

            if (planesIsBenefic == true && planetIsMalefic == false)
            {
                pakshaBala = pakshaBalaOfSubhas;
            }

            if (planesIsBenefic == false && planetIsMalefic == true)
            {
                pakshaBala = pakshaBalaOfPapas;
            }

            //Chandra's Paksha Bala is always to be doubled
            if (planetName == Library.PlanetName.Moon)
            {
                pakshaBala = pakshaBala * 2.0;
            }

            //if paksha bala is 0
            if (pakshaBala == 0)
            {
                //raise error
                throw new Exception("Paksha bala not found, error!");
            }

            return new Shashtiamsa(pakshaBala);
        }

        /// <summary>
        /// Nathonnathabala: Midnight to midday,
        /// the Sun, Jupiter and Venus gain strength proportionately
        /// till they get maximum at zenith. The other
        /// planets, except Mercury. a,re gaining strength from
        /// midday to midnight proportionately. In the same
        /// way, Mercury is always strong and gets 60 shashtiamsas.
        /// </summary>
        public static Shashtiamsa PlanetNathonnathaBala(PlanetName planetName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            //get local apparent time
            var localApparentTime = Calculate.LocalApparentTime(time);

            //convert birth time (reckoned from midnight) into degrees at 15 degrees per hour
            var hour = localApparentTime.Hour;
            var minuteInHours = localApparentTime.Minute / 60.0;
            var secondInHours = localApparentTime.Second / 3600.0;
            //total hours
            var totalTimeInHours = hour + minuteInHours + secondInHours;

            //convert hours to degrees
            const double degreesPerHour = 15;
            var birthTimeInDegrees = totalTimeInHours * degreesPerHour;

            //if birth time in degrees exceeds 180 degrees subtract it from 360
            if (birthTimeInDegrees > 180)
            {
                birthTimeInDegrees = 360 - birthTimeInDegrees;
            }

            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Venus)
            {
                var divBala = birthTimeInDegrees / 3;

                return new Shashtiamsa(divBala);
            }

            if (planetName == Library.PlanetName.Saturn || planetName == Library.PlanetName.Moon || planetName == Library.PlanetName.Mars)
            {
                var ratriBala = (180 - birthTimeInDegrees) / 3;

                return new Shashtiamsa(ratriBala);
            }

            if (planetName == Library.PlanetName.Mercury)
            {
                //Budha has always a Divaratri Bala of 60 Shashtiamsas
                return new Shashtiamsa(60);

            }

            throw new Exception("Planet Nathonnatha Bala not found, error!");
        }

        /// <summary>
        /// Gets Dig Bala of a planet.
        /// Jupiter and Mercury are strong in Lagna (Ascendant),
        /// the Sun and Mars in the 10th, Saturn in
        /// the 7th and the Moon and Venus in the 4th. The
        /// opposite houses are weak , points. Divide the
        /// distance between the longitude of the planet and
        /// its depression point by 3. Quotient is the strength.
        /// </summary>
        public static Shashtiamsa PlanetDigBala(PlanetName planetName, Time time)
        {
            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == Library.PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == Library.PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return Shashtiamsa.Zero; }


            //get planet longitude
            var planetLongitude = Calculate.PlanetNirayanaLongitude(time, planetName);

            //
            Angle powerlessPointLongitude = null;
            House powerlessHouse;


            //subtract the longitude of the 4th house from the longitudes of the Sun and Mars.
            if (planetName == Library.PlanetName.Sun || planetName == Library.PlanetName.Mars)
            {
                powerlessHouse = Calculate.House(HouseName.House4, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtract the 7th house, from Jupiter and Mercury.
            if (planetName == Library.PlanetName.Jupiter || planetName == Library.PlanetName.Mercury)
            {
                powerlessHouse = Calculate.House(HouseName.House7, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtracc the 10th house from Venus and the Moon
            if (planetName == Library.PlanetName.Venus || planetName == Library.PlanetName.Moon)
            {
                powerlessHouse = Calculate.House(HouseName.House10, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //from Saturn, the ascendant.
            if (planetName == Library.PlanetName.Saturn)
            {
                powerlessHouse = Calculate.House(HouseName.House1, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //get Digbala arc
            //Digbala arc= planet's long. - its powerless cardinal point.
            //var digBalaArc = planetLongitude.GetDifference(powerlessPointLongitude);
            var xxx = powerlessPointLongitude.TotalDegrees == null ? Angle.Zero : powerlessPointLongitude;
            var digBalaArc = Calculate.DistanceBetweenPlanets(planetLongitude, xxx);

            //If difference is more than 180° 
            if (digBalaArc > Angle.Degrees180)
            {
                //subtract it from 360 degrees.
                //digBalaArc = digBalaArc.GetDifference(Angle.Degrees360);
                digBalaArc = Calculate.DistanceBetweenPlanets(digBalaArc, Angle.Degrees360);
            }

            //The Digbala arc of a ptanet, divided by 3, gives the Digbala
            var digBala = digBalaArc.TotalDegrees / 3;



            return new Shashtiamsa(digBala);

        }

        /// <summary>
        /// Bhava Bala.-Bhava means house and
        /// Bala means strength. Bhava Bala is the potency or
        /// strength of the house or bhava or signification. We
        /// have already seen that there are 12 bhavas which
        /// comprehend all human events. Each bhava signifies
        /// or indicates certain events or functions. For
        /// instance, the first bhava represents Thanu or body,
        /// the appearance of the individual, his complexion,
        /// his disposition, his stature, etc.
        ///
        /// If it attains certain strength, the native will enjoy the indications of
        /// the bhava fully, otherwise he will not sufficiently
        /// enjoy them. The strength of a bhava is composed
        /// of three factors, viz., (1) Bhavadhipathi Bala,
        /// (2) Bhava Digbala, (3) Bhava Drishti Bala.
        /// </summary>
        public static Shashtiamsa HouseStrength(HouseName inputHouse, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseStrength), inputHouse, time, Ayanamsa), _getBhavabala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getBhavabala()
            {
                //get all the sub-strengths into a list 
                var subStrengthList = new List<HouseSubStrength>();

                subStrengthList.Add(BhavaAdhipathiBala(time));
                subStrengthList.Add(CalcBhavaDigBala(time));
                subStrengthList.Add(BhavaDrishtiBala(time));

                var totalBhavaBala = new Dictionary<HouseName, double>();

                foreach (var houseToTotal in Library.House.AllHouses)
                {
                    //to get the total strength of the a house, we combine 3 types sub-strengths
                    double total = 0;
                    foreach (var subStrength in subStrengthList) { total += subStrength.Power[houseToTotal]; }
                    totalBhavaBala[houseToTotal] = total;
                }

                return new Shashtiamsa(totalBhavaBala[inputHouse]);

            }

        }

        /// <summary>
        /// House received aspect strength
        /// 
        /// Bhavadrishti Bala.-Each bhava in a
        /// horoscope remains aspected by certain planets.
        /// Sometimes the aspect cast on a bhava will be positive
        /// and sometimes it will be negative according
        /// as it is aspected by benefics or malefics.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength BhavaDrishtiBala(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaDrishtiBala), time, Ayanamsa), _calcBhavaDrishtiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaDrishtiBala()
            {
                double vdrishti;

                //assign initial negative or positive value based on benefic or malefic planet
                var sp = goodAndBad();


                var drishti = GetDrishtiKendra(time);


                double bala = 0;

                var BhavaDrishtiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    bala = 0;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {

                        bala += (sp[planet] * drishti[planet.ToString() + house.ToString()]);

                    }

                    BhavaDrishtiBala[house] = bala;
                }


                var newHouseResult = new HouseSubStrength(BhavaDrishtiBala, "BhavaDrishtiBala");

                return newHouseResult;



                //------------------LOCAL FUNCTIONS---------------------

                Dictionary<PlanetName, int> goodAndBad()
                {

                    var _sp = new Dictionary<PlanetName, int>();

                    //assign initial negative or positive value based on benefic or malefic planet
                    foreach (var p in Library.PlanetName.All7Planets)
                    {
                        //Though in the earlier pages Mercury is defined either as a subba
                        //(benefic) or papa (malefic) according to its association is with a benefic or
                        //malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
                        //be deemed as a full benefic. This is in accord with the injunctions of
                        //classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).

                        if (p == Library.PlanetName.Mercury)
                        {
                            _sp[p] = 1;
                            continue;
                        }

                        if (Calculate.IsPlanetBenefic(p, time))
                        {
                            _sp[p] = 1;
                        }
                        else
                        {
                            _sp[p] = -1;
                        }
                    }

                    return _sp;
                }

                Dictionary<String, double> GetDrishtiKendra(Time time1)
                {

                    //planet & house no. is used key
                    var _drishti = new Dictionary<String, double>();

                    double drishtiKendra;

                    foreach (var planet in Library.PlanetName.All7Planets)
                    {
                        foreach (var houseNo in Library.House.AllHouses)
                        {
                            //house is considered as a Drusya Graha (aspected body)
                            var houseMid = Calculate.House(houseNo, time1).GetMiddleLongitude();
                            var plantLong = Calculate.PlanetNirayanaLongitude(time1, planet);

                            //Subtract the longitude of the Drishti (aspecting)
                            // planet from that of the Drusya (aspected) Bhava
                            // Madhya. The Drishti Kendra is obtained.
                            drishtiKendra = (houseMid - plantLong).TotalDegrees;

                            //In finding the Drishti Kendra always add 360° to the longitude of the
                            //Drusya (Bhava Madhya) when it is less than the longitude of the
                            //Drishta (aspecting Graha).
                            if (drishtiKendra < 0) { drishtiKendra += 360; }

                            //get special aspect if any
                            vdrishti = FindViseshaDrishti(drishtiKendra, planet);

                            if ((planet == Library.PlanetName.Mercury) || (planet == Library.PlanetName.Jupiter))
                            {
                                //take the Drishti values of Guru and Budha on the Bhava Madhya as they are
                                _drishti[planet.ToString() + (houseNo).ToString()] = FindDrishtiValue(drishtiKendra) + vdrishti;
                            }
                            else
                            {
                                //take a fourth of the aspect value of other Grahas over the Bhava Madhya
                                _drishti[planet.ToString() + (houseNo).ToString()] = (FindDrishtiValue(drishtiKendra) + vdrishti) / 4.00;
                            }
                        }
                    }


                    return _drishti;
                }
            }

        }

        /// <summary>
        /// House strength from different types of signs
        /// 
        /// Bhava Digbala.-This is the strength
        /// acquired by the different bhavas falling in the
        /// different groups or types of signs.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength CalcBhavaDigBala(Time time)
        {

            var BhavaDigBala = new Dictionary<HouseName, double>();

            int dig = 0;

            //for every house
            foreach (var houseNumber in Library.House.AllHouses)
            {
                //a particular bhava acquires strength by its mid-point
                //falling in a particular kind of sign.

                //so get mid point of house
                var mid = Calculate.House(houseNumber, time).GetMiddleLongitude().TotalDegrees;
                var houseSign = Calculate.HouseSignName(houseNumber, time);

                //Therefore first find the number of a given Bhava Madhya and subtract
                // it from 1, if the given Bhava Madhya is situated
                // in Vrischika
                if ((mid >= 210.00)
                    && (mid <= 240.00))
                {
                    dig = 1 - (int)houseNumber;
                }
                //Subtract it from 4, if the given Bhava
                // Madhya is situated in Mesha, Vrishabha, Simha,
                // first half of Makara or last half of Dhanus.
                else if (((mid >= 0.00) && (mid <= 60.00))
                         || ((mid >= 120.00) && (mid <= 150.00))
                         || ((mid >= 255.00) && (mid <= 285.00)))
                {
                    dig = 4 - (int)houseNumber;
                }
                //Subtract it from 7 if in Mithuna, Thula, Kumbha, Kanya or
                // first half of Dhanus
                else if (((mid >= 60.00) && (mid <= 90.00))
                         || ((mid >= 150.00) && (mid <= 210.00))
                         || ((mid >= 300.00) && (mid <= 330.00))
                         || ((mid >= 240.00) && (mid <= 255.00)))
                {
                    dig = 7 - (int)houseNumber;
                }
                //and lastly from 1O if in Kataka, Meena and last half of Makara.
                else if (((mid >= 90.00) && (mid <= 120.00))
                         || ((mid >= 330.00) && (mid <= 360.00))
                         || ((mid >= 285.00) && (mid <= 300.00)))
                {
                    dig = 10 - (int)houseNumber;
                }


                //If the difference exceeds 6, subtract it from 12, otherwise
                //take it as it is and multiply this difference by 1O.
                //And you will get Bhava digbala of the particular bhava.

                if (dig < 0)
                {
                    dig = dig + 12;
                }

                if (dig > 6)
                {
                    dig = 12 - dig;
                }

                //store digbala value in return list with house number
                BhavaDigBala[houseNumber] = (double)dig * 10;

            }


            var newHouseResult = new HouseSubStrength(BhavaDigBala, "BhavaDigBala");

            return newHouseResult;

        }

        /// <summary>
        /// Bhavadhipatbi Bala: This is the potency
        /// of the lord of the bhava.
        /// For all 12 houses
        /// </summary>
        public static HouseSubStrength BhavaAdhipathiBala(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(BhavaAdhipathiBala), time, Ayanamsa), _calcBhavaAdhipathiBala);


            //UNDERLYING FUNCTION
            HouseSubStrength _calcBhavaAdhipathiBala()
            {
                PlanetName houseLord;

                var BhavaAdhipathiBala = new Dictionary<HouseName, double>();

                foreach (var house in Library.House.AllHouses)
                {

                    //get current house lord
                    houseLord = Calculate.LordOfHouse(house, time);

                    //The Shadbala Pinda (aggregate of the Shadbalas) of the lord of the
                    //bhava constitutes its Bhavadhipathi Bala.
                    //get Shadbala Pinda of lord (total strength) in shashtiamsas
                    BhavaAdhipathiBala[house] = PlanetShadbalaPinda(houseLord, time).ToDouble();

                }

                var newHouseResult = new HouseSubStrength(BhavaAdhipathiBala, "BhavaAdhipathiBala");

                return newHouseResult;

            }

        }

        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            //get all planets
            //var allPlanetByStrenght = AstronomicalCalculator.GetAllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 > threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        public static List<PlanetName> BeneficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;
        }


        /// <summary>
        /// 0 index is strongest
        /// </summary>
        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength > threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;


        }

        public static List<HouseName> BeneficHouseListByShadbala(Time personBirthTime)
        {
            //get all planets
            var allPlanetByStrenght = Calculate.AllHousesOrderedByStrength(personBirthTime);

            //take top 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[0]);
            //returnList.Add(allPlanetByStrenght[1]);
            //returnList.Add(allPlanetByStrenght[2]);

            return returnList;


        }

        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime, int threshold)
        {

            var returnList = new List<PlanetName>();
            var yyy = Calculate.AllPlanetStrength(personBirthTime);
            foreach (var planet in yyy)
            {
                if (planet.Item1 < threshold)
                {
                    returnList.Add(planet.Item2);
                }
            }
            return returnList;
        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<PlanetName> MaleficPlanetListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllPlanetOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<PlanetName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        /// <summary>
        /// 0 index is most malefic
        /// </summary>
        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime, int threshold)
        {
            var returnList = new List<HouseName>();

            //create a list with planet names & its strength (unsorted)
            foreach (var house in Library.House.AllHouses)
            {
                //get house strength
                var strength = HouseStrength(house, personBirthTime).ToDouble();

                if (strength < threshold)
                {
                    returnList.Add(house);
                }


            }

            return returnList;
        }

        public static List<HouseName> MaleficHouseListByShadbala(Time personBirthTime)
        {

            //get all planets
            var allPlanetByStrenght = Calculate.AllHousesOrderedByStrength(personBirthTime);

            //take last 3 as needed planets
            var returnList = new List<HouseName>();
            returnList.Add(allPlanetByStrenght[^1]);
            //returnList.Add(allPlanetByStrenght[^2]);
            //returnList.Add(allPlanetByStrenght[^3]);

            return returnList;

        }

        #endregion

        #region TAGS STATIC

        /// <summary>
        /// keywords or tag related to a house
        /// </summary>
        public static string GetHouseTags(HouseName house)
        {
            switch (house)
            {
                case HouseName.House1: return "beginning of life, childhood, health, environment, personality, the physical body and character";
                case HouseName.House2: return "family, face, right eye, food, wealth, literary gift, and manner and source of death, self-acquisition and optimism";
                case HouseName.House3: return "brothers and sisters, intelligence, cousins and other immediate relations";
                case HouseName.House4: return "peace of mind, home life, mother, conveyances, house property, landed and ancestral properties, education and neck and shoulders";
                case HouseName.House5: return "children, grandfather, intelligence, emotions and fame";
                case HouseName.House6: return "debts, diseases, enemies, miseries, sorrows, illness and disappointments";
                case HouseName.House7: return "wife, husband, marriage, urinary organs, marital happiness, sexual diseases, business partner, diplomacy, talent, energies and general happiness";
                case HouseName.House8: return "longevity, legacies and gifts and unearned wealth, cause of death, disgrace, degradation and details pertaining to death";
                case HouseName.House9: return "father, righteousness, preceptor, grandchildren, intuition, religion, sympathy, fame, charities, leadership, journeys and communications with spirits";
                case HouseName.House10: return "occupation, profession, temporal honours, foreign travels, self-respect, knowledge and dignity and means of livelihood";
                case HouseName.House11: return "means of gains, elder brother and freedom from misery";
                case HouseName.House12: return "losses, expenditure, waste, extravagance, sympathy, divine knowledge, Moksha and the state after death";
                default: throw new Exception("House details not found!");
            }
        }

        /// <summary>
        /// Given a zodiac sign, will return astro keywords related to sign
        /// These details would be highly useful in the delineation of
        /// character and mental disposition
        /// Source:Hindu Predictive Astrology pg.16
        /// </summary>
        public static string GetSignTags(ZodiacName zodiacName)
        {
            switch (zodiacName)
            {
                case ZodiacName.Aries:
                    return @"movable, odd, masculine, cruel, fiery, of short ascension, rising by hinder part, powerful during the night";
                case ZodiacName.Taurus:
                    return @"fixed, even, feminine, mild,earthy, fruitful, of short ascension, rising by hinder part";
                case ZodiacName.Gemini:
                    return @"common, odd, masculine, cruel, airy, barren, of short ascension, rising by the head.";
                case ZodiacName.Cancer:
                    return @"even, movable, feminine, mild, watery, of long ascension, rising by the hinder part and fruitful.";
                case ZodiacName.Leo:
                    return @"fixed, odd, masculine, cruel, fiery, of long ascension, barren, rising by the head.";
                case ZodiacName.Virgo:
                    return @"common, even, feminine, mild, earthy, of long ascension, rising by the head.";
                case ZodiacName.Libra:
                    return @"movable, odd, masculine, cruel, airy, of long ascension, rising by the head.";
                case ZodiacName.Scorpio:
                    return @"fixed, even, feminine, mild, watery, of long ascension, rising by the head.";
                case ZodiacName.Sagittarius:
                    return @"common, odd, masculine, cruel, fiery, of long ascension, rising by the hinder part.";
                case ZodiacName.Capricorn:
                    return @"movable, even, feminine, mild, earthy, of long ascension, rising by hinder part";
                case ZodiacName.Aquarius:
                    return @"fixed, odd, masculine, cruel, fruitful, airy, of short ascension, rising by the head.";
                case ZodiacName.Pisces:
                    return @"common, feminine, water, even, mild, of short ascension, rising by head and hinder part.";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Get keywords related to a planet.
        /// </summary>
        public static string GetPlanetTags(PlanetName lordOfHouse)
        {
            switch (lordOfHouse.Name)
            {
                case PlanetName.PlanetNameEnum.Sun:
                    return "Father, masculine, malefic, copper colour, philosophical tendency, royal, ego, sons, patrimony, self reliance, political power, windy and bilious temperament, month, places of worship, money-lenders, goldsmith, bones, fires, coronation chambers, doctoring capacity";
                case PlanetName.PlanetNameEnum.Moon:
                    return "Mother, feminine, mind, benefic when waxing, malefic when waning, white colour, women, sea-men, pearls, gems, water, fishermen, stubbornness, romances, bath-rooms, blood, popularity, human responsibilities";
                case PlanetName.PlanetNameEnum.Mars:
                    return "Brothers, masculine, blood-red colour, malefic, refined taste, base metals, vegetation, rotten things, orators, ambassadors, military activities, commerce, aerial journeys, weaving, public speakers.";
                case PlanetName.PlanetNameEnum.Mercury:
                    return "Profession, benefic if well associated, hermaphrodite, green colour, mercantile activity, public speakers, cold nervous, intelligence";
                case PlanetName.PlanetNameEnum.Jupiter:
                    return "Children, masculine, benefic, bright yellow colour, devotion, truthfulness, religious fervour, philosophical wisdom, corpulence";
                case PlanetName.PlanetNameEnum.Venus:
                    return "Wife, feminine, benefic, mixture of all colours, love affairs, sensual pleasure, family bliss, harems of ill-fame, vitality";
                case PlanetName.PlanetNameEnum.Saturn:
                    return "Longevity, malefic, hermaphrodite, dark colour, stubbornness, impetuosity, demoralisation, windy diseases, despondency, gambling";
                case PlanetName.PlanetNameEnum.Rahu:
                    return "Maternal relations, malefic, feminine, renunciation, corruption, epidemics";
                case PlanetName.PlanetNameEnum.Ketu:
                    return "Paternal relations, Hermaphrodite, malefic, religious, sectarian principles, pride, selfishness, occultism, mendicancy";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Source: Hindu Predictive Astrology pg.17
        /// </summary>
        public static string GetHouseType(HouseName houseNumber)
        {
            //Quadrants (kendras) are l, 4, 7 and 10.
            //Trines(Trikonas) are 5 and 9.
            //Cadent houses (Panaparas) are 2, 5, 8 and 11
            //Succeedent houses (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
            //Upachayas are 3, 6, 10 and 11.

            var returnString = "";

            switch (houseNumber)
            {
                //Quadrants (kendras) are l, 4, 7 and 10.
                case HouseName.House1:
                case HouseName.House4:
                case HouseName.House7:
                case HouseName.House10:
                    returnString += @"Quadrants (kendras)";
                    break;
                //Trines(Trikonas) are 5 and 9.
                case HouseName.House5:
                case HouseName.House9:
                    returnString += @"Trines (Trikonas)";
                    break;
            }

            switch (houseNumber)
            {
                //Cadent (Panaparas) are 2, 5, 8 and 11
                case HouseName.House2:
                case HouseName.House5:
                case HouseName.House8:
                case HouseName.House11:
                    returnString += @"Cadent (Panaparas)";
                    break;
                //Succeedent (Apoklimas) are 3, 6, 9 and 12 (9th being a trikona must be omitted)
                case HouseName.House3:
                case HouseName.House6:
                case HouseName.House9:
                case HouseName.House12:
                    returnString += @"Succeedent (Apoklimas)";
                    break;
            }

            switch (houseNumber)
            {
                //Upachayas are 3, 6, 10 and 11.
                case HouseName.House3:
                case HouseName.House6:
                case HouseName.House10:
                case HouseName.House11:
                    returnString += @"Upachayas";
                    break;

            }

            return returnString;
        }

        /// <summary>
        /// Get general planetary info for person's dasa (hardcoded table)
        /// It is intended to be used to interpret dasa predictions
        /// as such should be displayed next to dasa chart.
        /// This method is direct translation from the book.
        /// Similar to method GetPlanetDasaNature
        /// Data from pg 80 of Key-planets for Each Sign in Hindu Predictive Astrology
        /// </summary>
        public static string GetDasaInfoForAscendant(ZodiacName ascendantName)
        {
            //As soon as tbc Dasas and Bhuktis are determined, the next
            //step would be to find out the good and evil planets for each
            //ascendant so that in applying the principles to decipher the
            //future history of man, the student may be able to carefully
            //analyse the intensilty or good or evil combinations and proceed
            //further with his predictions when applying the results of
            //Dasas and other combinations.

            switch (ascendantName)
            {
                case ZodiacName.Aries:
                    return @"
                        Aries - Saturn, Mercury and Venus are ill-disposed.
                        Jupiter and the Sun are auspicious. The mere combination
                        of Jupiler and Saturn produces no beneficial results. Jupiter
                        is the Yogakaraka or the planet producing success. If Venus
                        becomes a maraka, he will not kill the native but planets like
                        Saturn will bring about death to the person.
                        ";
                case ZodiacName.Taurus:
                    return @"
                        Taurus - Saturn is the most auspicious and powerful
                        planet. Jupiter, Venus and the Moon are evil planets. Saturn
                        alone produces Rajayoga. The native will be killed in the
                        periods and sub-periods of Jupiter, Venus and the Moon if
                        they get death-inflicting powers.
                        ";
                case ZodiacName.Gemini:
                    return @"
                        Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                        is most beneficial and in conjunction with Saturn in good signs
                        produces and excellent career of much fame. Combination
                        of Saturn and Jupiter produces similar results as in Aries.
                        Venus and Mercury, when well associated, cause Rajayoga.
                        The Moon will not kill the person even though possessed of
                        death-inflicting powers.
                        ";
                case ZodiacName.Cancer:
                    return @"
                        Cancer - Venus and Mercury are evil. Jupiter and Mars
                        give beneficial results. Mars is the Rajayogakaraka
                        (conferor of name and fame). The combination of Mars and Jupiter
                        also causes Rajayoga (combination for political success). The
                        Sun does not kill the person although possessed of maraka
                        powers. Venus and other inauspicious planets kill the native.
                        Mars in combination with the Moon or Jupiter in favourable
                        houses especially the 1st, the 5th, the 9th and the 10th
                        produces much reputation.
                        ";
                case ZodiacName.Leo:
                    return @"
                        Leo - Mars is the most auspicious and favourable planet.
                        The combination of Venus and Jupiter does not cause Rajayoga
                        but the conjunction of Jupiter and Mars in favourable
                        houses produce Rajayoga. Saturn, Venus and Mercury are
                        evil. Saturn does not kill the native when he has the maraka
                        power but Mercury and other evil planets inflict death when
                        they get maraka powers.
                        ";
                case ZodiacName.Virgo:
                    return @"
                        Virgo - Venus alone is the most powerful. Mercury and
                        Venus when combined together cause Rajayoga. Mars and
                        the Moon are evil. The Sun does not kill the native even if
                        be becomes a maraka but Venus, the Moon and Jupiter will
                        inflict death when they are possessed of death-infticting power.
                        ";
                case ZodiacName.Libra:
                    return @"
                        Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                        and Mars are inauspicious. Mercury and Saturn produce good.
                        The conjunction of the Moon and Mercury produces Rajayoga.
                        Mars himself will not kill the person. Jupiter, Venus
                        and Mars when possessed of maraka powers certainly kill the
                        nalive.
                        ";
                case ZodiacName.Scorpio:
                    return @"
                        Scorpio - Jupiter is beneficial. The Sun and the Moon
                        produce Rajayoga. Mercury and Venus are evil. Jupiter,
                        even if be becomes a maraka, does not inflict death. Mercury
                        and other evil planets, when they get death-inlflicting powers,
                        do not certainly spare the native.
                        ";
                case ZodiacName.Sagittarius:
                    return @"
                        Sagittarius - Mars is the best planet and in conjunction
                        with Jupiter, produces much good. The Sun and Mars also
                        produce good. Venus is evil. When the Sun and Mars
                        combine together they produce Rajayoga. Saturn does not
                        bring about death even when he is a maraka. But Venus
                        causes death when be gets jurisdiction as a maraka planet.
                        ";
                case ZodiacName.Capricorn:
                    return @"
                        Capricorn - Venus is the most powerful planet and in
                        conjunction with Mercury produces Rajayoga. Mars, Jupiter
                        and the Moon are evil.
                        ";
                case ZodiacName.Aquarius:
                    return @"
                        Aquarius - Venus alone is auspicious. The combination of
                        Venus and Mars causes Rajayoga. Jupiter and the Moon are
                        evil.
                        ";
                case ZodiacName.Pisces:
                    return @"
                        Pisces - The Moon and Mars are auspicious. Mars is
                        most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                        Saturn, Venus, the Sun and Mercury are evil. Mars
                        himself does not kill the person even if he is a maraka.
                        ";
                default:
                    throw new ArgumentOutOfRangeException(nameof(ascendantName), ascendantName, null);
            }

        }

        #endregion

        //--------------------------------------------------------------------------------------------

#if DEBUG
        /// <summary>
        /// Special debug function
        /// </summary>
        public static string BouncBackInputAsString(PlanetName planetName, Time time) => planetName.ToString();

#endif
    }
}

