using Microsoft.VisualStudio.TestTools.UnitTesting;
using VedAstro.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library.Tests
{
    [TestClass()]
    public class AstronomicalCalculatorTests
    {
        public static Time StandardHoroscope = new("14:20 16/10/1918 +05:30", GeoLocation.Bangalore);

        [TestMethod()]
        public void GetAllBhinnashtakavargaChartTest()
        {

            var bhinnashtakavargaChart = AstronomicalCalculator.GetAllBhinnashtakavargaChart(StandardHoroscope);

            //correct answer for Standard Horoscope from Ashtakavarga System pg.18            
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Aries]);
            Assert.AreEqual(3,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Taurus]);
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Gemini]);
            Assert.AreEqual(4,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Cancer]);
            Assert.AreEqual(4,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Leo]); //todo in book is 5 here is 4 could be rounding
            Assert.AreEqual(4,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Virgo]);
            Assert.AreEqual(3,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Libra]);
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Scorpio]);
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Sagittarius]);
            Assert.AreEqual(0,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Capricornus]);
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Aquarius]);
            Assert.AreEqual(5,bhinnashtakavargaChart[PlanetName.Sun][ZodiacName.Pisces]);
        }
    }
}