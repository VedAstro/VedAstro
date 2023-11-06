using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SwissEphNet;
using static VedAstro.Library.Calculate;

namespace VedAstro.Library
{
    public static class CalculateKP
    {

        #region WEBSITE METHODS

        #region HORARY

        public static ZodiacSign HouseZodiacSignHorary(HouseName inputHouse, Time birthTime, int horaryNumber)
        {
            //get house start longitudes for KP system
            var allHouseCuspsRaw = AllHouseCuspLongitudesHorary(birthTime, horaryNumber);

            //get zodiac sign at house start longitude longitude
            var zodiacSignAtLong = ZodiacSignAtLongitude(allHouseCuspsRaw[inputHouse]);

            return zodiacSignAtLong;

        }

        public static HouseName PlanetHouseHorary(PlanetName inputPlanet, Time birthTime, int horaryNumber)
        {
            // Get the starting longitudes of all houses.
            var cusps = AllHouseCuspLongitudesHorary(birthTime, horaryNumber);

            // Calculate the Nirayana longitude of the current planet.
            var planetLongitude = PlanetNirayanaLongitude(inputPlanet, birthTime);

            // Find the first house that contains the current planet.
            var houseForPlanet = House.AllHouses.FirstOrDefault(house => IsPlanetInHouseKP(cusps, planetLongitude, house));


            return houseForPlanet;
        }

        #endregion

        #region KUNDALI

        public static ZodiacSign HouseZodiacSignKundali(HouseName inputHouse, Time birthTime)
        {
            //get house start longitudes for KP system
            var allHouseCuspsRaw = AllHouseCuspLongitudesKundali(birthTime);

            //get zodiac sign at house start longitude longitude
            var zodiacSignAtLong = ZodiacSignAtLongitude(allHouseCuspsRaw[inputHouse]);

            return zodiacSignAtLong;

        }

        public static HouseName PlanetHouseKundali(PlanetName inputPlanet, Time birthTime)
        {
            //get house start longitudes
            var cusps = AllHouseCuspLongitudesKundali(birthTime);


            foreach (var house in House.AllHouses)
            {
                //if planet is in house than add to list and exit, don't check others
                var planetLongitude = PlanetNirayanaLongitude(inputPlanet, birthTime);
                var isAddToList = IsPlanetInHouseKP(cusps, planetLongitude, house);

                if (isAddToList)//exit once house found
                {
                    return house;
                }
            }

            //when no house found
            return HouseName.Empty;
        }

        #endregion

        #endregion

        #region GENERAL

        public static PlanetName PlanetLordOfConstellation(PlanetName inputPlanet, Time birthTime)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, birthTime);

            // The value is the lord of the constellation at the planet's longitude
            var value = LordOfConstellation(ConstellationAtLongitude(nirayanaDegrees).GetConstellationName());

