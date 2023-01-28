using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissEphNet;

namespace Jothidam
{
    
    public static partial class Tools
    {
        private static Angle Ayanamsa(int year)
        {

            return Angle.FromSeconds((long)(Math.Round((year - 397) * 50.3333333333)));
        }

        private static Angle SayanaToNirayana(Angle longitude, int year)
        {
            return longitude - Ayanamsa(year);
        }

        private static double SayanaToNirayana(double longitude, int year)
        {
            return longitude - Ayanamsa(year).TotalDegrees;
        }

        public static DateTime LMTtoGMT(DateTime time, GeoCoordinate coordinate)
        {
            //Needs work look at STDtoLMT

            Angle longitudeTime = Angle.FromDegrees(coordinate.Longitude.TotalDegrees / 15);
            TimeSpan longTime = new TimeSpan((int)longitudeTime.Degrees, (int)longitudeTime.Minutes, (int)longitudeTime.Seconds);
            time = time - longTime;
            //longitude.TotalDegrees

            return time;
        }

        public static TimeSpan GMTInterval(DateTimeOffset date)
        {
            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDate = date.ToUniversalTime();

            if (utcDate.Hour >= 12)
                return new TimeSpan(utcDate.Hour, utcDate.Hour, utcDate.Second, utcDate.Millisecond) - TimeSpan.FromHours(12);
            else //(time.Hour < 12)
            {
                DateTimeOffset predate = utcDate.Subtract(TimeSpan.FromDays(1));
                predate = new DateTime(predate.Year, predate.Month, predate.Day, 12, 0, 0, DateTimeKind.Utc);

                return date - predate;
            }


        }

        public static DateTimeOffset STDtoLMT(DateTimeOffset date, GeoCoordinate location)
        {
            return date.ToOffset(location.LMToffset);
        }

        public static DateTimeOffset LMTtoSTD(DateTimeOffset date, StandardTimeKind stdKind)
        {

            DateTimeOffset stdDate = DateTimeOffset.MinValue;

            switch (stdKind)
            {
                case StandardTimeKind.EET:
                    stdDate = date.ToOffset(new TimeSpan(2, 0, 0));
                    break;
                case StandardTimeKind.Malaysia:
                    stdDate = date.ToOffset(new TimeSpan(8, 0, 0));
                    break;
                case StandardTimeKind.India:
                    stdDate = date.ToOffset(new TimeSpan(5, 30, 0));
                    break;
            }


            return stdDate;
        }
        public static List<DateTimeOffset> LMTtoSTDList(List<DateTimeOffset> dateList, StandardTimeKind timeKind)
        {
            List<DateTimeOffset> returnList = new List<DateTimeOffset>();

            foreach (DateTimeOffset day in dateList)
            {
                DateTimeOffset dayTemp = Tools.LMTtoSTD(day, timeKind);
                returnList.Add(dayTemp);
            }


            return returnList;
        }

        public static List<DateSpan> DateSumerizer(List<DateTimeOffset> dateList)
        {

            List<DateSpan> dateSpanArray = new List<DateSpan>();

            
            int prevday = 0;

            for (int i = 0; i < dateList.Count; i++)
            {
                if (dateList[i].Day != prevday)
                {
                    DateSpan temp = new DateSpan();
                    temp.start = dateList[i];
                    dateSpanArray.Add(temp);

                    prevday = dateList[i].Day;
                }
                else
                {
                    if ((i+1) == dateList.Count)
                    {
                        dateSpanArray[(dateSpanArray.Count - 1)].end = dateList[i];
                    }
                    else 
                    {
                        if (dateList[i + 1].Day != prevday)
                        {
                            dateSpanArray[(dateSpanArray.Count - 1)].end = dateList[i];
                        }
                    }
                }
            }


            return dateSpanArray;


        }

        public static List<DateTimeOffset> DateSearcher(List<DateFilter> dateFilterList, List<DateTimeOffset> dateRange)
        {
            List<DateTimeOffset> returnDates = Tools.EachHalfHour(dateRange[0], dateRange[1]).ToList();
            
            foreach (DateFilter filter in dateFilterList)
            {
                returnDates = filter.Process(returnDates);
            }

            return returnDates;
        }

