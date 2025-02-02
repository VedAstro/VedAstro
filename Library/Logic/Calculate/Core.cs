using SwissEphNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VedAstro.Library.PlanetName;

namespace VedAstro.Library
{
    public partial class Calculate
    {
        /// <summary>
        /// Gets all houses owned by a planet at a given time 
        /// </summary>
        public static List<HouseName> HousesOwnedByPlanet(PlanetName inputPlanet, Time time)
        {
            //Given a planet, return Zodiac Signs owned by it Ex. Ju, returns Sag an Pis
            var signsOwned = Calculate.ZodiacSignsOwnedByPlanet(inputPlanet);

            //given a Zodiac Sign, return, House Number (or Cusp number as its actually called)
            var houseList = new List<HouseName>();

            //get signs of all houses
            var houseSigns = Calculate.AllHouseZodiacSigns(time);


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

        /// <summary>
        /// Given a sign name and time will get the house that it is in, based on middle longitude.
        /// </summary>
        public static HouseName HouseFromSignName(ZodiacName zodiacName, Time inputTime)
        {
            //get signs for all houses
            //TODO cache down
            var houses = Calculate.AllHouseZodiacSigns(inputTime);

            //pick out and return only for input sign
            HouseName houseName = houses.Where(e => e.Value.GetSignName() == zodiacName).Select(e => e.Key).FirstOrDefault();

            return houseName;

        }

        /// <summary>
        /// Gets total hours in a vedic day, that is duration from sunset to sunrise
        /// NOTE: does not account if birth time is outside sunrise & sunset range
        /// </summary>
        public static double DayDurationHours(Time time)
        {
            var sunset = Calculate.SunsetTime(time);
            var sunrise = Calculate.SunriseTime(time);

            var totalHours = sunset.Subtract(sunrise).TotalHours;
            return totalHours;
        }

        /// <summary>
        /// A day starts at the time of sunrise and ends at the time of sunset. A
        /// night starts at the time of sunset and ends at the time of next day's sunrise.
        /// </summary>
        public static bool IsNightBirth(Time birthTime)
        {
            //get sunset time for that day
            var sunset = Calculate.SunsetTime(birthTime);

            //get next day sunrise time
            var nextDay = birthTime.AddHours(23);
            var sunriseNextDay = Calculate.SunriseTime(nextDay);

            //check if given birth time is within this time frame
            var xx = birthTime >= sunset;
            var cc = birthTime <= sunriseNextDay;

            //if so then night birth!
            return xx && cc;
        }

        /// <summary>
        /// A day starts at the time of sunrise and ends at the time of sunset. A
        /// night starts at the time of sunset and ends at the time of next day's sunrise.
        /// </summary>
        public static bool IsDayBirth(Time birthTime)
        {
            //get sunrise time for that day
            var sunrise = Calculate.SunriseTime(birthTime);

            //get day sunset time
            var sunset = Calculate.SunsetTime(birthTime);

            //check if given birth time is within this time frame
            var xx = birthTime >= sunrise;
            var cc = birthTime <= sunset;

            //if so then day birth!
            return xx && cc;
        }

        /// <summary>
        /// As the name suggests, Ghataka chakra is seen for any kind of injuries,
        /// may it be Physical or Mental. The injuries can be inflicted at an inopportune
        /// moment or by an inimical person. Both of these can be seen from the Ghataka Chakra.
        /// For the first instance, the inopportune time can be seen from the horoscope of the
        /// moment of occurance of the event and for the latter case, the same can be seen from
        /// the horoscope of the person inflicting pain and injury.
        /// Source : https://studylib.net/doc/27493638/secrets-of-ghataka-chakra
        /// </summary>
        public static List<string> GhatakaChakra(Time time, Time birthTime)
        {
            //below table of all possible combinations
            var GhatakaChakraTable = new Dictionary<ZodiacName, GhatakaRow>()
            {
                { ZodiacName.Aries, new GhatakaRow(ZodiacName.Aries, LunarDayGroup.Nanda, VedAstro.Library.DayOfWeek.Sunday, ConstellationName.Makha, ZodiacName.Aries, ZodiacName.Libra)},
                { ZodiacName.Taurus, new GhatakaRow(ZodiacName.Virgo, LunarDayGroup.Purna, VedAstro.Library.DayOfWeek.Saturday, ConstellationName.Hasta, ZodiacName.Taurus, ZodiacName.Scorpio)},
                { ZodiacName.Gemini, new GhatakaRow(ZodiacName.Aquarius,LunarDayGroup.Bhadra , VedAstro.Library.DayOfWeek.Monday , ConstellationName.Swathi ,ZodiacName.Cancer,ZodiacName.Capricorn)},
                { ZodiacName.Cancer, new GhatakaRow(ZodiacName.Leo,LunarDayGroup.Bhadra ,VedAstro.Library.DayOfWeek.Wednesday ,ConstellationName.Anuradha,ZodiacName.Libra,ZodiacName.Aries)},
                { ZodiacName.Leo, new GhatakaRow(ZodiacName.Capricorn,LunarDayGroup.Jaya ,VedAstro.Library.DayOfWeek.Saturday ,ConstellationName.Moola,ZodiacName.Capricorn,ZodiacName.Cancer)},
                { ZodiacName.Virgo, new GhatakaRow(ZodiacName.Gemini,LunarDayGroup.Purna ,VedAstro.Library.DayOfWeek.Saturday ,ConstellationName.Sravana,ZodiacName.Pisces,ZodiacName.Virgo)},
                { ZodiacName.Libra, new GhatakaRow(ZodiacName.Sagittarius,LunarDayGroup.Rikta,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Satabhisha,ZodiacName.Virgo,ZodiacName.Pisces)},
                { ZodiacName.Scorpio, new GhatakaRow(ZodiacName.Taurus,LunarDayGroup.Nanda,VedAstro.Library.DayOfWeek.Friday,ConstellationName.Revathi,ZodiacName.Taurus,ZodiacName.Scorpio)},
                { ZodiacName.Sagittarius, new GhatakaRow(ZodiacName.Pisces,LunarDayGroup.Jaya,VedAstro.Library.DayOfWeek.Friday,ConstellationName.Aswini,ZodiacName.Sagittarius,ZodiacName.Gemini)},
                { ZodiacName.Capricorn, new GhatakaRow(ZodiacName.Leo,LunarDayGroup.Rikta,VedAstro.Library.DayOfWeek.Tuesday,ConstellationName.Rohini,ZodiacName.Aquarius,ZodiacName.Leo)},
                { ZodiacName.Aquarius, new GhatakaRow(ZodiacName.Sagittarius,LunarDayGroup.Jaya,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Aridra,ZodiacName.Gemini,ZodiacName.Sagittarius)},
                { ZodiacName.Pisces, new GhatakaRow(ZodiacName.Aquarius,LunarDayGroup.Purna,VedAstro.Library.DayOfWeek.Thursday,ConstellationName.Aslesha,ZodiacName.Leo,ZodiacName.Aquarius)}
            };

            //get janma rasi
            var janmaRasi = Calculate.MoonSignName(birthTime);

            //get data points that could make this happen at check time
            ZodiacName moonSign = Calculate.MoonSignName(time);
            LunarDayGroup tithiGroup = Calculate.LunarDay(time).GetLunarDayGroup();
            DayOfWeek vedicDay = Calculate.DayOfWeek(time);
            ConstellationName moonConstellation = Calculate.MoonConstellation(time).GetConstellationName();
            ZodiacName lagna = Calculate.LagnaSignName(time);

            //add any to list, can occur more than 1
            var foundGhataka = new List<string>();

            //if any one of the above data points match with any one GhatakRow then add to list
            //can be more than 1 added
            if (GhatakaChakraTable.TryGetValue(janmaRasi, out GhatakaRow ghatakaRow))
            {
                //check each and add
                if (ghatakaRow.MoonSign == moonSign) { foundGhataka.Add(nameof(ghatakaRow.MoonSign)); }
                if (ghatakaRow.TithiGroup == tithiGroup) { foundGhataka.Add(nameof(ghatakaRow.TithiGroup)); }
                if (ghatakaRow.WeekDay == vedicDay) { foundGhataka.Add(nameof(ghatakaRow.WeekDay)); }
                if (ghatakaRow.MoonConstellation == moonConstellation) { foundGhataka.Add(nameof(ghatakaRow.MoonConstellation)); }
                if (ghatakaRow.LagnaSameSex == lagna) { foundGhataka.Add(nameof(ghatakaRow.LagnaSameSex)); }
            }

            //return list
            return foundGhataka;
        }

        /// <summary>
        /// Calculate Lord of Star (Constellation) given Constellation. Returns Star Lord Name
        /// </summary>
        public static PlanetName LordOfConstellation(ConstellationName constellation)
        {
            switch (constellation)
            {
                case ConstellationName.Aswini:
                case ConstellationName.Makha:
                case ConstellationName.Moola:
                    return Ketu;
                    break;
                case ConstellationName.Bharani:
                case ConstellationName.Pubba:
                case ConstellationName.Poorvashada:
                    return Venus;
                case ConstellationName.Krithika:
                case ConstellationName.Uttara:
                case ConstellationName.Uttarashada:
                    //case ConstellationName.Abhijit:
                    return VedAstro.Library.PlanetName.Sun;
                case ConstellationName.Rohini:
                case ConstellationName.Hasta:
                case ConstellationName.Sravana:
                    return VedAstro.Library.PlanetName.Moon;
                case ConstellationName.Mrigasira:
                case ConstellationName.Chitta:
                case ConstellationName.Dhanishta:
                    return VedAstro.Library.PlanetName.Mars;
                case ConstellationName.Aridra:
                case ConstellationName.Swathi:
                case ConstellationName.Satabhisha:
                    return VedAstro.Library.PlanetName.Rahu;
                case ConstellationName.Punarvasu:
                case ConstellationName.Vishhaka:
                case ConstellationName.Poorvabhadra:
                    return VedAstro.Library.PlanetName.Jupiter;
                case ConstellationName.Pushyami:
                case ConstellationName.Anuradha:
                case ConstellationName.Uttarabhadra:
                    return VedAstro.Library.PlanetName.Saturn;
                case ConstellationName.Aslesha:
                case ConstellationName.Jyesta:
                case ConstellationName.Revathi:
                    return VedAstro.Library.PlanetName.Mercury;
                default:
                    return VedAstro.Library.PlanetName.Empty;
            }

            throw new Exception("End of Line");

        }

        /// <summary>
        /// Checks if a planet is in a Watery or aqua sign
        /// </summary>
        public static bool IsPlanetInWaterySign(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetRasiD1Sign(planetName, time);

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
        /// Gets Moon's position or day in lunar calendar
        /// </summary>
        public static LunarDay LunarDay(Time time)
        {
            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(Sun, time);
            Angle moonLong = PlanetNirayanaLongitude(Moon, time);

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
        public static Constellation MoonConstellation(Time time) => PlanetConstellation(Moon, time);

        /// <summary>
        /// Gets the constellation behind a planet at a given time
        /// </summary>
        public static Constellation PlanetConstellation(PlanetName planet, Time time)
        {
            //get position of planet in longitude
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

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
            var moonSign = PlanetRasiD1Sign(Moon, time);

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
        /// Also know as Panchanga Yoga
        /// Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')
        /// </summary>
        public static NithyaYoga NithyaYoga(Time time)
        {
            //Nithya Yoga = (Longitude of Sun + Longitude of Moon) / 13°20' (or 800')

            //get position of sun & moon in longitude
            Angle sunLongitude = PlanetNirayanaLongitude(Sun, time);
            Angle moonLongitude = PlanetNirayanaLongitude(Moon, time);

            var jointLongitudeInMinutes = (sunLongitude + moonLongitude).Normalize360().TotalMinutes;

            //get joint motion in longitude of the Sun and the Moon
            //var jointLongitudeInMinutes = sunLongitude.TotalMinutes + moonLongitude.TotalMinutes;



            //get unrounded nithya yoga number by
            //dividing joint longitude by 800'
            var rawNithyaYogaNumber = jointLongitudeInMinutes / 800;

            //round to ceiling to get whole number
            var nithyaYogaNumber = Math.Ceiling(rawNithyaYogaNumber);

            //convert nithya yoga number to type
            var nithyaYoga = VedAstro.Library.NithyaYoga.FromNumber(nithyaYogaNumber);

            //return to caller

            return nithyaYoga;
        }

        /// <summary>
        /// Used for muhurtha of auspicious activities, part of Panchang like Tithi, Nakshatra, Yoga, etc.
        /// Each tithi is divided into 2 karanas. There are 11 karanas: (1) Bava, (2)
        /// Balava, (3) Kaulava, (4) Taitula, (5) Garija, (6) Vanija, (7) Vishti, (8) Sakuna,
        /// (9) Chatushpada, (10) Naga, and, (11) Kimstughna. The first 7 karanas
        /// repeat 8 times starting from the 2nd half of the first lunar day of a month.
        /// The last 4 karanas come just once in a month, starting from the 2nd half of
        /// the 29th lunar day and ending at the 1st half of the first lunar day.
        /// </summary>
        public static Karana Karana(Time time)
        {
            //declare karana as empty first
            Karana? karanaToReturn = null;

            //get position of sun & moon
            Angle sunLong = PlanetNirayanaLongitude(Sun, time);
            Angle moonLong = PlanetNirayanaLongitude(Moon, time);

            //get raw lunar date
            double rawlunarDate;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
            {
                rawlunarDate = (moonLong - sunLong).TotalDegrees / 12.0;
            }
            else
            {
                rawlunarDate = ((moonLong + Angle.Degrees360) - sunLong).TotalDegrees / 12.0;
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
            var sunSign = PlanetRasiD1Sign(Sun, time);

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
                while (true) //breaks when found
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
        /// Gets list of all planets that's in a house/bhava at a given time
        /// based on house longitudes and not sign. Method 1.
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
        /// Gets list of all planets that's in a house at a given time based on sign the
        /// house and planet is in and not house longitudes. Method 2.
        /// </summary>
        public static List<PlanetName> PlanetsInHouseBasedOnSign(HouseName houseNumber, Time time)
        {
            //get house sign
            var houseSign = Calculate.HouseSignName(houseNumber, time);

            //get all planets in sign
            var planetsInSign = Calculate.PlanetsInSign(houseSign, time);

            //return list
            return planetsInSign;
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
                var planetSign = PlanetRasiD1Sign(planet, time);

                //if correct sign add to return list
                if (planetSign.GetSignName() == signName) { returnList.Add(planet); }
            }

            return returnList;
        }

        /// <summary>
        /// Gets the Nirayana longitude of all 9 planets (corrected with Ayanamsa)
        /// </summary>
        public static List<PlanetLongitude> AllPlanetLongitude(Time time)
        {
            //get longitudes of all planets
            var sunLongitude = PlanetNirayanaLongitude(Sun, time);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetNirayanaLongitude(Moon, time);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetNirayanaLongitude(Mars, time);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetNirayanaLongitude(Mercury, time);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetNirayanaLongitude(Jupiter, time);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetNirayanaLongitude(Venus, time);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetNirayanaLongitude(Saturn, time);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetNirayanaLongitude(Rahu, time);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetNirayanaLongitude(Ketu, time);
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
            var sunLongitude = PlanetSayanaLongitude(Sun, time);
            var sun = new PlanetLongitude(Sun, sunLongitude);

            var moonLongitude = PlanetSayanaLongitude(Moon, time);
            var moon = new PlanetLongitude(Moon, moonLongitude);

            var marsLongitude = PlanetSayanaLongitude(Mars, time);
            var mars = new PlanetLongitude(Mars, marsLongitude);

            var mercuryLongitude = PlanetSayanaLongitude(Mercury, time);
            var mercury = new PlanetLongitude(Mercury, mercuryLongitude);

            var jupiterLongitude = PlanetSayanaLongitude(Jupiter, time);
            var jupiter = new PlanetLongitude(Jupiter, jupiterLongitude);

            var venusLongitude = PlanetSayanaLongitude(Venus, time);
            var venus = new PlanetLongitude(Venus, venusLongitude);

            var saturnLongitude = PlanetSayanaLongitude(Saturn, time);
            var saturn = new PlanetLongitude(Saturn, saturnLongitude);

            var rahuLongitude = PlanetSayanaLongitude(Rahu, time);
            var rahu = new PlanetLongitude(Rahu, rahuLongitude);

            var ketuLongitude = PlanetSayanaLongitude(Ketu, time);
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
        /// based on house longitudes NOT sign
        /// </summary>
        public static HouseName HousePlanetOccupiesBasedOnLongitudes(PlanetName planetName, Time time)
        {
            //get the planets longitude
            var planetLongitude = PlanetNirayanaLongitude(planetName, time);

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
        /// based on house sign NOT longitude
        /// </summary>
        public static HouseName HousePlanetOccupiesBasedOnSign(PlanetName planetName, Time time)
        {
            // Get the sign of the planet
            var planetSign = Calculate.PlanetRasiD1Sign(planetName, time);

            // Get all signs of houses at middle longitude
            var houseSigns = Calculate.AllHouseZodiacSigns(time);

            // Find the house with the matching sign
            var foundHouse = houseSigns.Where(yy => yy.Value.GetSignName() == planetSign.GetSignName()).FirstOrDefault();

            // Return the house name
            return foundHouse.Key;

        }

        /// <summary>
        /// List of all planets and the houses they are located in at a given time
        /// </summary>
        public static Dictionary<PlanetName, HouseName> HouseAllPlanetOccupiesBasedOnLongitudes(Time time)
        {
            var returnList = new Dictionary<PlanetName, HouseName>();

            foreach (var planet in PlanetName.All9Planets)
            {
                var houseIsIn = HousePlanetOccupiesBasedOnLongitudes(planet, time);
                returnList.Add(planet, houseIsIn);
            }

            return returnList;
        }

        /// <summary>
        /// Gets lord of given house at given time
        /// The lord is the Graha (planet) in whose Rasi (sign) the Bhavamadhya falls
        /// </summary>
        public static PlanetName LordOfHouse(HouseName houseNumber, Time time)
        {
            //get sign name based on house number
            var houseSignName = HouseSignName(houseNumber, time);

            //get the lord of the house sign
            var lordOfHouseSign = LordOfZodiacSign(houseSignName);

            return lordOfHouseSign;
        }

        /// <summary>
        /// Gets the lord of zodiac sign planet is in, aka "Planet Sign Lord"
        /// </summary>
        public static PlanetName PlanetLordOfZodiacSign(PlanetName inputPlanet, Time time)
        {
            // Calculate the Nirayana longitude (sidereal longitude in Vedic astrology) 
            // of the current planet at the birth time.
            var nirayanaDegrees = PlanetNirayanaLongitude(inputPlanet, time);

            var zodiacSign = ZodiacSignAtLongitude(nirayanaDegrees);

            return LordOfZodiacSign(zodiacSign.GetSignName());
        }

        /// <summary>
        /// Gets the lord of constellation planet is in, aka "Planet Star Lord"
        /// </summary>
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
        /// For all houses. 
        /// Calculate Lord of Star (Constellation) given Constellation. Returns Star Lord Name
        /// TODO MARKED FOR OBLIVION CHECK WITH CP JOIS
        /// </summary>
        public static Dictionary<HouseName, PlanetName> AllHouseConstellationLord(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, PlanetName>();

            //get for all houses
            foreach (var house in VedAstro.Library.House.AllHouses)
            {
                // get constellation house is in middle longitude
                var houseConste = Calculate.HouseConstellation(house, time);

                //get lord based on constellation
                var calcResult = Calculate.LordOfConstellation(houseConste.GetConstellationName());
                allHouses.Add(house, calcResult);
            }

            return allHouses;
        }

        /// <summary>
        /// Gets the constellation lord of a house at middle longitude of the house.
        /// </summary>
        public static PlanetName HouseConstellationLord(HouseName houseNumber, Time time)
        {
            // get constellation house is in middle longitude
            var houseConste = Calculate.HouseConstellation(houseNumber, time);

            //gets lord based on constellation
            var calcResult = Calculate.LordOfConstellation(houseConste.GetConstellationName());

            return calcResult;
        }

        /// <summary>
        /// Gets the constellation at middle longitude of the house.
        /// </summary>
        public static Constellation HouseConstellation(HouseName houseNumber, Time time)
        {
            //get all houses
            var allHouses = AllHouseLongitudes(time);

            //get the house specified 
            var specifiedHouse = allHouses.Find(house => house.GetHouseName() == houseNumber);

            //get sign of the specified house
            var middleLongitude = specifiedHouse.GetMiddleLongitude();
            var houseConstellation = ConstellationAtLongitude(middleLongitude);

            //return the name of house sign
            return houseConstellation;
        }

        /// <summary>
        /// Gets all the planets in a house based on a sign
        /// </summary>
        public static Dictionary<HouseName, List<PlanetName>> AllHousePlanetsInHouseBasedOnSign(Time time)
        {
            //get all houses
            var allHouses = new Dictionary<HouseName, List<PlanetName>>();

            //get for all houses
            foreach (var house in VedAstro.Library.House.AllHouses)
            {
                var calcHouseSign = Calculate.PlanetsInHouseBasedOnSign(house, time);
                allHouses.Add(house, calcHouseSign);
            }

            return allHouses;
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
        public static ZodiacName SignCountedFromPlanetSign(int countToNextSign, PlanetName startPlanet, Time inputTime)
        {
            var planetSignName = PlanetRasiD1Sign(startPlanet, inputTime).GetSignName();
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
        /// Checks if a given planet is in a given sign at a given time
        /// </summary>
        public static bool IsPlanetInSign(PlanetName planetName, ZodiacName signInput, Time time)
        {
            var currentSign = PlanetRasiD1Sign(planetName, time).GetSignName();

            //check if sign match
            return currentSign == signInput;
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
            var planetSignName = PlanetRasiD1Sign(planetName, time).GetSignName();

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

            // Mandi or Gulika is said to aspect 2nd, 7th
            // and 12th Houses from the sign of occupation. 
            if (planetName == Maandi)
            {
                //get signs planet is aspecting
                var sign2ndFromMaandi = SignCountedFromInputSign(planetSignName, 2);
                var sign7thFromMaandi = SignCountedFromInputSign(planetSignName, 7);
                var sign12thFromMaandi = SignCountedFromInputSign(planetSignName, 12);

                //add signs to return list
                planetSignList.Add(sign2ndFromMaandi);
                planetSignList.Add(sign7thFromMaandi);
                planetSignList.Add(sign12thFromMaandi);
            }

            // Mandi or Gulika is said to aspect 2nd, 7th
            // and 12th Houses from the sign of occupation. 
            if (planetName == Gulika)
            {
                //get signs planet is aspecting
                var sign2ndFromGulika = SignCountedFromInputSign(planetSignName, 2);
                var sign7thFromGulika = SignCountedFromInputSign(planetSignName, 7);
                var sign12thFromGulika = SignCountedFromInputSign(planetSignName, 12);

                //add signs to return list
                planetSignList.Add(sign2ndFromGulika);
                planetSignList.Add(sign7thFromGulika);
                planetSignList.Add(sign12thFromGulika);
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
        /// Similar to Exaltation but covers a range not just a point
        /// Moolathrikonas, these are positions similar to exaltation.
        /// NOTE:
        /// - No moolatrikone for Rahu & Ketu, no error will be raised
        /// </summary>
        public static bool IsPlanetInMoolatrikona(PlanetName planetName, Time time)
        {
            //get sign planet is in
            var planetSign = PlanetRasiD1Sign(planetName, time);

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
                    return PlanetToSignRelationship.Empty;
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


            return PlanetToPlanetRelationship.Empty;
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
            var sunSignName = PlanetRasiD1Sign(Sun, time).GetSignName();
            var moonSignName = PlanetRasiD1Sign(Moon, time).GetSignName();
            var marsSignName = PlanetRasiD1Sign(Mars, time).GetSignName();
            var mercurySignName = PlanetRasiD1Sign(Mercury, time).GetSignName();
            var jupiterSignName = PlanetRasiD1Sign(Jupiter, time).GetSignName();
            var venusSignName = PlanetRasiD1Sign(Venus, time).GetSignName();
            var saturnSignName = PlanetRasiD1Sign(Saturn, time).GetSignName();
            var rahuSignName = PlanetRasiD1Sign(Rahu, time).GetSignName();
            var ketuSignName = PlanetRasiD1Sign(Ketu, time).GetSignName();


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
            var planetSignName = PlanetRasiD1Sign(planetName, time).GetSignName();

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
        /// House start middle and end longitudes
        /// </summary>
        public static House HouseLongitude(HouseName houseNumber, Time time)
        {
            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey(nameof(HouseLongitude), houseNumber, time, Ayanamsa), _getHouse);

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
        /// Birth Time In Ghatis
        /// Also know as "Suryodayadi Jananakala Ghatikah".
        /// It is customary among the Hindus to mention
        /// the time of birth as "Suryodayadi Jananakala
        /// Ghatikaha ", i.e., the number of ghatis passed
        /// from sunrise up to the moment of birth.
        /// sunrise to sunrise, and consists of 60 Ghatis
        /// </summary>
        public static Angle IshtaKaala(Time birthTime)
        {
            //check if sunrise before or after
            var isBefore = Calculate.IsBeforeSunrise(birthTime);

            //if birthTime is before sunrise then use previous day sunrise
            TimeSpan timeDifference;
            if (isBefore)
            {
                var preSunrise = Calculate.SunriseTime(birthTime.SubtractHours(23));
                timeDifference = birthTime.Subtract(preSunrise); //sunrise day before
            }
            else
            {
                var sunrise = Calculate.SunriseTime(birthTime);
                timeDifference = birthTime.Subtract(sunrise); ;
            }

            //(Birth Time - Sunrise) x 2.5 = Suryodayadi Jananakala Ghatikaha. 
            var differenceHours = timeDifference.TotalHours;
            var ghatis = differenceHours * 2.5;

            //return round(ghatis to 2 decimal places)
            var xx = VedicTime.FromTotalGhatis(ghatis);
            return Angle.FromDegrees(ghatis);
        }

        /// <summary>
        /// Given a time, will check if it occured before or after sunrise for that given day.
        /// Returns true if given time is before sunrise
        /// </summary>
        public static bool IsBeforeSunrise(Time birthTime)
        {
            //get sunrise for that day
            var sunrise = Calculate.SunriseTime(birthTime);

            //if time is before than it must be smalller
            var isBefore = birthTime < sunrise;

            return isBefore;
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

            //ensure hora is within 1 to 24
            if (hora > 24) { hora = 24; }

            return (int)hora;

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
        /// Checks if a planet is in a Trikona house (trines)(1,5,9)
        /// Equals to "Is Jupiter in Trine from Lagna"
        /// </summary>
        public static bool IsPlanetInTrikona(PlanetName planet, Time time)
        {
            //get current planet house
            var planetHouse = HousePlanetOccupiesBasedOnLongitudes(planet, time);

            //check if planet is in Trine
            var isPlanetInTrine = planetHouse == HouseName.House1 ||
                                  planetHouse == HouseName.House5 ||
                                  planetHouse == HouseName.House9;

            return isPlanetInTrine;
        }

        /// <summary>
        /// Checks if a planet is in a kendra house (4,7,10)
        /// Equals to "Is Jupiter in Kendra from Lagna"
        /// Also know as quadrants or angles
        /// NOTE House 1 not included because follow bv raman's book pg 16 (Astrology for Beginners)
        /// </summary>
        public static bool IsPlanetInKendra(PlanetName planet, Time time)
        {
            //The 4th, the 7th and the 10th are the Kendras
            var planetHouse = HousePlanetOccupiesBasedOnLongitudes(planet, time);

            //check if planet is in kendra
            var isPlanetInKendra = planetHouse == HouseName.House4 || planetHouse == HouseName.House7 || planetHouse == HouseName.House10;

            return isPlanetInKendra;
        }

        /// <summary>
        /// Checks if a planet is in a Upachayas (3rd, 6th, 10th, and 11th)
        /// </summary>
        public static bool IsPlanetInUpachaya(PlanetName planet, Time time)
        {
            //get current house
            var planetHouse = HousePlanetOccupiesBasedOnLongitudes(planet, time);

            //check if planet is in 3rd, 6th, 10th, or 11th
            var isPlanetInUpachayas = planetHouse == HouseName.House3 ||
                                      planetHouse == HouseName.House6 ||
                                      planetHouse == HouseName.House10 ||
                                      planetHouse == HouseName.House11;

            return isPlanetInUpachayas;
        }

        /// <summary>
        /// Checks if any 1 given planet is in a kendra house (4,7,10)
        /// Equals to "Is Jupiter or Venus in Kendra from Lagna"
        /// Also know as quadrants or angles
        /// NOTE House 1 not included because follow bv raman's book pg 16 (Astrology for Beginners)
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
            var startSign = PlanetRasiD1Sign(startPlanet, time);

            //get position of "kendra from" planet
            var endSign = PlanetRasiD1Sign(endPlanet, time);

            //count distance between signs
            var count = CountFromSignToSign(startSign.GetSignName(), endSign.GetSignName());

            return count;
        }

        /// <summary>
        /// Checks if the lord of a house is in the specified house.
        /// Example question : Is Lord of 1st house in 2nd house?
        /// </summary>
        public static bool IsHouseLordInHouseBasedOnLongitudes(HouseName lordHouse, HouseName occupiedHouse, Time time)
        {
            //get the house lord
            var houseLord = LordOfHouse(lordHouse, time);

            //get house the lord is in
            var houseIsIn = HousePlanetOccupiesBasedOnLongitudes(houseLord, time);

            //if it matches then occuring
            return houseIsIn == occupiedHouse;
        }

        /// <summary>
        /// Checks if the lord of a house is in the specified house.
        /// Example question : Is Lord of 1st house in 2nd house?
        /// </summary>
        public static bool IsHouseLordInHouseBasedOnSign(HouseName lordHouse, HouseName occupiedHouse, Time time)
        {
            //get the house lord
            var houseLord = LordOfHouse(lordHouse, time);

            //get house the lord is in
            var houseIsIn = HousePlanetOccupiesBasedOnSign(houseLord, time);

            //if it matches then occuring
            return houseIsIn == occupiedHouse;
        }

        /// <summary>
        /// Checks if a planet is conjuct with an evil/malefic planet
        /// </summary>
        public static bool IsPlanetConjunctWithMaleficPlanets(PlanetName planetName, Time time)
        {
            //get all the planets conjuct with inputed planet
            var planetsInConjunct = PlanetsInConjunction(planetName, time);

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
            var planetsInConjunct = PlanetsInConjunction(inputPlanet, time);

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
            var planetsInConjunct = PlanetsInConjunction(inputPlanet, time);

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
            var planetsInHouse = PlanetsInHouseBasedOnSign(houseNumber, time);

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
            var planetsInHouse = PlanetsInHouseBasedOnSign(houseNumber, time);

            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any planet in house is an good one
            var goodFound = planetsInHouse.FindAll(planet => goodPlanets.Contains(planet)).Any();

            return goodFound;

        }

        /// <summary>
        /// Checks if any good/benefic planets are in kendra houses house
        /// </summary>
        public static bool IsBeneficsInKendra(Time time)
        {
            //get all good planets
            var goodPlanets = BeneficPlanetList(time);

            //check if any one of the good planets is in a kendra
            foreach (var planet in goodPlanets)
            {
                var isInKendra = IsPlanetInKendra(planet, time);

                //if planet found in kendra, end here as true (found)
                if (isInKendra) { return true; }
            }

            //if control reaches here than no benefic in kendra found, return false
            return false;

        }

        /// <summary>
        /// Checks if all malefics are in places in Upachayas.
        /// Malefic planets are those that are generally considered to bring challenges or difficulties.
        /// The Upachayas are the 3rd, 6th, 10th, and 11th houses.
        /// These houses are known as the houses of growth and expansion.
        /// When malefic planets are in these houses, they can drive ambition and personal growth.
        /// </summary>
        public static bool IsAllMaleficsInUpachayas(Time time)
        {
            //get all bad planets
            var badPlanets = MaleficPlanetList(time);

            //all planets must be in
            foreach (var planet in badPlanets)
            {
                var isInUpachaya = IsPlanetInUpachaya(planet, time);

                //if not in, then end as not occuring
                if (!isInUpachaya) { return false; }
            }

            //if control reaches true
            return true;

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
        public static bool IsPlanetAspectedByMaleficPlanets(PlanetName planetReceivingAspect, Time time)
        {
            //get list of evil planets
            var evilPlanets = MaleficPlanetList(time);

            //check if any of the evil planets is aspecting inputed planet
            var evilAspectFound = evilPlanets.FindAll(evilPlanet => IsPlanetAspectedByPlanet(planetReceivingAspect, evilPlanet, time)).Any();
            return evilAspectFound;

        }

        /// <summary>
        /// Returns a list of all malefic planets aspecting the specified receiving planet.
        /// </summary>
        public static List<PlanetName> GetAllMaleficPlanetsAspecting(PlanetName planetReceivingAspect, Time time)
        {
            // Get the list of malefic planets
            var maleficPlanets = MaleficPlanetList(time);

            // Filter the list to include only those planets that are aspecting the input planet
            var maleficPlanetsAspecting = maleficPlanets
                .Where(maleficPlanet => IsPlanetAspectedByPlanet(planetReceivingAspect, maleficPlanet, time))
                .ToList();

            return maleficPlanetsAspecting;
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
            var planetsAspecting = PlanetsAspectingPlanet(inputPlanet, time);

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
            var planetsAspecting = PlanetsAspectingPlanet(inputPlanet, time);

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
            var lagnaLordSign = PlanetRasiD1Sign(lagnaLord, time).GetSignName();

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
        public static int CountFromConstellationToConstellation(Constellation start, Constellation end)
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
        /// Checks if a planet is in a given house at a specified time, based on longittudes
        /// Note: use longitudes as specified in BV Raman's ashtakvarga book
        /// </summary>
        /// <param name="houseNumber">house number to check</param>
        public static bool IsPlanetInHouse(PlanetName planet, HouseName houseNumber, Time time)
        {
            return HousePlanetOccupiesBasedOnLongitudes(planet, time) == houseNumber;
        }

        /// <summary>
        /// Checks if a planet is in a given house at a specified time, using KP method
        /// </summary>
        /// <param name="cusps">can be both Horary & Kundali house cusps (point between houses)</param>
        /// <param name="planetNirayanaDegrees"></param>
        /// <param name="house"></param>
        /// <returns></returns>
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
        /// Checks if a planet is in a given house at a specified time 
        /// </summary>
        public static bool IsAllPlanetsInHouse(List<PlanetName> planetList, HouseName houseNumber, Time time)
        {
            //calculate each planet, even if 1 planet is out, then return as false
            foreach (var planetName in planetList)
            {
                var tempVal = IsPlanetInHouse(planetName, houseNumber, time);
                if (tempVal == false) { return false; }
            }

            //if control reaches here than all planets in house
            return true;
        }

        /// <summary>
        /// Checks if any one planet in list of planets is in a given house at a specified time, based on sign
        /// </summary>
        /// <param name="houseNumber">house number to check</param>
        public static bool IsAnyPlanetsInHouse(List<PlanetName> planetList, HouseName houseNumber, Time time)
        {
            foreach (var planetName in planetList)
            {
                if (IsPlanetInHouse(planetName, houseNumber, time))
                {
                    return true;
                }
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
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

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
        public static bool IsPlanetExaltedDegree(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

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
        /// Checks if a planet is in Exaltation sign
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
        public static bool IsPlanetExaltedSign(PlanetName planet, Time time)
        {
            //get planet location
            var planetLongitude = PlanetNirayanaLongitude(planet, time);

            //convert planet longitude to zodiac sign
            var planetZodiac = ZodiacSignAtLongitude(planetLongitude);

            //get the longitude where planet is Exaltation
            var point = PlanetExaltationPoint(planet);

            //check if planet is in Exaltation sign
            var sameSign = planetZodiac.GetSignName() == point.GetSignName();

            //check only degree ignore minutes & seconds
            var planetIsExaltationSign = sameSign;

            return planetIsExaltationSign;
        }

        /// <summary>
        /// Checks if the moon is FULL, moon day 15 at given time
        /// </summary>
        public static bool IsFullMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = LunarDay(time).GetLunarDayNumber();

            //if day 15, it is full moon
            return lunarDayNumber == 15;
        }

        /// <summary>
        /// Checks if the moon is New, moon day 1 at given time
        /// </summary>
        public static bool IsNewMoon(Time time)
        {
            //get the lunar date number
            int lunarDayNumber = LunarDay(time).GetLunarDayNumber();

            //if day 1, it is new moon
            return lunarDayNumber == 1 || lunarDayNumber == 0;
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
        /// Soumyas
        /// Source : Astrology for beginners pg 30
        /// </summary>
        public static bool IsPlanetBeneficToLagna(PlanetName planetName, Time birthTime)
        {
            var lagna = Calculate.LagnaSignName(birthTime);

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
        public static bool IsPlanetMaleficToLagna(PlanetName planetName, Time birthTime)
        {
            var lagna = Calculate.LagnaSignName(birthTime);

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
        public static bool IsPlanetYogakarakaToLagna(PlanetName planetName, Time birthTime)
        {
            var lagna = Calculate.LagnaSignName(birthTime);

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
        public static bool IsPlanetMarakaToLagna(PlanetName planetName, Time birthTime)
        {
            var lagna = Calculate.LagnaSignName(birthTime);

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
        /// planet position determined by longitude
        /// note: rahu and ketu return false always
        /// </summary>
        public static bool IsPlanetInOwnHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupiesBasedOnLongitudes(planetName, time);

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
        /// Checks if planet is placed in own sign
        /// planet position determined by sign NOT longitude
        /// note: rahu and ketu return false always
        /// </summary>
        public static bool IsPlanetInOwnSign(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupiesBasedOnSign(planetName, time);

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
        /// Returns true if planet is in friendly sign
        /// </summary>
        public static bool IsPlanetInFriendSign(PlanetName planetName, Time time)
        {
            var currentPlanetSign = Calculate.PlanetRasiD1Sign(planetName, time);

            var signRelationship = PlanetRelationshipWithSign(planetName, currentPlanetSign.GetSignName(), time);

            //check relationship
            var isFriend = signRelationship == PlanetToSignRelationship.FriendVarga;
            var isBestFriend = signRelationship == PlanetToSignRelationship.BestFriendVarga;
            if (isFriend || isBestFriend)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Returns true if planet is in enemy sign
        /// </summary>
        public static bool IsPlanetInEnemySign(PlanetName planetName, Time time)
        {
            var currentPlanetSign = Calculate.PlanetRasiD1Sign(planetName, time);

            var signRelationship = PlanetRelationshipWithSign(planetName, currentPlanetSign.GetSignName(), time);

            //check relationship
            var isEnemy = signRelationship == PlanetToSignRelationship.EnemyVarga;
            var isSuperEnemy = signRelationship == PlanetToSignRelationship.BitterEnemyVarga;
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
        /// True if a planet is in a house sign owned by an enemy. Rahu and Ketu is false always
        /// </summary>
        public static bool IsPlanetInEnemyHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupiesBasedOnLongitudes(planetName, time);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //check relationship
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
        public static bool IsPlanetInFriendHouse(PlanetName planetName, Time time)
        {
            //find out if planet is rahu or ketu, because not all calculations supported
            var isRahuKetu = planetName == Rahu || planetName == Ketu;

            //get current house
            var _planetCurrentHouse = HousePlanetOccupiesBasedOnLongitudes(planetName, time);

            //relationship with current house
            var _currentHouseRelation = isRahuKetu ? 0 : PlanetRelationshipWithHouse(_planetCurrentHouse, planetName, time);

            //check relationship
            var isFriend = _currentHouseRelation == PlanetToSignRelationship.FriendVarga;
            var isBestFriend = _currentHouseRelation == PlanetToSignRelationship.BestFriendVarga;
            if (isFriend || isBestFriend)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get a person's varna or color (character)
        /// A person's varna can be observed in real life
        /// </summary>
        public static Varna BirthVarna(Time birthTime)
        {
            //get ruling sign
            var ruleSign = PlanetRasiD1Sign(Moon, birthTime).GetSignName();

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
        /// Gets all planets in certain sign from the moon. Exp: get planets 3rd from the moon
        /// </summary>
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int signsFromMoon, PlanetName startPlanet, Time birthTime)
        {
            //get the sign to check
            var moonNthSign = SignCountedFromPlanetSign(signsFromMoon, startPlanet, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(moonNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsInASignFromLagna(int signsFromLagna, Time birthTime)
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
        public static List<PlanetName> AllPlanetsSignsFromPlanet(int[] signsFromList, PlanetName startPlanet, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsSignsFromPlanet(sigsFrom, startPlanet, birthTime);
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
            var planetNthSign = SignCountedFromPlanetSign(signsFromMoon, startPlanet, birthTime);

            //get all the planets in the sign
            var planetsIn = PlanetsInSign(planetNthSign, birthTime);

            return planetsIn;
        }

        /// <summary>
        /// Gets all planets in certain sign from the Lagna/Ascendant, given list of signs. Exp: get planets 3rd from the Lagna/Ascendant
        /// </summary>
        public static List<PlanetName> AllPlanetsInSignsFromLagna(int[] signsFromList, Time birthTime)
        {
            var returnList = new List<PlanetName>();

            foreach (var sigsFrom in signsFromList)
            {
                //get all planets in given number (house) from moon
                var temp = AllPlanetsInASignFromLagna(sigsFrom, birthTime);
                returnList.AddRange(temp);
            }

            //remove duplicates
            return returnList.Distinct().ToList();

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
            var planetsFromLagna = AllPlanetsInSignsFromLagna(signsFromList, birthTime);

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
        public static bool IsBeneficsInSignsFromPlanet(int[] signsFromList, PlanetName startPlanet, Time birthTime)
        {
            //get all planets that are standard benefics at given time
            var beneficList = BeneficPlanetList(birthTime).ToArray();

            //get all planets in given list of signs from moon
            var isOccuring = IsPlanetsInSignsFromPlanet(signsFromList, beneficList, startPlanet, birthTime);

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

        /// <summary>
        /// Gets the middle longitude of house 1 to house 12
        /// using Swiss Epehemris swe_houses
        /// </summary>
        public static double[] GetAllHouseNirayanaMiddleLongitudes(Time time)
        {
            int iflag = SwissEph.SEFLG_SIDEREAL;

            //get location at place of time
            var location = time.GetGeoLocation();

            //Convert DOB to Julian Day
            var jul_day_UT = TimeToJulianUniversalTime(time);

            SwissEph swissEph = new SwissEph();

            swissEph.swe_set_sid_mode(Ayanamsa, 0, 0);

            double[] cusps = new double[13];
            //we have to supply ascmc to make the function run
            double[] ascmc = new double[10];

            //Note: using Placidus house system to match Raphael's Ephemeris, as used in Raman's books
            swissEph.swe_houses_ex(jul_day_UT, iflag, location.Latitude(), location.Longitude(), 'P', cusps, ascmc);

            //we only return cusps, cause that is what is used
            return cusps;

        }

        /// <summary>
        /// Gives the middle longitude of all houses at a give time
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
                var cusps = GetAllHouseNirayanaMiddleLongitudes(time);

                //Get Sayana Long. of cusp of ascend.
                var udayaLagna = Angle.FromDegrees(cusps[1]);

                //Get Sayana Long. of cusp of tenth-house
                var madhyaLagna = Angle.FromDegrees(cusps[10]);

                //Add 180° to each of these two, to get the Nirayana Asta Lagna (Western Horizon)
                //and the Pathala Lagna (Lower Meridian)
                var astaLagna = udayaLagna + Angle.Degrees180;
                var pathalaLagna = madhyaLagna + Angle.Degrees180;

                //if longitude is more than 360°, expunge 360°
                astaLagna = astaLagna.Normalize360();
                pathalaLagna = pathalaLagna.Normalize360();

                //assign angular house middle longitudes, houses 1,4,7,10
                house1MiddleLongitude = udayaLagna;
                house4MiddleLongitude = pathalaLagna;
                house7MiddleLongitude = astaLagna;
                house10MiddleLongitude = madhyaLagna;

                //2.0 Get middle longitudes of non-angular houses
                //2.1 Calculate arcs
                Angle arcA = GetArc(house1MiddleLongitude, house4MiddleLongitude);
                Angle arcB = GetArc(house4MiddleLongitude, house7MiddleLongitude);
                Angle arcC = GetArc(house7MiddleLongitude, house10MiddleLongitude);
                Angle arcD = GetArc(house10MiddleLongitude, house1MiddleLongitude);

                //2.2 Trisect each arc
                //Cacl House 2 & 3
                house2MiddleLongitude = house1MiddleLongitude + arcA.Divide(3);
                house2MiddleLongitude = house2MiddleLongitude.Normalize360();
                house3MiddleLongitude = house2MiddleLongitude + arcA.Divide(3);
                house3MiddleLongitude = house3MiddleLongitude.Normalize360();

                //Cacl House 5 & 6
                house5MiddleLongitude = house4MiddleLongitude + arcB.Divide(3);
                house5MiddleLongitude = house5MiddleLongitude.Normalize360();
                house6MiddleLongitude = house5MiddleLongitude + arcB.Divide(3);
                house6MiddleLongitude = house6MiddleLongitude.Normalize360();

                //Cacl House 8 & 9
                house8MiddleLongitude = house7MiddleLongitude + arcC.Divide(3);
                house8MiddleLongitude = house8MiddleLongitude.Normalize360();
                house9MiddleLongitude = house8MiddleLongitude + arcC.Divide(3);
                house9MiddleLongitude = house9MiddleLongitude.Normalize360();

                //Cacl House 11 & 12
                house11MiddleLongitude = house10MiddleLongitude + arcD.Divide(3);
                house11MiddleLongitude = house11MiddleLongitude.Normalize360();
                house12MiddleLongitude = house11MiddleLongitude + arcD.Divide(3);
                house12MiddleLongitude = house12MiddleLongitude.Normalize360();

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


            //--------------------LOCALS-------------------
            // Calculates the arc between two longitudes, considering the circular zodiac.
            Angle GetArc(Angle from, Angle to)
            {
                if (to < from)
                {
                    to += Angle.Degrees360;
                }
                return (to - from).Normalize360();
            }

        }

        /// <summary>
        /// Gets all the planets that are in conjunction with the inputed planet
        ///
        /// Note:
        /// 1.The planet inputed is not included in return list
        /// 
        /// 2. Theory behind conjunction :-Two heavenly bodies in the same longitude.
        ///
        /// "The effect of an aspect is felt even if the planets are not
        /// exactly in the mutual distances mentioned above. Therefore,
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
        public static List<PlanetName> PlanetsInConjunction(PlanetName inputPlanet, Time time)
        {
            //set 8° degrees as max space around planet where conjunction occurs
            var conjunctionOrbMax = new Angle(8, 0, 0);

            //get longitude of inputed planet
            var inputedPlanet = PlanetNirayanaLongitude(inputPlanet, time);

            //get all planet longitudes
            List<PlanetLongitude> allPlanetLongitudeList = AllPlanetLongitude(time);

            //a place to store conjunct planets 
            var conjunctPlanets = new List<PlanetName>();

            //loop through each planet
            foreach (var planet in allPlanetLongitudeList)
            {
                //skip the inputed planet
                if (planet.GetPlanetName() == inputPlanet) { continue; }

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

    }
}
