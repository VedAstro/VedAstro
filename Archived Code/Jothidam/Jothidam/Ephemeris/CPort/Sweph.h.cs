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

/************************************************************
   $Header: /home/dieter/sweph/RCS/sweph.h,v 1.74 2008/06/16 10:07:20 dieter Exp $
   definitions and constants SWISSEPH

  Authors: Dieter Koch and Alois Treindl, Astrodienst Zurich

************************************************************/
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
    using System.Linq;
    using System.Text;

    partial class Sweph
    {
        /*
         * move over from swephexp.h
         */

        public const String SE_VERSION = "2.04";

        /// <summary>
        /// 2000 January 1.5
        /// </summary>
        public const double J2000 = 2451545.0;
        /// <summary>
        /// 1950 January 0.923 
        /// </summary>
        public const double B1950 = 2433282.42345905;
        /// <summary>
        /// 1900 January 0.5
        /// </summary>
        public const double J1900 = 2415020.0;

        public const int MPC_CERES = 1;
        public const int MPC_PALLAS = 2;
        public const int MPC_JUNO = 3;
        public const int MPC_VESTA = 4;
        public const int MPC_CHIRON = 2060;
        public const int MPC_PHOLUS = 5145;

        public const string SE_NAME_SUN = "Sun";
        public const string SE_NAME_MOON = "Moon";
        public const string SE_NAME_MERCURY = "Mercury";
        public const string SE_NAME_VENUS = "Venus";
        public const string SE_NAME_MARS = "Mars";
        public const string SE_NAME_JUPITER = "Jupiter";
        public const string SE_NAME_SATURN = "Saturn";
        public const string SE_NAME_URANUS = "Uranus";
        public const string SE_NAME_NEPTUNE = "Neptune";
        public const string SE_NAME_PLUTO = "Pluto";
        public const string SE_NAME_MEAN_NODE = "mean Node";
        public const string SE_NAME_TRUE_NODE = "true Node";
        public const string SE_NAME_MEAN_APOG = "mean Apogee";
        public const string SE_NAME_OSCU_APOG = "osc. Apogee";
        public const string SE_NAME_INTP_APOG = "intp. Apogee";
        public const string SE_NAME_INTP_PERG = "intp. Perigee";
        public const string SE_NAME_EARTH = "Earth";
        public const string SE_NAME_CERES = "Ceres";
        public const string SE_NAME_PALLAS = "Pallas";
        public const string SE_NAME_JUNO = "Juno";
        public const string SE_NAME_VESTA = "Vesta";
        public const string SE_NAME_CHIRON = "Chiron";
        public const string SE_NAME_PHOLUS = "Pholus";


        public const string SE_NAME_CUPIDO = "Cupido";
        public const string SE_NAME_HADES = "Hades";
        public const string SE_NAME_ZEUS = "Zeus";
        public const string SE_NAME_KRONOS = "Kronos";
        public const string SE_NAME_APOLLON = "Apollon";
        public const string SE_NAME_ADMETOS = "Admetos";
        public const string SE_NAME_VULKANUS = "Vulkanus";
        public const string SE_NAME_POSEIDON = "Poseidon";
        public const string SE_NAME_ISIS = "Isis";
        public const string SE_NAME_NIBIRU = "Nibiru";
        public const string SE_NAME_HARRINGTON = "Harrington";
        public const string SE_NAME_NEPTUNE_LEVERRIER = "Leverrier";
        public const string SE_NAME_NEPTUNE_ADAMS = "Adams";
        public const string SE_NAME_PLUTO_LOWELL = "Lowell";
        public const string SE_NAME_PLUTO_PICKERING = "Pickering";
        public const string SE_NAME_VULCAN = "Vulcan";
        public const string SE_NAME_WHITE_MOON = "White Moon";

        ///*
        // * earlier content
        // */

        //#define PI             = M_PI;	/* 3.14159265358979323846, math.h */
        public const double PI = Math.PI;
        public const double TWOPI = (2.0 * PI);

        public const int ENDMARK = -99;

        public const int SEI_EPSILON = -2;
        public const int SEI_NUTATION = -1;
        public const int SEI_EMB = 0;
        public const int SEI_EARTH = 0;
        public const int SEI_SUN = 0;
        public const int SEI_MOON = 1;
        public const int SEI_MERCURY = 2;
        public const int SEI_VENUS = 3;
        public const int SEI_MARS = 4;
        public const int SEI_JUPITER = 5;
        public const int SEI_SATURN = 6;
        public const int SEI_URANUS = 7;
        public const int SEI_NEPTUNE = 8;
        public const int SEI_PLUTO = 9;
        public const int SEI_SUNBARY = 10;	/* barycentric sun */
        public const int SEI_ANYBODY = 11;	/* any asteroid */
        public const int SEI_CHIRON = 12;
        public const int SEI_PHOLUS = 13;
        public const int SEI_CERES = 14;
        public const int SEI_PALLAS = 15;
        public const int SEI_JUNO = 16;
        public const int SEI_VESTA = 17;

        public const int SEI_NPLANETS = 18;

        public const int SEI_MEAN_NODE = 0;
        public const int SEI_TRUE_NODE = 1;
        public const int SEI_MEAN_APOG = 2;
        public const int SEI_OSCU_APOG = 3;
        public const int SEI_INTP_APOG = 4;
        public const int SEI_INTP_PERG = 5;

        public const int SEI_NNODE_ETC = 6;

        public const int SEI_FLG_HELIO = 1;
        public const int SEI_FLG_ROTATE = 2;
        public const int SEI_FLG_ELLIPSE = 4;
        public const int SEI_FLG_EMBHEL = 8;   	/* TRUE, if heliocentric earth is given
                                                 * instead of barycentric sun 
                                                 * i.e. bary sun is computed from 
                                                 * barycentric and heliocentric earth */

        public const int SEI_FILE_PLANET = 0;
        public const int SEI_FILE_MOON = 1;
        public const int SEI_FILE_MAIN_AST = 2;
        public const int SEI_FILE_ANY_AST = 3;
        public const int SEI_FILE_FIXSTAR = 4;

        //#if 0
        //#define SEI_FILE_TEST_ENDIAN     (97L * 65536L + 98L * 256L + 99L) /*abc*/
        //#endif
        public const long SEI_FILE_TEST_ENDIAN = (0x616263L); 	/* abc*/
        public const int SEI_FILE_BIGENDIAN = 0;
        public const int SEI_FILE_NOREORD = 0;
        public const int SEI_FILE_LITENDIAN = 1;
        public const int SEI_FILE_REORD = 2;

        public const int SEI_FILE_NMAXPLAN = 50;
        public const int SEI_FILE_EFPOSBEGIN = 500;

        public const string SE_FILE_SUFFIX = "se1";

        public const int SEI_NEPHFILES = 7;
        public const int SEI_CURR_FPOS = -1;
        public const int SEI_NMODELS = 20;

        public const double SEI_ECL_GEOALT_MAX = 25000.0;
        public const double SEI_ECL_GEOALT_MIN = (-500.0);

        /* Chiron's orbit becomes chaotic 
         * before 720 AD and after 4606 AD, because of close encounters
         * with Saturn. Accepting a maximum error of 5 degrees, 
         * the ephemeris is good between the following dates:
         */
        /*#define CHIRON_START    1958470.5  	* 1.1.650 old limit until v. 2.00 */
        public const double CHIRON_START = 1967601.5;  	/* 1.1.675 */
        public const double CHIRON_END = 3419437.5;  	/* 1.1.4650 */

        /* Pholus's orbit is unstable as well, because he sometimes
         * approaches Saturn.
         * Accepting a maximum error of 5 degrees,
         * the ephemeris is good after the following date:
         */
        /* #define PHOLUS_START    314845.5  	* 1.1.-3850  old limit until v. 2.00 */
        public const double PHOLUS_START    =640648.5;	/* 1.1.-2958 jul */
        public const double PHOLUS_END = 4390617.5;  	/* 1.1.7309 */

        public const double MOSHPLEPH_START = 625000.5;
        public const double MOSHPLEPH_END = 2818000.5;
        public const double MOSHLUEPH_START = 625000.5;
        public const double MOSHLUEPH_END = 2818000.5;
        /*#define MOSHNDEPH_START	=-254900.5; */
        /* 14 Feb -5410 00:00 ET jul.cal.*/
        /*#define MOSHNDEPH_END  	=3697000.5; */
        /* 11 Dec 5409 00:00 ET, greg. cal */
        public const double MOSHNDEPH_START = -3100015.5;	/* 15 Aug -13200 00:00 ET jul.cal.*/
        public const double MOSHNDEPH_END = 8000016.5;       /* 15 Mar 17191 00:00 ET, greg. cal */
        /*
        #define MOSHPLEPH_START	 =-225000.5;
        #define MOSHPLEPH_END  	=3600000.5;
        #define MOSHLUEPH_START	= -225000.5;
        #define MOSHLUEPH_END  	=3600000.5;
        */
        public const double JPL_DE431_START = -3027215.5;
        public const double JPL_DE431_END = 7930192.5;

        //#if FALSE	/*	Alois commented out, not used anywhere  */
        //#define JPLEPH_START	 625307.5;	/* about -3000 (DE406) */
        //#define JPLEPH_END	2816848.5;	/* about  3000 (DE406) */
        //#define SWIEPH_START	 625614.927151;
        //#define SWIEPH_END	2813641.5;
        //#define ALLEPH_START	MOSHPLEPH_START;
        //#define ALLEPH_END	MOSHPLEPH_END;
        //#define BEG_YEAR       (-3000);
        //#define END_YEAR       3000;
        //#endif

        public const int MAXORD = 40;

        public const double NCTIES = 6.0;     /* number of centuries per eph. file */

        public const int OK = (0);
        public const int ERR = (-1);
        public const int NOT_AVAILABLE = (-2);
        public const int BEYOND_EPH_LIMITS = (-3);

        public const int J_TO_J2000 = 1;
        public const int J2000_TO_J = -1;

        /* we always use Astronomical Almanac constants, if available */
        public const double MOON_MEAN_DIST = 384400000.0;		/* in m, AA 1996, F2 */
        public const double MOON_MEAN_INCL = 5.1453964;		/* AA 1996, D2 */
        public const double MOON_MEAN_ECC = 0.054900489;		/* AA 1996, F2 */
        /* #define SUN_EARTH_MRAT=  328900.561400;           Su/(Ea+Mo) AA 2006 K7 */
        public const double SUN_EARTH_MRAT = 332946.050895;           /* Su / (Ea only) AA 2006 K7 */
        public const double EARTH_MOON_MRAT = (1 / 0.0123000383);	/* AA 2006, K7 */
        //#if 0
        //#define EARTH_MOON_MRAT 81.30056907419062	/* de431 */
        //#endif
        //#if 0
        //#define EARTH_MOON_MRAT =81.30056		/* de406 */
        //#endif
        public const double AUNIT = 1.49597870691e+11;  	/* au in meters, AA 2006 K6 */
        public const double CLIGHT = 2.99792458e+8;   	/* m/s, AA 1996 K6 */
        //#if 0
        //#define HELGRAVCONST   = 1.32712438e+20;		/* G * M(sun), m^3/sec^2, AA 1996 K6 */
        //#endif
        public const double HELGRAVCONST = 1.32712440017987e+20;	/* G * M(sun), m^3/sec^2, AA 2006 K6 */
        public const double GEOGCONST = 3.98600448e+14; 		/* G * M(earth) m^3/sec^2, AA 1996 K6 */
        public const double KGAUSS = 0.01720209895;		/* Gaussian gravitational constant K6 */
        public const double SUN_RADIUS = (959.63 / 3600 * SwissEph.DEGTORAD);  /*  Meeus germ. p 391 */
        public const double EARTH_RADIUS = 6378136.6;		/* AA 2006 K6 */
        /*#define EARTH_OBLATENESS= (1.0/ 298.257223563)	 * AA 1998 K13 */
        public const double EARTH_OBLATENESS = (1.0 / 298.25642);	/* AA 2006 K6 */
        public const double EARTH_ROT_SPEED = (7.2921151467e-5 * 86400); /* in rad/day, expl. suppl., p 162 */

        public const double LIGHTTIME_AUNIT = (499.0047838061 / 3600 / 24); 	/* 8.3167 minutes (days), AA 2006 K6 */

        /* node of ecliptic measured on ecliptic 2000 */
        public const double SSY_PLANE_NODE_E2000 = (107.582569 * SwissEph.DEGTORAD);
        /* node of ecliptic measured on solar system rotation plane */
        public const double SSY_PLANE_NODE = (107.58883388 * SwissEph.DEGTORAD);
        /* inclination of ecliptic against solar system rotation plane */
        public const double SSY_PLANE_INCL = (1.578701 * SwissEph.DEGTORAD);

        public const double KM_S_TO_AU_CTY = 21.095;			/* km/s to AU/century */
        public const double MOON_SPEED_INTV = 0.00005; 		/* 4.32 seconds (in days) */
        public const double PLAN_SPEED_INTV = 0.0001; 	        /* 8.64 seconds (in days) */
        public const double MEAN_NODE_SPEED_INTV = 0.001;
        public const double NODE_CALC_INTV = 0.0001;
        public const double NODE_CALC_INTV_MOSH = 0.1;
        public const double NUT_SPEED_INTV = 0.0001;
        public const double DEFL_SPEED_INTV = 0.0000005;

        public const double SE_LAPSE_RATE = 0.0065;  /* deg K / m, for refraction */

        public double square_sum(CPointer<double> x) { return (x[0] * x[0] + x[1] * x[1] + x[2] * x[2]); }
        public double dot_prod(CPointer<double> x, CPointer<double> y) { return (x[0] * y[0] + x[1] * y[1] + x[2] * y[2]); }

        public int[] PNOINT2JPL = new int[] { SweJPL.J_EARTH, SweJPL.J_MOON, SweJPL.J_MERCURY, SweJPL.J_VENUS, SweJPL.J_MARS, SweJPL.J_JUPITER, 
            SweJPL.J_SATURN, SweJPL.J_URANUS, SweJPL.J_NEPTUNE, SweJPL.J_PLUTO, SweJPL.J_SUN, };

        /* planetary radii in meters */
        public const int NDIAM = (SwissEph.SE_VESTA + 1);
        public static double[] pla_diam = new double[NDIAM]{1392000000.0, /* Sun */
                                   3476300.0, /* Moon */
                                   2439000.0 * 2, /* Mercury */
                                   6052000.0 * 2, /* Venus */
                                   3397200.0 * 2, /* Mars */
                                  71398000.0 * 2, /* Jupiter */
                                  60000000.0 * 2, /* Saturn */
                                  25400000.0 * 2, /* Uranus */
                                  24300000.0 * 2, /* Neptune */
                                   2500000.0 * 2, /* Pluto */
                                   0, 0, 0, 0,    /* nodes and apogees */
                                   6378140.0 * 2, /* Earth */
                                         0.0, /* Chiron */
                                         0.0, /* Pholus */
                                    913000.0, /* Ceres */
                                    523000.0, /* Pallas */
                                    244000.0, /* Juno */
                                    501000.0, /* Vesta */
        };


        /* Ayanamsas 
         * For each ayanamsa, there are two values:
         * t0       epoch of ayanamsa, TDT (ET)
         * ayan_t0  ayanamsa value at epoch
         */
        public struct aya_init { public double t0; public double ayan_t0;};
        public static aya_init[] ayanamsa = new aya_init[]{
            new aya_init{t0=2433282.5, ayan_t0=24.042044444},	/* 0: Fagan/Bradley (Default) */
            /*{J1900, 360 - 337.53953},   * 1: Lahiri (Robert Hand) */
            new aya_init{t0=2435553.5, ayan_t0=23.250182778 - 0.004660222},   /* 1: Lahiri (derived from:
                       * Indian Astronomical Ephemeris 1989, p. 556;
                       * the subtracted value is nutation) */
            new aya_init{t0=J1900, ayan_t0=360 - 333.58695},   /* 2: De Luce (Robert Hand) */
            new aya_init{t0=J1900, ayan_t0=360 - 338.98556},   /* 3: Raman (Robert Hand) */
            new aya_init{t0=J1900, ayan_t0=360 - 341.33904},   /* 4: Ushashashi (Robert Hand) */
            new aya_init{t0=J1900, ayan_t0=360 - 337.636111},  /* 5: Krishnamurti (Robert Hand) */
            new aya_init{t0=J1900, ayan_t0=360 - 333.0369024}, /* 6: Djwhal Khool; (Graham Dawson)  
                                         *    Aquarius entered on 1 July 2117 */
            new aya_init{t0=J1900, ayan_t0=360 - 338.917778},  /* 7: Yukteshwar; (David Cochrane) */
            new aya_init{t0=J1900, ayan_t0=360 - 338.634444},  /* 8: JN Bhasin; (David Cochrane) */
            new aya_init{t0=1684532.5, ayan_t0=-3.36667},      /* 9: Babylonian, Kugler 1 */
            new aya_init{t0=1684532.5, ayan_t0=-4.76667},      /*10: Babylonian, Kugler 2 */
            new aya_init{t0=1684532.5, ayan_t0=-5.61667},      /*11: Babylonian, Kugler 3 */
            new aya_init{t0=1684532.5, ayan_t0=-4.56667},      /*12: Babylonian, Huber */
            new aya_init{t0=1673941, ayan_t0=-5.079167},       /*13: Babylonian, Mercier;
                                         *    eta Piscium culminates with zero point */
            new aya_init{t0=1684532.5, ayan_t0=-4.44088389},   /*14: t0 is defined by Aldebaran at 15 Taurus */
            new aya_init{t0=1674484, ayan_t0=-9.33333},        /*15: Hipparchos */
            new aya_init{t0=1927135.8747793, ayan_t0=0},       /*16: Sassanian */
            /*{1746443.513, 0},          *17: Galactic Center at 0 Sagittarius */
            new aya_init{t0=1746447.518, ayan_t0=0},           /*17: Galactic Center at 0 Sagittarius */
            new aya_init{t0=J2000, ayan_t0=0},	                /*18: J2000 */
            new aya_init{t0=J1900, ayan_t0=0},	                /*19: J1900 */
            new aya_init{t0=B1950, ayan_t0=0},	                /*20: B1950 */
            new aya_init{t0=1903396.8128654, ayan_t0=0},	/*21: Suryasiddhanta, assuming
                                              ingress of mean Sun into Aries at point
                              of mean equinox of date on
                              21.3.499, noon, Ujjain (75.7684565 E)
                                              = 7:30:31.57 UT */
            new aya_init{t0=1903396.8128654,ayan_t0=-0.21463395},/*22: Suryasiddhanta, assuming
                                              ingress of mean Sun into Aries at
                              true position of mean Sun at same epoch */
            new aya_init{t0=1903396.7895321, ayan_t0=0},	/*23: Aryabhata, same date, but UT 6:56:55.57
                                              analogous 21 */
            new aya_init{t0=1903396.7895321,ayan_t0=-0.23763238},/*24: Aryabhata, analogous 22 */
            new aya_init{t0=1903396.8128654,ayan_t0=-0.79167046},/*25: SS, Revati/zePsc at polar long. 359°50'*/
            new aya_init{t0=1903396.8128654, ayan_t0=2.11070444},/*26: SS, Citra/Spica at polar long. 180° */
            new aya_init{t0=0, ayan_t0=0},	                /*27: True Citra (Spica exactly at 0 Libra) */
            new aya_init{t0=0, ayan_t0=0},	                /*28: True Revati (zeta Psc exactly at 0 Aries) */
            new aya_init{t0=0, ayan_t0=0},			/*29: True Pushya (delta Cnc exactly a 16 Cancer */
            new aya_init{t0=0, ayan_t0=0},	                /*30: - */
            };

        /*
         * stuff exported from swemplan.c and swemmoon.c 
         * and constants used inside these functions.
        ************************************************************/

        public const double STR = 4.8481368110953599359e-6; /* radians per arc second */

        ///* moon, s. moshmoon.c */
        //extern int swi_mean_node(double jd, double *x, char *serr);
        //extern int swi_mean_apog(double jd, double *x, char *serr);
        //extern int swi_moshmoon(double tjd, AS_BOOL do_save, double *xpm, char *serr) ;
        //extern int swi_moshmoon2(double jd, double *x);
        //extern int swi_intp_apsides(double J, double *pol, int ipli);

        ///* planets, s. moshplan.c */
        //extern int swi_moshplan(double tjd, int ipli, AS_BOOL do_save, double *xpret, double *xeret, char *serr);
        //extern int swi_moshplan2(double J, int iplm, double *pobj);
        //extern int swi_osc_el_plan(double tjd, double *xp, int ipl, int ipli, double *xearth, double *xsun, char *serr);
        //extern int32 swi_init_swed_if_start(void);
        //extern int32 swi_set_tid_acc(double tjd_ut, int32 iflag, int32 denum, char *serr);
        //extern int32 swi_get_tid_acc(double tjd_ut, int32 iflag, int32 denum, int32 *denumret, double *tid_acc, char *serr);


        /* nutation */
        public class nut
        {
            public nut() {
                nutlo = new double[2];
                matrix = new double[3, 3];
            }
            public double tnut;
            public double[] nutlo { get; private set; }	/* nutation in longitude and obliquity */
            public double snut, cnut;	/* sine and cosine of nutation in obliquity */
            public double[,] matrix { get; private set; }
        }

        public class plantbl
        {
            public plantbl() {
                max_harmonic = new sbyte[9];
            }
            public plantbl(sbyte[] m, sbyte mp, sbyte[] at, double[] l1, double[] l2, double[] l3, double d) {
                max_harmonic = m;
                max_power_of_t = mp;
                arg_tbl = at;
                lon_tbl = l1;
                lat_tbl = l2;
                rad_tbl = l3;
                distance = d;
            }
            public sbyte[] max_harmonic { get; private set; }
            public sbyte max_power_of_t;
            public sbyte[] arg_tbl;
            public double[] lon_tbl;
            public double[] lat_tbl;
            public double[] rad_tbl;
            public double distance;
        };

        public class file_data
        {
            public string fnam = null;	/* ephemeris file name */
            public int fversion;		/* version number of file */
            public string astnam;	/* asteroid name, if asteroid file */
            public Int32 sweph_denum;     /* DE number of JPL ephemeris, which this file
             * is derived from. */
            public CFile fptr;		/* ephemeris file pointer */
            public double tfstart;       /* file may be used from this date */
            public double tfend;         /*      through this date          */
            public Int32 iflg; 		/* byte reorder flag and little/bigendian flag */
            public short npl;		/* how many planets in file */
            public int[] ipl = new int[SEI_FILE_NMAXPLAN];	/* planet numbers */
        }

        public struct gen_const
        {
            public double clight,
               aunit,
               helgravconst,
               ratme,
               sunradius;
        }

        public class save_positions
        {
            public save_positions() {
                xsaves = new double[24];
            }
            public int ipl;
            public double tsave;
            public Int32 iflgsave;
            /* position at t = tsave,
             * in ecliptic polar (offset 0),
             *    ecliptic cartesian (offset 6), 
             *    equatorial polar (offset 12),
             *    and equatorial cartesian coordinates (offset 18).
             * 6 doubles each for position and speed coordinates.
             */
            public double[] xsaves { get; private set; }
        };

        public class node_data
        {
            public node_data() {
                x = new double[6];
                xreturn = new double[24];
            }
            /* result of most recent data evaluation for this body: */
            public double teval = 0;		/* time for which last computation was made */
            public Int32 iephe = 0;            /* which ephemeris was used */
            public double[] x { get; private set; }		/* position and speed vectors equatorial J2000 */
            public Int32 xflgs = 0;		/* hel., light-time, aberr., prec. flags etc. */
            public double[] xreturn { get; private set; }   /* return positions: 
             * xreturn+0	ecliptic polar coordinates
             * xreturn+6	ecliptic cartesian coordinates
             * xreturn+12	equatorial polar coordinates
             * xreturn+18	equatorial cartesian coordinates
             */
        }

        public class topo_data
        {
            public topo_data() {
                xobs = new double[6];
            }
            public double geolon, geolat, geoalt;
            public double teval;
            public double tjd_ut;
            public double[] xobs { get; private set; }
        }

        public struct sid_data
        {
            public Int32 sid_mode;
            public double ayan_t0;
            public double t0;
        };

        /* dpsi and deps loaded for 100 years after 1962 */
        static int SWE_DATA_DPSI_DEPS = 36525;

        /* if this is changed, then also update initialisation in sweph.c */
        public class swe_data
        {
            public swe_data()
            {
                Reset(true);
            }
            public void Reset(bool full)
            {
                if (full)
                {
                    fidat = Enumerable.Range(0, SEI_NEPHFILES).Select(i => new file_data()).ToArray();
                    savedat = Enumerable.Range(0, SwissEph.SE_NPLANETS + 1).Select(i => new save_positions()).ToArray();
                    pldat = Enumerable.Range(0, SEI_NPLANETS).Select(i => new plan_data()).ToArray();
                    nddat = Enumerable.Range(0, SEI_NNODE_ETC).Select(i => new plan_data()).ToArray();
                    topd = new topo_data();
                    //dpsi = new double[36525];
                    //deps = new double[36525];
                    astro_models = new Int32[Sweph.SEI_NMODELS];
                }
                oec = new epsilon();
                oec2000 = new epsilon();
                nut = new nut();
                nut2000 = new nut();
                nutv = new nut();
            }
            public bool ephe_path_is_set;
            public bool /*short*/ jpl_file_is_open;
            public CFile fixfp;/* fixed stars file pointer */
            public string ephepath = String.Empty;
            public string jplfnam = String.Empty;
            public Int32 jpldenum;
            public Int32 last_epheflag;
            public bool geopos_is_set;
            public bool ayana_is_set;
            public bool is_old_starfile;
            public double eop_tjd_beg = 0;
            public double eop_tjd_beg_horizons = 0;
            public double eop_tjd_end = 0;
            public double eop_tjd_end_add = 0;
            public int eop_dpsi_loaded = 0;
            public double tid_acc;
            public bool is_tid_acc_manual;
            public bool init_dt_done;
            public file_data[] fidat { get; private set; }
            public gen_const gcdat;
            public plan_data[] pldat { get; private set; }
            //#if 0
            //  struct node_data nddat[SEI_NNODE_ETC];
            //#else
            public plan_data[] nddat { get; private set; }
            //#endif
            public save_positions[] savedat { get; private set; }
            public epsilon oec { get; internal set; }
            public epsilon oec2000 { get; internal set; }
            public nut nut { get; internal set; }
            public nut nut2000 { get; internal set; }
            public nut nutv { get; internal set; }
            public topo_data topd { get; internal set; }
            public sid_data sidd;
            public string astelem = string.Empty;
            public double ast_G;
            public double ast_H;
            public double ast_diam;
            public int i_saved_planet_name;
            public string saved_planet_name;
            //public double[] dpsi { get; private set; }  /* works for 100 years after 1962 */
            //public double[] deps { get; private set; }
            public double[] dpsi;
            public double[] deps;
            public Int32[] astro_models { get; internal set; }
            public Int32 timeout;
        }

        //extern TLS struct swe_data FAR swed;

        /* obliquity of ecliptic */
        public class epsilon
        {
            public double teps, eps, seps, ceps; 	/* jd, eps, sin(eps), cos(eps) */
        };

        public class plan_data
        {
            public plan_data() {
                x = new double[6];
                xreturn = new double[24];
            }
            /* the following data are read from file only once, immediately after 
             * file has been opened */
            public int ibdy;		/* internal body number */
            public Int32 iflg;		/* contains several bit flags describing the data:	
             * SEI_FLG_HELIO: true if helio, false if bary
             * SEI_FLG_ROTATE: TRUE if coefficients are referred 
             *      to coordinate system of orbital plane 
             * SEI_FLG_ELLIPSE: TRUE if reference ellipse */
            public int ncoe;		/* # of coefficients of ephemeris polynomial,
               is polynomial order + 1  */
            /* where is the segment index on the file */
            public Int32 lndx0;   	/* file position of begin of planet's index */
            public Int32 nndx;		/* number of index entries on file: computed */
            public double tfstart;	/* file contains ephemeris for tfstart thru tfend */
            public double tfend;         /*      for this particular planet !!!            */
            public double dseg;		/* segment size (days covered by a polynomial)  */
            /* orbital elements: */
            public double telem;		/* epoch of elements */
            public double prot;
            public double qrot;
            public double dprot;
            public double dqrot;
            public double rmax;		/* normalisation factor of cheby coefficients */
            /* in addition, if reference ellipse is used: */
            public double peri;
            public double dperi;
            public double[] refep;	/* pointer to cheby coeffs of reference ellipse,
             * size of data is 2 x ncoe */
            /* unpacked segment information, only updated when a segment is read: */
            public double tseg0, tseg1;	/* start and end jd of current segment */
            public double[] segp;         /* pointer to unpacked cheby coeffs of segment;
             * the size is 3 x ncoe */
            public int neval;		/* how many coefficients to evaluate. this may
             * be less than ncoe */
            /* result of most recent data evaluation for this body: */
            public double teval;		/* time for which previous computation was made */
            public Int32 iephe;            /* which ephemeris was used */
            public double[] x { get; private set; }		/* position and speed vectors equatorial J2000 */
            public Int32 xflgs;		/* hel., light-time, aberr., prec. flags etc. */
            public double[] xreturn { get; private set; }   /* return positions:
             * xreturn+0	ecliptic polar coordinates
             * xreturn+6	ecliptic cartesian coordinates
             * xreturn+12	equatorial polar coordinates
             * xreturn+18	equatorial cartesian coordinates
             */
        }

    }

}
