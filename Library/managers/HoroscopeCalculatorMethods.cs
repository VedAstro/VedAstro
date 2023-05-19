using System.Linq;

namespace VedAstro.Library
{
    public class HoroscopeCalculatorMethods
    {
        #region HOROSCOPE

        #region Lord of 1st being Situated in Different Houses

        [EventCalculator(EventName.House1LordInHouse1)]
        public static CalculatorResult House1LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House1, time), new[] { HouseName.House1, HouseName.House1 }, time);

        [EventCalculator(EventName.House1LordInHouse2)]
        public static CalculatorResult House1LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House2, time), new[] { HouseName.House1, HouseName.House2 }, time);

        [EventCalculator(EventName.House1LordInHouse3)]
        public static CalculatorResult House1LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House3, time), new[] { HouseName.House1, HouseName.House3 }, time);

        [EventCalculator(EventName.House1LordInHouse4)]
        public static CalculatorResult House1LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House4, time), new[] { HouseName.House1, HouseName.House4 }, time);

        [EventCalculator(EventName.House1LordInHouse5)]
        public static CalculatorResult House1LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House5, time), new[] { HouseName.House1, HouseName.House5 }, time);

        [EventCalculator(EventName.House1LordInHouse6)]
        public static CalculatorResult House1LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House6, time), new[] { HouseName.House1, HouseName.House6 }, time);

        [EventCalculator(EventName.House1LordInHouse7)]
        public static CalculatorResult House1LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House7, time), new[] { HouseName.House1, HouseName.House7 }, time);

        [EventCalculator(EventName.House1LordInHouse8)]
        public static CalculatorResult House1LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House8, time), new[] { HouseName.House1, HouseName.House8 }, time);

        [EventCalculator(EventName.House1LordInHouse9)]
        public static CalculatorResult House1LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House9, time), new[] { HouseName.House1, HouseName.House9 }, time);

        [EventCalculator(EventName.House1LordInHouse10)]
        public static CalculatorResult House1LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House10, time), new[] { HouseName.House1, HouseName.House10 }, time);

        [EventCalculator(EventName.House1LordInHouse11)]
        public static CalculatorResult House1LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House11, time), new[] { HouseName.House1, HouseName.House11 }, time);

        [EventCalculator(EventName.House1LordInHouse12)]
        public static CalculatorResult House1LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House1, HouseName.House12, time), new[] { HouseName.House1, HouseName.House12 }, time);

        #endregion

        #region Lord of 2nd being Situated in Different Houses

        [EventCalculator(EventName.House2LordInHouse1)]
        public static CalculatorResult House2LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House1, time), new[] { HouseName.House2, HouseName.House1 }, time);

        [EventCalculator(EventName.House2LordInHouse2)]
        public static CalculatorResult House2LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House2, time), new[] { HouseName.House2, HouseName.House2 }, time);

        [EventCalculator(EventName.House2LordInHouse3)]
        public static CalculatorResult House2LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House3, time), new[] { HouseName.House2, HouseName.House3 }, time);

        [EventCalculator(EventName.House2LordInHouse4)]
        public static CalculatorResult House2LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House4, time), new[] { HouseName.House2, HouseName.House4 }, time);

        [EventCalculator(EventName.House2LordInHouse5)]
        public static CalculatorResult House2LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House5, time), new[] { HouseName.House2, HouseName.House5 }, time);

        [EventCalculator(EventName.House2LordInHouse6)]
        public static CalculatorResult House2LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House6, time), new[] { HouseName.House2, HouseName.House6 }, time);

        [EventCalculator(EventName.House2LordInHouse7)]
        public static CalculatorResult House2LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House7, time), new[] { HouseName.House2, HouseName.House7 }, time);

        [EventCalculator(EventName.House2LordInHouse8)]
        public static CalculatorResult House2LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House8, time), new[] { HouseName.House2, HouseName.House8 }, time);

        [EventCalculator(EventName.House2LordInHouse9)]
        public static CalculatorResult House2LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House9, time), new[] { HouseName.House2, HouseName.House9 }, time);

        [EventCalculator(EventName.House2LordInHouse10)]
        public static CalculatorResult House2LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House10, time), new[] { HouseName.House2, HouseName.House10 }, time);

        [EventCalculator(EventName.House2LordInHouse11)]
        public static CalculatorResult House2LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House11, time), new[] { HouseName.House2, HouseName.House11 }, time);

        [EventCalculator(EventName.House2LordInHouse12)]
        public static CalculatorResult House2LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House2, HouseName.House12, time), new[] { HouseName.House2, HouseName.House12 }, time);

        #endregion

        #region Lord of 3rd being Situated in Different Houses

        [EventCalculator(EventName.House3LordInHouse1)]
        public static CalculatorResult House3LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House1, time), new[] { HouseName.House3, HouseName.House1 }, time);

        [EventCalculator(EventName.House3LordInHouse2)]
        public static CalculatorResult House3LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House2, time), new[] { HouseName.House3, HouseName.House2 }, time);

        [EventCalculator(EventName.House3LordInHouse3)]
        public static CalculatorResult House3LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House3, time), new[] { HouseName.House3, HouseName.House3 }, time);

        [EventCalculator(EventName.House3LordInHouse4)]
        public static CalculatorResult House3LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House4, time), new[] { HouseName.House3, HouseName.House4 }, time);

        [EventCalculator(EventName.House3LordInHouse5)]
        public static CalculatorResult House3LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House5, time), new[] { HouseName.House3, HouseName.House5 }, time);

        [EventCalculator(EventName.House3LordInHouse6)]
        public static CalculatorResult House3LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House6, time), new[] { HouseName.House3, HouseName.House6 }, time);

        [EventCalculator(EventName.House3LordInHouse7)]
        public static CalculatorResult House3LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House7, time), new[] { HouseName.House3, HouseName.House7 }, time);

        [EventCalculator(EventName.House3LordInHouse8)]
        public static CalculatorResult House3LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House8, time), new[] { HouseName.House3, HouseName.House8 }, time);

        [EventCalculator(EventName.House3LordInHouse9)]
        public static CalculatorResult House3LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House9, time), new[] { HouseName.House3, HouseName.House9 }, time);

        [EventCalculator(EventName.House3LordInHouse10)]
        public static CalculatorResult House3LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House10, time), new[] { HouseName.House3, HouseName.House10 }, time);

        [EventCalculator(EventName.House3LordInHouse11)]
        public static CalculatorResult House3LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House11, time), new[] { HouseName.House3, HouseName.House11 }, time);

        [EventCalculator(EventName.House3LordInHouse12)]
        public static CalculatorResult House3LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House3, HouseName.House12, time), new[] { HouseName.House3, HouseName.House12 }, time);

        #endregion

        #region Lord of the 4th House Occupying Different Houses

        [EventCalculator(EventName.House4LordInHouse1)]
        public static CalculatorResult House4LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House1, time), new[] { HouseName.House4, HouseName.House1 }, time);

        [EventCalculator(EventName.House4LordInHouse2)]
        public static CalculatorResult House4LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House2, time), new[] { HouseName.House4, HouseName.House2 }, time);

        [EventCalculator(EventName.House4LordInHouse3)]
        public static CalculatorResult House4LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House3, time), new[] { HouseName.House4, HouseName.House3 }, time);

        [EventCalculator(EventName.House4LordInHouse4)]
        public static CalculatorResult House4LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House4, time), new[] { HouseName.House4, HouseName.House4 }, time);

        [EventCalculator(EventName.House4LordInHouse5)]
        public static CalculatorResult House4LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House5, time), new[] { HouseName.House4, HouseName.House5 }, time);

        [EventCalculator(EventName.House4LordInHouse6)]
        public static CalculatorResult House4LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House6, time), new[] { HouseName.House4, HouseName.House6 }, time);

        [EventCalculator(EventName.House4LordInHouse7)]
        public static CalculatorResult House4LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House7, time), new[] { HouseName.House4, HouseName.House7 }, time);

        [EventCalculator(EventName.House4LordInHouse8)]
        public static CalculatorResult House4LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House8, time), new[] { HouseName.House4, HouseName.House8 }, time);

        [EventCalculator(EventName.House4LordInHouse9)]
        public static CalculatorResult House4LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House9, time), new[] { HouseName.House4, HouseName.House9 }, time);

        [EventCalculator(EventName.House4LordInHouse10)]
        public static CalculatorResult House4LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House10, time), new[] { HouseName.House4, HouseName.House10 }, time);

        [EventCalculator(EventName.House4LordInHouse11)]
        public static CalculatorResult House4LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House11, time), new[] { HouseName.House4, HouseName.House11 }, time);

        [EventCalculator(EventName.House4LordInHouse12)]
        public static CalculatorResult House4LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House4, HouseName.House12, time), new[] { HouseName.House4, HouseName.House12 }, time);

        #endregion

        #region Lord of the 5th House Occupying Different Houses

        [EventCalculator(EventName.House5LordInHouse1)]
        public static CalculatorResult House5LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House1, time), new[] { HouseName.House5, HouseName.House1 }, time);

        [EventCalculator(EventName.House5LordInHouse2)]
        public static CalculatorResult House5LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House2, time), new[] { HouseName.House5, HouseName.House2 }, time);

        [EventCalculator(EventName.House5LordInHouse3)]
        public static CalculatorResult House5LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House3, time), new[] { HouseName.House5, HouseName.House3 }, time);

        [EventCalculator(EventName.House5LordInHouse4)]
        public static CalculatorResult House5LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House4, time), new[] { HouseName.House5, HouseName.House4 }, time);

        [EventCalculator(EventName.House5LordInHouse5)]
        public static CalculatorResult House5LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House5, time), new[] { HouseName.House5, HouseName.House5 }, time);

        [EventCalculator(EventName.House5LordInHouse6)]
        public static CalculatorResult House5LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House6, time), new[] { HouseName.House5, HouseName.House6 }, time);

        [EventCalculator(EventName.House5LordInHouse7)]
        public static CalculatorResult House5LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House7, time), new[] { HouseName.House5, HouseName.House7 }, time);

        [EventCalculator(EventName.House5LordInHouse8)]
        public static CalculatorResult House5LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House8, time), new[] { HouseName.House5, HouseName.House8 }, time);

        [EventCalculator(EventName.House5LordInHouse9)]
        public static CalculatorResult House5LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House9, time), new[] { HouseName.House5, HouseName.House9 }, time);

        [EventCalculator(EventName.House5LordInHouse10)]
        public static CalculatorResult House5LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House10, time), new[] { HouseName.House5, HouseName.House10 }, time);

        [EventCalculator(EventName.House5LordInHouse11)]
        public static CalculatorResult House5LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House11, time), new[] { HouseName.House5, HouseName.House11 }, time);

        [EventCalculator(EventName.House5LordInHouse12)]
        public static CalculatorResult House5LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House5, HouseName.House12, time), new[] { HouseName.House5, HouseName.House12 }, time);

        #endregion

        #region Lord of the 6th House Occupying Different Houses

        [EventCalculator(EventName.House6LordInHouse1)]
        public static CalculatorResult House6LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House1, time), new[] { HouseName.House6, HouseName.House1 }, time);

        [EventCalculator(EventName.House6LordInHouse2)]
        public static CalculatorResult House6LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House2, time), new[] { HouseName.House6, HouseName.House2 }, time);

        [EventCalculator(EventName.House6LordInHouse3)]
        public static CalculatorResult House6LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House3, time), new[] { HouseName.House6, HouseName.House3 }, time);

        [EventCalculator(EventName.House6LordInHouse4)]
        public static CalculatorResult House6LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House4, time), new[] { HouseName.House6, HouseName.House4 }, time);

        [EventCalculator(EventName.House6LordInHouse5)]
        public static CalculatorResult House6LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House5, time), new[] { HouseName.House6, HouseName.House5 }, time);

        [EventCalculator(EventName.House6LordInHouse6)]
        public static CalculatorResult House6LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House6, time), new[] { HouseName.House6, HouseName.House6 }, time);

        [EventCalculator(EventName.House6LordInHouse7)]
        public static CalculatorResult House6LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House7, time), new[] { HouseName.House6, HouseName.House7 }, time);

        [EventCalculator(EventName.House6LordInHouse8)]
        public static CalculatorResult House6LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House8, time), new[] { HouseName.House6, HouseName.House8 }, time);

        [EventCalculator(EventName.House6LordInHouse9)]
        public static CalculatorResult House6LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House9, time), new[] { HouseName.House6, HouseName.House9 }, time);

        [EventCalculator(EventName.House6LordInHouse10)]
        public static CalculatorResult House6LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House10, time), new[] { HouseName.House6, HouseName.House10 }, time);

        [EventCalculator(EventName.House6LordInHouse11)]
        public static CalculatorResult House6LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House11, time), new[] { HouseName.House6, HouseName.House11 }, time);

        [EventCalculator(EventName.House6LordInHouse12)]
        public static CalculatorResult House6LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House6, HouseName.House12, time), new[] { HouseName.House6, HouseName.House12 }, time);

        #endregion

        #region Lord of the 7th House Occupying Different Houses

        [EventCalculator(EventName.House7LordInHouse1)]
        public static CalculatorResult House7LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House1, time), new[] { HouseName.House7, HouseName.House1 }, time);
        [EventCalculator(EventName.House7LordInHouse2)]
        public static CalculatorResult House7LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House2, time), new[] { HouseName.House7, HouseName.House2 }, time);
        [EventCalculator(EventName.House7LordInHouse3)]
        public static CalculatorResult House7LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House3, time), new[] { HouseName.House7, HouseName.House3 }, time);
        [EventCalculator(EventName.House7LordInHouse4)]
        public static CalculatorResult House7LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House4, time), new[] { HouseName.House7, HouseName.House4 }, time);
        [EventCalculator(EventName.House7LordInHouse5)]
        public static CalculatorResult House7LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House5, time), new[] { HouseName.House7, HouseName.House5 }, time);
        [EventCalculator(EventName.House7LordInHouse6)]
        public static CalculatorResult House7LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House6, time), new[] { HouseName.House7, HouseName.House6 }, time);
        [EventCalculator(EventName.House7LordInHouse7)]
        public static CalculatorResult House7LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House7, time), new[] { HouseName.House7, HouseName.House7 }, time);
        [EventCalculator(EventName.House7LordInHouse8)]
        public static CalculatorResult House7LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House8, time), new[] { HouseName.House7, HouseName.House8 }, time);
        [EventCalculator(EventName.House7LordInHouse9)]
        public static CalculatorResult House7LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House9, time), new[] { HouseName.House7, HouseName.House9 }, time);
        [EventCalculator(EventName.House7LordInHouse10)]
        public static CalculatorResult House7LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House10, time), new[] { HouseName.House7, HouseName.House10 }, time);
        [EventCalculator(EventName.House7LordInHouse11)]
        public static CalculatorResult House7LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House11, time), new[] { HouseName.House7, HouseName.House11 }, time);
        [EventCalculator(EventName.House7LordInHouse12)]
        public static CalculatorResult House7LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House7, HouseName.House12, time), new[] { HouseName.House7, HouseName.House12 }, time);

        #endregion

        #region Lord of the 8th House Occupying Different Houses

        [EventCalculator(EventName.House8LordInHouse1)]
        public static CalculatorResult House8LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House1, time), new[] { HouseName.House8, HouseName.House1 }, time);

        [EventCalculator(EventName.House8LordInHouse2)]
        public static CalculatorResult House8LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House2, time), new[] { HouseName.House8, HouseName.House2 }, time);

        [EventCalculator(EventName.House8LordInHouse3)]
        public static CalculatorResult House8LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House3, time), new[] { HouseName.House8, HouseName.House3 }, time);

        [EventCalculator(EventName.House8LordInHouse4)]
        public static CalculatorResult House8LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House4, time), new[] { HouseName.House8, HouseName.House4 }, time);

        [EventCalculator(EventName.House8LordInHouse5)]
        public static CalculatorResult House8LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House5, time), new[] { HouseName.House8, HouseName.House5 }, time);

        [EventCalculator(EventName.House8LordInHouse6)]
        public static CalculatorResult House8LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House6, time), new[] { HouseName.House8, HouseName.House6 }, time);

        [EventCalculator(EventName.House8LordInHouse7)]
        public static CalculatorResult House8LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House7, time), new[] { HouseName.House8, HouseName.House7 }, time);

        [EventCalculator(EventName.House8LordInHouse8)]
        public static CalculatorResult House8LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House8, time), new[] { HouseName.House8, HouseName.House8 }, time);

        [EventCalculator(EventName.House8LordInHouse9)]
        public static CalculatorResult House8LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House9, time), new[] { HouseName.House8, HouseName.House9 }, time);

        [EventCalculator(EventName.House8LordInHouse10)]
        public static CalculatorResult House8LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House10, time), new[] { HouseName.House8, HouseName.House10 }, time);

        [EventCalculator(EventName.House8LordInHouse11)]
        public static CalculatorResult House8LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House11, time), new[] { HouseName.House8, HouseName.House11 }, time);

        [EventCalculator(EventName.House8LordInHouse12)]
        public static CalculatorResult House8LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House8, HouseName.House12, time), new[] { HouseName.House8, HouseName.House12 }, time);

        #endregion

        #region Lord of the 9th House Occupying Different Houses

        [EventCalculator(EventName.House9LordInHouse1)]
        public static CalculatorResult House9LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House1, time), new[] { HouseName.House9, HouseName.House1 }, time);

        [EventCalculator(EventName.House9LordInHouse2)]
        public static CalculatorResult House9LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House2, time), new[] { HouseName.House9, HouseName.House2 }, time);

        [EventCalculator(EventName.House9LordInHouse3)]
        public static CalculatorResult House9LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House3, time), new[] { HouseName.House9, HouseName.House3 }, time);

        [EventCalculator(EventName.House9LordInHouse4)]
        public static CalculatorResult House9LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House4, time), new[] { HouseName.House9, HouseName.House4 }, time);

        [EventCalculator(EventName.House9LordInHouse5)]
        public static CalculatorResult House9LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House5, time), new[] { HouseName.House9, HouseName.House5 }, time);

        [EventCalculator(EventName.House9LordInHouse6)]
        public static CalculatorResult House9LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House6, time), new[] { HouseName.House9, HouseName.House6 }, time);

        [EventCalculator(EventName.House9LordInHouse7)]
        public static CalculatorResult House9LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House7, time), new[] { HouseName.House9, HouseName.House7 }, time);

        [EventCalculator(EventName.House9LordInHouse8)]
        public static CalculatorResult House9LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House8, time), new[] { HouseName.House9, HouseName.House8 }, time);

        [EventCalculator(EventName.House9LordInHouse9)]
        public static CalculatorResult House9LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House9, time), new[] { HouseName.House9, HouseName.House9 }, time);

        [EventCalculator(EventName.House9LordInHouse10)]
        public static CalculatorResult House9LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House10, time), new[] { HouseName.House9, HouseName.House10 }, time);

        [EventCalculator(EventName.House9LordInHouse11)]
        public static CalculatorResult House9LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House11, time), new[] { HouseName.House9, HouseName.House11 }, time);

        [EventCalculator(EventName.House9LordInHouse12)]
        public static CalculatorResult House9LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House9, HouseName.House12, time), new[] { HouseName.House9, HouseName.House12 }, time);

        #endregion

        #region Lord of the 10th House Occupying Different Houses

        [EventCalculator(EventName.House10LordInHouse1)]
        public static CalculatorResult House10LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House1, time), new[] { HouseName.House10, HouseName.House1 }, time);
        [EventCalculator(EventName.House10LordInHouse2)]
        public static CalculatorResult House10LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House2, time), new[] { HouseName.House10, HouseName.House2 }, time);
        [EventCalculator(EventName.House10LordInHouse3)]
        public static CalculatorResult House10LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House3, time), new[] { HouseName.House10, HouseName.House3 }, time);
        [EventCalculator(EventName.House10LordInHouse4)]
        public static CalculatorResult House10LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House4, time), new[] { HouseName.House10, HouseName.House4 }, time);
        [EventCalculator(EventName.House10LordInHouse5)]
        public static CalculatorResult House10LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House5, time), new[] { HouseName.House10, HouseName.House5 }, time);
        [EventCalculator(EventName.House10LordInHouse6)]
        public static CalculatorResult House10LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House6, time), new[] { HouseName.House10, HouseName.House6 }, time);
        [EventCalculator(EventName.House10LordInHouse7)]
        public static CalculatorResult House10LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House7, time), new[] { HouseName.House10, HouseName.House7 }, time);
        [EventCalculator(EventName.House10LordInHouse8)]
        public static CalculatorResult House10LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House8, time), new[] { HouseName.House10, HouseName.House8 }, time);
        [EventCalculator(EventName.House10LordInHouse9)]
        public static CalculatorResult House10LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House9, time), new[] { HouseName.House10, HouseName.House9 }, time);
        [EventCalculator(EventName.House10LordInHouse10)]
        public static CalculatorResult House10LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House10, time), new[] { HouseName.House10, HouseName.House10 }, time);
        [EventCalculator(EventName.House10LordInHouse11)]
        public static CalculatorResult House10LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House11, time), new[] { HouseName.House10, HouseName.House11 }, time);
        [EventCalculator(EventName.House10LordInHouse12)]
        public static CalculatorResult House10LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House10, HouseName.House12, time), new[] { HouseName.House10, HouseName.House12 }, time);


        #endregion

        #region Lord of the 11th House Occupying Different Houses

        [EventCalculator(EventName.House11LordInHouse1)]
        public static CalculatorResult House11LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House1, time), new[] { HouseName.House11, HouseName.House1 }, time);

        [EventCalculator(EventName.House11LordInHouse2)]
        public static CalculatorResult House11LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House2, time), new[] { HouseName.House11, HouseName.House2 }, time);

        [EventCalculator(EventName.House11LordInHouse3)]
        public static CalculatorResult House11LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House3, time), new[] { HouseName.House11, HouseName.House3 }, time);

        [EventCalculator(EventName.House11LordInHouse4)]
        public static CalculatorResult House11LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House4, time), new[] { HouseName.House11, HouseName.House4 }, time);

        [EventCalculator(EventName.House11LordInHouse5)]
        public static CalculatorResult House11LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House5, time), new[] { HouseName.House11, HouseName.House5 }, time);

        [EventCalculator(EventName.House11LordInHouse6)]
        public static CalculatorResult House11LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House6, time), new[] { HouseName.House11, HouseName.House6 }, time);

        [EventCalculator(EventName.House11LordInHouse7)]
        public static CalculatorResult House11LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House7, time), new[] { HouseName.House11, HouseName.House7 }, time);

        [EventCalculator(EventName.House11LordInHouse8)]
        public static CalculatorResult House11LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House8, time), new[] { HouseName.House11, HouseName.House8 }, time);

        [EventCalculator(EventName.House11LordInHouse9)]
        public static CalculatorResult House11LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House9, time), new[] { HouseName.House11, HouseName.House9 }, time);

        [EventCalculator(EventName.House11LordInHouse10)]
        public static CalculatorResult House11LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House10, time), new[] { HouseName.House11, HouseName.House10 }, time);

        [EventCalculator(EventName.House11LordInHouse11)]
        public static CalculatorResult House11LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House11, time), new[] { HouseName.House11, HouseName.House11 }, time);

        [EventCalculator(EventName.House11LordInHouse12)]
        public static CalculatorResult House11LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House11, HouseName.House12, time), new[] { HouseName.House11, HouseName.House12 }, time);

        #endregion

        #region Lord of the 12th House Occupying Different Houses

        [EventCalculator(EventName.House12LordInHouse1)]
        public static CalculatorResult House12LordInHouse1Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House1, time), new[] { HouseName.House12, HouseName.House1 }, time);

        [EventCalculator(EventName.House12LordInHouse2)]
        public static CalculatorResult House12LordInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House2, time), new[] { HouseName.House12, HouseName.House2 }, time);

        [EventCalculator(EventName.House12LordInHouse3)]
        public static CalculatorResult House12LordInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House3, time), new[] { HouseName.House12, HouseName.House3 }, time);

        [EventCalculator(EventName.House12LordInHouse4)]
        public static CalculatorResult House12LordInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House4, time), new[] { HouseName.House12, HouseName.House4 }, time);

        [EventCalculator(EventName.House12LordInHouse5)]
        public static CalculatorResult House12LordInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House5, time), new[] { HouseName.House12, HouseName.House5 }, time);

        [EventCalculator(EventName.House12LordInHouse6)]
        public static CalculatorResult House12LordInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House6, time), new[] { HouseName.House12, HouseName.House6 }, time);

        [EventCalculator(EventName.House12LordInHouse7)]
        public static CalculatorResult House12LordInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House7, time), new[] { HouseName.House12, HouseName.House7 }, time);

        [EventCalculator(EventName.House12LordInHouse8)]
        public static CalculatorResult House12LordInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House8, time), new[] { HouseName.House12, HouseName.House8 }, time);

        [EventCalculator(EventName.House12LordInHouse9)]
        public static CalculatorResult House12LordInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House9, time), new[] { HouseName.House12, HouseName.House9 }, time);

        [EventCalculator(EventName.House12LordInHouse10)]
        public static CalculatorResult House12LordInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House10, time), new[] { HouseName.House12, HouseName.House10 }, time);

        [EventCalculator(EventName.House12LordInHouse11)]
        public static CalculatorResult House12LordInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House11, time), new[] { HouseName.House12, HouseName.House11 }, time);

        [EventCalculator(EventName.House12LordInHouse12)]
        public static CalculatorResult House12LordInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseLordInHouse(HouseName.House12, HouseName.House12, time), new[] { HouseName.House12, HouseName.House12 }, time);

        #endregion


        #region Different Signs Ascending

        [EventCalculator(EventName.AriesRising)]
        public static CalculatorResult AriesRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Aries, time), new[] { HouseName.House1, }, new[] { ZodiacName.Aries }, time);

        [EventCalculator(EventName.TaurusRising)]
        public static CalculatorResult TaurusRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Taurus, time), new[] { HouseName.House1, }, new[] { ZodiacName.Taurus }, time);

        [EventCalculator(EventName.GeminiRising)]
        public static CalculatorResult GeminiRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Gemini, time), new[] { HouseName.House1, }, new[] { ZodiacName.Gemini }, time);

        [EventCalculator(EventName.CancerRising)]
        public static CalculatorResult CancerRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Cancer, time), new[] { HouseName.House1, }, new[] { ZodiacName.Cancer }, time);

        [EventCalculator(EventName.LeoRising)]
        public static CalculatorResult LeoRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Leo, time), new[] { HouseName.House1, }, new[] { ZodiacName.Leo }, time);

        [EventCalculator(EventName.VirgoRising)]
        public static CalculatorResult VirgoRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Virgo, time), new[] { HouseName.House1, }, new[] { ZodiacName.Virgo }, time);

        [EventCalculator(EventName.LibraRising)]
        public static CalculatorResult LibraRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Libra, time), new[] { HouseName.House1, }, new[] { ZodiacName.Libra }, time);

        [EventCalculator(EventName.ScorpioRising)]
        public static CalculatorResult ScorpioRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Scorpio, time), new[] { HouseName.House1, }, new[] { ZodiacName.Scorpio }, time);

        [EventCalculator(EventName.SagittariusRising)]
        public static CalculatorResult SagittariusRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Sagittarius, time), new[] { HouseName.House1, }, new[] { ZodiacName.Sagittarius }, time);

        [EventCalculator(EventName.CapricornusRising)]
        public static CalculatorResult CapricornusRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Capricornus, time), new[] { HouseName.House1, }, new[] { ZodiacName.Capricornus }, time);

        [EventCalculator(EventName.AquariusRising)]
        public static CalculatorResult AquariusRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Aquarius, time), new[] { HouseName.House1, }, new[] { ZodiacName.Aquarius }, time);

        [EventCalculator(EventName.PiscesRising)]
        public static CalculatorResult PiscesRisingOccuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsHouseSignName(1, ZodiacName.Pisces, time), new[] { HouseName.House1, }, new[] { ZodiacName.Pisces }, time);

        #endregion


        //Planets in the 1-12th House

        #region Planets in the 1st House

        [EventCalculator(EventName.SunInHouse1)]
        public static CalculatorResult SunInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 1), new[] { HouseName.House1, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse1)]
        public static CalculatorResult MoonInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 1), new[] { HouseName.House1, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse1)]
        public static CalculatorResult MarsInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 1), new[] { HouseName.House1, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse1)]
        public static CalculatorResult MercuryInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 1), new[] { HouseName.House1, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse1)]
        public static CalculatorResult JupiterInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 1), new[] { HouseName.House1, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse1)]
        public static CalculatorResult VenusInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 1), new[] { HouseName.House1, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse1)]
        public static CalculatorResult SaturnInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 1), new[] { HouseName.House1, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse1)]
        public static CalculatorResult RahuInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 1), new[] { HouseName.House1, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse1)]
        public static CalculatorResult KetuInHouse1(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 1), new[] { HouseName.House1, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 2nd House

        [EventCalculator(EventName.SunInHouse2)]
        public static CalculatorResult SunInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 2), new[] { HouseName.House2, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse2)]
        public static CalculatorResult MoonInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 2), new[] { HouseName.House2, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse2)]
        public static CalculatorResult MarsInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 2), new[] { HouseName.House2, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse2)]
        public static CalculatorResult MercuryInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 2), new[] { HouseName.House2, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse2)]
        public static CalculatorResult JupiterInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 2), new[] { HouseName.House2, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse2)]
        public static CalculatorResult VenusInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 2), new[] { HouseName.House2, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse2)]
        public static CalculatorResult SaturnInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 2), new[] { HouseName.House2, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse2)]
        public static CalculatorResult RahuInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 2), new[] { HouseName.House2, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse2)]
        public static CalculatorResult KetuInHouse2Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 2), new[] { HouseName.House2, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 3rd House

        [EventCalculator(EventName.SunInHouse3)]
        public static CalculatorResult SunInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 3), new[] { HouseName.House3, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse3)]
        public static CalculatorResult MoonInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 3), new[] { HouseName.House3, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse3)]
        public static CalculatorResult MarsInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 3), new[] { HouseName.House3, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse3)]
        public static CalculatorResult MercuryInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 3), new[] { HouseName.House3, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse3)]
        public static CalculatorResult JupiterInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 3), new[] { HouseName.House3, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse3)]
        public static CalculatorResult VenusInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 3), new[] { HouseName.House3, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse3)]
        public static CalculatorResult SaturnInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 3), new[] { HouseName.House3, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse3)]
        public static CalculatorResult RahuInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 3), new[] { HouseName.House3, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse3)]
        public static CalculatorResult KetuInHouse3Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 3), new[] { HouseName.House3, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 4th House

        [EventCalculator(EventName.SunInHouse4)]
        public static CalculatorResult SunInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 4), new[] { HouseName.House4, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse4)]
        public static CalculatorResult MoonInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 4), new[] { HouseName.House4, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse4)]
        public static CalculatorResult MarsInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 4), new[] { HouseName.House4, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse4)]
        public static CalculatorResult MercuryInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 4), new[] { HouseName.House4, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse4)]
        public static CalculatorResult JupiterInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 4), new[] { HouseName.House4, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse4)]
        public static CalculatorResult VenusInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 4), new[] { HouseName.House4, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse4)]
        public static CalculatorResult SaturnInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 4), new[] { HouseName.House4, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse4)]
        public static CalculatorResult RahuInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 4), new[] { HouseName.House4, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse4)]
        public static CalculatorResult KetuInHouse4Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 4), new[] { HouseName.House4, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 5th House

        [EventCalculator(EventName.SunInHouse5)]
        public static CalculatorResult SunInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 5), new[] { HouseName.House5, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse5)]
        public static CalculatorResult MoonInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 5), new[] { HouseName.House5, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse5)]
        public static CalculatorResult MarsInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 5), new[] { HouseName.House5, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse5)]
        public static CalculatorResult MercuryInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 5), new[] { HouseName.House5, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse5)]
        public static CalculatorResult JupiterInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 5), new[] { HouseName.House5, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse5)]
        public static CalculatorResult VenusInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 5), new[] { HouseName.House5, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse5)]
        public static CalculatorResult SaturnInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 5), new[] { HouseName.House5, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse5)]
        public static CalculatorResult RahuInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 5), new[] { HouseName.House5, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse5)]
        public static CalculatorResult KetuInHouse5Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 5), new[] { HouseName.House5, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 6th House

        [EventCalculator(EventName.SunInHouse6)]
        public static CalculatorResult SunInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 6), new[] { HouseName.House6, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse6)]
        public static CalculatorResult MoonInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 6), new[] { HouseName.House6, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse6)]
        public static CalculatorResult MarsInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 6), new[] { HouseName.House6, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse6)]
        public static CalculatorResult MercuryInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 6), new[] { HouseName.House6, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse6)]
        public static CalculatorResult JupiterInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 6), new[] { HouseName.House6, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse6)]
        public static CalculatorResult VenusInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 6), new[] { HouseName.House6, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse6)]
        public static CalculatorResult SaturnInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 6), new[] { HouseName.House6, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse6)]
        public static CalculatorResult RahuInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 6), new[] { HouseName.House6, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse6)]
        public static CalculatorResult KetuInHouse6Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 6), new[] { HouseName.House6, }, new[] { PlanetName.Ketu }, time);


        //Planets in the 7th House

        [EventCalculator(EventName.SunInHouse7)]
        public static CalculatorResult SunInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 7), new[] { HouseName.House7, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse7)]
        public static CalculatorResult MoonInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 7), new[] { HouseName.House7, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse7)]
        public static CalculatorResult MarsInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 7), new[] { HouseName.House7, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse7)]
        public static CalculatorResult MercuryInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 7), new[] { HouseName.House7, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse7)]
        public static CalculatorResult JupiterInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 7), new[] { HouseName.House7, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse7)]
        public static CalculatorResult VenusInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 7), new[] { HouseName.House7, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse7)]
        public static CalculatorResult SaturnInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 7), new[] { HouseName.House7, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse7)]
        public static CalculatorResult RahuInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 7), new[] { HouseName.House7, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse7)]
        public static CalculatorResult KetuInHouse7Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 7), new[] { HouseName.House7, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 8th House

        [EventCalculator(EventName.SunInHouse8)]
        public static CalculatorResult SunInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 8), new[] { HouseName.House8, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse8)]
        public static CalculatorResult MoonInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 8), new[] { HouseName.House8, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse8)]
        public static CalculatorResult MarsInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 8), new[] { HouseName.House8, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse8)]
        public static CalculatorResult MercuryInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 8), new[] { HouseName.House8, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse8)]
        public static CalculatorResult JupiterInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 8), new[] { HouseName.House8, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse8)]
        public static CalculatorResult VenusInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 8), new[] { HouseName.House8, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse8)]
        public static CalculatorResult SaturnInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 8), new[] { HouseName.House8, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse8)]
        public static CalculatorResult RahuInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 8), new[] { HouseName.House8, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse8)]
        public static CalculatorResult KetuInHouse8Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 8), new[] { HouseName.House8, }, new[] { PlanetName.Ketu }, time);


        #endregion

        #region Planets in the 9th House

        [EventCalculator(EventName.SunInHouse9)]
        public static CalculatorResult SunInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 9), new[] { HouseName.House9, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse9)]
        public static CalculatorResult MoonInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 9), new[] { HouseName.House9, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse9)]
        public static CalculatorResult MarsInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 9), new[] { HouseName.House9, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse9)]
        public static CalculatorResult MercuryInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 9), new[] { HouseName.House9, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse9)]
        public static CalculatorResult JupiterInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 9), new[] { HouseName.House9, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse9)]
        public static CalculatorResult VenusInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 9), new[] { HouseName.House9, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse9)]
        public static CalculatorResult SaturnInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 9), new[] { HouseName.House9, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse9)]
        public static CalculatorResult RahuInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 9), new[] { HouseName.House9, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse9)]
        public static CalculatorResult KetuInHouse9Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 9), new[] { HouseName.House9, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 10th House

        [EventCalculator(EventName.SunInHouse10)]
        public static CalculatorResult SunInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 10), new[] { HouseName.House10, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse10)]
        public static CalculatorResult MoonInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 10), new[] { HouseName.House10, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse10)]
        public static CalculatorResult MarsInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 10), new[] { HouseName.House10, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse10)]
        public static CalculatorResult MercuryInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 10), new[] { HouseName.House10, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse10)]
        public static CalculatorResult JupiterInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 10), new[] { HouseName.House10, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse10)]
        public static CalculatorResult VenusInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 10), new[] { HouseName.House10, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse10)]
        public static CalculatorResult SaturnInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 10), new[] { HouseName.House10, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse10)]
        public static CalculatorResult RahuInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 10), new[] { HouseName.House10, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse10)]
        public static CalculatorResult KetuInHouse10Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 10), new[] { HouseName.House10, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 11th House

        [EventCalculator(EventName.SunInHouse11)]
        public static CalculatorResult SunInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 11), new[] { HouseName.House11, }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse11)]
        public static CalculatorResult MoonInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 11), new[] { HouseName.House11, }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse11)]
        public static CalculatorResult MarsInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 11), new[] { HouseName.House11, }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse11)]
        public static CalculatorResult MercuryInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 11), new[] { HouseName.House11, }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse11)]
        public static CalculatorResult JupiterInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 11), new[] { HouseName.House11, }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse11)]
        public static CalculatorResult VenusInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 11), new[] { HouseName.House11, }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse11)]
        public static CalculatorResult SaturnInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 11), new[] { HouseName.House11, }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse11)]
        public static CalculatorResult RahuInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 11), new[] { HouseName.House11, }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse11)]
        public static CalculatorResult KetuInHouse11Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 11), new[] { HouseName.House11, }, new[] { PlanetName.Ketu }, time);

        #endregion

        #region Planets in the 12th House

        [EventCalculator(EventName.SunInHouse12)]
        public static CalculatorResult SunInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Sun, 12), new[] { HouseName.House12 }, new[] { PlanetName.Sun }, time);

        [EventCalculator(EventName.MoonInHouse12)]
        public static CalculatorResult MoonInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Moon, 12), new[] { HouseName.House12 }, new[] { PlanetName.Moon }, time);

        [EventCalculator(EventName.MarsInHouse12)]
        public static CalculatorResult MarsInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mars, 12), new[] { HouseName.House12 }, new[] { PlanetName.Mars }, time);

        [EventCalculator(EventName.MercuryInHouse12)]
        public static CalculatorResult MercuryInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Mercury, 12), new[] { HouseName.House12 }, new[] { PlanetName.Mercury }, time);

        [EventCalculator(EventName.JupiterInHouse12)]
        public static CalculatorResult JupiterInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Jupiter, 12), new[] { HouseName.House12 }, new[] { PlanetName.Jupiter }, time);

        [EventCalculator(EventName.VenusInHouse12)]
        public static CalculatorResult VenusInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Venus, 12), new[] { HouseName.House12 }, new[] { PlanetName.Venus }, time);

        [EventCalculator(EventName.SaturnInHouse12)]
        public static CalculatorResult SaturnInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Saturn, 12), new[] { HouseName.House12 }, new[] { PlanetName.Saturn }, time);

        [EventCalculator(EventName.RahuInHouse12)]
        public static CalculatorResult RahuInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Rahu, 12), new[] { HouseName.House12 }, new[] { PlanetName.Rahu }, time);

        [EventCalculator(EventName.KetuInHouse12)]
        public static CalculatorResult KetuInHouse12Occuring(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInHouse(time, PlanetName.Ketu, 12), new[] { HouseName.House12 }, new[] { PlanetName.Ketu }, time);

        #endregion


        #region 2ND HOUSE SPECIAL COMBINATIONS

        [EventCalculator(EventName.Lord2WithEvilInHouse)]
        public static CalculatorResult Lord2WithEvilInHouse(Time time, Person person)
        {
            //If the 2nd lord is in the 2nd with(1) evil planets or aspected by him(2), he will be poor.
            //NOTE: 1."with" here is interpreted as same house
            //      2. interpreted as evil planets transmitting aspect to 2nd lord (receiving aspect)
            //TODO check validity


            //if 2nd lord not in second, end here
            var lord = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lordPlace = AstronomicalCalculator.GetHousePlanetIsIn(time, lord);
            if (lordPlace != 2) { return CalculatorResult.NotOccuring(); }

            //evil planet in house 2, prediction occuring
            var evilInHouse2 = AstronomicalCalculator.IsMaleficPlanetInHouse(2, time);

            //if evil planets aspect the lord, prediction occuring
            var aspectedByEvil = AstronomicalCalculator.IsPlanetAspectedByMaleficPlanets(lord, time);

            //either one true for prediction to occur
            var occurring = evilInHouse2 || aspectedByEvil;

            return CalculatorResult.New(occurring, lord);
        }

        [EventCalculator(EventName.SaturnIn2WithVenus)]
        public static CalculatorResult SaturnIn2WithVenus(Time time, Person person)
        {
            //Ordinary wealth is indicated if Saturn is in the 2nd aspected by Venus.

            //if saturn not in 2nd end here
            var saturnHouse = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Saturn);
            var saturnIn2 = saturnHouse == 2;
            if (!saturnIn2) { return CalculatorResult.NotOccuring(); }

            //if venus is aspecting saturn, event occuring
            var venusAspecting =
                AstronomicalCalculator.IsPlanetAspectedByPlanet(PlanetName.Saturn, PlanetName.Venus, time);

            return CalculatorResult.New(venusAspecting, new[] { HouseName.House2 }, new[] { PlanetName.Saturn, PlanetName.Venus }, time);
        }

        [EventCalculator(EventName.MoonMarsIn2WithSaturnAspect)]
        public static CalculatorResult MoonMarsIn2WithSaturnAspect(Time time, Person person)
        {
            //If the Moon and Mars reside in the 2nd bhava and Saturn aspects it,
            //he suffers from a peculiar skin disease.

            //moon and mars in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 2;
            var marsIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mars) == 2;

            //saturn aspects 2nd House
            var saturnAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = moonIn2 && marsIn2 && saturnAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mars, PlanetName.Saturn }, time);
        }

        [EventCalculator(EventName.MercuryAndEvilIn2WithMoonAspect)]
        public static CalculatorResult MercuryAndEvilIn2WithMoonAspect(Time time, Person person)
        {
            //The situation of Mercury in the 2nd with another evil planet aspected by the Moon is bad for saving money.
            //Even if there is any ancestral wealth, it will be spent—rather wasted on extravagant purposes.

            //is mercury in 2nd house
            var mercuryIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Mercury) == 2;

            //evil planet in 2nd house
            var evilPlanetIn2 = AstronomicalCalculator.IsMaleficPlanetInHouse(2, time);

            //moon aspects 2nd House
            var moonAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Moon, time);

            //check if all conditions met
            var occuring = mercuryIn2 && evilPlanetIn2 && moonAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mercury }, time);
        }

        [EventCalculator(EventName.SunIn2WithNoSaturnAspect)]
        public static CalculatorResult SunIn2WithNoSaturnAspect(Time time, Person person)
        {
            //The Sun in the 2nd without being aspected by Saturn is favourable for a steady fortune.

            //sun in 2nd
            var sunIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Sun) == 2;

            //saturn aspects 2nd House
            var saturnNotAspects2nd = !AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Saturn, time);

            //check if all conditions met
            var occuring = sunIn2 && saturnNotAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.MoonIn2WithMercuryAspect)]
        public static CalculatorResult MoonIn2WithMercuryAspect(Time time, Person person)
        {
            //The Moon being placed in the 2nd and aspected by Mercury is favourable for earning money by self-exertion.

            //moon in 2nd
            var moonIn2 = AstronomicalCalculator.GetHousePlanetIsIn(time, PlanetName.Moon) == 2;

            //mercury aspects 2nd House
            var mercuryAspects2nd =
                AstronomicalCalculator.IsHouseAspectedByPlanet(HouseName.House2, PlanetName.Mercury, time);

            //check if all conditions met
            var occuring = moonIn2 && mercuryAspects2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Moon, PlanetName.Mercury }, time);
        }

        [EventCalculator(EventName.Lord2And3In6WithEvilPlanet)]
        public static CalculatorResult Lord2And3In6WithEvilPlanet(Time time, Person person)
        {
            //He will be poor if lords of the 2nd and 3rd are in the 6th with or aspected by evil planets.

            //lord 2 in 6th
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 6;

            //lord 3 in 6th
            var lord3 = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, time);
            var lord3In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord3) == 6;

            //evil planets in 6th house OR aspecting the 6th
            var evilPlanetIn6 = AstronomicalCalculator.IsMaleficPlanetInHouse(6, time);
            var evilPlanetAspects6 = AstronomicalCalculator.IsMaleficPlanetAspectHouse(HouseName.House6, time);
            var evilPresentIn6 = evilPlanetIn6 || evilPlanetAspects6;

            //check if all conditions met
            var occuring = lord2In6 && lord3In6 && evilPresentIn6;

            return CalculatorResult.New(occuring, new[] { HouseName.House2, HouseName.House3, HouseName.House6 }, new[] { lord2, lord3 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse1)]
        public static CalculatorResult Lord2InHouse1(Time time, Person person)
        {
            //If the second lord is in the first — One earns money by his own exertions and generally by manual labour.

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 1;

            //check if all conditions met
            var occuring = lord2In1;

            return CalculatorResult.New(occuring, new[] { HouseName.House1 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse1AndLord1InHouse2)]
        public static CalculatorResult Lord2InHouse1AndLord1InHouse2(Time time, Person person)
        {
            //In the second — Riches will be acquired without effort if the 1st and 2nd lords have exchanged their houses.
            //Note: Prediction is part of positions of lord 2 in varies houses,
            //      but for lord 2 in house 2, this "exchange" is mentioned.
            //      Further checking needed.

            //lord 1 in house 2
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, time);
            var lord1In2 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord1) == 2;

            //lord 2 in house 1
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In1 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 1;

            //check if all conditions met
            var occuring = lord2In1 && lord1In2;

            return CalculatorResult.New(occuring, new[] { HouseName.House1, HouseName.House2 }, new[] { lord1, lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse3)]
        public static CalculatorResult Lord2InHouse3(Time time, Person person)
        {
            //In the third — Loss from relatives, brothers and gain from travels and journeys.

            //lord 2 in house 3
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In3 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 3;

            //check if all conditions met
            var occuring = lord2In3;

            return CalculatorResult.New(occuring, new[] { HouseName.House3 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse4)]
        public static CalculatorResult Lord2InHouse4(Time time, Person person)
        {
            //In the fourth - Through mother, inheritance.

            //lord 2 in house 4
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In4 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 4;

            //check if all conditions met
            var occuring = lord2In4;

            return CalculatorResult.New(occuring, new[] { HouseName.House4 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse5)]
        public static CalculatorResult Lord2InHouse5(Time time, Person person)
        {
            //In the fifth — Ancestral properties, speculation and chance games.

            //lord 2 in house 5
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In5 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 5;

            //check if all conditions met
            var occuring = lord2In5;

            return CalculatorResult.New(occuring, new[] { HouseName.House5 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse6)]
        public static CalculatorResult Lord2InHouse6(Time time, Person person)
        {
            //In the sixth — Broker's business, loss from relatives.

            //lord 2 in house 6
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In6 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 6;

            //check if all conditions met
            var occuring = lord2In6;

            return CalculatorResult.New(occuring, new[] { HouseName.House6 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse7)]
        public static CalculatorResult Lord2InHouse7(Time time, Person person)
        {
            //In the seventh — Gain after marriage but loss from sickness, etc., of wife.

            //lord 2 in house 7
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In7 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 7;

            //check if all conditions met
            var occuring = lord2In7;

            return CalculatorResult.New(occuring, new[] { HouseName.House7 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse8)]
        public static CalculatorResult Lord2InHouse8(Time time, Person person)
        {
            //In the eighth — Legacies and enemies (source of income).

            //lord 2 in house 8
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In8 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 8;

            //check if all conditions met
            var occuring = lord2In8;

            return CalculatorResult.New(occuring, new[] { HouseName.House8 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse9)]
        public static CalculatorResult Lord2InHouse9(Time time, Person person)
        {
            //In the ninth — From father, voyages and shipping.

            //lord 2 in house 9
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In9 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 9;

            //check if all conditions met
            var occuring = lord2In9;

            var info = $"Lord 2:{lord2}";
            return CalculatorResult.New(occuring, new[] { HouseName.House9 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse10)]
        public static CalculatorResult Lord2InHouse10(Time time, Person person)
        {
            //In the tenth — Profession, eminent people, government favours.

            //lord 2 in house 10
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In10 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 10;

            //check if all conditions met
            var occuring = lord2In10;

            return CalculatorResult.New(occuring, new[] { HouseName.House10 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse11)]
        public static CalculatorResult Lord2InHouse11(Time time, Person person)
        {
            //In the eleventh — From different means.

            //lord 2 in house 11
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In11 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 11;

            //check if all conditions met
            var occuring = lord2In11;

            return CalculatorResult.New(occuring, new[] { HouseName.House11 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.Lord2InHouse12)]
        public static CalculatorResult Lord2InHouse12(Time time, Person person)
        {
            //In the twelfth — Gain from servants and unscrupulous means including illegal gratifications.

            //lord 2 in house 12
            var lord2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, time);
            var lord2In12 = AstronomicalCalculator.GetHousePlanetIsIn(time, lord2) == 12;

            //check if all conditions met
            var occuring = lord2In12;

            var info = $"Lord 2:{lord2}";
            return CalculatorResult.New(occuring, new[] { HouseName.House12 }, new[] { lord2 }, time);
        }

        [EventCalculator(EventName.MaleficIn11FromArudha)]
        public static CalculatorResult MaleficIn11FromArudha(Time time, Person person)
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

        [EventCalculator(EventName.BeneficIn11FromArudha)]
        public static CalculatorResult BeneficIn11FromArudha(Time time, Person person)
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
        [EventCalculator(EventName.SunInAries)]
        public static CalculatorResult SunInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Aries, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.SunInTaurus)]
        public static CalculatorResult SunInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Taurus, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.SunInGemini)]
        public static CalculatorResult SunInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Gemini, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.SunInCancer)]
        public static CalculatorResult SunInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Cancer, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.SunInLeo)]
        public static CalculatorResult SunInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Leo, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.SunInVirgo)]
        public static CalculatorResult SunInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Virgo, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.SunInLibra)]
        public static CalculatorResult SunInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Libra, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.SunInScorpio)]
        public static CalculatorResult SunInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Scorpio, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.SunInSagittarius)]
        public static CalculatorResult SunInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Sagittarius, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.SunInCapricornus)]
        public static CalculatorResult SunInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Capricornus, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.SunInAquarius)]
        public static CalculatorResult SunInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Aquarius, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.SunInPisces)]
        public static CalculatorResult SunInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Sun, ZodiacName.Pisces, time), new[] { PlanetName.Sun }, new[] { ZodiacName.Pisces }, time);

        //MOON
        [EventCalculator(EventName.MoonInAries)]
        public static CalculatorResult MoonInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Aries, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.MoonInTaurus)]
        public static CalculatorResult MoonInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Taurus, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.MoonInGemini)]
        public static CalculatorResult MoonInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Gemini, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.MoonInCancer)]
        public static CalculatorResult MoonInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Cancer, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.MoonInLeo)]
        public static CalculatorResult MoonInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Leo, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.MoonInVirgo)]
        public static CalculatorResult MoonInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Virgo, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.MoonInLibra)]
        public static CalculatorResult MoonInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Libra, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.MoonInScorpio)]
        public static CalculatorResult MoonInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Scorpio, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.MoonInSagittarius)]
        public static CalculatorResult MoonInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Sagittarius, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.MoonInCapricornus)]
        public static CalculatorResult MoonInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Capricornus, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.MoonInAquarius)]
        public static CalculatorResult MoonInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Aquarius, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.MoonInPisces)]
        public static CalculatorResult MoonInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Moon, ZodiacName.Pisces, time), new[] { PlanetName.Moon }, new[] { ZodiacName.Pisces }, time);


        //MARS
        [EventCalculator(EventName.MarsInAries)]
        public static CalculatorResult MarsInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Aries, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.MarsInTaurus)]
        public static CalculatorResult MarsInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Taurus, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.MarsInGemini)]
        public static CalculatorResult MarsInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Gemini, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.MarsInCancer)]
        public static CalculatorResult MarsInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Cancer, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.MarsInLeo)]
        public static CalculatorResult MarsInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Leo, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.MarsInVirgo)]
        public static CalculatorResult MarsInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Virgo, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.MarsInLibra)]
        public static CalculatorResult MarsInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Libra, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.MarsInScorpio)]
        public static CalculatorResult MarsInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Scorpio, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.MarsInSagittarius)]
        public static CalculatorResult MarsInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Sagittarius, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.MarsInCapricornus)]
        public static CalculatorResult MarsInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Capricornus, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.MarsInAquarius)]
        public static CalculatorResult MarsInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Aquarius, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.MarsInPisces)]
        public static CalculatorResult MarsInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mars, ZodiacName.Pisces, time), new[] { PlanetName.Mars }, new[] { ZodiacName.Pisces }, time);


        //MERCURY
        [EventCalculator(EventName.MercuryInAries)]
        public static CalculatorResult MercuryInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Aries, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.MercuryInTaurus)]
        public static CalculatorResult MercuryInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Taurus, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.MercuryInGemini)]
        public static CalculatorResult MercuryInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Gemini, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.MercuryInCancer)]
        public static CalculatorResult MercuryInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Cancer, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.MercuryInLeo)]
        public static CalculatorResult MercuryInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Leo, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.MercuryInVirgo)]
        public static CalculatorResult MercuryInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Virgo, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.MercuryInLibra)]
        public static CalculatorResult MercuryInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Libra, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.MercuryInScorpio)]
        public static CalculatorResult MercuryInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Scorpio, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.MercuryInSagittarius)]
        public static CalculatorResult MercuryInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Sagittarius, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.MercuryInCapricornus)]
        public static CalculatorResult MercuryInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Capricornus, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.MercuryInAquarius)]
        public static CalculatorResult MercuryInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Aquarius, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.MercuryInPisces)]
        public static CalculatorResult MercuryInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Mercury, ZodiacName.Pisces, time), new[] { PlanetName.Mercury }, new[] { ZodiacName.Pisces }, time);

        //JUPITER
        [EventCalculator(EventName.JupiterInAries)]
        public static CalculatorResult JupiterInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Aries, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.JupiterInTaurus)]
        public static CalculatorResult JupiterInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Taurus, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.JupiterInGemini)]
        public static CalculatorResult JupiterInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Gemini, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.JupiterInCancer)]
        public static CalculatorResult JupiterInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Cancer, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.JupiterInLeo)]
        public static CalculatorResult JupiterInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Leo, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.JupiterInVirgo)]
        public static CalculatorResult JupiterInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Virgo, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.JupiterInLibra)]
        public static CalculatorResult JupiterInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Libra, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.JupiterInScorpio)]
        public static CalculatorResult JupiterInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Scorpio, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.JupiterInSagittarius)]
        public static CalculatorResult JupiterInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Sagittarius, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.JupiterInCapricornus)]
        public static CalculatorResult JupiterInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Capricornus, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.JupiterInAquarius)]
        public static CalculatorResult JupiterInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Aquarius, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.JupiterInPisces)]
        public static CalculatorResult JupiterInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Jupiter, ZodiacName.Pisces, time), new[] { PlanetName.Jupiter }, new[] { ZodiacName.Pisces }, time);

        //VENUS
        [EventCalculator(EventName.VenusInAries)]
        public static CalculatorResult VenusInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Aries, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.VenusInTaurus)]
        public static CalculatorResult VenusInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Taurus, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.VenusInGemini)]
        public static CalculatorResult VenusInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Gemini, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.VenusInCancer)]
        public static CalculatorResult VenusInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Cancer, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.VenusInLeo)]
        public static CalculatorResult VenusInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Leo, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.VenusInVirgo)]
        public static CalculatorResult VenusInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Virgo, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.VenusInLibra)]
        public static CalculatorResult VenusInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Libra, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.VenusInScorpio)]
        public static CalculatorResult VenusInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Scorpio, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.VenusInSagittarius)]
        public static CalculatorResult VenusInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Sagittarius, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.VenusInCapricornus)]
        public static CalculatorResult VenusInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Capricornus, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.VenusInAquarius)]
        public static CalculatorResult VenusInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Aquarius, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.VenusInPisces)]
        public static CalculatorResult VenusInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Venus, ZodiacName.Pisces, time), new[] { PlanetName.Venus }, new[] { ZodiacName.Pisces }, time);


        //SATURN
        [EventCalculator(EventName.SaturnInAries)]
        public static CalculatorResult SaturnInAries(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Aries, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Aries }, time);
        [EventCalculator(EventName.SaturnInTaurus)]
        public static CalculatorResult SaturnInTaurus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Taurus, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Taurus }, time);
        [EventCalculator(EventName.SaturnInGemini)]
        public static CalculatorResult SaturnInGemini(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Gemini, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Gemini }, time);
        [EventCalculator(EventName.SaturnInCancer)]
        public static CalculatorResult SaturnInCancer(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Cancer, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Cancer }, time);
        [EventCalculator(EventName.SaturnInLeo)]
        public static CalculatorResult SaturnInLeo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Leo, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Leo }, time);
        [EventCalculator(EventName.SaturnInVirgo)]
        public static CalculatorResult SaturnInVirgo(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Virgo, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Virgo }, time);
        [EventCalculator(EventName.SaturnInLibra)]
        public static CalculatorResult SaturnInLibra(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Libra, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Libra }, time);
        [EventCalculator(EventName.SaturnInScorpio)]
        public static CalculatorResult SaturnInScorpio(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Scorpio, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Scorpio }, time);
        [EventCalculator(EventName.SaturnInSagittarius)]
        public static CalculatorResult SaturnInSagittarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Sagittarius, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Sagittarius }, time);
        [EventCalculator(EventName.SaturnInCapricornus)]
        public static CalculatorResult SaturnInCapricornus(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Capricornus, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Capricornus }, time);
        [EventCalculator(EventName.SaturnInAquarius)]
        public static CalculatorResult SaturnInAquarius(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Aquarius, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Aquarius }, time);
        [EventCalculator(EventName.SaturnInPisces)]
        public static CalculatorResult SaturnInPisces(Time time, Person person) => CalculatorResult.New(AstronomicalCalculator.IsPlanetInSign(PlanetName.Saturn, ZodiacName.Pisces, time), new[] { PlanetName.Saturn }, new[] { ZodiacName.Pisces }, time);



        #endregion


        #region MARRIAGE

        [EventCalculator(EventName.MarsVenusIn7th)]
        public static CalculatorResult MarsVenusIn7th(Time time, Person person)
        {
            //When Mars and Venus are in the 7th, the boy or girl concerned will have strong sex instincts
            //and such an individual should be mated to one who has similar instincts

            //mars in 7th at birth
            var marsIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mars, 7);

            //venus in 7th at birth
            var venusIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 7);

            //occuring if all conditions met
            var occuring = marsIn7th && venusIn7th;

            return CalculatorResult.New(occuring, new[] { HouseName.House7 }, new[] { PlanetName.Mars, PlanetName.Venus }, time);

        }

        [EventCalculator(EventName.MercuryOrJupiterIn7th)]
        public static CalculatorResult MercuryOrJupiterIn7th(Time time, Person person)
        {
            // Mercury or Jupiter in the 7th, makes one under-sexed.
            // And such an individual should not be mated to a person with strong sex instincts.

            //Mercury in 7th at birth
            var mercuryIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mercury, 7);

            //Jupiter in 7th at birth
            var jupiterIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Jupiter, 7);

            //occuring if either conditions met
            var occuring = mercuryIn7th || jupiterIn7th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoLagna7thLordSaturnIn2)]
        public static CalculatorResult LeoLagna7thLordSaturnIn2(Time time, Person person)
        {
            //When Leo is Lagna and the 7th lord Saturn is in the 2nd, the
            // husband will be subservient to the wife carrying out all her orders.

            //lagna is leo
            var leoIsLagna = AstronomicalCalculator.GetHouseSignName(1, person.BirthTime) == ZodiacName.Leo;

            //is 7th lord saturn
            var isLord7thSaturn = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime) ==
                                  PlanetName.Saturn;

            //is saturn in 2nd
            var isSaturnIn2nd =
                AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 2);


            //occuring conditions met
            var occuring = leoIsLagna && isLord7thSaturn && isSaturnIn2nd;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnIn7thNotLagnaLord)]
        public static CalculatorResult SaturnIn7thNotLagnaLord(Time time, Person person)
        {
            //Saturn in the 7th house is also indicative of unhappiness in marriage
            // unless Saturn happens to be either lord of Lagna or lord of the 7th.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 7);

            //saturn is not lord of lagna
            var saturnNotLagnaLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime) != PlanetName.Saturn;

            //saturn is not lord of 7th
            var saturnNot7thLord =
                AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime) != PlanetName.Saturn;


            //occuring conditions met
            var occuring = isSaturnIn7th && saturnNotLagnaLord && saturnNot7thLord;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsIn7thNoBenefics)]
        public static CalculatorResult MarsIn7thNoBenefics(Time time, Person person)
        {
            //If Kuja is in the 7th house unaspected or not joined by benefics,
            //there will be frequent quarrels in the married life often leading to
            //misunderstandings and separation.

            //is mars in 7th house
            var isMarsIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Mars, 7);

            //no benefics aspecting 7th house
            var beneficsNotAspect7th = !AstronomicalCalculator.IsBeneficPlanetAspectHouse(HouseName.House7, person.BirthTime);

            //no benefics located in 7th
            var beneficNotFoundIn7th = !AstronomicalCalculator.IsBeneficPlanetInHouse(7, person.BirthTime);

            //occuring conditions met
            var occuring = isMarsIn7th && beneficsNotAspect7th && beneficNotFoundIn7th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunVenusIn5th7th9th)]
        public static CalculatorResult SunVenusIn5th7th9th(Time time, Person person)
        {
            //According to Prasna Marga the famous Kerala work on Astrology, if
            //the Sun and Venus occupy the 5th, 7th, or 9th house then the native will
            //lack marital happiness.
            //
            //NOTE : *is intepreted as in the same house at the same time

            //is sun & venus in 5th
            var isSunIn5th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 5);
            var isVenusIn5th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 5);
            var sunAndVenusIn5th = isSunIn5th && isVenusIn5th;

            //is sun & venus in 7th
            var isSunIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 7);
            var isVenusIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 7);
            var sunAndVenusIn7th = isSunIn7th && isVenusIn7th;

            //is sun & venus in 9th
            var isSunIn9th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Sun, 9);
            var isVenusIn9th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Venus, 9);
            var sunAndVenusIn9th = isSunIn9th && isVenusIn9th;


            //occuring conditions met
            var occuring = sunAndVenusIn5th || sunAndVenusIn7th || sunAndVenusIn9th;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord7And1Friends)]
        public static CalculatorResult Lord7And1Friends(Time time, Person person)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise there will be no harmony.


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1,
                person.BirthTime);

            //occuring only if best friends or normal friends nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.BestFriend) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Friend);

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord7And1NotFriends)]
        public static CalculatorResult Lord7And1NotFriends(Time time, Person person)
        {

            //If the lords of the 7th and 1st are friends then the native will be loved
            //by his wife. Otherwise* there will be no harmony.
            //
            //* Intepreted as enemies or bitter enemies only, neutral is not inlcuded


            //get lord of 7th and 1st house
            var lord7 = AstronomicalCalculator.GetLordOfHouse(HouseName.House7, person.BirthTime);
            var lord1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //get the relationship
            var lord7And1Relationship = AstronomicalCalculator.GetPlanetCombinedRelationshipWithPlanet(lord7, lord1,
                person.BirthTime);

            //occuring only if bitter enemies or normal enemies nothing else
            var occuring = (lord7And1Relationship == PlanetToPlanetRelationship.BitterEnemy) ||
                           (lord7And1Relationship == PlanetToPlanetRelationship.Enemy);

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnIn7th)]
        public static CalculatorResult SaturnIn7th(Time time, Person person)
        {
            //Saturn in the 7th
            //confers stability in the marriage but the, husband or wife manifests
            //coldness and not warmth.

            //is saturn in 7th house
            var isSaturnIn7th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, PlanetName.Saturn, 7);

            //occuring conditions met
            var occuring = isSaturnIn7th;

            return new() { Occuring = occuring };
        }

        #endregion

        #region GENERAL RULES

        //[EventCalculator(EventName.LordInTrine)]
        //public static CalculatorResult LordInTrine(Time time, Person person)
        //{
        //    //The lords of trines are always ausp1c10us and produce good


        //    return new()
        //    {
        //        Occuring = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries
        //    };
        //}

        #endregion

        //CUSTOM
        [EventCalculator(EventName.GeminiRisingWithEvilPlanet)]
        public static CalculatorResult GeminiRisingWithEvilPlanet(Time time, Person person)
        {
            //1.gemini rising 
            var geminiRising = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Gemini;

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

        [EventCalculator(EventName.AriesRisingWithEvilPlanet)]
        public static CalculatorResult AriesRisingWithEvilPlanet(Time time, Person person)
        {
            //Mental affliction and derangement are also likely since Saturn and the Moon are in Aries.

            //1.aries rising 
            var ariesRising = AstronomicalCalculator.GetHouseSignName(1, time) == ZodiacName.Aries;

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
