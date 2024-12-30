using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ExCSS;
using SwissEphNet;
using static VedAstro.Library.Calculate;

namespace VedAstro.Library
{
    public static class CalculateKP
    {

        #region WEBSITE METHODS

        public static Dictionary<HouseName, ZodiacSign> AllHouseZodiacSign(Time time, int horaryNumber = 0)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, ZodiacSign>();

            //get for all houses
            foreach (var house in Library.House.AllHouses)
            {
                var calcHouseSign = CalculateKP.HouseZodiacSign(house, time, horaryNumber);
                allHouses.Add(house, calcHouseSign);
            }

            return allHouses;
        }

        public static ZodiacSign HouseZodiacSign(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            //get zodiac sign at house start longitude longitude
            var zodiacSignAtLong = ZodiacSignAtLongitude(allHouseCuspsRaw[inputHouse]);

            return zodiacSignAtLong;

        }

        /// <summary>
        /// Based on horary number and rotate degrees, this method will auto select the correct house cusps
        /// </summary>
        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudes(Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            //if is horary use horary method
            var isHorary = horaryNumber != 0;

            //if rotateDegrees is not zero, use a different method
            var isRotated = Math.Abs(rotateDegrees) > 0.0001;

            //get house start longitudes for KP system
            Dictionary<HouseName, Angle> allHouseCuspsRaw;
            if (isHorary)
            {
                allHouseCuspsRaw = isRotated
                    ? AllHouseCuspLongitudesRotateHorary(time, horaryNumber, rotateDegrees)
                    : AllHouseCuspLongitudesHorary(time, horaryNumber);
            }
            else
            {
                allHouseCuspsRaw = isRotated
                    ? AllHouseCuspLongitudesRotateKundali(time, rotateDegrees)
                    : AllHouseCuspLongitudesKundali(time);
            }

            return allHouseCuspsRaw;
        }

