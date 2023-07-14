using System.Linq;


    //█▀▀█ █▀▀▄ █░░ █░░█ 　 █▀▀ █▀▀█ █▀▀▄ █▀▀ 　 █░░░█ █░░█ █▀▀ █▀▀▄ 　 █░░█ █▀▀█ █░░█ 　 █▀▀ █▀▀█ █▀▀▄ █▀▀▄ █▀▀█ ▀▀█▀▀ 
    //█░░█ █░░█ █░░ █▄▄█ 　 █░░ █░░█ █░░█ █▀▀ 　 █▄█▄█ █▀▀█ █▀▀ █░░█ 　 █▄▄█ █░░█ █░░█ 　 █░░ █▄▄█ █░░█ █░░█ █░░█ ░░█░░ 
    //▀▀▀▀ ▀░░▀ ▀▀▀ ▄▄▄█ 　 ▀▀▀ ▀▀▀▀ ▀▀▀░ ▀▀▀ 　 ░▀░▀░ ▀░░▀ ▀▀▀ ▀░░▀ 　 ▄▄▄█ ▀▀▀▀ ░▀▀▀ 　 ▀▀▀ ▀░░▀ ▀░░▀ ▀░░▀ ▀▀▀▀ ░░▀░░ 

    //█▀▄▀█ █▀▀ █▀▀▄ ░▀░ ▀▀█▀▀ █▀▀█ ▀▀█▀▀ █▀▀ ░░ 
    //█░▀░█ █▀▀ █░░█ ▀█▀ ░░█░░ █▄▄█ ░░█░░ █▀▀ ▄▄ 
    //▀░░░▀ ▀▀▀ ▀▀▀░ ▀▀▀ ░░▀░░ ▀░░▀ ░░▀░░ ▀▀▀ ░█ 

    //█▀▀█ █▀▀▄ █░░ █░░█ 　 █▀▄▀█ █▀▀ █▀▀▄ ░▀░ ▀▀█▀▀ █▀▀█ ▀▀█▀▀ █▀▀ 　 █░░░█ █░░█ █▀▀ █▀▀▄ 　 █░░█ █▀▀█ █░░█ 
    //█░░█ █░░█ █░░ █▄▄█ 　 █░▀░█ █▀▀ █░░█ ▀█▀ ░░█░░ █▄▄█ ░░█░░ █▀▀ 　 █▄█▄█ █▀▀█ █▀▀ █░░█ 　 █▄▄█ █░░█ █░░█ 
    //▀▀▀▀ ▀░░▀ ▀▀▀ ▄▄▄█ 　 ▀░░░▀ ▀▀▀ ▀▀▀░ ▀▀▀ ░░▀░░ ▀░░▀ ░░▀░░ ▀▀▀ 　 ░▀░▀░ ▀░░▀ ▀▀▀ ▀░░▀ 　 ▄▄▄█ ▀▀▀▀ ░▀▀▀ 

    //█▀▀ █▀▀█ █▀▀▄ █▀▀▄ █▀▀█ ▀▀█▀▀ 　 █▀▀ █▀▀█ █▀▀▄ █▀▀ 
    //█░░ █▄▄█ █░░█ █░░█ █░░█ ░░█░░ 　 █░░ █░░█ █░░█ █▀▀ 
    //▀▀▀ ▀░░▀ ▀░░▀ ▀░░▀ ▀▀▀▀ ░░▀░░ 　 ▀▀▀ ▀▀▀▀ ▀▀▀░ ▀▀▀

namespace VedAstro.Library
{
    /// <summary>
    /// Calculators specific for Horoscope events
    /// Only a person's birth time is needed
    /// </summary>
    public class HoroscopeCalculatorMethods
    {
        public readonly record struct YogaResult(bool IsOccurred, double Strength, string Notes);


        #region YOGA DETECTOR

        [HoroscopeCalculator(HoroscopeName.KemadrumaYoga)]
        public static CalculatorResult KemadrumaYoga(Time time)
        {
            //needed to make code compile in WIP
            return CalculatorResult.NotOccuring();

          
        }

        #endregion

        #region HOROSCOPE