        public static List<DateTimeOffset> GetNailCutDate(DateTimeOffset[] dateRange)
        {
            //Enumerates dates
            List<DateTimeOffset> returntemp = EachHalfHour(dateRange[0], dateRange[1]).ToList();//for testing
            List<DateTimeOffset> returnDates = EachHalfHour(dateRange[0], dateRange[1]).ToList();

            //Done
            DayOfWeek[] dayCriteria = { DayOfWeek.Friday, DayOfWeek.Saturday };

            foreach (DateTimeOffset day in returntemp)
            {
                foreach (DayOfWeek x in dayCriteria)
                {
                    if (day.DayOfWeek == x)
                        returnDates.Remove(day);
                }
            }

            //Done
            int[] lunarCriteria = { 0, 8, 9, 14, 15 };

            foreach (DateTimeOffset day in Tools.LunarDaySearch(lunarCriteria, returnDates))
            {
                returnDates.Remove(day);
            }

            return returnDates;
        }
       
        public static List<DateTimeOffset> GetHairCutDate(DateTimeOffset[] dateRange)
        {
            //Enumerates dates
            List<DateTimeOffset> returnDates = EachHalfHour(dateRange[0], dateRange[1]).ToList();


            RulingConstellation[] constellationCriteria = {

                                                            new RulingConstellation(Constellation.Pushyami, 1),
                                                            new RulingConstellation(Constellation.Pushyami, 2),
                                                            new RulingConstellation(Constellation.Pushyami, 3),
                                                            new RulingConstellation(Constellation.Pushyami, 4),

                                                            new RulingConstellation(Constellation.Punarvasu, 1),
                                                            new RulingConstellation(Constellation.Punarvasu, 2),
                                                            new RulingConstellation(Constellation.Punarvasu, 3),
                                                            new RulingConstellation(Constellation.Punarvasu, 4),

                                                            new RulingConstellation(Constellation.Revathi, 1),
                                                            new RulingConstellation(Constellation.Revathi, 2),
                                                            new RulingConstellation(Constellation.Revathi, 3),
                                                            new RulingConstellation(Constellation.Revathi, 4),

                                                            new RulingConstellation(Constellation.Hasta, 1),
                                                            new RulingConstellation(Constellation.Hasta, 2),
                                                            new RulingConstellation(Constellation.Hasta, 3),
                                                            new RulingConstellation(Constellation.Hasta, 4),

                                                            new RulingConstellation(Constellation.Sravana, 1),
                                                            new RulingConstellation(Constellation.Sravana, 2),
                                                            new RulingConstellation(Constellation.Sravana, 3),
                                                            new RulingConstellation(Constellation.Sravana, 4),

                                                            new RulingConstellation(Constellation.Dhanishta, 1),
                                                            new RulingConstellation(Constellation.Dhanishta, 2),
                                                            new RulingConstellation(Constellation.Dhanishta, 3),
                                                            new RulingConstellation(Constellation.Dhanishta, 4),

                                                            new RulingConstellation(Constellation.Mrigasira, 1),
                                                            new RulingConstellation(Constellation.Mrigasira, 2),
                                                            new RulingConstellation(Constellation.Mrigasira, 3),
                                                            new RulingConstellation(Constellation.Mrigasira, 4),

                                                            new RulingConstellation(Constellation.Aswini, 1),
                                                            new RulingConstellation(Constellation.Aswini, 2),
                                                            new RulingConstellation(Constellation.Aswini, 3),
                                                            new RulingConstellation(Constellation.Aswini, 4),

                                                            new RulingConstellation(Constellation.Chitta, 1),
                                                            new RulingConstellation(Constellation.Chitta, 2),
                                                            new RulingConstellation(Constellation.Chitta, 3),
                                                            new RulingConstellation(Constellation.Chitta, 4),

                                                            new RulingConstellation(Constellation.Jyesta, 1),
                                                            new RulingConstellation(Constellation.Jyesta, 2),
                                                            new RulingConstellation(Constellation.Jyesta, 3),
                                                            new RulingConstellation(Constellation.Jyesta, 4),

                                                            new RulingConstellation(Constellation.Satabhisha, 1),
                                                            new RulingConstellation(Constellation.Satabhisha, 2),
                                                            new RulingConstellation(Constellation.Satabhisha, 3),
                                                            new RulingConstellation(Constellation.Satabhisha, 4),

                                                            new RulingConstellation(Constellation.Swathi, 1),
                                                            new RulingConstellation(Constellation.Swathi, 2),
                                                            new RulingConstellation(Constellation.Swathi, 3),
                                                            new RulingConstellation(Constellation.Swathi, 4),
                                                          };

            returnDates = Tools.RulingConstellationSearch(returnDates, constellationCriteria.ToList());



            int[] lunarCriteria = { 0, 4, 6, 14, 15 };

            foreach (DateTimeOffset day in Tools.LunarDaySearch(lunarCriteria, returnDates.ToList()))
            {
                returnDates.Remove(day);
            }


            return returnDates;
        }

