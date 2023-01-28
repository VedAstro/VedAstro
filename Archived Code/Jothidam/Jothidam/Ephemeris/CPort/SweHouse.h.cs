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

/*******************************************************
$Header: /home/dieter/sweph/RCS/swehouse.h,v 1.74 2008/06/16 10:07:20 dieter Exp $
module swehouse.h
house and (simple) aspect calculation 

*******************************************************/

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

    partial class SweHouse
    {
        public class houses
        {
            public houses() {
                cusp = new double[37];
            }
            public double[] cusp;
            public double ac;
            public double mc;
            public double vertex;
            public double equasc;
            public double coasc1;
            public double coasc2;
            public double polasc;
        }

        //#define HOUSES 	struct houses;
        public const double VERY_SMALL = 1E-10;

        public double degtocs(double x) { return (SE.swe_d2l((x) * SwissEph.DEG)); }
        public double cstodeg(double x) { return (double)((x) * SwissEph.CS2DEG); }

        public double sind(double x) { return Math.Sin(x * SwissEph.DEGTORAD); }
        public double cosd(double x) { return Math.Cos(x * SwissEph.DEGTORAD); }
        public double tand(double x) { return Math.Tan(x * SwissEph.DEGTORAD); }
        public double asind(double x) { return (Math.Asin(x) * SwissEph.RADTODEG); }
        public double acosd(double x) { return (Math.Acos(x) * SwissEph.RADTODEG); }
        public double atand(double x) { return (Math.Atan(x) * SwissEph.RADTODEG); }
        public double atan2d(double y, double x) { return (Math.Atan2(y, x) * SwissEph.RADTODEG); }

    }
}
