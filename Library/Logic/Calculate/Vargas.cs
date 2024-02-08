using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//I'VE ALREADY BEEN PAID
//MONEY AND GENIUS DO NOT WALK TOGETHER
//EASIER FOR A CAMEL TO GO THROUGH AN EYE OF NEEDLE
//THAN FOR RICH MAN TO GO THE KINGDOM OF GOD

//HOW MANY WHEN LOOKING INTO THE EYES OF THEIR BELOVED, CRY WITH TEARS OF JOY EVERY TIME.
//I SAW YOU YESTERDAY, STILL WHEN I SEE YOU AGAIN, TEARS OF JOY
//HOW CAN IT BE? THAT YOU CAN FALL IN LOVE EVERYDAY
//IT CAN BE WITH GOD, THESE ARE AMONG THE THINGS NOT THOUGHT IN SCHOOLS
//THAT GOD IS REAL, HE IS THERE EVER WATCHING, EVER LOVING
//MORE REAL THAN MOUNTAINS, MORE REAL THAN PAIN.
//
//WHEN DOWN IN THE MOUTH,
//REMEMBER SAINT ANTHONY,
//HE CAME OUT ALL RIGHT.

namespace VedAstro.Library
{
    /// <summary>
    /// Vargas or Subtle Divisions
    /// It is easy to appreciate that any given sign remains on the horizon for an
    /// average of two hours, plus or minus a few minutes. This means that all
    /// persons born during that time will have a similar planetary disposition in
    /// their charts. Also, a given sign rises on the horizon at approximately the
    /// same time (with a difference of approx. 4 minutes) on the subsequent day
    /// also. It is possible that the planetary disposition as well as the rising sign
    /// may remain unaffected even if the two births happen a day apart.
    ///
    /// In cases of
    /// twins too, where the rising signs and the planetary positions are likely to be
    /// similar, segregation of the natives appears difficult.
    /// One of the very brilliant methods of overcoming the difficulties
    /// mentioned above is the use of vargas or subtle divisions. Each sign is
    /// divided into a specific number of parts. Thus, the lagna or the rising sign
    /// falls in a specific area of a division. In any given division or varga, the
    /// placement of the lagna and the planets forms that specific varga or divisional
    /// chart.
    /// </summary>
    public static class Vargas
    {
        /// <summary>
        /// This is an abstacted calculator made to work with table data from
        /// Elements of Vedic Astrology - K. S. Charak (chapter on vargas)
        /// </summary>
        /// <param name="precomputedTable">special tables pre made in code for each varga type</param>
        public static ZodiacSign VargasCoreCalculator(ZodiacSign zodiacSign, Dictionary<DegreeRange, ZodiacName> precomputedTable, int divisionNumber)
        {
            // Get the degree within the sign
            var degreesInSign = zodiacSign.GetDegreesInSign().TotalDegrees;

            //find where table indexes meet
            foreach (var rowData in precomputedTable)
            {
                //NOTE : scan is assumed to begin at small number and work way up
                var isInRange = rowData.Key.IsWithinRange(degreesInSign);

                //return pre-computed sign
                var zodiacName = rowData.Value;
                if (isInRange)
                {
                    //NOTE : degrees in sign have to be converted (special logic) for specific division type
                    var divisionalDegreesInSign = Calculate.DivisionalLongitude(degreesInSign, divisionNumber);
                    return new ZodiacSign(zodiacName, divisionalDegreesInSign);
                }
            }

            throw new Exception("END OF LINE!");
        }

