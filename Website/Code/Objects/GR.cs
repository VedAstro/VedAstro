namespace Website
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

        public static double W667 => Math.Round(W1080 / GoldenRatio, 1);
        public static string W667px => $"{W667}px";

        public static double W412 => Math.Round(W667 / GoldenRatio, 1);
        public static string W412px => $"{W412}px";

        public static double W255 => Math.Round(W412 / GoldenRatio, 1);
        public static string W255px => $"{W255}px";

        public static double W157 => Math.Round(W255 / GoldenRatio, 1);
        public static string W157px => $"{W157}px";

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
