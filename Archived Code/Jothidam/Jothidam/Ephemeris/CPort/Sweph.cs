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
   $Header: /home/dieter/sweph/RCS/sweph.c,v 1.76 2009/07/10 14:08:53 dieter Exp $

   Ephemeris computations

  Authors: Dieter Koch and Alois Treindl, Astrodienst Zurich

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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    partial class Sweph : BaseCPort
    {
        public Sweph(SwissEph se)
            : base(se) {
            pnoint2jpl = PNOINT2JPL.ToArray();
        }

        const int IS_PLANET = 0;
        const int IS_MOON = 1;
        const int IS_ANY_BODY = 2;
        const int IS_MAIN_ASTEROID = 3;

        const bool DO_SAVE = true;
        const bool NO_SAVE = false;

        bool SID_TNODE_FROM_ECL_T0 = false;

        //#define SEFLG_EPHMASK	(SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH)

        struct meff_ele
        {
            public meff_ele(double ar, double am) {
                r = ar;
                m = am;
            }
            public double r, m;
        };

        /****************
         * global stuff *
         ****************/
        //struct swe_data FAR swed = {FALSE,	/* ephe_path_is_set = FALSE */
        //                            FALSE,	/* jpl_file_is_open = FALSE */
        //                            NULL,	/* fixfp, fixed stars file pointer */
        //#if 0
        //                SE_EPHE_PATH,	/* ephepath, ephemeris path */
        //                SE_FNAME_DFT,	/* jplfnam, JPL file name, default */
        //#else
        //                "",		/* ephepath, ephemeris path */
        //                "",		/* jplfnam, JPL file name, default */
        //#endif
        //                0,		/* jpldenum */
		//	              0,          /* last_epheflag */
        //                FALSE,	/* geopos_is_set, for topocentric */
        //                FALSE,	/* ayana_is_set, ayanamsa is set */
        //                FALSE,	/* is_old_starfile, fixstars.cat is used (default is sefstars.txt) */
                //0.0, 0.0, 0.0, 0.0, /* eop_tjd_... */
                //0,		/* eop_dpsi_loaded */
                //0,          /* eop_dpsi_loaded */
                //0.0,     /* tid_acc */
                //FALSE,    /* is_tid_acc_manual */
                //FALSE,    /* init_dt_done */
        //                };
        internal swe_data swed = new swe_data() {
            ephe_path_is_set = false,
            jpl_file_is_open = false,
            fixfp = null,
//#if 0
            ephepath = SwissEph.SE_EPHE_PATH,
            jplfnam = SwissEph.SE_FNAME_DFT,
//#else
            //ephepath = "",
            //jplfnam = "",
//#endif
            jpldenum = 0,
            last_epheflag = 0,          /* last_epheflag */
            geopos_is_set = false,
            ayana_is_set = false,
            is_old_starfile = false,
            eop_tjd_beg = 0.0,
            eop_tjd_beg_horizons = 0.0,
            eop_tjd_end = 0.0,
            eop_tjd_end_add = 0.0,          /* eop_tjd_... */
            eop_dpsi_loaded = 0,     		/* eop_dpsi_loaded */
            tid_acc = 0.0,
            is_tid_acc_manual = false,
            init_dt_done = false
        };

        /*************
         * constants *
         *************/

        static readonly string[] ayanamsa_name = new string[] {
           "Fagan/Bradley",
           "Lahiri",
           "De Luce",
           "Raman",
           "Ushashashi",
           "Krishnamurti",
           "Djwhal Khul",
           "Yukteshwar",
           "J.N. Bhasin",
           "Babylonian/Kugler 1",
           "Babylonian/Kugler 2",
           "Babylonian/Kugler 3",
           "Babylonian/Huber",
           "Babylonian/Eta Piscium",
           "Babylonian/Aldebaran = 15 Tau",
           "Hipparchos",
           "Sassanian",
           "Galact. Center = 0 Sag",
           "J2000",
           "J1900",
           "B1950",
           "Suryasiddhanta",
           "Suryasiddhanta, mean Sun",
           "Aryabhata",
           "Aryabhata, mean Sun",
           "SS Revati",
           "SS Citra",
           "True Citra",
           "True Revati",
           "True Pushya",
        };
        //int[] pnoint2jpl = PNOINT2JPL;
        int[] pnoint2jpl = null;        // This field is assigned in constructor

        int[] pnoext2int = new int[] { SEI_SUN, SEI_MOON, SEI_MERCURY, SEI_VENUS, SEI_MARS, SEI_JUPITER, SEI_SATURN, SEI_URANUS, SEI_NEPTUNE, SEI_PLUTO, 0, 0, 0, 0, SEI_EARTH, SEI_CHIRON, SEI_PHOLUS, SEI_CERES, SEI_PALLAS, SEI_JUNO, SEI_VESTA, };

        //static int32 swecalc(double tjd, int ipl, int32 iflag, double *x, char *serr);
        //static int do_fread(void *targ, int size, int count, int corrsize, 
        //            FILE *fp, int32 fpos, int freord, int fendian, int ifno, 
        //            char *serr);
        //static int get_new_segment(double tjd, int ipli, int ifno, char *serr);
        //static int main_planet(double tjd, int ipli, int32 epheflag, int32 iflag,
        //               char *serr);
        //static int main_planet_bary(double tjd, int ipli, int32 epheflag, int32 iflag, 
        //        AS_BOOL do_save, 
        //        double *xp, double *xe, double *xs, double *xm, 
        //        char *serr);
        //static int sweplan(double tjd, int ipli, int ifno, int32 iflag, AS_BOOL do_save, 
        //           double *xp, double *xpe, double *xps, double *xpm,
        //           char *serr);
        //static int swemoon(double tjd, int32 iflag, AS_BOOL do_save, double *xp, char *serr);
        //static int sweph(double tjd, int ipli, int ifno, int32 iflag, double *xsunb, AS_BOOL do_save, 
        //        double *xp, char *serr);
        //static int jplplan(double tjd, int ipli, int32 iflag, AS_BOOL do_save,
        //           double *xp, double *xpe, double *xps, char *serr);
        //static void rot_back(int ipl);
        //static int read_const(int ifno, char *serr);
        //static void embofs(double *xemb, double *xmoon);
        //static int app_pos_etc_plan(int ipli, int32 iflag, char *serr);
        //static int app_pos_etc_plan_osc(int ipl, int ipli, int32 iflag, char *serr);
        //static int app_pos_etc_sun(int32 iflag, char *serr);
        //static int app_pos_etc_moon(int32 iflag, char *serr);
        //static int app_pos_etc_sbar(int32 iflag, char *serr);
        //extern int swi_plan_for_osc_elem(int32 iflag, double tjd, double *xx);
        //static void swi_close_keep_topo_etc(void); 
        //static int app_pos_etc_mean(int ipl, int32 iflag, char *serr);
        //static void nut_matrix(struct nut *nu, struct epsilon *oec); 
        //static void calc_epsilon(double tjd, int32 iflag, struct epsilon *e);
        //static int lunar_osc_elem(double tjd, int ipl, int32 iflag, char *serr);
        //static int intp_apsides(double tjd, int ipl, int32 iflag, char *serr); 
        //static double meff(double r);
        //static void denormalize_positions(double *x0, double *x1, double *x2);
        //static void calc_speed(double *x0, double *x1, double *x2, double dt);
        //static int32 plaus_iflag(int32 iflag, int32 ipl, double tjd, char *serr);
        //static int app_pos_rest(struct plan_data *pdp, int32 iflag, 
        //    double *xx, double *x2000, struct epsilon *oe, char *serr);
        //static int open_jpl_file(double *ss, char *fname, char *fpath, char *serr);
        //static void free_planets(void);

        //#ifdef TRACE
        //static void trace_swe_calc(int param, double tjd, int ipl, int32 iflag, double *xx, char *serr);
        //static void trace_swe_fixstar(int swtch, char *star, double tjd, int32 iflag, double *xx, char *serr);
        //static void trace_swe_get_planet_name(int swtch, int ipl, char *s);
        //#endif

        public string swe_version() {
            return SE_VERSION;
        }

        /* The routine called by the user.
         * It checks whether a position for the same planet, the same t, and the
         * same flag bits has already been computed. 
         * If yes, this position is returned. Otherwise it is computed.
         * -> If the SEFLG_SPEED flag has been specified, the speed will be returned
         * at offset 3 of position array x[]. Its precision is probably better 
         * than 0.002"/day.
         * -> If the SEFLG_SPEED3 flag has been specified, the speed will be computed
         * from three positions. This speed is less accurate than SEFLG_SPEED,
         * i.e. better than 0.1"/day. And it is much slower. It is used for 
         * program tests only.
         * -> If no speed flag has been specified, no speed will be returned.
         */
        public Int32 swe_calc(double tjd, int ipl, Int32 iflag,
            double[] xx, ref string serr) {
            int i, j;
            Int32 iflgcoor;
            Int32 iflgsave = iflag;
            Int32 epheflag;
            save_positions sd;
            double[] x = new double[6], /*xs,*/ x0 = new double[24], x2 = new double[24];
            CPointer<Double> xs;
            double dt;
            serr = null;
#if TRACE
            //#ifdef FORCE_IFLAG
            //  /*
            //   * If this source file is compiled with /DFORCE_IFLAG or -DFORCE_IFLAG
            //   * and also with TRACE, then the actual value of iflag used in swe_calc()
            //   * can be manipulated from the outside of an application:
            //   * Create a text file 'force.flg' and put one text line into it
            //   * containing a number, e.g. 1024
            //   * This number will be or'ed into the iflag used by the caller of swe_calc()
            //   *
            //   * See the code below for the details.
            //   * This is not an important mechanism. We used it to debug an application
            //   * which showed strange behaviour, by compiling a special DLL with TRACE and
            //   * FORCE_IFLAG and then running the application with this DLL (we had no
            //   * source code of the application itself).
            //   */
            //  static TLS int force_flag = 0;
            //  static TLS int32 iflag_forced = 0;
            //  static TLS int force_flag_checked = 0;
            //  FILE *fp;
            //  char s[AS_MAXCH], *sp;
            //  memset(x, 0, sizeof(double) * 6);
            //  /* if the following file exists, flag is read from it and or'ed into iflag */
            //  if (!force_flag_checked) {
            //    if ((fp = fopen(fname_force_flg, BFILE_R_ACCESS)) != NULL) {
            //      force_flag = 1;
            //      fgets(s, AS_MAXCH, fp);
            //      if ((sp = strchr(s, '\n')) != NULL)
            //    *sp = '\0';
            //      iflag_forced = atol(s);
            //      fclose(fp);
            //    }
            //    force_flag_checked = 1;
            //  }
            //  if (force_flag)
            //    iflag |= iflag_forced;
            //#endif
            //  swi_open_trace(serr);
            trace_swe_calc(1, tjd, ipl, iflag, xx, null);
#endif //* TRACE */

            /* function calls for Pluto with asteroid number 134340
               * are treated as calls for Pluto as main body SE_PLUTO.
               * Reason: Our numerical integrator takes into account Pluto
               * perturbation and therefore crashes with body 134340 Pluto. */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            /* if ephemeris flag != ephemeris flag of last call,
             * we clear the save area, to prevent swecalc() using
             * previously computed data for current calculation.
             * except with ipl = SE_ECL_NUT which is not dependent 
             * on ephemeris, and except if change is from 
             * ephemeris = 0 to ephemeris = SEFLG_DEFAULTEPH
             * or vice-versa.
             */
            epheflag = iflag & SwissEph.SEFLG_EPHMASK;
              if ((epheflag & SwissEph.SEFLG_MOSEPH)!=0)
                epheflag = SwissEph.SEFLG_MOSEPH;
              else if ((epheflag & SwissEph.SEFLG_JPLEPH)!=0)
                epheflag = SwissEph.SEFLG_JPLEPH;
              else 
                epheflag = SwissEph.SEFLG_SWIEPH;
              if (swi_init_swed_if_start() == 1 && 0==(epheflag & SwissEph.SEFLG_MOSEPH) ) {
                serr= "Please call swe_set_ephe_path() or swe_set_jplfile() before calling swe_calc() or swe_calc_ut()";
              }
              if (swed.last_epheflag != epheflag)
              {
                  free_planets();
                  /* close and free ephemeris files */
                  if (ipl != SwissEph.SE_ECL_NUT)
                  {  /* because file will not be reopened with this ipl */
                      if (swed.jpl_file_is_open)
                      {
                          SE.SweJPL.swi_close_jpl_file();
                          swed.jpl_file_is_open = false;
                      }
                      for (i = 0; i < SEI_NEPHFILES; i++)
                      {
                          if (swed.fidat[i].fptr != null)
                              //fclose(swed.fidat[i].fptr);
                              swed.fidat[i].fptr.Dispose();
                          //memset((void *) &swed.fidat[i], 0, sizeof(struct file_data));
                          swed.fidat[i] = new file_data();
                      }
                      swed.last_epheflag = epheflag;
                  }
              }
            /* high precision speed prevails fast speed */
            if ((iflag & SwissEph.SEFLG_SPEED3) != 0 && (iflag & SwissEph.SEFLG_SPEED) != 0)
                iflag = iflag & ~SwissEph.SEFLG_SPEED3;
            /* cartesian flag excludes radians flag */
            if ((iflag & SwissEph.SEFLG_XYZ) != 0 && (iflag & SwissEph.SEFLG_RADIANS) != 0)
                iflag = iflag & ~SwissEph.SEFLG_RADIANS;
            /*  if (iflag & SEFLG_ICRS)
                iflag |= SEFLG_J2000;*/
            /* pointer to save area */
            if (ipl < SwissEph.SE_NPLANETS && ipl >= SwissEph.SE_SUN)
                sd = swed.savedat[ipl];
            else
                /* other bodies, e.g. asteroids called with ipl = SE_AST_OFFSET + MPC# */
                sd = swed.savedat[SwissEph.SE_NPLANETS];
            /* 
             * if position is available in save area, it is returned.
             * this is the case, if tjd = tsave and iflag = iflgsave.
             * coordinate flags can be neglected, because save area 
             * provides all coordinate types.
             * if ipl > SE_AST(EROID)_OFFSET, ipl must be checked, 
             * because all asteroids called by MPC number share the same
             * save area.
             */
            iflgcoor = SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_XYZ | SwissEph.SEFLG_RADIANS;
            if (sd.tsave == tjd && tjd != 0 && ipl == sd.ipl) {
                if ((sd.iflgsave & ~iflgcoor) == (iflag & ~iflgcoor))
                    goto end_swe_calc;
            }
            /* 
             * otherwise, new position must be computed 
             */
            if ((iflag & SwissEph.SEFLG_SPEED3) == 0) {
                /* 
                 * with high precision speed from one call of swecalc() 
                 * (FAST speed)
                 */
                sd.tsave = tjd;
                sd.ipl = ipl;
                if ((sd.iflgsave = swecalc(tjd, ipl, iflag, sd.xsaves, out serr)) == ERR)
                    goto return_error;
            } else {
                /* 
                 * with speed from three calls of swecalc(), slower and less accurate.
                 * (SLOW speed, for test only)
                 */
                sd.tsave = tjd;
                sd.ipl = ipl;
                switch (ipl) {
                    case SwissEph.SE_MOON:
                        dt = MOON_SPEED_INTV;
                        break;
                    case SwissEph.SE_OSCU_APOG:
                    case SwissEph.SE_TRUE_NODE:
                        /* this is the optimum dt with Moshier ephemeris, but not with
                         * JPL ephemeris or SWISSEPH. To avoid completely false speed
                         * in case that JPL is wanted but the program returns Moshier,
                         * we use Moshier optimum.
                         * For precise speed, use JPL and FAST speed computation,
                         */
                        dt = NODE_CALC_INTV_MOSH;
                        break;
                    default:
                        dt = PLAN_SPEED_INTV;
                        break;
                }
                if ((sd.iflgsave = swecalc(tjd - dt, ipl, iflag, x0, out serr)) == ERR)
                    goto return_error;
                if ((sd.iflgsave = swecalc(tjd + dt, ipl, iflag, x2, out serr)) == ERR)
                    goto return_error;
                if ((sd.iflgsave = swecalc(tjd, ipl, iflag, sd.xsaves, out serr)) == ERR)
                    goto return_error;
                denormalize_positions(x0, sd.xsaves, x2);
                calc_speed(x0, sd.xsaves, x2, dt);
            }
        end_swe_calc:
            if ((iflag & SwissEph.SEFLG_EQUATORIAL) != 0)
                xs = sd.xsaves.GetPointer(12);	/* equatorial coordinates */
            else
                xs = sd.xsaves;	/* ecliptic coordinates */
            if ((iflag & SwissEph.SEFLG_XYZ) != 0)
                xs = xs + 6;		/* cartesian coordinates */
            if (ipl == SwissEph.SE_ECL_NUT)
                i = 4;
            else
                i = 3;
            for (j = 0; j < i; j++)
                x[j] = xs + j;
            for (j = i; j < 6; j++)
                x[j] = 0;
            if ((iflag & (SwissEph.SEFLG_SPEED3 | SwissEph.SEFLG_SPEED)) != 0) {
                for (j = 3; j < 6; j++)
                    x[j] = (xs + j);
            }
            //#if 1
            if ((iflag & SwissEph.SEFLG_RADIANS) != 0) {
                if (ipl == SwissEph.SE_ECL_NUT) {
                    for (j = 0; j < 4; j++)
                        x[j] *= SwissEph.DEGTORAD;
                } else {
                    for (j = 0; j < 2; j++)
                        x[j] *= SwissEph.DEGTORAD;
                    if ((iflag & (SwissEph.SEFLG_SPEED3 | SwissEph.SEFLG_SPEED)) != 0) {
                        for (j = 3; j < 5; j++)
                            x[j] *= SwissEph.DEGTORAD;
                    }
                }
            }
            //#endif
            for (i = 0; i <= 5; i++)
                xx[i] = x[i];
            iflag = sd.iflgsave;
            /* if no ephemeris has been specified, do not return chosen ephemeris */
            if ((iflgsave & SwissEph.SEFLG_EPHMASK) == 0)
                iflag = iflag & ~SwissEph.SEFLG_DEFAULTEPH;
#if TRACE
            trace_swe_calc(2, tjd, ipl, iflag, xx, serr);
#endif
            return iflag;
        return_error:
            for (i = 0; i <= 5; i++)
                xx[i] = 0;
#if TRACE
            trace_swe_calc(2, tjd, ipl, iflag, xx, serr);
#endif
            return ERR;
        }

        public Int32 swe_calc_ut(double tjd_ut, Int32 ipl, Int32 iflag,
            double[] xx, ref string serr)
        {
            double deltat;
            Int32 retval = OK;
            String sdummy = null;
            Int32 epheflag = 0;
            iflag = plaus_iflag(iflag, ipl, tjd_ut, out serr);
            epheflag = iflag & SwissEph.SEFLG_EPHMASK;
            if (epheflag == 0)
            {
                epheflag = SwissEph.SEFLG_SWIEPH;
                iflag |= SwissEph.SEFLG_SWIEPH;
            }
            deltat = SE.swe_deltat_ex(tjd_ut, iflag, ref serr);
            retval = swe_calc(tjd_ut + deltat, ipl, iflag, xx, ref serr);
            /* if ephe required is not ephe returned, adjust delta t: */
            if ((retval & SwissEph.SEFLG_EPHMASK) != epheflag)
            {
                deltat = SE.swe_deltat_ex(tjd_ut, retval, ref sdummy);
                retval = swe_calc(tjd_ut + deltat, ipl, iflag, xx, ref sdummy);
            }
            return retval;
        }

        Int32 swecalc(double tjd, int ipl, Int32 iflag, CPointer<double> x, out string serr) {
            int i;
            int ipli, ipli_ast, ifno;
            int retc;
            Int32 epheflag = SwissEph.SEFLG_DEFAULTEPH;
            plan_data pdp;
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            //#if 0
            //  struct node_data *ndp;
            //#else
            plan_data ndp;
            //#endif
            double[] xp, xp2;
            double[] ss = new double[3];
            string serr2 = null;
            serr = null;
            /****************************************** 
             * iflag plausible?                       * 
             ******************************************/
            iflag = plaus_iflag(iflag, ipl, tjd, out serr);
            /****************************************** 
             * which ephemeris is wanted, which is used?
             * Three ephemerides are possible: MOSEPH, SWIEPH, JPLEPH.
             * JPLEPH is best, SWIEPH is nearly as good, MOSEPH is least precise.
             * The availability of the various ephemerides depends on the installed
             * ephemeris files in the users ephemeris directory. This can change at
             * any time.
             * Swisseph should try to fulfil the wish of the user for a specific
             * ephemeris, but use a less precise one if the desired ephemeris is not
             * available for the given date and body. 
             * If internal ephemeris errors are detected (data error, file length error)
             * an error is returned.
             * If the time range is bad but another ephemeris can deliver this range,
             * the other ephemeris is used.
             * If no ephemeris is specified, DEFAULTEPH is assumed as desired.
             * DEFAULTEPH is defined at compile time, usually as JPLEPH.
             * The caller learns from the return flag which ephemeris was used.
             * ephe_flag is extracted from iflag, but can change later if the
             * desired ephe is not available.
             ******************************************/
            if ((iflag & SwissEph.SEFLG_MOSEPH) != 0)
                epheflag = SwissEph.SEFLG_MOSEPH;
            if ((iflag & SwissEph.SEFLG_SWIEPH) != 0)
                epheflag = SwissEph.SEFLG_SWIEPH;
            if ((iflag & SwissEph.SEFLG_JPLEPH) != 0)
                epheflag = SwissEph.SEFLG_JPLEPH;
            /* no barycentric calculations with Moshier ephemeris */
            if ((iflag & SwissEph.SEFLG_BARYCTR) != 0 && (iflag & SwissEph.SEFLG_MOSEPH) != 0) {
                serr = "barycentric Moshier positions are not supported.";
                return ERR;
            }
            if (epheflag != SwissEph.SEFLG_MOSEPH && !swed.ephe_path_is_set && !swed.jpl_file_is_open)
                swe_set_ephe_path(null);
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0 && !swed.ayana_is_set)
                swe_set_sid_mode(SwissEph.SE_SIDM_FAGAN_BRADLEY, 0, 0);
            /****************************************** 
             * obliquity of ecliptic 2000 and of date * 
             ******************************************/
            swi_check_ecliptic(tjd, iflag);
            /******************************************
             * nutation                               * 
             ******************************************/
            swi_check_nutation(tjd, iflag);
            /****************************************** 
             * select planet and ephemeris            * 
             *                                        * 
             * ecliptic and nutation                  * 
             ******************************************/
            if (ipl == SwissEph.SE_ECL_NUT) {
                x[0] = swed.oec.eps + swed.nut.nutlo[1];	/* true ecliptic */
                x[1] = swed.oec.eps;			/* mean ecliptic */
                x[2] = swed.nut.nutlo[0];		/* nutation in longitude */
                x[3] = swed.nut.nutlo[1];		/* nutation in obliquity */
                /*if ((iflag & SEFLG_RADIANS) == 0)*/
                for (i = 0; i <= 3; i++)
                    x[i] *= SwissEph.RADTODEG;
                return (iflag);
                /****************************************** 
                 * moon                                   * 
                 ******************************************/
            } else if (ipl == SwissEph.SE_MOON) {
                /* internal planet number */
                ipli = SEI_MOON;
                pdp = swed.pldat[ipli];
                xp = pdp.xreturn;
                switch (epheflag) {
                    case SwissEph.SEFLG_JPLEPH:
                        retc = jplplan(tjd, ipli, iflag, DO_SAVE, null, null, null, ref serr);
                        /* read error or corrupt file */
                        if (retc == ERR)
                            goto return_error;
                        /* jpl ephemeris not on disk or date beyond ephemeris range 
                         *     or file corrupt */
                        if (retc == NOT_AVAILABLE) {
                            iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                            serr += " \ntrying Swiss Eph; ";
                            goto sweph_moon;
                        } else if (retc == BEYOND_EPH_LIMITS) {
                            if (tjd > MOSHLUEPH_START && tjd < MOSHLUEPH_END) {
                                iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_MOSEPH;
                                serr += " \nusing Moshier Eph; ";
                                goto moshier_moon;
                            } else
                                goto return_error;
                        }
                        break;
                    case SwissEph.SEFLG_SWIEPH:
                    sweph_moon:
                        //#if 0
                        //    /* for hel. or bary. position, we need earth and sun as well;
                        //         * this is done by sweplan(), but not by swemoon() */
                        //        if (iflag & (SEFLG_HELCTR | SEFLG_BARYCTR | SEFLG_NOABERR)) 
                        //      retc = sweplan(tjd, ipli, SEI_FILE_MOON, iflag, DO_SAVE,
                        //            NULL, NULL, NULL, NULL, serr);
                        //    else
                        //      retc = swemoon(tjd, iflag, DO_SAVE, pdp.x, serr);/**/
                        //#else
                        retc = sweplan(tjd, ipli, SEI_FILE_MOON, iflag, DO_SAVE,
                                null, null, null, null, ref serr);
                        //#endif
                        if (retc == ERR)
                            goto return_error;
                        /* if sweph file not found, switch to moshier */
                        if (retc == NOT_AVAILABLE) {
                            if (tjd > MOSHLUEPH_START && tjd < MOSHLUEPH_END) {
                                iflag = (iflag & ~SwissEph.SEFLG_SWIEPH) | SwissEph.SEFLG_MOSEPH;
                                serr = " \nusing Moshier eph.; ";
                                goto moshier_moon;
                            } else
                                goto return_error;
                        }
                        break;
                    case SwissEph.SEFLG_MOSEPH:
                    moshier_moon:
                        retc = SE.SwemMoon.swi_moshmoon(tjd, DO_SAVE, null, ref serr);/**/
                        if (retc == ERR)
                            goto return_error;
                        /* for hel. position, we need earth as well */
                        retc = SE.SwemPlan.swi_moshplan(tjd, SEI_EARTH, DO_SAVE, null, null, ref serr);/**/
                        if (retc == ERR)
                            goto return_error;
                        break;
                    default:
                        break;
                }
                /* heliocentric, lighttime etc. */
                if ((retc = app_pos_etc_moon(iflag, ref serr)) != OK)
                    goto return_error; /* retc may be wrong with sidereal calculation */
                /********************************************** 
                 * barycentric sun                            * 
                 * (only JPL and SWISSEPH ephemerises)        *
                 **********************************************/
            } else if (ipl == SwissEph.SE_SUN && (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                /* barycentric sun must be handled separately because of
                 * the following reasons:
                 * ordinary planetary computations use the function 
                 * main_planet() and its subfunction jplplan(),
                 * see further below.
                 * now, these functions need the swisseph internal 
                 * planetary indices, where SEI_EARTH = SEI_SUN = 0.
                 * therefore they don't know the difference between
                 * a barycentric sun and a barycentric earth and 
                 * always return barycentric earth.
                 * to avoid this problem, many functions would have to
                 * be changed. as an alternative, we choose a more 
                 * separate handling. */
                ipli = SEI_SUN;	/* = SEI_EARTH ! */
                xp = pedp.xreturn;
                switch (epheflag) {
                    case SwissEph.SEFLG_JPLEPH:
                        /* open ephemeris, if still closed */
                        if (!swed.jpl_file_is_open) {
                            retc = open_jpl_file(ss, swed.jplfnam, swed.ephepath, ref serr);
                            if (retc != OK)
                                goto sweph_sbar;
                        }
                        retc = SE.SweJPL.swi_pleph(tjd, SweJPL.J_SUN, SweJPL.J_SBARY, psdp.x, ref serr);
                        if (retc == ERR || retc == BEYOND_EPH_LIMITS) {
                            SE.SweJPL.swi_close_jpl_file();
                            swed.jpl_file_is_open = false;
                            goto return_error;
                        }
                        /* jpl ephemeris not on disk or date beyond ephemeris range 
                     *     or file corrupt */
                        if (retc == NOT_AVAILABLE) {
                            iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                            serr = (serr ?? String.Empty) + serr;
                            goto sweph_sbar;
                        }
                        psdp.teval = tjd;
                        break;
                    case SwissEph.SEFLG_SWIEPH:
                    sweph_sbar:
                        /* sweplan() provides barycentric sun as a by-product in save area;
                         * it is saved in swed.pldat[SEI_SUNBARY].x */
                        retc = sweplan(tjd, SEI_EARTH, SEI_FILE_PLANET, iflag, DO_SAVE, null, null, null, null, ref serr);
                        //#if 1
                        if (retc == ERR || retc == NOT_AVAILABLE)
                            goto return_error;
                        //#else	/* this code would be needed if barycentric moshier calculation
                        //     * were implemented */
                        //    if (retc == ERR)
                        //      goto return_error;
                        //    /* if sweph file not found, switch to moshier */
                        //        if (retc == NOT_AVAILABLE) {
                        //      if (tjd > MOSHLUEPH_START && tjd < MOSHLUEPH_END) {
                        //        iflag = (iflag & ~SEFLG_SWIEPH) | SEFLG_MOSEPH;
                        //        if (serr != NULL && strlen(serr) + 30 < AS_MAXCH)
                        //          serr+= " \nusing Moshier; ";
                        //        goto moshier_sbar;
                        //      } else
                        //        goto return_error;
                        //    }
                        //#endif
                        psdp.teval = tjd;
                        /* pedp->teval = tjd; */
                        break;
                    default:
                        //#if 0
                        //    moshier_sbar:
                        //#endif
                        return ERR;
                    //break;
                }
                /* flags */
                if ((retc = app_pos_etc_sbar(iflag, ref serr)) != OK)
                    goto return_error;
                /* iflag has possibly changed */
                iflag = pedp.xflgs;
                /* barycentric sun is now in save area of barycentric earth.
                 * (pedp->xreturn = swed.pldat[SEI_EARTH].xreturn).
                 * in case a barycentric earth computation follows for the same
                 * date, the planetary functions will return the barycentric 
                 * SUN unless we force a new computation of pedp->xreturn.
                 * this can be done by initializing the save of iflag.
                 */
                pedp.xflgs = -1;
                /****************************************** 
                 * mercury - pluto                        * 
                 ******************************************/
            } else if (ipl == SwissEph.SE_SUN 	/* main planet */
                || ipl == SwissEph.SE_MERCURY
                || ipl == SwissEph.SE_VENUS
                || ipl == SwissEph.SE_MARS
                || ipl == SwissEph.SE_JUPITER
                || ipl == SwissEph.SE_SATURN
                || ipl == SwissEph.SE_URANUS
                || ipl == SwissEph.SE_NEPTUNE
                || ipl == SwissEph.SE_PLUTO
                || ipl == SwissEph.SE_EARTH) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                    if (ipl == SwissEph.SE_SUN) {
                        /* heliocentric position of Sun does not exist */
                        for (i = 0; i < 24; i++)
                            x[i] = 0;
                        return iflag;
                    }
                } else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    ;
                } else {		/* geocentric */
                    if (ipl == SwissEph.SE_EARTH) {
                        /* geocentric position of Earth does not exist */
                        for (i = 0; i < 24; i++)
                            x[i] = 0;
                        return iflag;
                    }
                }
                /* internal planet number */
                ipli = pnoext2int[ipl];
                pdp = swed.pldat[ipli];
                xp = pdp.xreturn;
                retc = main_planet(tjd, ipli, epheflag, iflag, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* iflag has possibly changed in main_planet() */
                iflag = pdp.xflgs;
                /*********************i************************ 
                 * mean lunar node                            * 
                 * for comment s. moshmoon.c, swi_mean_node() *
                 **********************************************/
            } else if (ipl == SwissEph.SE_MEAN_NODE) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar node not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                ndp = swed.nddat[SEI_MEAN_NODE];
                xp = ndp.xreturn;
                xp2 = ndp.x;
                retc = SE.SwemMoon.swi_mean_node(tjd, xp2, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* speed (is almost constant; variation < 0.001 arcsec) */
                retc = SE.SwemMoon.swi_mean_node(tjd - MEAN_NODE_SPEED_INTV, xp2.GetPointer(3), ref serr);
                if (retc == ERR)
                    goto return_error;
                xp2[3] = SE.swe_difrad2n(xp2[0], xp2[3]) / MEAN_NODE_SPEED_INTV;
                xp2[4] = xp2[5] = 0;
                ndp.teval = tjd;
                ndp.xflgs = -1;
                /* lighttime etc. */
                if ((retc = app_pos_etc_mean(SEI_MEAN_NODE, iflag, ref serr)) != OK)
                    goto return_error;
                /* to avoid infinitesimal deviations from latitude = 0 
                 * that result from conversions */
                if ((iflag & SwissEph.SEFLG_SIDEREAL) == 0 && (iflag & SwissEph.SEFLG_J2000) == 0) {
                    ndp.xreturn[1] = 0.0;	/* ecl. latitude       */
                    ndp.xreturn[4] = 0.0;	/*               speed */
                    ndp.xreturn[5] = 0.0;	/*      radial   speed */
                    ndp.xreturn[8] = 0.0;	/* z coordinate        */
                    ndp.xreturn[11] = 0.0;	/*               speed */
                }
                if (retc == ERR)
                    goto return_error;
                /********************************************** 
                 * mean lunar apogee ('dark moon', 'lilith')  *
                 * for comment s. moshmoon.c, swi_mean_apog() *
                 **********************************************/
            } else if (ipl == SwissEph.SE_MEAN_APOG) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar apogee not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                ndp = swed.nddat[SEI_MEAN_APOG];
                xp = ndp.xreturn;
                xp2 = ndp.x;
                retc = SE.SwemMoon.swi_mean_apog(tjd, xp2, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* speed (is not constant! variation ~= several arcsec) */
                retc = SE.SwemMoon.swi_mean_apog(tjd - MEAN_NODE_SPEED_INTV, xp2.GetPointer(3), ref serr);
                if (retc == ERR)
                    goto return_error;
                for (i = 0; i <= 1; i++)
                    xp2[3 + i] = SE.swe_difrad2n(xp2[i], xp2[3 + i]) / MEAN_NODE_SPEED_INTV;
                xp2[5] = 0;
                ndp.teval = tjd;
                ndp.xflgs = -1;
                /* lighttime etc. */
                if ((retc = app_pos_etc_mean(SEI_MEAN_APOG, iflag, ref serr)) != OK)
                    goto return_error;
                /* to avoid infinitesimal deviations from r-speed = 0 
                 * that result from conversions */
                ndp.xreturn[5] = 0.0;	/*               speed */
                if (retc == ERR)
                    goto return_error;
                /*********************************************** 
                 * osculating lunar node ('true node')         *    
                 ***********************************************/
            } else if (ipl == SwissEph.SE_TRUE_NODE) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar node not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                ndp = swed.nddat[SEI_TRUE_NODE];
                xp = ndp.xreturn;
                retc = lunar_osc_elem(tjd, SEI_TRUE_NODE, iflag, ref serr);
                iflag = ndp.xflgs;
                /* to avoid infinitesimal deviations from latitude = 0 
                 * that result from conversions */
                if ((iflag & SwissEph.SEFLG_SIDEREAL) == 0 && (iflag & SwissEph.SEFLG_J2000) == 0) {
                    ndp.xreturn[1] = 0.0;	/* ecl. latitude       */
                    ndp.xreturn[4] = 0.0;	/*               speed */
                    ndp.xreturn[8] = 0.0;	/* z coordinate        */
                    ndp.xreturn[11] = 0.0;	/*               speed */
                }
                if (retc == ERR)
                    goto return_error;
                /*********************************************** 
                 * osculating lunar apogee                     *    
                 ***********************************************/
            } else if (ipl == SwissEph.SE_OSCU_APOG) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar apogee not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                if (tjd < MOSHLUEPH_START || tjd > MOSHLUEPH_END)
                {
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    serr = C.sprintf("Interpolated apsides are restricted to JD %8.1f - JD %8.1f",
                            MOSHLUEPH_START, MOSHLUEPH_END);
                    return ERR;
                }
                ndp = swed.nddat[SEI_OSCU_APOG];
                xp = ndp.xreturn;
                retc = lunar_osc_elem(tjd, SEI_OSCU_APOG, iflag, ref serr);
                iflag = ndp.xflgs;
                if (retc == ERR)
                    goto return_error;
                /*********************************************** 
                 * interpolated lunar apogee                   *    
                 ***********************************************/
            } else if (ipl == SwissEph.SE_INTP_APOG) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar apogee not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                if (tjd < MOSHLUEPH_START || tjd > MOSHLUEPH_END)
                {
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    serr = C.sprintf("Interpolated apsides are restricted to JD %8.1f - JD %8.1f",
                            MOSHLUEPH_START, MOSHLUEPH_END);
                    return ERR;
                }
                ndp = swed.nddat[SEI_INTP_APOG];
                xp = ndp.xreturn;
                retc = intp_apsides(tjd, SEI_INTP_APOG, iflag, ref serr);
                iflag = ndp.xflgs;
                if (retc == ERR)
                    goto return_error;
                /*********************************************** 
                 * interpolated lunar perigee                  *    
                 ***********************************************/
            } else if (ipl == SwissEph.SE_INTP_PERG) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    /* heliocentric/barycentric lunar apogee not allowed */
                    for (i = 0; i < 24; i++)
                        x[i] = 0;
                    return iflag;
                }
                ndp = swed.nddat[SEI_INTP_PERG];
                xp = ndp.xreturn;
                retc = intp_apsides(tjd, SEI_INTP_PERG, iflag, ref serr);
                iflag = ndp.xflgs;
                if (retc == ERR)
                    goto return_error;
                /*********************************************** 
                 * minor planets                               *    
                 ***********************************************/
            } else if (ipl == SwissEph.SE_CHIRON
              || ipl == SwissEph.SE_PHOLUS
              || ipl == SwissEph.SE_CERES		/* Ceres - Vesta */
              || ipl == SwissEph.SE_PALLAS
              || ipl == SwissEph.SE_JUNO
              || ipl == SwissEph.SE_VESTA
              || ipl > SwissEph.SE_AST_OFFSET) {
                /* internal planet number */
                if (ipl < SwissEph.SE_NPLANETS)
                    ipli = pnoext2int[ipl];
                else if (ipl <= SwissEph.SE_AST_OFFSET + MPC_VESTA) {
                    ipli = SEI_CERES + ipl - SwissEph.SE_AST_OFFSET - 1;
                    ipl = SwissEph.SE_CERES + ipl - SwissEph.SE_AST_OFFSET - 1;
                    //#if 0
                    //    } else if (ipl == SE_AST_OFFSET + MPC_CHIRON) {
                    //      ipli = SEI_CHIRON;
                    //      ipl = SE_CHIRON;
                    //    } else if (ipl == SE_AST_OFFSET + MPC_PHOLUS) {
                    //      ipli = SEI_PHOLUS;
                    //      ipl = SE_PHOLUS;
                    //#endif
                } else {			/* any asteroid except*/
                    ipli = SEI_ANYBODY;
                }
                if (ipli == SEI_ANYBODY)
                    ipli_ast = ipl;
                else
                    ipli_ast = ipli;
                pdp = swed.pldat[ipli];
                xp = pdp.xreturn;
                if (ipli_ast > SwissEph.SE_AST_OFFSET)
                    ifno = SEI_FILE_ANY_AST;
                else
                    ifno = SEI_FILE_MAIN_AST;
                if (ipli == SEI_CHIRON && (tjd < CHIRON_START || tjd > CHIRON_END)) {
                    serr = C.sprintf("Chiron's ephemeris is restricted to JD %8.1f - JD %8.1f",
                        CHIRON_START, CHIRON_END);
                    return ERR;
                }
                if (ipli == SEI_PHOLUS && (tjd < PHOLUS_START || tjd > PHOLUS_END)) {
                    serr = C.sprintf(
                        "Pholus's ephemeris is restricted to JD %8.1f - JD %8.1f",
                        PHOLUS_START, PHOLUS_END);
                    return ERR;
                }
            do_asteroid:
                /* earth and sun are also needed */
                retc = main_planet(tjd, SEI_EARTH, epheflag, iflag, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* iflag (ephemeris bit) has possibly changed in main_planet() */
                iflag = swed.pldat[SEI_EARTH].xflgs;
                /* asteroid */
                if (!String.IsNullOrWhiteSpace(serr)) {
                    serr2 = serr;
                    serr = String.Empty;
                }
                /* asteroid */
                retc = sweph(tjd, ipli_ast, ifno, iflag, psdp.x, DO_SAVE, null, ref serr);
                if (retc == ERR || retc == NOT_AVAILABLE)
                    goto return_error;
                retc = app_pos_etc_plan(ipli_ast, iflag, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* app_pos_etc_plan() might have failed, if t(light-time)
                 * is beyond ephemeris range. in this case redo with Moshier 
                 */
                if (retc == NOT_AVAILABLE || retc == BEYOND_EPH_LIMITS) {
                    if (epheflag != SwissEph.SEFLG_MOSEPH) {
                        iflag = (iflag & ~SwissEph.SEFLG_EPHMASK) | SwissEph.SEFLG_MOSEPH;
                        epheflag = SwissEph.SEFLG_MOSEPH;
                        serr += "\nusing Moshier eph.; ";
                        goto do_asteroid;
                    } else
                        goto return_error;
                }
                /* add warnings from earth/sun computation */
                if (String.IsNullOrWhiteSpace(serr) && !String.IsNullOrWhiteSpace(serr2))
                    serr = "sun: " + serr2;
                /*********************************************** 
                 * fictitious planets                          *    
                 * (Isis-Transpluto and Uranian planets)       *
                 ***********************************************/
            } else if (ipl >= SwissEph.SE_FICT_OFFSET && ipl <= SwissEph.SE_FICT_MAX) {
                //#if 0
                //       ipl == SE_CUPIDO
                //    || ipl == SE_HADES
                //    || ipl == SE_ZEUS
                //    || ipl == SE_KRONOS
                //    || ipl == SE_APOLLON
                //    || ipl == SE_ADMETOS
                //    || ipl == SE_VULKANUS
                //    || ipl == SE_POSEIDON
                //    || ipl == SE_ISIS
                //    || ipl == SE_NEPTUNE_LEVERRIER
                //    || ipl == SE_NEPTUNE_ADAMS) 
                //#endif
                /* internal planet number */
                ipli = SEI_ANYBODY;
                pdp = swed.pldat[ipli];
                xp = pdp.xreturn;
            do_fict_plan:
                /* the earth for geocentric position */
                retc = main_planet(tjd, SEI_EARTH, epheflag, iflag, ref serr);
                /* iflag (ephemeris bit) has possibly changed in main_planet() */
                iflag = swed.pldat[SEI_EARTH].xflgs;
                /* planet from osculating elements */
                if (SE.SwemPlan.swi_osc_el_plan(tjd, pdp.x, ipl - SwissEph.SE_FICT_OFFSET, ipli, pedp.x, psdp.x, ref serr) != OK)
                    goto return_error;
                if (retc == ERR)
                    goto return_error;
                retc = app_pos_etc_plan_osc(ipl, ipli, iflag, ref serr);
                if (retc == ERR)
                    goto return_error;
                /* app_pos_etc_plan_osc() might have failed, if t(light-time)
                 * is beyond ephemeris range. in this case redo with Moshier 
                 */
                if (retc == NOT_AVAILABLE || retc == BEYOND_EPH_LIMITS) {
                    if (epheflag != SwissEph.SEFLG_MOSEPH) {
                        iflag = (iflag & ~SwissEph.SEFLG_EPHMASK) | SwissEph.SEFLG_MOSEPH;
                        epheflag = SwissEph.SEFLG_MOSEPH;
                        serr += "\nusing Moshier eph.; ";
                        goto do_fict_plan;
                    } else
                        goto return_error;
                }
                /*********************************************** 
                 * invalid body number                         *    
                 ***********************************************/
            } else {
                serr = C.sprintf("illegal planet number %d.", ipl);
                goto return_error;
            }
            for (i = 0; i < 24; i++)
                x[i] = xp[i];
            return (iflag);
        /*********************************************** 
         * return error                                * 
         ***********************************************/
        return_error: ;
            for (i = 0; i < 24; i++)
                x[i] = 0;
            return ERR;
        }

        void free_planets()
        {
            int i;
            /* free planets data space */
            for (i = 0; i < Sweph.SEI_NPLANETS; i++)
            {
                if (swed.pldat[i].segp != null)
                {
                    swed.pldat[i].segp = null;
                }
                if (swed.pldat[i].refep != null)
                {
                    swed.pldat[i].refep = null;
                }
                swed.pldat[i] = new plan_data();
            }
            for (i = 0; i <= SwissEph.SE_NPLANETS; i++) /* "<=" is correct! see decl. */
                swed.savedat[i] = new save_positions();
            /* clear node data space */
            for (i = 0; i < Sweph.SEI_NNODE_ETC; i++)
            {
                //#if 0
                //    memset((void *) &swed.nddat[i], 0, sizeof(struct node_data));
                //#else
                swed.nddat[i] = new plan_data();
                //#endif
            }
        }

        /* Function initialises swed structure. 
         * Returns 1 if initialisation is done, otherwise 0 */
        public Int32 swi_init_swed_if_start()
        {
            /* initialisation of swed, when called first time from */
            if (!swed.ephe_path_is_set)
            {
                swed = new swe_data();
                swed.ephepath = SwissEph.SE_EPHE_PATH;
                swed.jplfnam = SwissEph.SE_FNAME_DFT;
                SE.swe_set_tid_acc(SwissEph.SE_TIDAL_AUTOMATIC);
                return 1;
            }
            return 0;
        }

        /* closes all open files, frees space of planetary data, 
         * deletes memory of all computed positions 
         */
        void swi_close_keep_topo_etc()
        {
            int i;
            /* close SWISSEPH files */
            for (i = 0; i < SEI_NEPHFILES; i++)
            {
                if (swed.fidat[i].fptr != null)
                    swed.fidat[i].fptr.Dispose();
                swed.fidat[i] = new file_data();
            }
            free_planets();
            swed.oec = new epsilon();
            swed.oec2000 = new epsilon();
            swed.nut = new nut();
            swed.nut2000 = new nut();
            swed.nutv = new nut();
            swed.astro_models = new Int32[SEI_NMODELS];
            /* close JPL file */
            SE.SweJPL.swi_close_jpl_file();
            swed.jpl_file_is_open = false;
            swed.jpldenum = 0;
            /* close fixed stars */
            if (swed.fixfp != null)
            {
                swed.fixfp.Dispose();
                swed.fixfp = null;
            }
            SE.swe_set_tid_acc(SwissEph.SE_TIDAL_AUTOMATIC);
            swed.is_old_starfile = false;
            swed.i_saved_planet_name = 0;
            swed.saved_planet_name = String.Empty;
            swed.timeout = 0;
        }

        /* closes all open files, frees space of planetary data, 
         * deletes memory of all computed positions 
         */
        public void swe_close() {
            int i;
            /* close SWISSEPH files */
            for (i = 0; i < SEI_NEPHFILES; i++) {
                if (swed.fidat[i].fptr != null)
                    swed.fidat[i].fptr.Dispose();
                swed.fidat[i] = new file_data();
            }
            free_planets();
            swed.Reset(false);
            /* close JPL file */
            SE.SweJPL.swi_close_jpl_file();
            swed.jpl_file_is_open = false;
            swed.jpldenum = 0;
            /* close fixed stars */
            if (swed.fixfp != null) {
                swed.fixfp.Dispose();
                swed.fixfp = null;
            }
            SE.SwephLib.swe_set_tid_acc(SwissEph.SE_TIDAL_AUTOMATIC);
            swed.geopos_is_set = false;
            swed.ayana_is_set = false;
            swed.is_old_starfile = false;
            swed.i_saved_planet_name = 0;
            swed.saved_planet_name=String.Empty;
            swed.topd = new topo_data();
            swed.sidd = new sid_data();
            swed.timeout = 0;
            swed.last_epheflag = 0;
            if (swed.dpsi != null)
            {
              //free(swed.dpsi);
              swed.dpsi = null;
            }
            if (swed.deps != null)
            {
                //free(swed.deps);
                swed.deps = null;
            }
            /*  swed.ephe_path_is_set = FALSE;
              *swed.ephepath = '\0'; */
#if TRACE
            //#define TRACE_CLOSE FALSE
            //  swi_open_trace(NULL);
            //  if (swi_fp_trace_c != NULL) {
            //    if (swi_trace_count < TRACE_COUNT_MAX) {
            //trace(true, "\n/*SWE_CLOSE*/");
            //      fputs("  swe_close();\n", swi_fp_trace_c);
            //trace(true, "  swe_close();");
            //#if TRACE_CLOSE
            //      fputs("}\n", swi_fp_trace_c);
            //#endif
            //      fflush(swi_fp_trace_c);
            //    }
            //#if TRACE_CLOSE
            //    fclose(swi_fp_trace_c);
            //#endif
            //  }
            //#if TRACE_CLOSE
            //  if (swi_fp_trace_out != NULL)
            //    fclose(swi_fp_trace_out);
            //  swi_fp_trace_c = NULL;
            //  swi_fp_trace_out = NULL;
            //#endif
#endif  //* TRACE */
        }

        /* sets ephemeris file path. 
         * also calls swe_close(). this makes sure that swe_calc()
         * won't return planet positions previously computed from other
         * ephemerides
         */
        public void swe_set_ephe_path(string path) {
            int i, iflag;
            string s; string sdummy = null;
            //string sp;
            double[] xx = new double[6];
            /* close all open files and delete all planetary data */
            swi_close_keep_topo_etc();
            swi_init_swed_if_start();
            swed.ephe_path_is_set = true;
            /* environment variable SE_EPHE_PATH has priority */
            //  if ((sp = getenv("SE_EPHE_PATH")) != NULL 
            //    && strlen(sp) != 0
            //    && strlen(sp) <= AS_MAXCH-1-13) {
            //    s= sp;
            //  } else if (path == NULL) {
            //    s= SE_EPHE_PATH;
            //  } else if (strlen(path) <= AS_MAXCH-1-13)
            //    s= path;
            //  else
            //    s= SE_EPHE_PATH;
            s = !String.IsNullOrWhiteSpace(path) ? path : SwissEph.SE_EPHE_PATH;
            i = s.Length;
            //  if (*(s + i - 1) != *DIR_GLUE && *s != '\0')
            //    s+= DIR_GLUE;
            if (!s.EndsWith(SwissEph.DIR_GLUE.ToString()))
                s = s + SwissEph.DIR_GLUE;
            swed.ephepath = s;
            /* try to open lunar ephemeris, in order to get DE number and set
             * tidal acceleration of the Moon */
            iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_J2000 | SwissEph.SEFLG_TRUEPOS | SwissEph.SEFLG_ICRS;
            swed.last_epheflag = 2;
            swe_calc(J2000, SwissEph.SE_MOON, iflag, xx, ref sdummy);
            if (swed.fidat[SEI_FILE_MOON].fptr != null) {
                SE.SwephLib.swi_set_tid_acc(0, 0, swed.fidat[SEI_FILE_MOON].sweph_denum, ref sdummy);
            }
#if TRACE
            //  swi_open_trace(NULL);
            //  if (swi_trace_count < TRACE_COUNT_MAX) {
            //    if (swi_fp_trace_c != NULL) {
            //      fputs("\n/*SWE_SET_EPHE_PATH*/\n", swi_fp_trace_c);
            //      if (path == NULL) 
            //        fputs("  *s = '\\0';\n", swi_fp_trace_c);
            //      else
            //    fprintf(swi_fp_trace_c, "  strcpy(s, \"%s\");\n", path);
            //      fputs("  swe_set_ephe_path(s);\n", swi_fp_trace_c);
            //      fputs("  printf(\"swe_set_ephe_path: path_in = \");", swi_fp_trace_c);
            //      fputs("  printf(s);\n", swi_fp_trace_c);
            //      fputs("  \tprintf(\"\\tpath_set = unknown to swetrace\\n\"); /* unknown to swetrace */\n", swi_fp_trace_c);
            //      fflush(swi_fp_trace_c);
            //    }
            //    if (swi_fp_trace_out != NULL) {
            //      fputs("swe_set_ephe_path: path_in = ", swi_fp_trace_out);
            //      if (path != NULL)
            //          fputs(path, swi_fp_trace_out);
            //      fputs("\tpath_set = ", swi_fp_trace_out);
            //      fputs(s, swi_fp_trace_out);
            //      fputs("\n", swi_fp_trace_out);
            trace("swe_set_ephe_path: path_in = %s\tpath_set = %s", path, s);
            //      fflush(swi_fp_trace_out);
            //    }
            //  }
#endif
        }

        void load_dpsi_deps() {
            CFile fp;
            string s, sdummy = String.Empty;
            string[] cpos;
            int n = 0, np, iyear, mjd = 0, mjdsv = 0;
            double dpsi, deps, TJDOFS = 2400000.5;
            if (swed.eop_dpsi_loaded > 0)
                return;
            fp = swi_fopen(-1, SwephLib.DPSI_DEPS_IAU1980_FILE_EOPC04, swed.ephepath, ref sdummy);
            if (fp == null) {
                swed.eop_dpsi_loaded = ERR;
                return;
            }
            if ((swed.dpsi = new double[Sweph.SWE_DATA_DPSI_DEPS]) == null)
            {
                swed.eop_dpsi_loaded = ERR;
                return;
            }
            if ((swed.deps = new double[Sweph.SWE_DATA_DPSI_DEPS]) == null)
            {
                swed.eop_dpsi_loaded = ERR;
                return;
            }
            swed.eop_tjd_beg_horizons = SwephLib.DPSI_DEPS_IAU1980_TJD0_HORIZONS;
            while ((s = fp.ReadLine()) != null) {
                cpos = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                np = cpos.Length;
                if ((iyear = int.Parse(cpos[0])) == 0)
                    continue;
                mjd = int.Parse(cpos[3]);
                /* is file in one-day steps? */
                if (mjdsv > 0 && mjd - mjdsv != 1) {
                    /* we cannot return error but we note it as follows: */
                    swed.eop_dpsi_loaded = -2;
                    fp.Dispose();
                    return;
                }
                if (n == 0)
                    swed.eop_tjd_beg = mjd + TJDOFS;
                swed.dpsi[n] = double.Parse(cpos[8], CultureInfo.InvariantCulture);
                swed.deps[n] = double.Parse(cpos[9], CultureInfo.InvariantCulture);
                /*    fprintf(stderr, "tjd=%f, dpsi=%f, deps=%f\n", mjd + 2400000.5, swed.dpsi[n] * 1000, swed.deps[n] * 1000);exit(0);*/
                n++;
                mjdsv = mjd;
            }
            swed.eop_tjd_end = mjd + TJDOFS;
            swed.eop_dpsi_loaded = 1;
            fp.Dispose();
            /* file finals.all may have some more data, and especially estimations 
             * for the near future */
            fp = swi_fopen(-1, SwephLib.DPSI_DEPS_IAU1980_FILE_FINALS, swed.ephepath, ref sdummy);
            if (fp == null)
                return; /* return without error as existence of file is not mandatory */
            while ((s = fp.ReadLine()) != null) {
                mjd = int.Parse(s.Substring(7));
                if (mjd + TJDOFS <= swed.eop_tjd_end)
                    continue;
                if (n >= Sweph.SWE_DATA_DPSI_DEPS)
                    return;
                /* are data in one-day steps? */
                if (mjdsv > 0 && mjd - mjdsv != 1) {
                    /* no error, as we do have data; however, if this file is usefull,
                     * then swed.eop_dpsi_loaded will be set to 2 */
                    swed.eop_dpsi_loaded = -3;
                    fp.Dispose();
                    return;
                }
                /* dpsi, deps Bulletin B */
                dpsi = double.Parse(s.Substring(168), CultureInfo.InvariantCulture);
                deps = double.Parse(s.Substring(178), CultureInfo.InvariantCulture);
                if (dpsi == 0) {
                    /* try dpsi, deps Bulletin A */
                    dpsi = double.Parse(s.Substring(99), CultureInfo.InvariantCulture);
                    deps = double.Parse(s.Substring(118), CultureInfo.InvariantCulture);
                }
                if (dpsi == 0) {
                    swed.eop_dpsi_loaded = 2;
                    /*printf("dpsi from %f to %f \n", swed.eop_tjd_beg, swed.eop_tjd_end);*/
                    fp.Dispose();
                    return;
                }
                swed.eop_tjd_end = mjd + TJDOFS;
                swed.dpsi[n] = dpsi / 1000.0;
                swed.deps[n] = deps / 1000.0;
                /*fprintf(stderr, "tjd=%f, dpsi=%f, deps=%f\n", mjd + 2400000.5, swed.dpsi[n] * 1000, swed.deps[n] * 1000);*/
                n++;
                mjdsv = mjd;
            }
            swed.eop_dpsi_loaded = 2;
            fp.Dispose();
        }

        /* sets jpl file name.
         * also calls swe_close(). this makes sure that swe_calc()
         * won't return planet positions previously computed from other
         * ephemerides
         */
        public void swe_set_jpl_file(string fname) {
            string sp, sdummy = String.Empty;
            int retc, spi;
            double[] ss = new double[3];
            /* close all open files and delete all planetary data */
            swi_close_keep_topo_etc();
            swi_init_swed_if_start();
            /* if path is contained in fnam, it is filled into the path variable */
            //sp = strrchr(fname, (int)*DIR_GLUE);
            //if (sp == NULL)
            //    sp = fname;
            //else
            //    sp = sp + 1;
            //if (strlen(sp) >= AS_MAXCH)
            //    sp[AS_MAXCH] = '\0';
            spi = fname.LastIndexOf(SwissEph.DIR_GLUE);
            sp = spi < 0 ? fname : fname.Substring(spi + 1);
            swed.jplfnam = sp;
            /* open ephemeris, if still closed */
            retc = open_jpl_file(ss, swed.jplfnam, swed.ephepath, ref sdummy);
            if (retc == OK) {
                if (swed.jpldenum >= 403) {
                    /*if (INCLUDE_CODE_FOR_DPSI_DEPS_IAU1980) */
                    load_dpsi_deps();
                }
            }
#if TRACE
            //swi_open_trace(NULL);
            //if (swi_trace_count < TRACE_COUNT_MAX) {
            //  if (swi_fp_trace_c != NULL) {
            //    fputs("\n/*SWE_SET_JPL_FILE*/\n", swi_fp_trace_c);
            //    fprintf(swi_fp_trace_c, "  strcpy(s, \"%s\");\n", fname);
            //    fputs("  swe_set_jpl_file(s);\n", swi_fp_trace_c);
            //    fputs("  printf(\"swe_set_jpl_file: fname_in = \");", swi_fp_trace_c);
            //    fputs("  printf(s);\n", swi_fp_trace_c);
            //    fputs("  printf(\"\\tfname_set = unknown to swetrace\\n\");  /* unknown to swetrace */\n", swi_fp_trace_c);
            //    fflush(swi_fp_trace_c);
            //  }
            //  if (swi_fp_trace_out != NULL) {
            trace("swe_set_jpl_file: fname_in = ");
            trace(fname);
            trace("\tfname_set = ");
            trace(sp);
            trace("\n");
            //    fflush(swi_fp_trace_out);
            //  }
            //}
#endif
        }

        /* calculates obliquity of ecliptic and stores it together
         * with its date, sine, and cosine
         */
        void calc_epsilon(double tjd, Int32 iflag, epsilon e) {
            e.teps = tjd;
            e.eps = SE.SwephLib.swi_epsiln(tjd, iflag);
            e.seps = Math.Sin(e.eps);
            e.ceps = Math.Cos(e.eps);
        }

        /* computes a main planet from any ephemeris, if it 
         * has not yet been computed for this date.
         * since a geocentric position requires the earth, the
         * earth's position will be computed as well. With SWISSEPH
         * files the barycentric sun will be done as well.
         * With Moshier, the moon will be done as well.
         *
         * tjd 		= julian day
         * ipli		= body number
         * epheflag	= which ephemeris? JPL, SWISSEPH, Moshier?
         * iflag	= other flags
         *
         * the geocentric apparent position of ipli (or whatever has
         * been specified in iflag) will be saved in
         * &swed.pldat[ipli].xreturn[];
         *
         * the barycentric (heliocentric with Moshier) position J2000
         * will be kept in 
         * &swed.pldat[ipli].x[];
         */
        int main_planet(double tjd, int ipli, Int32 epheflag, Int32 iflag,
                       ref string serr) {
            int retc;
            switch (epheflag) {
                case SwissEph.SEFLG_JPLEPH:
                    retc = jplplan(tjd, ipli, iflag, DO_SAVE, null, null, null, ref serr);
                    /* read error or corrupt file */
                    if (retc == ERR)
                        return ERR;
                    /* jpl ephemeris not on disk or date beyond ephemeris range */
                    if (retc == NOT_AVAILABLE) {
                        iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                        serr += " \ntrying Swiss Eph; ";
                        goto sweph_planet;
                    } else if (retc == BEYOND_EPH_LIMITS) {
                        if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                            iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_MOSEPH;
                            serr += " \nusing Moshier Eph; ";
                            goto moshier_planet;
                        } else
                            return ERR;
                    }
                    /* geocentric, lighttime etc. */
                    if (ipli == SEI_SUN)
                        retc = app_pos_etc_sun(iflag, ref serr)/**/;
                    else
                        retc = app_pos_etc_plan(ipli, iflag, ref serr);
                    if (retc == ERR)
                        return ERR;
                    /* t for light-time beyond ephemeris range */
                    if (retc == NOT_AVAILABLE) {
                        iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                        serr += " \ntrying Swiss Eph; ";
                        goto sweph_planet;
                    } else if (retc == BEYOND_EPH_LIMITS) {
                        if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                            iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_MOSEPH;
                            serr += " \nusing Moshier Eph; ";
                            goto moshier_planet;
                        } else
                            return ERR;
                    }
                    break;
                case SwissEph.SEFLG_SWIEPH:
                sweph_planet:
                    /* compute barycentric planet (+ earth, sun, moon) */
                    retc = sweplan(tjd, ipli, SEI_FILE_PLANET, iflag, DO_SAVE, null, null, null, null, ref serr);
                    if (retc == ERR)
                        return ERR;
                    /* if sweph file not found, switch to moshier */
                    if (retc == NOT_AVAILABLE) {
                        if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                            iflag = (iflag & ~SwissEph.SEFLG_SWIEPH) | SwissEph.SEFLG_MOSEPH;
                            serr += " \nusing Moshier eph.; ";
                            goto moshier_planet;
                        } else
                            return ERR;
                    }
                    /* geocentric, lighttime etc. */
                    if (ipli == SEI_SUN)
                        retc = app_pos_etc_sun(iflag, ref serr)/**/;
                    else
                        retc = app_pos_etc_plan(ipli, iflag, ref serr);
                    if (retc == ERR)
                        return ERR;
                    /* if sweph file for t(lighttime) not found, switch to moshier */
                    if (retc == NOT_AVAILABLE) {
                        if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                            iflag = (iflag & ~SwissEph.SEFLG_SWIEPH) | SwissEph.SEFLG_MOSEPH;
                            serr += " \nusing Moshier eph.; ";
                            goto moshier_planet;
                        } else
                            return ERR;
                    }
                    break;
                case SwissEph.SEFLG_MOSEPH:
                moshier_planet:
                    retc = SE.SwemPlan.swi_moshplan(tjd, ipli, DO_SAVE, null, null, ref serr);/**/
                    if (retc == ERR)
                        return ERR;
                    /* geocentric, lighttime etc. */
                    if (ipli == SEI_SUN)
                        retc = app_pos_etc_sun(iflag, ref  serr)/**/;
                    else
                        retc = app_pos_etc_plan(ipli, iflag, ref serr);
                    if (retc == ERR)
                        return ERR;
                    break;
                default:
                    break;
            }
            return OK;
        }

        /* Computes a main planet from any ephemeris or returns
         * it again, if it has been computed before.
         * In barycentric equatorial position of the J2000 equinox.
         * The earth's position is computed as well. With SWISSEPH
         * and JPL ephemeris the barycentric sun is computed, too.
         * With Moshier, the moon is returned, as well.
         *
         * tjd 		= julian day
         * ipli		= body number
         * epheflag	= which ephemeris? JPL, SWISSEPH, Moshier?
         * iflag	= other flags
         * xp, xe, xs, and xm are the pointers, where the program
         * either finds or stores (if not found) the barycentric 
         * (heliocentric with Moshier) positions of the following 
         * bodies:
         * xp		planet
         * xe		earth
         * xs		sun
         * xm		moon
         * 
         * xm is used with Moshier only 
         */
        int main_planet_bary(double tjd, int ipli, Int32 epheflag, Int32 iflag, bool do_save,
                       CPointer<double> xp, CPointer<double> xe, CPointer<double> xs, CPointer<double> xm,
                       ref string serr) {
            int i, retc;
            switch (epheflag) {
                case SwissEph.SEFLG_JPLEPH:
                    retc = jplplan(tjd, ipli, iflag, do_save, xp, xe, xs, ref serr);
                    /* read error or corrupt file */
                    if (retc == ERR || retc == BEYOND_EPH_LIMITS)
                        return retc;
                    /* jpl ephemeris not on disk or date beyond ephemeris range */
                    if (retc == NOT_AVAILABLE) {
                        iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                        serr += " \ntrying Swiss Eph; ";
                        goto sweph_planet;
                    }
                    break;
                case SwissEph.SEFLG_SWIEPH:
                sweph_planet:
                    /* compute barycentric planet (+ earth, sun, moon) */
                    retc = sweplan(tjd, ipli, SEI_FILE_PLANET, iflag, do_save, xp, xe, xs, xm, ref serr);
                    //#if 1
                    if (retc == ERR || retc == NOT_AVAILABLE)
                        return retc;
                    //#else /* if barycentric moshier calculation were implemented */
                    //      if (retc == ERR)
                    //    return ERR;
                    //      /* if sweph file not found, switch to moshier */
                    //      if (retc == NOT_AVAILABLE) {
                    //    if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                    //      iflag = (iflag & ~SEFLG_SWIEPH) | SEFLG_MOSEPH;
                    //      if (serr != NULL && strlen(serr) + 30 < AS_MAXCH)
                    //        serr+= " \nusing Moshier eph.; ";
                    //      goto moshier_planet;
                    //    } else
                    //      goto return_error;
                    //      }
                    //#endif
                    break;
                case SwissEph.SEFLG_MOSEPH:
                    //#if 0
                    //      moshier_planet:
                    //#endif
                    retc = SE.SwemPlan.swi_moshplan(tjd, ipli, do_save, xp, xe, ref serr);/**/
                    if (retc == ERR)
                        return ERR;
                    for (i = 0; i <= 5; i++)
                        xs[i] = 0;
                    break;
                default:
                    break;
            }
            return OK;
        }

        /* SWISSEPH 
         * this routine computes heliocentric cartesian equatorial coordinates
         * of equinox 2000 of
         * geocentric moon
         * 
         * tjd 		julian date
         * iflag	flag
         * do_save	save J2000 position in save area pdp->x ?
         * xp		array of 6 doubles for lunar position and speed
         * serr		error string
         */
        int swemoon(double tjd, Int32 iflag, bool do_save, CPointer<double> xpret, ref string serr) {
            int i, retc;
            plan_data pdp = swed.pldat[SEI_MOON];
            Int32 speedf1, speedf2;
            double[] xx = new double[6]; CPointer<double> xp;
            if (do_save)
                xp = pdp.x;
            else
                xp = xx;
            /* if planet has already been computed for this date, return 
             * if speed flag has been turned on, recompute planet */
            speedf1 = pdp.xflgs & SwissEph.SEFLG_SPEED;
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            if (tjd == pdp.teval
              && pdp.iephe == SwissEph.SEFLG_SWIEPH
              && (0 == speedf2 || speedf1 != 0)) {
                xp = pdp.x;
            } else {
                /* call sweph for moon */
                retc = sweph(tjd, SEI_MOON, SEI_FILE_MOON, iflag, null, do_save, xp, ref serr);
                if (retc != OK)
                    return (retc);
                if (do_save) {
                    pdp.teval = tjd;
                    pdp.xflgs = -1;
                    pdp.iephe = SwissEph.SEFLG_SWIEPH;
                }
            }
            if (xpret != null)
                for (i = 0; i <= 5; i++)
                    xpret[i] = xp[i];
            return (OK);
        }

        /* SWISSEPH 
         * this function computes 
         * 1. a barycentric planet 
         * plus, under certain conditions, 
         * 2. the barycentric sun, 
         * 3. the barycentric earth, and 
         * 4. the geocentric moon,
         * in barycentric cartesian equatorial coordinates J2000.
         *
         * these are the data needed for calculation of light-time etc.
         *
         * tjd 		julian date
         * ipli		SEI_ planet number
         * ifno		ephemeris file number
         * do_save	write new positions in save area
         * xp		array of 6 doubles for planet's position and velocity
         * xpe                                 earth's  
         * xps                                 sun's
         * xpm                                 moon's
         * serr		error string
         *
         * xp - xpm can be NULL. if do_save is TRUE, all of them can be NULL.
         * the positions will be written into the save area (swed.pldat[ipli].x)
         */
        int sweplan(double tjd, int ipli, int ifno, Int32 iflag, bool do_save,
                   CPointer<double> xpret, CPointer<double> xperet, CPointer<double> xpsret, CPointer<double> xpmret,
                   ref string serr) {
            int i, retc;
            bool do_earth = false, do_moon = false, do_sunbary = false;
            plan_data pdp = swed.pldat[ipli];
            plan_data pebdp = swed.pldat[SEI_EMB];
            plan_data psbdp = swed.pldat[SEI_SUNBARY];
            plan_data pmdp = swed.pldat[SEI_MOON];
            double[] xxp = new double[6], xxm = new double[6], xxs = new double[6], xxe = new double[6];
            CPointer<double> xp, xpe, xpm, xps;
            Int32 speedf1, speedf2;
            /* xps (barycentric sun) may be necessary because some planets on sweph 
             * file are heliocentric, other ones are barycentric. without xps, 
             * the heliocentric ones cannot be returned barycentrically.
             */
            if (do_save || ipli == SEI_SUNBARY || (pdp.iflg & SEI_FLG_HELIO) != 0
              || xpsret != null || (iflag & SwissEph.SEFLG_HELCTR) != 0)
                do_sunbary = true;
            if (do_save || ipli == SEI_EARTH || xperet != null)
                do_earth = true;
            if (ipli == SEI_MOON) {
                //#if 0
                //  if (iflag & (SEFLG_HELCTR | SEFLG_BARYCTR | SEFLG_NOABERR)) 
                //      do_earth = TRUE;
                //  if (iflag & (SEFLG_HELCTR | SEFLG_NOABERR))
                //      do_sunbary = TRUE;
                //#else
                do_earth = true;
                do_sunbary = true;
                //#endif
            }
            if (do_save || ipli == SEI_MOON || ipli == SEI_EARTH || xperet != null || xpmret != null)
                do_moon = true;
            if (do_save) {
                xp = pdp.x;
                xpe = pebdp.x;
                xps = psbdp.x;
                xpm = pmdp.x;
            } else {
                xp = xxp;
                xpe = xxe;
                xps = xxs;
                xpm = xxm;
            }
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            /* barycentric sun */
            if (do_sunbary) {
                speedf1 = psbdp.xflgs & SwissEph.SEFLG_SPEED;
                /* if planet has already been computed for this date, return 
                 * if speed flag has been turned on, recompute planet */
                if (tjd == psbdp.teval
                  && psbdp.iephe == SwissEph.SEFLG_SWIEPH
                  && (speedf2 == 0 || speedf1 != 0)) {
                    for (i = 0; i <= 5; i++)
                        xps[i] = psbdp.x[i];
                } else {
                    retc = sweph(tjd, SEI_SUNBARY, SEI_FILE_PLANET, iflag, null, do_save, xps, ref serr);/**/
                    if (retc != OK)
                        return (retc);
                }
                if (xpsret != null)
                    for (i = 0; i <= 5; i++)
                        xpsret[i] = xps[i];
            }
            /* moon */
            if (do_moon) {
                speedf1 = pmdp.xflgs & SwissEph.SEFLG_SPEED;
                if (tjd == pmdp.teval
                  && pmdp.iephe == SwissEph.SEFLG_SWIEPH
                  && (speedf2 == 0 || speedf1 != 0)) {
                    for (i = 0; i <= 5; i++)
                        xpm[i] = pmdp.x[i];
                } else {
                    retc = sweph(tjd, SEI_MOON, SEI_FILE_MOON, iflag, null, do_save, xpm, ref serr);
                    if (retc == ERR)
                        return (retc);
                    /* if moon file doesn't exist, take moshier moon */
                    if (swed.fidat[SEI_FILE_MOON].fptr == null) {
                        serr = " \nusing Moshier eph. for moon; ";
                        retc = SE.SwemMoon.swi_moshmoon(tjd, do_save, xpm, ref serr);
                        if (retc != OK)
                            return (retc);
                    }
                }
                if (xpmret != null)
                    for (i = 0; i <= 5; i++)
                        xpmret[i] = xpm[i];
            }
            /* barycentric earth */
            if (do_earth) {
                speedf1 = pebdp.xflgs & SwissEph.SEFLG_SPEED;
                if (tjd == pebdp.teval
                  && pebdp.iephe == SwissEph.SEFLG_SWIEPH
                  && (speedf2 == 0 || speedf1 != 0)) {
                    for (i = 0; i <= 5; i++)
                        xpe[i] = pebdp.x[i];
                } else {
                    retc = sweph(tjd, SEI_EMB, SEI_FILE_PLANET, iflag, null, do_save, xpe, ref serr);
                    if (retc != OK)
                        return (retc);
                    /* earth from emb and moon */
                    embofs(xpe, xpm);
                    /* speed is needed, if
                     * 1. true position is being computed before applying light-time etc.
                     *    this is the position saved in pdp.x.
                     *    in this case, speed is needed for light-time correction.
                     * 2. the speed flag has been specified.
                     */
                    if (xpe == pebdp.x || (iflag & SwissEph.SEFLG_SPEED) != 0)
                        embofs(xpe + 3, xpm + 3);
                }
                if (xperet != null)
                    for (i = 0; i <= 5; i++)
                        xperet[i] = xpe[i];
            }
            if (ipli == SEI_MOON) {
                for (i = 0; i <= 5; i++)
                    xp[i] = xpm[i];
            } else if (ipli == SEI_EARTH) {
                for (i = 0; i <= 5; i++)
                    xp[i] = xpe[i];
            } else if (ipli == SEI_SUN) {
                for (i = 0; i <= 5; i++)
                    xp[i] = xps[i];
            } else {
                /* planet */
                speedf1 = pdp.xflgs & SwissEph.SEFLG_SPEED;
                if (tjd == pdp.teval
                  && pdp.iephe == SwissEph.SEFLG_SWIEPH
                  && (speedf2 == 0 || speedf1 != 0)) {
                    for (i = 0; i <= 5; i++)
                        xp[i] = pdp.x[i];
                    return (OK);
                } else {
                    retc = sweph(tjd, ipli, ifno, iflag, null, do_save, xp, ref serr);
                    if (retc != OK)
                        return (retc);
                    /* if planet is heliocentric, it must be transformed to barycentric */
                    if ((pdp.iflg & SEI_FLG_HELIO) != 0) {
                        /* now barycentric planet */
                        for (i = 0; i <= 2; i++)
                            xp[i] += xps[i];
                        if (do_save || (iflag & SwissEph.SEFLG_SPEED) != 0)
                            for (i = 3; i <= 5; i++)
                                xp[i] += xps[i];
                    }
                }
            }
            if (xpret != null)
                for (i = 0; i <= 5; i++)
                    xpret[i] = xp[i];
            return (OK);
        }

        /* jpl ephemeris.
         * this function computes 
         * 1. a barycentric planet position
         * plus, under certain conditions,
         * 2. the barycentric sun, 
         * 3. the barycentric earth,
         * in barycentric cartesian equatorial coordinates J2000.

         * tjd		julian day
         * ipli		sweph internal planet number
         * do_save	write new positions in save area
         * xp		array of 6 doubles for planet's position and speed vectors
         * xpe		                       earth's
         * xps		                       sun's
         * serr		pointer to error string
         *
         * xp - xps can be NULL. if do_save is TRUE, all of them can be NULL.
         * the positions will be written into the save area (swed.pldat[ipli].x)
         */
        int jplplan(double tjd, int ipli, Int32 iflag, bool do_save,
                   CPointer<double> xpret, CPointer<double> xperet, CPointer<double> xpsret, ref string serr) {
            int i, retc;
            bool do_earth = false, do_sunbary = false;
            double[] ss = new double[3];
            double[] xxp = new double[6], xxe = new double[6], xxs = new double[6];
            CPointer<double> xp, xpe, xps;
            int ictr = SweJPL.J_SBARY;
            plan_data pdp = swed.pldat[ipli];
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            /* we assume Teph ~= TDB ~= TT. The maximum error is < 0.002 sec, 
             * corresponding to an ephemeris error < 0.001 arcsec for the moon */
            /* double tjd_tdb, T;
               T = (tjd - 2451545.0)/36525.0;
               tjd_tdb = tjd + (0.001657 * Math.Sin(628.3076 * T + 6.2401)
                  + 0.000022 * Math.Sin(575.3385 * T + 4.2970)
                  + 0.000014 * Math.Sin(1256.6152 * T + 6.1969)) / 8640.0;*/
            if (do_save) {
                xp = pdp.x;
                xpe = pedp.x;
                xps = psdp.x;
            } else {
                xp = xxp;
                xpe = xxe;
                xps = xxs;
            }
            if (do_save || ipli == SEI_EARTH || xperet != null
              || (ipli == SEI_MOON)) /* && (iflag & (SEFLG_HELCTR | SEFLG_BARYCTR | SEFLG_NOABERR)))) */
                do_earth = true;
            if (do_save || ipli == SEI_SUNBARY || xpsret != null
              || (ipli == SEI_MOON)) /* && (iflag & (SEFLG_HELCTR | SEFLG_NOABERR)))) */
                do_sunbary = true;
            if (ipli == SEI_MOON)
                ictr = SweJPL.J_EARTH;
            /* open ephemeris, if still closed */
            if (!swed.jpl_file_is_open) {
                retc = open_jpl_file(ss, swed.jplfnam, swed.ephepath, ref serr);
                if (retc != OK)
                    return (retc);
            }
            if (do_earth) {
                /* barycentric earth */
                if (tjd != pedp.teval || tjd == 0) {
                    retc = SE.SweJPL.swi_pleph(tjd, SweJPL.J_EARTH, SweJPL.J_SBARY, xpe, ref serr);
                    if (do_save) {
                        pedp.teval = tjd;
                        pedp.xflgs = -1;	/* new light-time etc. required */
                        pedp.iephe = SwissEph.SEFLG_JPLEPH;
                    }
                    if (retc != OK)
                    {
                        SE.SweJPL.swi_close_jpl_file();
                        swed.jpl_file_is_open = false;
                        return retc;
                    }
                }
                else {
                    xpe = pedp.x;
                }
                if (xperet != null)
                    for (i = 0; i <= 5; i++)
                        xperet[i] = xpe[i];

            }
            if (do_sunbary) {
                /* barycentric sun */
                if (tjd != psdp.teval || tjd == 0) {
                    retc = SE.SweJPL.swi_pleph(tjd, SweJPL.J_SUN, SweJPL.J_SBARY, xps, ref serr);
                    if (do_save) {
                        psdp.teval = tjd;
                        psdp.xflgs = -1;
                        psdp.iephe = SwissEph.SEFLG_JPLEPH;
                    }
                    if (retc != OK)
                    {
                        SE.SweJPL.swi_close_jpl_file();
                        swed.jpl_file_is_open = false;
                        return retc;
                    }
                }
                else {
                    xps = psdp.x;
                }
                if (xpsret != null)
                    for (i = 0; i <= 5; i++)
                        xpsret[i] = xps[i];
            }
            /* earth is wanted */
            if (ipli == SEI_EARTH) {
                for (i = 0; i <= 5; i++)
                    xp[i] = xpe[i];
                /* sunbary is wanted */
            } if (ipli == SEI_SUNBARY) {
                for (i = 0; i <= 5; i++)
                    xp[i] = xps[i];
                /* other planet */
            } else {
                /* if planet already computed */
                if (tjd == pdp.teval && pdp.iephe == SwissEph.SEFLG_JPLEPH) {
                    xp = pdp.x;
                } else {
                    retc = SE.SweJPL.swi_pleph(tjd, pnoint2jpl[ipli], ictr, xp, ref serr);
                    if (do_save) {
                        pdp.teval = tjd;
                        pdp.xflgs = -1;
                        pdp.iephe = SwissEph.SEFLG_JPLEPH;
                    }
                    if (retc != OK)
                    {
                        SE.SweJPL.swi_close_jpl_file();
                        swed.jpl_file_is_open = false;
                        return retc;
                    }
                }
            }
            if (xpret != null)
                for (i = 0; i <= 5; i++)
                    xpret[i] = xp[i];
            return (OK);
        }

        /* 
         * this function looks for an ephemeris file, 
         * opens it, if not yet open,
         * reads constants, if not yet read,
         * computes a planet, if not yet computed 
         * attention: asteroids are heliocentric
         *            other planets barycentric
         * 
         * tjd 		julian date
         * ipli		SEI_ planet number
         * ifno		ephemeris file number
         * xsunb	INPUT (!) array of 6 doubles containing barycentric sun
         *              (must be given with asteroids)
         * do_save	boolean: save result in save area
         * xp		return array of 6 doubles for planet's position
         * serr		error string
         */
        int sweph(double tjd, int ipli, int ifno, Int32 iflag, CPointer<double> xsunb, bool do_save, CPointer<double> xpret, ref string serr) {
            int i, ipl, retc, subdirlen;
            string s = String.Empty, subdirnam = String.Empty, fname = String.Empty/*, sp*/;
            int spi;
            double t, tsv;
            double[] xemb = new double[6], xx = new double[6], xp;
            plan_data pdp;
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            file_data fdp = swed.fidat[ifno];
            Int32 speedf1, speedf2;
            bool need_speed;
            ipl = ipli;
            if (ipli > SwissEph.SE_AST_OFFSET)
                ipl = SEI_ANYBODY;
            pdp = swed.pldat[ipl];
            if (do_save)
                xp = pdp.x;
            else
                xp = xx;
            /* if planet has already been computed for this date, return.
             * if speed flag has been turned on, recompute planet */
            speedf1 = pdp.xflgs & SwissEph.SEFLG_SPEED;
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            if (tjd == pdp.teval
              && pdp.iephe == SwissEph.SEFLG_SWIEPH
              && (0 == speedf2 || speedf1 != 0)
                  && ipl < SEI_ANYBODY) {
                if (xpret != null)
                    for (i = 0; i <= 5; i++)
                        xpret[i] = pdp.x[i];
                return (OK);
            }
            /****************************** 
             * get correct ephemeris file * 
             ******************************/
            if (fdp.fptr != null) {
                /* if tjd is beyond file range, close old file.
                 * if new asteroid, close old file. */
                if (tjd < fdp.tfstart || tjd > fdp.tfend
                  || (ipl == SEI_ANYBODY && ipli != pdp.ibdy)) {
                    //fclose(fdp.fptr);
                    fdp.fptr.Dispose();
                    fdp.fptr = null;
                    //if (pdp.refep != null)
                    //    free((void*)pdp.refep);
                    pdp.refep = null;
                    //if (pdp.segp != null)
                    //    free((void*)pdp.segp);
                    pdp.segp = null;
                }
            }
            /* if sweph file not open, find and open it */
            if (fdp.fptr == null) {
                SE.SwephLib.swi_gen_filename(tjd, ipli, out fname);
                //sp = strrchr(subdirnam, (int)*DIR_GLUE);
                //if (sp != NULL) {
                //    *sp = '\0';
                //    subdirlen = strlen(subdirnam);
                //} else {
                //    subdirlen = 0;
                //}
                spi = fname.LastIndexOf(SwissEph.DIR_GLUE);
                if (spi > 0) {
                    subdirnam = fname.Substring(0, spi);
                    subdirlen = subdirnam.Length;
                } else {
                    subdirlen = 0;
                }
                s = fname;
            again:
                fdp.fptr = swi_fopen(ifno, s, swed.ephepath, ref serr);
                if (fdp.fptr == null) {
                    /*
                     * if it is a numbered asteroid file, try also for short files (..s.se1)
                     * On the second try, the inserted 's' will be seen and not tried again.
                     */
                    if (ipli > SwissEph.SE_AST_OFFSET) {
                        int sppi = s.IndexOf('.');
                        //if (spp > s && *(spp - 1) != 's') {	/* no 's' before '.' ? */
                        //    spp=C.sprintf("s.%s", SE_FILE_SUFFIX);	/* insert an 's' */
                        //    goto again;
                        //}
                        if (sppi > 1 && s[sppi - 1] != 's') {
                            s = C.sprintf("%ss.%s", s.Substring(0, sppi), SE_FILE_SUFFIX);
                            goto again;
                        }
                        /*
                         * if we still have 'ast0' etc. in front of the filename, 
                         * we remove it now, remove the 's' also, 
                         * and try in the main ephemeris directory instead of the 
                         * asteroid subdirectory.
                         */
                        sppi--;	/* point to the character before '.' which must be a 's' */
                        // remove the s
                        s = s.Substring(0, sppi) + s.Substring(sppi + 1);
                        if (s.StartsWith(subdirnam)) {
                            s = s.Substring(subdirlen + 1);
                            goto again;
                        }
                    }
                    return (NOT_AVAILABLE);
                }
                /* during the search error messages may have been built, delete them */
                serr = String.Empty;
                retc = read_const(ifno, ref serr);
                if (retc != OK)
                    return (retc);
            }
            /* if first ephemeris file (J-3000), it might start a mars period
             * after -3000. if last ephemeris file (J3000), it might end a
             * 4000-day-period before 3000. */
            if (tjd < fdp.tfstart || tjd > fdp.tfend) {
                if (tjd < fdp.tfstart)
                    s = C.sprintf("jd %f < Swiss Eph. lower limit %f;",
                          tjd, fdp.tfstart);
                else
                    s = C.sprintf("jd %f > Swiss Eph. upper limit %f;",
                          tjd, fdp.tfend);
                //if (strlen(serr) + strlen(s) < AS_MAXCH)
                serr += s;
                return (NOT_AVAILABLE);
            }
            /******************************
             * get planet's position      
             ******************************/
            /* get new segment, if necessary */
            if (pdp.segp == null || tjd < pdp.tseg0 || tjd > pdp.tseg1) {
                retc = get_new_segment(tjd, ipl, ifno, ref serr);
                if (retc != OK)
                    return (retc);
                /* rotate cheby coeffs back to equatorial system.
                 * if necessary, add reference orbit. */
                if ((pdp.iflg & SEI_FLG_ROTATE) != 0)
                    rot_back(ipl); /**/
                else
                    pdp.neval = pdp.ncoe;
            }
            /* evaluate chebyshew polynomial for tjd */
            t = (tjd - pdp.tseg0) / pdp.dseg;
            t = t * 2 - 1;
            /* speed is needed, if
             * 1. true position is being computed before applying light-time etc.
             *    this is the position saved in pdp.x.
             *    in this case, speed is needed for light-time correction.
             * 2. the speed flag has been specified.
             */
            need_speed = (do_save || (iflag & SwissEph.SEFLG_SPEED) != 0);
            for (i = 0; i <= 2; i++) {
                xp[i] = SE.SwephLib.swi_echeb(t, pdp.segp.GetPointer(i * pdp.ncoe), pdp.neval);
                if (need_speed)
                    xp[i + 3] = SE.SwephLib.swi_edcheb(t, pdp.segp.GetPointer(i * pdp.ncoe), pdp.neval) / pdp.dseg * 2;
                else
                    xp[i + 3] = 0;	/* von Alois als billiger fix, evtl. illegal */
            }
            /* if planet wanted is barycentric sun and must be computed
             * from heliocentric earth and barycentric earth: the 
             * computation above gives heliocentric earth, therefore we
             * have to compute barycentric earth and subtract heliocentric
             * earth from it. this may be necessary with calls from 
             * sweplan() and from app_pos_etc_sun() (light-time). */
            if (ipl == SEI_SUNBARY && (pdp.iflg & SEI_FLG_EMBHEL) != 0) {
                /* sweph() calls sweph() !!! for EMB.
                 * Attention: a new calculation must be forced in any case.
                 * Otherwise EARTH (instead of EMB) will possibly taken from 
                 * save area.
                 * to force new computation, set pedp.teval = 0 and restore it
                 * after call of sweph(EMB). 
                 */
                tsv = pedp.teval;
                pedp.teval = 0;
                retc = sweph(tjd, SEI_EMB, ifno, iflag | SwissEph.SEFLG_SPEED, null, NO_SAVE, xemb, ref serr);
                if (retc != OK)
                    return (retc);
                pedp.teval = tsv;
                for (i = 0; i <= 2; i++)
                    xp[i] = xemb[i] - xp[i];
                if (need_speed)
                    for (i = 3; i <= 5; i++)
                        xp[i] = xemb[i] - xp[i];
            }
            //#if 1
            /* asteroids are heliocentric.
             * if JPL or SWISSEPH, convert to barycentric */
            if ((iflag & SwissEph.SEFLG_JPLEPH) != 0 || (iflag & SwissEph.SEFLG_SWIEPH) != 0) {
                if (ipl >= SEI_ANYBODY) {
                    for (i = 0; i <= 2; i++)
                        xp[i] += xsunb[i];
                    if (need_speed)
                        for (i = 3; i <= 5; i++)
                            xp[i] += xsunb[i];
                }
            }
            //#endif
            if (do_save) {
                pdp.teval = tjd;
                pdp.xflgs = -1;	/* do new computation of light-time etc. */
                if (ifno == SEI_FILE_PLANET || ifno == SEI_FILE_MOON)
                    pdp.iephe = SwissEph.SEFLG_SWIEPH;/**/
                else
                    pdp.iephe = psdp.iephe;
            }
            if (xpret != null)
                for (i = 0; i <= 5; i++)
                    xpret[i] = xp[i];
            return (OK);
        }

        /*
         * Alois 2.12.98: inserted error message generation for file not found 
         */
        public CFile swi_fopen(int ifno, string fname, string ephepath, ref string serr) {
            int np, i/*, j*/;
            CFile fp = null;
            string fnamp;
            string[] cpos;
            //char s[2 * AS_MAXCH];
            //char s1[AS_MAXCH];
            string s = String.Empty, s1 = String.Empty;
            //if (ifno >= 0) {
            //    fnamp = swed.fidat[ifno].fnam;
            //} else {
            //    fnamp = fn;
            //}
            s1 = ephepath;
            cpos = s1.Split(new char[] { SwissEph.PATH_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            np = cpos.Length;
            s = String.Empty;
            for (i = 0; i < np; i++) {
                s = cpos[i];
                fnamp = s.TrimEnd('\\', '/') + "\\" + fname;
                if (ifno >= 0) {
                    swed.fidat[ifno].fnam = fnamp;
                }
                fp = SE.LoadFile(fnamp);
                if (fp != null) return fp;
                //    if (strcmp(s, ".") == 0) { /* current directory */
                //      *s = '\0';
                //    } else {
                //      j = strlen(s);
                //      if (*s != '\0' && *(s + j - 1) != *DIR_GLUE)
                //    s+= DIR_GLUE;
                //    }
                //    if (strlen(s) + strlen(fname) < AS_MAXCH) {
                //      s+= fname;
                //    } else {
                //      if (serr != NULL)
                //    serr=C.sprintf("error: file path and name must be shorter than %d.", AS_MAXCH);
                //      return NULL;
                //    }
                //    fnamp= s;
                //    fp = fopen(fnamp, BFILE_R_ACCESS);
                //    if (fp != NULL) 
                //      return fp;
            }
            serr = C.sprintf("SwissEph file '%s' not found in PATH '%s'", fname, ephepath);
            return null;
        }

        Int32 get_denum(Int32 ipli, Int32 iflag)
        {
            file_data fdp = null;
            if ((iflag & SwissEph.SEFLG_MOSEPH) != 0)
                return 403;
            if ((iflag & SwissEph.SEFLG_JPLEPH) != 0)
            {
                if (swed.jpldenum > 0)
                    return swed.jpldenum;
                else
                    return SwissEph.SE_DE_NUMBER;
            }
            if (ipli > SwissEph.SE_AST_OFFSET)
            {
                fdp = swed.fidat[SEI_FILE_ANY_AST];
            }
            else if (ipli == SEI_CHIRON
              || ipli == SEI_PHOLUS
              || ipli == SEI_CERES
              || ipli == SEI_PALLAS
              || ipli == SEI_JUNO
              || ipli == SEI_VESTA)
            {
                fdp = swed.fidat[SEI_FILE_MAIN_AST];
            }
            else if (ipli == SEI_MOON)
            {
                fdp = swed.fidat[SEI_FILE_MOON];
            }
            else
            {
                fdp = swed.fidat[SEI_FILE_PLANET];
            }
            if (fdp != null)
            {
                if (fdp.sweph_denum != 0)
                    return fdp.sweph_denum;
                else
                    return SwissEph.SE_DE_NUMBER;
            }
            return SwissEph.SE_DE_NUMBER;
        }

        /* converts planets from barycentric to geocentric,
         * apparent positions
         * precession and nutation
         * according to flags
         * ipli		planet number
         * iflag	flags
         * serr         error string
         */
        int app_pos_etc_plan(int ipli, Int32 iflag, ref string serr) {
            int i, j, niter, retc = OK;
            int ipl, ifno, ibody;
            Int32 flg1, flg2;
            double[] xx = new double[6], dx = new double[3]; double dt, t, dtsave_for_defl;
            double[] xobs = new double[6], xobs2 = new double[6];
            double[] xearth = new double[6], xsun = new double[6];
            double[] xxsp = new double[6], xxsv = new double[6];
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data pdp;
            epsilon oe = swed.oec2000;
            Int32 epheflag = iflag & SwissEph.SEFLG_EPHMASK;
            t = dtsave_for_defl = 0;	/* dummy assignment to silence gcc */
            /* ephemeris file */
            if (ipli > SwissEph.SE_AST_OFFSET) {
                ifno = SEI_FILE_ANY_AST;
                ibody = IS_ANY_BODY;
                pdp = swed.pldat[SEI_ANYBODY];
            } else if (ipli == SEI_CHIRON
                || ipli == SEI_PHOLUS
                || ipli == SEI_CERES
                || ipli == SEI_PALLAS
                || ipli == SEI_JUNO
                || ipli == SEI_VESTA) {
                ifno = SEI_FILE_MAIN_AST;
                ibody = IS_MAIN_ASTEROID;
                pdp = swed.pldat[ipli];
            } else {
                ifno = SEI_FILE_PLANET;
                ibody = IS_PLANET;
                pdp = swed.pldat[ipli];
            }
            //#if 0
            //  {
            //  struct plan_data *psp = &swed.pldat[SEI_SUNBARY];
            //  printf("planet %.14f %.14f %.14f\n", pdp.x[0], pdp.x[1], pdp.x[2]);
            //  printf("sunbary %.14f %.14f %.14f\n", psp.x[0], psp.x[1], psp.x[2]);
            //  }
            //#endif
            /* if the same conversions have already been done for the same 
             * date, then return */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = pdp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            if (flg1 == flg2) {
                pdp.xflgs = iflag;
                pdp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                return OK;
            }
            /* the conversions will be done with xx[]. */
            for (i = 0; i <= 5; i++)
                xx[i] = pdp.x[i];
            /* if heliocentric position is wanted */
            if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                if (pdp.iephe == SwissEph.SEFLG_JPLEPH || pdp.iephe == SwissEph.SEFLG_SWIEPH)
                    for (i = 0; i <= 5; i++)
                        xx[i] -= swed.pldat[SEI_SUNBARY].x[i];
            }
            /************************************
             * observer: geocenter or topocenter
             ************************************/
            /* if topocentric position is wanted  */
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                if (swed.topd.teval != pedp.teval
                  || swed.topd.teval == 0) {
                      if (swi_get_observer(pedp.teval, iflag | SwissEph.SEFLG_NONUT, DO_SAVE, xobs, ref serr) != OK)
                        return ERR;
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = swed.topd.xobs[i];
                }
                /* barycentric position of observer */
                for (i = 0; i <= 5; i++)
                    xobs[i] = xobs[i] + pedp.x[i];
            } else {
                /* barycentric position of geocenter */
                for (i = 0; i <= 5; i++)
                    xobs[i] = pedp.x[i];
            }
            /*******************************
             * light-time geocentric       * 
             *******************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                /* number of iterations - 1 */
                if (pdp.iephe == SwissEph.SEFLG_JPLEPH || pdp.iephe == SwissEph.SEFLG_SWIEPH)
                    niter = 1;
                else 	/* SwissEph.SEFLG_MOSEPH or planet from osculating elements */
                    niter = 0;
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    /* 
                     * Apparent speed is influenced by the fact that dt changes with
                     * motion. This makes a difference of several hundredths of an
                     * arc second. To take this into account, we compute 
                     * 1. true position - apparent position at time t - 1.
                     * 2. true position - apparent position at time t.
                     * 3. the difference between the two is the part of the daily motion 
                     * that results from the change of dt.
                     */
                    for (i = 0; i <= 2; i++)
                        xxsv[i] = xxsp[i] = xx[i] - xx[i + 3];
                    for (j = 0; j <= niter; j++) {
                        for (i = 0; i <= 2; i++) {
                            dx[i] = xxsp[i];
                            if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR))
                                dx[i] -= (xobs[i] - xobs[i + 3]);
                        }
                        /* new dt */
                        dt = Math.Sqrt(square_sum(dx)) * AUNIT / CLIGHT / 86400.0;
                        for (i = 0; i <= 2; i++) 	/* rough apparent position at t-1 */
                            xxsp[i] = xxsv[i] - dt * pdp.x[i + 3];
                    }
                    /* true position - apparent position at time t-1 */
                    for (i = 0; i <= 2; i++)
                        xxsp[i] = xxsv[i] - xxsp[i];
                }
                /* dt and t(apparent) */
                for (j = 0; j <= niter; j++) {
                    for (i = 0; i <= 2; i++) {
                        dx[i] = xx[i];
                        if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR))
                            dx[i] -= xobs[i];
                    }
                    dt = Math.Sqrt(square_sum(dx)) * AUNIT / CLIGHT / 86400.0;
                    /* new t */
                    t = pdp.teval - dt;
                    dtsave_for_defl = dt;
                    for (i = 0; i <= 2; i++) 		/* rough apparent position at t*/
                        xx[i] = pdp.x[i] - dt * pdp.x[i + 3];
                }
                /* part of daily motion resulting from change of dt */
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    for (i = 0; i <= 2; i++)
                        xxsp[i] = pdp.x[i] - xx[i] - xxsp[i];
                /* new position, accounting for light-time (accurate) */
                switch (epheflag) {
                    case SwissEph.SEFLG_JPLEPH:
                        if (ibody >= IS_ANY_BODY)
                            ipl = -1; /* will not be used */ /*pnoint2jpl[SEI_ANYBODY];*/
                        else
                            ipl = pnoint2jpl[ipli];
                        if (ibody == IS_PLANET) {
                            retc = SE.SweJPL.swi_pleph(t, ipl, SweJPL.J_SBARY, xx, ref serr);
                            if (retc != OK) {
                                SE.SweJPL.swi_close_jpl_file();
                                swed.jpl_file_is_open = false;
                            }
                        } else { 	/* asteroid */
                            /* first sun */
                            retc = SE.SweJPL.swi_pleph(t, SweJPL.J_SUN, SweJPL.J_SBARY, xsun, ref serr);
                            if (retc != OK) {
                                SE.SweJPL.swi_close_jpl_file();
                                swed.jpl_file_is_open = false;
                            }
                            /* asteroid */
                            retc = sweph(t, ipli, ifno, iflag, xsun, NO_SAVE, xx, ref serr);
                        }
                        if (retc != OK)
                            return (retc);
                        /* for accuracy in speed, we need earth as well */
                        if ((iflag & SwissEph.SEFLG_SPEED) != 0
                          && 0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR)) {
                              retc = SE.SweJPL.swi_pleph(t, SweJPL.J_EARTH, SweJPL.J_SBARY, xearth, ref serr);
                            if (retc != OK) {
                                SE.SweJPL.swi_close_jpl_file();
                                swed.jpl_file_is_open = false;
                                return (retc);
                            }
                        }
                        break;
                    case SwissEph.SEFLG_SWIEPH:
                        if (ibody == IS_PLANET)
                            retc = sweplan(t, ipli, ifno, iflag, NO_SAVE, xx, xearth, xsun, null, ref serr);
                        else { 		/*asteroid*/
                            retc = sweplan(t, SEI_EARTH, SEI_FILE_PLANET, iflag, NO_SAVE, xearth, null, xsun, null, ref serr);
                            if (retc == OK)
                                retc = sweph(t, ipli, ifno, iflag, xsun, NO_SAVE, xx, ref serr);
                        }
                        if (retc != OK)
                            return (retc);
                        break;
                    case SwissEph.SEFLG_MOSEPH:
                    default:
                        /* 
                         * with moshier or other ephemerides, subtraction of dt * speed 
                         * is sufficient (has been done in light-time iteration above)
                         */
                        //#if 0
                        //    for (i = 0; i <= 2; i++) {
                        //      xx[i] = pdp.x[i] - dt * pdp.x[i+3];/**/
                        //      xx[i+3] = pdp.x[i+3];
                        //    }
                        //#endif
                        /* if speed flag is true, we call swi_moshplan() for new t.
                     * this does not increase position precision,
                     * but speed precision, which becomes better than 0.01"/day.
                     * for precise speed, we need earth as well.
                     */
                        if ((iflag & SwissEph.SEFLG_SPEED) != 0
                          && 0 == (iflag & (SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR))) {
                            if (ibody == IS_PLANET)
                                retc = SE.SwemPlan.swi_moshplan(t, ipli, NO_SAVE, xxsv, xearth, ref serr);
                            else {		/* if asteroid */
                                retc = sweph(t, ipli, ifno, iflag, null, NO_SAVE, xxsv, ref serr);
                                if (retc == OK)
                                    retc = SE.SwemPlan.swi_moshplan(t, SEI_EARTH, NO_SAVE, xearth, xearth, ref serr);
                            }
                            if (retc != OK)
                                return (retc);
                            /* only speed is taken from this computation, otherwise position
                             * calculations with and without speed would not agree. The difference
                             * would be about 0.01", which is far below the intrinsic error of the
                             * moshier ephemeris.
                             */
                            for (i = 3; i <= 5; i++)
                                xx[i] = xxsv[i];
                        }
                        break;
                }
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                    if (pdp.iephe == SwissEph.SEFLG_JPLEPH || pdp.iephe == SwissEph.SEFLG_SWIEPH)
                        for (i = 0; i <= 5; i++)
                            xx[i] -= swed.pldat[SEI_SUNBARY].x[i];
                }
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    /* observer position for t(light-time) */
                    if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                        if (swi_get_observer(t, iflag | SwissEph.SEFLG_NONUT, NO_SAVE, xobs2, ref serr) != OK)
                            return ERR;
                        for (i = 0; i <= 5; i++)
                            xobs2[i] += xearth[i];
                    } else {
                        for (i = 0; i <= 5; i++)
                            xobs2[i] = xearth[i];
                    }
                }
            }
            /*******************************
             * conversion to geocenter     * 
             *******************************/
            if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR)) {
                /* subtract earth */
                for (i = 0; i <= 5; i++)
                    xx[i] -= xobs[i];
                //#if 0
                //    /* earth and planets are barycentric with jpl and swisseph,
                //     * but asteroids are heliocentric. therefore, add baryctr. sun */
                //    if (ibody != IS_PLANET && !(iflag & SEFLG_MOSEPH)) {
                //      for (i = 0; i <= 5; i++) 
                //    xx[i] += swed.pldat[SEI_SUNBARY].x[i];
                //    }
                //#endif
                if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0) {
                    /* 
                     * Apparent speed is also influenced by
                     * the change of dt during motion.
                     * Neglect of this would result in an error of several 0.01"
                     */
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        for (i = 3; i <= 5; i++)
                            xx[i] -= xxsp[i - 3];
                }
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /************************************
             * relativistic deflection of light *
             ************************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOGDEFL))
                /* SEFLG_NOGDEFL is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_deflect_light(xx, dtsave_for_defl, iflag);
            /**********************************
             * 'annual' aberration of light   *
             **********************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOABERR)) {
                /* SEFLG_NOABERR is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_aberr_light(xx, xobs, iflag);
                /* 
                 * Apparent speed is also influenced by
                 * the difference of speed of the earth between t and t-dt. 
                 * Neglecting this would involve an error of several 0.1"
                 */
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    for (i = 3; i <= 5; i++)
                        xx[i] += xobs[i] - xobs2[i];
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            //#if 0
            //swi_cartpol(xx, xx);
            //xx[0] -= 0.053 / 3600.0 * DEGTORAD;
            //swi_polcart(xx, xx);
            //#endif
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && get_denum(ipli, epheflag) >= 403)
            {
                SE.SwephLib.swi_bias(xx, t, iflag, false);
            }/**/
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = xx[i];
            /************************************************
             * precession, equator 2000 . equator of date *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                SE.SwephLib.swi_precess(xx, pdp.teval, iflag, J2000_TO_J);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, pdp.teval, iflag, J2000_TO_J);
                oe = swed.oec;
            } else {
                oe = swed.oec2000;
            }
            return app_pos_rest(pdp, iflag, xx, xxsv, ref oe, ref serr);
        }

        int app_pos_rest(plan_data pdp, Int32 iflag,
                               CPointer<double> xx, CPointer<double> x2000,
                               ref epsilon oe, ref string serr) {
            int i;
            double daya;
            double[] xxsv = new double[24];
            /************************************************
             * nutation                                     *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_NONUT))
                swi_nutate(xx, iflag, false);
            /* now we have equatorial cartesian coordinates; save them */
            for (i = 0; i <= 5; i++)
                pdp.xreturn[18 + i] = xx[i];
            /************************************************
             * transformation to ecliptic.                  *
             * with sidereal calc. this will be overwritten *
             * afterwards.                                  *
             ************************************************/
            SE.SwephLib.swi_coortrf2(xx, xx, oe.seps, oe.ceps);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, oe.seps, oe.ceps);
            if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                SE.SwephLib.swi_coortrf2(xx, xx, swed.nut.snut, swed.nut.cnut);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, swed.nut.snut, swed.nut.cnut);
            }
            /* now we have ecliptic cartesian coordinates */
            for (i = 0; i <= 5; i++)
                pdp.xreturn[6 + i] = xx[i];
            /************************************
             * sidereal positions               *
             ************************************/
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                /* project onto ecliptic t0 */
                if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0) {
                    if (swi_trop_ra2sid_lon(x2000, pdp.xreturn.GetPointer(6), pdp.xreturn.GetPointer(18), iflag) != OK)
                        return ERR;
                    /* project onto solar system equator */
                } else if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                    if (swi_trop_ra2sid_lon_sosy(x2000, pdp.xreturn.GetPointer(6), iflag) != OK)
                        return ERR;
                } else {
                    /* traditional algorithm */
                    SE.SwephLib.swi_cartpol_sp(pdp.xreturn.GetPointer(6), pdp.xreturn);
                    /* note, swe_get_ayanamsa_ex() disturbs present calculations, if sun is calculated with 
                     * TRUE_CHITRA ayanamsha, because the ayanamsha also calculates the sun.
                     * Therefore current values are saved... */
                    for (i = 0; i < 24; i++)
                        xxsv[i] = pdp.xreturn[i];
                    if (SE.swe_get_ayanamsa_ex(pdp.teval, iflag, out daya, ref serr) == ERR)
                        return ERR;
                    /* ... and restored */
                    for (i = 0; i < 24; i++)
                        pdp.xreturn[i] = xxsv[i];
                    pdp.xreturn[0] -= daya * SwissEph.DEGTORAD;
                    SE.SwephLib.swi_polcart_sp(pdp.xreturn, pdp.xreturn.GetPointer(6));
                }
            }
            /************************************************
             * transformation to polar coordinates          *
             ************************************************/
            SE.SwephLib.swi_cartpol_sp(pdp.xreturn.GetPointer(18), pdp.xreturn.GetPointer(12));
            SE.SwephLib.swi_cartpol_sp(pdp.xreturn.GetPointer(6), pdp.xreturn);
            /********************** 
             * radians to degrees *
             **********************/
            /*if ((iflag & SEFLG_RADIANS) == 0) {*/
            for (i = 0; i < 2; i++) {
                pdp.xreturn[i] *= SwissEph.RADTODEG;		/* ecliptic */
                pdp.xreturn[i + 3] *= SwissEph.RADTODEG;
                pdp.xreturn[i + 12] *= SwissEph.RADTODEG;	/* equator */
                pdp.xreturn[i + 15] *= SwissEph.RADTODEG;
            }
            /*pdp.xreturn[12] -= (0.053 / 3600.0); */
            /*}*/
            /* save, what has been done */
            pdp.xflgs = iflag;
            pdp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
            return OK;
        }

        public void swe_set_sid_mode(Int32 sid_mode, double t0, double ayan_t0) {
            sid_data sip = swed.sidd;
            swi_init_swed_if_start();
            try
            {
                if (sid_mode < 0)
                    sid_mode = 0;
                sip.sid_mode = sid_mode;
                if (sid_mode >= SwissEph.SE_SIDBITS)
                    sid_mode %= SwissEph.SE_SIDBITS;
                /* standard equinoxes: positions always referred to ecliptic of t0 */
                if (sid_mode == SwissEph.SE_SIDM_J2000
                    || sid_mode == SwissEph.SE_SIDM_J1900
                    || sid_mode == SwissEph.SE_SIDM_B1950) {
                    sip.sid_mode &= ~SwissEph.SE_SIDBIT_SSY_PLANE;
                    sip.sid_mode |= SwissEph.SE_SIDBIT_ECL_T0;
                }
                if (sid_mode == SwissEph.SE_SIDM_TRUE_CITRA || sid_mode == SwissEph.SE_SIDM_TRUE_REVATI || sid_mode == SwissEph.SE_SIDM_TRUE_PUSHYA)
                    sip.sid_mode &= ~(SwissEph.SE_SIDBIT_ECL_T0 | SwissEph.SE_SIDBIT_SSY_PLANE);
                if (sid_mode >= SwissEph.SE_NSIDM_PREDEF && sid_mode != SwissEph.SE_SIDM_USER)
                    sip.sid_mode = sid_mode = SwissEph.SE_SIDM_FAGAN_BRADLEY;
                swed.ayana_is_set = true;
                if (sid_mode == SwissEph.SE_SIDM_USER) {
                    sip.t0 = t0;
                    sip.ayan_t0 = ayan_t0;
                } else {
                    sip.t0 = ayanamsa[sid_mode].t0;
                    sip.ayan_t0 = ayanamsa[sid_mode].ayan_t0;
                }
            }
            finally {
                swed.sidd = sip;
            }
            swi_force_app_pos_etc();
        }

        public Int32 swe_get_ayanamsa_ex(double tjd_et, Int32 iflag, out double daya, ref string serr) {
            double[] x = new double[6]; double eps;
            sid_data sip = swed.sidd;
            string star = string.Empty; //string sdummy = null;
            Int32 epheflag, otherflag, retflag;
            iflag = plaus_iflag(iflag, -1, tjd_et, out serr);
            epheflag = iflag & SwissEph.SEFLG_EPHMASK;
            otherflag = iflag & ~SwissEph.SEFLG_EPHMASK;
            daya = 0.0;
            iflag &= SwissEph.SEFLG_EPHMASK;
            iflag |= SwissEph.SEFLG_NONUT;
            /* warning, if swe_set_ephe_path() or swe_set_jplfile() was not called yet,
             * although ephemeris files are required */
            if (swi_init_swed_if_start() == 1 && 0==(epheflag & SwissEph.SEFLG_MOSEPH) && (sip.sid_mode == SwissEph.SE_SIDM_TRUE_CITRA || sip.sid_mode == SwissEph.SE_SIDM_TRUE_REVATI || sip.sid_mode == SwissEph.SE_SIDM_TRUE_PUSHYA))
            {
                serr = "Please call swe_set_ephe_path() or swe_set_jplfile() before calling swe_get_ayanamsa_ex()";
            }
            if (!swed.ayana_is_set)
                swe_set_sid_mode(SwissEph.SE_SIDM_FAGAN_BRADLEY, 0, 0);
            if (sip.sid_mode == SwissEph.SE_SIDM_TRUE_CITRA) {
                star = "Spica"; /* Citra */
                if ((retflag = swe_fixstar(star, tjd_et, iflag, x, ref serr)) == ERR)
                {
                    return ERR;
                }
                /*fprintf(stderr, "serr=%s\n", serr);*/
                daya = SE.swe_degnorm(x[0] - 180);
                return (retflag & SwissEph.SEFLG_EPHMASK);
            }
            if (sip.sid_mode == SwissEph.SE_SIDM_TRUE_REVATI) {
                star = ",zePsc"; /* Revati */
                if ((retflag = swe_fixstar(star, tjd_et, iflag, x, ref serr)) == ERR)
                    return ERR;
                daya = SE.swe_degnorm(x[0]);
                return (retflag & SwissEph.SEFLG_EPHMASK);
                /*return swe_degnorm(x[0] - 359.83333333334);*/
            }
            if (sip.sid_mode == SwissEph.SE_SIDM_TRUE_PUSHYA)
            {
                star = ",deCnc"; /* Pushya = Asellus Australis */
                if ((retflag = swe_fixstar(star, tjd_et, iflag, x, ref serr)) == ERR)
                    return ERR;
                daya = SE.swe_degnorm(x[0] - 106);
                return (retflag & SwissEph.SEFLG_EPHMASK);
            }
            /* vernal point (tjd), cartesian */
            x[0] = 1;
            x[1] = x[2] = 0;
            /* to J2000 */
            if (tjd_et != J2000)
                SE.SwephLib.swi_precess(x, tjd_et, 0, J_TO_J2000);
            /* to t0 */
            SE.SwephLib.swi_precess(x, sip.t0, 0, J2000_TO_J);
            /* to ecliptic */
            eps = SE.SwephLib.swi_epsiln(sip.t0, 0);
            SE.SwephLib.swi_coortrf(x, x, eps);
            /* to polar */
            SE.SwephLib.swi_cartpol(x, x);
            /* subtract initial value of ayanamsa */
            x[0] = x[0] * SwissEph.RADTODEG - sip.ayan_t0;
            /* get ayanamsa */
            daya = SE.swe_degnorm(-x[0]);
            return iflag;
        }

        public Int32 swe_get_ayanamsa_ex_ut(double tjd_ut, Int32 iflag, out double daya, ref string serr)
        {
            double deltat;
            Int32 retflag = OK;
            Int32 epheflag = iflag & SwissEph.SEFLG_EPHMASK;
            if (epheflag == 0)
            {
                epheflag = SwissEph.SEFLG_SWIEPH;
                iflag |= SwissEph.SEFLG_SWIEPH;
            }
            deltat = SE.swe_deltat_ex(tjd_ut, iflag, ref serr);
            retflag = swe_get_ayanamsa_ex(tjd_ut + deltat, iflag, out daya, ref serr);
            /* if ephe required is not ephe returned, adjust delta t: */
            if ((retflag & SwissEph.SEFLG_EPHMASK) != epheflag)
            {
                deltat = SE.swe_deltat_ex(tjd_ut, retflag, ref serr);
                retflag = swe_get_ayanamsa_ex(tjd_ut + deltat, iflag, out daya, ref serr);
            }
            return retflag;
        }

        /* the ayanamsa (precession in longitude) 
         * according to Newcomb's definition: 360 -
         * longitude of the vernal point of t referred to the
         * ecliptic of t0.
         */
        public double swe_get_ayanamsa(double tjd_et)
        {
            double daya;
            string sdummy = null;
            Int32 iflag = SE.SwephLib.swi_guess_ephe_flag();
            swe_get_ayanamsa_ex(tjd_et, iflag, out daya, ref sdummy);
            return daya;
        }

        public double swe_get_ayanamsa_ut(double tjd_ut) {
            double daya;
            string sdummy = null;
            Int32 iflag = SE.SwephLib.swi_guess_ephe_flag();
            swe_get_ayanamsa_ex(tjd_ut + SE.swe_deltat_ex(tjd_ut, iflag, ref sdummy), 0, out daya, ref sdummy);
            return daya;
        }

        /* 
         * input coordinates are J2000, cartesian.
         * xout 	ecliptical sidereal position
         * xoutr 	equatorial sidereal position
         */
        public int swi_trop_ra2sid_lon(CPointer<double> xin, CPointer<double> xout, CPointer<double> xoutr, Int32 iflag) {
            double[] x = new double[6];
            int i;
            sid_data sip = swed.sidd;
            epsilon oectmp = new epsilon();
            for (i = 0; i <= 5; i++)
                x[i] = xin[i];
            if (sip.t0 != J2000) {
                /* iflag must not contain SwissEph.SEFLG_JPLHOR here */
                SE.SwephLib.swi_precess(x, sip.t0, 0, J2000_TO_J);
                SE.SwephLib.swi_precess(x.GetPointer(3), sip.t0, 0, J2000_TO_J);	/* speed */
            }
            for (i = 0; i <= 5; i++)
                xoutr[i] = x[i];
            calc_epsilon(swed.sidd.t0, iflag, oectmp);
            SE.SwephLib.swi_coortrf2(x, x, oectmp.seps, oectmp.ceps);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                SE.SwephLib.swi_coortrf2(x.GetPointer(3), x.GetPointer(3), oectmp.seps, oectmp.ceps);
            /* to polar coordinates */
            SE.SwephLib.swi_cartpol_sp(x, x);
            /* subtract ayan_t0 */
            x[0] -= sip.ayan_t0 * SwissEph.DEGTORAD;
            /* back to cartesian */
            SE.SwephLib.swi_polcart_sp(x, xout);
            return OK;
        }

        /* 
         * input coordinates are J2000, cartesian.
         * xout 	ecliptical sidereal position
         * xoutr 	equatorial sidereal position
         */
        public int swi_trop_ra2sid_lon_sosy(CPointer<double> xin, CPointer<double> xout, Int32 iflag) {
            double[] x = new double[6], x0 = new double[6];
            int i;
            sid_data sip = swed.sidd;
            epsilon oe = swed.oec2000;
            double plane_node = SSY_PLANE_NODE_E2000;
            double plane_incl = SSY_PLANE_INCL;
            for (i = 0; i <= 5; i++)
                x[i] = xin[i];
            /* planet to ecliptic 2000 */
            SE.SwephLib.swi_coortrf2(x, x, oe.seps, oe.ceps);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                SE.SwephLib.swi_coortrf2(x.GetPointer(3), x.GetPointer(3), oe.seps, oe.ceps);
            /* to polar coordinates */
            SE.SwephLib.swi_cartpol_sp(x, x);
            /* to solar system equator */
            x[0] -= plane_node;
            SE.SwephLib.swi_polcart_sp(x, x);
            SE.SwephLib.swi_coortrf(x, x, plane_incl);
            SE.SwephLib.swi_coortrf(x.GetPointer(3), x.GetPointer(3), plane_incl);
            SE.SwephLib.swi_cartpol_sp(x, x);
            /* zero point of t0 in J2000 system */
            x0[0] = 1;
            x0[1] = x0[2] = 0;
            if (sip.t0 != J2000) {
                /* iflag must not contain SEFLG_JPLHOR here */
                SE.SwephLib.swi_precess(x0, sip.t0, 0, J_TO_J2000);
            }
            /* zero point to ecliptic 2000 */
            SE.SwephLib.swi_coortrf2(x0, x0, oe.seps, oe.ceps);
            /* to polar coordinates */
            SE.SwephLib.swi_cartpol(x0, x0);
            /* to solar system equator */
            x0[0] -= plane_node;
            SE.SwephLib.swi_polcart(x0, x0);
            SE.SwephLib.swi_coortrf(x0, x0, plane_incl);
            SE.SwephLib.swi_cartpol(x0, x0);
            /* measure planet from zero point */
            x[0] -= x0[0];
            x[0] *= SwissEph.RADTODEG;
            /* subtract ayan_t0 */
            x[0] -= sip.ayan_t0;
            x[0] = SE.swe_degnorm(x[0]) * SwissEph.DEGTORAD;
            /* back to cartesian */
            SE.SwephLib.swi_polcart_sp(x, xout);
            return OK;
        }

        /* converts planets from barycentric to geocentric,
         * apparent positions
         * precession and nutation
         * according to flags
         * ipli		planet number
         * iflag	flags
         */
        int app_pos_etc_plan_osc(int ipl, int ipli, Int32 iflag, ref string serr) {
            int i, j, niter, retc;
            double[] xx = new double[6], dx = new double[3]; double dt, dtsave_for_defl;
            double[] xearth = new double[6], xsun = new double[6], xmoon = new double[6];
            double[] xxsv = new double[6], xxsp = new double[3] { 0, 0, 0 }, xobs = new double[6], xobs2 = new double[6];
            double t;
            plan_data pdp = swed.pldat[ipli];
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            epsilon oe = swed.oec2000;
            Int32 epheflag = SwissEph.SEFLG_DEFAULTEPH;
            dt = dtsave_for_defl = 0;	/* dummy assign to silence gcc */
            if ((iflag & SwissEph.SEFLG_MOSEPH) != 0)
                epheflag = SwissEph.SEFLG_MOSEPH;
            else if ((iflag & SwissEph.SEFLG_SWIEPH) != 0)
                epheflag = SwissEph.SEFLG_SWIEPH;
            else if ((iflag & SwissEph.SEFLG_JPLEPH) != 0)
                epheflag = SwissEph.SEFLG_JPLEPH;
            /* the conversions will be done with xx[]. */
            for (i = 0; i <= 5; i++)
                xx[i] = pdp.x[i];
            /************************************
             * barycentric position is required *
             ************************************/
            /* = heliocentric position with Moshier ephemeris */
            /************************************
             * observer: geocenter or topocenter
             ************************************/
            /* if topocentric position is wanted  */
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                if (swed.topd.teval != pedp.teval
                  || swed.topd.teval == 0) {
                      if (swi_get_observer(pedp.teval, iflag | SwissEph.SEFLG_NONUT, DO_SAVE, xobs, ref serr) != OK)
                        return ERR;
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = swed.topd.xobs[i];
                }
                /* barycentric position of observer */
                for (i = 0; i <= 5; i++)
                    xobs[i] = xobs[i] + pedp.x[i];
            } else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                for (i = 0; i <= 5; i++)
                    xobs[i] = 0;
            } else if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                if ((iflag & SwissEph.SEFLG_MOSEPH) != 0) {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = 0;
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = psdp.x[i];
                }
            } else {
                for (i = 0; i <= 5; i++)
                    xobs[i] = pedp.x[i];
            }
            /*******************************
             * light-time                  * 
             *******************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                niter = 1;
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    /* 
                     * Apparent speed is influenced by the fact that dt changes with
                     * motion. This makes a difference of several hundredths of an
                     * arc second. To take this into account, we compute 
                     * 1. true position - apparent position at time t - 1.
                     * 2. true position - apparent position at time t.
                     * 3. the difference between the two is the daily motion resulting from
                     * the change of dt.
                     */
                    for (i = 0; i <= 2; i++)
                        xxsv[i] = xxsp[i] = xx[i] - xx[i + 3];
                    for (j = 0; j <= niter; j++) {
                        for (i = 0; i <= 2; i++) {
                            dx[i] = xxsp[i];
                            if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR))
                                dx[i] -= (xobs[i] - xobs[i + 3]);
                        }
                        /* new dt */
                        dt = Math.Sqrt(square_sum(dx)) * AUNIT / CLIGHT / 86400.0;
                        for (i = 0; i <= 2; i++)
                            xxsp[i] = xxsv[i] - dt * pdp.x[i + 3];/* rough apparent position */
                    }
                    /* true position - apparent position at time t-1 */
                    for (i = 0; i <= 2; i++)
                        xxsp[i] = xxsv[i] - xxsp[i];
                }
                /* dt and t(apparent) */
                for (j = 0; j <= niter; j++) {
                    for (i = 0; i <= 2; i++) {
                        dx[i] = xx[i];
                        if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR))
                            dx[i] -= xobs[i];
                    }
                    /* new dt */
                    dt = Math.Sqrt(square_sum(dx)) * AUNIT / CLIGHT / 86400.0;
                    dtsave_for_defl = dt;
                    /* new position: subtract t * speed 
                     */
                    for (i = 0; i <= 2; i++) {
                        xx[i] = pdp.x[i] - dt * pdp.x[i + 3];/**/
                        xx[i + 3] = pdp.x[i + 3];
                    }
                }
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    /* part of daily motion resulting from change of dt */
                    for (i = 0; i <= 2; i++)
                        xxsp[i] = pdp.x[i] - xx[i] - xxsp[i];
                    t = pdp.teval - dt;
                    /* for accuracy in speed, we will need earth as well */
                    retc = main_planet_bary(t, SEI_EARTH, epheflag, iflag, NO_SAVE, xearth, xearth, xsun, xmoon, ref serr);
                    if (SE.SwemPlan.swi_osc_el_plan(t, xx, ipl - SwissEph.SE_FICT_OFFSET, ipli, xearth, xsun, ref serr) != OK)
                        return ERR;
                    if (retc != OK)
                        return (retc);
                    if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                        if (swi_get_observer(t, iflag | SwissEph.SEFLG_NONUT, NO_SAVE, xobs2, ref serr) != OK)
                            return ERR;
                        for (i = 0; i <= 5; i++)
                            xobs2[i] += xearth[i];
                    } else {
                        for (i = 0; i <= 5; i++)
                            xobs2[i] = xearth[i];
                    }
                }
            }
            /*******************************
             * conversion to geocenter     * 
             *******************************/
            for (i = 0; i <= 5; i++)
                xx[i] -= xobs[i];
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                /* 
                 * Apparent speed is also influenced by
                 * the change of dt during motion.
                 * Neglect of this would result in an error of several 0.01"
                 */
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    for (i = 3; i <= 5; i++)
                        xx[i] -= xxsp[i - 3];
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /************************************
             * relativistic deflection of light *
             ************************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOGDEFL))
                /* SEFLG_NOGDEFL is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_deflect_light(xx, dtsave_for_defl, iflag);
            /**********************************
             * 'annual' aberration of light   *
             **********************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOABERR)) {
                /* SEFLG_NOABERR is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_aberr_light(xx, xobs, iflag);
                /* 
                 * Apparent speed is also influenced by
                 * the difference of speed of the earth between t and t-dt. 
                 * Neglecting this would involve an error of several 0.1"
                 */
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    for (i = 3; i <= 5; i++)
                        xx[i] += xobs[i] - xobs2[i];
            }
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = xx[i];
            /************************************************
             * precession, equator 2000 . equator of date *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                SE.SwephLib.swi_precess(xx, pdp.teval, iflag, J2000_TO_J);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, pdp.teval, iflag, J2000_TO_J);
                oe = swed.oec;
            } else
                oe = swed.oec2000;
            return app_pos_rest(pdp, iflag, xx, xxsv, ref oe, ref serr);
        }

        /* influence of precession on speed 
         * xx		position and speed of planet in equatorial cartesian
         *		coordinates */
        public void swi_precess_speed(CPointer<double> xx, double t, Int32 iflag, int direction) {
            epsilon oe;
            double fac, dpre, dpre2, ddummy;
            double tprec = (t - J2000) / 36525.0;
            int prec_model = swed.astro_models[SwissEph.SE_MODEL_PREC_LONGTERM];
            if (prec_model == 0) prec_model = SwissEph.SEMOD_PREC_DEFAULT;
            if (direction == J2000_TO_J)
            {
                fac = 1;
                oe = swed.oec;
            } else {
                fac = -1;
                oe = swed.oec2000;
            }
            /* first correct rotation.
             * this costs some sines and cosines, but neglect might
             * involve an error > 1"/day */
            SE.SwephLib.swi_precess(xx + 3, t, iflag, direction);
            /* then add 0.137"/day */
            SE.SwephLib.swi_coortrf2(xx, xx, oe.seps, oe.ceps);
            SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, oe.seps, oe.ceps);
            SE.SwephLib.swi_cartpol_sp(xx, xx);
            if (prec_model == SwissEph.SEMOD_PREC_VONDRAK_2011) {
                SE.SwephLib.swi_ldp_peps(t, out dpre, out ddummy);
                SE.SwephLib.swi_ldp_peps(t + 1, out dpre2, out ddummy);
                xx[3] += (dpre2 - dpre) * fac;
            } else {
                xx[3] += (50.290966 + 0.0222226 * tprec) / 3600 / 365.25 * SwissEph.DEGTORAD * fac;
                /* formula from Montenbruck, German 1994, p. 18 */
            }
            SE.SwephLib.swi_polcart_sp(xx, xx);
            SE.SwephLib.swi_coortrf2(xx, xx, -oe.seps, oe.ceps);
            SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, -oe.seps, oe.ceps);
        }

        /* multiplies cartesian equatorial coordinates with previously
         * calculated nutation matrix. also corrects speed. 
         */
        public void swi_nutate(CPointer<double> xx, Int32 iflag, bool backward) {
            int i;
            double[] x = new double[6], xv = new double[6];
            for (i = 0; i <= 2; i++) {
                if (backward)
                    x[i] = xx[0] * swed.nut.matrix[i, 0] +
                       xx[1] * swed.nut.matrix[i, 1] +
                       xx[2] * swed.nut.matrix[i, 2];
                else
                    x[i] = xx[0] * swed.nut.matrix[0, i] +
                       xx[1] * swed.nut.matrix[1, i] +
                       xx[2] * swed.nut.matrix[2, i];
            }
            if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                /* correct speed:
                 * first correct rotation */
                for (i = 0; i <= 2; i++) {
                    if (backward)
                        x[i + 3] = xx[3] * swed.nut.matrix[i, 0] +
                             xx[4] * swed.nut.matrix[i, 1] +
                             xx[5] * swed.nut.matrix[i, 2];
                    else
                        x[i + 3] = xx[3] * swed.nut.matrix[0, i] +
                             xx[4] * swed.nut.matrix[1, i] +
                             xx[5] * swed.nut.matrix[2, i];
                }
                /* then apparent motion due to change of nutation during day.
                 * this makes a difference of 0.01" */
                for (i = 0; i <= 2; i++) {
                    if (backward)
                        xv[i] = xx[0] * swed.nutv.matrix[i, 0] +
                               xx[1] * swed.nutv.matrix[i, 1] +
                               xx[2] * swed.nutv.matrix[i, 2];
                    else
                        xv[i] = xx[0] * swed.nutv.matrix[0, i] +
                               xx[1] * swed.nutv.matrix[1, i] +
                               xx[2] * swed.nutv.matrix[2, i];
                    /* new speed */
                    xx[3 + i] = x[3 + i] + (x[i] - xv[i]) / NUT_SPEED_INTV;
                }
            }
            /* new position */
            for (i = 0; i <= 2; i++)
                xx[i] = x[i];
        }

        /* computes 'annual' aberration
         * xx		planet's position accounted for light-time 
         *              and gravitational light deflection
         * xe    	earth's position and speed
         */
        public void swi_aberr_light(CPointer<double> xx, CPointer<double> xe, Int32 iflag) {
            int i;
            double[] xxs = new double[6], v = new double[6], u = new double[6]; double ru;
            double[] xx2 = new double[6]; double dx1, dx2;
            double b_1, f1, f2;
            double v2;
            double intv = PLAN_SPEED_INTV;
            for (i = 0; i <= 5; i++)
                u[i] = xxs[i] = xx[i];
            ru = Math.Sqrt(square_sum(u));
            for (i = 0; i <= 2; i++)
                v[i] = xe[i + 3] / 24.0 / 3600.0 / CLIGHT * AUNIT;
            v2 = square_sum(v);
            b_1 = Math.Sqrt(1 - v2);
            f1 = dot_prod(u, v) / ru;
            f2 = 1.0 + f1 / (1.0 + b_1);
            for (i = 0; i <= 2; i++)
                xx[i] = (b_1 * xx[i] + f2 * ru * v[i]) / (1.0 + f1);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                /* correction of speed
                 * the influence of aberration on apparent velocity can
                 * reach 0.4"/day
                 */
                for (i = 0; i <= 2; i++)
                    u[i] = xxs[i] - intv * xxs[i + 3];
                ru = Math.Sqrt(square_sum(u));
                f1 = dot_prod(u, v) / ru;
                f2 = 1.0 + f1 / (1.0 + b_1);
                for (i = 0; i <= 2; i++)
                    xx2[i] = (b_1 * u[i] + f2 * ru * v[i]) / (1.0 + f1);
                for (i = 0; i <= 2; i++) {
                    dx1 = xx[i] - xxs[i];
                    dx2 = xx2[i] - u[i];
                    dx1 -= dx2;
                    xx[i + 3] += dx1 / intv;
                }
            }
        }

        /* computes relativistic light deflection by the sun
         * ipli 	sweph internal planet number 
         * xx		planet's position accounted for light-time
         * dt		dt of light-time
         */
        public void swi_deflect_light(CPointer<double> xx, double dt, Int32 iflag) {
            int i;
            double[] xx2 = new double[6];
            double[] u = new double[6], e = new double[6], q = new double[6]; double ru, re, rq, uq, ue, qe, g1, g2;
            //#if 1
            double[] xx3 = new double[6]; double dx1, dx2, dtsp;
            //#endif
            double[] xsun = new double[6], xearth = new double[6];
            double sina, sin_sunr, meff_fact;
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            Int32 iephe = pedp.iephe;
            for (i = 0; i <= 5; i++)
                xearth[i] = pedp.x[i];
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0)
                for (i = 0; i <= 5; i++)
                    xearth[i] += swed.topd.xobs[i];
            /* U = planetbary(t-tau) - earthbary(t) = planetgeo */
            for (i = 0; i <= 2; i++)
                u[i] = xx[i];
            /* Eh = earthbary(t) - sunbary(t) = earthhel */
            if (iephe == SwissEph.SEFLG_JPLEPH || iephe == SwissEph.SEFLG_SWIEPH)
                for (i = 0; i <= 2; i++)
                    e[i] = xearth[i] - psdp.x[i];
            else
                for (i = 0; i <= 2; i++)
                    e[i] = xearth[i];
            /* Q = planetbary(t-tau) - sunbary(t-tau) = 'planethel' */
            /* first compute sunbary(t-tau) for */
            if (iephe == SwissEph.SEFLG_JPLEPH || iephe == SwissEph.SEFLG_SWIEPH) {
                for (i = 0; i <= 2; i++)
                    /* this is sufficient precision */
                    xsun[i] = psdp.x[i] - dt * psdp.x[i + 3];
                for (i = 3; i <= 5; i++)
                    xsun[i] = psdp.x[i];
            } else {
                for (i = 0; i <= 5; i++)
                    xsun[i] = psdp.x[i];
            }
            for (i = 0; i <= 2; i++)
                q[i] = xx[i] + xearth[i] - xsun[i];
            ru = Math.Sqrt(square_sum(u));
            rq = Math.Sqrt(square_sum(q));
            re = Math.Sqrt(square_sum(e));
            for (i = 0; i <= 2; i++) {
                u[i] /= ru;
                q[i] /= rq;
                e[i] /= re;
            }
            uq = dot_prod(u, q);
            ue = dot_prod(u, e);
            qe = dot_prod(q, e);
            /* When a planet approaches the center of the sun in superior
             * conjunction, the formula for the deflection angle as given
             * in Expl. Suppl. p. 136 cannot be used. The deflection seems
             * to increase rapidly towards infinity. The reason is that the 
             * formula considers the sun as a point mass. AA recommends to 
             * set deflection = 0 in such a case. 
             * However, to get a continous motion, we modify the formula
             * for a non-point-mass, taking into account the mass distribution
             * within the sun. For more info, s. meff().
             */
            sina = Math.Sqrt(1 - ue * ue);	/* sin(angle) between sun and planet */
            sin_sunr = SUN_RADIUS / re; 	/* sine of sun radius (= sun radius) */
            if (sina < sin_sunr)
                meff_fact = meff(sina / sin_sunr);
            else
                meff_fact = 1;
            g1 = 2.0 * HELGRAVCONST * meff_fact / CLIGHT / CLIGHT / AUNIT / re;
            g2 = 1.0 + qe;
            /* compute deflected position */
            for (i = 0; i <= 2; i++)
                xx2[i] = ru * (u[i] + g1 / g2 * (uq * e[i] - ue * q[i]));
            if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                /* correction of speed
                 * influence of light deflection on a planet's apparent speed:
                 * for an outer planet at the solar limb with 
                 * |v(planet) - v(sun)| = 1 degree, this makes a difference of 7"/day. 
                 * if the planet is within the solar disc, the difference may increase
                 * to 30" or more.
                 * e.g. mercury at j2434871.45: 
                 *	distance from sun 		45"
                 *	1. speed without deflection     2d10'10".4034
                 *    2. speed with deflection        2d10'42".8460 (-speed flag)
                 *    3. speed with deflection        2d10'43".4824 (< 3 positions/
                 *							   -speed3 flag)
                 * 3. is not very precise. Smaller dt would give result closer to 2.,
                 * but will probably never be as good as 2, unless int32 doubles are 
                 * used. (try also j2434871.46!!)
                 * however, in such a case speed changes rapidly. before being
                 * passed by the sun, the planet accelerates, and after the sun
                 * has passed it slows down. some time later it regains 'normal'
                 * speed.
                 * to compute speed, we do the same calculation as above with
                 * slightly different u, e, q, and find out the difference in
                 * deflection.
                 */
                dtsp = -DEFL_SPEED_INTV;
                /* U = planetbary(t-tau) - earthbary(t) = planetgeo */
                for (i = 0; i <= 2; i++)
                    u[i] = xx[i] - dtsp * xx[i + 3];
                /* Eh = earthbary(t) - sunbary(t) = earthhel */
                if (iephe == SwissEph.SEFLG_JPLEPH || iephe == SwissEph.SEFLG_SWIEPH) {
                    for (i = 0; i <= 2; i++)
                        e[i] = xearth[i] - psdp.x[i] -
                               dtsp * (xearth[i + 3] - psdp.x[i + 3]);
                } else
                    for (i = 0; i <= 2; i++)
                        e[i] = xearth[i] - dtsp * xearth[i + 3];
                /* Q = planetbary(t-tau) - sunbary(t-tau) = 'planethel' */
                for (i = 0; i <= 2; i++)
                    q[i] = u[i] + xearth[i] - xsun[i] -
                       dtsp * (xearth[i + 3] - xsun[i + 3]);
                ru = Math.Sqrt(square_sum(u));
                rq = Math.Sqrt(square_sum(q));
                re = Math.Sqrt(square_sum(e));
                for (i = 0; i <= 2; i++) {
                    u[i] /= ru;
                    q[i] /= rq;
                    e[i] /= re;
                }
                uq = dot_prod(u, q);
                ue = dot_prod(u, e);
                qe = dot_prod(q, e);
                sina = Math.Sqrt(1 - ue * ue);	/* sin(angle) between sun and planet */
                sin_sunr = SUN_RADIUS / re; 	/* sine of sun radius (= sun radius) */
                if (sina < sin_sunr)
                    meff_fact = meff(sina / sin_sunr);
                else
                    meff_fact = 1;
                g1 = 2.0 * HELGRAVCONST * meff_fact / CLIGHT / CLIGHT / AUNIT / re;
                g2 = 1.0 + qe;
                for (i = 0; i <= 2; i++)
                    xx3[i] = ru * (u[i] + g1 / g2 * (uq * e[i] - ue * q[i]));
                for (i = 0; i <= 2; i++) {
                    dx1 = xx2[i] - xx[i];
                    dx2 = xx3[i] - u[i] * ru;
                    dx1 -= dx2;
                    xx[i + 3] += dx1 / dtsp;
                }
            } /* endif speed */
            /* deflected position */
            for (i = 0; i <= 2; i++)
                xx[i] = xx2[i];
        }

        /* converts the sun from barycentric to geocentric,
         *          the earth from barycentric to heliocentric
         * computes
         * apparent position,
         * precession, and nutation
         * according to flags
         * iflag	flags
         * serr         error string
         */
        int app_pos_etc_sun(Int32 iflag, ref string serr) {
            int i, j, niter, retc = OK;
            Int32 flg1, flg2;
            double[] xx = new double[6], xxsv = new double[6], dx = new double[3]; double dt = 0, t = 0;
            double[] xearth = new double[6], xsun = new double[6], xobs = new double[6];
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            epsilon oe = swed.oec2000;
            /* if the same conversions have already been done for the same 
             * date, then return */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = pedp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            if (flg1 == flg2) {
                pedp.xflgs = iflag;
                pedp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                return OK;
            }
            /************************************
             * observer: geocenter or topocenter
             ************************************/
            /* if topocentric position is wanted  */
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                if (swed.topd.teval != pedp.teval
                  || swed.topd.teval == 0) {
                      if (swi_get_observer(pedp.teval, iflag | SwissEph.SEFLG_NONUT, DO_SAVE, xobs, ref serr) != OK)
                        return ERR;
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = swed.topd.xobs[i];
                }
                /* barycentric position of observer */
                for (i = 0; i <= 5; i++)
                    xobs[i] = xobs[i] + pedp.x[i];
            } else {
                /* barycentric position of geocenter */
                for (i = 0; i <= 5; i++)
                    xobs[i] = pedp.x[i];
            }
            /***************************************
             * true heliocentric position of earth *
             ***************************************/
            if (pedp.iephe == SwissEph.SEFLG_MOSEPH || (iflag & SwissEph.SEFLG_BARYCTR) != 0)
                for (i = 0; i <= 5; i++)
                    xx[i] = xobs[i];
            else
                for (i = 0; i <= 5; i++)
                    xx[i] = xobs[i] - psdp.x[i];
            /*******************************
             * light-time                  * 
             *******************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                /* number of iterations - 1 
                 * the following if() does the following:
                 * with jpl and swiss ephemeris:
                 *   with geocentric computation of sun:
                 *     light-time correction of barycentric sun position.
                 *   with heliocentric or barycentric computation of earth:
                 *     light-time correction of barycentric earth position.
                 * with moshier ephemeris (heliocentric!!!): 
                 *   with geocentric computation of sun:
                 *     nothing! (aberration will be done later)
                 *   with heliocentric or barycentric computation of earth:
                 *     light-time correction of heliocentric earth position.
                 */
                if (pedp.iephe == SwissEph.SEFLG_JPLEPH || pedp.iephe == SwissEph.SEFLG_SWIEPH
                  || (iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    for (i = 0; i <= 5; i++) {
                        xearth[i] = xobs[i];
                        if (pedp.iephe == SwissEph.SEFLG_MOSEPH)
                            xsun[i] = 0;
                        else
                            xsun[i] = psdp.x[i];
                    }
                    niter = 1;	/* # of iterations */
                    for (j = 0; j <= niter; j++) {
                        /* distance earth-sun */
                        for (i = 0; i <= 2; i++) {
                            dx[i] = xearth[i];
                            if (0 == (iflag & SwissEph.SEFLG_BARYCTR))
                                dx[i] -= xsun[i];
                        }
                        /* new t */
                        dt = Math.Sqrt(square_sum(dx)) * AUNIT / CLIGHT / 86400.0;
                        t = pedp.teval - dt;
                        /* new position */
                        switch (pedp.iephe) {
                            /* if geocentric sun, new sun at t' 
                             * if heliocentric or barycentric earth, new earth at t' */
                            case SwissEph.SEFLG_JPLEPH:
                                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0)
                                    retc = SE.SweJPL.swi_pleph(t, SweJPL.J_EARTH, SweJPL.J_SBARY, xearth, ref serr);
                                else
                                    retc = SE.SweJPL.swi_pleph(t, SweJPL.J_SUN, SweJPL.J_SBARY, xsun, ref serr);
                                if (retc != OK) {
                                    SE.SweJPL.swi_close_jpl_file();
                                    swed.jpl_file_is_open = false;
                                    return (retc);
                                }
                                break;
                            case SwissEph.SEFLG_SWIEPH:
                                /*
                                  retc = sweph(t, SEI_SUN, SEI_FILE_PLANET, iflag, NULL, NO_SAVE, xearth, serr);
                                */
                                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0)
                                    retc = sweplan(t, SEI_EARTH, SEI_FILE_PLANET, iflag, NO_SAVE, xearth, null, xsun, null, ref serr);
                                else
                                    retc = sweph(t, SEI_SUNBARY, SEI_FILE_PLANET, iflag, null, NO_SAVE, xsun, ref serr);
                                break;
                            case SwissEph.SEFLG_MOSEPH:
                                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 || (iflag & SwissEph.SEFLG_BARYCTR) != 0)
                                    retc = SE.SwemPlan.swi_moshplan(t, SEI_EARTH, NO_SAVE, xearth, xearth, ref serr);
                                /* with moshier there is no barycentric sun */
                                break;
                            default:
                                retc = ERR;
                                break;
                        }
                        if (retc != OK)
                            return (retc);
                    }
                    /* apparent heliocentric earth */
                    for (i = 0; i <= 5; i++) {
                        xx[i] = xearth[i];
                        if (0 == (iflag & SwissEph.SEFLG_BARYCTR))
                            xx[i] -= xsun[i];
                    }
                }
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /*******************************
             * conversion to geocenter     * 
             *******************************/
            if (0 == (iflag & SwissEph.SEFLG_HELCTR) && 0 == (iflag & SwissEph.SEFLG_BARYCTR))
                for (i = 0; i <= 5; i++)
                    xx[i] = -xx[i];
            /**********************************
             * 'annual' aberration of light   *
             **********************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOABERR)) {
                /* SEFLG_NOABERR is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_aberr_light(xx, xobs, iflag);
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && get_denum(SEI_SUN, iflag) >= 403) {
                SE.SwephLib.swi_bias(xx, t, iflag, false);
            }/**/
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = xx[i];
            /************************************************
             * precession, equator 2000 -> equator of date *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                SE.SwephLib.swi_precess(xx, pedp.teval, iflag, J2000_TO_J);/**/
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, pedp.teval, iflag, J2000_TO_J);/**/
                oe = swed.oec;
            } else
                oe = swed.oec2000;
            return app_pos_rest(pedp, iflag, xx, xxsv, ref oe, ref serr);
        }


        /* transforms the position of the moon:
         * heliocentric position
         * barycentric position
         * astrometric position
         * apparent position
         * precession and nutation
         * 
         * note: 
         * for apparent positions, we consider the earth-moon
         * system as independant.
         * for astrometric positions (SEFLG_NOABERR), we 
         * consider the motions of the earth and the moon 
         * related to the solar system barycenter.
         */
        int app_pos_etc_moon(Int32 iflag, ref string serr) {
            int i;
            Int32 flg1, flg2;
            double[] xx = new double[6], xxsv = new double[6], xobs = new double[6], xxm = new double[6], xs = new double[6], xe = new double[6], xobs2 = new double[6];
            double dt; string sdummy = null;
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            plan_data pdp = swed.pldat[SEI_MOON];
            epsilon oe = swed.oec;
            double t = 0;
            Int32 retc;
            /* if the same conversions have already been done for the same 
             * date, then return */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = pdp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            if (flg1 == flg2) {
                pdp.xflgs = iflag;
                pdp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                return OK;
            }
            /* the conversions will be done with xx[]. */
            for (i = 0; i <= 5; i++) {
                xx[i] = pdp.x[i];
                xxm[i] = xx[i];
            }
            /***********************************
             * to solar system barycentric
             ***********************************/
            for (i = 0; i <= 5; i++)
                xx[i] += pedp.x[i];
            /*******************************
             * observer
             *******************************/
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                if (swed.topd.teval != pdp.teval
                  || swed.topd.teval == 0) {
                      if (swi_get_observer(pdp.teval, iflag | SwissEph.SEFLG_NONUT, DO_SAVE, xobs, ref sdummy) != OK)
                        return ERR;
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs[i] = swed.topd.xobs[i];
                }
                for (i = 0; i <= 5; i++)
                    xxm[i] -= xobs[i];
                for (i = 0; i <= 5; i++)
                    xobs[i] += pedp.x[i];
            } else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                for (i = 0; i <= 5; i++)
                    xobs[i] = 0;
                for (i = 0; i <= 5; i++)
                    xxm[i] += pedp.x[i];
            } else if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                for (i = 0; i <= 5; i++)
                    xobs[i] = psdp.x[i];
                for (i = 0; i <= 5; i++)
                    xxm[i] += pedp.x[i] - psdp.x[i];
            } else {
                for (i = 0; i <= 5; i++)
                    xobs[i] = pedp.x[i];
            }
            /*******************************
             * light-time                  * 
             *******************************/
            if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0) {
                dt = Math.Sqrt(square_sum(xxm)) * AUNIT / CLIGHT / 86400.0;
                t = pdp.teval - dt;
                switch (pdp.iephe) {
                    case SwissEph.SEFLG_JPLEPH:
                        retc = SE.SweJPL.swi_pleph(t, SweJPL.J_MOON, SweJPL.J_EARTH, xx, ref serr);
                        if (retc == OK)
                            retc = SE.SweJPL.swi_pleph(t, SweJPL.J_EARTH, SweJPL.J_SBARY, xe, ref serr);
                        if (retc == OK && (iflag & SwissEph.SEFLG_HELCTR) != 0)
                            retc = SE.SweJPL.swi_pleph(t, SweJPL.J_SUN, SweJPL.J_SBARY, xs, ref serr);
                        if (retc != OK) {
                            SE.SweJPL.swi_close_jpl_file();
                            swed.jpl_file_is_open = false;
                        }
                        for (i = 0; i <= 5; i++)
                            xx[i] += xe[i];
                        break;
                    case SwissEph.SEFLG_SWIEPH:
                        retc = sweplan(t, SEI_MOON, SEI_FILE_MOON, iflag, NO_SAVE, xx, xe, xs, null, ref serr);
                        if (retc != OK)
                            return (retc);
                        for (i = 0; i <= 5; i++)
                            xx[i] += xe[i];
                        break;
                    case SwissEph.SEFLG_MOSEPH:
                        /* this method results in an error of a milliarcsec in speed */
                        for (i = 0; i <= 2; i++) {
                            xx[i] -= dt * xx[i + 3];
                            xe[i] = pedp.x[i] - dt * pedp.x[i + 3];
                            xe[i + 3] = pedp.x[i + 3];
                            xs[i] = 0;
                            xs[i + 3] = 0;
                        }
                        break;
                }
                if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                    if (swi_get_observer(t, iflag | SwissEph.SEFLG_NONUT, NO_SAVE, xobs2, ref sdummy) != OK)
                        return ERR;
                    for (i = 0; i <= 5; i++)
                        xobs2[i] += xe[i];
                } else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0) {
                    for (i = 0; i <= 5; i++)
                        xobs2[i] = 0;
                } else if ((iflag & SwissEph.SEFLG_HELCTR) != 0) {
                    for (i = 0; i <= 5; i++)
                        xobs2[i] = xs[i];
                } else {
                    for (i = 0; i <= 5; i++)
                        xobs2[i] = xe[i];
                }
            }
            /*************************
             * to correct center 
             *************************/
            for (i = 0; i <= 5; i++)
                xx[i] -= xobs[i];
            /**********************************
             * 'annual' aberration of light   *
             **********************************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOABERR)) {
                /* SEFLG_NOABERR is on, if SEFLG_HELCTR or SEFLG_BARYCTR */
                swi_aberr_light(xx, xobs, iflag);
                /* 
                 * Apparent speed is also influenced by
                 * the difference of speed of the earth between t and t-dt. 
                 * Neglecting this would lead to an error of several 0.1"
                 */
                //#if 1
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    for (i = 3; i <= 5; i++)
                        xx[i] += xobs[i] - xobs2[i];
                //#endif
            }
            /* if !speedflag, speed = 0 */
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && get_denum(SEI_MOON, iflag) >= 403) {
                SE.SwephLib.swi_bias(xx, t, iflag, false);
            }/**/
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = xx[i];
            /************************************************
             * precession, equator 2000 -> equator of date *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                SE.SwephLib.swi_precess(xx, pdp.teval, iflag, J2000_TO_J);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, pdp.teval, iflag, J2000_TO_J);
                oe = swed.oec;
            } else
                oe = swed.oec2000;
            return app_pos_rest(pdp, iflag, xx, xxsv, ref oe, ref serr);
        }

        /* transforms the position of the barycentric sun:
         * precession and nutation
         * according to flags
         * iflag	flags
         * serr         error string
         */
        int app_pos_etc_sbar(Int32 iflag, ref string serr) {
            int i;
            double[] xx = new double[6], xxsv = new double[6]; double dt;
            plan_data psdp = swed.pldat[SEI_EARTH];
            plan_data psbdp = swed.pldat[SEI_SUNBARY];
            epsilon oe = swed.oec;
            /* the conversions will be done with xx[]. */
            for (i = 0; i <= 5; i++)
                xx[i] = psbdp.x[i];
            /**************
             * light-time *
             **************/
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                dt = Math.Sqrt(square_sum(xx)) * AUNIT / CLIGHT / 86400.0;
                for (i = 0; i <= 2; i++)
                    xx[i] -= dt * xx[i + 3];	/* apparent position */
            }
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && get_denum(SEI_SUN, iflag) >= 403) {
                SE.SwephLib.swi_bias(xx, psdp.teval, iflag, false);
            }/**/
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = xx[i];
            /************************************************
             * precession, equator 2000 -> equator of date *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                SE.SwephLib.swi_precess(xx, psbdp.teval, iflag, J2000_TO_J);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, psbdp.teval, iflag, J2000_TO_J);
                oe = swed.oec;
            } else
                oe = swed.oec2000;
            return app_pos_rest(psdp, iflag, xx, xxsv, ref oe, ref serr);
        }

        /* transforms position of mean lunar node or apogee:
         * input is polar coordinates in mean ecliptic of date.
         * output is, according to iflag:
         * position accounted for light-time
         * position referred to J2000 (i.e. precession subtracted)
         * position with nutation 
         * equatorial coordinates
         * cartesian coordinates
         * heliocentric position is not allowed ??????????????
         *         DAS WAERE ZIEMLICH AUFWENDIG. SONNE UND ERDE MUESSTEN
         *         SCHON VORHANDEN SEIN!
         * ipl		bodynumber (SE_MEAN_NODE or SE_MEAN_APOG)
         * iflag	flags
         * serr         error string
         */
        int app_pos_etc_mean(int ipl, Int32 iflag, ref string serr) {
            int i;
            Int32 flg1, flg2;
            double[] xx = new double[6], xxsv = new double[6];
            //#if 0
            //  struct node_data *pdp = &swed.nddat[ipl];
            //#else
            plan_data pdp = swed.nddat[ipl];
            //#endif
            epsilon oe;
            /* if the same conversions have already been done for the same 
             * date, then return */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = pdp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            if (flg1 == flg2) {
                pdp.xflgs = iflag;
                pdp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                return OK;
            }
            for (i = 0; i <= 5; i++)
                xx[i] = pdp.x[i];
            /* cartesian equatorial coordinates */
            SE.SwephLib.swi_polcart_sp(xx, xx);
            SE.SwephLib.swi_coortrf2(xx, xx, -swed.oec.seps, swed.oec.ceps);
            SE.SwephLib.swi_coortrf2(xx.GetPointer(3), xx.GetPointer(3), -swed.oec.seps, swed.oec.ceps);
            //#if 0 
            //  /****************************************************
            //   * light-time, this is only a few milliarcseconds * 
            //   ***************************************************/
            //  if ((iflag & SEFLG_TRUEPOS) == 0) { 
            //    dt = pdp.x[3] * AUNIT / CLIGHT / 86400;     
            //    for (i = 0; i <= 2; i++)
            //      xx[i] -= dt * xx[i+3];
            //  }
            //#endif
            if (0 == (iflag & SwissEph.SEFLG_SPEED))
                for (i = 3; i <= 5; i++)
                    xx[i] = 0;
            /* J2000 coordinates; required for sidereal positions */
            if (((iflag & SwissEph.SEFLG_SIDEREAL) != 0
              && (swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0)
                || (swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                for (i = 0; i <= 5; i++)
                    xxsv[i] = xx[i];
                /* xxsv is not J2000 yet! */
                if (pdp.teval != J2000) {
                    SE.SwephLib.swi_precess(xxsv, pdp.teval, iflag, J_TO_J2000);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        swi_precess_speed(xxsv, pdp.teval, iflag, J_TO_J2000);
                }
            }
            /*****************************************************
             * if no precession, equator of date -> equator 2000 *
             *****************************************************/
            if ((iflag & SwissEph.SEFLG_J2000) != 0) {
                SE.SwephLib.swi_precess(xx, pdp.teval, iflag, J_TO_J2000);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(xx, pdp.teval, iflag, J_TO_J2000);
                oe = swed.oec2000;
            } else
                oe = swed.oec;
            return app_pos_rest(pdp, iflag, xx, xxsv, ref oe, ref serr);
        }

        /* fetch chebyshew coefficients from sweph file for
         * tjd 		time
         * ipli		planet number
         * ifno		file number
         * serr		error string
         */
        int get_new_segment(double tjd, int ipli, int ifno, ref string serr) {
            int i, j, k, m, n, o, icoord, retc;
            Int32 iseg;
            Int32 fpos;
            int nsizes; int[] nsize = new int[6];
            int nco;
            int idbl;
            char[] c = new char[4];
            plan_data pdp = swed.pldat[ipli];
            file_data fdp = swed.fidat[ifno];
            CFile fp = fdp.fptr;
            int freord = (int)fdp.iflg & SEI_FILE_REORD;
            int fendian = (int)fdp.iflg & SEI_FILE_LITENDIAN;
            UInt32[] longs = new UInt32[MAXORD + 1];
            /* compute segment number */
            iseg = (Int32)((tjd - pdp.tfstart) / pdp.dseg);
            /*if (tjd - pdp.tfstart < 0)
                return(NOT_AVAILABLE);*/
            pdp.tseg0 = pdp.tfstart + iseg * pdp.dseg;
            pdp.tseg1 = pdp.tseg0 + pdp.dseg;
            /* get file position of coefficients from file */
            fpos = pdp.lndx0 + iseg * 3;
            retc = do_fread(ref fpos, 3, 1, 4, ref fp, fpos, freord, fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error_gns;
            fp.Seek(fpos, SeekOrigin.Begin);
            /* clear space of chebyshew coefficients */
            if (pdp.segp == null)
                pdp.segp = new double[pdp.ncoe * 3];
            //memset((void*)pdp.segp, 0, (size_t)pdp.ncoe * 3 * 8);
            for (int ii = 0; ii < pdp.segp.Length; ii++) {
                pdp.segp[ii] = 0;
            }
            /* read coefficients for 3 coordinates */
            for (icoord = 0; icoord < 3; icoord++) {
                idbl = icoord * pdp.ncoe;
                /* first read header */
                /* first bit indicates number of sizes of packed coefficients */
                retc = do_fread(c, 1, 2, 1, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error_gns;
                if ((c[0] & 128) != 0)
                {
                    nsizes = 6;
                    retc = do_fread(c.GetPointer(2), 1, 2, 1, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                    if (retc != OK)
                        goto return_error_gns;
                    nsize[0] = (int)c[1] / 16;
                    nsize[1] = (int)c[1] % 16;
                    nsize[2] = (int)c[2] / 16;
                    nsize[3] = (int)c[2] % 16;
                    nsize[4] = (int)c[3] / 16;
                    nsize[5] = (int)c[3] % 16;
                    nco = nsize[0] + nsize[1] + nsize[2] + nsize[3] + nsize[4] + nsize[5];
                } else {
                    nsizes = 4;
                    nsize[0] = (int)c[0] / 16;
                    nsize[1] = (int)c[0] % 16;
                    nsize[2] = (int)c[1] / 16;
                    nsize[3] = (int)c[1] % 16;
                    nco = nsize[0] + nsize[1] + nsize[2] + nsize[3];
                }
                /* there may not be more coefficients than interpolation
                 * order + 1 */
                if (nco > pdp.ncoe) {
                    //serr=C.sprintf("error in ephemeris file: %d coefficients instead of %d. ", nco, pdp.ncoe);
                    //if (strlen(serr) + strlen(fdp.fnam) < AS_MAXCH - 1) {
                    serr = C.sprintf("error in ephemeris file %s: %d coefficients instead of %d. ", fdp.fnam, nco, pdp.ncoe);
                    //}
                    //free(pdp.segp);
                    pdp.segp = null;
                    return (ERR);
                }
                /* now unpack */
                for (i = 0; i < nsizes; i++) {
                    if (nsize[i] == 0)
                        continue;
                    if (i < 4) {
                        j = (4 - i);
                        k = nsize[i];
                        retc = do_fread(longs, j, k, 4, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                        if (retc != OK)
                            goto return_error_gns;
                        for (m = 0; m < k; m++, idbl++)
                        {
                            if ((longs[m] & 1) != 0) 	/* will be negative */
                                pdp.segp[idbl] = -(((longs[m] + 1) / 2) / 1e+9 * pdp.rmax / 2);
                            else
                                pdp.segp[idbl] = (longs[m] / 2) / 1e+9 * pdp.rmax / 2;
                        }
                    } else if (i == 4) {		/* half byte packing */
                        j = 1;
                        k = (nsize[i] + 1) / 2;
                        retc = do_fread(longs, j, k, 4, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                        if (retc != OK)
                            goto return_error_gns;
                        for (m = 0, j = 0;
                             m < k && j < nsize[i];
                             m++) {
                            for (n = 0, o = 16;
                                 n < 2 && j < nsize[i];
                                 n++, j++, idbl++, longs[m] %= (uint)o, o /= 16) {
                                if ((longs[m] & o) != 0)
                                    pdp.segp[idbl] =
                                     -(((longs[m] + o) / o / 2) * pdp.rmax / 2 / 1e+9);
                                else
                                    pdp.segp[idbl] = (longs[m] / o / 2) * pdp.rmax / 2 / 1e+9;
                            }
                        }
                    } else if (i == 5) {		/* quarter byte packing */
                        j = 1;
                        k = (nsize[i] + 3) / 4;
                        retc = do_fread(longs, j, k, 4, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                        if (retc != OK)
                            return (retc);
                        for (m = 0, j = 0;
                             m < k && j < nsize[i];
                             m++) {
                            for (n = 0, o = 64;
                                 n < 4 && j < nsize[i];
                                 n++, j++, idbl++, longs[m] %= (uint)o, o /= 4) {
                                if ((longs[m] & o) != 0)
                                    pdp.segp[idbl] =
                                     -(((longs[m] + o) / o / 2) * pdp.rmax / 2 / 1e+9);
                                else
                                    pdp.segp[idbl] = (longs[m] / o / 2) * pdp.rmax / 2 / 1e+9;
                            }
                        }
                    }
                }
            }
            //#if 0
            //  if (ipli == SEI_SUNBARY) {
            //    printf("%d, %x\n", fpos, fpos);
            //    for (i = 0; i < pdp.ncoe; i++)
            //      printf("%e, %e, %e\n", pdp.segp[i], pdp.segp[i+pdp.ncoe], pdp.segp[i+2*pdp.ncoe]);
            //  }
            //#endif
            return (OK);
        return_error_gns:
            fdp.fptr.Dispose();
            fdp.fptr = null;
            free_planets();
            return ERR;
        }

        /* SWISSEPH
         * reads constants on ephemeris file
         * ifno         file #
         * serr         error string
         */
        int read_const(int ifno, ref string serr)
        {
            string /*c,*/ sp;
            //char c2;
            string s = String.Empty, s2 = String.Empty;
            string sastnam = String.Empty;
            int i, ipli, kpl, spi;
            int retc;
            int fendian, freord;
            int lastnam = 19;
            CFile fp;
            Int32 lng = 0;
            UInt32 ulng = 0;
            Int32 flen = 0, fpos = 0;
            short nplan = 0;
            Int32 testendian = 0;
            double[] doubles = new double[20];
            plan_data pdp;
            file_data fdp = swed.fidat[ifno];
            string serr_file_damage = "Ephemeris file %s is damaged (0). ";
            int nbytes_ipl = 2;
            fp = fdp.fptr;
            /************************************* 
             * version number of file            *
             *************************************/
            sp = fp.ReadLine();
            if (String.IsNullOrEmpty(sp))
                goto file_damage;
            //sp = strchr(s, '\r');
            //*sp = '\0';
            //sp = s;
            //while (isdigit((int)*sp) == 0 && *sp != '\0')
            //    sp++;
            //if (*sp == '\0')
            //    goto file_damage;
            var match = Regex.Match(sp, @"^.+(\d+)$");
            if (!match.Success) goto file_damage;
            /* version unused so far */
            fdp.fversion = int.Parse(match.Groups[1].Value);
            /************************************* 
             * correct file name?                *
             *************************************/
            s = fp.ReadLine().Trim();
            if (String.IsNullOrEmpty(s))
                goto file_damage;
            /* file name, without path */
            sp = fdp.fnam;
            if (sp.LastIndexOf(SwissEph.DIR_GLUE) > 0)
                sp = sp.Substring(sp.LastIndexOf(SwissEph.DIR_GLUE) + 1);
            if (!s.Equals(sp, StringComparison.CurrentCultureIgnoreCase))
            {
                serr = C.sprintf("Ephemeris file name '%s' wrong; rename '%s' ", sp, s);
                goto return_error;
            }
            /************************************* 
             * copyright                         *
             *************************************/
            //sp = fgets(s, AS_MAXCH, fp);
            //if (sp == NULL || strstr(sp, "\r\n") == NULL)
            //    goto file_damage;
            s = fp.ReadLine();
            if (String.IsNullOrEmpty(s))
                goto file_damage;
            /**************************************** 
             * orbital elements, if single asteroid *
             ****************************************/
            if (ifno == SEI_FILE_ANY_AST)
            {
                s = fp.ReadLine();
                if (String.IsNullOrEmpty(s))
                    goto file_damage;
                spi = 0;
                /* MPC number and name; will be analyzed below:
                 * search "asteroid name" */
                sp = s.TrimStart();
                spi = sp.IndexOfFirstNot("0123456789".ToCharArray()) + 1;
                i = spi;
                sastnam = s.Substring(spi, lastnam + i);
                /* save elements, they are required for swe_plan_pheno() */
                swed.astelem = s;
                /* required for magnitude */
                swed.ast_H = double.Parse(s.Substring(35 + i, 7).Trim(), CultureInfo.InvariantCulture);
                swed.ast_G = double.Parse(s.Substring(42 + i, 7).Trim(), CultureInfo.InvariantCulture);
                if (swed.ast_G == 0) swed.ast_G = 0.15;
                /* diameter in kilometers, not always given: */
                s2 = s.Substring(51 + i, 7).Trim();
                //*(s2 + 7) = '\0';
                if (String.IsNullOrEmpty(s2))
                    swed.ast_diam = 0;
                else
                    swed.ast_diam = double.Parse(s2, CultureInfo.InvariantCulture);
                if (swed.ast_diam == 0)
                {
                    /* estimate the diameter from magnitude; assume albedo = 0.15 */
                    swed.ast_diam = 1329 / Math.Sqrt(0.15) * Math.Pow(10, -0.2 * swed.ast_H);
                }
                //#if 0
                //    i = 5;
                //    while (*(sp+i) != ' ')
                //      i++;
                //    j = i - 5;
                //    strncpy(sastnam, sp, lastnam+i);
                //    *(sastnam+lastnam+i) = 0;
                //    /* save elements, they are required for swe_plan_pheno() */
                //    strcpy(swed.astelem, s);
                //    /* required for magnitude */
                //    swed.ast_G = atof(sp + 40 + j);
                //    swed.ast_H = atof(sp + 46 + j);
                //    /* diameter in kilometers, not always given: */
                //    strncpy(s2, sp+56+j, 7);
                //    *(s2 + 7) = '\0';
                //    swed.ast_diam = atof(s2);
                //#endif
            }
            /************************************* 
             * one int32 for test of byte order   * 
             *************************************/
            if (!fp.Read(ref testendian))
                goto file_damage;
            /* is byte order correct?            */
            if (testendian == SEI_FILE_TEST_ENDIAN) {
                freord = SEI_FILE_NOREORD;
            } else {
                freord = SEI_FILE_REORD;
                //sp = (char*)&lng;
                //c = (char*)&testendian;
                //for (i = 0; i < 4; i++)
                //    *(sp + i) = *(c + 3 - i);
                lng = SE.SweJPL.reorder(testendian);
                if (lng != SEI_FILE_TEST_ENDIAN)
                    goto file_damage;
                /* printf("%d  %x\n", lng, lng);*/
            }
            /* is file bigendian or littlendian? 
             * test first byte of test integer, which is highest if bigendian */
            //c = (char*)&testendian;
            //c2 = SEI_FILE_TEST_ENDIAN / 16777216L;
            //if (*c == c2)
            //if (testendian / 16777216 == SEI_FILE_TEST_ENDIAN / 16777216)
            //    fendian = SEI_FILE_BIGENDIAN;
            //else
            //    fendian = SEI_FILE_LITENDIAN;
            if (!BitConverter.IsLittleEndian)
                fendian = SEI_FILE_BIGENDIAN;
            else
                fendian = SEI_FILE_LITENDIAN;
            fdp.iflg = (Int32)freord | fendian;
            /************************************* 
             * length of file correct?           * 
             *************************************/
            retc = do_fread(ref lng, 4, 1, 4, ref fp, SEI_CURR_FPOS, freord,
                fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            fpos = (int)fp.Position;
            if (fp.Seek(0L, SeekOrigin.End) != 0)
                goto file_damage;
            flen = (int)fp.Position;
            if (lng != flen)
                goto file_damage;
            /********************************************************** 
             * DE number of JPL ephemeris which this file is based on * 
             **********************************************************/
            retc = do_fread(ref fdp.sweph_denum, 4, 1, 4, ref fp, fpos, freord,
                fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            //swed.jpldenum = (short)fdp.sweph_denum;
            /************************************* 
             * start and end epoch of file       * 
             *************************************/
            retc = do_fread(ref fdp.tfstart, 8, 1, 8, ref fp, SEI_CURR_FPOS,
                freord, fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            retc = do_fread(ref fdp.tfend, 8, 1, 8, ref fp, SEI_CURR_FPOS, freord,
                fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            /************************************* 
             * how many planets are in file?     * 
             *************************************/
            retc = do_fread(ref nplan, 2, 1, 2, ref fp, SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            if (nplan > 256)
            {
                nbytes_ipl = 4;
                nplan %= 256;
            }
            if (nplan < 1 || nplan > 20)
                goto file_damage;
            fdp.npl = nplan;
            /* which ones?                       */
            retc = do_fread(fdp.ipl, nbytes_ipl, (int)nplan, sizeof(int), ref fp, SEI_CURR_FPOS,
                freord, fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            /************************************* 
             * asteroid name                     * 
             *************************************/
            if (ifno == SEI_FILE_ANY_AST)
            {
                string sastno = String.Empty;
                int j;
                /* name of asteroid is taken from orbital elements record
                 * read above */
                j = 4;	/* old astorb.dat had only 4 characters for MPC# */
                while (sastnam[j] != ' ' && j < 10)	/* new astorb.dat has 5 */
                    j++;
                sastno = sastnam.Substring(0, j);
                //sastno[j] = '\0';
                long l;
                if (!long.TryParse(sastno, out l))
                    i = 0;
                else
                    i = (int)l;
                if (i == fdp.ipl[0] - SwissEph.SE_AST_OFFSET)
                {
                    /* element record is from bowell database */
                    fdp.astnam = sastnam.Substring(j + 1, lastnam);
                    /* overread old ast. name field */
                    if (!fp.ReadString(ref s, 30))
                        goto file_damage;
                }
                else
                {
                    /* older elements record structure: the name
                     * is taken from old name field */
                    if (!fp.ReadString(ref fdp.astnam, 30))
                        goto file_damage;
                }
                /* in worst case strlen of not null terminated area! */
                //i = fdp.astnam.Length - 1;
                //if (i < 0)
                //    i = 0;
                //sp = fdp.astnam + i;
                //while (*sp == ' ') {
                //    sp--;
                //}
                //sp[1] = '\0';
                i = fdp.astnam.IndexOf('\0');
                if (i >= 0) fdp.astnam = fdp.astnam.Substring(0, i);
                fdp.astnam = fdp.astnam.TrimEnd(' ', '\0');
            }
            /************************************* 
             * check CRC                         * 
             *************************************/
            fpos = (int)fp.Position;
            /* read CRC from file */
            retc = do_fread(ref ulng, 4, 1, 4, ref fp, SEI_CURR_FPOS, freord,
          fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            /* read check area from file */
            fp.Seek(0L, SeekOrigin.Begin);
            /* must check that defined length of s is less than fpos */
            //if (fpos - 1 > 2 * AS_MAXCH)
            //    goto file_damage;
            byte[] crcBuff = new byte[fpos];
            if (fp.Read(crcBuff, 0, fpos) != fpos)
                goto file_damage;
            //#if 1
            if (SE.SwephLib.swi_crc32(crcBuff, (int)fpos) != ulng)
                goto file_damage;
            /*printf("crc %d %d\n", ulng2, ulng);*/
            //#endif
            fp.Seek(fpos + 4, SeekOrigin.Begin);
            /************************************* 
             * read general constants            * 
             *************************************/
            /* clight, aunit, helgravconst, ratme, sunradius 
             * these constants are currently not in use */
            retc = do_fread(doubles, 8, 5, 8, ref fp, SEI_CURR_FPOS, freord,
          fendian, ifno, ref serr);
            if (retc != OK)
                goto return_error;
            swed.gcdat.clight = doubles[0];
            swed.gcdat.aunit = doubles[1];
            swed.gcdat.helgravconst = doubles[2];
            swed.gcdat.ratme = doubles[3];
            swed.gcdat.sunradius = doubles[4];
            /************************************* 
             * read constants of planets         * 
             *************************************/
            for (kpl = 0; kpl < fdp.npl; kpl++)
            {
                /* get SEI_ planet number */
                ipli = fdp.ipl[kpl];
                if (ipli >= SwissEph.SE_AST_OFFSET)
                    pdp = swed.pldat[SEI_ANYBODY];
                else
                    pdp = swed.pldat[ipli];
                pdp.ibdy = ipli;
                /* file position of planet's index */
                retc = do_fread(ref pdp.lndx0, 4, 1, 4, ref fp, SEI_CURR_FPOS,
            freord, fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error;
                /* flags: helio/geocentric, rotation, reference ellipse */
                retc = do_fread(ref pdp.iflg, 1, 1, sizeof(Int32), ref fp,
            SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error;
                /* number of chebyshew coefficients / segment  */
                /* = interpolation order +1                    */
                retc = do_fread(ref pdp.ncoe, 1, 1, sizeof(int), ref fp,
            SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error;
                /* rmax = normalisation factor */
                retc = do_fread(ref lng, 4, 1, 4, ref fp, SEI_CURR_FPOS, freord,
            fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error;
                pdp.rmax = lng / 1000.0;
                /* start and end epoch of planetary ephemeris,   */
                /* segment length, and orbital elements          */
                retc = do_fread(doubles, 8, 10, 8, ref fp, SEI_CURR_FPOS, freord,
            fendian, ifno, ref serr);
                if (retc != OK)
                    goto return_error;
                pdp.tfstart = doubles[0];
                pdp.tfend = doubles[1];
                pdp.dseg = doubles[2];
                pdp.nndx = (Int32)((doubles[1] - doubles[0] + 0.1) / doubles[2]);
                pdp.telem = doubles[3];
                pdp.prot = doubles[4];
                pdp.dprot = doubles[5];
                pdp.qrot = doubles[6];
                pdp.dqrot = doubles[7];
                pdp.peri = doubles[8];
                pdp.dperi = doubles[9];
                /* alloc space for chebyshew coefficients */
                /* if reference ellipse is used, read its coefficients */
                if ((pdp.iflg & SEI_FLG_ELLIPSE) != 0)
                {
                    if (pdp.refep != null)
                    { /* if switch to other eph. file */
                        //free(pdp.refep);
                        pdp.refep = null;    /* 2015-may-5 */
                        if (pdp.segp != null)
                        {
                            //free(pdp.segp);     /* array of coefficients of */
                            pdp.segp = null;     /* ephemeris segment        */
                        }
                    }
                    //pdp.refep = malloc((size_t)pdp.ncoe * 2 * 8);
                    pdp.refep = new double[pdp.ncoe * 2];
                    retc = do_fread(pdp.refep, 8, 2 * pdp.ncoe, 8, ref fp,
              SEI_CURR_FPOS, freord, fendian, ifno, ref serr);
                    if (retc != OK)
                    {
                        //free(pdp.refep);  /* 2015-may-5 */
                        pdp.refep = null;  /* 2015-may-5 */
                        goto return_error;
                    }
                }/**/
            }
            return (OK);
        file_damage:
            //if (serr != null) {
            //*serr = '\0';
            //if (strlen(serr_file_damage) + strlen(fdp.fnam) < AS_MAXCH) {
            serr = C.sprintf(serr_file_damage, fdp.fnam);
        //}
        //}
        return_error:
            fdp.fptr.Dispose();
            fdp.fptr = null;
            free_planets();
            return (ERR);
        }

        /* SWISSEPH
         * reads from a file and, if necessary, reorders bytes 
         * targ 	target pointer
         * size		size of item to be read
         * count	number of items
         * corrsize	in what size should it be returned 
         *		(e.g. 3 byte int -> 4 byte int)
         * fp		file pointer
         * fpos		file position: if (fpos >= 0) then fseek
         * freord	reorder bytes or no
         * fendian	little/bigendian
         * ifno		file number
         * serr		error string
         */
        int do_fread(byte[] trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            int i, j, k;
            int totsize;
            byte[] space = new byte[1000];
            CPointer<byte> targ = trg;
            totsize = size * count;
            if (fpos >= 0)
                fp.Seek(fpos, SeekOrigin.Begin);
            /* if no byte reorder has to be done, and read size == return size */
            if (0 == freord && size == corrsize) {
                if (fp.Read(trg, 0, totsize) == 0) {
                    serr = C.sprintf("Ephemeris file %s is damaged (2).", swed.fidat[ifno].fnam);
                    return (ERR);
                } else
                    return (OK);
            } else {
                if (fp.Read(space, 0, totsize) == 0) {
                    serr = C.sprintf("Ephemeris file %s is damaged (4).", swed.fidat[ifno].fnam);
                    return (ERR);
                }
                if (size != corrsize) {
                    for (int ii = 0; ii < count * corrsize; ii++) {
                        targ[ii] = 0;
                    }
                }
                for (i = 0; i < count; i++) {
                    for (j = size - 1; j >= 0; j--) {
                        if (freord != 0)
                            k = size - j - 1;
                        else
                            k = j;
                        if (size != corrsize)
                            if ((fendian == SEI_FILE_BIGENDIAN && 0 == freord) ||
                                (fendian == SEI_FILE_LITENDIAN && freord != 0))
                                k += corrsize - size;
                        targ[i * corrsize + k] = space[i * size + j];
                    }
                }
            }
            return (OK);
        }
        int do_fread(CPointer<char> trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            var buff = new byte[count * corrsize];
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            for (int i = 0; i < count * corrsize; i++) {
                trg[i] = (char)buff[i];
            }
            return res;
        }
        int do_fread(CPointer<uint> trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            var buff = new byte[count * corrsize];
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            for (int i = 0; i < count; i++) {
                trg[i] = BitConverter.ToUInt32(buff, i * corrsize);
            }
            return res;
        }
        int do_fread(CPointer<int> trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            var buff = new byte[count * corrsize];
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            for (int i = 0; i < count; i++) {
                trg[i] = BitConverter.ToInt32(buff, i * corrsize);
            }
            return res;
        }
        int do_fread(CPointer<double> trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            var buff = new byte[count * corrsize];
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            for (int i = 0; i < count; i++) {
                trg[i] = BitConverter.ToDouble(buff, i * corrsize);
            }
            return res;
        }
        int do_fread(ref short trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            byte[] buff = BitConverter.GetBytes(trg);
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            trg = BitConverter.ToInt16(buff, 0);
            return res;
        }
        int do_fread(ref Int32 trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            byte[] buff = BitConverter.GetBytes(trg);
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            trg = BitConverter.ToInt32(buff, 0);
            return res;
        }
        int do_fread(ref UInt32 trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            byte[] buff = BitConverter.GetBytes(trg);
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            trg = BitConverter.ToUInt32(buff, 0);
            return res;
        }
        int do_fread(ref Double trg, int size, int count, int corrsize, ref CFile fp, Int32 fpos, int freord, int fendian, int ifno, ref string serr) {
            byte[] buff = BitConverter.GetBytes(trg);
            var res = do_fread(buff, size, count, corrsize, ref fp, fpos, freord, fendian, ifno, ref serr);
            trg = BitConverter.ToDouble(buff, 0);
            return res;
        }

        /* SWISSEPH
         * adds reference orbit to chebyshew series (if SEI_FLG_ELLIPSE),
         * rotates series to mean equinox of J2000
         *
         * ipli		planet number
         */
        void rot_back(int ipli) {
            int i;
            double t, tdiff;
            double qav, pav, dn;
            double omtild, com, som, cosih2;
            double[,] x = new double[MAXORD + 1, 3];
            double[] uix = new double[3], uiy = new double[3], uiz = new double[3];
            double xrot, yrot, zrot;
            CPointer<double> chcfx, chcfy, chcfz;
            CPointer<double> refepx, refepy;
            double seps2000 = swed.oec2000.seps;
            double ceps2000 = swed.oec2000.ceps;
            plan_data pdp = swed.pldat[ipli];
            int nco = pdp.ncoe;
            t = pdp.tseg0 + pdp.dseg / 2;
            chcfx = pdp.segp;
            chcfy = chcfx + nco;
            chcfz = chcfx + 2 * nco;
            tdiff = (t - pdp.telem) / 365250.0;
            if (ipli == SEI_MOON) {
                dn = pdp.prot + tdiff * pdp.dprot;
                i = (int)(dn / TWOPI);
                dn -= i * TWOPI;
                qav = (pdp.qrot + tdiff * pdp.dqrot) * Math.Cos(dn);
                pav = (pdp.qrot + tdiff * pdp.dqrot) * Math.Sin(dn);
            } else {
                qav = pdp.qrot + tdiff * pdp.dqrot;
                pav = pdp.prot + tdiff * pdp.dprot;
            }
            /*calculate cosine and sine of average perihelion longitude. */
            for (i = 0; i < nco; i++) {
                x[i, 0] = chcfx[i];
                x[i, 1] = chcfy[i];
                x[i, 2] = chcfz[i];
            }
            if ((pdp.iflg & SEI_FLG_ELLIPSE) != 0) {
                refepx = pdp.refep;
                refepy = refepx + nco;
                omtild = pdp.peri + tdiff * pdp.dperi;
                i = (int)(omtild / TWOPI);
                omtild -= i * TWOPI;
                com = Math.Cos(omtild);
                som = Math.Sin(omtild);
                /*add reference orbit.  */
                for (i = 0; i < nco; i++) {
                    x[i, 0] = chcfx[i] + com * refepx[i] - som * refepy[i];
                    x[i, 1] = chcfy[i] + com * refepy[i] + som * refepx[i];
                }
            }
            /* construct right handed orthonormal system with first axis along
               origin of longitudes and third axis along angular momentum    
               this uses the standard formulas for equinoctal variables   
               (see papers by broucke and by cefola).      */
            cosih2 = 1.0 / (1.0 + qav * qav + pav * pav);
            /*     calculate orbit pole. */
            uiz[0] = 2.0 * pav * cosih2;
            uiz[1] = -2.0 * qav * cosih2;
            uiz[2] = (1.0 - qav * qav - pav * pav) * cosih2;
            /*     calculate origin of longitudes vector. */
            uix[0] = (1.0 + qav * qav - pav * pav) * cosih2;
            uix[1] = 2.0 * qav * pav * cosih2;
            uix[2] = -2.0 * pav * cosih2;
            /*     calculate vector in orbital plane orthogonal to origin of    
                  longitudes.                                               */
            uiy[0] = 2.0 * qav * pav * cosih2;
            uiy[1] = (1.0 - qav * qav + pav * pav) * cosih2;
            uiy[2] = 2.0 * qav * cosih2;
            /*     rotate to actual orientation in space.         */
            for (i = 0; i < nco; i++) {
                xrot = x[i, 0] * uix[0] + x[i, 1] * uiy[0] + x[i, 2] * uiz[0];
                yrot = x[i, 0] * uix[1] + x[i, 1] * uiy[1] + x[i, 2] * uiz[1];
                zrot = x[i, 0] * uix[2] + x[i, 1] * uiy[2] + x[i, 2] * uiz[2];
                if (Math.Abs(xrot) + Math.Abs(yrot) + Math.Abs(zrot) >= 1e-14)
                    pdp.neval = i;
                x[i, 0] = xrot;
                x[i, 1] = yrot;
                x[i, 2] = zrot;
                if (ipli == SEI_MOON) {
                    /* rotate to j2000 equator */
                    x[i, 1] = ceps2000 * yrot - seps2000 * zrot;
                    x[i, 2] = seps2000 * yrot + ceps2000 * zrot;
                }
            }
            for (i = 0; i < nco; i++) {
                chcfx[i] = x[i, 0];
                chcfy[i] = x[i, 1];
                chcfz[i] = x[i, 2];
            }
        }

        /* Adjust position from Earth-Moon barycenter to Earth
         *
         * xemb = hel./bar. position or velocity vectors of emb (input)
         *                                                  earth (output)
         * xmoon= geocentric position or velocity vector of moon
         */
        void embofs(CPointer<double> xemb, CPointer<double> xmoon) {
            int i;
            for (i = 0; i <= 2; i++)
                xemb[i] -= xmoon[i] / (EARTH_MOON_MRAT + 1.0);
        }

        /* calculates the nutation matrix
         * nu		pointer to nutation data structure
         * oe		pointer to epsilon data structure
         */
        void nut_matrix(nut nu, epsilon oe) {
            double psi, eps;
            double sinpsi, cospsi, sineps, coseps, sineps0, coseps0;
            psi = nu.nutlo[0];
            eps = oe.eps + nu.nutlo[1];
            sinpsi = Math.Sin(psi);
            cospsi = Math.Cos(psi);
            sineps0 = oe.seps;
            coseps0 = oe.ceps;
            sineps = Math.Sin(eps);
            coseps = Math.Cos(eps);
            nu.matrix[0, 0] = cospsi;
            nu.matrix[0, 1] = sinpsi * coseps;
            nu.matrix[0, 2] = sinpsi * sineps;
            nu.matrix[1, 0] = -sinpsi * coseps0;
            nu.matrix[1, 1] = cospsi * coseps * coseps0 + sineps * sineps0;
            nu.matrix[1, 2] = cospsi * sineps * coseps0 - coseps * sineps0;
            nu.matrix[2, 0] = -sinpsi * sineps0;
            nu.matrix[2, 1] = cospsi * coseps * sineps0 - sineps * coseps0;
            nu.matrix[2, 2] = cospsi * sineps * sineps0 + coseps * coseps0;
        }

        /* lunar osculating elements, i.e.
         * osculating node ('true' node) and
         * osculating apogee ('black moon', 'lilith').
         * tjd		julian day
         * ipl		body number, i.e. SEI_TRUE_NODE or SEI_OSCU_APOG
         * iflag	flags (which ephemeris, nutation, etc.)
         * serr		error string
         *
         * definitions and remarks:
         * the osculating node and the osculating apogee are defined
         * as the orbital elements of the momentary lunar orbit.
         * their advantage is that when the moon crosses the ecliptic,
         * it is really at the osculating node, and when it passes
         * its greatest distance from earth it is really at the
         * osculating apogee. with the mean elements this is not
         * the case. (some define the apogee as the second focus of 
         * the lunar ellipse. but, as seen from the geocenter, both 
         * points are in the same direction.)
         * problems:
         * the osculating apogee is given in the 'New International
         * Ephemerides' (Editions St. Michel) as the 'True Lilith'.
         * however, this name is misleading. this point is based on
         * the idea that the lunar orbit can be approximated by an
         * ellipse. 
         * arguments against this: 
         * 1. this procedure considers celestial motions as two body
         *    problems. this is quite good for planets, but not for
         *    the moon. the strong gravitational attraction of the sun 
         *    destroys the idea of an ellipse.
         * 2. the NIE 'True Lilith' has strong oscillations around the
         *    mean one with an amplitude of about 30 degrees. however,
         *    when the moon is in apogee, its distance from the mean
         *    apogee never exceeds 5 degrees.
         * besides, the computation of NIE is INACCURATE. the mistake 
         * reaches 20 arc minutes.
         * According to Santoni, the point was calculated using 'les 58
         * premiers termes correctifs au Perigee moyen' published by
         * Chapront and Chapront-Touze. And he adds: "Nous constatons
         * que meme en utilisant ces 58 termes CORRECTIFS, l'erreur peut
         * atteindre 0,5d!" (p. 13) We avoid this error, computing the
         * orbital elements directly from the position and the speed vector.
         *
         * how about the node? it is less problematic, because we
         * we needn't derive it from an orbital ellipse. we can say:
         * the axis of the osculating nodes is the intersection line of
         * the actual orbital plane of the moon and the plane of the 
         * ecliptic. or: the osculating nodes are the intersections of
         * the two great circles representing the momentary apparent 
         * orbit of the moon and the ecliptic. in this way they make
         * some sense. then, the nodes are really an axis, and they
         * have no geocentric distance. however, in this routine
         * we give a distance derived from the osculating ellipse.
         * the node could also be defined as the intersection axis
         * of the lunar orbital plane and the solar orbital plane,
         * which is not precisely identical to the ecliptic. this 
         * would make a difference of several arcseconds.
         *
         * is it possible to keep the idea of a continuously moving
         * apogee that is exact at the moment when the moon passes
         * its greatest distance from earth?
         * to achieve this, we would probably have to interpolate between 
         * the actual apogees. 
         * the nodes could also be computed by interpolation. the resulting
         * nodes would deviate from the so-called 'true node' by less than
         * 30 arc minutes.
         *
         * sidereal and j2000 true node are first computed for the ecliptic
         * of epoch and then precessed to ecliptic of t0(ayanamsa) or J2000.
         * there is another procedure that computes the node for the ecliptic
         * of t0(ayanamsa) or J2000. it is excluded by
         * #ifdef SID_TNODE_FROM_ECL_T0
         */
        int lunar_osc_elem(double tjd, int ipl, Int32 iflag, ref string serr) {
            int i, j, istart;
            int ipli = SEI_MOON;
            Int32 epheflag = SwissEph.SEFLG_DEFAULTEPH;
            int retc = ERR;
            Int32 flg1, flg2;
            double daya;
            //#if 0
            //  struct node_data *ndp, *ndnp, *ndap;
            //#else
            plan_data ndp, ndnp, ndap;
            //#endif
            epsilon oe;
            double speed_intv = NODE_CALC_INTV;	/* to silence gcc warning */
            double a, b;
            double[][] xpos = CreateArray<double>(3, 6),
                xx = CreateArray<double>(3, 6),
                xxa = CreateArray<double>(3, 6);
            double[] xnorm = new double[6], r = new double[6];
            double[] xp;
            double rxy, rxyz, t, dt, fac, sgn;
            double sinnode, cosnode, sinincl, cosincl, sinu, cosu, sinE, cosE;
            double uu, ny, sema, ecce, Gmsm, c2, v2, pp;
            Int32 speedf1, speedf2;
            sid_data sip = new sid_data();
            if (SID_TNODE_FROM_ECL_T0) {
                sip = swed.sidd;
                epsilon oectmp = new epsilon();
                if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                    calc_epsilon(sip.t0, iflag, oectmp);
                    oe = oectmp;
                } else if ((iflag & SwissEph.SEFLG_J2000) != 0)
                    oe = swed.oec2000;
                else
                    oe = swed.oec;
            } else
                oe = swed.oec;
            ndp = swed.nddat[ipl];
            /* if elements have already been computed for this date, return 
             * if speed flag has been turned on, recompute */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = ndp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            speedf1 = ndp.xflgs & SwissEph.SEFLG_SPEED;
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            if (tjd == ndp.teval
              && tjd != 0
              && flg1 == flg2
              && (0 == speedf2 || speedf1 != 0)) {
                ndp.xflgs = iflag;
                ndp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                return OK;
            }
            /* the geocentric position vector and the speed vector of the
             * moon make up the lunar orbital plane. the position vector 
             * of the node is along the intersection line of the orbital 
             * plane and the plane of the ecliptic.
             * to calculate the osculating node, we need one lunar position
             * with speed.
             * to calculate the speed of the osculating node, we need 
             * three lunar positions and the speed of each of them.
             * this is relatively cheap, if the jpl-moon or the swisseph
             * moon is used. with the moshier moon this is much more 
             * expensive, because then we need 9 lunar positions for 
             * three speeds. but one position and speed can normally
             * be taken from swed.pldat[moon], which corresponds to
             * three moshier moon calculations.
             * the same is also true for the osculating apogee: we need 
             * three lunar positions and speeds.
             */
            /*********************************************
             * now three lunar positions with speeds     * 
             *********************************************/
            if ((iflag & SwissEph.SEFLG_MOSEPH) != 0)
                epheflag = SwissEph.SEFLG_MOSEPH;
            else if ((iflag & SwissEph.SEFLG_SWIEPH) != 0)
                epheflag = SwissEph.SEFLG_SWIEPH;
            else if ((iflag & SwissEph.SEFLG_JPLEPH) != 0)
                epheflag = SwissEph.SEFLG_JPLEPH;
            /* there may be a moon of wrong ephemeris in save area
             * force new computation: */
            swed.pldat[SEI_MOON].teval = 0;
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                istart = 0;
            else
                istart = 2;
            //if (serr != NULL)
            //    *serr = '\0';
            serr = String.Empty;
        three_positions:
            switch (epheflag) {
                case SwissEph.SEFLG_JPLEPH:
                    speed_intv = NODE_CALC_INTV;
                    for (i = istart; i <= 2; i++) {
                        if (i == 0)
                            t = tjd - speed_intv;
                        else if (i == 1)
                            t = tjd + speed_intv;
                        else
                            t = tjd;
                        xp = xpos[i];
                        retc = jplplan(t, ipli, iflag, NO_SAVE, xp, null, null, ref serr);
                        /* read error or corrupt file */
                        if (retc == ERR)
                            return (ERR);
                        /* light-time-corrected moon for apparent node 
                         * this makes a difference of several milliarcseconds with
                         * the node and 0.1" with the apogee.
                         * the simple formual 'x[j] -= dt * speed' should not be 
                         * used here. the error would be greater than the advantage
                         * of computation speed. */
                        if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0 && retc >= OK) {
                            dt = Math.Sqrt(square_sum(xpos[i])) * AUNIT / CLIGHT / 86400.0;
                            retc = jplplan(t - dt, ipli, iflag, NO_SAVE, xpos[i], null, null, ref serr);/**/
                            /* read error or corrupt file */
                            if (retc == ERR)
                                return (ERR);
                        }
                        /* jpl ephemeris not on disk, or date beyond ephemeris range */
                        if (retc == NOT_AVAILABLE) {
                            iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_SWIEPH;
                            epheflag = SwissEph.SEFLG_SWIEPH;
                            //if (serr != NULL && strlen(serr) + 30 < AS_MAXCH)
                            serr += " \ntrying Swiss Eph; ";
                            break;
                        } else if (retc == BEYOND_EPH_LIMITS) {
                            if (tjd > MOSHLUEPH_START && tjd < MOSHLUEPH_END) {
                                iflag = (iflag & ~SwissEph.SEFLG_JPLEPH) | SwissEph.SEFLG_MOSEPH;
                                epheflag = SwissEph.SEFLG_MOSEPH;
                                //if (serr != NULL && strlen(serr) + 30 < AS_MAXCH)
                                serr += " \nusing Moshier Eph; ";
                                break;
                            } else
                                return ERR;
                        }
                        /* precession and nutation etc. */
                        retc = swi_plan_for_osc_elem(iflag | SwissEph.SEFLG_SPEED, t, xpos[i]); /* retc is always ok */
                    }
                    break;
                case SwissEph.SEFLG_SWIEPH:
                    //#if 0
                    //      sweph_moon:
                    //#endif
                    speed_intv = NODE_CALC_INTV;
                    for (i = istart; i <= 2; i++) {
                        if (i == 0)
                            t = tjd - speed_intv;
                        else if (i == 1)
                            t = tjd + speed_intv;
                        else
                            t = tjd;
                        retc = swemoon(t, iflag | SwissEph.SEFLG_SPEED, NO_SAVE, xpos[i], ref serr);/**/
                        if (retc == ERR)
                            return (ERR);
                        /* light-time-corrected moon for apparent node (~ 0.006") */
                        if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0 && retc >= OK) {
                            dt = Math.Sqrt(square_sum(xpos[i])) * AUNIT / CLIGHT / 86400.0;
                            retc = swemoon(t - dt, iflag | SwissEph.SEFLG_SPEED, NO_SAVE, xpos[i], ref serr);/**/
                            if (retc == ERR)
                                return (ERR);
                        }
                        if (retc == NOT_AVAILABLE) {
                            if (tjd > MOSHPLEPH_START && tjd < MOSHPLEPH_END) {
                                iflag = (iflag & ~SwissEph.SEFLG_SWIEPH) | SwissEph.SEFLG_MOSEPH;
                                epheflag = SwissEph.SEFLG_MOSEPH;
                                //if (serr != NULL && strlen(serr) + 30 < AS_MAXCH)
                                serr += " \nusing Moshier eph.; ";
                                break;
                            } else
                                return ERR;
                        }
                        /* precession and nutation etc. */
                        retc = swi_plan_for_osc_elem(iflag | SwissEph.SEFLG_SPEED, t, xpos[i]); /* retc is always ok */
                    }
                    break;
                case SwissEph.SEFLG_MOSEPH:
                    //#if 0
                    //      moshier_moon:
                    //#endif
                    /* with moshier moon, we need a greater speed_intv, because here the
                     * node and apogee oscillate wildly within small intervals */
                    speed_intv = NODE_CALC_INTV_MOSH;
                    for (i = istart; i <= 2; i++) {
                        if (i == 0)
                            t = tjd - speed_intv;
                        else if (i == 1)
                            t = tjd + speed_intv;
                        else
                            t = tjd;
                        retc = SE.SwemMoon.swi_moshmoon(t, NO_SAVE, xpos[i], ref serr);/**/
                        if (retc == ERR)
                            return (retc);
                        //#if 0
                        //    /* light-time-corrected moon for apparent node.
                        //     * can be neglected with moshier */
                        //    if ((iflag & SEFLG_TRUEPOS) == 0 && retc >= OK) { 
                        //      dt =Math.Sqrt(square_sum(xpos[i])) * AUNIT / CLIGHT / 86400;     
                        //      retc = swi_moshmoon(t-dt, NO_SAVE, xpos[i], serr);/**/
                        //        }
                        //#endif
                        /* precession and nutation etc. */
                        retc = swi_plan_for_osc_elem(iflag | SwissEph.SEFLG_SPEED, t, xpos[i]); /* retc is always ok */
                    }
                    break;
                default:
                    break;
            }
            if (retc == NOT_AVAILABLE || retc == BEYOND_EPH_LIMITS)
                goto three_positions;
            /*********************************************
             * node with speed                           * 
             *********************************************/
            /* node is always needed, even if apogee is wanted */
            ndnp = swed.nddat[SEI_TRUE_NODE];
            /* three nodes */
            for (i = istart; i <= 2; i++) {
                if (Math.Abs(xpos[i][5]) < 1e-15)
                    xpos[i][5] = 1e-15;
                fac = xpos[i][2] / xpos[i][5];
                sgn = xpos[i][5] / Math.Abs(xpos[i][5]);
                for (j = 0; j <= 2; j++)
                    xx[i][j] = (xpos[i][j] - fac * xpos[i][j + 3]) * sgn;
            }
            /* now we have the correct direction of the node, the
             * intersection of the lunar plane and the ecliptic plane.
             * the distance is the distance of the point where the tangent
             * of the lunar motion penetrates the ecliptic plane.
             * this can be very large, e.g. j2415080.37372.
             * below, a new distance will be derived from the osculating 
             * ellipse. 
             */
            /* save position and speed */
            for (i = 0; i <= 2; i++) {
                ndnp.x[i] = xx[2][i];
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    b = (xx[1][i] - xx[0][i]) / 2;
                    a = (xx[1][i] + xx[0][i]) / 2 - xx[2][i];
                    ndnp.x[i + 3] = (2 * a + b) / speed_intv;
                } else
                    ndnp.x[i + 3] = 0;
                ndnp.teval = tjd;
                ndnp.iephe = epheflag;
            }
            /************************************************************
             * apogee with speed                                        * 
             * must be computed anyway to get the node's distance       *
             ************************************************************/
            ndap = swed.nddat[SEI_OSCU_APOG];
            Gmsm = GEOGCONST * (1 + 1 / EARTH_MOON_MRAT) / AUNIT / AUNIT / AUNIT * 86400.0 * 86400.0;
            /* three apogees */
            for (i = istart; i <= 2; i++) {
                /* node */
                rxy = Math.Sqrt(xx[i][0] * xx[i][0] + xx[i][1] * xx[i][1]);
                cosnode = xx[i][0] / rxy;
                sinnode = xx[i][1] / rxy;
                /* inclination */
                SE.SwephLib.swi_cross_prod(xpos[i], xpos[i].GetPointer(3), xnorm);
                rxy = xnorm[0] * xnorm[0] + xnorm[1] * xnorm[1];
                c2 = (rxy + xnorm[2] * xnorm[2]);
                rxyz = Math.Sqrt(c2);
                rxy = Math.Sqrt(rxy);
                sinincl = rxy / rxyz;
                cosincl = Math.Sqrt(1 - sinincl * sinincl);
                /* argument of latitude */
                cosu = xpos[i][0] * cosnode + xpos[i][1] * sinnode;
                sinu = xpos[i][2] / sinincl;
                uu = Math.Atan2(sinu, cosu);
                /* semi-axis */
                rxyz = Math.Sqrt(square_sum(xpos[i]));
                v2 = square_sum((xpos[i].GetPointer(3)));
                sema = 1 / (2 / rxyz - v2 / Gmsm);
                /* eccentricity */
                pp = c2 / Gmsm;
                ecce = Math.Sqrt(1 - pp / sema);
                /* eccentric anomaly */
                cosE = 1 / ecce * (1 - rxyz / sema);
                sinE = 1 / ecce / Math.Sqrt(sema * Gmsm) * dot_prod(xpos[i], (xpos[i].GetPointer(3)));
                /* true anomaly */
                ny = 2 * Math.Atan(Math.Sqrt((1 + ecce) / (1 - ecce)) * sinE / (1 + cosE));
                /* distance of apogee from ascending node */
                xxa[i][0] = SE.SwephLib.swi_mod2PI(uu - ny + PI);
                xxa[i][1] = 0;			/* latitude */
                xxa[i][2] = sema * (1 + ecce);	/* distance */
                /* transformation to ecliptic coordinates */
                SE.SwephLib.swi_polcart(xxa[i], xxa[i]);
                SE.SwephLib.swi_coortrf2(xxa[i], xxa[i], -sinincl, cosincl);
                SE.SwephLib.swi_cartpol(xxa[i], xxa[i]);
                /* adding node, we get apogee in ecl. coord. */
                xxa[i][0] += Math.Atan2(sinnode, cosnode);
                SE.SwephLib.swi_polcart(xxa[i], xxa[i]);
                /* new distance of node from orbital ellipse:
                 * true anomaly of node: */
                ny = SE.SwephLib.swi_mod2PI(ny - uu);
                /* eccentric anomaly */
                cosE = Math.Cos(2 * Math.Atan(Math.Tan(ny / 2) / Math.Sqrt((1 + ecce) / (1 - ecce))));
                /* new distance */
                r[0] = sema * (1 - ecce * cosE);
                /* old node distance */
                r[1] = Math.Sqrt(square_sum(xx[i]));
                /* correct length of position vector */
                for (j = 0; j <= 2; j++)
                    xx[i][j] *= r[0] / r[1];
            }
            /* save position and speed */
            for (i = 0; i <= 2; i++) {
                /* apogee */
                ndap.x[i] = xxa[2][i];
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    ndap.x[i + 3] = (xxa[1][i] - xxa[0][i]) / speed_intv / 2;
                else
                    ndap.x[i + 3] = 0;
                ndap.teval = tjd;
                ndap.iephe = epheflag;
                /* node */
                ndnp.x[i] = xx[2][i];
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    ndnp.x[i + 3] = (xx[1][i] - xx[0][i]) / speed_intv / 2;/**/
                else
                    ndnp.x[i + 3] = 0;
            }
            /**********************************************************************
             * precession and nutation have already been taken into account
             * because the computation is on the basis of lunar positions
             * that have gone through swi_plan_for_osc_elem. 
             * light-time is already contained in lunar positions.
             * now compute polar and equatorial coordinates:
             **********************************************************************/
            for (j = 0; j <= 1; j++) {
                double[] x = new double[6];
                if (j == 0)
                    ndp = swed.nddat[SEI_TRUE_NODE];
                else
                    ndp = swed.nddat[SEI_OSCU_APOG];
                //memset(ndp.xreturn, 0, 24 * sizeof(double));
                for (int ii = 0; ii < ndp.xreturn.Length; ii++) {
                    ndp.xreturn[ii] = 0;
                }
                /* cartesian ecliptic */
                for (i = 0; i <= 5; i++)
                    ndp.xreturn[6 + i] = ndp.x[i];
                /* polar ecliptic */
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                /* cartesian equatorial */
                SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(6), ndp.xreturn.GetPointer(18), -oe.seps, oe.ceps);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(9), ndp.xreturn.GetPointer(21), -oe.seps, oe.ceps);
                if (SID_TNODE_FROM_ECL_T0) {
                    /* sideral: we return NORMAL equatorial coordinates, there are no
                     * sidereal ones */
                    if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                        /* to J2000 */
                        SE.SwephLib.swi_precess(ndp.xreturn.GetPointer(18), sip.t0, iflag, J_TO_J2000);
                        if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                            swi_precess_speed(ndp.xreturn.GetPointer(21), sip.t0, iflag, J_TO_J2000);
                        if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                            /* to tjd */
                            SE.SwephLib.swi_precess(ndp.xreturn.GetPointer(18), tjd, iflag, J2000_TO_J);
                            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                                swi_precess_speed(ndp.xreturn.GetPointer(21), tjd, iflag, J2000_TO_J);
                        }
                    }
                }
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(18), -swed.nut.snut, swed.nut.cnut);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(21), ndp.xreturn.GetPointer(21), -swed.nut.snut, swed.nut.cnut);
                }
                /* polar equatorial */
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                ndp.xflgs = iflag;
                ndp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
                if (SID_TNODE_FROM_ECL_T0) {
                    /* node and apogee are already referred to t0; 
                     * nothing has to be done */
                } else {
                    if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                        /* node and apogee are referred to t; 
                         * the ecliptic position must be transformed to t0 */
                        /* rigorous algorithm */
                        if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0
                          || (swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                            for (i = 0; i <= 5; i++)
                                x[i] = ndp.xreturn[18 + i];
                            /* remove nutation */
                            if (0 == (iflag & SwissEph.SEFLG_NONUT))
                                swi_nutate(x, iflag, true);
                            /* precess to J2000 */
                            SE.SwephLib.swi_precess(x, tjd, iflag, J_TO_J2000);
                            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                                swi_precess_speed(x, tjd, iflag, J_TO_J2000);
                            if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0)
                                swi_trop_ra2sid_lon(x, ndp.xreturn.GetPointer(6), ndp.xreturn.GetPointer(18), iflag);
                            /* project onto solar system equator */
                            else if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0)
                                swi_trop_ra2sid_lon_sosy(x, ndp.xreturn.GetPointer(6), iflag);
                            /* to polar */
                            SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                            SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                            /* traditional algorithm;
                             * this is a bit clumsy, but allows us to keep the
                             * sidereal code together */
                        } else {
                            SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                            if (swe_get_ayanamsa_ex(ndp.teval, iflag, out daya, ref serr) == ERR)
                                return ERR;
                            ndp.xreturn[0] -= daya * SwissEph.DEGTORAD;
                            SE.SwephLib.swi_polcart_sp(ndp.xreturn, ndp.xreturn.GetPointer(6));
                        }
                    } else if ((iflag & SwissEph.SEFLG_J2000) != 0) {
                        /* node and apogee are referred to t; 
                         * the ecliptic position must be transformed to J2000 */
                        for (i = 0; i <= 5; i++)
                            x[i] = ndp.xreturn[18 + i];
                        /* precess to J2000 */
                        SE.SwephLib.swi_precess(x, tjd, iflag, J_TO_J2000);
                        if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                            swi_precess_speed(x, tjd, iflag, J_TO_J2000);
                        for (i = 0; i <= 5; i++)
                            ndp.xreturn[18 + i] = x[i];
                        SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                        SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(6), swed.oec2000.seps, swed.oec2000.ceps);
                        if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                            SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(21), ndp.xreturn.GetPointer(9), swed.oec2000.seps, swed.oec2000.ceps);
                        SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                    }
                }
                /********************** 
                 * radians to degrees *
                 **********************/
                /*if (!(iflag & SwissEph.SEFLG_RADIANS)) {*/
                for (i = 0; i < 2; i++) {
                    ndp.xreturn[i] *= SwissEph.RADTODEG;	/* ecliptic */
                    ndp.xreturn[i + 3] *= SwissEph.RADTODEG;
                    ndp.xreturn[i + 12] *= SwissEph.RADTODEG;	/* equator */
                    ndp.xreturn[i + 15] *= SwissEph.RADTODEG;
                }
                ndp.xreturn[0] = SE.swe_degnorm(ndp.xreturn[0]);
                ndp.xreturn[12] = SE.swe_degnorm(ndp.xreturn[12]);
                /*}*/
            }
            return OK;
        }

        /* lunar osculating elements, i.e.
         */
        int intp_apsides(double tjd, int ipl, Int32 iflag, ref string serr) {
            int i;
            Int32 flg1, flg2;
            plan_data ndp;
            epsilon oe;
            nut nut;
            double daya;
            double speed_intv = 0.1;
            double t, dt;
            double[][] xpos = CreateArray<Double>(3, 6); double[] xx = new double[6], x = new double[6];
            Int32 speedf1, speedf2;
            oe = swed.oec;
            nut = swed.nut;
            ndp = swed.nddat[ipl];
            /* if same calculation was done before, return
             * if speed flag has been turned on, recompute */
            flg1 = iflag & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            flg2 = ndp.xflgs & ~SwissEph.SEFLG_EQUATORIAL & ~SwissEph.SEFLG_XYZ;
            speedf1 = ndp.xflgs & SwissEph.SEFLG_SPEED;
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            if (tjd == ndp.teval
              && tjd != 0
              && flg1 == flg2
              && (0 == speedf2 || speedf1 != 0)) {
                ndp.xflgs = iflag;
                ndp.iephe = iflag & SwissEph.SEFLG_MOSEPH;
                return OK;
            }
            /*********************************************
             * now three apsides * 
             *********************************************/
            for (t = tjd - speed_intv, i = 0; i < 3; t += speed_intv, i++) {
                if (0 == (iflag & SwissEph.SEFLG_SPEED) && i != 1) continue;
                SE.SwemMoon.swi_intp_apsides(t, xpos[i], ipl);
            }
            /************************************************************
             * apsis with speed                                         * 
             ************************************************************/
            for (i = 0; i < 3; i++) {
                xx[i] = xpos[1][i];
                xx[i + 3] = 0;
            }
            if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                xx[3] = SE.swe_difrad2n(xpos[2][0], xpos[0][0]) / speed_intv / 2.0;
                xx[4] = (xpos[2][1] - xpos[0][1]) / speed_intv / 2.0;
                xx[5] = (xpos[2][2] - xpos[0][2]) / speed_intv / 2.0;
            }
            //memset((void*)ndp.xreturn, 0, 24 * sizeof(double));
            for (int ii = 0; ii < 24; ii++) {
                ndp.xreturn[ii] = 0;
            }
            /* ecliptic polar to cartesian */
            SE.SwephLib.swi_polcart_sp(xx, xx);
            /* light-time */
            if (0 == (iflag & SwissEph.SEFLG_TRUEPOS)) {
                dt = Math.Sqrt(square_sum(xx)) * AUNIT / CLIGHT / 86400.0;
                for (i = 1; i < 3; i++)
                    xx[i] -= dt * xx[i + 3];
            }
            for (i = 0; i <= 5; i++)
                ndp.xreturn[i + 6] = xx[i];
            /*printf("%.10f, %.10f, %.10f, %.10f\n", xx[0] /DEGTORAD, xx[1] / DEGTORAD, xx [2], xx[3] /DEGTORAD);*/
            /* equatorial cartesian */
            SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(6), ndp.xreturn.GetPointer(18), -oe.seps, oe.ceps);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(9), ndp.xreturn.GetPointer(21), -oe.seps, oe.ceps);
            ndp.teval = tjd;
            ndp.xflgs = iflag;
            ndp.iephe = iflag & SwissEph.SEFLG_EPHMASK;
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                /* apogee is referred to t; 
                 * the ecliptic position must be transformed to t0 */
                /* rigorous algorithm */
                if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0
                || (swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                    for (i = 0; i <= 5; i++)
                        x[i] = ndp.xreturn[18 + i];
                    /* precess to J2000 */
                    SE.SwephLib.swi_precess(x, tjd, iflag, J_TO_J2000);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        swi_precess_speed(x, tjd, iflag, J_TO_J2000);
                    if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0)
                        swi_trop_ra2sid_lon(x, ndp.xreturn.GetPointer(6), ndp.xreturn.GetPointer(18), iflag);
                    /* project onto solar system equator */
                    else if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0)
                        swi_trop_ra2sid_lon_sosy(x, ndp.xreturn.GetPointer(6), iflag);
                    /* to polar */
                    SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                    SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                } else {
                    /* traditional algorithm */
                    SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
                    if (SE.swe_get_ayanamsa_ex(ndp.teval, iflag, out daya, ref serr) == ERR)
                        return ERR;
                    ndp.xreturn[0] -= daya * SwissEph.DEGTORAD;
                    SE.SwephLib.swi_polcart_sp(ndp.xreturn, ndp.xreturn.GetPointer(6));
                    SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                }
            } else if ((iflag & SwissEph.SEFLG_J2000) != 0) {
                /* node and apogee are referred to t; 
                 * the ecliptic position must be transformed to J2000 */
                for (i = 0; i <= 5; i++)
                    x[i] = ndp.xreturn[18 + i];
                /* precess to J2000 */
                SE.SwephLib.swi_precess(x, tjd, iflag, J_TO_J2000);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(x, tjd, iflag, J_TO_J2000);
                for (i = 0; i <= 5; i++)
                    ndp.xreturn[18 + i] = x[i];
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(6), swed.oec2000.seps, swed.oec2000.ceps);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(21), ndp.xreturn.GetPointer(9), swed.oec2000.seps, swed.oec2000.ceps);
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
            } else {
                /* tropical ecliptic positions */
                /* precession has already been taken into account, but not nutation */
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    swi_nutate(ndp.xreturn.GetPointer(18), iflag, false);
                }
                /* equatorial polar */
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(12));
                /* ecliptic cartesian */
                SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(18), ndp.xreturn.GetPointer(6), oe.seps, oe.ceps);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(21), ndp.xreturn.GetPointer(9), oe.seps, oe.ceps);
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(6), ndp.xreturn.GetPointer(6), nut.snut, nut.cnut);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.SwephLib.swi_coortrf2(ndp.xreturn.GetPointer(9), ndp.xreturn.GetPointer(9), nut.snut, nut.cnut);
                }
                /* ecliptic polar */
                SE.SwephLib.swi_cartpol_sp(ndp.xreturn.GetPointer(6), ndp.xreturn);
            }
            /********************** 
             * radians to degrees *
             **********************/
            /*if (!(iflag & SwissEph.SEFLG_RADIANS)) {*/
            for (i = 0; i < 2; i++) {
                ndp.xreturn[i] *= SwissEph.RADTODEG;		/* ecliptic */
                ndp.xreturn[i + 3] *= SwissEph.RADTODEG;
                ndp.xreturn[i + 12] *= SwissEph.RADTODEG;	/* equator */
                ndp.xreturn[i + 15] *= SwissEph.RADTODEG;
            }
            ndp.xreturn[0] = SE.SwephLib.swe_degnorm(ndp.xreturn[0]);
            ndp.xreturn[12] = SE.SwephLib.swe_degnorm(ndp.xreturn[12]);
            /*}*/
            return OK;
        }

        /* transforms the position of the moon in a way we can use it
         * for calculation of osculating node and apogee:
         * precession and nutation (attention to speed vector!)
         * according to flags
         * iflag	flags
         * tjd          time for which the element is computed
         *              i.e. date of ecliptic
         * xx           array equatorial cartesian position and speed
         * serr         error string
         */
        public int swi_plan_for_osc_elem(Int32 iflag, double tjd, CPointer<double> xx) {
            int i;
            double[] x = new double[6];
            nut nuttmp = new nut();
            nut nutp = nuttmp;	/* dummy assign, to silence gcc warning */
            epsilon oe = swed.oec;
            epsilon oectmp = new epsilon();
            sid_data sip = new sid_data(); ;
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && get_denum(SEI_SUN, iflag) >= 403) {
                SE.SwephLib.swi_bias(xx, tjd, iflag, false);
            }/**/
            /************************************************
             * precession, equator 2000 -> equator of date  *
             * attention: speed vector has to be rotated,   *
             * but daily precession 0.137" may not be added!*/
            /*
#if SID_TNODE_FROM_ECL_T0
  sid_data sip = &swed.sidd;
  /* For sidereal calculation we need node refered*
   * to ecliptic of t0 of ayanamsa                *
   ************************************************ /
  if (iflag & SEFLG_SIDEREAL) {
    tjd = sip.t0;
    swi_precess(xx, tjd, iflag, J2000_TO_J);
    swi_precess(xx+3, tjd, iflag, J2000_TO_J); 
    calc_epsilon(tjd, iflag, &oectmp);
    oe = &oectmp;
  } else if (!(iflag & SEFLG_J2000)) {
#endif
            swi_precess(xx, tjd, iflag, J2000_TO_J);
            swi_precess(xx + 3, tjd, iflag, J2000_TO_J);
            /* epsilon * /
            if (tjd == swed.oec.teps)
                oe = swed.oec;
            else if (tjd == J2000)
                oe = swed.oec2000;
            else {
                calc_epsilon(tjd, iflag, oectmp);
                oe = oectmp;
            }
#if SID_TNODE_FROM_ECL_T0
  } else	/* if SEFLG_J2000 * /
    oe = &swed.oec2000;
#endif
            */

            if (SID_TNODE_FROM_ECL_T0) {
                sip = swed.sidd;
                /* For sidereal calculation we need node refered*
                 * to ecliptic of t0 of ayanamsa                *
                 ************************************************/
                if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                    tjd = sip.t0;
                    SE.SwephLib.swi_precess(xx, tjd, iflag, J2000_TO_J);
                    SE.SwephLib.swi_precess(xx + 3, tjd, iflag, J2000_TO_J);
                    calc_epsilon(tjd, iflag, oectmp);
                    oe = oectmp;
                } else if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                    SE.SwephLib.swi_precess(xx, tjd, iflag, J2000_TO_J);
                    SE.SwephLib.swi_precess(xx + 3, tjd, iflag, J2000_TO_J);
                    /* epsilon */
                    if (tjd == swed.oec.teps)
                        oe = swed.oec;
                    else if (tjd == J2000)
                        oe = swed.oec2000;
                    else {
                        calc_epsilon(tjd, iflag, oectmp);
                        oe = oectmp;
                    }

                } else	/* if SEFLG_J2000 */
                    oe = swed.oec2000;
            } else {
                SE.SwephLib.swi_precess(xx, tjd, iflag, J2000_TO_J);
                SE.SwephLib.swi_precess(xx + 3, tjd, iflag, J2000_TO_J);
                /* epsilon */
                if (tjd == swed.oec.teps)
                    oe = swed.oec;
                else if (tjd == J2000)
                    oe = swed.oec2000;
                else {
                    calc_epsilon(tjd, iflag, oectmp);
                    oe = oectmp;
                }
            }

            /************************************************
               * nutation                                     *
               * again: speed vector must be rotated, but not *
               * added 'speed' of nutation                    *
               ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                if (tjd == swed.nut.tnut) {
                    nutp = swed.nut;
                } else if (tjd == J2000) {
                    nutp = swed.nut2000;
                } else if (tjd == swed.nutv.tnut) {
                    nutp = swed.nutv;
                } else {
                    nutp = nuttmp;
                    SE.SwephLib.swi_nutation(tjd, iflag, nutp.nutlo);
                    nutp.tnut = tjd;
                    nutp.snut = Math.Sin(nutp.nutlo[1]);
                    nutp.cnut = Math.Cos(nutp.nutlo[1]);
                    nut_matrix(nutp, oe);
                }
                for (i = 0; i <= 2; i++) {
                    x[i] = xx[0] * nutp.matrix[0, i] +
                       xx[1] * nutp.matrix[1, i] +
                       xx[2] * nutp.matrix[2, i];
                }
                /* speed:
                 * rotation only */
                for (i = 0; i <= 2; i++) {
                    x[i + 3] = xx[3] * nutp.matrix[0, i] +
                         xx[4] * nutp.matrix[1, i] +
                         xx[5] * nutp.matrix[2, i];
                }
                for (i = 0; i <= 5; i++)
                    xx[i] = x[i];
            }
            /************************************************
             * transformation to ecliptic                   *
             ************************************************/
            SE.SwephLib.swi_coortrf2(xx, xx, oe.seps, oe.ceps);
            SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, oe.seps, oe.ceps);
            if (SID_TNODE_FROM_ECL_T0 && (iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                /* subtract ayan_t0 */
                SE.SwephLib.swi_cartpol_sp(xx, xx);
                xx[0] -= sip.ayan_t0;
                SE.SwephLib.swi_polcart_sp(xx, xx);
            } else
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(xx, xx, nutp.snut, nutp.cnut);
                    SE.SwephLib.swi_coortrf2(xx + 3, xx + 3, nutp.snut, nutp.cnut);
                }
            return (OK);
        }

        static meff_ele[] eff_arr = new meff_ele[] {
          /*
           * r , m_eff for photon passing the sun at min distance r (fraction of Rsun)
           * the values where computed with sun_model.c, which is a classic
           * treatment of a photon passing a gravity field, multiplied by 2.
           * The sun mass distribution m(r) is from Michael Stix, The Sun, p. 47.
           */
          new meff_ele(1.000, 1.000000),
          new meff_ele(0.990, 0.999979),
          new meff_ele(0.980, 0.999940),
          new meff_ele(0.970, 0.999881),
          new meff_ele(0.960, 0.999811),
          new meff_ele(0.950, 0.999724),
          new meff_ele(0.940, 0.999622),
          new meff_ele(0.930, 0.999497),
          new meff_ele(0.920, 0.999354),
          new meff_ele(0.910, 0.999192),
          new meff_ele(0.900, 0.999000),
          new meff_ele(0.890, 0.998786),
          new meff_ele(0.880, 0.998535),
          new meff_ele(0.870, 0.998242),
          new meff_ele(0.860, 0.997919),
          new meff_ele(0.850, 0.997571),
          new meff_ele(0.840, 0.997198),
          new meff_ele(0.830, 0.996792),
          new meff_ele(0.820, 0.996316),
          new meff_ele(0.810, 0.995791),
          new meff_ele(0.800, 0.995226),
          new meff_ele(0.790, 0.994625),
          new meff_ele(0.780, 0.993991),
          new meff_ele(0.770, 0.993326),
          new meff_ele(0.760, 0.992598),
          new meff_ele(0.750, 0.991770),
          new meff_ele(0.740, 0.990873),
          new meff_ele(0.730, 0.989919),
          new meff_ele(0.720, 0.988912),
          new meff_ele(0.710, 0.987856),
          new meff_ele(0.700, 0.986755),
          new meff_ele(0.690, 0.985610),
          new meff_ele(0.680, 0.984398),
          new meff_ele(0.670, 0.982986),
          new meff_ele(0.660, 0.981437),
          new meff_ele(0.650, 0.979779),
          new meff_ele(0.640, 0.978024),
          new meff_ele(0.630, 0.976182),
          new meff_ele(0.620, 0.974256),
          new meff_ele(0.610, 0.972253),
          new meff_ele(0.600, 0.970174),
          new meff_ele(0.590, 0.968024),
          new meff_ele(0.580, 0.965594),
          new meff_ele(0.570, 0.962797),
          new meff_ele(0.560, 0.959758),
          new meff_ele(0.550, 0.956515),
          new meff_ele(0.540, 0.953088),
          new meff_ele(0.530, 0.949495),
          new meff_ele(0.520, 0.945741),
          new meff_ele(0.510, 0.941838),
          new meff_ele(0.500, 0.937790),
          new meff_ele(0.490, 0.933563),
          new meff_ele(0.480, 0.928668),
          new meff_ele(0.470, 0.923288),
          new meff_ele(0.460, 0.917527),
          new meff_ele(0.450, 0.911432),
          new meff_ele(0.440, 0.905035),
          new meff_ele(0.430, 0.898353),
          new meff_ele(0.420, 0.891022),
          new meff_ele(0.410, 0.882940),
          new meff_ele(0.400, 0.874312),
          new meff_ele(0.390, 0.865206),
          new meff_ele(0.380, 0.855423),
          new meff_ele(0.370, 0.844619),
          new meff_ele(0.360, 0.833074),
          new meff_ele(0.350, 0.820876),
          new meff_ele(0.340, 0.808031),
          new meff_ele(0.330, 0.793962),
          new meff_ele(0.320, 0.778931),
          new meff_ele(0.310, 0.763021),
          new meff_ele(0.300, 0.745815),
          new meff_ele(0.290, 0.727557),
          new meff_ele(0.280, 0.708234),
          new meff_ele(0.270, 0.687583),
          new meff_ele(0.260, 0.665741),
          new meff_ele(0.250, 0.642597),
          new meff_ele(0.240, 0.618252),
          new meff_ele(0.230, 0.592586),
          new meff_ele(0.220, 0.565747),
          new meff_ele(0.210, 0.537697),
          new meff_ele(0.200, 0.508554),
          new meff_ele(0.190, 0.478420),
          new meff_ele(0.180, 0.447322),
          new meff_ele(0.170, 0.415454),
          new meff_ele(0.160, 0.382892),
          new meff_ele(0.150, 0.349955),
          new meff_ele(0.140, 0.316691),
          new meff_ele(0.130, 0.283565),
          new meff_ele(0.120, 0.250431),
          new meff_ele(0.110, 0.218327),
          new meff_ele(0.100, 0.186794),
          new meff_ele(0.090, 0.156287),
          new meff_ele(0.080, 0.128421),
          new meff_ele(0.070, 0.102237),
          new meff_ele(0.060, 0.077393),
          new meff_ele(0.050, 0.054833),
          new meff_ele(0.040, 0.036361),
          new meff_ele(0.030, 0.020953),
          new meff_ele(0.020, 0.009645),
          new meff_ele(0.010, 0.002767),
          new meff_ele(0.000, 0.000000)
        };
        double meff(double r) {
            double f, m;
            int i;
            if (r <= 0)
                return 0.0;
            else if (r >= 1)
                return 1.0;
            for (i = 0; eff_arr[i].r > r; i++)
                ;	/* empty body */
            f = (r - eff_arr[i - 1].r) / (eff_arr[i].r - eff_arr[i - 1].r);
            m = eff_arr[i - 1].m + f * (eff_arr[i].m - eff_arr[i - 1].m);
            return m;
        }

        void denormalize_positions(CPointer<double> x0, CPointer<double> x1, CPointer<double> x2) {
            int i;
            /* x*[0] = ecliptic longitude, x*[12] = rectascension */
            for (i = 0; i <= 12; i += 12) {
                if (x1[i] - x0[i] < -180)
                    x0[i] -= 360;
                if (x1[i] - x0[i] > 180)
                    x0[i] += 360;
                if (x1[i] - x2[i] < -180)
                    x2[i] -= 360;
                if (x1[i] - x2[i] > 180)
                    x2[i] += 360;
            }
        }

        void calc_speed(CPointer<double> x0, CPointer<double> x1, CPointer<double> x2, double dt) {
            int i, j, k;
            double a, b;
            for (j = 0; j <= 18; j += 6) {
                for (i = 0; i < 3; i++) {
                    k = j + i;
                    b = (x2[k] - x0[k]) / 2;
                    a = (x2[k] + x0[k]) / 2 - x1[k];
                    x1[k + 3] = (2 * a + b) / dt;
                }
            }
        }

        void swi_check_ecliptic(double tjd, Int32 iflag) {
            if (swed.oec2000.teps != J2000) {
                calc_epsilon(J2000, iflag, swed.oec2000);
            }
            if (tjd == J2000) {
                swed.oec.teps = swed.oec2000.teps;
                swed.oec.eps = swed.oec2000.eps;
                swed.oec.seps = swed.oec2000.seps;
                swed.oec.ceps = swed.oec2000.ceps;
                return;
            }
            if (swed.oec.teps != tjd || tjd == 0) {
                calc_epsilon(tjd, iflag, swed.oec);
            }
        }

        Int32 nutflag = 0;  // static var in swi_check_nutation
        /* computes nutation, if it is wanted and has not yet been computed.
         * if speed flag has been turned on since last computation, 
         * nutation is recomputed */
        void swi_check_nutation(double tjd, Int32 iflag) {
            Int32 speedf1, speedf2;
            double t;
            speedf1 = nutflag & SwissEph.SEFLG_SPEED;
            speedf2 = iflag & SwissEph.SEFLG_SPEED;
            if ((iflag & SwissEph.SEFLG_NONUT) == 0
              && (tjd != swed.nut.tnut || tjd == 0
              || (speedf1 == 0 && speedf2 != 0))) {
                  SE.SwephLib.swi_nutation(tjd, iflag, swed.nut.nutlo);
                swed.nut.tnut = tjd;
                swed.nut.snut = Math.Sin(swed.nut.nutlo[1]);
                swed.nut.cnut = Math.Cos(swed.nut.nutlo[1]);
                nutflag = iflag;
                nut_matrix(swed.nut, swed.oec);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    /* once more for 'speed' of nutation, which is needed for 
                     * planetary speeds */
                    t = tjd - NUT_SPEED_INTV;
                    SE.SwephLib.swi_nutation(t, iflag, swed.nutv.nutlo);
                    swed.nutv.tnut = t;
                    swed.nutv.snut = Math.Sin(swed.nutv.nutlo[1]);
                    swed.nutv.cnut = Math.Cos(swed.nutv.nutlo[1]);
                    nut_matrix(swed.nutv, swed.oec);
                }
            }
        }

        Int32 plaus_iflag(Int32 iflag, Int32 ipl, double tjd, out string serr) {
            Int32 epheflag = 0;
            serr = null;
            int jplhor_model = swed.astro_models[SwissEph.SE_MODEL_JPLHOR_MODE];
            int jplhora_model = swed.astro_models[SwissEph.SE_MODEL_JPLHORA_MODE];
            if (jplhor_model == 0) jplhor_model = SwissEph.SEMOD_JPLHOR_DEFAULT;
            if (jplhora_model == 0) jplhora_model = SwissEph.SEMOD_JPLHORA_DEFAULT;
            /* either Horizons mode or simplified Horizons mode, not both */
            if ((iflag & SwissEph.SEFLG_JPLHOR) != 0)
                iflag &= ~SwissEph.SEFLG_JPLHOR_APPROX;
            /* if topocentric bit, turn helio- and barycentric bits off;
             */
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                iflag = iflag & ~(SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR);
            }
            /* if heliocentric bit, turn aberration and deflection off */
            if ((iflag & SwissEph.SEFLG_HELCTR) != 0)
                iflag |= SwissEph.SEFLG_NOABERR | SwissEph.SEFLG_NOGDEFL; /*iflag |= SwissEph.SEFLG_TRUEPOS;*/
            /* same, if barycentric bit */
            if ((iflag & SwissEph.SEFLG_BARYCTR) != 0)
                iflag |= SwissEph.SEFLG_NOABERR | SwissEph.SEFLG_NOGDEFL; /*iflag |= SwissEph.SEFLG_TRUEPOS;*/
            /* if no_precession bit is set, set also no_nutation bit */
            if ((iflag & SwissEph.SEFLG_J2000) != 0)
                iflag |= SwissEph.SEFLG_NONUT;
            /* if sidereal bit is set, set also no_nutation bit *
             * also turn JPL Horizons mode off */
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                iflag |= SwissEph.SEFLG_NONUT;
                iflag = iflag & ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            }
            /* if truepos is set, turn off grav. defl. and aberration */
            if ((iflag & SwissEph.SEFLG_TRUEPOS) != 0)
                iflag |= (SwissEph.SEFLG_NOGDEFL | SwissEph.SEFLG_NOABERR);
            if ((iflag & SwissEph.SEFLG_MOSEPH) != 0)
                epheflag = SwissEph.SEFLG_MOSEPH;
            if ((iflag & SwissEph.SEFLG_SWIEPH) != 0)
                epheflag = SwissEph.SEFLG_SWIEPH;
            if ((iflag & SwissEph.SEFLG_JPLEPH) != 0)
                epheflag = SwissEph.SEFLG_JPLEPH;
            if (epheflag == 0)
                epheflag = SwissEph.SEFLG_DEFAULTEPH;
            iflag = (iflag & ~SwissEph.SEFLG_EPHMASK) | epheflag;
            /* SwissEph.SEFLG_JPLHOR only with JPL and Swiss Ephemeeris */
            if ((epheflag & SwissEph.SEFLG_JPLEPH) == 0)
                iflag = iflag & ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            /* planets that have no JPL Horizons mode */
            if (ipl == SwissEph.SE_OSCU_APOG || ipl == SwissEph.SE_TRUE_NODE
                || ipl == SwissEph.SE_MEAN_APOG || ipl == SwissEph.SE_MEAN_NODE
                || ipl == SwissEph.SE_INTP_APOG || ipl == SwissEph.SE_INTP_PERG)
                iflag = iflag & ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            if (ipl >= SwissEph.SE_FICT_OFFSET && ipl <= SwissEph.SE_FICT_MAX)
                iflag = iflag & ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            /* SwissEph.SEFLG_JPLHOR requires SwissEph.SEFLG_ICRS, if calculated with * precession/nutation IAU 1980 and corrections dpsi, deps */
            if ((iflag & SwissEph.SEFLG_JPLHOR) != 0) {
                if (swed.eop_dpsi_loaded <= 0
                   || ((tjd < swed.eop_tjd_beg || tjd > swed.eop_tjd_end)
                   && jplhor_model != SwissEph.SEMOD_JPLHOR_EXTENDED_1800))
                {
                    /*&& !USE_HORIZONS_METHOD_BEFORE_1980)) {*/
                    switch (swed.eop_dpsi_loaded)
                    {
                        case 0:
                            serr = "you did not call swe_set_jpl_file(); default to SEFLG_JPLHOR_APPROX";
                            break;
                        case -1:
                            serr = "file eop_1962_today.txt not found; default to SEFLG_JPLHOR_APPROX";
                            break;
                        case -2:
                            serr = "file eop_1962_today.txt corrupt; default to SEFLG_JPLHOR_APPROX";
                            break;
                        case -3:
                            serr = "file eop_finals.txt corrupt; default to SEFLG_JPLHOR_APPROX";
                            break;
                    }
                    iflag &= ~SwissEph.SEFLG_JPLHOR;
                    iflag |= SwissEph.SEFLG_JPLHOR_APPROX;
                }
            }
            if ((iflag & SwissEph.SEFLG_JPLHOR) != 0)
                iflag |= SwissEph.SEFLG_ICRS;
            /*if ((iflag & SwissEph.SEFLG_JPLHOR_APPROX) && FRAME_BIAS_APPROX_HORIZONS) */
            /*if ((iflag & SEFLG_JPLHOR_APPROX) && !APPROXIMATE_HORIZONS_ASTRODIENST)*/
            if ((iflag & SwissEph.SEFLG_JPLHOR_APPROX)!=0 && jplhora_model != SwissEph.SEMOD_JPLHORA_1)
                iflag |= SwissEph.SEFLG_ICRS;
            return iflag;
        }

        /**********************************************************
         * get fixstar positions
         * parameters:
         * star 	name of star or line number in star file 
         *		(start from 1, don't count comment).
         *    		If no error occurs, the name of the star is returned
         *	        in the format trad_name, nomeclat_name
         *
         * tjd 		absolute julian day
         * iflag	s. swecalc(); speed bit does not function
         * x		pointer for returning the ecliptic coordinates
         * serr		error return string
        **********************************************************/
        string slast_stardata = String.Empty;
        string slast_starname = String.Empty;
        string sdummy = null;
        public Int32 swe_fixstar(string star, double tjd, Int32 iflag,
          double[] xx, ref string serr) {
            int i;
            int star_nr = 0;
            bool isnomclat = false;
            int cmplen;
            double daya;
            double[] x = new double[6], xxsv = new double[6], xobs = new double[6]; CPointer<double> xpo = null;
            string[] cpos;
            string sstar = String.Empty;
            string fstar = String.Empty;
            string s = String.Empty; string sp = String.Empty/*, sp2*/;	/* 20 byte for SE_STARFILE */
            double ra_s, ra_pm, de_pm, ra, de, t, cosra, cosde, sinra, sinde;
            double ra_h, ra_m, de_d, de_m, de_s;
            string sde_d;
            double epoch, radv, parall, u;
            int line = 0;
            int fline = 0;
            plan_data pedp = swed.pldat[SEI_EARTH];
            plan_data psdp = swed.pldat[SEI_SUNBARY];
            epsilon oe = swed.oec2000;
            int retc = 0;
            Int32 epheflag, iflgsave;
            iflag |= SwissEph.SEFLG_SPEED; /* we need this in order to work correctly */
            iflgsave = iflag;
            serr = String.Empty;
#if TRACE
            //swi_open_trace(serr);
            trace_swe_fixstar(1, star, tjd, iflag, xx, serr);
#endif //* TRACE */
            iflag = plaus_iflag(iflag, -1, tjd, out serr);
              epheflag = iflag & SwissEph.SEFLG_EPHMASK;
              if (swi_init_swed_if_start() == 1 && 0==(epheflag & SwissEph.SEFLG_MOSEPH) ) {
                serr= "Please call swe_set_ephe_path() or swe_set_jplfile() before calling swe_fixstar() or swe_fixstar_ut()";
              }
              if (swed.last_epheflag != epheflag)
              {
                  free_planets();
                  /* close and free ephemeris files */
                  if (swed.jpl_file_is_open)
                  {
                      SE.SweJPL.swi_close_jpl_file();
                      swed.jpl_file_is_open = false;
                  }
                  for (i = 0; i < SEI_NEPHFILES; i++)
                  {
                      if (swed.fidat[i].fptr != null)
                          swed.fidat[i].fptr.Dispose();
                      swed.fidat[i] = new file_data();
                  }
                  swed.last_epheflag = epheflag;
              }
              /* high precision speed prevails fast speed */
            /* JPL Horizons is only reproduced with SwissEph.SEFLG_JPLEPH */
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0 && !swed.ayana_is_set)
                swe_set_sid_mode(SwissEph.SE_SIDM_FAGAN_BRADLEY, 0, 0);
            /****************************************** 
             * obliquity of ecliptic 2000 and of date * 
             ******************************************/
            swi_check_ecliptic(tjd, iflag);
            /******************************************
             * nutation                               * 
             ******************************************/
            swi_check_nutation(tjd, iflag);
            sstar = star.Length >= SwissEph.SE_MAX_STNAME ? star.Substring(0, SwissEph.SE_MAX_STNAME) : star;
            //sstar[SE_MAX_STNAME] = '\0';
            if (sstar[0] == ',') {
                isnomclat = true;
            } else if (Char.IsDigit(sstar[0])) {
                star_nr = int.Parse(sstar);
            } else {
                /* traditional name of star to lower case */
                //for (sp = sstar; *sp != '\0'; sp++)
                //    *sp = tolower((int)*sp);
                //if ((sp = strchr(sstar, ',')) != NULL)
                //    *sp = '\0';
                sstar = sstar.ToLower();
            }
            /*swi_right_trim(sstar);*/
            //while ((sp = strchr(sstar, ' ')) != NULL)
            //    sp = sp + 1;
            sstar = sstar.TrimEnd();
            cmplen = sstar.Length;
            if (cmplen == 0) {
                serr = C.sprintf("swe_fixstar(): star name empty");
                retc = ERR;
                goto return_err;
            }
            /* star elements from last call: */
            //if (*slast_stardata != '\0' && strcmp(slast_starname, sstar) == 0) {
            if (!String.IsNullOrWhiteSpace(slast_stardata) && slast_starname.Equals(sstar)) {
                s = slast_stardata;
                goto found;
            }
            /******************************************************
             * Star file
             * close to the beginning, a few stars selected by Astrodienst.
             * These can be accessed by giving their number instead of a name.
             * All other stars can be accessed by name.
             * Comment lines start with # and are ignored.
             ******************************************************/
            if (swed.fixfp == null) {
                if ((swed.fixfp = swi_fopen(SEI_FILE_FIXSTAR, SwissEph.SE_STARFILE, swed.ephepath, ref serr)) == null) {
                    swed.is_old_starfile = true;
                    if ((swed.fixfp = swi_fopen(SEI_FILE_FIXSTAR, SwissEph.SE_STARFILE_OLD, swed.ephepath, ref sdummy)) == null) {
                        swed.is_old_starfile = false;
                        /* no fixed star file available. If Spica is called, we provide it
                         * even without a star file, because Spica is required for the
                         * Ayanamsha SE_SIDM_TRUE_CITRA */
                        if (star.StartsWith("Spica")) {
                            s = "Spica,alVir,ICRS,13,25,11.5793,-11,09,40.759,-42.50,-31.73,1.0,12.44,1.04,-10,3672";
                            sstar = "spica";
                            goto found;
                            /* Ayanamsha SE_SIDM_TRUE_REVATI */
                        }
                        else if (star.Contains(",zePsc") || star.StartsWith("Revati"))
                        {
                            s = "Revati,zePsc,ICRS,01,13,43.8857,7,34,31.274,141.66,-55.62,0.0,22.09,5.204,06,174";
                            /* Ayanamsha SE_SIDM_TRUE_PUSHYA */
                            sstar = "revati";
                            goto found;
                        }
                        else if (star.Contains(",deCnc") || star.StartsWith("Pushya"))
                        {
                            s = "Pushya,deCnc,ICRS,08,44,41.0996,+18,09,15.511,-17.10,-228.46,17.14,23.97,3.94,18,2027";
                            sstar = "pushya";
                            goto found;
                        }
                        retc = ERR;
                        goto return_err;
                    }
                }
            }
            swed.fixfp.Seek(0, SeekOrigin.Begin);
            while ((s = swed.fixfp.ReadLine()) != null) {
                fline++;
                if (s.StartsWith("#")) continue;
                line++;
                if (star_nr == line)
                    goto found;
                else if (star_nr > 0)
                    continue;
                var spi = s.IndexOf(',');
                if (spi < 0) {
                    serr = C.sprintf("star file %s damaged at line %d", SwissEph.SE_STARFILE, fline);
                    retc = ERR;
                    goto return_err;
                }
                if (isnomclat) {
                    if (!sp.StartsWith(sstar.Substring(0, cmplen)))
                        goto found;
                    else
                        continue;
                }
                fstar = s.Substring(0, spi);
                fstar = fstar.TrimEnd();
                i = fstar.Length;
                if (i < (int)cmplen)
                    continue;
                if (sstar.Equals(fstar, StringComparison.CurrentCultureIgnoreCase))
                    goto found;
            }
            serr = C.sprintf("star %s not found", star);
            retc = ERR;
            goto return_err;
        found:
            slast_stardata = s;
            slast_starname = sstar;
            //i = swi_cutstr(s, ",", cpos, 20);
            //swi_right_trim(cpos[0]);
            //swi_right_trim(cpos[1]);
            cpos = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            i = cpos.Length;
            cpos[0] = cpos[0].TrimEnd();
            cpos[1] = cpos[1].TrimEnd();
            if (i < 13) {
                serr = C.sprintf("data of star '%s,%s' incomplete", cpos[0], cpos[1]);
                retc = ERR;
                goto return_err;
            }
            epoch = C.atof(cpos[2]);
            ra_h = C.atof(cpos[3]);
            ra_m = C.atof(cpos[4]);
            ra_s = C.atof(cpos[5]);
            de_d = C.atof(cpos[6]);
            sde_d = cpos[6];
            de_m = C.atof(cpos[7]);
            de_s = C.atof(cpos[8]);
            ra_pm = C.atof(cpos[9]);
            de_pm = C.atof(cpos[10]);
            radv = C.atof(cpos[11]);
            parall = C.atof(cpos[12]);
            /* return trad. name, nomeclature name */
            if (cpos[0].Length > SwissEph.SE_MAX_STNAME)
                cpos[0] = cpos[0].Substring(0, SwissEph.SE_MAX_STNAME);
            if (cpos[1].Length > SwissEph.SE_MAX_STNAME - 1)
                cpos[1] = cpos[1].Substring(0, SwissEph.SE_MAX_STNAME);
            star = cpos[0];
            if (cpos[0].Length + cpos[1].Length + 1 < SwissEph.SE_MAX_STNAME - 1)
                star += C.sprintf(",%s", cpos[1]);
            /****************************************
             * position and speed (equinox)
             ****************************************/
            /* ra and de in degrees */
            ra = (ra_s / 3600.0 + ra_m / 60.0 + ra_h) * 15.0;
            if (sde_d.IndexOf('-') < 0)
                de = de_s / 3600.0 + de_m / 60.0 + de_d;
            else
                de = -de_s / 3600.0 - de_m / 60.0 + de_d;
            /* speed in ra and de, degrees per century */
            if (swed.is_old_starfile == true) {
                ra_pm = ra_pm * 15 / 3600.0;
                de_pm = de_pm / 3600.0;
            } else {
                ra_pm = ra_pm / 10.0 / 3600.0;
                de_pm = de_pm / 10.0 / 3600.0;
                parall /= 1000.0;
            }
            /* parallax, degrees */
            if (parall > 1)
                parall = (1 / parall / 3600.0);
            else
                parall /= 3600;
            /* radial velocity in AU per century */
            radv *= KM_S_TO_AU_CTY;
            /*printf("ra=%.17f,de=%.17f,ma=%.17f,md=%.17f,pa=%.17f,rv=%.17f\n",ra,de,ra_pm,de_pm,parall,radv);*/
            /* radians */
            ra *= SwissEph.DEGTORAD;
            de *= SwissEph.DEGTORAD;
            ra_pm *= SwissEph.DEGTORAD;
            de_pm *= SwissEph.DEGTORAD;
            ra_pm /= Math.Cos(de); /* catalogues give proper motion in RA as great circle */
            parall *= SwissEph.DEGTORAD;
            x[0] = ra;
            x[1] = de;
            x[2] = 1;	/* -> unit vector */
            /* cartesian */
            SE.SwephLib.swi_polcart(x, x);
            /*space motion vector */
            cosra = Math.Cos(ra);
            cosde = Math.Cos(de);
            sinra = Math.Sin(ra);
            sinde = Math.Sin(de);
            x[3] = -ra_pm * cosde * sinra - de_pm * sinde * cosra
                      + radv * parall * cosde * cosra;
            x[4] = ra_pm * cosde * cosra - de_pm * sinde * sinra
                      + radv * parall * cosde * sinra;
            x[5] = de_pm * cosde + radv * parall * sinde;
            x[3] /= 36525;
            x[4] /= 36525;
            x[5] /= 36525;
            /******************************************
             * FK5
             ******************************************/
            if (epoch == 1950) {
                SE.SwephLib.swi_FK4_FK5(x, B1950);
                SE.SwephLib.swi_precess(x, B1950, 0, J_TO_J2000);
                SE.SwephLib.swi_precess(x.GetPointer(3), B1950, 0, J_TO_J2000);
            }
            /* FK5 to ICRF, if jpl ephemeris is referred to ICRF.
             * With data that are already ICRF, epoch = 0 */
            if (epoch != 0) {
                SE.SwephLib.swi_icrs2fk5(x, iflag, true); /* backward, i. e. to icrf */
                /* with ephemerides < DE403, we now convert to J2000 */
                if (get_denum(SEI_SUN, iflag) >= 403) {
                    SE.SwephLib.swi_bias(x, J2000, SwissEph.SEFLG_SPEED, false);
                }
            }
            //#if 0
            //  if (((iflag & SEFLG_NOGDEFL) == 0 || (iflag & SEFLG_NOABERR) == 0)
            //    && (iflag & SEFLG_HELCTR) == 0
            //    && (iflag & SEFLG_BARYCTR) == 0
            //    && (iflag & SEFLG_TRUEPOS) == 0) 
            //#endif
            /**************************************************** 
             * earth/sun 
             * for parallax, light deflection, and aberration,
             ****************************************************/
            if (0 == (iflag & SwissEph.SEFLG_BARYCTR) && (0 == (iflag & SwissEph.SEFLG_HELCTR) || 0 == (iflag & SwissEph.SEFLG_MOSEPH))) {
                if ((retc = main_planet(tjd, SEI_EARTH, epheflag, iflag, ref serr)) != OK) {
//#if 1
      retc = ERR;
      goto return_err;
//#else
//      *serr = '\0';
//#endif
                    //iflag &= ~(SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_HELCTR);
                    ///* on error, we provide barycentric position: */
                    //iflag |= SwissEph.SEFLG_BARYCTR | SwissEph.SEFLG_TRUEPOS | SwissEph.SEFLG_NOGDEFL;
                    //retc = iflag;
                } else {
                    /* iflag (ephemeris bit) may have changed in main_planet() */
                    iflag = swed.pldat[SEI_EARTH].xflgs;
                }
            }
            /************************************
             * observer: geocenter or topocenter
             ************************************/
            /* if topocentric position is wanted  */
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                if (swi_get_observer(pedp.teval, iflag | SwissEph.SEFLG_NONUT, NO_SAVE, xobs, ref serr) != OK)
                    goto return_err;
                /* barycentric position of observer */
                for (i = 0; i <= 5; i++)
                    xobs[i] = xobs[i] + pedp.x[i];
            } else if (0 == (iflag & SwissEph.SEFLG_BARYCTR) && (0 == (iflag & SwissEph.SEFLG_HELCTR) || 0 == (iflag & SwissEph.SEFLG_MOSEPH))) {
                /* barycentric position of geocenter */
                for (i = 0; i <= 5; i++)
                    xobs[i] = pedp.x[i];
            }
            /************************************
             * position and speed at tjd        *
             ************************************/
            if (epoch == 1950)
                t = (tjd - B1950);	/* days since 1950.0 */
            else /* epoch == 2000 */
                t = (tjd - J2000);	/* days since 2000.0 */
            /* for parallax */
            if ((iflag & SwissEph.SEFLG_HELCTR) != 0 && (iflag & SwissEph.SEFLG_MOSEPH) != 0)
                xpo = null;		/* no parallax, if moshier and heliocentric */
            else if ((iflag & SwissEph.SEFLG_HELCTR) != 0)
                xpo = psdp.x;
            else if ((iflag & SwissEph.SEFLG_BARYCTR) != 0)
                xpo = null;		/* no parallax, if barycentric */
            else
                xpo = xobs;
            if (xpo == null) {
                for (i = 0; i <= 2; i++)
                    x[i] += t * x[i + 3];
            } else {
                for (i = 0; i <= 2; i++) {
                    x[i] += t * x[i + 3] - parall * xpo[i];
                    x[i + 3] -= parall * xpo[i + 3];
                }
            }
            /************************************
             * relativistic deflection of light *
             ************************************/
            for (i = 0; i <= 5; i++)
                x[i] *= 10000;	/* great distance, to allow 
             * algorithm used with planets */
            if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0 && (iflag & SwissEph.SEFLG_NOGDEFL) == 0) {
                swi_deflect_light(x, 0, iflag & SwissEph.SEFLG_SPEED);
            }
            /**********************************
             * 'annual' aberration of light   *
             * speed is incorrect !!!         *
             **********************************/
            if ((iflag & SwissEph.SEFLG_TRUEPOS) == 0 && (iflag & SwissEph.SEFLG_NOABERR) == 0)
                swi_aberr_light(x, xpo, iflag & SwissEph.SEFLG_SPEED);
            /* ICRS to J2000 */
            if (0 == (iflag & SwissEph.SEFLG_ICRS) && (get_denum(SEI_SUN, iflag) >= 403 || (iflag & SwissEph.SEFLG_BARYCTR) != 0)) {
                SE.SwephLib.swi_bias(x, tjd, iflag, false);
            }/**/
            /* save J2000 coordinates; required for sidereal positions */
            for (i = 0; i <= 5; i++)
                xxsv[i] = x[i];
            /************************************************
             * precession, equator 2000 -> equator of date *
             ************************************************/
            /*x[0] = -0.374018403; x[1] = -0.312548592; x[2] = -0.873168719;*/
            if ((iflag & SwissEph.SEFLG_J2000) == 0) {
                SE.SwephLib.swi_precess(x, tjd, iflag, J2000_TO_J);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    swi_precess_speed(x, tjd, iflag, J2000_TO_J);
                oe = swed.oec;
            } else
                oe = swed.oec2000;
            /************************************************
             * nutation                                     *
             ************************************************/
            if (0 == (iflag & SwissEph.SEFLG_NONUT))
                swi_nutate(x, 0, false);
            //if (false) {
            //    double r = Math.Sqrt(x[0] * x[0] + x[1] * x[1] + x[2] * x[2]);
            //    printf("%.17f %.17f %f\n", x[0] / r, x[1] / r, x[2] / r);
            //}
            /************************************************
             * unit vector (distance = 1)                   *
             ************************************************/
            u = Math.Sqrt(square_sum(x));
            for (i = 0; i <= 5; i++)
                x[i] /= u;
            u = Math.Sqrt(square_sum(xxsv));
            for (i = 0; i <= 5; i++)
                xxsv[i] /= u;
            /************************************************
             * set speed = 0, because not correct (aberration) 
             ************************************************/
            for (i = 3; i <= 5; i++)
                x[i] = xxsv[i] = 0;
            /************************************************
             * transformation to ecliptic.                  *
             * with sidereal calc. this will be overwritten *
             * afterwards.                                  *
             ************************************************/
            if ((iflag & SwissEph.SEFLG_EQUATORIAL) == 0) {
                SE.SwephLib.swi_coortrf2(x, x, oe.seps, oe.ceps);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(x.GetPointer(3), x.GetPointer(3), oe.seps, oe.ceps);
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(x, x, swed.nut.snut, swed.nut.cnut);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.SwephLib.swi_coortrf2(x.GetPointer(3), x.GetPointer(3), swed.nut.snut, swed.nut.cnut);
                }
            }
            /************************************
             * sidereal positions               *
             ************************************/
            if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                /* rigorous algorithm */
                if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0) {
                    if (swi_trop_ra2sid_lon(xxsv, x, xxsv, iflag) != OK)
                        goto return_err;
                    if ((iflag & SwissEph.SEFLG_EQUATORIAL) != 0)
                        for (i = 0; i <= 5; i++)
                            x[i] = xxsv[i];
                    /* project onto solar system equator */
                } else if ((swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                    if (swi_trop_ra2sid_lon_sosy(xxsv, x, iflag) != OK)
                        return ERR;
                    if ((iflag & SwissEph.SEFLG_EQUATORIAL) != 0)
                        for (i = 0; i <= 5; i++)
                            x[i] = xxsv[i];
                    /* traditional algorithm */
                } else {
                    SE.SwephLib.swi_cartpol_sp(x, x);
                    if (swe_get_ayanamsa_ex(tjd, iflag, out daya, ref serr) == ERR)
                        return ERR;
                    x[0] -= daya * SwissEph.DEGTORAD;
                    SE.SwephLib.swi_polcart_sp(x, x);
                }
            }
            /************************************************
             * transformation to polar coordinates          *
             ************************************************/
            if ((iflag & SwissEph.SEFLG_XYZ) == 0)
                SE.SwephLib.swi_cartpol_sp(x, x);
            /********************** 
             * radians to degrees *
             **********************/
            if ((iflag & SwissEph.SEFLG_RADIANS) == 0 && (iflag & SwissEph.SEFLG_XYZ) == 0) {
                for (i = 0; i < 2; i++) {
                    x[i] *= SwissEph.RADTODEG;
                    x[i + 3] *= SwissEph.RADTODEG;
                }
            }
            for (i = 0; i <= 5; i++)
                xx[i] = x[i];
            /* if no ephemeris has been specified, do not return chosen ephemeris */
            if ((iflgsave & SwissEph.SEFLG_EPHMASK) == 0)
                iflag = iflag & ~SwissEph.SEFLG_DEFAULTEPH;
            iflag = iflag & ~SwissEph.SEFLG_SPEED;
