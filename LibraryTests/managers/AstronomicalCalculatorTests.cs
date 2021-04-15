using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genso.Astrology.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Library.Tests
{
    [TestClass()]
    public class AstronomicalCalculatorTests
    {
        [TestMethod()]
        public void GetPlanetSaptavargajaBalaTest()
        {

            Assert.Fail();
        }

        [TestMethod()]
        public void GetPlanetRasiSignTest()
        {
            var endStdTime = DateTimeOffset.ParseExact("06:42 16/04/2021 +08:00", Time.GetDateTimeFormat(), null);
            var geoLocation = new GeoLocation("Ipoh", 101, 4.59);
            var birthTime = new Time(endStdTime, geoLocation);


            AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, birthTime);
        }
    }
}