        public static List<DateTimeOffset> GetStudyDate(DateTimeOffset[] dateRange)
        {
            //Enumerates dates
            List<DateTimeOffset> returnDates = EachHalfHour(dateRange[0], dateRange[1]).ToList();


            RulingConstellation[] constellationCriteria = {
                                                            new RulingConstellation(Constellation.Mrigasira, 1),
                                                            new RulingConstellation(Constellation.Mrigasira, 2),
                                                            new RulingConstellation(Constellation.Mrigasira, 3),
                                                            new RulingConstellation(Constellation.Mrigasira, 4),

                                                            new RulingConstellation(Constellation.Aridra, 1),
                                                            new RulingConstellation(Constellation.Aridra, 2),
                                                            new RulingConstellation(Constellation.Aridra, 3),
                                                            new RulingConstellation(Constellation.Aridra, 4),

                                                            new RulingConstellation(Constellation.Punarvasu, 1),
                                                            new RulingConstellation(Constellation.Punarvasu, 2),
                                                            new RulingConstellation(Constellation.Punarvasu, 3),
                                                            new RulingConstellation(Constellation.Punarvasu, 4),

                                                            new RulingConstellation(Constellation.Pushyami, 1),
                                                            new RulingConstellation(Constellation.Pushyami, 2),
                                                            new RulingConstellation(Constellation.Pushyami, 3),
                                                            new RulingConstellation(Constellation.Pushyami, 4),

                                                            new RulingConstellation(Constellation.Hasta, 1),
                                                            new RulingConstellation(Constellation.Hasta, 2),
                                                            new RulingConstellation(Constellation.Hasta, 3),
                                                            new RulingConstellation(Constellation.Hasta, 4),

                                                            new RulingConstellation(Constellation.Chitta, 1),
                                                            new RulingConstellation(Constellation.Chitta, 2),
                                                            new RulingConstellation(Constellation.Chitta, 3),
                                                            new RulingConstellation(Constellation.Chitta, 4),

                                                            new RulingConstellation(Constellation.Swathi, 1),
                                                            new RulingConstellation(Constellation.Swathi, 2),
                                                            new RulingConstellation(Constellation.Swathi, 3),
                                                            new RulingConstellation(Constellation.Swathi, 4),

                                                            new RulingConstellation(Constellation.Sravana, 1),
                                                            new RulingConstellation(Constellation.Sravana, 2),
                                                            new RulingConstellation(Constellation.Sravana, 3),
                                                            new RulingConstellation(Constellation.Sravana, 4),

                                                            new RulingConstellation(Constellation.Dhanishta, 1),
                                                            new RulingConstellation(Constellation.Dhanishta, 2),
                                                            new RulingConstellation(Constellation.Dhanishta, 3),
                                                            new RulingConstellation(Constellation.Dhanishta, 4),

                                                            new RulingConstellation(Constellation.Satabhisha, 1),
                                                            new RulingConstellation(Constellation.Satabhisha, 2),
                                                            new RulingConstellation(Constellation.Satabhisha, 3),
                                                            new RulingConstellation(Constellation.Satabhisha, 4),

                                                            new RulingConstellation(Constellation.Aswini, 1),
                                                            new RulingConstellation(Constellation.Aswini, 2),
                                                            new RulingConstellation(Constellation.Aswini, 3),
                                                            new RulingConstellation(Constellation.Aswini, 4),
                                                          };

            returnDates = Tools.RulingConstellationSearch(returnDates, constellationCriteria.ToList());



            return returnDates;
        }


