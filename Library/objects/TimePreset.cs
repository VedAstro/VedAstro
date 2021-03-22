using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// A simple holder for time presets of hours
    /// Example : 1 minute in hours = 0.01666
    /// </summary>
    public static class TimePreset
    {
        public static readonly double Minute1 = 1.0 / 60.0;
        public static readonly double Minute2 = 2.0 / 60.0;
        public static readonly double Minute3 = 3.0 / 60.0;
        public static readonly double Minute4 = 4.0 / 60.0;
        public static readonly double Minute5 = 5.0 / 60.0;
        public static readonly double Minute10 = 10.0 / 60.0;
        public static readonly double Minute15 = 15.0 / 60.0;
        public static readonly double Minute30 = 30.0 / 60.0;
        public static readonly double Hour1 = 1;
    }
}
