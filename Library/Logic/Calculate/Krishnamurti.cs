using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class Krishnamurti
    {
        /// <summary>
        /// Special order of planets for KP system
        /// NOTE: - extracted out to make unambigious
        /// NOTE: Starts with Ketu and very important to keep this order (below code depend)
        /// </summary>
        public static readonly List<PlanetName> All9Planets = new()
        {
            PlanetName.Ketu, PlanetName.Venus, PlanetName.Sun, PlanetName.Moon,
            PlanetName.Mars,PlanetName.Rahu,PlanetName.Jupiter, PlanetName.Saturn, PlanetName.Mercury
        };

        /// <summary>
        /// KP astrology uses the "cusps", which are the points where two houses meet.
        /// The cusps are assigned to a planet and a sub-lord, which are then related to the houses they signify. 
        /// Based on horary number and rotate degrees, this method will auto select the correct house cusps
        /// </summary>
        public static Dictionary<HouseName, Angle> AllHouseCuspLongitudes(Time time, int horaryNumber = 0, double rotateDegrees = 0)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(AllHouseCuspLongitudes), time, Calculate.Ayanamsa), _allHouseCuspLongitudes);

            //UNDERLYING FUNCTION
            Dictionary<HouseName, Angle> _allHouseCuspLongitudes()
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


            //------------------------LOCAL FUNCTIONS------------------------

            static Dictionary<HouseName, Angle> AllHouseCuspLongitudesRotateHorary(Time time, int horaryNumber, double rotateDegrees)
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

                var siderealAsc = HoraryNumberToSiderealAscendant(horaryNumber);
                var armc = Calculate.AscendantDegreesToARMC(siderealAsc, eps, location.Latitude(), time);

                var lat = location.Latitude();

                Console.WriteLine("armc {0} eps {1} tropAsc {2} lat {3}", armc, eps, siderealAsc, lat);

                swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

                //base cusps created - now repeat now with provided Long Lagna needs to be moved to
                armc = Calculate.AscendantDegreesToARMC(rotateDegrees, eps, location.Latitude(), time);
                


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
            /// Calculates the astrological house cusps for a given time and horary number.
            /// It uses the Swiss Ephemeris to compute high precision astronomical data.
            /// The method converts the horary number to Tropical Ascendant degrees,
            /// then to ARMC (Sidereal Time), which is used to calculate house cusps.
            /// The house system is calculated using the ARMC, latitude, and obliquity of the ecliptic.
            /// The results are packaged into a dictionary mapping each house to its corresponding angle
            /// </summary>
            static Dictionary<HouseName, Angle> AllHouseCuspLongitudesHorary(Time time, int horaryNumber)
            {
                //get location at place of time
                var location = time.GetGeoLocation();
                Console.WriteLine("Inside AllHouseCuspLongHorary with GeoLoc {0}", location.Latitude());

                SwissEph swissEphHorary = new SwissEph();
                var swissEphVersion = swissEphHorary.swe_version();
                Console.WriteLine("{0}", swissEphVersion);
                var swissLibPath = swissEphHorary.swe_get_library_path();
                Console.WriteLine("{0}", swissLibPath);
                swissEphHorary.swe_set_ephe_path(null);

                //var jul_day_ut = Calculate.TimeToJulianDay(time); REMOVE LATER
                //Console.WriteLine("Julian Day for 18 June 1970 10:30a {0}", jul_day_ut);

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //define the flag for sidereal calculations
                const int iFlag = SwissEph.SEFLG_SIDEREAL; //May not be useful for Horary ARMC based calculations. Remove later

                // The obliquity of the ecliptic is the angle between the ecliptic and the celestial equator. 
                // It changes over time and is calculated for a specific time.
                var eps = 1.0; //Updated
                Console.WriteLine("Ecliptic Obliquity eps {0}", eps);

                // The horary number is converted to Tropical Ascendant degrees.
                // The Tropical Ascendant is the degree of the zodiac that is rising
                // on the eastern horizon at the time for which the horoscope is cast.
                var siderealAsc = 0.5; //Updated
                Console.WriteLine("siderealAsc Degrees {0}", siderealAsc);
                var tropAsc = siderealAsc + Calculate.AyanamsaDegree(time).TotalDegrees;
                Console.WriteLine("tropAsc Degrees {0}", tropAsc);

                // The Ascendant degree is then converted to the ARMC (Sidereal Time).
                // The ARMC is used in the calculation of house cusps.
                var armc = 0.9;
                // armc Updated  

                Console.WriteLine("armc {0}", armc);
                Console.WriteLine("Latitude {0}", location.Latitude());

                //set ayanamsa
                swissEphHorary.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);

                double daya;
                string serr = "";
                var jul_day_ut = Calculate.TimeToJulianDay(time);
                

                Console.Write("Ayanamsa {0}; Ayanamsa Degrees {1}, ", Calculate.Ayanamsa, Calculate.AyanamsaDegree(time));
                var ayanamsaDegree = Calculate.AyanamsaDegree(time);
                Console.WriteLine("Ayanamsa Degree is {0}, {1} Deg {2} Min {3} Secs ", ayanamsaDegree.Rounded,
                    ayanamsaDegree.Degrees, ayanamsaDegree.Minutes, ayanamsaDegree.Seconds);

                // The house system is calculated using the ARMC, latitude, and obliquity of the ecliptic.
                // The 'P' parameter specifies the Placidus house system.
                //Updated
                //IMPORTANT: At this point cusps is returning Tropical cusp positions. Matches LOKPA Horary Cusps with LOKPA settings set to Tropical Zodiac
                //Convert back to Sidereal below by deducting Ayanamsa.

                //package data before sending
                //revert back to sidereal by deducting Ayanamsa
                var housesDictionary = new Dictionary<HouseName, Angle>();
                foreach (var house in House.AllHouses)
                {
                    //start of house longitude of 0-360
                    var hseLong = cusps[(int)house];
                    var hseLongSidereal = 0.7;
                    housesDictionary.Add(house, Angle.FromDegrees(hseLongSidereal));
                }
                //return Sidereal cusps;
                return housesDictionary;
            }

            /// <summary>
            /// Rotates the chart to provided new lagna
            /// Hence the call twice to calculate cusps, first one to get base cusps based on HorNUm and then Rotate to provided Lagna tropAsc
            /// Moves Lagna to a Long Degree (the new Ascendant and gets new KP (Placidus) House Cusps 
            /// using Swiss Epehemris swe_houses_ex
            /// </summary>
            static Dictionary<HouseName, Angle> AllHouseCuspLongitudesRotateKundali(Time time, double rotateDegrees)
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                SwissEph swissEph = new SwissEph();

                double[] cusps = new double[13];

                //we have to supply ascmc to make the function run
                double[] ascmc = new double[10];

                //set ayanamsa
                swissEph.swe_set_sid_mode(Calculate.Ayanamsa, 0, 0);

                var eps = 0.5;

                var armc = 0.98;

                var lat = location.Latitude();

                //NOTE: KP astrology uses the Placidus house system, which divides the zodiac into 12 unequal houses


                //base cusps created - now repeat now with provided Long Lagna needs to be moved to
                armc = 0.98;
                


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
            static Dictionary<HouseName, Angle> AllHouseCuspLongitudesKundali(Time time)
            {
                //get location at place of time
                var location = time.GetGeoLocation();

                //Convert DOB to Julian Day
                var julDayUt = Calculate.TimeToJulianDay(time);

                SwissEph swissEph = new SwissEph();
                double[] cusps = new double[13];
                double[] ascmc = new double[10];

                //set ayanamsa
                //Updated

                //define the flag for sidereal calculations
                const int iFlag = SwissEph.SEFLG_SIDEREAL;

                //calculate the house cusps and ascmc values
                //Updated

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

        }


        /// <summary>
        /// This method calculates the sidereal ascendant for a given horary number.
        /// It does this by iterating over all constellations and planets, calculating
        /// various parameters before and after adding a certain degree to the sidereal
        /// ascendant (siderealAsc), and handling special cases such as when sidAsc is 0 or
        /// when there are overlapping signs. The sidereal ascendant corresponding to the
        /// given horary number is then returned.
        /// </summary>
        public static double HoraryNumberToSiderealAscendant(int horaryNumber)
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = Krishnamurti.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

            // Iterate over all constellations
            foreach (var constellation in allConstellations)
            {
                var tempConstel = constellation;
                var lordofConstel = Calculate.LordOfConstellation(tempConstel);
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

                // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                var zSignAtLongBefore = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                var constellationBefore = Calculate.ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                var constellationLordBefore = Calculate.LordOfConstellation(constellationBefore.GetConstellationName());


                siderealAsc = siderealAsc + siderealAscDeg;
                var zSignAtLongAfter = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                var zSignAfterLord = Calculate.LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                var constellationAfter = Calculate.ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                var constellationLordAfter = Calculate.LordOfConstellation(constellationAfter.GetConstellationName());

                // Check if siderealAsc ends on 30, 60, 90, 120....
                //var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);

                // Handle overlapping signs issue
               
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
        /// Gets sub-lord at a given longitude (planet or house cusp) 
        /// </summary>
        /// <param name="longitude">planet or house cusp longitude</param>
        public static PlanetName SubLordAtLongitude(Angle longitude)
        {
            // Initialize variables
            var siderealAscDeg = 0.00;
            var all9Planets = Krishnamurti.All9Planets;
            var allConstellations = Constellation.AllConstellation;
            var constellationList = new List<Tuple<int, ZodiacName, PlanetName, ConstellationName, PlanetName, PlanetName, double>>();
            var cntA = 0;
            var siderealAsc = 0.000;

            // Iterate over all constellations
            foreach (var constellation in allConstellations)
            {
                var tempConstel = constellation;
                var lordofConstel = Calculate.LordOfConstellation(tempConstel);
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
                    //lordofConstel = LordOfConstellation(constellationA);
                    // Assign siderealAscDeg based on the planet name
                   
                    // Calculate various parameters before and after adding siderealAscDeg to siderealAsc
                    var zSignAtLongBefore = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationBefore = Calculate.ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordBefore = Calculate.LordOfConstellation(constellationBefore.GetConstellationName());
                    // Special handling for siderealAsc == 0.00
                   
                    siderealAsc = siderealAsc + siderealAscDeg;
                    var zSignAtLongAfter = Calculate.ZodiacSignAtLongitude(Angle.FromDegrees(siderealAsc));
                    var zSignAfterLord = Calculate.LordOfZodiacSign(zSignAtLongAfter.GetSignName());
                    var constellationAfter = Calculate.ConstellationAtLongitude(Angle.FromDegrees(siderealAsc));
                    var constellationLordAfter = Calculate.LordOfConstellation(constellationAfter.GetConstellationName());
                    // Check if siderealAsc ends on 30, 60, 90, 120....
                    //var siderealAscSpansSigns = Math.Round(siderealAsc % 30, 6);
                    // Handle overlapping signs issue
                   
                }
            }

            // Find the horary number in the constellation list and return the corresponding siderealAsc
            var countX = 0;

            var planetSubLord = constellationList[countX].Item6;

            while (countX <= 248)
            {
                if (longitude.TotalDegrees < constellationList[0].Item7)
                {
                    planetSubLord = constellationList[0].Item6;
                    break;
                }
                if ((constellationList[countX].Item7 <= longitude.TotalDegrees) &&
                    (longitude.TotalDegrees <= constellationList[countX + 1].Item7))
                {
                    //NOTE:
                    //the -1 because in the list we record end longitudes.
                    //We have to return start longitudes.
                    //the end longitude of the previous one is the start of the current one. 
                    planetSubLord = constellationList[countX + 1].Item6;
                    break;
                }
                countX++;
            }

            return planetSubLord;
        }

    }
}