        public static List<DateTimeOffset> TarabalaBeta(List<DateTimeOffset> dateRange, Horoscope horoscope)
        {
            List<DateTimeOffset> returnDates = Tools.EachHalfHour(dateRange[0], dateRange[1]).ToList();

            string tarabala = "\0";
            int counter;
            int daycon;
            int birth;

            foreach (DateTimeOffset day in returnDates)
            {
                daycon = (int)Tools.GetRulingConstellation(day).Name;

                birth = (int)horoscope.JanmaNakshatra.Name;

                
                counter = Math.Abs(daycon - birth);
                
                counter++; //includes constellation


                if (counter > 9)
                {
                    counter = counter % 9;
                    if (counter == 0)
                        counter = 9;
                }
                    

                if (counter == 0)
                {
                    Console.WriteLine("Something wrong here!");
                }

                switch (counter)
                {
                    case 1:
                        tarabala = "Janma - Indicates danger to body";
                        break;
                    case 2:
                        tarabala = "Sampat - Wealth and prosperity";
                        break;
                    case 3:
                        tarabala = "Vipat - Dangers, losses and accidents";
                        break;
                    case 4:
                        tarabala = "Kshema - Prosperity";
                        break;
                    case 5:
                        tarabala = "Pratyak - Obstacles";
                        break;
                    case 6:
                        tarabala = "Sadhana - Realisation of ambitions";
                        break;
                    case 7:
                        tarabala = "Naidhana - Dangers";
                        break;
                    case 8:
                        tarabala = "Mitra - Good";
                        break;
                    case 9:
                        tarabala = "Parama mitra - Very favourable";
                        break;

                }

                Console.WriteLine(Tools.LMTtoSTD(day, StandardTimeKind.EET) + "\t" + tarabala);
            }


            return returnDates;
        }
        
        public static List<DateTimeOffset> PersonalTravelFilterBeta(List<DateTimeOffset> dateRange, GeoCoordinate location, Horoscope horoscope)
        {
            List<DateTimeOffset> dateList = Tools.EachHalfHour(dateRange[0], dateRange[1]).ToList();

            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                Zodiac risingZodiac = Tools.GetRisingZodiac(day, location);
                if (risingZodiac == horoscope.JanmaRasi)
                    returnDates.Add(day);
            }

            return returnDates;
        }

