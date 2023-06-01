using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Genso.Astrology.Library.Tests
{
    [TestClass()]
    public class AstronomicalCalculatorTests
    {
        [TestMethod()]
        public void GetPlanetRasiSignTest()
        {
            //prepare data
            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var birthTime = new Time("06:42 16/04/2021 +08:00", geoLocation);

            //calculate data
            var result = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, birthTime);

            //print data
            Console.WriteLine(result);
        }

        [TestMethod()]
        public void GetEpochIntervalTest()
        {

            //Example 36.-Find the interval in case of a birth on Tuesday,
            //2nd 11 April 1895
            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);//not important
            var birthTime = new Time("00:00 02/04/1895 +00:00", geoLocation);
            var daysInterval = AstronomicalCalculator.GetEpochInterval(birthTime);

            //correct result is 4
            Assert.IsTrue(daysInterval == -1735);
        }

        [TestMethod()]
        public void GetMadhyaTest()
        {
            //tested to confirm bug fix for dates before 1900
            
            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);//not important
            var birthTime = new Time("00:00 01/01/1899 +00:00", geoLocation);
            var daysInterval = AstronomicalCalculator.GetEpochInterval(birthTime);
            var planetMean = AstronomicalCalculator.GetMadhya(daysInterval, birthTime);

            var geoLocation2 = new GeoLocation("Ipoh", 101, 4.59);//not important
            var birthTime2 = new Time("00:00 01/01/1910 +00:00", geoLocation2);
            var daysInterval2 = AstronomicalCalculator.GetEpochInterval(birthTime2);
            var planetMean2 = AstronomicalCalculator.GetMadhya(daysInterval2, birthTime2);

            var geoLocation3 = new GeoLocation("Ipoh", 101, 4.59);//not important
            var birthTime3 = new Time("00:00 01/01/1911 +00:00", geoLocation3);
            var daysInterval3 = AstronomicalCalculator.GetEpochInterval(birthTime3);
            var planetMean3 = AstronomicalCalculator.GetMadhya(daysInterval3, birthTime3);

        }

        [TestMethod()]
        public void CountFromSignToSignTest()
        {
            //test
            var count = AstronomicalCalculator.CountFromSignToSign(ZodiacName.Aquarius, ZodiacName.Taurus);

            //correct result is 4
            Assert.IsTrue(count == 4);

        }


        [TestMethod()]
        public void GetLongitudeAtZodiacSignTest()
        {
            //TEST 190 = 10 in Libra
            var libra10 = new ZodiacSign(ZodiacName.Libra, Angle.FromDegrees(10));
            var longitude = AstronomicalCalculator.GetLongitudeAtZodiacSign(libra10);
            var testSign = AstronomicalCalculator.GetZodiacSignAtLongitude(longitude);

            var expected = libra10.GetDegreesInSign().TotalDegrees;
            var actual = testSign.GetDegreesInSign().TotalDegrees;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetLunarMonthTest1()
        {

            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var time = new Time("15:00 01/01/2022 +08:00", geoLocation);

            var lunarMonthTest = AstronomicalCalculator.GetLunarMonth(time);

            var lunarMonthCorrect = LunarMonth.Margasiram;

            Assert.AreEqual(lunarMonthCorrect, lunarMonthTest);
        }

        [TestMethod()]
        public void GetLunarMonthTest2()
        {

            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var time = new Time("15:00 01/03/2022 +08:00", geoLocation);

            var lunarMonthTest = AstronomicalCalculator.GetLunarMonth(time);

            var lunarMonthCorrect = LunarMonth.Magham;

            Assert.AreEqual(lunarMonthCorrect, lunarMonthTest);
        }

        [TestMethod()]
        public void GetLunarMonthTest3()
        {

            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var time = new Time("15:00 01/01/2021 +08:00", geoLocation);

            var lunarMonthTest = AstronomicalCalculator.GetLunarMonth(time);

            var lunarMonthCorrect = LunarMonth.Margasiram;

            Assert.AreEqual(lunarMonthCorrect, lunarMonthTest);
        }

        [TestMethod()]
        public void GetLunarMonthTest4()
        {

            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var time = new Time("15:00 01/01/2020 +08:00", geoLocation);

            var lunarMonthTest = AstronomicalCalculator.GetLunarMonth(time);

            var lunarMonthCorrect = LunarMonth.Pooshiam;

            Assert.AreEqual(lunarMonthCorrect, lunarMonthTest);
        }
    }
}