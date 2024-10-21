using System.Linq;
using System;

namespace VedAstro.Library
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayStringAttribute : Attribute
    {
        public string Value { get; }

        public DisplayStringAttribute(string value) { Value = value; }
    }

    /// <summary>
    /// Represents the name of the match prediction method
    /// Only used in specialized match calculators
    /// </summary>
    public enum MatchPredictionName
    {
        Empty = 0,

        [DisplayString("Mahendra")]
        Mahendra,

        [DisplayString("Nadi Kuta")]
        NadiKuta,

        [DisplayString("Guna Kuta")]
        GunaKuta,

        [DisplayString("Varna")]
        Varna,

        [DisplayString("Yoni Kuta")]
        YoniKuta,

        [DisplayString("Vedha")]
        Vedha,

        [DisplayString("Vasya Kuta")]
        VasyaKuta,

        [DisplayString("Graha Maitram")]
        GrahaMaitram,

        [DisplayString("Rasi Kuta")]
        RasiKuta,

        [DisplayString("Stree Deergha")]
        StreeDeergha,

        [DisplayString("Dina Kuta")]
        DinaKuta,

        [DisplayString("Kuja Dosa")]
        KujaDosa,

        [DisplayString("Rajju")]
        Rajju,

        [DisplayString("Lagna And 7th Good")]
        LagnaAnd7thGood,

        [DisplayString("Bad Constellation")]
        BadConstellation,

        [DisplayString("Sex Energy")]
        SexEnergy
    }

    public static class MatchPredictionNameExtensions
    {
        /// <summary>
        /// Human friendly Kuta name for display
        /// Will use original name, if human friendly name not provided
        /// </summary>
        public static string ToDisplayString(this MatchPredictionName value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = (DisplayStringAttribute)fieldInfo.GetCustomAttributes(typeof(DisplayStringAttribute), false).FirstOrDefault();
            return attribute?.Value ?? value.ToString();
        }
    }

}
