/*
   This is a port of the Swiss Ephemeris Free Edition, Version 2.00.00
   of Astrodienst AG, Switzerland from the original C Code to .Net. For
   copyright see the original copyright notices below and additional
   copyright notes in the file named LICENSE, or - if this file is not
   available - the copyright notes at http://www.astro.ch/swisseph/ and
   following.
   
   For any questions or comments regarding this port, you should
   ONLY contact me and not Astrodienst, as the Astrodienst AG is not involved
   in this port in any way.

   Yanos : ygrenier@ygrenier.com
*/

/* SWISSEPH 
 $Header: /home/dieter/sweph/RCS/swehel.c,v 1.1 2009/04/21 06:05:59 dieter Exp dieter $

  Heliacal risings and related calculations
  
  Author: Victor Reijs
  This program code is a translation of part of:
  Victor Reijs' software ARCHAEOCOSMO (archaeoastronomy and
  geodesy functions), 
  http://www.iol.ie/~geniet/eng/archaeocosmoprocedures.htm

  Translation from VB into C by Dieter Koch

  Problem reports can be sent to victor.reijs@gmail.com or dieter@astro.ch
  
  Copyright (c) Victor Reijs, 2008

  License conditions
  ------------------

  This file is part of Swiss Ephemeris.

  Swiss Ephemeris is distributed with NO WARRANTY OF ANY KIND.  No author
  or distributor accepts any responsibility for the consequences of using it,
  or for whether it serves any particular purpose or works at all, unless he
  or she says so in writing.  

  Swiss Ephemeris is made available by its authors under a dual licensing
  system. The software developer, who uses any part of Swiss Ephemeris
  in his or her software, must choose between one of the two license models,
  which are
  a) GNU public license version 2 or later
  b) Swiss Ephemeris Professional License

  The choice must be made before the software developer distributes software
  containing parts of Swiss Ephemeris to others, and before any public
  service using the developed software is activated.

  If the developer choses the GNU GPL software license, he or she must fulfill
  the conditions of that license, which includes the obligation to place his
  or her whole software project under the GNU GPL or a compatible license.
  See http://www.gnu.org/licenses/old-licenses/gpl-2.0.html

  If the developer choses the Swiss Ephemeris Professional license,
  he must follow the instructions as found in http://www.astro.com/swisseph/ 
  and purchase the Swiss Ephemeris Professional Edition from Astrodienst
  and sign the corresponding license contract.

  The License grants you the right to use, copy, modify and redistribute
  Swiss Ephemeris, but only under certain conditions described in the License.
  Among other things, the License requires that the copyright notices and
  this notice be preserved on all copies.

  The authors of Swiss Ephemeris have no control or influence over any of
  the derived works, i.e. over software or services created by other
  programmers which use Swiss Ephemeris functions.

  The names of the authors or of the copyright holder must not
  be used for promoting any software, product or service which uses or contains
  the Swiss Ephemeris. This copyright notice is the ONLY place where the
  names of the authors can legally appear, except in cases where they have
  given special permission in writing.

  The trademarks 'Swiss Ephemeris' and 'Swiss Ephemeris inside' may be used
  for promoting such software, products or services.
*/
namespace SwissEphNet.CPort
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class SweHel : BaseCPort
    {
        public SweHel(SwissEph se)
            : base(se) {
        }


        int PLSV = 0; /*if Planet, Lunar and Stellar Visibility formula is needed PLSV=1*/
        const double criticalangle = 0.0; /*[deg]*/
        const double BNIGHT = 1479.0; /*[nL]*/
        const double BNIGHT_FACTOR = 1.0;
        //const double PI = Math.PI;
        const double Min2Deg = (1.0 / 60.0);
        const int DEBUG = 0;
        const int DONE = 1;
        const int MaxTryHours = 4;
        const int TimeStepDefault = 1;
        const int LocalMinStep = 8;

        /* time constants */
        const double Y2D = 365.25; /*[Day]*/
        const double D2Y = (1 / Y2D); /*[Year]*/
        const double D2H = 24.0; /*[Hour]*/
        const double H2S = 3600.0; /*[sec]*/
        const double D2S = (D2H * H2S); /*[sec]*/
        const double S2H = (1.0 / H2S); /*[Hour]*/
        const double JC2D = 36525.0; /*[Day]*/
        const double M2S = 60.0; /*[sec]*/

        /* Determines which algorimths are used*/
        //int USE_DELTA_T_VR = 0;
        const int REFR_SINCLAIR = 0;
        const int REFR_BENNETTH = 1;
        const int FormAstroRefrac = REFR_SINCLAIR; /*for Astronomical refraction can be "bennetth" or "sinclair"*/
        const int GravitySource = 2; /*0=RGO, 1=Wikipedia,2=Exp. Suppl. 1992,3=van der Werf*/
        const int REarthSource = 1; /*0=RGO (constant), 1=WGS84 method*/

        //static int StartYear = 1820; /*[year]*/
        const double Average = 1.80546834626888; /*[msec/cy]*/
        const double Periodicy = 1443.67123144531; /*[year]*/
        const double Amplitude = 3.75606495492684; /*[msec]*/
        const int phase = 0; /*[deg]*/
        const int MAX_COUNT_SYNPER = 5;  /* search within 10 synodic periods */
        const int MAX_COUNT_SYNPER_MAX = 1000000;  /* high, so there is not max count */
        const double AvgRadiusMoon = (15.541 / 60); /* '[Deg] at 2007 CE or BCE*/

        /* WGS84 ellipsoid constants
         * http://w3sli.wcape.gov.za/Surveys/Mapping/wgs84.htm*/
        const double Ra = 6378136.6;   /*'[m]*/
        const double Rb = 6356752.314; /*'[m]*/

        /* choices in Schaefer's model */
        const double nL2erg = (1.02E-15);
        const double erg2nL = (1 / nL2erg); /*erg2nL to nLambert*/
        const double MoonDistance = 384410.4978; /*[km]*/
        const double scaleHwater = 3000.0; /*[m] Ricchiazzi [1997] 8200 Schaefer [2000]*/
        const double scaleHrayleigh = 8515.0; /*[m] Su [2003] 8200 Schaefer [2000]*/
        const double scaleHaerosol = 3745.0; /*m Su [2003] 1500 Schaefer [2000]*/
        const double scaleHozone = 20000.0; /*[m] Schaefer [2000]*/
        const double astr2tau = 0.921034037197618;  /*LN(10 ^ 0.4)*/
        const double tau2astr = 1.0 / astr2tau;

        /* meteorological constants*/
        const double C2K = 273.15; /*[K]*/
        const double DELTA = 18.36;
        const double TempNulDiff = 0.000001;
        const double PressRef = 1000; /*[mbar]*/
        const double MD = 28.964; /*[kg] Mol weight of dry air van der Werf*/
        const double MW = 18.016; /*[kg] Mol weight of water vapor*/
        const double GCR = 8314.472; /*[L/kmol/K] van der Werf*/
        const double LapseSA = 0.0065; /*[K/m] standard atmosphere*/
        const double LapseDA = 0.0098; /*[K/m] dry adiabatic*/

        /* lowest apparent altitude to provide*/
        const double LowestAppAlt = -3.5; /*[Deg]*/

        /*optimization delta*/
        const double swehel_epsilon = 0.001;
        /* for Airmass usage*/
        int staticAirmass = 0; /* use staticAirmass=1 instead depending on difference k's*/

        /* optic stuff */
        const int GOpticMag = 1; /*telescope magnification*/
        const double GOpticTrans = 0.8; /*telescope transmission*/
        const int GBinocular = 1; /*1-binocular 0=monocular*/
        const int GOpticDia = 50; /*telescope diameter [mm]*/

        double mymin(double a, double b) {
            if (a <= b)
                return a;
            return b;
        }

        double mymax(double a, double b) {
            if (a >= b)
                return a;
            return b;
        }

        /*###################################################################*/
        double Tanh(double x) {
            return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x));
        }

        /*
        ' B [nL]
        ' SN [-]
        ' CVA [deg]
        */
        double CVA(double B, double SN) {
            /*Schaefer, Astronomy and the limits of vision, Archaeoastronomy, 1993*/
            if (B > BNIGHT)
                return (40.0 / SN) * Math.Pow(10, (8.28 * Math.Pow(B, (-0.29)))) / 60.0 / 60.0;
            else
                return mymin(900, 380 / SN * Math.Pow(10, (0.3 * Math.Pow(B, (-0.29))))) / 60.0 / 60.0;
        }

        /*
        ' age [year]
        ' B [nL]
        ' PupilDia [mm]
        */
        double PupilDia(double Age, double B) {
            /* age dependancy from Garstang [2000]*/
            return (0.534 - 0.00211 * Age - (0.236 - 0.00127 * Age) * Tanh(0.4 * Math.Log(B) / Math.Log(10) - 2.2)) * 10;
        }

        /*
        'Input
        ' Bback [nL]
        ' kX [-]
        ' Binocular [-]
        ' OpticMag [-]
        ' OpticDia [mm]
        ' OpticTrans [-]
        ' JDNDaysUT [JDN]
        ' Age [Year]
        ' SN [-]
        ' ObjectName
        ' TypeFactor [0=itensity factor 1=background factor]
        'Output
        ' OpticFactor [-]
        */
        double OpticFactor(double Bback, double kX, double[] dobs, double JDNDaysUT, string ObjectName, int TypeFactor, int helflag) {
            double Pst, CIb, CIi, ObjectSize, Fb, Fe, Fsc, Fci, Fcb, Ft, Fp, Fa, Fr, Fm;
            double Age = dobs[0];
            double SN = dobs[1], SNi;
            double Binocular = dobs[2];
            double OpticMag = dobs[3];
            double OpticDia = dobs[4];
            double OpticTrans = dobs[5];
            SNi = SN;
            if (SNi <= 0.00000001) SNi = 0.00000001;
            /* 23 jaar as standard from Garstang*/
            Pst = PupilDia(23, Bback);
            if (OpticMag == 1) { /*OpticMagn=1 means using eye*/
                OpticTrans = 1;
                OpticDia = Pst;
            }
            //#if 0 /*is done in default_heliacal_parameters()*/
            //  if (OpticMag == 0) { /*OpticMagn=0 (undefined) using eye*/
            //    OpticTrans = 1;
            //    OpticDia = Pst;
            //    Binocular = 1;
            //    OpticMag = 1;
            //  }
            //#endif
            /* Schaefer, Astronomy and the limits of vision, Archaeoastronomy, 1993*/
            CIb = 0.7; /* color of background (from Ben Sugerman)*/
            CIi = 0.5; /* Color index for white (from Ben Sugerman), should be function of ObjectName*/
            ObjectSize = 0;
            if (String.Compare(ObjectName, "moon") == 0) {
                /*ObjectSize and CI needs to be determined (depending on JDNDaysUT)*/
                ;
            }
            Fb = 1;
            if (Binocular == 0) Fb = 1.41;
            if (Bback < BNIGHT && 0 == (helflag & SwissEph.SE_HELFLAG_VISLIM_PHOTOPIC)) {
                Fe = Math.Pow(10, (0.48 * kX));
                Fsc = mymin(1, (1 - Math.Pow(Pst / 124.4, 4)) / (1 - Math.Pow((OpticDia / OpticMag / 124.4), 4)));
                Fci = Math.Pow(10, (-0.4 * (1 - CIi / 2.0)));
                Fcb = Math.Pow(10, (-0.4 * (1 - CIb / 2.0)));
            } else {
                Fe = Math.Pow(10, (0.4 * kX));
                Fsc = mymin(1, Math.Pow((OpticDia / OpticMag / Pst), 2) * (1 - Math.Exp(-Math.Pow((Pst / 6.2), 2))) / (1 - Math.Exp(-Math.Pow((OpticDia / OpticMag / 6.2), 2))));
                Fci = 1;
                Fcb = 1;
            }
            Ft = 1 / OpticTrans;
            Fp = mymax(1, Math.Pow((Pst / (OpticMag * PupilDia(Age, Bback))), 2));
            Fa = Math.Pow((Pst / OpticDia), 2);
            Fr = (1 + 0.03 * Math.Pow((OpticMag * ObjectSize / CVA(Bback, SNi)), 2)) / Math.Pow(SNi, 2);
            Fm = Math.Pow(OpticMag, 2);
#if DEBUG
            trace("Pst=%f\n", Pst);
            trace("Fb =%f\n", Fb);
            trace("Fe =%f\n", Fe);
            trace("Ft =%f\n", Ft);
            trace("Fp =%f\n", Fp);
            trace("Fa =%f\n", Fa);
            trace("Fm =%f\n", Fm);
            trace("Fsc=%f\n", Fsc);
            trace("Fci=%f\n", Fci);
            trace("Fcb=%f\n", Fcb);
            trace("Fr =%f\n", Fr);
#endif
            if (TypeFactor == 0)
                return Fb * Fe * Ft * Fp * Fa * Fr * Fsc * Fci;
            else
                return Fb * Ft * Fp * Fa * Fm * Fsc * Fcb;
        }

        /*###################################################################
        */
        Int32 DeterObject(string ObjectName) {
            //char s[AS_MAXCH];
            //char *sp;
            Int32 ipl;
            var s = ObjectName.ToLower();
            if (s.StartsWith("sun"))
                return SwissEph.SE_SUN;
            if (s.StartsWith("venus"))
                return SwissEph.SE_VENUS;
            if (s.StartsWith("mars"))
                return SwissEph.SE_MARS;
            if (s.StartsWith("mercur"))
                return SwissEph.SE_MERCURY;
            if (s.StartsWith("jupiter"))
                return SwissEph.SE_JUPITER;
            if (s.StartsWith("saturn"))
                return SwissEph.SE_SATURN;
            if (s.StartsWith("uranus"))
                return SwissEph.SE_URANUS;
            if (s.StartsWith("neptun"))
                return SwissEph.SE_NEPTUNE;
            if (s.StartsWith("moon"))
                return SwissEph.SE_MOON;
            if ((ipl = int.Parse(s)) > 0) {
                ipl += SwissEph.SE_AST_OFFSET;
                return ipl;
            }
            return -1;
        }

        //#if 0
        //int32 call_swe_calc(double tjd, int32 ipl, int32 iflag, double *x, char *serr) 
        //{
        //  int32 retval = OK, ipli, i;
        //  double dtjd;
        //  static TLS double tjdsv[3];
        //  static TLS double xsv[3,6];
        //  static TLS int32 iflagsv[3];
        //  ipli = ipl;
        //  if (ipli > SE_MOON) 
        //    ipli = 2;
        //  dtjd = tjd - tjdsv[ipli];
        //  if (tjdsv[ipli] != 0 && iflag == iflagsv[ipli] && Math.Abs(dtjd) < 5.0 / 1440.0) {
        //    for (i = 0; i < 3; i++) 
        //      x[i] = xsv[ipli,i] + dtjd * xsv[ipli,i+3];
        //    for (i = 3; i < 6; i++) 
        //      x[i] = xsv[ipli,i];
        //  } else {
        //    retval = swe_calc(tjd, ipl, iflag, x, serr);
        //    tjdsv[ipli] = tjd;
        //    iflagsv[ipli] = iflag;
        //    for (i = 0; i < 6; i++) 
        //      xsv[ipli,i] = x[i];
        //  }
        //  return retval;
        //}
        //#endif

        /* avoids problems with star name string that may be overwritten by 
           swe_fixstar() */
        Int32 call_swe_fixstar(string star, double tjd, Int32 iflag, double[] xx, ref string serr) {
            Int32 retval;
            string star2 = star;
            retval = SE.swe_fixstar(star2, tjd, iflag, xx, ref serr);
            return retval;
        }

        /* avoids problems with star name string that may be overwritten by 
           swe_fixstar_mag() */
        double call_swe_fixstar_mag_dmag;
        string call_swe_fixstar_mag_star_save;
        Int32 call_swe_fixstar_mag(string star, ref double mag, ref string serr) {
            Int32 retval;
            string star2;
            if (String.Compare(star, call_swe_fixstar_mag_star_save) == 0) {
                mag = call_swe_fixstar_mag_dmag;
                return SwissEph.OK;
            }
            call_swe_fixstar_mag_star_save = star;
            star2 = star;
            retval = SE.swe_fixstar_mag(ref star2, ref call_swe_fixstar_mag_dmag, ref serr);
            mag = call_swe_fixstar_mag_dmag;
            return retval;
        }

        /* avoids problems with star name string that may be overwritten by 
           swe_fixstar() */
        Int32 call_swe_rise_trans(double tjd, Int32 ipl, string star, Int32 helflag, Int32 eventtype, double[] dgeo, double atpress, double attemp, ref double tret, ref string serr) {
            Int32 retval;
            Int32 iflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            string star2 = star;
            retval = SE.swe_rise_trans(tjd, ipl, star2, iflag, eventtype, dgeo, atpress, attemp, ref tret, ref serr);
            return retval;
        }

        /* 
         * Written by Dieter Koch:
         * Fast function for risings and settings of planets, can be used instead of 
         * swe_rise_trans(), which is much slower.
         * For circumpolar and near-circumpolar planets use swe_rise_trans(), or 
         * generally use it for geographical latitudes higher than 58N/S.
         * For fixed stars, swe_rise_trans() is fast enough.
         */
        Int32 calc_rise_and_set(double tjd_start, Int32 ipl, double[] dgeo, double[] datm, Int32 eventflag, Int32 helflag, ref double trise, ref string serr) {
            int retc = SwissEph.OK, i;
            double sda, dfac = 1 / 365.25;
            double[] xs = new double[6], xx = new double[6], xaz = new double[6], xaz2 = new double[6];
            double rdi, rh;
            double tjd0 = tjd_start, tjdrise;
            double tjdnoon = (int)tjd0 - dgeo[0] / 15.0 / 24.0;
            Int32 iflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            Int32 epheflag = iflag;
            iflag |= SwissEph.SEFLG_EQUATORIAL;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            if (SE.swe_calc_ut(tjd0, SwissEph.SE_SUN, iflag, xs, ref serr) == 0) {
                serr = "error in calc_rise_and_set(): calc(sun) failed ";
                return SwissEph.ERR;
            }
            if (SE.swe_calc_ut(tjd0, ipl, iflag, xx, ref serr) == 0) {
                serr = "error in calc_rise_and_set(): calc(sun) failed ";
                return SwissEph.ERR;
            }
            tjdnoon -= SE.swe_degnorm(xs[0] - xx[0]) / 360.0 + 0;
            /* is planet above horizon or below? */
            SE.swe_azalt(tjd0, SwissEph.SE_EQU2HOR, dgeo, datm[0], datm[1], xx, xaz);
            if ((eventflag & SwissEph.SE_CALC_RISE) != 0) {
                if (xaz[2] > 0) {
                    while (tjdnoon - tjd0 < 0.5) { /*printf("e");*/tjdnoon += 1; }
                    while (tjdnoon - tjd0 > 1.5) { /*printf("f");*/tjdnoon -= 1; }
                } else {
                    while (tjdnoon - tjd0 < 0.0) { /*printf("g");*/tjdnoon += 1; }
                    while (tjdnoon - tjd0 > 1.0) { /*printf("h");*/tjdnoon -= 1; }
                }
            } else {
                if (xaz[2] > 0) {
                    while (tjd0 - tjdnoon > 0.5) { /*printf("a");*/ tjdnoon += 1; }
                    while (tjd0 - tjdnoon < -0.5) { /*printf("b");*/ tjdnoon -= 1; }
                } else {
                    while (tjd0 - tjdnoon > 0.0) { /*printf("c");*/ tjdnoon += 1; }
                    while (tjd0 - tjdnoon < -1.0) { /*printf("d");*/ tjdnoon -= 1; }
                }
            }
            /* position of planet */
            if (SE.swe_calc_ut(tjdnoon, ipl, iflag, xx, ref serr) == SwissEph.ERR) {
                serr = "error in calc_rise_and_set(): calc(sun) failed ";
                return SwissEph.ERR;
            }
            /* apparent radius of solar disk (ignoring refraction) */
            rdi = Math.Asin(696000000.0 / 1.49597870691e+11 / xx[2]) / SwissEph.DEGTORAD;
            if ((eventflag & SwissEph.SE_BIT_DISC_CENTER) != 0)
                rdi = 0;
            /* true altitude of sun, when it appears at the horizon */
            /* refraction for a body visible at the horizon at 0m above sea,
             * atmospheric temperature 10 deg C, atmospheric pressure 1013.25 is 34.5 arcmin*/
            rh = -(34.5 / 60.0 + rdi);
            /* semidiurnal arc of sun */
            sda = Math.Acos(-Math.Tan(dgeo[1] * SwissEph.DEGTORAD) * Math.Tan(xx[1] * SwissEph.DEGTORAD)) * SwissEph.RADTODEG;
            /* rough rising and setting times */
            if ((eventflag & SwissEph.SE_CALC_RISE) != 0)
                tjdrise = tjdnoon - sda / 360.0;
            else
                tjdrise = tjdnoon + sda / 360.0;
            /*ph->tset = tjd_start + sda / 360.0;*/
            /* now calculate more accurate rising and setting times.
             * use vertical speed in order to determine crossing of the horizon  
             * refraction of 34' and solar disk diameter of 16' = 50' = 0.84 deg */
            iflag = epheflag | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_EQUATORIAL;
            if (ipl == SwissEph.SE_MOON)
                iflag |= SwissEph.SEFLG_TOPOCTR;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            for (i = 0; i < 2; i++) {
                if (SE.swe_calc_ut(tjdrise, ipl, iflag, xx, ref serr) == SwissEph.ERR)
                {
                    /*fprintf(stderr, "hev4 tjd=%f, ipl=%d, iflag=%d\n", tjdrise, ipl, iflag);*/
                    return SwissEph.ERR;
                }
                SE.swe_azalt(tjdrise, SwissEph.SE_EQU2HOR, dgeo, datm[0], datm[1], xx, xaz);
                xx[0] -= xx[3] * dfac;
                xx[1] -= xx[4] * dfac;
                SE.swe_azalt(tjdrise - dfac, SwissEph.SE_EQU2HOR, dgeo, datm[0], datm[1], xx, xaz2);
                tjdrise -= (xaz[1] - rh) / (xaz[1] - xaz2[1]) * dfac;
                /*fprintf(stderr, "%f\n", ph->trise);*/
            }
            trise = tjdrise;
            return retc;
        }

        Int32 my_rise_trans(double tjd, Int32 ipl, string starname, Int32 eventtype, Int32 helflag, double[] dgeo, double[] datm, ref double tret, ref string serr) {
            int retc = SwissEph.OK;
            if (!String.IsNullOrEmpty(starname))
                ipl = DeterObject(starname);
            /* for non-circumpolar planets we can use a faster algorithm */
            /*if (!(helflag & SE_HELFLAG_HIGH_PRECISION) && ipl != -1 && Math.Abs(dgeo[1]) < 58) {*/
            if (ipl != -1 && Math.Abs(dgeo[1]) < 63) {
                retc = calc_rise_and_set(tjd, ipl, dgeo, datm, eventtype, helflag, ref tret, ref serr);
                /* for stars and circumpolar planets we use a rigorous algorithm */
            } else {
                retc = call_swe_rise_trans(tjd, ipl, starname, helflag, eventtype, dgeo, datm[0], datm[1], ref tret, ref serr);
            }
            /*  printf("%f, %f\n", tjd, *tret);*/
            return retc;
        }

        /*###################################################################
        ' JDNDaysUT [Days]
        ' dgeo [array: longitude, latitude, eye height above sea m]
        ' TempE [C]
        ' PresE [mbar]
        ' ObjectName (string)
        ' RSEvent (1=rise, 2=set,3=up transit,4=down transit)
        ' Rim [0=center,1=top]
        ' RiseSet [Day]
        */
        Int32 RiseSet(double JDNDaysUT, double[] dgeo, double[] datm, string ObjectName, Int32 RSEvent, Int32 helflag, Int32 Rim, ref double tret, ref string serr) {
            Int32 eventtype = RSEvent, Planet, retval;
            if (Rim == 0)
                eventtype |= SwissEph.SE_BIT_DISC_CENTER;
            Planet = DeterObject(ObjectName);
            if (Planet != -1)
                retval = my_rise_trans(JDNDaysUT, Planet, "", eventtype, helflag, dgeo, datm, ref tret, ref serr);
            else
                retval = my_rise_trans(JDNDaysUT, -1, ObjectName, eventtype, helflag, dgeo, datm, ref tret, ref serr);
            return retval;
        }

        /*###################################################################
        ' JDNDaysUT [Days]
        ' actual [0= approximation, 1=actual]
        ' SunRA [deg]
        */
        double SunRA_tjdlast;
        double SunRA_ralast;
        double SunRA(double JDNDaysUT, Int32 helflag, ref string serr) {
            int imon = 0, iday = 0, iyar = 0, calflag = SwissEph.SE_GREG_CAL;
            double dut = 0;
            if (JDNDaysUT == SunRA_tjdlast)
                return SunRA_ralast;
            if (SwissEph.SIMULATE_VICTORVB) {
                if (true) { /*helflag & SE_HELFLAG_HIGH_PRECISION) {*/
                    double tjd_tt;
                    double[] x = new double[6];
                    Int32 epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
                    Int32 iflag = epheflag | SwissEph.SEFLG_EQUATORIAL;
                    iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
                    tjd_tt = JDNDaysUT + SE.swe_deltat_ex(JDNDaysUT, epheflag, ref serr);
                    if (SE.swe_calc(tjd_tt, SwissEph.SE_SUN, iflag, x, ref serr) != SwissEph.ERR)
                    {
                        SunRA_ralast = x[0];
                        SunRA_tjdlast = JDNDaysUT;
                        return SunRA_ralast;
                    }
                }
            }
            SE.swe_revjul(JDNDaysUT, calflag, ref iyar, ref imon, ref iday, ref dut); /* this seems to be much faster than calling swe_revjul() ! Note: only because SunRA is called 1000s of times */
            SunRA_tjdlast = JDNDaysUT;
            SunRA_ralast = SE.swe_degnorm((imon + (iday - 1) / 30.4 - 3.69) * 30);
            /*ralast = (DatefromJDut(JDNDaysUT, 2) + (DatefromJDut(JDNDaysUT, 3) - 1) / 30.4 - 3.69) * 30;*/
            return SunRA_ralast;
        }

        /*###################################################################
        ' Temp [C]
        ' Kelvin [K]
        */
        double Kelvin(double Temp) {
            /*' http://en.wikipedia.org/wiki/Kelvin*/
            return Temp + C2K;
        }

        /*###################################################################
        ' AppAlt [deg]
        ' TempE [C]
        ' PresE [mbar]
        ' TopoAltitudefromAppAlt [deg]
        */
        double TopoAltfromAppAlt(double AppAlt, double TempE, double PresE) {
            double R = 0;
            double retalt = 0;
            if (AppAlt >= LowestAppAlt) {
                if (AppAlt > 17.904104638432)
                    R = 0.97 / Math.Tan(AppAlt * SwissEph.DEGTORAD);
                else
                    R = (34.46 + 4.23 * AppAlt + 0.004 * AppAlt * AppAlt) / (1 + 0.505 * AppAlt + 0.0845 * AppAlt * AppAlt);
                R = (PresE - 80) / 930 / (1 + 0.00008 * (R + 39) * (TempE - 10)) * R;
                retalt = AppAlt - R * Min2Deg;
            } else {
                retalt = AppAlt;
            }
            return retalt;
        }

        /*###################################################################
        ' TopoAlt [deg]
        ' TempE [C]
        ' PresE [mbar]
        ' AppAltfromTopoAlt [deg]
        ' call this instead of swe_azalt(), because it is faster (lower precision
        ' is required)
        */
        double AppAltfromTopoAlt(double TopoAlt, double TempE, double PresE, Int32 helflag) {
            /* using methodology of Newtown derivatives (analogue to what Swiss Emphemeris uses)*/
            int i, nloop = 2;
            double newAppAlt = TopoAlt;
            double newTopoAlt = 0.0;
            double oudAppAlt = newAppAlt;
            double oudTopoAlt = newTopoAlt;
            double verschil, retalt;
            if ((helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION) != 0)
                nloop = 5;
            for (i = 0; i <= nloop; i++) {
                newTopoAlt = newAppAlt - TopoAltfromAppAlt(newAppAlt, TempE, PresE);
                /*newTopoAlt = newAppAlt - swe_refrac(newAppAlt, PresE, TempE, SE_CALC_APP_TO_TRUE);*/
                verschil = newAppAlt - oudAppAlt;
                oudAppAlt = newTopoAlt - oudTopoAlt - verschil;
                if ((verschil != 0) && (oudAppAlt != 0))
                    verschil = newAppAlt - verschil * (TopoAlt + newTopoAlt - newAppAlt) / oudAppAlt;
                else
                    verschil = TopoAlt + newTopoAlt;
                oudAppAlt = newAppAlt;
                oudTopoAlt = newTopoAlt;
                newAppAlt = verschil;
            }
            retalt = TopoAlt + newTopoAlt;
            if (retalt < LowestAppAlt)
                retalt = TopoAlt;
            return retalt;
        }

        /*###################################################################
        ' TopoAlt [deg]
        ' TopoDecl [deg]
        ' Lat [deg]
        ' HourAngle [hour]
        */
        double HourAngle(double TopoAlt, double TopoDecl, double Lat) {
            double Alti = TopoAlt * SwissEph.DEGTORAD;
            double decli = TopoDecl * SwissEph.DEGTORAD;
            double Lati = Lat * SwissEph.DEGTORAD;
            double ha = (Math.Sin(Alti) - Math.Sin(Lati) * Math.Sin(decli)) / Math.Cos(Lati) / Math.Cos(decli);
            if (ha < -1) ha = -1;
            if (ha > 1) ha = 1;
            /* from http://star-www.st-and.ac.uk/~fv/webnotes/chapt12.htm*/
            return Math.Acos(ha) / SwissEph.DEGTORAD / 15.0;
        }

        /*###################################################################
        ' JDNDaysUT [Days]
        ' dgeo [array: longitude, latitude, eye height above sea m]
        ' TempE [C]
        ' PresE [mbar]
        ' ObjectName [-]
        ' Angle (0 = TopoAlt, 1 = Azi, 2=Topo Declination, 3=Topo Rectascension, 4=AppAlt,5=Geo Declination, 6=Geo Rectascension)
        ' ObjectLoc [deg]
         */
        Int32 ObjectLoc(double JDNDaysUT, double[] dgeo, double[] datm, string ObjectName, Int32 Angle, Int32 helflag, ref double dret, ref string serr) {
            double[] x = new double[6], xin = new double[3], xaz = new double[3]; double tjd_tt;
            Int32 Planet;
            Int32 epheflag;
            Int32 iflag = SwissEph.SEFLG_EQUATORIAL;
            epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            iflag |= epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            if (Angle < 5) iflag = iflag | SwissEph.SEFLG_TOPOCTR;
            if (Angle == 7) Angle = 0;
            tjd_tt = JDNDaysUT + SE.swe_deltat_ex(JDNDaysUT, epheflag, ref serr);
            Planet = DeterObject(ObjectName);
            if (Planet != -1) {
                if (SE.swe_calc(tjd_tt, Planet, iflag, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            } else {
                if (call_swe_fixstar(ObjectName, tjd_tt, iflag, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            if (Angle == 2 || Angle == 5) {
                dret = x[1];
            } else {
                if (Angle == 3 || Angle == 6) {
                    dret = x[0];
                } else {
                    xin[0] = x[0];
                    xin[1] = x[1];
                    SE.swe_azalt(JDNDaysUT, SwissEph.SE_EQU2HOR, dgeo, datm[0], datm[1], xin, xaz);
                    if (Angle == 0)
                        dret = xaz[1];
                    if (Angle == 4)
                        dret = AppAltfromTopoAlt(xaz[1], datm[0], datm[1], helflag);
                    if (Angle == 1) {
                        xaz[0] += 180;
                        if (xaz[0] >= 360)
                            xaz[0] -= 360;
                        dret = xaz[0];
                    }
                }
            }
            return SwissEph.OK;
        }

        /*###################################################################
        ' JDNDaysUT [Days]
        ' dgeo [array: longitude, latitude, eye height above sea m]
        ' TempE [C]
        ' PresE [mbar]
        ' ObjectName [-]
        ' Angle (0 = TopoAlt, 1 = Azi, 2=Topo Declination, 3=Topo Rectascension, 4=AppAlt,5=Geo Declination, 6=Geo Rectascension)
        ' ObjectLoc [deg]
         */
        Int32 azalt_cart(double JDNDaysUT, double[] dgeo, double[] datm, string ObjectName, Int32 helflag, double[] dret, ref string serr) {
            double[] x = new double[6], xin = new double[3], xaz = new double[3]; double tjd_tt;
            Int32 Planet;
            Int32 epheflag;
            Int32 iflag = SwissEph.SEFLG_EQUATORIAL;
            epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            iflag |= epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            iflag = iflag | SwissEph.SEFLG_TOPOCTR;
            tjd_tt = JDNDaysUT + SE.swe_deltat_ex(JDNDaysUT, epheflag, ref serr);
            Planet = DeterObject(ObjectName);
            if (Planet != -1) {
                if (SE.swe_calc(tjd_tt, Planet, iflag, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            } else {
                if (call_swe_fixstar(ObjectName, tjd_tt, iflag, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            xin[0] = x[0];
            xin[1] = x[1];
            SE.swe_azalt(JDNDaysUT, SwissEph.SE_EQU2HOR, dgeo, datm[0], datm[1], xin, xaz);
            dret[0] = xaz[0];
            dret[1] = xaz[1]; /* true altitude */
            dret[2] = xaz[2]; /* apparent altitude */
            /* also return cartesian coordinates, for apparent altitude */
            xaz[1] = xaz[2];
            xaz[2] = 1;
            SE.SwephLib.swi_polcart(xaz, xaz);
            dret[3] = xaz[0];
            dret[4] = xaz[1];
            dret[5] = xaz[2];
            return SwissEph.OK;
        }

        /*###################################################################
        ' LatA [rad]
        ' LongA [rad]
        ' LatB [rad]
        ' LongB [rad]
        ' DistanceAngle [rad]
        */
        double DistanceAngle(double LatA, double LongA, double LatB, double LongB) {
            double dlon = LongB - LongA;
            double dlat = LatB - LatA;
            /* Haversine formula
             * http://www.movable-type.co.uk/scripts/GIS-FAQ-5.1.html
             * R.W. Sinnott, Virtues of the Haversine, Sky and Telescope, vol. 68, no. 2, 1984, p. 159
             */
            double sindlat2 = Math.Sin(dlat / 2);
            double sindlon2 = Math.Sin(dlon / 2);
            double corde = sindlat2 * sindlat2 + Math.Cos(LatA) * Math.Cos(LatB) * sindlon2 * sindlon2;
            if (corde > 1) corde = 1;
            return 2 * Math.Asin(Math.Sqrt(corde));
        }

        /*###################################################################
        ' heighteye [m]
        ' TempS [C]
        ' RH [%]
        ' kW [-]
        */
        double kW(double HeightEye, double TempS, double RH) {
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 128*/
            double WT = 0.031;
            WT *= 0.94 * (RH / 100.0) * Math.Exp(TempS / 15) * Math.Exp(-1 * HeightEye / scaleHwater);
            return WT;
        }

        /*###################################################################
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' lat [deg]
        ' kOZ [-]
        */
        double koz_last, kOZ_alts_last, kOZ_sunra_last;
        double kOZ(double AltS, double sunra, double Lat) {
            double CHANGEKO, OZ, LT, kOZret;
            if (AltS == kOZ_alts_last && sunra == kOZ_sunra_last)
                return koz_last;
            kOZ_alts_last = AltS; kOZ_sunra_last = sunra;
            OZ = 0.031;
            LT = Lat * SwissEph.DEGTORAD;
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 128*/
            kOZret = OZ * (3.0 + 0.4 * (LT * Math.Cos(sunra * SwissEph.DEGTORAD) - Math.Cos(3 * LT))) / 3.0;
            /* depending on day/night vision (altitude of sun < start astronomical twilight), KO changes from 100% to 30%
             * see extinction section of Vistas in Astronomy page 343*/
            CHANGEKO = (100 - 11.6 * mymin(6, mymax(-AltS - 12, 0))) / 100;
            koz_last = kOZret * CHANGEKO;
            return koz_last;
        }

        /*###################################################################
        ' AltS [deg]
        ' heighteye [m]
        ' kR [-]
        */
        double kR(double AltS, double HeightEye) {
            /* depending on day/night vision (altitude of sun < start astronomical twilight),
             * lambda eye sensibility changes
             * see extinction section of Vistas in Astronomy page 343*/
            double CHANGEK, LAMBDA;
            double val = -AltS - 12;
            if (val < 0) val = 0;
            if (val > 6) val = 6;
            /*CHANGEK = (1 - 0.166667 * Min(6, Max(-AltS - 12, 0)));*/
            CHANGEK = (1 - 0.166667 * val);
            LAMBDA = 0.55 + (CHANGEK - 1) * 0.04;
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 128 */
            return 0.1066 * Math.Exp(-1 * HeightEye / scaleHrayleigh) * Math.Pow(LAMBDA / 0.55, -4);
        }

        int Sgn(double x) {
            if (x < 0)
                return -1;
            return 1;
        }

        /*###################################################################
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' TempS [C]
        ' RH [%]
        ' VR [km]
        ' ka [-]
        */
        double ka_alts_last, ka_sunra_last, ka_ka_last;
        double ka(double AltS, double sunra, double Lat, double HeightEye, double TempS, double RH, double VR, ref string serr) {
            double CHANGEKA, LAMBDA, BetaVr, Betaa, kaact;
            double SL = Sgn(Lat);
            /* depending on day/night vision (altitude of sun < start astronomical twilight),
             * lambda eye sensibility changes
             * see extinction section of Vistas in Astronomy page 343 */
            if (AltS == ka_alts_last && sunra == ka_sunra_last)
                return ka_ka_last;
            ka_alts_last = AltS; ka_sunra_last = sunra;
            CHANGEKA = (1 - 0.166667 * mymin(6, mymax(-AltS - 12, 0)));
            LAMBDA = 0.55 + (CHANGEKA - 1) * 0.04;
            if (VR != 0) {
                if (VR >= 1) {
                    /* Visbility range from http://www1.cs.columbia.edu/CAVE/publications/pdfs/Narasimhan_CVPR03.pdf
                     * http://www.icao.int/anb/SG/AMOSSG/meetings/amossg3/wp/SN11Rev.pdf where MOR=2.995/ke
                     * factor 1.3 is the relation between "prevailing visibility" and 
                     * meteorological range was derived by Koshmeider in the 1920's */
                    BetaVr = 3.912 / VR;
                    Betaa = BetaVr - (kW(HeightEye, TempS, RH) / scaleHwater + kR(AltS, HeightEye) / scaleHrayleigh) * 1000 * astr2tau;
                    kaact = Betaa * scaleHaerosol / 1000 * tau2astr;
                    if (kaact < 0) {
                        serr = "The provided Meteorological range is too long, when taking into acount other atmospheric parameters"; /* is a warning */
                        /* return 0; * return "#HIGHVR"; */
                    }
                } else {
                    kaact = VR - kW(HeightEye, TempS, RH) - kR(AltS, HeightEye) - kOZ(AltS, sunra, Lat);
                    if (kaact < 0) {
                        serr = "The provided atmosphic coeefficent (ktot) is too low, when taking into acount other atmospheric parameters"; /* is a warning */
                        /* return 0; * "#LOWktot"; */
                    }
                }
            } else {
                /* From Schaefer , Archaeoastronomy, XV, 2000, page 128 */
                if (SwissEph.SIMULATE_VICTORVB) {
                    if (RH <= 0.00000001) RH = 0.00000001;
                    if (RH >= 99.99999999) RH = 99.99999999;
                }
                kaact = 0.1 * Math.Exp(-1 * HeightEye / scaleHaerosol) * Math.Pow(1 - 0.32 / Math.Log(RH / 100.0), 1.33) * (1 + 0.33 * SL * Math.Sin(sunra * SwissEph.DEGTORAD));
                kaact = kaact * Math.Pow(LAMBDA / 0.55, -1.3);
            }
            ka_ka_last = kaact;
            return kaact;
        }

        /*###################################################################
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' TempS [C]
        ' RH [%]
        ' VR [km]
        ' ExtType [0=ka,1=kW,2=kR,3=kOZ,4=ktot]
        ' kt [-]
        */
        double kt(double AltS, double sunra, double Lat, double HeightEye, double TempS, double RH, double VR, Int32 ExtType, ref string serr) {
            double kRact = 0;
            double kWact = 0;
            double kOZact = 0;
            double kaact = 0;
            if (ExtType == 2 || ExtType == 4)
                kRact = kR(AltS, HeightEye);
            if (ExtType == 1 || ExtType == 4)
                kWact = kW(HeightEye, TempS, RH);
            if (ExtType == 3 || ExtType == 4)
                kOZact = kOZ(AltS, sunra, Lat);
            if (ExtType == 0 || ExtType == 4)
                kaact = ka(AltS, sunra, Lat, HeightEye, TempS, RH, VR, ref serr);
            if (kaact < 0)
                kaact = 0;
            return kWact + kRact + kOZact + kaact;
        }

        /*###################################################################
        ' AppAlt0 [deg]
        ' PresS [mbar]
        ' Airmass [??]
        */
        double Airmass(double AppAltO, double Press) {
            double airm, zend;
            zend = (90 - AppAltO) * SwissEph.DEGTORAD;
            if (zend > Math.PI / 2)
                zend = Math.PI / 2;
            airm = 1 / (Math.Cos(zend) + 0.025 * Math.Exp(-11 * Math.Cos(zend)));
            return Press / 1013 * airm;
        }

        /*###################################################################
        ' scaleH '[m]
        ' zend [rad]
        ' PresS [mbar]
        ' Xext [-]
        */
        double Xext(double scaleH, double zend, double Press) {
            return Press / 1013.0 / (Math.Cos(zend) + 0.01 * Math.Sqrt(scaleH / 1000.0) * Math.Exp(-30.0 / Math.Sqrt(scaleH / 1000.0) * Math.Cos(zend)));
        }

        /*###################################################################
        ' scaleH '[m]
        ' zend [rad]
        ' PresS [mbar]
        ' Xlay [-]
        */
        double Xlay(double scaleH, double zend, double Press) {
            /*return Press / 1013.0 /Math.Sqrt(1.0 - pow(sin(zend) / (1.0 + (scaleH / Ra)), 2));*/
            double a = Math.Sin(zend) / (1.0 + (scaleH / Ra));
            return Press / 1013.0 / Math.Sqrt(1.0 - a * a);
        }

        /*###################################################################
        ' Meteorological formula
        '###################################################################
        ' TempS [C]
        ' HeightEye [m]
        ' TempEfromTempS [C]
        */
        double TempEfromTempS(double TempS, double HeightEye, double Lapse) {
            return TempS - Lapse * HeightEye;
        }

        /*###################################################################
        ' TempS [C]
        ' PresS [mbar]
        ' HeightEye [m]
        ' PresEfromPresS [mbar]
        */
        double PresEfromPresS(double TempS, double Press, double HeightEye) {
            return Press * Math.Exp(-9.80665 * 0.0289644 / (Kelvin(TempS) + 3.25 * HeightEye / 1000) / 8.31441 * HeightEye);
        }

        /*###################################################################
        ' AltO [deg]
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' TempS [C]
        ' PresS [mbar]
        ' RH [%]
        ' VR [km]
        ' Deltam [-]
        */
        double Deltam_alts_last, Deltam_alto_last, Deltam_sunra_last, Deltam_deltam_last;
        double Deltam(double AltO, double AltS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double zend, xR, XW, Xa, XOZ;
            double PresE = PresEfromPresS(datm[1], datm[0], HeightEye);
            double TempE = TempEfromTempS(datm[1], HeightEye, LapseSA);
            double AppAltO = AppAltfromTopoAlt(AltO, TempE, PresE, helflag);
            double deltam;
            if (AltS == Deltam_alts_last && AltO == Deltam_alto_last && sunra == Deltam_sunra_last)
                return Deltam_deltam_last;
            Deltam_alts_last = AltS; Deltam_alto_last = AltO; Deltam_sunra_last = sunra;
            if (staticAirmass == 0) {
                zend = (90 - AppAltO) * SwissEph.DEGTORAD;
                if (zend > Math.PI / 2)
                    zend = Math.PI / 2;
                /* From Schaefer , Archaeoastronomy, XV, 2000, page 128*/
                xR = Xext(scaleHrayleigh, zend, datm[0]);
                XW = Xext(scaleHwater, zend, datm[0]);
                Xa = Xext(scaleHaerosol, zend, datm[0]);
                XOZ = Xlay(scaleHozone, zend, datm[0]);
                deltam = kR(AltS, HeightEye) * xR + kt(AltS, sunra, Lat, HeightEye, datm[1], datm[2], datm[3], 0, ref serr) * Xa + kOZ(AltS, sunra, Lat) * XOZ + kW(HeightEye, datm[1], datm[2]) * XW;
            } else {
                deltam = kt(AltS, sunra, Lat, HeightEye, datm[1], datm[2], datm[3], 4, ref serr) * Airmass(AppAltO, datm[0]);
            }
            Deltam_deltam_last = deltam;
            return deltam;
        }

        /*###################################################################
        ' AltO [deg]
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' TempS [C]
        ' PresS [mbar]
        ' RH [%]
        ' VR [km]
        ' Bn [nL]
        */
        double Bn(double AltO, double JDNDayUT, double AltS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double PresE = PresEfromPresS(datm[1], datm[0], HeightEye);
            double TempE = TempEfromTempS(datm[1], HeightEye, LapseSA);
            double AppAltO = AppAltfromTopoAlt(AltO, TempE, PresE, helflag);
            double zend, YearB, MonthB, DayB, Bna, kX, Bnb;
            double B0 = 0.0000000000001, dut = 0;
            int iyar = 0, imon = 0, iday = 0;
            /* Below altitude of 10 degrees, the Bn stays the same (see page 343 Vistas in Astronomy) */
            if (AppAltO < 10)
                AppAltO = 10;
            zend = (90 - AppAltO) * SwissEph.DEGTORAD;
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 128 and adjusted for sunspot period*/
            /*YearB = DatefromJDut(JDNDayUT, 1);
              MonthB = DatefromJDut(JDNDayUT, 2);
              DayB = DatefromJDut(JDNDayUT, 3);*/
            SE.swe_revjul(JDNDayUT, SwissEph.SE_GREG_CAL, ref iyar, ref imon, ref iday, ref dut);
            YearB = iyar; MonthB = imon; DayB = iday;
            Bna = B0 * (1 + 0.3 * Math.Cos(6.283 * (YearB + ((DayB - 1) / 30.4 + MonthB - 1) / 12 - 1990.33) / 11.1));
            kX = Deltam(AltO, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 129 */
            Bnb = Bna * (0.4 + 0.6 / Math.Sqrt(1 - 0.96 * Math.Pow(Math.Sin(zend), 2))) * Math.Pow(10, -0.4 * kX);
            return mymax(Bnb, 0) * erg2nL;
        }

        /*###################################################################
        ' JDNDaysUT [-]
        ' dgeo [array: longitude, latitude, eye height above sea m]
        ' TempE [C]
        ' PresE [mbar]
        ' ObjectName [-]
        ' Magnitude [-]
        */
        Int32 Magnitude(double JDNDaysUT, double[] dgeo, string ObjectName, Int32 helflag, ref double dmag, ref string serr) {
            double[] x = new double[20];
            Int32 Planet, iflag, epheflag;
            epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            dmag = -99.0;
            Planet = DeterObject(ObjectName);
            iflag = SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL | epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            if (Planet != -1) {
                /**dmag = Phenomena(JDNDaysUT, Lat, Longitude, HeightEye, TempE, PresE, ObjectName, 4);*/
                SE.swe_set_topo(dgeo[0], dgeo[1], dgeo[2]);
                if (SE.swe_pheno_ut(JDNDaysUT, Planet, iflag, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dmag = x[4];
            } else {
                if (call_swe_fixstar_mag(ObjectName, ref dmag, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            return SwissEph.OK;
        }

        //#if 0
        //static int32 fast_magnitude(double tjd, double *dgeo, char *ObjectName, int32 helflag, double *dmag, char *serr)
        //{
        //  int32 retval = OK, ipl, ipli;
        //  double dtjd;
        //  static TLS double tjdsv[3];
        //  static TLS double dmagsv[3];
        //  static TLS int32 helflagsv[3];
        //  ipl = DeterObject(ObjectName);
        //  ipli = ipl;
        //  if (ipli > SE_MOON) 
        //    ipli = 2;
        //  dtjd = tjd - tjdsv[ipli];
        //  if (tjdsv[ipli] != 0 && helflag == helflagsv[ipli] && Math.Abs(dtjd) < 5.0 / 1440.0) {
        //    *dmag = dmagsv[ipli];
        //  } else {
        //    retval = Magnitude(tjd, dgeo, ObjectName, helflag, dmag, serr);
        //    tjdsv[ipli] = tjd;
        //    helflagsv[ipli] = helflag;
        //    dmagsv[ipli] = *dmag;
        //  }
        //  return retval;
        //}
        //#endif

        /*###################################################################
        ' dist [km]
        ' phasemoon [-]
        ' MoonsBrightness [-]
        */
        double MoonsBrightness(double dist, double phasemoon) {
            double log10 = 2.302585092994;
            /*Moon's brightness changes with distance: http://hem.passagen.se/pausch/comp/ppcomp.html#15 */
            return -21.62 + 5 * Math.Log(dist / (Ra / 1000)) / log10 + 0.026 * Math.Abs(phasemoon) + 0.000000004 * Math.Pow(phasemoon, 4);
        }

        /*###################################################################
        ' AltM [deg]
        ' AziM [deg]
        ' AziS [deg]
        ' MoonPhase [deg]
        */
        double MoonPhase(double AltM, double AziM, double AziS) {
            double AltMi = AltM * SwissEph.DEGTORAD;
            double AziMi = AziM * SwissEph.DEGTORAD;
            double AziSi = AziS * SwissEph.DEGTORAD;
            return 180 - Math.Acos(Math.Cos(AziSi - AziMi) * Math.Cos(AltMi + 0.95 * SwissEph.DEGTORAD)) / SwissEph.DEGTORAD;
        }

        /*###################################################################
        ' Pressure [mbar]
        */
        double Bm(double AltO, double AziO, double AltM, double AziM, double AltS, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double M0 = -11.05;
            double Bm = 0;
            double RM, kXM, kX, C3, FM, phasemoon, MM;
            if (AltM > -0.26) {
                /* moon only adds light when (partly) above horizon
                 * From Schaefer , Archaeoastronomy, XV, 2000, page 129*/
                RM = DistanceAngle(AltO * SwissEph.DEGTORAD, AziO * SwissEph.DEGTORAD, AltM * SwissEph.DEGTORAD, AziM * SwissEph.DEGTORAD) / SwissEph.DEGTORAD;
                kXM = Deltam(AltM, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
                kX = Deltam(AltO, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
                C3 = Math.Pow(10, -0.4 * kXM);
                FM = (62000000.0) / RM / RM + Math.Pow(10, 6.15 - RM / 40) + Math.Pow(10, 5.36) * (1.06 + Math.Pow(Math.Cos(RM * SwissEph.DEGTORAD), 2));
                Bm = FM * C3 + 440000 * (1 - C3);
                phasemoon = MoonPhase(AltM, AziM, AziS);
                MM = MoonsBrightness(MoonDistance, phasemoon);
                Bm = Bm * Math.Pow(10, -0.4 * (MM - M0 + 43.27));
                Bm = Bm * (1 - Math.Pow(10, -0.4 * kX));
            }
            Bm = mymax(Bm, 0) * erg2nL;
            return Bm;
        }

        /*###################################################################
        ' Pressure [mbar]
        */
        double Btwi(double AltO, double AziO, double AltS, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double M0 = -11.05;
            double MS = -26.74;
            double PresE = PresEfromPresS(datm[1], datm[0], HeightEye);
            double TempE = TempEfromTempS(datm[1], HeightEye, LapseSA);
            double AppAltO = AppAltfromTopoAlt(AltO, TempE, PresE, helflag);
            double ZendO = 90 - AppAltO;
            double RS = DistanceAngle(AltO * SwissEph.DEGTORAD, AziO * SwissEph.DEGTORAD, AltS * SwissEph.DEGTORAD, AziS * SwissEph.DEGTORAD) / SwissEph.DEGTORAD;
            double kX = Deltam(AltO, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            double k = kt(AltS, sunra, Lat, HeightEye, datm[1], datm[2], datm[3], 4, ref serr);
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 129*/
            double Btwi = Math.Pow(10, -0.4 * (MS - M0 + 32.5 - AltS - (ZendO / (360 * k))));
            Btwi = Btwi * (100 / RS) * (1 - Math.Pow(10, -0.4 * kX));
            Btwi = mymax(Btwi, 0) * erg2nL;
            return Btwi;
        }

        /*###################################################################
        ' Pressure [mbar]
        */
        double Bday(double AltO, double AziO, double AltS, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double M0 = -11.05;
            double MS = -26.74;
            double RS = DistanceAngle(AltO * SwissEph.DEGTORAD, AziO * SwissEph.DEGTORAD, AltS * SwissEph.DEGTORAD, AziS * SwissEph.DEGTORAD) / SwissEph.DEGTORAD;
            double kXS = Deltam(AltS, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            double kX = Deltam(AltO, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 129*/
            double C4 = Math.Pow(10, -0.4 * kXS);
            double FS = (62000000.0) / RS / RS + Math.Pow(10, (6.15 - RS / 40)) + Math.Pow(10, 5.36) * (1.06 + Math.Pow(Math.Cos(RS * SwissEph.DEGTORAD), 2));
            double Bday = FS * C4 + 440000.0 * (1 - C4);
            Bday = Bday * Math.Pow(10, (-0.4 * (MS - M0 + 43.27)));
            Bday = Bday * (1 - Math.Pow(10, -0.4 * kX));
            Bday = mymax(Bday, 0) * erg2nL;
            return Bday;
        }

        /*###################################################################
        ' Value [nL]
        ' PresS [mbar]
        ' Bcity [nL]
        */
        double Bcity(double Value, double Press) {
            double Bcity = Value;
            Bcity = mymax(Bcity, 0);
            return Bcity;
        }

        /*###################################################################
        ' Pressure [mbar]
        */
        double Bsky(double AltO, double AziO, double AltM, double AziM, double JDNDaysUT, double AltS, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref string serr) {
            double Bsky = 0;
            if (AltS < -3) {
                Bsky += Btwi(AltO, AziO, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            } else {
                if (AltS > 4) {
                    Bsky += Bday(AltO, AziO, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr);
                } else {
                    Bsky += mymin(Bday(AltO, AziO, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr), Btwi(AltO, AziO, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr));
                }
            }
            /* if max. Bm [1E7] <5% of Bsky don't add Bm*/
            if (Bsky < 200000000.0)
                Bsky += Bm(AltO, AziO, AltM, AziM, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            if (AltS <= 0)
                Bsky += Bcity(0, datm[0]);
            /* if max. Bn [250] <5% of Bsky don't add Bn*/
            if (Bsky < 5000)
                Bsky = Bsky + Bn(AltO, JDNDaysUT, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            /* if max. Bm [1E7] <5% of Bsky don't add Bm*/
            return Bsky;
        }

        /* default handling:
         * 1. datm (atmospheric conditions):
         * datm consists of 
         *     [0]  atmospheric pressure
         *     [1]  temperature
         *     [2]  relative humidity
         *     [3]  extinction coefficient
         * In order to get default values for [0..2], set datm[0] = 0.
         * Default values for [1-2] are only provided if [0] == 0.
         * [3] defaults outside this function, depending on [0-2].
         * 
         * 2. dobs (observer definition):
         *     [0]  age (default 36)
         *     [1]  Snellen ratio or visual acuity of observer (default 1)
         */
        void default_heliacal_parameters(double[] datm, double[] dgeo, double[] dobs, int helflag) {
            int i;
            if (datm[0] <= 0) {
                /* estimate atmospheric pressure, according to the
                 * International Standard Atmosphere (ISA) */
                datm[0] = 1013.25 * Math.Pow(1 - 0.0065 * dgeo[2] / 288, 5.255);
                /* temperature */
                if (datm[1] == 0)
                    datm[1] = 15 - 0.0065 * dgeo[2];
                /* relative humidity, independent of atmospheric pressure and altitude */
                if (datm[2] == 0)
                    datm[2] = 40;
                /* note: datm[3] / VR defaults outside this function */
            } else {
                if (SwissEph.SIMULATE_VICTORVB) {
                    if (datm[2] <= 0.00000001) datm[2] = 0.00000001;
                    if (datm[2] >= 99.99999999) datm[2] = 99.99999999;
                }
            }
            /* age of observer */
            if (dobs[0] == 0)
                dobs[0] = 36;
            /* SN Snellen factor of the visual acuity of the observer */
            if (dobs[1] == 0)
                dobs[1] = 1;
            if (0 == (helflag & SwissEph.SE_HELFLAG_OPTICAL_PARAMS)) {
                for (i = 2; i <= 5; i++)
                    dobs[i] = 0;
            }
            /* OpticMagn undefined -> use eye */
            if (dobs[3] == 0) {
                dobs[2] = 1; /* Binocular = 1 */
                dobs[3] = 1; /* OpticMagn = 1: use eye */
                /* dobs[4] and dobs[5] (OpticDia and OpticTrans) will be defaulted in 
                 * OpticFactor() */
            }
        }

        /*###################################################################
        ' age [Year]
        ' SN [-]
        ' AltO [deg]
        ' AziO [deg]
        ' AltM [deg]
        ' AziM [deg]
        ' MoonDistance [km]
        ' JDNDaysUT [-]
        ' AltS [deg]
        ' AziS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' TempS [C]
        ' PresS [mbar]
        ' RH [%]
        ' VR [km]
        ' VisLimMagn [-]
        */
        double VisLimMagn(double[] dobs, double AltO, double AziO, double AltM, double AziM, double JDNDaysUT, double AltS, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref Int32 scotopic_flag, ref string serr) {
            double C1, C2, Th, kX, Bsk, CorrFactor1, CorrFactor2;
            double log10 = 2.302585092994;
            /*double Age = dobs[0];*/
            /*double SN = dobs[1];*/
            Bsk = Bsky(AltO, AziO, AltM, AziM, JDNDaysUT, AltS, AziS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            /* Schaefer, Astronomy and the limits of vision, Archaeoastronomy, 1993 Verder:*/
            kX = Deltam(AltO, AltS, sunra, Lat, HeightEye, datm, helflag, ref serr);
            /* influence of age*/
            /*Fa = mymax(1, pow(p(23, Bsk) / p(Age, Bsk), 2)); */
            CorrFactor1 = OpticFactor(Bsk, kX, dobs, JDNDaysUT, "", 1, helflag);
            CorrFactor2 = OpticFactor(Bsk, kX, dobs, JDNDaysUT, "", 0, helflag);
            /* From Schaefer , Archaeoastronomy, XV, 2000, page 129*/
            if (Bsk < BNIGHT && 0 == (helflag & SwissEph.SE_HELFLAG_VISLIM_PHOTOPIC)) {
                C1 = 1.5848931924611e-10; /*pow(10, -9.8);*/ /* C1 = 10 ^ (-9.8);*/
                C2 = 0.012589254117942; /*pow(10, -1.9);*/ /* C2 = 10 ^ (-1.9);*/
                scotopic_flag = 1;
            } else {
                C1 = 4.4668359215096e-9; /*pow(10, -8.35);*/ /* C1 = 10 ^ (-8.35);*/
                C2 = 1.2589254117942e-6; /*pow(10, -5.9);*/ /* C2 = 10 ^ (-5.9);*/
                scotopic_flag = 0;
            }
            if (BNIGHT * BNIGHT_FACTOR > Bsk && BNIGHT / BNIGHT_FACTOR < Bsk)
                scotopic_flag |= 2;
            /*Th = C1 * pow(1 +Math.Sqrt(C2 * Bsk), 2) * Fa;*/
            Bsk = Bsk / CorrFactor1;
            Th = C1 * Math.Pow(1 + Math.Sqrt(C2 * Bsk), 2) * CorrFactor2;
#if DEBUG
            trace("Bsk=%f\n", Bsk);
            trace("kX =%f\n", kX);
            trace("Th =%f\n", Th);
            trace("CorrFactor1=%f\n", CorrFactor1);
            trace("CorrFactor2=%f\n", CorrFactor2);
#endif
            /* Visual limiting magnitude of point source*/
            //#if 0
            //  if (SN <= 0.00000001)
            //    SN = 0.00000001;
            //  return -16.57 - 2.5 * (log(Th) / log10) - kX + 5.0 * (log(SN) / log10);*/
            //#endif
            return -16.57 - 2.5 * (Math.Log(Th) / log10);
        }

        /* Limiting magnitude in dark skies 
         * function returns:
         * -1   Error
         * -2   Object is below horizon
         *  0   OK, photopic vision
         *  |1  OK, scotopic vision
         *  |2  OK, near limit photopic/scotopic
        */
        public Int32 swe_vis_limit_mag(double tjdut, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 helflag, double[] dret, ref string serr) {
            Int32 retval = SwissEph.OK, i, scotopic_flag = 0;
            double AltO = 0, AziO = 0, AltM = 0, AziM = 0, AltS = 0, AziS = 0;
            double sunra;
            for (i = 0; i < 7; i++)
                dret[i] = 0;
            if (DeterObject(ObjectName) == SwissEph.SE_SUN)
            {
                serr = "it makes no sense to call swe_vis_limit_mag() for the Sun";
                return SwissEph.ERR;
            }
            SE.SwephLib.swi_set_tid_acc(tjdut, helflag, 0, ref serr);
            sunra = SunRA(tjdut, helflag, ref serr);
            default_heliacal_parameters(datm, dgeo, dobs, helflag);
            SE.swe_set_topo(dgeo[0], dgeo[1], dgeo[2]);
            if (ObjectLoc(tjdut, dgeo, datm, ObjectName, 0, helflag, ref AltO, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (AltO < 0) {
                serr = "object is below local horizon";
                dret[0] = -100;
                return -2;
            }
            if (ObjectLoc(tjdut, dgeo, datm, ObjectName, 1, helflag, ref AziO, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if ((helflag & SwissEph.SE_HELFLAG_VISLIM_DARK) != 0) {
                AltS = -90;
                AziS = 0;
            } else {
                if (ObjectLoc(tjdut, dgeo, datm, "sun", 0, helflag, ref AltS, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (ObjectLoc(tjdut, dgeo, datm, "sun", 1, helflag, ref AziS, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            if (ObjectName.StartsWith("moon") ||
                (helflag & SwissEph.SE_HELFLAG_VISLIM_DARK) != 0 ||
                (helflag & SwissEph.SE_HELFLAG_VISLIM_NOMOON) != 0
               ) {
                AltM = -90; AziM = 0;
            } else {
                if (ObjectLoc(tjdut, dgeo, datm, "moon", 0, helflag, ref AltM, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (ObjectLoc(tjdut, dgeo, datm, "moon", 1, helflag, ref AziM, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
#if DEBUG
            {
                int ii;
                for (ii = 0; i < 6; ii++)
                    trace("dobs[%d] = %f\n", ii, dobs[ii]);
                trace("AltO = %.10f, AziO = %.10f\n", AltO, AziO);
                trace("AltM = %.10f, AziM = %.10f\n", AltM, AziM);
                trace("AltS = %.10f, AziS = %.10f\n", AltS, AziS);
                trace("JD = %.10f\n", tjdut);
                trace("lat = %f, eyeh = %f\n", dgeo[1], dgeo[2]);
                for (ii = 0; i < 4; ii++)
                    trace("datm[%d] = %f\n", i, datm[ii]);
                trace("helflag = %d\n", helflag);
            }
#endif
            dret[0] = VisLimMagn(dobs, AltO, AziO, AltM, AziM, tjdut, AltS, AziS, sunra, dgeo[1], dgeo[2], datm, helflag, ref scotopic_flag, ref serr);
            dret[1] = AltO;
            dret[2] = AziO;
            dret[3] = AltS;
            dret[4] = AziS;
            dret[5] = AltM;
            dret[6] = AziM;
            if (Magnitude(tjdut, dgeo, ObjectName, helflag, ref (dret[7]), ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            retval = scotopic_flag;
            /*dret[8] = (double) is_scotopic;*/
            /*if (*serr != '\0') * in VisLimMagn(), serr is only a warning *
              retval = ERR; */
            return retval;
        }

        /*###################################################################
        ' Magn [-]
        ' age [Year]
        ' SN [-]
        ' AltO [deg]
        ' AziO [deg]
        ' AltM [deg]
        ' AziM [deg]
        ' MoonDistance [km]
        ' JDNDaysUT [-]
        ' AziS [deg]
        ' lat [deg]
        ' heighteye [m]
        ' Temperature [C]
        ' Pressure [mbar]
        ' RH [%]
        ' VR [km]
        ' TopoArcVisionis [deg]
        */
        Int32 TopoArcVisionis(double Magn, double[] dobs, double AltO, double AziO, double AltM, double AziM, double JDNDaysUT, double AziS, double sunra, double Lat, double HeightEye, double[] datm, Int32 helflag, ref double dret, ref string serr) {
            double Xm, Ym, AltSi, AziSi;
            double xR = 0;
            double Xl = 45;
            double Yl, Yr;
            int idummy = 0;
            Yl = Magn - VisLimMagn(dobs, AltO, AziO, AltM, AziM, JDNDaysUT, AltO - Xl, AziS, sunra, Lat, HeightEye, datm, helflag, ref idummy, ref serr);
            /* if (*serr != '\0') return ERR; * serr is only a warning */
            Yr = Magn - VisLimMagn(dobs, AltO, AziO, AltM, AziM, JDNDaysUT, AltO - xR, AziS, sunra, Lat, HeightEye, datm, helflag, ref idummy, ref serr);
            /* if (*serr != '\0') return ERR; * serr is only a warning */
            /* http://en.wikipedia.org/wiki/Bisection_method*/
            if ((Yl * Yr) <= 0) {
                while (Math.Abs(xR - Xl) > swehel_epsilon) {
                    /*Calculate midpoint of domain*/
                    Xm = (xR + Xl) / 2.0;
                    AltSi = AltO - Xm;
                    AziSi = AziS;
                    Ym = Magn - VisLimMagn(dobs, AltO, AziO, AltM, AziM, JDNDaysUT, AltSi, AziSi, sunra, Lat, HeightEye, datm, helflag, ref idummy, ref serr);
                    /* if (*serr != '\0') return ERR; * serr is only a warning */
                    if ((Yl * Ym) > 0) {
                        /* Throw away left half*/
                        Xl = Xm;
                        Yl = Ym;
                    } else {
                        /* Throw away right half */
                        xR = Xm;
                        Yr = Ym;
                    }
                }
                Xm = (xR + Xl) / 2.0;
            } else {
                Xm = 99;
            }
            if (Xm < AltO)
                Xm = AltO;
            dret = Xm;
            return SwissEph.OK;
        }

        public Int32 swe_topo_arcus_visionis(double tjdut, double[] dgeo, double[] datm, double[] dobs, Int32 helflag, double mag, double azi_obj, double alt_obj, double azi_sun, double azi_moon, double alt_moon, ref double dret, ref string serr) {
            double sunra;
            SE.SwephLib.swi_set_tid_acc(tjdut, helflag, 0, ref serr);
            sunra = SunRA(tjdut, helflag, ref serr);
            if (!String.IsNullOrEmpty(serr))
                return SwissEph.ERR;
            return TopoArcVisionis(mag, dobs, alt_obj, azi_obj, alt_moon, azi_moon, tjdut, azi_sun, sunra, dgeo[1], dgeo[2], datm, helflag, ref dret, ref serr);
        }

        /*###################################################################*/
        /*' Magn [-]
        ' age [Year]
        ' SN Snellen factor of the visual aquity of the observer
          see: http://www.i-see.org/eyecharts.html#make-your-own
        ' AziO [deg]
        ' AltM [deg]
        ' AziM [deg]
        ' MoonDistance [km]
        ' JDNDaysUT [-]
        ' AziS [deg]
        ' Lat [deg]
        ' HeightEye [m]
        ' Temperature [C]
        ' Pressure [mbar]
        ' RH [%]   relative humidity
        ' VR [km]  Meteorological Range, 
          see http://www.iol.ie/~geniet/eng/atmoastroextinction.htm
        ' TypeAngle 
        '   [0=Object's altitude, 
        '    1=Arcus Visonis (Object's altitude - Sun's altitude), 
        '    2=Sun's altitude]
        ' HeliacalAngle [deg]
        */
        Int32 HeliacalAngle(double Magn, double[] dobs, double AziO, double AltM, double AziM, double JDNDaysUT, double AziS, double[] dgeo, double[] datm, Int32 helflag, double[] dangret, ref string serr) {
            double x = 0, minx = 0, maxx = 0, xmin = 0, ymin = 0, Xl = 0, xR = 0, Yr = 0, Yl = 0, Xm = 0, Ym = 0, xmd = 0, ymd = 0;
            double Arc = 0, DELTAx = 0;
            double sunra = SunRA(JDNDaysUT, helflag, ref serr);
            double Lat = dgeo[1];
            double HeightEye = dgeo[2];
            if (PLSV == 1) {
                dangret[0] = criticalangle;
                dangret[1] = criticalangle + Magn * 2.492 + 13.447;
                dangret[2] = -(Magn * 2.492 + 13.447); /* Magn * 1.1 + 8.9;*/
                return SwissEph.OK;
            }
            minx = 2;
            maxx = 20;
            xmin = 0;
            ymin = 10000;
            for (x = minx; x <= maxx; x++) {
                if (TopoArcVisionis(Magn, dobs, x, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, Lat, HeightEye, datm, helflag, ref Arc, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (Arc < ymin) {
                    ymin = Arc;
                    xmin = x;
                }
            }
            Xl = xmin - 1;
            xR = xmin + 1;
            if (TopoArcVisionis(Magn, dobs, xR, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, Lat, HeightEye, datm, helflag, ref Yr, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (TopoArcVisionis(Magn, dobs, Xl, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, Lat, HeightEye, datm, helflag, ref Yl, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /* http://en.wikipedia.org/wiki/Bisection_method*/
            while (Math.Abs(xR - Xl) > 0.1) {
                /* Calculate midpoint of domain */
                Xm = (xR + Xl) / 2.0;
                DELTAx = 0.025;
                xmd = Xm + DELTAx;
                if (TopoArcVisionis(Magn, dobs, Xm, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, Lat, HeightEye, datm, helflag, ref Ym, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (TopoArcVisionis(Magn, dobs, xmd, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, Lat, HeightEye, datm, helflag, ref ymd, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (Ym >= ymd) {
                    /* Throw away left half */
                    Xl = Xm;
                    Yl = Ym;
                } else {
                    /*Throw away right half */
                    xR = Xm;
                    Yr = Ym;
                }
            }
            Xm = (xR + Xl) / 2.0;
            Ym = (Yr + Yl) / 2.0;
            dangret[1] = Ym;
            dangret[2] = Xm - Ym;
            dangret[0] = Xm;
            return SwissEph.OK;
        }

        public Int32 swe_heliacal_angle(double tjdut, double[] dgeo, double[] datm, double[] dobs, Int32 helflag, double mag, double azi_obj, double azi_sun, double azi_moon, double alt_moon, double[] dret, ref string serr) {
            if (dgeo[2] < Sweph.SEI_ECL_GEOALT_MIN || dgeo[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr = C.sprintf("location for heliacal events must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return SwissEph.ERR;
            }
            SE.SwephLib.swi_set_tid_acc(tjdut, helflag, 0, ref serr);
            return HeliacalAngle(mag, dobs, azi_obj, alt_moon, azi_moon, tjdut, azi_sun, dgeo, datm, helflag, dret, ref serr);
        }

        /*###################################################################
        ' AltO [deg]
        ' AziO [deg]
        ' AltS [deg]
        ' AziS [deg]
        ' parallax [deg]
        ' WidthMoon [deg]
        */
        double WidthMoon(double AltO, double AziO, double AltS, double AziS, double parallax) {
            /* Yallop 1998, page 3*/
            double GeoAltO = AltO + parallax;
            return 0.27245 * parallax * (1 + Math.Sin(GeoAltO * SwissEph.DEGTORAD) * Math.Sin(parallax * SwissEph.DEGTORAD)) * (1 - Math.Cos((AltS - GeoAltO) * SwissEph.DEGTORAD) * Math.Cos((AziS - AziO) * SwissEph.DEGTORAD));
        }

        /*###################################################################
        ' W [deg]
        ' LengthMoon [deg]
        */
        double LengthMoon(double W, double Diamoon) {
            double Wi, D;
            if (Diamoon == 0) Diamoon = AvgRadiusMoon * 2;
            Wi = W * 60;
            D = Diamoon * 60;
            /* Crescent length according: http://calendar.ut.ac.ir/Fa/Crescent/Data/Sultan2005.pdf*/
            return (D - 0.3 * (D + Wi) / 2.0 / Wi) / 60.0;
        }

        /*###################################################################
        ' W [deg]
        ' GeoARCVact [deg]
        ' q [-]
        */
        double qYallop(double W, double GeoARCVact) {
            double Wi = W * 60;
            return (GeoARCVact - (11.8371 - 6.3226 * Wi + 0.7319 * Wi * Wi - 0.1018 * Wi * Wi * Wi)) / 10;
        }

        /*###################################################################
        'A (0,p)
        'B (1,q)
        'C (0,r)
        'D (1,s)
        */
        double crossing(double A, double B, double C, double D) {
            return (C - A) / ((B - A) - (D - C));
        }

        /*###################################################################*/
        Int32 DeterTAV(double[] dobs, double JDNDaysUT, double[] dgeo, double[] datm, string ObjectName, Int32 helflag, ref double dret, ref string serr) {
            double Magn = 0, AltO = 0, AziS = 0, AziO = 0, AziM = 0, AltM = 0;
            double sunra = SunRA(JDNDaysUT, helflag, ref serr);
            if (Magnitude(JDNDaysUT, dgeo, ObjectName, helflag, ref Magn, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (ObjectLoc(JDNDaysUT, dgeo, datm, ObjectName, 0, helflag, ref AltO, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (ObjectLoc(JDNDaysUT, dgeo, datm, ObjectName, 1, helflag, ref AziO, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (ObjectName.StartsWith("moon")) {
                AltM = -90;
                AziM = 0;
            } else {
                if (ObjectLoc(JDNDaysUT, dgeo, datm, "moon", 0, helflag, ref AltM, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (ObjectLoc(JDNDaysUT, dgeo, datm, "moon", 1, helflag, ref AziM, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            if (ObjectLoc(JDNDaysUT, dgeo, datm, "sun", 1, helflag, ref AziS, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (TopoArcVisionis(Magn, dobs, AltO, AziO, AltM, AziM, JDNDaysUT, AziS, sunra, dgeo[1], dgeo[2], datm, helflag, ref dret, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            return SwissEph.OK;
        }

        /*###################################################################
        ' A y-value at x=1
        ' B y-value at x=0
        ' C y-value at x=-1
        ' x2min minimum for the quadratic function
        */
        double x2min(double A, double B, double C) {
            double term = A + C - 2 * B;
            if (term == 0)
                return 0;
            return -(A - C) / 2.0 / term;
        }


        /*###################################################################
        ' A y-value at x=1
        ' B y-value at x=0
        ' C y-value at x=-1
        ' x
        ' y is y-value of quadratic function
        */
        double funct2(double A, double B, double C, double x) {
            return (A + C - 2 * B) / 2.0 * x * x + (A - C) / 2.0 * x + B;
        }

        void strcpy_VBsafe(out string sout, string sin) {
            //char *sp, *sp2; 
            //int iw = 0;
            //sp = sin; 
            //sp2 = sout;
            //while((isalnum(*sp) || *sp == ' ' || *sp == '-') && iw < 30) {
            //  *sp2 = *sp;
            //  sp++; sp2++; iw++;
            //}
            //*sp2 = '\0';
            int i = 0;
            while ((Char.IsLetterOrDigit(sin[i]) || sin[i] == ' ' || sin[i] == '-') && i < 30)
                i++;
            sout = sin.Substring(0, i);
        }

        /*###################################################################
        ' JDNDaysUT [JDN]
        ' HPheno
        '0=AltO [deg]		topocentric altitude of object (unrefracted)
        '1=AppAltO [deg]        apparent altitude of object (refracted)
        '2=GeoAltO [deg]        geocentric altitude of object
        '3=AziO [deg]           azimuth of object
        '4=AltS [deg]           topocentric altitude of Sun
        '5=AziS [deg]           azimuth of Sun
        '6=TAVact [deg]         actual topocentric arcus visionis
        '7=ARCVact [deg]        actual (geocentric) arcus visionis
        '8=DAZact [deg]         actual difference between object's and sun's azimuth
        '9=ARCLact [deg]        actual longitude difference between object and sun
        '10=kact [-]            extinction coefficient
        '11=minTAV [deg]        smallest topocentric arcus visionis
        '12=TfistVR [JDN]       first time object is visible, according to VR
        '13=TbVR [JDN]          optimum time the object is visible, according to VR
        '14=TlastVR [JDN]       last time object is visible, according to VR
        '15=TbYallop[JDN]       best time the object is visible, according to Yallop
        '16=WMoon [deg]         cresent width of moon
        '17=qYal [-]            q-test value of Yallop 
        '18=qCrit [-]           q-test criterion of Yallop
        '19=ParO [deg]          parallax of object
        '20 Magn [-]            magnitude of object
        '21=RiseO [JDN]         rise/set time of object
        '22=RiseS [JDN]         rise/set time of sun
        '23=Lag [JDN]           rise/set time of object minus rise/set time of sun
        '24=TvisVR [JDN]        visibility duration
        '25=LMoon [deg]         cresent length of moon
        '26=CVAact [deg]
        '27=Illum [%] 'new
        '28=CVAact [deg] 'new
        '29=MSk [-]
        */
        public Int32 swe_heliacal_pheno_ut(double JDNDaysUT, double[] dgeo, double[] datm, double[] dobs, string ObjectNameIn, Int32 TypeEvent, Int32 helflag, double[] darr, ref string serr) {
            double AziS = 0, AltS = 0, AltS2 = 0, AziO = 0, AltO = 0, AltO2 = 0, GeoAltO = 0, AppAltO = 0, DAZact = 0, TAVact = 0, ParO = 0, MagnO = 0;
            double ARCVact, ARCLact, kact, WMoon, LMoon = 0, qYal, qCrit;
            double RiseSetO = 0, RiseSetS = 0, Lag, TbYallop, TfirstVR, TlastVR, TbVR;
            double MinTAV = 0, MinTAVact, Ta, Tc, TimeStep, TimePointer, MinTAVoud = 0, DeltaAltoud = 0, DeltaAlt, TvisVR, crosspoint;
            double OldestMinTAV, extrax, illum;
            double elong; double[] attr = new double[30];
            double TimeCheck = 0, LocalminCheck = 0;
            Int32 retval = SwissEph.OK, RS, Planet;
            bool noriseO = false;
            string ObjectName;
            double sunra;
            Int32 iflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            if (dgeo[2] < Sweph.SEI_ECL_GEOALT_MIN || dgeo[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr = C.sprintf("location for heliacal events must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return SwissEph.ERR;
            }
            SE.SwephLib.swi_set_tid_acc(JDNDaysUT, helflag, 0, ref serr);
            sunra = SunRA(JDNDaysUT, helflag, ref serr);
            /* note, the fixed stars functions rewrite the star name. The input string 
               may be too short, so we have to make sure we have enough space */
            strcpy_VBsafe(out ObjectName, ObjectNameIn);
            default_heliacal_parameters(datm, dgeo, dobs, helflag);
            SE.swe_set_topo(dgeo[0], dgeo[1], dgeo[2]);
            retval = ObjectLoc(JDNDaysUT, dgeo, datm, "sun", 1, helflag, ref AziS, ref serr);
            if (retval == SwissEph.OK)
                retval = ObjectLoc(JDNDaysUT, dgeo, datm, "sun", 0, helflag, ref AltS, ref serr);
            if (retval == SwissEph.OK)
                retval = ObjectLoc(JDNDaysUT, dgeo, datm, ObjectName, 1, helflag, ref AziO, ref serr);
            if (retval == SwissEph.OK)
                retval = ObjectLoc(JDNDaysUT, dgeo, datm, ObjectName, 0, helflag, ref AltO, ref serr);
            if (retval == SwissEph.OK)
                retval = ObjectLoc(JDNDaysUT, dgeo, datm, ObjectName, 7, helflag, ref GeoAltO, ref serr);
            if (retval == SwissEph.ERR)
                return SwissEph.ERR;
            AppAltO = AppAltfromTopoAlt(AltO, datm[1], datm[0], helflag);
            DAZact = AziS - AziO;
            TAVact = AltO - AltS;
            /*this parallax seems to be somewhat smaller then in Yallop and SkyMap! Needs to be studied*/
            ParO = GeoAltO - AltO;
            if (Magnitude(JDNDaysUT, dgeo, ObjectName, helflag, ref MagnO, ref  serr) == SwissEph.ERR)
                return SwissEph.ERR;
            ARCVact = TAVact + ParO;
            ARCLact = Math.Acos(Math.Cos(ARCVact * SwissEph.DEGTORAD) * Math.Cos(DAZact * SwissEph.DEGTORAD)) / SwissEph.DEGTORAD;
            Planet = DeterObject(ObjectName);
            if (Planet == -1) {
                elong = ARCLact;
                illum = 100;
            } else {
                retval = SE.swe_pheno_ut(JDNDaysUT, Planet, iflag | (SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL), attr, ref serr);
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                elong = attr[2];
                illum = attr[1] * 100;
            }
            kact = kt(AltS, sunra, dgeo[1], dgeo[2], datm[1], datm[2], datm[3], 4, ref  serr);
            //if (false) {
            //    darr[26] = kR(AltS, dgeo[2]);
            //    darr[27] = kW(dgeo[2], datm[1], datm[2]);
            //    darr[28] = kOZ(AltS, sunra, dgeo[1]);
            //    darr[29] = ka(AltS, sunra, dgeo[1], dgeo[2], datm[1], datm[2], datm[3], ref  serr);
            //    darr[30] = darr[26] + darr[27] + darr[28] + darr[29];
            //}
            WMoon = 0;
            qYal = 0;
            qCrit = 0;
            LMoon = 0;
            if (Planet == SwissEph.SE_MOON) {
                WMoon = WidthMoon(AltO, AziO, AltS, AziS, ParO);
                LMoon = LengthMoon(WMoon, 0);
                qYal = qYallop(WMoon, ARCVact);
                if (qYal > 0.216) qCrit = 1; /* A */
                if (qYal < 0.216 && qYal > -0.014) qCrit = 2; /* B */
                if (qYal < -0.014 && qYal > -0.16) qCrit = 3; /* C */
                if (qYal < -0.16 && qYal > -0.232) qCrit = 4; /* D */
                if (qYal < -0.232 && qYal > -0.293) qCrit = 5; /* E */
                if (qYal < -0.293) qCrit = 6; /* F */
            }
            /*determine if rise or set event*/
            RS = 2;
            if (TypeEvent == 1 || TypeEvent == 4) RS = 1;
            retval = RiseSet(JDNDaysUT - 4.0 / 24.0, dgeo, datm, "sun", RS, helflag, 0, ref RiseSetS, ref serr);
            if (retval == SwissEph.ERR)
                return SwissEph.ERR;
            retval = RiseSet(JDNDaysUT - 4.0 / 24.0, dgeo, datm, ObjectName, RS, helflag, 0, ref RiseSetO, ref serr);
            if (retval == SwissEph.ERR)
                return SwissEph.ERR;
            TbYallop = SwissEph.TJD_INVALID;
            if (retval == -2) { /* object does not rise or set */
                Lag = 0;
                noriseO = true;
            } else {
                Lag = RiseSetO - RiseSetS;
                if (Planet == SwissEph.SE_MOON)
                    TbYallop = (RiseSetO * 4 + RiseSetS * 5) / 9.0;
            }
            if ((TypeEvent == 3 || TypeEvent == 4) && (Planet == -1 || Planet >= SwissEph.SE_MARS)) {
                TfirstVR = SwissEph.TJD_INVALID;
                TbVR = SwissEph.TJD_INVALID;
                TlastVR = SwissEph.TJD_INVALID;
                TvisVR = 0;
                MinTAV = 0;
                goto output_heliacal_pheno;
            }
            /* If HPheno >= 11 And HPheno <= 14 Or HPheno = 24 Then*/
            /*te bepalen m.b.v. walkthrough*/
            MinTAVact = 199;
            DeltaAlt = 0;
            OldestMinTAV = 0;
            Ta = 0;
            Tc = 0;
            TbVR = 0;
            TimeStep = -TimeStepDefault / 24.0 / 60.0;
            if (RS == 2) TimeStep = -TimeStep;
            TimePointer = RiseSetS - TimeStep;
            do {
                TimePointer = TimePointer + TimeStep;
                OldestMinTAV = MinTAVoud;
                MinTAVoud = MinTAVact;
                DeltaAltoud = DeltaAlt;
                retval = ObjectLoc(TimePointer, dgeo, datm, "sun", 0, helflag, ref AltS2, ref serr);
                if (retval == SwissEph.OK)
                    retval = ObjectLoc(TimePointer, dgeo, datm, ObjectName, 0, helflag, ref AltO2, ref serr);
                if (retval != SwissEph.OK)
                    return SwissEph.ERR;
                DeltaAlt = AltO2 - AltS2;
                if (DeterTAV(dobs, TimePointer, dgeo, datm, ObjectName, helflag, ref MinTAVact, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (MinTAVoud < MinTAVact && TbVR == 0) {
                    /* determine if this is a local minimum with object still above horizon*/
                    TimeCheck = TimePointer + Sgn(TimeStep) * LocalMinStep / 24.0 / 60.0;
                    if (RiseSetO != 0) {
                        if (TimeStep > 0)
                            TimeCheck = mymin(TimeCheck, RiseSetO);
                        else
                            TimeCheck = mymax(TimeCheck, RiseSetO);
                    }
                    if (DeterTAV(dobs, TimeCheck, dgeo, datm, ObjectName, helflag, ref LocalminCheck, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (LocalminCheck > MinTAVact) {
                        extrax = x2min(MinTAVact, MinTAVoud, OldestMinTAV);
                        TbVR = TimePointer - (1 - extrax) * TimeStep;
                        MinTAV = funct2(MinTAVact, MinTAVoud, OldestMinTAV, extrax);
                    }
                }
                if (DeltaAlt > MinTAVact && Tc == 0 && TbVR == 0) {
                    crosspoint = crossing(DeltaAltoud, DeltaAlt, MinTAVoud, MinTAVact);
                    Tc = TimePointer - TimeStep * (1 - crosspoint);
                }
                if (DeltaAlt < MinTAVact && Ta == 0 && Tc != 0) {
                    crosspoint = crossing(DeltaAltoud, DeltaAlt, MinTAVoud, MinTAVact);
                    Ta = TimePointer - TimeStep * (1 - crosspoint);
                }
            } while (Math.Abs(TimePointer - RiseSetS) <= MaxTryHours / 24.0 && Ta == 0 && !((TbVR != 0 && (TypeEvent == 3 || TypeEvent == 4) && !ObjectName.StartsWith("moon") && !ObjectName.StartsWith("venus") && !ObjectName.StartsWith("mercury"))));
            if (RS == 2) {
                TfirstVR = Tc;
                TlastVR = Ta;
            } else {
                TfirstVR = Ta;
                TlastVR = Tc;
            }
            if (TfirstVR == 0 && TlastVR == 0) {
                if (RS == 1)
                    TfirstVR = TbVR - 0.000001;
                else
                    TlastVR = TbVR + 0.000001;
            }
            if (!noriseO) {
                if (RS == 1)
                    TfirstVR = mymax(TfirstVR, RiseSetO);
                else
                    TlastVR = mymin(TlastVR, RiseSetO);
            }
            TvisVR = SwissEph.TJD_INVALID; /*"#NA!" */
            if (TlastVR != 0 && TfirstVR != 0)
                TvisVR = TlastVR - TfirstVR;
            if (TlastVR == 0) TlastVR = SwissEph.TJD_INVALID; /*"#NA!" */
            if (TbVR == 0) TbVR = SwissEph.TJD_INVALID; /*"#NA!" */
            if (TfirstVR == 0) TfirstVR = SwissEph.TJD_INVALID; /*"#NA!" */
        output_heliacal_pheno:
            /*End If*/
            darr[0] = AltO;
            darr[1] = AppAltO;
            darr[2] = GeoAltO;
            darr[3] = AziO;
            darr[4] = AltS;
            darr[5] = AziS;
            darr[6] = TAVact;
            darr[7] = ARCVact;
            darr[8] = DAZact;
            darr[9] = ARCLact;
            darr[10] = kact;
            darr[11] = MinTAV;
            darr[12] = TfirstVR;
            darr[13] = TbVR;
            darr[14] = TlastVR;
            darr[15] = TbYallop;
            darr[16] = WMoon;
            darr[17] = qYal;
            darr[18] = qCrit;
            darr[19] = ParO;
            darr[20] = MagnO;
            darr[21] = RiseSetO;
            darr[22] = RiseSetS;
            darr[23] = Lag;
            darr[24] = TvisVR;
            darr[25] = LMoon;
            darr[26] = elong;
            darr[27] = illum;
            return SwissEph.OK;
        }

        //#if 0
        //int32 FAR PASCAL_CONV HeliacalJDut(double JDNDaysUTStart, double Age, double SN, double Lat, double Longitude, double HeightEye, double Temperature, double Pressure, double RH, double VR, char *ObjectName, int TypeEvent, char *AVkind, double *dret, char *serr)
        //{
        //  double dgeo[3], datm[4], dobs[6];
        //  int32 helflag = SE_HELFLAG_HIGH_PRECISION;
        //  helflag |= SE_HELFLAG_AVKIND_VR;
        //  dgeo[0] = Longitude;
        //  dgeo[1] = Lat;
        //  dgeo[2] = HeightEye;
        //  datm[0] = Pressure;
        //  datm[1] = Temperature;
        //  datm[2] = RH;
        //  datm[3] = VR;
        //  dobs[0] = Age;
        //  dobs[1] = SN;
        //  return swe_heliacal_ut(JDNDaysUTStart, dgeo, datm, dobs, ObjectName, TypeEvent, helflag, dret, serr);
        //}
        //#endif

        double get_synodic_period(int Planet) {
            /* synodic periods from:
             * Kelley/Milone/Aveni, "Exploring ancient Skies", p. 43. */
            switch (Planet) {
                case SwissEph.SE_MOON: return 29.530588853;
                case SwissEph.SE_MERCURY: return 115.8775;
                case SwissEph.SE_VENUS: return 583.9214;
                case SwissEph.SE_MARS: return 779.9361;
                case SwissEph.SE_JUPITER: return 398.8840;
                case SwissEph.SE_SATURN: return 378.0919;
                case SwissEph.SE_URANUS: return 369.6560;
                case SwissEph.SE_NEPTUNE: return 367.4867;
                case SwissEph.SE_PLUTO: return 366.7207;
            }
            return 366; /* for stars and default for far away planets */
        }

        /*###################################################################*/
        Int32 moon_event_arc_vis(double JDNDaysUTStart, double[] dgeo, double[] datm, double[] dobs, Int32 TypeEvent, Int32 helflag, double[] dret, ref string serr) {
            double[] x = new double[20]; double MinTAV, MinTAVoud, OldestMinTAV;
            double phase1, phase2, JDNDaysUT, JDNDaysUTi;
            double tjd_moonevent = 0, tjd_moonevent_start = 0;
            double DeltaAltoud, TimeCheck, LocalminCheck = 0;
            double AltS = 0, AltO = 0, DeltaAlt = 90;
            string ObjectName;
            Int32 iflag, Daystep, goingup, Planet, retval;
            Int32 avkind = helflag & SwissEph.SE_HELFLAG_AVKIND;
            Int32 epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            dret[0] = JDNDaysUTStart; /* will be returned in error case */
            if (avkind == 0)
                avkind = SwissEph.SE_HELFLAG_AVKIND_VR;
            if (avkind != SwissEph.SE_HELFLAG_AVKIND_VR) {
                serr = "error: in valid AV kind for the moon";
                return SwissEph.ERR;
            }
            if (TypeEvent == 1 || TypeEvent == 2) {
                serr = "error: the moon has no morning first or evening last";
                return SwissEph.ERR;
            }
            ObjectName = "moon";
            Planet = SwissEph.SE_MOON;
            iflag = SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL | epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            Daystep = 1;
            if (TypeEvent == 3) {
                /*morning last */
                TypeEvent = 2;
            } else {
                /*evening first*/
                TypeEvent = 1;
                Daystep = -Daystep;
            }
            /* check Synodic/phase Period */
            JDNDaysUT = JDNDaysUTStart;
            /* start 30 days later if TypeEvent=4 (1) */
            if (TypeEvent == 1) JDNDaysUT = JDNDaysUT + 30;
            /* determination of new moon date */
            SE.swe_pheno_ut(JDNDaysUT, Planet, iflag, x, ref serr);
            phase2 = x[0];
            goingup = 0;
            do {
                JDNDaysUT = JDNDaysUT + Daystep;
                phase1 = phase2;
                SE.swe_pheno_ut(JDNDaysUT, Planet, iflag, x, ref serr);
                phase2 = x[0];
                if (phase2 > phase1)
                    goingup = 1;
            } while (goingup == 0 || (goingup == 1 && (phase2 > phase1)));
            /* fix the date to get the day with the smallest phase (nwest moon) */
            JDNDaysUT = JDNDaysUT - Daystep;
            /* initialize the date to look for set */
            JDNDaysUTi = JDNDaysUT;
            JDNDaysUT = JDNDaysUT - Daystep;
            MinTAVoud = 199;
            do {
                JDNDaysUT = JDNDaysUT + Daystep;
                if ((retval = RiseSet(JDNDaysUT, dgeo, datm, ObjectName, TypeEvent, helflag, 0, ref tjd_moonevent, ref serr)) != SwissEph.OK)
                    return retval;
                tjd_moonevent_start = tjd_moonevent;
                MinTAV = 199;
                OldestMinTAV = MinTAV;
                do {
                    OldestMinTAV = MinTAVoud;
                    MinTAVoud = MinTAV;
                    DeltaAltoud = DeltaAlt;
                    tjd_moonevent = tjd_moonevent - 1.0 / 60.0 / 24.0 * Sgn(Daystep);
                    if (ObjectLoc(tjd_moonevent, dgeo, datm, "sun", 0, helflag, ref AltS, ref  serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (ObjectLoc(tjd_moonevent, dgeo, datm, ObjectName, 0, helflag, ref AltO, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    DeltaAlt = AltO - AltS;
                    if (DeterTAV(dobs, tjd_moonevent, dgeo, datm, ObjectName, helflag, ref MinTAV, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    TimeCheck = tjd_moonevent - LocalMinStep / 60.0 / 24.0 * Sgn(Daystep);
                    if (DeterTAV(dobs, TimeCheck, dgeo, datm, ObjectName, helflag, ref LocalminCheck, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    /*printf("%f, %f <= %f\n", tjd_moonevent, MinTAV, MinTAVoud);*/
                    /* while (MinTAV <= MinTAVoud && Math.Abs(tjd_moonevent - tjd_moonevent_start) < 120.0 / 60.0 / 24.0);*/
                } while ((MinTAV <= MinTAVoud || LocalminCheck < MinTAV) && Math.Abs(tjd_moonevent - tjd_moonevent_start) < 120.0 / 60.0 / 24.0);
                /* while (DeltaAlt < MinTAVoud && Math.Abs(JDNDaysUT - JDNDaysUTi) < 15);*/
            } while (DeltaAltoud < MinTAVoud && Math.Abs(JDNDaysUT - JDNDaysUTi) < 15);
            if (Math.Abs(JDNDaysUT - JDNDaysUTi) < 15) {
                tjd_moonevent += (1 - x2min(MinTAV, MinTAVoud, OldestMinTAV)) * Sgn(Daystep) / 60.0 / 24.0;
            } else {
                serr = "no date found for lunar event";
                return SwissEph.ERR;
            }
            dret[0] = tjd_moonevent;
            return SwissEph.OK;
        }

        Int32 heliacal_ut_arc_vis(double JDNDaysUTStart, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 TypeEventIn, Int32 helflag, double[] dret, ref string serr_ret) {
            double[] x = new double[6];
            double[] xin = new double[2];
            double[] xaz = new double[2];
            double[] dang = new double[3];
            double objectmagn = 0, maxlength, DayStep;
            double JDNDaysUT, JDNDaysUTfinal, JDNDaysUTstep, JDNDaysUTstepoud, JDNarcvisUT, tjd_tt, tret = 0, OudeDatum, JDNDaysUTinp = JDNDaysUTStart, JDNDaysUTtijd;
            double ArcusVis, ArcusVisDelta, ArcusVisPto, ArcusVisDeltaoud;
            double Trise, sunsangle, Theliacal, Tdelta, Angle;
            double TimeStep, TimePointer, OldestMinTAV = 0, MinTAVoud = 0, MinTAVact, extrax, TbVR = 0;
            double AziS, AltS, AziO, AltO, DeltaAlt;
            double direct, Pressure, Temperature, d;
            Int32 epheflag, retval = SwissEph.OK;
            Int32 iflag, Planet, eventtype;
            Int32 TypeEvent = TypeEventIn;
            int doneoneday;
            string serr;
            dret[0] = JDNDaysUTStart;
            serr = String.Empty;
            Planet = DeterObject(ObjectName);
            Pressure = datm[0];
            Temperature = datm[1];
            /* determine Magnitude of star*/
            if ((retval = Magnitude(JDNDaysUTStart, dgeo, ObjectName, helflag, ref objectmagn, ref serr)) == SwissEph.ERR)
                goto swe_heliacal_err;
            epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            iflag = SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL | epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            /* start values for search of heliacal rise
             * maxlength = phase period in days, smaller than minimal synodic period */
            /* days per step (for heliacal rise) in power of two */
            switch (Planet) {
                case SwissEph.SE_MERCURY:
                    DayStep = 1; maxlength = 100; break;
                case SwissEph.SE_VENUS:
                    DayStep = 64; maxlength = 384; break;
                case SwissEph.SE_MARS:
                    DayStep = 128; maxlength = 640; break;
                case SwissEph.SE_JUPITER:
                    DayStep = 64; maxlength = 384; break;
                case SwissEph.SE_SATURN:
                    DayStep = 64; maxlength = 256; break;
                default:
                    DayStep = 64; maxlength = 256; break;
            }
            /* heliacal setting */
            eventtype = TypeEvent;
            if (eventtype == 2) DayStep = -DayStep;
            /* acronychal setting */
            if (eventtype == 4) {
                eventtype = 1;
                DayStep = -DayStep;
            }
            /* acronychal rising */
            if (eventtype == 3) eventtype = 2;
            eventtype |= SwissEph.SE_BIT_DISC_CENTER;
            /* normalize the maxlength to the step size */
            {
                /* check each Synodic/phase Period */
                JDNDaysUT = JDNDaysUTStart;
                /* make sure one can find an event on the just after the JDNDaysUTStart */
                JDNDaysUTfinal = JDNDaysUT + maxlength;
                JDNDaysUT = JDNDaysUT - 1;
                if (DayStep < 0) {
                    JDNDaysUTtijd = JDNDaysUT;
                    JDNDaysUT = JDNDaysUTfinal;
                    JDNDaysUTfinal = JDNDaysUTtijd;
                }
                /* prepair the search */
                JDNDaysUTstep = JDNDaysUT - DayStep;
                doneoneday = 0;
                ArcusVisDelta = 199;
                ArcusVisPto = -5.55;
                do { /* this is a do {} while() loop */
                    if (Math.Abs(DayStep) == 1) doneoneday = 1;
                    do { /* this is a do {} while() loop */
                        /* init search for heliacal rise */
                        JDNDaysUTstepoud = JDNDaysUTstep;
                        ArcusVisDeltaoud = ArcusVisDelta;
                        JDNDaysUTstep = JDNDaysUTstep + DayStep;
                        /* determine rise/set time */
                        if ((retval = my_rise_trans(JDNDaysUTstep, SwissEph.SE_SUN, "", eventtype, helflag, dgeo, datm, ref tret, ref serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                        /* determine time compensation to get Sun's altitude at heliacal rise */
                        tjd_tt = tret + SE.swe_deltat_ex(tret, epheflag, ref serr);
                        if ((retval = SE.swe_calc(tjd_tt, SwissEph.SE_SUN, iflag, x, ref serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                        xin[0] = x[0];
                        xin[1] = x[1];
                        SE.swe_azalt(tret, SwissEph.SE_EQU2HOR, dgeo, Pressure, Temperature, xin, xaz);
                        Trise = HourAngle(xaz[1], x[1], dgeo[1]);
                        sunsangle = ArcusVisPto;
                        if ((helflag & SwissEph.SE_HELFLAG_AVKIND_MIN7) != 0) sunsangle = -7;
                        if ((helflag & SwissEph.SE_HELFLAG_AVKIND_MIN9) != 0) sunsangle = -9;
                        Theliacal = HourAngle(sunsangle, x[1], dgeo[1]);
                        Tdelta = Theliacal - Trise;
                        if (TypeEvent == 2 || TypeEvent == 3) Tdelta = -Tdelta;
                        /* determine appr.time when sun is at the wanted Sun's altitude */
                        JDNarcvisUT = tret - Tdelta / 24;
                        tjd_tt = JDNarcvisUT + SE.swe_deltat_ex(JDNarcvisUT, epheflag, ref serr);
                        /* determine Sun's position */
                        if ((retval = SE.swe_calc(tjd_tt, SwissEph.SE_SUN, iflag, x, ref serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                        xin[0] = x[0];
                        xin[1] = x[1];
                        SE.swe_azalt(JDNarcvisUT, SwissEph.SE_EQU2HOR, dgeo, Pressure, Temperature, xin, xaz);
                        AziS = xaz[0] + 180;
                        if (AziS >= 360) AziS = AziS - 360;
                        AltS = xaz[1];
                        /* determine Moon's position */
                        //#if 0
                        //  double AltM, AziM;
                        //    if ((retval = swe_calc(tjd_tt, SE_MOON, iflag, x, serr)) == ERR)
                        //      goto swe_heliacal_err;
                        //    xin[0] = x[0];
                        //    xin[1] = x[1];
                        //    swe_azalt(JDNarcvisUT, SE_EQU2HOR, dgeo, Pressure, Temperature, xin, xaz);
                        //    AziM = xaz[0] + 180;
                        //    if (AziM >= 360) AziM = AziM - 360;
                        //    AltM = xaz[1];
                        //#endif
                        /* determine object's position */
                        if (Planet != -1) {
                            if ((retval = SE.swe_calc(tjd_tt, Planet, iflag, x, ref serr)) == SwissEph.ERR)
                                goto swe_heliacal_err;
                            /* determine magnitude of Planet */
                            if ((retval = Magnitude(JDNarcvisUT, dgeo, ObjectName, helflag, ref objectmagn, ref serr)) == SwissEph.ERR)
                                goto swe_heliacal_err;
                        } else {
                            if ((retval = call_swe_fixstar(ObjectName, tjd_tt, iflag, x, ref serr)) == SwissEph.ERR)
                                goto swe_heliacal_err;
                        }
                        xin[0] = x[0];
                        xin[1] = x[1];
                        SE.swe_azalt(JDNarcvisUT, SwissEph.SE_EQU2HOR, dgeo, Pressure, Temperature, xin, xaz);
                        AziO = xaz[0] + 180;
                        if (AziO >= 360) AziO = AziO - 360;
                        AltO = xaz[1];
                        /* determine arcusvisionis */
                        DeltaAlt = AltO - AltS;
                        /*if ((retval = HeliacalAngle(objectmagn, dobs, AziO, AltM, AziM, JDNarcvisUT, AziS, dgeo, datm, helflag, dang, serr)) == ERR)*/
                        if ((retval = HeliacalAngle(objectmagn, dobs, AziO, -1, 0, JDNarcvisUT, AziS, dgeo, datm, helflag, dang, ref  serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                        ArcusVis = dang[1];
                        ArcusVisPto = dang[2];
                        ArcusVisDelta = DeltaAlt - ArcusVis;
                        /*} while (((ArcusVisDeltaoud > 0 && ArcusVisDelta < 0) || ArcusVisDelta < 0) && (JDNDaysUTfinal - JDNDaysUTstep) * Sgn(DayStep) > 0);*/
                    } while ((ArcusVisDeltaoud > 0 || ArcusVisDelta < 0) && (JDNDaysUTfinal - JDNDaysUTstep) * Sgn(DayStep) > 0);
                    if (doneoneday == 0 && (JDNDaysUTfinal - JDNDaysUTstep) * Sgn(DayStep) > 0) {
                        /* go back to date before heliacal altitude */
                        ArcusVisDelta = ArcusVisDeltaoud;
                        DayStep = ((int)(Math.Abs(DayStep) / 2.0)) * Sgn(DayStep);
                        JDNDaysUTstep = JDNDaysUTstepoud;
                    }
                } while (doneoneday == 0 && (JDNDaysUTfinal - JDNDaysUTstep) * Sgn(DayStep) > 0);
            }
            d = (JDNDaysUTfinal - JDNDaysUTstep) * Sgn(DayStep);
            if (d <= 0 || d >= maxlength) {
                dret[0] = JDNDaysUTinp; /* no date found, just return input */
                retval = -2; /* marks "not found" within synodic period */
                serr = C.sprintf("heliacal event not found within maxlength %f\n", maxlength);
                goto swe_heliacal_err;
            }
            //#if 0
            //  if (helflag & SE_HELFLAG_AVKIND_VR) {
            //    double darr[40];
            //    if (swe_heliacal_pheno_ut(JDNarcvisUT, dgeo, datm, dobs, ObjectName, TypeEvent, helflag, darr, serr) != OK)
            //      return ERR;
            //    JDNarcvisUT = darr[13];
            //    }
            //  }
            //#endif
            direct = TimeStepDefault / 24.0 / 60.0;
            if (DayStep < 0) direct = -direct;
            if ((helflag & SwissEph.SE_HELFLAG_AVKIND_VR) != 0) {
                /*te bepalen m.b.v. walkthrough*/
                TimeStep = direct;
                TbVR = 0;
                TimePointer = JDNarcvisUT;
                if (DeterTAV(dobs, TimePointer, dgeo, datm, ObjectName, helflag, ref OldestMinTAV, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                TimePointer = TimePointer + TimeStep;
                if (DeterTAV(dobs, TimePointer, dgeo, datm, ObjectName, helflag, ref MinTAVoud, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (MinTAVoud > OldestMinTAV) {
                    TimePointer = JDNarcvisUT;
                    TimeStep = -TimeStep;
                    MinTAVact = OldestMinTAV;
                } else {
                    MinTAVact = MinTAVoud;
                    MinTAVoud = OldestMinTAV;
                }
                /*TimePointer = TimePointer - Timestep*/
                do {
                    TimePointer = TimePointer + TimeStep;
                    OldestMinTAV = MinTAVoud;
                    MinTAVoud = MinTAVact;
                    if (DeterTAV(dobs, TimePointer, dgeo, datm, ObjectName, helflag, ref MinTAVact, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (MinTAVoud < MinTAVact) {
                        extrax = x2min(MinTAVact, MinTAVoud, OldestMinTAV);
                        TbVR = TimePointer - (1 - extrax) * TimeStep;
                    }
                } while (TbVR == 0);
                JDNarcvisUT = TbVR;
            }
            /*if (strncmp(AVkind, "pto", 3) == 0) */
            if ((helflag & SwissEph.SE_HELFLAG_AVKIND_PTO) != 0) {
                do {
                    OudeDatum = JDNarcvisUT;
                    JDNarcvisUT = JDNarcvisUT - direct;
                    tjd_tt = JDNarcvisUT + SE.swe_deltat_ex(JDNarcvisUT, epheflag, ref serr);
                    if (Planet != -1)
                    {
                        if ((retval = SE.swe_calc(tjd_tt, Planet, iflag, x, ref serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                    } else {
                        if ((retval = call_swe_fixstar(ObjectName, tjd_tt, iflag, x, ref  serr)) == SwissEph.ERR)
                            goto swe_heliacal_err;
                    }
                    xin[0] = x[0];
                    xin[1] = x[1];
                    SE.swe_azalt(JDNarcvisUT, SwissEph.SE_EQU2HOR, dgeo, Pressure, Temperature, xin, xaz);
                    Angle = xaz[1];
                } while (Angle > 0);
                JDNarcvisUT = (JDNarcvisUT + OudeDatum) / 2.0;
            }
            if (JDNarcvisUT < -9999999 || JDNarcvisUT > 9999999) {
                dret[0] = JDNDaysUT; /* no date found, just return input */
                serr = "no heliacal date found";
                retval = SwissEph.ERR;
                goto swe_heliacal_err;
            }
            dret[0] = JDNarcvisUT;
        swe_heliacal_err:
            if (!String.IsNullOrEmpty(serr))
                serr_ret = serr;
            return retval;
        }

        Int32 get_asc_obl(double tjd, Int32 ipl, string star, Int32 iflag, double[] dgeo, bool desc_obl, ref double daop, ref string serr) {
            Int32 retval;
            Int32 epheflag = iflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            double[] x = new double[6]; double adp;
            string s;
            string star2;
            star2 = star;
            if (ipl == -1) {
                if ((retval = SE.swe_fixstar(star2, tjd, epheflag | SwissEph.SEFLG_EQUATORIAL, x, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
            } else {
                if ((retval = SE.swe_calc(tjd, ipl, epheflag | SwissEph.SEFLG_EQUATORIAL, x, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            adp = Math.Tan(dgeo[1] * SwissEph.DEGTORAD) * Math.Tan(x[1] * SwissEph.DEGTORAD);
            if (Math.Abs(adp) > 1) {
                if (!String.IsNullOrEmpty(star))
                    s = star;
                else
                    s = SE.swe_get_planet_name(ipl);
                serr = C.sprintf("%s is circumpolar, cannot calculate heliacal event", s);
                return -2;
            }
            adp = Math.Asin(adp) / SwissEph.DEGTORAD;
            if (desc_obl)
                daop = x[0] + adp;
            else
                daop = x[0] - adp;
            daop = SE.swe_degnorm(daop);
            return SwissEph.OK;
        }

        //#if 0
        //static int32 get_asc_obl_old(double tjd, int32 ipl, char *star, int32 iflag, double *dgeo, AS_BOOL desc_obl, double *daop, char *serr)
        //{
        //  int32 retval;
        //  int32 epheflag = iflag & (SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH);
        //  double x[6], adp;
        //  char s[AS_MAXCH];
        //  if (star != NULL && *star != '\0') {
        //    if ((retval = call_swe_fixstar(star, tjd, epheflag | SEFLG_EQUATORIAL, x, serr)) == ERR)
        //      return ERR;
        //  } else {
        //    if ((retval = swe_calc(tjd, ipl, epheflag | SEFLG_EQUATORIAL, x, serr)) == ERR)
        //      return ERR;
        //  }
        //  adp = tan(dgeo[1] * DEGTORAD) * tan(x[1] * DEGTORAD); 
        //  if (Math.Abs(adp) > 1) {
        //    if (star != NULL && *star != '\0') 
        //      s = star;
        //    else 
        //      swe_get_planet_name(ipl, s);
        //    serr=C.sprintf("%s is circumpolar, cannot calculate heliacal event", s);
        //    return -2;
        //  }
        //  adp = asin(adp) / DEGTORAD;
        //  if (desc_obl)
        //    *daop = x[0] + adp;
        //  else
        //    *daop = x[0] - adp;
        //  *daop = swe_degnorm(*daop);
        //  return OK;
        //}
        //#endif

        Int32 get_asc_obl_diff(double tjd, Int32 ipl, string star, Int32 iflag, double[] dgeo, bool desc_obl, bool is_acronychal, ref double dsunpl, ref string serr) {
            Int32 retval = SwissEph.OK;
            double aosun = 0, aopl = 0;
            /* ascensio obliqua of sun */
            retval = get_asc_obl(tjd, SwissEph.SE_SUN, "", iflag, dgeo, desc_obl, ref aosun, ref serr);
            if (retval != SwissEph.OK)
                return retval;
            if (is_acronychal) {
                if (desc_obl == true)
                    desc_obl = false;
                else
                    desc_obl = true;
            }
            /* ascensio obliqua of body */
            retval = get_asc_obl(tjd, ipl, star, iflag, dgeo, desc_obl, ref aopl, ref serr);
            if (retval != SwissEph.OK)
                return retval;
            dsunpl = SE.swe_degnorm(aosun - aopl);
            if (is_acronychal)
                dsunpl = SE.swe_degnorm(dsunpl - 180);
            if (dsunpl > 180) dsunpl -= 360;
            return SwissEph.OK;
        }

        //#if 0
        //static int32 get_asc_obl_diff_old(double tjd, int32 ipl, char *star, int32 iflag, double *dgeo, AS_BOOL desc_obl, double *dsunpl, char *serr)
        //{
        //  int32 retval = OK;
        //  double aosun, aopl;
        //  /* ascensio obliqua of sun */
        //  retval = get_asc_obl(tjd, SE_SUN, "", iflag, dgeo, desc_obl, &aosun, serr);
        //  if (retval != OK)
        //    return retval;
        //  /* ascensio obliqua of body */
        //  retval = get_asc_obl(tjd, ipl, star, iflag, dgeo, desc_obl, &aopl, serr);
        //  if (retval != OK)
        //    return retval;
        //  *dsunpl = swe_degnorm(aosun - aopl);
        //  return OK;
        //}
        //#endif

        /* times of 
         * - superior and inferior conjunction (Mercury and Venus)
         * - conjunction and opposition (ipl >= Mars)
         */
        static readonly double[] tcon = new double[] {
              0, 0, 
              2451550, 2451550,  /* Moon */
              2451604, 2451670,  /* Mercury */
              2451980, 2452280,  /* Venus */
              2451727, 2452074,  /* Mars */
              2451673, 2451877,  /* Jupiter */ 
              2451675, 2451868,  /* Saturn */
              2451581, 2451768,  /* Uranus */ 
              2451568, 2451753,  /* Neptune */
            };

        Int32 find_conjunct_sun(double tjd_start, Int32 ipl, Int32 helflag, Int32 TypeEvent, ref double tjd, ref string serr) {
            Int32 epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            int i;
            double tjdcon, tjd0, ds, dsynperiod, daspect = 0; double[] x = new double[6], xs = new double[6];
            if (ipl >= SwissEph.SE_MARS && TypeEvent >= 3)
                daspect = 180;
            i = (TypeEvent - 1) / 2 + ipl * 2;
            tjd0 = tcon[i];
            dsynperiod = get_synodic_period(ipl);
            tjdcon = tjd0 + (Math.Floor((tjd_start - tjd0) / dsynperiod) + 1) * dsynperiod;
            ds = 100;
            while (ds > 0.5) {
                if (SE.swe_calc(tjdcon, ipl, epheflag | SwissEph.SEFLG_SPEED, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(tjdcon, SwissEph.SE_SUN, epheflag | SwissEph.SEFLG_SPEED, xs, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                ds = SE.swe_degnorm(x[0] - xs[0] - daspect);
                if (ds > 180) ds -= 360;
                tjdcon -= ds / (x[3] - xs[3]);
            }
            tjd = tjdcon;
            return SwissEph.OK;
        }

        Int32 get_asc_obl_with_sun(double tjd_start, Int32 ipl, string star, Int32 helflag, Int32 evtyp, double dperiod, double[] dgeo, ref double tjdret, ref string serr) {
            int i, retval;
            bool is_acronychal = false;
            Int32 epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            double dsunpl = 1, dsunpl_save, dsunpl_test = 0, tjd, daystep;
            bool desc_obl = false, retro = false;
            if (evtyp == SwissEph.SE_EVENING_LAST || evtyp == SwissEph.SE_EVENING_FIRST)
                desc_obl = true;
            if (evtyp == SwissEph.SE_MORNING_FIRST || evtyp == SwissEph.SE_EVENING_LAST)
                retro = true;
            if (evtyp == SwissEph.SE_ACRONYCHAL_RISING)
                desc_obl = true;
            if (evtyp == SwissEph.SE_ACRONYCHAL_RISING || evtyp == SwissEph.SE_ACRONYCHAL_SETTING) {
                is_acronychal = true;
                if (ipl != SwissEph.SE_MOON)
                    retro = true;
            }
            //  if (evtyp == 3 || evtyp == 4)
            //    dangsearch = 180;
            /* find date when sun and object have the same ascensio obliqua */
            tjd = tjd_start;
            dsunpl_save = -999999999;
            retval = get_asc_obl_diff(tjd, ipl, star, epheflag, dgeo, desc_obl, is_acronychal, ref dsunpl, ref serr);
            if (retval != SwissEph.OK)  /* retval may be ERR or -2 */
                return retval;
            daystep = 20;
            i = 0;
            while (dsunpl_save == -999999999 ||
                /*Math.Abs(dsunpl - dsunpl_save) > 180 ||*/
                Math.Abs(dsunpl) + Math.Abs(dsunpl_save) > 180 ||
                (retro && !(dsunpl_save < 0 && dsunpl >= 0)) ||
                (!retro && !(dsunpl_save >= 0 && dsunpl < 0))) {
                i++;
                if (i > 5000) {
                    serr = C.sprintf("loop in get_asc_obl_with_sun() (1)");
                    return SwissEph.ERR;
                }
                dsunpl_save = dsunpl;
                tjd += 10.0;
                if (dperiod > 0 && tjd - tjd_start > dperiod)
                    return -2;
                retval = get_asc_obl_diff(tjd, ipl, star, epheflag, dgeo, desc_obl, is_acronychal, ref dsunpl, ref serr);
                if (retval != SwissEph.OK)  /* retval may be ERR or -2 */
                    return retval;
            }
            tjd_start = tjd - daystep;
            daystep /= 2.0;
            tjd = tjd_start + daystep;
            retval = get_asc_obl_diff(tjd, ipl, star, epheflag, dgeo, desc_obl, is_acronychal, ref dsunpl_test, ref serr);
            if (retval != SwissEph.OK)  /* retval may be ERR or -2 */
                return retval;
            i = 0;
            while (Math.Abs(dsunpl) > 0.00001) {
                i++;
                if (i > 5000) {
                    serr = C.sprintf("loop in get_asc_obl_with_sun() (2)");
                    return SwissEph.ERR;
                }
                if (dsunpl_save * dsunpl_test >= 0) {
                    dsunpl_save = dsunpl_test;
                    tjd_start = tjd;
                } else {
                    dsunpl = dsunpl_test;
                }
                daystep /= 2.0;
                tjd = tjd_start + daystep;
                retval = get_asc_obl_diff(tjd, ipl, star, epheflag, dgeo, desc_obl, is_acronychal, ref dsunpl_test, ref serr);
                if (retval != SwissEph.OK)  /* retval may be ERR or -2 */
                    return retval;
            }
            tjdret = tjd;
            return SwissEph.OK;
        }

        //#if 0
        ///* works only for fixed stars */
        //static int32 get_asc_obl_with_sun_old(double tjd_start, int32 ipl, char *star, int32 helflag, int32 TypeEvent, double *dgeo, double *tjdret, char *serr)
        //{
        //  int retval;
        //  int32 epheflag = helflag & (SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH);
        //  double dsunpl = 1, tjd, daystep, dsunpl_save;
        //  double dsynperiod = 367;
        //  double dangsearch = 0;
        //  AS_BOOL desc_obl = FALSE;
        //  if (TypeEvent == 2 || TypeEvent == 3)
        //    desc_obl = TRUE;
        //  if (TypeEvent == 3 || TypeEvent == 4)
        //    dangsearch = 180;
        //  /* find date when sun and object have the same ascensio obliqua */
        //  daystep = dsynperiod;
        //  tjd = tjd_start;
        //  retval = get_asc_obl_diff(tjd, ipl, star, epheflag, dgeo, desc_obl, &dsunpl, serr);
        //  if (retval != OK)  /* retval may be ERR or -2 */
        //    return retval;
        //  while (dsunpl < 359.99999) {
        //    dsunpl_save = dsunpl;
        //    daystep /= 2.0;
        //    retval = get_asc_obl_diff(tjd + daystep, ipl, star, epheflag, dgeo, desc_obl, &dsunpl, serr);
        //    if (retval != OK)  /* retval may be ERR or -2 */
        //      return retval;
        //    if (dsunpl > dsunpl_save)
        //      tjd += daystep;
        //    else
        //      dsunpl = dsunpl_save;
        //  }
        //  *tjdret = tjd;
        //  return OK;
        //}
        //#endif

        //#if 0
        ///* works only for fixed stars */
        //static int32 get_asc_obl_acronychal(double tjd_start, int32 ipl, char *star, int32 helflag, int32 TypeEvent, double *dgeo, double *tjdret, char *serr)
        //{
        //  int retval;
        //  int32 epheflag = helflag & (SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH);
        //  double dsunpl = 1, tjd, daystep, dsunpl_save;
        //  double dsynperiod = 367;
        //  double aosun, aopl;
        //  AS_BOOL sun_desc = TRUE, obj_desc = FALSE;
        //  daystep = dsynperiod;
        //  tjd = tjd_start;
        //  if (TypeEvent == 4) {
        //    sun_desc = FALSE; 
        //    obj_desc = TRUE;
        //  }
        //  /* ascensio (descensio) obliqua of sun */
        //  retval = get_asc_obl(tjd, SE_SUN, "", epheflag, dgeo, sun_desc, &aosun, serr);
        //  if (retval != OK)  /* retval may be ERR or -2 */
        //    return retval;
        //  /* ascensio (descensio) obliqua of body */
        //  retval = get_asc_obl(tjd, ipl, star, epheflag, dgeo, obj_desc, &aopl, serr);
        //  if (retval != OK)  /* retval may be ERR or -2 */
        //    return retval;
        //  dsunpl = swe_degnorm(aosun - aopl + 180);
        //  while (dsunpl < 359.99999) {
        //    dsunpl_save = dsunpl;
        //    daystep /= 2.0;
        //    /* ascensio (descensio) obliqua of sun */
        //    retval = get_asc_obl(tjd+daystep, SE_SUN, "", epheflag, dgeo, sun_desc, &aosun, serr);
        //    if (retval != OK)  /* retval may be ERR or -2 */
        //      return retval;
        //    /* ascensio (descensio) obliqua of body */
        //    retval = get_asc_obl(tjd+daystep, ipl, star, epheflag, dgeo, obj_desc, &aopl, serr);
        //    if (retval != OK)  /* retval may be ERR or -2 */
        //      return retval;
        //    dsunpl = swe_degnorm(aosun - aopl + 180);
        //    if (dsunpl > dsunpl_save)
        //      tjd += daystep;
        //    else
        //      dsunpl = dsunpl_save;
        //  }
        //  *tjdret = tjd;
        //  return OK;
        //}
        //#endif

        Int32 get_heliacal_day(double tjd, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 helflag, Int32 TypeEvent, ref double thel, ref string serr) {
            Int32 is_rise_or_set = 0, ndays, retval, retval_old;
            double direct_day = 0, direct_time = 0, tfac, tend, daystep, tday, vdelta, tret = 0;
            double[] darr = new double[30]; double vd, dmag = 0;
            Int32 ipl = DeterObject(ObjectName);
            /* 
             * find the day and minute on which the object becomes visible 
             */
            switch (TypeEvent) {
                /* morning first */
                case 1: is_rise_or_set = SwissEph.SE_CALC_RISE;
                    direct_day = 1; direct_time = -1;
                    break;
                /* evening last */
                case 2: is_rise_or_set = SwissEph.SE_CALC_SET;
                    direct_day = -1; direct_time = 1;
                    break;
                /* evening first */
                case 3: is_rise_or_set = SwissEph.SE_CALC_SET;
                    direct_day = 1; direct_time = 1;
                    break;
                /* morning last */
                case 4: is_rise_or_set = SwissEph.SE_CALC_RISE;
                    direct_day = -1; direct_time = -1;
                    break;
            }
            tfac = 1;
            switch (ipl) {
                case SwissEph.SE_MOON:
                    ndays = 16;
                    daystep = 1;
                    break;
                case SwissEph.SE_MERCURY:
                    ndays = 60; tjd -= 0 * direct_day;
                    daystep = 5;
                    tfac = 5;
                    break;
                case SwissEph.SE_VENUS:
                    ndays = 300; tjd -= 30 * direct_day;
                    daystep = 5;
                    if (TypeEvent >= 3) {
                        daystep = 15;
                        tfac = 3;
                    }
                    break;
                case SwissEph.SE_MARS:
                    ndays = 400;
                    daystep = 15;
                    tfac = 5;
                    break;
                case SwissEph.SE_SATURN:
                    ndays = 300;
                    daystep = 20;
                    tfac = 5;
                    break;
                case -1:
                    ndays = 300;
                    if (call_swe_fixstar_mag(ObjectName, ref dmag, ref serr) == SwissEph.ERR)
                    {
                        return SwissEph.ERR;
                    }
                    daystep = 15;
                    tfac = 10;
                    if (dmag > 2) {
                        daystep = 15;
                    }
                    if (dmag < 0) {
                        tfac = 3;
                    }
                    break;
                default:
                    ndays = 300;
                    daystep = 15;
                    tfac = 3;
                    break;
            }
            tend = tjd + ndays * direct_day;
            retval_old = -2;
            for (tday = tjd;
                 (direct_day > 0 && tday < tend) || (direct_day < 0 && tday > tend);
                 tday += daystep * direct_day) {
                vdelta = -100;
                if ((retval = my_rise_trans(tday, SwissEph.SE_SUN, "", is_rise_or_set, helflag, dgeo, datm, ref tret, ref serr)) == SwissEph.ERR)
                {
                    return SwissEph.ERR;
                }
                /* sun does not rise: try next day */
                if (retval == -2) {
                    retval_old = retval;
                    continue;
                }
                retval = swe_vis_limit_mag(tret, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
                if (retval == SwissEph.ERR)
                    return SwissEph.ERR;
                //#if 1
                /*  object has appeared above horizon: reduce daystep */
                if (retval_old == -2 && retval >= 0 && daystep > 1) {
                    retval_old = retval;
                    tday -= daystep * direct_day;
                    daystep = 1;
                    /* Note: beyond latitude 55N (?), Mars can have a morning last. 
                     * If the period of visibility is less than 5 days, we may miss the
                     * event. I don't know if this happens */
                    if (ipl >= SwissEph.SE_MARS || ipl == -1)
                        daystep = 5;
                    continue;
                }
                retval_old = retval;
                //#endif
                /*  object below horizon: try next day */
                if (retval == -2)
                    continue;
                vdelta = darr[0] - darr[7];
                /* find minute of object's becoming visible */
                while (retval != -2 && (vd = darr[0] - darr[7]) < 0) {
                    if (vd < -1.0)
                        tret += 5.0 / 1440.0 * direct_time * tfac;
                    else if (vd < -0.5)
                        tret += 2.0 / 1440.0 * direct_time * tfac;
                    else if (vd < -0.1)
                        tret += 1.0 / 1440.0 * direct_time * tfac;
                    else
                        tret += 1.0 / 1440.0 * direct_time;
                    retval = swe_vis_limit_mag(tret, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
                    if (retval == SwissEph.ERR)
                        return SwissEph.ERR;
                }
                vdelta = darr[0] - darr[7];
                /* object is visible, save time of appearance */
                if (vdelta > 0) {
                    if ((ipl >= SwissEph.SE_MARS || ipl == -1) && daystep > 1) {
                        tday -= daystep * direct_day;
                        daystep = 1;
                    } else {
                        thel = tret;
                        return SwissEph.OK;
                    }
                }
            }
            serr = C.sprintf("heliacal event does not happen");
            return -2;
        }

        Int32 time_optimum_visibility(double tjd, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 helflag, ref double tret, ref string serr) {
            Int32 retval, retval_sv, i;
            double d, vl, phot_scot_opic, phot_scot_opic_sv; double[] darr = new double[10];
            tret = tjd;
            retval = swe_vis_limit_mag(tjd, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
            if (retval == SwissEph.ERR) return SwissEph.ERR;
            retval_sv = retval;
            vl = darr[0] - darr[7];
            phot_scot_opic_sv = retval & SwissEph.SE_SCOTOPIC_FLAG;
            for (i = 0, d = 100.0 / 86400.0; i < 3; i++, d /= 10.0) {
                while ((retval = swe_vis_limit_mag(tjd - d, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr)) >= 0
                && darr[0] > darr[7]
                && darr[0] - darr[7] > vl) {
                    tjd -= d; vl = darr[0] - darr[7];
                    retval_sv = retval;
                    phot_scot_opic_sv = retval & SwissEph.SE_SCOTOPIC_FLAG;
                    /*  printf("1: %f\n", darr[8]);*/
                }
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                while ((retval = swe_vis_limit_mag(tjd + d, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr)) >= 0
                    && darr[0] > darr[7]
                && darr[0] - darr[7] > vl) {
                    tjd += d; vl = darr[0] - darr[7];
                    retval_sv = retval;
                    phot_scot_opic_sv = retval & SwissEph.SE_SCOTOPIC_FLAG;
                    /*  printf("2: %f\n", darr[8]);*/
                }
                if (retval == SwissEph.ERR) return SwissEph.ERR;
            }
            /*  printf("3: %f <-> %f\n", darr[8], phot_scot_opic_sv);*/
            tret = tjd;
            if (retval >= 0) {
                /* search for optimum came to an end because change scotopic/photopic: */
                phot_scot_opic = (retval & SwissEph.SE_SCOTOPIC_FLAG);
                if (phot_scot_opic_sv != phot_scot_opic) {
                    /* calling function writes warning into serr */
                    return -2;
                }
                /* valid result found but it is close to the scotopic/photopic limit */
                if ((retval_sv & SwissEph.SE_MIXEDOPIC_FLAG) != 0) {
                    return -2;
                }
            }
            return SwissEph.OK;
        }

        Int32 time_limit_invisible(double tjd, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 helflag, Int32 direct, ref double tret, ref string serr) {
            Int32 retval, retval_sv, i, ncnt = 3;
            double d = 0, phot_scot_opic, phot_scot_opic_sv; double[] darr = new double[10];
            double d0 = 100.0 / 86400.0;
            tret = tjd;
            if (String.Compare(ObjectName, "moon") == 0) {
                d0 *= 10;
                ncnt = 4;
            }
            retval = swe_vis_limit_mag(tjd + d * direct, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
            if (retval == SwissEph.ERR) return SwissEph.ERR;
            retval_sv = retval;
            phot_scot_opic_sv = retval & SwissEph.SE_SCOTOPIC_FLAG;
            for (i = 0, d = d0; i < ncnt; i++, d /= 10.0) {
                while ((retval = swe_vis_limit_mag(tjd + d * direct, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr)) >= 0
                    && darr[0] > darr[7]) {
                    tjd += d * direct;
                    retval_sv = retval;
                    phot_scot_opic_sv = retval & SwissEph.SE_SCOTOPIC_FLAG;
                    /*    printf("%d: %f\n", direct, darr[8]); */
                }
            }
            /*   printf("4: %f, %f/%f %f <-> %f\n", darr[8], darr[0], darr[7], tjd, phot_scot_opic_sv); */
            tret = tjd;
            /* if object disappears at setting, retval is -2, but we want it OK, and
             * also suppress the warning "object is below local horizon" */
            serr = String.Empty;
            if (retval >= 0) {
                /* search for limit came to an end because change scotopic/photopic: */
                phot_scot_opic = (retval & SwissEph.SE_SCOTOPIC_FLAG);
                if (phot_scot_opic_sv != phot_scot_opic) {
                    /* calling function writes warning into serr */
                    return -2;
                }
                /* valid result found but it is close to the scotopic/photopic limit */
                if ((retval_sv & SwissEph.SE_MIXEDOPIC_FLAG) != 0) {
                    return -2;
                }
            }
            return SwissEph.OK;
        }

        Int32 get_acronychal_day(double tjd, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 helflag, Int32 TypeEvent, ref double thel, ref string serr) {
            double tret = 0, tret_dark = 0, dtret; double[] darr = new double[30];
            /* x[6], xaz[6], alto, azio, alto_dark, azio_dark;*/
            Int32 retval, is_rise_or_set, direct;
            Int32 ipl = DeterObject(ObjectName);
            helflag |= SwissEph.SE_HELFLAG_VISLIM_PHOTOPIC;
            /*int32 epheflag = helflag & (SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH);*/
            /* int32 iflag = epheflag | SEFLG_EQUATORIAL | SEFLG_TOPOCTR;*/
            if (TypeEvent == 3 || TypeEvent == 5) {
                is_rise_or_set = SwissEph.SE_CALC_RISE;
                /* tret = tjdc - 3;
                if (ipl >= SE_MARS)
                  tret = tjdc - 3;*/
                direct = -1;
            } else {
                is_rise_or_set = SwissEph.SE_CALC_SET;
                /*tret = tjdc + 3;
                if (ipl >= SE_MARS)
                  tret = tjdc + 3;*/
                direct = 1;
            }
            dtret = 999;
            //#if 0
            //  while (Math.Abs(dtret) > 0.5) {
            //#else
            while (Math.Abs(dtret) > 0.5 / 1440.0) {
                //#endif
                tjd += 0.7 * direct;
                if (direct < 0) tjd -= 1;
                retval = my_rise_trans(tjd, ipl, ObjectName, is_rise_or_set, helflag, dgeo, datm, ref tjd, ref serr);
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                retval = swe_vis_limit_mag(tjd, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                while (darr[0] < darr[7])
                {
                    tjd += 10.0 / 1440.0 * -direct;
                    retval = swe_vis_limit_mag(tjd, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr);
                    if (retval == SwissEph.ERR) return SwissEph.ERR;
                }
                retval = time_limit_invisible(tjd, dgeo, datm, dobs, ObjectName, helflag | SwissEph.SE_HELFLAG_VISLIM_DARK, direct, ref tret_dark, ref serr);
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                retval = time_limit_invisible(tjd, dgeo, datm, dobs, ObjectName, helflag | SwissEph.SE_HELFLAG_VISLIM_NOMOON, direct, ref tret, ref serr);
                if (retval == SwissEph.ERR) return SwissEph.ERR;
                //#if 0
                //    if (azalt_cart(tret_dark, dgeo, datm, ObjectName, helflag, darr, serr) == ERR)
                //      return ERR;
                //    if (azalt_cart(tret, dgeo, datm, ObjectName, helflag, darr+6, serr) == ERR)
                //      return ERR;
                //    dtret = acos(swi_dot_prod_unit(darr+3, darr+9)) / DEGTORAD;
                //#else
                dtret = Math.Abs(tret - tret_dark);
                //#endif
            }
            if (azalt_cart(tret, dgeo, datm, "sun", helflag, darr, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            thel = tret;
            if (darr[1] < -12) {
                serr = C.sprintf("acronychal rising/setting not available, %f", darr[1]);
                return SwissEph.OK;
            } else {
                serr = C.sprintf("solar altitude, %f", darr[1]);
            }
            return SwissEph.OK;
        }

        Int32 get_heliacal_details(double tday, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 TypeEvent, Int32 helflag, double[] dret, ref string serr) {
            Int32 i, retval, direct;
            bool optimum_undefined, limit_1_undefined, limit_2_undefined;
            /* find next optimum visibility */
            optimum_undefined = false;
            retval = time_optimum_visibility(tday, dgeo, datm, dobs, ObjectName, helflag, ref (dret[1]), ref serr);
            if (retval == SwissEph.ERR) return SwissEph.ERR;
            if (retval == -2) {
                retval = SwissEph.OK;
                optimum_undefined = true; /* change photopic <-> scotopic vision */
            }
            /* find moment of becoming visible */
            direct = 1;
            if (TypeEvent == 1 || TypeEvent == 4)
                direct = -1;
            limit_1_undefined = false;
            retval = time_limit_invisible(tday, dgeo, datm, dobs, ObjectName, helflag, direct, ref (dret[0]), ref serr);
            if (retval == SwissEph.ERR) return SwissEph.ERR;
            if (retval == -2) {
                retval = SwissEph.OK;
                limit_1_undefined = true; /* change photopic <-> scotopic vision */
            }
            /* find moment of end of visibility */
            direct *= -1;
            limit_2_undefined = false;
            retval = time_limit_invisible(dret[1], dgeo, datm, dobs, ObjectName, helflag, direct, ref (dret[2]), ref  serr);
            if (retval == SwissEph.ERR) return SwissEph.ERR;
            if (retval == -2) {
                retval = SwissEph.OK;
                limit_2_undefined = true; /* change photopic <-> scotopic vision */
            }
            /* correct sequence of times: 
             * with event types 2 and 3 swap dret[0] and dret[2] */
            if (TypeEvent == 2 || TypeEvent == 3) {
                tday = dret[2];
                dret[2] = dret[0];
                dret[0] = tday;
                i = limit_1_undefined ? 1 : 0;
                limit_1_undefined = limit_2_undefined;
                limit_2_undefined = i != 0;
            }
            /*if (retval == OK && dret[0] == dret[1]) */
            if (optimum_undefined || limit_1_undefined || limit_2_undefined) {
                serr = C.sprintf("return values [");
                if (limit_1_undefined)
                    serr += "0,";
                if (optimum_undefined)
                    serr += "1,";
                if (limit_2_undefined)
                    serr += "2,";
                serr += "] are uncertain due to change between photopic and scotopic vision";
            }
            return SwissEph.OK;
        }

        Int32 heliacal_ut_vis_lim(double tjd_start, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 TypeEventIn, Int32 helflag, double[] dret, ref string serr_ret) {
            int i;
            double /*d, direct = 1,*/ tjd, tday = 0; double[] darr = new double[10];
            Int32 epheflag, retval = SwissEph.OK, helflag2;
            Int32 iflag, ipl;
            Int32 TypeEvent = TypeEventIn;
            string serr;
            for (i = 0; i < 10; i++)
                dret[i] = 0;
            dret[0] = tjd_start;
            serr = String.Empty;
            ipl = DeterObject(ObjectName);
            epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            iflag = SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL | epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            if (ipl == SwissEph.SE_MERCURY)
                tjd = tjd_start - 30;
            else
                tjd = tjd_start - 50; /* -50 makes sure, that no event is missed, 
                                     * but may return an event before start date */
            helflag2 = helflag;
            /*helflag2 &= ~SE_HELFLAG_HIGH_PRECISION;*/
            /* 
             * heliacal event
             */
            if (ipl == SwissEph.SE_MERCURY || ipl == SwissEph.SE_VENUS || TypeEvent <= 2) {
                if (ipl == -1) {
                    /* find date when star rises with sun (cosmic rising) */
                    retval = get_asc_obl_with_sun(tjd, ipl, ObjectName, helflag, TypeEvent, 0, dgeo, ref tjd, ref serr);
                    if (retval != SwissEph.OK)
                        goto swe_heliacal_err; /* retval may be -2 or ERR */
                } else {
                    /* find date of conjunction of object with sun */
                    if ((retval = find_conjunct_sun(tjd, ipl, helflag, TypeEvent, ref tjd, ref serr)) == SwissEph.ERR)
                    {
                        goto swe_heliacal_err;
                    }
                }
                /* find the day and minute on which the object becomes visible */
                retval = get_heliacal_day(tjd, dgeo, datm, dobs, ObjectName, helflag2, TypeEvent, ref tday, ref serr);
                if (retval != SwissEph.OK)
                    goto swe_heliacal_err;
                /* 
                 * acronychal event
                 */
            } else {
                if (/*1 ||*/ ipl == -1) {
                    /*retval = get_asc_obl_acronychal(tjd, ipl, ObjectName, helflag2, TypeEvent, dgeo, &tjd, serr);*/
                    retval = get_asc_obl_with_sun(tjd, ipl, ObjectName, helflag, TypeEvent, 0, dgeo, ref tjd, ref serr);
                    if (retval != SwissEph.OK)
                        goto swe_heliacal_err;
                } else {
                    /* find date of conjunction of object with sun */
                    if ((retval = find_conjunct_sun(tjd, ipl, helflag, TypeEvent, ref tjd, ref serr)) == SwissEph.ERR)
                        goto swe_heliacal_err;
                }
                tday = tjd;
                retval = get_acronychal_day(tjd, dgeo, datm, dobs, ObjectName, helflag2, TypeEvent, ref tday, ref serr);
                if (retval != SwissEph.OK)
                    goto swe_heliacal_err;
            }
            dret[0] = tday;
            if (0 == (helflag & SwissEph.SE_HELFLAG_NO_DETAILS)) {
                /* more precise event times for 
                 * - morning first, evening last
                 * - venus and mercury's evening first and morning last
                 */
                if (ipl == SwissEph.SE_MERCURY || ipl == SwissEph.SE_VENUS || TypeEvent <= 2) {
                    retval = get_heliacal_details(tday, dgeo, datm, dobs, ObjectName, TypeEvent, helflag2, dret, ref serr);
                    if (retval == SwissEph.ERR) goto swe_heliacal_err;
                    //} else if (false) {
                    //    if (TypeEvent == 4 || TypeEvent == 6) direct = -1;
                    //    for (i = 0, d = 100.0 / 86400.0; i < 3; i++, d /= 10.0) {
                    //        while ((retval = swe_vis_limit_mag(dret[0] + d * direct, dgeo, datm, dobs, ObjectName, helflag, darr, ref serr)) == -2 || (retval >= 0 && darr[0] < darr[7])) {
                    //            dret[0] += d * direct;
                    //        }
                    //    }
                    //    /* the last time step must be added */
                    //    if (retval == OK)
                    //        dret[0] += 1.0 / 86400.0 * direct;
                }
            } /* if (1) */
        swe_heliacal_err:
            if (!String.IsNullOrEmpty(serr))
                serr_ret = serr;
            return retval;
        }

        /*###################################################################*/
        Int32 moon_event_vis_lim(double tjdstart, double[] dgeo, double[] datm, double[] dobs, Int32 TypeEvent, Int32 helflag, double[] dret, ref string serr_ret) {
            double tjd, trise = 0;
            string serr = String.Empty;
            string ObjectName;
            Int32 iflag, ipl, retval, helflag2, direct;
            Int32 epheflag = helflag & (SwissEph.SEFLG_JPLEPH | SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_MOSEPH);
            dret[0] = tjdstart; /* will be returned in error case */
            if (TypeEvent == 1 || TypeEvent == 2) {
                serr = "error: the moon has no morning first or evening last";
                return SwissEph.ERR;
            }
            ObjectName = "moon";
            ipl = SwissEph.SE_MOON;
            iflag = SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL | epheflag;
            if (0 == (helflag & SwissEph.SE_HELFLAG_HIGH_PRECISION))
                iflag |= SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS;
            helflag2 = helflag;
            helflag2 &= ~SwissEph.SE_HELFLAG_HIGH_PRECISION;
            /* check Synodic/phase Period */
            tjd = tjdstart - 30; /* -50 makes sure, that no event is missed, 
                         * but may return an event before start date */
            if ((retval = find_conjunct_sun(tjd, ipl, helflag, TypeEvent, ref tjd, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            /* find the day and minute on which the object becomes visible */
            retval = get_heliacal_day(tjd, dgeo, datm, dobs, ObjectName, helflag2, TypeEvent, ref tjd, ref serr);
            if (retval != SwissEph.OK)
                goto moon_event_err;
            dret[0] = tjd;
            /* find next optimum visibility */
            retval = time_optimum_visibility(tjd, dgeo, datm, dobs, ObjectName, helflag, ref tjd, ref serr);
            if (retval == SwissEph.ERR) goto moon_event_err;
            dret[1] = tjd;
            /* find moment of becoming visible */
            /* Note: The on the day of fist light the moon may become visible 
             * already during day. It also may appear during day, disappear again
             * and then reappear after sunset */
            direct = 1;
            if (TypeEvent == 4)
                direct = -1;
            retval = time_limit_invisible(tjd, dgeo, datm, dobs, ObjectName, helflag, direct, ref tjd, ref serr);
            if (retval == SwissEph.ERR) goto moon_event_err;
            dret[2] = tjd;
            /* find moment of end of visibility */
            direct *= -1;
            retval = time_limit_invisible(dret[1], dgeo, datm, dobs, ObjectName, helflag, direct, ref tjd, ref  serr);
            dret[0] = tjd;
            if (retval == SwissEph.ERR) goto moon_event_err;
            /* if the moon is visible before sunset, we return sunset as start time */
            if (TypeEvent == 3) {
                if ((retval = my_rise_trans(tjd, SwissEph.SE_SUN, "", SwissEph.SE_CALC_SET, helflag, dgeo, datm, ref trise, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (trise < dret[1]) {
                    dret[0] = trise;
                    /* do not warn, it happens too often */
                    /*serr = "start time given is sunset, but moon is observable before that";*/
                }
                /* if the moon is visible after sunrise, we return sunrise as end time */
            } else {
                if ((retval = my_rise_trans(dret[1], SwissEph.SE_SUN, "", SwissEph.SE_CALC_RISE, helflag, dgeo, datm, ref trise, ref  serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (dret[0] > trise) {
                    dret[0] = trise;
                    /* do not warn, it happens too often */
                    /*serr = "end time given is sunrise, but moon is observable after that";*/
                }
            }
            /* correct order of the three times: */
            if (TypeEvent == 4) {
                tjd = dret[0];
                dret[0] = dret[2];
                dret[2] = tjd;
            }
        moon_event_err:
            if (!String.IsNullOrEmpty(serr))
                serr_ret = serr;
            return retval;
        }

        Int32 MoonEventJDut(double JDNDaysUTStart, double[] dgeo, double[] datm, double[] dobs, Int32 TypeEvent, Int32 helflag, double[] dret, ref string serr) {
            Int32 avkind = helflag & SwissEph.SE_HELFLAG_AVKIND;
            if (avkind != 0)
                return moon_event_arc_vis(JDNDaysUTStart, dgeo, datm, dobs, TypeEvent, helflag, dret, ref serr);
            else
                return moon_event_vis_lim(JDNDaysUTStart, dgeo, datm, dobs, TypeEvent, helflag, dret, ref serr);
        }

        Int32 heliacal_ut(double JDNDaysUTStart, double[] dgeo, double[] datm, double[] dobs, string ObjectName, Int32 TypeEventIn, Int32 helflag, double[] dret, ref string serr_ret) {
            Int32 avkind = helflag & SwissEph.SE_HELFLAG_AVKIND;
            if (avkind != 0)
                return heliacal_ut_arc_vis(JDNDaysUTStart, dgeo, datm, dobs, ObjectName, TypeEventIn, helflag, dret, ref serr_ret);
            else
                return heliacal_ut_vis_lim(JDNDaysUTStart, dgeo, datm, dobs, ObjectName, TypeEventIn, helflag, dret, ref serr_ret);
        }

        /*' Magn [-]
        ' tjd_ut            start date (JD) for event search
        ' dgeo[3]           geogr. longitude, latitude, eye height (m above sea level)
        ' datm[4]           atm. pressure, temperature, RH, and VR
        ' - pressure        atmospheric pressure (mbar, =hPa) default 1013.25hPa
        ' - temperature      deg C, default 15 deg C (if at
        '                   If both attemp and atpress are 0, a temperature and
        '                   atmospheric pressure are estimated from the above-mentioned
        '                   default values and the height above sea level.
        ' - RH              relative humidity in %
        ' - VR              VR>=1: the Meteorological range: default 40 km
        '                   1>VR>0: the ktot (so the total atmospheric coefficient): 
        '                   a good default would be 0.25
        '                   VR=-1: the ktot is calculated from the other atmospheric 
        '                   constants.
        ' age [Year]        default 36, experienced sky observer in ancient times
        '                   optimum age is 23
        ' SN                Snellen factor of the visual aquity of the observer
        '                   default 1
        '                   see: http://www.i-see.org/eyecharts.html#make-your-own
        ' TypeEvent         1 morning first
        '                   2 evening last
        '                   3 evening first
        '                   4 morning last
        ' dret		    output: time (tjd_ut) of heliacal event
        '                   dret[0]: beginning of visibility (Julian day number)
        '                   dret[1]: optimum visibility (Julian day number; 0 if SE_HELFLAG_AV)
        '                   dret[2]: end of visibility (Julian day number; 0 if SE_HELFLAG_AV)
        ' see http://www.iol.ie/~geniet/eng/atmoastroextinction.htm
        */
        public Int32 swe_heliacal_ut(double JDNDaysUTStart, double[] dgeo, double[] datm, double[] dobs, string ObjectNameIn, Int32 TypeEvent, Int32 helflag, double[] dret, ref string serr_ret) {
            Int32 retval, Planet, itry;
            string ObjectName = string.Empty, serr = string.Empty, s = string.Empty;
            double tjd0 = JDNDaysUTStart, tjd, dsynperiod, tjdmax, tadd;
            Int32 MaxCountSynodicPeriod = MAX_COUNT_SYNPER;
            string[] sevent = new String[] { "", "morning first", "evening last", "evening first", "morning last", "acronychal rising", "acronychal setting" };
            if (dgeo[2] < Sweph.SEI_ECL_GEOALT_MIN || dgeo[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr_ret = C.sprintf("location for heliacal events must be between %.0f and %.0f m above sea\n", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return SwissEph.ERR;
            }
            SE.SwephLib.swi_set_tid_acc(JDNDaysUTStart, helflag, 0, ref serr);
            if ((helflag & SwissEph.SE_HELFLAG_LONG_SEARCH) != 0)
                MaxCountSynodicPeriod = MAX_COUNT_SYNPER_MAX;
            /*  if (helflag & SE_HELFLAG_SEARCH_1_PERIOD)
                  MaxCountSynodicPeriod = 1; */
            serr = String.Empty;
            serr_ret = String.Empty;
            /* note, the fixed stars functions rewrite the star name. The input string 
               may be too short, so we have to make sure we have enough space */
            strcpy_VBsafe(out ObjectName, ObjectNameIn);
            default_heliacal_parameters(datm, dgeo, dobs, helflag);
            SE.swe_set_topo(dgeo[0], dgeo[1], dgeo[2]);
            Planet = DeterObject(ObjectName);
            if (Planet == SwissEph.SE_SUN)
            {
                serr_ret = "the sun has no heliacal rising or setting\n";
                return SwissEph.ERR;
            }
            /* 
             * Moon events
             */
            if (Planet == SwissEph.SE_MOON) {
                if (TypeEvent == 1 || TypeEvent == 2) {
                    serr_ret = C.sprintf("%s (event type %d) does not exist for the moon\n", sevent[TypeEvent], TypeEvent);
                    return SwissEph.ERR;
                }
                tjd = tjd0;
                retval = MoonEventJDut(tjd, dgeo, datm, dobs, TypeEvent, helflag, dret, ref serr);
                while (retval != -2 && dret[0] < tjd0) {
                    tjd += 15;
                    serr = String.Empty;
                    retval = MoonEventJDut(tjd, dgeo, datm, dobs, TypeEvent, helflag, dret, ref serr);
                }
                if (!String.IsNullOrEmpty(serr))
                    serr_ret = serr;
                return retval;
            }
            /* 
             * planets and fixed stars 
             */
            if (0 == (helflag & SwissEph.SE_HELFLAG_AVKIND)) {
                if (Planet == -1 || Planet >= SwissEph.SE_MARS) {
                    if (TypeEvent == 3 || TypeEvent == 4) {
                        if (Planet == -1)
                            s = ObjectName;
                        else
                            s = SE.swe_get_planet_name(Planet);
                        serr_ret = C.sprintf("%s (event type %d) does not exist for %s\n", sevent[TypeEvent], TypeEvent, s);
                        return SwissEph.ERR;
                    }
                }
            }
            /* arcus visionis method: set the TypeEvent for acronychal events */
            if ((helflag & SwissEph.SE_HELFLAG_AVKIND) != 0) {
                if (Planet == -1 || Planet >= SwissEph.SE_MARS) {
                    if (TypeEvent == SwissEph.SE_ACRONYCHAL_RISING)
                        TypeEvent = 3;
                    if (TypeEvent == SwissEph.SE_ACRONYCHAL_SETTING)
                        TypeEvent = 4;
                }
                /* acronychal rising and setting (cosmic setting) are ill-defined.
                 * We do not calculate them with the "visibility limit method" */
            } else /*if (true)*/ {
                if (TypeEvent == SwissEph.SE_ACRONYCHAL_RISING || TypeEvent == SwissEph.SE_ACRONYCHAL_SETTING) {
                    if (Planet == -1)
                        s = ObjectName;
                    else
                        s = SE.swe_get_planet_name(Planet);
                    serr_ret = C.sprintf("%s (event type %d) is not provided for %s\n", sevent[TypeEvent], TypeEvent, s);
                    return SwissEph.ERR;
                }
            }
            dsynperiod = get_synodic_period(Planet);
            tjdmax = tjd0 + dsynperiod * MaxCountSynodicPeriod;
            tadd = dsynperiod * 0.6;
            if (Planet == SwissEph.SE_MERCURY)
                tadd = 30;
            /* 
             * this is the outer loop over n synodic periods 
             */
            tjd = tjd0;
            retval = -2;  /* indicates that another synodic period has to be done */
            for (itry = 0;
                 tjd < tjdmax && retval == -2;
                 itry++, tjd += tadd) {
                serr = String.Empty;
                retval = heliacal_ut(tjd, dgeo, datm, dobs, ObjectName, TypeEvent, helflag, dret, ref serr);
                /* if resulting event date < start date for search (tjd0): retry starting
                 * from half a period later. The event must be found now, unless there
                 * is none, as is often the case with Mercury */
                while (retval != -2 && dret[0] < tjd0) {
                    tjd += tadd;
                    serr = String.Empty;
                    retval = heliacal_ut(tjd, dgeo, datm, dobs, ObjectName, TypeEvent, helflag, dret, ref serr);
                }
            }
            /* 
             * no event was found within MaxCountSynodicPeriod, return error
             */
            if ((helflag & SwissEph.SE_HELFLAG_SEARCH_1_PERIOD) != 0 && (retval == -2 || dret[0] > tjd0 + dsynperiod * 1.5)) {
                serr = "no heliacal date found within this synodic period";
                retval = -2;
            } else if (retval == -2) {
                serr = C.sprintf("no heliacal date found within %d synodic periods", MaxCountSynodicPeriod);
                retval = SwissEph.ERR;
            }
            if (!String.IsNullOrEmpty(serr))
                serr_ret = serr;
            return retval;
        }

    }

}
