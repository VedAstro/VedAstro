using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{


    public static class YogaDetector
    {

        [API("PlanetHoraSign", "", Category.StarsAboveMe)]
        public static bool Kemadruma(Time time)
        {
            //get planet sign
            var planetSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, time);

            //get planet sign name
            var planetSignName = planetSign.GetSignName();

            //get planet degrees in sign
            var degreesInSign = planetSign.GetDegreesInSign().TotalDegrees;

            //declare flags
            var planetInFirstHora = false;
            var planetInSecondHora = false;

            //1.0 get which hora planet is in
            //if sign in first hora (0 to 15 degrees)
            if (degreesInSign >= 0 && degreesInSign <= 15)
            {
                planetInFirstHora = true;
            }

            //if sign in second hora (15 to 30 degrees)
            if (degreesInSign > 15 && degreesInSign <= 30)
            {
                planetInSecondHora = true;
            }

            //2.0 check which type of sign the planet is in

            //if planet is in odd sign
            if (IsOddSign(planetSignName))
            {
                //if planet in first hora
                if (planetInFirstHora == true && planetInSecondHora == false)
                {
                    //governed by the Sun (Leo)
                    return ZodiacName.Leo;
                }

                //if planet in second hora
                if (planetInFirstHora == false && planetInSecondHora == true)
                {
                    //governed by the Moon (Cancer)
                    return ZodiacName.Cancer;
                }

            }


            //if planet is in even sign
            if (IsEvenSign(planetSignName))
            {
                //if planet in first hora
                if (planetInFirstHora == true && planetInSecondHora == false)
                {
                    //governed by the Moon (Cancer)
                    return ZodiacName.Cancer;

                }

                //if planet in second hora
                if (planetInFirstHora == false && planetInSecondHora == true)
                {
                    //governed by the Sun (Leo)
                    return ZodiacName.Leo;
                }

            }

            throw new Exception("Planet hora not found, error!");
        }


    }
}
