using System.Collections.Generic;

namespace VedAstro.Library
{
    /// <summary>
    /// A collection of methods used to calculate if an event is occuring
    /// Note:
    /// - Attributes are used to link a particular method to the event data stored in database
    /// - Split across file because VS IDE started to lag with autocomplete (too much code at once)
    /// </summary>
    public static partial class EventCalculatorMethods
    {
        #region GOCHARA

        [EventCalculator(EventName.GocharaSummary)]
        public static CalculatorResult GocharaSummary(Time time, Person person)
        {
            return CalculatorResult.NotOccuring();
            //get all gochara ocured att time
            var occuringGocharaList = new List<CalculatorResult>() { };

            //loop list
            var goodCount = 0;
            var badCount = 0;
            foreach (var result in occuringGocharaList)
            {
                //result.
            }

            //summarize good & bad count to final value

            //Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 1)
        }

        [EventCalculator(EventName.SunGocharaInHouse1)]
        public static CalculatorResult SunGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 1) };

        [EventCalculator(EventName.SunGocharaInHouse2)]
        public static CalculatorResult SunGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 2) };

        [EventCalculator(EventName.SunGocharaInHouse3)]
        public static CalculatorResult SunGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 3) };

        [EventCalculator(EventName.SunGocharaInHouse4)]
        public static CalculatorResult SunGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 4) };

        [EventCalculator(EventName.SunGocharaInHouse5)]
        public static CalculatorResult SunGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 5) };

        [EventCalculator(EventName.SunGocharaInHouse6)]
        public static CalculatorResult SunGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 6) };

        [EventCalculator(EventName.SunGocharaInHouse7)]
        public static CalculatorResult SunGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 7) };

        [EventCalculator(EventName.SunGocharaInHouse8)]
        public static CalculatorResult SunGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 8) };

        [EventCalculator(EventName.SunGocharaInHouse9)]
        public static CalculatorResult SunGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 9) };

        [EventCalculator(EventName.SunGocharaInHouse10)]
        public static CalculatorResult SunGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 10) };

        [EventCalculator(EventName.SunGocharaInHouse11)]
        public static CalculatorResult SunGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 11) };

        [EventCalculator(EventName.SunGocharaInHouse12)]
        public static CalculatorResult SunGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Sun, 12) };

        //MOON
        [EventCalculator(EventName.MoonGocharaInHouse1)]
        public static CalculatorResult MoonGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 1) };

        [EventCalculator(EventName.MoonGocharaInHouse2)]
        public static CalculatorResult MoonGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 2) };

        [EventCalculator(EventName.MoonGocharaInHouse3)]
        public static CalculatorResult MoonGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 3) };

        [EventCalculator(EventName.MoonGocharaInHouse4)]
        public static CalculatorResult MoonGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 4) };

        [EventCalculator(EventName.MoonGocharaInHouse5)]
        public static CalculatorResult MoonGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 5) };

        [EventCalculator(EventName.MoonGocharaInHouse6)]
        public static CalculatorResult MoonGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 6) };

        [EventCalculator(EventName.MoonGocharaInHouse7)]
        public static CalculatorResult MoonGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 7) };

        [EventCalculator(EventName.MoonGocharaInHouse8)]
        public static CalculatorResult MoonGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 8) };

        [EventCalculator(EventName.MoonGocharaInHouse9)]
        public static CalculatorResult MoonGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 9) };

        [EventCalculator(EventName.MoonGocharaInHouse10)]
        public static CalculatorResult MoonGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 10) };

        [EventCalculator(EventName.MoonGocharaInHouse11)]
        public static CalculatorResult MoonGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 11) };

        [EventCalculator(EventName.MoonGocharaInHouse12)]
        public static CalculatorResult MoonGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Moon, 12) };

        //MARS
        [EventCalculator(EventName.MarsGocharaInHouse1)]
        public static CalculatorResult MarsGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 1) };

        [EventCalculator(EventName.MarsGocharaInHouse2)]
        public static CalculatorResult MarsGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 2) };

        [EventCalculator(EventName.MarsGocharaInHouse3)]
        public static CalculatorResult MarsGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 3) };

        [EventCalculator(EventName.MarsGocharaInHouse4)]
        public static CalculatorResult MarsGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 4) };

        [EventCalculator(EventName.MarsGocharaInHouse5)]
        public static CalculatorResult MarsGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 5) };

        [EventCalculator(EventName.MarsGocharaInHouse6)]
        public static CalculatorResult MarsGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 6) };

        [EventCalculator(EventName.MarsGocharaInHouse7)]
        public static CalculatorResult MarsGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 7) };

        [EventCalculator(EventName.MarsGocharaInHouse8)]
        public static CalculatorResult MarsGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 8) };

        [EventCalculator(EventName.MarsGocharaInHouse9)]
        public static CalculatorResult MarsGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 9) };

        [EventCalculator(EventName.MarsGocharaInHouse10)]
        public static CalculatorResult MarsGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 10) };

        [EventCalculator(EventName.MarsGocharaInHouse11)]
        public static CalculatorResult MarsGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 11) };

        [EventCalculator(EventName.MarsGocharaInHouse12)]
        public static CalculatorResult MarsGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mars, 12) };

        //MERCURY
        [EventCalculator(EventName.MercuryGocharaInHouse1)]
        public static CalculatorResult MercuryGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 1) };

        [EventCalculator(EventName.MercuryGocharaInHouse2)]
        public static CalculatorResult MercuryGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 2) };

        [EventCalculator(EventName.MercuryGocharaInHouse3)]
        public static CalculatorResult MercuryGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 3) };

        [EventCalculator(EventName.MercuryGocharaInHouse4)]
        public static CalculatorResult MercuryGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 4) };

        [EventCalculator(EventName.MercuryGocharaInHouse5)]
        public static CalculatorResult MercuryGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 5) };

        [EventCalculator(EventName.MercuryGocharaInHouse6)]
        public static CalculatorResult MercuryGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 6) };

        [EventCalculator(EventName.MercuryGocharaInHouse7)]
        public static CalculatorResult MercuryGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 7) };

        [EventCalculator(EventName.MercuryGocharaInHouse8)]
        public static CalculatorResult MercuryGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 8) };

        [EventCalculator(EventName.MercuryGocharaInHouse9)]
        public static CalculatorResult MercuryGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 9) };

        [EventCalculator(EventName.MercuryGocharaInHouse10)]
        public static CalculatorResult MercuryGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 10) };

        [EventCalculator(EventName.MercuryGocharaInHouse11)]
        public static CalculatorResult MercuryGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 11) };

        [EventCalculator(EventName.MercuryGocharaInHouse12)]
        public static CalculatorResult MercuryGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Mercury, 12) };

        //JUPITER
        [EventCalculator(EventName.JupiterGocharaInHouse1)]
        public static CalculatorResult JupiterGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 1) };

        [EventCalculator(EventName.JupiterGocharaInHouse2)]
        public static CalculatorResult JupiterGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 2) };

        [EventCalculator(EventName.JupiterGocharaInHouse3)]
        public static CalculatorResult JupiterGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 3) };

        [EventCalculator(EventName.JupiterGocharaInHouse4)]
        public static CalculatorResult JupiterGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 4) };

        [EventCalculator(EventName.JupiterGocharaInHouse5)]
        public static CalculatorResult JupiterGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 5) };

        [EventCalculator(EventName.JupiterGocharaInHouse6)]
        public static CalculatorResult JupiterGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 6) };

        [EventCalculator(EventName.JupiterGocharaInHouse7)]
        public static CalculatorResult JupiterGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 7) };

        [EventCalculator(EventName.JupiterGocharaInHouse8)]
        public static CalculatorResult JupiterGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 8) };

        [EventCalculator(EventName.JupiterGocharaInHouse9)]
        public static CalculatorResult JupiterGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 9) };

        [EventCalculator(EventName.JupiterGocharaInHouse10)]
        public static CalculatorResult JupiterGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 10) };

        [EventCalculator(EventName.JupiterGocharaInHouse11)]
        public static CalculatorResult JupiterGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 11) };

        [EventCalculator(EventName.JupiterGocharaInHouse12)]
        public static CalculatorResult JupiterGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Jupiter, 12) };

        //VENUS
        [EventCalculator(EventName.VenusGocharaInHouse1)]
        public static CalculatorResult VenusGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 1) };

        [EventCalculator(EventName.VenusGocharaInHouse2)]
        public static CalculatorResult VenusGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 2) };

        [EventCalculator(EventName.VenusGocharaInHouse3)]
        public static CalculatorResult VenusGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 3) };

        [EventCalculator(EventName.VenusGocharaInHouse4)]
        public static CalculatorResult VenusGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 4) };

        [EventCalculator(EventName.VenusGocharaInHouse5)]
        public static CalculatorResult VenusGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 5) };

        [EventCalculator(EventName.VenusGocharaInHouse6)]
        public static CalculatorResult VenusGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 6) };

        [EventCalculator(EventName.VenusGocharaInHouse7)]
        public static CalculatorResult VenusGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 7) };

        [EventCalculator(EventName.VenusGocharaInHouse8)]
        public static CalculatorResult VenusGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 8) };

        [EventCalculator(EventName.VenusGocharaInHouse9)]
        public static CalculatorResult VenusGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 9) };

        [EventCalculator(EventName.VenusGocharaInHouse10)]
        public static CalculatorResult VenusGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 10) };

        [EventCalculator(EventName.VenusGocharaInHouse11)]
        public static CalculatorResult VenusGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 11) };

        [EventCalculator(EventName.VenusGocharaInHouse12)]
        public static CalculatorResult VenusGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Venus, 12) };

        //SATURN
        [EventCalculator(EventName.SaturnGocharaInHouse1)]
        public static CalculatorResult SaturnGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 1) };

        [EventCalculator(EventName.SaturnGocharaInHouse2)]
        public static CalculatorResult SaturnGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 2) };

        [EventCalculator(EventName.SaturnGocharaInHouse3)]
        public static CalculatorResult SaturnGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 3) };

        [EventCalculator(EventName.SaturnGocharaInHouse4)]
        public static CalculatorResult SaturnGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 4) };

        [EventCalculator(EventName.SaturnGocharaInHouse5)]
        public static CalculatorResult SaturnGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 5) };

        [EventCalculator(EventName.SaturnGocharaInHouse6)]
        public static CalculatorResult SaturnGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 6) };

        [EventCalculator(EventName.SaturnGocharaInHouse7)]
        public static CalculatorResult SaturnGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 7) };

        [EventCalculator(EventName.SaturnGocharaInHouse8)]
        public static CalculatorResult SaturnGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 8) };

        [EventCalculator(EventName.SaturnGocharaInHouse9)]
        public static CalculatorResult SaturnGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 9) };

        [EventCalculator(EventName.SaturnGocharaInHouse10)]
        public static CalculatorResult SaturnGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 10) };

        [EventCalculator(EventName.SaturnGocharaInHouse11)]
        public static CalculatorResult SaturnGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 11) };

        [EventCalculator(EventName.SaturnGocharaInHouse12)]
        public static CalculatorResult SaturnGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Saturn, 12) };

        //RAHU
        [EventCalculator(EventName.RahuGocharaInHouse1)]
        public static CalculatorResult RahuGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 1) };

        [EventCalculator(EventName.RahuGocharaInHouse2)]
        public static CalculatorResult RahuGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 2) };

        [EventCalculator(EventName.RahuGocharaInHouse3)]
        public static CalculatorResult RahuGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 3) };

        [EventCalculator(EventName.RahuGocharaInHouse4)]
        public static CalculatorResult RahuGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 4) };

        [EventCalculator(EventName.RahuGocharaInHouse5)]
        public static CalculatorResult RahuGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 5) };

        [EventCalculator(EventName.RahuGocharaInHouse6)]
        public static CalculatorResult RahuGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 6) };

        [EventCalculator(EventName.RahuGocharaInHouse7)]
        public static CalculatorResult RahuGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 7) };

        [EventCalculator(EventName.RahuGocharaInHouse8)]
        public static CalculatorResult RahuGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 8) };

        [EventCalculator(EventName.RahuGocharaInHouse9)]
        public static CalculatorResult RahuGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 9) };

        [EventCalculator(EventName.RahuGocharaInHouse10)]
        public static CalculatorResult RahuGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 10) };

        [EventCalculator(EventName.RahuGocharaInHouse11)]
        public static CalculatorResult RahuGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 11) };

        [EventCalculator(EventName.RahuGocharaInHouse12)]
        public static CalculatorResult RahuGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Rahu, 12) };

        //KETU
        [EventCalculator(EventName.KetuGocharaInHouse1)]
        public static CalculatorResult KetuGocharaInHouse1(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 1) };

        [EventCalculator(EventName.KetuGocharaInHouse2)]
        public static CalculatorResult KetuGocharaInHouse2(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 2) };

        [EventCalculator(EventName.KetuGocharaInHouse3)]
        public static CalculatorResult KetuGocharaInHouse3(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 3) };

        [EventCalculator(EventName.KetuGocharaInHouse4)]
        public static CalculatorResult KetuGocharaInHouse4(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 4) };

        [EventCalculator(EventName.KetuGocharaInHouse5)]
        public static CalculatorResult KetuGocharaInHouse5(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 5) };

        [EventCalculator(EventName.KetuGocharaInHouse6)]
        public static CalculatorResult KetuGocharaInHouse6(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 6) };

        [EventCalculator(EventName.KetuGocharaInHouse7)]
        public static CalculatorResult KetuGocharaInHouse7(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 7) };

        [EventCalculator(EventName.KetuGocharaInHouse8)]
        public static CalculatorResult KetuGocharaInHouse8(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 8) };

        [EventCalculator(EventName.KetuGocharaInHouse9)]
        public static CalculatorResult KetuGocharaInHouse9(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 9) };

        [EventCalculator(EventName.KetuGocharaInHouse10)]
        public static CalculatorResult KetuGocharaInHouse10(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 10) };

        [EventCalculator(EventName.KetuGocharaInHouse11)]
        public static CalculatorResult KetuGocharaInHouse11(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 11) };

        [EventCalculator(EventName.KetuGocharaInHouse12)]
        public static CalculatorResult KetuGocharaInHouse12(Time time, Person person) => new() { Occuring = AstronomicalCalculator.IsGocharaOccurring(person.BirthTime, time, PlanetName.Ketu, 12) };

        #endregion GOCHARA

        #region DASAS

        #region SUN DASA

        [EventCalculator(EventName.AriesSunDasa)]
        public static CalculatorResult AriesSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSunDasa)]
        public static CalculatorResult TaurusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSunDasa)]
        public static CalculatorResult GeminiSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSunDasa)]
        public static CalculatorResult CancerSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSunDasa)]
        public static CalculatorResult LeoSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSunDasa)]
        public static CalculatorResult VirgoSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSunDasa)]
        public static CalculatorResult LibraSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSunDasa)]
        public static CalculatorResult ScorpioSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSunDasa)]
        public static CalculatorResult SagittariusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSunDasa)]
        public static CalculatorResult CapricornusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSunDasa)]
        public static CalculatorResult AquariusSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSunDasa)]
        public static CalculatorResult PiscesSunDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion SUN DASA

        #region MOON DASA

        [EventCalculator(EventName.AriesMoonDasa)]
        public static CalculatorResult AriesMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMoonDasa)]
        public static CalculatorResult TaurusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMoonDasa)]
        public static CalculatorResult GeminiMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMoonDasa)]
        public static CalculatorResult CancerMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMoonDasa)]
        public static CalculatorResult LeoMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMoonDasa)]
        public static CalculatorResult VirgoMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMoonDasa)]
        public static CalculatorResult LibraMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMoonDasa)]
        public static CalculatorResult ScorpioMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMoonDasa)]
        public static CalculatorResult SagittariusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMoonDasa)]
        public static CalculatorResult CapricornusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMoonDasa)]
        public static CalculatorResult AquariusMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMoonDasa)]
        public static CalculatorResult PiscesMoonDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion MOON DASA

        #region MARS DASA

        [EventCalculator(EventName.AriesMarsDasa)]
        public static CalculatorResult AriesMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMarsDasa)]
        public static CalculatorResult TaurusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMarsDasa)]
        public static CalculatorResult GeminiMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMarsDasa)]
        public static CalculatorResult CancerMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMarsDasa)]
        public static CalculatorResult LeoMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMarsDasa)]
        public static CalculatorResult VirgoMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMarsDasa)]
        public static CalculatorResult LibraMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMarsDasa)]
        public static CalculatorResult ScorpioMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMarsDasa)]
        public static CalculatorResult SagittariusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMarsDasa)]
        public static CalculatorResult CapricornusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMarsDasa)]
        public static CalculatorResult AquariusMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMarsDasa)]
        public static CalculatorResult PiscesMarsDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion MARS DASA

        #region RAHU DASA

        [EventCalculator(EventName.AriesRahuDasa)]
        public static CalculatorResult AriesRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusRahuDasa)]
        public static CalculatorResult TaurusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiRahuDasa)]
        public static CalculatorResult GeminiRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerRahuDasa)]
        public static CalculatorResult CancerRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoRahuDasa)]
        public static CalculatorResult LeoRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoRahuDasa)]
        public static CalculatorResult VirgoRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraRahuDasa)]
        public static CalculatorResult LibraRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioRahuDasa)]
        public static CalculatorResult ScorpioRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusRahuDasa)]
        public static CalculatorResult SagittariusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusRahuDasa)]
        public static CalculatorResult CapricornusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusRahuDasa)]
        public static CalculatorResult AquariusRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesRahuDasa)]
        public static CalculatorResult PiscesRahuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion RAHU DASA

        #region JUPITER DASA

        [EventCalculator(EventName.AriesJupiterDasa)]
        public static CalculatorResult AriesJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusJupiterDasa)]
        public static CalculatorResult TaurusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiJupiterDasa)]
        public static CalculatorResult GeminiJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerJupiterDasa)]
        public static CalculatorResult CancerJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoJupiterDasa)]
        public static CalculatorResult LeoJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoJupiterDasa)]
        public static CalculatorResult VirgoJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraJupiterDasa)]
        public static CalculatorResult LibraJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioJupiterDasa)]
        public static CalculatorResult ScorpioJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusJupiterDasa)]
        public static CalculatorResult SagittariusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusJupiterDasa)]
        public static CalculatorResult CapricornusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusJupiterDasa)]
        public static CalculatorResult AquariusJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesJupiterDasa)]
        public static CalculatorResult PiscesJupiterDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER DASA

        #region SATURN DASA

        [EventCalculator(EventName.AriesSaturnDasa)]
        public static CalculatorResult AriesSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSaturnDasa)]
        public static CalculatorResult TaurusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSaturnDasa)]
        public static CalculatorResult GeminiSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSaturnDasa)]
        public static CalculatorResult CancerSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSaturnDasa)]
        public static CalculatorResult LeoSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSaturnDasa)]
        public static CalculatorResult VirgoSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSaturnDasa)]
        public static CalculatorResult LibraSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSaturnDasa)]
        public static CalculatorResult ScorpioSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSaturnDasa)]
        public static CalculatorResult SagittariusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSaturnDasa)]
        public static CalculatorResult CapricornusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSaturnDasa)]
        public static CalculatorResult AquariusSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSaturnDasa)]
        public static CalculatorResult PiscesSaturnDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion SATURN DASA

        #region MERCURY DASA

        [EventCalculator(EventName.AriesMercuryDasa)]
        public static CalculatorResult AriesMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMercuryDasa)]
        public static CalculatorResult TaurusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMercuryDasa)]
        public static CalculatorResult GeminiMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMercuryDasa)]
        public static CalculatorResult CancerMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMercuryDasa)]
        public static CalculatorResult LeoMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMercuryDasa)]
        public static CalculatorResult VirgoMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMercuryDasa)]
        public static CalculatorResult LibraMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMercuryDasa)]
        public static CalculatorResult ScorpioMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMercuryDasa)]
        public static CalculatorResult SagittariusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMercuryDasa)]
        public static CalculatorResult CapricornusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMercuryDasa)]
        public static CalculatorResult AquariusMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMercuryDasa)]
        public static CalculatorResult PiscesMercuryDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY DASA

        #region KETU DASA

        [EventCalculator(EventName.AriesKetuDasa)]
        public static CalculatorResult AriesKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusKetuDasa)]
        public static CalculatorResult TaurusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiKetuDasa)]
        public static CalculatorResult GeminiKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerKetuDasa)]
        public static CalculatorResult CancerKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoKetuDasa)]
        public static CalculatorResult LeoKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoKetuDasa)]
        public static CalculatorResult VirgoKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraKetuDasa)]
        public static CalculatorResult LibraKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioKetuDasa)]
        public static CalculatorResult ScorpioKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusKetuDasa)]
        public static CalculatorResult SagittariusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusKetuDasa)]
        public static CalculatorResult CapricornusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusKetuDasa)]
        public static CalculatorResult AquariusKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesKetuDasa)]
        public static CalculatorResult PiscesKetuDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion KETU DASA

        #region VENUS DASA

        [EventCalculator(EventName.AriesVenusDasa)]
        public static CalculatorResult AriesVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusVenusDasa)]
        public static CalculatorResult TaurusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiVenusDasa)]
        public static CalculatorResult GeminiVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerVenusDasa)]
        public static CalculatorResult CancerVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoVenusDasa)]
        public static CalculatorResult LeoVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoVenusDasa)]
        public static CalculatorResult VirgoVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraVenusDasa)]
        public static CalculatorResult LibraVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioVenusDasa)]
        public static CalculatorResult ScorpioVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusVenusDasa)]
        public static CalculatorResult SagittariusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusVenusDasa)]
        public static CalculatorResult CapricornusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusVenusDasa)]
        public static CalculatorResult AquariusVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesVenusDasa)]
        public static CalculatorResult PiscesVenusDasa(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetDasaOcurring = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetDasaOcurring;

            return new() { Occuring = occuring };
        }

        #endregion VENUS DASA

        #endregion DASAS

        //BHUKTI

        #region SUN BHUKTI

        [EventCalculator(EventName.SunDasaSunBhukti)]
        public static CalculatorResult SunDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMoonBhukti)]
        public static CalculatorResult SunDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMarsBhukti)]
        public static CalculatorResult SunDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaRahuBhukti)]
        public static CalculatorResult SunDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaJupiterBhukti)]
        public static CalculatorResult SunDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaSaturnBhukti)]
        public static CalculatorResult SunDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaMercuryBhukti)]
        public static CalculatorResult SunDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaKetuBhukti)]
        public static CalculatorResult SunDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunDasaVenusBhukti)]
        public static CalculatorResult SunDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Sun;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion SUN BHUKTI

        #region MOON BHUKTI

        [EventCalculator(EventName.MoonDasaSunBhukti)]
        public static CalculatorResult MoonDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMoonBhukti)]
        public static CalculatorResult MoonDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMarsBhukti)]
        public static CalculatorResult MoonDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaRahuBhukti)]
        public static CalculatorResult MoonDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaJupiterBhukti)]
        public static CalculatorResult MoonDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaSaturnBhukti)]
        public static CalculatorResult MoonDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaMercuryBhukti)]
        public static CalculatorResult MoonDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaKetuBhukti)]
        public static CalculatorResult MoonDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonDasaVenusBhukti)]
        public static CalculatorResult MoonDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Moon;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion MOON BHUKTI

        #region MARS BHUKTI

        [EventCalculator(EventName.MarsDasaSunBhukti)]
        public static CalculatorResult MarsDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMoonBhukti)]
        public static CalculatorResult MarsDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMarsBhukti)]
        public static CalculatorResult MarsDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaRahuBhukti)]
        public static CalculatorResult MarsDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaJupiterBhukti)]
        public static CalculatorResult MarsDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaSaturnBhukti)]
        public static CalculatorResult MarsDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaMercuryBhukti)]
        public static CalculatorResult MarsDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaKetuBhukti)]
        public static CalculatorResult MarsDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsDasaVenusBhukti)]
        public static CalculatorResult MarsDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mars;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion MARS BHUKTI

        #region RAHU BHUKTI

        [EventCalculator(EventName.RahuDasaSunBhukti)]
        public static CalculatorResult RahuDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMoonBhukti)]
        public static CalculatorResult RahuDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMarsBhukti)]
        public static CalculatorResult RahuDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaRahuBhukti)]
        public static CalculatorResult RahuDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaJupiterBhukti)]
        public static CalculatorResult RahuDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaSaturnBhukti)]
        public static CalculatorResult RahuDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaMercuryBhukti)]
        public static CalculatorResult RahuDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaKetuBhukti)]
        public static CalculatorResult RahuDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuDasaVenusBhukti)]
        public static CalculatorResult RahuDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Rahu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion RAHU BHUKTI

        #region JUPITER BHUKTI

        [EventCalculator(EventName.JupiterDasaSunBhukti)]
        public static CalculatorResult JupiterDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMoonBhukti)]
        public static CalculatorResult JupiterDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMarsBhukti)]
        public static CalculatorResult JupiterDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaRahuBhukti)]
        public static CalculatorResult JupiterDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaJupiterBhukti)]
        public static CalculatorResult JupiterDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaSaturnBhukti)]
        public static CalculatorResult JupiterDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaMercuryBhukti)]
        public static CalculatorResult JupiterDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaKetuBhukti)]
        public static CalculatorResult JupiterDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterDasaVenusBhukti)]
        public static CalculatorResult JupiterDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Jupiter;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER BHUKTI

        #region SATURN BHUKTI

        [EventCalculator(EventName.SaturnDasaSunBhukti)]
        public static CalculatorResult SaturnDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMoonBhukti)]
        public static CalculatorResult SaturnDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMarsBhukti)]
        public static CalculatorResult SaturnDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaRahuBhukti)]
        public static CalculatorResult SaturnDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaJupiterBhukti)]
        public static CalculatorResult SaturnDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaSaturnBhukti)]
        public static CalculatorResult SaturnDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaMercuryBhukti)]
        public static CalculatorResult SaturnDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaKetuBhukti)]
        public static CalculatorResult SaturnDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnDasaVenusBhukti)]
        public static CalculatorResult SaturnDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Saturn;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion SATURN BHUKTI

        #region MERCURY BHUKTI

        [EventCalculator(EventName.MercuryDasaSunBhukti)]
        public static CalculatorResult MercuryDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMoonBhukti)]
        public static CalculatorResult MercuryDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMarsBhukti)]
        public static CalculatorResult MercuryDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaRahuBhukti)]
        public static CalculatorResult MercuryDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaJupiterBhukti)]
        public static CalculatorResult MercuryDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaSaturnBhukti)]
        public static CalculatorResult MercuryDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaMercuryBhukti)]
        public static CalculatorResult MercuryDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaKetuBhukti)]
        public static CalculatorResult MercuryDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryDasaVenusBhukti)]
        public static CalculatorResult MercuryDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Mercury;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY BHUKTI

        #region KETU BHUKTI

        [EventCalculator(EventName.KetuDasaSunBhukti)]
        public static CalculatorResult KetuDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMoonBhukti)]
        public static CalculatorResult KetuDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMarsBhukti)]
        public static CalculatorResult KetuDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaRahuBhukti)]
        public static CalculatorResult KetuDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaJupiterBhukti)]
        public static CalculatorResult KetuDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaSaturnBhukti)]
        public static CalculatorResult KetuDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaMercuryBhukti)]
        public static CalculatorResult KetuDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaKetuBhukti)]
        public static CalculatorResult KetuDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuDasaVenusBhukti)]
        public static CalculatorResult KetuDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Ketu;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion KETU BHUKTI

        #region VENUS BHUKTI

        [EventCalculator(EventName.VenusDasaSunBhukti)]
        public static CalculatorResult VenusDasaSunBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var dasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var bhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMoonBhukti)]
        public static CalculatorResult VenusDasaMoonBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMarsBhukti)]
        public static CalculatorResult VenusDasaMarsBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaRahuBhukti)]
        public static CalculatorResult VenusDasaRahuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaJupiterBhukti)]
        public static CalculatorResult VenusDasaJupiterBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaSaturnBhukti)]
        public static CalculatorResult VenusDasaSaturnBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaMercuryBhukti)]
        public static CalculatorResult VenusDasaMercuryBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaKetuBhukti)]
        public static CalculatorResult VenusDasaKetuBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusDasaVenusBhukti)]
        public static CalculatorResult VenusDasaVenusBhukti(Time time, Person person)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check dasa
            var isCorrectDasa = currentDasaBhuktiAntaram.Dasa == PlanetName.Venus;

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrectDasa && isCorrectBhukti;

            return new() { Occuring = occuring };
        }

        #endregion VENUS BHUKTI

        //ANTARAM

        #region SUN ANTARAMS

        [EventCalculator(EventName.SunBhuktiSunAntaram)]
        public static CalculatorResult SunBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonBhuktiSunAntaram)]
        public static CalculatorResult MoonBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsBhuktiSunAntaram)]
        public static CalculatorResult MarsBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuBhuktiSunAntaram)]
        public static CalculatorResult RahuBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterBhuktiSunAntaram)]
        public static CalculatorResult JupiterBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnBhuktiSunAntaram)]
        public static CalculatorResult SaturnBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercuryBhuktiSunAntaram)]
        public static CalculatorResult MercuryBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuBhuktiSunAntaram)]
        public static CalculatorResult KetuBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusBhuktiSunAntaram)]
        public static CalculatorResult VenusBhuktiSunAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN ANTARAMS

        #region MOON ANTARAMS

        [EventCalculator(EventName.SunBhuktiMoonAntaram)]
        public static CalculatorResult SunBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonBhuktiMoonAntaram)]
        public static CalculatorResult MoonBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsBhuktiMoonAntaram)]
        public static CalculatorResult MarsBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuBhuktiMoonAntaram)]
        public static CalculatorResult RahuBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterBhuktiMoonAntaram)]
        public static CalculatorResult JupiterBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnBhuktiMoonAntaram)]
        public static CalculatorResult SaturnBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryBhuktiMoonAntaram)]
        public static CalculatorResult MercuryBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuBhuktiMoonAntaram)]
        public static CalculatorResult KetuBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusBhuktiMoonAntaram)]
        public static CalculatorResult VenusBhuktiMoonAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON ANTARAMS

        #region MARS ANTARAMS

        [EventCalculator(EventName.SunBhuktiMarsAntaram)]
        public static CalculatorResult SunBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonBhuktiMarsAntaram)]
        public static CalculatorResult MoonBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsBhuktiMarsAntaram)]
        public static CalculatorResult MarsBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuBhuktiMarsAntaram)]
        public static CalculatorResult RahuBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterBhuktiMarsAntaram)]
        public static CalculatorResult JupiterBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnBhuktiMarsAntaram)]
        public static CalculatorResult SaturnBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryBhuktiMarsAntaram)]
        public static CalculatorResult MercuryBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuBhuktiMarsAntaram)]
        public static CalculatorResult KetuBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusBhuktiMarsAntaram)]
        public static CalculatorResult VenusBhuktiMarsAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS ANTARAMS

        #region RAHU ANTARAMS

        [EventCalculator(EventName.SunBhuktiRahuAntaram)]
        public static CalculatorResult SunBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonBhuktiRahuAntaram)]
        public static CalculatorResult MoonBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsBhuktiRahuAntaram)]
        public static CalculatorResult MarsBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuBhuktiRahuAntaram)]
        public static CalculatorResult RahuBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterBhuktiRahuAntaram)]
        public static CalculatorResult JupiterBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnBhuktiRahuAntaram)]
        public static CalculatorResult SaturnBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryBhuktiRahuAntaram)]
        public static CalculatorResult MercuryBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuBhuktiRahuAntaram)]
        public static CalculatorResult KetuBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusBhuktiRahuAntaram)]
        public static CalculatorResult VenusBhuktiRahuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU ANTARAMS

        #region JUPITER ANTARAMS

        [EventCalculator(EventName.SunBhuktiJupiterAntaram)]
        public static CalculatorResult SunBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonBhuktiJupiterAntaram)]
        public static CalculatorResult MoonBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsBhuktiJupiterAntaram)]
        public static CalculatorResult MarsBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuBhuktiJupiterAntaram)]
        public static CalculatorResult RahuBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterBhuktiJupiterAntaram)]
        public static CalculatorResult JupiterBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnBhuktiJupiterAntaram)]
        public static CalculatorResult SaturnBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryBhuktiJupiterAntaram)]
        public static CalculatorResult MercuryBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuBhuktiJupiterAntaram)]
        public static CalculatorResult KetuBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusBhuktiJupiterAntaram)]
        public static CalculatorResult VenusBhuktiJupiterAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER ANTARAMS

        #region SATURN ANTARAMS

        [EventCalculator(EventName.SunBhuktiSaturnAntaram)]
        public static CalculatorResult SunBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonBhuktiSaturnAntaram)]
        public static CalculatorResult MoonBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsBhuktiSaturnAntaram)]
        public static CalculatorResult MarsBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuBhuktiSaturnAntaram)]
        public static CalculatorResult RahuBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterBhuktiSaturnAntaram)]
        public static CalculatorResult JupiterBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnBhuktiSaturnAntaram)]
        public static CalculatorResult SaturnBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercuryBhuktiSaturnAntaram)]
        public static CalculatorResult MercuryBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuBhuktiSaturnAntaram)]
        public static CalculatorResult KetuBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusBhuktiSaturnAntaram)]
        public static CalculatorResult VenusBhuktiSaturnAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN ANTARAMS

        #region MERCURY ANTARAMS

        [EventCalculator(EventName.SunBhuktiMercuryAntaram)]
        public static CalculatorResult SunBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonBhuktiMercuryAntaram)]
        public static CalculatorResult MoonBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsBhuktiMercuryAntaram)]
        public static CalculatorResult MarsBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuBhuktiMercuryAntaram)]
        public static CalculatorResult RahuBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterBhuktiMercuryAntaram)]
        public static CalculatorResult JupiterBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnBhuktiMercuryAntaram)]
        public static CalculatorResult SaturnBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryBhuktiMercuryAntaram)]
        public static CalculatorResult MercuryBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuBhuktiMercuryAntaram)]
        public static CalculatorResult KetuBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusBhuktiMercuryAntaram)]
        public static CalculatorResult VenusBhuktiMercuryAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY ANTARAMS

        #region KETU ANTARAMS

        [EventCalculator(EventName.SunBhuktiKetuAntaram)]
        public static CalculatorResult SunBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonBhuktiKetuAntaram)]
        public static CalculatorResult MoonBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsBhuktiKetuAntaram)]
        public static CalculatorResult MarsBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuBhuktiKetuAntaram)]
        public static CalculatorResult RahuBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterBhuktiKetuAntaram)]
        public static CalculatorResult JupiterBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnBhuktiKetuAntaram)]
        public static CalculatorResult SaturnBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryBhuktiKetuAntaram)]
        public static CalculatorResult MercuryBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuBhuktiKetuAntaram)]
        public static CalculatorResult KetuBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusBhuktiKetuAntaram)]
        public static CalculatorResult VenusBhuktiKetuAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU ANTARAMS

        #region VENUS ANTARAMS

        [EventCalculator(EventName.SunBhuktiVenusAntaram)]
        public static CalculatorResult SunBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonBhuktiVenusAntaram)]
        public static CalculatorResult MoonBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsBhuktiVenusAntaram)]
        public static CalculatorResult MarsBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuBhuktiVenusAntaram)]
        public static CalculatorResult RahuBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterBhuktiVenusAntaram)]
        public static CalculatorResult JupiterBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnBhuktiVenusAntaram)]
        public static CalculatorResult SaturnBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryBhuktiVenusAntaram)]
        public static CalculatorResult MercuryBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuBhuktiVenusAntaram)]
        public static CalculatorResult KetuBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusBhuktiVenusAntaram)]
        public static CalculatorResult VenusBhuktiVenusAntaram(Time time, Person person) => PlanetBhuktiPlanetAntaram(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS ANTARAMS

        //SUKSHMA

        #region SUN SUKSHMA

        [EventCalculator(EventName.SunAntaramSunSukshma)]
        public static CalculatorResult SunAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonAntaramSunSukshma)]
        public static CalculatorResult MoonAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsAntaramSunSukshma)]
        public static CalculatorResult MarsAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuAntaramSunSukshma)]
        public static CalculatorResult RahuAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterAntaramSunSukshma)]
        public static CalculatorResult JupiterAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnAntaramSunSukshma)]
        public static CalculatorResult SaturnAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercuryAntaramSunSukshma)]
        public static CalculatorResult MercuryAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuAntaramSunSukshma)]
        public static CalculatorResult KetuAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusAntaramSunSukshma)]
        public static CalculatorResult VenusAntaramSunSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN SUKSHMA

        #region MOON SUKSHMA

        [EventCalculator(EventName.SunAntaramMoonSukshma)]
        public static CalculatorResult SunAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonAntaramMoonSukshma)]
        public static CalculatorResult MoonAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsAntaramMoonSukshma)]
        public static CalculatorResult MarsAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuAntaramMoonSukshma)]
        public static CalculatorResult RahuAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterAntaramMoonSukshma)]
        public static CalculatorResult JupiterAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnAntaramMoonSukshma)]
        public static CalculatorResult SaturnAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryAntaramMoonSukshma)]
        public static CalculatorResult MercuryAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuAntaramMoonSukshma)]
        public static CalculatorResult KetuAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusAntaramMoonSukshma)]
        public static CalculatorResult VenusAntaramMoonSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON SUKSHMA

        #region MARS SUKSHMA

        [EventCalculator(EventName.SunAntaramMarsSukshma)]
        public static CalculatorResult SunAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonAntaramMarsSukshma)]
        public static CalculatorResult MoonAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsAntaramMarsSukshma)]
        public static CalculatorResult MarsAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuAntaramMarsSukshma)]
        public static CalculatorResult RahuAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterAntaramMarsSukshma)]
        public static CalculatorResult JupiterAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnAntaramMarsSukshma)]
        public static CalculatorResult SaturnAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryAntaramMarsSukshma)]
        public static CalculatorResult MercuryAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuAntaramMarsSukshma)]
        public static CalculatorResult KetuAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusAntaramMarsSukshma)]
        public static CalculatorResult VenusAntaramMarsSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS SUKSHMA

        #region RAHU SUKSHMA

        [EventCalculator(EventName.SunAntaramRahuSukshma)]
        public static CalculatorResult SunAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonAntaramRahuSukshma)]
        public static CalculatorResult MoonAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsAntaramRahuSukshma)]
        public static CalculatorResult MarsAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuAntaramRahuSukshma)]
        public static CalculatorResult RahuAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterAntaramRahuSukshma)]
        public static CalculatorResult JupiterAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnAntaramRahuSukshma)]
        public static CalculatorResult SaturnAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryAntaramRahuSukshma)]
        public static CalculatorResult MercuryAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuAntaramRahuSukshma)]
        public static CalculatorResult KetuAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusAntaramRahuSukshma)]
        public static CalculatorResult VenusAntaramRahuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU SUKSHMA

        #region JUPITER SUKSHMA

        [EventCalculator(EventName.SunAntaramJupiterSukshma)]
        public static CalculatorResult SunAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonAntaramJupiterSukshma)]
        public static CalculatorResult MoonAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsAntaramJupiterSukshma)]
        public static CalculatorResult MarsAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuAntaramJupiterSukshma)]
        public static CalculatorResult RahuAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterAntaramJupiterSukshma)]
        public static CalculatorResult JupiterAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnAntaramJupiterSukshma)]
        public static CalculatorResult SaturnAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryAntaramJupiterSukshma)]
        public static CalculatorResult MercuryAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuAntaramJupiterSukshma)]
        public static CalculatorResult KetuAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusAntaramJupiterSukshma)]
        public static CalculatorResult VenusAntaramJupiterSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER SUKSHMA

        #region SATURN SUKSHMA

        [EventCalculator(EventName.SunAntaramSaturnSukshma)]
        public static CalculatorResult SunAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonAntaramSaturnSukshma)]
        public static CalculatorResult MoonAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsAntaramSaturnSukshma)]
        public static CalculatorResult MarsAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuAntaramSaturnSukshma)]
        public static CalculatorResult RahuAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterAntaramSaturnSukshma)]
        public static CalculatorResult JupiterAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnAntaramSaturnSukshma)]
        public static CalculatorResult SaturnAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercuryAntaramSaturnSukshma)]
        public static CalculatorResult MercuryAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuAntaramSaturnSukshma)]
        public static CalculatorResult KetuAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusAntaramSaturnSukshma)]
        public static CalculatorResult VenusAntaramSaturnSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN SUKSHMA

        #region MERCURY SUKSHMA

        [EventCalculator(EventName.SunAntaramMercurySukshma)]
        public static CalculatorResult SunAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonAntaramMercurySukshma)]
        public static CalculatorResult MoonAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsAntaramMercurySukshma)]
        public static CalculatorResult MarsAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuAntaramMercurySukshma)]
        public static CalculatorResult RahuAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterAntaramMercurySukshma)]
        public static CalculatorResult JupiterAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnAntaramMercurySukshma)]
        public static CalculatorResult SaturnAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryAntaramMercurySukshma)]
        public static CalculatorResult MercuryAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuAntaramMercurySukshma)]
        public static CalculatorResult KetuAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusAntaramMercurySukshma)]
        public static CalculatorResult VenusAntaramMercurySukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY SUKSHMA

        #region KETU SUKSHMA

        [EventCalculator(EventName.SunAntaramKetuSukshma)]
        public static CalculatorResult SunAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonAntaramKetuSukshma)]
        public static CalculatorResult MoonAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsAntaramKetuSukshma)]
        public static CalculatorResult MarsAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuAntaramKetuSukshma)]
        public static CalculatorResult RahuAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterAntaramKetuSukshma)]
        public static CalculatorResult JupiterAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnAntaramKetuSukshma)]
        public static CalculatorResult SaturnAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryAntaramKetuSukshma)]
        public static CalculatorResult MercuryAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuAntaramKetuSukshma)]
        public static CalculatorResult KetuAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusAntaramKetuSukshma)]
        public static CalculatorResult VenusAntaramKetuSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU SUKSHMA

        #region VENUS SUKSHMA

        [EventCalculator(EventName.SunAntaramVenusSukshma)]
        public static CalculatorResult SunAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonAntaramVenusSukshma)]
        public static CalculatorResult MoonAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsAntaramVenusSukshma)]
        public static CalculatorResult MarsAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuAntaramVenusSukshma)]
        public static CalculatorResult RahuAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterAntaramVenusSukshma)]
        public static CalculatorResult JupiterAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnAntaramVenusSukshma)]
        public static CalculatorResult SaturnAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryAntaramVenusSukshma)]
        public static CalculatorResult MercuryAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuAntaramVenusSukshma)]
        public static CalculatorResult KetuAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusAntaramVenusSukshma)]
        public static CalculatorResult VenusAntaramVenusSukshma(Time time, Person person) => PlanetAntaramPlanetSukshma(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS SUKSHMA

        //PRANA

        #region SUN Prana

        [EventCalculator(EventName.SunSukshmaSunPrana)]
        public static CalculatorResult SunSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSukshmaSunPrana)]
        public static CalculatorResult MoonSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSukshmaSunPrana)]
        public static CalculatorResult MarsSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSukshmaSunPrana)]
        public static CalculatorResult RahuSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSukshmaSunPrana)]
        public static CalculatorResult JupiterSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSukshmaSunPrana)]
        public static CalculatorResult SaturnSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySukshmaSunPrana)]
        public static CalculatorResult MercurySukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSukshmaSunPrana)]
        public static CalculatorResult KetuSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSukshmaSunPrana)]
        public static CalculatorResult VenusSukshmaSunPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN Prana

        #region MOON Prana

        [EventCalculator(EventName.SunSukshmaMoonPrana)]
        public static CalculatorResult SunSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonSukshmaMoonPrana)]
        public static CalculatorResult MoonSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsSukshmaMoonPrana)]
        public static CalculatorResult MarsSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuSukshmaMoonPrana)]
        public static CalculatorResult RahuSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterSukshmaMoonPrana)]
        public static CalculatorResult JupiterSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnSukshmaMoonPrana)]
        public static CalculatorResult SaturnSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercurySukshmaMoonPrana)]
        public static CalculatorResult MercurySukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuSukshmaMoonPrana)]
        public static CalculatorResult KetuSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusSukshmaMoonPrana)]
        public static CalculatorResult VenusSukshmaMoonPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON Prana

        #region MARS Prana

        [EventCalculator(EventName.SunSukshmaMarsPrana)]
        public static CalculatorResult SunSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonSukshmaMarsPrana)]
        public static CalculatorResult MoonSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsSukshmaMarsPrana)]
        public static CalculatorResult MarsSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuSukshmaMarsPrana)]
        public static CalculatorResult RahuSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterSukshmaMarsPrana)]
        public static CalculatorResult JupiterSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnSukshmaMarsPrana)]
        public static CalculatorResult SaturnSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercurySukshmaMarsPrana)]
        public static CalculatorResult MercurySukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuSukshmaMarsPrana)]
        public static CalculatorResult KetuSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusSukshmaMarsPrana)]
        public static CalculatorResult VenusSukshmaMarsPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS Prana

        #region RAHU Prana

        [EventCalculator(EventName.SunSukshmaRahuPrana)]
        public static CalculatorResult SunSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonSukshmaRahuPrana)]
        public static CalculatorResult MoonSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsSukshmaRahuPrana)]
        public static CalculatorResult MarsSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuSukshmaRahuPrana)]
        public static CalculatorResult RahuSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterSukshmaRahuPrana)]
        public static CalculatorResult JupiterSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnSukshmaRahuPrana)]
        public static CalculatorResult SaturnSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercurySukshmaRahuPrana)]
        public static CalculatorResult MercurySukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuSukshmaRahuPrana)]
        public static CalculatorResult KetuSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusSukshmaRahuPrana)]
        public static CalculatorResult VenusSukshmaRahuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU Prana

        #region JUPITER Prana

        [EventCalculator(EventName.SunSukshmaJupiterPrana)]
        public static CalculatorResult SunSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonSukshmaJupiterPrana)]
        public static CalculatorResult MoonSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsSukshmaJupiterPrana)]
        public static CalculatorResult MarsSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuSukshmaJupiterPrana)]
        public static CalculatorResult RahuSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterSukshmaJupiterPrana)]
        public static CalculatorResult JupiterSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnSukshmaJupiterPrana)]
        public static CalculatorResult SaturnSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercurySukshmaJupiterPrana)]
        public static CalculatorResult MercurySukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuSukshmaJupiterPrana)]
        public static CalculatorResult KetuSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusSukshmaJupiterPrana)]
        public static CalculatorResult VenusSukshmaJupiterPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER Prana

        #region SATURN Prana

        [EventCalculator(EventName.SunSukshmaSaturnPrana)]
        public static CalculatorResult SunSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSukshmaSaturnPrana)]
        public static CalculatorResult MoonSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSukshmaSaturnPrana)]
        public static CalculatorResult MarsSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSukshmaSaturnPrana)]
        public static CalculatorResult RahuSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSukshmaSaturnPrana)]
        public static CalculatorResult JupiterSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSukshmaSaturnPrana)]
        public static CalculatorResult SaturnSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySukshmaSaturnPrana)]
        public static CalculatorResult MercurySukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSukshmaSaturnPrana)]
        public static CalculatorResult KetuSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSukshmaSaturnPrana)]
        public static CalculatorResult VenusSukshmaSaturnPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN Prana

        #region MERCURY Prana

        [EventCalculator(EventName.SunSukshmaMercuryPrana)]
        public static CalculatorResult SunSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonSukshmaMercuryPrana)]
        public static CalculatorResult MoonSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsSukshmaMercuryPrana)]
        public static CalculatorResult MarsSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuSukshmaMercuryPrana)]
        public static CalculatorResult RahuSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterSukshmaMercuryPrana)]
        public static CalculatorResult JupiterSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnSukshmaMercuryPrana)]
        public static CalculatorResult SaturnSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercurySukshmaMercuryPrana)]
        public static CalculatorResult MercurySukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuSukshmaMercuryPrana)]
        public static CalculatorResult KetuSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusSukshmaMercuryPrana)]
        public static CalculatorResult VenusSukshmaMercuryPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY Prana

        #region KETU Prana

        [EventCalculator(EventName.SunSukshmaKetuPrana)]
        public static CalculatorResult SunSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonSukshmaKetuPrana)]
        public static CalculatorResult MoonSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsSukshmaKetuPrana)]
        public static CalculatorResult MarsSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuSukshmaKetuPrana)]
        public static CalculatorResult RahuSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterSukshmaKetuPrana)]
        public static CalculatorResult JupiterSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnSukshmaKetuPrana)]
        public static CalculatorResult SaturnSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercurySukshmaKetuPrana)]
        public static CalculatorResult MercurySukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuSukshmaKetuPrana)]
        public static CalculatorResult KetuSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusSukshmaKetuPrana)]
        public static CalculatorResult VenusSukshmaKetuPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU Prana

        #region VENUS Prana

        [EventCalculator(EventName.SunSukshmaVenusPrana)]
        public static CalculatorResult SunSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonSukshmaVenusPrana)]
        public static CalculatorResult MoonSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsSukshmaVenusPrana)]
        public static CalculatorResult MarsSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuSukshmaVenusPrana)]
        public static CalculatorResult RahuSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterSukshmaVenusPrana)]
        public static CalculatorResult JupiterSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnSukshmaVenusPrana)]
        public static CalculatorResult SaturnSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercurySukshmaVenusPrana)]
        public static CalculatorResult MercurySukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuSukshmaVenusPrana)]
        public static CalculatorResult KetuSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusSukshmaVenusPrana)]
        public static CalculatorResult VenusSukshmaVenusPrana(Time time, Person person) => PlanetSukshmaPlanetPrana(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS Prana

        #region DASA SPECIAL RULES

        [EventCalculator(EventName.Lord6And8Dasa)]
        public static CalculatorResult Lord6And8Dasa(Time time, Person person)
        {
            //The Dasa period of the lords of the 6th and the 8th
            // produce harmful results unless they acquire beneficence
            // otherwise.

            //get lord 6th house
            var lord6th = AstronomicalCalculator.GetLordOfHouse(HouseName.House6, person.BirthTime);

            //is lord 6th dasa occuring
            var isLord6thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord6th;

            //get lord 8th house
            var lord8th = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord8thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord8th;

            //occuring if one of the conditions met
            var occuring = isLord6thDasa || isLord8thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord5And9Dasa)]
        public static CalculatorResult Lord5And9Dasa(Time time, Person person)
        {
            //The periods of lords of the 5th and the 9th are said
            // to be good, so much so that the periods of planets, which are
            // joined or otherwise related with them, are also supposed to
            // give rise to good.

            //get lord 5th house
            var lord5th = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord5th;

            //get lord 9th house
            var lord9th = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord9th;

            //occuring if one of the conditions met
            var occuring = isLord5thDasa || isLord9thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord5And9DasaBhukti)]
        public static CalculatorResult Lord5And9DasaBhukti(Time time, Person person)
        {
            //The sub-period of the lord of the 5th in the major
            //period of the lord of the 9th or vice versa is supposed to
            //produce good effects.

            //get lord 5th house
            var lord5th = AstronomicalCalculator.GetLordOfHouse(HouseName.House5, person.BirthTime);
            //get lord 9th house
            var lord9th = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 5th dasa occuring
            var isLord5thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord5th;
            var isLord5thBhukti = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti == lord5th;

            //is lord 9th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lord9th;
            var isLord9thBhukti = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti == lord9th;

            //condition 1
            //bhukti 5th lord & dasa 9th lord
            var condition1 = isLord5thBhukti && isLord9thDasa;

            //condition 2
            //dasa 5th lord & bhukti 9th lord
            var condition2 = isLord5thDasa && isLord9thBhukti;

            //occuring if one of the conditions met
            var occuring = condition1 || condition2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.BhuktiDasaLordInBadHouses)]
        public static CalculatorResult BhuktiDasaLordInBadHouses(Time time, Person person)
        {
            //Unfavourable results will be realised when the sublord (bhukti)
            // and the major lord (dasa) are situated in the 6th and the 8th or
            // the 12th and the 2nd from each other respectively.

            //get bukti lord
            var buhktiLord = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Bhukti;

            //get dasa lord =
            var dasaLord = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa;

            //condition 1
            //is bukti lord in 6th house at birth
            var bhuktiLordIn6th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, buhktiLord, 6);
            //is dasa lord in 8th house at birth
            var dasaLordIn8th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, dasaLord, 8);
            //check if both planets are in bad houses at the same time
            var buhktiDasaIn6And8 = bhuktiLordIn6th && dasaLordIn8th;

            //condition 2
            //is bukti lord in 12th house at birth
            var bhuktiLordIn12th = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, buhktiLord, 12);
            //is dasa lord in 2nd house at birth
            var dasaLordIn2nd = AstronomicalCalculator.IsPlanetInHouse(person.BirthTime, dasaLord, 2);
            //check if both planets are in bad houses at the same time
            var buhktiDasaIn12And2 = bhuktiLordIn12th && dasaLordIn2nd;

            //occuring if one of the conditions are met
            var occuring = buhktiDasaIn6And8 || buhktiDasaIn12And2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord2Dasa)]
        public static CalculatorResult Lord2Dasa(Time time, Person person)
        {
            //Lord of the 2nd in his Dasa gives wealth

            //get lord 2nd house
            var lordHouse2 = AstronomicalCalculator.GetLordOfHouse(HouseName.House2, person.BirthTime);

            //is lord 2nd dasa occuring
            var isLord2Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse2;

            //occuring if one of the conditions met
            var occuring = isLord2Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Lord3Dasa)]
        public static CalculatorResult Lord3Dasa(Time time, Person person)
        {
            //Lord of the 3rd during his Dasa gives new friends,
            // help to brothers, leadership, and physical pain (if afflicted).

            //get lord 3rd house
            var lordHouse3 = AstronomicalCalculator.GetLordOfHouse(HouseName.House3, person.BirthTime);

            //is lord 3rd dasa occuring
            var isLord3Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse3;

            //occuring if one of the conditions met
            var occuring = isLord3Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LagnaLordDasa)]
        public static CalculatorResult LagnaLordDasa(Time time, Person person)
        {
            //todo powerful not accounted for, future added it in

            //When Lagna (Ascendant) is powerful, during the Dasa
            // of lord of Lagna, favourable results can be expected to occur
            // - such as rise in profession, good health and general prosperity.

            //get lord of 1st house
            var lordHouse1 = AstronomicalCalculator.GetLordOfHouse(HouseName.House1, person.BirthTime);

            //is lord 1st house dasa occuring
            var isLord1Dasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == lordHouse1;

            //occuring if one of the conditions met
            var occuring = isLord1Dasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Saturn4thDasa)]
        public static CalculatorResult Saturn4thDasa(Time time, Person person)
        {
            //The Dasa period of Saturn, if it happens to be the 4th
            // Dasa, will be unfavourable. If Saturn is strong and favourably disposed,
            // the evil effects get considerably modified.

            //is saturn dasa occuring
            var isSaturnDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Saturn;

            //is the 4th dasa
            var is4thDasa = AstronomicalCalculator.GetCurrentDasaCountFromBirth(person.BirthTime, time) == 4;

            //occuring if one of the conditions met
            var occuring = isSaturnDasa && is4thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.Jupiter6thDasa)]
        public static CalculatorResult Jupiter6thDasa(Time time, Person person)
        {
            //The Dasa period of Jupiter will be unfavourable if it
            // happens to be the 6th Dasa.

            //is jupiter dasa occuring
            var isJupiterDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Jupiter;

            //is the 6th dasa
            var is6thDasa = AstronomicalCalculator.GetCurrentDasaCountFromBirth(person.BirthTime, time) == 6;

            //occuring if one of the conditions met
            var occuring = isJupiterDasa && is6thDasa;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ElevatedSunDasa)]
        public static CalculatorResult ElevatedSunDasa(Time time, Person person)
        {
            //If the Sun is elevated, he displays wisdom, gets
            //money, attains fame and happiness

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //is sun elevated
            //todo what is elvated?
            var isSunElevated = false;

            //occuring if one of the conditions met
            var occuring = isSunDasa && isSunElevated;

            return CalculatorResult.New(occuring, PlanetName.Sun);
        }

        [EventCalculator(EventName.SunWithLord9Or10Dasa)]
        public static CalculatorResult SunWithLord9Or10Dasa(Time time, Person person)
        {
            //The Sun in good position, in own house or joined with lord of 9 or
            //10 - happiness, gains, riches, honours

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //is sun in own house
            var sunInOwn = AstronomicalCalculator.IsPlanetInOwnHouse(person.BirthTime, PlanetName.Sun);

            //is sun joined (same house) with lord 9 or 10
            var a = AstronomicalCalculator.IsPlanetSameHouseWithHouseLord(person.BirthTime, 9, PlanetName.Sun);
            var b = AstronomicalCalculator.IsPlanetSameHouseWithHouseLord(person.BirthTime, 10, PlanetName.Sun);
            var sunJoined9Or10 = a || b;

            var sunInGoodPosition = sunInOwn || sunJoined9Or10;

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunInGoodPosition;

            return CalculatorResult.New(occuring, new[] { HouseName.House9, HouseName.House10 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunWithLord5Dasa)]
        public static CalculatorResult SunWithLord5Dasa(Time time, Person person)
        {
            //the Sun with lord of 5 - birth of children.

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //is sun with lord of 5th
            var sunWithLord5th = AstronomicalCalculator.IsPlanetSameHouseWithHouseLord(person.BirthTime, 5, PlanetName.Sun);

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunWithLord5th;

            return CalculatorResult.New(occuring, new[] { HouseName.House5 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunWithLord2Dasa)]
        public static CalculatorResult SunWithLord2Dasa(Time time, Person person)
        {
            //The Sun when related to lord of 2 - becomes rich, earns money, secures
            //property, gains, favours from influential persons.

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            //is sun with lord of 2nd
            var sunWithLord2nd = AstronomicalCalculator.IsPlanetSameHouseWithHouseLord(person.BirthTime, 2, PlanetName.Sun);

            //occuring if one of the conditions met
            var occuring = isSunDasa && sunWithLord2nd;

            return CalculatorResult.New(occuring, new[] { HouseName.House2 }, new[] { PlanetName.Sun }, time);
        }

        [EventCalculator(EventName.SunBadPositionDasa)]
        public static CalculatorResult SunBadPositionDasa(Time time, Person person)
        {
            //The Sun when debilitated or occupies the 6th or the
            //8th house or in cojunction with evil planets contracts evil diseases, loss of wealth, suffers from reverses
            //in employment, penalty and becomes ill.

            //TODO
            return new() { Occuring = false };
        }

        [EventCalculator(EventName.ExaltatedSunDasa)]
        public static CalculatorResult ExaltatedSunDasa(Time time, Person person)
        {
            //The Dasa of the Sun in deep exaltation : Sudden
            //gains in cattle and wealth, much travelling in eastern
            //countries, residence in foreign countries, quarrels
            //among friends and relations, pleasure trios and picnic
            //parties and lovely women.

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time).Dasa == PlanetName.Sun;

            var isSunExalted = AstronomicalCalculator.IsPlanetExaltated(PlanetName.Sun, time);

            //conditions met
            var occuring = isSunDasa && isSunExalted;

            return CalculatorResult.New(occuring, PlanetName.Sun);
        }

        #endregion DASA SPECIAL RULES

        //SPECIAL SHORTCUT FUNCTIONS

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetBhuktiPlanetAntaram(Time time, Person person, PlanetName bhuktiPlanet, PlanetName antaramPlanet)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check bhukti
            var isCorrectBhukti = currentDasaBhuktiAntaram.Bhukti == bhuktiPlanet;

            //check antaram
            var isCorrectAntaram = currentDasaBhuktiAntaram.Antaram == antaramPlanet;

            //occuring if all conditions met
            var occuring = isCorrectAntaram && isCorrectBhukti;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(bhuktiPlanet, antaramPlanet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetAntaramPlanetSukshma(Time time, Person person, PlanetName antaramPlanet, PlanetName sukshmaPlanet)
        {
            //get dasas for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check Antaram
            var isCorrectAntaram = currentDasaBhuktiAntaram.Antaram == antaramPlanet;

            //check Sukshma
            var isCorrectSukshma = currentDasaBhuktiAntaram.Sukshma == sukshmaPlanet;

            //occuring if all conditions met
            var occuring = isCorrectAntaram && isCorrectSukshma;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(antaramPlanet, sukshmaPlanet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetSukshmaPlanetPrana(Time time, Person person, PlanetName sukshmaPlanet, PlanetName pranaPlanet)
        {
            //get whole dasa for current time
            var currentDasaBhuktiAntaram = AstronomicalCalculator.GetCurrentDasaBhuktiAntaram(person.BirthTime, time);

            //check Sukshma
            var isCorrectSukshma = currentDasaBhuktiAntaram.Sukshma == sukshmaPlanet;

            //check prana
            var isCorrectPrana = currentDasaBhuktiAntaram.Prana == pranaPlanet;

            //occuring if all conditions met
            var occuring = isCorrectSukshma && isCorrectPrana;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(sukshmaPlanet, pranaPlanet);

                var result = new CalculatorResult() { Occuring = occuring, NatureOverride = periodPrediction.eventNature, DescriptionOverride = periodPrediction.desciption };

                return result;
            }
            else
            {
                return CalculatorResult.NotOccuring();
            }
        }
    }
}