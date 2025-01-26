using Microsoft.VisualStudio.TestTools.UnitTesting;
using VedAstro.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bing.ImageSearch.Models;
using SwissEphNet;
using System.Runtime.Intrinsics.X86;

namespace VedAstro.Library.Tests
{
    /// <summary>
    /// All test for book - Ashtakvarga System 0f Prediction - BV Raman
    /// </summary>
    [TestClass()]
    public class CalculateAshtakvargaTests
    {
        Time KarlMarx = new("02:00 05/05/1818 +02:00", new GeoLocation("", 6.637, 49.75));
        Time HavelockEllis = new("08:15 02/02/1859 +00:00", new GeoLocation("", 0.0957, 51.377));
        Time HenryFord = new("07:00 30/07/1863 -05:32", new GeoLocation("", -83, 42));
        Time HenryFord2 = new("18:32 29/07/1863 -05:32", new GeoLocation("", -83, 42));

        Time StandardHoroscope = new("14:20 16/10/1918 +05:30", GeoLocation.Bangalore);

        /// <summary>
        /// PASS : 27/11/2024
        /// </summary>
        [TestMethod()]
        public void SunAshtakavargaYoga10Test()
        {
            // pg. 39
            // In the horoscope of a person born on 8th
            // August 1912 A.D., at 7-35 p.m . (I S.T.) at
            // Bangalore, the Sun is in Cancer in the 6th from
            // Lagna aspected by Jupiter from Scorpio, having
            // 6 bind us in his own Ashtakavarga. The father
            // died in the 31st year of the native, i.e., after the 25th year
            Time horoscope = new("19:35 08/08/1912 +05:30", GeoLocation.Bangalore);

            //sun is in cancer
            var sunSign = Calculate.PlanetRasiD1Sign(PlanetName.Sun, horoscope);
            Assert.AreEqual(ZodiacName.Cancer, sunSign.GetSignName()); //check if sun's sign is cancer

            var countFromLagna = Calculate.SignCountedFromLagnaSign(6, horoscope);
            Assert.AreEqual(ZodiacName.Cancer, countFromLagna); //check if 6th sign is cancer

            //6 bind us in his own Ashtakavarga
            var bindus = Calculate.PlanetAshtakvargaBindu(PlanetName.Sun, sunSign.GetSignName(), horoscope);
            Assert.AreEqual(6, bindus); //check if 6 bindus

            //aspected by Jupiter
            var aspectedByJupiter = Calculate.IsPlanetAspectedByPlanet(PlanetName.Sun, PlanetName.Jupiter, horoscope);
            Assert.AreEqual(true, aspectedByJupiter); //check if aspect by jupiter

            //above test should also be verified by same yoga calculation
            var isOccuring = CalculateHoroscope.SunAshtakavargaYoga10(horoscope);

            Assert.AreEqual(true, isOccuring.Occuring);

        }

        [TestMethod()]
        public void SunAshtakavargaYoga3Test()
        {
            //In the horoscope of "Roosevelt" the Sun, as lord of Lagna, is in Lagna,
            //associated with 5 bindus. According to classical texts, such a disposition
            //of the Sun clearly indicates long life and kingship (nrupatischirayu).
            //Naradeeya is emphatic that when the Sun as lord of Lagna is associated with
            //5, 6 or 7 bindus, the native becomes a "King of many countries" (Bahu-bhumipala).

            Time franklinDRoosevelt = new Time(new LocalMeanTime("20:00 30/08/1882", -73.9), TimeSpan.Zero, new GeoLocation("Hyde Park, USA", -73.935242, 41.791840));

            //Time franklinDRoosevelt = new("01:37 30/08/1882 +00:00", new GeoLocation("Hyde Park, USA", -73.935242, 41.791840));

            var isOccuring = CalculateHoroscope.SunAshtakavargaYoga3(franklinDRoosevelt);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        [TestMethod()]
        public void MoonAshtakavargaYogaTest()
        {
            // In the horoscope of Karl Marx, Moon as
            // 6th lord is in the 3rd. with only 2 bindus and is 
            // associated with Rāhu. Marx is said to have ruined bis health by overwork. 

            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga1A(KarlMarx);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        /// <summary>
        /// PASS : 29/11/2024
        /// </summary>
        [TestMethod()]
        public void MoonAshtakavargaYogaTest2()
        {
            // Havelock Ellis had the Moon (6th lord) weakly disposed in the
            // 12th associated with only 3 bindus. The Moon 
            // is aspected by Saturn. He had to face heavy
            // litigation due to his writings which were then
            // considered obscene. 

            //aspected by Saturn
            var aspectedBySaturn = Calculate.IsPlanetAspectedByPlanet(PlanetName.Moon, PlanetName.Saturn, HavelockEllis);
            Assert.AreEqual(true, aspectedBySaturn);


            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga1A(HavelockEllis);
            Assert.AreEqual(true, isOccuring.Occuring);
        }

        /// <summary>
        /// PASS : 28/11/2024
        /// </summary>
        [TestMethod()]
        public void MoonAshtakavargaYoga3Test()
        {
            //As an instance I may refer to the case of
            //Henry Ford. The Moon as lord of the 9th is
            //in the 6th associated with 7 bindus.

            var isOccuring = CalculateHoroscope.MoonAshtakavargaYoga2B(HenryFord);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        /// <summary>
        /// PASS : 30/11/2024
        /// </summary>
        [TestMethod()]
        public void MarsAshtakavargaYoga7Test()
        {
            // page 53
            // One will become wealthy if Mars, associated with 4 or more bindus, 
            // occupies the Lagna, Chandra Lagna (Moon's house), or the 9th or 10th house, 
            // which should also happen to be his own or exaltation sign.

            //In the Standard Horoscope, Mars is in the 10th from the Moon, having 5 bindus (BR).
            //Though born in a middle class family, the native became fairly rich.

            var isOccuring = CalculateHoroscope.MarsAshtakavargaYoga7(StandardHoroscope);

            Assert.AreEqual(true, isOccuring.Occuring);
        }

        /// <summary>
        /// PASS : 4/12/2024
        /// </summary>
        [TestMethod()]
        public void MarsAshtakavargaYoga8And12Test()
        {
            // page 62
            // In the Standard Horoscope, Mercury (Chart No. 12) is in a quadrant with 3 bindus aspected by Jupiter.
            // Ketu is in the 5th associated with 6 bindus while combination (8) requires only 3 bindus.
            // The native's Mercury's Dasa begins in 1964. She has immense interest in Vedic learning
            // and Astrology and Mercury's Dasa should therefore prove highly significant in enabling
            // her to attain good insight into these branches of knowledge. Venus, lord of the sign occupied by Mercury,
            // is in the 9th with 5 bindus. Consequently, combination (12) bestows great intelligence on the subject.

            var isOccuring = CalculateHoroscope.MercuryAshtakavargaYoga8(StandardHoroscope);
            Assert.AreEqual(true, isOccuring.Occuring);

            var isOccuringB = CalculateHoroscope.MercuryAshtakavargaYoga12A(StandardHoroscope);
            Assert.AreEqual(true, isOccuring.Occuring);
        }
    }
}