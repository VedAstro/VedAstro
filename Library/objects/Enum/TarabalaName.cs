using System;

namespace Genso.Astrology.Library
{
    [Flags]
    public enum TarabalaName
    {
        /// <summary>
        /// It indicates danger to body
        /// </summary>
        Janma = 1,
        /// <summary>
        /// Wealth and prosperity
        /// </summary>
        Sampat = 2,
        /// <summary>
        /// Dangers, losses and accidents
        /// </summary>
        Vipat = 3,
        /// <summary>
        /// Prosperity
        /// </summary>
        Kshema = 4,
        /// <summary>
        /// Obstacles
        /// </summary>
        Pratyak = 5,
        /// <summary>
        /// Realisation of ambitions
        /// </summary>
        Sadhana = 6,
        /// <summary>
        /// Dangers
        /// </summary>
        Naidhana = 7,
        /// <summary>
        /// Good
        /// </summary>
        Mitra = 8,
        /// <summary>
        /// Very favourable
        /// </summary>
        ParamaMitra = 9

    }
}