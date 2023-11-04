using System;

namespace VedAstro.Library
{

    //█░░ ▄▀█ █▄█ █▀█ █░█ ▀█▀   █▀ █ ▀█ █▀▀
    //█▄▄ █▀█ ░█░ █▄█ █▄█ ░█░   ▄█ █ █▄ ██▄

    /// <summary>
    /// Special class to give Golden Ration nicely
    /// Theory: human symmetrical distance, where each distance is reletive to the one before
    /// </summary>
    public static class GR
    {
        /// <summary>
        /// used for dynamic design layout
        /// </summary>
        public const double GoldenRatio = 1.61803;
        public const double ContentWidth = 1080; //page content 

        public static double W1080 => 1080;
        public static string W1080px => $"{W1080}px";

        public static double W824 => GR.W667 + GR.W157;
        public static string W824px => $"{W824}px";

        public static double W764 => GR.W667 + GR.W97;
        public static string W764px => $"{W764}px";

        public static double W667 => Math.Round(W1080 / GoldenRatio, 1);
        public static string W667px => $"{W667}px";

        public static double W546 => W509 + W37;
        public static string W546px => $"{W546}px";

        public static double W509 => W412 + W97;
        public static string W509px => $"{W509}px";

        public static double W412 => Math.Round(W667 / GoldenRatio, 1);
        public static string W412px => $"{W412}px";

        public static double W352 => W255 + W97;
        public static string W352px => $"{W352}px";

        public static double W291 => W157 + W134;
        public static string W291px => $"{W291}px";

        public static double W255 => Math.Round(W412 / GoldenRatio, 1);
        public static string W255px => $"{W255}px";

        public static double W231 => GR.W194 + GR.W37;
        public static string W231px => $"{W231}px";

        public static double W194 => GR.W157 + GR.W37;
        public static string W194px => $"{W194}px";

        public static double W157 => Math.Round(W255 / GoldenRatio, 1);
        public static string W157px => $"{W157}px";

        public static double W134 => GR.W97 + GR.W37;
        public static string W134px => $"{W134}px";

        public static double W97 => Math.Round(W157 / GoldenRatio, 1);
        public static string W97px => $"{W97}px";

        public static double W60 => Math.Round(W97 / GoldenRatio, 1);
        public static string W60px => $"{W60}px";

        public static double W37 => Math.Round(W60 / GoldenRatio, 1);
        public static string W37px => $"{W37}px";

        public static double W22 => Math.Round(W37 / GoldenRatio, 1);
        public static string W22px => $"{W22}px";

        public static double W14 => Math.Round(W22 / GoldenRatio, 1);
        public static string W14px => $"{W14}px";

        public static double W8 => Math.Round(W14 / GoldenRatio, 1);
        public static string W8px => $"{W8}px";

        public static double W5 => Math.Round(W8 / GoldenRatio, 1);
        public static string W5px => $"{W5}px";

        public static double W3 => Math.Round(W5 / GoldenRatio, 1);
        public static string W3px => $"{W3}px";

        public static double W2 => Math.Round(W3 / GoldenRatio, 1);
        public static string W2px => $"{W2}px";

        public static double W1 => Math.Round(W2 / GoldenRatio, 1);
        public static string W1px => $"{W1}px";




    }
}
