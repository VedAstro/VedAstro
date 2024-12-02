using Microsoft.VisualStudio.TestTools.UnitTesting;
using VedAstro.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VedAstro.Library.PlanetName;
using ScottPlot.Drawing.Colormaps;
using SwissEphNet;

namespace VedAstro.Library.Tests
{
    /// <summary>
    /// Test From Book - Manual Of Hindu Astrology (1935) - BV RAMAN
    /// </summary>
    [TestClass()]
    public class ManualOfHinduAstrologyTests
    {

        private static Time StandardHoroscope = new("14:20 16/10/1918 +05:30", GeoLocation.Bangalore);
        private static Time IllustratedHoroscope = new Time(new LocalMeanTime("05:45 03/05/1932", 75), TimeSpan.Zero, new GeoLocation("", 75, 13));

        /// <summary>
        /// Article 97 - Manual Of Hindu Astrology - pg 78
        /// PASS : 21/11/2024
        /// </summary>
        [TestMethod()]
        public void LmtToStdTest()
        {
            DateTimeOffset gmtIllustratedHoroscope = Calculate.LmtToStd(new LocalMeanTime("05:45 03/05/1932", 75), TimeSpan.Zero); //time span 0 for GMT/UTC Greenwich

            //same day at 12:45AM
            var correctGmt = new DateTimeOffset(1932, 05, 03, 0, 45, 0, TimeSpan.Zero);

            Assert.AreEqual(correctGmt, gmtIllustratedHoroscope);
        }

        /// <summary>
        /// Article 49 - Manual Of Hindu Astrology - pg 22
        /// PASS : 21/11/2024
        /// </summary>
        [TestMethod()]
        public void AyanamsaDegreeTest()
        {
            Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

            // Example 1: Determine the Ayanamsa for 1912 A.D.
            // Calculation steps:
            // 1912 - 397 = 1515
            // 1515 × 50⅓ = 76,255"
            // Result:
            // 76,255" = 21° 10' 55"
            var correctAyanamsa1 = new Angle(21, 10, 55);
            var calculatedAyanamsa1 = Calculate.AyanamsaDegree(new Time("00:00 01/10/1912 +05:30", GeoLocation.Bangalore));
            Assert.AreEqual(correctAyanamsa1, calculatedAyanamsa1);

            // Example 2: Find the Ayanamsa for 1918 A.D.
            // Calculation steps:
            // 1918 - 397 = 1521
            // 1521 × 50⅓ = 76,557"
            // Result:
            // 76,557" = 21° 15' 57"
            var correctAyanamsa2 = new Angle(21, 15, 57);
            var calculatedAyanamsa2 = Calculate.AyanamsaDegree(new Time("00:00 01/10/1918 +05:30", GeoLocation.Bangalore));
            Assert.AreEqual(correctAyanamsa2, calculatedAyanamsa2);

        }

