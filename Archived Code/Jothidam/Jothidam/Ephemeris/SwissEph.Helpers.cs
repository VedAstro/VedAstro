using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
	/// <summary>
	/// Some helpers
	/// </summary>
    partial class SwissEph
    {

        /// <summary>
        /// Get the hour decimal value
        /// </summary>
        /// <param name="hour">Hour</param>
        /// <param name="minute">Minute</param>
        /// <param name="second">Second</param>
        /// <returns>The hour in decimal valeu</returns>
        public static double GetHourValue(int hour, int minute, int second) {
            return (Double)hour + (minute / 60.0) + (second / 3600.0);
        }

    }
}
