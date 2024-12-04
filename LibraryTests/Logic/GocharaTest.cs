using Microsoft.VisualStudio.TestTools.UnitTesting;
using VedAstro.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.ConditionalFormatting.Contracts;

namespace VedAstro.Library.Tests
{
    [TestClass()]
    public class GocharaTests
    {
        [TestMethod()]
        public void WeakestHouseTest()
        {
            //birth where house 5 is bad
            Time testBirth = new("12:44 23/04/1994 +08:00", GeoLocation.Ipoh);

            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", testBirth, Gender.Male);

            //6th sign from natal moon (Aquarius)
            var _6thSignFromMoon = Calculate.SignCountedFromPlanetSign(6, PlanetName.Moon, testBirth);

            //bindu for aquarius in saturn's ashtakvarga
            var bindu = Calculate.PlanetAshtakvargaBindu(PlanetName.Saturn, _6thSignFromMoon, testBirth);

            Time testNow = new("00:00 15/10/2024 +08:00", GeoLocation.Ipoh);
            var xxx = EventCalculatorMethods.SaturnTransit5Bindu(testNow, johnDoe);
        }
    }
}