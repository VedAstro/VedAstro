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
   $Header: /home/dieter/sweph/RCS/swemplan.c,v 1.74 2008/06/16 10:07:20 dieter Exp $
   Moshier planet routines

   modified for SWISSEPH by Dieter Koch

**************************************************************/
/* Copyright (C) 1997 - 2008 Astrodienst AG, Switzerland.  All rights reserved.
  
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

  Authors of the Swiss Ephemeris: Dieter Koch and Alois Treindl

  The authors of Swiss Ephemeris have no control or influence over any of
  the derived works, i.e. over software or services created by other
  programmers which use Swiss Ephemeris functions.

  The names of the authors or of the copyright holder (Astrodienst) must not
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
    using System.Globalization;
    using System.Linq;
    using System.Text;

    class SwemPlan : BaseCPort
    {
        public SwemPlan(SwissEph se)
            : base(se) {

        }
        //#include <string.h>
        //#include "swephexp.h"
        //#include "sweph.h"
        //#include "swephlib.h"
        //#include "swemptab.h"

        const double TIMESCALE = 3652500.0;

        double mods3600(double x) { return ((x) - 1.296e6 * Math.Floor((x) / 1.296e6)); }

        const int FICT_GEO = 1;
        const double KGAUSS_GEO = 0.0000298122353216; /* Earth only */
        ///* #define KGAUSS_GEO 0.00002999502129737  Earth + Moon */

        //static void embofs_mosh(double J, double *xemb);
        //static int check_t_terms(double t, char *sinp, double *doutp);

        //static int read_elements_file(int32 ipl, double tjd, 
        //  double *tjd0, double *tequ, 
        //  double *mano, double *sema, double *ecce, 
        //  double *parg, double *node, double *incl,
        //  char *pname, int32 *fict_ifl, char *serr);

        static readonly int[] pnoint2msh = new int[] { 2, 2, 0, 1, 3, 4, 5, 6, 7, 8, };


        /* From Simon et al (1994)  */
        static readonly double[] freqs = new double[]
        {
        /* Arc sec per 10000 Julian years.  */
          53810162868.8982,
          21066413643.3548,
          12959774228.3429,
          6890507749.3988,
          1092566037.7991,
          439960985.5372,
          154248119.3933,
          78655032.0744,
          52272245.1795
        };

        static readonly double[] phases = new double[]
        {
        /* Arc sec.  */
          252.25090552 * 3600.0,
          181.97980085 * 3600.0,
          100.46645683 * 3600.0,
          355.43299958 * 3600.0,
          34.35151874 * 3600.0,
          50.07744430 * 3600.0,
          314.05500511 * 3600.0,
          304.34866548 * 3600.0,
          860492.1546,
        };

        readonly Sweph.plantbl[] planets = new Sweph.plantbl[]{
            SwemTab.mer404,
            SwemTab.ven404,
            SwemTab.ear404,
            SwemTab.mar404,
            SwemTab.jup404,
            SwemTab.sat404,
            SwemTab.ura404,
            SwemTab.nep404,
            SwemTab.plu404
        };

        // rename with prefix plan_ because conflict with ss in swemmoon.c
        double[,] plan_ss = new double[9, 24];
        double[,] plan_cc = new double[9, 24];

        //static void sscc (int k, double arg, int n);

        int swi_moshplan2(double J, int iplm, CPointer<double> pobj) {
            int i, j, k, m, k1, ip, np, nt;
            sbyte[] p; int pidx;
            double[] plarr, pbarr, prarr; int plidx, pbidx, pridx;
            double su, cu, sv, cv, T;
            double t, sl, sb, sr;
            Sweph.plantbl plan = planets[iplm];

            T = (J - Sweph.J2000) / TIMESCALE;
            /* Calculate sin( i*MM ), etc. for needed multiple angles.  */
            for (i = 0; i < 9; i++) {
                if ((j = plan.max_harmonic[i]) > 0) {
                    sr = (mods3600(freqs[i] * T) + phases[i]) * Sweph.STR;
                    plan_sscc(i, sr, j);
                }
            }

            /* Point to start of table of arguments. */
            p = plan.arg_tbl; pidx = 0;
            /* Point to tabulated cosine and sine amplitudes.  */
            plarr = plan.lon_tbl; plidx = 0;
            pbarr = plan.lat_tbl; pbidx = 0;
            prarr = plan.rad_tbl; pridx = 0;
            sl = 0.0;
            sb = 0.0;
            sr = 0.0;

            for (; ; ) {
                /* argument of sine and cosine */
                /* Number of periodic arguments. */
                np = p[pidx++];
                if (np < 0)
                    break;
                if (np == 0) {			/* It is a polynomial term.  */
                    nt = p[pidx++];
                    /* Longitude polynomial. */
                    cu = plarr[plidx++];
                    for (ip = 0; ip < nt; ip++) {
                        cu = cu * T + plarr[plidx++];
                    }
                    sl += mods3600(cu);
                    /* Latitude polynomial. */
                    cu = pbarr[pbidx++];
                    for (ip = 0; ip < nt; ip++) {
                        cu = cu * T + pbarr[pbidx++];
                    }
                    sb += cu;
                    /* Radius polynomial. */
                    cu = prarr[pridx++];
                    for (ip = 0; ip < nt; ip++) {
                        cu = cu * T + prarr[pridx++];
                    }
                    sr += cu;
                    continue;
                }
                k1 = 0;
                cv = 0.0;
                sv = 0.0;
                for (ip = 0; ip < np; ip++) {
                    /* What harmonic.  */
                    j = p[pidx++];
                    /* Which planet.  */
                    m = p[pidx++] - 1;
                    if (j != 0) {
                        k = j;
                        if (j < 0)
                            k = -k;
                        k -= 1;
                        su = plan_ss[m, k];	/* sin(k*angle) */
                        if (j < 0)
                            su = -su;
                        cu = plan_cc[m, k];
                        if (k1 == 0) {		/* set first angle */
                            sv = su;
                            cv = cu;
                            k1 = 1;
                        } else {		/* combine angles */
                            t = su * cv + cu * sv;
                            cv = cu * cv - su * sv;
                            sv = t;
                        }
                    }
                }
                /* Highest power of T.  */
                nt = p[pidx++];
                /* Longitude. */
                cu = plarr[plidx++];
                su = plarr[plidx++];
                for (ip = 0; ip < nt; ip++) {
                    cu = cu * T + plarr[plidx++];
                    su = su * T + plarr[plidx++];
                }
                sl += cu * cv + su * sv;
                /* Latitiude. */
                cu = pbarr[pbidx++];
                su = pbarr[pbidx++];
                for (ip = 0; ip < nt; ip++) {
                    cu = cu * T + pbarr[pbidx++];
                    su = su * T + pbarr[pbidx++];
                }
                sb += cu * cv + su * sv;
                /* Radius. */
                cu = prarr[pridx++];
                su = prarr[pridx++];
                for (ip = 0; ip < nt; ip++) {
                    cu = cu * T + prarr[pridx++];
                    su = su * T + prarr[pridx++];
                }
                sr += cu * cv + su * sv;
            }
            pobj[0] = Sweph.STR * sl;
            pobj[1] = Sweph.STR * sb;
            pobj[2] = Sweph.STR * plan.distance * sr + plan.distance;
            return SwissEph.OK;
        }

        /* Moshier ephemeris.
         * computes heliocentric cartesian equatorial coordinates of
         * equinox 2000
         * for earth and a planet
         * tjd		julian day
         * ipli		internal SWEPH planet number
         * xp		array of 6 doubles for planet's position and speed
         * xe		                       earth's
         * serr		error string
         */
        public int swi_moshplan(double tjd, int ipli, bool do_save, CPointer<double> xpret, CPointer<double> xeret, ref string serr) {
            int i;
            bool do_earth = false;
            double[] dx = new double[3], x2 = new double[3], xxe = new double[6], xxp = new double[6];
            double[] xp, xe;
            double dt;
            string s = String.Empty;
            int iplm = pnoint2msh[ipli];
            Sweph.plan_data pdp = SE.Sweph.swed.pldat[ipli];
            Sweph.plan_data pedp = SE.Sweph.swed.pldat[Sweph.SEI_EARTH];
            double seps2000 = SE.Sweph.swed.oec2000.seps;
            double ceps2000 = SE.Sweph.swed.oec2000.ceps;
            if (do_save) {
                xp = pdp.x;
                xe = pedp.x;
            } else {
                xp = xxp;
                xe = xxe;
            }
            if (do_save || ipli == Sweph.SEI_EARTH || xeret != null)
                do_earth = true;
            /* tjd beyond ephemeris limits, give some margin for spped at edge */
            if (tjd < Sweph.MOSHPLEPH_START - 0.3 || tjd > Sweph.MOSHPLEPH_END + 0.3) {
                s = C.sprintf("jd %f outside Moshier planet range %.2f .. %.2f ",
                      tjd, Sweph.MOSHPLEPH_START, Sweph.MOSHPLEPH_END);
                serr = (serr ?? String.Empty) + s;
                return (SwissEph.ERR);
            }
            /* earth, for geocentric position */
            if (do_earth) {
                if (tjd == pedp.teval
                  && pedp.iephe == SwissEph.SEFLG_MOSEPH) {
                    xe = pedp.x;
                } else {
                    /* emb */
                    swi_moshplan2(tjd, pnoint2msh[Sweph.SEI_EMB], xe); /* emb hel. ecl. 2000 polar */
                    SE.SwephLib.swi_polcart(xe, xe);			  /* to cartesian */
                    SE.SwephLib.swi_coortrf2(xe, xe, -seps2000, ceps2000);/* and equator 2000 */
                    embofs_mosh(tjd, xe);		  /* emb -> earth */
                    if (do_save) {
                        pedp.teval = tjd;
                        pedp.xflgs = -1;
                        pedp.iephe = SwissEph.SEFLG_MOSEPH;
                    }
                    /* one more position for speed. */
                    swi_moshplan2(tjd - Sweph.PLAN_SPEED_INTV, pnoint2msh[Sweph.SEI_EMB], x2);
                    SE.SwephLib.swi_polcart(x2, x2);
                    SE.SwephLib.swi_coortrf2(x2, x2, -seps2000, ceps2000);
                    embofs_mosh(tjd - Sweph.PLAN_SPEED_INTV, x2);/**/
                    for (i = 0; i <= 2; i++)
                        dx[i] = (xe[i] - x2[i]) / Sweph.PLAN_SPEED_INTV;
                    /* store speed */
                    for (i = 0; i <= 2; i++) {
                        xe[i + 3] = dx[i];
                    }
                }
                if (xeret != null)
                    for (i = 0; i <= 5; i++)
                        xeret[i] = xe[i];
            }
            /* earth is the planet wanted */
            if (ipli == Sweph.SEI_EARTH) {
                xp = xe;
            } else {
                /* other planet */
                /* if planet has already been computed, return */
                if (tjd == pdp.teval && pdp.iephe == SwissEph.SEFLG_MOSEPH) {
                    xp = pdp.x;
                } else {
                    swi_moshplan2(tjd, iplm, xp);
                    SE.SwephLib.swi_polcart(xp, xp);
                    SE.SwephLib.swi_coortrf2(xp, xp, -seps2000, ceps2000);
                    if (do_save) {
                        pdp.teval = tjd;/**/
                        pdp.xflgs = -1;
                        pdp.iephe = SwissEph.SEFLG_MOSEPH;
                    }
                    /* one more position for speed. 
                     * the following dt gives good speed for light-time correction
                     */
                    //#if 0
                    //  for (i = 0; i <= 2; i++) 
                    //dx[i] = xp[i] - pedp.x[i];
                    //  dt = LIGHTTIME_AUNIT * Math.Sqrt(square_sum(dx));   
                    //#endif
                    dt = Sweph.PLAN_SPEED_INTV;
                    swi_moshplan2(tjd - dt, iplm, x2);
                    SE.SwephLib.swi_polcart(x2, x2);
                    SE.SwephLib.swi_coortrf2(x2, x2, -seps2000, ceps2000);
                    for (i = 0; i <= 2; i++)
                        dx[i] = (xp[i] - x2[i]) / dt;
                    /* store speed */
                    for (i = 0; i <= 2; i++) {
                        xp[i + 3] = dx[i];
                    }
                }
                if (xpret != null)
                    for (i = 0; i <= 5; i++)
                        xpret[i] = xp[i];
            }
            return (SwissEph.OK);
        }


        /* Prepare lookup table of sin and cos ( i*Lj )
         * for required multiple angles
         */
        void plan_sscc(int k, double arg, int n) {
            double cu, su, cv, sv, s;
            int i;

            su = Math.Sin(arg);
            cu = Math.Cos(arg);
            plan_ss[k, 0] = su;		/* sin(L) */
            plan_cc[k, 0] = cu;		/* cos(L) */
            sv = 2.0 * su * cu;
            cv = cu * cu - su * su;
            plan_ss[k, 1] = sv;		/* sin(2L) */
            plan_cc[k, 1] = cv;
            for (i = 2; i < n; i++) {
                s = su * cv + cu * sv;
                cv = cu * cv - su * sv;
                sv = s;
                plan_ss[k, i] = sv;		/* sin( i+1 L ) */
                plan_cc[k, i] = cv;
            }
        }


        /* Adjust position from Earth-Moon barycenter to Earth
         *
         * J = Julian day number
         * xemb = rectangular equatorial coordinates of Earth
         */
        void embofs_mosh(double tjd, CPointer<double> xemb) {
            double T, M, a, L, B, p;
            double smp, cmp, s2mp, c2mp, s2d, c2d, sf, cf;
            double s2f, sx, cx; double[] xyz = new double[6];
            double seps = SE.Sweph.swed.oec.seps;
            double ceps = SE.Sweph.swed.oec.ceps;
            int i;
            /* Short series for position of the Moon
             */
            T = (tjd - Sweph.J1900) / 36525.0;
            /* Mean anomaly of moon (MP) */
            a = SE.swe_degnorm(((1.44e-5 * T + 0.009192) * T + 477198.8491) * T + 296.104608);
            a *= SwissEph.DEGTORAD;
            smp = Math.Sin(a);
            cmp = Math.Cos(a);
            s2mp = 2.0 * smp * cmp;		/* sin(2MP) */
            c2mp = cmp * cmp - smp * smp;	/* cos(2MP) */
            /* Mean elongation of moon (D) */
            a = SE.swe_degnorm(((1.9e-6 * T - 0.001436) * T + 445267.1142) * T + 350.737486);
            a = 2.0 * SwissEph.DEGTORAD * a;
            s2d = Math.Sin(a);
            c2d = Math.Cos(a);
            /* Mean distance of moon from its ascending node (F) */
            a = SE.swe_degnorm(((-3.0e-7 * T - 0.003211) * T + 483202.0251) * T + 11.250889);
            a *= SwissEph.DEGTORAD;
            sf = Math.Sin(a);
            cf = Math.Cos(a);
            s2f = 2.0 * sf * cf;	/* sin(2F) */
            sx = s2d * cmp - c2d * smp;	/* sin(2D - MP) */
            cx = c2d * cmp + s2d * smp;	/* cos(2D - MP) */
            /* Mean longitude of moon (LP) */
            L = ((1.9e-6 * T - 0.001133) * T + 481267.8831) * T + 270.434164;
            /* Mean anomaly of sun (M) */
            M = SE.swe_degnorm(((-3.3e-6 * T - 1.50e-4) * T + 35999.0498) * T + 358.475833);
            /* Ecliptic longitude of the moon */
            L = L
              + 6.288750 * smp
              + 1.274018 * sx
              + 0.658309 * s2d
              + 0.213616 * s2mp
              - 0.185596 * Math.Sin(SwissEph.DEGTORAD * M)
              - 0.114336 * s2f;
            /* Ecliptic latitude of the moon */
            a = smp * cf;
            sx = cmp * sf;
            B = 5.128189 * sf
              + 0.280606 * (a + sx)		/* sin(MP+F) */
              + 0.277693 * (a - sx)		/* sin(MP-F) */
              + 0.173238 * (s2d * cf - c2d * sf);	/* sin(2D-F) */
            B *= SwissEph.DEGTORAD;
            /* Parallax of the moon */
            p = 0.950724
              + 0.051818 * cmp
              + 0.009531 * cx
              + 0.007843 * c2d
              + 0.002824 * c2mp;
            p *= SwissEph.DEGTORAD;
            /* Elongation of Moon from Sun
             */
            L = SE.swe_degnorm(L);
            L *= SwissEph.DEGTORAD;
            /* Distance in au */
            a = 4.263523e-5 / Math.Sin(p);
            /* Convert to rectangular ecliptic coordinates */
            xyz[0] = L;
            xyz[1] = B;
            xyz[2] = a;
            SE.SwephLib.swi_polcart(xyz, xyz);
            /* Convert to equatorial */
            SE.SwephLib.swi_coortrf2(xyz, xyz, -seps, ceps);
            /* Precess to equinox of J2000.0 */
            SE.SwephLib.swi_precess(xyz, tjd, 0, Sweph.J_TO_J2000);/**/
            /* now emb -> earth */
            for (i = 0; i <= 2; i++)
                xemb[i] -= xyz[i] / (Sweph.EARTH_MOON_MRAT + 1.0);
        }

        /* orbital elements of planets that are computed from osculating elements
         *   epoch
         *   equinox
         *   mean anomaly,
         *   semi axis,
         *   eccentricity,
         *   argument of perihelion,
         *   ascending node
         *   inclination
         */
        public bool SE_NEELY = true;               /* use James Neely's revised elements 
                                         *      of Uranian planets*/
        static readonly string[] plan_fict_nam = new string[SwissEph.SE_NFICT_ELEM]{
            "Cupido", "Hades", "Zeus", "Kronos", 
           "Apollon", "Admetos", "Vulkanus", "Poseidon",
           "Isis-Transpluto", "Nibiru", "Harrington",
           "Leverrier", "Adams",
           "Lowell", "Pickering"};

        public string swi_get_fict_name(Int32 ipl) {
            string snam = null, serr = null; double dummy = 0; int idummy = 0;
            if (read_elements_file(ipl, 0, ref dummy, ref dummy,
                 ref dummy, ref dummy, ref dummy, ref dummy, ref dummy, ref dummy,
                 ref snam, ref idummy, ref serr) == SwissEph.ERR)
                snam = "name not found";
            return snam;
        }

        static readonly double[,] plan_oscu_elem_neely = new double[SwissEph.SE_NFICT_ELEM, 8] {
          {Sweph.J1900, Sweph.J1900, 163.7409, 40.99837, 0.00460, 171.4333, 129.8325, 1.0833},/* Cupido Neely */ 
          {Sweph.J1900, Sweph.J1900,  27.6496, 50.66744, 0.00245, 148.1796, 161.3339, 1.0500},/* Hades Neely */
          {Sweph.J1900, Sweph.J1900, 165.1232, 59.21436, 0.00120, 299.0440,   0.0000, 0.0000},/* Zeus Neely */
          {Sweph.J1900, Sweph.J1900, 169.0193, 64.81960, 0.00305, 208.8801,   0.0000, 0.0000},/* Kronos Neely */ 
          {Sweph.J1900, Sweph.J1900, 138.0533, 70.29949, 0.00000,   0.0000,   0.0000, 0.0000},/* Apollon Neely */
          {Sweph.J1900, Sweph.J1900, 351.3350, 73.62765, 0.00000,   0.0000,   0.0000, 0.0000},/* Admetos Neely */
          {Sweph.J1900, Sweph.J1900,  55.8983, 77.25568, 0.00000,   0.0000,   0.0000, 0.0000},/* Vulcanus Neely */
          {Sweph.J1900, Sweph.J1900, 165.5163, 83.66907, 0.00000,   0.0000,   0.0000, 0.0000},/* Poseidon Neely */
          /* Isis-Transpluto; elements from "Die Sterne" 3/1952, p. 70ff. 
           * Strubell does not give an equinox. 1945 is taken to best reproduce 
           * ASTRON ephemeris. (This is a strange choice, though.)
           * The epoch is 1772.76. The year is understood to have 366 days.
           * The fraction is counted from 1 Jan. 1772 */
          {2368547.66, 2431456.5, 0.0, 77.775, 0.3, 0.7, 0, 0},
          /* Nibiru, elements from Christian Woeltge, Hannover */
          {1856113.380954, 1856113.380954, 0.0, 234.8921, 0.981092, 103.966, -44.567, 158.708},
          /* Harrington, elements from Astronomical Journal 96(4), Oct. 1988 */
          {2374696.5, Sweph.J2000, 0.0, 101.2, 0.411, 208.5, 275.4, 32.4},
          /* Leverrier's Neptune, 
            according to W.G. Hoyt, "Planets X and Pluto", Tucson 1980, p. 63 */
          {2395662.5, 2395662.5, 34.05, 36.15, 0.10761, 284.75, 0, 0}, 
          /* Adam's Neptune */
          {2395662.5, 2395662.5, 24.28, 37.25, 0.12062, 299.11, 0, 0}, 
          /* Lowell's Pluto */
          {2425977.5, 2425977.5, 281, 43.0, 0.202, 204.9, 0, 0}, 
          /* Pickering's Pluto */
          {2425977.5, 2425977.5, 48.95, 55.1, 0.31, 280.1, 100, 15}, /**/
        };
        static double[,] plan_oscu_elem_no_neely = new double[SwissEph.SE_NFICT_ELEM, 8] {
          {Sweph.J1900, Sweph.J1900, 104.5959, 40.99837,  0, 0, 0, 0}, /* Cupido   */
          {Sweph.J1900, Sweph.J1900, 337.4517, 50.667443, 0, 0, 0, 0}, /* Hades    */
          {Sweph.J1900, Sweph.J1900, 104.0904, 59.214362, 0, 0, 0, 0}, /* Zeus     */
          {Sweph.J1900, Sweph.J1900,  17.7346, 64.816896, 0, 0, 0, 0}, /* Kronos   */
          {Sweph.J1900, Sweph.J1900, 138.0354, 70.361652, 0, 0, 0, 0}, /* Apollon  */
          {Sweph.J1900, Sweph.J1900,  -8.678,  73.736476, 0, 0, 0, 0}, /* Admetos  */
          {Sweph.J1900, Sweph.J1900,  55.9826, 77.445895, 0, 0, 0, 0}, /* Vulkanus */
          {Sweph.J1900, Sweph.J1900, 165.3595, 83.493733, 0, 0, 0, 0}, /* Poseidon */
          /* Isis-Transpluto; elements from "Die Sterne" 3/1952, p. 70ff. 
           * Strubell does not give an equinox. 1945 is taken to best reproduce 
           * ASTRON ephemeris. (This is a strange choice, though.)
           * The epoch is 1772.76. The year is understood to have 366 days.
           * The fraction is counted from 1 Jan. 1772 */
          {2368547.66, 2431456.5, 0.0, 77.775, 0.3, 0.7, 0, 0},
          /* Nibiru, elements from Christian Woeltge, Hannover */
          {1856113.380954, 1856113.380954, 0.0, 234.8921, 0.981092, 103.966, -44.567, 158.708},
          /* Harrington, elements from Astronomical Journal 96(4), Oct. 1988 */
          {2374696.5, Sweph.J2000, 0.0, 101.2, 0.411, 208.5, 275.4, 32.4},
          /* Leverrier's Neptune, 
            according to W.G. Hoyt, "Planets X and Pluto", Tucson 1980, p. 63 */
          {2395662.5, 2395662.5, 34.05, 36.15, 0.10761, 284.75, 0, 0}, 
          /* Adam's Neptune */
          {2395662.5, 2395662.5, 24.28, 37.25, 0.12062, 299.11, 0, 0}, 
          /* Lowell's Pluto */
          {2425977.5, 2425977.5, 281, 43.0, 0.202, 204.9, 0, 0}, 
          /* Pickering's Pluto */
          {2425977.5, 2425977.5, 48.95, 55.1, 0.31, 280.1, 100, 15}, /**/
        };

        double[,] plan_oscu_elem { get { return SE_NEELY ? plan_oscu_elem_neely : plan_oscu_elem_no_neely; } }

        /* computes a planet from osculating elements *
         * tjd		julian day
         * ipl		body number
         * ipli 	body number in planetary data structure
         * iflag	flags
         */
        public int swi_osc_el_plan(double tjd, CPointer<double> xp, int ipl, int ipli, CPointer<double> xearth, CPointer<double> xsun, ref string serr) {
            double[] pqr = new double[9], x = new double[6];
            double eps, K, fac, rho, cose, sine;
            double alpha, beta, zeta, sigma, M2, Msgn, M_180_or_0;
            double tjd0 = 0, tequ = 0, mano = 0, sema = 0, ecce = 0, parg = 0, node = 0, incl = 0, dmot;
            double cosnode, sinnode, cosincl, sinincl, cosparg, sinparg;
            double M, E; string sdummy = null;
            Sweph.plan_data pedp = SE.Sweph.swed.pldat[Sweph.SEI_EARTH];
            Sweph.plan_data pdp = SE.Sweph.swed.pldat[ipli];
            Int32 fict_ifl = 0;
            int i;
            /* orbital elements, either from file or, if file not found,
             * from above built-in set  
             */
            if (read_elements_file(ipl, tjd, ref tjd0, ref tequ,
                 ref mano, ref sema, ref ecce, ref parg, ref node, ref incl,
                 ref sdummy, ref fict_ifl, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            dmot = 0.9856076686 * SwissEph.DEGTORAD / sema / Math.Sqrt(sema);	/* daily motion */
            if ((fict_ifl & FICT_GEO) != 0)
                dmot /= Math.Sqrt(Sweph.SUN_EARTH_MRAT);
            cosnode = Math.Cos(node);
            sinnode = Math.Sin(node);
            cosincl = Math.Cos(incl);
            sinincl = Math.Sin(incl);
            cosparg = Math.Cos(parg);
            sinparg = Math.Sin(parg);
            /* Gaussian vector */
            pqr[0] = cosparg * cosnode - sinparg * cosincl * sinnode;
            pqr[1] = -sinparg * cosnode - cosparg * cosincl * sinnode;
            pqr[2] = sinincl * sinnode;
            pqr[3] = cosparg * sinnode + sinparg * cosincl * cosnode;
            pqr[4] = -sinparg * sinnode + cosparg * cosincl * cosnode;
            pqr[5] = -sinincl * cosnode;
            pqr[6] = sinparg * sinincl;
            pqr[7] = cosparg * sinincl;
            pqr[8] = cosincl;
            /* Kepler problem */
            E = M = SE.SwephLib.swi_mod2PI(mano + (tjd - tjd0) * dmot); /* mean anomaly of date */
            /* better E for very high eccentricity and small M */
            if (ecce > 0.975) {
                M2 = M * SwissEph.RADTODEG;
                if (M2 > 150 && M2 < 210) {
                    M2 -= 180;
                    M_180_or_0 = 180;
                } else
                    M_180_or_0 = 0;
                if (M2 > 330)
                    M2 -= 360;
                if (M2 < 0) {
                    M2 = -M2;
                    Msgn = -1;
                } else
                    Msgn = 1;
                if (M2 < 30) {
                    M2 *= SwissEph.DEGTORAD;
                    alpha = (1 - ecce) / (4 * ecce + 0.5);
                    beta = M2 / (8 * ecce + 1);
                    zeta = Math.Pow(beta + Math.Sqrt(beta * beta + alpha * alpha), 1 / 3);
                    sigma = zeta - alpha / 2;
                    sigma = sigma - 0.078 * sigma * sigma * sigma * sigma * sigma / (1 + ecce);
                    E = Msgn * (M2 + ecce * (3 * sigma - 4 * sigma * sigma * sigma))
                          + M_180_or_0;
                }
            }
            E = SE.SwephLib.swi_kepler(E, M, ecce);
            /* position and speed, referred to orbital plane */
            if ((fict_ifl & FICT_GEO) != 0)
                K = KGAUSS_GEO / Math.Sqrt(sema);
            else
                K = Sweph.KGAUSS / Math.Sqrt(sema);
            cose = Math.Cos(E);
            sine = Math.Sin(E);
            fac = Math.Sqrt((1 - ecce) * (1 + ecce));
            rho = 1 - ecce * cose;
            x[0] = sema * (cose - ecce);
            x[1] = sema * fac * sine;
            x[3] = -K * sine / rho;
            x[4] = K * fac * cose / rho;
            /* transformation to ecliptic */
            xp[0] = pqr[0] * x[0] + pqr[1] * x[1];
            xp[1] = pqr[3] * x[0] + pqr[4] * x[1];
            xp[2] = pqr[6] * x[0] + pqr[7] * x[1];
            xp[3] = pqr[0] * x[3] + pqr[1] * x[4];
            xp[4] = pqr[3] * x[3] + pqr[4] * x[4];
            xp[5] = pqr[6] * x[3] + pqr[7] * x[4];
            /* transformation to equator */
            eps = SE.SwephLib.swi_epsiln(tequ, 0);
            SE.SwephLib.swi_coortrf(xp, xp, -eps);
            SE.SwephLib.swi_coortrf(xp + 3, xp + 3, -eps);
            /* precess to J2000 */
            if (tequ != Sweph.J2000) {
                SE.SwephLib.swi_precess(xp, tequ, 0, Sweph.J_TO_J2000);
                SE.SwephLib.swi_precess(xp + 3, tequ, 0, Sweph.J_TO_J2000);
            }
            /* to solar system barycentre */
            if ((fict_ifl & FICT_GEO) != 0) {
                for (i = 0; i <= 5; i++) {
                    xp[i] += xearth[i];
                }
            } else {
                for (i = 0; i <= 5; i++) {
                    xp[i] += xsun[i];
                }
            }
            if (pdp.x == xp) {
                pdp.teval = tjd;	/* for precession! */
                pdp.iephe = pedp.iephe;
            }
            return SwissEph.OK;
        }

        //#if 1
        /* note: input parameter tjd is required for T terms in elements */
        int read_elements_file(Int32 ipl, double tjd,
          ref double tjd0, ref double tequ,
          ref double mano, ref double sema, ref double ecce,
          ref double parg, ref double node, ref double incl,
          ref string pname, ref Int32 fict_ifl, ref string serr) {
            int //i, 
                iline, iplan, retc, ncpos;
            CFile fp = null;
            string s = string.Empty, sp;
            string[] cpos; String serri = string.Empty;
            bool elem_found = false;
            double tt = 0;
            try {
                /* -1, because file information is not saved, file is always closed */
                if ((fp = SE.Sweph.swi_fopen(-1, SwissEph.SE_FICTFILE, SE.Sweph.swed.ephepath, ref serr)) == null) {
                    /* file does not exist, use built-in bodies */
                    if (ipl >= SwissEph.SE_NFICT_ELEM) {
                        //if (serr != null)
                        serr = C.sprintf("error no elements for fictitious body no %7.0f", (double)ipl);
                        return SwissEph.ERR;
                    }
                    //    if (tjd0 != NULL)
                    tjd0 = plan_oscu_elem[ipl, 0];			/* epoch */
                    //    if (tequ != NULL)
                    tequ = plan_oscu_elem[ipl, 1];			/* equinox */
                    //    if (mano != NULL)
                    mano = plan_oscu_elem[ipl, 2] * SwissEph.DEGTORAD;	/* mean anomaly */
                    //    if (sema != NULL)
                    sema = plan_oscu_elem[ipl, 3];			/* semi-axis */
                    //    if (ecce != NULL)
                    ecce = plan_oscu_elem[ipl, 4];			/* eccentricity */
                    //    if (parg != NULL)
                    parg = plan_oscu_elem[ipl, 5] * SwissEph.DEGTORAD;	/* arg. of peri. */
                    //    if (node != NULL)
                    node = plan_oscu_elem[ipl, 6] * SwissEph.DEGTORAD;	/* asc. node */
                    //    if (incl != NULL)
                    incl = plan_oscu_elem[ipl, 7] * SwissEph.DEGTORAD;	/* inclination */
                    //    if (pname != NULL)
                    pname = plan_fict_nam[ipl];
                    return SwissEph.OK;
                }
                /* 
                 * find elements in file 
                 */
                iline = 0;
                iplan = -1;
                while ((s = fp.ReadLine()) != null) {
                    iline++;
                    //sp = s;
                    //    while(*sp == ' ' || *sp == '\t')
                    //      sp++;
                    //    swi_strcpy(s, sp);
                    s = s.TrimStart(' ', '\t');
                    //    if (*s == '#')
                    //      continue;
                    if (s.StartsWith("#")) continue;
                    //    if (*s == '\r')
                    //      continue;
                    //    if (*s == '\n')
                    //      continue;
                    //    if (*s == '\0')
                    //      continue;
                    if (String.IsNullOrWhiteSpace(s)) continue;
                    //    if ((sp = strchr(s, '#')) != NULL)
                    //      *sp = '\0';
                    int ip = s.IndexOf('#');
                    if (ip >= 0) s = s.Substring(0, ip);
                    //    ncpos = swi_cutstr(s, ",", cpos, 20);
                    cpos = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    ncpos = cpos.Length;
                    serri = C.sprintf("error in file %s, line %7.0f:", SwissEph.SE_FICTFILE, (double)iline);
                    if (ncpos < 9) {
                        //      if (serr != NULL) {
                        serr = C.sprintf("%s nine elements required", serri);
                        //      }
                        //goto return_err;
                        return SwissEph.ERR;
                    }
                    iplan++;
                    if (iplan != ipl)
                        continue;
                    elem_found = true;
                    /* epoch of elements */
                    //    if (tjd0 != NULL) {
                    sp = cpos[0].ToLower();
                    if (sp.StartsWith("j2000"))
                        tjd0 = Sweph.J2000;
                    else if (sp.StartsWith("b1950"))
                        tjd0 = Sweph.B1950;
                    else if (sp.StartsWith("j1900"))
                        tjd0 = Sweph.J1900;
                    else if (sp.StartsWith("j") || sp.StartsWith("b")) {
                        serr = C.sprintf("%s invalid epoch", serri);
                        return SwissEph.ERR;
                    } else
                        tjd0 = double.Parse(sp, CultureInfo.InvariantCulture);
                    tt = tjd - tjd0;
                    //    }
                    /* equinox */
                    //    if (tequ != NULL) {
                    sp = cpos[1].TrimStart(' ', '\t').ToLower();
                    if (sp.StartsWith("j2000"))
                        tequ = Sweph.J2000;
                    else if (sp.StartsWith("b1950"))
                        tequ = Sweph.B1950;
                    else if (sp.StartsWith("j1900"))
                        tequ = Sweph.J1900;
                    else if (sp.StartsWith("jdate"))
                        tequ = tjd;
                    else if (sp.StartsWith("j") || sp.StartsWith("b")) {
                        serr = C.sprintf("%s invalid equinox", serri);
                        return SwissEph.ERR;
                    } else
                        tequ = double.Parse(sp, CultureInfo.InvariantCulture);
                    //    }
                    /* mean anomaly t0 */
                    //    if (mano != NULL) {
                    retc = check_t_terms(tt, cpos[2], out mano);
                    mano = SE.swe_degnorm(mano);
                    if (retc == SwissEph.ERR) {
                        serr = C.sprintf("%s mean anomaly value invalid", serri);
                        return SwissEph.ERR;
                    }
                    /* if mean anomaly has t terms (which happens with fictitious 
                     * planet Vulcan), we set
                     * epoch = tjd, so that no motion will be added anymore 
                     * equinox = tjd */
                    if (retc == 1) {
                        tjd0 = tjd;
                    }
                    mano *= SwissEph.DEGTORAD;
                    //    }
                    /* semi-axis */
                    //    if (sema != NULL) {
                    retc = check_t_terms(tt, cpos[3], out sema);
                    if (sema <= 0 || retc == SwissEph.ERR) {
                        serr = C.sprintf("%s semi-axis value invalid", serri);
                        return SwissEph.ERR;
                    }
                    //    }
                    /* eccentricity */
                    //    if (ecce != NULL) {
                    retc = check_t_terms(tt, cpos[4], out ecce);
                    if (ecce >= 1 || ecce < 0 || retc == SwissEph.ERR) {
                        serr = C.sprintf("%s eccentricity invalid (no parabolic or hyperbolic orbits allowed)", serri);
                        return SwissEph.ERR;
                    }
                    //    }
                    /* perihelion argument */
                    //    if (parg != NULL) {
                    retc = check_t_terms(tt, cpos[5], out parg);
                    parg = SE.swe_degnorm(parg);
                    if (retc == SwissEph.ERR) {
                        serr = C.sprintf("%s perihelion argument value invalid", serri);
                        return SwissEph.ERR;
                    }
                    parg *= SwissEph.DEGTORAD;
                    //    }
                    /* node */
                    //    if (node != NULL) {
                    retc = check_t_terms(tt, cpos[6], out node);
                    node = SE.swe_degnorm(node);
                    if (retc == SwissEph.ERR) {
                        serr = C.sprintf("%s node value invalid", serri);
                        return SwissEph.ERR;
                    }
                    node *= SwissEph.DEGTORAD;
                    //    }
                    /* inclination */
                    //    if (incl != NULL) {
                    retc = check_t_terms(tt, cpos[7], out incl);
                    incl = SE.swe_degnorm(incl);
                    if (retc == SwissEph.ERR) {
                        serr = C.sprintf("%s inclination value invalid", serri);
                        return SwissEph.ERR;
                    }
                    incl *= SwissEph.DEGTORAD;
                    //    }
                    /* planet name */
                    //    if (pname != NULL) {
                    pname = cpos[8].Trim(' ', '\t');
                    //    }
                    /* geocentric */
                    if (ncpos > 9) {
                        cpos[9] = cpos[9].ToLower();
                        if (cpos[9].Contains("geo"))
                            fict_ifl |= FICT_GEO;
                    }
                    break;
                }
                if (!elem_found) {
                    serr = C.sprintf("%s elements for planet %7.0f not found", serri, (double)ipl);
                    return SwissEph.ERR;
                }
            }
            finally {
                if (fp != null)
                    fp.Dispose();
            }
            //  fclose(fp);
            return SwissEph.OK;
            //return_err:
            //  fclose(fp);
            //  return ERR;
        }
        //#endif

        int check_t_terms(double t, string sinp, out double doutp) {
            int i, isgn = 1, z;
            int retc = 0;
            string sp;
            double[] tt = new double[5]; double fac;
            tt[0] = t / 36525;
            tt[1] = tt[0];
            tt[2] = tt[1] * tt[1];
            tt[3] = tt[2] * tt[1];
            tt[4] = tt[3] * tt[1];
            if (sinp.Contains(new char[] { '+', '-' }))
                retc = 1; /* with additional terms */
            sp = sinp;
            doutp = 0;
            fac = 1;
            z = 0;
            while (true) {
                sp = sp.TrimStart(' ', '\t');
                if (String.IsNullOrWhiteSpace(sp) || sp.StartsWith("+") || sp.StartsWith("-")) {
                    if (z > 0)
                        doutp += fac;
                    isgn = 1;
                    if (sp != null && sp.StartsWith("-"))
                        isgn = -1;
                    fac = 1 * isgn;
                    if (String.IsNullOrWhiteSpace(sp))
                        return retc;
                    sp = sp.Substring(1);
                } else {
                    sp = sp.TrimStart('*', ' ', '\t');
                    if (sp != null && sp.StartsWith("t", StringComparison.OrdinalIgnoreCase)) {
                        /* a T */
                        sp = sp.Substring(1);
                        if (sp != null && (sp.StartsWith("+") || sp.StartsWith("-")))
                            fac *= tt[0];
                        else
                        {
                            if (!int.TryParse(sp, out i)) i = 0;
                            if (i <= 4 && i >= 0)
                                fac *= tt[i];
                        }
                    } else {
                        /* a number */
                        int cnt = sp.IndexOfFirstNot('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.');
                        String sval = cnt < 0 ? sp : sp.Substring(0, cnt);
                        var val = double.Parse(sval, CultureInfo.InvariantCulture);
                        if (val != 0 || sp.StartsWith("0"))
                            fac *= val;
                    }
                    if (sp != null) {
                        sp = sp.TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.');
                    }
                }
                z++;
            }
            //return retc;	/* there have been additional terms */
        }
    }

}
