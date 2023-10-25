using System;
using System.Collections.Generic;
using SwissEphNet;
using VedAstro.Library;
using static VedAstro.Library.Calculate;

namespace VedAstro.Library
{
    public static class CalculateKP
    {
        /// <summary>
        /// Gets all KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// this is simply a test method to test the KPHoraryLongitudes and KPBirthtimLongitudes methods
        /// </summary>
        // Kp Cusp Calculations
        public static void KpPrintHouseCuspLong(int Ayanamsa,Time birthtime, int horNum)
        {
            Console.WriteLine("Birth Time is {0} ", birthtime);
            Console.WriteLine("Ayanamsa is {0} ", Calculate.Ayanamsa);
            var ayanamsaDegree = Calculate.AyanamsaDegree(birthtime);
            Console.WriteLine("Ayanamsa Degree is {0}, {1} Deg {2} Min {3} Secs ", ayanamsaDegree.Rounded,
                ayanamsaDegree.Degrees,
                ayanamsaDegree.Minutes, ayanamsaDegree.Seconds);

            Console.WriteLine("================== KP Houses and Planets - From SWISS EPH Modified Method ===================");
            //Choose one of the following statements. Comment the other one out for testing
            //var cusps = GetKPBirthTimeHouseLongitudes(Calculate.Ayanamsa, birthtime);
            var cusps = GetKPHoraryHouseLongitudes(Calculate.Ayanamsa, birthtime, horNum);

            int cnt = 0;

            Console.WriteLine(" ====== ");
            foreach (var h in cusps)
            {
                var cuspDMS = h.Value;

                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(cuspDMS);

                Console.WriteLine("House{0} Start Longitude {1} in DMS: {2} Deg {3} Mins {4} Secs; Zodiac {5} {6} Deg {7} Mins {8} Secs",
                    cnt, h,
                    cuspDMS.Degrees, cuspDMS.Minutes, cuspDMS.Seconds, zodiacSignAtLong.GetSignName(),
                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                    zodiacSignAtLong.GetDegreesInSign().Seconds);

                cnt++;

            }
            //House Processing Complete ------------------------------

            //Now Process the Planets
            var allPlanets = VedAstro.Library.PlanetName.All9Planets;
            var x = 0;
            var allPlanetConstellation = Calculate.AllPlanetConstellation(birthtime);
            /*
            foreach (var planetConstellation in allPlanetConstellation)
            {
                Console.WriteLine(" {0} {1}", planetConstellation.Key, planetConstellation.Value.GetConstellationName().ToString()); //allPlanetConstellation[0].GetQuarter().ToString());
                var yy = Calculate.LordOfConstellation(planetConstellation.Value.GetConstellationName());
            } */

            foreach (PlanetName planet in allPlanets)
            {
                Angle planetNirayanaDegrees = Calculate.PlanetNirayanaLongitude(birthtime, planet);
                Console.Write("{0} {1} {2} Deg {3} Min {4} Secs ; ", planet.Name, planetNirayanaDegrees.TotalDegrees,
                    planetNirayanaDegrees.Degrees, planetNirayanaDegrees.Minutes,
                    planetNirayanaDegrees.Seconds);
                var planetConstellation = Calculate.PlanetConstellation(birthtime, planet);

                x = 1;
                while ((x + 1) <= cusps.Count) //check each house for the logic below
                {
                    if ((x + 1) < cusps.Count) //Do not exceed the bounds of the array
                    {
                        if (cusps[(HouseName)x + 1] > cusps[(HouseName)x]) //check if cusp longitude is smaller than next cusp longitude
                                                     //because the last house will have cusp long larger then next house start
                        {
                            if ((planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)x].TotalDegrees) &&
                                (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)x + 1].TotalDegrees)) //this means that the planet falls in between these house cusps
                            {
                                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6}; StarL {7}", planet.Name, x, zodiacSignAtLong.GetSignName(),
                                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                    zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);
                                break;
                            }
                        }
                        else //if next cusp start long is smaller than current cusp we are rotating through 360 deg
                        {
                            if ((planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)x].TotalDegrees) ||
                                (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)x + 1].TotalDegrees))
                            {
                                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S; SignL {6} StarL {7} ", planet.Name, x, zodiacSignAtLong.GetSignName(),
                                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                    zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);
                                break;
                            }
                        }
                    }
                    else
                    {
                        var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                        var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                        var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                        Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6} StarL {0}", planet.Name, x, zodiacSignAtLong.GetSignName(),
                            zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                            zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);


                        break;
                    }
                    x++;
                }
            }
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

        public static Dictionary<HouseName, ZodiacSign> AllHouseSignKP(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, ZodiacSign>();

            //get for all houses
            foreach (var house in House.AllHouses)
            {
                var calcHouseSign = HouseSignKP(house, time);
                allHouses.Add(house, calcHouseSign);
            }

            return allHouses;
        }

        /// <summary>
        /// Gets the zodiac sign at begin longitude of the house with degrees data
        /// Specialized for KP astrology system
        /// </summary>
        public static ZodiacSign HouseSignKP(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseLongitudesKP(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            var beginLongitude = specifiedHouse.GetBeginLongitude();
            var houseSign = ZodiacSignAtLongitude(beginLongitude);

            //return the name of house sign
            return houseSign;
        }

        /// <summary>
        /// List of all planets and the houses they are located in at a given time
        /// using KP Krishnamurti system, note KP ayanamsa is hard set
        /// </summary>
        public static Dictionary<PlanetName, HouseName> AllPlanetHousePositionsKP(Time time)
        {
            //hard set KP ayanamsa to match commercial software default output
            Calculate.Ayanamsa = (int)Ayanamsa.KRISHNAMURTI;
            var returnList = new Dictionary<PlanetName, HouseName>();
            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = HousePlanetIsInKP(time, planet);
                returnList.Add(planet, houseIsIn);
            }
            return returnList;
        }

        /// <summary>

        /// <summary>
        /// Gets all KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// </summary>
        public static Dictionary<HouseName, Angle> GetKPHoraryHouseLongitudes(int Ayanamsa, Time time, int horaryNumber)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetKPHoraryHouseLongitudes), Ayanamsa, time, horaryNumber),
                _getHouseKPHoraryLongitudes);


            //UNDERLYING FUNCTION
            Dictionary <HouseName, Angle> _getHouseKPHoraryLongitudes()
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

                //CPJ - set this flag and use the swe_houses_ex method - it returns SIDEREAL KP Longitudes diretly.
                //No need for additional step of deducting ayanmsa from Sayana Longitudes
                var iflag = SwissEph.SEFLG_SIDEREAL;

                //NOTE:
                //if you use P which is Placidus there is a high chances you will get unequal houses from the SwissEph library itself...
                // you have to use V - 'V'Vehlow equal (Asc. in middle of house 1)
                // swissEph.swe_houses(jul_day_UT, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);
                // swissEph.swe_houses_ex(jul_day_UT, iflag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);
                //we only return cusps, cause that is what is used for now

                var eps = Calculate.EclipticObliquity(time);

                var tropAsc = horaryNumberTropAsc(horaryNumber);
                var armc = ConvertTropAscToArmc(tropAsc, eps, location.Latitude(), time);

                var lat = location.Latitude();

                Console.WriteLine("armc {0} eps {1} tropAsc {2} lat {3}", armc, eps, tropAsc, lat);

                swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

                //Create Dictionary for return
                var housesDictionary = new Dictionary<HouseName, Angle>();
                foreach (var house in House.AllHouses)
                {
                    //start of house longitude of 0-360
                    var hseLong = cusps[(int)house];
                    housesDictionary.Add(house, Angle.FromDegrees(hseLong));
                }

                //return cusps;
                return housesDictionary;
            }


            //special function localized to allow caching
            //note: there is another version that does caching
            double TimeToJulianDay(Time time)
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
        }

        public static double ConvertTropAscToArmc(double tropicalAscendant, double obliquityOfEcliptic, double geographicLatitude,Time time)
        {
            //Main method from Group.IO posted by K S Upendra 2019
            double rightAscension =
                Math.Atan(Math.Cos(obliquityOfEcliptic * Math.PI / 180) * Math.Tan(tropicalAscendant * Math.PI / 180)) *
                180 / Math.PI;
            double declination =
                Math.Asin(Math.Sin(obliquityOfEcliptic * Math.PI / 180) * Math.Sin(tropicalAscendant * Math.PI / 180)) *
                180 / Math.PI;
            double obliqueAscension = rightAscension -
                                      (Math.Asin(Math.Tan(declination * Math.PI / 180) *
                                                 Math.Tan(geographicLatitude * Math.PI / 180)) * 180 / Math.PI);
            double armc = 0;

            if (tropicalAscendant > 0 && tropicalAscendant < 90)
            {
                armc = 270 + obliqueAscension;
            }
            else if (tropicalAscendant > 90 && tropicalAscendant < 180)
            {
                armc = 90 + obliqueAscension;
            }
            else if (tropicalAscendant > 180 && tropicalAscendant < 270)
            {
                armc = 90 + obliqueAscension;
            }
            else if (tropicalAscendant > 270 && tropicalAscendant < 360)
            {
                armc = 270 + obliqueAscension;
            }

            return armc;
        }

        /// <summary>
        /// Gets all KP (Placidus) House Cusps for a given Time
        /// using Swiss Epehemris swe_houses_ex
        /// </summary>
        public static double[] GetKPBirthTimeHouseLongitudes(int Ayanamsa, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetKPBirthTimeHouseLongitudes), time, Ayanamsa),
                _getHouseKPLongitudes);


            //UNDERLYING FUNCTION
            double[] _getHouseKPLongitudes()
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

                //CPJ - set this flag and use the swe_houses_ex method - it returns SIDEREAL KP Longitudes diretly.
                //No need for additional step of deducting ayanmsa from Sayana Longitudes
                //var iflag = SwissEph.SEFLG_SIDEREAL || SwissEph.SEFLG_SWIEPH;
                const int iflag = SwissEph.SEFLG_SIDEREAL; //| SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;

                //NOTE:
                //if you use P which is Placidus there is a high chances you will get unequal houses from the SwissEph library itself...
                // you have to use V - 'V'Vehlow equal (Asc. in middle of house 1)
                swissEph.swe_houses_ex(jul_day_UT, iflag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);
                //we only return cusps, cause that is what is used for now

                return cusps;
            }

            //special function localized to allow caching
            //note: there is another version that does caching
            double TimeToJulianDay(Time time)
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
        }

        /// <summary>
        /// List of all planets and the zodiac signs they are located in at a given time
        /// using KP Krishnamurti system, note KP ayanamsa is hard set
        /// </summary>
        public static Dictionary<PlanetName, ZodiacSign> AllPlanetZodiacSignsKP(Time time)
        {

            //hard set KP ayanamsa to match commercial software default output
            Calculate.Ayanamsa = (int)Ayanamsa.KRISHNAMURTI;

            var returnList = new Dictionary<PlanetName, ZodiacSign>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = PlanetSignName(planet, time);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// Gets all houses with their constelation for KP Krishnamurti system
        /// </summary>
        public static Dictionary<HouseName, PlanetConstellation> AllHouseConstellationKP(Time time)
        {
            //get all house positions
            var housePositions = AllHouseLongitudesKP(time);

            //fill the planet constellations
            var returnList = new Dictionary<HouseName, PlanetConstellation>();
            foreach (var house in housePositions)
            {
                var constellation = ConstellationAtLongitude(house.GetBeginLongitude());
                returnList.Add(house.GetHouseName(), constellation);
            }

            return returnList;
        }

        //Horary Number Generation
        //Convert Given Horary Number to TropAsc Longitude
        public static double horaryNumberTropAsc(int horaryNumber)
        {
            var horaryNumbertropAscList = new List<Tuple<double, ConstellationName, PlanetName, PlanetName, double>>();
            var padaSize = 30.00 / 9;
            var count = 1;
            var padaNumber = 1;
            var tropAscDeg = 0.00;
            var x = PlanetName.Ketu;
            var all9Planets = PlanetName.All9Planets;
            var allZodiacs = ZodiacSign.All12ZodiacNames;
            var allConstellations = PlanetConstellation.AllConstellation;

            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var tropAsc = 0.000;

            foreach (var constellation in allConstellations)
            {
                var tempConstel = constellation;
                var lordofConstel = Calculate.LordOfConstellation(tempConstel);
                var planetnameArray = all9Planets.ToArray();
                int cntB = 0;
                int index = 0;

                while (cntB <= 8)
                {
                    if (planetnameArray[cntB] == lordofConstel)
                    {
                        index = cntB;
                        break;
                    }
                    cntB++;
                }

                PlanetName[] planetnameArrayB = new PlanetName[9];
                int cntC = 0;
                while (cntC <= 8)
                {
                    planetnameArrayB[cntC] = planetnameArray[index];
                    cntC++;
                    index++;
                    if (index == planetnameArray.Length)
                    {
                        index = 0;
                    }
                }

                foreach (var planetName in planetnameArrayB)
                {

                    var constellationA = tempConstel;
                    lordofConstel = Calculate.LordOfConstellation(constellationA);

                    switch (planetName.Name)
                    {
                        case PlanetName.PlanetNameEnum.Ketu:
                            tropAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Venus:
                            tropAscDeg = new Angle(2, 13, 20).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Sun:
                            tropAscDeg = new Angle(0, 40, 0).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Moon:
                            tropAscDeg = new Angle(1, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mars:
                            tropAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Rahu:
                            tropAscDeg = new Angle(2, 00, 00).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Jupiter:
                            tropAscDeg = new Angle(1, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Saturn:
                            tropAscDeg = new Angle(2, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mercury:
                            tropAscDeg = new Angle(1, 53, 20).TotalDegrees;
                            break;
                    }

                    var zSignAtLongBefore = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationBefore = Calculate.ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationLordBefore =
                        Calculate.LordOfConstellation(constellationBefore.GetConstellationName());

                    if (tropAsc == 0.00)
                    {
                        var longBefore = tropAsc - 0.00001 + 360;
                        zSignAtLongBefore = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = Calculate.ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore =
                            Calculate.LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                        constellationBefore = Calculate.ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                        constellationLordBefore =
                            Calculate.LordOfConstellation(constellationBefore.GetConstellationName());
                    }

                    tropAsc = tropAsc + tropAscDeg;

                    var zSignAtLongAfter = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                    var zSignAfterLord = Calculate.LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = Calculate.ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationLordAfter =
                        Calculate.LordOfConstellation(constellationAfter.GetConstellationName());

                    //does tropAsc end on 30, 60, 90, 120....
                    var tropAscSpansSigns = Math.Round(tropAsc % 30, 6);

                    // REFACTOR - Calculate.IsFireSign(zSignAtLongAfter.GetSignName());

                    //overlapping signs issue
                    if ((zSignAtLongAfter.GetSignName() != zSignAtLongBefore.GetSignName()) &&
                        (constellationLordAfter == constellationLordBefore)
                        && ((zSignAtLongBefore.GetSignName() == ZodiacName.Aries) ||
                            (zSignAtLongBefore.GetSignName() == ZodiacName.Leo) ||
                            (zSignAtLongBefore.GetSignName() == ZodiacName.Sagittarius) ||
                            (zSignAtLongBefore.GetSignName() == ZodiacName.Gemini) ||
                            (zSignAtLongBefore.GetSignName() == ZodiacName.Libra) ||
                            (zSignAtLongBefore.GetSignName() == ZodiacName.Aquarius)))
                    {
                        var remainderFromDivBy30 = (tropAsc % 30.00); //past signchange degree amount
                        var preSignChangeDegrees = tropAscDeg - remainderFromDivBy30;
                        tropAsc = tropAsc - tropAscDeg +
                                  preSignChangeDegrees; //this is one Entry into the List. this get us to Sign engpoint
                                                        //log entry into List
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord,
                                constellationA, constellationLordAfter, planetName, tropAsc));

                       /* Console.WriteLine(
                            "Horary# {0},Zodiac {1} ZSign Lord {2} Constel/Star {3}, StarLord {4}, SubLord {5} Asc {6} AscDMS: {7:000}D {8:00}M {9:00}S",
                            cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellation,
                            constellationLordAfter, planetName, tropAsc, Angle.FromDegrees(tropAsc).Degrees,
                            Angle.FromDegrees(tropAsc).Minutes, Angle.FromDegrees(tropAsc).Seconds);
                        */

                        cntA++;
                        //next process the balance into the nextSign
                        var tropAscB = tropAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord,
                                constellationA, constellationLordAfter, planetName, tropAscB));

                        /*Console.WriteLine(
                            "Horary# {0},Zodiac {1} ZSign Lord {2} Constel/Star {3}, StarLord {4}, SubLord {5} Asc {6} AscDMS: {7:000}D {8:00}M {9:00}S",
                            cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellation,
                            constellationLordAfter, planetName, tropAscB, Angle.FromDegrees(tropAscB).Degrees,
                            Angle.FromDegrees(tropAscB).Minutes, Angle.FromDegrees(tropAscB).Seconds);
                        */
                        cntA++;
                        tropAsc = tropAscB;
                    }
                    else
                    {
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord,
                                constellationA, constellationLordAfter, planetName, tropAsc));

                        /*Console.WriteLine(
                            "Horary# {0},Zodiac {1} ZSign Lord {2} Constel/Star {3}, StarLord {4}, SubLord {5} Asc {6} AscDMS: {7:000}D {8:00}M {9:00}S",
                            cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellation,
                            constellationLordAfter, planetName, tropAsc, Angle.FromDegrees(tropAsc).Degrees,
                            Angle.FromDegrees(tropAsc).Minutes, Angle.FromDegrees(tropAsc).Seconds);
                        */
                        cntA++;
                    }

                }
            }
            var countX = 0;
            while (countX <= 248)
            {
                if (constellationList[countX].Item1 == horaryNumber)
                {
                    tropAsc = constellationList[countX - 1].Item7;  //the -1 because in the list we record end longitudes.
                    Console.WriteLine("Horary Asc {0}", tropAsc);                                                //We have to return start longitudes.
                                                                    //the end longitude of the previous one is the start of the current one. 
                }

                countX++;
            }

            return tropAsc;
        }

        /// <summary>
        /// Gets longitudes for houses under Krishnamurti (KP) astrology system
        /// Note: Ayanamsa hard set to Krishnamurti
        /// </summary>
        public static List<House> AllHouseLongitudesKP(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHouseLongitudesKP), time, Calculate.Ayanamsa), _allHouseLongitudesKP);


            //UNDERLYING FUNCTION

            List<House> _allHouseLongitudesKP()
            {
                //get house positions modified for KP system in raw 
                var swissEphCusps = _houseLongitudesKP();

                //4.0 Initialize houses into list
                var houseList = new List<House>();

                foreach (var house in House.AllHouses)
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
                swissEph.swe_set_sid_mode((int)Ayanamsa.KRISHNAMURTI, 0, 0);

                var iflag = SwissEph.SEFLG_SIDEREAL;

                //NOTE:
                //if you use P which is Placidus there for Krishamurti system
                swissEph.swe_houses_ex(jul_day_UT, iflag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);

                //we only return cusps, cause that is what is used for now
                return cusps;
            }




        }

    }
}
