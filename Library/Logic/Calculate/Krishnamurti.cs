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

                

                var lat = location.Latitude();

                Console.WriteLine("armc {0} eps {1} tropAsc {2} lat {3}", armc, eps, siderealAsc, lat);

                swissEph.swe_houses_armc(armc, lat, eps, 'P', cusps, ascmc);

                //base cusps created - now repeat now with provided Long Lagna needs to be moved to
                armc = Calculate.AscendantDegreesToARMC(rotateDegrees, eps, location.Latitude(), time);
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

           
}
