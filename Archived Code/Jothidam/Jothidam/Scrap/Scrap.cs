using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jothidam.Scrap
{
    class Scrap
    {



        //public static List<DateTimeOffset> GetTravelDate(DateTimeOffset[] dateRange, TravelCriteria criteriaFlag)
        //{
        //    //Enumerates dates
        //    List<DateTimeOffset> returnDates = EachHalfHour(dateRange[0], dateRange[1]).ToList();

        //    //Done
        //    //LunarDay01
        //    if (criteriaFlag.HasFlag(TravelCriteria.LunarDay01))
        //    {
        //        int[] lunarCriteria = { 2, 3, 5, 7, 10, 11, 13 };
        //        returnDates = Tools.LunarDaySearch(returnDates, lunarCriteria.ToList());
        //    }

        //    //Done
        //    //Constelation01
        //    if (criteriaFlag.HasFlag(TravelCriteria.Constelation01))
        //    {
        //        RulingConstellation[] constellationCriteria = {
        //                                                    new RulingConstellation(Constellation.Mrigasira, 2),
        //                                                    new RulingConstellation(Constellation.Mrigasira, 3),
        //                                                    new RulingConstellation(Constellation.Mrigasira, 4),
        //                                                    new RulingConstellation(Constellation.Aswini, 2),
        //                                                    new RulingConstellation(Constellation.Aswini, 3),
        //                                                    new RulingConstellation(Constellation.Aswini, 4),
        //                                                    new RulingConstellation(Constellation.Pushyami, 2),
        //                                                    new RulingConstellation(Constellation.Pushyami, 3),
        //                                                    new RulingConstellation(Constellation.Pushyami, 4),
        //                                                    new RulingConstellation(Constellation.Punarvasu, 2),
        //                                                    new RulingConstellation(Constellation.Punarvasu, 3),
        //                                                    new RulingConstellation(Constellation.Punarvasu, 4),
        //                                                    new RulingConstellation(Constellation.Hasta, 2),
        //                                                    new RulingConstellation(Constellation.Hasta, 3),
        //                                                    new RulingConstellation(Constellation.Hasta, 4),
        //                                                    new RulingConstellation(Constellation.Anuradha, 2),
        //                                                    new RulingConstellation(Constellation.Anuradha, 3),
        //                                                    new RulingConstellation(Constellation.Anuradha, 4),
        //                                                    new RulingConstellation(Constellation.Sravana, 2),
        //                                                    new RulingConstellation(Constellation.Sravana, 3),
        //                                                    new RulingConstellation(Constellation.Sravana, 4),
        //                                                    new RulingConstellation(Constellation.Moola, 2),
        //                                                    new RulingConstellation(Constellation.Moola, 3),
        //                                                    new RulingConstellation(Constellation.Moola, 4),
        //                                                    new RulingConstellation(Constellation.Dhanishta, 2),
        //                                                    new RulingConstellation(Constellation.Dhanishta, 3),
        //                                                    new RulingConstellation(Constellation.Dhanishta, 4),
        //                                                    new RulingConstellation(Constellation.Revathi, 2),
        //                                                    new RulingConstellation(Constellation.Revathi, 3),
        //                                                    new RulingConstellation(Constellation.Revathi, 4)

        //                                                  };

        //        returnDates = Tools.RulingConstellationSearch(returnDates, constellationCriteria.ToList());
        //    }

        //    //TravelEast
        //    if (criteriaFlag.HasFlag(TravelCriteria.TravelEast))
        //    {
        //        returnDates = Tools.TravelDirectionSearch(returnDates, TravelDirection.East);
        //    }

        //    //TravelWest
        //    if (criteriaFlag.HasFlag(TravelCriteria.TravelWest))
        //    {
        //        returnDates = Tools.TravelDirectionSearch(returnDates, TravelDirection.West);
        //    }

        //    //TravelNorth
        //    if (criteriaFlag.HasFlag(TravelCriteria.TravelNorth))
        //    {
        //        returnDates = Tools.TravelDirectionSearch(returnDates, TravelDirection.North);
        //    }

        //    //TravelSouth
        //    if (criteriaFlag.HasFlag(TravelCriteria.TravelSouth))
        //    {
        //        returnDates = Tools.TravelDirectionSearch(returnDates, TravelDirection.South);
        //    }


        //    /////TEST
        //    GeoCoordinate BirthPlace = new GeoCoordinate(Angle.FromDegrees(4.021758), Hemisphere.North, Angle.FromDegrees(101.020966), Hemisphere.East);
        //    DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1994, 4, 23, 12, 44, 0, new TimeSpan(8, 0, 0)), BirthPlace);
        //    Horoscope horscp = new Horoscope(DOBLMT, BirthPlace);

        //    GeoCoordinate local = new GeoCoordinate(Angle.FromDegrees(60.200719), Hemisphere.North, Angle.FromDegrees(24.942931), Hemisphere.East);

        //    returnDates = Tools.PersonalTravelFilterBeta(returnDates, local, horscp);

        //    return returnDates;

        //}

        

     







        //public static dynamic CriteriaInitializer(int dateFilterId)
        //{
        //    int input_id = dateFilterId;


        //    //Lunar day
        //    DatabaseDataSetTableAdapters.lunar_day_criteriasTableAdapter lunarTblAdapter = new DatabaseDataSetTableAdapters.lunar_day_criteriasTableAdapter();
        //    DatabaseDataSet.lunar_day_criteriasDataTable lunarDataTbl = lunarTblAdapter.GetData();            
        //    IEnumerable<DatabaseDataSet.lunar_day_criteriasRow> lunarRowList = lunarDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

        //    if (lunarRowList.Count() > 0)
        //    {
        //        List<int> returnValue = new List<int> { };

        //        foreach (var i in lunarRowList)
        //        {
        //            returnValue.Add(i.lunar_day);
        //        }

        //        return returnValue.ToArray(); 
        //    }


        //    //Constellation
        //    DatabaseDataSetTableAdapters.constellation_criteriasTableAdapter constTblAdapter = new DatabaseDataSetTableAdapters.constellation_criteriasTableAdapter();
        //    DatabaseDataSet.constellation_criteriasDataTable constDataTbl = constTblAdapter.GetData(); 
        //    IEnumerable<DatabaseDataSet.constellation_criteriasRow> constRowList = constDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

        //    if (constRowList.Count() < 0)
        //    {
        //        List<RulingConstellation> returnValue = new List<RulingConstellation> { };

        //        foreach (var i in constRowList)
        //        {   
        //            //Gets and converts constelation from string to type
        //            Constellation constellation_name = new Constellation().Parse(i.constellation_name);

        //            returnValue.Add(new RulingConstellation(constellation_name,i.quarter));
        //        }

        //        return returnValue.ToArray();
        //    }

        //    //Zodiac
        //    DatabaseDataSetTableAdapters.zodiac_criteriasTableAdapter zodTblAdapter = new DatabaseDataSetTableAdapters.zodiac_criteriasTableAdapter();
        //    DatabaseDataSet.zodiac_criteriasDataTable zodDataTbl = zodTblAdapter.GetData();
        //    IEnumerable<DatabaseDataSet.zodiac_criteriasRow> zodRowList = zodDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

        //    if (zodRowList.Count() < 0)
        //    {
        //        List<Zodiac> returnValue = new List<Zodiac> { };

        //        foreach (var i in zodRowList)
        //        {
        //            returnValue.Add(new Zodiac().Parse(i.zodiac));
        //        }

        //        return returnValue.ToArray();
        //    }

        //    //Week Day
        //    DatabaseDataSetTableAdapters.week_day_criteriasTableAdapter dayTblAdapter = new DatabaseDataSetTableAdapters.week_day_criteriasTableAdapter();
        //    DatabaseDataSet.week_day_criteriasDataTable dayDataTbl = dayTblAdapter.GetData();
        //    IEnumerable<DatabaseDataSet.week_day_criteriasRow> dayRowList = dayDataTbl.Where(e => e.date_filter_id == input_id).OrderBy(e => e.id);

        //    if (dayRowList.Count() < 0)
        //    {
        //        List<DayOfWeek> returnValue = new List<DayOfWeek> { };

        //        foreach (var i in dayRowList)
        //        {
        //            returnValue.Add(new DayOfWeek().Parse(i.week_day));
        //        }

        //        return returnValue.ToArray();
        //    }


        //    return 0;

        //}


        //private void InitializeHouseLongitudes()
        //{
        //    Houses = new House[13] { new House(), new House(), new House(), new House(), new House(), 
        //        new House(), new House(), new House(), new House(),new House(),new House(),new House(),new House()};

        //    //Generate House 1 & 10
        //    double jul_day_UT;
        //    SwissEph swissEph = new SwissEph();
        //    DateTime BirthDateGMT = Tools.LMTtoGMT(BirthDate, BirthLocation);
        //    double[] cusps = new double[13];
        //    double[] ascmc = new double[10];
        //    //Convert DOB to Julian Day
        //    jul_day_UT = swissEph.swe_julday(BirthDateGMT.Year, BirthDateGMT.Month, BirthDateGMT.Day, BirthDateGMT.TimeOfDay.TotalHours, SwissEph.SE_GREG_CAL);

        //    swissEph.swe_houses(jul_day_UT, BirthLocation.Latitude.TotalDegrees, BirthLocation.Longitude.TotalDegrees, 'P', cusps, ascmc);


        //    Houses[1].Longitude = cusps[1];
        //    Houses[10].Longitude = cusps[10];

        //    //Convert Sayana to Nirayana
        //    Houses[1].Longitude = Tools.SayanaToNirayana(Houses[1].Longitude, BirthDate.Year); //Udaya Lagna (E. Horizon)
        //    Houses[10].Longitude = Tools.SayanaToNirayana(Houses[10].Longitude, BirthDate.Year); //Madhya Lagna (Upper Meridian)

        //    //Calc House 7 & 4
        //    Houses[7].Longitude = (Houses[1].Longitude + 180); //Asta Lagna (W. Horizon)
        //    if (Houses[7].Longitude > 360)
        //        Houses[7].Longitude -= 360; //Expunge 360

        //    Houses[4].Longitude = (Houses[10].Longitude + 180); //Patala Lagna (Low. Meridian)
        //    if (Houses[4].Longitude > 360)
        //        Houses[4].Longitude -= 360; //Expunge 360

        //    //Cacl ars
        //    double arcA, arcB, arcC, arcD;

        //    if (Houses[4].Longitude < Houses[1].Longitude)
        //        arcA = ((Houses[4].Longitude + 360) - Houses[1].Longitude);
        //    else
        //        arcA = (Houses[4].Longitude - Houses[1].Longitude);

        //    if (Houses[7].Longitude < Houses[4].Longitude)
        //        arcB = ((Houses[7].Longitude + 360) - Houses[4].Longitude);
        //    else
        //        arcB = (Houses[7].Longitude - Houses[4].Longitude);

        //    if (Houses[10].Longitude < Houses[7].Longitude)
        //        arcC = ((Houses[10].Longitude + 360) - Houses[7].Longitude);
        //    else
        //        arcC = (Houses[10].Longitude - Houses[7].Longitude);

        //    if (Houses[1].Longitude < Houses[10].Longitude)
        //        arcD = ((Houses[1].Longitude + 360) - Houses[10].Longitude);
        //    else
        //        arcD = (Houses[1].Longitude - Houses[10].Longitude);



        //    //Cacl House 2 & 3
        //    Houses[2].Longitude = Houses[1].Longitude + (arcA / 3.0);
        //    Houses[3].Longitude = Houses[2].Longitude + (arcA / 3.0);


        //    //Cacl House 5 & 6
        //    Houses[5].Longitude = Houses[4].Longitude + (arcB / 3.0);
        //    Houses[6].Longitude = Houses[5].Longitude + (arcB / 3.0);

        //    //Cacl House 8 & 9
        //    Houses[8].Longitude = Houses[7].Longitude + (arcC / 3.0);
        //    Houses[9].Longitude = Houses[8].Longitude + (arcC / 3.0);

        //    //Cacl House 11 & 12
        //    Houses[11].Longitude = Houses[10].Longitude + (arcD / 3.0);
        //    Houses[12].Longitude = Houses[11].Longitude + (arcD / 3.0);

        //    for (int i = 1; i < 13; i++)
        //    {
        //        if (Houses[i].Longitude > 360)
        //            Houses[i].Longitude -= 360;
        //    }

        //}





        //public static Angle GetPlanetNirayanaLongitudeBeta(DateTimeOffset date, PlanetName planetName)
        //{
        //    Angle returnValue;

        //    //Converts LMT to UTC (GMT)
        //    DateTimeOffset utcDate = date.ToUniversalTime();

        //    DateTime date12PM = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 12, 0, 0);

        //    DateTime predate12PM = date12PM.Subtract(TimeSpan.FromDays(1));

        //    //Get longitude on day at 12PM
        //    Angle longitude = GetPlanetEphemerisLongitude(date12PM, planetName);

        //    //Get longitude on previous day at 12PM
        //    Angle predaylongitude = GetPlanetEphemerisLongitude(predate12PM, planetName);


        //    //Conversion to Hindu Nirayana Longitudes

        //    //1 - Arc traversed in G. M. T. interval of birth

        //    Angle Speed = longitude - predaylongitude;
        //    TimeSpan GMTInterval = Tools.GMTInterval(date); //LMT to GMT
        //    Angle arcTraversed = Angle.FromDegrees(Math.Exp(Math.Log10(Math.Abs(Speed.TotalDegrees)) + Math.Log10(GMTInterval.TotalHours)));



        //    //2 - Sayana Long, at birth
        //    Angle SayanaLong = Angle.Zero;

        //    if (Speed.TotalDegrees < 0 || planetName == PlanetName.Rahu) //in case of Retrograde planets and Rahu
        //        SayanaLong = Angle.FromDegrees(predaylongitude.TotalDegrees) - arcTraversed;

        //    else if (Speed.TotalDegrees >= 0 || planetName == PlanetName.Sun || planetName == PlanetName.Moon) //in case of Sun, Moon and other planets having direct motion, 
        //        SayanaLong = Angle.FromDegrees(predaylongitude.TotalDegrees) + arcTraversed;



        //    //3 - Hindu Nirayana Long = Sayana Long — Ayanamsa.

        //    Angle birthAnyanamsa = Tools.Ayanamsa(date.Year);

        //    if (SayanaLong.TotalDegrees < birthAnyanamsa.TotalDegrees)
        //        returnValue = (SayanaLong + Angle.FromDegrees(360)) - birthAnyanamsa;
        //    else
        //        returnValue = SayanaLong - birthAnyanamsa;

        //    if (planetName == PlanetName.Ketu)
        //    {
        //        returnValue -= Angle.FromDegrees(180);
        //    }


        //    return returnValue;

        //}



        //private void FillPlanet(Planet value)
        //{
        //    //Convert LMT to GMT
        //    DateTime BirthDateGMT = Tools.LMTtoGMT(BirthDate, BirthLocation);

        //    double[] lon_lat_rad = EphCalc(BirthDateGMT, value.Name);

        //    DateTime predate = BirthDateGMT.Subtract(TimeSpan.FromDays(1));

        //    double[] lon_lat_radPRE = EphCalc(predate, value.Name);

        //    //Conversion to Hindu Nirayana Longitudes

        //    //1 - Arc traversed in G. M. T. interval of birth

        //    value.Speed = lon_lat_rad[0] - lon_lat_radPRE[0];
        //    TimeSpan GMTInterval = Tools.GMTInterval(BirthDateGMT);
        //    Angle arcTraversed = Angle.FromDegrees(Math.Exp(Math.Log10(Math.Abs(value.Speed)) + Math.Log10(GMTInterval.TotalHours)));



        //    //2 - Sayana Long, at birth
        //    Angle SayanaLong = Angle.Zero;

        //    if (value.Motion == PlanetMotion.Retrograde || value.Name == PlanetName.Rahu) //in case of Retrograde planets and Rahu
        //        SayanaLong = Angle.FromDegrees(lon_lat_radPRE[0]) - arcTraversed;

        //    else if (value.Motion == PlanetMotion.Direct || value.Name == PlanetName.Sun || value.Name == PlanetName.Moon) //in case of Sun, Moon and other planets having direct motion, 
        //        SayanaLong = Angle.FromDegrees(lon_lat_radPRE[0]) + arcTraversed;



        //    //3 - Hindu Nirayana Long = Sayana Long — Ayanamsa.

        //    Angle birthAnyanamsa = Tools.Ayanamsa(BirthDate.Year);

        //    if (SayanaLong.TotalDegrees < birthAnyanamsa.TotalDegrees)
        //        value.Longitude = Angle.FromDegrees(360) - (SayanaLong - birthAnyanamsa);
        //    else
        //        value.Longitude = SayanaLong - birthAnyanamsa;

        //    if (value.Name == PlanetName.Ketu)
        //    {
        //        value.Longitude -= Angle.FromDegrees(180);
        //    }
        //}

        ///*
        // DateTime date : has to be in GMT
        // Purp : Calculates ephemeris only for 12PM 
        // */
        //public static double[] EphCalc(DateTime date, PlanetName planetName)
        //{
        //    int year = date.Year;
        //    int month = date.Month;
        //    int day = date.Day;
        //    double hour = 12; // 12PM
        //    int gregflag = SwissEph.SE_GREG_CAL; //GREGORIAN CALENDAR
        //    int iflag = SwissEph.SEFLG_SWIEPH + SwissEph.SEFLG_SPEED;
        //    double[] lon_lat_rad = new double[6], x2 = new double[6], xequ = new double[6], xcart = new double[6],
        //    xcartq = new double[6], xobl = new double[6], xaz = new double[6], xt = new double[6], xsv = new double[6];
        //    string err_msg = "";
        //    double jul_day_ET;
        //    double jul_day_UT;
        //    int planet = 0;

        //    switch (planetName)
        //    {
        //        case PlanetName.Sun:
        //            planet = SwissEph.SE_SUN;
        //            break;
        //        case PlanetName.Moon:
        //            planet = SwissEph.SE_MOON;
        //            break;
        //        case PlanetName.Mars:
        //            planet = SwissEph.SE_MARS;
        //            break;
        //        case PlanetName.Mercury:
        //            planet = SwissEph.SE_MERCURY;
        //            break;
        //        case PlanetName.Jupiter:
        //            planet = SwissEph.SE_JUPITER;
        //            break;
        //        case PlanetName.Venus:
        //            planet = SwissEph.SE_VENUS;
        //            break;
        //        case PlanetName.Saturn:
        //            planet = SwissEph.SE_SATURN;
        //            break;
        //        case PlanetName.Rahu:
        //            planet = SwissEph.SE_MEAN_NODE;
        //            break;
        //        case PlanetName.Ketu:
        //            planet = SwissEph.SE_MEAN_NODE;
        //            break;
        //    }

        //    SwissEph x = new SwissEph();

        //    //Convert DOB to Julian Day
        //    jul_day_UT = x.swe_julday(year, month, day, hour, gregflag);

        //    //Convert Julian Day to Ephemeris Time
        //    jul_day_ET = jul_day_UT + x.swe_deltat(jul_day_UT);

        //    //Calls Ephemeris
        //    int ret_flag = x.swe_calc(jul_day_ET, planet, iflag, lon_lat_rad, ref err_msg);

        //    return lon_lat_rad;
        //}
    }
}
