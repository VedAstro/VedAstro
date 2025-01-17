using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VedAstro.Library.Tests
{
    /// <summary>
    /// All test for book - Predicting through Jaimini's Chara Dasha: K N Rao
    /// </summary>
    [TestClass()]
    public class CalculateJaiminiTests
    {
        [TestMethod()]
        public void JaiminiCahraDasaTest()
        {
            //In one year and six months after January 1981, we reach sub-period of Pisces. 
            //Pisces is aspected by the Putrakaraka(child-giver) Mercury.
            //In August 1982 she gave birth to a daughter.

            Calculate.Ayanamsa = (int)Ayanamsa.RAMAN;

            //Jan 19. 1958 2:45 pm. Hanoi 
            Time testHoroscope = new("14:45 19/01/1958 +05:30", new GeoLocation("Hanoi", 105.8342, 21.0278)); //° N, ° E

            //get sub period 
            //One year and six months after January 1981 = July 1982.
            Time checkTime = new("00:00 05/08/1982 +05:30", new GeoLocation("Hanoi", 105.8342, 21.0278)); //° N, ° E
            var charaDasa = Calculate.GetCharaDasaAtTime(testHoroscope, checkTime);

            var planetsAspectingSign = Calculate.SignsPlanetIsAspecting(PlanetName.Mercury, testHoroscope);

        }

    }
}