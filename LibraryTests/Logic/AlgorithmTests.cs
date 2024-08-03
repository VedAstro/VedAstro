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
    public class AlgorithmTests
    {
        [TestMethod()]
        public void WeakestHouseTest()
        {
            //birth where house 5 is bad
            Time testBirth = new("12:44 23/04/1994 +08:00", GeoLocation.Bangkok);
            // TEMP hack to place time in Person (wrapped) 
            var johnDoe = new Person("", testBirth, Gender.Male);


            //var saturnTransitHouse5 =
            //    new Event(EventName.SaturnGocharaInHouse5, EventNature.Bad,
            //        "",
            //        Time.Empty,
            //        Time.Empty,
            //        new List<EventTag>() { EventTag.Gochara });

            //get score for weakest house
            //var xx = Algorithm.WeakestHouse(saturnTransitHouse5, johnDoe);

            //Assert.AreEqual(-1, xx);
        }
    }
}