        public static void SearchLunarDay(List<int> criteria, List<DateTimeOffset> dateList)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                int lunarDay = GetLunarDay(day);
                foreach (int intDay in criteria)
                {
                    if (lunarDay == intDay)
                    {
                        returnDates.Add(day);
                    }
                }
            }

            dateList = returnDates;
        }

        public static List<DateTimeOffset> LunarDaySearch(int[] criteria, List<DateTimeOffset> dateList)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                int lunarDay = GetLunarDay(day);
                foreach (int intDay in criteria)
                {
                    if (lunarDay == intDay)
                    {
                        returnDates.Add(day);
                    }
                }
            }

            return returnDates;
        }

        public static List<DateTimeOffset> DayOfWeekSearch(List<DateTimeOffset> dateList, List<DayOfWeek> criteria)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                foreach (DayOfWeek dayofweek in criteria)
                {
                    if (day.DayOfWeek == dayofweek)
                    {
                        returnDates.Add(day);
                    }
                }
            }

            return returnDates;
        }

        public static List<DateTimeOffset> RulingConstellationSearch(List<DateTimeOffset> dateList, List<RulingConstellation> criteria)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                RulingConstellation rulingConst = Tools.GetRulingConstellation(day);
                foreach (RulingConstellation x in criteria)
                {
                    if (rulingConst.Name == x.Name && rulingConst.Quarter == x.Quarter)
                    {
                        returnDates.Add(day);
                    }
                }
            }

            return returnDates;
        }

        public static List<DateTimeOffset> TravelDirectionSearch(List<DateTimeOffset> dateList, TravelDirection criteria)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            foreach (DateTimeOffset day in dateList)
            {
                TravelDirection travelDirection = Tools.GetTravelDirection(day);
                if (travelDirection.HasFlag(criteria))
                {
                    returnDates.Add(day);
                }
            }

            return returnDates;
        }

        public static IEnumerable<DateTimeOffset> EachHalfHour(DateTimeOffset start, DateTimeOffset end)
        {
            for (var day = start; day <= end; day = day.AddHours(1))
                yield return day;
        }

        public static int GetLunarDay(DateTimeOffset date)
        {
            Angle sunLong = Tools.GetPlanetNirayanaLongitude(date, PlanetName.Sun);
            Angle moonLong = Tools.GetPlanetNirayanaLongitude(date, PlanetName.Moon);

            double lunarDay = (moonLong - sunLong).TotalDegrees / 12;

            if (moonLong.TotalDegrees > sunLong.TotalDegrees)
                lunarDay = (moonLong - sunLong).TotalDegrees / 12.0;
            else
                lunarDay = ((moonLong + Angle.FromDegrees(360)) - sunLong).TotalDegrees / 12.0;
            
            //0 = New moon
            //15 = Full moon
            return (int)Math.Ceiling(lunarDay);

        }

        public static RulingConstellation GetRulingConstellation(DateTimeOffset date)
        {
            Angle moonLong = Tools.GetPlanetNirayanaLongitude(date, PlanetName.Moon);

            return new RulingConstellation(moonLong.TotalMinutes / 800.0);
        }

        public static TravelDirection GetTravelDirection(DateTimeOffset date)
        {
            TravelDirection returnDirection = TravelDirection.None;
            DayOfWeek day = date.DayOfWeek;
            TimeSpan time = date.TimeOfDay; //in LMT not 100% sure
            
            //Ghatis converted to hours at 1hour = 2.5G
            TimeSpan mondayCutoff = TimeSpan.FromHours(0); //3.2
            TimeSpan tuesdayCutoff = TimeSpan.FromHours(0); //4.8 recommended to avoid totally, hence from 12AM = 0h
            TimeSpan wednesdayCutoff = TimeSpan.FromHours(0); //4.8
            TimeSpan thursdayCutoff = TimeSpan.FromHours(0); //8.8
            TimeSpan fridayCutoff = TimeSpan.FromHours(0); //6
            TimeSpan saturdayCutoff = TimeSpan.FromHours(0); //3.2
            TimeSpan sundayCutoff = TimeSpan.FromHours(0); //6

            switch (day)
            {
                case DayOfWeek.Monday:
                    if (time >= mondayCutoff)
                        returnDirection = TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Tuesday:
                    if (time >= tuesdayCutoff)
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Wednesday:
                    if (time >= wednesdayCutoff)
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Thursday:
                    if (time >= thursdayCutoff)
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Friday:
                    if (time >= fridayCutoff)
                        returnDirection = TravelDirection.East | TravelDirection.North | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Saturday:
                    if (time >= saturdayCutoff)
                        returnDirection = TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;

                case DayOfWeek.Sunday:
                    if (time >= sundayCutoff)
                        returnDirection = TravelDirection.East | TravelDirection.North | TravelDirection.South;
                    else
                        returnDirection = TravelDirection.East | TravelDirection.West | TravelDirection.North | TravelDirection.South;
                    break;
            }

            return returnDirection;
           
        }

        public static House[] GetHouses(DateTimeOffset date, GeoCoordinate location)
        {
            House[] Houses = new House[13] { new House(), new House(), new House(), new House(), new House(), 
                new House(), new House(), new House(), new House(),new House(),new House(),new House(),new House()};

            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDate = date.ToUniversalTime();

            //Generate House 1 & 10
            double jul_day_UT;
            SwissEph swissEph = new SwissEph();
            double[] cusps = new double[13];
            double[] ascmc = new double[10];


            //Convert DOB to Julian Day
            jul_day_UT = swissEph.swe_julday(utcDate.Year, utcDate.Month, utcDate.Day, utcDate.TimeOfDay.TotalHours, SwissEph.SE_GREG_CAL);

            //Get 1 & 10 house
            swissEph.swe_houses(jul_day_UT, location.Latitude.TotalDegrees, location.Longitude.TotalDegrees, 'P', cusps, ascmc);


            Houses[1].Longitude = cusps[1];
            Houses[10].Longitude = cusps[10];


            //Convert Sayana to Nirayana
            Houses[1].Longitude = Tools.SayanaToNirayana(Houses[1].Longitude, utcDate.Year); //Udaya Lagna (E. Horizon)
            Houses[10].Longitude = Tools.SayanaToNirayana(Houses[10].Longitude, utcDate.Year); //Madhya Lagna (Upper Meridian)

            //Calc House 7 & 4
            Houses[7].Longitude = (Houses[1].Longitude + 180); //Asta Lagna (W. Horizon)
            if (Houses[7].Longitude > 360)
                Houses[7].Longitude -= 360; //Expunge 360

            Houses[4].Longitude = (Houses[10].Longitude + 180); //Patala Lagna (Low. Meridian)
            if (Houses[4].Longitude > 360)
                Houses[4].Longitude -= 360; //Expunge 360

            //Cacl ars
            double arcA, arcB, arcC, arcD;

            if (Houses[4].Longitude < Houses[1].Longitude)
                arcA = ((Houses[4].Longitude + 360) - Houses[1].Longitude);
            else
                arcA = (Houses[4].Longitude - Houses[1].Longitude);

            if (Houses[7].Longitude < Houses[4].Longitude)
                arcB = ((Houses[7].Longitude + 360) - Houses[4].Longitude);
            else
                arcB = (Houses[7].Longitude - Houses[4].Longitude);

            if (Houses[10].Longitude < Houses[7].Longitude)
                arcC = ((Houses[10].Longitude + 360) - Houses[7].Longitude);
            else
                arcC = (Houses[10].Longitude - Houses[7].Longitude);

            if (Houses[1].Longitude < Houses[10].Longitude)
                arcD = ((Houses[1].Longitude + 360) - Houses[10].Longitude);
            else
                arcD = (Houses[1].Longitude - Houses[10].Longitude);


            //Cacl House 2 & 3
            Houses[2].Longitude = Houses[1].Longitude + (arcA / 3.0);
            Houses[3].Longitude = Houses[2].Longitude + (arcA / 3.0);

            //Cacl House 5 & 6
            Houses[5].Longitude = Houses[4].Longitude + (arcB / 3.0);
            Houses[6].Longitude = Houses[5].Longitude + (arcB / 3.0);

            //Cacl House 8 & 9
            Houses[8].Longitude = Houses[7].Longitude + (arcC / 3.0);
            Houses[9].Longitude = Houses[8].Longitude + (arcC / 3.0);

            //Cacl House 11 & 12
            Houses[11].Longitude = Houses[10].Longitude + (arcD / 3.0);
            Houses[12].Longitude = Houses[11].Longitude + (arcD / 3.0);

            for (int i = 1; i < 13; i++)
            {
                if (Houses[i].Longitude > 360)
                    Houses[i].Longitude -= 360;
            }


            //Calculate house sadhis

            ZodiacNum znum = 12; //ZNUM loops back to 1 after 12

            for (int i = 1; i < 13; i++)
            {
                if (Houses[i].Longitude < Houses[znum].Longitude)
                    Houses[i].Arambha = (Houses[znum].Longitude + (Houses[i].Longitude + 360)) / 2.0;
                else
                    Houses[i].Arambha = (Houses[znum].Longitude + Houses[i].Longitude) / 2.0;

                if (Houses[i].Arambha > 360)
                {
                    Houses[i].Arambha -= 360;
                }
                znum++;
            }


            znum = 2;

            for (int i = 1; i < 13; i++)
            {
                Houses[i].Anthya = Houses[znum].Arambha;
                znum++;
            }


            return Houses;
        
        }

        public static Zodiac GetZodiac(Angle longitude)
        {
            return (Zodiac)Math.Ceiling((longitude.TotalDegrees % 360.0) / 30.0);
        }

        public static Zodiac GetRulingZodiac(DateTimeOffset date)
        {
            Angle moonLong = Tools.GetPlanetNirayanaLongitude(date, PlanetName.Moon);

            return GetZodiac(moonLong);
        }

        public static Zodiac GetRisingZodiac(DateTimeOffset date, GeoCoordinate location)
        {
            House[] houses = Tools.GetHouses(date, location);

            return Tools.GetZodiac(Angle.FromDegrees(houses[1].Longitude));

        }

        public static Angle GetPlanetNirayanaLongitude(DateTimeOffset date, PlanetName planetName)
        {
            Angle returnValue;

            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDate = date.ToUniversalTime();

            //Get sayana longitude on day 
            Angle longitude = GetPlanetEphemerisLongitude(utcDate, planetName);

            //3 - Hindu Nirayana Long = Sayana Long — Ayanamsa.

            Angle birthAnyanamsa = Tools.Ayanamsa(utcDate.Year);

            if (longitude.TotalDegrees < birthAnyanamsa.TotalDegrees)
                returnValue = (longitude + Angle.FromDegrees(360)) - birthAnyanamsa;
            else
                returnValue = longitude - birthAnyanamsa;

            //Calculates Kethu with inital values from Rahu
            if (planetName == PlanetName.Ketu)
                returnValue -= Angle.FromDegrees(180);


            return returnValue;

        }

        public static Angle GetPlanetEphemerisLongitude(DateTimeOffset date, PlanetName planetName)
        {
            //Converts LMT to UTC (GMT)
            DateTimeOffset utcDate = date.ToUniversalTime();

            int planet = 0;
            int year = utcDate.Year;
            int month = utcDate.Month;
            int day = utcDate.Day;
            double hour = (utcDate.TimeOfDay).TotalHours;
            int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR
            int iflag = SwissEph.SEFLG_SWIEPH;  //+ SwissEph.SEFLG_SPEED;
            double[] longitude = new double[6];
            string err_msg = "";
            double jul_day_ET;
            double jul_day_UT;
            SwissEph ephemeris = new SwissEph();

            // Convert DOB to ET
            jul_day_UT = ephemeris.swe_julday(year, month, day, hour, gregflag);//DOB to Julian Day
            jul_day_ET = jul_day_UT + ephemeris.swe_deltat(jul_day_UT);//Julian Day to ET


            //Convert PlanetName to SE_PLANET type
            switch (planetName)
            {
                case PlanetName.Sun:
                    planet = SwissEph.SE_SUN;
                    break;
                case PlanetName.Moon:
                    planet = SwissEph.SE_MOON;
                    break;
                case PlanetName.Mars:
                    planet = SwissEph.SE_MARS;
                    break;
                case PlanetName.Mercury:
                    planet = SwissEph.SE_MERCURY;
                    break;
                case PlanetName.Jupiter:
                    planet = SwissEph.SE_JUPITER;
                    break;
                case PlanetName.Venus:
                    planet = SwissEph.SE_VENUS;
                    break;
                case PlanetName.Saturn:
                    planet = SwissEph.SE_SATURN;
                    break;
                case PlanetName.Rahu:
                    planet = SwissEph.SE_MEAN_NODE;
                    break;
                case PlanetName.Ketu:
                    planet = SwissEph.SE_MEAN_NODE;
                    break;
            }

            //Get planet long
            int ret_flag = ephemeris.swe_calc(jul_day_ET, planet, iflag, longitude, ref err_msg);


            return Angle.FromDegrees(longitude[0]);

        }

        public static DateTimeOffset DOBFormatter(DateTime date, GeoCoordinate coordinate)
        {
            Angle apparentTime = Angle.FromDegrees(coordinate.Longitude.TotalDegrees / 15);
            int minuteTemp = (int)Math.Round(apparentTime.Seconds/60.0);//Rounds seconds
            TimeSpan offsetTime = new TimeSpan((int)apparentTime.Degrees, (int)apparentTime.Minutes + minuteTemp, 0);
            
            DateTime thisDate = date;
            DateTimeOffset thisTime;
            
            switch (coordinate.LongitudeHemisphere)
	        {
                case Hemisphere.West:
                    offsetTime = TimeSpan.FromHours(offsetTime.TotalHours * -1); //Converts to negative
                    thisTime = new DateTimeOffset(thisDate, offsetTime);
                    break;
                case Hemisphere.East:
                    thisTime = new DateTimeOffset(thisDate, offsetTime);
                    break;
                default://hemisphere zero
                    thisTime = new DateTimeOffset(thisDate, offsetTime);
                    break;
	        }

            return thisTime;
            
           
        }
    
    }

   

}
