using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissEphNet;
using System.Reflection;
using System.IO;

namespace Jothidam
{
    class Program
    {
        static void Main(string[] args)
        {

            executer();

            GeoCoordinate Bangalore = new GeoCoordinate(Angle.FromDegrees(13), Hemisphere.North, new Angle(77, 35, 0), Hemisphere.East);


            GeoCoordinate Std_Place = new GeoCoordinate(Angle.FromDegrees(13), Hemisphere.North, new Angle(77, 35, 0), Hemisphere.East);
            DateTime Std_DOBLMT = new DateTime(1918, 10, 16, 14, 0, 0);
            DateTimeOffset Std_DOB = Tools.DOBFormatter(Std_DOBLMT,Std_Place);

            GeoCoordinate Ils_Place = new GeoCoordinate(Angle.FromDegrees(13), Hemisphere.North, Angle.FromDegrees(75), Hemisphere.East);
            DateTime Ils_DOBLMT = new DateTime(1932, 5, 3, 5, 45, 0);
            DateTimeOffset Ils_DOB = Tools.DOBFormatter(Ils_DOBLMT,Ils_Place);

            GeoCoordinate Vig_Place = new GeoCoordinate(Angle.FromDegrees(4.0161133), Hemisphere.North, Angle.FromDegrees(100.9215738), Hemisphere.East);
            DateTime Vig_DOBLMT = new DateTime(1994, 4, 23, 12, 44, 0);
            DateTimeOffset Vig_DOB = Tools.DOBFormatter(Vig_DOBLMT, Vig_Place);

            GeoCoordinate moon_Place = new GeoCoordinate(Angle.FromDegrees(13), Hemisphere.North, new Angle(77, 35, 0), Hemisphere.East);
            DateTime moon_DOBLMT = new DateTime(1896, 8, 8, 4, 10, 20);
            DateTime moon_DOBGMT = Tools.LMTtoGMT(moon_DOBLMT, moon_Place);


            //Horoscope horscp = new Horoscope(Vig_DOBLMT, Vig_Place);
            //Horoscope horscp = new Horoscope(Std_DOBLMT, Std_Place);
            //Horoscope horscp = new Horoscope(Ils_DOBLMT, Ils_Place);
            ////Horoscope horscp = new Horoscope(moon_DOBLMT, moon_Place);
            //horscp.PrintStuff();

            //for (int i = 0; i < 9; i++)
            //{

            //    Console.WriteLine("{0}\t{1}\t{2}", horscp.Planets[i].Name, (horscp.Planets[i].Longitude.TotalDegrees), 0);

            //}


            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Sun).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Moon).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Mars).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Mercury).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Jupiter).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Venus).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Saturn).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Rahu).TotalDegrees);
            //Console.WriteLine(Tools.GetPlanetNirayanaLongitude(Ils_DOB, PlanetName.Ketu).TotalDegrees);


            //------------------
            //TEMP TEST
            GeoCoordinate temp_localFin = new GeoCoordinate(Angle.FromDegrees(60.200719), Hemisphere.North, Angle.FromDegrees(24.942931), Hemisphere.East);
            DateTime temp_DOBLMT = new DateTime(2017, 1, 23, 15, 47, 0);
            DateTimeOffset Temp_DOB = Tools.DOBFormatter(temp_DOBLMT, temp_localFin);

            double yyy = Tools.GetPlanetNirayanaLongitude(Temp_DOB, PlanetName.Moon).TotalMinutes;
            RulingConstellation xxx = new RulingConstellation(yyy / 800.0);
            //RulingConstellation xxx = Tools.GetRulingConstellation(Temp_DOB);
            Console.WriteLine(xxx);
            Console.ReadLine();
            //-------------------

            GeoCoordinate localFin = new GeoCoordinate(Angle.FromDegrees(60.200719), Hemisphere.North, Angle.FromDegrees(24.942931), Hemisphere.East);
            GeoCoordinate local41 = new GeoCoordinate(Angle.FromDegrees(4.572943), Hemisphere.North, Angle.FromDegrees(101.125033), Hemisphere.East);
            GeoCoordinate localCheras = new GeoCoordinate(Angle.FromDegrees(3.091526), Hemisphere.North, Angle.FromDegrees(101.715225), Hemisphere.East);
            //3.0915260314941406,101.71522521972656

            DateTimeOffset start = Tools.STDtoLMT(new DateTimeOffset(2017, 1, 1, 0, 0, 0, new TimeSpan(2, 0, 0)), localFin);
            DateTimeOffset end = Tools.STDtoLMT(new DateTimeOffset(2017, 12, 31, 0, 0, 0, new TimeSpan(2, 0, 0)), localFin);
            List<DateTimeOffset> dateRange = new List<DateTimeOffset> { start, end };

            GeoCoordinate BirthPlace = new GeoCoordinate(Angle.FromDegrees(4.021758), Hemisphere.North, Angle.FromDegrees(101.020966), Hemisphere.East); //Teluk intan
            //GeoCoordinate BirthPlace = new GeoCoordinate(Angle.FromDegrees(3.917729), Hemisphere.North, Angle.FromDegrees(101.382143), Hemisphere.East); //Trolak
            DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1994, 4, 23, 12, 44, 0, new TimeSpan(8, 0, 0)), BirthPlace); //Vignes
                                                                                                                                   //DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1963, 12, 2, 0, 20, 0, new TimeSpan(8, 0, 0)), BirthPlace); //Anba
                                                                                                                                   //DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1963, 8, 5, 6, 15, 0, new TimeSpan(8, 0, 0)), BirthPlace); //mums
                                                                                                                                   //DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1992, 7, 31, 12, 48, 0, new TimeSpan(8, 0, 0)), BirthPlace); //Dhiviya
                                                                                                                                   //DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1998, 2, 23, 13, 16, 0, new TimeSpan(8, 0, 0)), BirthPlace); //Sindhu



            //Horoscope horscp = new Horoscope(DOBLMT, BirthPlace);



            //SearchInput searchInput = new SearchInput();
            //searchInput.Horoscope = horscp;
            //searchInput.Location = localFin;

            //int[,] selectedFilter = { { 1004, (int)FilterKind.Find }, { 1005, (int)FilterKind.Remove } };

            //List<DateFilter> filterList = DateFilter.CreateList(searchInput, selectedFilter);


            //-------------------------------
            // Print to file code
            //FileStream ostrm;
            //StreamWriter writer;
            //TextWriter oldOut = Console.Out;
            //try
            //{
            //    ostrm = new FileStream("C:/Users/developer/Documents/output.txt", FileMode.OpenOrCreate, FileAccess.Write);
            //    writer = new StreamWriter(ostrm);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Cannot open Redirect.txt for writing");
            //    Console.WriteLine(e.Message);
            //    return;
            //}
            //Console.SetOut(writer);


            //foreach (DateTimeOffset day in Tools.GetStudyDate(dateRange.ToArray()))
            //{
            //    DateTimeOffset dayTemp = Tools.LMTtoSTD(day, StandardTimeKind.EET);
            //    Console.WriteLine(dayTemp);
            //}

            ////Tools.TarabalaBeta(dateRange, horscp);

            //Console.SetOut(oldOut);
            //writer.Close();
            //ostrm.Close();
            //Console.WriteLine("Done");
            //------------------------------




            //foreach (DateTimeOffset day in Tools.GetNailCutDate(dateRange.ToArray()))
            //{
            //    DateTimeOffset dayTemp = Tools.LMTtoSTD(day, StandardTimeKind.EET);
            //    Console.WriteLine(dayTemp);
            //}



            //List<DateTimeOffset> STDlist = Tools.LMTtoSTDList(Tools.GetStudyDate(dateRange.ToArray()), StandardTimeKind.EET);


            //List<DateSpan> zzz = Tools.DateSumerizer(STDlist);

            //foreach (DateSpan x in zzz)
            //{
            //    Console.WriteLine(x);
            //}





            Console.WriteLine("\n");


            Console.WriteLine("done");
            Console.ReadLine();
        

        }


        static void executer()
        {

            GeoCoordinate localFin = new GeoCoordinate(Angle.FromDegrees(60.200719), Hemisphere.North, Angle.FromDegrees(24.942931), Hemisphere.East);
            GeoCoordinate local41 = new GeoCoordinate(Angle.FromDegrees(4.572943), Hemisphere.North, Angle.FromDegrees(101.125033), Hemisphere.East);

            GeoCoordinate BirthPlace = new GeoCoordinate(Angle.FromDegrees(4.021758), Hemisphere.North, Angle.FromDegrees(101.020966), Hemisphere.East); //Teluk intan


            DateTimeOffset DOBLMT = Tools.STDtoLMT(new DateTimeOffset(1994, 4, 23, 12, 44, 0, new TimeSpan(8, 0, 0)), BirthPlace); //Vignes


            DateTimeOffset start = Tools.STDtoLMT(new DateTimeOffset(2017, 7, 22, 0, 0, 0, new TimeSpan(3, 0, 0)), localFin);
            DateTimeOffset end = Tools.STDtoLMT(new DateTimeOffset(2017, 8, 31, 0, 0, 0, new TimeSpan(3, 0, 0)), localFin);
            List<DateTimeOffset> dateRange = new List<DateTimeOffset> { start, end };

            Horoscope horscp = new Horoscope(DOBLMT, BirthPlace);

            List<DateTimeOffset> x = Tools.PersonalTravelFilterBeta(dateRange, localFin, horscp);


           



            Console.WriteLine("\n");


            Console.WriteLine(x);
            Console.ReadLine();


        }



    }


}