        public static Constellation HouseConstellation(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {

            //get house start longitudes for KP system (horary or kundali)
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            //get Constellation at house start longitude
            var constellationAtLong = ConstellationAtLongitude(allHouseCuspsRaw[inputHouse]);

            return constellationAtLong;

        }

        public static PlanetName HouseSubLord(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            //get start long for inputed house
            var houseStartLong = allHouseCuspsRaw[inputHouse];

            //special calc to get sub lord
            var subLord = SubLordAtLongitude(houseStartLong);

            return subLord;
        }

        public static HouseName PlanetHouse(PlanetName inputPlanet, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            // Calculate the Nirayana longitude of the current planet.
            var planetLongitude = PlanetNirayanaLongitude(inputPlanet, time);

            // Find the first house that contains the current planet.
            var houseForPlanet = House.AllHouses.FirstOrDefault(house => IsPlanetInHouseKP(allHouseCuspsRaw, planetLongitude, house));

            return houseForPlanet;
        }

        public static PlanetName HouseLordOfZodiacSign(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            //get start long for inputed house
            var houseStartLong = allHouseCuspsRaw[inputHouse];

            //from long get zodiac sign
            var zodiacSign = ZodiacSignAtLongitude(houseStartLong);

            //return lord of zodiac sign
            return LordOfZodiacSign(zodiacSign.GetSignName());
        }

        public static List<HouseName> HousesOwnedByPlanet(PlanetName inputPlanet, Time time, int horaryNumber = 0)
        {
            //Given a planet, return Zodiac Signs owned by it Ex. Ju, returns Sag an Pis
            var signsOwned = Calculate.ZodiacSignsOwnedByPlanet(inputPlanet);

            //given a Zodiac Sign, return, House Number (or Cusp number as its actually called)
            var houseList = new List<HouseName>();


            //get signs of all houses
            var houseSigns = AllHouseZodiacSign(time, horaryNumber);


            foreach (var zodiacName in signsOwned)
            {

                //get all houses that have inputed zodiac name
                List<HouseName> matchingHouses = houseSigns
                    .Where(pair => pair.Value.GetSignName() == zodiacName)
                    .Select(pair => pair.Key)
                    .ToList();

                //add to return list
                houseList.AddRange(matchingHouses);
            }


            //Next present these houses as Owned by a planet.
            return houseList;
        }

        public static PlanetName HouseLordOfConstellation(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            //get start long for inputed house
            var houseStartLong = allHouseCuspsRaw[inputHouse];

            // The value is the lord of the constellation at the house's longitude
            var value = LordOfConstellation(ConstellationAtLongitude(houseStartLong).GetConstellationName());

            return value;
        }

        public static List<PlanetName> PlanetsInHouse(HouseName inputHouse, Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            var allHouseCuspsRaw = AllHouseCuspLongitudes(time, horaryNumber, rotateDegrees);

            // Find the first house that contains the current planet.
            var houseForPlanet = PlanetName.All9Planets.Where(planet =>
            {
                // Calculate the Nirayana longitude of the current planet.
                var planetLongitude = PlanetNirayanaLongitude(planet, time);

                return IsPlanetInHouseKP(allHouseCuspsRaw, planetLongitude, inputHouse);
            }).ToList();

            return houseForPlanet;
        }

        public static PlanetName PlanetLordOfConstellation(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            // The value is the lord of the constellation at the planet's longitude
            var value = LordOfConstellation(ConstellationAtLongitude(nirayanaDegrees).GetConstellationName());

            // Add the key-value pair to the dictionary
            return value;
        }

        public static PlanetName PlanetLordOfZodiacSign(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            var zodiacSign = ZodiacSignAtLongitude(nirayanaDegrees);

            return LordOfZodiacSign(zodiacSign.GetSignName());

        }

        public static PlanetName PlanetSubLord(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            var subLord = SubLordAtLongitude(nirayanaDegrees);

            return subLord;
        }

        #endregion

        #region KP SPECIFIC CALCS

        public static bool IsPlanetInHouseKP(Dictionary<HouseName, Angle> cusps, Angle planetNirayanaDegrees, HouseName house)
        {
            //get house number
            var houseNumber = (int)house;
            // Check if houseNumber is within the bounds of the array
            if (houseNumber >= 0 && houseNumber < cusps.Count)
            {
                //check if cusp longitude is smaller than next cusp longitude
                if (houseNumber + 1 < cusps.Count && cusps[(HouseName)houseNumber + 1] > cusps[(HouseName)houseNumber])
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) &&
                           //this means that the planet falls in between these house cusps
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }
                //if next cusp start long is smaller than current cusp we are rotating through 360 deg
                else if (houseNumber + 1 < cusps.Count)
                {
                    return (planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees) ||
                           (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)houseNumber + 1].TotalDegrees);
                }
                // If houseNumber is the last index in the cusps array
                else
                {
                    return planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)houseNumber].TotalDegrees;
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
            Console.WriteLine("Inside AllHouseCuspLongHorary with GeoLoc {0}", location.Latitude());

            SwissEph swissEphHorary = new SwissEph();
            var swissEphVersion = swissEphHorary.swe_version();
            Console.WriteLine("{0}", swissEphVersion);
            var swissLibPath = swissEphHorary.swe_get_library_path();
            Console.WriteLine("{0}",swissLibPath);
            swissEphHorary.swe_set_ephe_path(null);

            //var jul_day_ut = Calculate.TimeToJulianDay(time); REMOVE LATER
            //Console.WriteLine("Julian Day for 18 June 1970 10:30a {0}", jul_day_ut);

            double[] cusps = new double[13];

            //we have to supply ascmc to make the function run
            double[] ascmc = new double[10];

            //set ayanamsa
            //swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);
            // Console.WriteLine("Ayanamsa {0}; Ayanamsa Degrees {1}", Calculate.Ayanamsa, Calculate.AyanamsaDegree(time));

            //define the flag for sidereal calculations
            const int iFlag = SwissEph.SEFLG_SIDEREAL; //May not be useful for Horary ARMC based calculations. Remove later

            // The obliquity of the ecliptic is the angle between the ecliptic and the celestial equator. 
            // It changes over time and is calculated for a specific time.
            var eps = EclipticObliquity(time);
            Console.WriteLine("Ecliptic Obliquity eps {0}", eps);

            // The horary number is converted to Tropical Ascendant degrees.
            // The Tropical Ascendant is the degree of the zodiac that is rising
            // on the eastern horizon at the time for which the horoscope is cast.
            var siderealAsc = HoraryNumberSiderealAsc(horaryNumber);
            Console.WriteLine("siderealAsc Degrees {0}", siderealAsc);
            var tropAsc = siderealAsc + Calculate.AyanamsaDegree(time).TotalDegrees;
            Console.WriteLine("tropAsc Degrees {0}", tropAsc);

            // The Ascendant degree is then converted to the ARMC (Sidereal Time).
            // The ARMC is used in the calculation of house cusps.
            var armc = ConvertAscToARMC(tropAsc, eps, location.Latitude(), time);
           // armc = 341.65;// hard set for testing
            
            Console.WriteLine("armc {0}", armc);
            Console.WriteLine("Latitude {0}", location.Latitude());

            //set ayanamsa
            swissEphHorary.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);
            
            double daya;
            string serr="";
            var jul_day_ut = Calculate.TimeToJulianDay(time);
            swissEphHorary.swe_get_ayanamsa_ex(jul_day_ut, iFlag, out daya, ref serr);

            Console.Write("Ayanamsa {0}; Ayanamsa Degrees {1}, ", Calculate.Ayanamsa, Calculate.AyanamsaDegree(time));
            var ayanamsaDegree = Calculate.AyanamsaDegree(time);
            Console.WriteLine("Ayanamsa Degree is {0}, {1} Deg {2} Min {3} Secs ", ayanamsaDegree.Rounded,
                ayanamsaDegree.Degrees, ayanamsaDegree.Minutes, ayanamsaDegree.Seconds);

            // The house system is calculated using the ARMC, latitude, and obliquity of the ecliptic.
            // The 'P' parameter specifies the Placidus house system.
            swissEphHorary.swe_houses_armc(armc, location.Latitude(), eps, 'P', cusps, ascmc);
            //IMPORTANT: At this point cusps is returning Tropical cusp positions. Matches LOKPA Horary Cusps with LOKPA settings set to Tropical Zodiac
            //Convert back to Sidereal below by deducting Ayanamsa.
            
            //package data before sending
            //revert back to idereal by minussing Ayanamsa
            var housesDictionary = new Dictionary<HouseName, Angle>();
            foreach (var house in House.AllHouses)
            {
                //start of house longitude of 0-360
                var hseLong = cusps[(int)house];
                var hseLongSidereal = swissEphHorary.swe_degnorm(cusps[(int)house] - Calculate.AyanamsaDegree(time).TotalDegrees);
                housesDictionary.Add(house, Angle.FromDegrees(hseLongSidereal));
            }
            //return Sidereal cusps;
            return housesDictionary;
        }

        public static double GetNutation(Time time)
        {
            SwissEph swissEph = new SwissEph();
            double[] x = new double[6];
            string serr = "";

            var jul_day_ut = Calculate.TimeToJulianDay(time);
            //Console.WriteLine("Julian Day for 18 June 1970 10:30a {0}", jul_day_ut);

            swissEph.swe_calc(jul_day_ut, SwissEph.SE_ECL_NUT, 0, x, ref serr);
            return x[2]; //See SWISS EPH docs and confirm array location - is it 1 or 2??
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
        /// THIS IS NOT A COPY - THIS ONE ROTATES THE CHART TO PROVIDED NEW LAGNA - LAST Param in Signature
        /// Hence the call twice to calculate cusps, first one to get base cusps based on HorNUm and then Rotate to provided Lagna tropAsc
        /// Moves Lagna to a Long Degree (the new Ascendant and gets new KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// </summary>
        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudesRotateKundali(Time time, double rotateDegrees)
        {
            //get location at place of time
            var location = time.GetGeoLocation();

            SwissEph swissEph = new SwissEph();

            double[] cusps = new double[13];

            //we have to supply ascmc to make the function run
            double[] ascmc = new double[10];

            //set ayanamsa
            swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);


            var eps = Calculate.EclipticObliquity(time);

            var armc = ConvertAscToARMC(rotateDegrees, eps, location.Latitude(), time);

            var lat = location.Latitude();

            swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

            //base cusps created - now repeat now with provided Long Lagna needs to be moved to
            armc = ConvertAscToARMC(rotateDegrees, eps, location.Latitude(), time);
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

        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudesRotateHorary(Time time, int horaryNumber, double rotateDegrees)
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
            swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);


            var eps = Calculate.EclipticObliquity(time);

            var tropAscFromHorNum = HoraryNumberSiderealAsc(horaryNumber);
            var armc = ConvertAscToARMC(tropAscFromHorNum, eps, location.Latitude(), time);

            var lat = location.Latitude();

            Console.WriteLine("armc {0} eps {1} tropAsc {2} lat {3}", armc, eps, tropAscFromHorNum, lat);

            swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

            //base cusps created - now repeat now with provided Long Lagna needs to be moved to
            armc = ConvertAscToARMC(rotateDegrees, eps, location.Latitude(), time);
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


        /// <summary>
        /// This method is used to convert the tropical ascendant to the ARMC (Ascendant Right Meridian Circle).
        /// It first calculates the right ascension and declination using the provided tropical ascendant and
        /// obliquity of the ecliptic. Then, it calculates the oblique ascension by subtracting a value derived
        /// from the declination and geographic latitude from the right ascension. Finally, it calculates the ARMC
        /// based on the value of the tropical ascendant and the oblique ascension.
        /// </summary>
        public static double ConvertAscToARMC(double Ascendant, double obliquityOfEcliptic, double geographicLatitude, Time time)
        {
            // The main method is taken from a post by K S Upendra on Group.IO in 2019
            // Calculate the right ascension using the formula:
            // atan(cos(obliquityOfEcliptic) * tan(tropicalAscendant))
            double rightAscension =
                Math.Atan(Math.Cos(obliquityOfEcliptic * Math.PI / 180) * Math.Tan(Ascendant * Math.PI / 180)) *
                180 / Math.PI;
            // Calculate the declination using the formula:
            // asin(sin(obliquityOfEcliptic) * sin(tropicalAscendant))
            double declination =
                Math.Asin(Math.Sin(obliquityOfEcliptic * Math.PI / 180) * Math.Sin(Ascendant * Math.PI / 180)) *
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
            if (Ascendant >= 0 && Ascendant < 90)
            {
                armc = 270 + obliqueAscension;
            }
            else if (Ascendant >= 90 && Ascendant < 180)
            {
                armc = 90 + obliqueAscension;
            }
            else if (Ascendant >= 180 && Ascendant < 270)
            {
                armc = 90 + obliqueAscension;
            }
            else if (Ascendant >= 270 && Ascendant < 360)
            {
                armc = 270 + obliqueAscension;
            }
            // Return the calculated armc value
            return armc;
        }

        /// <summary>
        /// This method calculates the sidereal ascendant for a given horary number.
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the sidereal
        /// ascendant (siderealAsc), and handling special cases such as when sidAsc is 0 or
        /// when there are overlapping signs. The sidereal ascendant corresponding to the
        /// given horary number is then returned.
        /// </summary>
        public static double HoraryNumberSiderealAsc(int horaryNumber)
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = PlanetName.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

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
                    // Assign sidAscDeg based on the planet name
                    switch (planetName.Name)
                    {
                        case PlanetName.PlanetNameEnum.Ketu:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Venus:
                            siderealAscDeg = new Angle(2, 13, 20).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Sun:
                            siderealAscDeg = new Angle(0, 40, 0).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Moon:
                            siderealAscDeg = new Angle(1, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mars:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Rahu:
                            siderealAscDeg = new Angle(2, 00, 00).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Jupiter:
                            siderealAscDeg = new Angle(1, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Saturn:
                            siderealAscDeg = new Angle(2, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mercury:
                            siderealAscDeg = new Angle(1, 53, 20).TotalDegrees;
                            break;
                    }
                    // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                    var zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());

                    // Special handling for siderealAsc == 0.00
                    if (siderealAsc == 0.00)
                    {
                        var longBefore = siderealAsc - 0.00001 + 360;
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    siderealAsc = siderealAsc + siderealAscDeg;
                    var zSignAtLongAfter = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var zSignAfterLord = LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordAfter = LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if siderealAsc ends on 30, 60, 90, 120....
                    var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);
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
                        var remainderFromDivBy30 = (siderealAsc % 30.00); //past signchange degree amount
                        var preSignChangeDegrees = siderealAscDeg - remainderFromDivBy30;
                        siderealAsc = siderealAsc - siderealAscDeg + preSignChangeDegrees; //this is one Entry into the List. this get us to the zodiac Sign endpoint
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                        //next process the balance into the nextSign
                        var siderealAscB = siderealAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAscB));
                        cntA++;
                        siderealAsc = siderealAscB;
                    }
                    else
                    {
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                    }
                }
            }

            // Find the horary number in the constellation list and return the corresponding siderealAsc
            var countX = 0;
            while (countX <= 248)
            {
                if (horaryNumber == 1)
                {
                    siderealAsc = 0.00;
                    break;
                }

                else if (constellationList[countX].Item1 == horaryNumber)
                {
                    //NOTE:
                    //the -1 because in the list we record end longitudes.
                    //We have to return start longitudes.
                    //the end longitude of the previous one is the start of the current one. 
                    siderealAsc = constellationList[countX - 1].Item7;
                    break;
                }
                countX++;
            }
            return siderealAsc;
        }

        /// <summary>
        ///this methodname should be amended. Its for any given Longitude - planet or house/cusp long.
        /// Not just PlanetLongitude.
        /// This method first calculates the SubLord table.
        /// then returns the SubLord PlanetName for a given PlanetLongitude.
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the tropical
        /// ascendant (siderealAsc), and handling special cases such as when siderealAsc is 0 or
        /// when there are overlapping signs.
        /// The subLord pertaining to a Planet given Longitude is returned
        /// </summary>
        public static PlanetName SubLordAtLongitude(Angle planetLongitude)
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = PlanetName.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

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
                    // Assign siderealAscDeg based on the planet name
                    switch (planetName.Name)
                    {
                        case PlanetName.PlanetNameEnum.Ketu:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Venus:
                            siderealAscDeg = new Angle(2, 13, 20).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Sun:
                            siderealAscDeg = new Angle(0, 40, 0).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Moon:
                            siderealAscDeg = new Angle(1, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mars:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Rahu:
                            siderealAscDeg = new Angle(2, 00, 00).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Jupiter:
                            siderealAscDeg = new Angle(1, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Saturn:
                            siderealAscDeg = new Angle(2, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mercury:
                            siderealAscDeg = new Angle(1, 53, 20).TotalDegrees;
                            break;
                    }
                    // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                    var zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    // Special handling for siderealAsc == 0.00
                    if (siderealAsc == 0.00)
                    {
                        var longBefore = siderealAsc - 0.00001 + 360;
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    siderealAsc = siderealAsc + siderealAscDeg;
                    var zSignAtLongAfter = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var zSignAfterLord = LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordAfter = LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if siderealAsc ends on 30, 60, 90, 120....
                    var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);
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
                        var remainderFromDivBy30 = (siderealAsc % 30.00); //past signchange degree amount
                        var preSignChangeDegrees = siderealAscDeg - remainderFromDivBy30;
                        siderealAsc = siderealAsc - siderealAscDeg + preSignChangeDegrees; //this is one Entry into the List. this get us to Sign engpoint
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                        //next process the balance into the nextSign
                        var siderealAscB = siderealAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAscB));
                        cntA++;
                        siderealAsc = siderealAscB;
                    }
                    else
                    {
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                    }
                }
            }

            // Find the horary number in the constellation list and return the corresponding siderealAsc
            var countX = 0;


            var planetSubLord = constellationList[countX].Item6;

            while (countX <= 248)
            {
                if (planetLongitude.TotalDegrees < constellationList[0].Item7)
                {
                    planetSubLord = constellationList[0].Item6 ;
                    break;
                }
                if ((constellationList[countX].Item7 <= planetLongitude.TotalDegrees) &&
                    (planetLongitude.TotalDegrees <= constellationList[countX + 1].Item7))
                {
                    //NOTE:
                    //the -1 because in the list we record end longitudes.
                    //We have to return start longitudes.
                    //the end longitude of the previous one is the start of the current one. 
                    planetSubLord = constellationList[countX+1].Item6;
                    break;
                }
                countX++;
            }

            return planetSubLord;
        }


        /// <summary>
        /// This method first calculates the SubLord Table for a given Longitude.
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the tropical
        /// ascendant (siderealAsc), and handling special cases such as when siderealAsc is 0 or
        /// when there are overlapping signs.
        /// The subNumber pertaining to given Longitude is returned
        /// </summary>
        public static int SubNumberAtLongitude(Angle longitude)
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = PlanetName.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

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
                    // Assign siderealAscDeg based on the planet name
                    switch (planetName.Name)
                    {
                        case PlanetName.PlanetNameEnum.Ketu:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Venus:
                            siderealAscDeg = new Angle(2, 13, 20).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Sun:
                            siderealAscDeg = new Angle(0, 40, 0).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Moon:
                            siderealAscDeg = new Angle(1, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mars:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Rahu:
                            siderealAscDeg = new Angle(2, 00, 00).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Jupiter:
                            siderealAscDeg = new Angle(1, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Saturn:
                            siderealAscDeg = new Angle(2, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mercury:
                            siderealAscDeg = new Angle(1, 53, 20).TotalDegrees;
                            break;
                    }
                    // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                    var zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    // Special handling for siderealAsc == 0.00
                    if (siderealAsc == 0.00)
                    {
                        var longBefore = siderealAsc - 0.00001 + 360;
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    siderealAsc = siderealAsc + siderealAscDeg;
                    var zSignAtLongAfter = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var zSignAfterLord = LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordAfter = LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if siderealAsc ends on 30, 60, 90, 120....
                    var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);
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
                        var remainderFromDivBy30 = (siderealAsc % 30.00); //past signchange degree amount
                        var preSignChangeDegrees = siderealAscDeg - remainderFromDivBy30;
                        siderealAsc = siderealAsc - siderealAscDeg + preSignChangeDegrees; //this is one Entry into the List. this get us to Sign engpoint
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                        //next process the balance into the nextSign
                        var siderealAscB = siderealAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAscB));
                        cntA++;
                        siderealAsc = siderealAscB;
                    }
                    else
                    {
                        constellationList.Add(new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA, constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                    }
                }
            }

            // Find the horary number in the constellation list and return the corresponding siderealAsc
            var countX = 0;
            var subNo = 0;

            var planetSubLord = constellationList[countX].Item6;

            while (countX <= 248)
            {
                if (longitude.TotalDegrees < constellationList[0].Item7)
                {
                    planetSubLord = constellationList[0].Item6;
                    subNo = 1;
                    break;
                }
                if ((constellationList[countX].Item7 <= longitude.TotalDegrees) &&
                    (longitude.TotalDegrees <= constellationList[countX + 1].Item7))
                {
                    //NOTE:
                    //the +1 because in the list we record end longitudes.
                    //We have to return start longitudes.
                    //the end longitude of the previous one is the start of the current one. Hence return x+1
                    planetSubLord = constellationList[countX+1].Item6;
                    subNo = constellationList[countX+1].Item1;
                    break;
                }
                countX++;
            }

            return subNo;
        }

        /// <summary>
        /// This method ONLY Builds the Sign/Star/SubLord table.
        /// The sssTable is agnostic of birthTime. Its more a dicision of the Signs/Stars into Subs
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the tropical
        /// ascendant (siderealAsc), and handling special cases such as when siderealAsc is 0 or
        /// when there are overlapping signs.
        /// The Table is returned
        /// </summary>
        public static List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>
            BuildSignStarSubLordTable()
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = PlanetName.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList =
                new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

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
                    // Assign siderealAscDeg based on the planet name
                    switch (planetName.Name)
                    {
                        case PlanetName.PlanetNameEnum.Ketu:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Venus:
                            siderealAscDeg = new Angle(2, 13, 20).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Sun:
                            siderealAscDeg = new Angle(0, 40, 0).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Moon:
                            siderealAscDeg = new Angle(1, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mars:
                            siderealAscDeg = new Angle(0, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Rahu:
                            siderealAscDeg = new Angle(2, 00, 00).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Jupiter:
                            siderealAscDeg = new Angle(1, 46, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Saturn:
                            siderealAscDeg = new Angle(2, 06, 40).TotalDegrees;
                            break;
                        case PlanetName.PlanetNameEnum.Mercury:
                            siderealAscDeg = new Angle(1, 53, 20).TotalDegrees;
                            break;
                    }

                    // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                    var zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    // Special handling for siderealAsc == 0.00
                    if (siderealAsc == 0.00)
                    {
                        var longBefore = siderealAsc - 0.00001 + 360;
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(longBefore));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(longBefore));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }
                    else
                    {
                        zSignAtLongBefore = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationBefore = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                        constellationLordBefore = LordOfConstellation(constellationBefore.GetConstellationName());
                    }

                    siderealAsc = siderealAsc + siderealAscDeg;
                    var zSignAtLongAfter = ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var zSignAfterLord = LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordAfter = LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if siderealAsc ends on 30, 60, 90, 120....
                    var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);
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
                        var remainderFromDivBy30 = (siderealAsc % 30.00); //past signchange degree amount
                        var preSignChangeDegrees = siderealAscDeg - remainderFromDivBy30;
                        siderealAsc = siderealAsc - siderealAscDeg +
                                  preSignChangeDegrees; //this is one Entry into the List. this get us to Sign engpoint
                        //log entry into List
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA,
                                constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                        //next process the balance into the nextSign
                        var siderealAscB = siderealAsc + remainderFromDivBy30;
                        //log entry into List
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA,
                                constellationLordAfter, planetName, siderealAscB));
                        cntA++;
                        siderealAsc = siderealAscB;
                    }
                    else
                    {
                        constellationList.Add(
                            new Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>(
                                cntA + 1, zSignAtLongAfter.GetSignName(), zSignAfterLord, constellationA,
                                constellationLordAfter, planetName, siderealAsc));
                        cntA++;
                    }
                }
            }
            return constellationList; //return Sign/Star/SubLord Table as List<Tuple<>>
        }

        /// <summary>
        /// THIS IS NOT A COPY - THIS ONE ROTATES THE CHART TO PROVIDED NEW LAGNA - LAST Param in Signature
        /// Hence the call twice to calculate cusps, first one to get base cusps based on HorNUm and then Rotate to provided Lagna siderealAsc
        /// Moves Lagna to a Long Degree (the new Ascendant and gets new KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// </summary>
        public static Dictionary<HouseName, Angle> MoveLagnaToSpecificLongGetNewHouseCusps(int Ayanamsa, Time time,
            int horNum, double siderealAsc)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(
                new CacheKey(nameof(MoveLagnaToSpecificLongGetNewHouseCusps), Ayanamsa, time, horNum, siderealAsc),
                _getNewCuspLongitudes);


            //UNDERLYING FUNCTION
            Dictionary<HouseName, Angle> _getNewCuspLongitudes()
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                SwissEph swissEph = new SwissEph();

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //set ayanamsa
                swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);


                var eps = Calculate.EclipticObliquity(time);

                var armc = ConvertAscToARMC(siderealAsc, eps, location.Latitude(), time);

                var lat = location.Latitude();

                swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

                //base cusps created - now repeat now with provided Long Lagna needs to be moved to
                armc = ConvertAscToARMC(siderealAsc, eps, location.Latitude(), time);
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

        public static ZodiacSign HouseSignName(double houseCuspStartDeg)
        {
            var hZSign = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(houseCuspStartDeg));
            return hZSign;
        }

        public static Constellation HouseConstellationName(double houseCuspStartDeg)
        {
            var hConstellation = Calculate.ConstellationAtLongitude(Angle.FromDegrees(houseCuspStartDeg));
            return hConstellation;
        }


        #endregion

        #region TEMP METHODS

        /// <summary>
        /// Gets all KP (Placidus) House Cusps 
        /// using Swiss Epehemris swe_houses_ex
        /// this is simply a test method to test the KPHoraryLongitudes and KPBirthtimLongitudes methods
        /// </summary>
        // Kp Cusp Calculations
        public static void KpPrintHouseCuspLong(int Ayanamsa, Time refTime, int horNum)
        {
            Console.Write("Birth Time is {0} DayOfWeek: ", refTime);
            DateTime dt = new DateTime(refTime.StdYear(), refTime.StdMonth(), refTime.StdDate());
            Console.WriteLine(dt.DayOfWeek);

            Console.WriteLine("Horary Number is {0}", horNum);

            Console.WriteLine("Ayanamsa is {0} ", Calculate.Ayanamsa);
            var ayanamsaDegree = Calculate.AyanamsaDegree(refTime);
            Console.WriteLine("Ayanamsa Degree is {0}, {1} Deg {2} Min {3} Secs ", ayanamsaDegree.Rounded,
                ayanamsaDegree.Degrees,
                ayanamsaDegree.Minutes, ayanamsaDegree.Seconds);

            Console.WriteLine("================== KP Houses and Planets - From SWISS EPH Modified Method ===================");

            var cusps = new Dictionary<HouseName, Angle>();
            if (horNum == 0)
            {
                cusps = AllHouseCuspLongitudesKundali(refTime);
            }
            else
            {
                cusps = AllHouseCuspLongitudesHorary(refTime, horNum);
            }


            PrintCuspsToConsole(cusps);

            //House Processing Complete ------------------------------


            //Now Process the Planets
            var planetDataDict = CalculateKP.PlanetData(Calculate.Ayanamsa, refTime, horNum);

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

            //the next section focuses on counting how many times a Planet is present as StarLord
            //this will tell us which Planets are dominant in a persons life
            //because planets give the results of thier StarLords. 
            var allPlanets = VedAstro.Library.PlanetName.All9Planets;
            int planetcnt = 0;
            foreach (var planet in allPlanets)
            {
                planetcnt = 0;
                foreach (var planetKey in allPlanets)
                {
                    if (planetDataDict[planetKey].Item5 == planet)
                    {
                        planetcnt++;
                    }
                }
                Console.WriteLine("Planet {0} appears {1} times as StarLord", planet, planetcnt);
            }
        }

        public static void PrintCuspsToConsole(Dictionary<HouseName, Angle> cusps)
        {
            int cnt = 0;

            Console.WriteLine(" ====== ");
            foreach (var h in cusps)
            {
                var cuspDMS = h.Value;

                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(cuspDMS);

                Console.WriteLine(
                    "Long {0} in DMS: {1} Deg {2} Mins {3} Secs; Zodiac {4} {5} Deg {6} Mins {7} Secs in Star: {8}",
                    h,
                    cuspDMS.Degrees, cuspDMS.Minutes, cuspDMS.Seconds, zodiacSignAtLong.GetSignName(),
                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                    zodiacSignAtLong.GetDegreesInSign().Seconds, ConstellationAtLongitude(cuspDMS));

                Console.WriteLine("SignL: {0}, StarL: {1}, SubL: {2}",
                    LordOfZodiacSign(zodiacSignAtLong.GetSignName()),
                    LordOfConstellation(ConstellationAtLongitude(cuspDMS).GetConstellationName()),
                    SubLordAtLongitude(cuspDMS));

                cnt++;
            }
        }


        /// <summary>
        /// Process planet positions and returns Dictionary for webpage table
        /// </summary>
        public static Dictionary<PlanetName, (Angle, HouseName, ZodiacName, ConstellationName, PlanetName, PlanetName, PlanetName)>
            PlanetData(int Ayanamsa, Time birthtime, int horNum)
        {
            Dictionary<PlanetName, (Angle, HouseName, ZodiacName, ConstellationName, PlanetName, PlanetName, PlanetName)>
                planetTableData =
                    new Dictionary<PlanetName, (Angle, HouseName, ZodiacName, ConstellationName, PlanetName, PlanetName, PlanetName
                        )>();

            var allPlanets = VedAstro.Library.PlanetName.All9Planets;
            var x = 0;
            var allPlanetConstellation = Calculate.AllPlanetConstellation(birthtime);
            /*
            foreach (var planetConstellation in allPlanetConstellation)
            {
                Console.WriteLine(" {0} {1}", planetConstellation.Key, planetConstellation.Value.GetConstellationName().ToString()); //allPlanetConstellation[0].GetQuarter().ToString());
                var yy = Calculate.LordOfConstellation(planetConstellation.Value.GetConstellationName());
            } */

            var cusps = new Dictionary<HouseName, Angle>();

            if (horNum == 0)
            {
                cusps = AllHouseCuspLongitudesKundali(birthtime);
            }
            else
            {
                cusps = AllHouseCuspLongitudesHorary(birthtime, horNum);
            }

            Console.WriteLine("");
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
                while (x <= cusps.Count) //check each house for the logic below
                {
                    //Console.WriteLine("Counter x {0}", x);
                    if (x < cusps.Count) //if there is 'next cusp' still left to compare with;(without exceeding the bounds of the array)
                    {
                        //Console.WriteLine("not at the near end of array");
                        if (cusps[(HouseName)x + 1] > cusps[(HouseName)x]) //check if cusp longitude is smaller than next cusp longitude
                                                                           //because the last house will have cusp long larger then next house start long
                        {
                            //Console.WriteLine("Next Cusp Long is greater than current Cusp Long");
                           // Console.WriteLine("planet degrees {0}; cusp x degrees {1} cusp x+1 degrees {2}", planetNirayanaDegrees.TotalDegrees, cusps[(HouseName)x].TotalDegrees, cusps[(HouseName)x + 1].TotalDegrees);
                            //if the planet longitude falls between this cusp long AND next cusp long, perform process
                            if ((planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)x].TotalDegrees) &&
                                (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)x + 1].TotalDegrees)) //this means that the planet falls in between these house cusps
                            {
                                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                                var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                var subLordAtLongitude = CalculateKP.SubLordAtLongitude(planetNirayanaDegrees);

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6}; StarL {7}; SubL {8} ", planet.Name, x, zodiacSignAtLong.GetSignName(),
                                                            zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                                            zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation, subLordAtLongitude);
                                planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees),(HouseName) x, zodiacSignAtLong.GetSignName(),
                                    constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation, subLordAtLongitude));

                                break;
                            }
                        }
                        else //if next cusp start long is smaller than current cusp - means that we are rotating through 360 deg mark
                             //hence use the OR condition
                        {
                            //Console.WriteLine("Next Cusp Long is greater than current Cusp Long");
                            //Console.WriteLine("planet degrees {0}; cusp x degrees {1} cusp x+1 degrees {2}", planetNirayanaDegrees.TotalDegrees, cusps[(HouseName)x].TotalDegrees, cusps[(HouseName)x + 1].TotalDegrees);

                            if ((planetNirayanaDegrees.TotalDegrees >= cusps[(HouseName)x].TotalDegrees) ||
                                (planetNirayanaDegrees.TotalDegrees <= cusps[(HouseName)x + 1].TotalDegrees))
                            {
                                var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                                var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                                var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                                var subLordAtLongitude = CalculateKP.SubLordAtLongitude(planetNirayanaDegrees);

                                Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S; SignL {6} StarL {7} SubL {8} ", planet.Name, x + 1, zodiacSignAtLong.GetSignName(),
                                    zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                                    zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation, subLordAtLongitude);
                                planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees), (HouseName) (x+1), zodiacSignAtLong.GetSignName(),
                                    constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation, subLordAtLongitude));
                                break;
                            }
                        }
                    }
                    else //this means that there is NO 'next cusp' still left to compare with;(without exceeding the bounds of the array)
                         // we are in the 12th house now; planet is in the 12th house
                    {
                        //Console.WriteLine("Arrived in teh else conditon for 12th house, no next cusp available to compare");
                        
                        //Console.WriteLine("planet degrees {0}; cusp x degrees {1} ", planetNirayanaDegrees.TotalDegrees, cusps[(HouseName)x].TotalDegrees);

                        var zodiacSignAtLong = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                        var lordOfZodiac = Calculate.LordOfZodiacSign(zodiacSignAtLong.GetSignName());
                        var constellationAtLong = Calculate.ConstellationAtLongitude(Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees));
                        var lordOfConstellation = Calculate.LordOfConstellation(planetConstellation.GetConstellationName());

                        var subLordAtLongitude = CalculateKP.SubLordAtLongitude(planetNirayanaDegrees);

                        //we use x+1 in the Console.Writeline becuase the counter is still at 11th house, but the else condition has brought us to the 12th house
                        Console.WriteLine("Planet {0} is in House {1} {2} {3} D {4} M {5} S ; SignL {6} StarL {7} ; SubL {8} ", planet.Name, x, zodiacSignAtLong.GetSignName(),
                            zodiacSignAtLong.GetDegreesInSign().Degrees, zodiacSignAtLong.GetDegreesInSign().Minutes,
                            zodiacSignAtLong.GetDegreesInSign().Seconds, lordOfZodiac, lordOfConstellation, subLordAtLongitude);

                        planetTableData.Add(planet, (Angle.FromDegrees(planetNirayanaDegrees.TotalDegrees), (HouseName) x, zodiacSignAtLong.GetSignName(),
                            constellationAtLong.GetConstellationName(), lordOfZodiac, lordOfConstellation, subLordAtLongitude));
                        break;
                    }
                    x++;
                }
                
                
            }
            Console.WriteLine("Processing Planet Data Complete....");
            Console.WriteLine("");
            return planetTableData;
        }

        public static HouseName HouseAtLongitude(Dictionary<HouseName, Angle> cuspsDictionary, Angle ilong)
        {
            HouseName hName = HouseName.Empty;
            foreach (var h in cuspsDictionary)
            {
                if (h.Value == ilong)
                {
                    hName = h.Key;
                    break;
                }
            }

            return hName;
        }

        #endregion

    }
}
