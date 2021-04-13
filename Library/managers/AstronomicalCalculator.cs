using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Google.Apis.Auth.OAuth2.Requests;
using Genso.Astrology.Library;
using SwissEphNet;
using DayOfWeek = Genso.Astrology.Library.DayOfWeek;
using System.Reflection;
using Genso.Framework;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Collection of astronomical calculator functions
    /// Note : Many of the functions here use cacheing machanism
    /// </summary>
    public static class AstronomicalCalculator
    {
        //CACHED FUNCTIONS
        //NOTE : These are functions that don't call other functions from this class
        //       Only functions that don't call other cached functions are allowed to be cached
        //       otherwise, it's errorneous in parallel

        public static Angle GetAyanamsa(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetAyanamsa", time), _getAyanamsa);


            //UNDERLYING FUNCTION
            Angle _getAyanamsa()
            {
                int year = LmtToUtc(time).Year;

                var returnValue = new Angle(seconds: (long)(Math.Round((year - 397) * 50.3333333333)));

                return returnValue;
            }

        }

        /// <summary>
        /// NOTE This method connects SwissEph Library with Muhurta Library
        /// </summary>
        public static Angle GetPlanetSayanaLongitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSayanaLongitude", time, planetName), _getPlanetSayanaLongitude);


            //UNDERLYING FUNCTION

            Angle _getPlanetSayanaLongitude()
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
                if (planetName == PlanetName.Sun)
                    planet = SwissEph.SE_SUN;
                else if (planetName == PlanetName.Moon)
                {
                    planet = SwissEph.SE_MOON;
                }
                else if (planetName == PlanetName.Mars)
                {
                    planet = SwissEph.SE_MARS;
                }
                else if (planetName == PlanetName.Mercury)
                {
                    planet = SwissEph.SE_MERCURY;
                }
                else if (planetName == PlanetName.Jupiter)
                {
                    planet = SwissEph.SE_JUPITER;
                }
                else if (planetName == PlanetName.Venus)
                {
                    planet = SwissEph.SE_VENUS;
                }
                else if (planetName == PlanetName.Saturn)
                {
                    planet = SwissEph.SE_SATURN;
                }
                else if (planetName == PlanetName.Rahu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }
                else if (planetName == PlanetName.Ketu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

                //data in results at index 0 is longitude
                return new Angle(degrees: results[0]);

            }


        }
        public static Angle GetPlanetSayanaLatitude(Time time, PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSayanaLatitude", time, planetName), _getPlanetSayanaLatitude);


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
                if (planetName == PlanetName.Sun)
                    planet = SwissEph.SE_SUN;
                else if (planetName == PlanetName.Moon)
                {
                    planet = SwissEph.SE_MOON;
                }
                else if (planetName == PlanetName.Mars)
                {
                    planet = SwissEph.SE_MARS;
                }
                else if (planetName == PlanetName.Mercury)
                {
                    planet = SwissEph.SE_MERCURY;
                }
                else if (planetName == PlanetName.Jupiter)
                {
                    planet = SwissEph.SE_JUPITER;
                }
                else if (planetName == PlanetName.Venus)
                {
                    planet = SwissEph.SE_VENUS;
                }
                else if (planetName == PlanetName.Saturn)
                {
                    planet = SwissEph.SE_SATURN;
                }
                else if (planetName == PlanetName.Rahu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }
                else if (planetName == PlanetName.Ketu)
                {
                    planet = SwissEph.SE_MEAN_NODE;
                }

                //Get planet long
                int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

                //data in results at index 1 is latitude
                return new Angle(degrees: results[1]);

            }


        }

        public static double GetPlanetSpeed(Time time, PlanetName planetName)
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
            if (planetName == PlanetName.Sun)
                planet = SwissEph.SE_SUN;
            else if (planetName == PlanetName.Moon)
            {
                planet = SwissEph.SE_MOON;
            }
            else if (planetName == PlanetName.Mars)
            {
                planet = SwissEph.SE_MARS;
            }
            else if (planetName == PlanetName.Mercury)
            {
                planet = SwissEph.SE_MERCURY;
            }
            else if (planetName == PlanetName.Jupiter)
            {
                planet = SwissEph.SE_JUPITER;
            }
            else if (planetName == PlanetName.Venus)
            {
                planet = SwissEph.SE_VENUS;
            }
            else if (planetName == PlanetName.Saturn)
            {
                planet = SwissEph.SE_SATURN;
            }
            else if (planetName == PlanetName.Rahu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }
            else if (planetName == PlanetName.Ketu)
            {
                planet = SwissEph.SE_MEAN_NODE;
            }

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, results, ref err_msg);

            //data in results at index 3 is speed in right ascension (deg/day)
            return results[3];
        }

        public static PlanetConstellation GetPlanetConstellation(Angle planetLongitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetConstellation", planetLongitude), _getPlanetConstellation);



            //UNDERLYING FUNCTION
            PlanetConstellation _getPlanetConstellation()
            {
                //get planet's longitude in minutes
                var planetLongitudeInMinutes = planetLongitude.TotalMinutes;

                //The ecliptic is divided into 27 constellations
                //of 13° 20' (or 800' of arc) each. Hence divide 800
                var roughConstellationNumber = planetLongitudeInMinutes / 800.0;

                //get constellation number
                var constellationNumber = (int)Math.Ceiling(roughConstellationNumber);

                //calculate quarter from body position
                int quarter;

                var remainder = roughConstellationNumber - Math.Floor(roughConstellationNumber);

                if (remainder >= 0 && remainder <= 0.25) quarter = 1;
                else if (remainder > 0.25 && remainder <= 0.5) quarter = 2;
                else if (remainder > 0.5 && remainder <= 0.75) quarter = 3;
                else if (remainder > 0.75 && remainder <= 1) quarter = 4;
                else quarter = 0;

                //initialize new constellation with number & quarter
                var constellation = new PlanetConstellation(constellationNumber, quarter);

                //return constellation value
                return constellation;
            }

        }

        /// <summary>
        /// Get zodiac sign at the longitude
        /// And contains also the degrees in that sign
        /// </summary>
        public static ZodiacSign GetZodiacSignAtLongitude(Angle longitude)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetZodiacSignAtLongitude", longitude), _getZodiacSignAtLongitude);


            //UNDERLYING FUNCTION
            ZodiacSign _getZodiacSignAtLongitude()
            {
                //max degrees of each sign
                const double maxDegreesInSign = 30.0;

                //get rough zodiac number
                double roughZodiacNumber = (longitude.TotalDegrees % 360.0) / maxDegreesInSign;

                //Calculate degrees in zodiac sign
                //get remainder from rough zodiac number
                var roughZodiacNumberRemainder = roughZodiacNumber - Math.Truncate(roughZodiacNumber);

                //convert remainder to degrees in current sign
                var degreesInSign = roughZodiacNumberRemainder * maxDegreesInSign;

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

                //if rough zodiac number is less than 0, then return Pisces else return calculated zodiac
                ZodiacName currentSignName = (roughZodiacNumber < 0) ? ZodiacName.Pisces : calculatedZodiac;

                //return new instance of planet sign
                var degreesAngle = Angle.FromDegrees(Math.Abs(degreesInSign)); //make always positive
                return new ZodiacSign(currentSignName, degreesAngle);
            }


        }

        public static DayOfWeek GetDayOfWeek(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetDayOfWeek", time), _getDayOfWeek);


            //UNDERLYING FUNCTION
            DayOfWeek _getDayOfWeek()
            {
                //The Hindu day begins with sunrise and continues till
                //next sunrise.The first hora on any day will be the
                //first hour after sunrise and the last hora, the hour
                //before sunrise the next day.

                //TODO Change to new day system
                //TODO make test first

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
        public static PlanetName GetLordOfHora(int hora, DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Sun;
                        case 2: return PlanetName.Venus;
                        case 3: return PlanetName.Mercury;
                        case 4: return PlanetName.Moon;
                        case 5: return PlanetName.Saturn;
                        case 6: return PlanetName.Jupiter;
                        case 7: return PlanetName.Mars;
                        case 8: return PlanetName.Sun;
                        case 9: return PlanetName.Venus;
                        case 10: return PlanetName.Mercury;
                        case 11: return PlanetName.Moon;
                        case 12: return PlanetName.Saturn;
                        case 13: return PlanetName.Jupiter;
                        case 14: return PlanetName.Mars;
                        case 15: return PlanetName.Sun;
                        case 16: return PlanetName.Venus;
                        case 17: return PlanetName.Mercury;
                        case 18: return PlanetName.Moon;
                        case 19: return PlanetName.Saturn;
                        case 20: return PlanetName.Jupiter;
                        case 21: return PlanetName.Mars;
                        case 22: return PlanetName.Sun;
                        case 23: return PlanetName.Venus;
                        case 24: return PlanetName.Mercury;
                    }
                    break;
                case DayOfWeek.Monday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Moon;
                        case 2: return PlanetName.Saturn;
                        case 3: return PlanetName.Jupiter;
                        case 4: return PlanetName.Mars;
                        case 5: return PlanetName.Sun;
                        case 6: return PlanetName.Venus;
                        case 7: return PlanetName.Mercury;
                        case 8: return PlanetName.Moon;
                        case 9: return PlanetName.Saturn;
                        case 10: return PlanetName.Jupiter;
                        case 11: return PlanetName.Mars;
                        case 12: return PlanetName.Sun;
                        case 13: return PlanetName.Venus;
                        case 14: return PlanetName.Mercury;
                        case 15: return PlanetName.Moon;
                        case 16: return PlanetName.Saturn;
                        case 17: return PlanetName.Jupiter;
                        case 18: return PlanetName.Mars;
                        case 19: return PlanetName.Sun;
                        case 20: return PlanetName.Venus;
                        case 21: return PlanetName.Mercury;
                        case 22: return PlanetName.Moon;
                        case 23: return PlanetName.Saturn;
                        case 24: return PlanetName.Jupiter;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Mars;
                        case 2: return PlanetName.Sun;
                        case 3: return PlanetName.Venus;
                        case 4: return PlanetName.Mercury;
                        case 5: return PlanetName.Moon;
                        case 6: return PlanetName.Saturn;
                        case 7: return PlanetName.Jupiter;
                        case 8: return PlanetName.Mars;
                        case 9: return PlanetName.Sun;
                        case 10: return PlanetName.Venus;
                        case 11: return PlanetName.Mercury;
                        case 12: return PlanetName.Moon;
                        case 13: return PlanetName.Saturn;
                        case 14: return PlanetName.Jupiter;
                        case 15: return PlanetName.Mars;
                        case 16: return PlanetName.Sun;
                        case 17: return PlanetName.Venus;
                        case 18: return PlanetName.Mercury;
                        case 19: return PlanetName.Moon;
                        case 20: return PlanetName.Saturn;
                        case 21: return PlanetName.Jupiter;
                        case 22: return PlanetName.Mars;
                        case 23: return PlanetName.Sun;
                        case 24: return PlanetName.Venus;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Mercury;
                        case 2: return PlanetName.Moon;
                        case 3: return PlanetName.Saturn;
                        case 4: return PlanetName.Jupiter;
                        case 5: return PlanetName.Mars;
                        case 6: return PlanetName.Sun;
                        case 7: return PlanetName.Venus;
                        case 8: return PlanetName.Mercury;
                        case 9: return PlanetName.Moon;
                        case 10: return PlanetName.Saturn;
                        case 11: return PlanetName.Jupiter;
                        case 12: return PlanetName.Mars;
                        case 13: return PlanetName.Sun;
                        case 14: return PlanetName.Venus;
                        case 15: return PlanetName.Mercury;
                        case 16: return PlanetName.Moon;
                        case 17: return PlanetName.Saturn;
                        case 18: return PlanetName.Jupiter;
                        case 19: return PlanetName.Mars;
                        case 20: return PlanetName.Sun;
                        case 21: return PlanetName.Venus;
                        case 22: return PlanetName.Mercury;
                        case 23: return PlanetName.Moon;
                        case 24: return PlanetName.Saturn;
                    }
                    break;
                case DayOfWeek.Thursday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Jupiter;
                        case 2: return PlanetName.Mars;
                        case 3: return PlanetName.Sun;
                        case 4: return PlanetName.Venus;
                        case 5: return PlanetName.Mercury;
                        case 6: return PlanetName.Moon;
                        case 7: return PlanetName.Saturn;
                        case 8: return PlanetName.Jupiter;
                        case 9: return PlanetName.Mars;
                        case 10: return PlanetName.Sun;
                        case 11: return PlanetName.Venus;
                        case 12: return PlanetName.Mercury;
                        case 13: return PlanetName.Moon;
                        case 14: return PlanetName.Saturn;
                        case 15: return PlanetName.Jupiter;
                        case 16: return PlanetName.Mars;
                        case 17: return PlanetName.Sun;
                        case 18: return PlanetName.Venus;
                        case 19: return PlanetName.Mercury;
                        case 20: return PlanetName.Moon;
                        case 21: return PlanetName.Saturn;
                        case 22: return PlanetName.Jupiter;
                        case 23: return PlanetName.Mars;
                        case 24: return PlanetName.Sun;
                    }
                    break;
                case DayOfWeek.Friday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Venus;
                        case 2: return PlanetName.Mercury;
                        case 3: return PlanetName.Moon;
                        case 4: return PlanetName.Saturn;
                        case 5: return PlanetName.Jupiter;
                        case 6: return PlanetName.Mars;
                        case 7: return PlanetName.Sun;
                        case 8: return PlanetName.Venus;
                        case 9: return PlanetName.Mercury;
                        case 10: return PlanetName.Moon;
                        case 11: return PlanetName.Saturn;
                        case 12: return PlanetName.Jupiter;
                        case 13: return PlanetName.Mars;
                        case 14: return PlanetName.Sun;
                        case 15: return PlanetName.Venus;
                        case 16: return PlanetName.Mercury;
                        case 17: return PlanetName.Moon;
                        case 18: return PlanetName.Saturn;
                        case 19: return PlanetName.Jupiter;
                        case 20: return PlanetName.Mars;
                        case 21: return PlanetName.Sun;
                        case 22: return PlanetName.Venus;
                        case 23: return PlanetName.Mercury;
                        case 24: return PlanetName.Moon;
                    }
                    break;
                case DayOfWeek.Saturday:
                    switch (hora)
                    {
                        case 1: return PlanetName.Saturn;
                        case 2: return PlanetName.Jupiter;
                        case 3: return PlanetName.Mars;
                        case 4: return PlanetName.Sun;
                        case 5: return PlanetName.Venus;
                        case 6: return PlanetName.Mercury;
                        case 7: return PlanetName.Moon;
                        case 8: return PlanetName.Saturn;
                        case 9: return PlanetName.Jupiter;
                        case 10: return PlanetName.Mars;
                        case 11: return PlanetName.Sun;
                        case 12: return PlanetName.Venus;
                        case 13: return PlanetName.Mercury;
                        case 14: return PlanetName.Moon;
                        case 15: return PlanetName.Saturn;
                        case 16: return PlanetName.Jupiter;
                        case 17: return PlanetName.Mars;
                        case 18: return PlanetName.Sun;
                        case 19: return PlanetName.Venus;
                        case 20: return PlanetName.Mercury;
                        case 21: return PlanetName.Moon;
                        case 22: return PlanetName.Saturn;
                        case 23: return PlanetName.Jupiter;
                        case 24: return PlanetName.Mars;
                    }
                    break;
            }

            throw new Exception("Did not find hora, something wrong!");

        }

        /// <summary>
        /// Gets the junction point (sandhi) between 2 consecutive
        /// houses, where one house begins and the other ends.
        /// </summary>
        public static Angle GetHouseJunctionPoint(Angle previousHouse, Angle nextHouse)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetHouseJunctionPoint", previousHouse, nextHouse), _getHouseJunctionPoint);


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

        public static PlanetName GetLordOfZodiacSign(ZodiacName signName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetLordOfZodiacSign", signName), _getLordOfZodiacSign);


            //UNDERLYING FUNCTION
            PlanetName _getLordOfZodiacSign()
            {
                switch (signName)
                {
                    //Aries and Scorpio are ruled by Mars;
                    case ZodiacName.Aries:
                    case ZodiacName.Scorpio:
                        return PlanetName.Mars;

                    //Taurus and Libra by Venus;
                    case ZodiacName.Taurus:
                    case ZodiacName.Libra:
                        return PlanetName.Venus;

                    //Gemini and Virgo by Mercury;
                    case ZodiacName.Gemini:
                    case ZodiacName.Virgo:
                        return PlanetName.Mercury;

                    //Cancer by the Moon;
                    case ZodiacName.Cancer:
                        return PlanetName.Moon;

                    //Leo by the Sun ;
                    case ZodiacName.Leo:
                        return PlanetName.Sun;

                    //Sagittarius and Pisces by Jupiter
                    case ZodiacName.Sagittarius:
                    case ZodiacName.Pisces:
                        return PlanetName.Jupiter;

                    //Capricorn and Aquarius by Saturn.
                    case ZodiacName.Capricornus:
                    case ZodiacName.Aquarius:
                        return PlanetName.Saturn;
                    default:
                        throw new Exception("Lord of sign not found, error!");
                }

            }

        }

        /// <summary>
        /// Gets next zodiac sign after input sign
        /// </summary>
        public static ZodiacName GetNextZodiacSign(ZodiacName inputSign)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetNextZodiacSign", inputSign), _getNextZodiacSign);


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
        public static int GetNextHouseNumber(int inputHouseNumber)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetNextHouseNumber", inputHouseNumber), _getNextHouseNumber);


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

        public static ZodiacSign GetPlanetExaltationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetExaltationPoint", planetName), _getPlanetExaltationPoint);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetExaltationPoint()
            {
                //Sun in the 10th degree of Aries;
                if (planetName == PlanetName.Sun)
                {
                    return new ZodiacSign(ZodiacName.Aries, Angle.FromDegrees(10));
                }
                // Moon 3rd of Taurus;
                else if (planetName == PlanetName.Moon)
                {
                    return new ZodiacSign(ZodiacName.Taurus, Angle.FromDegrees(3));
                }

                // Mars 28th of Capricorn ;
                else if (planetName == PlanetName.Mars)
                {
                    return new ZodiacSign(ZodiacName.Capricornus, Angle.FromDegrees(28));
                }

                // Mercury 15th of Virgo;
                else if (planetName == PlanetName.Mercury)
                {
                    return new ZodiacSign(ZodiacName.Virgo, Angle.FromDegrees(15));
                }

                // Jupiter 5th of Cancer;
                else if (planetName == PlanetName.Jupiter)
                {
                    return new ZodiacSign(ZodiacName.Cancer, Angle.FromDegrees(5));
                }

                // Venus 27th of Pisces and
                else if (planetName == PlanetName.Venus)
                {
                    return new ZodiacSign(ZodiacName.Pisces, Angle.FromDegrees(27));
                }

                // Saturn 20th of Libra.
                else if (planetName == PlanetName.Saturn)
                {
                    return new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(20));
                }

                throw new Exception("Planet exaltation point not found, error!");

            }

        }

        public static Angle GetPlanetDebilitationPoint(PlanetName planetName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetDebilitationPoint", planetName), _getPlanetDebilitationPoint);


            //UNDERLYING FUNCTION
            Angle _getPlanetDebilitationPoint()
            {
                //The 7th house or the
                // 180th degree from the place of exaltation is the
                // place of debilitation or fall. The Sun is debilitated-
                // in the 10th degree of Libra, the Moon 3rd
                // of Scorpio and so on.

                if (planetName == PlanetName.Sun)
                {
                    return Angle.FromDegrees(190);
                }
                else if (planetName == PlanetName.Moon)
                {
                    return Angle.FromDegrees(213);
                }
                else if (planetName == PlanetName.Mars)
                {
                    return Angle.FromDegrees(118);
                }
                else if (planetName == PlanetName.Mercury)
                {
                    return Angle.FromDegrees(345);
                }
                else if (planetName == PlanetName.Jupiter)
                {
                    return Angle.FromDegrees(275);
                }
                else if (planetName == PlanetName.Venus)
                {
                    return Angle.FromDegrees(177);
                }
                else if (planetName == PlanetName.Saturn)
                {
                    return Angle.FromDegrees(20);
                }

                throw new Exception("Planet debilitation point not found, error!");

            }


        }

        /// <summary>
        /// Returns true if zodiac sign is an Even sign,  Yugma Rasis
        /// </summary>
        public static bool IsEvenSign(ZodiacName planetSignName)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("IsEvenSign", planetSignName), _isEvenSign);


            //UNDERLYING FUNCTION
            bool _isEvenSign()
            {
                if (planetSignName == ZodiacName.Taurus || planetSignName == ZodiacName.Cancer || planetSignName == ZodiacName.Virgo ||
                    planetSignName == ZodiacName.Scorpio || planetSignName == ZodiacName.Capricornus || planetSignName == ZodiacName.Pisces)
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
            return CacheManager.GetCache(new CacheKey("IsOddSign", planetSignName), _isOddSign);


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

        public static PlanetToPlanetRelationship GetPlanetPermanentRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetPermanentRelationshipWithPlanet", mainPlanet, secondaryPlanet), _getPlanetPermanentRelationshipWithPlanet);


            //UNDERLYING FUNCTION
            PlanetToPlanetRelationship _getPlanetPermanentRelationshipWithPlanet()
            {
                bool planetInEnemies = false;
                bool planetInNeutrals = false;
                bool planetInFriends = false;


                //if main planet is sun
                if (mainPlanet == PlanetName.Sun)
                {
                    //List planets friends, neutrals & enemies
                    var sunFriends = new List<PlanetName>() { PlanetName.Moon, PlanetName.Mars, PlanetName.Jupiter };
                    var sunNeutrals = new List<PlanetName>() { PlanetName.Mercury };
                    var sunEnemies = new List<PlanetName>() { PlanetName.Venus, PlanetName.Saturn };

                    //check if planet is found in any of the lists
                    planetInFriends = sunFriends.Contains(secondaryPlanet);
                    planetInNeutrals = sunNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = sunEnemies.Contains(secondaryPlanet);
                }

                //if main planet is moon
                if (mainPlanet == PlanetName.Moon)
                {
                    //List planets friends, neutrals & enemies
                    var moonFriends = new List<PlanetName>() { PlanetName.Sun, PlanetName.Mercury };
                    var moonNeutrals = new List<PlanetName>() { PlanetName.Mars, PlanetName.Jupiter, PlanetName.Venus, PlanetName.Saturn };
                    var moonEnemies = new List<PlanetName>() { };

                    //check if planet is found in any of the lists
                    planetInFriends = moonFriends.Contains(secondaryPlanet);
                    planetInNeutrals = moonNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = moonEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mars
                if (mainPlanet == PlanetName.Mars)
                {
                    //List planets friends, neutrals & enemies
                    var marsFriends = new List<PlanetName>() { PlanetName.Sun, PlanetName.Moon, PlanetName.Jupiter };
                    var marsNeutrals = new List<PlanetName>() { PlanetName.Venus, PlanetName.Saturn };
                    var marsEnemies = new List<PlanetName>() { PlanetName.Mercury };

                    //check if planet is found in any of the lists
                    planetInFriends = marsFriends.Contains(secondaryPlanet);
                    planetInNeutrals = marsNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = marsEnemies.Contains(secondaryPlanet);

                }

                //if main planet is mercury
                if (mainPlanet == PlanetName.Mercury)
                {
                    //List planets friends, neutrals & enemies
                    var mercuryFriends = new List<PlanetName>() { PlanetName.Sun, PlanetName.Venus };
                    var mercuryNeutrals = new List<PlanetName>() { PlanetName.Mars, PlanetName.Jupiter, PlanetName.Saturn };
                    var mercuryEnemies = new List<PlanetName>() { PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = mercuryFriends.Contains(secondaryPlanet);
                    planetInNeutrals = mercuryNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = mercuryEnemies.Contains(secondaryPlanet);

                }

                //if main planet is jupiter
                if (mainPlanet == PlanetName.Jupiter)
                {
                    //List planets friends, neutrals & enemies
                    var jupiterFriends = new List<PlanetName>() { PlanetName.Sun, PlanetName.Moon, PlanetName.Mars };
                    var jupiterNeutrals = new List<PlanetName>() { PlanetName.Saturn };
                    var jupiterEnemies = new List<PlanetName>() { PlanetName.Mercury, PlanetName.Venus };

                    //check if planet is found in any of the lists
                    planetInFriends = jupiterFriends.Contains(secondaryPlanet);
                    planetInNeutrals = jupiterNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = jupiterEnemies.Contains(secondaryPlanet);

                }

                //if main planet is venus
                if (mainPlanet == PlanetName.Venus)
                {
                    //List planets friends, neutrals & enemies
                    var venusFriends = new List<PlanetName>() { PlanetName.Mercury, PlanetName.Saturn };
                    var venusNeutrals = new List<PlanetName>() { PlanetName.Mars, PlanetName.Jupiter };
                    var venusEnemies = new List<PlanetName>() { PlanetName.Sun, PlanetName.Moon };

                    //check if planet is found in any of the lists
                    planetInFriends = venusFriends.Contains(secondaryPlanet);
                    planetInNeutrals = venusNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = venusEnemies.Contains(secondaryPlanet);

                }

                //if main planet is saturn
                if (mainPlanet == PlanetName.Saturn)
                {
                    //List planets friends, neutrals & enemies
                    var saturnFriends = new List<PlanetName>() { PlanetName.Mercury, PlanetName.Venus };
                    var saturnNeutrals = new List<PlanetName>() { PlanetName.Jupiter };
                    var saturnEnemies = new List<PlanetName>() { PlanetName.Sun, PlanetName.Moon, PlanetName.Mars };

                    //check if planet is found in any of the lists
                    planetInFriends = saturnFriends.Contains(secondaryPlanet);
                    planetInNeutrals = saturnNeutrals.Contains(secondaryPlanet);
                    planetInEnemies = saturnEnemies.Contains(secondaryPlanet);

                }

                //return planet relationship based on where planet is found
                if (planetInFriends)
                {
                    return PlanetToPlanetRelationship.Mitra;
                }
                if (planetInNeutrals)
                {
                    return PlanetToPlanetRelationship.Sama;
                }
                if (planetInEnemies)
                {
                    return PlanetToPlanetRelationship.Satru;
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
            return CacheManager.GetCache(new CacheKey("ConvertJulianTimeToNormalTime", julianTime), _convertJulianTimeToNormalTime);


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
        public static DateTimeOffset GetGreenwichTimeFromJulianDays(double julianTime)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetGreenwichTimeFromJulianDays", julianTime), _convertJulianTimeToNormalTime);


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

        public static double ConvertLmtToJulian(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("ConvertLmtToJulian", time), _convertLmtToJulian);


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
        /// Gets Local mean time (LMT) at Greenwich (UTC) in Julian days based on the inputed time
        /// </summary>
        public static double GetGreenwichLmtInJulianDays(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetGreenwichLmtInJulianDays", time), _getGreenwichLmtInJulianDays);


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

        public static double[] GetHouse1And10Longitudes(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetHouse1And10Longitudes", time), _getHouse1And10Longitudes);


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

                swissEph.swe_houses(jul_day_UT, location.GetLatitude(), location.GetLongitude(), 'P', cusps, ascmc);

                //we only return cusps, cause that is what is used for now
                return cusps;
            }

        }

        public static DateTimeOffset LmtToUtc(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("LmtToUtc", time), _lmtToUtc);


            //UNDERLYING FUNCTION
            DateTimeOffset _lmtToUtc()
            {
                return time.GetLmtDateTimeOffset().ToUniversalTime();
            }
        }

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        public static Angle TimeToLongitude(TimeSpan time)
        {
            //degrees is equivelant to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }



        //NORMAL FUNCTIONS
        //FUNCTIONS THAT CALL OTHER FUNCTIONS IN THIS CLASS

        /// <summary>
        /// Gets the ephemris time that is consumed by Swiss Ephemeris
        /// </summary>
        public static double TimeToEphemerisTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("TimeToEphemerisTime", time), _timeToEphemerisTime);


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

        public static Angle GetPlanetNirayanaLongitude(Time time, PlanetName planetName)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetNirayanaLongitude", time, planetName), _getPlanetNirayanaLongitude);


            //UNDERLYING FUNCTION
            Angle _getPlanetNirayanaLongitude()
            {
                //declare return value
                Angle returnValue;


                //Get sayana longitude on day 
                Angle longitude = GetPlanetSayanaLongitude(time, planetName);


                //3 - Hindu Nirayana Long = Sayana Long — Ayanamsa.

                Angle birthAyanamsa = GetAyanamsa(time);

                if (longitude.TotalDegrees < birthAyanamsa.TotalDegrees)
                    returnValue = (longitude + Angle.Degrees360) - birthAyanamsa;
                else
                    returnValue = longitude - birthAyanamsa;

                //Calculates Kethu with inital values from Rahu
                if ((PlanetName)planetName == PlanetName.Ketu)
                    returnValue -= Angle.Degrees180;

                return returnValue;
            }


        }

        public static LunarDay GetLunarDay(Time time)
        {
            //get position of sun & moon
            Angle sunLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Sun);
            Angle moonLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Moon);

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

        public static PlanetConstellation GetMoonConstellation(Time time)
        {
            //get position of moon in longitude
            Angle moonLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Moon);

            //return the constellation behind the moon
            return GetPlanetConstellation(moonLongitude);
        }

        public static Tarabala GetTarabala(Time time, Person person)
        {
            int dayRulingConstellationNumber = GetMoonConstellation(time).GetConstellationNumber();

            int birthRulingConstellationNumber = GetMoonConstellation(person.GetBirthDateTime()).GetConstellationNumber();

            int counter = 0;

            int cycle;


            //Need to count from birthRulingConstellationNumber to dayRulingConstellationNumber

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
        /// Chandrabala or lunar strength
        ///
        /// Reference:
        /// Chandrabala. - As we have already said above, the consideration of the
        /// Moon and his position are of much importance in Muhurtha. To be at its
        /// best, the Moon should not occupy in the election chart, a position that
        /// happens to represent the 6th, 8th or 12th from the person's Janma Rasi.
        /// </summary>
        public static int GetChandrabala(Time time, Person person)
        {
            //initialize chandrabala number as 0
            int chandrabalaNumber = 0;

            //get zodiac name & convert to its number
            var dayMoonSignNumber = (int)GetMoonSignName(time);
            var birthMoonSignNumber = (int)GetMoonSignName(person.GetBirthDateTime());


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

        public static ZodiacName GetMoonSignName(Time time)
        {
            //get zodiac sign behind the moon
            var moonSign = GetPlanetRasiSign(PlanetName.Moon, time);

            //return name of zodiac sign
            return moonSign.GetSignName();
        }

        public static NithyaYoga GetNithyaYoga(Time time)
        {
            //Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')

            //get position of sun & moon in longitude
            Angle sunLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Sun);
            Angle moonLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Moon);

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

        public static Karana GetKarana(Time time)
        {
            //declare karana as empty first
            Karana? karanaToReturn = null;

            //get position of sun & moon
            Angle sunLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Sun);
            Angle moonLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Moon);

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
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Kimstughna : Karana.Bava;
                    break;
                case 23:
                case 16:
                case 9:
                case 2:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Balava : Karana.Kaulava;
                    break;
                case 24:
                case 17:
                case 10:
                case 3:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Taitula : Karana.Garija;
                    break;
                case 25:
                case 18:
                case 11:
                case 4:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Vanija : Karana.Visti;
                    break;
                case 26:
                case 19:
                case 12:
                case 5:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Bava : Karana.Balava;
                    break;
                case 27:
                case 20:
                case 13:
                case 6:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Kaulava : Karana.Taitula;
                    break;
                case 28:
                case 21:
                case 14:
                case 7:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Garija : Karana.Vanija;
                    break;
                case 22:
                case 15:
                case 8:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Visti : Karana.Bava;
                    break;
                case 29:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Visti : Karana.Sakuna;
                    break;
                case 30:
                    karanaToReturn = lunarDayAlreadyTraversed <= 0.5 ? Karana.Chatushpada : Karana.Naga;
                    break;

            }

            //if karana not found throw error
            if (karanaToReturn == null)
            {
                throw new Exception("Karana could not be found!");
            }

            return (Karana)karanaToReturn;
        }

        public static ZodiacSign GetSunSign(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetSunSign", time), _getSunSign);


            //UNDERLYING FUNCTION
            ZodiacSign _getSunSign()
            {
                //get zodiac sign behind the sun
                var sunSign = GetPlanetRasiSign(PlanetName.Sun, time);

                //return zodiac sign behind sun
                return sunSign;

            }

        }

        ///<summary>
        ///Find time when Sun was in 0.001 degrees
        ///in current sign (just entered sign)
        ///</summary>
        public static Time GetTimeSunEnteredCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetTimeSunEnteredCurrentSign", time), _getTimeSunEnteredCurrentSign);


            //UNDERLYING FUNCTION
            Time _getTimeSunEnteredCurrentSign()
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
                var currentSunSign = AstronomicalCalculator.GetSunSign(time);

                //if entered time not yet found
                while (true)
                {
                    //get the sign at possible entered time
                    var possibleSunSign = AstronomicalCalculator.GetSunSign(possibleEnteredTime);

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
        ///     1. degrees Sun is in sign is more than 29.999 degress (very very close to leaving sign)
        ///     2. accuracy limit is hit
        ///</summary>
        public static Time GetTimeSunLeavesCurrentSign(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetTimeSunLeavesCurrentSign", time), _getTimeSunLeavesCurrentSign);


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
                var currentSunSign = AstronomicalCalculator.GetSunSign(time);

                //find leaving time
                while (true)
                {
                    //get the sign at possible leaving time
                    var possibleSunSign = AstronomicalCalculator.GetSunSign(possibleLeavingTime);

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

        public static List<House> GetHouses(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetHouses", time), _getHouses);


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
                var ayanamsa = GetAyanamsa(time);

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
                house1MiddleLongitude = udayaLagna;
                house4MiddleLongitude = pathalaLagna;
                house7MiddleLongitude = astaLagna;
                house10MiddleLongitude = madhyaLagna;

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

                house1EndLongitude = house2BeginLongitude = GetHouseJunctionPoint(house1MiddleLongitude, house2MiddleLongitude);
                house2EndLongitude = house3BeginLongitude = GetHouseJunctionPoint(house2MiddleLongitude, house3MiddleLongitude);
                house3EndLongitude = house4BeginLongitude = GetHouseJunctionPoint(house3MiddleLongitude, house4MiddleLongitude);
                house4EndLongitude = house5BeginLongitude = GetHouseJunctionPoint(house4MiddleLongitude, house5MiddleLongitude);
                house5EndLongitude = house6BeginLongitude = GetHouseJunctionPoint(house5MiddleLongitude, house6MiddleLongitude);
                house6EndLongitude = house7BeginLongitude = GetHouseJunctionPoint(house6MiddleLongitude, house7MiddleLongitude);
                house7EndLongitude = house8BeginLongitude = GetHouseJunctionPoint(house7MiddleLongitude, house8MiddleLongitude);
                house8EndLongitude = house9BeginLongitude = GetHouseJunctionPoint(house8MiddleLongitude, house9MiddleLongitude);
                house9EndLongitude = house10BeginLongitude = GetHouseJunctionPoint(house9MiddleLongitude, house10MiddleLongitude);
                house10EndLongitude = house11BeginLongitude = GetHouseJunctionPoint(house10MiddleLongitude, house11MiddleLongitude);
                house11EndLongitude = house12BeginLongitude = GetHouseJunctionPoint(house11MiddleLongitude, house12MiddleLongitude);
                house12EndLongitude = house1BeginLongitude = GetHouseJunctionPoint(house12MiddleLongitude, house1MiddleLongitude);

                //4.0 Initialize houses into list
                var houseList = new List<House>();

                houseList.Add(new House(1, house1BeginLongitude, house1MiddleLongitude, house1EndLongitude));
                houseList.Add(new House(2, house2BeginLongitude, house2MiddleLongitude, house2EndLongitude));
                houseList.Add(new House(3, house3BeginLongitude, house3MiddleLongitude, house3EndLongitude));
                houseList.Add(new House(4, house4BeginLongitude, house4MiddleLongitude, house4EndLongitude));
                houseList.Add(new House(5, house5BeginLongitude, house5MiddleLongitude, house5EndLongitude));
                houseList.Add(new House(6, house6BeginLongitude, house6MiddleLongitude, house6EndLongitude));
                houseList.Add(new House(7, house7BeginLongitude, house7MiddleLongitude, house7EndLongitude));
                houseList.Add(new House(8, house8BeginLongitude, house8MiddleLongitude, house8EndLongitude));
                houseList.Add(new House(9, house9BeginLongitude, house9MiddleLongitude, house9EndLongitude));
                houseList.Add(new House(10, house10BeginLongitude, house10MiddleLongitude, house10EndLongitude));
                houseList.Add(new House(11, house11BeginLongitude, house11MiddleLongitude, house11EndLongitude));
                houseList.Add(new House(12, house12BeginLongitude, house12MiddleLongitude, house12EndLongitude));


                return houseList;

            }



        }

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
        public static List<PlanetName> GetPlanetsInConjuction(Time time, PlanetName inputedPlanetName)
        {
            //set 8° degrees as max space around planet where conjunction occurs
            var conjunctionOrbMax = new Angle(8, 0, 0);

            //get longitude of inputed planet
            var inputedPlanet = GetPlanetNirayanaLongitude(time, inputedPlanetName);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = GetAllPlanetLongitude(time);

            //a place to store conjunct planets 
            var conjunctPlanets = new List<PlanetName>();

            //loop through each planet
            foreach (var planet in allPlanetLongitudeList)
            {
                //skip the inputed planet
                if (planet.GetPlanetName() == inputedPlanetName) { continue; }

                //get the space between the planet in longitude
                var spaceBetween = GetDistanceBetweenPlanets(inputedPlanet, planet.GetPlanetLongitude());

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
        public static Angle GetDistanceBetweenPlanets(PlanetName planet1, PlanetName planet2, Time time)
        {
            var planet1Longitude = GetPlanetNirayanaLongitude(time, planet1);
            var planet2Longitude = GetPlanetNirayanaLongitude(time, planet2);

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
        /// <summary>
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for
        /// - Expects you to calculate longitude
        /// </summary>
        public static Angle GetDistanceBetweenPlanets(Angle planet1, Angle planet2)
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
        /// Gets the planets in the house
        /// </summary>
        public static List<PlanetName> GetPlanetsInHouse(int houseNumber, Time time)
        {

            //declare empty list of planets
            var listOfPlanetInHouse = new List<PlanetName>();

            //get all houses
            var houseList = GetHouses(time);

            //find house that matches input house number
            var house = houseList.Find(h => h.GetHouseNumber() == houseNumber);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = GetAllPlanetLongitude(time);

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
        /// Gets longitude positions of all planets
        /// </summary>
        public static List<PlanetLongitude> GetAllPlanetLongitude(Time time)
        {

            //get longitudes of all planets
            var sunLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Sun);
            var sun = new PlanetLongitude(PlanetName.Sun, sunLongitude);

            var moonLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Moon);
            var moon = new PlanetLongitude(PlanetName.Moon, moonLongitude);

            var marsLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Mars);
            var mars = new PlanetLongitude(PlanetName.Mars, marsLongitude);

            var mercuryLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Mercury);
            var mercury = new PlanetLongitude(PlanetName.Mercury, mercuryLongitude);

            var jupiterLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Jupiter);
            var jupiter = new PlanetLongitude(PlanetName.Jupiter, jupiterLongitude);

            var venusLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Venus);
            var venus = new PlanetLongitude(PlanetName.Venus, venusLongitude);

            var saturnLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Saturn);
            var saturn = new PlanetLongitude(PlanetName.Saturn, saturnLongitude);

            var rahuLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Rahu);
            var rahu = new PlanetLongitude(PlanetName.Rahu, rahuLongitude);

            var ketuLongitude = GetPlanetNirayanaLongitude(time, PlanetName.Ketu);
            var ketu = new PlanetLongitude(PlanetName.Ketu, ketuLongitude);



            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        public static int GetHousePlanetIsIn(Time time, PlanetName planetName)
        {

            //get the planets longitude
            var planetLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, planetName);

            //get all houses
            var houseList = AstronomicalCalculator.GetHouses(time);

            //loop through all houses
            foreach (var house in houseList)
            {
                //check if planet is in house's range
                var planetIsInHouse = house.IsLongitudeInHouseRange(planetLongitude);

                //if planet is in house
                if (planetIsInHouse)
                {
                    //return house's number
                    return house.GetHouseNumber();
                }
            }

            //if planet not found in any house, raise error
            throw new Exception("Planet not in any house, error!");

        }

        /// <summary>
        /// The lord of a bhava is
        /// the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// </summary>
        public static PlanetName GetLordOfHouse(HouseName houseNumber, Time time)
        {
            //get sign name based on house number //TODO Change to use house name instead of casting to int
            var houseSignName = AstronomicalCalculator.GetHouseSignName((int)houseNumber, time);

            //get the lord of the house sign
            var lordOfHouseSign = AstronomicalCalculator.GetLordOfZodiacSign(houseSignName);

            return lordOfHouseSign;
        }

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        public static ZodiacName GetHouseSignName(int houseNumber, Time time)
        {

            //get all houses
            var allHouses = AstronomicalCalculator.GetHouses(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseNumber() == houseNumber);

            //get sign of the specified house
            var houseSign = AstronomicalCalculator.GetZodiacSignAtLongitude(specifiedHouse.GetMiddleLongitude());

            //return the name of house sign
            return houseSign.GetSignName();
        }

        public static ZodiacName GetNavamsaSignNameFromLongitude(Angle longitude)
        {
            //1.0 Get ordinary zodiac sign name
            //get ordinary zodiac sign
            var ordinarySign = AstronomicalCalculator.GetZodiacSignAtLongitude(longitude);

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
                //Taurus, Capricornus, Virgo - from Capricornus.
                case ZodiacName.Taurus:
                case ZodiacName.Capricornus:
                case ZodiacName.Virgo:
                    firstNavamsa = ZodiacName.Capricornus;
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
            ZodiacName signAtNavamsa = AstronomicalCalculator.GetSignCountedFromInputSign(firstNavamsa, navamsaNumber);

            return signAtNavamsa;

        }

        /// <summary>
        /// Exp : Get 4th sign from Cancer
        /// </summary>
        public static ZodiacName GetSignCountedFromInputSign(ZodiacName inputSign, int countToNextSign)
        {
            //assign counted to same as starting sign at first
            ZodiacName signCountedTo = inputSign;

            //get the next sign the same number as the count to next sign
            for (int i = 1; i < countToNextSign; i++)
            {
                //get the next zodiac sign from the current counted to sign
                signCountedTo = GetNextZodiacSign(signCountedTo);
            }

            return signCountedTo;

        }

        /// <summary>
        /// Exp : Get 4th house from 5th house (input house)
        /// </summary>
        public static int GetHouseCountedFromInputHouse(int inputHouseNumber, int countToNextHouse)
        {
            //assign count to same as starting house at first
            int houseCountedTo = inputHouseNumber;

            //get the next house the same number as the count to next house
            for (int i = 1; i < countToNextHouse; i++)
            {
                //get the next house number from the current counted to house
                houseCountedTo = GetNextHouseNumber(houseCountedTo);
            }

            return houseCountedTo;

        }

        /// <summary>
        /// Get zodiac sign planet is in.
        /// </summary>
        public static ZodiacSign GetPlanetRasiSign(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetRasiSign", planetName, time), _getPlanetRasiSign);


            //UNDERLYING FUNCTION
            ZodiacSign _getPlanetRasiSign()
            {
                //get longitude of planet
                var longitudeOfPlanet = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, planetName);

                //get sign planet is in
                var signPlanetIsIn = AstronomicalCalculator.GetZodiacSignAtLongitude(longitudeOfPlanet);

                //return
                return signPlanetIsIn;

            }

        }

        /// <summary>
        /// Get navamsa sign of planet
        /// </summary>
        public static ZodiacName GetPlanetNavamsaSign(PlanetName planetName, Time time)
        {
            //get planets longitude
            var planetLongitude = GetPlanetNirayanaLongitude(time, planetName);

            //get navamsa sign at longitude
            var navamsaSignOfPlanet = AstronomicalCalculator.GetNavamsaSignNameFromLongitude(planetLongitude);

            return navamsaSignOfPlanet;
        }


        /// <summary>
        /// All their location with a quarter sight, the 5th and the
        /// 9th houses with a half sight, the 4th and the 8th houses
        /// with three-quarters of a sight and the 7th house with
        /// a full sight.
        /// 
        /// </summary>
        public static List<ZodiacName> GetSignsPlanetIsAspecting(PlanetName planetName, Time time)
        {

            //create empty list of signs
            var planetSignList = new List<ZodiacName>();

            //get zodiac sign name which the planet is currently in
            var planetSignName = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

            // Saturn powerfully aspects the 3rd and the 10th houses
            if (planetName == PlanetName.Saturn)
            {
                //get signs planet is aspecting
                var sign3FromSaturn = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 3);
                var sign10FromSaturn = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 10);

                //add signs to return list
                planetSignList.Add(sign3FromSaturn);
                planetSignList.Add(sign10FromSaturn);

            }

            // Jupiter the 5th and the 9th houses
            if (planetName == PlanetName.Jupiter)
            {
                //get signs planet is aspecting
                var sign5FromJupiter = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 5);
                var sign9FromJupiter = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 9);

                //add signs to return list
                planetSignList.Add(sign5FromJupiter);
                planetSignList.Add(sign9FromJupiter);

            }

            // Mars, the 4th and the 8th houses
            if (planetName == PlanetName.Mars)
            {
                //get signs planet is aspecting
                var sign4FromMars = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 4);
                var sign8FromMars = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 8);

                //add signs to return list
                planetSignList.Add(sign4FromMars);
                planetSignList.Add(sign8FromMars);

            }

            //All planets aspect 7th house

            //get signs planet is aspecting
            var sign7FromPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 7);

            //add signs to return list
            planetSignList.Add(sign7FromPlanet);


            return planetSignList;

        }

        /// <summary>
        /// Get navamsa sign of house (mid point)
        /// TODO: Checking for correctness needed
        /// </summary>
        public static ZodiacName GetHouseNavamsaSign(HouseName house, Time time)
        {
            //get all houses
            var allHouseList = AstronomicalCalculator.GetHouses(time);

            //get house mid longitude
            var houseMiddleLongitude = allHouseList.Find(hs => hs.GetHouseNumber() == (int)house).GetMiddleLongitude();

            //get navamsa house sign at house mid longitude
            var navamsaSign = AstronomicalCalculator.GetNavamsaSignNameFromLongitude(houseMiddleLongitude);

            return navamsaSign;
        }

        public static ZodiacName GetPlanetThrimsamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

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
            if (AstronomicalCalculator.IsOddSign(planetSignName))
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
                    return ZodiacName.Capricornus;

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
            if (AstronomicalCalculator.IsEvenSign(planetSignName))
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
                    return ZodiacName.Capricornus;

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

        public static ZodiacName GetPlanetDwadasamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare const number for saptamsa calculation
            const double maxDwadasamsaDegrees = 2.5; // 30/12
            const double maxSignDegrees = 30.0;

            //get rough Dwadasamsa number
            double roughDwadasamsaNumber = (degreesInSign % maxSignDegrees) / maxDwadasamsaDegrees;

            //get rounded Dwadasamsa number
            var dwadasamsaNumber = (int)Math.Ceiling(roughDwadasamsaNumber);

            //get Dwadasamsa sign from counting with Dwadasamsa number
            var dwadasamsaSign = GetSignCountedFromInputSign(planetSignName, dwadasamsaNumber);

            return dwadasamsaSign;
        }

        public static ZodiacName GetPlanetSaptamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

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
                return AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, saptamsaNumber);
            }

            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                var countToNextSign = saptamsaNumber + 6;
                return AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, countToNextSign);
            }


            throw new Exception("Saptamsa not found, error!");
        }

        public static ZodiacName GetPlanetDrekkanaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

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
                return AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 5);
            }

            //if planet is in 3rd drekkana
            if (degreesInSign > 20 && degreesInSign <= 30)
            {
                //return 9th sign from planets current sign
                return AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 9);
            }

            throw new Exception("Planet drekkana not found, error!");
        }

        public static bool IsPlanetInMoolatrikona(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

            //Sun's Moola Thrikona is Leo (0°-20°);
            if (planetName == PlanetName.Sun)
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
            if (planetName == PlanetName.Moon)
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
            if (planetName == PlanetName.Mercury)
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
            if (planetName == PlanetName.Jupiter)
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
            if (planetName == PlanetName.Mars)
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
            if (planetName == PlanetName.Venus)
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
            if (planetName == PlanetName.Saturn)
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

        public static ZodiacName GetPlanetHoraSign(PlanetName planetName, Time time)
        {
            //get planet sign
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

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
        /// Gets a planet's relationship to a sign, based on the relation to the lord
        /// </summary>
        public static PlanetToSignRelationship GetPlanetRelationshipWithSign(PlanetName planetName, ZodiacName zodiacSignName, Time time)
        {
            //types of relationship
            //Swavarga - own varga
            //Samavarga - neutral's varga
            //Mitravarga - friendly varga
            //Adhi Mitravarga - Intimate friend varga
            //Satruvarga - enemy's varga
            //Adhi Satruvarga - Bitter enemy varga

            //Get lord of zodiac sign
            var lordOfSign = AstronomicalCalculator.GetLordOfZodiacSign(zodiacSignName);

            //if lord of sign is same as input planet
            if (planetName == lordOfSign)
            {
                //return own varga, swavarga
                return PlanetToSignRelationship.Swavarga;
            }

            //else, get relationship between input planet and lord of sign
            PlanetToPlanetRelationship relationshipToLordOfSign =
                AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(planetName, lordOfSign, time);

            //return relation ship with sign based on relationship with lord of sign
            switch (relationshipToLordOfSign)
            {
                case PlanetToPlanetRelationship.AdhiMitra:
                    return PlanetToSignRelationship.AdhiMitravarga;
                case PlanetToPlanetRelationship.Mitra:
                    return PlanetToSignRelationship.Mitravarga;
                case PlanetToPlanetRelationship.AdhiSatru:
                    return PlanetToSignRelationship.AdhiSatruvarga;
                case PlanetToPlanetRelationship.Satru:
                    return PlanetToSignRelationship.Satruvarga;
                case PlanetToPlanetRelationship.Sama:
                    return PlanetToSignRelationship.Samavarga;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// In order to find the strengths of planets we have
        /// to mix the temporary relations and the permanent
        /// relations. Thus a temporary enemy plus a permanent
        /// or natural enemy becomes a bitter enemy.
        /// </summary>
        public static PlanetToPlanetRelationship GetPlanetCombinedRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {
            //get planet's permanent relationship
            PlanetToPlanetRelationship planetPermanentRelationship =
                AstronomicalCalculator.GetPlanetPermanentRelationshipWithPlanet(mainPlanet, secondaryPlanet);

            //get planet's temporary relationship
            PlanetToPlanetRelationship planetTemporaryRelationship =
                AstronomicalCalculator.GetPlanetTemporaryRelationshipWithPlanet(mainPlanet, secondaryPlanet, time);

            //Tatkalika Mitra + Naisargika Mitra = Adhi Mitras
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Mitra && planetPermanentRelationship == PlanetToPlanetRelationship.Mitra)
            {
                //they both become intimate friends (Adhi Mitras).
                return PlanetToPlanetRelationship.AdhiMitra;
            }

            //Tatkalika Mitra + Naisargika Satru = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Mitra && planetPermanentRelationship == PlanetToPlanetRelationship.Satru)
            {
                return PlanetToPlanetRelationship.Sama;
            }

            //Tatkalika Mitra + Naisargika Sama = Mitra
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Mitra && planetPermanentRelationship == PlanetToPlanetRelationship.Sama)
            {
                return PlanetToPlanetRelationship.Mitra;
            }

            //Tatkalika Satru + Naisargika Satru = Adhi Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Satru && planetPermanentRelationship == PlanetToPlanetRelationship.Satru)
            {
                return PlanetToPlanetRelationship.AdhiSatru;
            }

            //Tatkalika Satru + Naisargika Mitra = Sama
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Satru && planetPermanentRelationship == PlanetToPlanetRelationship.Mitra)
            {
                return PlanetToPlanetRelationship.Sama;
            }

            //Tatkalika Satru + Naisargika Sama = Satru
            if (planetTemporaryRelationship == PlanetToPlanetRelationship.Satru && planetPermanentRelationship == PlanetToPlanetRelationship.Sama)
            {
                return PlanetToPlanetRelationship.Satru;
            }

            throw new Exception("Combined planet relationship not found, error!");
        }

        /// <summary>
        /// Gets a planets relationship with a house,
        /// Based on the relation between the planet and the lord of the sign of the house
        /// Note : needs verification if this is correct
        /// </summary>
        public static PlanetToSignRelationship GetPlanetRelationshipWithHouse(HouseName house, PlanetName planet, Time time)
        {
            //get sign the house is in
            var houseSign = GetHouseSignName((int)house, time);

            //get the planet's relationship with the sign
            var relationship = GetPlanetRelationshipWithSign(planet, houseSign, time);

            return relationship;
        }

        public static PlanetToPlanetRelationship GetPlanetTemporaryRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {

            //1.0 get planet's friends
            var friendlyPlanetList = AstronomicalCalculator.GetPlanetTemporaryFriendList(mainPlanet, time);

            //check if planet is found in friend list
            var planetFoundInFriendList = friendlyPlanetList.Contains(secondaryPlanet);

            //if found in friend list
            if (planetFoundInFriendList)
            {
                //return relationship as friend
                return PlanetToPlanetRelationship.Mitra;
            }

            //2.0 get planet's enemies
            var enemyPlanetList = AstronomicalCalculator.GetPlanetTemporaryEnemyList(mainPlanet, time);

            //check if planet is found in enemy list
            var planetFoundInEnemyList = enemyPlanetList.Contains(secondaryPlanet);

            //if found in enemy list
            if (planetFoundInEnemyList)
            {
                //return relationship as enemy
                return PlanetToPlanetRelationship.Satru;
            }


            throw new Exception("Temporary planet relationship not found, error!");
        }

        //public static List<PlanetName> GetPlanetTemporaryEnemyList(PlanetName planetName, Time time)
        //{
        //    //Enemy houses 1,5,6,7,8,9

        //    //get house planet is currently in
        //    var mainPlanetHouseNumber = AstronomicalCalculator.GetHousePlanetIsIn(time, planetName);

        //    //Get houses of enemies of main planet
        //    //get planets in 1
        //    var house1FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 1);
        //    //get planets in 5
        //    var house5FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 5);
        //    //get planets in 6
        //    var house6FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 6);
        //    //get planets in 7
        //    var house7FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 7);
        //    //get planets in 8
        //    var house8FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 8);
        //    //get planets in 9
        //    var house9FromMainPlanet = AstronomicalCalculator.GetHouseCountedFromInputHouse(mainPlanetHouseNumber, 9);

        //    //add houses of friendly planets to a list
        //    var housesOfEnemyPlanet = new List<int>(){house1FromMainPlanet, house5FromMainPlanet, house6FromMainPlanet,
        //                                                house7FromMainPlanet, house8FromMainPlanet, house9FromMainPlanet};

        //    //declare list of enemy planets
        //    var enemyPlanetList = new List<PlanetName>();

        //    //loop through the houses and fill the enemy planet list
        //    foreach (var house in housesOfEnemyPlanet)
        //    {
        //        //get the planets in the current house
        //        var enemyPlanetsInThisHouse = AstronomicalCalculator.GetPlanetsInHouse(house, time);

        //        //add the planets in to the list
        //        enemyPlanetList.AddRange(enemyPlanetsInThisHouse);
        //    }

        //    //remove rahu & ketu from list
        //    enemyPlanetList.Remove(PlanetName.Rahu);
        //    enemyPlanetList.Remove(PlanetName.Ketu);


        //    return enemyPlanetList;

        //}

        public static List<PlanetName> GetPlanetTemporaryEnemyList(PlanetName planetName, Time time)
        {
            //Signs where enemy planets are located 1,5,6,7,8,9

            //get sign planet is currently in
            var planetSignName = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

            //Get signs of enemies of main planet
            //get planets in 1
            var sign1FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 1);
            //get planets in 5
            var sign5FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 5);
            //get planets in 6
            var sign6FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 6);
            //get planets in 7
            var sign7FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 7);
            //get planets in 8
            var sign8FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 8);
            //get planets in 9
            var sign9FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 9);

            //add signs of enemy planets to a list
            var signsOfEnemyPlanet = new List<ZodiacName>(){sign1FromMainPlanet, sign5FromMainPlanet, sign6FromMainPlanet,
                                                        sign7FromMainPlanet, sign8FromMainPlanet, sign9FromMainPlanet};

            //declare list of enemy planets
            var enemyPlanetList = new List<PlanetName>();

            //loop through the signs and fill the enemy planet list
            foreach (var sign in signsOfEnemyPlanet)
            {
                //get the planets in the current sign
                var enemyPlanetsInThisSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

                //add the planets in to the list
                enemyPlanetList.AddRange(enemyPlanetsInThisSign);
            }

            //remove rahu & ketu from list
            enemyPlanetList.Remove(PlanetName.Rahu);
            enemyPlanetList.Remove(PlanetName.Ketu);

            //remove the main planet from list
            enemyPlanetList.Remove(planetName);


            return enemyPlanetList;

        }

        public static List<PlanetName> GetPlanetInSign(ZodiacName signName, Time time)
        {
            //get all planets locations in signs
            var sunSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, time).GetSignName();
            var moonSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, time).GetSignName();
            var marsSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, time).GetSignName();
            var mercurySignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, time).GetSignName();
            var jupiterSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, time).GetSignName();
            var venusSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, time).GetSignName();
            var saturnSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, time).GetSignName();
            var rahuSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, time).GetSignName();
            var ketuSignName = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, time).GetSignName();


            //create empty list of planet names to return
            var planetFoundInSign = new List<PlanetName>();

            //if planet is in same sign as input sign add planet to list
            if (sunSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Sun);
            }
            if (moonSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Moon);
            }
            if (marsSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Mars);
            }
            if (mercurySignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Mercury);
            }
            if (jupiterSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Jupiter);
            }
            if (venusSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Venus);
            }
            if (saturnSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Saturn);
            }
            if (rahuSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Rahu);
            }
            if (ketuSignName == signName)
            {
                planetFoundInSign.Add(PlanetName.Ketu);
            }


            return planetFoundInSign;
        }

        public static List<PlanetName> GetPlanetTemporaryFriendList(PlanetName planetName, Time time)
        {
            //The planets in -the 2nd, 3rd, 4th, 10th, 11th and
            // 12th signs from any other planet becomes his
            // (Tatkalika) friend.

            //get sign planet is currently in
            var planetSignName = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

            //Get signs of friends of main planet
            //get planets in 2nd
            var sign2FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 2);
            //get planets in 3rd
            var sign3FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 3);
            //get planets in 4th
            var sign4FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 4);
            //get planets in 10th
            var sign10FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 10);
            //get planets in 11th
            var sign11FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 11);
            //get planets in 12th
            var sign12FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 12);

            //add houses of friendly planets to a list
            var signsOfFriendlyPlanet = new List<ZodiacName>(){sign2FromMainPlanet, sign3FromMainPlanet, sign4FromMainPlanet,
                                                        sign10FromMainPlanet, sign11FromMainPlanet, sign12FromMainPlanet};

            //declare list of friendly planets
            var friendlyPlanetList = new List<PlanetName>();

            //loop through the signs and fill the friendly planet list
            foreach (var sign in signsOfFriendlyPlanet)
            {
                //get the planets in the current sign
                var friendlyPlanetsInThisSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

                //add the planets in to the list
                friendlyPlanetList.AddRange(friendlyPlanetsInThisSign);
            }

            //remove rahu & ketu from list
            friendlyPlanetList.Remove(PlanetName.Rahu);
            friendlyPlanetList.Remove(PlanetName.Ketu);


            return friendlyPlanetList;

        }

        public static double GetGreenwichApparentInJulianDays(Time time)
        {
            //convert lmt to julian days, in universal time (UT)
            var localMeanTimeInJulian_UT = GetGreenwichLmtInJulianDays(time);

            //get longitude of location
            double longitude = time.GetGeoLocation().GetLongitude();

            //delcare output variables
            double localApparentTimeInJulian;
            string errorString = "";

            //convert lmt to local apparent time (LAT)
            using SwissEph ephemeris = new();
            ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);


            return localApparentTimeInJulian;
        }

        public static DateTime GetLocalApparentTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetLocalApparentTime", time), _getLocalApparentTime);


            //UNDERLYING FUNCTION
            DateTime _getLocalApparentTime()
            {
                //convert lmt to julian days, in universal time (UT)
                var localMeanTimeInJulian_UT = ConvertLmtToJulian(time);

                //get longitude of location
                double longitude = time.GetGeoLocation().GetLongitude();

                //delcare output variables
                double localApparentTimeInJulian;
                string errorString = null;

                //initialize ephemeris
                SwissEph ephemeris = new SwissEph();

                //convert lmt to local apparent time (LAT)
                ephemeris.swe_lmt_to_lat(localMeanTimeInJulian_UT, longitude, out localApparentTimeInJulian, ref errorString);

                var localApparentTime = AstronomicalCalculator.ConvertJulianTimeToNormalTime(localApparentTimeInJulian);

                return localApparentTime;

            }

        }

        /// <summary>
        /// This method exists mainly for testing internal time calculation of LMT
        /// Important that this method passes the test at all times, so much depends on this
        /// </summary>
        public static DateTimeOffset GetLocalMeanTime(Time time) => time.GetLmtDateTimeOffset();

        public static House GetHouse(HouseName houseNumber, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetHouse", houseNumber, time), _getHouse);


            //UNDERLYING FUNCTION
            House _getHouse()
            {
                //get all house list
                var allHouses = GetHouses(time);

                //get required house from list
                var requiredHouse = allHouses.Find(h => h.GetHouseNumber() == (int)houseNumber);

                return requiredHouse;

            }

        }

        public static PanchakaName GetPanchaka(Time time)
        {
            //If the remainder is 1 (mrityu panchakam), it
            // indicates danger; if 2 (agni panchakam), risk from fire; if 4 (raja
            // panchakam), bad results; if 6 (chora panchakam), evil happenings and if
            // 8 (roga panchakam), disease. If the remainder is 3, 5, 7 or zero then it is
            // good.

            //get the number of the lunar day (from the 1st of the month),
            var lunarDateNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDateNumber();

            //get the number of the constellation (from Aswini)
            var rullingConstellationNumber = AstronomicalCalculator.GetMoonConstellation(time).GetConstellationNumber();

            //Number of weekday
            var weekdayNumber = (int)AstronomicalCalculator.GetDayOfWeek(time);

            //Number of zodiacal sign, number of the Lagna (from Aries).
            var risingSignNumber = (int)AstronomicalCalculator.GetHouseSignName(1, time);

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

        public static PlanetName GetLordOfWeekday(Time time)
        {
            //Sunday Sun
            //Monday Moon
            //Tuesday Mars
            //Wednesday Mercury
            //Thursday Jupiter
            //Friday Venus
            //Saturday Saturn


            //get the weekday
            var weekday = GetDayOfWeek(time);

            //based on weekday return the planet lord
            switch (weekday)
            {
                case DayOfWeek.Sunday: return PlanetName.Sun;
                case DayOfWeek.Monday: return PlanetName.Moon;
                case DayOfWeek.Tuesday: return PlanetName.Mars;
                case DayOfWeek.Wednesday: return PlanetName.Mercury;
                case DayOfWeek.Thursday: return PlanetName.Jupiter;
                case DayOfWeek.Friday: return PlanetName.Venus;
                case DayOfWeek.Saturday: return PlanetName.Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static PlanetName GetLordOfWeekday(DayOfWeek weekday)
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
                case DayOfWeek.Sunday: return PlanetName.Sun;
                case DayOfWeek.Monday: return PlanetName.Moon;
                case DayOfWeek.Tuesday: return PlanetName.Mars;
                case DayOfWeek.Wednesday: return PlanetName.Mercury;
                case DayOfWeek.Thursday: return PlanetName.Jupiter;
                case DayOfWeek.Friday: return PlanetName.Venus;
                case DayOfWeek.Saturday: return PlanetName.Saturn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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
        public static int GetHoraAtBirth(Time time)
        {
            TimeSpan hours;

            var birthTime = time.GetLmtDateTimeOffset();
            var sunriseTime = GetSunriseTime(time).GetLmtDateTimeOffset();

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
                sunriseTime = GetSunriseTime(previousDay).GetLmtDateTimeOffset();

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
        /// get sunrise time for that day
        /// </summary>
        public static Time GetSunriseTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetSunriseTime", time), _getSunriseTime);


            //UNDERLYING FUNCTION
            Time _getSunriseTime()
            {
                //1. Calculate sunrise time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_RISE | srflag; //set for sunrise
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().GetLongitude(), time.GetGeoLocation().GetLatitude(), 0 };
                double riseTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature

                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());


                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = AstronomicalCalculator.GetGreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunrise time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref riseTimeRaw, ref errorMsg);


                //2. Convert raw sun rise time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = AstronomicalCalculator.GetGreenwichTimeFromJulianDays(riseTimeRaw);

                //return sunrise time at orginal location to caller
                var stdOriginal = sunriseLmtAtGreenwich.ToOffset(time.GetStdDateTimeOffset().Offset);
                var sunriseTime = new Time(stdOriginal, time.GetGeoLocation());
                return sunriseTime;

            }

        }

        /// <summary>
        /// Get actual sunset time for that day at that place
        /// </summary>
        public static Time GetSunsetTime(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetSunsetTime", time), _getSunsetTime);


            //UNDERLYING FUNCTION
            Time _getSunsetTime()
            {
                //1. Calculate sunset time

                //prepare data to do calculation
                const int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_SIDEREAL;
                const int srflag = SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER; //disk is at center of horizon
                var options = SwissEph.SE_CALC_SET | srflag; //set for sunset
                var planet = SwissEph.SE_SUN;

                double[] geopos = new Double[3] { time.GetGeoLocation().GetLongitude(), time.GetGeoLocation().GetLatitude(), 0 };
                double setTimeRaw = 0;

                var errorMsg = "";
                const double atpress = 0.0; //pressure
                const double attemp = 0.0;  //temperature


                //create a new time at 12 am on the same day, as calculator searches for sunrise after the inputed time
                var oriLmt = time.GetLmtDateTimeOffset();
                var lmtAt12Am = new DateTime(oriLmt.Year, oriLmt.Month, oriLmt.Day, 0, 0, 0);
                var timeAt12Am = new Time(lmtAt12Am, time.GetStdDateTimeOffset().Offset, time.GetGeoLocation());

                //get LMT at Greenwich in Julian days
                var julianLmtUtcTime = AstronomicalCalculator.GetGreenwichLmtInJulianDays(timeAt12Am);

                //do calculation for sunset time
                using SwissEph ephemeris = new();
                int ret = ephemeris.swe_rise_trans(julianLmtUtcTime, planet, "", iflag, options, geopos, atpress, attemp, ref setTimeRaw, ref errorMsg);



                //2. Convert raw sun set time (julian lmt utc) to normal time (std)

                //julian days back to normal time (greenwich)
                var sunriseLmtAtGreenwich = AstronomicalCalculator.GetGreenwichTimeFromJulianDays(setTimeRaw);

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
        public static DateTime GetNoonTime(Time time)
        {
            //get apparent time
            var localApparentTime = AstronomicalCalculator.GetLocalApparentTime(time);
            var apparentNoon = new DateTime(localApparentTime.Year, localApparentTime.Month, localApparentTime.Day, 12, 0, 0);

            return apparentNoon;
        }


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
        ///
        /// 
        /// Note:
        /// ATM malefic planets override benefic
        /// TODO not sure if malefic planet overrides benefic if both are conjunct
        /// </summary>
        public static bool IsMercuryMalefic(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("IsMercuryMalefic", time), _isMercuryMalefic);


            //UNDERLYING FUNCTION
            bool _isMercuryMalefic()
            {
                //if mercury is already with malefics,then not checking if conjunct with benefic (not 100% sure)
                if (conjunctWithMalefic()) { return true; }

                //if conjunct with benefic, then it is benefic
                if (conjunctWithBenefic()) { return false; }

                //if not conjunct with any planet, should be malefic (not 100% sure, maybe aspects needs to be considered)
                //TODO NOTE : Further checking on this point is needed, for now just place as benefic with mild warning
                LogManager.Debug("Info:Mercury not conjunct, but placed as benefic!");
                return false;


                //------------FUNCTIONS-------------

                bool conjunctWithMalefic()
                {
                    //list the planets that will make mercury malefic
                    var evilPlanetNameList = new List<PlanetName>() { PlanetName.Sun, PlanetName.Saturn, PlanetName.Mars, PlanetName.Rahu, PlanetName.Ketu };

                    //if moon is malefic, add to malefic list
                    if (!IsMoonBenefic(time)) { evilPlanetNameList.Add(PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, PlanetName.Mercury);

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
                    var beneficPlanetNameList = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Venus };

                    //if moon is benefic, add to benefic list
                    if (IsMoonBenefic(time)) { beneficPlanetNameList.Add(PlanetName.Moon); }

                    //get all planets in conjunction with mercury
                    var planetsConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, PlanetName.Mercury);

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
            int lunarDateNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDateNumber();

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

        public static bool IsPlanetBenefic(PlanetName planetName, Time time)
        {
            //get list of benefic planets
            var beneficPlanetList = GetBeneficPlanetList(time);

            //check if input planet is in the list
            var planetIsBenefic = beneficPlanetList.Contains(planetName);

            return planetIsBenefic;
        }

        /// <summary>
        /// Benefics, on the other hand, tend to do good ; but
        /// sometimes they also become capable of doing harm.
        /// </summary>
        public static List<PlanetName> GetBeneficPlanetList(Time time)
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

        public static bool IsPlanetMalefic(PlanetName planetName, Time time)
        {
            //get list of malefic planets
            var maleficPlanetList = GetMaleficPlanetList(time);

            //check if input planet is in the list
            var planetIsMalefic = maleficPlanetList.Contains(planetName);

            return planetIsMalefic;
        }

        /// <summary>
        /// Gets list of permenant malefic planets,
        /// for moon & mercury it is based on changing factors
        ///
        /// Malefics are always inclined to do harm, but under certain
        /// conditions, the intensity of the mischief is tempered.
        /// </summary>
        public static List<PlanetName> GetMaleficPlanetList(Time time)
        {
            //Add permanent evil planets to list first
            var listOfEvilPlanets = new List<PlanetName>() { PlanetName.Sun, PlanetName.Saturn, PlanetName.Mars, PlanetName.Rahu, PlanetName.Ketu };

            //check if moon is evil
            var moonIsEvil = IsMoonBenefic(time) == false;

            //if moon is evil add to evil list
            if (moonIsEvil)
            {
                listOfEvilPlanets.Add(PlanetName.Moon);
            }

            //check if mercury is evil
            var mercuryIsEvil = IsMercuryMalefic(time);
            //if mercury is evil add to evil list
            if (mercuryIsEvil)
            {
                listOfEvilPlanets.Add(PlanetName.Mercury);
            }

            return listOfEvilPlanets;
        }

        /// <summary>
        /// Gets planet aspected by the inputed planet
        /// </summary>
        public static List<PlanetName> GetPlanetsInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = AstronomicalCalculator.GetSignsPlanetIsAspecting(planet, time);

            //get all the planets located in these signs
            var planetsAspected = new List<PlanetName>();
            foreach (var zodiacSign in signAspecting)
            {
                var planetInSign = AstronomicalCalculator.GetPlanetInSign(zodiacSign, time);
                //add to list
                planetsAspected.AddRange(planetInSign);
            }


            //return these planets as aspected by input planet
            return planetsAspected;

        }

        /// <summary>
        /// Gets houses aspected by the inputed planet
        /// </summary>
        public static List<HouseName> GetHousesInAspect(PlanetName planet, Time time)
        {
            //get signs planet is aspecting
            var signAspecting = AstronomicalCalculator.GetSignsPlanetIsAspecting(planet, time);

            //get all the houses located in these signs
            var housesAspected = new List<HouseName>();
            foreach (var house in House.AllHouses)
            {
                //get sign of house
                var houseSign = AstronomicalCalculator.GetHouseSignName((int)house, time);

                //add house to list if sign is aspected by planet
                if (signAspecting.Contains(houseSign)) { housesAspected.Add(house); }
            }

            //return the houses aspected by input planet
            return housesAspected;

        }

        /// <summary>
        /// Checks if the a planet is aspected by another planet
        /// </summary>
        public static bool IsPlanetAspectedByPlanet(PlanetName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get planets aspected by transmiting planet
            var planetsInAspect = AstronomicalCalculator.GetPlanetsInAspect(transmitingAspect, time);

            //if receiving planet is in list of currently aspected
            return planetsInAspect.Contains(receiveingAspect);

        }
        /// <summary>
        /// Checks if a house is aspected by a planet
        /// </summary>
        public static bool IsHouseAspectedByPlanet(HouseName receiveingAspect, PlanetName transmitingAspect, Time time)
        {
            //get houses aspected by transmiting planet
            var houseInAspect = AstronomicalCalculator.GetHousesInAspect(transmitingAspect, time);

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
            var planetAConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, planetA);
            var planetBConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, planetB);

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
        /// 0 index being strongest to 6 index being weakest
        ///
        /// Note:
        /// Significance of being Powerful.-Among
        /// the several planets associated with a bhava, that,
        /// which has the greatest Sbadbala, influences the
        /// bhava most.
        /// </summary>
        public static PlanetName[] GetAllPlanetOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetAllPlanetOrderedByStrength", time), _getAllPlanetOrderedByStrength);


            //UNDERLYING FUNCTION
            PlanetName[] _getAllPlanetOrderedByStrength()
            {
                var planetStrenghtList = new Dictionary<double, PlanetName>();

                //create a list with planet names & its strength (unsorted)
                foreach (var planet in PlanetName.AllPlanets)
                {
                    //get planet strength in rupas
                    var strength = GetPlanetShadbalaPinda(planet, time).ToRupa();

                    //devide strength by minimum limit (based on planet)
                    var strengthAfterLimit = strength / getLimit(planet);

                    //place in list with planet name
                    planetStrenghtList[strengthAfterLimit] = planet;

                }


                //sort that list from strongest planet to weakest planet
                var keys_sorted = planetStrenghtList.Keys.ToList();
                keys_sorted.Sort();

                PlanetName[] sortedArray = new PlanetName[7];
                var count = 6;
                foreach (var key in keys_sorted)
                {
                    sortedArray[count] = planetStrenghtList[key];
                    count--;
                }

                return sortedArray;


                //-------------FUNCTIONS

                //get a preset strength limit for planet
                double getLimit(PlanetName _planet)
                {
                    if (_planet == PlanetName.Sun) { return 5; }
                    else if (_planet == PlanetName.Moon) { return 6; }
                    else if (_planet == PlanetName.Mars) { return 5; }
                    else if (_planet == PlanetName.Mercury) { return 7; }
                    else if (_planet == PlanetName.Jupiter) { return 6.5; }
                    else if (_planet == PlanetName.Venus) { return 5.5; }
                    else if (_planet == PlanetName.Saturn) { return 5; }

                    throw new Exception("Planet not specified!");
                }
            }

        }

        /// <summary>
        /// Returns an array of all houses sorted by strength,
        /// 0 index being strongest to 11 index being weakest
        ///
        /// </summary>
        public static HouseName[] GetAllHousesOrderedByStrength(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetAllHousesOrderedByStrength", time), _getAllHousesOrderedByStrength);


            //UNDERLYING FUNCTION
            HouseName[] _getAllHousesOrderedByStrength()
            {
                var houseStrenghtList = new Dictionary<double, HouseName>();

                //create a list with planet names & its strength (unsorted)
                foreach (var house in House.AllHouses)
                {
                    //get house strength
                    var strength = GetBhavabala(house, time).ToRupa();

                    //place in list with house number
                    houseStrenghtList[strength] = house;

                }


                //sort that list from strongest house to weakest house
                var keys_sorted = houseStrenghtList.Keys.ToList();
                keys_sorted.Sort();

                var sortedArray = new HouseName[12];
                var count = 11;
                foreach (var key in keys_sorted)
                {
                    sortedArray[count] = houseStrenghtList[key];
                    count--;
                }

                return sortedArray;
            }

        }

        /// <summary>
        /// Shadbala :the six sources of strength and weakness the planets
        /// The importanc of and the part played by the Shadbalas,
        /// in the science of horoscopy, are manifold
        ///
        /// In order to obtain the total strength of
        /// the Shadbala Pinda of each planet, we have to add
        /// together its Sthana Bala, Dik Bala, Kala Bala.
        /// 'Chesta Bala and Naisargika Bala. And the Graha's
        /// Drik Bala must be added to or subtracted from the
        /// above sum according as it is positive or negative.
        /// The resul.t obtained is the Shadbala Pinda of the
        /// planet in Shashtiamsas.
        /// </summary>
        public static Shashtiamsa GetPlanetShadbalaPinda(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetShadbalaPinda", planetName, time), _getPlanetShadbalaPinda);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadbalaPinda()
            {
                //Sthana bala (Positional Strength)
                var sthanaBala = GetPlanetSthanaBala(planetName, time);

                //Get dik bala (Directional Strength)
                var digBala = GetPlanetDigBala(planetName, time);

                //Get kala bala (Temporal Strength)
                var kalaBala = GetPlanetKalaBala(planetName, time);

                //Get Chesta bala (Motional Strength)
                var chestaBala = GetPlanetChestaBala(planetName, time);

                //Get Naisargika bala (Natural Strength)
                var naisargikaBala = GetPlanetNaisargikaBala(planetName, time);

                //Get Drik bala (Aspect Strength)
                var drikBala = GetPlanetDrikBala(planetName, time);


                //Get total Shadbala Pinda
                var total = sthanaBala + digBala + kalaBala + chestaBala + naisargikaBala + drikBala;

                return total;
            }

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
        public static Shashtiamsa GetPlanetDrikBala(PlanetName planetName, Time time)
        {


            double dk;
            var drishti = new Dictionary<String, double>();
            double vdrishti;
            var sp = new Dictionary<PlanetName, int>();


            foreach (var p in PlanetName.AllPlanets)
            {
                if (AstronomicalCalculator.IsPlanetBenefic(p, time))
                {
                    sp[p] = 1;
                }
                else
                {
                    sp[p] = -1;
                }

            }


            foreach (var i in PlanetName.AllPlanets)
            {
                foreach (var j in PlanetName.AllPlanets)
                {
                    //Finding Drishti Kendra or Aspect Angle
                    var planetNirayanaLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, j).TotalDegrees;
                    var nirayanaLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, i).TotalDegrees;
                    dk = planetNirayanaLongitude - nirayanaLongitude;

                    if (dk < 0) { dk += 360; }

                    //get special aspect if any
                    vdrishti = FindViseshaDrishti(dk, i);

                    drishti[i.ToString() + j.ToString()] = FindDrishtiValue(dk) + vdrishti;

                }
            }

            double bala = 0;

            var DrikBala = new Dictionary<PlanetName, double>();

            foreach (var i in PlanetName.AllPlanets)
            {
                bala = 0;

                foreach (var j in PlanetName.AllPlanets)
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

            if (p == PlanetName.Saturn)
            {
                if (((dk >= 60) && (dk <= 90)) || ((dk >= 270) && (dk <= 300)))
                {
                    vdrishti = 45;
                }

            }
            else if (p == PlanetName.Jupiter)
            {

                if (((dk >= 120) && (dk <= 150))
                    || ((dk >= 240) && (dk <= 270)))
                {
                    vdrishti = 30;
                }

            }
            else if (p == PlanetName.Mars)
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
        public static Shashtiamsa GetPlanetNaisargikaBala(PlanetName planetName, Time time)
        {
            if (planetName == PlanetName.Sun) { return new Shashtiamsa(60); }
            else if (planetName == PlanetName.Moon) { return new Shashtiamsa(51.43); }
            else if (planetName == PlanetName.Venus) { return new Shashtiamsa(42.85); }
            else if (planetName == PlanetName.Jupiter) { return new Shashtiamsa(34.28); }
            else if (planetName == PlanetName.Mercury) { return new Shashtiamsa(25.70); }
            else if (planetName == PlanetName.Mars) { return new Shashtiamsa(17.14); }
            else if (planetName == PlanetName.Saturn) { return new Shashtiamsa(8.57); }

            throw new Exception("Planet not specified!");
        }

        /// <summary>
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
        /// </summary>
        public static Shashtiamsa GetPlanetChestaBala(PlanetName planetName, Time time)
        {
            //the Sun and the Moon doesn not retrograde, so 0 chesta bala
            if (planetName == PlanetName.Sun || planetName == PlanetName.Moon) { return Shashtiamsa.Zero; }

            //get the interval between birth date and the date of the epoch (1900)
            var interval = GetInterval(time);

            //get the mean longitudes of all planets
            var madhya = GetMadhya(interval, time);

            //get the apogee of all planets
            var seegh = GetSeeghrochcha(madhya, interval, time);


            //calculate chesta kendra, also called Seeghra kendra
            var planetLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, planetName).TotalDegrees;
            var chestaKendra = (seegh[planetName] - ((madhya[planetName] + planetLongitude) / 2.0));


            //If the Chesta kendra exceeds 180°, subtract it from 360, otherwise
            //keep it as it is. The remainder represents the reduced Chesta kendra
            if (chestaKendra < 360.00) { chestaKendra = chestaKendra + 360; }
            chestaKendra = chestaKendra % 360;
            if (chestaKendra > 180.00) { chestaKendra = 360 - chestaKendra; }


            //The Chesta Bala is zero when the Chesta kendra is also zero. When it is
            //180° the Chesta Bala is 60 Shashtiamsas. In intermediate position, the
            //Bala is found by proportion (devide by 3)
            var chestaBala = (chestaKendra / 3.00);

            return new Shashtiamsa(chestaBala);



            //------------------------FUNCTIONS--------------

            //Get mean longitudes (Madhya)
            Dictionary<PlanetName, double> GetMadhya(double _interval, Time time1)
            {
                int _birthYear = time1.GetLmtDateTimeOffset().Year;

                var madhya = new Dictionary<PlanetName, double>();

                //this is the position of Madhya Ravi at the moment of birth
                madhya[PlanetName.Sun] = madhya[PlanetName.Mercury] = madhya[PlanetName.Venus] = ((_interval * 0.9855931) + 257.4568) % 360;
                madhya[PlanetName.Mars] = ((_interval * 0.5240218) + 270.22) % 360;

                var correction1 = 3.33 + (0.0067 * (_birthYear - 1900));
                madhya[PlanetName.Jupiter] = (((_interval * 0.08310024) + 220.04) - correction1) % 360;

                correction1 = 5 + (0.001 * (_birthYear - 1900));
                madhya[PlanetName.Saturn] = ((_interval * 0.03333857) + 236.74 + correction1) % 360;

                return madhya;
            }

            //Seeghrochcha is the apogee of the planet
            //It is required to find the Chesta kendra.
            Dictionary<PlanetName, double> GetSeeghrochcha(Dictionary<PlanetName, double> _madhya, double _interval, Time time1)
            {
                int _birthYear = time1.GetLmtDateTimeOffset().Year;
                var seegh = new Dictionary<PlanetName, double>();
                double correction;

                //The mean longitude of the Sun will be the Seeghrochcha of Kuja, Guru and Sani.
                seegh[PlanetName.Mars] = seegh[PlanetName.Jupiter] = seegh[PlanetName.Saturn] = _madhya[PlanetName.Sun];

                correction = 6.670 + (0.00133 * (_birthYear - 1900));
                seegh[PlanetName.Mercury] = ((_interval * 4.092385) + 164.00 + correction) % 360;

                correction = 5 + (0.0001 * (_birthYear - 1900));
                seegh[PlanetName.Venus] = (((_interval * 1.602159) + 328.51) - correction) % 360;

                return seegh;
            }

            //Get interval from the epoch to the birth date
            double GetInterval(Time time1)
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


                double utime = new TimeSpan(time1.GetLmtDateTimeOffset().Hour, time1.GetLmtDateTimeOffset().Minute, 0).TotalHours +
                               ((5 + (double)(4.00 / 60.00)) - time1.GetLmtDateTimeOffset().Offset.TotalHours);

                //The result represents the interval from the epoch to the birth date.
                double interval = epochDays + (double)(utime / 24.00);

                return interval;
            }
        }

        /// <summary>
        /// circulation time of the objects in years, used by cheshta bala calculation
        /// </summary>
        public static double GetPlanetCirculationTime(PlanetName planetName)
        {

            if (planetName == PlanetName.Sun) { return 1.0; }
            if (planetName == PlanetName.Moon) { return .082; }
            if (planetName == PlanetName.Mars) { return 1.88; }
            if (planetName == PlanetName.Mercury) { return .24; }
            if (planetName == PlanetName.Jupiter) { return 11.86; }
            if (planetName == PlanetName.Venus) { return .62; }
            if (planetName == PlanetName.Saturn) { return 29.46; }

            throw new Exception("Planet circulation time not defined!");

        }

        /// <summary>
        /// Sapthavargajabala: This is the strength of a
        /// planet due to its residence in the seven sub-divisions
        /// according to its relation with the dispositor.
        /// </summary>
        public static Shashtiamsa GetPlanetSaptavargajaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSaptavargajaBala", planetName, time), _getPlanetSaptavargajaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSaptavargajaBala()
            {
                //declare total value
                double totalSaptavargajaBalaInShashtiamsa = 0;

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();


                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = AstronomicalCalculator.IsPlanetInMoolatrikona(planetName, time);

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
                    var planetRasi = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();
                    var rasiSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //get planet hora
                var planetHora = AstronomicalCalculator.GetPlanetHoraSign(planetName, time);
                var horaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);


                //get planet drekkana
                var planetDrekkana = AstronomicalCalculator.GetPlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //get planet saptamsa
                var planetSaptamsa = AstronomicalCalculator.GetPlanetSaptamsaSign(planetName, time);
                var saptamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetSaptamsa, time);
                //add planet saptamsa relationship to list
                planetSignRelationshipList.Add(saptamsaSignRelationship);


                //get planet navamsa
                var planetNavamsa = AstronomicalCalculator.GetPlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //get planet dwadasamsa
                var planetDwadasamsa = AstronomicalCalculator.GetPlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //get planet thrimsamsa
                var planetThrimsamsa = AstronomicalCalculator.GetPlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
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
                    if (planetToSignRelationship == PlanetToSignRelationship.Swavarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.AdhiMitravarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Mitravarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.Samavarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Satruvarga)
                    {
                        totalSaptavargajaBalaInShashtiamsa = totalSaptavargajaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.AdhiSatruvarga)
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
        public static Shashtiamsa GetPlanetShadvargaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetShadvargaBala", planetName, time), _getPlanetShadvargaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetShadvargaBala()
            {

                //declare empty list of planet sign relationships
                var planetSignRelationshipList = new List<PlanetToSignRelationship>();

                //1.
                //get planet rasi Moolatrikona.
                var planetInMoolatrikona = AstronomicalCalculator.IsPlanetInMoolatrikona(planetName, time);

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
                    var planetRasi = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();
                    var rasiSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetRasi, time);

                    //add planet rasi relationship to list
                    planetSignRelationshipList.Add(rasiSignRelationship);
                }

                //2.
                //get planet hora
                var planetHora = AstronomicalCalculator.GetPlanetHoraSign(planetName, time);
                var horaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetHora, time);
                //add planet hora relationship to list
                planetSignRelationshipList.Add(horaSignRelationship);

                //3.
                //get planet drekkana
                var planetDrekkana = AstronomicalCalculator.GetPlanetDrekkanaSign(planetName, time);
                var drekkanaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetDrekkana, time);
                //add planet drekkana relationship to list
                planetSignRelationshipList.Add(drekkanaSignRelationship);


                //4.
                //get planet navamsa
                var planetNavamsa = AstronomicalCalculator.GetPlanetNavamsaSign(planetName, time);
                var navamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetNavamsa, time);
                //add planet navamsa relationship to list
                planetSignRelationshipList.Add(navamsaSignRelationship);


                //5.
                //get planet dwadasamsa
                var planetDwadasamsa = AstronomicalCalculator.GetPlanetDwadasamsaSign(planetName, time);
                var dwadasamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetDwadasamsa, time);
                //add planet dwadasamsa relationship to list
                planetSignRelationshipList.Add(dwadasamsaSignRelationship);


                //6.
                //get planet thrimsamsa
                var planetThrimsamsa = AstronomicalCalculator.GetPlanetThrimsamsaSign(planetName, time);
                var thrimsamsaSignRelationship = AstronomicalCalculator.GetPlanetRelationshipWithSign(planetName, planetThrimsamsa, time);
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
                    if (planetToSignRelationship == PlanetToSignRelationship.Swavarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 30;
                    }

                    //in Adhi Mitravarga 22.5 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.AdhiMitravarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 22.5;
                    }

                    //in Mitravarga 15 · Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Mitravarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 15;
                    }

                    //in Samavarga 7.5 Shashtiamsas ~
                    if (planetToSignRelationship == PlanetToSignRelationship.Samavarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 7.5;
                    }

                    //in Satruvarga 3.75 Shashtiamsas;
                    if (planetToSignRelationship == PlanetToSignRelationship.Satruvarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 3.75;
                    }

                    //in Adhi Satruvarga 1.875 Shashtiamsas.
                    if (planetToSignRelationship == PlanetToSignRelationship.AdhiSatruvarga)
                    {
                        totalShadvargaBalaInShashtiamsa = totalShadvargaBalaInShashtiamsa + 1.875;
                    }

                }


                return new Shashtiamsa(totalShadvargaBalaInShashtiamsa);

            }

        }

        //TODO : use of shadvarga bala might be wrong here, needs clarification
        //      problem is too much of time goes under bad, doesnt seem right
        //      for now we put it 140 threhold so guarenteed to be strong
        //      and doesn not occur all the time
        public static bool IsPlanetStrongInShadvarga(PlanetName planet, Time time)
        {
            //get planet shadvarga bala
            var planetBala = AstronomicalCalculator.GetPlanetShadvargaBala(planet, time).ToDouble();

            //Note: To determine if shadvarga bala value is strong or weak
            //a neutral point is set, anything above is strong & below is weak
            var neutralPoint = AstronomicalCalculator.GetPlanetShadvargaBalaNeutralPoint(planet);

            //if above neutral number, is strong else it is weak
            return planetBala > 140;
        }


        /// <summary>
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
        public static Shashtiamsa GetPlanetSthanaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSthanaBala", planetName, time), _getPlanetSthanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetSthanaBala()
            {
                //Get Ochcha Bala (exaltation strength)
                var ochchaBala = GetPlanetOchchabala(planetName, time);

                //Get Saptavargaja Bala
                var saptavargajaBala = GetPlanetSaptavargajaBala(planetName, time);

                //Get Ojayugmarasyamsa Bala
                var ojayugmarasymsaBala = GetPlanetOjayugmarasyamsaBala(planetName, time);

                //Get Kendra Bala
                var kendraBala = GetPlanetKendraBala(planetName, time);

                //Drekkana Bala
                var drekkanaBala = GetPlanetDrekkanaBala(planetName, time);

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
        public static Shashtiamsa GetPlanetDrekkanaBala(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

            //get degrees in sign 
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //if male planet -Ravi, Guru and Kuja.
            if (planetName == PlanetName.Sun || planetName == PlanetName.Jupiter || planetName == PlanetName.Mars)
            {
                //if planet is in 1st drekkana
                if (degreesInSign >= 0 && degreesInSign <= 10.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Hermaphrodite Planets.-Sani and Budba
            if (planetName == PlanetName.Saturn || planetName == PlanetName.Mercury)
            {
                //if planet is in 2nd drekkana
                if (degreesInSign > 10 && degreesInSign <= 20.0)
                {
                    //return 15 bala
                    return new Shashtiamsa(15);
                }

            }

            //if Female Planets.-Chandra and Sukra
            if (planetName == PlanetName.Moon || planetName == PlanetName.Venus)
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
        public static Shashtiamsa GetPlanetKendraBala(PlanetName planetName, Time time)
        {
            //get number of the sign planet is in
            var planetSignNumber = (int)AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

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
        /// Ojayugmarasiba/a: In odd Rasi and Navamsa,
        /// the Sun, Mars, Jupiter, Mercury and Saturn
        /// get strength and the rest in even signs
        /// </summary>
        public static Shashtiamsa GetPlanetOjayugmarasyamsaBala(PlanetName planetName, Time time)
        {
            //get planet rasi sign
            var planetRasiSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

            //get planet navamsa sign
            var planetNavamsaSign = AstronomicalCalculator.GetPlanetNavamsaSign(planetName, time);

            //declare total Ojayugmarasyamsa Bala as 0 first
            double totalOjayugmarasyamsaBalaInShashtiamsas = 0;

            //if planet is the moon or venus
            if (planetName == PlanetName.Moon || planetName == PlanetName.Venus)
            {
                //if rasi sign is an even sign
                if (AstronomicalCalculator.IsEvenSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an even sign
                if (AstronomicalCalculator.IsEvenSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }
            //if planet is Sun, Mars, Jupiter, Mercury and Saturn
            else if (planetName == PlanetName.Sun || planetName == PlanetName.Mars ||
                     planetName == PlanetName.Jupiter || planetName == PlanetName.Mercury || planetName == PlanetName.Saturn)
            {
                //if rasi sign is an odd sign
                if (AstronomicalCalculator.IsOddSign(planetRasiSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

                //if navamsa sign is an odd sign
                if (AstronomicalCalculator.IsOddSign(planetNavamsaSign))
                {
                    //add 15 Shashtiamsas
                    totalOjayugmarasyamsaBalaInShashtiamsas += 15;
                }

            }

            return new Shashtiamsa(totalOjayugmarasyamsaBalaInShashtiamsas);
        }

        public static Shashtiamsa GetPlanetKalaBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetKalaBala", planetName, time), _getPlanetKalaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetKalaBala()
            {
                //place to store pre kala bala values
                var kalaBalaList = new Dictionary<PlanetName, Shashtiamsa>();


                //Yuddha Bala requires all planet's pre kala bala
                //so calculate pre kala bala for all planets first
                foreach (var planet in PlanetName.AllPlanets)
                {
                    //calculate pre kala bala
                    var preKalaBala = GetPreKalaBala(planet, time);

                    //store in a list sorted by planet name, to be accessed later
                    kalaBalaList.Add(planet, preKalaBala);
                }


                //calculate Yuddha Bala
                var yuddhaBala = GetPlanetYuddhaBala(planetName, kalaBalaList, time);


                //Total Kala Bala
                var total = kalaBalaList[planetName] + yuddhaBala;


                return total;


                //---------------FUNCTIONS--------------
                Shashtiamsa GetPreKalaBala(PlanetName planetName, Time time)
                {
                    //Nathonnatha Bala
                    var nathonnathaBala = GetPlanetNathonnathaBala(planetName, time);

                    //Paksha Bala
                    var pakshaBala = GetPlanetPakshaBala(planetName, time);

                    //Tribhaga Bala
                    var tribhagaBala = GetPlanetTribhagaBala(planetName, time);

                    //Abda Bala
                    var abdaBala = GetPlanetAbdaBala(planetName, time);

                    //Masa Bala
                    var masaBala = GetPlanetMasaBala(planetName, time);

                    //Vara Bala
                    var varaBala = GetPlanetVaraBala(planetName, time);

                    //Hora Bala
                    var horaBala = GetPlanetHoraBala(planetName, time);

                    //Ayana Bala
                    var ayanaBala = GetPlanetAyanaBala(planetName, time);

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
        public static Shashtiamsa GetPlanetYuddhaBala(PlanetName inputedPlanet, Dictionary<PlanetName, Shashtiamsa> preKalaBalaValues, Time time)
        {
            //All the planets excepting Sun and Moon may enter into war (Yuddha)
            if (inputedPlanet == PlanetName.Moon || inputedPlanet == PlanetName.Sun) { return Shashtiamsa.Zero; }


            //place to store winner & loser balas
            var yudhdhabala = new Dictionary<PlanetName, Shashtiamsa>();


            //get all planets that are conjunct with inputed planet
            var conjunctPlanetList = AstronomicalCalculator.GetPlanetsInConjuction(time, inputedPlanet);

            //remove rahu & kethu if present, they are not included in Yuddha Bala calculations
            conjunctPlanetList.RemoveAll(pl => pl == PlanetName.Rahu || pl == PlanetName.Ketu);


            foreach (var checkingPlanet in conjunctPlanetList)
            {

                //All the planets excepting Sun and Moon may enter into war (Yuddha)
                //no need to calculate Yuddha, move to next planet, if sun or moon
                if (checkingPlanet == PlanetName.Moon || checkingPlanet == PlanetName.Sun) { continue; }


                //get distance between conjunct planet & inputed planet
                var inputedPlanetLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, inputedPlanet);
                var checkingPlanetLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, checkingPlanet);
                var distance = AstronomicalCalculator.GetDistanceBetweenPlanets(inputedPlanetLong, checkingPlanetLong);


                //if the distance between them is less than one degree
                if (distance < Angle.FromDegrees(1))
                {
                    PlanetName winnerPlanet = null;
                    PlanetName losserPlanet = null;

                    //The conquering planet is the one whose longitude is less.
                    if (inputedPlanetLong < checkingPlanetLong) { winnerPlanet = inputedPlanet; losserPlanet = checkingPlanet; } //inputed planet won
                    else if (inputedPlanetLong > checkingPlanetLong) { winnerPlanet = checkingPlanet; losserPlanet = inputedPlanet; } //checking planet won
                    else if (inputedPlanetLong == checkingPlanetLong) { throw new Exception("Planets same longitude! Not expected"); } //unlikely chance, throw error

                    //When two planets are in war, get the sum of the various Balas, viv., Sthanabala, the
                    // Dikbala and the Kalabala (up to Horabala) described hitherto of the fighting planets. Find out the
                    // difference between these two sums.
                    var shadbaladiff = Math.Abs(preKalaBalaValues[inputedPlanet].ToDouble() - preKalaBalaValues[checkingPlanet].ToDouble());


                    //Divide shadbala difference by the difference between the diameters of the discs of the two fighting planets
                    var diameterDifference = GetPlanetDiscDiameter(inputedPlanet).GetDifference(GetPlanetDiscDiameter(checkingPlanet));


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

            //Bimba Parimanas.-This means the diameters of the discs of the planets.
            static Angle GetPlanetDiscDiameter(PlanetName planet)
            {
                if (planet == PlanetName.Mars) { return new Angle(0, 9, 4); }
                if (planet == PlanetName.Mercury) { return new Angle(0, 6, 6); }
                if (planet == PlanetName.Jupiter) { return new Angle(0, 190, 4); }
                if (planet == PlanetName.Venus) { return new Angle(0, 16, 6); }
                if (planet == PlanetName.Saturn) { return new Angle(0, 158, 0); }

                //control should not come here, report error
                throw new Exception("Disc diameter now found!");
            }

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
        public static Shashtiamsa GetPlanetAyanaBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetAyanaBala", planetName, time), _getPlanetAyanaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAyanaBala()
            {
                double ayanaBala = 0;

                //get plant kranti (negative south, positive north)
                var kranti = GetPlanetDeclination(planetName, time);

                //prepare values for calculation of ayanabala
                var x = Angle.FromDegrees(24);
                var isNorthDeclination = kranti < 0 ? false : true;

                //get declination without negative (absolute value), easier for calculation
                var absKranti = Math.Abs((double)kranti);

                //In case of Sukra, Ravi, Kuja and Guru their north declinations are
                //additive and south declinations are subtractive
                if (planetName == PlanetName.Venus || planetName == PlanetName.Sun || planetName == PlanetName.Mars || planetName == PlanetName.Jupiter)
                {
                    //additive
                    if (isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }

                    //And double the Ayanabala in the case of the Sun
                    if (planetName == PlanetName.Sun) { ayanaBala = ayanaBala * 2; }

                }
                //In case of Sani and Chandra, their south declinations are additive while their
                //north declinations are subtractive.
                else if (planetName == PlanetName.Saturn || planetName == PlanetName.Moon)
                {
                    //additive
                    if (!isNorthDeclination) { ayanaBala = ((24 + absKranti) / 48) * 60; }

                    //subtractive
                    else { ayanaBala = ((24 - absKranti) / 48) * 60; }
                }
                //For Budha the declination, north or south, is always additive.
                else if (planetName == PlanetName.Mercury)
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
        public static double GetPlanetDeclination(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetDeclination", planetName, time), _getPlanetDeclination);


            //UNDERLYING FUNCTION
            double _getPlanetDeclination()
            {
                //for degree to radian conversion
                const double DEG2RAD = 0.0174532925199433;

                var eps = GetPlanetEps(planetName, time);

                var tlen = AstronomicalCalculator.GetPlanetSayanaLongitude(time, planetName);
                var lat = AstronomicalCalculator.GetPlanetSayanaLatitude(time, planetName);

                //if kranti (declination), is a negative number, it means south, else north of equator
                var kranti = lat.TotalDegrees + eps * Math.Sin(DEG2RAD * tlen.TotalDegrees);

                return kranti;
            }

        }

        /// <summary>
        /// TODO find out what on earth this calculation is
        /// What is EPS?S
        /// </summary>
        public static double GetPlanetEps(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetEps", planetName, time), _getPlanetEps);


            //UNDERLYING FUNCTION
            double _getPlanetEps()
            {
                double eps;

                string err = "";
                double[] x = new double[6];

                SwissEph ephemeris = new SwissEph();

                // Convert DOB to ET
                var jul_day_ET = AstronomicalCalculator.TimeToEphemerisTime(time);

                ephemeris.swe_calc(jul_day_ET, SwissEph.SE_ECL_NUT, 0, x, ref err);

                eps = x[0];

                return eps;
            }

        }

        /// <summary>
        /// AKA Horadhipathi Bala
        /// </summary>
        public static Shashtiamsa GetPlanetHoraBala(PlanetName planetName, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetHoraBala", planetName, time), _getPlanetHoraBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetHoraBala()
            {
                //first ascertain the weekday of birth
                var birthWeekday = AstronomicalCalculator.GetDayOfWeek(time);

                //ascertain the number of hours elapsed from sunrise to birth
                //This shows the number of horas passed.
                var hora = AstronomicalCalculator.GetHoraAtBirth(time);

                //get lord of hora (hour)
                var lord = AstronomicalCalculator.GetLordOfHora(hora, birthWeekday);

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
        public static Shashtiamsa GetPlanetAbdaBala(PlanetName planetName, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetAbdaBala", planetName, time), _getPlanetAbdaBala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getPlanetAbdaBala()
            {
                //calculate year lord
                dynamic yearAndMonthLord = GetYearAndMonthLord(time);
                PlanetName yearLord = yearAndMonthLord.YearLord;

                //if inputed planet is year lord than 15 Shashtiamsas
                if (yearLord == planetName) { return new Shashtiamsa(15); }

                //not year lord, 0 Shashtiamsas
                return Shashtiamsa.Zero;
            }


        }

        public static Shashtiamsa GetPlanetMasaBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the month of birth is assigned a value of 30 Shashtiamsas
            //as his Masabala.


            //calculate month lord
            dynamic yearAndMonthLord = GetYearAndMonthLord(time);
            PlanetName monthLord = yearAndMonthLord.MonthLord;

            //if inputed planet is month lord than 30 Shashtiamsas
            if (monthLord == planetName) { return new Shashtiamsa(30); }

            //not month lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        public static Shashtiamsa GetPlanetVaraBala(PlanetName planetName, Time time)
        {
            //The planet who is the lord of
            //the day of birth is assigned a value of 45 Shashtiamsas
            //as his Varabala.

            //calculate day lord
            PlanetName dayLord = AstronomicalCalculator.GetLordOfWeekday(time);

            //if inputed planet is day lord than 45 Shashtiamsas
            if (dayLord == planetName) { return new Shashtiamsa(45); }

            //not day lord, 0 Shashtiamsas
            return Shashtiamsa.Zero;

        }

        /// <summary>
        /// Gets year & month lord at inputed time
        /// </summary>
        public static object GetYearAndMonthLord(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetYearAndMonthLord", time), _getYearAndMonthLord);


            //UNDERLYING FUNCTION
            object _getYearAndMonthLord()
            {
                //set default
                var yearLord = PlanetName.Sun;
                var monthLord = PlanetName.Sun;


                //initialize ephemeris
                using SwissEph ephemeris = new SwissEph();

                double ut_arghana = ephemeris.swe_julday(1827, 5, 2, 0, SwissEph.SE_GREG_CAL);
                double ut_noon = AstronomicalCalculator.GetGreenwichLmtInJulianDays(time);

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
                yearLord = AstronomicalCalculator.GetLordOfWeekday(yearWeekday);

                //Masadhipath : The planet that rules the weekday of the commencement of the month of the birth
                monthLord = AstronomicalCalculator.GetLordOfWeekday(monthWeekday);

                //package year & month lord together & return
                return new { YearLord = yearLord, MonthLord = monthLord };


                //---------------------FUNCTION--------------------

                //converts swiss epehmeris weekday numbering to muhurtha weekday numbering
                DayOfWeek swissEphWeekDayToMuhurthaDay(int dayNumber)
                {
                    switch (dayNumber)
                    {
                        case 0: return DayOfWeek.Monday;
                        case 1: return DayOfWeek.Tuesday;
                        case 2: return DayOfWeek.Wednesday;
                        case 3: return DayOfWeek.Thursday;
                        case 4: return DayOfWeek.Friday;
                        case 5: return DayOfWeek.Saturday;
                        case 6: return DayOfWeek.Sunday;
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
        public static Shashtiamsa GetPlanetTribhagaBala(PlanetName planetName, Time time)
        {
            PlanetName ret = PlanetName.Jupiter;

            var sunsetTime = AstronomicalCalculator.GetSunsetTime(time);

            if (IsDayBirth(time))
            {
                //find out which part of the day birth took place

                var sunriseTime = AstronomicalCalculator.GetSunriseTime(time);

                //substraction should always return a positive number, since sunset is always after sunrise
                double lengthHours = (sunsetTime.Subtract(sunriseTime).TotalHours) / 3;
                double offset = time.Subtract(sunriseTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = PlanetName.Mercury; break;
                    case 1: ret = PlanetName.Sun; break;
                    case 2: ret = PlanetName.Saturn; break;
                }
            }
            else
            {
                //get sunrise time at on next day to get duration of the night
                var nextDayTime = time.AddHours(24);
                var nextDaySunrise = AstronomicalCalculator.GetSunriseTime(nextDayTime);

                double lengthHours = (nextDaySunrise.Subtract(sunsetTime).TotalHours) / 3;
                double offset = time.Subtract(sunsetTime).TotalHours;
                int part = (int)(Math.Floor(offset / lengthHours));
                switch (part)
                {
                    case 0: ret = PlanetName.Moon; break;
                    case 1: ret = PlanetName.Venus; break;
                    case 2: ret = PlanetName.Mars; break;
                }
            }

            //Always assign a value of 60 Shashtiamsas
            //to Guru irrespective of whether birth is during
            //night or during day.
            if (planetName == PlanetName.Jupiter || planetName == ret) { return new Shashtiamsa(60); }

            return new Shashtiamsa(0);
        }

        /// <summary>
        /// Oochchabala : The distance between the
        /// planet's longitude and its debilitation point, divided
        /// by 3, gives its exaltation strength or oochchabaJa.
        /// </summary>
        public static Shashtiamsa GetPlanetOchchabala(PlanetName planetName, Time time)
        {
            //1.0 Get Planet longitude
            var planetLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, planetName);

            //2.0 Get planet debilitation point
            var planetDebilitationPoint = AstronomicalCalculator.GetPlanetDebilitationPoint(planetName);

            //3.0 Get difference between planet longitude & debilitation point
            //var difference = planetLongitude.GetDifference(planetDebilitationPoint); //todo need checking
            var difference = AstronomicalCalculator.GetDistanceBetweenPlanets(planetLongitude, planetDebilitationPoint);

            //4.0 If difference is more than 180 degrees
            if (difference.TotalDegrees > 180)
            {
                //get the difference of it with 360 degrees
                //difference = difference.GetDifference(Angle.Degrees360);
                difference = AstronomicalCalculator.GetDistanceBetweenPlanets(difference, Angle.Degrees360);

            }

            //5.0 Divide difference with 3 to get ochchabala
            var ochchabalaInShashtiamsa = difference.TotalDegrees / 3;

            //return value in shashtiamsa type
            return new Shashtiamsa(ochchabalaInShashtiamsa);
        }

        //private static Time JulianTimeToTime(double riseTimeRaw, GeoLocation geoLocation)
        //{
        //    SwissEph ephemeris = new SwissEph();

        //    int year = 0;
        //    int month = 0;
        //    int day = 0;
        //    double hour = 0;

        //    ephemeris.swe_revjul(riseTimeRaw, 1, ref year, ref month, ref day, ref hour);

        //    var seperatedHour = TimeSpan.FromHours(hour);


        //    var time = new DateTimeOffset(year, month, day, seperatedHour.Hours, seperatedHour.Minutes, seperatedHour.Seconds, seperatedHour.Milliseconds, new TimeSpan(0, 0, 0));

        //}

        /// <summary>
        /// Determines if the input time is day during day, used for birth times
        /// if day returns true
        /// </summary>
        public static bool IsDayBirth(Time time)
        {
            //get sunrise & sunset times
            var sunrise = AstronomicalCalculator.GetSunriseTime(time).GetLmtDateTimeOffset();
            var sunset = AstronomicalCalculator.GetSunsetTime(time).GetLmtDateTimeOffset();
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
        public static Shashtiamsa GetPlanetPakshaBala(PlanetName planetName, Time time)
        {
            double pakshaBala = 0;

            //get moon phase
            var moonPhase = AstronomicalCalculator.GetLunarDay(time).GetMoonPhase();

            var sunLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Sun);
            var moonLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Moon);

            //var differenceBetweenMoonSun = moonLongitude.GetDifference(sunLongitude);
            var differenceBetweenMoonSun = AstronomicalCalculator.GetDistanceBetweenPlanets(moonLongitude, sunLongitude);

            //When Moon's Long.-Sun's Long. exceeds 180, deduct it from 360°
            if (differenceBetweenMoonSun.TotalDegrees > 180)
            {
                differenceBetweenMoonSun = AstronomicalCalculator.GetDistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360);
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
                var totalDegrees = AstronomicalCalculator.GetDistanceBetweenPlanets(differenceBetweenMoonSun, Angle.Degrees360).TotalDegrees;
                pakshaBalaOfSubhas = totalDegrees / 3.0;
            }

            //60 Shashtiamsas diminished by paksha Bala Of Subhas gives the Paksha Bala of Papas
            var pakshaBalaOfPapas = 60.0 - pakshaBalaOfSubhas;

            //if planet is malefic
            var planetIsMalefic = AstronomicalCalculator.IsPlanetMalefic(planetName, time);
            var planesIsBenefic = AstronomicalCalculator.IsPlanetBenefic(planetName, time);

            if (planesIsBenefic == true && planetIsMalefic == false)
            {
                pakshaBala = pakshaBalaOfSubhas;
            }

            if (planesIsBenefic == false && planetIsMalefic == true)
            {
                pakshaBala = pakshaBalaOfPapas;
            }

            //Chandra's Paksha Bala is always to be doubled
            if (planetName == PlanetName.Moon)
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
        public static Shashtiamsa GetPlanetNathonnathaBala(PlanetName planetName, Time time)
        {
            //get local apparent time
            var localApparentTime = AstronomicalCalculator.GetLocalApparentTime(time);

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

            if (planetName == PlanetName.Sun || planetName == PlanetName.Jupiter || planetName == PlanetName.Venus)
            {
                var divBala = birthTimeInDegrees / 3;

                return new Shashtiamsa(divBala);
            }

            if (planetName == PlanetName.Saturn || planetName == PlanetName.Moon || planetName == PlanetName.Mars)
            {
                var ratriBala = (180 - birthTimeInDegrees) / 3;

                return new Shashtiamsa(ratriBala);
            }

            if (planetName == PlanetName.Mercury)
            {
                //Budha has always a Divaratri Bala of 60 Shashtiamsas
                return new Shashtiamsa(60);

            }

            throw new Exception("Planet Nathonnatha Bala not found, error!");
        }

        /// <summary>
        /// Jupiter and Mercury are strong in Lagoa (Ascendant),
        /// 'the Sun and Mars in the 10th, Saturn in
        /// the 7th and the Moon and Venus in the 4th. The
        /// opposite houses are weak , points. Divide the
        /// distance between the longitude of the planet and
        /// its depression point by 3. Quotient is the strength.
        /// </summary>
        public static Shashtiamsa GetPlanetDigBala(PlanetName planetName, Time time)
        {
            //get planet longitude
            var planetLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, planetName);

            //
            Angle powerlessPointLongitude = null;
            House powerlessHouse;


            //subtract the longitude of the 4th house from the longitudes of the Sun and Mars.
            if (planetName == PlanetName.Sun || planetName == PlanetName.Mars)
            {
                powerlessHouse = AstronomicalCalculator.GetHouse(HouseName.House4, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtract the 7th house, from Jupiter and Mercury.
            if (planetName == PlanetName.Jupiter || planetName == PlanetName.Mercury)
            {
                powerlessHouse = AstronomicalCalculator.GetHouse(HouseName.House7, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //Subtracc the 10th house from Venus and the Moon
            if (planetName == PlanetName.Venus || planetName == PlanetName.Moon)
            {
                powerlessHouse = AstronomicalCalculator.GetHouse(HouseName.House10, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //from Saturn, the ascendant.
            if (planetName == PlanetName.Saturn)
            {
                powerlessHouse = AstronomicalCalculator.GetHouse(HouseName.House1, time);
                powerlessPointLongitude = powerlessHouse.GetMiddleLongitude();
            }

            //get Digbala arc
            //Digbala arc= planet's long. - its powerless cardinal point.
            //var digBalaArc = planetLongitude.GetDifference(powerlessPointLongitude);
            var digBalaArc = AstronomicalCalculator.GetDistanceBetweenPlanets(planetLongitude, powerlessPointLongitude);

            //If difference is more than 180° 
            if (digBalaArc > Angle.Degrees180)
            {
                //subtract it from 360 degrees.
                //digBalaArc = digBalaArc.GetDifference(Angle.Degrees360);
                digBalaArc = AstronomicalCalculator.GetDistanceBetweenPlanets(digBalaArc, Angle.Degrees360);
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
        public static Shashtiamsa GetBhavabala(HouseName house, Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetBhavabala", house, time), _getBhavabala);


            //UNDERLYING FUNCTION
            Shashtiamsa _getBhavabala()
            {
                var BhavaBala = new Dictionary<string, Dictionary<HouseName, double>>();
                BhavaBala["BhavaAdhipathiBala"] = CalcBhavaAdhipathiBala(time);
                BhavaBala["BhavaDigBala"] = CalcBhavaDigBala(time);
                BhavaBala["BhavaDrishtiBala"] = CalcBhavaDrishtiBala(time);

                var balaTypes = new List<string>() { "BhavaAdhipathiBala", "BhavaDigBala", "BhavaDrishtiBala" };

                var totalBhavaBala = new Dictionary<HouseName, double>();

                foreach (var _house in House.AllHouses)
                {
                    double total = 0;
                    foreach (var b in balaTypes)
                    {
                        total = total + BhavaBala[b][_house];
                    }
                    totalBhavaBala[_house] = total;

                }

                return new Shashtiamsa(totalBhavaBala[house]);

            }

        }

        /// <summary>
        /// Bhavadrishti Bala.-Each bhava in a
        /// horoscope remains aspected by certain planets.
        /// Sometimes the aspect cast on a bhava will be positive
        /// and sometimes it will be negative according
        /// as it is aspected by benefics or malefics.
        /// For all 12 houses
        /// </summary>
        public static Dictionary<HouseName, double> CalcBhavaDrishtiBala(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("CalcBhavaDrishtiBala", time), _calcBhavaDrishtiBala);


            //UNDERLYING FUNCTION
            Dictionary<HouseName, double> _calcBhavaDrishtiBala()
            {
                double vdrishti;

                //assign initial negative or positive value based on benefic or malefic planet
                var sp = goodAndBad();


                var drishti = GetDrishtiKendra(time);


                double bala = 0;

                var BhavaDrishtiBala = new Dictionary<HouseName, double>();

                foreach (var house in House.AllHouses)
                {

                    bala = 0;

                    foreach (var planet in PlanetName.AllPlanets)
                    {

                        bala = bala + (sp[planet] * drishti[planet.ToString() + house.ToString()]);

                    }

                    BhavaDrishtiBala[house] = bala;
                }

                return BhavaDrishtiBala;


                //------------------FUNCTIONS---------------------

                Dictionary<PlanetName, int> goodAndBad()
                {

                    var _sp = new Dictionary<PlanetName, int>();

                    //assign initial negative or positive value based on benefic or malefic planet
                    foreach (var p in PlanetName.AllPlanets)
                    {
                        //Though in the earlier pages Mercury is defined either as a subba
                        //(benefic) or papa (malefic) according to its association is with a benefic or
                        //malefic, Mercury for purposes of calculating Drisbtibala of Bbavas is to
                        //be deemed as a full benefic. This is in accord with the injunctions of
                        //classical writers (Gurugnabbyam tu yuktasya poomamekam tu yojayet).

                        if (p == PlanetName.Mercury)
                        {
                            _sp[p] = 1;
                            continue;
                        }

                        if (AstronomicalCalculator.IsPlanetBenefic(p, time))
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

                    foreach (var planet in PlanetName.AllPlanets)
                    {
                        foreach (var houseNo in House.AllHouses)
                        {
                            //house is considered as a Drusya Graha (aspected body)
                            var houseMid = AstronomicalCalculator.GetHouse(houseNo, time1).GetMiddleLongitude();
                            var plantLong = AstronomicalCalculator.GetPlanetNirayanaLongitude(time1, planet);

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

                            if ((planet == PlanetName.Mercury) || (planet == PlanetName.Jupiter))
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
        /// Bhava Digbala.-This is the strength
        /// acquired by the different bhavas falling in the
        /// different groups or types of signs.
        /// For all 12 houses
        /// </summary>
        public static Dictionary<HouseName, double> CalcBhavaDigBala(Time time)
        {

            var BhavaDigBala = new Dictionary<HouseName, double>();

            int dig = 0;

            //for every house
            foreach (var i in House.AllHouses)
            {
                //a particular bhava acquires strength by its mid-point
                //falling in a particular kind of sign.

                //so get mid point of house
                var mid = AstronomicalCalculator.GetHouse(i, time).GetMiddleLongitude().TotalDegrees;
                var houseSign = AstronomicalCalculator.GetHouseSignName((int)i, time);

                //Therefore first find the number of a given Bhava Madhya and subtract
                // it from 1, if the given Bhava Madhya is situated
                // in Vrischika
                if ((mid >= 210.00)
                    && (mid <= 240.00))
                {
                    dig = 1 - (int)i;
                }
                //Subtract it from 4, if the given Bhava
                // Madhya is situated in Mesha, Vrishabha, Simha,
                // first half of Makara or last half of Dhanus.
                else if (((mid >= 0.00) && (mid <= 60.00))
                         || ((mid >= 120.00) && (mid <= 150.00))
                         || ((mid >= 255.00) && (mid <= 285.00)))
                {
                    dig = 4 - (int)i;
                }
                //Subtract it from 7 if in Mithuna, Thula, Kumbha, Kanya or
                // first half of Dhanus
                else if (((mid >= 60.00) && (mid <= 90.00))
                         || ((mid >= 150.00) && (mid <= 210.00))
                         || ((mid >= 300.00) && (mid <= 330.00))
                         || ((mid >= 240.00) && (mid <= 255.00)))
                {
                    dig = 7 - (int)i;
                }
                //and lastly from 1O if in Kataka, Meena and last half of Makara.
                else if (((mid >= 90.00) && (mid <= 120.00))
                         || ((mid >= 330.00) && (mid <= 360.00))
                         || ((mid >= 285.00) && (mid <= 300.00)))
                {
                    dig = 10 - (int)i;
                }


                //If the diffcfrence exceeds 6, subtract it from 12, otherwise
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
                BhavaDigBala[i] = (double)dig * 10;

            }

            return BhavaDigBala;


        }

        /// <summary>
        /// Bhavadhipatbi Bala.-This is the potency
        /// of the lord of the bhava.
        /// For all 12 houses
        /// </summary>
        public static Dictionary<HouseName, double> CalcBhavaAdhipathiBala(Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("CalcBhavaAdhipathiBala", time), _calcBhavaAdhipathiBala);


            //UNDERLYING FUNCTION
            Dictionary<HouseName, double> _calcBhavaAdhipathiBala()
            {
                PlanetName houseLord;

                var BhavaAdhipathiBala = new Dictionary<HouseName, double>();

                foreach (var house in House.AllHouses)
                {

                    //get current house lord
                    houseLord = AstronomicalCalculator.GetLordOfHouse(house, time);

                    //The Shadbala Pinda (aggregate of the Shadbalas) of the lord of the
                    //bhava constitutes its Bhavadhipathi Bala.

                    //get Shadbala Pinda of lord (total strength) in shashtiamsas
                    BhavaAdhipathiBala[house] = GetPlanetShadbalaPinda(houseLord, time).ToDouble();

                }

                return BhavaAdhipathiBala;

            }

        }

        #endregion

        /// <summary>
        ///Checks if planet A is in good aspect to planet B
        ///
        /// Note:
        /// A is transmiter, B is receiver
        /// 
        /// An aspect is good or bad according to the relation
        /// between the aspecting and the aspected body
        /// </summary>
        public static bool IsPlanetInGoodAspectToPlanet(PlanetName receivingAspect, PlanetName transmitingAspect, Time time)
        {
            //check if transmiting planet is aspecting receiving planet
            var isAspecting = AstronomicalCalculator.IsPlanetAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(receivingAspect, transmitingAspect, time);
            var isGood = aspectNature == PlanetToPlanetRelationship.AdhiMitra ||
                         aspectNature == PlanetToPlanetRelationship.Mitra;

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
            var isAspecting = AstronomicalCalculator.IsHouseAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = AstronomicalCalculator.GetPlanetRelationshipWithHouse(receivingAspect, transmitingAspect, time);

            var isGood = aspectNature == PlanetToSignRelationship.Swavarga || //Swavarga - own varga
                         aspectNature == PlanetToSignRelationship.Mitravarga || //Mitravarga - friendly varga
                         aspectNature == PlanetToSignRelationship.AdhiMitravarga; //Adhi Mitravarga - Intimate friend varga


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
        public static double GetPlanetSthanaBalaNeutralPoint(PlanetName planet)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetSthanaBalaNeutralPoint", planet), _getPlanetSthanaBalaNeutralPoint);



            double _getPlanetSthanaBalaNeutralPoint()
            {
                int max = 0, min = 0;

                if (planet == PlanetName.Saturn) { max = 297; min = 59; }
                if (planet == PlanetName.Mars) { max = 362; min = 60; }
                if (planet == PlanetName.Jupiter) { max = 296; min = 77; }
                if (planet == PlanetName.Mercury) { max = 295; min = 47; }
                if (planet == PlanetName.Venus) { max = 284; min = 60; }
                if (planet == PlanetName.Sun) { max = 327; min = 52; }
                if (planet == PlanetName.Moon) { max = 311; min = 54; }

                //calculate neutral point
                var neutralPoint = ((max - min) / 2) + min;

                if (neutralPoint <= 0) { throw new Exception("Planet does not have neutral point!"); }

                return neutralPoint;
            }
        }

        /// <summary>
        /// To determine if Shadvarga bala is strong or weak
        /// a neutral point is set, anything above is strong & below is weak
        ///
        /// Note:
        /// Neutral point is derived from all possible Shadvarga bala values across
        /// 25 years (2000-2025), with 1 hour granularity
        ///
        /// Formula used = ((max-min)/2)+min (min is added because lowest value is not 0)
        /// max = hightest possible value
        /// min = lowest possible value
        /// </summary>
        public static double GetPlanetShadvargaBalaNeutralPoint(PlanetName planet)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("GetPlanetShadvargaBalaNeutralPoint", planet), _getPlanetShadvargaBalaNeutralPoint);



            double _getPlanetShadvargaBalaNeutralPoint()
            {
                int max = 0, min = 0;

                if (planet == PlanetName.Saturn) { max = 150; min = 11; }
                if (planet == PlanetName.Mars) { max = 188; min = 21; }
                if (planet == PlanetName.Jupiter) { max = 172; min = 17; }
                if (planet == PlanetName.Mercury) { max = 150; min = 17; }
                if (planet == PlanetName.Venus) { max = 158; min = 15; }
                if (planet == PlanetName.Sun) { max = 180; min = 17; }
                if (planet == PlanetName.Moon) { max = 165; min = 26; }

                //calculate neutral point
                var neutralPoint = ((max - min) / 2) + min;

                if (neutralPoint <= 0) { throw new Exception("Planet does not have neutral point!"); }

                return neutralPoint;
            }
        }




        /// <summary>
        /// Checks if a planet is in a kendra house (4,7,10)
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName planet, Time time)
        {
            //The 4th, the 7th and the 10th are the Kendras
            var planetHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, planet);

            //check if planet is in kendra
            return planetHouse == 4 || planetHouse == 7 || planetHouse == 10;
        }

        /// <summary>
        /// Checks if the lord of a house is in the specified house.
        /// Example question : Is Lord of 1st house in 2nd house?
        /// </summary>
        public static bool IsHouseLordInHouse(HouseName lordHouse, HouseName occupiedHouse, Time time)
        {
            //get the house lord
            var houseLord = AstronomicalCalculator.GetLordOfHouse(lordHouse, time);

            //get house the lord is in
            var houseIsIn = AstronomicalCalculator.GetHousePlanetIsIn(time, houseLord);

            //if it matches then occuring
            return houseIsIn == (int)occupiedHouse;

        }
    }

}


//--------------ARCHIVED CODE



//POSIBBLY VERY WRONG 
//public static bool IsMoonWeak(Time time)
//{
//    //Moon is weak or new moon from the 10th day of the dark half of the lunar month
//    //to the 5th day of the bright half of the next lunar month

//    //set moon as not weak at first
//    var moonIsWeak = false;

//    //get the lunar date number
//    int lunarDateNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDateNumber();

//    //10th day of dark half to 15th day of dark half = lunar date number 25 to 30
//    if (lunarDateNumber >= 25 && lunarDateNumber <= 30)
//    {
//        moonIsWeak = true;
//    }

//    //1st day of bright half to 5th day of bright half = lunar date number 1 to 5
//    if (lunarDateNumber >= 1 && lunarDateNumber <= 5)
//    {
//        moonIsWeak = true;
//    }

//    return moonIsWeak;
//}

//public static bool IsMoonStrong(Time time)
//{
//    //Moon is full from the 10th day of the bright half
//    //of the lunar month to the 5th day of the dark half
//    //of the same.
//    throw new NotImplementedException();
//}