#if TRACE
            trace_swe_fixstar(2, star, tjd, iflag, xx, serr);
#endif
            return iflag;
        return_err:
            for (i = 0; i <= 5; i++)
                xx[i] = 0;
#if TRACE
            trace_swe_fixstar(2, star, tjd, iflag, xx, serr);
#endif
            return retc;
        }

        public Int32 swe_fixstar_ut(string star, double tjd_ut, Int32 iflag,
          double[] xx, ref string serr) {
              double deltat;
              Int32 retflag;
              string sdummy = null;
              Int32 epheflag = 0;
              iflag = plaus_iflag(iflag, -1, tjd_ut, out serr);
              epheflag = iflag & SwissEph.SEFLG_EPHMASK;
              if (epheflag == 0)
              {
                  epheflag = SwissEph.SEFLG_SWIEPH;
                  iflag |= SwissEph.SEFLG_SWIEPH;
              }
              deltat = SE.swe_deltat_ex(tjd_ut, iflag, ref serr);
              /* if ephe required is not ephe returned, adjust delta t: */
              retflag = swe_fixstar(star, tjd_ut + deltat, iflag, xx, ref serr);
              if ((retflag & SwissEph.SEFLG_EPHMASK) != epheflag)
              {
                  deltat = SE.swe_deltat_ex(tjd_ut, retflag, ref sdummy);
                  retflag = swe_fixstar(star, tjd_ut + deltat, iflag, xx, ref sdummy);
              }
              return retflag;
          }

        /**********************************************************
         * get fixstar magnitude
         * parameters:
         * star 	name of star or line number in star file 
         *		(start from 1, don't count comment).
         *    		If no error occurs, the name of the star is returned
         *	        in the format trad_name, nomeclat_name
         *
         * mag 		pointer to a double, for star magnitude
         * serr		error return string
        **********************************************************/
        public Int32 swe_fixstar_mag(ref string star, ref double mag, ref string serr) {
            int i;
            int star_nr = 0;
            bool isnomclat = false;
            int cmplen;
            string[] cpos = new string[20];
            string sstar; string sdummy = null;
            string fstar;
            string s = String.Empty/*, sp, sp2*/;	/* 20 byte for SE_STARFILE */
            int line = 0;
            int fline = 0;
            int retc; int spi;
            serr = String.Empty;
            /******************************************************
             * Star file
             * close to the beginning, a few stars selected by Astrodienst.
             * These can be accessed by giving their number instead of a name.
             * All other stars can be accessed by name.
             * Comment lines start with # and are ignored.
             ******************************************************/
            if (swed.fixfp == null) {
                if ((swed.fixfp = swi_fopen(SEI_FILE_FIXSTAR, SwissEph.SE_STARFILE, swed.ephepath, ref serr)) == null) {
                    swed.is_old_starfile = true;
                    if ((swed.fixfp = swi_fopen(SEI_FILE_FIXSTAR, SwissEph.SE_STARFILE_OLD, swed.ephepath, ref sdummy)) == null) {
                        retc = ERR;
                        goto return_err;
                    }
                }
            }
            swed.fixfp.Seek(0, SeekOrigin.Begin);
            sstar = star;
            if (sstar.StartsWith(",")) {
                isnomclat = true;
            } else if (sstar.Length > 1 && Char.IsDigit(sstar[0])) {
                star_nr = int.Parse(sstar);
            } else {
                /* traditional name of star to lower case */
                sstar = sstar.ToLower();
                spi = sstar.IndexOf(',');
                if (spi >= 0)
                    sstar = sstar.Substring(0, spi);
            }
            sstar = sstar.TrimEnd();
            cmplen = sstar.Length;
            if (cmplen == 0) {
                serr = C.sprintf("swe_fixstar_mag(): star name empty");
                retc = ERR;
                goto return_err;
            }
            while ((s = swed.fixfp.ReadLine()) != null) {
                fline++;
                if (s.StartsWith("#")) continue;
                line++;
                if (star_nr == line)
                    goto found;
                else if (star_nr > 0)
                    continue;
                if ((spi = s.IndexOf(",")) == 0) {
                    serr = C.sprintf("star file %s damaged at line %d", SwissEph.SE_STARFILE, fline);
                    retc = ERR;
                    goto return_err;
                }
                if (isnomclat) {
                    if (s.StartsWith(sstar))
                        goto found;
                    else
                        continue;
                }
                //sp = '\0';	/* cut off first field */
                //strncpy(fstar, s, SE_MAX_STNAME);
                //*sp = ',';
                //fstar[SE_MAX_STNAME] = '\0';	/* force termination */
                fstar = s.Substring(0, spi).TrimEnd();
                i = fstar.Length;
                if (i < (int)cmplen)
                    continue;
                //for (sp2 = fstar; *sp2 != '\0'; sp2++) {
                //    *sp2 = tolower((int)*sp2);
                //}
                fstar = fstar.ToLower();
                if (fstar.StartsWith(sstar))
                    goto found;
            }
            serr = C.sprintf("star %s not found", star);
            retc = ERR;
            goto return_err;
        found:
            cpos = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            i = cpos.Length;
            cpos[0] = cpos[0].TrimEnd();
            cpos[1] = cpos[1].TrimEnd();
            if (i < 13) {
                serr = C.sprintf("data of star '%s,%s' incomplete", cpos[0], cpos[1]);
                retc = ERR;
                goto return_err;
            }
            mag = double.Parse(cpos[13], CultureInfo.InvariantCulture);
            /* return trad. name, nomeclature name */
            star = String.Format("{0},{1}", cpos[0], cpos[1]);
            return OK;
        return_err:
            mag = 0;
            return retc;
        }

        //#if 0
        //int swe_fixstar(char *star, double tjd, int32 iflag, double *xx, char *serr)
        //{
        //  int i, j;
        //  int32 iflgcoor = SEFLG_EQUATORIAL | SEFLG_XYZ | SEFLG_RADIANS;
        //  int retc;
        //  double *xs, x0[6], x2[6];
        //  double dt;
        //  /* only one speed flag */
        //  if (iflag & SEFLG_SPEED3)
        //    iflag |= SEFLG_SPEED;
        //  /* cartesian flag excludes radians flag */
        //  if ((iflag & SEFLG_XYZ) && (iflag & SEFLG_RADIANS))
        //    iflag = iflag & ~SEFLG_RADIANS;
        //  if ((iflag & SEFLG_SPEED) == 0) {
        //    /* without speed: */
        //    if ((retc = swecalc(tjd, ipl, iflag, xx, serr)) == ERR) 
        //      goto return_error;
        //  } else {
        //    /* with speed from three calls of fixstar() */
        //    dt = PLAN_SPEED_INTV;
        //    if ((retc = fixstar(star, tjd-dt, iflag, x0, serr)) == ERR)
        //      goto return_error; 
        //    if ((retc = fixstar(star, tjd+dt, iflag, x2, serr)) == ERR)
        //      goto return_error; 
        //    if ((retc = fixstar(star, tjd, iflag, xx, serr)) == ERR)
        //      goto return_error; 
        //    denormalize_positions(x0, xx, x2); /* nonsense !!!!!!!!!!! */
        //    calc_speed(x0, xx, x2, dt);
        //  }
        //  return retc;
        //  return_error:
        //  for (i = 0; i < 6; i++)
        //    xx[i] = 0;
        //  return ERR; 
        //}

        //#endif


        public string swe_get_planet_name(int ipl) {
            //int i;
            //Int32 retc;
            double[] xp = new double[6];
            string s = String.Empty, sdummy = String.Empty;
#if TRACE
            //  swi_open_trace(NULL);
            trace_swe_get_planet_name(1, ipl, s);
#endif
            swi_init_swed_if_start();
            /* function calls for Pluto with asteroid number 134340
               * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            if (ipl != 0 && ipl == swed.i_saved_planet_name) {
                return swed.saved_planet_name;
            }
            switch (ipl) {
                case SwissEph.SE_SUN:
                    s = SE_NAME_SUN;
                    break;
                case SwissEph.SE_MOON:
                    s = SE_NAME_MOON;
                    break;
                case SwissEph.SE_MERCURY:
                    s = SE_NAME_MERCURY;
                    break;
                case SwissEph.SE_VENUS:
                    s = SE_NAME_VENUS;
                    break;
                case SwissEph.SE_MARS:
                    s = SE_NAME_MARS;
                    break;
                case SwissEph.SE_JUPITER:
                    s = SE_NAME_JUPITER;
                    break;
                case SwissEph.SE_SATURN:
                    s = SE_NAME_SATURN;
                    break;
                case SwissEph.SE_URANUS:
                    s = SE_NAME_URANUS;
                    break;
                case SwissEph.SE_NEPTUNE:
                    s = SE_NAME_NEPTUNE;
                    break;
                case SwissEph.SE_PLUTO:
                    s = SE_NAME_PLUTO;
                    break;
                case SwissEph.SE_MEAN_NODE:
                    s = SE_NAME_MEAN_NODE;
                    break;
                case SwissEph.SE_TRUE_NODE:
                    s = SE_NAME_TRUE_NODE;
                    break;
                case SwissEph.SE_MEAN_APOG:
                    s = SE_NAME_MEAN_APOG;
                    break;
                case SwissEph.SE_OSCU_APOG:
                    s = SE_NAME_OSCU_APOG;
                    break;
                case SwissEph.SE_INTP_APOG:
                    s = SE_NAME_INTP_APOG;
                    break;
                case SwissEph.SE_INTP_PERG:
                    s = SE_NAME_INTP_PERG;
                    break;
                case SwissEph.SE_EARTH:
                    s = SE_NAME_EARTH;
                    break;
                case SwissEph.SE_CHIRON:
                case SwissEph.SE_AST_OFFSET + MPC_CHIRON:
                    s = SE_NAME_CHIRON;
                    break;
                case SwissEph.SE_PHOLUS:
                case SwissEph.SE_AST_OFFSET + MPC_PHOLUS:
                    s = SE_NAME_PHOLUS;
                    break;
                case SwissEph.SE_CERES:
                case SwissEph.SE_AST_OFFSET + MPC_CERES:
                    s = SE_NAME_CERES;
                    break;
                case SwissEph.SE_PALLAS:
                case SwissEph.SE_AST_OFFSET + MPC_PALLAS:
                    s = SE_NAME_PALLAS;
                    break;
                case SwissEph.SE_JUNO:
                case SwissEph.SE_AST_OFFSET + MPC_JUNO:
                    s = SE_NAME_JUNO;
                    break;
                case SwissEph.SE_VESTA:
                case SwissEph.SE_AST_OFFSET + MPC_VESTA:
                    s = SE_NAME_VESTA;
                    break;
                default:
                    /* fictitious planets */
                    if (ipl >= SwissEph.SE_FICT_OFFSET && ipl <= SwissEph.SE_FICT_MAX) {
                        s = SE.SwemPlan.swi_get_fict_name(ipl - SwissEph.SE_FICT_OFFSET);
                        break;
                    }
                    /* asteroids */
                    if (ipl > SwissEph.SE_AST_OFFSET) {
                        /* if name is already available */
                        if (ipl == swed.fidat[SEI_FILE_ANY_AST].ipl[0])
                            s = swed.fidat[SEI_FILE_ANY_AST].astnam;
                        /* else try to get it from ephemeris file */
                        else {
                            var retc = sweph(J2000, ipl, SEI_FILE_ANY_AST, 0, null, NO_SAVE, xp, ref sdummy);
                            if (retc != ERR && retc != NOT_AVAILABLE)
                                s = swed.fidat[SEI_FILE_ANY_AST].astnam;
                            else
                                s = C.sprintf("%d: not found", ipl - SwissEph.SE_AST_OFFSET);
                        }
                        /* If there is a provisional designation only in ephemeris file,
                         * we look for a name in seasnam.txt, which can be updated by
                         * the user.
                         * Some old ephemeris files return a '?' in the first position.
                         * There are still a couple of unnamed bodies that got their
                         * provisional designation before 1925, when the current method
                         * of provisional designations was introduced. They have an 'A'
                         * as the first character, e.g. A924 RC. 
                         * The file seasnam.txt may contain comments starting with '#'.
                         * There must be at least two columns: 
                         * 1. asteroid catalog number
                         * 2. asteroid name
                         * The asteroid number may or may not be in brackets
                         */
                        if (s[0] == '?' || Char.IsDigit(s[1])) {
                            int ipli = (int)(ipl - SwissEph.SE_AST_OFFSET), iplf = 0;
                            CFile fp;
                            String sp;
                            //char si[AS_MAXCH], *sp, *sp2;
                            if ((fp = swi_fopen(-1, SwissEph.SE_ASTNAMFILE, swed.ephepath, ref sdummy)) != null) {
                                while (ipli != iplf && ((sp = fp.ReadLine()) != null)) {
                                    sp = sp.TrimStart(' ', '\t', '(', '[', '{');
                                    if (String.IsNullOrWhiteSpace(sp) || sp.StartsWith("#"))
                                        continue;
                                    /* catalog number of body of current line */
                                    int spi = sp.IndexOfFirstNot('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
                                    if (spi < 0) continue;
                                    iplf = int.Parse(sp.Substring(0, spi));
                                    if (ipli != iplf)
                                        continue;
                                    sp = sp.Substring(spi);
                                    /* set pointer after catalog number */
                                    spi = sp.IndexOfAny(new char[] { ' ', '\t' });
                                    if (spi < 0) continue;
                                    s = sp.Substring(spi).TrimStart(' ', '\t');
                                }
                                fp.Dispose();
                            }
                        }
                    } else {
                        s = C.sprintf("%d", ipl);
                    }
                    break;
            }
#if TRACE
            //  swi_open_trace(NULL);
            trace_swe_get_planet_name(2, ipl, s);
#endif
            //if (strlen(s) < 80) {
            swed.i_saved_planet_name = ipl;
            swed.saved_planet_name = s;
            //}
            return s;
        }

        public string swe_get_ayanamsa_name(Int32 isidmode) {
            isidmode %= SwissEph.SE_SIDBITS;
            if (isidmode < SwissEph.SE_NSIDM_PREDEF)
                return ayanamsa_name[isidmode];
            return null;
        }

