using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{


    public static class YogaDetector
    {


        [API("Kemadruma", "", Category.StarsAboveMe)]
        public static YogaResult Kemadruma(Time time)
        {
            //get planet longitude
            var moonLongitude = AstronomicalCalculator.GetPlanetNirayanaLongitude(time, PlanetName.Moon);

            //plus 15 to 
        }


    }


    public readonly record struct YogaResult(bool IsOccurred, double Strength, string Notes);
}
