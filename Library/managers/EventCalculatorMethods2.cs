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

        [EventCalculator(EventName.AriesSunPD1)]
        public static CalculatorResult AriesSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSunPD1)]
        public static CalculatorResult TaurusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSunPD1)]
        public static CalculatorResult GeminiSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSunPD1)]
        public static CalculatorResult CancerSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSunPD1)]
        public static CalculatorResult LeoSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSunPD1)]
        public static CalculatorResult VirgoSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSunPD1)]
        public static CalculatorResult LibraSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSunPD1)]
        public static CalculatorResult ScorpioSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSunPD1)]
        public static CalculatorResult SagittariusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSunPD1)]
        public static CalculatorResult CapricornusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSunPD1)]
        public static CalculatorResult AquariusSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSunPD1)]
        public static CalculatorResult PiscesSunPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Sun, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion SUN DASA

        #region MOON DASA

        [EventCalculator(EventName.AriesMoonPD1)]
        public static CalculatorResult AriesMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMoonPD1)]
        public static CalculatorResult TaurusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMoonPD1)]
        public static CalculatorResult GeminiMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMoonPD1)]
        public static CalculatorResult CancerMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMoonPD1)]
        public static CalculatorResult LeoMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMoonPD1)]
        public static CalculatorResult VirgoMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMoonPD1)]
        public static CalculatorResult LibraMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMoonPD1)]
        public static CalculatorResult ScorpioMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMoonPD1)]
        public static CalculatorResult SagittariusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMoonPD1)]
        public static CalculatorResult CapricornusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMoonPD1)]
        public static CalculatorResult AquariusMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMoonPD1)]
        public static CalculatorResult PiscesMoonPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Moon, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MOON DASA

        #region MARS DASA

        [EventCalculator(EventName.AriesMarsPD1)]
        public static CalculatorResult AriesMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMarsPD1)]
        public static CalculatorResult TaurusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMarsPD1)]
        public static CalculatorResult GeminiMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMarsPD1)]
        public static CalculatorResult CancerMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMarsPD1)]
        public static CalculatorResult LeoMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMarsPD1)]
        public static CalculatorResult VirgoMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMarsPD1)]
        public static CalculatorResult LibraMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMarsPD1)]
        public static CalculatorResult ScorpioMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMarsPD1)]
        public static CalculatorResult SagittariusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMarsPD1)]
        public static CalculatorResult CapricornusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMarsPD1)]
        public static CalculatorResult AquariusMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMarsPD1)]
        public static CalculatorResult PiscesMarsPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mars, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MARS DASA

        #region RAHU DASA

        [EventCalculator(EventName.AriesRahuPD1)]
        public static CalculatorResult AriesRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusRahuPD1)]
        public static CalculatorResult TaurusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiRahuPD1)]
        public static CalculatorResult GeminiRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerRahuPD1)]
        public static CalculatorResult CancerRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoRahuPD1)]
        public static CalculatorResult LeoRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoRahuPD1)]
        public static CalculatorResult VirgoRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraRahuPD1)]
        public static CalculatorResult LibraRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioRahuPD1)]
        public static CalculatorResult ScorpioRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusRahuPD1)]
        public static CalculatorResult SagittariusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusRahuPD1)]
        public static CalculatorResult CapricornusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusRahuPD1)]
        public static CalculatorResult AquariusRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesRahuPD1)]
        public static CalculatorResult PiscesRahuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Rahu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion RAHU DASA

        #region JUPITER DASA

        [EventCalculator(EventName.AriesJupiterPD1)]
        public static CalculatorResult AriesJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusJupiterPD1)]
        public static CalculatorResult TaurusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiJupiterPD1)]
        public static CalculatorResult GeminiJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerJupiterPD1)]
        public static CalculatorResult CancerJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoJupiterPD1)]
        public static CalculatorResult LeoJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoJupiterPD1)]
        public static CalculatorResult VirgoJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraJupiterPD1)]
        public static CalculatorResult LibraJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioJupiterPD1)]
        public static CalculatorResult ScorpioJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusJupiterPD1)]
        public static CalculatorResult SagittariusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusJupiterPD1)]
        public static CalculatorResult CapricornusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusJupiterPD1)]
        public static CalculatorResult AquariusJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesJupiterPD1)]
        public static CalculatorResult PiscesJupiterPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Jupiter, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER DASA

        #region SATURN DASA

        [EventCalculator(EventName.AriesSaturnPD1)]
        public static CalculatorResult AriesSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusSaturnPD1)]
        public static CalculatorResult TaurusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiSaturnPD1)]
        public static CalculatorResult GeminiSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerSaturnPD1)]
        public static CalculatorResult CancerSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoSaturnPD1)]
        public static CalculatorResult LeoSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoSaturnPD1)]
        public static CalculatorResult VirgoSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraSaturnPD1)]
        public static CalculatorResult LibraSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioSaturnPD1)]
        public static CalculatorResult ScorpioSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusSaturnPD1)]
        public static CalculatorResult SagittariusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusSaturnPD1)]
        public static CalculatorResult CapricornusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusSaturnPD1)]
        public static CalculatorResult AquariusSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesSaturnPD1)]
        public static CalculatorResult PiscesSaturnPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Saturn, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion SATURN DASA

        #region MERCURY DASA

        [EventCalculator(EventName.AriesMercuryPD1)]
        public static CalculatorResult AriesMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusMercuryPD1)]
        public static CalculatorResult TaurusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiMercuryPD1)]
        public static CalculatorResult GeminiMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerMercuryPD1)]
        public static CalculatorResult CancerMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoMercuryPD1)]
        public static CalculatorResult LeoMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoMercuryPD1)]
        public static CalculatorResult VirgoMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraMercuryPD1)]
        public static CalculatorResult LibraMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioMercuryPD1)]
        public static CalculatorResult ScorpioMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusMercuryPD1)]
        public static CalculatorResult SagittariusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusMercuryPD1)]
        public static CalculatorResult CapricornusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusMercuryPD1)]
        public static CalculatorResult AquariusMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesMercuryPD1)]
        public static CalculatorResult PiscesMercuryPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Mercury, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY DASA

        #region KETU DASA

        [EventCalculator(EventName.AriesKetuPD1)]
        public static CalculatorResult AriesKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusKetuPD1)]
        public static CalculatorResult TaurusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiKetuPD1)]
        public static CalculatorResult GeminiKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerKetuPD1)]
        public static CalculatorResult CancerKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoKetuPD1)]
        public static CalculatorResult LeoKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoKetuPD1)]
        public static CalculatorResult VirgoKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraKetuPD1)]
        public static CalculatorResult LibraKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioKetuPD1)]
        public static CalculatorResult ScorpioKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusKetuPD1)]
        public static CalculatorResult SagittariusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusKetuPD1)]
        public static CalculatorResult CapricornusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusKetuPD1)]
        public static CalculatorResult AquariusKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesKetuPD1)]
        public static CalculatorResult PiscesKetuPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Ketu, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion KETU DASA

        #region VENUS DASA

        [EventCalculator(EventName.AriesVenusPD1)]
        public static CalculatorResult AriesVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aries;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.TaurusVenusPD1)]
        public static CalculatorResult TaurusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Taurus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.GeminiVenusPD1)]
        public static CalculatorResult GeminiVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Gemini;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CancerVenusPD1)]
        public static CalculatorResult CancerVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Cancer;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LeoVenusPD1)]
        public static CalculatorResult LeoVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Leo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VirgoVenusPD1)]
        public static CalculatorResult VirgoVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Virgo;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.LibraVenusPD1)]
        public static CalculatorResult LibraVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Libra;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.ScorpioVenusPD1)]
        public static CalculatorResult ScorpioVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Scorpio;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SagittariusVenusPD1)]
        public static CalculatorResult SagittariusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Sagittarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.CapricornusVenusPD1)]
        public static CalculatorResult CapricornusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Capricornus;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.AquariusVenusPD1)]
        public static CalculatorResult AquariusVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Aquarius;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.PiscesVenusPD1)]
        public static CalculatorResult PiscesVenusPD1(Time time, Person person)
        {
            //planet in sign at birth
            var planetInSign = AstronomicalCalculator.GetPlanetRasiSign(PlanetName.Venus, person.BirthTime).GetSignName() == ZodiacName.Pisces;

            //current dasa is of planet
            var planetPD1Ocurring = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = planetInSign && planetPD1Ocurring;

            return new() { Occuring = occuring };
        }

        #endregion VENUS DASA

        #endregion DASAS

        //PD2

        #region SUN PD2

        [EventCalculator(EventName.SunSunPD2)]
        public static CalculatorResult SunSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMoonPD2)]
        public static CalculatorResult SunMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMarsPD2)]
        public static CalculatorResult SunMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunRahuPD2)]
        public static CalculatorResult SunRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunJupiterPD2)]
        public static CalculatorResult SunJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunSaturnPD2)]
        public static CalculatorResult SunSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunMercuryPD2)]
        public static CalculatorResult SunMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunKetuPD2)]
        public static CalculatorResult SunKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SunVenusPD2)]
        public static CalculatorResult SunVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Sun;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion SUN PD2

        #region MOON PD2

        [EventCalculator(EventName.MoonSunPD2)]
        public static CalculatorResult MoonSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMoonPD2)]
        public static CalculatorResult MoonMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMarsPD2)]
        public static CalculatorResult MoonMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonRahuPD2)]
        public static CalculatorResult MoonRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonJupiterPD2)]
        public static CalculatorResult MoonJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonSaturnPD2)]
        public static CalculatorResult MoonSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonMercuryPD2)]
        public static CalculatorResult MoonMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonKetuPD2)]
        public static CalculatorResult MoonKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MoonVenusPD2)]
        public static CalculatorResult MoonVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Moon;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MOON PD2

        #region MARS PD2

        [EventCalculator(EventName.MarsSunPD2)]
        public static CalculatorResult MarsSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMoonPD2)]
        public static CalculatorResult MarsMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMarsPD2)]
        public static CalculatorResult MarsMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsRahuPD2)]
        public static CalculatorResult MarsRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsJupiterPD2)]
        public static CalculatorResult MarsJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsSaturnPD2)]
        public static CalculatorResult MarsSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsMercuryPD2)]
        public static CalculatorResult MarsMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsKetuPD2)]
        public static CalculatorResult MarsKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MarsVenusPD2)]
        public static CalculatorResult MarsVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mars;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MARS PD2

        #region RAHU PD2

        [EventCalculator(EventName.RahuSunPD2)]
        public static CalculatorResult RahuSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMoonPD2)]
        public static CalculatorResult RahuMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMarsPD2)]
        public static CalculatorResult RahuMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuRahuPD2)]
        public static CalculatorResult RahuRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuJupiterPD2)]
        public static CalculatorResult RahuJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuSaturnPD2)]
        public static CalculatorResult RahuSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuMercuryPD2)]
        public static CalculatorResult RahuMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuKetuPD2)]
        public static CalculatorResult RahuKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.RahuVenusPD2)]
        public static CalculatorResult RahuVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Rahu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion RAHU PD2

        #region JUPITER PD2

        [EventCalculator(EventName.JupiterSunPD2)]
        public static CalculatorResult JupiterSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMoonPD2)]
        public static CalculatorResult JupiterMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMarsPD2)]
        public static CalculatorResult JupiterMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterRahuPD2)]
        public static CalculatorResult JupiterRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterJupiterPD2)]
        public static CalculatorResult JupiterJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterSaturnPD2)]
        public static CalculatorResult JupiterSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterMercuryPD2)]
        public static CalculatorResult JupiterMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterKetuPD2)]
        public static CalculatorResult JupiterKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.JupiterVenusPD2)]
        public static CalculatorResult JupiterVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Jupiter;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion JUPITER PD2

        #region SATURN PD2

        [EventCalculator(EventName.SaturnSunPD2)]
        public static CalculatorResult SaturnSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMoonPD2)]
        public static CalculatorResult SaturnMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMarsPD2)]
        public static CalculatorResult SaturnMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnRahuPD2)]
        public static CalculatorResult SaturnRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnJupiterPD2)]
        public static CalculatorResult SaturnJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnSaturnPD2)]
        public static CalculatorResult SaturnSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnMercuryPD2)]
        public static CalculatorResult SaturnMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnKetuPD2)]
        public static CalculatorResult SaturnKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.SaturnVenusPD2)]
        public static CalculatorResult SaturnVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Saturn;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion SATURN PD2

        #region MERCURY PD2

        [EventCalculator(EventName.MercurySunPD2)]
        public static CalculatorResult MercurySunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMoonPD2)]
        public static CalculatorResult MercuryMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMarsPD2)]
        public static CalculatorResult MercuryMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryRahuPD2)]
        public static CalculatorResult MercuryRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryJupiterPD2)]
        public static CalculatorResult MercuryJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercurySaturnPD2)]
        public static CalculatorResult MercurySaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryMercuryPD2)]
        public static CalculatorResult MercuryMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryKetuPD2)]
        public static CalculatorResult MercuryKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.MercuryVenusPD2)]
        public static CalculatorResult MercuryVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Mercury;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion MERCURY PD2

        #region KETU PD2

        [EventCalculator(EventName.KetuSunPD2)]
        public static CalculatorResult KetuSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMoonPD2)]
        public static CalculatorResult KetuMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMarsPD2)]
        public static CalculatorResult KetuMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuRahuPD2)]
        public static CalculatorResult KetuRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuJupiterPD2)]
        public static CalculatorResult KetuJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuSaturnPD2)]
        public static CalculatorResult KetuSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuMercuryPD2)]
        public static CalculatorResult KetuMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuKetuPD2)]
        public static CalculatorResult KetuKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.KetuVenusPD2)]
        public static CalculatorResult KetuVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Ketu;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion KETU PD2

        #region VENUS PD2

        [EventCalculator(EventName.VenusSunPD2)]
        public static CalculatorResult VenusSunPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var dasa = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var bhukti = currentPlanetDasas.PD2 == PlanetName.Sun;

            //occuring if all conditions met
            var occuring = dasa && bhukti;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMoonPD2)]
        public static CalculatorResult VenusMoonPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Moon;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMarsPD2)]
        public static CalculatorResult VenusMarsPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mars;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusRahuPD2)]
        public static CalculatorResult VenusRahuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Rahu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusJupiterPD2)]
        public static CalculatorResult VenusJupiterPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Jupiter;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusSaturnPD2)]
        public static CalculatorResult VenusSaturnPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Saturn;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusMercuryPD2)]
        public static CalculatorResult VenusMercuryPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Mercury;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusKetuPD2)]
        public static CalculatorResult VenusKetuPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Ketu;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        [EventCalculator(EventName.VenusVenusPD2)]
        public static CalculatorResult VenusVenusPD2(Time time, Person person)
        {
            //get dasas for current time
            var currentPlanetDasas = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check dasa
            var isCorrect = currentPlanetDasas.PD1 == PlanetName.Venus;

            //check bhukti
            var isCorrectPD2 = currentPlanetDasas.PD2 == PlanetName.Venus;

            //occuring if all conditions met
            var occuring = isCorrect && isCorrectPD2;

            return new() { Occuring = occuring };
        }

        #endregion VENUS PD2

        //ANTARAM

        #region SUN PD3

        [EventCalculator(EventName.SunSunPD3)]
        public static CalculatorResult SunSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD3)]
        public static CalculatorResult MoonSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD3)]
        public static CalculatorResult MarsSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD3)]
        public static CalculatorResult RahuSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD3)]
        public static CalculatorResult JupiterSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD3)]
        public static CalculatorResult SaturnSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD3)]
        public static CalculatorResult MercurySunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD3)]
        public static CalculatorResult KetuSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD3)]
        public static CalculatorResult VenusSunPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD3

        #region MOON PD3

        [EventCalculator(EventName.SunMoonPD3)]
        public static CalculatorResult SunMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD3)]
        public static CalculatorResult MoonMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD3)]
        public static CalculatorResult MarsMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD3)]
        public static CalculatorResult RahuMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD3)]
        public static CalculatorResult JupiterMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD3)]
        public static CalculatorResult SaturnMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD3)]
        public static CalculatorResult MercuryMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD3)]
        public static CalculatorResult KetuMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD3)]
        public static CalculatorResult VenusMoonPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD3

        #region MARS PD3

        [EventCalculator(EventName.SunMarsPD3)]
        public static CalculatorResult SunMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD3)]
        public static CalculatorResult MoonMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD3)]
        public static CalculatorResult MarsMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD3)]
        public static CalculatorResult RahuMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD3)]
        public static CalculatorResult JupiterMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD3)]
        public static CalculatorResult SaturnMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD3)]
        public static CalculatorResult MercuryMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD3)]
        public static CalculatorResult KetuMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD3)]
        public static CalculatorResult VenusMarsPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD3

        #region RAHU PD3

        [EventCalculator(EventName.SunRahuPD3)]
        public static CalculatorResult SunRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD3)]
        public static CalculatorResult MoonRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD3)]
        public static CalculatorResult MarsRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD3)]
        public static CalculatorResult RahuRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD3)]
        public static CalculatorResult JupiterRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD3)]
        public static CalculatorResult SaturnRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD3)]
        public static CalculatorResult MercuryRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD3)]
        public static CalculatorResult KetuRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD3)]
        public static CalculatorResult VenusRahuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD3

        #region JUPITER PD3

        [EventCalculator(EventName.SunJupiterPD3)]
        public static CalculatorResult SunJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD3)]
        public static CalculatorResult MoonJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD3)]
        public static CalculatorResult MarsJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD3)]
        public static CalculatorResult RahuJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD3)]
        public static CalculatorResult JupiterJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD3)]
        public static CalculatorResult SaturnJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD3)]
        public static CalculatorResult MercuryJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD3)]
        public static CalculatorResult KetuJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD3)]
        public static CalculatorResult VenusJupiterPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD3

        #region SATURN PD3

        [EventCalculator(EventName.SunSaturnPD3)]
        public static CalculatorResult SunSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD3)]
        public static CalculatorResult MoonSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD3)]
        public static CalculatorResult MarsSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD3)]
        public static CalculatorResult RahuSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD3)]
        public static CalculatorResult JupiterSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD3)]
        public static CalculatorResult SaturnSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD3)]
        public static CalculatorResult MercurySaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD3)]
        public static CalculatorResult KetuSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD3)]
        public static CalculatorResult VenusSaturnPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD3

        #region MERCURY PD3

        [EventCalculator(EventName.SunMercuryPD3)]
        public static CalculatorResult SunMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD3)]
        public static CalculatorResult MoonMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD3)]
        public static CalculatorResult MarsMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD3)]
        public static CalculatorResult RahuMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD3)]
        public static CalculatorResult JupiterMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD3)]
        public static CalculatorResult SaturnMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD3)]
        public static CalculatorResult MercuryMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD3)]
        public static CalculatorResult KetuMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD3)]
        public static CalculatorResult VenusMercuryPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD3

        #region KETU PD3

        [EventCalculator(EventName.SunKetuPD3)]
        public static CalculatorResult SunKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD3)]
        public static CalculatorResult MoonKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD3)]
        public static CalculatorResult MarsKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD3)]
        public static CalculatorResult RahuKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD3)]
        public static CalculatorResult JupiterKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD3)]
        public static CalculatorResult SaturnKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD3)]
        public static CalculatorResult MercuryKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD3)]
        public static CalculatorResult KetuKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD3)]
        public static CalculatorResult VenusKetuPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD3

        #region VENUS PD3

        [EventCalculator(EventName.SunVenusPD3)]
        public static CalculatorResult SunVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD3)]
        public static CalculatorResult MoonVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD3)]
        public static CalculatorResult MarsVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD3)]
        public static CalculatorResult RahuVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD3)]
        public static CalculatorResult JupiterVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD3)]
        public static CalculatorResult SaturnVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD3)]
        public static CalculatorResult MercuryVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD3)]
        public static CalculatorResult KetuVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD3)]
        public static CalculatorResult VenusVenusPD3(Time time, Person person) => PlanetPD2PlanetPD3(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD3

        //SUKSHMA

        #region SUN PD4

        [EventCalculator(EventName.SunSunPD4)]
        public static CalculatorResult SunSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD4)]
        public static CalculatorResult MoonSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD4)]
        public static CalculatorResult MarsSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD4)]
        public static CalculatorResult RahuSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD4)]
        public static CalculatorResult JupiterSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD4)]
        public static CalculatorResult SaturnSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD4)]
        public static CalculatorResult MercurySunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD4)]
        public static CalculatorResult KetuSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD4)]
        public static CalculatorResult VenusSunPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD4

        #region MOON PD4

        [EventCalculator(EventName.SunMoonPD4)]
        public static CalculatorResult SunMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD4)]
        public static CalculatorResult MoonMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD4)]
        public static CalculatorResult MarsMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD4)]
        public static CalculatorResult RahuMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD4)]
        public static CalculatorResult JupiterMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD4)]
        public static CalculatorResult SaturnMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD4)]
        public static CalculatorResult MercuryMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD4)]
        public static CalculatorResult KetuMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD4)]
        public static CalculatorResult VenusMoonPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD4

        #region MARS PD4

        [EventCalculator(EventName.SunMarsPD4)]
        public static CalculatorResult SunMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD4)]
        public static CalculatorResult MoonMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD4)]
        public static CalculatorResult MarsMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD4)]
        public static CalculatorResult RahuMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD4)]
        public static CalculatorResult JupiterMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD4)]
        public static CalculatorResult SaturnMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD4)]
        public static CalculatorResult MercuryMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD4)]
        public static CalculatorResult KetuMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD4)]
        public static CalculatorResult VenusMarsPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD4

        #region RAHU PD4

        [EventCalculator(EventName.SunRahuPD4)]
        public static CalculatorResult SunRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD4)]
        public static CalculatorResult MoonRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD4)]
        public static CalculatorResult MarsRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD4)]
        public static CalculatorResult RahuRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD4)]
        public static CalculatorResult JupiterRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD4)]
        public static CalculatorResult SaturnRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD4)]
        public static CalculatorResult MercuryRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD4)]
        public static CalculatorResult KetuRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD4)]
        public static CalculatorResult VenusRahuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD4

        #region JUPITER PD4

        [EventCalculator(EventName.SunJupiterPD4)]
        public static CalculatorResult SunJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD4)]
        public static CalculatorResult MoonJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD4)]
        public static CalculatorResult MarsJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD4)]
        public static CalculatorResult RahuJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD4)]
        public static CalculatorResult JupiterJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD4)]
        public static CalculatorResult SaturnJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD4)]
        public static CalculatorResult MercuryJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD4)]
        public static CalculatorResult KetuJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD4)]
        public static CalculatorResult VenusJupiterPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD4

        #region SATURN PD4

        [EventCalculator(EventName.SunSaturnPD4)]
        public static CalculatorResult SunSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD4)]
        public static CalculatorResult MoonSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD4)]
        public static CalculatorResult MarsSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD4)]
        public static CalculatorResult RahuSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD4)]
        public static CalculatorResult JupiterSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD4)]
        public static CalculatorResult SaturnSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD4)]
        public static CalculatorResult MercurySaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD4)]
        public static CalculatorResult KetuSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD4)]
        public static CalculatorResult VenusSaturnPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD4

        #region MERCURY PD4

        [EventCalculator(EventName.SunMercuryPD4)]
        public static CalculatorResult SunMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD4)]
        public static CalculatorResult MoonMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD4)]
        public static CalculatorResult MarsMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD4)]
        public static CalculatorResult RahuMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD4)]
        public static CalculatorResult JupiterMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD4)]
        public static CalculatorResult SaturnMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD4)]
        public static CalculatorResult MercuryMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD4)]
        public static CalculatorResult KetuMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD4)]
        public static CalculatorResult VenusMercuryPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD4

        #region KETU PD4

        [EventCalculator(EventName.SunKetuPD4)]
        public static CalculatorResult SunKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD4)]
        public static CalculatorResult MoonKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD4)]
        public static CalculatorResult MarsKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD4)]
        public static CalculatorResult RahuKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD4)]
        public static CalculatorResult JupiterKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD4)]
        public static CalculatorResult SaturnKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD4)]
        public static CalculatorResult MercuryKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD4)]
        public static CalculatorResult KetuKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD4)]
        public static CalculatorResult VenusKetuPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD4

        #region VENUS PD4

        [EventCalculator(EventName.SunVenusPD4)]
        public static CalculatorResult SunVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD4)]
        public static CalculatorResult MoonVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD4)]
        public static CalculatorResult MarsVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD4)]
        public static CalculatorResult RahuVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD4)]
        public static CalculatorResult JupiterVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD4)]
        public static CalculatorResult SaturnVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD4)]
        public static CalculatorResult MercuryVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD4)]
        public static CalculatorResult KetuVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD4)]
        public static CalculatorResult VenusVenusPD4(Time time, Person person) => PlanetPD3PlanetPD4(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD4

        //PRANA

        #region SUN PD5

        [EventCalculator(EventName.SunSunPD5)]
        public static CalculatorResult SunSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD5)]
        public static CalculatorResult MoonSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD5)]
        public static CalculatorResult MarsSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD5)]
        public static CalculatorResult RahuSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD5)]
        public static CalculatorResult JupiterSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD5)]
        public static CalculatorResult SaturnSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD5)]
        public static CalculatorResult MercurySunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD5)]
        public static CalculatorResult KetuSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD5)]
        public static CalculatorResult VenusSunPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD5

        #region MOON PD5

        [EventCalculator(EventName.SunMoonPD5)]
        public static CalculatorResult SunMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD5)]
        public static CalculatorResult MoonMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD5)]
        public static CalculatorResult MarsMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD5)]
        public static CalculatorResult RahuMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD5)]
        public static CalculatorResult JupiterMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD5)]
        public static CalculatorResult SaturnMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD5)]
        public static CalculatorResult MercuryMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD5)]
        public static CalculatorResult KetuMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD5)]
        public static CalculatorResult VenusMoonPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD5

        #region MARS PD5

        [EventCalculator(EventName.SunMarsPD5)]
        public static CalculatorResult SunMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD5)]
        public static CalculatorResult MoonMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD5)]
        public static CalculatorResult MarsMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD5)]
        public static CalculatorResult RahuMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD5)]
        public static CalculatorResult JupiterMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD5)]
        public static CalculatorResult SaturnMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD5)]
        public static CalculatorResult MercuryMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD5)]
        public static CalculatorResult KetuMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD5)]
        public static CalculatorResult VenusMarsPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD5

        #region RAHU PD5

        [EventCalculator(EventName.SunRahuPD5)]
        public static CalculatorResult SunRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD5)]
        public static CalculatorResult MoonRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD5)]
        public static CalculatorResult MarsRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD5)]
        public static CalculatorResult RahuRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD5)]
        public static CalculatorResult JupiterRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD5)]
        public static CalculatorResult SaturnRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD5)]
        public static CalculatorResult MercuryRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD5)]
        public static CalculatorResult KetuRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD5)]
        public static CalculatorResult VenusRahuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD5

        #region JUPITER PD5

        [EventCalculator(EventName.SunJupiterPD5)]
        public static CalculatorResult SunJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD5)]
        public static CalculatorResult MoonJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD5)]
        public static CalculatorResult MarsJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD5)]
        public static CalculatorResult RahuJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD5)]
        public static CalculatorResult JupiterJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD5)]
        public static CalculatorResult SaturnJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD5)]
        public static CalculatorResult MercuryJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD5)]
        public static CalculatorResult KetuJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD5)]
        public static CalculatorResult VenusJupiterPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD5

        #region SATURN PD5

        [EventCalculator(EventName.SunSaturnPD5)]
        public static CalculatorResult SunSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD5)]
        public static CalculatorResult MoonSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD5)]
        public static CalculatorResult MarsSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD5)]
        public static CalculatorResult RahuSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD5)]
        public static CalculatorResult JupiterSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD5)]
        public static CalculatorResult SaturnSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD5)]
        public static CalculatorResult MercurySaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD5)]
        public static CalculatorResult KetuSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD5)]
        public static CalculatorResult VenusSaturnPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD5

        #region MERCURY PD5

        [EventCalculator(EventName.SunMercuryPD5)]
        public static CalculatorResult SunMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD5)]
        public static CalculatorResult MoonMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD5)]
        public static CalculatorResult MarsMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD5)]
        public static CalculatorResult RahuMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD5)]
        public static CalculatorResult JupiterMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD5)]
        public static CalculatorResult SaturnMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD5)]
        public static CalculatorResult MercuryMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD5)]
        public static CalculatorResult KetuMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD5)]
        public static CalculatorResult VenusMercuryPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD5

        #region KETU PD5

        [EventCalculator(EventName.SunKetuPD5)]
        public static CalculatorResult SunKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD5)]
        public static CalculatorResult MoonKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD5)]
        public static CalculatorResult MarsKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD5)]
        public static CalculatorResult RahuKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD5)]
        public static CalculatorResult JupiterKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD5)]
        public static CalculatorResult SaturnKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD5)]
        public static CalculatorResult MercuryKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD5)]
        public static CalculatorResult KetuKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD5)]
        public static CalculatorResult VenusKetuPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD5

        #region VENUS PD5

        [EventCalculator(EventName.SunVenusPD5)]
        public static CalculatorResult SunVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD5)]
        public static CalculatorResult MoonVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD5)]
        public static CalculatorResult MarsVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD5)]
        public static CalculatorResult RahuVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD5)]
        public static CalculatorResult JupiterVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD5)]
        public static CalculatorResult SaturnVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD5)]
        public static CalculatorResult MercuryVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD5)]
        public static CalculatorResult KetuVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD5)]
        public static CalculatorResult VenusVenusPD5(Time time, Person person) => PlanetPD4PlanetPD5(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD5


        //PD6

        #region SUN PD6

        [EventCalculator(EventName.SunSunPD6)]
        public static CalculatorResult SunSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD6)]
        public static CalculatorResult MoonSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD6)]
        public static CalculatorResult MarsSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD6)]
        public static CalculatorResult RahuSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD6)]
        public static CalculatorResult JupiterSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD6)]
        public static CalculatorResult SaturnSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD6)]
        public static CalculatorResult MercurySunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD6)]
        public static CalculatorResult KetuSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD6)]
        public static CalculatorResult VenusSunPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD6

        #region MOON PD6

        [EventCalculator(EventName.SunMoonPD6)]
        public static CalculatorResult SunMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD6)]
        public static CalculatorResult MoonMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD6)]
        public static CalculatorResult MarsMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD6)]
        public static CalculatorResult RahuMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD6)]
        public static CalculatorResult JupiterMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD6)]
        public static CalculatorResult SaturnMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD6)]
        public static CalculatorResult MercuryMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD6)]
        public static CalculatorResult KetuMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD6)]
        public static CalculatorResult VenusMoonPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD6

        #region MARS PD6

        [EventCalculator(EventName.SunMarsPD6)]
        public static CalculatorResult SunMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD6)]
        public static CalculatorResult MoonMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD6)]
        public static CalculatorResult MarsMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD6)]
        public static CalculatorResult RahuMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD6)]
        public static CalculatorResult JupiterMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD6)]
        public static CalculatorResult SaturnMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD6)]
        public static CalculatorResult MercuryMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD6)]
        public static CalculatorResult KetuMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD6)]
        public static CalculatorResult VenusMarsPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD6

        #region RAHU PD6

        [EventCalculator(EventName.SunRahuPD6)]
        public static CalculatorResult SunRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD6)]
        public static CalculatorResult MoonRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD6)]
        public static CalculatorResult MarsRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD6)]
        public static CalculatorResult RahuRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD6)]
        public static CalculatorResult JupiterRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD6)]
        public static CalculatorResult SaturnRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD6)]
        public static CalculatorResult MercuryRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD6)]
        public static CalculatorResult KetuRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD6)]
        public static CalculatorResult VenusRahuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD6

        #region JUPITER PD6

        [EventCalculator(EventName.SunJupiterPD6)]
        public static CalculatorResult SunJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD6)]
        public static CalculatorResult MoonJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD6)]
        public static CalculatorResult MarsJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD6)]
        public static CalculatorResult RahuJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD6)]
        public static CalculatorResult JupiterJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD6)]
        public static CalculatorResult SaturnJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD6)]
        public static CalculatorResult MercuryJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD6)]
        public static CalculatorResult KetuJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD6)]
        public static CalculatorResult VenusJupiterPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD6

        #region SATURN PD6

        [EventCalculator(EventName.SunSaturnPD6)]
        public static CalculatorResult SunSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD6)]
        public static CalculatorResult MoonSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD6)]
        public static CalculatorResult MarsSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD6)]
        public static CalculatorResult RahuSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD6)]
        public static CalculatorResult JupiterSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD6)]
        public static CalculatorResult SaturnSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD6)]
        public static CalculatorResult MercurySaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD6)]
        public static CalculatorResult KetuSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD6)]
        public static CalculatorResult VenusSaturnPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD6

        #region MERCURY PD6

        [EventCalculator(EventName.SunMercuryPD6)]
        public static CalculatorResult SunMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD6)]
        public static CalculatorResult MoonMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD6)]
        public static CalculatorResult MarsMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD6)]
        public static CalculatorResult RahuMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD6)]
        public static CalculatorResult JupiterMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD6)]
        public static CalculatorResult SaturnMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD6)]
        public static CalculatorResult MercuryMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD6)]
        public static CalculatorResult KetuMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD6)]
        public static CalculatorResult VenusMercuryPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD6

        #region KETU PD6

        [EventCalculator(EventName.SunKetuPD6)]
        public static CalculatorResult SunKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD6)]
        public static CalculatorResult MoonKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD6)]
        public static CalculatorResult MarsKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD6)]
        public static CalculatorResult RahuKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD6)]
        public static CalculatorResult JupiterKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD6)]
        public static CalculatorResult SaturnKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD6)]
        public static CalculatorResult MercuryKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD6)]
        public static CalculatorResult KetuKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD6)]
        public static CalculatorResult VenusKetuPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD6

        #region VENUS PD6

        [EventCalculator(EventName.SunVenusPD6)]
        public static CalculatorResult SunVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD6)]
        public static CalculatorResult MoonVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD6)]
        public static CalculatorResult MarsVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD6)]
        public static CalculatorResult RahuVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD6)]
        public static CalculatorResult JupiterVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD6)]
        public static CalculatorResult SaturnVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD6)]
        public static CalculatorResult MercuryVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD6)]
        public static CalculatorResult KetuVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD6)]
        public static CalculatorResult VenusVenusPD6(Time time, Person person) => PlanetPD5PlanetPD6(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD6


        //PD7

        #region SUN PD7

        [EventCalculator(EventName.SunSunPD7)]
        public static CalculatorResult SunSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD7)]
        public static CalculatorResult MoonSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD7)]
        public static CalculatorResult MarsSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD7)]
        public static CalculatorResult RahuSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD7)]
        public static CalculatorResult JupiterSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD7)]
        public static CalculatorResult SaturnSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD7)]
        public static CalculatorResult MercurySunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD7)]
        public static CalculatorResult KetuSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD7)]
        public static CalculatorResult VenusSunPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD7

        #region MOON PD7

        [EventCalculator(EventName.SunMoonPD7)]
        public static CalculatorResult SunMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD7)]
        public static CalculatorResult MoonMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD7)]
        public static CalculatorResult MarsMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD7)]
        public static CalculatorResult RahuMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD7)]
        public static CalculatorResult JupiterMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD7)]
        public static CalculatorResult SaturnMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD7)]
        public static CalculatorResult MercuryMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD7)]
        public static CalculatorResult KetuMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD7)]
        public static CalculatorResult VenusMoonPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD7

        #region MARS PD7

        [EventCalculator(EventName.SunMarsPD7)]
        public static CalculatorResult SunMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD7)]
        public static CalculatorResult MoonMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD7)]
        public static CalculatorResult MarsMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD7)]
        public static CalculatorResult RahuMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD7)]
        public static CalculatorResult JupiterMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD7)]
        public static CalculatorResult SaturnMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD7)]
        public static CalculatorResult MercuryMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD7)]
        public static CalculatorResult KetuMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD7)]
        public static CalculatorResult VenusMarsPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD7

        #region RAHU PD7

        [EventCalculator(EventName.SunRahuPD7)]
        public static CalculatorResult SunRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD7)]
        public static CalculatorResult MoonRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD7)]
        public static CalculatorResult MarsRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD7)]
        public static CalculatorResult RahuRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD7)]
        public static CalculatorResult JupiterRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD7)]
        public static CalculatorResult SaturnRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD7)]
        public static CalculatorResult MercuryRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD7)]
        public static CalculatorResult KetuRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD7)]
        public static CalculatorResult VenusRahuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD7

        #region JUPITER PD7

        [EventCalculator(EventName.SunJupiterPD7)]
        public static CalculatorResult SunJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD7)]
        public static CalculatorResult MoonJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD7)]
        public static CalculatorResult MarsJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD7)]
        public static CalculatorResult RahuJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD7)]
        public static CalculatorResult JupiterJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD7)]
        public static CalculatorResult SaturnJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD7)]
        public static CalculatorResult MercuryJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD7)]
        public static CalculatorResult KetuJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD7)]
        public static CalculatorResult VenusJupiterPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD7

        #region SATURN PD7

        [EventCalculator(EventName.SunSaturnPD7)]
        public static CalculatorResult SunSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD7)]
        public static CalculatorResult MoonSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD7)]
        public static CalculatorResult MarsSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD7)]
        public static CalculatorResult RahuSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD7)]
        public static CalculatorResult JupiterSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD7)]
        public static CalculatorResult SaturnSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD7)]
        public static CalculatorResult MercurySaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD7)]
        public static CalculatorResult KetuSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD7)]
        public static CalculatorResult VenusSaturnPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD7

        #region MERCURY PD7

        [EventCalculator(EventName.SunMercuryPD7)]
        public static CalculatorResult SunMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD7)]
        public static CalculatorResult MoonMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD7)]
        public static CalculatorResult MarsMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD7)]
        public static CalculatorResult RahuMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD7)]
        public static CalculatorResult JupiterMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD7)]
        public static CalculatorResult SaturnMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD7)]
        public static CalculatorResult MercuryMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD7)]
        public static CalculatorResult KetuMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD7)]
        public static CalculatorResult VenusMercuryPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD7

        #region KETU PD7

        [EventCalculator(EventName.SunKetuPD7)]
        public static CalculatorResult SunKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD7)]
        public static CalculatorResult MoonKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD7)]
        public static CalculatorResult MarsKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD7)]
        public static CalculatorResult RahuKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD7)]
        public static CalculatorResult JupiterKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD7)]
        public static CalculatorResult SaturnKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD7)]
        public static CalculatorResult MercuryKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD7)]
        public static CalculatorResult KetuKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD7)]
        public static CalculatorResult VenusKetuPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD7

        #region VENUS PD7

        [EventCalculator(EventName.SunVenusPD7)]
        public static CalculatorResult SunVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD7)]
        public static CalculatorResult MoonVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD7)]
        public static CalculatorResult MarsVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD7)]
        public static CalculatorResult RahuVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD7)]
        public static CalculatorResult JupiterVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD7)]
        public static CalculatorResult SaturnVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD7)]
        public static CalculatorResult MercuryVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD7)]
        public static CalculatorResult KetuVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD7)]
        public static CalculatorResult VenusVenusPD7(Time time, Person person) => PlanetPD6PlanetPD7(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD7


        //PD8

        #region SUN PD8

        [EventCalculator(EventName.SunSunPD8)]
        public static CalculatorResult SunSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Sun);

        [EventCalculator(EventName.MoonSunPD8)]
        public static CalculatorResult MoonSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Sun);

        [EventCalculator(EventName.MarsSunPD8)]
        public static CalculatorResult MarsSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Sun);

        [EventCalculator(EventName.RahuSunPD8)]
        public static CalculatorResult RahuSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Sun);

        [EventCalculator(EventName.JupiterSunPD8)]
        public static CalculatorResult JupiterSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Sun);

        [EventCalculator(EventName.SaturnSunPD8)]
        public static CalculatorResult SaturnSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Sun);

        [EventCalculator(EventName.MercurySunPD8)]
        public static CalculatorResult MercurySunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Sun);

        [EventCalculator(EventName.KetuSunPD8)]
        public static CalculatorResult KetuSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Sun);

        [EventCalculator(EventName.VenusSunPD8)]
        public static CalculatorResult VenusSunPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Sun);

        #endregion SUN PD8

        #region MOON PD8

        [EventCalculator(EventName.SunMoonPD8)]
        public static CalculatorResult SunMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Moon);

        [EventCalculator(EventName.MoonMoonPD8)]
        public static CalculatorResult MoonMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Moon);

        [EventCalculator(EventName.MarsMoonPD8)]
        public static CalculatorResult MarsMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Moon);

        [EventCalculator(EventName.RahuMoonPD8)]
        public static CalculatorResult RahuMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Moon);

        [EventCalculator(EventName.JupiterMoonPD8)]
        public static CalculatorResult JupiterMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Moon);

        [EventCalculator(EventName.SaturnMoonPD8)]
        public static CalculatorResult SaturnMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Moon);

        [EventCalculator(EventName.MercuryMoonPD8)]
        public static CalculatorResult MercuryMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Moon);

        [EventCalculator(EventName.KetuMoonPD8)]
        public static CalculatorResult KetuMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Moon);

        [EventCalculator(EventName.VenusMoonPD8)]
        public static CalculatorResult VenusMoonPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Moon);

        #endregion MOON PD8

        #region MARS PD8

        [EventCalculator(EventName.SunMarsPD8)]
        public static CalculatorResult SunMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Mars);

        [EventCalculator(EventName.MoonMarsPD8)]
        public static CalculatorResult MoonMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Mars);

        [EventCalculator(EventName.MarsMarsPD8)]
        public static CalculatorResult MarsMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Mars);

        [EventCalculator(EventName.RahuMarsPD8)]
        public static CalculatorResult RahuMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Mars);

        [EventCalculator(EventName.JupiterMarsPD8)]
        public static CalculatorResult JupiterMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Mars);

        [EventCalculator(EventName.SaturnMarsPD8)]
        public static CalculatorResult SaturnMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Mars);

        [EventCalculator(EventName.MercuryMarsPD8)]
        public static CalculatorResult MercuryMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Mars);

        [EventCalculator(EventName.KetuMarsPD8)]
        public static CalculatorResult KetuMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Mars);

        [EventCalculator(EventName.VenusMarsPD8)]
        public static CalculatorResult VenusMarsPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Mars);

        #endregion MARS PD8

        #region RAHU PD8

        [EventCalculator(EventName.SunRahuPD8)]
        public static CalculatorResult SunRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Rahu);

        [EventCalculator(EventName.MoonRahuPD8)]
        public static CalculatorResult MoonRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Rahu);

        [EventCalculator(EventName.MarsRahuPD8)]
        public static CalculatorResult MarsRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Rahu);

        [EventCalculator(EventName.RahuRahuPD8)]
        public static CalculatorResult RahuRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Rahu);

        [EventCalculator(EventName.JupiterRahuPD8)]
        public static CalculatorResult JupiterRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Rahu);

        [EventCalculator(EventName.SaturnRahuPD8)]
        public static CalculatorResult SaturnRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Rahu);

        [EventCalculator(EventName.MercuryRahuPD8)]
        public static CalculatorResult MercuryRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Rahu);

        [EventCalculator(EventName.KetuRahuPD8)]
        public static CalculatorResult KetuRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Rahu);

        [EventCalculator(EventName.VenusRahuPD8)]
        public static CalculatorResult VenusRahuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Rahu);

        #endregion RAHU PD8

        #region JUPITER PD8

        [EventCalculator(EventName.SunJupiterPD8)]
        public static CalculatorResult SunJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Jupiter);

        [EventCalculator(EventName.MoonJupiterPD8)]
        public static CalculatorResult MoonJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Jupiter);

        [EventCalculator(EventName.MarsJupiterPD8)]
        public static CalculatorResult MarsJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Jupiter);

        [EventCalculator(EventName.RahuJupiterPD8)]
        public static CalculatorResult RahuJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Jupiter);

        [EventCalculator(EventName.JupiterJupiterPD8)]
        public static CalculatorResult JupiterJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Jupiter);

        [EventCalculator(EventName.SaturnJupiterPD8)]
        public static CalculatorResult SaturnJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Jupiter);

        [EventCalculator(EventName.MercuryJupiterPD8)]
        public static CalculatorResult MercuryJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Jupiter);

        [EventCalculator(EventName.KetuJupiterPD8)]
        public static CalculatorResult KetuJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Jupiter);

        [EventCalculator(EventName.VenusJupiterPD8)]
        public static CalculatorResult VenusJupiterPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Jupiter);

        #endregion JUPITER PD8

        #region SATURN PD8

        [EventCalculator(EventName.SunSaturnPD8)]
        public static CalculatorResult SunSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Saturn);

        [EventCalculator(EventName.MoonSaturnPD8)]
        public static CalculatorResult MoonSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Saturn);

        [EventCalculator(EventName.MarsSaturnPD8)]
        public static CalculatorResult MarsSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Saturn);

        [EventCalculator(EventName.RahuSaturnPD8)]
        public static CalculatorResult RahuSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Saturn);

        [EventCalculator(EventName.JupiterSaturnPD8)]
        public static CalculatorResult JupiterSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Saturn);

        [EventCalculator(EventName.SaturnSaturnPD8)]
        public static CalculatorResult SaturnSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Saturn);

        [EventCalculator(EventName.MercurySaturnPD8)]
        public static CalculatorResult MercurySaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Saturn);

        [EventCalculator(EventName.KetuSaturnPD8)]
        public static CalculatorResult KetuSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Saturn);

        [EventCalculator(EventName.VenusSaturnPD8)]
        public static CalculatorResult VenusSaturnPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Saturn);

        #endregion SATURN PD8

        #region MERCURY PD8

        [EventCalculator(EventName.SunMercuryPD8)]
        public static CalculatorResult SunMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Mercury);

        [EventCalculator(EventName.MoonMercuryPD8)]
        public static CalculatorResult MoonMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Mercury);

        [EventCalculator(EventName.MarsMercuryPD8)]
        public static CalculatorResult MarsMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Mercury);

        [EventCalculator(EventName.RahuMercuryPD8)]
        public static CalculatorResult RahuMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Mercury);

        [EventCalculator(EventName.JupiterMercuryPD8)]
        public static CalculatorResult JupiterMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Mercury);

        [EventCalculator(EventName.SaturnMercuryPD8)]
        public static CalculatorResult SaturnMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Mercury);

        [EventCalculator(EventName.MercuryMercuryPD8)]
        public static CalculatorResult MercuryMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Mercury);

        [EventCalculator(EventName.KetuMercuryPD8)]
        public static CalculatorResult KetuMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Mercury);

        [EventCalculator(EventName.VenusMercuryPD8)]
        public static CalculatorResult VenusMercuryPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Mercury);

        #endregion MERCURY PD8

        #region KETU PD8

        [EventCalculator(EventName.SunKetuPD8)]
        public static CalculatorResult SunKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Ketu);

        [EventCalculator(EventName.MoonKetuPD8)]
        public static CalculatorResult MoonKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Ketu);

        [EventCalculator(EventName.MarsKetuPD8)]
        public static CalculatorResult MarsKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Ketu);

        [EventCalculator(EventName.RahuKetuPD8)]
        public static CalculatorResult RahuKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Ketu);

        [EventCalculator(EventName.JupiterKetuPD8)]
        public static CalculatorResult JupiterKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Ketu);

        [EventCalculator(EventName.SaturnKetuPD8)]
        public static CalculatorResult SaturnKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Ketu);

        [EventCalculator(EventName.MercuryKetuPD8)]
        public static CalculatorResult MercuryKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Ketu);

        [EventCalculator(EventName.KetuKetuPD8)]
        public static CalculatorResult KetuKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Ketu);

        [EventCalculator(EventName.VenusKetuPD8)]
        public static CalculatorResult VenusKetuPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Ketu);

        #endregion KETU PD8

        #region VENUS PD8

        [EventCalculator(EventName.SunVenusPD8)]
        public static CalculatorResult SunVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Sun, PlanetName.Venus);

        [EventCalculator(EventName.MoonVenusPD8)]
        public static CalculatorResult MoonVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Moon, PlanetName.Venus);

        [EventCalculator(EventName.MarsVenusPD8)]
        public static CalculatorResult MarsVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mars, PlanetName.Venus);

        [EventCalculator(EventName.RahuVenusPD8)]
        public static CalculatorResult RahuVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Rahu, PlanetName.Venus);

        [EventCalculator(EventName.JupiterVenusPD8)]
        public static CalculatorResult JupiterVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Jupiter, PlanetName.Venus);

        [EventCalculator(EventName.SaturnVenusPD8)]
        public static CalculatorResult SaturnVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Saturn, PlanetName.Venus);

        [EventCalculator(EventName.MercuryVenusPD8)]
        public static CalculatorResult MercuryVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Mercury, PlanetName.Venus);

        [EventCalculator(EventName.KetuVenusPD8)]
        public static CalculatorResult KetuVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Ketu, PlanetName.Venus);

        [EventCalculator(EventName.VenusVenusPD8)]
        public static CalculatorResult VenusVenusPD8(Time time, Person person) => PlanetPD7PlanetPD8(time, person, PlanetName.Venus, PlanetName.Venus);

        #endregion VENUS PD8




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
            var isLord6thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord6th;

            //get lord 8th house
            var lord8th = AstronomicalCalculator.GetLordOfHouse(HouseName.House8, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord8thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord8th;

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
            var isLord5thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord5th;

            //get lord 9th house
            var lord9th = AstronomicalCalculator.GetLordOfHouse(HouseName.House9, person.BirthTime);

            //is lord 8th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord9th;

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
            var isLord5thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord5th;
            var isLord5thBhukti = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD2 == lord5th;

            //is lord 9th dasa occuring
            var isLord9thDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lord9th;
            var isLord9thBhukti = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD2 == lord9th;

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
            var buhktiLord = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD2;

            //get dasa lord =
            var dasaLord = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1;

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
            var isLord2Dasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lordHouse2;

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
            var isLord3Dasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lordHouse3;

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
            var isLord1Dasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == lordHouse1;

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
            var isSaturnDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Saturn;

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
            var isJupiterDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Jupiter;

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
            var isSunDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

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
            var isSunDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

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
            var isSunDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

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
            var isSunDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

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

        /// <summary>
        /// TODO NOTES : this used to be pumped into life predictor,
        ///              but now its just sitting here because suspected to overthrow the final prediction
        /// </summary>
        [EventCalculator(EventName.ExaltedSunDasa)]
        public static CalculatorResult ExaltedSunDasa(Time time, Person person)
        {
            //The Dasa of the Sun in deep exaltation : Sudden
            //gains in cattle and wealth, much travelling in eastern
            //countries, residence in foreign countries, quarrels
            //among friends and relations, pleasure trios and picnic
            //parties and lovely women.

            //is sun dasa occuring
            var isSunDasa = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time).PD1 == PlanetName.Sun;

            var isSunExalted = AstronomicalCalculator.IsPlanetExalted(PlanetName.Sun, time);

            //conditions met
            var occuring = isSunDasa && isSunExalted;

            return CalculatorResult.New(occuring, PlanetName.Sun);
        }

        #endregion DASA SPECIAL RULES

        //SPECIAL SHORTCUT FUNCTIONS

        /// <summary>
        /// special shortcut method to make code smaller, easier to read & maintain
        /// </summary>
        private static CalculatorResult PlanetPD2PlanetPD3(Time time, Person person, PlanetName bhuktiPlanet, PlanetName antaramPlanet)
        {
            //get dasas for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check bhukti
            var isCorrectBhukti = currentPlanetPeriod.PD2 == bhuktiPlanet;

            //check antaram
            var isCorrectAntaram = currentPlanetPeriod.PD3 == antaramPlanet;

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
        private static CalculatorResult PlanetPD3PlanetPD4(Time time, Person person, PlanetName antaramPlanet, PlanetName sukshmaPlanet)
        {
            //get dasas for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check Antaram
            var isCorrectAntaram = currentPlanetPeriod.PD3 == antaramPlanet;

            //check Sukshma
            var isCorrectSukshma = currentPlanetPeriod.PD4 == sukshmaPlanet;

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
        private static CalculatorResult PlanetPD4PlanetPD5(Time time, Person person, PlanetName PD4Planet, PlanetName PD5Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check PD4
            var isCorrectPD4 = currentPlanetPeriod.PD4 == PD4Planet;

            //check PD5
            var isCorrectPD5 = currentPlanetPeriod.PD5 == PD5Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD4 && isCorrectPD5;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(PD4Planet, PD5Planet);

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
        private static CalculatorResult PlanetPD5PlanetPD6(Time time, Person person, PlanetName PD5Planet, PlanetName PD6Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check PD5
            var isCorrectPD5 = currentPlanetPeriod.PD5 == PD5Planet;

            //check PD6
            var isCorrectPD6 = currentPlanetPeriod.PD6 == PD6Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD5 && isCorrectPD6;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(PD5Planet, PD6Planet);

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
        private static CalculatorResult PlanetPD6PlanetPD7(Time time, Person person, PlanetName PD6Planet, PlanetName PD7Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check PD6
            var isCorrectPD6 = currentPlanetPeriod.PD6 == PD6Planet;

            //check PD7
            var isCorrectPD7 = currentPlanetPeriod.PD7 == PD7Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD6 && isCorrectPD7;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(PD6Planet, PD7Planet);

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
        private static CalculatorResult PlanetPD7PlanetPD8(Time time, Person person, PlanetName PD7Planet, PlanetName PD8Planet)
        {
            //get whole dasa for current time
            var currentPlanetPeriod = AstronomicalCalculator.GetCurrentPlanetDasas(person.BirthTime, time);

            //check PD7
            var isCorrectPD7 = currentPlanetPeriod.PD7 == PD7Planet;

            //check PD8
            var isCorrectPD8 = currentPlanetPeriod.PD8 == PD8Planet;

            //occuring if all conditions met
            var occuring = isCorrectPD7 && isCorrectPD8;

            //only get prediction if event occurring, else waste compute cycles
            if (occuring)
            {
                //nature & description override, based on cyclic relationship between planets
                var periodPrediction = AstronomicalCalculator.GetPlanetDasaMajorPlanetAndMinorRelationship(PD7Planet, PD8Planet);

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





//                WHAT DO YOU SEE?
//           THAT'S WHAT'S IN YOUR MIND!
//     THERE IS NO VICE NOR VIRTUE IN THIS WORLD
//          ALL IS BUT A REFLECTION OF YOU!
//
//              SO, WHAT DO YOU SEE? -- Walter Kovacs AKA Rorschach 
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣠⠤⠶⠶⠶⠤⠤⣄⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⡤⠚⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠳⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣠⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⣠⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⡼⢣⡶⠀⠀⠀⠀⠀⣠⠖⠉⣀⡴⠶⠊⢷⡆⠀⠀⠀⠀⠀⠀⠀⠀⢹⡄⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⢀⣼⠃⡼⠁⠀⠀⠀⣠⠞⣡⡴⠋⠁⠀⠀⠀⠘⣿⣧⡀⠀⠀⠀⣇⠀⠀⠸⡇⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⣼⡏⢠⡇⠀⠀⠀⣼⣣⠞⠃⠀⠀⠀⣀⣠⣤⣄⣿⡇⢧⡀⠀⠀⢻⣄⡀⠀⡗⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⢸⡇⢸⡇⠀⢠⣼⣿⣏⡀⠀⠀⢠⢾⠛⠉⠁⢀⣿⡉⠛⣷⣄⠀⢸⣿⠻⣶⣧⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠘⡇⠀⣇⡴⢿⣿⡉⠉⠉⠁⠀⠀⠀⣀⣶⠾⢿⡛⠟⢶⣼⡇⠀⢸⢻⡀⣹⣿⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⣧⠀⢸⡆⣿⣛⠩⣿⣷⠀⠀⠀⠀⠙⠉⣷⢾⣿⡇⠾⠀⢻⠀⢸⠇⣽⠛⢹⡇⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⢸⡄⠀⠻⣏⠛⠳⡿⢿⡞⠀⠀⠀⠀⠀⠾⠿⠿⠇⠆⠀⠀⣆⣼⣱⡇⠀⠈⣧⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⢳⠀⠀⠙⡄⠀⠙⠉⢹⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⡿⠁⢿⡀⠀⢸⣇⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠈⢧⠀⠀⢻⡄⠀⠀⠈⡷⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⠇⢰⠘⡇⠀⠈⢿⣆⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠈⢷⡀⠘⡟⠦⣀⠀⠀⠒⠒⠋⠉⠁⠀⠀⢀⣠⠆⠀⣹⠀⣈⡇⢹⡄⠀⠸⣿⡄⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠱⣄⣷⠀⢹⡳⣄⡀⠀⠀⠀⠀⣀⡴⠚⠁⠀⠀⣿⠀⣟⢿⠈⡇⠀⠀⣹⣻⡀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⣼⠻⣧⣿⠇⢰⡍⠳⠤⣴⠛⠁⠀⠀⠀⠀⢰⣿⠀⡿⢸⠀⢸⡀⢀⣿⡇⣿⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣠⠇⢀⡼⠋⢀⣮⡇⣀⠴⡟⠀⠀⠀⠀⠀⠀⣿⠃⠻⣅⣼⠀⢸⡇⢸⣹⠀⣿⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢀⠏⢀⡞⢀⣀⡸⠟⠋⠁⠘⠁⠀⠀⠀⠀⠀⠀⣿⠀⠀⠉⠳⢄⣸⣧⣧⣟⡼⠉⠀⠀⠀
//⠀⠀⠀⠀⢀⡤⠿⠤⠚⢲⣟⣁⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡀⡀⠀⢀⣀⠙⠛⠿⢾⣄⠀⠀⠀⠀
//⠀⠀⢀⡴⠋⠀⠀⠀⠀⠀⠀⠀⠉⠉⠉⠛⠓⠀⠴⠒⠒⠚⠒⠛⠛⣿⡉⠉⠉⠀⠀⠀⠀⠀⠀⠙⢢⡀⠀
//⠀⠀⣸⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⣄
//⠀⢸⢹⠀⠀⠀⢀⡤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿
//⠀⠀⢸⠀⠀⢀⣾⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣶⡄⠀⠀⠀⠀⠀⠀⠀⣼⠛
//⠀⠀⠸⣇⣰⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡇⠀⠀⠀⠀⠀⠀⢀⡏⠀
//⠀⠀⣰⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢷⠀⠀⠀⠀⠀⠀⣼⠀⠀
//⠀⡞⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡶⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡇⠀⠀⠀⠀⣰⠃⠀⠀
//⣼⢃⣴⡃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⠀⠀⠀⢳⠀⠀⠀⢠⠏⠀⠀⠀
//⣿⠈⠿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢤⣶⠀⠀⢸⠀⠀⠀⣾⠀⠀⠀⠀
//⢹⡆⠀⠀⠐⠊⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⡀⠀⠀⠀⠀⠀⠀⠀⣄⠀⠉⠈⠀⠀⡼⠀⠀⢰⡇⠀⠀⠀⠀
//⠀⠹⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡄⠀⠀⢳⡀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀⣸⠃⠀⠀⣼⠀⠀⠀⠀⠀
//⠀⠀⠈⠳⢤⣀⣀⣀⣀⣀⡴⠖⠋⠀⠀⠀⠀⠙⠦⣀⠀⠀⠀⠀⠀⠀⢀⣠⠾⠁⠀⠀⢰⡏⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⣇⢻⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠓⠒⠒⠒⠋⠉⣿⠀⠀⠀⠀⣸⠁⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢽⣼⠀⠀⠄⠀⠀⠀⠀⠀⠀⠲⣄⠀⠀⠀⠀⠀⠀⠀⠀⢀⡇⠀⠀⠀⢠⡇⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢸⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⠀⠀⣸⠁⠀⠀⠀⣸⠁⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⣿⡇⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⡿⠀⠀⠀⢰⡇⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢿⣿⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⠀⢸⠃⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢸⢹⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⠀⠀⠀⠀⢼⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢸⡼⣇⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢘⣿⠀⠀⠀⠀⣸⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠘⣧⣿⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⢠⡏⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣹⠃⠀⠀⠀⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣄⠀⠀⣾⠃⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢦⣴⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢀⡾⠀⠀⠀⠀⠀⣽⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⡼⠁⠀⠀⠀⠀⠈⠿⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⡄⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⣸⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢦⣀⠀⠀⠀⠀⠀
//       I SEE BEAUTY, I SEE SWEETNESS
//I SEE THE GENIUS OF MAN TO CARVE NUMBERS INTO EMOTION
//          I SEE GOODNESS IN ALL