        /// <summary>
        /// Article 72 - Manual Of Hindu Astrology - pg 56
        /// PASS : 3/12/2024 (1.8 degrees tolerance!)
        /// </summary>
        [TestMethod()]
        public void PlanetNirayanaLongitudeTest()
        {
            Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

            // Planetary Positions from pg 56
            var sunCorrectLong = new Angle(179, 8, 0);
            var moonCorrectLong = new Angle(311, 40, 0);
            var marsCorrectLong = new Angle(229, 49, 0);
            var mercuryCorrectLong = new Angle(180, 33, 0);
            var jupiterCorrectLong = new Angle(83, 35, 0);
            var venusCorrectLong = new Angle(170, 4, 0);
            var saturnCorrectLong = new Angle(124, 51, 0);
            var rahuCorrectLong = new Angle(233, 23, 0);
            var kethuCorrectLong = new Angle(53, 23, 0);


            //get longitudes of all planets
            var sunLongitude = Calculate.PlanetNirayanaLongitude(Sun, StandardHoroscope);
            var moonLongitude = Calculate.PlanetNirayanaLongitude(Moon, StandardHoroscope);
            var marsLongitude = Calculate.PlanetNirayanaLongitude(Mars, StandardHoroscope);
            var mercuryLongitude = Calculate.PlanetNirayanaLongitude(Mercury, StandardHoroscope);
            var jupiterLongitude = Calculate.PlanetNirayanaLongitude(Jupiter, StandardHoroscope);
            var venusLongitude = Calculate.PlanetNirayanaLongitude(Venus, StandardHoroscope);
            var saturnLongitude = Calculate.PlanetNirayanaLongitude(Saturn, StandardHoroscope);
            var rahuLongitude = Calculate.PlanetNirayanaLongitude(Rahu, StandardHoroscope);
            var ketuLongitude = Calculate.PlanetNirayanaLongitude(Ketu, StandardHoroscope);

            // Test with values calculation with tolerance
            var toleranceLimit = 1.8;

            Assert.AreEqual(sunCorrectLong.TotalDegrees, sunLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(moonCorrectLong.TotalDegrees, moonLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(marsCorrectLong.TotalDegrees, marsLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(mercuryCorrectLong.TotalDegrees, mercuryLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(jupiterCorrectLong.TotalDegrees, jupiterLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(venusCorrectLong.TotalDegrees, venusLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(saturnCorrectLong.TotalDegrees, saturnLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(rahuCorrectLong.TotalDegrees, rahuLongitude.TotalDegrees, toleranceLimit);
            Assert.AreEqual(kethuCorrectLong.TotalDegrees, ketuLongitude.TotalDegrees, toleranceLimit);

        }

        /// <summary>
        /// Article 90 - Manual Of Hindu Astrology - pg 74
        /// FOR : Standard Horoscope
        /// PASS : 3/12/2024 (1.9 degrees tolerance!)
        /// </summary>
        [TestMethod()]
        public void HouseLongitudesTest1()
        {
            Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

            //correct house longitudes for standard horoscope
            var house1 = new House(HouseName.House1, new Angle(281, 36, 40), new Angle(294, 57, 0), new Angle(311, 36, 40));
            var house2 = new House(HouseName.House2, new Angle(311, 36, 40), new Angle(328, 16, 20), new Angle(344, 56, 0));
            var house3 = new House(HouseName.House3, new Angle(344, 56, 0), new Angle(1, 35, 40), new Angle(18, 15, 20));
            var house4 = new House(HouseName.House4, new Angle(18, 15, 20), new Angle(34, 55, 0), new Angle(48, 15, 20));
            var house5 = new House(HouseName.House5, new Angle(48, 15, 20), new Angle(61, 35, 40), new Angle(74, 56, 0));
            var house6 = new House(HouseName.House6, new Angle(74, 56, 0), new Angle(88, 16, 20), new Angle(101, 36, 40));
            var house7 = new House(HouseName.House7, new Angle(101, 36, 40), new Angle(114, 57, 0), new Angle(131, 36, 40));
            var house8 = new House(HouseName.House8, new Angle(131, 36, 40), new Angle(148, 16, 20), new Angle(164, 56, 0));
            var house9 = new House(HouseName.House9, new Angle(164, 56, 0), new Angle(181, 35, 40), new Angle(198, 15, 20));
            var house10 = new House(HouseName.House10, new Angle(198, 15, 20), new Angle(214, 55, 0), new Angle(228, 15, 20));
            var house11 = new House(HouseName.House11, new Angle(228, 15, 20), new Angle(241, 35, 40), new Angle(254, 56, 0));
            var house12 = new House(HouseName.House12, new Angle(254, 56, 0), new Angle(268, 16, 20), new Angle(281, 36, 40));

            //calculates tests
            List<House> houseLongitudes = Calculate.AllHouseLongitudes(StandardHoroscope);

            // Check if calculated values are correct with tolerance
            var toleranceLimit = 1.9;

            Assert.AreEqual(house1.GetBeginLongitude().TotalDegrees, houseLongitudes[0].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house1.GetMiddleLongitude().TotalDegrees, houseLongitudes[0].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house1.GetEndLongitude().TotalDegrees, houseLongitudes[0].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house2.GetBeginLongitude().TotalDegrees, houseLongitudes[1].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house2.GetMiddleLongitude().TotalDegrees, houseLongitudes[1].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house2.GetEndLongitude().TotalDegrees, houseLongitudes[1].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house3.GetBeginLongitude().TotalDegrees, houseLongitudes[2].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house3.GetMiddleLongitude().TotalDegrees, houseLongitudes[2].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house3.GetEndLongitude().TotalDegrees, houseLongitudes[2].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house4.GetBeginLongitude().TotalDegrees, houseLongitudes[3].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house4.GetMiddleLongitude().TotalDegrees, houseLongitudes[3].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house4.GetEndLongitude().TotalDegrees, houseLongitudes[3].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house5.GetBeginLongitude().TotalDegrees, houseLongitudes[4].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house5.GetMiddleLongitude().TotalDegrees, houseLongitudes[4].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house5.GetEndLongitude().TotalDegrees, houseLongitudes[4].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house6.GetBeginLongitude().TotalDegrees, houseLongitudes[5].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house6.GetMiddleLongitude().TotalDegrees, houseLongitudes[5].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house6.GetEndLongitude().TotalDegrees, houseLongitudes[5].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house7.GetBeginLongitude().TotalDegrees, houseLongitudes[6].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house7.GetMiddleLongitude().TotalDegrees, houseLongitudes[6].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house7.GetEndLongitude().TotalDegrees, houseLongitudes[6].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house8.GetBeginLongitude().TotalDegrees, houseLongitudes[7].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house8.GetMiddleLongitude().TotalDegrees, houseLongitudes[7].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house8.GetEndLongitude().TotalDegrees, houseLongitudes[7].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house9.GetBeginLongitude().TotalDegrees, houseLongitudes[8].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house9.GetMiddleLongitude().TotalDegrees, houseLongitudes[8].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house9.GetEndLongitude().TotalDegrees, houseLongitudes[8].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house10.GetBeginLongitude().TotalDegrees, houseLongitudes[9].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house10.GetMiddleLongitude().TotalDegrees, houseLongitudes[9].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house10.GetEndLongitude().TotalDegrees, houseLongitudes[9].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house11.GetBeginLongitude().TotalDegrees, houseLongitudes[10].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house11.GetMiddleLongitude().TotalDegrees, houseLongitudes[10].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house11.GetEndLongitude().TotalDegrees, houseLongitudes[10].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house12.GetBeginLongitude().TotalDegrees, houseLongitudes[11].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house12.GetMiddleLongitude().TotalDegrees, houseLongitudes[11].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house12.GetEndLongitude().TotalDegrees, houseLongitudes[11].GetEndLongitude().TotalDegrees, toleranceLimit);

        }

        /// <summary>
        /// Manual Of Hindu Astrology - pg 87
        /// FOR : Illustrated Horoscope
        /// PASS : 3/12/2024 (0.7 degrees tolerance!)
        /// </summary>
        [TestMethod()]
        public void HouseLongitudesTest2()
        {
            Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

            //correct house longitudes for standard horoscope
            var house1 = new House(HouseName.House1, new Angle(4, 35.5, 0), new Angle(21, 0, 0), new Angle(34, 35.5, 0));
            var house2 = new House(HouseName.House2, new Angle(34, 35.5, 0), new Angle(48, 11, 0), new Angle(61, 56.5, 0));
            var house3 = new House(HouseName.House3, new Angle(61, 46.5, 0), new Angle(75, 22, 0), new Angle(88, 57, 0));
            var house4 = new House(HouseName.House4, new Angle(88, 57, 0), new Angle(102, 32, 0), new Angle(118, 57, 0));
            var house5 = new House(HouseName.House5, new Angle(118, 57, 0), new Angle(135, 22, 0), new Angle(151, 46.5, 0));
            var house6 = new House(HouseName.House6, new Angle(151, 46.5, 0), new Angle(168, 11, 0), new Angle(184, 35.5, 0));
            var house7 = new House(HouseName.House7, new Angle(184, 35.5, 0), new Angle(201, 0, 0), new Angle(214, 35.5, 0));
            var house8 = new House(HouseName.House8, new Angle(214, 35.5, 0), new Angle(228, 11, 0), new Angle(241, 46.5, 0));
            var house9 = new House(HouseName.House9, new Angle(241, 46.5, 0), new Angle(255, 22, 0), new Angle(268, 57, 0));
            var house10 = new House(HouseName.House10, new Angle(268, 57, 0), new Angle(282, 32, 0), new Angle(298, 57, 0));
            var house11 = new House(HouseName.House11, new Angle(298, 57, 0), new Angle(315, 22, 0), new Angle(331, 46.5, 0));
            var house12 = new House(HouseName.House12, new Angle(331, 46.5, 0), new Angle(348, 11, 0), new Angle(4, 35.5, 0));


            //calculates tests
            List<House> houseLongitudes = Calculate.AllHouseLongitudes(IllustratedHoroscope);

            // Check if calculated values are correct with tolerance
            var toleranceLimit = 0.7;

            Assert.AreEqual(house1.GetBeginLongitude().TotalDegrees, houseLongitudes[0].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house1.GetMiddleLongitude().TotalDegrees, houseLongitudes[0].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house1.GetEndLongitude().TotalDegrees, houseLongitudes[0].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house2.GetBeginLongitude().TotalDegrees, houseLongitudes[1].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house2.GetMiddleLongitude().TotalDegrees, houseLongitudes[1].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house2.GetEndLongitude().TotalDegrees, houseLongitudes[1].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house3.GetBeginLongitude().TotalDegrees, houseLongitudes[2].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house3.GetMiddleLongitude().TotalDegrees, houseLongitudes[2].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house3.GetEndLongitude().TotalDegrees, houseLongitudes[2].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house4.GetBeginLongitude().TotalDegrees, houseLongitudes[3].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house4.GetMiddleLongitude().TotalDegrees, houseLongitudes[3].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house4.GetEndLongitude().TotalDegrees, houseLongitudes[3].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house5.GetBeginLongitude().TotalDegrees, houseLongitudes[4].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house5.GetMiddleLongitude().TotalDegrees, houseLongitudes[4].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house5.GetEndLongitude().TotalDegrees, houseLongitudes[4].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house6.GetBeginLongitude().TotalDegrees, houseLongitudes[5].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house6.GetMiddleLongitude().TotalDegrees, houseLongitudes[5].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house6.GetEndLongitude().TotalDegrees, houseLongitudes[5].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house7.GetBeginLongitude().TotalDegrees, houseLongitudes[6].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house7.GetMiddleLongitude().TotalDegrees, houseLongitudes[6].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house7.GetEndLongitude().TotalDegrees, houseLongitudes[6].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house8.GetBeginLongitude().TotalDegrees, houseLongitudes[7].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house8.GetMiddleLongitude().TotalDegrees, houseLongitudes[7].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house8.GetEndLongitude().TotalDegrees, houseLongitudes[7].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house9.GetBeginLongitude().TotalDegrees, houseLongitudes[8].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house9.GetMiddleLongitude().TotalDegrees, houseLongitudes[8].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house9.GetEndLongitude().TotalDegrees, houseLongitudes[8].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house10.GetBeginLongitude().TotalDegrees, houseLongitudes[9].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house10.GetMiddleLongitude().TotalDegrees, houseLongitudes[9].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house10.GetEndLongitude().TotalDegrees, houseLongitudes[9].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house11.GetBeginLongitude().TotalDegrees, houseLongitudes[10].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house11.GetMiddleLongitude().TotalDegrees, houseLongitudes[10].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house11.GetEndLongitude().TotalDegrees, houseLongitudes[10].GetEndLongitude().TotalDegrees, toleranceLimit);

            Assert.AreEqual(house12.GetBeginLongitude().TotalDegrees, houseLongitudes[11].GetBeginLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house12.GetMiddleLongitude().TotalDegrees, houseLongitudes[11].GetMiddleLongitude().TotalDegrees, toleranceLimit);
            Assert.AreEqual(house12.GetEndLongitude().TotalDegrees, houseLongitudes[11].GetEndLongitude().TotalDegrees, toleranceLimit);

        }


    }
}