#if TRACE
        void trace_swe_calc(int swtch, double tjd, int ipl, Int32 iflag, CPointer<double> xx, string serr) {
            //  if (swi_trace_count >= TRACE_COUNT_MAX)
            //    return;
            switch (swtch) {
                case 1:
                    //trace(true, "\n/*SWE_CALC*/");
                    //trace(true, "  tjd = %.9f; ipl = %d; iflag = %d;", tjd, ipl, iflag);
                    //trace(true, "  iflgret = swe_calc(tjd, ipl, iflag, xx, serr);");
                    break;
                case 2:
                    //trace(true, "  printf(\"swe_calc: %f\\t%d\\t%d\\t%f\\t%f\\t%f\\t%f\\t%f\\t%f\\t\", "); 
                    //trace(true, "\ttjd, ipl, iflgret, xx[0], xx[1], xx[2], xx[3], xx[4], xx[5]);");
                    //trace(true, "  if (*serr != '\\0') printf(serr); printf(\"\\n\");");
                    trace("swe_calc: %f\t%d\t%d\t%f\t%f\t%f\t%f\t%f\t%f\t%s", tjd, ipl, iflag, xx[0], xx[1], xx[2], xx[3], xx[4], xx[5], serr);
                    break;
                //    default:
                //      break;
            }
        }

        void trace_swe_fixstar(int swtch, string star, double tjd, Int32 iflag, CPointer<double> xx, string serr) {
            //if (swi_trace_count >= TRACE_COUNT_MAX)
            //    return;
            switch (swtch) {
                case 1:
                    //if (swi_fp_trace_c != NULL) {
                    //    fputs("\n/*SWE_FIXSTAR*/\n", swi_fp_trace_c);
                    //    fprintf(swi_fp_trace_c, "  strcpy(star, \"%s\");", star);
                    //    fprintf(swi_fp_trace_c, " tjd = %.9f;", tjd);
                    //    fprintf(swi_fp_trace_c, " iflag = %d;\n", iflag);
                    //    fprintf(swi_fp_trace_c, "  iflgret = swe_fixstar(star, tjd, iflag, xx, serr);");
                    //    fprintf(swi_fp_trace_c, "   /* xx = %d */\n", (int32)xx);
                    //    fflush(swi_fp_trace_c);
                    //}
                    break;
                case 2:
                    //if (swi_fp_trace_c != NULL) {
                    //    fputs("  printf(\"swe_fixstar: %s\\t%f\\t%d\\t%f\\t%f\\t%f\\t%f\\t%f\\t%f\\t\", ", swi_fp_trace_c);
                    //    fputs("\n\tstar, tjd, iflgret, xx[0], xx[1], xx[2], xx[3], xx[4], xx[5]);\n", swi_fp_trace_c);/**/
                    //    fputs("  if (*serr != '\\0')", swi_fp_trace_c);
                    //    fputs(" printf(serr);", swi_fp_trace_c);
                    //    fputs(" printf(\"\\n\");\n", swi_fp_trace_c);
                    //    fflush(swi_fp_trace_c);
                    //}
                    //if (swi_fp_trace_out != NULL) {
                    trace("swe_fixstar: %s\t%f\t%d\t%f\t%f\t%f\t%f\t%f\t%f\t%s",
                          star, tjd, iflag, xx[0], xx[1], xx[2], xx[3], xx[4], xx[5], serr);
                    //if (serr != NULL && *serr != '\0') {
                    //    fputs(serr, swi_fp_trace_out);
                    //}
                    //fputs("\n", swi_fp_trace_out);
                    //fflush(swi_fp_trace_out);
                    //}
                    break;
                default:
                    break;
            }
        }

        void trace_swe_get_planet_name(int swtch, int ipl, string s) {
            //if (swi_trace_count >= TRACE_COUNT_MAX)
            //    return;
            switch (swtch) {
                case 1:
                    //if (swi_fp_trace_c != NULL) {
                    //trace(true, "\n/*SWE_GET_PLANET_NAME*/");
                    //trace(true, "  ipl = %d;", ipl);
                    //trace(true, "  swe_get_planet_name(ipl, s);   /* s = %s */", s);
                    //    fflush(swi_fp_trace_c);
                    //}
                    break;
                case 2:
                    //if (swi_fp_trace_c != NULL) {
                    //trace(true, "  printf(\"swe_get_planet_name: %d\\t%s\\t\\n\", ipl, s);", ipl, s);/**/
                    //    fflush(swi_fp_trace_c);
                    //}
                    //if (swi_fp_trace_out != NULL) {
                    trace("swe_get_planet_name: %d\t%s\t", ipl, s);
                    //    fflush(swi_fp_trace_out);
                    //}
                    break;
                //default:
                //    break;
            }
        }

