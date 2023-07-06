

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissEphNet;



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
    public static partial class AstronomicalCalculator
    {

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        [API("Longitude", "time back to longitude", Category.Astronomical)]
        public static Angle TimeToLongitude(TimeSpan time)
        {
            //TODO function is a candidate for caching
            //degrees is equivelant to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }

        //NORMAL FUNCTIONS
        //FUNCTIONS THAT CALL OTHER FUNCTIONS IN THIS CLASS

        /// <summary>
        /// Gets the ephemris time that is consumed by Swiss Ephemeris
        /// </summary>
        [API("EphemerisTime", "", Category.Astronomical)]
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

        /// <summary>
        /// Gets planet longitude used vedic astrology
        /// Nirayana Longitude = Sayana Longitude corrected to Ayanamsa
        /// Number from 0 to 360, represent the degrees in the zodiac as viewed from earth
        /// Note: Since Niryana is corrected, in actuality 0 degrees will start at Taurus not Aries
        /// </summary>
        [API("NirayanaLongitude")]
        public static Angle GetPlanetNirayanaLongitude(Time time, PlanetName planetName)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetPlanetNirayanaLongitude), time, planetName), _getPlanetNirayanaLongitude);


            //UNDERLYING FUNCTION
            Angle _getPlanetNirayanaLongitude()
            {
                //declare return value
                Angle returnValue;


                //Get sayana longitude on day 
                Angle longitude = GetPlanetSayanaLongitude(time, planetName);


                //3 - Hindu Nirayana Long = Sayana Long — Ayanamsa.

                Angle birthAyanamsa = GetAyanamsa(time);

                //if below ayanamsa add 360 before minus
                returnValue = longitude.TotalDegrees < birthAyanamsa.TotalDegrees
                    ? (longitude + Angle.Degrees360) - birthAyanamsa
                    : longitude - birthAyanamsa;


                return returnValue;
            }


        }

        [API("LunarDay", "", Category.StarsAboveMe)]
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


        /// <summary>
        /// Gets constellation behind the moon (shortcut function)
        /// </summary>
        [API("MoonConstellation", "constellation behind the moon", Category.StarsAboveMe)]
        public static PlanetConstellation GetMoonConstellation(Time time) => GetPlanetConstellation(time, PlanetName.Moon);

        /// <summary>
        /// Gets the constellation behind a planet at a given time
        /// </summary>
        [API("Constellation")]
        public static PlanetConstellation GetPlanetConstellation(Time time, PlanetName planet)
        {
            //get position of planet in longitude
            var planetLongitude = GetPlanetNirayanaLongitude(time, planet);

            //return the constellation behind the planet
            return GetConstellationAtLongitude(planetLongitude);
        }

        [API("Tarabala")]
        public static Tarabala GetTarabala(Time time, Person person)
        {
            int dayRulingConstellationNumber = GetMoonConstellation(time).GetConstellationNumber();

            int birthRulingConstellationNumber = GetMoonConstellation(person.BirthTime).GetConstellationNumber();

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
        /// Chandrabala or lunar strength
        ///
        /// Reference:
        /// Chandrabala. - As we have already said above, the consideration of the
        /// Moon and his position are of much importance in Muhurtha. To be at its
        /// best, the Moon should not occupy in the election chart, a position that
        /// happens to represent the 6th, 8th or 12th from the person's Janma Rasi.
        /// </summary>
        [API("Chandrabala")]
        public static int GetChandrabala(Time time, Person person)
        {
            //TODO Needs to be updated with count sign from sign for better consistency
            //     also possible to leave it as is for better decoupling since this is working fine

            //initialize chandrabala number as 0
            int chandrabalaNumber = 0;

            //get zodiac name & convert to its number
            var dayMoonSignNumber = (int)GetMoonSignName(time);
            var birthMoonSignNumber = (int)GetMoonSignName(person.BirthTime);


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

        [API("MoonSign", "", Category.StarsAboveMe)]
        public static ZodiacName GetMoonSignName(Time time)
        {
            //get zodiac sign behind the moon
            var moonSign = GetPlanetRasiSign(PlanetName.Moon, time);

            //return name of zodiac sign
            return moonSign.GetSignName();
        }

        [API("NithyaYoga", "", Category.StarsAboveMe)]
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

        [API("Karana", "", Category.StarsAboveMe)]
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

        [API("SunSign", "", Category.StarsAboveMe)]
        public static ZodiacSign GetSunSign(Time time)
        {

            //get zodiac sign behind the sun
            var sunSign = GetPlanetRasiSign(PlanetName.Sun, time);

            //return zodiac sign behind sun
            return sunSign;

        }

        ///<summary>
        ///Find time when Sun was in 0.001 degrees
        ///in current sign (just entered sign)
        ///</summary>
        [API("TimeSunEnteredCurrentSign", "", Category.StarsAboveMe)]
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
        [API("TimeSunLeavesCurrentSign", "", Category.StarsAboveMe)]
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

        /// <summary>
        /// Calculates & creates all houses as list
        /// </summary>
        [API("HouseLongitudes", "", Category.Astrology)]
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

        [API("JulianDays")]
        public static double ConvertLmtToJulian(Time time)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(ConvertLmtToJulian), time), _convertLmtToJulian);


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
        [API("PlanetsInConjuction")]
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
        [API("DistanceBetweenPlanets")]
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
        /// Gets longitudinal space between 2 planets
        /// Note :
        /// - Longitude of planet after 360 is 0 degrees,
        ///   when calculating difference this needs to be accounted for
        /// - Expects you to calculate longitude
        /// </summary>
        [API("DistanceBetweenPlanetsLong")]
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
        [API("PlanetsInHouse")]
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
        [API("AllPlanetLongitude")]
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

        /// <summary>
        /// Gets longitude positions of all planets Sayana / Fixed zodiac 
        /// </summary>
        [API("AllPlanetFixedLongitude")]
        public static List<PlanetLongitude> GetAllPlanetFixedLongitude(Time time)
        {

            //get longitudes of all planets
            var sunLongitude = GetPlanetSayanaLongitude(time, PlanetName.Sun);
            var sun = new PlanetLongitude(PlanetName.Sun, sunLongitude);

            var moonLongitude = GetPlanetSayanaLongitude(time, PlanetName.Moon);
            var moon = new PlanetLongitude(PlanetName.Moon, moonLongitude);

            var marsLongitude = GetPlanetSayanaLongitude(time, PlanetName.Mars);
            var mars = new PlanetLongitude(PlanetName.Mars, marsLongitude);

            var mercuryLongitude = GetPlanetSayanaLongitude(time, PlanetName.Mercury);
            var mercury = new PlanetLongitude(PlanetName.Mercury, mercuryLongitude);

            var jupiterLongitude = GetPlanetSayanaLongitude(time, PlanetName.Jupiter);
            var jupiter = new PlanetLongitude(PlanetName.Jupiter, jupiterLongitude);

            var venusLongitude = GetPlanetSayanaLongitude(time, PlanetName.Venus);
            var venus = new PlanetLongitude(PlanetName.Venus, venusLongitude);

            var saturnLongitude = GetPlanetSayanaLongitude(time, PlanetName.Saturn);
            var saturn = new PlanetLongitude(PlanetName.Saturn, saturnLongitude);

            var rahuLongitude = GetPlanetSayanaLongitude(time, PlanetName.Rahu);
            var rahu = new PlanetLongitude(PlanetName.Rahu, rahuLongitude);

            var ketuLongitude = GetPlanetSayanaLongitude(time, PlanetName.Ketu);
            var ketu = new PlanetLongitude(PlanetName.Ketu, ketuLongitude);



            //add longitudes to list
            var allPlanetLongitudeList = new List<PlanetLongitude>
            {
                sun, moon, mars, mercury, jupiter, venus, saturn, ketu, rahu
            };


            //return list;
            return allPlanetLongitudeList;
        }

        [API("HousePlanetIsIn")]
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
        [API("LordOfHouse")]
        public static PlanetName GetLordOfHouse(HouseName houseNumber, Time time)
        {
            //get sign name based on house number //TODO Change to use house name instead of casting to int
            var houseSignName = AstronomicalCalculator.GetHouseSignName((int)houseNumber, time);

            //get the lord of the house sign
            var lordOfHouseSign = AstronomicalCalculator.GetLordOfZodiacSign(houseSignName);

            return lordOfHouseSign;
        }

        /// <summary>
        /// The lord of a bhava is
        /// the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// List overload to GetLordOfHouse (above method)
        /// </summary>
        [API("LordOfHouseList")]
        public static List<PlanetName> GetLordOfHouseList(List<HouseName> houseList, Time time)
        {
            var returnList = new List<PlanetName>();
            foreach (var house in houseList)
            {
                var tempLord = GetLordOfHouse(house, time);
                returnList.Add(tempLord);
            }

            return returnList;
        }

        /// <summary>
        /// Checks if the inputed sign was the sign of the house during the inputed time
        /// </summary>
        [API("IsHouseSignName")]
        public static bool IsHouseSignName(int house, ZodiacName sign, Time time) => GetHouseSignName(house, time) == sign;

        /// <summary>
        /// Gets the zodiac sign at middle longitude of the house.
        /// </summary>
        [API("HouseSignName")]
        public static ZodiacName GetHouseSignName(int houseNumber, Time time)
        {

            //get all houses
            var allHouses = AstronomicalCalculator.GetHouses(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseNumber() == houseNumber);

            //get sign of the specified house
            //Note :
            //When the middle longitude has just entered a new sign,
            //rounding the longitude shows better accuracy.
            //Example, with middle longitude 90.4694, becomes Cancer (0°28'9"),
            //but predictive results points to Gemini (30°0'0"), so rounding is implemented
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var roundedMiddleLongitude = Angle.FromDegrees(Math.Round(middleLongitude.TotalDegrees, 4)); //rounded to 5 places for accuracy
            var houseSignName = AstronomicalCalculator.GetZodiacSignAtLongitude(roundedMiddleLongitude).GetSignName();

            //for sake of testing, if sign is changed due to rounding, then log it
            var unroundedSignName = AstronomicalCalculator.GetZodiacSignAtLongitude(middleLongitude).GetSignName();

#if DEBUG_LOG
            if (unroundedSignName != houseSignName)
                {
                    LibLogger.Debug($"Due to rounding sign changed from {unroundedSignName} to {houseSignName}");
                }
#endif


            //return the name of house sign
            return houseSignName;
        }

        [API("NavamsaSignNameFromLongitude")]
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
        [API("SignCountedFromInputSign")]
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
        [API("HouseCountedFromInputHouse")]
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
        [API("Sign")]
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
        /// Checks if a given planet is in a given sign at a given time
        /// </summary>
        [API("IsPlanetInSign")]
        public static bool IsPlanetInSign(PlanetName planetName, ZodiacName signInput, Time time)
        {
            var currentSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

            //check if sign match
            return currentSign == signInput;
        }

        /// <summary>
        /// Get navamsa sign of planet
        /// </summary>
        [API("Navamsa")]
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
        [API("SignsPlanetIsAspecting")]
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
        [API("Navamsa")]
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

        [API("Thrimsamsa")]
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

        /// <summary>
        /// When a sign is divided into 12 equal parts each is called a dwadasamsa and measures 2.5 degrees.
        /// The Bhachakra can thus he said to contain 12x12=144 Dwadasamsas. The lords of the 12
        /// Dwadasamsas in a sign are the lords of the 12 signs from it, i.e.,
        /// the lord of the first Dwadasamsa in Mesha is Kuja, that of the second Sukra and so on.
        /// </summary>
        [API("Dwadasamsa")]
        public static ZodiacName GetPlanetDwadasamsaSign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

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
            var dwadasamsaSign = GetSignCountedFromInputSign(planetSignName, dwadasamsaNumber);

            return dwadasamsaSign;
        }

        [API("Saptamsa")]
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

        [API("Drekkana")]
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

        /// <summary>
        /// Similar to Exaltation but covers a range not just a point
        /// Moolathrikonas, these are positions similar to exaltation.
        /// NOTE:
        /// - No moolatrikone for Rahu & Ketu, no error will be raised
        /// </summary>
        [API("Moolatrikona")]
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


        /// <summary>
        /// Gets a planet's relationship to a sign, based on the relation to the lord
        /// Note :
        /// - Moolatrikona, Debilited & Exalted is not calculated heres
        /// - Rahu & ketu not accounted for
        /// </summary>
        [API("PlanetRelationshipWithSign")]
        public static PlanetToSignRelationship GetPlanetRelationshipWithSign(PlanetName planetName, ZodiacName zodiacSignName, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = planetName.Name == PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planetName.Name == PlanetName.PlanetNameEnum.Ketu;
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
            var lordOfSign = AstronomicalCalculator.GetLordOfZodiacSign(zodiacSignName);

            //if lord of sign is same as input planet
            if (planetName == lordOfSign)
            {
                //return own varga, swavarga
                return PlanetToSignRelationship.OwnVarga;
            }

            //else, get relationship between input planet and lord of sign
            PlanetToPlanetRelationship relationshipToLordOfSign = GetPlanetCombinedRelationshipWithPlanet(planetName, lordOfSign, time);

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
        /// In order to find the strengths of planets we have
        /// to mix the temporary relations and the permanent
        /// relations. Thus a temporary enemy plus a permanent
        /// or natural enemy becomes a bitter enemy.
        /// </summary>
        [API("PlanetCombinedRelationshipWithPlanet")]
        public static PlanetToPlanetRelationship GetPlanetCombinedRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {

            //no calculation for rahu and ketu here
            var isRahu = mainPlanet.Name == PlanetName.PlanetNameEnum.Rahu;
            var isKetu = mainPlanet.Name == PlanetName.PlanetNameEnum.Ketu;
            var isRahu2 = secondaryPlanet.Name == PlanetName.PlanetNameEnum.Rahu;
            var isKetu2 = secondaryPlanet.Name == PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu || isRahu2 || isKetu2;
            if (isRahuKetu) { return PlanetToPlanetRelationship.Empty; }


            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }

            //get planet's permanent relationship
            PlanetToPlanetRelationship planetPermanentRelationship = GetPlanetPermanentRelationshipWithPlanet(mainPlanet, secondaryPlanet);

            //get planet's temporary relationship
            PlanetToPlanetRelationship planetTemporaryRelationship = GetPlanetTemporaryRelationshipWithPlanet(mainPlanet, secondaryPlanet, time);

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
        /// Gets a planets relationship with a house,
        /// Based on the relation between the planet and the lord of the sign of the house
        /// Note : needs verification if this is correct
        /// </summary>
        [API("PlanetRelationshipWithHouse")]
        public static PlanetToSignRelationship GetPlanetRelationshipWithHouse(HouseName house, PlanetName planet, Time time)
        {
            //get sign the house is in
            var houseSign = GetHouseSignName((int)house, time);

            //get the planet's relationship with the sign
            var relationship = GetPlanetRelationshipWithSign(planet, houseSign, time);

            return relationship;
        }

        /// <summary>
        /// Temporary Friendship
        /// Planets found in the 2nd, 3rd, 4th, 10th, 11th
        /// and 12th signs from any other planet becomes the
        /// latter's temporary friends. The others are its enemies.
        /// </summary>
        [API("PlanetTemporaryRelationshipWithPlanet")]
        public static PlanetToPlanetRelationship GetPlanetTemporaryRelationshipWithPlanet(PlanetName mainPlanet, PlanetName secondaryPlanet, Time time)
        {
            //if main planet & secondary planet is same, then it is own plant (same planet), end here
            if (mainPlanet == secondaryPlanet) { return PlanetToPlanetRelationship.SamePlanet; }


            //1.0 get planet's friends
            var friendlyPlanetList = AstronomicalCalculator.GetPlanetTemporaryFriendList(mainPlanet, time);

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

        //public static List<PlanetName> GetPlanetTemporaryEnemyList(PlanetName planetName, Time time)
        //{
        //    //Signs where enemy planets are located 1,5,6,7,8,9

        //    //get sign planet is currently in
        //    var planetSignName = AstronomicalCalculator.GetPlanetRasiSign(planetName, time).GetSignName();

        //    //Get signs of enemies of main planet
        //    //get planets in 1
        //    var sign1FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 1);
        //    //get planets in 5
        //    var sign5FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 5);
        //    //get planets in 6
        //    var sign6FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 6);
        //    //get planets in 7
        //    var sign7FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 7);
        //    //get planets in 8
        //    var sign8FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 8);
        //    //get planets in 9
        //    var sign9FromMainPlanet = AstronomicalCalculator.GetSignCountedFromInputSign(planetSignName, 9);

        //    //add signs of enemy planets to a list
        //    var signsOfEnemyPlanet = new List<ZodiacName>(){sign1FromMainPlanet, sign5FromMainPlanet, sign6FromMainPlanet,
        //                                                sign7FromMainPlanet, sign8FromMainPlanet, sign9FromMainPlanet};

        //    //declare list of enemy planets
        //    var enemyPlanetList = new List<PlanetName>();

        //    //loop through the signs and fill the enemy planet list
        //    foreach (var sign in signsOfEnemyPlanet)
        //    {
        //        //get the planets in the current sign
        //        var enemyPlanetsInThisSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

        //        //add the planets in to the list
        //        enemyPlanetList.AddRange(enemyPlanetsInThisSign);
        //    }


        //    //remove rahu & ketu from list
        //    enemyPlanetList.Remove(PlanetName.Rahu);
        //    enemyPlanetList.Remove(PlanetName.Ketu);

        //    //remove the main planet from list
        //    enemyPlanetList.Remove(planetName);


        //    return enemyPlanetList;

        //}

        /// <summary>
        /// Gets all the planets in a sign
        /// </summary>
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

        /// <summary>
        /// The planets in -the 2nd, 3rd, 4th, 10th, 11th and
        /// 12th signs from any other planet becomes his
        /// (Tatkalika) friend.
        /// </summary>
        [API("TemporaryFriends")]
        public static List<PlanetName> GetPlanetTemporaryFriendList(PlanetName planetName, Time time)
        {
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

        [API("Panchaka", "", Category.StarsAboveMe)]
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

        [API("LordOfWeekday", "", Category.StarsAboveMe)]
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
        [API("Hora", "A hora is equal to 1/24th part of a day", Category.StarsAboveMe)]
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


        [API("PlanetHoraSign", "", Category.StarsAboveMe)]
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
        /// get sunrise time for that day
        /// </summary>
        [API("Sunrise")]
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
        [API("Sunset")]
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
        [API("LocalNoonTime", "Sun is exactly overhead at location")]
        public static DateTime GetNoonTime(Time time)
        {
            //get apparent time
            var localApparentTime = AstronomicalCalculator.GetLocalApparentTime(time);
            var apparentNoon = new DateTime(localApparentTime.Year, localApparentTime.Month, localApparentTime.Day, 12, 0, 0);

            return apparentNoon;
        }


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
            var isAspecting = AstronomicalCalculator.IsHouseAspectedByPlanet(receivingAspect, transmitingAspect, time);

            //if not aspecting at all, end here as not occuring
            if (!isAspecting) { return false; }

            //check if it is a good aspect
            var aspectNature = AstronomicalCalculator.GetPlanetRelationshipWithHouse(receivingAspect, transmitingAspect, time);

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
        [API("PlanetSthanaBalaNeutralPoint", "", Category.Astronomical)]
        public static double GetPlanetSthanaBalaNeutralPoint(PlanetName planet)
        {
            //no calculation for rahu and ketu here
            var isRahu = planet.Name == PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planet.Name == PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return 0; }


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
        /// Formula used = ((max-min)/2)+min (add min to get exact neutral point from 0 range)
        /// max = hightest possible value
        /// min = lowest possible value
        /// </summary>
        [API("ShadvargaBalaNeutralPoint")]
        public static double GetPlanetShadvargaBalaNeutralPoint(PlanetName planet)
        {

            //no calculation for rahu and ketu here
            var isRahu = planet.Name == PlanetName.PlanetNameEnum.Rahu;
            var isKetu = planet.Name == PlanetName.PlanetNameEnum.Ketu;
            var isRahuKetu = isRahu || isKetu;
            if (isRahuKetu) { return 0; }


            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(GetPlanetShadvargaBalaNeutralPoint), planet), _getPlanetShadvargaBalaNeutralPoint);



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
        [API("InKendra")]
        public static bool IsPlanetInKendra(PlanetName planet, Time time)
        {
            //The 4th, the 7th and the 10th are the Kendras
            var planetHouse = GetHousePlanetIsIn(time, planet);

            //check if planet is in kendra
            var isPlanetInKendra = planetHouse == 1 || planetHouse == 4 || planetHouse == 7 || planetHouse == 10;

            return isPlanetInKendra;
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

        /// <summary>
        /// Checks if a planet is conjuct with an evil/malefic planet
        /// </summary>
        [API("ConjunctWithMalefics")]
        public static bool IsPlanetConjunctWithMaleficPlanets(PlanetName planetName, Time time)
        {
            //get all the planets conjuct with inputed planet
            var planetsInConjunct = AstronomicalCalculator.GetPlanetsInConjuction(time, planetName);

            //get all evil planets
            var evilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

            //check if any conjunct planet is an evil one
            var evilFound = planetsInConjunct.FindAll(planet => evilPlanets.Contains(planet)).Any();
            return evilFound;

        }

        /// <summary>
        /// Checks if any evil/malefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsMaleficPlanetInHouse(int houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = AstronomicalCalculator.GetPlanetsInHouse(houseNumber, time);

            //get all evil planets
            var evilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

            //check if any planet in house is an evil one
            var evilFound = planetsInHouse.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;

        }

        /// <summary>
        /// Checks if any good/benefic planets are in a house
        /// Note : Planet to house relationship not account for
        /// TODO Account for planet to sign relationship, find reference
        /// </summary>
        public static bool IsBeneficPlanetInHouse(int houseNumber, Time time)
        {
            //get all the planets in the house
            var planetsInHouse = AstronomicalCalculator.GetPlanetsInHouse(houseNumber, time);

            //get all good planets
            var goodPlanets = AstronomicalCalculator.GetBeneficPlanetList(time);

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
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

            //check if any planet in sign is an evil one
            var evilFound = planetsInSign.FindAll(planet => evilPlanets.Contains(planet)).Any();

            return evilFound;
        }

        /// <summary>
        /// Gets list of evil/malefic planets in a sign
        /// </summary>
        public static List<PlanetName> GetMaleficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

            //get all evil planets
            var evilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

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
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = AstronomicalCalculator.GetBeneficPlanetList(time);

            //check if any planet in sign is an good one
            var goodFound = planetsInSign.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;
        }

        /// <summary>
        /// Gets any good/benefic planets in a sign
        /// </summary>
        public static List<PlanetName> GetBeneficPlanetListInSign(ZodiacName sign, Time time)
        {
            //get all the planets in the sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(sign, time);

            //get all good planets
            var goodPlanets = AstronomicalCalculator.GetBeneficPlanetList(time);

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
            var evilPlanets = AstronomicalCalculator.GetMaleficPlanetList(time);

            //check if any evil planet is aspecting the inputed house
            var evilFound = evilPlanets.FindAll(evilPlanet => AstronomicalCalculator.IsHouseAspectedByPlanet(house, evilPlanet, time)).Any();

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
            var goodPlanets = AstronomicalCalculator.GetBeneficPlanetList(time);

            //check if any good planet is aspecting the inputed house
            var goodFound = goodPlanets.FindAll(goodPlanet => AstronomicalCalculator.IsHouseAspectedByPlanet(house, goodPlanet, time)).Any();

            return goodFound;

        }

        /// <summary>
        /// Checks if a planet is receiving aspects from an evil planet
        /// </summary>
        [API("AspectedByMalefics")]
        public static bool IsPlanetAspectedByMaleficPlanets(PlanetName lord, Time time)
        {
            //get list of evil planets
            var evilPlanets = GetMaleficPlanetList(time);

            //check if any of the evil planets is aspecting inputed planet
            var evilAspectFound = evilPlanets.FindAll(evilPlanet =>
                IsPlanetAspectedByPlanet(lord, evilPlanet, time)).Any();
            return evilAspectFound;

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
        [API("ArudhaLagna", "bearing on the financial status", Category.StarsAboveMe)]
        public static ZodiacName GetArudhaLagnaSign(Time time)
        {
            //get janma lagna
            var janmaLagna = AstronomicalCalculator.GetHouseSignName(1, time);

            //get sign lord of janma lagna is in
            var lagnaLord = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var lagnaLordSign = AstronomicalCalculator.GetPlanetRasiSign(lagnaLord, time).GetSignName();

            //count the signs from janma to the sign the lord is in
            var janmaToLordCount = AstronomicalCalculator.CountFromSignToSign(janmaLagna, lagnaLordSign);

            //use the above count to find arudha sign from lord's sign
            var arudhaSign = AstronomicalCalculator.GetSignCountedFromInputSign(lagnaLordSign, janmaToLordCount);

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
        public static bool IsPlanetInHouse(Time time, PlanetName planet, int houseNumber)
        {
            return AstronomicalCalculator.GetHousePlanetIsIn(time, planet) == houseNumber;
        }

        /// <summary>
        /// Checks if a planet is in a longitude where it's in Debilitated
        /// Note : Rahu & ketu accounted for
        /// </summary>
        [API("Debilitated")]
        public static bool IsPlanetDebilitated(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = GetPlanetNirayanaLongitude(time, planet);

            //convert planet longitude to zodiac sign
            var planetZodiac = GetZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Debilited
            var point = GetPlanetDebilitationPoint(planet);

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
        [API("Exalted")]
        public static bool IsPlanetExalted(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = GetPlanetNirayanaLongitude(time, planet);

            //convert planet longitude to zodiac sign
            var planetZodiac = GetZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Exaltation
            var point = GetPlanetExaltationPoint(planet);

            //check if planet is in Exaltation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var sameDegree = planetZodiac.GetDegreesInSign().Degrees == point.GetDegreesInSign().Degrees;
            var planetIsExaltation = sameSign && sameDegree;

            return planetIsExaltation;
        }

        [API("LunarMonth", "name of vedic month", Category.StarsAboveMe)]
        public static LunarMonth GetLunarMonth(Time time)
        {

            return LunarMonth.Empty;

            //TODO NEEDS WORK
            throw new NotImplementedException();


            //get this months full moon date
            var fullMoonTime = getFullMoonTime();

            //sunrise
            var x = GetSunriseTime(time);
            var y = GetMoonConstellation(x).GetConstellationName();

        Calculate:
            //get the constellation behind the moon
            var constellation = GetMoonConstellation(fullMoonTime).GetConstellationName();

            //go back one constellation
            //constellation = constellation - 1;

            switch (constellation)
            {
                case ConstellationName.Aswini:
                    return LunarMonth.Aswijam;
                    break;
                case ConstellationName.Bharani:
                    break;
                case ConstellationName.Krithika:
                    return LunarMonth.Karthikam;
                    break;
                case ConstellationName.Rohini:
                    break;
                case ConstellationName.Mrigasira:
                case ConstellationName.Aridra:
                    return LunarMonth.Margasiram;
                    break;
                case ConstellationName.Punarvasu:
                    break;
                case ConstellationName.Pushyami:
                    return LunarMonth.Pooshiam;
                    break;
                case ConstellationName.Aslesha:
                    break;
                case ConstellationName.Makha:
                    return LunarMonth.Magham;
                    break;
                case ConstellationName.Pubba:
                    return LunarMonth.Phalgunam;
                    break;
                case ConstellationName.Uttara:
                    break;
                case ConstellationName.Hasta:
                    break;
                case ConstellationName.Chitta:
                    return LunarMonth.Chitram;
                    break;
                case ConstellationName.Swathi:
                    break;
                case ConstellationName.Vishhaka:
                    return LunarMonth.Visakham;
                    break;
                case ConstellationName.Anuradha:
                    break;
                case ConstellationName.Jyesta:
                    return LunarMonth.Jaistam;
                    break;
                case ConstellationName.Moola:
                    break;
                case ConstellationName.Poorvashada:
                    return LunarMonth.Ashadam;
                    break;
                case ConstellationName.Uttarashada:
                    break;
                case ConstellationName.Sravana:
                    return LunarMonth.Sravanam;
                    break;
                case ConstellationName.Dhanishta:
                    break;
                case ConstellationName.Satabhisha:
                    break;
                case ConstellationName.Poorvabhadra:
                    return LunarMonth.Bhadrapadam;
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
                int lunarDayNumber = GetLunarDay(time).GetLunarDateNumber();

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
        [API("IsFullMoon", "moon day 15", Category.StarsAboveMe)]
        public static bool IsFullMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = AstronomicalCalculator.GetLunarDay(time).GetLunarDayNumber();

            //if day 15, it is full moon
            return lunarDayNumber == 15;
        }



        /// <summary>
        /// Check if it is a Water / Aquatic sign
        /// Water Signs: this category include Cancer, Scorpio and Pisces.
        /// </summary>
        public static bool IsAquaticSign(ZodiacName moonSign) => moonSign is ZodiacName.Cancer or ZodiacName.Scorpio or ZodiacName.Pisces;

        /// <summary>
        /// Check if it is a Fire sign
        /// Fire Signs: this sign encloses Aries, Leo and Sagittarius.
        /// </summary>
        public static bool IsFireSign(ZodiacName moonSign) => moonSign is ZodiacName.Aries or ZodiacName.Leo or ZodiacName.Sagittarius;

        /// <summary>
        /// Check if it is a Earth sign
        /// Earth Signs: it contains Taurus, Virgo and Capricorn.
        /// </summary>
        public static bool IsEarthSign(ZodiacName moonSign) => moonSign is ZodiacName.Taurus or ZodiacName.Virgo or ZodiacName.Capricornus;

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
        public static EventNature GetPlanetAntaramNature(Person person, PlanetName planet)
        {
            //todo account for rahu & ketu
            //rahu & ketu not sure for now, just return neutral
            if (planet == PlanetName.Rahu || planet == PlanetName.Ketu) { return EventNature.Neutral; }

            //get nature from person's lagna
            var planetNature = GetNatureFromLagna();

            //if nature is neutral then use nature of relation to current house
            //assumed that bad relation to sign is bad planet (todo upgrade to bindu points)
            //note: generaly speaking a neutral planet shloud not exist, either good or bad
            if (planetNature == EventNature.Neutral)
            {
                var _planetCurrentHouse = GetHousePlanetIsIn(person.BirthTime, planet);

                var _currentHouseRelation = GetPlanetRelationshipWithHouse((HouseName)_planetCurrentHouse, planet, person.BirthTime);

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
                var personLagna = GetHouseSignName(1, person.BirthTime);

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
                        good = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Sun };
                        bad = new List<PlanetName>() { PlanetName.Saturn, PlanetName.Mercury, PlanetName.Venus };
                        break;
                    //Taurus - Saturn is the most auspicious and powerful
                    // planet. Jupiter, Venus and the Moon are evil planets. Saturn
                    // alone produces Rajayoga. The native will be killed in the
                    // periods and sub-periods of Jupiter, Venus and the Moon if
                    // they get death-inflicting powers.
                    case ZodiacName.Taurus:
                        good = new List<PlanetName>() { PlanetName.Saturn };
                        bad = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Venus, PlanetName.Moon };
                        break;
                    //Gemini - Mars, Jupiter and the Sun are evil. Venus alone
                    // is most beneficial and in conjunction with Saturn in good signs
                    // produces and excellent career of much fame. Combination
                    // of Saturn and Jupiter produces similar results as in Aries.
                    // Venus and Mercury, when well associated, cause Rajayoga.
                    // The Moon will not kill the person even though possessed of
                    // death-inflicting powers.
                    case ZodiacName.Gemini:
                        good = new List<PlanetName>() { PlanetName.Venus };
                        bad = new List<PlanetName>() { PlanetName.Mars, PlanetName.Jupiter, PlanetName.Sun };
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
                        good = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Mars };
                        bad = new List<PlanetName>() { PlanetName.Venus, PlanetName.Mercury };
                        break;
                    //Leo - Mars is the most auspicious and favourable planet.
                    // The combination of Venus and Jupiter does not cause Rajayoga
                    // but the conjunction of Jupiter and Mars in favourable
                    // houses produce Rajayoga. Saturn, Venus and Mercury are
                    // evil. Saturn does not kill the native when he has the maraka
                    // power but Mercury and other evil planets inflict death when
                    // they get maraka powers.
                    case ZodiacName.Leo:
                        good = new List<PlanetName>() { PlanetName.Mars };
                        bad = new List<PlanetName>() { PlanetName.Saturn, PlanetName.Venus, PlanetName.Mercury };
                        break;
                    //Virgo - Venus alone is the most powerful. Mercury and
                    // Venus when combined together cause Rajayoga. Mars and
                    // the Moon are evil. The Sun does not kill the native even if
                    // be becomes a maraka but Venus, the Moon and Jupiter will
                    // inflict death when they are possessed of death-infticting power.
                    case ZodiacName.Virgo:
                        good = new List<PlanetName>() { PlanetName.Venus };
                        bad = new List<PlanetName>() { PlanetName.Mars, PlanetName.Moon };
                        break;
                    // Libra - Saturn alone causes Rajayoga. Jupiter, the Sun
                    // and Mars are inauspicious. Mercury and Saturn produce good.
                    // The conjunction of the Moon and Mercury produces Rajayoga.
                    // Mars himself will not kill the person. Jupiter, Venus
                    // and Mars when possessed of maraka powers certainly kill the
                    // nalive.
                    case ZodiacName.Libra:
                        good = new List<PlanetName>() { PlanetName.Saturn, PlanetName.Mercury };
                        bad = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Sun, PlanetName.Mars };
                        break;
                    //Scorpio - Jupiter is beneficial. The Sun and the Moon
                    // produce Rajayoga. Mercury and Venus are evil. Jupiter,
                    // even if be becomes a maraka, does not inflict death. Mercury
                    // and other evil planets, when they get death-inlflicting powers,
                    // do not certainly spare the native.
                    case ZodiacName.Scorpio:
                        good = new List<PlanetName>() { PlanetName.Jupiter };
                        bad = new List<PlanetName>() { PlanetName.Mercury, PlanetName.Venus };
                        break;
                    //Sagittarius - Mars is the best planet and in conjunction
                    // with Jupiter, produces much good. The Sun and Mars also
                    // produce good. Venus is evil. When the Sun and Mars
                    // combine together they produce Rajayoga. Saturn does not
                    // bring about death even when he is a maraka. But Venus
                    // causes death when be gets jurisdiction as a maraka planet.
                    case ZodiacName.Sagittarius:
                        good = new List<PlanetName>() { PlanetName.Mars };
                        bad = new List<PlanetName>() { PlanetName.Venus };
                        break;
                    //Capricornus - Venus is the most powerful planet and in
                    // conjunction with Mercury produces Rajayoga. Mars, Jupiter
                    // and the Moon are evil.
                    case ZodiacName.Capricornus:
                        good = new List<PlanetName>() { PlanetName.Venus };
                        bad = new List<PlanetName>() { PlanetName.Mars, PlanetName.Jupiter, PlanetName.Moon };
                        break;
                    //Aquarius - Venus alone is auspicious. The combination of
                    // Venus and Mars causes Rajayoga. Jupiter and the Moon are
                    // evil.
                    case ZodiacName.Aquarius:
                        good = new List<PlanetName>() { PlanetName.Venus };
                        bad = new List<PlanetName>() { PlanetName.Jupiter, PlanetName.Moon };
                        break;
                    //Pisces - The Moon and Mars are auspicious. Mars is
                    // most powerful. Mars with the Moon or Jupiter causes Rajayoga.
                    // Saturn, Venus, the Sun and Mercury are evil. Mars
                    // himself does not kill the person even if he is a maraka.
                    case ZodiacName.Pisces:
                        good = new List<PlanetName>() { PlanetName.Moon, PlanetName.Mars };
                        bad = new List<PlanetName>() { PlanetName.Saturn, PlanetName.Venus, PlanetName.Sun, PlanetName.Mercury };
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
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Taurus:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars
                           || planetName == PlanetName.Mercury || planetName == PlanetName.Saturn;
                case ZodiacName.Gemini:
                    return planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Cancer:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Leo:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars;
                case ZodiacName.Virgo:
                    return planetName == PlanetName.Venus;
                case ZodiacName.Libra:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Scorpio:
                    return planetName == PlanetName.Jupiter || planetName == PlanetName.Sun || planetName == PlanetName.Moon;
                case ZodiacName.Sagittarius:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars;
                case ZodiacName.Capricornus:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Aquarius:
                    return planetName == PlanetName.Venus || planetName == PlanetName.Mars
                           || planetName == PlanetName.Sun || planetName == PlanetName.Saturn;
                case ZodiacName.Pisces:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Moon;
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
                    return planetName == PlanetName.Venus || planetName == PlanetName.Mercury || planetName == PlanetName.Saturn;
                case ZodiacName.Taurus:
                    return planetName == PlanetName.Moon || planetName == PlanetName.Jupiter || planetName == PlanetName.Venus;
                case ZodiacName.Gemini:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Cancer:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Leo:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Virgo:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Moon || planetName == PlanetName.Jupiter;
                case ZodiacName.Libra:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Moon || planetName == PlanetName.Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == PlanetName.Saturn || planetName == PlanetName.Venus || planetName == PlanetName.Mercury;
                case ZodiacName.Capricornus:
                    return planetName == PlanetName.Moon || planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == PlanetName.Jupiter || planetName == PlanetName.Moon;
                case ZodiacName.Pisces:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mercury
                           || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
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
                    return planetName == PlanetName.Sun;
                case ZodiacName.Taurus:
                    return planetName == PlanetName.Saturn;
                case ZodiacName.Gemini:
                    return planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Cancer:
                    return planetName == PlanetName.Mars;
                case ZodiacName.Leo:
                    return planetName == PlanetName.Mars;
                case ZodiacName.Virgo:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus;
                case ZodiacName.Libra:
                    return planetName == PlanetName.Moon || planetName == PlanetName.Mercury || planetName == PlanetName.Saturn;
                case ZodiacName.Scorpio:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Moon;
                case ZodiacName.Sagittarius:
                    return planetName == PlanetName.Sun || planetName == PlanetName.Mars;
                case ZodiacName.Capricornus:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus;
                case ZodiacName.Aquarius:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Venus;
                case ZodiacName.Pisces:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Jupiter || planetName == PlanetName.Moon;
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
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Saturn;
                case ZodiacName.Taurus:
                    return planetName == PlanetName.Jupiter || planetName == PlanetName.Venus;
                case ZodiacName.Gemini:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Cancer:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus;
                case ZodiacName.Leo:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus;
                case ZodiacName.Virgo:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Libra:
                    return planetName == PlanetName.Jupiter;
                case ZodiacName.Scorpio:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Sagittarius:
                    return planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
                case ZodiacName.Capricornus:
                    return planetName == PlanetName.Mars || planetName == PlanetName.Jupiter;
                case ZodiacName.Aquarius:
                    return planetName == PlanetName.Mars;
                case ZodiacName.Pisces:
                    return planetName == PlanetName.Mercury || planetName == PlanetName.Venus || planetName == PlanetName.Saturn;
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
            var isRahuKetu = planetName == PlanetName.Rahu || planetName == PlanetName.Ketu;

            //get current house
            var _planetCurrentHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, planetName);

            //relatioship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : AstronomicalCalculator.GetPlanetRelationshipWithHouse((HouseName)_planetCurrentHouse, planetName, time);

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
        /// Swiss Ephemeris "swe_calc" wrapper for open API 
        /// </summary>
        [API("SwissEphemeris")]
        public static dynamic swe_calc_wrapper(Time time, PlanetName planetName)
        {
            //Converts LMT to UTC (GMT)
            //DateTimeOffset utcDate = lmtDateTime.ToUniversalTime();

            int iflag = 2;//SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
            double[] results = new double[6];
            string err_msg = "";
            double jul_day_ET;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_ET = AstronomicalCalculator.TimeToEphemerisTime(time);

            //convert planet name, compatible with Swiss Eph
            int swissPlanet = Tools.VedAstroToSwissEph(planetName);

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, swissPlanet, iflag, results, ref err_msg);

            //data in results at index 0 is longitude
            var sweCalcResults = new
            {
                Longitude = results[0],
                Latitude = results[1],
                DistanceAU = results[2],
                SpeedLongitude = results[3],
                SpeedLatitude = results[4],
                SpeedDistance = results[5]
            };

            return sweCalcResults;
        }


        /// <summary>
        /// Checks if a planet is same house (not nessarly conjunct) with the lord of a certain house
        /// Example : Is Sun joined with lord of 9th?
        /// </summary>
        public static bool IsPlanetSameHouseWithHouseLord(Time birthTime, int houseNumber, PlanetName planet)
        {
            //get house of the lord in question
            var houseLord = GetLordOfHouse((HouseName)houseNumber, birthTime);
            var houseLordHouse = GetHousePlanetIsIn(birthTime, houseLord);

            //get house of input planet
            var inputPlanetHouse = GetHousePlanetIsIn(birthTime, planet);

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


        public static IEnumerable<MethodInfo> GetTimeHouseCalcs()
        {
            var returnList = new List<MethodInfo>();

            //get all calculators that can work with the inputed data
            var calculatorClass = typeof(AstronomicalCalculator);

            var calculators1 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == typeof(HouseName)  //planet name
                                     && parameter[1].ParameterType == typeof(Time)        //birth time
                               select calculatorInfo;

            //second possible order, technically should be aligned todo
            var calculators3 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 2 //only 2 params
                                     && parameter[0].ParameterType == typeof(Time)  //birth time
                                     && parameter[1].ParameterType == typeof(HouseName)        //planet name
                               select calculatorInfo;

            //these are for calculators with static tag data
            var calculators2 = from calculatorInfo in calculatorClass.GetMethods()
                               let parameter = calculatorInfo.GetParameters()
                               where parameter.Length == 1 //only 2 params
                                     && parameter[0].ParameterType == typeof(HouseName)  //planet name
                               select calculatorInfo;


            returnList.AddRange(calculators1);
            returnList.AddRange(calculators2);
            returnList.AddRange(calculators3);

            return returnList;

        }

        /// <summary>
        /// Based on Shadvarga get nature of house for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int GetHouseNatureScore(Time personBirthTime, HouseName inputHouse)
        {
            //if no house then no score
            if (inputHouse == HouseName.Empty)
            {
                return 0;
            }

            //get house score
            var houseStrength = GetHouseStrength(inputHouse, personBirthTime).ToDouble();

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
        /// Based on Shadvarga get nature of planet for a person,
        /// nature in number form to for easy calculation into summary
        /// good = 1, bad = -1, neutral = 0
        /// specially made method for life chart summary
        /// </summary>
        public static int GetPlanetNatureScore(Time personBirthTime, PlanetName inputPlanet)
        {
            //get house score
            var planetStrength = GetPlanetShadbalaPinda(inputPlanet, personBirthTime).ToDouble();


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

        public enum Varna
        {
            SudraServant = 1,
            VaisyaWorkmen = 2,
            KshatriyaWarrior = 3,
            BrahminScholar = 4
        }

        /// <summary>
        /// Get a person's varna or color
        /// A person's varna can be observed in real life
        /// </summary>
        public static Varna GetBirthVarna(Time birthTime)
        {
            //get ruling sign
            var ruleSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, birthTime).GetSignName();

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
                    case ZodiacName.Capricornus:
                        return Varna.SudraServant;

                    default: throw new Exception("");
                }
            }


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
