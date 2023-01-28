using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{

    /// <summary>
    /// C tools
    /// </summary>
    public static partial class C
    {
        /// <summary>
        /// 
        /// </summary>
        public static double atof(String s) {
            double result = 0;
            if (double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
                return result;
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public static double fmod(double numer, double denom)
        {
            return numer % denom;
        }

    }

}