#endif

        /* set geographic position and altitude of observer */
        public void swe_set_topo(double geolon, double geolat, double geoalt) {
            swi_init_swed_if_start();
            swed.topd.geolon = geolon;
            swed.topd.geolat = geolat;
            swed.topd.geoalt = geoalt;
            swed.geopos_is_set = true;
            /* to force new calculation of observer position vector */
            swed.topd.teval = 0;
            /* to force new calculation of light-time etc. 
             */
            swi_force_app_pos_etc();
        }

        public void swi_force_app_pos_etc() {
            int i;
            for (i = 0; i < SEI_NPLANETS; i++)
                swed.pldat[i].xflgs = -1;
            for (i = 0; i < SEI_NNODE_ETC; i++)
                swed.nddat[i].xflgs = -1;
            for (i = 0; i < SwissEph.SE_NPLANETS; i++) {
                swed.savedat[i].tsave = 0;
                swed.savedat[i].iflgsave = -1;
            }
        }

        public int swi_get_observer(double tjd, Int32 iflag,
            bool do_save, CPointer<double> xobs, ref string serr) {
            int i;
            double sidt, delt, tjd_ut, eps, nut; double[] nutlo = new double[2];
            double f = EARTH_OBLATENESS;
            double re = EARTH_RADIUS;
            double cosfi, sinfi, cc, ss, cosl, sinl, h;
            if (!swed.geopos_is_set) {
                serr = "geographic position has not been set";
                return ERR;
            }
            /* geocentric position of observer depends on sidereal time,
             * which depends on UT. 
             * compute UT from ET. this UT will be slightly different
             * from the user's UT, but this difference is extremely small.
             */
            delt = SE.swe_deltat_ex(tjd, iflag, ref serr);
            tjd_ut = tjd - delt;
            if (swed.oec.teps == tjd && swed.nut.tnut == tjd) {
                eps = swed.oec.eps;
                nutlo[1] = swed.nut.nutlo[1];
                nutlo[0] = swed.nut.nutlo[0];
            } else {
                eps = SE.SwephLib.swi_epsiln(tjd, iflag);
                if (0 == (iflag & SwissEph.SEFLG_NONUT))
                    SE.SwephLib.swi_nutation(tjd, iflag, nutlo);
            }
            if ((iflag & SwissEph.SEFLG_NONUT) != 0) {
                nut = 0;
            } else {
                eps += nutlo[1];
                nut = nutlo[0];
            }
            /* mean or apparent sidereal time, depending on whether or
             * not SEFLG_NONUT is set */
            sidt = SE.swe_sidtime0(tjd_ut, eps * SwissEph.RADTODEG, nut * SwissEph.RADTODEG);
            sidt *= 15;	/* in degrees */
            /* length of position and speed vectors;
             * the height above sea level must be taken into account.
             * with the moon, an altitude of 3000 m makes a difference 
             * of about 2 arc seconds.
             * height is referred to the average sea level. however, 
             * the spheroid (geoid), which is defined by the average 
             * sea level (or rather by all points of same gravitational
             * potential), is of irregular shape and cannot easily
             * be taken into account. therefore, we refer height to 
             * the surface of the ellipsoid. the resulting error 
             * is below 500 m, i.e. 0.2 - 0.3 arc seconds with the moon.
             */
            cosfi = Math.Cos(swed.topd.geolat * SwissEph.DEGTORAD);
            sinfi = Math.Sin(swed.topd.geolat * SwissEph.DEGTORAD);
            cc = 1 / Math.Sqrt(cosfi * cosfi + (1 - f) * (1 - f) * sinfi * sinfi);
            ss = (1 - f) * (1 - f) * cc;
            /* neglect polar motion (displacement of a few meters), as long as 
             * we use the earth ellipsoid */
            /* ... */
            /* add sidereal time */
            cosl = Math.Cos((swed.topd.geolon + sidt) * SwissEph.DEGTORAD);
            sinl = Math.Sin((swed.topd.geolon + sidt) * SwissEph.DEGTORAD);
            h = swed.topd.geoalt;
            xobs[0] = (re * cc + h) * cosfi * cosl;
            xobs[1] = (re * cc + h) * cosfi * sinl;
            xobs[2] = (re * ss + h) * sinfi;
            /* polar coordinates */
            SE.SwephLib.swi_cartpol(xobs, xobs);
            /* speed */
            xobs[3] = EARTH_ROT_SPEED;
            xobs[4] = xobs[5] = 0;
            SE.SwephLib.swi_polcart_sp(xobs, xobs);
            /* to AUNIT */
            for (i = 0; i <= 5; i++)
                xobs[i] /= AUNIT;
            /* subtract nutation, set backward flag */
            if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                SE.SwephLib.swi_coortrf2(xobs, xobs, -swed.nut.snut, swed.nut.cnut);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(xobs + 3, xobs + 3, -swed.nut.snut, swed.nut.cnut);
                swi_nutate(xobs, iflag, true);
            }
            /* precess to J2000 */
            SE.SwephLib.swi_precess(xobs, tjd, iflag, J_TO_J2000);
            if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                swi_precess_speed(xobs, tjd, iflag, J_TO_J2000);
            /* neglect frame bias (displacement of 45cm) */
            /* ... */
            /* save */
            if (do_save) {
                for (i = 0; i <= 5; i++)
                    swed.topd.xobs[i] = xobs[i];
                swed.topd.teval = tjd;
                swed.topd.tjd_ut = tjd_ut;	/* -> save area */
            }
            return OK;
        }

        /* Equation of Time
         *
         * The function returns the difference between 
         * local apparent and local mean time in days.
         * E = LAT - LMT
         * Input variable tjd is UT.
         */
        public Int32 swe_time_equ(double tjd_ut, out double E, ref string serr) {
            Int32 retval; E = 0;
            double t, dt; double[] x = new double[6];
            double sidt = SE.swe_sidtime(tjd_ut);
            Int32 iflag = SwissEph.SEFLG_EQUATORIAL;
            iflag = plaus_iflag(iflag, -1, tjd_ut, out serr);
            if (swi_init_swed_if_start() == 1 && 0 == (iflag & SwissEph.SEFLG_MOSEPH) )
            {
                serr = "Please call swe_set_ephe_path() or swe_set_jplfile() before calling swe_time_equ(), swe_lmt_to_lat() or swe_lat_to_lmt()";
            }
            if (swed.jpl_file_is_open)
                iflag |= SwissEph.SEFLG_JPLEPH;
            t = tjd_ut + 0.5;
            dt = t - Math.Floor(t);
            sidt -= dt * 24;
            sidt *= 15;
            if ((retval = swe_calc_ut(tjd_ut, SwissEph.SE_SUN, iflag, x, ref serr)) == ERR) {
                E = 0;
                return ERR;
            }
            dt = SE.swe_degnorm(sidt - x[0] - 180);
            if (dt > 180)
                dt -= 360;
            dt *= 4;
            E = dt / 1440.0;
            return OK;
        }

        public Int32 swe_lmt_to_lat(double tjd_lmt, double geolon, out double tjd_lat, ref string serr) {
            Int32 retval; serr = null;
            double E, tjd_lmt0;
            tjd_lmt0 = tjd_lmt - geolon / 360.0;
            retval = swe_time_equ(tjd_lmt0, out E, ref serr);
            tjd_lat = tjd_lmt + E;
            return retval;
        }

        public Int32 swe_lat_to_lmt(double tjd_lat, double geolon, out double tjd_lmt, ref string serr) {
            Int32 retval; serr = null;
            double E, tjd_lmt0;
            tjd_lmt0 = tjd_lat - geolon / 360.0;
            retval = swe_time_equ(tjd_lmt0, out E, ref serr);
            /* iteration */
            retval = swe_time_equ(tjd_lmt0 - E, out E, ref serr);
            retval = swe_time_equ(tjd_lmt0 - E, out E, ref serr);
            tjd_lmt = tjd_lat - E;
            return retval;
        }

        int open_jpl_file(CPointer<double> ss, string fname, string fpath, ref string serr) {
            int retc;
            string serr2 = String.Empty;
            retc = SE.SweJPL.swi_open_jpl_file(ss, fname, fpath, ref serr);
            /* If we fail with default JPL ephemeris (DE431), we try the second default
             * (DE406), but only if serr is not NULL and an warning message can be 
             * returned. */
            if (retc != OK && fname.Contains(SwissEph.SE_FNAME_DFT)) {
                retc = SE.SweJPL.swi_open_jpl_file(ss, SwissEph.SE_FNAME_DFT2, fpath, ref serr2);
                if (retc == OK) {
                    swed.jplfnam = SwissEph.SE_FNAME_DFT2;
                    serr2 = "Error with JPL ephemeris file ";
                    serr2 += SwissEph.SE_FNAME_DFT;
                    serr2 += C.sprintf(": %s", serr);
                    serr2 += ". Defaulting to ";
                    serr2 += SwissEph.SE_FNAME_DFT2;
                    serr = serr2;
                }
            }
            if (retc == OK) {
                swed.jpldenum = (short)SE.SweJPL.swi_get_jpl_denum();
                swed.jpl_file_is_open = true;
                SE.SwephLib.swi_set_tid_acc(0, 0, swed.jpldenum, ref serr);
            }
            return retc;
        }


        //#if 0
        //void FAR PASCAL_CONV swe_set_timeout(int32 tsec)
        //{
        //  if (tsec < 0) tsec = 0;
        //  swed.timeout = tsec;
        //}
        //#endif

        //#if 0
        //int FAR PASCAL_CONV swe_time_equ(double tjd_ut, double *E, char *serr)
        // /* Algorithm according to Meeus, German, p. 190ff.*/
        //  double L0, dpsi, eps, x[6], nutlo[2];
        //  double tau = (tjd - J2000) / 365250;
        //  double tau2 = tau * tau;
        //  double tau3 = tau * tau2;
        //  double tau4 = tau * tau3;
        //  double tau5 = tau * tau4;
        //  L0 = 280.4664567 + swe_degnorm(tau * 360007.6982779)
        //           + tau2 * 0.03032028 
        //           + tau3 * 1 / 49931
        //           - tau4 * 1 / 15299
        //           - tau5 * 1 / 1988000;
        //  swi_nutation(tjd, 0, nutlo);
        //  eps = (swi_epsiln(tjd) + nutlo[1]) * RADTODEG;
        //  dpsi = nutlo[0] * RADTODEG;
        //  if (swe_calc(tjd, SE_SUN, SEFLG_EQUATORIAL, x, serr) == ERR)
        //    return ERR;
        //  *E = swe_degnorm(L0 - 0.0057183 - x[0] + dpsi * Math.Cos(eps * DEGTORAD));
        //  if (*E > 180)
        //    *E -= 360;
        //  *E *= 4 / 1440.0;
        //  return OK;
        //}
        //#endif
    }
}
