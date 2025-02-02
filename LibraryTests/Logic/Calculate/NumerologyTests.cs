using VedAstro.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VedAstro.Library.Tests
{
    [TestClass()]
    public class NumerologyTests
    {
        /// <summary>
        /// In order
        /// to.illustrate the various principles described in
        /// this book, we shall consider the nativity of a
        /// fernale born on 16th October 1918 A.D.; at 2 h 20 m. P.M. (Indian Standard Time) at a place
        /// on 13° N. Lat. and 77° 35' E. Long. This
        /// horoscope will henceforth be termed as the
        /// Stan~1rd Horoscope
        /// </summary>
        public static Time StandardHoroscope = new("14:20 16/10/1918 +05:30", GeoLocation.Bangalore);

        /// <summary>
        /// Brookline, Massachusetts 42.3318° N, 71.1212° W
        /// </summary>
        public static Time JohnFKennedy = new("03:15 29/05/1917 +00:00", new GeoLocation("", -71.1212, 42.3318));


        //PASS
        [TestMethod()]
        public void BirthNumberTest()
        {
            var birthNumber = Calculate.BirthNumber(StandardHoroscope);

            //14:20 16/10/1918 +05:30 == 7 birth number
            Assert.AreEqual(7, birthNumber);

            birthNumber = Calculate.BirthNumber(JohnFKennedy);

            //03:15 29/05/1917 +00:00 == 2 birth number
            Assert.AreEqual(2, birthNumber);
        }


        //PASS
        [TestMethod()]
        public void DestinyNumberTest()
        {
            var destinyNumber = Calculate.DestinyNumber(StandardHoroscope);

            //14:20 16/10/1918 +05:30 == 27 == 9 birth number
            Assert.AreEqual(9, destinyNumber);

            destinyNumber = Calculate.DestinyNumber(JohnFKennedy);

            //03:15 29/05/1917 +00:00 == 34 == 7 birth number
            Assert.AreEqual(7, destinyNumber);
        }

        //PASS
        [TestMethod()]
        public void NameNumberPredictionTest()
        {
            var prediction = Calculate.NameNumberPrediction("Charlie Chaplin");

            Console.WriteLine(prediction);


            //House number predictions
            prediction = Calculate.NameNumberPrediction("13");

            Console.WriteLine(prediction);


            //flight number predictions
            prediction = Calculate.NameNumberPrediction("MH370");

            Console.WriteLine(prediction);

        }


        //PASS
        [TestMethod()]
        public void NameNumberTest()
        {
            var nameNumber = Calculate.NameNumber("Dr. Louis Pasteur");
            Assert.AreEqual(55, nameNumber);

            nameNumber = Calculate.NameNumber("Charles Spencer Chaplin");
            Assert.AreEqual(79, nameNumber);

            nameNumber = Calculate.NameNumber("Charlie Chaplin");
            Assert.AreEqual(46, nameNumber);

            nameNumber = Calculate.NameNumber("Neil A. Armstrong");
            Assert.AreEqual(46, nameNumber);

            nameNumber = Calculate.NameNumber("Vladimir Putin");
            Assert.AreEqual(46, nameNumber);

            nameNumber = Calculate.NameNumber("Robert Frost");
            Assert.AreEqual(46, nameNumber);

            nameNumber = Calculate.NameNumber("Joseph Stalin");
            Assert.AreEqual(46, nameNumber);

        }


    }
}