        #region Lord of 1st being Situated in Different Houses

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse1)]
        public static CalculatorResult House1LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House1, time), new[] { HouseName.House1, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse2)]
        public static CalculatorResult House1LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House2, time), new[] { HouseName.House1, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse3)]
        public static CalculatorResult House1LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House3, time), new[] { HouseName.House1, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse4)]
        public static CalculatorResult House1LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House4, time), new[] { HouseName.House1, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse5)]
        public static CalculatorResult House1LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House5, time), new[] { HouseName.House1, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse6)]
        public static CalculatorResult House1LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House6, time), new[] { HouseName.House1, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse7)]
        public static CalculatorResult House1LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House7, time), new[] { HouseName.House1, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse8)]
        public static CalculatorResult House1LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House8, time), new[] { HouseName.House1, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse9)]
        public static CalculatorResult House1LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House9, time), new[] { HouseName.House1, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse10)]
        public static CalculatorResult House1LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House10, time), new[] { HouseName.House1, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse11)]
        public static CalculatorResult House1LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House11, time), new[] { HouseName.House1, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House1LordInHouse12)]
        public static CalculatorResult House1LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House12, time), new[] { HouseName.House1, HouseName.House12 }, time);

        #endregion

        #region Lord of 2nd being Situated in Different Houses

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse1)]
        public static CalculatorResult House2LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House1, time), new[] { HouseName.House2, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse2)]
        public static CalculatorResult House2LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House2, time), new[] { HouseName.House2, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse3)]
        public static CalculatorResult House2LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House3, time), new[] { HouseName.House2, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse4)]
        public static CalculatorResult House2LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House4, time), new[] { HouseName.House2, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse5)]
        public static CalculatorResult House2LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House5, time), new[] { HouseName.House2, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse6)]
        public static CalculatorResult House2LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House6, time), new[] { HouseName.House2, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse7)]
        public static CalculatorResult House2LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House7, time), new[] { HouseName.House2, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse8)]
        public static CalculatorResult House2LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House8, time), new[] { HouseName.House2, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse9)]
        public static CalculatorResult House2LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House9, time), new[] { HouseName.House2, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse10)]
        public static CalculatorResult House2LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House10, time), new[] { HouseName.House2, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse11)]
        public static CalculatorResult House2LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House11, time), new[] { HouseName.House2, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House2LordInHouse12)]
        public static CalculatorResult House2LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House12, time), new[] { HouseName.House2, HouseName.House12 }, time);

        #endregion

        #region Lord of 3rd being Situated in Different Houses

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse1)]
        public static CalculatorResult House3LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House1, time), new[] { HouseName.House3, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse2)]
        public static CalculatorResult House3LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House2, time), new[] { HouseName.House3, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse3)]
        public static CalculatorResult House3LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House3, time), new[] { HouseName.House3, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse4)]
        public static CalculatorResult House3LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House4, time), new[] { HouseName.House3, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse5)]
        public static CalculatorResult House3LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House5, time), new[] { HouseName.House3, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse6)]
        public static CalculatorResult House3LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House6, time), new[] { HouseName.House3, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse7)]
        public static CalculatorResult House3LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House7, time), new[] { HouseName.House3, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse8)]
        public static CalculatorResult House3LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House8, time), new[] { HouseName.House3, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse9)]
        public static CalculatorResult House3LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House9, time), new[] { HouseName.House3, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse10)]
        public static CalculatorResult House3LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House10, time), new[] { HouseName.House3, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse11)]
        public static CalculatorResult House3LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House11, time), new[] { HouseName.House3, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House3LordInHouse12)]
        public static CalculatorResult House3LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House12, time), new[] { HouseName.House3, HouseName.House12 }, time);

        #endregion

        #region Lord of the 4th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse1)]
        public static CalculatorResult House4LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House1, time), new[] { HouseName.House4, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse2)]
        public static CalculatorResult House4LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House2, time), new[] { HouseName.House4, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse3)]
        public static CalculatorResult House4LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House3, time), new[] { HouseName.House4, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse4)]
        public static CalculatorResult House4LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House4, time), new[] { HouseName.House4, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse5)]
        public static CalculatorResult House4LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House5, time), new[] { HouseName.House4, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse6)]
        public static CalculatorResult House4LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House6, time), new[] { HouseName.House4, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse7)]
        public static CalculatorResult House4LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House7, time), new[] { HouseName.House4, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse8)]
        public static CalculatorResult House4LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House8, time), new[] { HouseName.House4, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse9)]
        public static CalculatorResult House4LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House9, time), new[] { HouseName.House4, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse10)]
        public static CalculatorResult House4LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House10, time), new[] { HouseName.House4, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse11)]
        public static CalculatorResult House4LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House11, time), new[] { HouseName.House4, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House4LordInHouse12)]
        public static CalculatorResult House4LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House12, time), new[] { HouseName.House4, HouseName.House12 }, time);

        #endregion

        #region Lord of the 5th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse1)]
        public static CalculatorResult House5LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House1, time), new[] { HouseName.House5, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse2)]
        public static CalculatorResult House5LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House2, time), new[] { HouseName.House5, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse3)]
        public static CalculatorResult House5LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House3, time), new[] { HouseName.House5, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse4)]
        public static CalculatorResult House5LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House4, time), new[] { HouseName.House5, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse5)]
        public static CalculatorResult House5LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House5, time), new[] { HouseName.House5, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse6)]
        public static CalculatorResult House5LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House6, time), new[] { HouseName.House5, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse7)]
        public static CalculatorResult House5LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House7, time), new[] { HouseName.House5, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse8)]
        public static CalculatorResult House5LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House8, time), new[] { HouseName.House5, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse9)]
        public static CalculatorResult House5LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House9, time), new[] { HouseName.House5, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse10)]
        public static CalculatorResult House5LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House10, time), new[] { HouseName.House5, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse11)]
        public static CalculatorResult House5LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House11, time), new[] { HouseName.House5, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House5LordInHouse12)]
        public static CalculatorResult House5LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House12, time), new[] { HouseName.House5, HouseName.House12 }, time);

        #endregion

        #region Lord of the 6th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse1)]
        public static CalculatorResult House6LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House1, time), new[] { HouseName.House6, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse2)]
        public static CalculatorResult House6LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House2, time), new[] { HouseName.House6, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse3)]
        public static CalculatorResult House6LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House3, time), new[] { HouseName.House6, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse4)]
        public static CalculatorResult House6LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House4, time), new[] { HouseName.House6, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse5)]
        public static CalculatorResult House6LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House5, time), new[] { HouseName.House6, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse6)]
        public static CalculatorResult House6LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House6, time), new[] { HouseName.House6, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse7)]
        public static CalculatorResult House6LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House7, time), new[] { HouseName.House6, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse8)]
        public static CalculatorResult House6LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House8, time), new[] { HouseName.House6, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse9)]
        public static CalculatorResult House6LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House9, time), new[] { HouseName.House6, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse10)]
        public static CalculatorResult House6LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House10, time), new[] { HouseName.House6, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse11)]
        public static CalculatorResult House6LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House11, time), new[] { HouseName.House6, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House6LordInHouse12)]
        public static CalculatorResult House6LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House12, time), new[] { HouseName.House6, HouseName.House12 }, time);

        #endregion

        #region Lord of the 7th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House7LordInHouse1)]
        public static CalculatorResult House7LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House1, time), new[] { HouseName.House7, HouseName.House1 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse2)]
        public static CalculatorResult House7LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House2, time), new[] { HouseName.House7, HouseName.House2 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse3)]
        public static CalculatorResult House7LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House3, time), new[] { HouseName.House7, HouseName.House3 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse4)]
        public static CalculatorResult House7LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House4, time), new[] { HouseName.House7, HouseName.House4 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse5)]
        public static CalculatorResult House7LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House5, time), new[] { HouseName.House7, HouseName.House5 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse6)]
        public static CalculatorResult House7LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House6, time), new[] { HouseName.House7, HouseName.House6 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse7)]
        public static CalculatorResult House7LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House7, time), new[] { HouseName.House7, HouseName.House7 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse8)]
        public static CalculatorResult House7LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House8, time), new[] { HouseName.House7, HouseName.House8 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse9)]
        public static CalculatorResult House7LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House9, time), new[] { HouseName.House7, HouseName.House9 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse10)]
        public static CalculatorResult House7LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House10, time), new[] { HouseName.House7, HouseName.House10 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse11)]
        public static CalculatorResult House7LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House11, time), new[] { HouseName.House7, HouseName.House11 }, time);
        [HoroscopeCalculator(HoroscopeName.House7LordInHouse12)]
        public static CalculatorResult House7LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House12, time), new[] { HouseName.House7, HouseName.House12 }, time);

        #endregion

        #region Lord of the 8th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse1)]
        public static CalculatorResult House8LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House1, time), new[] { HouseName.House8, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse2)]
        public static CalculatorResult House8LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House2, time), new[] { HouseName.House8, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse3)]
        public static CalculatorResult House8LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House3, time), new[] { HouseName.House8, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse4)]
        public static CalculatorResult House8LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House4, time), new[] { HouseName.House8, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse5)]
        public static CalculatorResult House8LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House5, time), new[] { HouseName.House8, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse6)]
        public static CalculatorResult House8LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House6, time), new[] { HouseName.House8, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse7)]
        public static CalculatorResult House8LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House7, time), new[] { HouseName.House8, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse8)]
        public static CalculatorResult House8LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House8, time), new[] { HouseName.House8, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse9)]
        public static CalculatorResult House8LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House9, time), new[] { HouseName.House8, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse10)]
        public static CalculatorResult House8LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House10, time), new[] { HouseName.House8, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse11)]
        public static CalculatorResult House8LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House11, time), new[] { HouseName.House8, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House8LordInHouse12)]
        public static CalculatorResult House8LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House12, time), new[] { HouseName.House8, HouseName.House12 }, time);

        #endregion

        #region Lord of the 9th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse1)]
        public static CalculatorResult House9LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House1, time), new[] { HouseName.House9, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse2)]
        public static CalculatorResult House9LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House2, time), new[] { HouseName.House9, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse3)]
        public static CalculatorResult House9LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House3, time), new[] { HouseName.House9, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse4)]
        public static CalculatorResult House9LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House4, time), new[] { HouseName.House9, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse5)]
        public static CalculatorResult House9LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House5, time), new[] { HouseName.House9, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse6)]
        public static CalculatorResult House9LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House6, time), new[] { HouseName.House9, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse7)]
        public static CalculatorResult House9LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House7, time), new[] { HouseName.House9, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse8)]
        public static CalculatorResult House9LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House8, time), new[] { HouseName.House9, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse9)]
        public static CalculatorResult House9LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House9, time), new[] { HouseName.House9, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse10)]
        public static CalculatorResult House9LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House10, time), new[] { HouseName.House9, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse11)]
        public static CalculatorResult House9LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House11, time), new[] { HouseName.House9, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House9LordInHouse12)]
        public static CalculatorResult House9LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House12, time), new[] { HouseName.House9, HouseName.House12 }, time);

        #endregion

        #region Lord of the 10th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House10LordInHouse1)]
        public static CalculatorResult House10LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House1, time), new[] { HouseName.House10, HouseName.House1 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse2)]
        public static CalculatorResult House10LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House2, time), new[] { HouseName.House10, HouseName.House2 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse3)]
        public static CalculatorResult House10LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House3, time), new[] { HouseName.House10, HouseName.House3 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse4)]
        public static CalculatorResult House10LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House4, time), new[] { HouseName.House10, HouseName.House4 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse5)]
        public static CalculatorResult House10LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House5, time), new[] { HouseName.House10, HouseName.House5 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse6)]
        public static CalculatorResult House10LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House6, time), new[] { HouseName.House10, HouseName.House6 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse7)]
        public static CalculatorResult House10LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House7, time), new[] { HouseName.House10, HouseName.House7 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse8)]
        public static CalculatorResult House10LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House8, time), new[] { HouseName.House10, HouseName.House8 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse9)]
        public static CalculatorResult House10LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House9, time), new[] { HouseName.House10, HouseName.House9 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse10)]
        public static CalculatorResult House10LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House10, time), new[] { HouseName.House10, HouseName.House10 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse11)]
        public static CalculatorResult House10LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House11, time), new[] { HouseName.House10, HouseName.House11 }, time);
        [HoroscopeCalculator(HoroscopeName.House10LordInHouse12)]
        public static CalculatorResult House10LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House12, time), new[] { HouseName.House10, HouseName.House12 }, time);


        #endregion

        #region Lord of the 11th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse1)]
        public static CalculatorResult House11LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House1, time), new[] { HouseName.House11, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse2)]
        public static CalculatorResult House11LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House2, time), new[] { HouseName.House11, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse3)]
        public static CalculatorResult House11LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House3, time), new[] { HouseName.House11, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse4)]
        public static CalculatorResult House11LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House4, time), new[] { HouseName.House11, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse5)]
        public static CalculatorResult House11LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House5, time), new[] { HouseName.House11, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse6)]
        public static CalculatorResult House11LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House6, time), new[] { HouseName.House11, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse7)]
        public static CalculatorResult House11LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House7, time), new[] { HouseName.House11, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse8)]
        public static CalculatorResult House11LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House8, time), new[] { HouseName.House11, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse9)]
        public static CalculatorResult House11LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House9, time), new[] { HouseName.House11, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse10)]
        public static CalculatorResult House11LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House10, time), new[] { HouseName.House11, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse11)]
        public static CalculatorResult House11LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House11, time), new[] { HouseName.House11, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House11LordInHouse12)]
        public static CalculatorResult House11LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House12, time), new[] { HouseName.House11, HouseName.House12 }, time);

        #endregion

        #region Lord of the 12th House Occupying Different Houses

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse1)]
        public static CalculatorResult House12LordInHouse1Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House1, time), new[] { HouseName.House12, HouseName.House1 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse2)]
        public static CalculatorResult House12LordInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House2, time), new[] { HouseName.House12, HouseName.House2 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse3)]
        public static CalculatorResult House12LordInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House3, time), new[] { HouseName.House12, HouseName.House3 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse4)]
        public static CalculatorResult House12LordInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House4, time), new[] { HouseName.House12, HouseName.House4 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse5)]
        public static CalculatorResult House12LordInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House5, time), new[] { HouseName.House12, HouseName.House5 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse6)]
        public static CalculatorResult House12LordInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House6, time), new[] { HouseName.House12, HouseName.House6 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse7)]
        public static CalculatorResult House12LordInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House7, time), new[] { HouseName.House12, HouseName.House7 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse8)]
        public static CalculatorResult House12LordInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House8, time), new[] { HouseName.House12, HouseName.House8 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse9)]
        public static CalculatorResult House12LordInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House9, time), new[] { HouseName.House12, HouseName.House9 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse10)]
        public static CalculatorResult House12LordInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House10, time), new[] { HouseName.House12, HouseName.House10 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse11)]
        public static CalculatorResult House12LordInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House11, time), new[] { HouseName.House12, HouseName.House11 }, time);

        [HoroscopeCalculator(HoroscopeName.House12LordInHouse12)]
        public static CalculatorResult House12LordInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House12, time), new[] { HouseName.House12, HouseName.House12 }, time);

        #endregion


        #region Different Signs Ascending

        [HoroscopeCalculator(HoroscopeName.AriesRising)]
        public static CalculatorResult AriesRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Aries, time), new[] { HouseName.House1, }, new[] { ZodiacName.Aries }, time);

        [HoroscopeCalculator(HoroscopeName.TaurusRising)]
        public static CalculatorResult TaurusRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Taurus, time), new[] { HouseName.House1, }, new[] { ZodiacName.Taurus }, time);

        [HoroscopeCalculator(HoroscopeName.GeminiRising)]
        public static CalculatorResult GeminiRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Gemini, time), new[] { HouseName.House1, }, new[] { ZodiacName.Gemini }, time);

        [HoroscopeCalculator(HoroscopeName.CancerRising)]
        public static CalculatorResult CancerRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Cancer, time), new[] { HouseName.House1, }, new[] { ZodiacName.Cancer }, time);

        [HoroscopeCalculator(HoroscopeName.LeoRising)]
        public static CalculatorResult LeoRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Leo, time), new[] { HouseName.House1, }, new[] { ZodiacName.Leo }, time);

        [HoroscopeCalculator(HoroscopeName.VirgoRising)]
        public static CalculatorResult VirgoRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Virgo, time), new[] { HouseName.House1, }, new[] { ZodiacName.Virgo }, time);

        [HoroscopeCalculator(HoroscopeName.LibraRising)]
        public static CalculatorResult LibraRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Libra, time), new[] { HouseName.House1, }, new[] { ZodiacName.Libra }, time);

        [HoroscopeCalculator(HoroscopeName.ScorpioRising)]
        public static CalculatorResult ScorpioRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Scorpio, time), new[] { HouseName.House1, }, new[] { ZodiacName.Scorpio }, time);

        [HoroscopeCalculator(HoroscopeName.SagittariusRising)]
        public static CalculatorResult SagittariusRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Sagittarius, time), new[] { HouseName.House1, }, new[] { ZodiacName.Sagittarius }, time);

        [HoroscopeCalculator(HoroscopeName.CapricornusRising)]
        public static CalculatorResult CapricornusRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Capricornus, time), new[] { HouseName.House1, }, new[] { ZodiacName.Capricornus }, time);

        [HoroscopeCalculator(HoroscopeName.AquariusRising)]
        public static CalculatorResult AquariusRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Aquarius, time), new[] { HouseName.House1, }, new[] { ZodiacName.Aquarius }, time);

        [HoroscopeCalculator(HoroscopeName.PiscesRising)]
        public static CalculatorResult PiscesRisingOccuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(HouseName.House1, ZodiacName.Pisces, time), new[] { HouseName.House1, }, new[] { ZodiacName.Pisces }, time);

        #endregion


        //Planets in the 1-12th House

        #region Planets in the 1st House

        [HoroscopeCalculator(HoroscopeName.SunInHouse1)]
        public static CalculatorResult SunInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse1)]
        public static CalculatorResult MoonInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse1)]
        public static CalculatorResult MarsInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse1)]
        public static CalculatorResult MercuryInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse1)]
        public static CalculatorResult JupiterInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse1)]
        public static CalculatorResult VenusInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse1)]
        public static CalculatorResult SaturnInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse1)]
        public static CalculatorResult RahuInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse1)]
        public static CalculatorResult KetuInHouse1(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House1), new[] { HouseName.House1, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 2nd House

        [HoroscopeCalculator(HoroscopeName.SunInHouse2)]
        public static CalculatorResult SunInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse2)]
        public static CalculatorResult MoonInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse2)]
        public static CalculatorResult MarsInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse2)]
        public static CalculatorResult MercuryInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse2)]
        public static CalculatorResult JupiterInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse2)]
        public static CalculatorResult VenusInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse2)]
        public static CalculatorResult SaturnInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse2)]
        public static CalculatorResult RahuInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse2)]
        public static CalculatorResult KetuInHouse2Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House2), new[] { HouseName.House2, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 3rd House

        [HoroscopeCalculator(HoroscopeName.SunInHouse3)]
        public static CalculatorResult SunInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse3)]
        public static CalculatorResult MoonInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse3)]
        public static CalculatorResult MarsInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse3)]
        public static CalculatorResult MercuryInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse3)]
        public static CalculatorResult JupiterInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse3)]
        public static CalculatorResult VenusInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse3)]
        public static CalculatorResult SaturnInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse3)]
        public static CalculatorResult RahuInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse3)]
        public static CalculatorResult KetuInHouse3Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House3), new[] { HouseName.House3, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 4th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse4)]
        public static CalculatorResult SunInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse4)]
        public static CalculatorResult MoonInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse4)]
        public static CalculatorResult MarsInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse4)]
        public static CalculatorResult MercuryInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse4)]
        public static CalculatorResult JupiterInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse4)]
        public static CalculatorResult VenusInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse4)]
        public static CalculatorResult SaturnInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse4)]
        public static CalculatorResult RahuInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse4)]
        public static CalculatorResult KetuInHouse4Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House4), new[] { HouseName.House4, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 5th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse5)]
        public static CalculatorResult SunInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse5)]
        public static CalculatorResult MoonInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse5)]
        public static CalculatorResult MarsInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse5)]
        public static CalculatorResult MercuryInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse5)]
        public static CalculatorResult JupiterInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse5)]
        public static CalculatorResult VenusInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse5)]
        public static CalculatorResult SaturnInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse5)]
        public static CalculatorResult RahuInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse5)]
        public static CalculatorResult KetuInHouse5Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House5), new[] { HouseName.House5, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 6th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse6)]
        public static CalculatorResult SunInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse6)]
        public static CalculatorResult MoonInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse6)]
        public static CalculatorResult MarsInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse6)]
        public static CalculatorResult MercuryInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse6)]
        public static CalculatorResult JupiterInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse6)]
        public static CalculatorResult VenusInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse6)]
        public static CalculatorResult SaturnInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse6)]
        public static CalculatorResult RahuInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse6)]
        public static CalculatorResult KetuInHouse6Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House6), new[] { HouseName.House6, }, new[] { PlanetName.Ketu }, time);


        //Planets in the 7th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse7)]
        public static CalculatorResult SunInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse7)]
        public static CalculatorResult MoonInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse7)]
        public static CalculatorResult MarsInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse7)]
        public static CalculatorResult MercuryInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse7)]
        public static CalculatorResult JupiterInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse7)]
        public static CalculatorResult VenusInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse7)]
        public static CalculatorResult SaturnInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse7)]
        public static CalculatorResult RahuInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse7)]
        public static CalculatorResult KetuInHouse7Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House7), new[] { HouseName.House7, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 8th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse8)]
        public static CalculatorResult SunInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse8)]
        public static CalculatorResult MoonInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse8)]
        public static CalculatorResult MarsInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse8)]
        public static CalculatorResult MercuryInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse8)]
        public static CalculatorResult JupiterInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse8)]
        public static CalculatorResult VenusInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse8)]
        public static CalculatorResult SaturnInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse8)]
        public static CalculatorResult RahuInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse8)]
        public static CalculatorResult KetuInHouse8Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House8), new[] { HouseName.House8, }, new[] { PlanetName.Ketu }, time);


        #endregion

        #region Planets in the 9th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse9)]
        public static CalculatorResult SunInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse9)]
        public static CalculatorResult MoonInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse9)]
        public static CalculatorResult MarsInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse9)]
        public static CalculatorResult MercuryInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse9)]
        public static CalculatorResult JupiterInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse9)]
        public static CalculatorResult VenusInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse9)]
        public static CalculatorResult SaturnInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse9)]
        public static CalculatorResult RahuInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse9)]
        public static CalculatorResult KetuInHouse9Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House9), new[] { HouseName.House9, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 10th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse10)]
        public static CalculatorResult SunInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse10)]
        public static CalculatorResult MoonInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse10)]
        public static CalculatorResult MarsInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse10)]
        public static CalculatorResult MercuryInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse10)]
        public static CalculatorResult JupiterInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse10)]
        public static CalculatorResult VenusInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse10)]
        public static CalculatorResult SaturnInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse10)]
        public static CalculatorResult RahuInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse10)]
        public static CalculatorResult KetuInHouse10Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House10), new[] { HouseName.House10, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 11th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse11)]
        public static CalculatorResult SunInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse11)]
        public static CalculatorResult MoonInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse11)]
        public static CalculatorResult MarsInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse11)]
        public static CalculatorResult MercuryInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse11)]
        public static CalculatorResult JupiterInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse11)]
        public static CalculatorResult VenusInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse11)]
        public static CalculatorResult SaturnInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse11)]
        public static CalculatorResult RahuInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse11)]
        public static CalculatorResult KetuInHouse11Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House11), new[] { HouseName.House11, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 12th House

        [HoroscopeCalculator(HoroscopeName.SunInHouse12)]
        public static CalculatorResult SunInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Sun }, time);

        [HoroscopeCalculator(HoroscopeName.MoonInHouse12)]
        public static CalculatorResult MoonInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Moon }, time);

        [HoroscopeCalculator(HoroscopeName.MarsInHouse12)]
        public static CalculatorResult MarsInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Mars }, time);

        [HoroscopeCalculator(HoroscopeName.MercuryInHouse12)]
        public static CalculatorResult MercuryInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Mercury }, time);

        [HoroscopeCalculator(HoroscopeName.JupiterInHouse12)]
        public static CalculatorResult JupiterInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Jupiter }, time);

        [HoroscopeCalculator(HoroscopeName.VenusInHouse12)]
        public static CalculatorResult VenusInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Venus }, time);

        [HoroscopeCalculator(HoroscopeName.SaturnInHouse12)]
        public static CalculatorResult SaturnInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Saturn }, time);

        [HoroscopeCalculator(HoroscopeName.RahuInHouse12)]
        public static CalculatorResult RahuInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Rahu }, time);

        [HoroscopeCalculator(HoroscopeName.KetuInHouse12)]
        public static CalculatorResult KetuInHouse12Occuring(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, HouseName.House12), new[] { HouseName.House12 }, new[] { PlanetName.Ketu }, time);

        #endregion


        #region 2ND HOUSE SPECIAL COMBINATIONS

        [HoroscopeCalculator(HoroscopeName.Lord2WithEvilInHouse)]
        public static CalculatorResult Lord2WithEvilInHouse(Time time)
        {
            //If the 2nd lord is in the 2nd with(1) evil planets or aspected by him(2), he will be poor.
            //NOTE: 1."with" here is interpreted as same house
            //      2. interpreted as evil planets transmitting aspect to 2nd lord (receiving aspect)
            //TODO check validity


            //if 2nd lord not in second, end here
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lordPlace = AstronomicalCalculator.GetHousePlanetIsIn(time, lord);
            if (lordPlace != HouseName.House2) { return CalculatorResult.NotOccuring(); }

            //evil planet in house 2, prediction occuring
            var evilInHouse2 = AstronomicalCalculator.IsMaleficPlanetInHouse(HouseName.House2, time);

            //if evil planets aspect the lord, prediction occuring
            var aspectedByEvil = AstronomicalCalculator.IsPlanetAspectedByMaleficPlanets(lord, time);

            //either one true for prediction to occur
            var occurring = evilInHouse2 || aspectedByEvil;

            return CalculatorResult.New(occurring, lord);
        }

        [HoroscopeCalculator(HoroscopeName.SaturnIn2WithVenus)]
        public static CalculatorResult SaturnIn2WithVenus(Time time)
        {
            //Ordinary wealth is indicated if Saturn is in the 2nd aspected by Venus.

            //if saturn not in 2nd end here
            var saturnHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn);
            var saturnIn2 = saturnHouse == HouseName.House2;
            if (!saturnIn2) { return CalculatorResult.NotOccuring(); }

            //if venus is aspecting saturn, event occuring
            var venusAspecting =
                AstronomicalCalculator.IsPlanetAspectedByPlanet(PlanetName.Saturn, PlanetName.Venus, time);

            return CalculatorResult.New(venusAspecting, new[] { HouseName.House2 }, new[] { PlanetName.Saturn, PlanetName.Venus }, time);
        }

        [HoroscopeCalculator(HoroscopeName.MoonMarsIn2WithSaturnAspect)]
        public static CalculatorResult MoonMarsIn2WithSaturnAspect(Time time)
        {
            //If the Moon and Mars reside in the 2nd bhava and Saturn aspects it,
            //he suffers from a peculiar skin disease.

            //moon and mars in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == HouseName.House2;
            var marsIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == HouseName.House2;

            //saturn aspects 2nd House
            var saturnAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = moonIn2 && marsIn2 && saturnAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mars, PlanetName.Saturn }, time);
        }

        [HoroscopeCalculator(HoroscopeName.MercuryAndEvilIn2WithMoonAspect)]
        public static CalculatorResult MercuryAndEvilIn2WithMoonAspect(Time time)
        {
            //The situation of Mercury in the 2nd with another evil planet aspected by the Moon is bad for saving money.
            //Even if there is any ancestral wealth, it will be spent—rather wasted on extravagant purposes.

            //is mercury in 2nd house
            var mercuryIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == HouseName.House2;

            //evil planet in 2nd house
            var evilPlanetIn2 = AstronomicalCalculator.IsMaleficPlanetInHouse(HouseName.House2, time);

            //moon aspects 2nd House
            var moonAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Moon, time);

            //check if all conditions met
            var occuring = mercuryIn2 && evilPlanetIn2 && moonAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mercury }, time);
        }

        [HoroscopeCalculator(HoroscopeName.SunIn2WithNoSaturnAspect)]
        public static CalculatorResult SunIn2WithNoSaturnAspect(Time time)
        {
            //The Sun in the 2nd without being aspected by Saturn is favourable for a steady fortune.

            //sun in 2nd
            var sunIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == HouseName.House2;

            //saturn aspects 2nd House
            var saturnNotAspects2nd = !AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = sunIn2 && saturnNotAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Sun }, time);
        }

        [HoroscopeCalculator(HoroscopeName.MoonIn2WithMercuryAspect)]
        public static CalculatorResult MoonIn2WithMercuryAspect(Time time)
        {
            //The Moon being placed in the 2nd and aspected by Mercury is favourable for earning money by self-exertion.

            //moon in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == HouseName.House2;

            //mercury aspects 2nd House
            var mercuryAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Mercury, time);

            //check if all conditions met
            var occuring = moonIn2 && mercuryAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mercury }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2And3In6WithEvilPlanet)]
        public static CalculatorResult Lord2And3In6WithEvilPlanet(Time time)
        {
            //He will be poor if lords of the 2nd and 3rd are in the 6th with or aspected by evil planets.

            //lord 2 in 6th
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House6;

            //lord 3 in 6th
            var lord3 = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var lord3In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord3) == HouseName.House6;

            //evil planets in 6th house OR aspecting the 6th
            var evilPlanetIn6 = AstronomicalCalculator.IsMaleficPlanetInHouse(HouseName.House6, time);
            var evilPlanetAspects6 = AstronomicalCalculator.IsMaleficPlanetAspectHouse(HouseName.House6, time);
            var evilPresentIn6 = evilPlanetIn6 || evilPlanetAspects6;

            //check if all conditions met
            var occuring = lord2In6 && lord3In6 && evilPresentIn6;

            return CalculatorResult.New(occuring, new[] { HouseName.House2, HouseName.House3, HouseName.House6 }, new[] { lord2, lord3 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse1)]
        public static CalculatorResult Lord2InHouse1(Time time)
        {
            //If the second lord is in the first — One earns money by his own exertions and generally by manual labour.

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House1;

            //check if all conditions met
            var occuring = lord2In1;

            return CalculatorResult.New(occuring, new[] { HouseName.House1 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse1AndLord1InHouse2)]
        public static CalculatorResult Lord2InHouse1AndLord1InHouse2(Time time)
        {
            //In the second — Riches will be acquired without effort if the 1st and 2nd lords have exchanged their houses.
            //Note: Prediction is part of positions of lord 2 in varies houses,
            //      but for lord 2 in house 2, this "exchange" is mentioned.
            //      Further checking needed.

            //lord 1 in house 2
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var lord1In2 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord1) == HouseName.House2;

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House1;

            //check if all conditions met
            var occuring = lord2In1 && lord1In2;

            return CalculatorResult.New(occuring, new[] { HouseName.House1, HouseName.House2 }, new[] { lord1, lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse3)]
        public static CalculatorResult Lord2InHouse3(Time time)
        {
            //In the third — Loss from relatives, brothers and gain from travels and journeys.

            //lord 2 in house 3
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In3 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House3;

            //check if all conditions met
            var occuring = lord2In3;

            return CalculatorResult.New(occuring, new[] { HouseName.House3 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse4)]
        public static CalculatorResult Lord2InHouse4(Time time)
        {
            //In the fourth - Through mother, inheritance.

            //lord 2 in house 4
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In4 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House4;

            //check if all conditions met
            var occuring = lord2In4;

            return CalculatorResult.New(occuring, new[] { HouseName.House4 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse5)]
        public static CalculatorResult Lord2InHouse5(Time time)
        {
            //In the fifth — Ancestral properties, speculation and chance games.

            //lord 2 in house 5
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In5 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House5;

            //check if all conditions met
            var occuring = lord2In5;

            return CalculatorResult.New(occuring, new[] { HouseName.House5 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse6)]
        public static CalculatorResult Lord2InHouse6(Time time)
        {
            //In the sixth — Broker's business, loss from relatives.

            //lord 2 in house 6
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House6;

            //check if all conditions met
            var occuring = lord2In6;

            return CalculatorResult.New(occuring, new[] { HouseName.House6 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse7)]
        public static CalculatorResult Lord2InHouse7(Time time)
        {
            //In the seventh — Gain after marriage but loss from sickness, etc., of wife.

            //lord 2 in house 7
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In7 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House7;

            //check if all conditions met
            var occuring = lord2In7;

            return CalculatorResult.New(occuring, new[] { HouseName.House7 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse8)]
        public static CalculatorResult Lord2InHouse8(Time time)
        {
            //In the eighth — Legacies and enemies (source of income).

            //lord 2 in house 8
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In8 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House8;

            //check if all conditions met
            var occuring = lord2In8;

            return CalculatorResult.New(occuring, new[] { HouseName.House8 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse9)]
        public static CalculatorResult Lord2InHouse9(Time time)
        {
            //In the ninth — From father, voyages and shipping.

            //lord 2 in house 9
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In9 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House9;

            //check if all conditions met
            var occuring = lord2In9;

            var info = $"Lord 2:{lord2}";
            return CalculatorResult.New(occuring, new[] { HouseName.House9 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse10)]
        public static CalculatorResult Lord2InHouse10(Time time)
        {
            //In the tenth — Profession, eminent people, government favours.

            //lord 2 in house 10
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In10 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House10;

            //check if all conditions met
            var occuring = lord2In10;

            return CalculatorResult.New(occuring, new[] { HouseName.House10 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse11)]
        public static CalculatorResult Lord2InHouse11(Time time)
        {
            //In the eleventh — From different means.

            //lord 2 in house 11
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In11 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House11;

            //check if all conditions met
            var occuring = lord2In11;

            return CalculatorResult.New(occuring, new[] { HouseName.House11 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.Lord2InHouse12)]
        public static CalculatorResult Lord2InHouse12(Time time)
        {
            //In the twelfth — Gain from servants and unscrupulous means including illegal gratifications.

            //lord 2 in house 12
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In12 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == HouseName.House12;

            //check if all conditions met
            var occuring = lord2In12;

            var info = $"Lord 2:{lord2}";
            return CalculatorResult.New(occuring, new[] { HouseName.House12 }, new[] { lord2 }, time);
        }

        [HoroscopeCalculator(HoroscopeName.MaleficIn11FromArudha)]
        public static CalculatorResult MaleficIn11FromArudha(Time time)
        {
            //The just or unjust means of earning depends upon the presence of
            //benefic or malefic planets in the 11th from Arudha Lagna.

            //Note : here only malefic is checked, if benefic are present than not accounted for
            //      it is gussed that results would be mixed, needs further confirmation


            //get Arudha Lagna
            var arudhaLagna = AstronomicalCalculator.GetArudhaLagnaSign(time);

            //get 11th sign from Arudha lagna
            var sign11fromArudha = AstronomicalCalculator.GetSignCountedFromInputSign(arudhaLagna, 11);

            //see if malefic planets are in that sign
            var maleficFound = AstronomicalCalculator.IsMaleficPlanetInSign(sign11fromArudha, time);

            //check if all conditions met
            var occuring = maleficFound;

            var malefics = AstronomicalCalculator.GetMaleficPlanetListInSign(sign11fromArudha, time);

            return CalculatorResult.New(occuring, new[] { HouseName.House11 }, malefics.ToArray(), time);
        }

        [HoroscopeCalculator(HoroscopeName.BeneficIn11FromArudha)]
        public static CalculatorResult BeneficIn11FromArudha(Time time)
        {
            //The just or unjust means of earning depends upon the presence of
            //benefic or malefic planets in the 11th from Arudha Lagna.

            //Note : here only benefic is checked, if malefic are present than not accounted for
            //      it is gussed that results would be mixed, needs further confirmation


            //get Arudha Lagna
            var arudhaLagna = AstronomicalCalculator.GetArudhaLagnaSign(time);

            //get 11th sign from Arudha lagna
            var sign11fromArudha = AstronomicalCalculator.GetSignCountedFromInputSign(arudhaLagna, 11);

            //see if benefic planets are in that sign
            var beneficFound = AstronomicalCalculator.IsBeneficPlanetInSign(sign11fromArudha, time);

            //check if all conditions met
            var occuring = beneficFound;

            var benefics = AstronomicalCalculator.GetBeneficPlanetListInSign(sign11fromArudha, time);

            return CalculatorResult.New(occuring, new[] { HouseName.House11 }, benefics.ToArray(), time);
        }





        //TODO 
        //If the lord of the 2nd is Jupiter or Jupiter resides unaspected by malefics, there will be much wealth.

        //If the lord of the 2nd is Jupiter or Jupiter resides unaspected by malefics, there will be much wealth.
        //He loses wealth if Mercury (aspected by the Moon) contacts this combination.

        //If lords of the 2nd and 11th interchange their places(1) or both are in kendras or quadrants and one aspected
        //or joined by Mercury or Jupiter, the person will be pretty rich.

        //One will always be indigent if lords of the 2nd and 11th remain separate without evil planets or aspected by them.

        //Money will be spent on moral purposes when Jupiter is in the 11th house, Venus in the 2nd and its lord with benefics.

        //If the 2nd lord is with good planets in a kendra or if the 2nd house has all the good
        //association and aspects he will be on good terms with relatives.

        //One becomes a good mathematician if Mars is in the 2nd with the Moon or aspected by Mercury. The same result can be
        //foretold if Jupiter is in the ascendant and Saturn in the 8th or if Jupiter is in a quadrant and the lord of Lagna or Mercury is exalted.

        //The person will be an able debator if the Sun or the Moon is aspected by Jupiter or Venus.


        #endregion


        #region PLANETS IN SIGN

        //SUN
        [HoroscopeCalculator(HoroscopeName.SunInAries)]
        public static CalculatorResult SunInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Aries, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.SunInTaurus)]
        public static CalculatorResult SunInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Taurus, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.SunInGemini)]
        public static CalculatorResult SunInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Gemini, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.SunInCancer)]
        public static CalculatorResult SunInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Cancer, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.SunInLeo)]
        public static CalculatorResult SunInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Leo, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.SunInVirgo)]
        public static CalculatorResult SunInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Virgo, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.SunInLibra)]
        public static CalculatorResult SunInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Libra, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.SunInScorpio)]
        public static CalculatorResult SunInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Scorpio, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.SunInSagittarius)]
        public static CalculatorResult SunInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Sagittarius, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.SunInCapricornus)]
        public static CalculatorResult SunInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Capricornus, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.SunInAquarius)]
        public static CalculatorResult SunInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Aquarius, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.SunInPisces)]
        public static CalculatorResult SunInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Pisces, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Pisces }, time);

        //MOON
        [HoroscopeCalculator(HoroscopeName.MoonInAries)]
        public static CalculatorResult MoonInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Aries, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInTaurus)]
        public static CalculatorResult MoonInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Taurus, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInGemini)]
        public static CalculatorResult MoonInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Gemini, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInCancer)]
        public static CalculatorResult MoonInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Cancer, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInLeo)]
        public static CalculatorResult MoonInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Leo, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInVirgo)]
        public static CalculatorResult MoonInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Virgo, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInLibra)]
        public static CalculatorResult MoonInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Libra, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInScorpio)]
        public static CalculatorResult MoonInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Scorpio, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInSagittarius)]
        public static CalculatorResult MoonInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Sagittarius, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInCapricornus)]
        public static CalculatorResult MoonInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Capricornus, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInAquarius)]
        public static CalculatorResult MoonInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Aquarius, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.MoonInPisces)]
        public static CalculatorResult MoonInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Pisces, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Pisces }, time);


        //MARS
        [HoroscopeCalculator(HoroscopeName.MarsInAries)]
        public static CalculatorResult MarsInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Aries, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInTaurus)]
        public static CalculatorResult MarsInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Taurus, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInGemini)]
        public static CalculatorResult MarsInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Gemini, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInCancer)]
        public static CalculatorResult MarsInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Cancer, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInLeo)]
        public static CalculatorResult MarsInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Leo, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInVirgo)]
        public static CalculatorResult MarsInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Virgo, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInLibra)]
        public static CalculatorResult MarsInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Libra, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInScorpio)]
        public static CalculatorResult MarsInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Scorpio, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInSagittarius)]
        public static CalculatorResult MarsInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Sagittarius, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInCapricornus)]
        public static CalculatorResult MarsInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Capricornus, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInAquarius)]
        public static CalculatorResult MarsInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Aquarius, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.MarsInPisces)]
        public static CalculatorResult MarsInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Pisces, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Pisces }, time);


        //MERCURY
        [HoroscopeCalculator(HoroscopeName.MercuryInAries)]
        public static CalculatorResult MercuryInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Aries, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInTaurus)]
        public static CalculatorResult MercuryInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Taurus, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInGemini)]
        public static CalculatorResult MercuryInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Gemini, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInCancer)]
        public static CalculatorResult MercuryInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Cancer, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInLeo)]
        public static CalculatorResult MercuryInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Leo, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInVirgo)]
        public static CalculatorResult MercuryInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Virgo, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInLibra)]
        public static CalculatorResult MercuryInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Libra, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInScorpio)]
        public static CalculatorResult MercuryInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Scorpio, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInSagittarius)]
        public static CalculatorResult MercuryInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Sagittarius, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInCapricornus)]
        public static CalculatorResult MercuryInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Capricornus, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInAquarius)]
        public static CalculatorResult MercuryInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Aquarius, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.MercuryInPisces)]
        public static CalculatorResult MercuryInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Pisces, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Pisces }, time);

        //JUPITER
        [HoroscopeCalculator(HoroscopeName.JupiterInAries)]
        public static CalculatorResult JupiterInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Aries, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInTaurus)]
        public static CalculatorResult JupiterInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Taurus, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInGemini)]
        public static CalculatorResult JupiterInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Gemini, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInCancer)]
        public static CalculatorResult JupiterInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Cancer, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInLeo)]
        public static CalculatorResult JupiterInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Leo, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInVirgo)]
        public static CalculatorResult JupiterInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Virgo, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInLibra)]
        public static CalculatorResult JupiterInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Libra, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInScorpio)]
        public static CalculatorResult JupiterInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Scorpio, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInSagittarius)]
        public static CalculatorResult JupiterInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Sagittarius, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInCapricornus)]
        public static CalculatorResult JupiterInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Capricornus, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInAquarius)]
        public static CalculatorResult JupiterInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Aquarius, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.JupiterInPisces)]
        public static CalculatorResult JupiterInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Pisces, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Pisces }, time);

        //VENUS
        [HoroscopeCalculator(HoroscopeName.VenusInAries)]
        public static CalculatorResult VenusInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Aries, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInTaurus)]
        public static CalculatorResult VenusInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Taurus, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInGemini)]
        public static CalculatorResult VenusInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Gemini, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInCancer)]
        public static CalculatorResult VenusInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Cancer, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInLeo)]
        public static CalculatorResult VenusInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Leo, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInVirgo)]
        public static CalculatorResult VenusInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Virgo, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInLibra)]
        public static CalculatorResult VenusInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Libra, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInScorpio)]
        public static CalculatorResult VenusInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Scorpio, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInSagittarius)]
        public static CalculatorResult VenusInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Sagittarius, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInCapricornus)]
        public static CalculatorResult VenusInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Capricornus, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInAquarius)]
        public static CalculatorResult VenusInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Aquarius, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.VenusInPisces)]
        public static CalculatorResult VenusInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Pisces, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Pisces }, time);


        //SATURN
        [HoroscopeCalculator(HoroscopeName.SaturnInAries)]
        public static CalculatorResult SaturnInAries(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Aries, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Aries }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInTaurus)]
        public static CalculatorResult SaturnInTaurus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Taurus, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Taurus }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInGemini)]
        public static CalculatorResult SaturnInGemini(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Gemini, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Gemini }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInCancer)]
        public static CalculatorResult SaturnInCancer(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Cancer, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Cancer }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInLeo)]
        public static CalculatorResult SaturnInLeo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Leo, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Leo }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInVirgo)]
        public static CalculatorResult SaturnInVirgo(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Virgo, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Virgo }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInLibra)]
        public static CalculatorResult SaturnInLibra(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Libra, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Libra }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInScorpio)]
        public static CalculatorResult SaturnInScorpio(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Scorpio, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Scorpio }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInSagittarius)]
        public static CalculatorResult SaturnInSagittarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Sagittarius, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Sagittarius }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInCapricornus)]
        public static CalculatorResult SaturnInCapricornus(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Capricornus, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Capricornus }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInAquarius)]
        public static CalculatorResult SaturnInAquarius(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Aquarius, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Aquarius }, time);
        [HoroscopeCalculator(HoroscopeName.SaturnInPisces)]
        public static CalculatorResult SaturnInPisces(Time time) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Pisces, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Pisces }, time);



        #endregion


        #region MARRIAGE

        [HoroscopeCalculator(HoroscopeName.MarsVenusIn7th)]
        public static CalculatorResult MarsVenusIn7th(Time time)
        {
            //When Mars and Venus are in the 7th, the boy or girl concerned will have strong sex instincts
            //and such an individual should be mated to one who has similar instincts

            //mars in 7th at birth
            var marsIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House7);

            //venus in 7th at birth
            var venusIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House7);

            //occuring if all conditions met
            var occuring = marsIn7th && venusIn7th;

            return CalculatorResult.New(occuring, new[] { HouseName.House7 }, new[] { PlanetName.Mars, PlanetName.Venus }, time);

        }

        [HoroscopeCalculator(HoroscopeName.MercuryOrJupiterIn7th)]
        public static CalculatorResult MercuryOrJupiterIn7th(Time time)
        {
            // Mercury or Jupiter in the 7th, makes one under-sexed.
            // And such an individual should not be mated to a person with strong sex instincts.

            //Mercury in 7th at birth
            var mercuryIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, HouseName.House7);

            //Jupiter in 7th at birth
            var jupiterIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, HouseName.House7);

            //occuring if either conditions met
            var occuring = mercuryIn7th || jupiterIn7th;

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.LeoLagna7thLordSaturnIn2)]
        public static CalculatorResult LeoLagna7thLordSaturnIn2(Time time)
        {
            //When Leo is Lagna and the 7th lord Saturn is in the 2nd, the
            // husband will be subservient to the wife carrying out all her orders.

            //lagna is leo
            var leoIsLagna = AstronomicalCalculator.GetHouseSignName(HouseName.House1, time) == ZodiacName.Leo;

            //is 7th lord saturn
            var isLord7thSaturn = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time) ==
                                  PlanetName.Saturn;

            //is saturn in 2nd
            var isSaturnIn2nd =
                AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House2);


            //occuring conditions met
            var occuring = leoIsLagna && isLord7thSaturn && isSaturnIn2nd;

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.SaturnIn7thNotLagnaLord)]
        public static CalculatorResult SaturnIn7thNotLagnaLord(Time time)
        {
            //Saturn in the 7th house is also indicative of unhappiness in marriage
            // unless Saturn happens to be either lord of Lagna or lord of the 7th.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House7);

            //saturn is not lord of lagna
            var saturnNotLagnaLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time) != PlanetName.Saturn;

            //saturn is not lord of 7th
            var saturnNot7thLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time) != PlanetName.Saturn;


            //occuring conditions met
            var occuring = isSaturnIn7th && saturnNotLagnaLord && saturnNot7thLord;

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.MarsIn7thNoBenefics)]
        public static CalculatorResult MarsIn7thNoBenefics(Time time)
        {
            //If Kuja is in the 7th house unaspected or not joined by benefics,
            //there will be frequent quarrels in the married life often leading to
            //misunderstandings and separation.

            //is mars in 7th house
            var isMarsIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, HouseName.House7);

            //no benefics aspecting 7th house
            var beneficsNotAspect7th = !AstronomicalCalculator.IsBeneficPlanetAspectHouse(HouseName.House7, time);

            //no benefics located in 7th
            var beneficNotFoundIn7th = !AstronomicalCalculator.IsBeneficPlanetInHouse(HouseName.House7, time);

            //occuring conditions met
            var occuring = isMarsIn7th && beneficsNotAspect7th && beneficNotFoundIn7th;

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.SunVenusIn5th7th9th)]
        public static CalculatorResult SunVenusIn5th7th9th(Time time)
        {
            //According to Prasna Marga the famous Kerala work on Astrology, if
            //the Sun and Venus occupy the 5th, 7th, or 9th house then the native will
            //lack marital happiness.
            //
            //NOTE : *is intepreted as in the same house at the same time

            //is sun & venus in 5th
            var isSunIn5th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House5);
            var isVenusIn5th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House5);
            var sunAndVenusIn5th = isSunIn5th && isVenusIn5th;

            //is sun & venus in 7th
            var isSunIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House7);
            var isVenusIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House7);
            var sunAndVenusIn7th = isSunIn7th && isVenusIn7th;

            //is sun & venus in 9th
            var isSunIn9th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, HouseName.House9);
            var isVenusIn9th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, HouseName.House9);
            var sunAndVenusIn9th = isSunIn9th && isVenusIn9th;


            //occuring conditions met
            var occuring = sunAndVenusIn5th || sunAndVenusIn7th || sunAndVenusIn9th;

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.Lord7And1Friends)]
        public static CalculatorResult Lord7And1Friends(Time time)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise there will be no harmony.


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1, time);

            //occuring only if best friends or normal friends nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.BestFriend) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Friend);

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.Lord7And1NotFriends)]
        public static CalculatorResult Lord7And1NotFriends(Time time)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise* there will be no harmony.
            //
            //* Intepreted as enemies or bitter enemies only, neutral is not inlcuded


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, time);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1, time);

            //occuring only if bitter enemies or normal enemies nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.BitterEnemy) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Enemy);

            return new() { Occuring = occuring };
        }

        [HoroscopeCalculator(HoroscopeName.SaturnIn7th)]
        public static CalculatorResult SaturnIn7th(Time time)
        {
            //Saturn in the 7th
            //confers stability in the marriage but the, husband or wife manifests
            //coldness and not warmth.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, HouseName.House7);

            //occuring conditions met
            var occuring = isSaturnIn7th;

            return new() { Occuring = occuring};
        }

        #endregion

        #region GENERAL RULES

        //[HoroscopeCalculator(HoroscopeName.LordInTrine)]
        //public static CalculatorResult LordInTrine(Time time)
        //{
        //    //The lords of trines are always ausp1c10us and produce good


        //    return new()
        //    {
        //        Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries
        //    };
        //}

        #endregion

        //CUSTOM
        [HoroscopeCalculator(HoroscopeName.GeminiRisingWithEvilPlanet)]
        public static CalculatorResult GeminiRisingWithEvilPlanet(Time time)
        {
            //1.gemini rising 
            var geminiRising = AstronomicalCalculator.GetHouseSignName(HouseName.House1, time) == ZodiacName.Gemini;

            //2.find evil planets in gemini
            //get planets in sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(ZodiacName.Gemini, time);
            //filer in only evil (malefic) planets 
            var evilPlanets = planetsInSign.Where(planet => AstronomicalCalculator.IsPlanetMalefic(planet, time));
            //mark if evil planets found in sign
            var evilPlanetFound = evilPlanets.Any();


            //both must be true for event to occur
            var occuring = geminiRising && evilPlanetFound;

            //extra info
            return CalculatorResult.New(occuring, new[] { HouseName.House1 }, evilPlanets.ToArray(), new[] { ZodiacName.Gemini }, time);
        }

        [HoroscopeCalculator(HoroscopeName.AriesRisingWithEvilPlanet)]
        public static CalculatorResult AriesRisingWithEvilPlanet(Time time)
        {
            //Mental affliction and derangement are also likely since Saturn and the Moon are in Aries.

            //1.aries rising 
            var ariesRising = AstronomicalCalculator.GetHouseSignName(HouseName.House1, time) == ZodiacName.Aries;

            //2.find if Saturn and the Moon are in Aries.
            //get planets in sign
            var planetsInSign = AstronomicalCalculator.GetPlanetInSign(ZodiacName.Aries, time);
            var evilPlanetFound = planetsInSign.Contains(PlanetName.Saturn) || planetsInSign.Contains(PlanetName.Moon);


            //both must be true for event to occur
            var occuring = ariesRising && evilPlanetFound;

            //extra info
            return CalculatorResult.New(occuring, new[] { HouseName.House1 }, new[] { PlanetName.Saturn, PlanetName.Moon }, new[] { ZodiacName.Aries }, time);
        }

        #endregion

    }
}
