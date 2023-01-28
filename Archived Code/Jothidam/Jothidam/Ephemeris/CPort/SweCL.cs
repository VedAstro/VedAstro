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
   $Header: /home/dieter/sweph/RCS/swecl.c,v 1.75 2008/08/26 07:23:27 dieter Exp $

    Ephemeris computations
    Author: Dieter Koch

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

    class SweCL : BaseCPort
    {
        public SweCL(SwissEph se)
            : base(se) {
        }

        public const int SEFLG_EPHMASK = SwissEph.SEFLG_EPHMASK;
        //static int find_maximum(double y00, double y11, double y2, double dx, 
        //            double *dxret, double *yret);
        //static int find_zero(double y00, double y11, double y2, double dx, 
        //            double *dxret, double *dxret2);
        //static double calc_dip(double geoalt, double atpress, double attemp, double lapse_rate);
        //static double calc_astronomical_refr(double geoalt,double atpress, double attemp);
        double const_lapse_rate = Sweph.SE_LAPSE_RATE;  /* for refraction */

        //#if 0
        //#define DSUN 	(1391978489.9 / AUNIT)	/* this value is consistent with
        //                     * 959.63 arcsec at AU distance (Astr. Alm.) */
        //#else
        const double DSUN = (1392000000.0 / Sweph.AUNIT);
        //#endif
        const double DMOON = (3476300.0 / Sweph.AUNIT);
        const double DEARTH = (6378140.0 * 2 / Sweph.AUNIT);
        const double RSUN = (DSUN / 2);
        const double RMOON = (DMOON / 2);
        const double REARTH = (DEARTH / 2);
        /*#define SEI_OCC_FAST    (16 * 1024L)*/
        //static int32 eclipse_where( double tjd_ut, int32 ipl, char *starname, int32 ifl, double *geopos, 
        //    double *dcore, char *serr);
        //static int32 eclipse_how( double tjd_ut, int32 ipl, char *starname, int32 ifl,
        //        double geolon, double geolat, double geohgt, 
        //    double *attr, char *serr);
        //static int32 eclipse_when_loc(double tjd_start, int32 ifl, double *geopos, 
        //    double *tret, double *attr, AS_BOOL backward, char *serr);
        //static int32 occult_when_loc(double tjd_start, int32 ipl, char *starname, int32 ifl, 
        //        double *geopos, double *tret, double *attr, AS_BOOL backward, char *serr);
        //static int32 lun_eclipse_how(double tjd_ut, int32 ifl, double *attr, 
        //        double *dcore, char *serr);
        //static int32 calc_mer_trans(
        //               double tjd_ut, int32 ipl, int32 epheflag, int32 rsmi,
        //               double *geopos,
        //               char *starname, 
        //               double *tret,
        //               char *serr); 
        //static int32 calc_planet_star(double tjd_et, int32 ipl, char *starname, int32 iflag, double *x, char *serr);

        struct saros_data
        {
            public saros_data(int s, double t) {
                series_no = s; tstart = t;
            }
            public int series_no; public double tstart;
        };

        const double SAROS_CYCLE = 6585.3213;
        const int NSAROS_SOLAR = 181;
        static saros_data[] saros_data_solar = new saros_data[NSAROS_SOLAR]  {
            new saros_data(0, 641886.5), /* 23 May -2955 */
            new saros_data(1, 672214.5), /* 04 Jun -2872 */
            new saros_data(2, 676200.5), /* 04 May -2861 */
            new saros_data(3, 693357.5), /* 24 Apr -2814 */
            new saros_data(4, 723685.5), /* 06 May -2731 */
            new saros_data(5, 727671.5), /* 04 Apr -2720 */
            new saros_data(6, 744829.5), /* 27 Mar -2673 */
            new saros_data(7, 775157.5), /* 08 Apr -2590 */
            new saros_data(8, 779143.5), /* 07 Mar -2579 */
            new saros_data(9, 783131.5), /* 06 Feb -2568 */
            new saros_data(10, 820044.5), /* 28 Feb -2467 */
            new saros_data(11, 810859.5), /* 06 Jan -2492 */
            new saros_data(12, 748993.5), /* 20 Aug -2662 */
            new saros_data(13, 792492.5), /* 23 Sep -2543 */
            new saros_data(14, 789892.5), /* 11 Aug -2550 */
            new saros_data(15, 787294.5), /* 01 Jul -2557 */
            new saros_data(16, 824207.5), /* 23 Jul -2456 */
            new saros_data(17, 834779.5), /* 03 Jul -2427 */
            new saros_data(18, 838766.5), /* 02 Jun -2416 */
            new saros_data(19, 869094.5), /* 15 Jun -2333 */
            new saros_data(20, 886251.5), /* 05 Jun -2286 */
            new saros_data(21, 890238.5), /* 05 May -2275 */
            new saros_data(22, 927151.5), /* 28 May -2174 */
            new saros_data(23, 937722.5), /* 07 May -2145 */
            new saros_data(24, 941709.5), /* 06 Apr -2134 */
            new saros_data(25, 978623.5), /* 30 Apr -2033 */
            new saros_data(26, 989194.5), /* 08 Apr -2004 */
            new saros_data(27, 993181.5), /* 09 Mar -1993 */
            new saros_data(28, 1023510.5), /* 22 Mar -1910 */
            new saros_data(29, 1034081.5), /* 01 Mar -1881 */
            new saros_data(30, 972214.5), /* 12 Oct -2051 */
            new saros_data(31, 1061811.5), /* 31 Jan -1805 */
            new saros_data(32, 1006529.5), /* 24 Sep -1957 */
            new saros_data(33, 997345.5), /* 02 Aug -1982 */
            new saros_data(34, 1021088.5), /* 04 Aug -1917 */
            new saros_data(35, 1038245.5), /* 25 Jul -1870 */
            new saros_data(36, 1042231.5), /* 23 Jun -1859 */
            new saros_data(37, 1065974.5), /* 25 Jun -1794 */
            new saros_data(38, 1089716.5), /* 26 Jun -1729 */
            new saros_data(39, 1093703.5), /* 26 May -1718 */
            new saros_data(40, 1117446.5), /* 28 May -1653 */
            new saros_data(41, 1141188.5), /* 28 May -1588 */
            new saros_data(42, 1145175.5), /* 28 Apr -1577 */
            new saros_data(43, 1168918.5), /* 29 Apr -1512 */
            new saros_data(44, 1192660.5), /* 30 Apr -1447 */
            new saros_data(45, 1196647.5), /* 30 Mar -1436 */
            new saros_data(46, 1220390.5), /* 01 Apr -1371 */
            new saros_data(47, 1244132.5), /* 02 Apr -1306 */
            new saros_data(48, 1234948.5), /* 08 Feb -1331 */
            new saros_data(49, 1265277.5), /* 22 Feb -1248 */
            new saros_data(50, 1282433.5), /* 11 Feb -1201 */
            new saros_data(51, 1207395.5), /* 02 Sep -1407 */
            new saros_data(52, 1217968.5), /* 14 Aug -1378 */
            new saros_data(53, 1254881.5), /* 06 Sep -1277 */
            new saros_data(54, 1252282.5), /* 25 Jul -1284 */
            new saros_data(55, 1262855.5), /* 06 Jul -1255 */
            new saros_data(56, 1293182.5), /* 17 Jul -1172 */
            new saros_data(57, 1297169.5), /* 17 Jun -1161 */
            new saros_data(58, 1314326.5), /* 07 Jun -1114 */
            new saros_data(59, 1344654.5), /* 19 Jun -1031 */
            new saros_data(60, 1348640.5), /* 18 May -1020 */
            new saros_data(61, 1365798.5), /* 10 May -0973 */
            new saros_data(62, 1396126.5), /* 22 May -0890 */
            new saros_data(63, 1400112.5), /* 20 Apr -0879 */
            new saros_data(64, 1417270.5), /* 11 Apr -0832 */
            new saros_data(65, 1447598.5), /* 24 Apr -0749 */
            new saros_data(66, 1444999.5), /* 12 Mar -0756 */
            new saros_data(67, 1462157.5), /* 04 Mar -0709 */
            new saros_data(68, 1492485.5), /* 16 Mar -0626 */
            new saros_data(69, 1456959.5), /* 09 Dec -0724 */
            new saros_data(70, 1421434.5), /* 05 Sep -0821 */
            new saros_data(71, 1471518.5), /* 19 Oct -0684 */
            new saros_data(72, 1455748.5), /* 16 Aug -0727 */
            new saros_data(73, 1466320.5), /* 27 Jul -0698 */
            new saros_data(74, 1496648.5), /* 08 Aug -0615 */
            new saros_data(75, 1500634.5), /* 07 Jul -0604 */
            new saros_data(76, 1511207.5), /* 18 Jun -0575 */
            new saros_data(77, 1548120.5), /* 11 Jul -0474 */
            new saros_data(78, 1552106.5), /* 09 Jun -0463 */
            new saros_data(79, 1562679.5), /* 21 May -0434 */
            new saros_data(80, 1599592.5), /* 13 Jun -0333 */
            new saros_data(81, 1603578.5), /* 12 May -0322 */
            new saros_data(82, 1614150.5), /* 22 Apr -0293 */
            new saros_data(83, 1644479.5), /* 05 May -0210 */
            new saros_data(84, 1655050.5), /* 14 Apr -0181 */
            new saros_data(85, 1659037.5), /* 14 Mar -0170 */
            new saros_data(86, 1695950.5), /* 06 Apr -0069 */
            new saros_data(87, 1693351.5), /* 23 Feb -0076 */
            new saros_data(88, 1631484.5), /* 06 Oct -0246 */
            new saros_data(89, 1727666.5), /* 04 Feb 0018 */
            new saros_data(90, 1672384.5), /* 28 Sep -0134 */
            new saros_data(91, 1663200.5), /* 06 Aug -0159 */
            new saros_data(92, 1693529.5), /* 19 Aug -0076 */
            new saros_data(93, 1710685.5), /* 09 Aug -0029 */
            new saros_data(94, 1714672.5), /* 09 Jul -0018 */
            new saros_data(95, 1738415.5), /* 11 Jul 0047 */
            new saros_data(96, 1755572.5), /* 01 Jul 0094 */
            new saros_data(97, 1766144.5), /* 11 Jun 0123 */
            new saros_data(98, 1789887.5), /* 12 Jun 0188 */
            new saros_data(99, 1807044.5), /* 03 Jun 0235 */
            new saros_data(100, 1817616.5), /* 13 May 0264 */
            new saros_data(101, 1841359.5), /* 15 May 0329 */
            new saros_data(102, 1858516.5), /* 05 May 0376 */
            new saros_data(103, 1862502.5), /* 04 Apr 0387 */
            new saros_data(104, 1892831.5), /* 17 Apr 0470 */
            new saros_data(105, 1903402.5), /* 27 Mar 0499 */
            new saros_data(106, 1887633.5), /* 23 Jan 0456 */
            new saros_data(107, 1924547.5), /* 15 Feb 0557 */
            new saros_data(108, 1921948.5), /* 04 Jan 0550 */
            new saros_data(109, 1873251.5), /* 07 Sep 0416 */
            new saros_data(110, 1890409.5), /* 30 Aug 0463 */
            new saros_data(111, 1914151.5), /* 30 Aug 0528 */
            new saros_data(112, 1918138.5), /* 31 Jul 0539 */
            new saros_data(113, 1935296.5), /* 22 Jul 0586 */
            new saros_data(114, 1959038.5), /* 23 Jul 0651 */
            new saros_data(115, 1963024.5), /* 21 Jun 0662 */
            new saros_data(116, 1986767.5), /* 23 Jun 0727 */
            new saros_data(117, 2010510.5), /* 24 Jun 0792 */
            new saros_data(118, 2014496.5), /* 24 May 0803 */
            new saros_data(119, 2031654.5), /* 15 May 0850 */
            new saros_data(120, 2061982.5), /* 27 May 0933 */
            new saros_data(121, 2065968.5), /* 25 Apr 0944 */
            new saros_data(122, 2083126.5), /* 17 Apr 0991 */
            new saros_data(123, 2113454.5), /* 29 Apr 1074 */
            new saros_data(124, 2104269.5), /* 06 Mar 1049 */
            new saros_data(125, 2108256.5), /* 04 Feb 1060 */
            new saros_data(126, 2151755.5), /* 10 Mar 1179 */
            new saros_data(127, 2083302.5), /* 10 Oct 0991 */
            new saros_data(128, 2080704.5), /* 29 Aug 0984 */
            new saros_data(129, 2124203.5), /* 03 Oct 1103 */
            new saros_data(130, 2121603.5), /* 20 Aug 1096 */
            new saros_data(131, 2132176.5), /* 01 Aug 1125 */
            new saros_data(132, 2162504.5), /* 13 Aug 1208 */
            new saros_data(133, 2166490.5), /* 13 Jul 1219 */
            new saros_data(134, 2177062.5), /* 22 Jun 1248 */
            new saros_data(135, 2207390.5), /* 05 Jul 1331 */
            new saros_data(136, 2217962.5), /* 14 Jun 1360 */
            new saros_data(137, 2228534.5), /* 25 May 1389 */
            new saros_data(138, 2258862.5), /* 06 Jun 1472 */
            new saros_data(139, 2269434.5), /* 17 May 1501 */
            new saros_data(140, 2273421.5), /* 16 Apr 1512 */
            new saros_data(141, 2310334.5), /* 19 May 1613 */
            new saros_data(142, 2314320.5), /* 17 Apr 1624 */
            new saros_data(143, 2311722.5), /* 07 Mar 1617 */
            new saros_data(144, 2355221.5), /* 11 Apr 1736 */
            new saros_data(145, 2319695.5), /* 04 Jan 1639 */
            new saros_data(146, 2284169.5), /* 19 Sep 1541 */
            new saros_data(147, 2314498.5), /* 12 Oct 1624 */
            new saros_data(148, 2325069.5), /* 21 Sep 1653 */
            new saros_data(149, 2329056.5), /* 21 Aug 1664 */
            new saros_data(150, 2352799.5), /* 24 Aug 1729 */
            new saros_data(151, 2369956.5), /* 14 Aug 1776 */
            new saros_data(152, 2380528.5), /* 26 Jul 1805 */
            new saros_data(153, 2404271.5), /* 28 Jul 1870 */
            new saros_data(154, 2421428.5), /* 19 Jul 1917 */
            new saros_data(155, 2425414.5), /* 17 Jun 1928 */
            new saros_data(156, 2455743.5), /* 01 Jul 2011 */
            new saros_data(157, 2472900.5), /* 21 Jun 2058 */
            new saros_data(158, 2476886.5), /* 20 May 2069 */
            new saros_data(159, 2500629.5), /* 23 May 2134 */
            new saros_data(160, 2517786.5), /* 13 May 2181 */
            new saros_data(161, 2515187.5), /* 01 Apr 2174 */
            new saros_data(162, 2545516.5), /* 15 Apr 2257 */
            new saros_data(163, 2556087.5), /* 25 Mar 2286 */
            new saros_data(164, 2487635.5), /* 24 Oct 2098 */
            new saros_data(165, 2504793.5), /* 16 Oct 2145 */
            new saros_data(166, 2535121.5), /* 29 Oct 2228 */
            new saros_data(167, 2525936.5), /* 06 Sep 2203 */
            new saros_data(168, 2543094.5), /* 28 Aug 2250 */
            new saros_data(169, 2573422.5), /* 10 Sep 2333 */
            new saros_data(170, 2577408.5), /* 09 Aug 2344 */
            new saros_data(171, 2594566.5), /* 01 Aug 2391 */
            new saros_data(172, 2624894.5), /* 13 Aug 2474 */
            new saros_data(173, 2628880.5), /* 12 Jul 2485 */
            new saros_data(174, 2646038.5), /* 04 Jul 2532 */
            new saros_data(175, 2669780.5), /* 05 Jul 2597 */
            new saros_data(176, 2673766.5), /* 04 Jun 2608 */
            new saros_data(177, 2690924.5), /* 27 May 2655 */
            new saros_data(178, 2721252.5), /* 09 Jun 2738 */
            new saros_data(179, 2718653.5), /* 28 Apr 2731 */
            new saros_data(180, 2729226.5), /* 08 Apr 2760 */
        };

        const int NSAROS_LUNAR = 180;
        static saros_data[] saros_data_lunar = new saros_data[NSAROS_LUNAR]{
            new saros_data(1, 782437.5), /* 14 Mar -2570 */
            new saros_data(2, 799593.5), /* 03 Mar -2523 */
            new saros_data(3, 783824.5), /* 30 Dec -2567 */
            new saros_data(4, 754884.5), /* 06 Oct -2646 */
            new saros_data(5, 824724.5), /* 22 Dec -2455 */
            new saros_data(6, 762857.5), /* 04 Aug -2624 */
            new saros_data(7, 773430.5), /* 16 Jul -2595 */
            new saros_data(8, 810343.5), /* 08 Aug -2494 */
            new saros_data(9, 807743.5), /* 26 Jun -2501 */
            new saros_data(10, 824901.5), /* 17 Jun -2454 */
            new saros_data(11, 855229.5), /* 29 Jun -2371 */
            new saros_data(12, 859215.5), /* 28 May -2360 */
            new saros_data(13, 876373.5), /* 20 May -2313 */
            new saros_data(14, 906701.5), /* 01 Jun -2230 */
            new saros_data(15, 910687.5), /* 30 Apr -2219 */
            new saros_data(16, 927845.5), /* 21 Apr -2172 */
            new saros_data(17, 958173.5), /* 04 May -2089 */
            new saros_data(18, 962159.5), /* 02 Apr -2078 */
            new saros_data(19, 979317.5), /* 24 Mar -2031 */
            new saros_data(20, 1009645.5), /* 05 Apr -1948 */
            new saros_data(21, 1007046.5), /* 22 Feb -1955 */
            new saros_data(22, 1017618.5), /* 02 Feb -1926 */
            new saros_data(23, 1054531.5), /* 25 Feb -1825 */
            new saros_data(24, 979493.5), /* 16 Sep -2031 */
            new saros_data(25, 976895.5), /* 06 Aug -2038 */
            new saros_data(26, 1020394.5), /* 09 Sep -1919 */
            new saros_data(27, 1017794.5), /* 28 Jul -1926 */
            new saros_data(28, 1028367.5), /* 09 Jul -1897 */
            new saros_data(29, 1058695.5), /* 21 Jul -1814 */
            new saros_data(30, 1062681.5), /* 19 Jun -1803 */
            new saros_data(31, 1073253.5), /* 30 May -1774 */
            new saros_data(32, 1110167.5), /* 23 Jun -1673 */
            new saros_data(33, 1114153.5), /* 22 May -1662 */
            new saros_data(34, 1131311.5), /* 13 May -1615 */
            new saros_data(35, 1161639.5), /* 25 May -1532 */
            new saros_data(36, 1165625.5), /* 24 Apr -1521 */
            new saros_data(37, 1176197.5), /* 03 Apr -1492 */
            new saros_data(38, 1213111.5), /* 27 Apr -1391 */
            new saros_data(39, 1217097.5), /* 26 Mar -1380 */
            new saros_data(40, 1221084.5), /* 24 Feb -1369 */
            new saros_data(41, 1257997.5), /* 18 Mar -1268 */
            new saros_data(42, 1255398.5), /* 04 Feb -1275 */
            new saros_data(43, 1186946.5), /* 07 Sep -1463 */
            new saros_data(44, 1283128.5), /* 06 Jan -1199 */
            new saros_data(45, 1227845.5), /* 29 Aug -1351 */
            new saros_data(46, 1225247.5), /* 19 Jul -1358 */
            new saros_data(47, 1255575.5), /* 31 Jul -1275 */
            new saros_data(48, 1272732.5), /* 21 Jul -1228 */
            new saros_data(49, 1276719.5), /* 21 Jun -1217 */
            new saros_data(50, 1307047.5), /* 03 Jul -1134 */
            new saros_data(51, 1317619.5), /* 13 Jun -1105 */
            new saros_data(52, 1328191.5), /* 23 May -1076 */
            new saros_data(53, 1358519.5), /* 05 Jun -0993 */
            new saros_data(54, 1375676.5), /* 26 May -0946 */
            new saros_data(55, 1379663.5), /* 25 Apr -0935 */
            new saros_data(56, 1409991.5), /* 07 May -0852 */
            new saros_data(57, 1420562.5), /* 16 Apr -0823 */
            new saros_data(58, 1424549.5), /* 16 Mar -0812 */
            new saros_data(59, 1461463.5), /* 09 Apr -0711 */
            new saros_data(60, 1465449.5), /* 08 Mar -0700 */
            new saros_data(61, 1436509.5), /* 13 Dec -0780 */
            new saros_data(62, 1493179.5), /* 08 Feb -0624 */
            new saros_data(63, 1457653.5), /* 03 Nov -0722 */
            new saros_data(64, 1435298.5), /* 20 Aug -0783 */
            new saros_data(65, 1452456.5), /* 11 Aug -0736 */
            new saros_data(66, 1476198.5), /* 12 Aug -0671 */
            new saros_data(67, 1480184.5), /* 11 Jul -0660 */
            new saros_data(68, 1503928.5), /* 14 Jul -0595 */
            new saros_data(69, 1527670.5), /* 15 Jul -0530 */
            new saros_data(70, 1531656.5), /* 13 Jun -0519 */
            new saros_data(71, 1548814.5), /* 04 Jun -0472 */
            new saros_data(72, 1579142.5), /* 17 Jun -0389 */
            new saros_data(73, 1583128.5), /* 16 May -0378 */
            new saros_data(74, 1600286.5), /* 07 May -0331 */
            new saros_data(75, 1624028.5), /* 08 May -0266 */
            new saros_data(76, 1628015.5), /* 07 Apr -0255 */
            new saros_data(77, 1651758.5), /* 09 Apr -0190 */
            new saros_data(78, 1675500.5), /* 10 Apr -0125 */
            new saros_data(79, 1672901.5), /* 27 Feb -0132 */
            new saros_data(80, 1683474.5), /* 07 Feb -0103 */
            new saros_data(81, 1713801.5), /* 19 Feb -0020 */
            new saros_data(82, 1645349.5), /* 21 Sep -0208 */
            new saros_data(83, 1649336.5), /* 22 Aug -0197 */
            new saros_data(84, 1686249.5), /* 13 Sep -0096 */
            new saros_data(85, 1683650.5), /* 02 Aug -0103 */
            new saros_data(86, 1694222.5), /* 13 Jul -0074 */
            new saros_data(87, 1731136.5), /* 06 Aug 0027 */
            new saros_data(88, 1735122.5), /* 05 Jul 0038 */
            new saros_data(89, 1745694.5), /* 15 Jun 0067 */
            new saros_data(90, 1776022.5), /* 27 Jun 0150 */
            new saros_data(91, 1786594.5), /* 07 Jun 0179 */
            new saros_data(92, 1797166.5), /* 17 May 0208 */
            new saros_data(93, 1827494.5), /* 30 May 0291 */
            new saros_data(94, 1838066.5), /* 09 May 0320 */
            new saros_data(95, 1848638.5), /* 19 Apr 0349 */
            new saros_data(96, 1878966.5), /* 01 May 0432 */
            new saros_data(97, 1882952.5), /* 31 Mar 0443 */
            new saros_data(98, 1880354.5), /* 18 Feb 0436 */
            new saros_data(99, 1923853.5), /* 24 Mar 0555 */
            new saros_data(100, 1881741.5), /* 06 Dec 0439 */
            new saros_data(101, 1852801.5), /* 11 Sep 0360 */
            new saros_data(102, 1889715.5), /* 05 Oct 0461 */
            new saros_data(103, 1893701.5), /* 03 Sep 0472 */
            new saros_data(104, 1897688.5), /* 04 Aug 0483 */
            new saros_data(105, 1928016.5), /* 16 Aug 0566 */
            new saros_data(106, 1938588.5), /* 27 Jul 0595 */
            new saros_data(107, 1942575.5), /* 26 Jun 0606 */
            new saros_data(108, 1972903.5), /* 08 Jul 0689 */
            new saros_data(109, 1990059.5), /* 27 Jun 0736 */
            new saros_data(110, 1994046.5), /* 28 May 0747 */
            new saros_data(111, 2024375.5), /* 10 Jun 0830 */
            new saros_data(112, 2034946.5), /* 20 May 0859 */
            new saros_data(113, 2045518.5), /* 29 Apr 0888 */
            new saros_data(114, 2075847.5), /* 13 May 0971 */
            new saros_data(115, 2086418.5), /* 21 Apr 1000 */
            new saros_data(116, 2083820.5), /* 11 Mar 0993 */
            new saros_data(117, 2120733.5), /* 03 Apr 1094 */
            new saros_data(118, 2124719.5), /* 02 Mar 1105 */
            new saros_data(119, 2062852.5), /* 14 Oct 0935 */
            new saros_data(120, 2086596.5), /* 16 Oct 1000 */
            new saros_data(121, 2103752.5), /* 06 Oct 1047 */
            new saros_data(122, 2094568.5), /* 14 Aug 1022 */
            new saros_data(123, 2118311.5), /* 16 Aug 1087 */
            new saros_data(124, 2142054.5), /* 17 Aug 1152 */
            new saros_data(125, 2146040.5), /* 17 Jul 1163 */
            new saros_data(126, 2169783.5), /* 18 Jul 1228 */
            new saros_data(127, 2186940.5), /* 09 Jul 1275 */
            new saros_data(128, 2197512.5), /* 18 Jun 1304 */
            new saros_data(129, 2214670.5), /* 10 Jun 1351 */
            new saros_data(130, 2238412.5), /* 10 Jun 1416 */
            new saros_data(131, 2242398.5), /* 10 May 1427 */
            new saros_data(132, 2266142.5), /* 12 May 1492 */
            new saros_data(133, 2289884.5), /* 13 May 1557 */
            new saros_data(134, 2287285.5), /* 01 Apr 1550 */
            new saros_data(135, 2311028.5), /* 13 Apr 1615 */
            new saros_data(136, 2334770.5), /* 13 Apr 1680 */
            new saros_data(137, 2292659.5), /* 17 Dec 1564 */
            new saros_data(138, 2276890.5), /* 15 Oct 1521 */
            new saros_data(139, 2326974.5), /* 09 Dec 1658 */
            new saros_data(140, 2304619.5), /* 25 Sep 1597 */
            new saros_data(141, 2308606.5), /* 25 Aug 1608 */
            new saros_data(142, 2345520.5), /* 19 Sep 1709 */
            new saros_data(143, 2349506.5), /* 18 Aug 1720 */
            new saros_data(144, 2360078.5), /* 29 Jul 1749 */
            new saros_data(145, 2390406.5), /* 11 Aug 1832 */
            new saros_data(146, 2394392.5), /* 11 Jul 1843 */
            new saros_data(147, 2411550.5), /* 02 Jul 1890 */
            new saros_data(148, 2441878.5), /* 15 Jul 1973 */
            new saros_data(149, 2445864.5), /* 13 Jun 1984 */
            new saros_data(150, 2456437.5), /* 25 May 2013 */
            new saros_data(151, 2486765.5), /* 06 Jun 2096 */
            new saros_data(152, 2490751.5), /* 07 May 2107 */
            new saros_data(153, 2501323.5), /* 16 Apr 2136 */
            new saros_data(154, 2538236.5), /* 10 May 2237 */
            new saros_data(155, 2529052.5), /* 18 Mar 2212 */
            new saros_data(156, 2473771.5), /* 08 Nov 2060 */
            new saros_data(157, 2563367.5), /* 01 Mar 2306 */
            new saros_data(158, 2508085.5), /* 21 Oct 2154 */
            new saros_data(159, 2505486.5), /* 09 Sep 2147 */
            new saros_data(160, 2542400.5), /* 03 Oct 2248 */
            new saros_data(161, 2546386.5), /* 02 Sep 2259 */
            new saros_data(162, 2556958.5), /* 12 Aug 2288 */
            new saros_data(163, 2587287.5), /* 27 Aug 2371 */
            new saros_data(164, 2597858.5), /* 05 Aug 2400 */
            new saros_data(165, 2601845.5), /* 06 Jul 2411 */
            new saros_data(166, 2632173.5), /* 18 Jul 2494 */
            new saros_data(167, 2649330.5), /* 09 Jul 2541 */
            new saros_data(168, 2653317.5), /* 08 Jun 2552 */
            new saros_data(169, 2683645.5), /* 22 Jun 2635 */
            new saros_data(170, 2694217.5), /* 01 Jun 2664 */
            new saros_data(171, 2698203.5), /* 01 May 2675 */
            new saros_data(172, 2728532.5), /* 15 May 2758 */
            new saros_data(173, 2739103.5), /* 24 Apr 2787 */
            new saros_data(174, 2683822.5), /* 16 Dec 2635 */
            new saros_data(175, 2740492.5), /* 11 Feb 2791 */
            new saros_data(176, 2724722.5), /* 09 Dec 2747 */
            new saros_data(177, 2708952.5), /* 05 Oct 2704 */
            new saros_data(178, 2732695.5), /* 07 Oct 2769 */
            new saros_data(179, 2749852.5), /* 27 Sep 2816 */
            new saros_data(180, 2753839.5), /* 28 Aug 2827 */
        };

        /* Computes geographic location and type of solar eclipse 
         * for a given tjd 
         * iflag:	to indicate ephemeris to be used
         *			(SEFLG_JPLEPH, SEFLG_SWIEPH, SEFLG_MOSEPH)
         *
         * Algorithms for the central line is taken from Montenbruck, pp. 179ff.,
         * with the exception, that we consider refraction for the maxima of
         * partial and noncentral eclipses.
         * Geographical positions are referred to sea level / the mean ellipsoid.
         *
         * Errors:
         * - from uncertainty of JPL-ephemerides (0.01 arcsec): 
         *	about 40 meters
         * - from displacement of shadow points by atmospheric refraction:
         *      a few meters 
         * - from deviation of the geoid from the ellipsoid 
         *      a few meters
         * - from polar motion
         *      a few meters
         * For geographical locations that are interesting for observation,
         * the error is always < 100 m.
         * However, if the sun is close to the horizon,
         * all of these errors can grow up to a km or more. 
         * 
         * Function returns:
         * -1 (ERR)	on error (e.g. if swe_calc() for sun or moon fails)
         * 0		if there is no solar eclipse at tjd
         * SE_ECL_TOTAL
         * SE_ECL_ANNULAR
         * SE_ECL_TOTAL | SE_ECL_CENTRAL
         * SE_ECL_TOTAL | SE_ECL_NONCENTRAL
         * SE_ECL_ANNULAR | SE_ECL_CENTRAL
         * SE_ECL_ANNULAR | SE_ECL_NONCENTRAL
         * SE_ECL_PARTIAL
         * 
         * geopos[0]:	geographic longitude of central line
         * geopos[1]:	geographic latitude of central line
         *
         * not implemented so far:
         *
         * geopos[2]:	geographic longitude of northern limit of umbra
         * geopos[3]:	geographic latitude of northern limit of umbra
         * geopos[4]:	geographic longitude of southern limit of umbra
         * geopos[5]:	geographic latitude of southern limit of umbra
         * geopos[6]:	geographic longitude of northern limit of penumbra
         * geopos[7]:	geographic latitude of northern limit of penumbra
         * geopos[8]:	geographic longitude of southern limit of penumbra
         * geopos[9]:	geographic latitude of southern limit of penumbra
         *
         * Attention: "northern" and "southern" limits of umbra do not 
         * necessarily correspond to the northernmost or southernmost 
         * geographic position, where the total, annular, or partial
         * phase is visible at a given time. 
         * Imagine a situation in northern summer, when the sun illuminates 
         * the northern polar circle. The southernmost point of the core 
         * shadow may then touch the north pole, and therefore the 
         * northernmost point will be more in the south.
         * Note also that with annular eclipses, the northern edge is 
         * usually geographically the southern one. With annular-total 
         * ones, the two lines cross, usually twice. The maximum is always
         * total in such cases. 
         *
         * attr[0]	fraction of solar diameter covered by moon (magnitude)
         * attr[1]	ratio of lunar diameter to solar one
         * attr[2]	fraction of solar disc covered by moon (obscuration)
         * attr[3]      diameter of core shadow in km
         * attr[4]	azimuth of sun at tjd
         * attr[5]	true altitude of sun above horizon at tjd
         * attr[6]	apparent altitude of sun above horizon at tjd
         * attr[7]	angular distance of moon from sun in degrees
         *         declare as attr[20] at least !
         */
        public Int32 swe_sol_eclipse_where(
                double tjd_ut,
                Int32 ifl,
                double[] geopos,
                double[] attr,
                ref string serr) {
            Int32 retflag, retflag2;
            double[] dcore = new double[10];
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_ut, ifl, 0, ref serr);
            if ((retflag = eclipse_where(tjd_ut, SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) < 0)
                return retflag;
            if ((retflag2 = eclipse_how(tjd_ut, SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], 0, attr, ref serr)) == SwissEph.ERR)
                return retflag2;
            attr[3] = dcore[0];
            return retflag;
        }

        public Int32 swe_lun_occult_where(
                double tjd_ut,
                Int32 ipl,
                string starname,
                Int32 ifl,
                double[] geopos,
                double[] attr,
                ref string serr) {
            Int32 retflag, retflag2;
            double[] dcore = new double[10];
            if (ipl < 0) ipl = 0;
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_ut, ifl, 0, ref serr);
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            if ((retflag = eclipse_where(tjd_ut, ipl, starname, ifl, geopos, dcore, ref serr)) < 0)
                return retflag;
            if ((retflag2 = eclipse_how(tjd_ut, ipl, starname, ifl, geopos[0], geopos[1], 0, attr, ref serr)) == SwissEph.ERR)
                return retflag2;
            attr[3] = dcore[0];
            return retflag;
        }

        /* Used by several swe_sol_eclipse_ functions.
         * Like swe_sol_eclipse_where(), but instead of attr[0], it returns:
         *
         * dcore[0]:	core shadow width in km
         * dcore[2]:	distance of shadow axis from geocenter r0
         * dcore[3]:	diameter of core shadow on fundamental plane d0
         * dcore[4]:	diameter of half-shadow on fundamental plane D0
         */
        Int32 eclipse_where(double tjd_ut, Int32 ipl, string starname, Int32 ifl, double[] geopos, double[] dcore,
               ref string serr) {
            int i;
            Int32 retc = 0, niter = 0;
            double[] e = new double[6], et = new double[6], rm = new double[6], rs = new double[6], rmt = new double[6], rst = new double[6], xs = new double[6], xst = new double[6];
            //#if 0
            //  double erm[6];
            //#endif
            double[] x = new double[6];
            double[] lm = new double[6], ls = new double[6], lx = new double[6];
            double dsm, dsmt, d0, D0, s0, r0, d, s, dm;
            double de = 6378140.0 / Sweph.AUNIT;
            double earthobl = 1 - Sweph.EARTH_OBLATENESS;
            double deltat, tjd, sidt;
            double drad;
            double sinf1, sinf2, cosf1, cosf2;
            double rmoon = RMOON;
            double dmoon = 2 * rmoon;
            Int32 iflag, iflag2;
            /* double ecce =Math.Sqrt(2 * EARTH_OBLATENESS - EARTH_OBLATENESS * EARTH_OBLATENESS); */
            bool no_eclipse = false;
            Sweph.epsilon oe = SE.Sweph.swed.oec;
            for (i = 0; i < 10; i++)
                dcore[i] = 0;
            /* nutation need not be in lunar and solar positions,
             * if mean sidereal time will be used */
            iflag = SwissEph.SEFLG_SPEED | SwissEph.SEFLG_EQUATORIAL | ifl;
            iflag2 = iflag | SwissEph.SEFLG_RADIANS;
            iflag = iflag | SwissEph.SEFLG_XYZ;
            deltat = SE.swe_deltat_ex(tjd_ut, ifl, ref serr);
            tjd = tjd_ut + deltat;
            /* moon in cartesian coordinates */
            if ((retc = SE.swe_calc(tjd, SwissEph.SE_MOON, iflag, rm, ref serr)) == SwissEph.ERR)
                return retc;
            /* moon in polar coordinates */
            if ((retc = SE.swe_calc(tjd, SwissEph.SE_MOON, iflag2, lm, ref serr)) == SwissEph.ERR)
                return retc;
            /* sun in cartesian coordinates */
            if ((retc = calc_planet_star(tjd, ipl, starname, iflag, rs, ref serr)) == SwissEph.ERR)
                return retc;
            /* sun in polar coordinates */
            if ((retc = calc_planet_star(tjd, ipl, starname, iflag2, ls, ref serr)) == SwissEph.ERR)
                return retc;
            /* save sun position */
            for (i = 0; i <= 2; i++)
                rst[i] = rs[i];
            /* save moon position */
            for (i = 0; i <= 2; i++)
                rmt[i] = rm[i];
            if ((iflag & SwissEph.SEFLG_NONUT) != 0)
                sidt = SE.swe_sidtime0(tjd_ut, oe.eps * SwissEph.RADTODEG, 0) * 15 * SwissEph.DEGTORAD;
            else
                sidt = SE.swe_sidtime(tjd_ut) * 15 * SwissEph.DEGTORAD;
            /*
             * radius of planet disk in AU
             */
            if (!String.IsNullOrEmpty(starname))
                drad = 0;
            else if (ipl < Sweph.NDIAM)
                drad = Sweph.pla_diam[ipl] / 2 / Sweph.AUNIT;
            else if (ipl > SwissEph.SE_AST_OFFSET)
                drad = SE.Sweph.swed.ast_diam / 2 * 1000 / Sweph.AUNIT; /* km -> m -> AU */
            else
                drad = 0;
        iter_where:
            for (i = 0; i <= 2; i++) {
                rs[i] = rst[i];
                rm[i] = rmt[i];
            }
            /* Account for oblateness of earth:
             * Instead of flattening the earth, we apply the 
             * correction to the z coordinate of the moon and
             * the sun. This makes the calculation easier.
             */
            for (i = 0; i <= 2; i++)
                lx[i] = lm[i];
            SE.SwephLib.swi_polcart(lx, rm);
            rm[2] /= earthobl;
            /* distance of moon from geocenter */
            dm = Math.Sqrt(SE.Sweph.square_sum(rm));
            /* Account for oblateness of earth */
            for (i = 0; i <= 2; i++)
                lx[i] = ls[i];
            SE.SwephLib.swi_polcart(lx, rs);
            rs[2] /= earthobl;
            /* sun - moon vector */
            for (i = 0; i <= 2; i++) {
                e[i] = (rm[i] - rs[i]);
                et[i] = (rmt[i] - rst[i]);
            }
            /* distance sun - moon */
            dsm = Math.Sqrt(SE.Sweph.square_sum(e));
            dsmt = Math.Sqrt(SE.Sweph.square_sum(et));
            /* sun - moon unit vector */
            for (i = 0; i <= 2; i++) {
                e[i] /= dsm;
                et[i] /= dsmt;
                //#if 0
                //    erm[i] = rm[i] / dm;
                //#endif
            }
            sinf1 = ((drad - rmoon) / dsm);
            cosf1 = Math.Sqrt(1 - sinf1 * sinf1);
            sinf2 = ((drad + rmoon) / dsm);
            cosf2 = Math.Sqrt(1 - sinf2 * sinf2);
            /* distance of moon from fundamental plane */
            s0 = -SE.Sweph.dot_prod(rm, e);
            /* distance of shadow axis from geocenter */
            r0 = Math.Sqrt(dm * dm - s0 * s0);
            /* diameter of core shadow on fundamental plane */
            d0 = (s0 / dsm * (drad * 2 - dmoon) - dmoon) / cosf1;
            /* diameter of half-shadow on fundamental plane */
            D0 = (s0 / dsm * (drad * 2 + dmoon) + dmoon) / cosf2;
            dcore[2] = r0;
            dcore[3] = d0;
            dcore[4] = D0;
            dcore[5] = cosf1;
            dcore[6] = cosf2;
            for (i = 2; i < 5; i++)
                dcore[i] *= Sweph.AUNIT / 1000.0;
            /**************************
             * central (total or annular) phase 
             **************************/
            retc = 0;
            if (de * cosf1 >= r0) {
                retc |= SwissEph.SE_ECL_CENTRAL;
            } else if (r0 <= de * cosf1 + Math.Abs(d0) / 2) {
                retc |= SwissEph.SE_ECL_NONCENTRAL;
            } else if (r0 <= de * cosf2 + D0 / 2) {
                retc |= (SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_NONCENTRAL);
            } else {
                serr = C.sprintf("no solar eclipse at tjd = %f", tjd);
                for (i = 0; i < 10; i++)
                    geopos[i] = 0;
                dcore[0] = 0;
                retc = 0;
                d = 0;
                no_eclipse = true;
                /*return retc;*/
            }
            /* distance of shadow point from fundamental plane */
            d = s0 * s0 + de * de - dm * dm;
            if (d > 0)
                d = Math.Sqrt(d);
            else
                d = 0;
            /* distance of moon from shadow point on earth */
            s = s0 - d;
            /* next: geographic position of eclipse center.
             * if shadow axis does not touch the earth, 
             * place on earth with maximum occultation is computed.
             */
            //#if 0	/* the following stuff is meaningless for observations */
            //  /* 
            //   * account for refraction at horizon
            //   */
            //  if (d == 0) {
            //    double ds, a, b;
            //    /* distance of sun from geocenter */
            //    ds =Math.Sqrt(square_sum(rs));
            //    a = PI - acos(swi_dot_prod_unit(e, erm));
            //       /* refraction at horizon + sun radius = about 0.83 degrees */
            //    b = 34.4556 / 60.0 * DEGTORAD + asin(drad / ds);
            //# if 0
            //     /* at edge of umbra and penumbra 
            //      * light rays are not parallel to shadow axis.
            //      * for a short time close to contact of umbra and
            //      * penumbra, an angle < 0.27 degrees would have
            //      * to be subtracted from b;
            //      */ 
            //    if (retc & SE_ECL_PARTIAL) {
            //      d = d0;
            //      sinf = sinf1;
            //    } else {
            //      d = D0;
            //      sinf = sinf2;
            //    }
            //    c = (r0 - de) / d * 2 * sinf;
            //    if (c > sinf1) {
            //      b -= .....;
            //    }
            //      printf("%f %f %f", a * RADTODEG, b * RADTODEG, s);
            //      printf(" %f\n", s);
            //# else
            //    if (retc & SE_ECL_PARTIAL) 
            //      b -= asin(sinf2);	/* maximum! */
            //    else
            //      b -= asin(sinf1);
            //# endif
            //    s += tan(b) * cos(PI / 2 - a) * dm;
            //  }
            //#endif
            /* geographic position of eclipse center (maximum) */
            for (i = 0; i <= 2; i++)
                xs[i] = rm[i] + s * e[i];
            /* we need geographic position with correct z, as well */
            for (i = 0; i <= 2; i++)
                xst[i] = xs[i];
            xst[2] *= earthobl;
            SE.SwephLib.swi_cartpol(xst, xst);
            if (niter <= 0) {
                double cosfi = Math.Cos(xst[1]);
                double sinfi = Math.Sin(xst[1]);
                double eobl = Sweph.EARTH_OBLATENESS;
                double cc = 1 / Math.Sqrt(cosfi * cosfi + (1 - eobl) * (1 - eobl) * sinfi * sinfi);
                double ss = (1 - eobl) * (1 - eobl) * cc;
                earthobl = ss;
                niter++;
                goto iter_where;
            }
            SE.SwephLib.swi_polcart(xst, xst);
            /* to longitude and latitude */
            SE.SwephLib.swi_cartpol(xs, xs);
            /* measure from sidereal time at greenwich */
            xs[0] -= sidt;
            xs[0] *= SwissEph.RADTODEG;
            xs[1] *= SwissEph.RADTODEG;
            xs[0] = SE.swe_degnorm(xs[0]);
            /* west is negative */
            if (xs[0] > 180)
                xs[0] -= 360;
            geopos[0] = xs[0];
            geopos[1] = xs[1];
            /* diameter of core shadow: 
             * first, distance moon - place of eclipse on earth */
            for (i = 0; i <= 2; i++)
                x[i] = rmt[i] - xst[i];
            s = Math.Sqrt(SE.Sweph.square_sum(x));
            /* diameter of core shadow at place of maximum eclipse */
            dcore[0] = (s / dsmt * (drad * 2 - dmoon) - dmoon) * cosf1;
            dcore[0] *= Sweph.AUNIT / 1000.0;
            /* diameter of penumbra at place of maximum eclipse */
            dcore[1] = (s / dsmt * (drad * 2 + dmoon) + dmoon) * cosf2;
            dcore[1] *= Sweph.AUNIT / 1000.0;
            if (0 == (retc & SwissEph.SE_ECL_PARTIAL) && !no_eclipse) {
                if (dcore[0] > 0) {
                    /*printf("annular\n");*/
                    retc |= SwissEph.SE_ECL_ANNULAR;
                } else {
                    /*printf("total\n");*/
                    retc |= SwissEph.SE_ECL_TOTAL;
                }
            }
            return retc;
        }

        Int32 calc_planet_star(double tjd_et, Int32 ipl, string starname, Int32 iflag, double[] x, ref string serr) {
            int i;
            int retc = SwissEph.OK;
            if (String.IsNullOrEmpty(starname)) {
                retc = SE.swe_calc(tjd_et, ipl, iflag, x, ref serr);
            } else {
                if ((retc = SE.swe_fixstar(starname, tjd_et, iflag, x, ref serr)) == SwissEph.OK) {
                    /* fixstars have the standard distance 1. 
                     * in the occultation routines, this might lead to errors 
                     * if interpreted as AU distance. To avoid this, we make it very high.
                     */
                    if ((iflag & SwissEph.SEFLG_XYZ) != 0) {
                        for (i = 0; i < 3; i++)
                            x[i] *= 100000000;
                    } else {
                        x[2] *= 100000000;
                    }
                }
            }
            return retc;
        }

        /* Computes attributes of a solar eclipse for given tjd, geo. longitude, 
         * geo. latitude, and geo. height.
         * 
         * retflag	SE_ECL_TOTAL or SE_ECL_ANNULAR or SE_ECL_PARTIAL
         *              SE_ECL_NONCENTRAL
         *              if 0, no eclipse is visible at geogr. position.
         * 
         * attr[0]	fraction of solar diameter covered by moon;
         *              with total/annular eclipses, it results in magnitude acc. to IMCCE.
         * attr[1]	ratio of lunar diameter to solar one
         * attr[2]	fraction of solar disc covered by moon (obscuration)
         * attr[3]      diameter of core shadow in km
         * attr[4]	azimuth of sun at tjd
         * attr[5]	true altitude of sun above horizon at tjd
         * attr[6]	apparent altitude of sun above horizon at tjd
         * attr[7]	elongation of moon in degrees
         * attr[8]	magnitude acc. to NASA;
         *              = attr[0] for partial and attr[1] for annular and total eclipses
         * attr[9]	saros series number
         * attr[10]	saros series member number
         *         declare as attr[20] at least !
         * 
         */
        public Int32 swe_sol_eclipse_how(
                  double tjd_ut,
                  Int32 ifl,
                  double[] geopos,
                  double[] attr,
                  ref string serr) {
            Int32 retflag, retflag2;
            double[] dcore = new double[10], ls = new double[6], xaz = new double[6];
            double[] geopos2 = new double[20];
            if (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                if (serr != null)
                    serr = C.sprintf("location for eclipses must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_ut, ifl, 0, ref serr);
            if ((retflag = eclipse_how(tjd_ut, SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], geopos[2], attr, ref serr)) == SwissEph.ERR)
                return retflag;
            if ((retflag2 = eclipse_where(tjd_ut, SwissEph.SE_SUN, null, ifl, geopos2, dcore, ref serr)) == SwissEph.ERR)
                return retflag2;
            if (retflag != 0)
                retflag |= (retflag2 & (SwissEph.SE_ECL_CENTRAL | SwissEph.SE_ECL_NONCENTRAL));
            attr[3] = dcore[0];
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            if (SE.swe_calc_ut(tjd_ut, SwissEph.SE_SUN, ifl | SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            swe_azalt(tjd_ut, SwissEph.SE_EQU2HOR, geopos, 0, 10, ls, xaz);
            attr[4] = xaz[0];
            attr[5] = xaz[1];
            attr[6] = xaz[2];
            if (xaz[2] <= 0)
                retflag = 0;
            return retflag;
        }

        //static int USE_AZ_NAV =0;
        Int32 eclipse_how(double tjd_ut, Int32 ipl, string starname, Int32 ifl,
                  double geolon, double geolat, double geohgt,
                  double[] attr, ref string serr) {
            int i, j, k;
            Int32 retc = 0;
            double te, d;
            double[] xs = new double[6], xm = new double[6], ls = new double[6], lm = new double[6], x1 = new double[6], x2 = new double[6];
            double rmoon, rsun, rsplusrm, rsminusrm;
            double dctr;
            double drad;
            Int32 iflag = SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_TOPOCTR | ifl;
            Int32 iflagcart = iflag | SwissEph.SEFLG_XYZ;
#if USE_AZ_NAV
  double mdd, eps, sidt, armc;
#endif
            double[] xh = new double[6]; double hmin_appr;
            double lsun, lmoon, lctr, lsunleft, a, b, sc1, sc2;
            double[] geopos = new double[3];
            for (i = 0; i < 10; i++)
                attr[i] = 0;
            geopos[0] = geolon;
            geopos[1] = geolat;
            geopos[2] = geohgt;
            te = tjd_ut + SE.swe_deltat_ex(tjd_ut, ifl, ref serr);
            SE.swe_set_topo(geolon, geolat, geohgt);
            if (calc_planet_star(te, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(te, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (calc_planet_star(te, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(te, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /*
             * radius of planet disk in AU
             */
            if (!String.IsNullOrEmpty(starname))
                drad = 0;
            else if (ipl < Sweph.NDIAM)
                drad = Sweph.pla_diam[ipl] / 2 / Sweph.AUNIT;
            else if (ipl > SwissEph.SE_AST_OFFSET)
                drad = SE.Sweph.swed.ast_diam / 2 * 1000 / Sweph.AUNIT; /* km -> m -> AU */
            else
                drad = 0;
            /* 
             * azimuth and altitude of sun or planet
             */
#if USE_AZ_NAV   //* old */
  eps = swi_epsiln(te, iflag);
  if (iflag & SEFLG_NONUT)
    sidt = swe_sidtime0(tjd_ut, eps * RADTODEG, 0) * 15;
  else
    sidt = swe_sidtime(tjd_ut) * 15;
  armc = sidt + geolon;
  mdd = swe_degnorm(ls[0] - armc);
  xh[0] = swe_degnorm(mdd - 90);
  xh[1] = ls[1];
  xh[2] = ls[2];
  swe_cotrans(xh, xh, 90 - geolat); /* azimuth from east, counterclock, via north */
#else
            swe_azalt(tjd_ut, SwissEph.SE_EQU2HOR, geopos, 0, 10, ls, xh); /* azimuth from south, clockwise, via west */
#endif
            /* eclipse description */
            rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
            rsun = Math.Asin(drad / ls[2]) * SwissEph.RADTODEG;
            rsplusrm = rsun + rmoon;
            rsminusrm = rsun - rmoon;
            for (i = 0; i < 3; i++) {
                x1[i] = xs[i] / ls[2];
                x2[i] = xm[i] / lm[2];
            }
            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
            /* 
             * phase 
             */
            if (dctr < rsminusrm)
                retc = SwissEph.SE_ECL_ANNULAR;
            else if (dctr < Math.Abs(rsminusrm))
                retc = SwissEph.SE_ECL_TOTAL;
            else if (dctr < rsplusrm)
                retc = SwissEph.SE_ECL_PARTIAL;
            else {
                retc = 0;
                serr = C.sprintf("no solar eclipse at tjd = %f", tjd_ut);
            }
            /*
             * ratio of diameter of moon to that of sun
             */
            if (rsun > 0)
                attr[1] = rmoon / rsun;
            else
                attr[1] = 0;
            /* 
             * eclipse magnitude:
             * fraction of solar diameter covered by moon
             */
            lsun = Math.Asin(rsun / 2 * SwissEph.DEGTORAD) * 2;
            lsunleft = (-dctr + rsun + rmoon);
            if (lsun > 0) {
                attr[0] = lsunleft / rsun / 2;
            } else {
                attr[0] = 100;
            }
            /*if (retc == SE_ECL_ANNULAR || retc == SE_ECL_TOTAL)
                attr[0] = attr[1];*/
            /*
             * obscuration:
             * fraction of solar disc obscured by moon
             */
            lsun = rsun;
            lmoon = rmoon;
            lctr = dctr;
            if (retc == 0 || lsun == 0) {
                attr[2] = 100;
            } else if (retc == SwissEph.SE_ECL_TOTAL || retc == SwissEph.SE_ECL_ANNULAR) {
                attr[2] = lmoon * lmoon / lsun / lsun;
            } else {
                a = 2 * lctr * lmoon;
                b = 2 * lctr * lsun;
                if (a < 1e-9) {
                    attr[2] = lmoon * lmoon / lsun / lsun;
                } else {
                    a = (lctr * lctr + lmoon * lmoon - lsun * lsun) / a;
                    if (a > 1) a = 1;
                    if (a < -1) a = -1;
                    b = (lctr * lctr + lsun * lsun - lmoon * lmoon) / b;
                    if (b > 1) b = 1;
                    if (b < -1) b = -1;
                    a = Math.Acos(a);
                    b = Math.Acos(b);
                    sc1 = a * lmoon * lmoon / 2;
                    sc2 = b * lsun * lsun / 2;
                    sc1 -= (Math.Cos(a) * Math.Sin(a)) * lmoon * lmoon / 2;
                    sc2 -= (Math.Cos(b) * Math.Sin(b)) * lsun * lsun / 2;
                    attr[2] = (sc1 + sc2) * 2 / Math.PI / lsun / lsun;
                }
            }
            attr[7] = dctr;
            /* approximate minimum height for visibility, considering
             * refraction and dip
             * 34.4556': refraction at horizon, from Bennets formulae 
             * 1.75' /Math.Sqrt(geohgt): dip of horizon
             * 0.37' /Math.Sqrt(geohgt): refraction between horizon and observer */
            hmin_appr = -(34.4556 + (1.75 + 0.37) * Math.Sqrt(geohgt)) / 60;
            if (xh[1] + rsun + Math.Abs(hmin_appr) >= 0 && retc != 0)
                retc |= SwissEph.SE_ECL_VISIBLE;	/* eclipse visible */
#if USE_AZ_NAV   //* old */
  attr[4] = swe_degnorm(90 - xh[0]); /* azimuth, from north, clockwise, via east */
#else
            attr[4] = xh[0];	/* azimuth, from south, clockwise, via west */
#endif
            attr[5] = xh[1]; /* height */
            attr[6] = xh[2]; /* height */
            if (ipl == SwissEph.SE_SUN && String.IsNullOrEmpty(starname)) {
                /* magnitude of solar eclipse according to NASA */
                attr[8] = attr[0]; /* fraction of diameter occulted */
                if ((retc & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR)) != 0)
                    attr[8] = attr[1]; /* ratio between diameters of sun and moon */
                /* saros series and member */
                for (i = 0; i < NSAROS_SOLAR; i++) {
                    d = (tjd_ut - saros_data_solar[i].tstart) / SAROS_CYCLE;
                    if (d < 0) continue;
                    j = (int)d;
                    if ((d - j) * SAROS_CYCLE < 2) {
                        attr[9] = (double)saros_data_solar[i].series_no;
                        attr[10] = (double)j + 1;
                        break;
                    }
                    k = j + 1;
                    if ((k - d) * SAROS_CYCLE < 2) {
                        attr[9] = (double)saros_data_solar[i].series_no;
                        attr[10] = (double)k + 1;
                        break;
                    }
                }
                if (i == NSAROS_SOLAR) {
                    attr[9] = attr[10] = -99999999;
                }
            }
            return retc;
        }

        /* When is the next solar eclipse anywhere on earth?
         *
         * input parameters:
         *
         * tjd_start    start time for search (UT)
         * ifl		ephemeris to be used (SEFLG_SWIEPH, etc.)
         * ifltype	eclipse type to be searched (SE_ECL_TOTAL, etc.)
         *              0, if any type of eclipse is wanted
         *
         * return values: 
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_ANNULAR or SE_ECL_PARTIAL
         *              or SE_ECL_ANNULAR_TOTAL
         *              SE_ECL_CENTRAL
         *              SE_ECL_NONCENTRAL
         *
         * tret[0]	time of maximum eclipse
         * tret[1]	time, when eclipse takes place at local apparent noon
         * tret[2]	time of eclipse begin
         * tret[3]	time of eclipse end
         * tret[4]	time of totality begin
         * tret[5]	time of totality end
         * tret[6]	time of center line begin
         * tret[7]	time of center line end
         * tret[8]	time when annular-total eclipse becomes total
         *      	   not implemented so far
         * tret[9]	time when annular-total eclipse becomes annular again
         *      	   not implemented so far
         *         declare as tret[10] at least!
         *
         */
        public Int32 swe_sol_eclipse_when_glob(double tjd_start, Int32 ifl, Int32 ifltype,
             double[] tret, bool backward, ref string serr) {
            int i, j, k, m, n, o, i1 = 0, i2 = 0;
            Int32 retflag = 0, retflag2 = 0;
            double de = 6378.140, a;
            double t, tt, tjd, tjds, dt, dtint, dta, dtb;
            double T, T2, T3, T4, K, M, Mm;
            double E, Ff;
            double[] xs = new double[6], xm = new double[6], ls = new double[6], lm = new double[6];
            double rmoon, rsun; double[] dcore = new double[10];
            double[] dc = new double[3]; double dctr;
            double twohr = 2.0 / 24.0;
            double tenmin = 10.0 / 24.0 / 60.0;
            double dt1 = 0, dt2 = 0;
            double[] geopos = new double[20], attr = new double[20];
            double dtstart, dtdiv;
            double[] xa = new double[6], xb = new double[6];
            int direction = 1;
            bool dont_times = false;
            Int32 iflag, iflagcart;
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_start, ifl, 0, ref serr);
            iflag = SwissEph.SEFLG_EQUATORIAL | ifl;
            iflagcart = iflag | SwissEph.SEFLG_XYZ;
            if (ifltype == (SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_CENTRAL)) {
                serr = "central partial eclipses do not exist";
                return SwissEph.ERR;
            }
            if (ifltype == (SwissEph.SE_ECL_ANNULAR_TOTAL | SwissEph.SE_ECL_NONCENTRAL))
            {
                serr = "non-central hybrid (annular-total) eclipses do not exist";
                return SwissEph.ERR;
            }
            if (ifltype == 0)
                ifltype = SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_PARTIAL
                       | SwissEph.SE_ECL_ANNULAR_TOTAL | SwissEph.SE_ECL_NONCENTRAL | SwissEph.SE_ECL_CENTRAL;
            if (ifltype == SwissEph.SE_ECL_TOTAL || ifltype == SwissEph.SE_ECL_ANNULAR || ifltype == SwissEph.SE_ECL_ANNULAR_TOTAL)
                ifltype |= (SwissEph.SE_ECL_NONCENTRAL | SwissEph.SE_ECL_CENTRAL);
            if (ifltype == SwissEph.SE_ECL_PARTIAL)
                ifltype |= SwissEph.SE_ECL_NONCENTRAL;
            if (backward)
                direction = -1;
            K = (int)((tjd_start - Sweph.J2000) / 365.2425 * 12.3685);
            K -= direction;
        next_try:
            retflag = 0;
            dont_times = false;
            for (i = 0; i <= 9; i++)
                tret[i] = 0;
            T = K / 1236.85;
            T2 = T * T; T3 = T2 * T; T4 = T3 * T;
            Ff = SE.swe_degnorm(160.7108 + 390.67050274 * K
                         - 0.0016341 * T2
                         - 0.00000227 * T3
                         + 0.000000011 * T4);
            if (Ff > 180)
                Ff -= 180;
            if (Ff > 21 && Ff < 159) { 	/* no eclipse possible */
                K += direction;
                goto next_try;
            }
            /* approximate time of geocentric maximum eclipse
             * formula from Meeus, German, p. 381 */
            tjd = 2451550.09765 + 29.530588853 * K
                                + 0.0001337 * T2
                                - 0.000000150 * T3
                                + 0.00000000073 * T4;
            M = SE.swe_degnorm(2.5534 + 29.10535669 * K
                                - 0.0000218 * T2
                                - 0.00000011 * T3);
            Mm = SE.swe_degnorm(201.5643 + 385.81693528 * K
                                + 0.1017438 * T2
                                + 0.00001239 * T3
                                + 0.000000058 * T4);
            E = 1 - 0.002516 * T - 0.0000074 * T2;
            M *= SwissEph.DEGTORAD;
            Mm *= SwissEph.DEGTORAD;
            tjd = tjd - 0.4075 * Math.Sin(Mm)
                      + 0.1721 * E * Math.Sin(M);
            /* 
             * time of maximum eclipse (if eclipse) = 
             * minimum geocentric angle between sun and moon edges.
             * After this time has been determined, check
             * whether or not an eclipse is taking place with
             * the functions eclipse_where() and _how().
             */
            dtstart = 1;
            if (tjd < 2000000 || tjd > 2500000)
                dtstart = 5;
            dtdiv = 4;
            for (dt = dtstart;
                 dt > 0.0001;
                 dt /= dtdiv) {
                for (i = 0, t = tjd - dt; i <= 2; i++, t += dt) {
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflag, ls, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflagcart, xs, ref  serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    for (m = 0; m < 3; m++) {
                        xa[m] = xs[m] / ls[2];
                        xb[m] = xm[m] / lm[2];
                    }
                    dc[i] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xa, xb)) * SwissEph.RADTODEG;
                    rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
                    rsun = Math.Asin(RSUN / ls[2]) * SwissEph.RADTODEG;
                    dc[i] -= (rmoon + rsun);
                }
                find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dctr);
                tjd += dtint + dt;
            }
            tjds = tjd - SE.swe_deltat_ex(tjd, ifl, ref serr);
            tjds = tjd - SE.swe_deltat_ex(tjds, ifl, ref serr);
            tjds = tjd = tjd - SE.swe_deltat_ex(tjds, ifl, ref serr);
            if ((retflag = eclipse_where(tjd, SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                return retflag;
            retflag2 = retflag;
            /* in extreme cases _where() returns no eclipse, where there is
             * actually a very small one, therefore call _how() with the
             * coordinates returned by _where(): */
            if ((retflag2 = eclipse_how(tjd, SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], 0, attr, ref serr)) == SwissEph.ERR)
                return retflag2;
            if (retflag2 == 0) {
                K += direction;
                goto next_try;
            }
            tret[0] = tjd;
            if ((backward && tret[0] >= tjd_start - 0.0001)
              || (!backward && tret[0] <= tjd_start + 0.0001)) {
                K += direction;
                goto next_try;
            }
            /*
             * eclipse type, SE_ECL_TOTAL, _ANNULAR, etc.
             * SE_ECL_ANNULAR_TOTAL will be discovered later
             */
            if ((retflag = eclipse_where(tjd, SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                return retflag;
            if (retflag == 0) {	/* can happen with extremely small percentage */
                retflag = SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_NONCENTRAL;
                tret[4] = tret[5] = tjd;	/* fix this ???? */
                dont_times = true;
            }
            /* 
             * check whether or not eclipse type found is wanted
             */
            /* non central eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_NONCENTRAL) && (retflag & SwissEph.SE_ECL_NONCENTRAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* central eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_CENTRAL) && (retflag & SwissEph.SE_ECL_CENTRAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* non annular eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_ANNULAR) && (retflag & SwissEph.SE_ECL_ANNULAR) != 0) {
                K += direction;
                goto next_try;
            }
            /* non partial eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_PARTIAL) && (retflag & SwissEph.SE_ECL_PARTIAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* annular-total eclipse will be discovered later */
            if (0 == (ifltype & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR_TOTAL)) && (retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                K += direction;
                goto next_try;
            }
            if (dont_times)
                goto end_search_global;
            /* 
             * n = 0: times of eclipse begin and end 
             * n = 1: times of totality begin and end 
             * n = 2: times of center line begin and end 
             */
            if ((retflag & SwissEph.SE_ECL_PARTIAL) != 0)
                o = 0;
            else if ((retflag & SwissEph.SE_ECL_NONCENTRAL) != 0)
                o = 1;
            else
                o = 2;
            dta = twohr;
            dtb = tenmin / 3.0;
            for (n = 0; n <= o; n++) {
                if (n == 0) {
                    /*dc[1] = dcore[3] / 2 + de - dcore[1];*/
                    i1 = 2; i2 = 3;
                } else if (n == 1) {
                    if ((retflag & SwissEph.SE_ECL_PARTIAL) != 0)
                        continue;
                    i1 = 4; i2 = 5;
                } else if (n == 2) {
                    if ((retflag & SwissEph.SE_ECL_NONCENTRAL) != 0)
                        continue;
                    i1 = 6; i2 = 7;
                }
                for (i = 0, t = tjd - dta; i <= 2; i += 1, t += dta) {
                    if ((retflag2 = eclipse_where(t, SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                        return retflag2;
                    if (n == 0)
                        dc[i] = dcore[4] / 2 + de / dcore[5] - dcore[2];
                    else if (n == 1)
                        dc[i] = Math.Abs(dcore[3]) / 2 + de / dcore[6] - dcore[2];
                    else if (n == 2)
                        dc[i] = de / dcore[6] - dcore[2];
                }
                find_zero(dc[0], dc[1], dc[2], dta, out dt1, out dt2);
                tret[i1] = tjd + dt1 + dta;
                tret[i2] = tjd + dt2 + dta;
                for (m = 0, dt = dtb; m < 3; m++, dt /= 3) {
                    for (j = i1; j <= i2; j += (i2 - i1)) {
                        for (i = 0, t = tret[j] - dt; i < 2; i++, t += dt) {
                            if ((retflag2 = eclipse_where(t, SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                                return retflag2;
                            if (n == 0)
                                dc[i] = dcore[4] / 2 + de / dcore[5] - dcore[2];
                            else if (n == 1)
                                dc[i] = Math.Abs(dcore[3]) / 2 + de / dcore[6] - dcore[2];
                            else if (n == 2)
                                dc[i] = de / dcore[6] - dcore[2];
                        }
                        dt1 = dc[1] / ((dc[1] - dc[0]) / dt);
                        tret[j] -= dt1;
                    }
                }
            }
            /*
             * annular-total eclipses
             */
            if ((retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                if ((retflag2 = eclipse_where(tret[0], SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[0] = dcore[0];
                if ((retflag2 = eclipse_where(tret[4], SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[1] = dcore[0];
                if ((retflag2 = eclipse_where(tret[5], SwissEph.SE_SUN, null, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[2] = dcore[0];
                /* the maximum is always total, and there is either one or
                 * to times before and after, when the core shadow becomes
                 * zero and totality changes into annularity or vice versa.
                 */
                if (dc[0] * dc[1] < 0 || dc[0] * dc[2] < 0) {
                    retflag |= SwissEph.SE_ECL_ANNULAR_TOTAL;
                    retflag &= ~SwissEph.SE_ECL_TOTAL;
                }
            }
            /* if eclipse is given but not wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_TOTAL) && (retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* if annular_total eclipse is given but not wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_ANNULAR_TOTAL) && (retflag & SwissEph.SE_ECL_ANNULAR_TOTAL) != 0) {
                K += direction;
                goto next_try;
            }
            /*
             * time of maximum eclipse at local apparent noon
             */
            /* first, find out, if there is a solar transit 
             * between begin and end of eclipse */
            k = 2;
            for (i = 0; i < 2; i++) {
                j = i + k;
                tt = tret[j] + SE.swe_deltat_ex(tret[j], ifl, ref serr);
                if (SE.swe_calc(tt, SwissEph.SE_SUN, iflag, ls, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(tt, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dc[i] = SE.swe_degnorm(ls[0] - lm[0]);
                if (dc[i] > 180)
                    dc[i] -= 360;
            }
            if (dc[0] * dc[1] >= 0)	/* no transit */
                tret[1] = 0;
            else {
                tjd = tjds;
                dt = 0.1;
                dt1 = (tret[3] - tret[2]) / 2.0;
                if (dt1 < dt)
                    dt = dt1 / 2.0;
                for (j = 0;
                    dt > 0.01;
                    j++, dt /= 3) {
                    for (i = 0, t = tjd; i <= 1; i++, t -= dt) {
                        tt = t + SE.swe_deltat_ex(t, ifl, ref serr);
                        if (SE.swe_calc(tt, SwissEph.SE_SUN, iflag, ls, ref  serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        if (SE.swe_calc(tt, SwissEph.SE_MOON, iflag, lm, ref  serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        dc[i] = SE.swe_degnorm(ls[0] - lm[0]);
                        if (dc[i] > 180)
                            dc[i] -= 360;
                        if (dc[i] > 180)
                            dc[i] -= 360;
                    }
                    a = (dc[1] - dc[0]) / dt;
                    if (a < 1e-10)
                        break;
                    dt1 = dc[0] / a;
                    tjd += dt1;
                }
                tret[1] = tjd;
            }
        end_search_global:
            return retflag;
            /*
             * the time of maximum occultation is practically identical
             * with the time of maximum core shadow diameter.
             *
             * the time, when duration of totality is maximal,
             * is not an interesting computation either. Near the maximum
             * occulation, the time of totality can be the same by 
             * a second for hundreds of kilometers (for 10 minutes 
             * or more).
             *  
             * for annular eclipses the maximum duration is close to the
             * beginning and the end of the center lines, where is also
             * the minimum of core shadow diameter.
             */
        }

        /* When is the next lunar occultation anywhere on earth?
         * This function also finds solar eclipses, but is less efficient
         * than swe_sol_eclipse_when_glob().
         * 
         * input parameters:
         *
         * tjd_start          start time for search (UT)
         * ipl                planet number of occulted body
         * starname           name of occulted star. Must be NULL or "", if a planetary
         *                    occultation is to be calculated. For the use of this
         *                    field, also see swe_fixstar().
         * ifl		      ephemeris to be used (SEFLG_SWIEPH, etc.)
         *          	      ephemeris flag. 
         *
         * ifltype	      eclipse type to be searched (SE_ECL_TOTAL, etc.)
         *                    0, if any type of eclipse is wanted
         *                    this functionality also works with occultations
         *
         * backward	      if 1, causes search backward in time
         *
         *                    If you want to have only one conjunction 
         *                    of the moon with the body tested, add the following flag:
         *                    backward |= SE_ECL_ONE_TRY. If this flag is not set, 
         *                    the function will search for an occultation until it
         *                    finds one. For bodies with ecliptical latitudes > 5,
         *                    the function may search successlessly until it reaches
         *                    the end of the ephemeris.
         *                    (Note: we do not add SE_ECL_ONE_TRY to ifl, because 
         *                    ifl may contain SEFLG_TOPOCTR (=SE_ECL_ONE_TRY) from
         *		      the parameter iflag of swe_calc() etc. Although the
         *                    topocentric flag is irrelevant here, it might cause 
         * 		      confusion.)
         *
         * return values: 
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_ANNULAR or SE_ECL_PARTIAL
         *              or SE_ECL_ANNULAR_TOTAL
         *              SE_ECL_CENTRAL
         *              SE_ECL_NONCENTRAL
         *
         * tret[0]	time of maximum eclipse
         * tret[1]	time, when eclipse takes place at local apparent noon
         * tret[2]	time of eclipse begin
         * tret[3]	time of eclipse end
         * tret[4]	time of totality begin
         * tret[5]	time of totality end
         * tret[6]	time of center line begin
         * tret[7]	time of center line end
         * tret[8]	time when annular-total eclipse becomes total
         *      	   not implemented so far
         * tret[9]	time when annular-total eclipse becomes annular again
         *      	   not implemented so far
         *         declare as tret[10] at least!
         *
         */
        public Int32 swe_lun_occult_when_glob(
             double tjd_start, Int32 ipl, string starname, Int32 ifl, Int32 ifltype,
             double[] tret, bool b_backward, ref string serr) {
            int i, j, k, m, n, o, i1 = 0, i2 = 0;
            Int32 retflag = 0, retflag2 = 0;
            double de = 6378.140, a;
            double t, tt, tjd = 0, tjds, dt, dtint, dta, dtb;
            double drad, dl;
            double[] xs = new double[6], xm = new double[6], ls = new double[6], lm = new double[6];
            double rmoon, rsun; double[] dcore = new double[10];
            double[] dc = new double[20]; double dctr;
            double twohr = 2.0 / 24.0;
            double tenmin = 10.0 / 24.0 / 60.0;
            double dt1 = 0, dt2 = 0, dadd2 = 1;
            double[] geopos = new double[20];
            double dtstart, dtdiv;
            int direction = 1;
            Int32 ifltype2;
            Int32 iflag, iflagcart;
            bool dont_times = false;
            int backward = b_backward ? 1 : 0;
            Int32 one_try = backward & SwissEph.SE_ECL_ONE_TRY;
            if (ipl < 0) ipl = 0;
            /*if (backward & SEI_OCC_FAST)
                dont_times = TRUE; */
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_start, ifl, 0, ref serr);
            iflag = SwissEph.SEFLG_EQUATORIAL | ifl;
            iflagcart = iflag | SwissEph.SEFLG_XYZ;
            backward &= 1;
            /* 
             * initializations 
             */
            if (ifltype == (SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_CENTRAL)) {
                serr = "central partial eclipses do not exist";
                return SwissEph.ERR;
            }
            if (ipl != SwissEph.SE_SUN)
            {
                ifltype2 = (ifltype & ~(SwissEph.SE_ECL_NONCENTRAL | SwissEph.SE_ECL_CENTRAL));
                if (ifltype2 == SwissEph.SE_ECL_ANNULAR || ifltype == SwissEph.SE_ECL_ANNULAR_TOTAL)
                {
                    serr=C.sprintf("annular occulation do not exist for object %d %s\n", ipl, starname);
                    return SwissEph.ERR;
                }
            }
            if (ipl != SwissEph.SE_SUN && (ifltype & (SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL))!=0)
                ifltype &= ~(SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL);
            if (ifltype == 0)
            {
                ifltype = SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_NONCENTRAL | SwissEph.SE_ECL_CENTRAL;
                if (ipl == SwissEph.SE_SUN)
                    ifltype |= (SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL);
            }
            if (ifltype != 0 & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL) != 0)
                ifltype |= (SwissEph.SE_ECL_NONCENTRAL | SwissEph.SE_ECL_CENTRAL);
            if ((ifltype & SwissEph.SE_ECL_PARTIAL)!=0)
                ifltype |= SwissEph.SE_ECL_NONCENTRAL;
            retflag = 0;
            for (i = 0; i <= 9; i++)
                tret[i] = 0;
            if (backward != 0)
                direction = -1;
            t = tjd_start;
            tjd = t;
        next_try:
            if (calc_planet_star(t, ipl, starname, ifl, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /* fixed stars with an ecliptic latitude > 7  or < -7 cannot have 
             * an occultation. Even lunar parallax andd proper motion of star
             * will never allow it. */
            if (Math.Abs(ls[1]) > 7 && !String.IsNullOrWhiteSpace(starname))
            {
                serr = C.sprintf("occultation never occurs: star %s has ecl. lat. %.1f", starname, ls[1]);
                return SwissEph.ERR;
            }
            if (SE.swe_calc(t, SwissEph.SE_MOON, ifl, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            dl = SE.swe_degnorm(ls[0] - lm[0]);
            if (direction < 0)
                dl -= 360;
            /* get rough conjunction in ecliptic longitude */
            while (Math.Abs(dl) > 0.1)
            {
                t += dl / 13;
                if (calc_planet_star(t, ipl, starname, ifl, ls, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(t, SwissEph.SE_MOON, ifl, lm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dl = SE.swe_degnorm(ls[0] - lm[0]);
                if (dl > 180) dl -= 360;
            }
            tjd = t;
            /* difference in latitude too big for an occultation */
            drad = Math.Abs(ls[1] - lm[1]);
            if (drad > 2)
            {
                if (one_try != 0)
                {
                    tret[0] = t + direction; /* return a date suitable for next try */
                    return 0;
                }
                t += direction * 20;
                tjd = t;
                goto next_try;
            }
            /*
             * radius of planet disk in AU
             */
            if (!String.IsNullOrEmpty(starname))
                drad = 0;
            else if (ipl < Sweph.NDIAM)
                drad = Sweph.pla_diam[ipl] / 2 / Sweph.AUNIT;
            else if (ipl > SwissEph.SE_AST_OFFSET)
                drad = SE.Sweph.swed.ast_diam / 2 * 1000 / Sweph.AUNIT; /* km -> m -> AU */
            else
                drad = 0;
            /* 
             * time of maximum eclipse (if eclipse) = 
             * minimum geocentric angle between sun and moon edges.
             * After this time has been determined, check
             * whether or not an eclipse is taking place with
             * the functions eclipse_where() and _how().
             */
            dtstart = dadd2; /* originally 1 */
            dtdiv = 3;
            for (dt = dtstart;
                 dt > 0.0001;
                 dt /= dtdiv) {
                for (i = 0, t = tjd - dt; i <= 2; i++, t += dt) {
                    if (calc_planet_star(t, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (calc_planet_star(t, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    dc[i] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xs, xm)) * SwissEph.RADTODEG;
                    rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
                    rsun = Math.Asin(drad / ls[2]) * SwissEph.RADTODEG;
                    dc[i] -= (rmoon + rsun);
                }
                find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dctr);
                tjd += dtint + dt;
            }
            tjd -= SE.swe_deltat_ex(tjd, ifl, ref serr);
            tjds = tjd;
            if ((retflag = eclipse_where(tjd, ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                return retflag;
            retflag2 = retflag;
            /* in extreme cases _where() returns no eclipse, where there is
             * actually a very small one, therefore call _how() with the
             * coordinates returned by _where(): */
            /* if ((retflag2 = eclipse_how(tjd, ipl, starname, ifl, geopos[0], geopos[1], 0, attr, serr)) == ERR)
              return retflag2; */
            if (retflag2 == 0) {
                /* only one try! */
                /* if (one_try && ((direction == 1 && tjd > tjd_start) || (direction == -1 && tjd < tjd_start))) {*/
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                tjd = t;
                goto next_try;
            }
            tret[0] = tjd;
            /* should not happen anymore Version 2.01 */
            if ((backward != 0 && tret[0] >= tjd_start - 0.0001)
              || (0 == backward && tret[0] <= tjd_start + 0.0001)) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                tjd = t;
                goto next_try;
            }
            /*
             * eclipse type, SE_ECL_TOTAL, _ANNULAR, etc.
             * SE_ECL_ANNULAR_TOTAL will be discovered later
             */
            if ((retflag = eclipse_where(tjd, ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                return retflag;
            if (retflag == 0) {	/* can happen with extremely small percentage */
                retflag = SwissEph.SE_ECL_PARTIAL | SwissEph.SE_ECL_NONCENTRAL;
                tret[4] = tret[5] = tjd;	/* fix this ???? */
                dont_times = true;
            }
            /* 
             * check whether or not eclipse type found is wanted
             */
            /* non central eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_NONCENTRAL) && (retflag & SwissEph.SE_ECL_NONCENTRAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /* central eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_CENTRAL) && (retflag & SwissEph.SE_ECL_CENTRAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /* non annular eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_ANNULAR) && (retflag & SwissEph.SE_ECL_ANNULAR) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /* non partial eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_PARTIAL) && (retflag & SwissEph.SE_ECL_PARTIAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /* annular-total eclipse will be discovered later */
            if (0 == (ifltype & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR_TOTAL)) && (retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            if (dont_times)
                goto end_search_global;
            /* 
             * n = 0: times of eclipse begin and end 
             * n = 1: times of totality begin and end 
             * n = 2: times of center line begin and end 
             */
            if ((retflag & SwissEph.SE_ECL_PARTIAL) != 0)
                o = 0;
            else if ((retflag & SwissEph.SE_ECL_NONCENTRAL) != 0)
                o = 1;
            else
                o = 2;
            dta = twohr;
            dtb = tenmin;
            for (n = 0; n <= o; n++) {
                if (n == 0) {
                    /*dc[1] = dcore[3] / 2 + de - dcore[1];*/
                    i1 = 2; i2 = 3;
                } else if (n == 1) {
                    if ((retflag & SwissEph.SE_ECL_PARTIAL) != 0)
                        continue;
                    i1 = 4; i2 = 5;
                } else if (n == 2) {
                    if ((retflag & SwissEph.SE_ECL_NONCENTRAL) != 0)
                        continue;
                    i1 = 6; i2 = 7;
                }
                for (i = 0, t = tjd - dta; i <= 2; i += 1, t += dta) {
                    if ((retflag2 = eclipse_where(t, ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                        return retflag2;
                    if (n == 0)
                        dc[i] = dcore[4] / 2 + de / dcore[5] - dcore[2];
                    else if (n == 1)
                        dc[i] = Math.Abs(dcore[3]) / 2 + de / dcore[6] - dcore[2];
                    else if (n == 2)
                        dc[i] = de / dcore[6] - dcore[2];
                }
                find_zero(dc[0], dc[1], dc[2], dta, out dt1, out dt2);
                tret[i1] = tjd + dt1 + dta;
                tret[i2] = tjd + dt2 + dta;
                for (m = 0, dt = dtb; m < 3; m++, dt /= 3) {
                    for (j = i1; j <= i2; j += (i2 - i1)) {
                        for (i = 0, t = tret[j] - dt; i < 2; i++, t += dt) {
                            if ((retflag2 = eclipse_where(t, ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                                return retflag2;
                            if (n == 0)
                                dc[i] = dcore[4] / 2 + de / dcore[5] - dcore[2];
                            else if (n == 1)
                                dc[i] = Math.Abs(dcore[3]) / 2 + de / dcore[6] - dcore[2];
                            else if (n == 2)
                                dc[i] = de / dcore[6] - dcore[2];
                        }
                        dt1 = dc[1] / ((dc[1] - dc[0]) / dt);
                        tret[j] -= dt1;
                    }
                }
            }
            /*
             * annular-total eclipses
             */
            if ((retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                if ((retflag2 = eclipse_where(tret[0], ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[0] = dcore[0];
                if ((retflag2 = eclipse_where(tret[4], ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[1] = dcore[0];
                if ((retflag2 = eclipse_where(tret[5], ipl, starname, ifl, geopos, dcore, ref serr)) == SwissEph.ERR)
                    return retflag2;
                dc[2] = dcore[0];
                /* the maximum is always total, and there is either one or
                 * to times before and after, when the core shadow becomes
                 * zero and totality changes into annularity or vice versa.
                 */
                if (dc[0] * dc[1] < 0 || dc[0] * dc[2] < 0) {
                    retflag |= SwissEph.SE_ECL_ANNULAR_TOTAL;
                    retflag &= ~SwissEph.SE_ECL_TOTAL;
                }
            }
            /* if eclipse is given but not wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_TOTAL) && (retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /* if annular_total eclipse is given but not wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_ANNULAR_TOTAL) && (retflag & SwissEph.SE_ECL_ANNULAR_TOTAL) != 0) {
                /*t= tjd + direction * dadd;*/
                t = tjd + direction * 20;
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                tjd = t;
                goto next_try;
            }
            /*
             * time of maximum eclipse at local apparent noon
             */
            /* first, find out, if there is a solar transit 
             * between begin and end of eclipse */
            k = 2;
            for (i = 0; i < 2; i++) {
                j = i + k;
                tt = tret[j] + SE.swe_deltat_ex(tret[j], ifl, ref serr);
                if (calc_planet_star(tt, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(tt, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dc[i] = SE.swe_degnorm(ls[0] - lm[0]);
                if (dc[i] > 180)
                    dc[i] -= 360;
            }
            if (dc[0] * dc[1] >= 0)	/* no transit */
                tret[1] = 0;
            else {
                tjd = tjds;
                dt = 0.1;
                dt1 = (tret[3] - tret[2]) / 2.0;
                if (dt1 < dt)
                    dt = dt1 / 2.0;
                for (j = 0;
                    dt > 0.01;
                    j++, dt /= 3) {
                    for (i = 0, t = tjd; i <= 1; i++, t -= dt) {
                        tt = t + SE.swe_deltat_ex(t, ifl, ref serr);
                        if (calc_planet_star(tt, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        if (SE.swe_calc(tt, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        dc[i] = SE.swe_degnorm(ls[0] - lm[0]);
                        if (dc[i] > 180)
                            dc[i] -= 360;
                        if (dc[i] > 180)
                            dc[i] -= 360;
                    }
                    a = (dc[1] - dc[0]) / dt;
                    if (a < 1e-10)
                        break;
                    dt1 = dc[0] / a;
                    tjd += dt1;
                }
                tret[1] = tjd;
            }
        end_search_global:
            return retflag;
            /*
             * the time of maximum occultation is practically identical
             * with the time of maximum core shadow diameter.
             *
             * the time, when duration of totality is maximal,
             * is not an interesting computation either. Near the maximum
             * occulation, the time of totality can be the same by 
             * a second for hundreds of kilometers (for 10 minutes 
             * or more).
             *  
             * for annular eclipses the maximum duration is close to the
             * beginning and the end of the center lines, where is also
             * the minimum of core shadow diameter.
             */
        }

        /* When is the next solar eclipse at a given geographical position?
         * Note the uncertainty of Delta T for the remote past and for
         * the future.
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_ANNULAR or SE_ECL_PARTIAL
         *              SE_ECL_VISIBLE, 
         *              SE_ECL_MAX_VISIBLE, 
         *              SE_ECL_1ST_VISIBLE, SE_ECL_2ND_VISIBLE
         *              SE_ECL_3ST_VISIBLE, SE_ECL_4ND_VISIBLE
         *
         * tret[0]	time of maximum eclipse
         * tret[1]	time of first contact
         * tret[2]	time of second contact
         * tret[3]	time of third contact
         * tret[4]	time of forth contact
         * tret[5]	time of sun rise between first and forth contact
         * tret[6]	time of sun set beween first and forth contact
         *
         * attr[0]	fraction of solar diameter covered by moon;
         *              with total/annular eclipses, it results in magnitude acc. to IMCCE.
         * attr[1]	ratio of lunar diameter to solar one
         * attr[2]	fraction of solar disc covered by moon (obscuration)
         * attr[3]      diameter of core shadow in km
         * attr[4]	azimuth of sun at tjd
         * attr[5]	true altitude of sun above horizon at tjd
         * attr[6]	apparent altitude of sun above horizon at tjd
         * attr[7]	elongation of moon in degrees
         * attr[8]	magnitude acc. to NASA;
         *              = attr[0] for partial and attr[1] for annular and total eclipses
         * attr[9]	saros series number
         * attr[10]	saros series member number
         *         declare as attr[20] at least !
         */
        public Int32 swe_sol_eclipse_when_loc(double tjd_start, Int32 ifl,
             double[] geopos, double[] tret, double[] attr, bool backward, ref string serr) {
            Int32 retflag = 0, retflag2 = 0;
            double[] geopos2 = new double[20], dcore = new double[10];
            if (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr = C.sprintf("location for eclipses must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_start, ifl, 0, ref serr);
            if ((retflag = eclipse_when_loc(tjd_start, ifl, geopos, tret, attr, backward, ref serr)) <= 0)
                return retflag;
            /* 
             * diameter of core shadow
             */
            if ((retflag2 = eclipse_where(tret[0], SwissEph.SE_SUN, null, ifl, geopos2, dcore, ref serr)) == SwissEph.ERR)
                return retflag2;
            retflag |= (retflag2 & SwissEph.SE_ECL_NONCENTRAL);
            attr[3] = dcore[0];
            return retflag;
        }

        /* When is the next solar eclipse at a given geographical position?
         * Note the uncertainty of Delta T for the remote past and for
         * the future.
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_ANNULAR or SE_ECL_PARTIAL
         *              SE_ECL_VISIBLE, 
         *              SE_ECL_MAX_VISIBLE, 
         *              SE_ECL_1ST_VISIBLE, SE_ECL_2ND_VISIBLE
         *              SE_ECL_3ST_VISIBLE, SE_ECL_4ND_VISIBLE
         *              SE_ECL_OCC_BEG_DAYLIGHT, SE_ECL_OCC_END_DAYLIGHT
         * The latter two indicate that the beginning or end of the occultation takes
         * place during the day. If Venus is occulted, it may be observable with the
         * naked eye; if other objects, it may be observable with telescopes.
         *
         * int32 ipl          planet number of occulted body
         * char* starname     name of occulted star. Must be NULL or "", if a planetary
         *                    occultation is to be calculated. For the use of this
         *                    field, also see swe_fixstar().
         * int32 ifl	      ephemeris flag. If you want to have only one conjunction 
         *                    of the moon with the body tested, add the following flag:
         *                    backward |= SE_ECL_ONE_TRY. If this flag is not set, 
         *                    the function will search for an occultation until it
         *                    finds one. For bodies with ecliptical latitudes > 5,
         *                    the function may search unsuccessfully until it reaches
         *                    the end of the ephemeris.
         *
         * for all other parameters, see function swe_sol_eclipse_when_loc().
         */
        public Int32 swe_lun_occult_when_loc(double tjd_start, Int32 ipl, string starname, Int32 ifl,
             double[] geopos, double[] tret, double[] attr, bool backward, ref string serr) {
            Int32 retflag = 0, retflag2 = 0;
            double[] geopos2 = new double[20], dcore = new double[10];
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr = C.sprintf("location for occultations must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            if (ipl < 0) ipl = 0;
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_start, ifl, 0, ref serr);
            if ((retflag = occult_when_loc(tjd_start, ipl, starname, ifl, geopos, tret, attr, backward ? 1 : 0, ref serr)) <= 0)
                return retflag;
            /* 
             * diameter of core shadow
             */
            if ((retflag2 = eclipse_where(tret[0], ipl, starname, ifl, geopos2, dcore, ref serr)) == SwissEph.ERR)
                return retflag2;
            retflag |= (retflag2 & SwissEph.SE_ECL_NONCENTRAL);
            attr[3] = dcore[0];
            return retflag;
        }

        Int32 eclipse_when_loc(double tjd_start, Int32 ifl, double[] geopos, double[] tret, double[] attr, bool backward, ref string serr) {
            int i, j, k, m;
            Int32 retflag = 0, retc;
            double t, tjd, dt, dtint, K, T, T2, T3, T4, F, M, Mm;
            double tjdr = 0, tjds = 0;
            double E, Ff, A1, Om;
            double[] xs = new double[6], xm = new double[6], ls = new double[6], lm = new double[6], x1 = new double[6], x2 = new double[6]; double dm, ds;
            double rmoon, rsun, rsplusrm, rsminusrm;
            double[] dc = new double[3]; double dctr, dctrmin;
            double twomin = 2.0 / 24.0 / 60.0;
            double tensec = 10.0 / 24.0 / 60.0 / 60.0;
            double twohr = 2.0 / 24.0;
            double tenmin = 10.0 / 24.0 / 60.0;
            double dt1 = 0, dt2 = 0, dtdiv, dtstart;
            Int32 iflag = SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_TOPOCTR | ifl;
            Int32 iflagcart = iflag | SwissEph.SEFLG_XYZ;
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            K = (int)((tjd_start - Sweph.J2000) / 365.2425 * 12.3685);
            if (backward)
                K++;
            else
                K--;
        next_try:
            T = K / 1236.85;
            T2 = T * T; T3 = T2 * T; T4 = T3 * T;
            Ff = F = SE.swe_degnorm(160.7108 + 390.67050274 * K
                         - 0.0016341 * T2
                         - 0.00000227 * T3
                         + 0.000000011 * T4);
            if (Ff > 180)
                Ff -= 180;
            if (Ff > 21 && Ff < 159) { 	/* no eclipse possible */
                if (backward)
                    K--;
                else
                    K++;
                goto next_try;
            }
            /* approximate time of geocentric maximum eclipse.
             * formula from Meeus, German, p. 381 */
            tjd = 2451550.09765 + 29.530588853 * K
                                + 0.0001337 * T2
                                - 0.000000150 * T3
                                + 0.00000000073 * T4;
            M = SE.swe_degnorm(2.5534 + 29.10535669 * K
                                - 0.0000218 * T2
                                - 0.00000011 * T3);
            Mm = SE.swe_degnorm(201.5643 + 385.81693528 * K
                                + 0.1017438 * T2
                                + 0.00001239 * T3
                                + 0.000000058 * T4);
            Om = SE.swe_degnorm(124.7746 - 1.56375580 * K
                                + 0.0020691 * T2
                                + 0.00000215 * T3);
            E = 1 - 0.002516 * T - 0.0000074 * T2;
            A1 = SE.swe_degnorm(299.77 + 0.107408 * K - 0.009173 * T2);
            M *= SwissEph.DEGTORAD;
            Mm *= SwissEph.DEGTORAD;
            F *= SwissEph.DEGTORAD;
            Om *= SwissEph.DEGTORAD;
            A1 *= SwissEph.DEGTORAD;
            tjd = tjd - 0.4075 * Math.Sin(Mm)
                      + 0.1721 * E * Math.Sin(M);
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            dtdiv = 2;
            dtstart = 0.5;
            if (tjd < 1900000 || tjd > 2500000)	/* because above formula is not good (delta t?) */
                dtstart = 2;
            for (dt = dtstart;
                 dt > 0.00001;
                 dt /= dtdiv) {
                if (dt < 0.1)
                    dtdiv = 3;
                for (i = 0, t = tjd - dt; i <= 2; i++, t += dt) {
                    /* this takes some time, but is necessary to avoid
                     * missing an eclipse */
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflag, ls, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                    ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                    for (k = 0; k < 3; k++) {
                        x1[k] = xs[k] / ds /*ls[2]*/;
                        x2[k] = xm[k] / dm /*lm[2]*/;
                    }
                    dc[i] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                }
                find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dctr);
                tjd += dtint + dt;
            }
            if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflagcart, xs, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflag, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xs, xm)) * SwissEph.RADTODEG;
            rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
            rsun = Math.Asin(RSUN / ls[2]) * SwissEph.RADTODEG;
            rsplusrm = rsun + rmoon;
            rsminusrm = rsun - rmoon;
            if (dctr > rsplusrm) {
                if (backward)
                    K--;
                else
                    K++;
                goto next_try;
            }
            tret[0] = tjd - SE.swe_deltat_ex(tjd, ifl, ref serr);
            tret[0] = tjd - SE.swe_deltat_ex(tret[0], ifl, ref serr); /* these two lines are an iteration! */
            if ((backward && tret[0] >= tjd_start - 0.0001)
              || (!backward && tret[0] <= tjd_start + 0.0001)) {
                if (backward)
                    K--;
                else
                    K++;
                goto next_try;
            }
            if (dctr < rsminusrm)
                retflag = SwissEph.SE_ECL_ANNULAR;
            else if (dctr < Math.Abs(rsminusrm))
                retflag = SwissEph.SE_ECL_TOTAL;
            else if (dctr <= rsplusrm)
                retflag = SwissEph.SE_ECL_PARTIAL;
            dctrmin = dctr;
            /* contacts 2 and 3 */
            if (dctr > Math.Abs(rsminusrm))  /* partial, no 2nd and 3rd contact */
                tret[2] = tret[3] = 0;
            else {
                dc[1] = Math.Abs(rsminusrm) - dctrmin;
                for (i = 0, t = tjd - twomin; i <= 2; i += 2, t = tjd + twomin) {
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                    ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                    rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                    rmoon *= 0.99916; /* gives better accuracy for 2nd/3rd contacts */
                    rsun = Math.Asin(RSUN / ds) * SwissEph.RADTODEG;
                    rsminusrm = rsun - rmoon;
                    for (k = 0; k < 3; k++) {
                        x1[k] = xs[k] / ds /*ls[2]*/;
                        x2[k] = xm[k] / dm /*lm[2]*/;
                    }
                    dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                    dc[i] = Math.Abs(rsminusrm) - dctr;
                }
                find_zero(dc[0], dc[1], dc[2], twomin, out dt1, out dt2);
                tret[2] = tjd + dt1 + twomin;
                tret[3] = tjd + dt2 + twomin;
                for (m = 0, dt = tensec; m < 2; m++, dt /= 10) {
                    for (j = 2; j <= 3; j++) {
                        if (SE.swe_calc(tret[j], SwissEph.SE_SUN, iflagcart | SwissEph.SEFLG_SPEED, xs, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        if (SE.swe_calc(tret[j], SwissEph.SE_MOON, iflagcart | SwissEph.SEFLG_SPEED, xm, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        for (i = 0; i < 2; i++) {
                            if (i == 1) {
                                for (k = 0; k < 3; k++) {
                                    xs[k] -= xs[k + 3] * dt;
                                    xm[k] -= xm[k + 3] * dt;
                                }
                            }
                            dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                            ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                            rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                            rmoon *= 0.99916; /* gives better accuracy for 2nd/3rd contacts */
                            rsun = Math.Asin(RSUN / ds) * SwissEph.RADTODEG;
                            rsminusrm = rsun - rmoon;
                            for (k = 0; k < 3; k++) {
                                x1[k] = xs[k] / ds /*ls[2]*/;
                                x2[k] = xm[k] / dm /*lm[2]*/;
                            }
                            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                            dc[i] = Math.Abs(rsminusrm) - dctr;
                        }
                        dt1 = -dc[0] / ((dc[0] - dc[1]) / dt);
                        tret[j] += dt1;
                    }
                }
                tret[2] -= SE.swe_deltat_ex(tret[2], ifl, ref serr);
                tret[3] -= SE.swe_deltat_ex(tret[3], ifl, ref serr);
            }
            /* contacts 1 and 4 */
            dc[1] = rsplusrm - dctrmin;
            for (i = 0, t = tjd - twohr; i <= 2; i += 2, t = tjd + twohr) {
                if (SE.swe_calc(t, SwissEph.SE_SUN, iflagcart, xs, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                rsun = Math.Asin(RSUN / ds) * SwissEph.RADTODEG;
                rsplusrm = rsun + rmoon;
                for (k = 0; k < 3; k++) {
                    x1[k] = xs[k] / ds /*ls[2]*/;
                    x2[k] = xm[k] / dm /*lm[2]*/;
                }
                dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                dc[i] = rsplusrm - dctr;
            }
            find_zero(dc[0], dc[1], dc[2], twohr, out dt1, out dt2);
            tret[1] = tjd + dt1 + twohr;
            tret[4] = tjd + dt2 + twohr;
            for (m = 0, dt = tenmin; m < 3; m++, dt /= 10) {
                for (j = 1; j <= 4; j += 3) {
                    if (SE.swe_calc(tret[j], SwissEph.SE_SUN, iflagcart | SwissEph.SEFLG_SPEED, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(tret[j], SwissEph.SE_MOON, iflagcart | SwissEph.SEFLG_SPEED, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    for (i = 0; i < 2; i++) {
                        if (i == 1) {
                            for (k = 0; k < 3; k++) {
                                xs[k] -= xs[k + 3] * dt;
                                xm[k] -= xm[k + 3] * dt;
                            }
                        }
                        dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                        ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                        rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                        rsun = Math.Asin(RSUN / ds) * SwissEph.RADTODEG;
                        rsplusrm = rsun + rmoon;
                        for (k = 0; k < 3; k++) {
                            x1[k] = xs[k] / ds /*ls[2]*/;
                            x2[k] = xm[k] / dm /*lm[2]*/;
                        }
                        dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                        dc[i] = Math.Abs(rsplusrm) - dctr;
                    }
                    dt1 = -dc[0] / ((dc[0] - dc[1]) / dt);
                    tret[j] += dt1;
                }
            }
            tret[1] -= SE.swe_deltat_ex(tret[1], ifl, ref serr);
            tret[4] -= SE.swe_deltat_ex(tret[4], ifl, ref serr);
            /*  
             * visibility of eclipse phases 
             */
            for (i = 4; i >= 0; i--) {	/* attr for i = 0 must be kept !!! */
                if (tret[i] == 0)
                    continue;
                if (eclipse_how(tret[i], SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], geopos[2],
                    attr, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                /*if (retflag2 & SE_ECL_VISIBLE) { could be wrong for 1st/4th contact */
                if (attr[6] > 0) {	/* this is safe, sun above horizon, using app. alt. */
                    retflag |= SwissEph.SE_ECL_VISIBLE;
                    switch (i) {
                        case 0: retflag |= SwissEph.SE_ECL_MAX_VISIBLE; break;
                        case 1: retflag |= SwissEph.SE_ECL_1ST_VISIBLE; break;
                        case 2: retflag |= SwissEph.SE_ECL_2ND_VISIBLE; break;
                        case 3: retflag |= SwissEph.SE_ECL_3RD_VISIBLE; break;
                        case 4: retflag |= SwissEph.SE_ECL_4TH_VISIBLE; break;
                        default: break;
                    }
                }
            }
            //#if 1
            if (0 == (retflag & SwissEph.SE_ECL_VISIBLE)) {
                if (backward)
                    K--;
                else
                    K++;
                goto next_try;
            }
            //#endif
            if ((retc = swe_rise_trans(tret[1] - 0.001, SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_RISE | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjdr, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc == -2) /* circumpolar sun */
                return retflag;
            if ((retc = swe_rise_trans(tret[1] - 0.001, SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_SET | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjds, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc == -2) /* circumpolar sun */
                return retflag;
            if (tjds < tret[1] || (tjds > tjdr && tjdr > tret[4]))
            {
                if (backward)
                    K--;
                else
                    K++;
                goto next_try;
            }
            if (tjdr > tret[1] && tjdr < tret[4])
            {
                tret[5] = tjdr;
                if (0 == (retflag & SwissEph.SE_ECL_MAX_VISIBLE)) {
                    tret[0] = tjdr;
                    if ((retc = eclipse_how(tret[5], SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], geopos[2], attr, ref serr)) == SwissEph.ERR)
                        return SwissEph.ERR;
                    retflag &= ~(SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_PARTIAL);
                    retflag |= (retc & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_PARTIAL));
                }
            }
            if (tjds > tret[1] && tjds < tret[4]) {
                tret[6] = tjds;
                if (0 == (retflag & SwissEph.SE_ECL_MAX_VISIBLE)) {
                    tret[0] = tjds;
                    if ((retc = eclipse_how(tret[6], SwissEph.SE_SUN, null, ifl, geopos[0], geopos[1], geopos[2], attr, ref serr)) == SwissEph.ERR)
                        return SwissEph.ERR;
                    retflag &= ~(SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_PARTIAL);
                    retflag |= (retc & (SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_PARTIAL));
                }
            }
            return retflag;
        }

        Int32 occult_when_loc(
             double tjd_start, Int32 ipl, string starname,
             Int32 ifl, double[] geopos, double[] tret, double[] attr,
             int backward, ref string serr)
        {
            int i, j, k, m;
            Int32 retflag = 0, retc;
            double t, tjd, dt, dtint;
            double tjdr = 0, tjds = 0;
            double[] xs = new double[6], xm = new double[6], ls = new double[6], lm = new double[6], x1 = new double[6], x2 = new double[6]; double dm, ds;
            double rmoon, rsun, rsplusrm, rsminusrm;
            double[] dc = new double[20]; double dctr, dctrmin;
            double twomin = 2.0 / 24.0 / 60.0;
            double tensec = 10.0 / 24.0 / 60.0 / 60.0;
            double twohr = 2.0 / 24.0;
            double tenmin = 10.0 / 24.0 / 60.0;
            double dt1 = 0, dt2 = 0, dtdiv, dtstart;
            double dadd2 = 1;
            double drad, dl;
            bool is_partial = false;
            Int32 iflag = SwissEph.SEFLG_TOPOCTR | ifl;
            Int32 iflaggeo = iflag & ~SwissEph.SEFLG_TOPOCTR;
            Int32 iflagcart = iflag | SwissEph.SEFLG_XYZ;
            int direction = 1;
            Int32 one_try = backward & SwissEph.SE_ECL_ONE_TRY;
            bool stop_after_this = false;
            backward &= 1;
            retflag = 0;
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            for (i = 0; i <= 9; i++)
                tret[i] = 0;
            if (backward != 0)
                direction = -1;
            t = tjd_start;
            tjd = tjd_start;
        next_try:
            is_partial = false;
            if (calc_planet_star(t, ipl, starname, iflaggeo, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /* fixed stars with an ecliptic latitude > 7  or < -7 cannot have 
             * an occultation. Even lunar parallax andd proper motion of star
             * will never allow it. */
            if (Math.Abs(ls[1]) > 7 && !String.IsNullOrWhiteSpace(starname))
            {
                serr = C.sprintf("occultation never occurs: star %s has ecl. lat. %.1f", starname, ls[1]);
                return SwissEph.ERR;
            }
            if (SE.swe_calc(t, SwissEph.SE_MOON, iflaggeo, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            dl = SE.swe_degnorm(ls[0] - lm[0]);
            if (direction < 0)
                dl -= 360;
            /* get rough conjunction in ecliptic longitude */
            while (Math.Abs(dl) > 0.1)
            {
                t += dl / 13;
                if (calc_planet_star(t, ipl, starname, iflaggeo, ls, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(t, SwissEph.SE_MOON, iflaggeo, lm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dl = SE.swe_degnorm(ls[0] - lm[0]);
                if (dl > 180) dl -= 360;
            }
            tjd = t;
            /* difference in latitude too big for an occultation */
            drad = Math.Abs(ls[1] - lm[1]);
            if (drad > 2)
            {
                if (one_try != 0)
                {
                    tret[0] = t + direction; /* return a date suitable for next try */
                    return 0;
                }
                t += direction * 20;
                tjd = t;
                goto next_try;
            }
            /*
             * radius of planet disk in AU
             */
            if (!String.IsNullOrEmpty(starname))
                drad = 0;
            else if (ipl < Sweph.NDIAM)
                drad = Sweph.pla_diam[ipl] / 2 / Sweph.AUNIT;
            else if (ipl > SwissEph.SE_AST_OFFSET)
                drad = SE.Sweph.swed.ast_diam / 2 * 1000 / Sweph.AUNIT; /* km -> m -> AU */
            else
                drad = 0;
            /* now find out, if there is an occultation at our geogr. location */
            dtdiv = 3;
            dtstart = dadd2; /* formerly 0.2 */
            for (dt = dtstart;
                 dt > 0.00001;
                 dt /= dtdiv)
            {
                if (dt < 0.01)
                    dtdiv = 2;
                for (i = 0, t = tjd - dt; i <= 2; i++, t += dt)
                {
                    /* this takes some time, but is necessary to avoid
                     * missing an eclipse */
                    if (calc_planet_star(t, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (calc_planet_star(t, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (dt < 0.1 && Math.Abs(ls[1] - lm[1]) > 2)
                    {
                        if (one_try != 0 || stop_after_this)
                        {
                            stop_after_this = true;
                        }
                        else
                        {
                            /*t = tjd + direction * 2;*/
                            t = tjd + direction * 20;
                            tjd = t;
                            goto next_try;
                        }
                    }
                    dc[i] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xs, xm)) * SwissEph.RADTODEG;
                    rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
                    rsun = Math.Asin(drad / ls[2]) * SwissEph.RADTODEG;
                    dc[i] -= (rmoon + rsun);
                }
                find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dctr);
                tjd += dtint + dt;
            }
            if (stop_after_this)
            { /* has one_try = TRUE */
                tret[0] = tjd;
                return 0;
            }
            if (calc_planet_star(tjd, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (calc_planet_star(tjd, ipl, starname, iflag, ls, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, SwissEph.SE_MOON, iflag, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xs, xm)) * SwissEph.RADTODEG;
            rmoon = Math.Asin(RMOON / lm[2]) * SwissEph.RADTODEG;
            rsun = Math.Asin(drad / ls[2]) * SwissEph.RADTODEG;
            rsplusrm = rsun + rmoon;
            rsminusrm = rsun - rmoon;
            if (dctr > rsplusrm)
            {
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                /*t = tjd + direction;*/
                t = tjd + direction * 20;
                tjd = t;
                goto next_try;
            }
            tret[0] = tjd - SE.swe_deltat_ex(tjd, ifl, ref serr);
            tret[0] = tjd - SE.swe_deltat_ex(tret[0], ifl, ref serr);
            if ((backward != 0 && tret[0] >= tjd_start - 0.0001)
              || (0 == backward && tret[0] <= tjd_start + 0.0001))
            {
                /* t = tjd + direction;*/
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                t = tjd + direction * 20;
                tjd = t;
                goto next_try;
            }
            if (dctr < rsminusrm)
                retflag = SwissEph.SE_ECL_ANNULAR;
            else if (dctr < Math.Abs(rsminusrm))
                retflag = SwissEph.SE_ECL_TOTAL;
            else if (dctr <= rsplusrm)
                retflag = SwissEph.SE_ECL_PARTIAL;
            dctrmin = dctr;
            /* contacts 2 and 3 */
            if (dctr > Math.Abs(rsminusrm))
            { /* partial, no 2nd and 3rd contact */
                tret[2] = tret[3] = 0;
                is_partial = true;
            }
            else
            {
                dc[1] = Math.Abs(rsminusrm) - dctrmin;
                for (i = 0, t = tjd - twomin; i <= 2; i += 2, t = tjd + twomin)
                {
                    if (calc_planet_star(t, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                    ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                    rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                    rmoon *= 0.99916; /* gives better accuracy for 2nd/3rd contacts */
                    rsun = Math.Asin(drad / ds) * SwissEph.RADTODEG;
                    rsminusrm = rsun - rmoon;
                    for (k = 0; k < 3; k++)
                    {
                        x1[k] = xs[k] / ds /*ls[2]*/;
                        x2[k] = xm[k] / dm /*lm[2]*/;
                    }
                    dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                    dc[i] = Math.Abs(rsminusrm) - dctr;
                }
                find_zero(dc[0], dc[1], dc[2], twomin, out dt1, out dt2);
                tret[2] = tjd + dt1 + twomin;
                tret[3] = tjd + dt2 + twomin;
                for (m = 0, dt = tensec; m < 2; m++, dt /= 10)
                {
                    for (j = 2; j <= 3; j++)
                    {
                        if (calc_planet_star(tret[j], ipl, starname, iflagcart | SwissEph.SEFLG_SPEED, xs, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        if (SE.swe_calc(tret[j], SwissEph.SE_MOON, iflagcart | SwissEph.SEFLG_SPEED, xm, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        for (i = 0; i < 2; i++)
                        {
                            if (i == 1)
                            {
                                for (k = 0; k < 3; k++)
                                {
                                    xs[k] -= xs[k + 3] * dt;
                                    xm[k] -= xm[k + 3] * dt;
                                }
                            }
                            dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                            ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                            rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                            rmoon *= 0.99916; /* gives better accuracy for 2nd/3rd contacts */
                            rsun = Math.Asin(drad / ds) * SwissEph.RADTODEG;
                            rsminusrm = rsun - rmoon;
                            for (k = 0; k < 3; k++)
                            {
                                x1[k] = xs[k] / ds /*ls[2]*/;
                                x2[k] = xm[k] / dm /*lm[2]*/;
                            }
                            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                            dc[i] = Math.Abs(rsminusrm) - dctr;
                        }
                        dt1 = -dc[0] / ((dc[0] - dc[1]) / dt);
                        tret[j] += dt1;
                    }
                }
                tret[2] -= SE.swe_deltat_ex(tret[2], ifl, ref serr);
                tret[3] -= SE.swe_deltat_ex(tret[3], ifl, ref serr);
                is_partial = false;
            }
            /* contacts 1 and 4 */
            dc[1] = rsplusrm - dctrmin;
            for (i = 0, t = tjd - twohr; i <= 2; i += 2, t = tjd + twohr)
            {
                if (calc_planet_star(t, ipl, starname, iflagcart, xs, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                rsun = Math.Asin(drad / ds) * SwissEph.RADTODEG;
                rsplusrm = rsun + rmoon;
                for (k = 0; k < 3; k++)
                {
                    x1[k] = xs[k] / ds /*ls[2]*/;
                    x2[k] = xm[k] / dm /*lm[2]*/;
                }
                dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                dc[i] = rsplusrm - dctr;
            }
            find_zero(dc[0], dc[1], dc[2], twohr, out dt1, out dt2);
            tret[1] = tjd + dt1 + twohr;
            tret[4] = tjd + dt2 + twohr;
            for (m = 0, dt = tenmin; m < 3; m++, dt /= 10)
            {
                for (j = 1; j <= 4; j += 3)
                {
                    if (calc_planet_star(tret[j], ipl, starname, iflagcart | SwissEph.SEFLG_SPEED, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(tret[j], SwissEph.SE_MOON, iflagcart | SwissEph.SEFLG_SPEED, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    for (i = 0; i < 2; i++)
                    {
                        if (i == 1)
                        {
                            for (k = 0; k < 3; k++)
                            {
                                xs[k] -= xs[k + 3] * dt;
                                xm[k] -= xm[k + 3] * dt;
                            }
                        }
                        dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                        ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                        rmoon = Math.Asin(RMOON / dm) * SwissEph.RADTODEG;
                        rsun = Math.Asin(drad / ds) * SwissEph.RADTODEG;
                        rsplusrm = rsun + rmoon;
                        for (k = 0; k < 3; k++)
                        {
                            x1[k] = xs[k] / ds /*ls[2]*/;
                            x2[k] = xm[k] / dm /*lm[2]*/;
                        }
                        dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
                        dc[i] = Math.Abs(rsplusrm) - dctr;
                    }
                    dt1 = -dc[0] / ((dc[0] - dc[1]) / dt);
                    tret[j] += dt1;
                }
            }
            tret[1] -= SE.swe_deltat_ex(tret[1], ifl, ref serr);
            tret[4] -= SE.swe_deltat_ex(tret[4], ifl, ref serr);
            /*  
             * visibility of eclipse phases 
             */
            for (i = 4; i >= 0; i--)
            {	/* attr for i = 0 must be kept !!! */
                if (tret[i] == 0)
                    continue;
                if (eclipse_how(tret[i], ipl, starname, ifl, geopos[0], geopos[1], geopos[2],
                    attr, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                /*if (retflag2 & SE_ECL_VISIBLE) { could be wrong for 1st/4th contact */
                if (attr[6] > 0)
                {	/* this is save, sun above horizon (using app. alt.) */
                    retflag |= SwissEph.SE_ECL_VISIBLE;
                    switch (i)
                    {
                        case 0: retflag |= SwissEph.SE_ECL_MAX_VISIBLE; break;
                        case 1: retflag |= SwissEph.SE_ECL_1ST_VISIBLE; break;
                        case 2: retflag |= SwissEph.SE_ECL_2ND_VISIBLE; break;
                        case 3: retflag |= SwissEph.SE_ECL_3RD_VISIBLE; break;
                        case 4: retflag |= SwissEph.SE_ECL_4TH_VISIBLE; break;
                        default: break;
                    }
                }
            }
            //#if 1
            if (0 == (retflag & SwissEph.SE_ECL_VISIBLE))
            {
                /* t = tjd + direction;*/
                if (one_try != 0)
                {
                    tret[0] = tjd;
                    return 0;
                }
                t = tjd + direction * 20;
                tjd = t;
                goto next_try;
            }
            //#endif
            if ((retc = swe_rise_trans(tret[1] - 0.1, ipl, starname, iflag, SwissEph.SE_CALC_RISE | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjdr, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc >= 0 && (retc = swe_rise_trans(tret[1] - 0.1, ipl, starname, iflag, SwissEph.SE_CALC_SET | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjds, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc >= 0)
            {
                if (tjdr > tret[1] && tjdr < tret[4])
                    tret[5] = tjdr;
                if (tjds > tret[1] && tjds < tret[4])
                    tret[6] = tjds;
            }
            /* note, circumpolar sun above horizon is not tested */
            if (!is_partial)
            {
                if ((retc = swe_rise_trans(tret[2], SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_RISE, geopos, 0, 0, ref tjdr, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (retc >= 0 && (retc = swe_rise_trans(tret[2], SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_SET, geopos, 0, 0, ref tjds, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (retc >= 0)
                {
                    if (tjds < tjdr)
                        retflag |= SwissEph.SE_ECL_OCC_BEG_DAYLIGHT;
                }
                if ((retc = swe_rise_trans(tret[3], SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_RISE, geopos, 0, 0, ref tjdr, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (retc >= 0 && (retc = swe_rise_trans(tret[3], SwissEph.SE_SUN, null, iflag, SwissEph.SE_CALC_SET, geopos, 0, 0, ref tjds, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (retc >= 0)
                {
                    if (tjds < tjdr)
                        retflag |= SwissEph.SE_ECL_OCC_END_DAYLIGHT;
                }
            }
            return retflag;
        }

        /* 
         * swe_azalt()
         * Computes azimut and height, from either ecliptic or 
         * equatorial coordinates
         *
         * input:
         *   tjd_ut	  
         *   iflag        either SE_ECL2HOR or SE_EQU2HOR
         *   geopos[3]    geograph. longitude, latitude, height above sea
         *   atpress	  atmospheric pressure at geopos in millibars (hPa)
         *   attemp	  atmospheric temperature in degrees C
         *   xin[2]       input coordinates polar, in degrees
         * 
         * Horizontal coordinates are returned in
         *   xaz[3]       xaz[0] = azimuth
         *                xaz[1] = true altitude
         *                xaz[2] = apparent altitude
         *
         * If atpress is not given (= 0), the programm assumes 1013.25 mbar;
         * if a non-zero height above sea is given, atpress is estimated.
         *   geohgt	  height of observer above sea (optional)
         */
        public void swe_azalt(
              double tjd_ut,
              Int32 calc_flag,
              double[] geopos,
              double atpress,
              double attemp,
              double[] xin,
              double[] xaz) {
            int i; string sdummy = String.Empty;
            double[] x = new double[6], xra = new double[3];
            double armc = SE.swe_degnorm(SE.swe_sidtime(tjd_ut) * 15 + geopos[0]);
            double mdd, eps_true;
            for (i = 0; i < 2; i++)
                xra[i] = xin[i];
            xra[2] = 1;
            if (calc_flag == SwissEph.SE_ECL2HOR) {
                SE.swe_calc(tjd_ut + SE.swe_deltat_ex(tjd_ut, -1, ref sdummy), SwissEph.SE_ECL_NUT, 0, x, ref sdummy);
                eps_true = x[0];
                SE.swe_cotrans(xra, xra, -eps_true);
            }
            mdd = SE.swe_degnorm(xra[0] - armc);
            x[0] = SE.swe_degnorm(mdd - 90);
            x[1] = xra[1];
            x[2] = 1;
            /* azimuth from east, counterclock */
            SE.swe_cotrans(x, x, 90 - geopos[1]);
            /* azimuth from south to west */
            x[0] = SE.swe_degnorm(x[0] + 90);
            xaz[0] = 360 - x[0];
            xaz[1] = x[1];		/* true height */
            if (atpress == 0) {
                /* estimate atmospheric pressure */
                atpress = 1013.25 * Math.Pow(1 - 0.0065 * geopos[2] / 288, 5.255);
            }
            xaz[2] = swe_refrac_extended(x[1], geopos[2], atpress, attemp, const_lapse_rate, SwissEph.SE_TRUE_TO_APP, null);
            /* xaz[2] = swe_refrac_extended(xaz[2], geopos[2], atpress, attemp, const_lapse_rate, SE_APP_TO_TRUE, NULL);*/
        }

        /* 
         * swe_azalt_rev()
         * computes either ecliptical or equatorial coordinates from
         * azimuth and true altitude in degrees.
         * For conversion between true and apparent altitude, there is
         * the function swe_refrac().
         *
         * input:
         *   tjd_ut	  
         *   iflag        either SE_HOR2ECL or SE_HOR2EQU
         *   xin[2]       azimut and true altitude, in degrees 
         */
        public void swe_azalt_rev(
              double tjd_ut,
              Int32 calc_flag,
              double[] geopos,
              double[] xin,
              double[] xout) {
            int i; string sdummy = String.Empty;
            double[] x = new double[6], xaz = new double[3];
            double geolon = geopos[0];
            double geolat = geopos[1];
            double armc = SE.swe_degnorm(SE.swe_sidtime(tjd_ut) * 15 + geolon);
            double eps_true, dang;
            for (i = 0; i < 2; i++)
                xaz[i] = xin[i];
            xaz[2] = 1;
            /* azimuth is from south, clockwise. 
             * we need it from east, counterclock */
            xaz[0] = 360 - xaz[0];
            xaz[0] = SE.swe_degnorm(xaz[0] - 90);
            /* equatorial positions */
            dang = geolat - 90;
            SE.swe_cotrans(xaz, xaz, dang);
            xaz[0] = SE.swe_degnorm(xaz[0] + armc + 90);
            xout[0] = xaz[0];
            xout[1] = xaz[1];
            /* ecliptic positions */
            if (calc_flag == SwissEph.SE_HOR2ECL) {
                SE.swe_calc(tjd_ut + SE.swe_deltat_ex(tjd_ut, -1, ref sdummy), SwissEph.SE_ECL_NUT, 0, x, ref sdummy);
                eps_true = x[0];
                SE.swe_cotrans(xaz, x, eps_true);
                xout[0] = x[0];
                xout[1] = x[1];
            }
        }

        /* swe_refrac()
         * Transforms apparent to true altitude and vice-versa.
         * These formulae do not handle the case when the
         * sun is visible below the geometrical horizon 
         * (from a mountain top or an air plane)
         * input:
         * double inalt;        * altitude of object in degrees *
         * double atpress;      * millibars (hectopascal) *
         * double attemp;       * degrees C *
         * int32  calc_flag;	* either SE_CALC_APP_TO_TRUE or 
         *                      *        SE_CALC_TRUE_TO_APP
         */
        public double swe_refrac(double inalt, double atpress, double attemp, Int32 calc_flag) {
            double a, refr;
            double pt_factor = atpress / 1010.0 * 283.0 / (273.0 + attemp);
            double trualt, appalt;
            //#if 0
            // /*
            //  * -- S. L. Moshier */
            //  double y, yy0, D0, N, D, P, Q;
            //  int i;
            //  if (calc_flag == SE_TRUE_TO_APP) {
            //    trualt = inalt;
            //    if( (trualt < -2.0) || (trualt >= 90.0) )
            //      return(trualt);
            //    /* For high altitude angle, AA page B61
            //     * Accuracy "usually about 0.1' ".
            //     */
            //    if( trualt > 15.0 ) {
            //      D = 0.00452*atpress/((273.0+attemp)*tan( DEGTORAD*trualt ));
            //      return(trualt + D);
            //    }
            //    /* Formula for low altitude is from the Almanac for Computers.
            //     * It gives the correction for observed altitude, so has
            //     * to be inverted numerically to get the observed from the true.
            //     * Accuracy about 0.2' for -20C < T < +40C and 970mb < P < 1050mb.
            //     */
            //    /* Start iteration assuming correction = 0
            //     */
            //    y = trualt;
            //    D = 0.0;
            //    /* Invert Almanac for Computers formula numerically
            //     */
            //    P = (atpress - 80.0)/930.0;
            //    Q = 4.8e-3 * (attemp - 10.0);
            //    yy0 = y;
            //    D0 = D;
            //    for( i=0; i<4; i++ ) {
            //      N = y + (7.31/(y+4.4));
            //      N = 1.0/tan(DEGTORAD*N);
            //      D = N*P/(60.0 + Q * (N + 39.0));
            //      N = y - yy0;
            //      yy0 = D - D0 - N; /* denominator of derivative */
            //      if( (N != 0.0) && (yy0 != 0.0) )
            //      /* Newton iteration with numerically estimated derivative */
            //        N = y - N*(trualt + D - y)/yy0;
            //      else
            //      /* Can't do it on first pass */
            //        N = trualt + D;
            //      yy0 = y;
            //      D0 = D;
            //      y = N;
            //    }
            //    return( trualt + D );
            //  } else {
            //#else
            /* another algorithm, from Meeus, German, p. 114ff.
             */
            if (calc_flag == SwissEph.SE_TRUE_TO_APP) {
                trualt = inalt;
                if (trualt > 15) {
                    a = Math.Tan((90 - trualt) * SwissEph.DEGTORAD);
                    refr = (58.276 * a - 0.0824 * a * a * a);
                    refr *= pt_factor / 3600.0;
                } else if (trualt > -5) {
                    /* the following tan is not defined for a value
                     * of trualt near -5.00158 and 89.89158 */
                    a = trualt + 10.3 / (trualt + 5.11);
                    if (a + 1e-10 >= 90)
                        refr = 0;
                    else
                        refr = 1.02 / Math.Tan(a * SwissEph.DEGTORAD);
                    refr *= pt_factor / 60.0;
                } else
                    refr = 0;
                appalt = trualt;
                if (appalt + refr > 0)
                    appalt += refr;
                return appalt;
            } else {
                //#endif
                /* apparent to true */
                appalt = inalt;
                /* the following tan is not defined for a value
                 * of inalt near -4.3285 and 89.9225 */
                a = appalt + 7.31 / (appalt + 4.4);
                if (a + 1e-10 >= 90)
                    refr = 0;
                else {
                    refr = 1.00 / Math.Tan(a * SwissEph.DEGTORAD);
                    refr -= 0.06 * Math.Sin(14.7 * refr + 13);
                }
                refr *= pt_factor / 60.0;
                trualt = appalt;
                if (appalt - refr > 0)
                    trualt = appalt - refr;
                return trualt;
            }
        }

        public void swe_set_lapse_rate(double lapse_rate) {
            const_lapse_rate = lapse_rate;
        }

        /* swe_refrac_extended()
         *
         * This function was created thanks to and with the help of the
         * archaeoastronomer Victor Reijs.
         * It is more correct and more skilled than the old function swe_refrac():
         * - it allows correct calculation of refraction for altitudes above sea > 0,
         *   where the ideal horizon and planets that are visible may have a 
         *   negative height. (for swe_refrac(), negative apparent heights do not 
         *   exist!)
         * - it allows to manipulate the refraction constant
         *
         * Transforms apparent to true altitude and vice-versa.
         * input:
         * double inalt;        * altitude of object above geometric horizon in degrees*
         *                      * geometric horizon = plane perpendicular to gravity *
         * double geoalt;       * altitude of observer above sea level in meters *
         * double atpress;      * millibars (hectopascal) *
         * double lapse_rate;    * (dT/dh) [deg K/m]
         * double attemp;       * degrees C *
         * int32  calc_flag;    * either SE_CALC_APP_TO_TRUE or
         *                      *        SE_CALC_TRUE_TO_APP
         *
         * function returns:
         * case 1, conversion from true altitude to apparent altitude
         * - apparent altitude, if body appears above is observable above ideal horizon
         * - true altitude (the input value), otherwise
         *   "ideal horizon" is the horizon as seen above an ideal sphere (as seen
         *   from a plane over the ocean with a clear sky)
         * case 2, conversion from apparent altitude to true altitude
         * - the true altitude resulting from the input apparent altitude, if this value
         *   is a plausible apparent altitude, i.e. if it is a position above the ideal
         *   horizon
         * - the input altitude otherwise
         *
         * in addition the array dret[] is given the following values
         * - dret[0] true altitude, if possible; otherwise input value
         * - dret[1] apparent altitude, if possible; otherwise input value
         * - dret[2] refraction
         * - dret[3] dip of the horizon
         *
         * The body is above the horizon if the dret[0] != dret[1]
         */
        public double swe_refrac_extended(double inalt, double geoalt, double atpress, double attemp, double lapse_rate, Int32 calc_flag, double[] dret) {
            double refr;
            double trualt;
            double dip = calc_dip(geoalt, atpress, attemp, lapse_rate);
            double D, D0, N, y, yy0;
            int i;
            /* make sure that inalt <=90 */
            if ((inalt > 90))
                inalt = 180 - inalt;
            if (calc_flag == SwissEph.SE_TRUE_TO_APP) {
                if (inalt < -10) {
                    if (dret != null) {
                        dret[0] = inalt;
                        dret[1] = inalt;
                        dret[2] = 0;
                        dret[3] = dip;
                    }
                    return inalt;
                }
                /* by iteration */
                y = inalt;
                D = 0.0;
                yy0 = 0;
                D0 = D;
                for (i = 0; i < 5; i++) {
                    D = calc_astronomical_refr(y, atpress, attemp);
                    N = y - yy0;
                    yy0 = D - D0 - N; /* denominator of derivative */
                    if (N != 0.0 && yy0 != 0.0) /* sic !!! code by Moshier */
                        N = y - N * (inalt + D - y) / yy0; /* Newton iteration with numerically estimated derivative */
                    else /* Can't do it on first pass */
                        N = inalt + D;
                    yy0 = y;
                    D0 = D;
                    y = N;
                }
                refr = D;
                if ((inalt + refr < dip)) {
                    if (dret != null) {
                        dret[0] = inalt;
                        dret[1] = inalt;
                        dret[2] = 0;
                        dret[3] = dip;
                    }
                    return inalt;
                }
                if (dret != null) {
                    dret[0] = inalt;
                    dret[1] = inalt + refr;
                    dret[2] = refr;
                    dret[3] = dip;
                }
                return inalt + refr;
            } else {
                refr = calc_astronomical_refr(inalt, atpress, attemp);
                trualt = inalt - refr;
                if (dret != null) {
                    if (inalt > dip) {
                        dret[0] = trualt;
                        dret[1] = inalt;
                        dret[2] = refr;
                        dret[3] = dip;
                    } else {
                        dret[0] = inalt;
                        dret[1] = inalt;
                        dret[2] = 0;
                        dret[3] = dip;
                    }
                }
                if (trualt > dip)
                    return trualt;
                else
                    return inalt;
            }
        }

        /* calculate the astronomical refraction
         * input parameters:
         * double inalt        * apparent altitude of object
         * double atpress      * atmospheric pressure millibars (hectopascal) *
         * double attemp       * atmospheric temperature degrees C *
         * returns double r in degrees
        */
        double calc_astronomical_refr(double inalt, double atpress, double attemp) {
            //#if 0
            //  /* formula based on G.G. Bennett, The calculation of astronomical refraction in marine navigation,
            //  * Journal of Inst. Navigation, No. 35, page 255-259, 1982,
            //  * page 257 for refraction formula: formula H
            //  * and page 259 for atmospheric compensation
            //  */
            //  double refractaccent = 1/tan(DEGTORAD*(inalt + 7.31/(inalt+4.4)));
            //  double r = (refractaccent - 0.06 * sin(DEGTORAD*(14.7*refractaccent +13)));
            //  r = ((atpress - 80) / 930 / (1 + 0.00008 * (r + 39) * (attemp - 10)) * r)/60;
            // return r;
            //#else
            /* Formula by Sinclair, see article mentioned above, p. 256. Better for 
             * apparent altitudes < 0;  */
            double r;
            if (inalt > 17.904104638432) { /* for continuous function, instead of '>15' */
                r = 0.97 / Math.Tan(inalt * SwissEph.DEGTORAD);
            } else {
                r = (34.46 + 4.23 * inalt + 0.004 * inalt * inalt) / (1 + 0.505 * inalt + 0.0845 * inalt * inalt);
            }
            r = ((atpress - 80) / 930 / (1 + 0.00008 * (r + 39) * (attemp - 10)) * r) / 60.0;
            return r;
            //#endif
        }

        /* calculate dip of the horizon
         * input parameters:
         * double geoalt       * altitude of observer above sea level in meters *
         * double atpress      * atmospheric pressure millibars (hectopascal) *
         * double attemp       * atmospheric temperature degrees C *
         * double lapse_rate   * (dT/dh) [deg K/m]
         * returns dip in degrees
         */
        double calc_dip(double geoalt, double atpress, double attemp, double lapse_rate) {
            /* below formula is based on A. Thom, Megalithic lunar observations, 1973 (page 32).
            * conversion to metric has been done by
            * V. Reijs, 2000, http://www.iol.ie/~geniet/eng/refract.htm
            */
            double krefr = (0.0342 + lapse_rate) / (0.154 * 0.0238);
            double d = 1 - 1.8480 * krefr * atpress / (273.16 + attemp) / (273.16 + attemp);
            /* return -0.03203*sqrt(geoalt)*sqrt(d); */
            /* double a = acos(1/(1+geoalt/EARTH_RADIUS));*/
            return -180.0 / Math.PI * Math.Acos(1 / (1 + geoalt / Sweph.EARTH_RADIUS)) * Math.Sqrt(d);
        }


        /* Computes attributes of a lunar eclipse for given tjd and geopos
         * 
         * retflag	SE_ECL_TOTAL or SE_ECL_PARTIAL
         *              SE_ECL_PENUMBRAL
         *              if 0, there is no eclipse
         * 
         * attr[0]	umbral magnitude at tjd
         * attr[1]      penumbral magnitude
         * attr[4]	azimuth of moon at tjd
         * attr[5]	true altitude of moon above horizon at tjd
         * attr[6]	apparent altitude of moon above horizon at tjd
         * attr[7]	distance of moon from opposition in degrees
         * attr[8]	umbral magnitude at tjd (= attr[0])
         * attr[9]	saros series number
         * attr[10]	saros series member number
         *         declare as attr[20] at least !
         * 
         */
        public Int32 swe_lun_eclipse_how(
                  double tjd_ut,
                  Int32 ifl,
                  double[] geopos,
                  double[] attr,
                  ref string serr) {
            double[] dcore = new double[10];
            double[] lm = new double[6], xaz = new double[6];
            Int32 retc;
            /* attention: geopos[] is not used so far; may be NULL */
            if (geopos != null)
                geopos[0] = geopos[0]; /* to shut up mint */
            if (geopos != null && (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX))
            {
                serr = C.sprintf("location for eclipses must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            ifl = ifl & ~SwissEph.SEFLG_TOPOCTR;
            ifl &= ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            SE.SwephLib.swi_set_tid_acc(tjd_ut, ifl, 0, ref serr);
            retc = lun_eclipse_how(tjd_ut, ifl, attr, dcore, ref serr);
            if (geopos == null) {
                return retc;
            }
            /* 
             * azimuth and altitude of moon
             */
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            if (SE.swe_calc_ut(tjd_ut, SwissEph.SE_MOON, ifl | SwissEph.SEFLG_TOPOCTR | SwissEph.SEFLG_EQUATORIAL, lm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            swe_azalt(tjd_ut, SwissEph.SE_EQU2HOR, geopos, 0, 10, lm, xaz);
            attr[4] = xaz[0];
            attr[5] = xaz[1];
            attr[6] = xaz[2];
            if (xaz[2] <= 0)
                retc = 0;
            return retc;
        }

        /*
         * attr[]: 	see swe_lun_eclipse_how()
         *
         * dcore[0]:	distance of shadow axis from geocenter r0
         * dcore[1]:	diameter of core shadow on fundamental plane d0
         * dcore[2]:	diameter of half-shadow on fundamental plane D0
         */
        Int32 lun_eclipse_how(
                  double tjd_ut,
                  Int32 ifl,
                  double[] attr,
                  double[] dcore,
                  ref string serr) {
            int i, j, k;
            Int32 retc = 0;
            double[] e = new double[6], rm = new double[6], rs = new double[6];
            double dsm, d0, D0, s0, r0, ds, dm;
            double dctr; double[] x1 = new double[6], x2 = new double[6];
            double f1, f2;
            double deltat, tjd, d;
            double cosf1, cosf2;
            double rmoon = RMOON;
            double dmoon = 2 * rmoon;
            Int32 iflag;
            for (i = 0; i < 10; i++)
                dcore[i] = 0;
            for (i = 0; i < 20; i++)
                attr[i] = 0;
            /* nutation need not be in lunar and solar positions,
             * if mean sidereal time will be used */
            iflag = SwissEph.SEFLG_SPEED | SwissEph.SEFLG_EQUATORIAL | ifl;
            iflag = iflag | SwissEph.SEFLG_XYZ;
            deltat = SE.swe_deltat_ex(tjd_ut, ifl, ref serr);
            tjd = tjd_ut + deltat;
            /* moon in cartesian coordinates */
            if (SE.swe_calc(tjd, SwissEph.SE_MOON, iflag, rm, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /* distance of moon from geocenter */
            dm = Math.Sqrt(SE.Sweph.square_sum(rm));
            /* sun in cartesian coordinates */
            if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflag, rs, ref serr) == SwissEph.ERR)
                return SwissEph.ERR;
            /* distance of sun from geocenter */
            ds = Math.Sqrt(SE.Sweph.square_sum(rs));
            for (i = 0; i < 3; i++) {
                x1[i] = rs[i] / ds;
                x2[i] = rm[i] / dm;
            }
            dctr = Math.Acos(SE.SwephLib.swi_dot_prod_unit(x1, x2)) * SwissEph.RADTODEG;
            /* selenocentric sun */
            for (i = 0; i <= 2; i++)
                rs[i] -= rm[i];
            /* selenocentric earth */
            for (i = 0; i <= 2; i++)
                rm[i] = -rm[i];
            /* sun - earth vector */
            for (i = 0; i <= 2; i++)
                e[i] = (rm[i] - rs[i]);
            /* distance sun - earth */
            dsm = Math.Sqrt(SE.Sweph.square_sum(e));
            /* sun - earth unit vector */
            for (i = 0; i <= 2; i++)
                e[i] /= dsm;
            f1 = ((RSUN - REARTH) / dsm);
            cosf1 = Math.Sqrt(1 - f1 * f1);
            f2 = ((RSUN + REARTH) / dsm);
            cosf2 = Math.Sqrt(1 - f2 * f2);
            /* distance of earth from fundamental plane */
            s0 = -SE.Sweph.dot_prod(rm, e);
            /* distance of shadow axis from selenocenter */
            r0 = Math.Sqrt(dm * dm - s0 * s0);
            /* diameter of core shadow on fundamental plane */
            /* one 50th is added for effect of atmosphere, AA98, L4 */
            d0 = Math.Abs(s0 / dsm * (DSUN - DEARTH) - DEARTH) * (1 + 1.0 / 50.0) / cosf1;
            /* diameter of half-shadow on fundamental plane */
            D0 = (s0 / dsm * (DSUN + DEARTH) + DEARTH) * (1 + 1.0 / 50.0) / cosf2;
            d0 /= cosf1;
            D0 /= cosf2;
            /* for better agreement with NASA: */
            d0 *= 0.99405;
            D0 *= 0.98813;
            dcore[0] = r0;
            dcore[1] = d0;
            dcore[2] = D0;
            dcore[3] = cosf1;
            dcore[4] = cosf2;
            /**************************
             * phase and umbral magnitude
             **************************/
            retc = 0;
            if (d0 / 2 >= r0 + rmoon / cosf1) {
                retc = SwissEph.SE_ECL_TOTAL;
                attr[0] = (d0 / 2 - r0 + rmoon) / dmoon;
            } else if (d0 / 2 >= r0 - rmoon / cosf1) {
                retc = SwissEph.SE_ECL_PARTIAL;
                attr[0] = (d0 / 2 - r0 + rmoon) / dmoon;
            } else if (D0 / 2 >= r0 - rmoon / cosf2) {
                retc = SwissEph.SE_ECL_PENUMBRAL;
                attr[0] = 0;
            } else {
                serr = C.sprintf("no lunar eclipse at tjd = %f", tjd);
            }
            attr[8] = attr[0];
            /**************************
             * penumbral magnitude
             **************************/
            attr[1] = (D0 / 2 - r0 + rmoon) / dmoon;
            if (retc != 0)
                attr[7] = 180 - Math.Abs(dctr);
            /* saros series and member */
            for (i = 0; i < NSAROS_LUNAR; i++) {
                d = (tjd_ut - saros_data_lunar[i].tstart) / SAROS_CYCLE;
                if (d < 0) continue;
                j = (int)d;
                if ((d - j) * SAROS_CYCLE < 2) {
                    attr[9] = (double)saros_data_lunar[i].series_no;
                    attr[10] = (double)j + 1;
                    break;
                }
                k = j + 1;
                if ((k - d) * SAROS_CYCLE < 2) {
                    attr[9] = (double)saros_data_lunar[i].series_no;
                    attr[10] = (double)k + 1;
                    break;
                }
            }
            if (i == NSAROS_LUNAR) {
                attr[9] = attr[10] = -99999999;
            }
            return retc;
        }

        /* When is the next lunar eclipse?
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_PENUMBRAL or SE_ECL_PARTIAL
         *
         * tret[0]	time of maximum eclipse
         * tret[1]	
         * tret[2]	time of partial phase begin (indices consistent with solar eclipses)
         * tret[3]	time of partial phase end
         * tret[4]	time of totality begin
         * tret[5]	time of totality end
         * tret[6]	time of penumbral phase begin
         * tret[7]	time of penumbral phase end
         */
        public Int32 swe_lun_eclipse_when(double tjd_start, Int32 ifl, Int32 ifltype,
             double[] tret, bool backward, ref string serr) {
            int i, j, m, n, o, i1 = 0, i2 = 0;
            Int32 retflag = 0, retflag2 = 0;
            double t, tjd, tjd2, dt, dtint, dta, dtb;
            double T, T2, T3, T4, K, F, M, Mm;
            double E, Ff, F1, A1, Om;
            double[] xs = new double[6], xm = new double[6]; double dm, ds;
            double rsun, rearth; double[] dcore = new double[10];
            double[] dc = new double[3]; double dctr;
            double twohr = 2.0 / 24.0;
            double tenmin = 10.0 / 24.0 / 60.0;
            double dt1 = 0, dt2 = 0;
            double kk;
            double[] attr = new double[20];
            double dtstart, dtdiv;
            double[] xa = new double[6], xb = new double[6];
            int direction = 1;
            Int32 iflag;
            Int32 iflagcart;
            ifl &= SEFLG_EPHMASK;
            SE.SwephLib.swi_set_tid_acc(tjd_start, ifl, 0, ref serr);
            iflag = SwissEph.SEFLG_EQUATORIAL | ifl;
            iflagcart = iflag | SwissEph.SEFLG_XYZ;
            ifltype &= ~(SwissEph.SE_ECL_CENTRAL | SwissEph.SE_ECL_NONCENTRAL);
            if (ifltype!=0 & (SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL)!=0)
            {
                ifltype &= ~(SwissEph.SE_ECL_ANNULAR | SwissEph.SE_ECL_ANNULAR_TOTAL);
                if (ifltype == 0)
                {
                    serr = "annular lunar eclipses don't exist";
                    return SwissEph.ERR; /* avoids infinite loop */
                }
            }
            if (ifltype == 0)
                ifltype = SwissEph.SE_ECL_TOTAL | SwissEph.SE_ECL_PENUMBRAL | SwissEph.SE_ECL_PARTIAL;
            if (backward)
                direction = -1;
            K = (int)((tjd_start - Sweph.J2000) / 365.2425 * 12.3685);
            K -= direction;
        next_try:
            retflag = 0;
            for (i = 0; i <= 9; i++)
                tret[i] = 0;
            kk = K + 0.5;
            T = kk / 1236.85;
            T2 = T * T; T3 = T2 * T; T4 = T3 * T;
            Ff = F = SE.swe_degnorm(160.7108 + 390.67050274 * kk
                         - 0.0016341 * T2
                         - 0.00000227 * T3
                         + 0.000000011 * T4);
            if (Ff > 180)
                Ff -= 180;
            if (Ff > 21 && Ff < 159) { 	/* no eclipse possible */
                K += direction;
                goto next_try;
            }
            /* approximate time of geocentric maximum eclipse
             * formula from Meeus, German, p. 381 */
            tjd = 2451550.09765 + 29.530588853 * kk
                                + 0.0001337 * T2
                                - 0.000000150 * T3
                                + 0.00000000073 * T4;
            M = SE.swe_degnorm(2.5534 + 29.10535669 * kk
                                - 0.0000218 * T2
                                - 0.00000011 * T3);
            Mm = SE.swe_degnorm(201.5643 + 385.81693528 * kk
                                + 0.1017438 * T2
                                + 0.00001239 * T3
                                + 0.000000058 * T4);
            Om = SE.swe_degnorm(124.7746 - 1.56375580 * kk
                                + 0.0020691 * T2
                                + 0.00000215 * T3);
            E = 1 - 0.002516 * T - 0.0000074 * T2;
            A1 = SE.swe_degnorm(299.77 + 0.107408 * kk - 0.009173 * T2);
            M *= SwissEph.DEGTORAD;
            Mm *= SwissEph.DEGTORAD;
            F *= SwissEph.DEGTORAD;
            Om *= SwissEph.DEGTORAD;
            F1 = F - 0.02665 * Math.Sin(Om) * SwissEph.DEGTORAD;
            A1 *= SwissEph.DEGTORAD;
            tjd = tjd - 0.4075 * Math.Sin(Mm)
                      + 0.1721 * E * Math.Sin(M)
                      + 0.0161 * Math.Sin(2 * Mm)
                      - 0.0097 * Math.Sin(2 * F1)
                      + 0.0073 * E * Math.Sin(Mm - M)
                      - 0.0050 * E * Math.Sin(Mm + M)
                      - 0.0023 * Math.Sin(Mm - 2 * F1)
                      + 0.0021 * E * Math.Sin(2 * M)
                      + 0.0012 * Math.Sin(Mm + 2 * F1)
                      + 0.0006 * E * Math.Sin(2 * Mm + M)
                      - 0.0004 * Math.Sin(3 * Mm)
                      - 0.0003 * E * Math.Sin(M + 2 * F1)
                      + 0.0003 * Math.Sin(A1)
                      - 0.0002 * E * Math.Sin(M - 2 * F1)
                      - 0.0002 * E * Math.Sin(2 * Mm - M)
                      - 0.0002 * Math.Sin(Om);
            /* 
             * precise computation:
             * time of maximum eclipse (if eclipse) = 
             * minimum selenocentric angle between sun and earth edges.
             * After this time has been determined, check
             * whether or not an eclipse is taking place with
             * the function lun_eclipse_how().
             */
            dtstart = 0.1;
            if (tjd < 2000000 || tjd > 2500000)
                dtstart = 5;
            dtdiv = 4;
            for (j = 0, dt = dtstart;
                 dt > 0.001;
                 j++, dt /= dtdiv) {
                for (i = 0, t = tjd - dt; i <= 2; i++, t += dt) {
                    if (SE.swe_calc(t, SwissEph.SE_SUN, iflagcart, xs, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(t, SwissEph.SE_MOON, iflagcart, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    for (m = 0; m < 3; m++) {
                        xs[m] -= xm[m];	/* selenocentric sun */
                        xm[m] = -xm[m];	/* selenocentric earth */
                    }
                    ds = Math.Sqrt(SE.Sweph.square_sum(xs));
                    dm = Math.Sqrt(SE.Sweph.square_sum(xm));
                    for (m = 0; m < 3; m++) {
                        xa[m] = xs[m] / ds;
                        xb[m] = xm[m] / dm;
                    }
                    dc[i] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xa, xb)) * SwissEph.RADTODEG;
                    rearth = Math.Asin(REARTH / dm) * SwissEph.RADTODEG;
                    rsun = Math.Asin(RSUN / ds) * SwissEph.RADTODEG;
                    dc[i] -= (rearth + rsun);
                }
                find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dctr);
                tjd += dtint + dt;
            }
            tjd2 = tjd - SE.swe_deltat_ex(tjd, ifl, ref serr);
            tjd2 = tjd - SE.swe_deltat_ex(tjd2, ifl, ref serr);
            tjd = tjd - SE.swe_deltat_ex(tjd2, ifl, ref serr);
            if ((retflag = swe_lun_eclipse_how(tjd, ifl, null, attr, ref serr)) == SwissEph.ERR)
                return retflag;
            if (retflag == 0) {
                K += direction;
                goto next_try;
            }
            tret[0] = tjd;
            if ((backward && tret[0] >= tjd_start - 0.0001)
              || (!backward && tret[0] <= tjd_start + 0.0001)) {
                K += direction;
                goto next_try;
            }
            /* 
             * check whether or not eclipse type found is wanted
             */
            /* non penumbral eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_PENUMBRAL) && (retflag & SwissEph.SE_ECL_PENUMBRAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* non partial eclipse is wanted: */
            if (0 == (ifltype & SwissEph.SE_ECL_PARTIAL) && (retflag & SwissEph.SE_ECL_PARTIAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* annular-total eclipse will be discovered later */
            if (0 == (ifltype & (SwissEph.SE_ECL_TOTAL)) && (retflag & SwissEph.SE_ECL_TOTAL) != 0) {
                K += direction;
                goto next_try;
            }
            /* 
             * n = 0: times of eclipse begin and end 
             * n = 1: times of totality begin and end 
             * n = 2: times of center line begin and end 
             */
            if ((retflag & SwissEph.SE_ECL_PENUMBRAL) != 0)
                o = 0;
            else if ((retflag & SwissEph.SE_ECL_PARTIAL) != 0)
                o = 1;
            else
                o = 2;
            dta = twohr;
            dtb = tenmin;
            for (n = 0; n <= o; n++) {
                if (n == 0) {
                    i1 = 6; i2 = 7;
                } else if (n == 1) {
                    i1 = 2; i2 = 3;
                } else if (n == 2) {
                    i1 = 4; i2 = 5;
                }
                //#if 1
                for (i = 0, t = tjd - dta; i <= 2; i += 1, t += dta) {
                    if ((retflag2 = lun_eclipse_how(t, ifl, attr, dcore, ref serr)) == SwissEph.ERR)
                        return retflag2;
                    if (n == 0)
                        dc[i] = dcore[2] / 2 + RMOON / dcore[4] - dcore[0];
                    else if (n == 1)
                        dc[i] = dcore[1] / 2 + RMOON / dcore[3] - dcore[0];
                    else if (n == 2)
                        dc[i] = dcore[1] / 2 - RMOON / dcore[3] - dcore[0];
                }
                find_zero(dc[0], dc[1], dc[2], dta, out dt1, out dt2);
                dtb = (dt1 + dta) / 2;
                tret[i1] = tjd + dt1 + dta;
                tret[i2] = tjd + dt2 + dta;
                //#else
                //    tret[i1] = tjd - dtb;
                //    tret[i2] = tjd + dtb;
                //#endif
                for (m = 0, dt = dtb / 2; m < 3; m++, dt /= 2) {
                    for (j = i1; j <= i2; j += (i2 - i1)) {
                        for (i = 0, t = tret[j] - dt; i < 2; i++, t += dt) {
                            if ((retflag2 = lun_eclipse_how(t, ifl, attr, dcore, ref serr)) == SwissEph.ERR)
                                return retflag2;
                            if (n == 0)
                                dc[i] = dcore[2] / 2 + RMOON / dcore[4] - dcore[0];
                            else if (n == 1)
                                dc[i] = dcore[1] / 2 + RMOON / dcore[3] - dcore[0];
                            else if (n == 2)
                                dc[i] = dcore[1] / 2 - RMOON / dcore[3] - dcore[0];
                        }
                        dt1 = dc[1] / ((dc[1] - dc[0]) / dt);
                        tret[j] -= dt1;
                    }
                }
            }
            return retflag;
        }

        /* When is the next lunar eclipse, observable at a geographic position?
         *
         * retflag	SE_ECL_TOTAL or SE_ECL_PENUMBRAL or SE_ECL_PARTIAL
         *
         * tret[0]	time of maximum eclipse
         * tret[1]	
         * tret[2]	time of partial phase begin (indices consistent with solar eclipses)
         * tret[3]	time of partial phase end
         * tret[4]	time of totality begin
         * tret[5]	time of totality end
         * tret[6]	time of penumbral phase begin
         * tret[7]	time of penumbral phase end
         * tret[8]	time of moonrise, if it occurs during the eclipse
         * tret[9]	time of moonset, if it occurs during the eclipse
         *
         * attr[0]      umbral magnitude at tjd
         * attr[1]      penumbral magnitude
         * attr[4]      azimuth of moon at tjd
         * attr[5]      true altitude of moon above horizon at tjd
         * attr[6]      apparent altitude of moon above horizon at tjd
         * attr[7]      distance of moon from opposition in degrees
         * attr[8]      umbral magnitude at tjd (= attr[0])
         * attr[9]      saros series number
         * attr[10]     saros series member number
         *         declare as attr[20] at least !
         */
        public Int32 swe_lun_eclipse_when_loc(double tjd_start, Int32 ifl,
             double[] geopos, double[] tret, double[] attr, bool backward, ref string serr)
        {
            Int32 retflag = 0, retflag2 = 0, retc;
            double tjdr = 0, tjds = 0, tjd_max = 0;
            int i;
            if (geopos != null && (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX))
            {
                serr = C.sprintf("location for eclipses must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            ifl &= ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
        next_lun_ecl:
            if ((retflag = swe_lun_eclipse_when(tjd_start, ifl, 0, tret, backward, ref serr)) == SwissEph.ERR)
            {
                return SwissEph.ERR;
            }
            /*  
             * visibility of eclipse phases 
             */
            retflag = 0;
            for (i = 7; i >= 0; i--)
            {
                if (i == 1) continue;
                if (tret[i] == 0) continue;
                if ((retflag2 = swe_lun_eclipse_how(tret[i], ifl, geopos, attr, ref serr)) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (attr[6] > 0)
                {  /* moon above horizon, using app. alt. */
                    retflag |= SwissEph.SE_ECL_VISIBLE;
                    switch (i)
                    {
                        case 0: retflag |= SwissEph.SE_ECL_MAX_VISIBLE; break;
                        case 2: retflag |= SwissEph.SE_ECL_PARTBEG_VISIBLE; break;
                        case 3: retflag |= SwissEph.SE_ECL_PARTEND_VISIBLE; break;
                        case 4: retflag |= SwissEph.SE_ECL_TOTBEG_VISIBLE; break;
                        case 5: retflag |= SwissEph.SE_ECL_TOTEND_VISIBLE; break;
                        case 6: retflag |= SwissEph.SE_ECL_PENUMBBEG_VISIBLE; break;
                        case 7: retflag |= SwissEph.SE_ECL_PENUMBEND_VISIBLE; break;
                        default: break;
                    }
                }
            }
            if (0 == (retflag & SwissEph.SE_ECL_VISIBLE))
            {
                if (backward)
                    tjd_start = tret[0] - 25;
                else
                    tjd_start = tret[0] + 25;
                goto next_lun_ecl;
            }
            /* moon rise and moon set */
            tjd_max = tret[0];
            if ((retc = swe_rise_trans(tret[6] - 0.001, SwissEph.SE_MOON, null, ifl, SwissEph.SE_CALC_RISE | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjdr, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc >= 0 && (retc = swe_rise_trans(tret[6] - 0.001, SwissEph.SE_MOON, null, ifl, SwissEph.SE_CALC_SET | SwissEph.SE_BIT_DISC_BOTTOM, geopos, 0, 0, ref tjds, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retc >= 0)
            {
                if (tjds < tret[6] || (tjds > tjdr && tjdr > tret[7]))
                {
                    if (backward)
                        tjd_start = tret[0] - 25;
                    else
                        tjd_start = tret[0] + 25;
                    goto next_lun_ecl;
                }
                if (tjdr > tret[6] && tjdr < tret[7])
                {
                    tret[6] = 0;
                    for (i = 2; i <= 5; i++)
                    {
                        if (tjdr > tret[i])
                            tret[i] = 0;
                    }
                    tret[8] = tjdr;
                    if (tjdr > tret[0])
                    {
                        tjd_max = tjdr;
                    }
                }
                if (tjds > tret[6] && tjds < tret[7])
                {
                    tret[7] = 0;
                    for (i = 2; i <= 5; i++)
                    {
                        if (tjds < tret[i])
                            tret[i] = 0;
                    }
                    tret[9] = tjds;
                    if (tjds < tret[0])
                    {
                        tjd_max = tjds;
                    }
                }
            }
            tret[0] = tjd_max;
            if ((retflag2 = swe_lun_eclipse_how(tjd_max, ifl, geopos, attr, ref serr)) == SwissEph.ERR)
                return SwissEph.ERR;
            if (retflag2 == 0)
            {
                if (backward)
                    tjd_start = tret[0] - 25;
                else
                    tjd_start = tret[0] + 25;
                goto next_lun_ecl;
            }
            retflag |= (retflag2 & SwissEph.SE_ECL_ALLTYPES_LUNAR);
            return retflag;
        }

        /* 
         * function calculates planetary phenomena
         * 
         * attr[0] = phase angle (earth-planet-sun)
         * attr[1] = phase (illumined fraction of disc)
         * attr[2] = elongation of planet
         * attr[3] = apparent diameter of disc
         * attr[4] = apparent magnitude
         * attr[5] = geocentric horizontal parallax (Moon)
         *         declare as attr[20] at least !
         *
         * Note: the lunar magnitude is quite a complicated thing,
         * but our algorithm is very simple. 
         * The phase of the moon, its distance from the earth and
         * the sun is considered, but no other factors.
         * 
         */
        const double EULER = 2.718281828459;
        const int NMAG_ELEM = (SwissEph.SE_VESTA + 1);
//#define MAG_IAU_1986
        static readonly double[,] mag_elem = new double[NMAG_ELEM, 4] {
                /* DTV-Atlas Astronomie, p. 32 */
                {-26.86, 0, 0, 0}, /* Sun */
                {-12.55, 0, 0, 0}, /* Moon */
//#ifdef MAG_IAU_1986
                /* IAU 1986 */
                {-0.42, 3.80, -2.73, 2.00}, /* Mercury */
                {-4.40, 0.09, 2.39, -0.65}, /* Venus */
                {- 1.52, 1.60, 0, 0},   /* Mars */
                {- 9.40, 0.5, 0, 0},    /* Jupiter */
                {- 8.88, -2.60, 1.25, 0.044},   /* Saturn */
                {- 7.19, 0.0, 0, 0},    /* Uranus */
                {- 6.87, 0.0, 0, 0},    /* Neptune */
                {- 1.00, 0.0, 0, 0},    /* Pluto */
//#else
//#endif
                {99, 0, 0, 0},          /* nodes and apogees */
                {99, 0, 0, 0},
                {99, 0, 0, 0},
                {99, 0, 0, 0},
                {99, 0, 0, 0},          /* Earth */
                /* from Bowell data base */
                {6.5, 0.15, 0, 0},      /* Chiron */
                {7.0, 0.15, 0, 0},      /* Pholus */
                {3.34, 0.12, 0, 0},     /* Ceres */
                {4.13, 0.11, 0, 0},     /* Pallas */
                {5.33, 0.32, 0, 0},     /* Juno */
                {3.20, 0.32, 0, 0},     /* Vesta */
                };
        public Int32 swe_pheno(double tjd, Int32 ipl, Int32 iflag, double[] attr, ref string serr) {
            int i;
            double[] xx = new double[6], xx2 = new double[6], xxs = new double[6], lbr = new double[6], lbr2 = new double[6]; double dt = 0, dd;
            double fac;
            double T, inx, om, sinB, u1, u2, du;
            double ph1, ph2; double[] me = new double[2];
            Int32 iflagp, epheflag;
            iflag &= ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            for (i = 0; i < 20; i++)
                attr[i] = 0;
            /* Ceres - Vesta must be SE_CERES etc., not 10001 etc. */
            if (ipl > SwissEph.SE_AST_OFFSET && ipl <= SwissEph.SE_AST_OFFSET + 4)
                ipl = ipl - SwissEph.SE_AST_OFFSET - 1 + SwissEph.SE_CERES;
            iflag = iflag & (SwissEph.SEFLG_EPHMASK |
                             SwissEph.SEFLG_TRUEPOS |
                             SwissEph.SEFLG_J2000 |
                             SwissEph.SEFLG_NONUT |
                             SwissEph.SEFLG_NOGDEFL |
                             SwissEph.SEFLG_NOABERR |
                             SwissEph.SEFLG_TOPOCTR);
            iflagp = iflag & (SwissEph.SEFLG_EPHMASK |
                             SwissEph.SEFLG_TRUEPOS |
                             SwissEph.SEFLG_J2000 |
                             SwissEph.SEFLG_NONUT |
                             SwissEph.SEFLG_NOABERR);
            iflagp |= SwissEph.SEFLG_HELCTR;
            epheflag = iflag & SEFLG_EPHMASK;
            /*  
             * geocentric planet
             */
            if (SE.swe_calc(tjd, (int)ipl, iflag | SwissEph.SEFLG_XYZ, xx, ref serr) == SwissEph.ERR)
                /* int cast can be removed when swe_calc() gets int32 ipl definition */
                return SwissEph.ERR;
            if (SE.swe_calc(tjd, (int)ipl, iflag, lbr, ref serr) == SwissEph.ERR)
                /* int cast can be removed when swe_calc() gets int32 ipl definition */
                return SwissEph.ERR;
            /* if moon, we need sun as well, for magnitude */
            if (ipl == SwissEph.SE_MOON)
                if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflag | SwissEph.SEFLG_XYZ, xxs, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            if (ipl != SwissEph.SE_SUN && ipl != SwissEph.SE_EARTH &&
              ipl != SwissEph.SE_MEAN_NODE && ipl != SwissEph.SE_TRUE_NODE &&
              ipl != SwissEph.SE_MEAN_APOG && ipl != SwissEph.SE_OSCU_APOG) {
                /*
                 * light time planet - earth
                 */
                  dt = lbr[2] * Sweph.AUNIT / Sweph.CLIGHT / 86400.0;
                if ((iflag & SwissEph.SEFLG_TRUEPOS) != 0)
                    dt = 0;
                /* 
                 * heliocentric planet at tjd - dt
                 */
                if (SE.swe_calc(tjd - dt, (int)ipl, iflagp | SwissEph.SEFLG_XYZ, xx2, ref  serr) == SwissEph.ERR)
                    /* int cast can be removed when swe_calc() gets int32 ipl definition */
                    return SwissEph.ERR;
                if (SE.swe_calc(tjd - dt, (int)ipl, iflagp, lbr2, ref  serr) == SwissEph.ERR)
                    /* int cast can be removed when swe_calc() gets int32 ipl definition */
                    return SwissEph.ERR;
                /*
                 * phase angle
                 */
                attr[0] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xx, xx2)) * SwissEph.RADTODEG;
                /*
                 * phase
                 */
                attr[1] = (1 + Math.Cos(attr[0] * SwissEph.DEGTORAD)) / 2;
            }
            /*
             * apparent diameter of disk
             */
            if (ipl < Sweph.NDIAM)
                dd = Sweph.pla_diam[ipl];
            else if (ipl > SwissEph.SE_AST_OFFSET)
                dd = SE.Sweph.swed.ast_diam * 1000;	/* km -> m */
            else
                dd = 0;
            if (lbr[2] < dd / 2 / Sweph.AUNIT)
                attr[3] = 180;  /* assume position on surface of earth */
            else
                attr[3] = Math.Asin(dd / 2 / Sweph.AUNIT / lbr[2]) * 2 * SwissEph.RADTODEG;
            /* 
             * apparent magnitude 
             */
            if (ipl > SwissEph.SE_AST_OFFSET || (ipl < NMAG_ELEM && mag_elem[ipl, 0] < 99)) {
                if (ipl == SwissEph.SE_SUN) {
                    /* ratio apparent diameter : average diameter */
                    fac = attr[3] / (Math.Asin(Sweph.pla_diam[SwissEph.SE_SUN] / 2.0 / Sweph.AUNIT) * 2 * SwissEph.RADTODEG);
                    fac *= fac;
                    attr[4] = mag_elem[ipl, 0] - 2.5 * Math.Log10(fac);
                } else if (ipl == SwissEph.SE_MOON) {
                    /* formula according to Allen, C.W., 1976, Astrophysical Quantities */
                    /*attr[4] = -21.62 + 5 * log10(384410497.8 / EARTH_RADIUS) / log10(10) + 0.026 * Math.Abs(attr[0]) + 0.000000004 * pow(attr[0], 4);*/
                    attr[4] = -21.62 + 5 * Math.Log10(lbr[2] * Sweph.AUNIT / Sweph.EARTH_RADIUS) / Math.Log10(10) + 0.026 * Math.Abs(attr[0]) + 0.000000004 * Math.Pow(attr[0], 4);
                    //#if 0
                    //      /* ratio apparent diameter : average diameter */
                    //      fac = attr[3] / (asin(pla_diam[SE_MOON] / 2.0 / 384400000.0) * 2 * SwissEph.RADTODEG);
                    //      /* distance sun - moon */
                    //      for (i = 0; i < 3; i++)
                    //        xxs[i] -= xx[i];
                    //      dsm =Math.Sqrt(square_sum(xxs));
                    //      /* account for phase and distance of moon: */
                    //      fac *= fac * attr[1];
                    //      /* account for distance of sun from moon: */
                    //      fac *= dsm * dsm;
                    //      attr[4] = mag_elem[ipl,0] - 2.5 * log10(fac);
                    //#endif
                    /*printf("1 = %f, 2 = %f\n", mag, mag2);*/
                } else if (ipl == SwissEph.SE_SATURN) {
                    /* rings are considered according to Meeus, German, p. 329ff. */
                    T = (tjd - dt - Sweph.J2000) / 36525.0;
                    inx = (28.075216 - 0.012998 * T + 0.000004 * T * T) * SwissEph.DEGTORAD;
                    om = (169.508470 + 1.394681 * T + 0.000412 * T * T) * SwissEph.DEGTORAD;
                    sinB = Math.Abs(Math.Sin(inx) * Math.Cos(lbr[1] * SwissEph.DEGTORAD)
                                  * Math.Sin(lbr[0] * SwissEph.DEGTORAD - om)
                                  - Math.Cos(inx) * Math.Sin(lbr[1] * SwissEph.DEGTORAD));
                    u1 = Math.Atan2(Math.Sin(inx) * Math.Tan(lbr2[1] * SwissEph.DEGTORAD)
                                           + Math.Cos(inx) * Math.Sin(lbr2[0] * SwissEph.DEGTORAD - om),
                                      Math.Cos(lbr2[0] * SwissEph.DEGTORAD - om)) * SwissEph.RADTODEG;
                    u2 = Math.Atan2(Math.Sin(inx) * Math.Tan(lbr[1] * SwissEph.DEGTORAD)
                                           + Math.Cos(inx) * Math.Sin(lbr[0] * SwissEph.DEGTORAD - om),
                                      Math.Cos(lbr[0] * SwissEph.DEGTORAD - om)) * SwissEph.RADTODEG;
                    du = SE.swe_degnorm(u1 - u2);
                    if (du > 10)
                        du = 360 - du;
                    attr[4] = 5 * Math.Log10(lbr2[2] * lbr[2])
                                + mag_elem[ipl, 1] * sinB
                                + mag_elem[ipl, 2] * sinB * sinB
                                + mag_elem[ipl, 3] * du
                                + mag_elem[ipl, 0];
                } else if (ipl < SwissEph.SE_CHIRON) {
                    // #ifdef MAG_IAU_1986
                    attr[4] = 5 * Math.Log10(lbr2[2] * lbr[2])
                              + mag_elem[ipl, 1] * attr[0] / 100.0
                              + mag_elem[ipl, 2] * attr[0] * attr[0] / 10000.0
                              + mag_elem[ipl, 3] * attr[0] * attr[0] * attr[0] / 1000000.0
                              + mag_elem[ipl, 0];
                    //#else
                    //#endif
                } else if (ipl < NMAG_ELEM || ipl > SwissEph.SE_AST_OFFSET) { /* asteroids */
                    ph1 = Math.Pow(EULER, -3.33 * Math.Pow(Math.Tan(attr[0] * SwissEph.DEGTORAD / 2), 0.63));
                    ph2 = Math.Pow(EULER, -1.87 * Math.Pow(Math.Tan(attr[0] * SwissEph.DEGTORAD / 2), 1.22));
                    if (ipl < NMAG_ELEM) {    /* main asteroids */
                        me[0] = mag_elem[ipl, 0];
                        me[1] = mag_elem[ipl, 1];
                    } else if (ipl == SwissEph.SE_AST_OFFSET + 1566) {
                        /* Icarus has elements from JPL database */
                        me[0] = 16.9;
                        me[1] = 0.15;
                    } else {      /* other asteroids */
                        me[0] = SE.Sweph.swed.ast_H;
                        me[1] = SE.Sweph.swed.ast_G;
                    }
                    attr[4] = 5 * Math.Log10(lbr2[2] * lbr[2])
                        + me[0]
                        - 2.5 * Math.Log10((1 - me[1]) * ph1 + me[1] * ph2);
                } else { /* ficticious bodies */
                    attr[4] = 0;
                }
            }
            if (ipl != SwissEph.SE_SUN && ipl != SwissEph.SE_EARTH) {
                /* 
                 * elongation of planet
                 */
                if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflag | SwissEph.SEFLG_XYZ, xx2, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                if (SE.swe_calc(tjd, SwissEph.SE_SUN, iflag, lbr2, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                attr[2] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xx, xx2)) * SwissEph.RADTODEG;
            }
            /* horizontal parallax */
            if (ipl == SwissEph.SE_MOON) {
                double sinhp; double[] xm = new double[6];
                /* geocentric horizontal parallax */
                /* Expl.Suppl. to the AA 1984, p.400 */
                if (SE.swe_calc(tjd, (int)ipl, epheflag | SwissEph.SEFLG_TRUEPOS | SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_RADIANS, xm, ref serr) == SwissEph.ERR)
                    /* int cast can be removed when swe_calc() gets int32 ipl definition */
                    return SwissEph.ERR;
                sinhp = Sweph.EARTH_RADIUS / xm[2] / Sweph.AUNIT;
                attr[5] = Math.Asin(sinhp) / SwissEph.DEGTORAD;
                /* topocentric horizontal parallax */
                if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                    if (SE.swe_calc(tjd, (int)ipl, epheflag | SwissEph.SEFLG_XYZ | SwissEph.SEFLG_TOPOCTR, xm, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    if (SE.swe_calc(tjd, (int)ipl, epheflag | SwissEph.SEFLG_XYZ, xx, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    attr[5] = Math.Acos(SE.SwephLib.swi_dot_prod_unit(xm, xx)) / SwissEph.DEGTORAD;
                    //#if 0
                    //      {
                    //      /* Expl. Suppl. to the Astronomical Almanac 1984, p. 400;
                    //       * Does not take into account 
                    //       * - the topocentric distance of the moon
                    //       * - the distance of the observer from the geocenter 
                    //       */
                    //      double tsid, h, e, f = EARTH_OBLATENESS;
                    //      double cosz, sinz, phi;
                    //      /* local apparent sidereal time */
                    //      tsid = swe_sidtime(tjd - swe_deltat_ex(tjd, iflag, serr)) * 15 + swed.topd.geolon;
                    //      /* local hour angle of the moon */
                    //      h = swe_degnorm(tsid - xm[0] / DEGTORAD);
                    //      /* geocentric latitude of the observer */
                    //      e =Math.Sqrt(f * (2 - f));
                    //      phi = atan((1 - e * e) * tan(swed.topd.geolat * DEGTORAD));
                    //      /* sine of geocentric zenith angle of moon */
                    //      cosz = sin(xm[1]) * sin(phi) + cos(xm[1]) * cos(phi) * cos(h * DEGTORAD);
                    //      sinz =Math.Sqrt(1 - cosz * cosz);
                    //      attr[5] = asin(sinz * sinhp / (1 - sinz * sinhp)) / DEGTORAD;
                    //      }
                    //#endif
                }
            }
            return SwissEph.OK;
        }

        public Int32 swe_pheno_ut(double tjd_ut, Int32 ipl, Int32 iflag, double[] attr, ref string serr) {
            double deltat;
            Int32 retflag = SwissEph.OK;
            Int32 epheflag = iflag & SwissEph.SEFLG_EPHMASK;
            if (epheflag == 0)
            {
                epheflag = SwissEph.SEFLG_SWIEPH;
                iflag |= SwissEph.SEFLG_SWIEPH;
            }
            deltat = SE.swe_deltat_ex(tjd_ut, iflag, ref serr);
            retflag = SE.swe_pheno(tjd_ut + deltat, ipl, iflag, attr, ref serr);
            /* if ephe required is not ephe returned, adjust delta t: */
            if ((retflag & SwissEph.SEFLG_EPHMASK) != epheflag)
            {
                deltat = SE.swe_deltat_ex(tjd_ut, retflag, ref serr);
                retflag = SE.swe_pheno(tjd_ut + deltat, ipl, iflag, attr, ref serr);
            }
            return retflag;
        }

        int find_maximum(double y00, double y11, double y2, double dx,
                                out double dxret, out double yret) {
            double a, b, c, x, y;
            c = y11;
            b = (y2 - y00) / 2.0;
            a = (y2 + y00) / 2.0 - c;
            x = -b / 2 / a;
            y = (4 * a * c - b * b) / 4 / a;
            dxret = (x - 1) * dx;
            yret = y;
            return SwissEph.OK;
        }

        int find_zero(double y00, double y11, double y2, double dx,
                                out double dxret, out double dxret2) {
            double a, b, c, x1, x2;
            dxret = dxret2 = 0;
            c = y11;
            b = (y2 - y00) / 2.0;
            a = (y2 + y00) / 2.0 - c;
            if (b * b - 4 * a * c < 0)
                return SwissEph.ERR;
            x1 = (-b + Math.Sqrt(b * b - 4 * a * c)) / 2 / a;
            x2 = (-b - Math.Sqrt(b * b - 4 * a * c)) / 2 / a;
            dxret = (x1 - 1) * dx;
            dxret2 = (x2 - 1) * dx;
            return SwissEph.OK;
        }

        double rdi_twilight(Int32 rsmi) {
            double rdi = 0;
            if ((rsmi & SwissEph.SE_BIT_CIVIL_TWILIGHT) != 0)
                rdi = 6;
            if ((rsmi & SwissEph.SE_BIT_NAUTIC_TWILIGHT) != 0)
                rdi = 12;
            if ((rsmi & SwissEph.SE_BIT_ASTRO_TWILIGHT) != 0)
                rdi = 18;
            return rdi;
        }

        /* rise, set, and meridian transits of sun, moon, planets, and stars
            *
            * tjd_ut	universal time from when on search ought to start
            * ipl		planet number, neglected, if starname is given
            * starname	pointer to string. if a planet, not a star, is
            *              wanted, starname must be NULL or ""
            * epheflag	used for ephemeris only
            * rsmi		SE_CALC_RISE, SE_CALC_SET, SE_CALC_MTRANSIT, SE_CALC_ITRANSIT
            *              | SE_BIT_DISC_CENTER      for rises of disc center of body
            *              | SE_BIT_DISC_BOTTOM    for rises of disc bottom of body
            *              | SE_BIT_NO_REFRACTION    to neglect refraction
            *              | SE_BIT_FIXED_DISC_SIZE  neglect the effect of distance on disc size
            * geopos	array of doubles for geogr. long., lat. and height above sea
            * atpress	atmospheric pressure
            * attemp	atmospheric temperature
            *
            * return variables:
            * tret         time of rise, set, meridian transits
            * serr[256]	error string
            * function return value -2 means that the body does not rise or set */
        //#define SEFLG_EPHMASK	(SEFLG_JPLEPH|SEFLG_SWIEPH|SEFLG_MOSEPH)
        public Int32 swe_rise_trans(
                       double tjd_ut, Int32 ipl, string starname,
                   Int32 epheflag, Int32 rsmi,
                       double[] geopos,
                   double atpress, double attemp,
                       ref double tret,
                       ref string serr) {
            return swe_rise_trans_true_hor(tjd_ut, ipl, starname, epheflag, rsmi, geopos, atpress, attemp, 0, ref tret, ref serr);
        }

        /* same as swe_rise_trans(), but allows to define the height of the horizon
         * at the point of the rising or setting (horhgt) */
        public Int32 swe_rise_trans_true_hor(
                       double tjd_ut, Int32 ipl, string starname,
                   Int32 epheflag, Int32 rsmi,
                       double[] geopos,
                   double atpress, double attemp,
                   double horhgt,
                       ref double tret,
                       ref string serr) {
            int i, j, k, ii, calc_culm, nculm = -1;
            double tjd_et = tjd_ut + SE.swe_deltat_ex(tjd_ut, epheflag, ref serr);
            double[] xc = new double[6], ah = new double[6];
            double[][] xh = CreateArray<double>(20, 6);
            double[] tculm = new double[4], tc = new double[20], h = new double[20], t2 = new double[6], dc = new double[6];
            double aha, tcu, dtint, dx, rdi, dd = 0;
            Int32 iflag = epheflag;
            int jmax = 14;
            double t, te, tt, dt, twohrs = 1.0 / 12.0;
            double curdist;
            bool do_fixstar = !String.IsNullOrEmpty(starname);
            if (geopos[2] < Sweph.SEI_ECL_GEOALT_MIN || geopos[2] > Sweph.SEI_ECL_GEOALT_MAX)
            {
                serr = C.sprintf("location for swe_rise_trans() must be between %.0f and %.0f m above sea", Sweph.SEI_ECL_GEOALT_MIN, Sweph.SEI_ECL_GEOALT_MAX);
                return Sweph.ERR;
            }
            /*SE.SwephLib.swi_set_tid_acc(tjd_ut, epheflag, 0, ref serr);*/
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            xh[0][0] = 0; /* to shut up mint */
            /* allowing SEFLG_NONUT and SEFLG_TRUEPOS speeds it up */
            iflag &= (SEFLG_EPHMASK | SwissEph.SEFLG_NONUT | SwissEph.SEFLG_TRUEPOS);
            tret = 0;
            iflag |= (SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_TOPOCTR);
            SE.swe_set_topo(geopos[0], geopos[1], geopos[2]);
            if ((rsmi & (SwissEph.SE_CALC_MTRANSIT | SwissEph.SE_CALC_ITRANSIT)) != 0)
                return calc_mer_trans(tjd_ut, ipl, epheflag, rsmi,
                    geopos, starname, ref tret, ref serr);
            if (0 == (rsmi & (SwissEph.SE_CALC_RISE | SwissEph.SE_CALC_SET)))
                rsmi |= SwissEph.SE_CALC_RISE;
            /* twilight calculation */
            if (ipl == SwissEph.SE_SUN && (rsmi & (SwissEph.SE_BIT_CIVIL_TWILIGHT | SwissEph.SE_BIT_NAUTIC_TWILIGHT | SwissEph.SE_BIT_ASTRO_TWILIGHT)) != 0) {
                rsmi |= (SwissEph.SE_BIT_NO_REFRACTION | SwissEph.SE_BIT_DISC_CENTER);
                horhgt = -rdi_twilight(rsmi);
                /* note: twilight is not dependent on height of horizon, so we can
                 * use this parameter and define a fictitious height of horizon */
            }
            /* find culmination points within 28 hours from t0 - twohrs.
             * culminations are required in case there are maxima or minima
             * in height slightly above or below the horizon.
             * we do not use meridian transits, because in polar regions
             * the culmination points may considerably deviate from
             * transits. also, there are cases where the moon rises in the
             * western half of the sky for a short time. 
             */
            if (do_fixstar) {
                if (SE.swe_fixstar(starname, tjd_et, iflag, xc, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            for (ii = 0, t = tjd_ut - twohrs; ii <= jmax; ii++, t += twohrs) {
                tc[ii] = t;
                if (!do_fixstar) {
                    te = t + SE.swe_deltat_ex(t, epheflag, ref serr);
                    if (SE.swe_calc(te, ipl, iflag, xc, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                }
                /* diameter of object in km */
                if (ii == 0) {
                    if (do_fixstar)
                        dd = 0;
                    else if ((rsmi & SwissEph.SE_BIT_DISC_CENTER) != 0)
                        dd = 0;
                    else if (ipl < Sweph.NDIAM)
                        dd = Sweph.pla_diam[ipl];
                    else if (ipl > SwissEph.SE_AST_OFFSET)
                        dd = SE.Sweph.swed.ast_diam * 1000;	/* km -> m */
                    else
                        dd = 0;
                }
                curdist = xc[2];
                if ((rsmi & SwissEph.SE_BIT_FIXED_DISC_SIZE) != 0) {
                    if (ipl == SwissEph.SE_SUN) {
                        curdist = 1.0;
                    } else if (ipl == SwissEph.SE_MOON) {
                        curdist = 0.00257;
                    }
                }
                /* apparent radius of disc */
                rdi = Math.Asin(dd / 2 / Sweph.AUNIT / curdist) * SwissEph.RADTODEG;
                /* true height of center of body */
                swe_azalt(t, SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, xh[ii]);
                if ((rsmi & SwissEph.SE_BIT_DISC_BOTTOM) != 0) {
                    /* true height of bottom point of body */
                    xh[ii][1] -= rdi;
                } else {
                    /* true height of uppermost point of body */
                    xh[ii][1] += rdi;
                }
                /* apparent height of uppermost point of body */
                if ((rsmi & SwissEph.SE_BIT_NO_REFRACTION) != 0) {
                    xh[ii][1] -= horhgt;
                    h[ii] = xh[ii][1];
                } else {
                    swe_azalt_rev(t, SwissEph.SE_HOR2EQU, geopos, xh[ii], xc);
                    swe_azalt(t, SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, xh[ii]);
                    xh[ii][1] -= horhgt;
                    xh[ii][2] -= horhgt;
                    h[ii] = xh[ii][2];
                }
                calc_culm = 0;
                if (ii > 1) {
                    dc[0] = xh[ii - 2][1];
                    dc[1] = xh[ii - 1][1];
                    dc[2] = xh[ii][1];
                    if (dc[1] > dc[0] && dc[1] > dc[2])
                        calc_culm = 1;
                    if (dc[1] < dc[0] && dc[1] < dc[2])
                        calc_culm = 2;
                }
                if (calc_culm != 0) {
                    dt = twohrs;
                    tcu = t - dt;
                    find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dx);
                    tcu += dtint + dt;
                    dt /= 3;
                    for (; dt > 0.0001; dt /= 3) {
                        for (i = 0, tt = tcu - dt; i < 3; tt += dt, i++) {
                            te = tt + SE.swe_deltat_ex(tt, epheflag, ref serr);
                            if (!do_fixstar)
                                if (SE.swe_calc(te, ipl, iflag, xc, ref serr) == SwissEph.ERR)
                                    return SwissEph.ERR;
                            swe_azalt(tt, SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, ah);
                            ah[1] -= horhgt;
                            dc[i] = ah[1];
                        }
                        find_maximum(dc[0], dc[1], dc[2], dt, out dtint, out dx);
                        tcu += dtint + dt;
                    }
                    nculm++;
                    tculm[nculm] = tcu;
                }
            }
            /* note: there can be a rise or set on the poles, even if 
             * there is no culmination. So, we must not leave here
             * in any case. */
            /* insert culminations into array of heights */
            for (i = 0; i <= nculm; i++) {
                for (j = 1; j <= jmax; j++) {
                    if (tculm[i] < tc[j]) {
                        for (k = jmax; k >= j; k--) {
                            tc[k + 1] = tc[k];
                            h[k + 1] = h[k];
                        }
                        tc[j] = tculm[i];
                        if (!do_fixstar) {
                            te = tc[j] + SE.swe_deltat_ex(tc[j], epheflag, ref serr);
                            if (SE.swe_calc(te, ipl, iflag, xc, ref serr) == SwissEph.ERR)
                                return SwissEph.ERR;
                        }
                        curdist = xc[2];
                        if ((rsmi & SwissEph.SE_BIT_FIXED_DISC_SIZE) != 0) {
                            if (ipl == SwissEph.SE_SUN) {
                                curdist = 1.0;
                            } else if (ipl == SwissEph.SE_MOON) {
                                curdist = 0.00257;
                            }
                        }
                        /* apparent radius of disc */
                        rdi = Math.Asin(dd / 2 / Sweph.AUNIT / curdist) * SwissEph.RADTODEG;
                        /* true height of center of body */
                        swe_azalt(tc[j], SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, ah);
                        if ((rsmi & SwissEph.SE_BIT_DISC_BOTTOM) != 0) {
                            /* true height of bottom point of body */
                            ah[1] -= rdi;
                        } else {
                            /* true height of uppermost point of body */
                            ah[1] += rdi;
                        }
                        /* apparent height of uppermost point of body */
                        if ((rsmi & SwissEph.SE_BIT_NO_REFRACTION) != 0) {
                            ah[1] -= horhgt;
                            h[j] = ah[1];
                        } else {
                            swe_azalt_rev(tc[j], SwissEph.SE_HOR2EQU, geopos, ah, xc);
                            swe_azalt(tc[j], SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, ah);
                            ah[1] -= horhgt;
                            ah[2] -= horhgt;
                            h[j] = ah[2];
                        }
                        jmax++;
                        break;
                    }
                }
            }
            tret = 0;
            /* find points with zero height. 
             * binary search */
            for (ii = 1; ii <= jmax; ii++) {
                if (h[ii - 1] * h[ii] >= 0)
                    continue;
                if (h[ii - 1] < h[ii] && 0 == (rsmi & SwissEph.SE_CALC_RISE))
                    continue;
                if (h[ii - 1] > h[ii] && 0 == (rsmi & SwissEph.SE_CALC_SET))
                    continue;
                dc[0] = h[ii - 1];
                dc[1] = h[ii];
                t2[0] = tc[ii - 1];
                t2[1] = tc[ii];
                for (i = 0; i < 20; i++) {
                    t = (t2[0] + t2[1]) / 2;
                    if (!do_fixstar) {
                        te = t + SE.swe_deltat_ex(t, epheflag, ref serr);
                        if (SE.swe_calc(te, ipl, iflag, xc, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                    }
                    curdist = xc[2];
                    if ((rsmi & SwissEph.SE_BIT_FIXED_DISC_SIZE) != 0) {
                        if (ipl == SwissEph.SE_SUN) {
                            curdist = 1.0;
                        } else if (ipl == SwissEph.SE_MOON) {
                            curdist = 0.00257;
                        }
                    }
                    /* apparent radius of disc */
                    rdi = Math.Asin(dd / 2 / Sweph.AUNIT / curdist) * SwissEph.RADTODEG;
                    /* true height of center of body */
                    swe_azalt(t, SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, ah);
                    if ((rsmi & SwissEph.SE_BIT_DISC_BOTTOM) != 0) {
                        /* true height of bottom point of body */
                        ah[1] -= rdi;
                    } else {
                        /* true height of uppermost point of body */
                        ah[1] += rdi;
                    }
                    /* apparent height of uppermost point of body */
                    if ((rsmi & SwissEph.SE_BIT_NO_REFRACTION) != 0) {
                        ah[1] -= horhgt;
                        aha = ah[1];
                    } else {
                        swe_azalt_rev(t, SwissEph.SE_HOR2EQU, geopos, ah, xc);
                        swe_azalt(t, SwissEph.SE_EQU2HOR, geopos, atpress, attemp, xc, ah);
                        ah[1] -= horhgt;
                        ah[2] -= horhgt;
                        aha = ah[2];
                    }
                    if (aha * dc[0] <= 0) {
                        dc[1] = aha;
                        t2[1] = t;
                    } else {
                        dc[0] = aha;
                        t2[0] = t;
                    }
                }
                if (t > tjd_ut) {
                    tret = t;
                    return SwissEph.OK;
                }
            }
            serr = C.sprintf("rise or set not found for planet %d", ipl);
            return -2; /* no t of rise or set found */
        }

        Int32 calc_mer_trans(
                       double tjd_ut, Int32 ipl, Int32 epheflag, Int32 rsmi,
                       double[] geopos,
                       string starname,
                       ref double tret,
                       ref string serr) {
            int i;
            double tjd_et = tjd_ut + SE.swe_deltat_ex(tjd_ut, epheflag, ref serr);
            double armc, armc0, arxc, t, te; double[] x0 = new double[6], x = new double[6];
            double mdd;
            Int32 iflag = epheflag;
            bool do_fixstar = !String.IsNullOrEmpty(starname);
            iflag &= SEFLG_EPHMASK;
            tret = 0;
            iflag |= (SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_TOPOCTR);
            armc0 = SE.swe_sidtime(tjd_ut) + geopos[0] / 15;
            if (armc0 >= 24)
                armc0 -= 24;
            if (armc0 < 0)
                armc0 += 24;
            armc0 *= 15;
            if (do_fixstar) {
                if (SE.swe_fixstar(starname, tjd_et, iflag, x0, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            } else {
                if (SE.swe_calc(tjd_et, ipl, iflag, x0, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            /*
             * meridian transits
             */
            x[0] = x0[0];
            x[1] = x0[1];
            t = tjd_ut;
            arxc = armc0;
            if ((rsmi & SwissEph.SE_CALC_ITRANSIT) != 0)
                arxc = SE.swe_degnorm(arxc + 180);
            for (i = 0; i < 4; i++) {
                mdd = SE.swe_degnorm(x[0] - arxc);
                if (i > 0 && mdd > 180)
                    mdd -= 360;
                t += mdd / 361;
                armc = SE.swe_sidtime(t) + geopos[0] / 15;
                if (armc >= 24)
                    armc -= 24;
                if (armc < 0)
                    armc += 24;
                armc *= 15;
                arxc = armc;
                if ((rsmi & SwissEph.SE_CALC_ITRANSIT) != 0)
                    arxc = SE.swe_degnorm(arxc + 180);
                if (!do_fixstar) {
                    te = t + SE.swe_deltat_ex(t, epheflag, ref serr);
                    if (SE.swe_calc(te, ipl, iflag, x, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                }
            }
            tret = t;
            return SwissEph.OK;
        }

        /*
        Nodes and apsides of planets and moon

        Planetary nodes can be defined in three different ways:
        a) They can be understood as a direction or as an axis 
          defined by the intersection line of two orbital planes. 
          E.g., the nodes of Mars are defined by the intersection 
          line of Mars' orbital plane with the ecliptic (= the 
          Earths orbit heliocentrically or the solar orbit 
          geocentrically). However, as Michael Erlewine points 
          out in his elaborate web page on this topic 
          (http://thenewage.com/resources/articles/interface.html),
          planetary nodes can be defined for any couple of 
          planets. E.g. there is also an intersection line for the 
          two orbital planes of Mars and Saturn.
          Because such lines are, in principle, infinite, the 
          heliocentric and the geocentric positions of the 
          planetary nodes will be the same. There are astrologers 
          that use such heliocentric planetary nodes in geocentric 
          charts.
          The ascending and the descending node will, in this 
          case, be in precise opposition.

        b) The planetary nodes can also be understood in a 
          different way, not as an axis, but as the two points on a 
          planetary orbit that are located precisely on the 
          intersection line of the two planes.
          This second definition makes no difference for the moon or 
          for heliocentric positions of planets, but it does so for 
          geocentric positions. There are two possibilities for 
          geocentric planetary nodes based on this definition. 
          1) The common solution is that the points on the 
            planets orbit are transformed to the geocenter. The 
            two points will not be in opposition anymore, or 
            they will only roughly be so with the outer planets. The 
            advantage of these nodes is that when a planet is in 
            conjunction with its node, then its ecliptic latitude 
            will be zero. This is not true when a planet is in 
            geocentric conjunction with its heliocentric node. 
            (And neither is it always true for the inner planets, 
            i.e. Mercury and Venus.)
          2) The second possibility that nobody seems to have 
            thought of so far: One may compute the points of 
            the earth's orbit that lie exactly on another planet's 
            orbital plane and transform it to the geocenter. The two 
            points will always be in an approximate square.

        c) Third, the planetary nodes could be defined as the 
          intersection points of the plane defined by their 
          momentary geocentric position and motion with the 
          plane of the ecliptic. Such points would move very fast 
          around the planetary stations. Here again, as in b)1), 
          the planet would cross the ecliptic and its ecliptic 
          latitude would be 0 exactly when it were in 
          conjunction with one of its nodes.

        The Swiss Ephemeris supports the solutions a) and b) 1).

        Possible definitions for apsides

        a) The planetary apsides can be defined as the perihelion and
          aphelion points on a planetary orbit. For a
          geocentric chart, these points could be transformed 
          from the heliocenter to the geocenter.
        b) However, one might consider these points as 
          astrologically relevant axes rather than as points on a 
          planetary orbit. Again, this would allow heliocentric 
          positions in a geocentric chart.

        Note: For the "Dark Moon" or "Lilith", which I usually 
        define as the lunar apogee, some astrologers give a 
        different definition. They understand it as the second focal 
        point of the moon's orbital ellipse. This definition does not 
        make a difference for geocentric positions, because the 
        apogee and the second focus are in exactly the same geocentric 
        direction. However, it makes a difference with topocentric 
        positions, because the two points do not have same distance. 
        Analogous "black planets" have been proposed: they would be the 
        second focal points of the planets' orbital ellipses. The 
        heliocentric positions of these "black planets" are identical 
        with the heliocentric positions of the aphelia, but geocentric 
        positions are not identical, because the focal points are 
        much closer to the sun than the aphelia.

        The Swiss Ephemeris allows to compute the "black planets" as well.

        Mean positions

        Mean nodes and apsides can be computed for the Moon, the 
        Earth and the planets Mercury - Neptune. They are taken 
        from the planetary theory VSOP87. Mean points can not be 
        calculated for Pluto and the asteroids, because there is no 
        planetary theory for them.
 
        Osculating nodes and apsides

        Nodes and apsides can also be derived from the osculating 
        orbital elements of a body, the paramaters that define an  
        ideal unperturbed elliptic (two-body) orbit. 
        For astrology, note that this is a simplification and 
        idealization. 
        Problem with Neptune: Neptune's orbit around the sun does not 
        have much in common with an ellipse. There are often two 
        perihelia and two aphelia within one revolution. As a result, 
        there is a wild oscillation of the osculating perihelion (and 
        aphelion). 
        In actuality, Neptune's orbit is not heliocentric orbit at all. 
        The twofold perihelia and aphelia are an effect of the motion of 
        the sun about the solar system barycenter. This motion is 
        much faster than the motion of Neptune, and Neptune 
        cannot react on such fast displacements of the Sun. As a 
        result, Neptune seems to move around the barycenter (or a 
        mean sun) rather than around the true sun. In fact, 
        Neptune's orbit around the barycenter is therefore closer to 
        an ellipse than the his orbit around the sun. The same 
        statement is also true for Saturn, Uranus and Pluto, but not 
        for Jupiter and the inner planets.

        This fundamental problem about osculating ellipses of 
        planetary orbits does of course not only affect the apsides 
        but also the nodes.

        Two solutions can be thought of for this problem: 
        1) The one would be to interpolate between actual 
          passages of the planets through their nodes and 
          apsides. However, this works only well with Mercury. 
          With all other planets, the supporting points are too far 
          apart as to make an accurate interpolation possible. 
          This solution is not implemented, here.
        2) The other solution is to compute the apsides of the 
          orbit around the barycenter rather than around the sun. 
          This procedure makes sense for planets beyond Jupiter, 
          it comes closer to the mean apsides and nodes for 
          planets that have such points defined. For all other 
          transsaturnian planets and asteroids, this solution yields 
          a kind of "mean" nodes and apsides. On the other hand, 
          the barycentric ellipse does not make any sense for 
          inner planets and Jupiter.

        The Swiss Ephemeris supports solution 2) for planets and 
        asteroids beyond Jupiter.

        Anyway, neither the heliocentric nor the barycentric ellipse 
        is a perfect representation of the nature of a planetary orbit, 
        and it will not yield the degree of precision that today's 
        astrology is used to.
        The best choice of method will probably be:
        - For Mercury - Neptune: mean nodes and apsides
        - For asteroids that belong to the inner asteroid belt: 
        osculating nodes/apsides from a heliocentric ellipse
        - For Pluto and outer asteroids: osculating nodes/apsides 
        from a barycentric ellipse

        The Moon is a special case: A "lunar true node" makes 
        more sense, because it can be defined without the idea of an 
        ellipse, e.g. as the intersection axis of the momentary lunar 
        orbital plane with the ecliptic. Or it can be said that the 
        momentary motion of the moon points to one of the two 
        ecliptic points that are called the "true nodes".  So, these 
        points make sense. With planetary nodes, the situation is 
        somewhat different, at least if we make a difference 
        between heliocentric and geocentric positions. If so, the 
        planetary nodes are points on a heliocentric orbital ellipse, 
        which are transformed to the geocenter. An ellipse is 
        required here, because a solar distance is required. In 
        contrast to the planetary nodes, the lunar node does not 
        require a distance, therefore manages without the idea of an 
        ellipse and does not share its weaknesses. 
        On the other hand, the lunar apsides DO require the idea of 
        an ellipse. And because the lunar ellipse is actually 
        extremely distorted, even more than any other celestial 
        ellipse, the "true Lilith" (apogee), for which printed 
        ephemerides are available, does not make any sense at all. 
        (See the chapter on the lunar node and apogee.)

        Special case: the Earth

        The Earth is another special case. Instead of the motion of 
        the Earth herself, the heliocentric motion of the Earth-
        Moon-Barycenter (EMB) is used to determine the 
        osculating perihelion. 
        There is no node of the earth orbit itself. However, there is 
        an axis around which the earth's orbital plane slowly rotates 
        due to planetary precession. The position points of this axis 
        are not calculated by the Swiss Ephemeris.

        Special case: the Sun

        In addition to the Earth (EMB) apsides, the function 
        computes so-to-say "apsides" of the sun, i.e. points on the 
        orbit of the Sun where it is closest to and where it is farthest 
        from the Earth. These points form an opposition and are 
        used by some astrologers, e.g. by the Dutch astrologer 
        George Bode or the Swiss astrologer Liduina Schmed. The 
        perigee, located at about 13 Capricorn, is called the 
        "Black Sun", the other one, in Cancer, the "Diamond".
        So, for a complete set of apsides, one ought to calculate 
        them for the Sun and the Earth and all other planets. 

        The modes of the Swiss Ephemeris function 
        swe_nod_aps()

        The  function swe_nod_aps() can be run in the following 
        modes:
        1) Mean positions are given for nodes and apsides of Sun, 
          Moon, Earth, and the up to Neptune. Osculating 
          positions are given with Pluto and all asteroids. This is 
          the default mode.
        2) Osculating positions are returned for nodes and apsides 
          of all planets.
        3) Same as 2), but for planets and asteroids beyond 
          Jupiter, a barycentric ellipse is used.
        4) Same as 1), but for Pluto and asteroids beyond Jupiter, 
          a barycentric ellipse is used.

        In all of these modes, the second focal point of the ellipse 
        can be computed instead of the aphelion.
        Like the planetary function swe_calc(), swe_nod_aps() is 
        able to return geocentric, topocentric, heliocentric, or 
        barycentric position.
         *
         * tjd_ut	 julian day, ephemeris time
         * ipl		 planet number
         * iflag	 as usual, SEFLG_HELCTR, etc.
         * xnasc         an array of 6 doubles: ascending node
         * xndsc         an array of 6 doubles: ascending node
         * xperi         an array of 6 doubles: perihelion
         * xaphe         an array of 6 doubles: aphelion
         * method        see below
         * serr          error message
         *
         * method        can have the following values:
         *               - 0 or SE_NODBIT_MEAN. MEAN positions are given for
         *                 nodes and apsides of Sun, Moon, Earth, and the 
         *                 planets up to Neptune. Osculating positions are
         *                 given with Pluto and all asteroids.
         *               - SE_NODBIT_OSCU. Osculating positions are given
         *                 for all nodes and apsides.
         *               - SE_NODBIT_OSCU_BAR. Osculating nodes and apsides
         *                 are computed from barycentric ellipses, for planets
         *                 beyond Jupiter, but from heliocentric ones for
         *                 ones for Jupiter and inner planets.
         *               - SE_NODBIT_MEAN and SE_NODBIT_OSCU_BAR can be combined.
         *                 The program behaves the same way as with simple 
         *                 SE_NODBIT_MEAN, but uses barycentric ellipses for
         *                 planets beyond Neptune and asteroids beyond Jupiter.
         *               - SE_NODBIT_FOCAL can be combined with any of the other
         *                 bits. The second focal points of the ellipses will 
         *                 be returned instead of the aphelia.
         */
        /* mean elements for Mercury - Neptune from VSOP87 (mean equinox of date) */
        static readonly double[][] el_node = CreateArray(new double[8, 4]
          {{ 48.330893,  1.1861890,  0.00017587,  0.000000211,}, /* Mercury */
          { 76.679920,  0.9011190,  0.00040665, -0.000000080,}, /* Venus   */
          {  0       ,  0        ,  0         ,  0          ,}, /* Earth   */
          { 49.558093,  0.7720923,  0.00001605,  0.000002325,}, /* Mars    */
          {100.464441,  1.0209550,  0.00040117,  0.000000569,}, /* Jupiter */
          {113.665524,  0.8770970, -0.00012067, -0.000002380,}, /* Saturn  */
          { 74.005947,  0.5211258,  0.00133982,  0.000018516,}, /* Uranus  */
          {131.784057,  1.1022057,  0.00026006, -0.000000636,}, /* Neptune */
          });
        static readonly double[][] el_peri = CreateArray(new double[8, 4] 
          {{ 77.456119,  1.5564775,  0.00029589,  0.000000056,}, /* Mercury */
          {131.563707,  1.4022188, -0.00107337, -0.000005315,}, /* Venus   */
          {102.937348,  1.7195269,  0.00045962,  0.000000499,}, /* Earth   */
          {336.060234,  1.8410331,  0.00013515,  0.000000318,}, /* Mars    */
          { 14.331309,  1.6126668,  0.00103127, -0.000004569,}, /* Jupiter */
          { 93.056787,  1.9637694,  0.00083757,  0.000004899,}, /* Saturn  */
          {173.005159,  1.4863784,  0.00021450,  0.000000433,}, /* Uranus  */
          { 48.123691,  1.4262677,  0.00037918, -0.000000003,}, /* Neptune */
          });
        static readonly double[][] el_incl = CreateArray(new double[8, 4] 
          {{  7.004986,  0.0018215, -0.00001809,  0.000000053,}, /* Mercury */
          {  3.394662,  0.0010037, -0.00000088, -0.000000007,}, /* Venus   */
          {  0,         0,          0,           0          ,}, /* Earth   */
          {  1.849726, -0.0006010,  0.00001276, -0.000000006,}, /* Mars    */
          {  1.303270, -0.0054966,  0.00000465, -0.000000004,}, /* Jupiter */
          {  2.488878, -0.0037363, -0.00001516,  0.000000089,}, /* Saturn  */
          {  0.773196,  0.0007744,  0.00003749, -0.000000092,}, /* Uranus  */
          {  1.769952, -0.0093082, -0.00000708,  0.000000028,}, /* Neptune */
          });
        static readonly double[][] el_ecce = CreateArray(new double[8, 4] 
          {{  0.20563175,  0.000020406, -0.0000000284, -0.00000000017,}, /* Mercury */
          {  0.00677188, -0.000047766,  0.0000000975,  0.00000000044,}, /* Venus   */
          {  0.01670862, -0.000042037, -0.0000001236,  0.00000000004,}, /* Earth   */
          {  0.09340062,  0.000090483, -0.0000000806, -0.00000000035,}, /* Mars    */
          {  0.04849485,  0.000163244, -0.0000004719, -0.00000000197,}, /* Jupiter */
          {  0.05550862, -0.000346818, -0.0000006456,  0.00000000338,}, /* Saturn  */
          {  0.04629590, -0.000027337,  0.0000000790,  0.00000000025,}, /* Uranus  */
          {  0.00898809,  0.000006408, -0.0000000008, -0.00000000005,}, /* Neptune */
          });
        static readonly double[][] el_sema = CreateArray(new double[8, 4] 
          {{  0.387098310,  0.0,  0.0,  0.0,}, /* Mercury */
          {  0.723329820,  0.0,  0.0,  0.0,}, /* Venus   */
          {  1.000001018,  0.0,  0.0,  0.0,}, /* Earth   */
          {  1.523679342,  0.0,  0.0,  0.0,}, /* Mars    */
          {  5.202603191,  0.0000001913,  0.0,  0.0,}, /* Jupiter */
          {  9.554909596,  0.0000021389,  0.0,  0.0,}, /* Saturn  */
          { 19.218446062, -0.0000000372,  0.00000000098,  0.0,}, /* Uranus  */
          { 30.110386869, -0.0000001663,  0.00000000069,  0.0,}, /* Neptune */
          });
        /* Ratios of mass of Sun to masses of the planets */
        static readonly double[] plmass = new double[9]  {
            6023600,        /* Mercury */
             408523.5,      /* Venus */
             328900.5,      /* Earth and Moon */
            3098710,        /* Mars */
               1047.350,    /* Jupiter */
               3498.0,      /* Saturn */
              22960,        /* Uranus */
              19314,        /* Neptune */
          130000000,        /* Pluto */
        };
        static readonly int[] ipl_to_elem = new int[15] { 2, 0, 0, 1, 3, 4, 5, 6, 7, 0, 0, 0, 0, 0, 2, };
        public Int32 swe_nod_aps(double tjd_et, Int32 ipl, Int32 iflag,
                              Int32 method,
                              double[] xnasc, double[] xndsc,
                              double[] xperi, double[] xaphe,
                              ref string serr) {
            int ij, i, j;
            Int32 iplx;
            Int32 ipli;
            int istart, iend;
            Int32 iflJ2000;
            double daya, plm;
            double t = (tjd_et - Sweph.J2000) / 36525, dt;
            double[] x = new double[6], xx = new double[24], xobs = new double[6], x2000 = new double[6];
            double[][] xpos = CreateArray<double>(3, 6); double[] xnorm = new double[6];
            double[] xposm = new double[6];
            double[][] xn = CreateArray<double>(3, 6), xs = CreateArray<double>(3, 6);
            double[][] xq = CreateArray<double>(3, 6), xa = CreateArray<double>(3, 6);
            double[] xobs2 = new double[6], x2 = new double[6];
            CPointer<double> xna, xnd, xpe, xap, xp;
            double incl, sema, ecce, parg, ea, vincl, vsema, vecce, pargx, eax;
            Sweph.plan_data pedp = SE.Sweph.swed.pldat[Sweph.SEI_EARTH];
            Sweph.plan_data psbdp = SE.Sweph.swed.pldat[Sweph.SEI_SUNBARY];
            Sweph.plan_data pldat = new Sweph.plan_data();
            CPointer<double> xsun = psbdp.x;
            CPointer<double> xear = pedp.x;
            CPointer<double> ep;
            double Gmsm, dzmin;
            double rxy, rxyz, fac, sgn;
            double sinnode, cosnode, sinincl, cosincl, sinu, cosu, sinE, cosE, cosE2;
            double uu, ny, ny2, c2, v2, pp, ro, ro2, rn, rn2;
            Sweph.epsilon oe;
            bool is_true_nodaps = false;
            bool do_aberr = 0 == (iflag & (SwissEph.SEFLG_TRUEPOS | SwissEph.SEFLG_NOABERR));
            bool do_defl = 0 == (iflag & SwissEph.SEFLG_TRUEPOS) && 0 == (iflag & SwissEph.SEFLG_NOGDEFL);
            bool do_focal_point = (method & SwissEph.SE_NODBIT_FOPOINT) != 0;
            bool ellipse_is_bary = false;
            Int32 iflg0;
            iflag &= ~(SwissEph.SEFLG_JPLHOR | SwissEph.SEFLG_JPLHOR_APPROX);
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            xna = xx;
            xnd = xx.GetPointer(6);
            xpe = xx.GetPointer(12);
            xap = xx.GetPointer(18);
            xpos[0][0] = 0; /* to shut up mint */
            /* to get control over the save area: */
            SE.Sweph.swi_force_app_pos_etc();
            method %= SwissEph.SE_NODBIT_FOPOINT;
            ipli = ipl;
            if (ipl == SwissEph.SE_SUN)
                ipli = SwissEph.SE_EARTH;
            if (ipl == SwissEph.SE_MOON) {
                do_defl = false;
                if (0 == (iflag & SwissEph.SEFLG_HELCTR))
                    do_aberr = false;
            }
            iflg0 = (iflag & (SEFLG_EPHMASK | SwissEph.SEFLG_NONUT)) | SwissEph.SEFLG_SPEED | SwissEph.SEFLG_TRUEPOS;
            if (ipli != SwissEph.SE_MOON)
                iflg0 |= SwissEph.SEFLG_HELCTR;
            if (ipl == SwissEph.SE_MEAN_NODE || ipl == SwissEph.SE_TRUE_NODE ||
                ipl == SwissEph.SE_MEAN_APOG || ipl == SwissEph.SE_OSCU_APOG ||
                ipl < 0 ||
                (ipl >= SwissEph.SE_NPLANETS && ipl <= SwissEph.SE_AST_OFFSET)) {
                /*(ipl >= SE_FICT_OFFSET && ipl - SE_FICT_OFFSET < SE_NFICT_ELEM)) */
                serr = C.sprintf("nodes/apsides for planet %5.0f are not implemented", (double)ipl);
                if (xnasc != null)
                    for (i = 0; i <= 5; i++)
                        xnasc[i] = 0;
                if (xndsc != null)
                    for (i = 0; i <= 5; i++)
                        xndsc[i] = 0;
                if (xaphe != null)
                    for (i = 0; i <= 5; i++)
                        xaphe[i] = 0;
                if (xperi != null)
                    for (i = 0; i <= 5; i++)
                        xperi[i] = 0;
                return SwissEph.ERR;
            }
            for (i = 0; i < 24; i++)
                xx[i] = 0;
            /***************************************
             * mean nodes and apsides 
             ***************************************/
            /* mean points only for Sun - Neptune */
            if ((method == 0 || (method & SwissEph.SE_NODBIT_MEAN) != 0) &&
                  ((ipl >= SwissEph.SE_SUN && ipl <= SwissEph.SE_NEPTUNE) || ipl == SwissEph.SE_EARTH)) {
                      if (ipl == SwissEph.SE_MOON) {
                    double[] xdummy = new double[] { xna[0], xna[3], xpe[0], xpe[3] };
                    SE.SwemMoon.swi_mean_lunar_elements(tjd_et, ref xdummy[0], ref xdummy[1], ref xdummy[2], ref xdummy[3]);
                    xna[0] = xdummy[0]; xna[3] = xdummy[1]; xpe[0] = xdummy[2]; xpe[3] = xdummy[3];
                    incl = Sweph.MOON_MEAN_INCL;
                    vincl = 0;
                    ecce = Sweph.MOON_MEAN_ECC;
                    vecce = 0;
                    sema = Sweph.MOON_MEAN_DIST / Sweph.AUNIT;
                    vsema = 0;
                } else {
                    iplx = ipl_to_elem[ipl];
                    ep = el_incl[iplx];
                    incl = ep[0] + ep[1] * t + ep[2] * t * t + ep[3] * t * t * t;
                    vincl = ep[1] / 36525;
                    ep = el_sema[iplx];
                    sema = ep[0] + ep[1] * t + ep[2] * t * t + ep[3] * t * t * t;
                    vsema = ep[1] / 36525;
                    ep = el_ecce[iplx];
                    ecce = ep[0] + ep[1] * t + ep[2] * t * t + ep[3] * t * t * t;
                    vecce = ep[1] / 36525;
                    ep = el_node[iplx];
                    /* ascending node */
                    xna[0] = ep[0] + ep[1] * t + ep[2] * t * t + ep[3] * t * t * t;
                    xna[3] = ep[1] / 36525;
                    /* perihelion */
                    ep = el_peri[iplx];
                    xpe[0] = ep[0] + ep[1] * t + ep[2] * t * t + ep[3] * t * t * t;
                    xpe[3] = ep[1] / 36525;
                }
                /* descending node */
                      xnd[0] = SE.swe_degnorm(xna[0] + 180);
                xnd[3] = xna[3];
                /* angular distance of perihelion from node */
                parg = xpe[0] = SE.swe_degnorm(xpe[0] - xna[0]);
                pargx = xpe[3] = SE.swe_degnorm(xpe[0] + xpe[3] - xna[3]);
                /* transform from orbital plane to mean ecliptic of date */
                SE.swe_cotrans(xpe, xpe, -incl);
                /* xpe+3 is aux. position, not speed!!! */
                SE.swe_cotrans(xpe + 3, xpe + 3, -incl - vincl);
                /* add node again */
                xpe[0] = SE.swe_degnorm(xpe[0] + xna[0]);
                /* xpe+3 is aux. position, not speed!!! */
                xpe[3] = SE.swe_degnorm(xpe[3] + xna[0] + xna[3]);
                /* speed */
                xpe[3] = SE.swe_degnorm(xpe[3] - xpe[0]);
                /* heliocentric distance of perihelion and aphelion */
                xpe[2] = sema * (1 - ecce);
                xpe[5] = (sema + vsema) * (1 - ecce - vecce) - xpe[2];
                /* aphelion */
                xap[0] = SE.swe_degnorm(xpe[0] + 180);
                xap[1] = -xpe[1];
                xap[3] = xpe[3];
                xap[4] = -xpe[4];
                if (do_focal_point) {
                    xap[2] = sema * ecce * 2;
                    xap[5] = (sema + vsema) * (ecce + vecce) * 2 - xap[2];
                } else {
                    xap[2] = sema * (1 + ecce);
                    xap[5] = (sema + vsema) * (1 + ecce + vecce) - xap[2];
                }
                /* heliocentric distance of nodes */
                ea = Math.Atan(Math.Tan(-parg * SwissEph.DEGTORAD / 2) * Math.Sqrt((1 - ecce) / (1 + ecce))) * 2;
                eax = Math.Atan(Math.Tan(-pargx * SwissEph.DEGTORAD / 2) * Math.Sqrt((1 - ecce - vecce) / (1 + ecce + vecce))) * 2;
                xna[2] = sema * (Math.Cos(ea) - ecce) / Math.Cos(parg * SwissEph.DEGTORAD);
                xna[5] = (sema + vsema) * (Math.Cos(eax) - ecce - vecce) / Math.Cos(pargx * SwissEph.DEGTORAD);
                xna[5] -= xna[2];
                ea = Math.Atan(Math.Tan((180 - parg) * SwissEph.DEGTORAD / 2) * Math.Sqrt((1 - ecce) / (1 + ecce))) * 2;
                eax = Math.Atan(Math.Tan((180 - pargx) * SwissEph.DEGTORAD / 2) * Math.Sqrt((1 - ecce - vecce) / (1 + ecce + vecce))) * 2;
                xnd[2] = sema * (Math.Cos(ea) - ecce) / Math.Cos((180 - parg) * SwissEph.DEGTORAD);
                xnd[5] = (sema + vsema) * (Math.Cos(eax) - ecce - vecce) / Math.Cos((180 - pargx) * SwissEph.DEGTORAD);
                xnd[5] -= xnd[2];
                /* no light-time correction because speed is extremely small */
                for (i = 0, xp = xx; i < 4; i++, xp += 6) {
                    /* to cartesian coordinates */
                    xp[0] *= SwissEph.DEGTORAD;
                    xp[1] *= SwissEph.DEGTORAD;
                    xp[3] *= SwissEph.DEGTORAD;
                    xp[4] *= SwissEph.DEGTORAD;
                    SE.SwephLib.swi_polcart_sp(xp, xp);
                }
                /***************************************
                 * "true" or osculating nodes and apsides 
                 ***************************************/
            } else {
                /* first, we need a heliocentric distance of the planet */
                if (SE.swe_calc(tjd_et, ipli, iflg0, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
                iflJ2000 = (iflag & SEFLG_EPHMASK) | SwissEph.SEFLG_J2000 | SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_XYZ | SwissEph.SEFLG_TRUEPOS | SwissEph.SEFLG_NONUT | SwissEph.SEFLG_SPEED;
                ellipse_is_bary = false;
                if (ipli != SwissEph.SE_MOON) {
                    if ((method & SwissEph.SE_NODBIT_OSCU_BAR) != 0 && x[2] > 6) {
                        iflJ2000 |= SwissEph.SEFLG_BARYCTR; /* only planets beyond Jupiter */
                        ellipse_is_bary = true;
                    } else {
                        iflJ2000 |= SwissEph.SEFLG_HELCTR;
                    }
                }
                /* we need three positions and three speeds
                 * for three nodes/apsides. from the three node positions,
                 * the speed of the node will be computed. */
                if (ipli == SwissEph.SE_MOON) {
                    dt = Sweph.NODE_CALC_INTV;
                    dzmin = 1e-15;
                    Gmsm = Sweph.GEOGCONST * (1 + 1 / Sweph.EARTH_MOON_MRAT) / Sweph.AUNIT / Sweph.AUNIT / Sweph.AUNIT * 86400.0 * 86400.0;
                } else {
                    if ((ipli >= SwissEph.SE_MERCURY && ipli <= SwissEph.SE_PLUTO) || ipli == SwissEph.SE_EARTH)
                        plm = 1 / plmass[ipl_to_elem[ipl]];
                    else
                        plm = 0;
                    dt = Sweph.NODE_CALC_INTV * 10 * x[2];
                    dzmin = 1e-15 * dt / Sweph.NODE_CALC_INTV;
                    Gmsm = Sweph.HELGRAVCONST * (1 + plm) / Sweph.AUNIT / Sweph.AUNIT / Sweph.AUNIT * 86400.0 * 86400.0;
                }
                if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                    istart = 0;
                    iend = 2;
                } else {
                    istart = iend = 0;
                    dt = 0;
                }
                for (i = istart, t = tjd_et - dt; i <= iend; i++, t += dt) {
                    if (istart == iend)
                        t = tjd_et;
                    if (SE.swe_calc(t, ipli, iflJ2000, xpos[i], ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                    /* the EMB is used instead of the earth */
                    if (ipli == SwissEph.SE_EARTH) {
                        if (SE.swe_calc(t, SwissEph.SE_MOON, iflJ2000 & ~(SwissEph.SEFLG_BARYCTR | SwissEph.SEFLG_HELCTR), xposm, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        for (j = 0; j <= 2; j++)
                            xpos[i][j] += xposm[j] / (Sweph.EARTH_MOON_MRAT + 1.0);
                    }
                    SE.Sweph.swi_plan_for_osc_elem(iflg0, t, xpos[i]);
                }
                for (i = istart; i <= iend; i++) {
                    if (Math.Abs(xpos[i][5]) < dzmin)
                        xpos[i][5] = dzmin;
                    fac = xpos[i][2] / xpos[i][5];
                    sgn = xpos[i][5] / Math.Abs(xpos[i][5]);
                    for (j = 0; j <= 2; j++) {
                        xn[i][j] = (xpos[i][j] - fac * xpos[i][j + 3]) * sgn;
                        xs[i][j] = -xn[i][j];
                    }
                }
                for (i = istart; i <= iend; i++) {
                    /* node */
                    rxy = Math.Sqrt(xn[i][0] * xn[i][0] + xn[i][1] * xn[i][1]);
                    cosnode = xn[i][0] / rxy;
                    sinnode = xn[i][1] / rxy;
                    /* inclination */
                    SE.SwephLib.swi_cross_prod(xpos[i], xpos[i].GetPointer(3), xnorm);
                    rxy = xnorm[0] * xnorm[0] + xnorm[1] * xnorm[1];
                    c2 = (rxy + xnorm[2] * xnorm[2]);
                    rxyz = Math.Sqrt(c2);
                    rxy = Math.Sqrt(rxy);
                    sinincl = rxy / rxyz;
                    cosincl = Math.Sqrt(1 - sinincl * sinincl);
                    if (xnorm[2] < 0) cosincl = -cosincl; /* retrograde asteroid, e.g. 20461 Dioretsa */
                    /* argument of latitude */
                    cosu = xpos[i][0] * cosnode + xpos[i][1] * sinnode;
                    sinu = xpos[i][2] / sinincl;
                    uu = Math.Atan2(sinu, cosu);
                    /* semi-axis */
                    rxyz = Math.Sqrt(SE.Sweph.square_sum(xpos[i]));
                    v2 = SE.Sweph.square_sum((xpos[i].GetPointer(3)));
                    sema = 1 / (2 / rxyz - v2 / Gmsm);
                    /* eccentricity */
                    pp = c2 / Gmsm;
                    ecce = Math.Sqrt(1 - pp / sema);
                    /* eccentric anomaly */
                    cosE = 1 / ecce * (1 - rxyz / sema);
                    sinE = 1 / ecce / Math.Sqrt(sema * Gmsm) * SE.Sweph.dot_prod(xpos[i], (xpos[i].GetPointer(3)));
                    /* true anomaly */
                    ny = 2 * Math.Atan(Math.Sqrt((1 + ecce) / (1 - ecce)) * sinE / (1 + cosE));
                    /* distance of perihelion from ascending node */
                    xq[i][0] = SE.SwephLib.swi_mod2PI(uu - ny);
                    xq[i][1] = 0;			/* latitude */
                    xq[i][2] = sema * (1 - ecce);	/* distance of perihelion */
                    /* transformation to ecliptic coordinates */
                    SE.SwephLib.swi_polcart(xq[i], xq[i]);
                    SE.SwephLib.swi_coortrf2(xq[i], xq[i], -sinincl, cosincl);
                    SE.SwephLib.swi_cartpol(xq[i], xq[i]);
                    /* adding node, we get perihelion in ecl. coord. */
                    xq[i][0] += Math.Atan2(sinnode, cosnode);
                    xa[i][0] = SE.SwephLib.swi_mod2PI(xq[i][0] + Math.PI);
                    xa[i][1] = -xq[i][1];
                    if (do_focal_point) {
                        xa[i][2] = sema * ecce * 2;	/* distance of aphelion */
                    } else {
                        xa[i][2] = sema * (1 + ecce);	/* distance of aphelion */
                    }
                    SE.SwephLib.swi_polcart(xq[i], xq[i]);
                    SE.SwephLib.swi_polcart(xa[i], xa[i]);
                    /* new distance of node from orbital ellipse:
                     * true anomaly of node: */
                    ny = SE.SwephLib.swi_mod2PI(ny - uu);
                    ny2 = SE.SwephLib.swi_mod2PI(ny + Math.PI);
                    /* eccentric anomaly */
                    cosE = Math.Cos(2 * Math.Atan(Math.Tan(ny / 2) / Math.Sqrt((1 + ecce) / (1 - ecce))));
                    cosE2 = Math.Cos(2 * Math.Atan(Math.Tan(ny2 / 2) / Math.Sqrt((1 + ecce) / (1 - ecce))));
                    /* new distance */
                    rn = sema * (1 - ecce * cosE);
                    rn2 = sema * (1 - ecce * cosE2);
                    /* old node distance */
                    ro = Math.Sqrt(SE.Sweph.square_sum(xn[i]));
                    ro2 = Math.Sqrt(SE.Sweph.square_sum(xs[i]));
                    /* correct length of position vector */
                    for (j = 0; j <= 2; j++) {
                        xn[i][j] *= rn / ro;
                        xs[i][j] *= rn2 / ro2;
                    }
                }
                for (i = 0; i <= 2; i++) {
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                        xpe[i] = xq[1][i];
                        xpe[i + 3] = (xq[2][i] - xq[0][i]) / dt / 2;
                        xap[i] = xa[1][i];
                        xap[i + 3] = (xa[2][i] - xa[0][i]) / dt / 2;
                        xna[i] = xn[1][i];
                        xna[i + 3] = (xn[2][i] - xn[0][i]) / dt / 2;
                        xnd[i] = xs[1][i];
                        xnd[i + 3] = (xs[2][i] - xs[0][i]) / dt / 2;
                    } else {
                        xpe[i] = xq[0][i];
                        xpe[i + 3] = 0;
                        xap[i] = xa[0][i];
                        xap[i + 3] = 0;
                        xna[i] = xn[0][i];
                        xna[i + 3] = 0;
                        xnd[i] = xs[0][i];
                        xnd[i + 3] = 0;
                    }
                }
                is_true_nodaps = true;
            }
            /* to set the variables required in the save area,
             * i.e. ecliptic, nutation, barycentric sun, earth
             * we compute the planet */
            if (ipli == SwissEph.SE_MOON && (iflag & (SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR)) != 0) {
                SE.Sweph.swi_force_app_pos_etc();
                if (SE.swe_calc(tjd_et, SwissEph.SE_SUN, iflg0, x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            } else {
                if (SE.swe_calc(tjd_et, ipli, iflg0 | (iflag & SwissEph.SEFLG_TOPOCTR), x, ref serr) == SwissEph.ERR)
                    return SwissEph.ERR;
            }
            /***********************
             * position of observer
             ***********************/
            if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                /* geocentric position of observer */
                if (SE.Sweph.swi_get_observer(tjd_et, iflag, false, xobs, ref serr) != SwissEph.OK)
                    return SwissEph.ERR;
                /*for (i = 0; i <= 5; i++)
                  xobs[i] = swed.topd.xobs[i];*/
            } else {
                for (i = 0; i <= 5; i++)
                    xobs[i] = 0;
            }
            if ((iflag & (SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR)) != Sweph.B1950) {
                if ((iflag & SwissEph.SEFLG_HELCTR) != 0 && 0 == (iflag & SwissEph.SEFLG_MOSEPH))
                    for (i = 0; i <= 5; i++)
                        xobs[i] = xsun[i];
            } else if (ipl == SwissEph.SE_SUN && 0 == (iflag & SwissEph.SEFLG_MOSEPH)) {
                for (i = 0; i <= 5; i++)
                    xobs[i] = xsun[i];
            } else {
                /* barycentric position of observer */
                for (i = 0; i <= 5; i++)
                    xobs[i] += xear[i];
            }
            /* ecliptic obliqity */
            if ((iflag & SwissEph.SEFLG_J2000) != 0)
                oe = SE.Sweph.swed.oec2000;
            else
                oe = SE.Sweph.swed.oec;
            /*************************************************
             * conversions shared by mean and osculating points
             *************************************************/
            for (ij = 0, xp = xx; ij < 4; ij++, xp += 6) {
                /* no nodes for earth */
                if (ipli == SwissEph.SE_EARTH && ij <= 1) {
                    for (i = 0; i <= 5; i++)
                        xp[i] = 0;
                    continue;
                }
                /*********************
                 * to equator 
                 *********************/
                if (is_true_nodaps && 0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(xp, xp, -SE.Sweph.swed.nut.snut, SE.Sweph.swed.nut.cnut);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.SwephLib.swi_coortrf2(xp + 3, xp + 3, -SE.Sweph.swed.nut.snut, SE.Sweph.swed.nut.cnut);
                }
                SE.SwephLib.swi_coortrf2(xp, xp, -oe.seps, oe.ceps);
                SE.SwephLib.swi_coortrf2(xp + 3, xp + 3, -oe.seps, oe.ceps);
                if (is_true_nodaps) {
                    /****************************
                     * to mean ecliptic of date 
                     ****************************/
                    if (0 == (iflag & SwissEph.SEFLG_NONUT))
                        SE.Sweph.swi_nutate(xp, iflag, true);
                }
                /*********************
                 * to J2000 
                 *********************/
                SE.SwephLib.swi_precess(xp, tjd_et, iflag, Sweph.J_TO_J2000);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.Sweph.swi_precess_speed(xp, tjd_et, iflag, Sweph.J_TO_J2000);
                /*********************
                 * to barycenter 
                 *********************/
                if (ipli == SwissEph.SE_MOON) {
                    for (i = 0; i <= 5; i++)
                        xp[i] += xear[i];
                } else {
                    if (0 == (iflag & SwissEph.SEFLG_MOSEPH) && !ellipse_is_bary)
                        for (j = 0; j <= 5; j++)
                            xp[j] += xsun[j];
                }
                /*********************
                 * to correct center 
                 *********************/
                for (j = 0; j <= 5; j++)
                    xp[j] -= xobs[j];
                /* geocentric perigee/apogee of sun */
                if (ipl == SwissEph.SE_SUN && 0 == (iflag & (SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR)))
                    for (j = 0; j <= 5; j++)
                        xp[j] = -xp[j];
                /*********************
                 * light deflection 
                 *********************/
                dt = Math.Sqrt(SE.Sweph.square_sum(xp)) * Sweph.AUNIT / Sweph.CLIGHT / 86400.0;
                if (do_defl)
                    SE.Sweph.swi_deflect_light(xp, dt, iflag);
                /*********************
                 * aberration 
                 *********************/
                if (do_aberr) {
                    SE.Sweph.swi_aberr_light(xp, xobs, iflag);
                    /* 
                     * Apparent speed is also influenced by
                     * the difference of speed of the earth between t and t-dt. 
                     * Neglecting this would result in an error of several 0.1"
                     */
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0) {
                        /* get barycentric sun and earth for t-dt into save area */
                        if (SE.swe_calc(tjd_et - dt, ipli, iflg0 | (iflag & SwissEph.SEFLG_TOPOCTR), x2, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        if ((iflag & SwissEph.SEFLG_TOPOCTR) != 0) {
                            /* geocentric position of observer */
                            /* if (swi_get_observer(tjd_et - dt, iflag, FALSE, xobs, serr) != OK)
                              return ERR;*/
                            for (i = 0; i <= 5; i++)
                                xobs2[i] = SE.Sweph.swed.topd.xobs[i];
                        } else {
                            for (i = 0; i <= 5; i++)
                                xobs2[i] = 0;
                        }
                        if ((iflag & (SwissEph.SEFLG_HELCTR | SwissEph.SEFLG_BARYCTR)) != 0) {
                            if ((iflag & SwissEph.SEFLG_HELCTR) != 0 && 0 == (iflag & SwissEph.SEFLG_MOSEPH))
                                for (i = 0; i <= 5; i++)
                                    xobs2[i] = xsun[i];
                        } else if (ipl == SwissEph.SE_SUN && 0 == (iflag & SwissEph.SEFLG_MOSEPH)) {
                            for (i = 0; i <= 5; i++)
                                xobs2[i] = xsun[i];
                        } else {
                            /* barycentric position of observer */
                            for (i = 0; i <= 5; i++)
                                xobs2[i] += xear[i];
                        }
                        for (i = 3; i <= 5; i++)
                            xp[i] += xobs[i] - xobs2[i];
                        /* The above call of swe_calc() has destroyed the
                         * parts of the save area 
                         * (i.e. bary sun, earth nutation matrix!). 
                         * to restore it:
                         */
                        if (SE.swe_calc(tjd_et, SwissEph.SE_SUN, iflg0 | (iflag & SwissEph.SEFLG_TOPOCTR), x2, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                    }
                }
                /*********************
                 * precession
                 *********************/
                /* save J2000 coordinates; required for sidereal positions */
                for (j = 0; j <= 5; j++)
                    x2000[j] = xp[j];
                if (0 == (iflag & SwissEph.SEFLG_J2000)) {
                    SE.SwephLib.swi_precess(xp, tjd_et, iflag, Sweph.J2000_TO_J);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.Sweph.swi_precess_speed(xp, tjd_et, iflag, Sweph.J2000_TO_J);
                }
                /*********************
                 * nutation           
                 *********************/
                if (0 == (iflag & SwissEph.SEFLG_NONUT))
                    SE.Sweph.swi_nutate(xp, iflag, false);
                /* now we have equatorial cartesian coordinates; keep them */
                for (j = 0; j <= 5; j++)
                    pldat.xreturn[18 + j] = xp[j];
                /************************************************
                 * transformation to ecliptic.                  *
                 * with sidereal calc. this will be overwritten *
                 * afterwards.                                  *
                 ************************************************/
                SE.SwephLib.swi_coortrf2(xp, xp, oe.seps, oe.ceps);
                if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                    SE.SwephLib.swi_coortrf2(xp + 3, xp + 3, oe.seps, oe.ceps);
                if (0 == (iflag & SwissEph.SEFLG_NONUT)) {
                    SE.SwephLib.swi_coortrf2(xp, xp, SE.Sweph.swed.nut.snut, SE.Sweph.swed.nut.cnut);
                    if ((iflag & SwissEph.SEFLG_SPEED) != 0)
                        SE.SwephLib.swi_coortrf2(xp + 3, xp + 3, SE.Sweph.swed.nut.snut, SE.Sweph.swed.nut.cnut);
                }
                /* now we have ecliptic cartesian coordinates */
                for (j = 0; j <= 5; j++)
                    pldat.xreturn[6 + j] = xp[j];
                /************************************
                 * sidereal positions               *
                 ************************************/
                if ((iflag & SwissEph.SEFLG_SIDEREAL) != 0) {
                    /* project onto ecliptic t0 */
                    if ((SE.Sweph.swed.sidd.sid_mode & SwissEph.SE_SIDBIT_ECL_T0) != 0) {
                        if (SE.Sweph.swi_trop_ra2sid_lon(x2000, pldat.xreturn.GetPointer(6), pldat.xreturn.GetPointer(18), iflag) != SwissEph.OK)
                            return SwissEph.ERR;
                        /* project onto solar system equator */
                    } else if ((SE.Sweph.swed.sidd.sid_mode & SwissEph.SE_SIDBIT_SSY_PLANE) != 0) {
                        if (SE.Sweph.swi_trop_ra2sid_lon_sosy(x2000, pldat.xreturn.GetPointer(6), iflag) != SwissEph.OK)
                            return SwissEph.ERR;
                    } else {
                        /* traditional algorithm */
                        if (SE.swe_get_ayanamsa_ex(tjd_et, iflag, out daya, ref serr) == SwissEph.ERR)
                            return SwissEph.ERR;
                        pldat.xreturn[0] -= daya * SwissEph.DEGTORAD;
                        SE.SwephLib.swi_polcart_sp(pldat.xreturn, pldat.xreturn.GetPointer(6));
                    }
                }
                if ((iflag & SwissEph.SEFLG_XYZ) != 0 && (iflag & SwissEph.SEFLG_EQUATORIAL) != 0) {
                    for (j = 0; j <= 5; j++)
                        xp[j] = pldat.xreturn[18 + j];
                    continue;
                }
                if ((iflag & SwissEph.SEFLG_XYZ) != 0) {
                    for (j = 0; j <= 5; j++)
                        xp[j] = pldat.xreturn[6 + j];
                    continue;
                }
                /************************************************
                 * transformation to polar coordinates          *
                 ************************************************/
                SE.SwephLib.swi_cartpol_sp(pldat.xreturn.GetPointer(18), pldat.xreturn.GetPointer(12));
                SE.SwephLib.swi_cartpol_sp(pldat.xreturn.GetPointer(6), pldat.xreturn);
                /********************** 
                 * radians to degrees *
                 **********************/
                if ((iflag & SwissEph.SEFLG_RADIANS) == 0)
                {
                    for (j = 0; j < 2; j++)
                    {
                        pldat.xreturn[j] *= SwissEph.RADTODEG;		/* ecliptic */
                        pldat.xreturn[j + 3] *= SwissEph.RADTODEG;
                        pldat.xreturn[j + 12] *= SwissEph.RADTODEG;	/* equator */
                        pldat.xreturn[j + 15] *= SwissEph.RADTODEG;
                    }
                }
                if ((iflag & SwissEph.SEFLG_EQUATORIAL) != 0) {
                    for (j = 0; j <= 5; j++)
                        xp[j] = pldat.xreturn[12 + j];
                    continue;
                } else {
                    for (j = 0; j <= 5; j++)
                        xp[j] = pldat.xreturn[j];
                    continue;
                }
            }
            for (i = 0; i <= 5; i++) {
                if (i > 2 && 0 == (iflag & SwissEph.SEFLG_SPEED))
                    xna[i] = xnd[i] = xpe[i] = xap[i] = 0;
                if (xnasc != null)
                    xnasc[i] = xna[i];
                if (xndsc != null)
                    xndsc[i] = xnd[i];
                if (xperi != null)
                    xperi[i] = xpe[i];
                if (xaphe != null)
                    xaphe[i] = xap[i];
            }
            return SwissEph.OK;
        }

        public Int32 swe_nod_aps_ut(double tjd_ut, Int32 ipl, Int32 iflag,
                              Int32 method,
                              double[] xnasc, double[] xndsc,
                              double[] xperi, double[] xaphe,
                              ref string serr) {
            /*swi_set_tid_acc(tjd_ut, iflag, 0, serr);*/
            return swe_nod_aps(tjd_ut + SE.swe_deltat_ex(tjd_ut, iflag, ref serr),
                                          ipl, iflag, method, xnasc, xndsc, xperi, xaphe,
          ref serr);
        }

        /* function finds the gauquelin sector position of a planet or fixed star
         * 
         * if starname != NULL then a star is computed.
         * iflag: use the flags SE_SWIEPH, SE_JPLEPH, SE_MOSEPH, SEFLG_TOPOCTR.
         *
         * imeth defines method:
         *    imeth = 0    use Placidus house position
         *    imeth = 1    use Placidus house posiition (with planetary lat = 0)
         *    imeth = 2    use rise and set of body's disc center
         *    imeth = 3    use rise and set of body's disc center with refraction
         * rise and set are defined as appearance and disappearance of disc center
         *
         * geopos is an array of 3 doubles for geo. longitude, geo. latitude, elevation.
         * atpress and attemp are only needed for imeth = 3. If imeth = 3,
         * If imeth=3 and atpress not given (= 0), the programm assumes 1013.25 mbar;
         * if a non-zero height above sea is given in geopos, atpress is estimated.
         * dgsect is return area (pointer to a double)
         * serr is pointer to error string, may be NULL
         */
        public Int32 swe_gauquelin_sector(double t_ut, Int32 ipl, string starname, Int32 iflag, Int32 imeth, double[] geopos, double atpress, double attemp, ref double dgsect, ref string serr) {
            bool rise_found = true;
            bool set_found = true;
            Int32 retval;
            double[] tret = new double[3];
            double t_et, t;
            double[] x0 = new double[6], nutlo = new double[2];
            double eps, armc;
            Int32 epheflag = iflag & SEFLG_EPHMASK;
            bool do_fixstar = !String.IsNullOrEmpty(starname);
            Int32 risemeth = 0; string sdummy = null;
            bool above_horizon = false;
            if (imeth < 0 || imeth > 5) {
                serr = C.sprintf("invalid method: %d", imeth);
                return SwissEph.ERR;
            }
            /* function calls for Pluto with asteroid number 134340
             * are treated as calls for Pluto as main body SE_PLUTO */
            if (ipl == SwissEph.SE_AST_OFFSET + 134340)
                ipl = SwissEph.SE_PLUTO;
            /* 
             * geometrically from ecl. longitude and latitude 
             */
            if (imeth == 0 || imeth == 1) {
                t_et = t_ut + SE.swe_deltat_ex(t_ut, iflag, ref serr);
                eps = SE.SwephLib.swi_epsiln(t_et, iflag) * SwissEph.RADTODEG;
                SE.SwephLib.swi_nutation(t_et, iflag, nutlo);
                nutlo[0] *= SwissEph.RADTODEG;
                nutlo[1] *= SwissEph.RADTODEG;
                armc = SE.swe_degnorm(SE.swe_sidtime0(t_ut, eps + nutlo[1], nutlo[0]) * 15 + geopos[0]);
                if (do_fixstar) {
                    if (SE.swe_fixstar(starname, t_et, iflag, x0, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                } else {
                    if (SE.swe_calc(t_et, ipl, iflag, x0, ref serr) == SwissEph.ERR)
                        return SwissEph.ERR;
                }
                if (imeth == 1)
                    x0[1] = 0;
                dgsect = SE.swe_house_pos(armc, geopos[1], eps + nutlo[1], 'G', x0, ref sdummy);
                return SwissEph.OK;
            }
            /* 
             * from rise and set times
             */
            if (imeth == 2 || imeth == 4)
                risemeth |= SwissEph.SE_BIT_NO_REFRACTION;
            if (imeth == 2 || imeth == 3)
                risemeth |= SwissEph.SE_BIT_DISC_CENTER;
            /* find the next rising time of the planet or star */
            retval = swe_rise_trans(t_ut, ipl, starname, epheflag, SwissEph.SE_CALC_RISE | risemeth, geopos, atpress, attemp, ref(tret[0]), ref serr);
            if (retval == SwissEph.ERR) {
                return SwissEph.ERR;
            } else if (retval == -2) {
                /* actually, we could return ERR here. However, we
                 * keep this variable, in case we implement an algorithm
                 * for Gauquelin sector positions of circumpolar bodies.
                 * As with the Ludwig Otto procedure with Placidus, one 
                 * could replace missing rises or sets by meridian transits,
                 * although there are cases where even this is not possible.
                 * Sometimes a body both appears and disappears on the western 
                 * part of the horizon. Using true culminations rather than meridan
                 * transits would not help in any case either, because there are
                 * cases where a body does not have a culmination within days,
                 * e.g. the sun near the poles.
                 */
                rise_found = false;
            }
            /* find the next setting time of the planet or star */
            retval = swe_rise_trans(t_ut, ipl, starname, epheflag, SwissEph.SE_CALC_SET | risemeth, geopos, atpress, attemp, ref (tret[1]), ref serr);
            if (retval == SwissEph.ERR) {
                return SwissEph.ERR;
            } else if (retval == -2) {
                set_found = false;
            }
            if (tret[0] < tret[1] && rise_found == true) {
                above_horizon = false;
                /* find last set */
                t = t_ut - 1.2;
                if (set_found) t = tret[1] - 1.2;
                set_found = true;
                retval = swe_rise_trans(t, ipl, starname, epheflag, SwissEph.SE_CALC_SET | risemeth, geopos, atpress, attemp, ref (tret[1]), ref serr);
                if (retval == SwissEph.ERR) {
                    return SwissEph.ERR;
                } else if (retval == -2) {
                    set_found = false;
                }
            } else if (tret[0] >= tret[1] && set_found == true) {
                above_horizon = true;
                /* find last rise */
                t = t_ut - 1.2;
                if (rise_found) t = tret[0] - 1.2;
                rise_found = true;
                retval = swe_rise_trans(t, ipl, starname, epheflag, SwissEph.SE_CALC_RISE | risemeth, geopos, atpress, attemp, ref (tret[0]), ref serr);
                if (retval == SwissEph.ERR) {
                    return SwissEph.ERR;
                } else if (retval == -2) {
                    rise_found = false;
                }
            }
            if (rise_found && set_found) {
                if (above_horizon) {
                    dgsect = (t_ut - tret[0]) / (tret[1] - tret[0]) * 18 + 1;
                } else {
                    dgsect = (t_ut - tret[1]) / (tret[0] - tret[1]) * 18 + 19;
                }
                return SwissEph.OK;
            } else {
                dgsect = 0;
                serr = C.sprintf("rise or set not found for planet %d", ipl);
                return SwissEph.ERR;
            }
        }

    }
}
