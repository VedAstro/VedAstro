using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
    /// <summary>
    /// Some value formats
    /// </summary>
    partial class SwissEph
    {

        public const int BIT_ROUND_SEC = 1;
        public const int BIT_ROUND_MIN = 2;
        public const int BIT_ZODIAC = 4;
        public const int BIT_LZEROES = 8;

        static string ZodiacSymbols = "♈♉♊♋♌♍♎♏♐♑♒♓";
        static string[] ZodiacShortNames = new String[]{
            "ar", "ta", "ge", "cn", "le", "vi", 
            "li", "sc", "sa", "cp", "aq", "pi"
        };
        static string[] ZodiacNames = new String[]{
            "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", 
            "Libra", "Scorpio", "Sagittarius", "Capricorn", "Aquarius", "Pisces"
        };

        /// <summary>
        /// Format to Degrees Minutes Seconds like dms() function in swewin.exe and swetest.exe.
        /// </summary>
        public String DMS(double value, int iFlag, bool outputExtraPrecision = false) {
            if (double.IsNaN(value)) return "nan";
            int izod;
            Int32 k, kdeg, kmin, ksec;
            string c = SwissEph.ODEGREE_STRING;
            string s1 = string.Empty;
            string s = string.Empty;
            int sgn;
            if ((iFlag & SwissEph.SEFLG_EQUATORIAL) != 0)
                c = "h";
            if (value < 0) {
                value = -value;
                sgn = -1;
            } else
                sgn = 1;
            if ((iFlag & BIT_ROUND_MIN) != 0)
                value = SwephLib.swe_degnorm(value + 0.5 / 60);
            if ((iFlag & BIT_ROUND_SEC) != 0)
                value = SwephLib.swe_degnorm(value + 0.5 / 3600);
            if ((iFlag & BIT_ZODIAC) != 0) {
                izod = (int)(value / 30);
                value = (value % 30.0);
                kdeg = (Int32)value;
                s = C.sprintf("  %2d %s ", kdeg, ZodiacShortNames[izod]);
            } else {
                kdeg = (Int32)value;
                s = C.sprintf(" %3d%s", kdeg, c);
            }
            value -= kdeg;
            value *= 60;
            kmin = (Int32)value;
            if ((iFlag & BIT_ZODIAC) != 0 && (iFlag & BIT_ROUND_MIN) != 0) {
                s1 = C.sprintf("%2d", kmin);
            } else {
                s1 = C.sprintf("%2d'", kmin);
            }
            s += s1;
            if ((iFlag & BIT_ROUND_MIN) != 0)
                goto return_dms;
            value -= kmin;
            value *= 60;
            ksec = (Int32)value;
            if ((iFlag & BIT_ROUND_SEC) != 0) {
                s1 = C.sprintf("%2d\"", ksec);
            } else {
                s1 = C.sprintf("%2d", ksec);
            }
            s += s1;
            if ((iFlag & BIT_ROUND_SEC) != 0)
                goto return_dms;
            value -= ksec;
            if (outputExtraPrecision) {
                k = (Int32)(value * 100000 + 0.5);
                s1 = C.sprintf(".%05d", k);
            } else {
                k = (Int32)(value * 10000 + 0.5);
                s1 = C.sprintf(".%04d", k);
            }
            s += s1;
        return_dms: 
            int spi;
            if (sgn < 0) {
                spi = s.IndexOfAny("0123456789".ToCharArray());
                s = String.Concat(s.Substring(0, spi - 1), '-', s.Substring(spi));
            }
            if ((iFlag & BIT_LZEROES) != 0) {
                s = s.Substring(0, 2) + s.Substring(2).Replace(' ', '0');
            }
            return (s);
        }

        /// <summary>
        /// Format to Hour Minutes Seconds like hms() function in swewin.exe and swetest.exe.
        /// </summary>
        public String HMS(double value, int iFlag, bool outputExtraPrecision = false) {
            // hms() function is a little buggy so we correct some formats
            var dmsResult = DMS(value, iFlag, outputExtraPrecision);
            int oPos = dmsResult.IndexOf(SwissEph.ODEGREE_STRING);
            if (oPos >= 0) {
                dmsResult = dmsResult
                    .Replace(SwissEph.ODEGREE_STRING, ":")
                    .Replace("'", ":");
                int sPos = dmsResult.IndexOf("\"");
                if (sPos >= 0) {
                    dmsResult = dmsResult.Substring(0, sPos);
                } else {
                    if (dmsResult.Length > oPos + 4)
                        dmsResult = dmsResult.Substring(0, oPos + 8);
                    else
                        dmsResult = dmsResult.Substring(0, oPos + 3);
                }
            }
            return dmsResult;
        }

        /// <summary>
        /// Format value to degrees/minutes/seconds
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>d : Degrees</item>
        /// <item>dd : Degrees leading space</item>
        /// <item>ddd : Degrees leading space</item>
        /// <item>dddd : Degrees leading space</item>
        /// <item>a : Absolute Degrees</item>
        /// <item>aa : Absolute Degrees leading space</item>
        /// <item>aaa : Absolute Degrees leading space</item>
        /// <item>n : Zodiac number</item>
        /// <item>nn : Zodiac number leading space</item>
        /// <item>g : Zodiac degrees (degrees % 30)</item>
        /// <item>gg : Zodiac degrees leading space</item>
        /// <item>m : minutes</item>
        /// <item>mm : minutes leading space</item>
        /// <item>s : seconds</item>
        /// <item>ss : seconds leading space</item>
        /// <item>p : seconds decimal part to 0.0 format</item>
        /// <item>pp : seconds decimal part to 0.00 format</item>
        /// <item>ppp : seconds decimal part to 0.000 format</item>
        /// <item>pppp : seconds decimal part to 0.0000 format</item>
        /// <item>ppppp : seconds decimal part to 0.00000 format</item>
        /// <item>z : Zodiac symbol</item>
        /// <item>zz : Zodiac short name</item>
        /// <item>zzz : Zodiac name</item>
        /// <item>- : Minus sign if value is negative</item>
        /// <item>+ : Minus sign if value is negative or space if positive</item>
        /// </list>
        /// <para>
        /// Standard formats are:
        /// <list type="bullet">
        /// <item>D1 : dddd°mm'ss.pppp</item>
        /// <item>D2 : dddd°mm'ss"</item>
        /// <item>Z1 : gg zz mm'ss.pppp</item>
        /// <item>Z2 : gg zz mm'ss"</item>
        /// </list>
        /// </para>
        /// <para>
        /// For d*, a*, n*, g*, m* and s*, the same uppercase format exists for leading 0 instead of space
        /// </para>
        /// </remarks>
        public static String FormatToDegreeMinuteSecond(double value, String format = null) {
            if (double.IsNaN(value)) return "nan";
            if (String.IsNullOrEmpty(format)) format = "dddd°mm'ss.pppp";
            switch (format) {
                case "D1": format = "dddd°mm'ss.pppp"; break;
                case "D2": format = "dddd°mm'ss\""; break;
                case "Z1": format = "gg zz mm'ss.pppp"; break;
                case "Z2": format = "gg zz mm'ss\""; break;
            }
            // Elements calculation
            var sgn = Math.Sign(value);
            double avalue = Math.Abs(value);
            int deg = (int)value;
            int adeg = (int)avalue;
            int znum = (int)((avalue % 360.0) / 30);
            int zdeg = (int)((avalue % 360.0) % 30.0);
            avalue -= adeg; avalue *= 60.0;
            int min = (int)avalue;
            avalue -= min; 
            double dsec = (avalue * 60.0);
            StringBuilder result = new StringBuilder();
            for (int i = 0, fmtLen = format.Length; i < fmtLen; i++) {
                char c = format[i];
                int l = 1;
                // Search length of segment
                char[] cf = null;
                switch (c) {
                    case 'd':
                    case 'D': cf = new char[] { 'd', 'D' }; break;
                    case 'a':
                    case 'A': cf = new char[] { 'a', 'A' }; break;
                    case 'n':
                    case 'N': cf = new char[] { 'n', 'N' }; break;
                    case 'g':
                    case 'G': cf = new char[] { 'g', 'G' }; break;
                    case 'm':
                    case 'M': cf = new char[] { 'm', 'M' }; break;
                    case 's':
                    case 'S': cf = new char[] { 's', 'S' }; break;
                    case 'p':
                    case 'P': cf = new char[] { 'p', 'P' }; break;
                    case 'z':
                    case 'Z': cf = new char[] { 'z', 'Z' }; break;
                }
                if (cf != null) {
                    while (i + 1 < fmtLen && (format[i + 1] == cf[0] || format[i + 1] == cf[1])) { i++; l++; }
                }
                // Format
                switch (c) {
                    case 'd': result.AppendFormat(String.Format("{{0,{0}}}", l), deg); break;
                    case 'D': result.AppendFormat(String.Format("{{0:D{0}}}", sgn < 0 ? l - 1 : l), deg); break;
                    case 'a': result.AppendFormat(String.Format("{{0,{0}}}", l), adeg); break;
                    case 'A': result.AppendFormat(String.Format("{{0:D{0}}}", l), adeg); break;
                    case 'n': result.AppendFormat(String.Format("{{0,{0}}}", l), znum); break;
                    case 'N': result.AppendFormat(String.Format("{{0:D{0}}}", l), znum); break;
                    case 'g': result.AppendFormat(String.Format("{{0,{0}}}", l), zdeg); break;
                    case 'G': result.AppendFormat(String.Format("{{0:D{0}}}", l), zdeg); break;
                    case 'm': result.AppendFormat(String.Format("{{0,{0}}}", l), min); break;
                    case 'M': result.AppendFormat(String.Format("{{0:D{0}}}", l), min); break;
                    case 's': result.AppendFormat(String.Format("{{0,{0}}}", l), (int)Math.Round(dsec, l)); break;
                    case 'S': result.AppendFormat(String.Format("{{0:D{0}}}", l), (int)Math.Round(dsec, l)); break;
                    case 'p':
                    case 'P':
                        var t = Math.Round(dsec, l);
                        var prec = t - (int)t;
                        prec = Math.Round(prec * Math.Pow(10, l));
                        result.AppendFormat(String.Format("{{0:D{0}}}", l), (int)prec);
                        break;
                    case 'z':
                    case 'Z':
                        switch (l) {
                            case 1:
                                result.Append(ZodiacSymbols[znum % 12]);
                                break;
                            case 2:
                                result.Append(ZodiacShortNames[znum % 12]);
                                break;
                            default:
                                result.Append(ZodiacNames[znum % 12]);
                                break;
                        }
                        break;
                    case '-':
                        if (sgn < 0) result.Append('-');
                        break;
                    case '+':
                        result.Append((sgn < 0) ? '-' : ' ');
                        break;
                    default:
                        result.Append(c);
                        break;
                }
            }
            return result.ToString();
        }

    }
}