        //MASSIVE STATIC TABLES

        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> HoraTable = new()
        {
            { ZodiacName.Aries, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Taurus, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
            { ZodiacName.Gemini, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Cancer, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
            { ZodiacName.Leo, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Virgo, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
            { ZodiacName.Libra, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Scorpio, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
            { ZodiacName.Sagittarius, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Capricorn, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
            { ZodiacName.Aquarius, new() { { new DegreeRange(0, 15), ZodiacName.Leo }, { new DegreeRange(15, 30), ZodiacName.Cancer } } },
            { ZodiacName.Pisces, new() { { new DegreeRange(0, 15), ZodiacName.Cancer }, { new DegreeRange(15, 30), ZodiacName.Leo } } },
        };

        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> DrekkanaTable = new()
        {
            { ZodiacName.Aries, new() { { new DegreeRange(0, 10), ZodiacName.Aries }, { new DegreeRange(10, 20), ZodiacName.Leo }, { new DegreeRange(20, 30), ZodiacName.Sagittarius } } },
            { ZodiacName.Taurus, new() { { new DegreeRange(0, 10), ZodiacName.Taurus }, { new DegreeRange(10, 20), ZodiacName.Virgo }, { new DegreeRange(20, 30), ZodiacName.Capricorn } } },
            { ZodiacName.Gemini, new() { { new DegreeRange(0, 10), ZodiacName.Gemini }, { new DegreeRange(10, 20), ZodiacName.Libra }, { new DegreeRange(20, 30), ZodiacName.Aquarius } } },
            { ZodiacName.Cancer, new() { { new DegreeRange(0, 10), ZodiacName.Cancer }, { new DegreeRange(10, 20), ZodiacName.Scorpio }, { new DegreeRange(20, 30), ZodiacName.Pisces } } },
            { ZodiacName.Leo, new() { { new DegreeRange(0, 10), ZodiacName.Leo }, { new DegreeRange(10, 20), ZodiacName.Sagittarius }, { new DegreeRange(20, 30), ZodiacName.Aries } } },
            { ZodiacName.Virgo, new() { { new DegreeRange(0, 10), ZodiacName.Virgo }, { new DegreeRange(10, 20), ZodiacName.Capricorn }, { new DegreeRange(20, 30), ZodiacName.Taurus } } },
            { ZodiacName.Libra, new() { { new DegreeRange(0, 10), ZodiacName.Libra }, { new DegreeRange(10, 20), ZodiacName.Aquarius }, { new DegreeRange(20, 30), ZodiacName.Gemini } } },
            { ZodiacName.Scorpio, new() { { new DegreeRange(0, 10), ZodiacName.Scorpio }, { new DegreeRange(10, 20), ZodiacName.Pisces }, { new DegreeRange(20, 30), ZodiacName.Cancer } } },
            { ZodiacName.Sagittarius, new() { { new DegreeRange(0, 10), ZodiacName.Sagittarius }, { new DegreeRange(10, 20), ZodiacName.Aries }, { new DegreeRange(20, 30), ZodiacName.Leo } } },
            { ZodiacName.Capricorn, new() { { new DegreeRange(0, 10), ZodiacName.Capricorn }, { new DegreeRange(10, 20), ZodiacName.Taurus }, { new DegreeRange(20, 30), ZodiacName.Virgo } } },
            { ZodiacName.Aquarius, new() { { new DegreeRange(0, 10), ZodiacName.Aquarius }, { new DegreeRange(10, 20), ZodiacName.Gemini }, { new DegreeRange(20, 30), ZodiacName.Libra } } },
            { ZodiacName.Pisces, new() { { new DegreeRange(0, 10), ZodiacName.Pisces }, { new DegreeRange(10, 20), ZodiacName.Cancer }, { new DegreeRange(20, 30), ZodiacName.Scorpio } } },
        };

        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> ChaturthamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 30), ZodiacName.Capricorn } } },

            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Taurus },
                { new DegreeRange(7.5, 15), ZodiacName.Leo },
                { new DegreeRange(15, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 30), ZodiacName.Aquarius } } },

            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Gemini },
                { new DegreeRange(7.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 30), ZodiacName.Pisces } } },

            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 15), ZodiacName.Libra },
                { new DegreeRange(15, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 30), ZodiacName.Aries } } },

            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 30), ZodiacName.Taurus } } },

            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Virgo },
                { new DegreeRange(7.5, 15), ZodiacName.Sagittarius },
                { new DegreeRange(15, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 30), ZodiacName.Gemini } } },

            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Libra },
                { new DegreeRange(7.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 22.5), ZodiacName.Aries },
                { new DegreeRange(22.5, 30), ZodiacName.Cancer } }
            },

            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 15), ZodiacName.Aquarius },
                { new DegreeRange(15, 22.5), ZodiacName.Taurus },
                { new DegreeRange(22.5, 30), ZodiacName.Leo } }
            },

            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 30), ZodiacName.Virgo } }
            },

            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 15), ZodiacName.Aries },
                { new DegreeRange(15, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 30), ZodiacName.Libra } }
            },

            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Aquarius },
                { new DegreeRange(7.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 22.5), ZodiacName.Leo },
                { new DegreeRange(22.5, 30), ZodiacName.Scorpio } }
            },

            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 22.5), ZodiacName.Virgo },
                { new DegreeRange(22.5, 30), ZodiacName.Sagittarius } }
            }
        };
    }
}