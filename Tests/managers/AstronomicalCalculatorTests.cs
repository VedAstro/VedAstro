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
            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var birthTime = new Time("06:42 16/04/2021 +08:00", geoLocation);


            AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, birthTime);
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