            // Add the key-value pair to the dictionary
            return value;
        }


        public static PlanetName PlanetLordOfZodiacSign(PlanetName inputPlanet, Time birthTime)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, birthTime);

            var zodiacSign = ZodiacSignAtLongitude(nirayanaDegrees);

            return LordOfZodiacSign(zodiacSign.GetSignName());

        }


        #endregion

        #region KP SPECIFIC CALCS

        public static bool IsPlanetInHouseKP(Dictionary<HouseName, Angle> cusps, Angle planetNirayanaDegrees, HouseName house)
        {
            //get house number
            var houseNumber = (int)house;

            if (houseNumber + 1 < cusps.Count) //Do not exceed the bounds of the array
            {
                //check if cusp longitude is smaller than next cusp longitude
                if (cusps[(HouseName)houseNumber + 1] > cusps[(HouseName)houseNumber])
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) &&
                           //this means that the planet falls in between these house cusps
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }

                //if next cusp start long is smaller than current cusp we are rotating through 360 deg
                else
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) ||
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates the astrological house cusps for a given time and horary number.
        /// It uses the Swiss Ephemeris to compute high precision astronomical data.
        /// The method converts the horary number to Tropical Ascendant degrees,
        /// then to ARMC (Sidereal Time), which is used to calculate house cusps.
        /// The house system is calculated using the ARMC, latitude, and obliquity of the ecliptic.
        /// The results are packaged into a dictionary mapping each house to its corresponding angle
        /// </summary>
        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudesHorary(Time time, int horaryNumber)
        {
            //get location at place of time
            var location = time.GetGeoLocation();

            SwissEph swissEph = new SwissEph();

            double[] cusps = new double[13];

            //we have to supply ascmc to make the function run
            double[] ascmc = new double[10];

            //set ayanamsa
            swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);

            // The obliquity of the ecliptic is the angle between the ecliptic and the celestial equator. 
            // It changes over time and is calculated for a specific time.
            var eps = EclipticObliquity(time);

            // The horary number is converted to Tropical Ascendant degrees.
            // The Tropical Ascendant is the degree of the zodiac that is rising
            // on the eastern horizon at the time for which the horoscope is cast.
            var tropAsc = HoraryNumberTropicalAsc(horaryNumber);

            // The Ascendant degree is then converted to the ARMC (Sidereal Time).
            // The ARMC is used in the calculation of house cusps.
            var armc = ConvertTropicalAscToARMC(tropAsc, eps, location.Latitude(), time);

            // The house system is calculated using the ARMC, latitude, and obliquity of the ecliptic.
            // The 'P' parameter specifies the Placidus house system.
            swissEph.swe_houses_armc(armc, location.Latitude(), eps, 'P', cusps, ascmc);

            //package data before sending
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

        /// <summary>
        /// Calculates the astrological house cusps for a specific time and location,
        /// which is a key aspect in the creation of a Kundali (astrological chart).
        /// 
        /// The function swe_houses_ex from the Swiss Ephemeris library is used to calculate the
        /// house cusps and the ascmc values (which include the Ascendant, Midheaven, etc.).
        /// The house system used is 'P' (Placidus).
        /// 
        /// The longitudes of the house cusps are then stored in a dictionary, with the house
        /// names as keys and the longitudes as values (converted to degrees). This dictionary is
        /// returned by the function, providing the longitudes of all houses in the Kundali.
        /// </summary>
        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudesKundali(Time time)
        {
            //get location at place of time
            var location = time.GetGeoLocation();

            //Convert DOB to Julian Day
            var jul_day_UT = TimeToJulianDay(time);


            SwissEph swissEph = new SwissEph();
            double[] cusps = new double[13];
            double[] ascmc = new double[10];

            //set ayanamsa
            swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);

            //define the flag for sidereal calculations
            const int iFlag = SwissEph.SEFLG_SIDEREAL;

            //calculate the house cusps and ascmc values
            swissEph.swe_houses_ex(jul_day_UT, iFlag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);

            //package data before sending
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

        /// <summary>
        /// This method is used to convert the tropical ascendant to the ARMC (Ascendant Right Meridian Circle).
        /// It first calculates the right ascension and declination using the provided tropical ascendant and
        /// obliquity of the ecliptic. Then, it calculates the oblique ascension by subtracting a value derived
        /// from the declination and geographic latitude from the right ascension. Finally, it calculates the ARMC
        /// based on the value of the tropical ascendant and the oblique ascension.
        /// </summary>
        public static double ConvertTropicalAscToARMC(double tropicalAscendant, double obliquityOfEcliptic, double geographicLatitude, Time time)
        {
            // The main method is taken from a post by K S Upendra on Group.IO in 2019
            // Calculate the right ascension using the formula:
            // atan(cos(obliquityOfEcliptic) * tan(tropicalAscendant))
            double rightAscension =
                Math.Atan(Math.Cos(obliquityOfEcliptic * Math.PI / 180) * Math.Tan(tropicalAscendant * Math.PI / 180)) *
                180 / Math.PI;
            // Calculate the declination using the formula:
            // asin(sin(obliquityOfEcliptic) * sin(tropicalAscendant))
            double declination =
                Math.Asin(Math.Sin(obliquityOfEcliptic * Math.PI / 180) * Math.Sin(tropicalAscendant * Math.PI / 180)) *
                180 / Math.PI;
            // Calculate the oblique ascension by subtracting the result of the following formula from the right ascension:
            // asin(tan(declination) * tan(geographicLatitude))
            double obliqueAscension = rightAscension -
                                      (Math.Asin(Math.Tan(declination * Math.PI / 180) *
                                                 Math.Tan(geographicLatitude * Math.PI / 180)) * 180 / Math.PI);
            // Initialize the armc variable
            double armc = 0;
            // Depending on the value of the tropical ascendant, calculate the armc using the formula:
            // armc = 270 + obliqueAscension or armc = 90 + obliqueAscension
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
            // Return the calculated armc value
            return armc;
        }

        /// <summary>
        /// This method calculates the tropical ascendant for a given horary number.
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the tropical
        /// ascendant (tropAsc), and handling special cases such as when tropAsc is 0 or
        /// when there are overlapping signs. The tropical ascendant corresponding to the
        /// given horary number is then returned.
        /// </summary>
        public static double HoraryNumberTropicalAsc(int horaryNumber)
        {
            // Initialize variables
            var tropAscDeg = 0.00;
            var all9Planets = PlanetName.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var tropAsc = 0.000;

            // Iterate over all constellations
            foreach (var constellation in allConstellations)
            {
                var tempConstel = constellation;
                var lordofConstel = LordOfConstellation(tempConstel);
                var planetnameArray = all9Planets.ToArray();
                int cntB = 0;
                int index = 0;

                // Find the index of the lord of the constellation in the planet array
                while (cntB <= 8)
                {
                    if (planetnameArray[cntB] == lordofConstel)
                    {
                        index = cntB;
                        break;
                    }
                    cntB++;
                }
                var planetnameArrayB = new PlanetName[9];
                int cntC = 0;

                // Reorder the planet array based on the index of the lord of the constellation
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
                // Iterate over the reordered planet array
                foreach (var planetName in planetnameArrayB)
                {
                    var constellationA = tempConstel;
                    lordofConstel = LordOfConstellation(constellationA);
                    // Assign tropAscDeg based on the planet name
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
                    // Calculate various parameters before and after adding tropAscDeg to tropAsc
                    var zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    // Special handling for tropAsc == 0.00
                    if (tropAsc == 0.00)
                    {
                        var longBefore = tropAsc - 0.00001 + 360;
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    tropAsc = tropAsc + tropAscDeg;
                    var zSignAtLongAfter = ZodiacSignAtLongitude(Angle.FromDegrees(tropAsc));
                    var zSignAfterLord = LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = ConstellationAtLongitude(Angle.FromDegrees(tropAsc));
                    var constellationLordAfter = LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if tropAsc ends on 30, 60, 90, 120....
                    var tropAscSpansSigns = Math.Round(tropAsc % 30, 6);
                    // Handle overlapping signs issue
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
                        tropAsc = tropAsc - tropAscDeg + preSignChangeDegrees; //this is one Entry into the List. this get us to Sign engpoint
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, tropAsc));
                        cntA++;
                        //next process the balance into the nextSign
                        var tropAscB = tropAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, tropAscB));
                        cntA++;
                        tropAsc = tropAscB;
                    }
                    else
                    {
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, tropAsc));
                        cntA++;
                    }
                }
            }

            // Find the horary number in the constellation list and return the corresponding tropAsc
            var countX = 0;
            while (countX <= 248)
            {
                if (constellationList[countX].Item1 == horaryNumber)
                {
                    //NOTE:
                    //the -1 because in the list we record end longitudes.
                    //We have to return start longitudes.
                    //the end longitude of the previous one is the start of the current one. 
                    tropAsc = constellationList[countX - 1].Item7;
                }
                countX++;
            }


            return tropAsc;
        }

        /// <summary>
        /// THIS IS NOT A COPY - THIS ONE ROTATES THE CHART TO PROVIDED NEW LAGNA - LAST Param in Signature
        /// Hence the call twice to calculate cusps, first one to get base cusps based on HorNUm and then Rotate to provided Lagna tropAsc
        /// Moves Lagna to a Long Degree (the new Ascendant and gets new KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// </summary>
        public static Dictionary<HouseName, Angle> MoveLagnaToSpecificLongGetNewHouseCusps(int Ayanamsa, Time time,
            int horNum, double tropAsc)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(
                new CacheKey(nameof(MoveLagnaToSpecificLongGetNewHouseCusps), Ayanamsa, time, horNum, tropAsc),
                _getNewCuspLongitudes);


            //UNDERLYING FUNCTION
            Dictionary<HouseName, Angle> _getNewCuspLongitudes()
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

                var tropAscFromHorNum = HoraryNumberTropicalAsc(horNum);
                var armc = ConvertTropicalAscToARMC(tropAscFromHorNum, eps, location.Latitude(), time);

                var lat = location.Latitude();

                Console.WriteLine("armc {0} eps {1} tropAsc {2} lat {3}", armc, eps, tropAscFromHorNum, lat);

                swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

                //base cusps created - now repeat now with provided Long Lagna needs to be moved to
                armc = ConvertTropicalAscToARMC(tropAsc, eps, location.Latitude(), time);
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
        }

        #endregion

            #region TEMPMETHODS

            /// <summary>
            /// Gets all KP (Placidus) House Cusps 
            /// using Swiss Epehemris swe_houses_ex
            /// this is simply a test method to test the KPHoraryLongitudes and KPBirthtimLongitudes methods
            /// </summary>
            // Kp Cusp Calculations
            public static void KpPrintHouseCuspLong(int Ayanamsa, Time birthtime, int horNum)
        {
            Console.WriteLine("Birth Time is {0} ", birthtime);
            Console.WriteLine("Ayanamsa is {0} ", Calculate.Ayanamsa);
            var ayanamsaDegree = Calculate.AyanamsaDegree(birthtime);
            Console.WriteLine("Ayanamsa Degree is {0}, {1} Deg {2} Min {3} Secs ", ayanamsaDegree.Rounded,
                ayanamsaDegree.Degrees,
                ayanamsaDegree.Minutes, ayanamsaDegree.Seconds);

            Console.WriteLine("================== KP Houses and Planets - From SWISS EPH Modified Method ===================");
            //Choose one of the following statements. Comment the other one out for testing
            var cusps = AllHouseCuspLongitudesKundali(birthtime);
            //var cusps = AllHouseCuspLongitudesHorary(birthtime, horNum);

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
            var planetDataDict = CalculateKP.PlanetData(Calculate.Ayanamsa, birthtime, horNum);

            //Temporarycomment below - Refactored code below into its own method PlanetData
            /*
            var allPlanets = VedAstro.Library.PlanetName.All9Planets;
            var x = 0;
            var allPlanetConstellation = Calculate.AllPlanetConstellation(birthtime);

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
            } */
        }

        /// <summary>
        /// Process planet positions and returns Dictionary for webpage table
        /// </summary>
        public static Dictionary<PlanetName, (Angle, ZodiacName, ConstellationName, PlanetName, PlanetName)> PlanetData(int Ayanamsa, Time birthtime, int horNum)
        {
            Dictionary<PlanetName, (Angle, ZodiacName, ConstellationName, PlanetName, PlanetName)> planetTableData = new Dictionary<PlanetName, (Angle, ZodiacName, ConstellationName, PlanetName, PlanetName)>();

            var allPlanets = VedAstro.Library.PlanetName.All9Planets;
            var x = 0;
            var allPlanetConstellation = Calculate.AllPlanetConstellation(birthtime);
            /*
            foreach (var planetConstellation in allPlanetConstellation)
            {
                Console.WriteLine(" {0} {1}", planetConstellation.Key, planetConstellation.Value.GetConstellationName().ToString()); //allPlanetConstellation[0].GetQuarter().ToString());
                var yy = Calculate.LordOfConstellation(planetConstellation.Value.GetConstellationName());
            } */

            var cusps = AllHouseCuspLongitudesHorary(birthtime, horNum);

            Console.WriteLine("Processing Planet Data now....");
            //Process Planet Data
            foreach (PlanetName planet in allPlanets)
            {
                Angle planetNirayanaDegrees = Calculate.PlanetNirayanaLongitude(planet, birthtime);
                Console.Write("{0} {1} {2} Deg {3} Min {4} Secs ; ", planet.Name, planetNirayanaDegrees.TotalDegrees,
                            planetNirayanaDegrees.Degrees, planetNirayanaDegrees.Minutes,
                            planetNirayanaDegrees.Seconds);
                var planetConstellation = Calculate.PlanetConstellation(planet, birthtime);

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
                                var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6}; StarL {7}", planet.Name, x, zodiacSignAtLong.GetSignName(),
                                                            zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                                            zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);
                                planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees), zodiacSignAtLong.GetSignName(),
                                    constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation));

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
                                var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S; SignL {6} StarL {7} ", planet.Name, x, zodiacSignAtLong.GetSignName(),
                                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                    zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);
                                planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees), zodiacSignAtLong.GetSignName(),
                                    constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation));
                                break;
                            }
                        }
                    }
                    else
                    {
                        var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                        var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                        var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                        var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                        Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6} StarL {0}", planet.Name, x, zodiacSignAtLong.GetSignName(),
                            zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                            zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation);

                        planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees), zodiacSignAtLong.GetSignName(),
                            constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation));
                        break;
                    }
                    x++;
                }
            }
            return planetTableData;
        }


        #endregion



    }
}
