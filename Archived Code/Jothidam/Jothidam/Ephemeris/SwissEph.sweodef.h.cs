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
   $Header: /home/dieter/sweph/RCS/sweodef.h,v 1.74 2008/06/16 10:07:20 dieter Exp $
   definitions and constants for all Swiss Ephemeris source files,
   only required for compiling the libraries, not for the external
   interface of the libraries.

   The definitions are a subset of Astrodienst's ourdef.h content
   and must be kept compatible. Everything not used in SwissEph
   has been deleted.

   Does auto-detection of MSDOS (TURBO_C or MS_C),  HPUNIX, Linux.
   Must be extended for more portability; there should be a way
   to detect byte order and file system type.
   
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
namespace SwissEphNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    partial class SwissEph
    {
        /// <summary>
        /// No error value
        /// </summary>
        public const int OK = (0);

        /// <summary>
        /// Error value
        /// </summary>
        public const int ERR = (-1);

        /// <summary>
        /// degree as string, utf8 encoding
        /// </summary>
        public const String ODEGREE_STRING = "°";

        public const double DEGTORAD = 0.0174532925199433;
        public const double RADTODEG = 57.2957795130823;

        public const int DEG = 360000;  /* degree expressed in centiseconds */
        public const int DEG7_30 = (2700000);	/* 7.5 degrees */
        public const int DEG15 = (15 * DEG);
        public const int DEG24 = (24 * DEG);
        public const int DEG30 = (30 * DEG);
        public const int DEG60 = (60 * DEG);
        public const int DEG90 = (90 * DEG);
        public const int DEG120 = (120 * DEG);
        public const int DEG150 = (150 * DEG);
        public const int DEG180 = (180 * DEG);
        public const int DEG270 = (270 * DEG);
        public const int DEG360 = (360 * DEG);

        public const double CSTORAD = 4.84813681109536E-08; /* centisec to rad: pi / 180 /3600/100 */
        public const double RADTOCS = 2.06264806247096E+07; /* rad to centisec 180*3600*100/pi */

        public const double CS2DEG = (1.0 / 360000.0);	/* centisec to degree */

        public static char PATH_SEPARATOR = ';';	/* semicolon as PATH separator */
        public static char DIR_GLUE = '\\';		/* glue string for directory/file */
    }

}
