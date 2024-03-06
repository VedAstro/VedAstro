using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    /// <summary>
    /// 60 Tatparas = 1 Para
    /// 60 Paras = 1 Vilipta
    /// 60 Viliptas = 1 Liptha
    /// 60 Lipthas = 1 Vighati
    /// 60 Vighatis = 1 Ghati
    /// 60 Ghatis = 1 Day
    ///
    /// (24 seconds = 1 Vighati; 24 minutes = 1 ghati; 1 hour = 2.5 ghatis.)
    /// </summary>
    public class VedicTime
    {
        private const double TATPARAS_PER_PARA = 60;
        private const double PARAS_PER_VILIPTA = 60;
        private const double VILIPTAS_PER_LIPTHA = 60;
        private const double LIPTHAS_PER_VIGHATI = 60;
        private const double VIGHATIS_PER_GHATI = 60;
        private const double SECONDS_PER_VIGHATI = 24;
        private const double MINUTES_PER_GHATI = 24;
        private const double HOURS_PER_DAY = 2.5;

        // Add the missing constants here:

        public double TatParas { get; private set; }
        public double Paras { get; set; }
        public double Viliptas { get; set; }
        public double Lipthas { get; set; }
        public double Vighatis { get; set; }
        public double Ghatis { get; private set; }


        public static VedicTime FromTotalGhatis(double totalGhatis)
        {
            // Round down to the nearest integer since partial Ghatis aren't allowed.
            long fullGhatis = (long)totalGhatis;
            double remainingGhatis = totalGhatis - fullGhatis;

            // Calculate other properties based on the total Ghatis.
            double vighatisFromRemaining = remainingGhatis * VIGHATIS_PER_GHATI;

            return new VedicTime
            {
                Ghatis = fullGhatis,
                Vighatis = vighatisFromRemaining,
            };
        }

        public override string ToString()
        {
            return $"{this.Ghatis}Gh. {this.Vighatis}Vi.";
        }
    }


}
