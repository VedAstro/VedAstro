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
    public class CalculateHoroscopeTests
    {
        Time KarlMarx = new("02:00 05/05/1818 +02:00", new GeoLocation("", 6.637, 49.75));
        Time HavelockEllis = new("08:15 02/02/1859 +00:00", new GeoLocation("", 0.0957, 51.377));
        Time HenryFord = new ("07:00 30/07/1863 -05:32", new GeoLocation("", -83, 42));
        Time HenryFord2 = new ("18:32 29/07/1863 -05:32", new GeoLocation("", -83, 42));

        [TestMethod()]
        public void MoonAshtakavargaYogaTest()
        {
            // In the horoscope of Karl Marx, Moon as
            // 6tb lord is in the 3rd. with only 2 bindus and is 
            // associated with Rāhu. Marx is said to have ruined bis health by overwork. 

            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga(KarlMarx);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        [TestMethod()]
        public void MoonAshtakavargaYogaTest2()
        {
            // Havelock Ellis had the Moon (6th lord) weakly disposed in the
            // 12th associated with only 3 bindus. The Moon 
            // is aspected by Saturn. He had to face heavy
            // litigation due to his writings which were then
            // considered obscene. 

            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga(HavelockEllis);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        [TestMethod()]
        public void MoonAshtakavargaYoga3Test()
        {
            //As an instance I may refer to the case of
            //Henry Ford. The Moon as lord of the 9th is
            //in the 6th associated with 7 bindus.

            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga3(HenryFord);

            Assert.AreEqual(true, isOccuring.Occuring);
        }
    }
}