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

        /// <summary>
        /// D2 : Hora or one-half of a sign (15°).
        /// </summary>
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

        /// <summary>
        /// D3 : Drekkana or one-third of a sign (10°).
        /// </summary>
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

        /// <summary>
        /// D4 : Chaturthamsha or one-fourth of a sign (7°30').
        /// </summary>
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

        /// <summary>
        /// D7 : Saptamsha or one-seventh of a sign (4°17'8.5").
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> SaptamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Aries },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Taurus },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Gemini },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Cancer },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Leo },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Virgo },
                { new DegreeRange(25.7142, 30), ZodiacName.Libra }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Scorpio },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Sagittarius },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Capricorn },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Aquarius },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Pisces },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Aries },
                { new DegreeRange(25.7142, 30), ZodiacName.Taurus }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Gemini },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Cancer },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Leo },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Virgo },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Libra },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Scorpio },
                { new DegreeRange(25.7142, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Cancer },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Leo },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Virgo },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Libra },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Scorpio },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Sagittarius },
                { new DegreeRange(25.7142, 30), ZodiacName.Capricorn }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Leo },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Virgo },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Libra },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Scorpio },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Sagittarius },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Capricorn },
                { new DegreeRange(25.7142, 30), ZodiacName.Aquarius }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Virgo },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Libra },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Scorpio },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Sagittarius },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Capricorn },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Aquarius },
                { new DegreeRange(25.7142, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Libra },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Scorpio },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Sagittarius },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Capricorn },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Aquarius },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Pisces },
                { new DegreeRange(25.7142, 30), ZodiacName.Aries }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Scorpio },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Sagittarius },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Capricorn },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Aquarius },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Pisces },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Aries },
                { new DegreeRange(25.7142, 30), ZodiacName.Taurus }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Sagittarius },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Capricorn },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Aquarius },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Pisces },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Aries },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Taurus },
                { new DegreeRange(25.7142, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Capricorn },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Aquarius },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Pisces },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Aries },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Taurus },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Gemini },
                { new DegreeRange(25.7142, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Aquarius },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Pisces },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Aries },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Taurus },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Gemini },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Cancer },
                { new DegreeRange(25.7142, 30), ZodiacName.Leo }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 4.2857), ZodiacName.Pisces },
                { new DegreeRange(4.2857, 8.5714), ZodiacName.Aries },
                { new DegreeRange(8.5714, 12.8571), ZodiacName.Taurus },
                { new DegreeRange(12.8571, 17.1428), ZodiacName.Gemini },
                { new DegreeRange(17.1428, 21.4285), ZodiacName.Cancer },
                { new DegreeRange(21.4285, 25.7142), ZodiacName.Leo },
                { new DegreeRange(25.7142, 30), ZodiacName.Virgo }
            }},

        };

        /// <summary>
        /// D9 : Navamsha or one-ninth of a sign (3°20').
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> NavamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Aries },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Taurus },
                { new DegreeRange(6.6667, 10), ZodiacName.Gemini },
                { new DegreeRange(10, 13.3333), ZodiacName.Cancer },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Leo },
                { new DegreeRange(16.6667, 20), ZodiacName.Virgo },
                { new DegreeRange(20, 23.3333), ZodiacName.Libra },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Scorpio },
                { new DegreeRange(26.6667, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Capricorn },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Aquarius },
                { new DegreeRange(6.6667, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 13.3333), ZodiacName.Aries },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Taurus },
                { new DegreeRange(16.6667, 20), ZodiacName.Gemini },
                { new DegreeRange(20, 23.3333), ZodiacName.Cancer },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Leo },
                { new DegreeRange(26.6667, 30), ZodiacName.Virgo }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Libra },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Scorpio },
                { new DegreeRange(6.6667, 10), ZodiacName.Sagittarius },
                { new DegreeRange(10, 13.3333), ZodiacName.Capricorn },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Aquarius },
                { new DegreeRange(16.6667, 20), ZodiacName.Pisces },
                { new DegreeRange(20, 23.3333), ZodiacName.Aries },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Taurus },
                { new DegreeRange(26.6667, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Cancer },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Leo },
                { new DegreeRange(6.6667, 10), ZodiacName.Virgo },
                { new DegreeRange(10, 13.3333), ZodiacName.Libra },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Scorpio },
                { new DegreeRange(16.6667, 20), ZodiacName.Sagittarius },
                { new DegreeRange(20, 23.3333), ZodiacName.Capricorn },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Aquarius },
                { new DegreeRange(26.6667, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Aries },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Taurus },
                { new DegreeRange(6.6667, 10), ZodiacName.Gemini },
                { new DegreeRange(10, 13.3333), ZodiacName.Cancer },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Leo },
                { new DegreeRange(16.6667, 20), ZodiacName.Virgo },
                { new DegreeRange(20, 23.3333), ZodiacName.Libra },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Scorpio },
                { new DegreeRange(26.6667, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Capricorn },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Aquarius },
                { new DegreeRange(6.6667, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 13.3333), ZodiacName.Aries },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Taurus },
                { new DegreeRange(16.6667, 20), ZodiacName.Gemini },
                { new DegreeRange(20, 23.3333), ZodiacName.Cancer },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Leo },
                { new DegreeRange(26.6667, 30), ZodiacName.Virgo }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Libra },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Scorpio },
                { new DegreeRange(6.6667, 10), ZodiacName.Sagittarius },
                { new DegreeRange(10, 13.3333), ZodiacName.Capricorn },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Aquarius },
                { new DegreeRange(16.6667, 20), ZodiacName.Pisces },
                { new DegreeRange(20, 23.3333), ZodiacName.Aries },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Taurus },
                { new DegreeRange(26.6667, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Cancer },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Leo },
                { new DegreeRange(6.6667, 10), ZodiacName.Virgo },
                { new DegreeRange(10, 13.3333), ZodiacName.Libra },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Scorpio },
                { new DegreeRange(16.6667, 20), ZodiacName.Sagittarius },
                { new DegreeRange(20, 23.3333), ZodiacName.Capricorn },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Aquarius },
                { new DegreeRange(26.6667, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Aries },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Taurus },
                { new DegreeRange(6.6667, 10), ZodiacName.Gemini },
                { new DegreeRange(10, 13.3333), ZodiacName.Cancer },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Leo },
                { new DegreeRange(16.6667, 20), ZodiacName.Virgo },
                { new DegreeRange(20, 23.3333), ZodiacName.Libra },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Scorpio },
                { new DegreeRange(26.6667, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Capricorn },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Aquarius },
                { new DegreeRange(6.6667, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 13.3333), ZodiacName.Aries },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Taurus },
                { new DegreeRange(16.6667, 20), ZodiacName.Gemini },
                { new DegreeRange(20, 23.3333), ZodiacName.Cancer },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Leo },
                { new DegreeRange(26.6667, 30), ZodiacName.Virgo }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Libra },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Scorpio },
                { new DegreeRange(6.6667, 10), ZodiacName.Sagittarius },
                { new DegreeRange(10, 13.3333), ZodiacName.Capricorn },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Aquarius },
                { new DegreeRange(16.6667, 20), ZodiacName.Pisces },
                { new DegreeRange(20, 23.3333), ZodiacName.Aries },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Taurus },
                { new DegreeRange(26.6667, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 3.3333), ZodiacName.Cancer },
                { new DegreeRange(3.3333, 6.6667), ZodiacName.Leo },
                { new DegreeRange(6.6667, 10), ZodiacName.Virgo },
                { new DegreeRange(10, 13.3333), ZodiacName.Libra },
                { new DegreeRange(13.3333, 16.6667), ZodiacName.Scorpio },
                { new DegreeRange(16.6667, 20), ZodiacName.Sagittarius },
                { new DegreeRange(20, 23.3333), ZodiacName.Capricorn },
                { new DegreeRange(23.3333, 26.6667), ZodiacName.Aquarius },
                { new DegreeRange(26.6667, 30), ZodiacName.Pisces }
            }},
        };

        /// <summary>
        /// D10 : Dashamsha or one-tenth of a sign (3°)
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> DashamamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 3), ZodiacName.Aries },
                { new DegreeRange(3, 6), ZodiacName.Taurus },
                { new DegreeRange(6, 9), ZodiacName.Gemini },
                { new DegreeRange(9, 12), ZodiacName.Cancer },
                { new DegreeRange(12, 15), ZodiacName.Leo },
                { new DegreeRange(15, 18), ZodiacName.Virgo },
                { new DegreeRange(18, 21), ZodiacName.Libra },
                { new DegreeRange(21, 24), ZodiacName.Scorpio },
                { new DegreeRange(24, 27), ZodiacName.Sagittarius },
                { new DegreeRange(27, 30), ZodiacName.Capricorn }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 3), ZodiacName.Capricorn },
                { new DegreeRange(3, 6), ZodiacName.Aquarius },
                { new DegreeRange(6, 9), ZodiacName.Pisces },
                { new DegreeRange(9, 12), ZodiacName.Aries },
                { new DegreeRange(12, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 18), ZodiacName.Gemini },
                { new DegreeRange(18, 21), ZodiacName.Cancer },
                { new DegreeRange(21, 24), ZodiacName.Leo },
                { new DegreeRange(24, 27), ZodiacName.Virgo },
                { new DegreeRange(27, 30), ZodiacName.Libra }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 3), ZodiacName.Gemini },
                { new DegreeRange(3, 6), ZodiacName.Cancer },
                { new DegreeRange(6, 9), ZodiacName.Leo },
                { new DegreeRange(9, 12), ZodiacName.Virgo },
                { new DegreeRange(12, 15), ZodiacName.Libra },
                { new DegreeRange(15, 18), ZodiacName.Scorpio },
                { new DegreeRange(18, 21), ZodiacName.Sagittarius },
                { new DegreeRange(21, 24), ZodiacName.Capricorn },
                { new DegreeRange(24, 27), ZodiacName.Aquarius },
                { new DegreeRange(27, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 3), ZodiacName.Pisces },
                { new DegreeRange(3, 6), ZodiacName.Aries },
                { new DegreeRange(6, 9), ZodiacName.Taurus },
                { new DegreeRange(9, 12), ZodiacName.Gemini },
                { new DegreeRange(12, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 18), ZodiacName.Leo },
                { new DegreeRange(18, 21), ZodiacName.Virgo },
                { new DegreeRange(21, 24), ZodiacName.Libra },
                { new DegreeRange(24, 27), ZodiacName.Scorpio },
                { new DegreeRange(27, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 3), ZodiacName.Leo },
                { new DegreeRange(3, 6), ZodiacName.Virgo },
                { new DegreeRange(6, 9), ZodiacName.Libra },
                { new DegreeRange(9, 12), ZodiacName.Scorpio },
                { new DegreeRange(12, 15), ZodiacName.Sagittarius },
                { new DegreeRange(15, 18), ZodiacName.Capricorn },
                { new DegreeRange(18, 21), ZodiacName.Aquarius },
                { new DegreeRange(21, 24), ZodiacName.Pisces },
                { new DegreeRange(24, 27), ZodiacName.Aries },
                { new DegreeRange(27, 30), ZodiacName.Taurus }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 3), ZodiacName.Taurus },
                { new DegreeRange(3, 6), ZodiacName.Gemini },
                { new DegreeRange(6, 9), ZodiacName.Cancer },
                { new DegreeRange(9, 12), ZodiacName.Leo },
                { new DegreeRange(12, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 18), ZodiacName.Libra },
                { new DegreeRange(18, 21), ZodiacName.Scorpio },
                { new DegreeRange(21, 24), ZodiacName.Sagittarius },
                { new DegreeRange(24, 27), ZodiacName.Capricorn },
                { new DegreeRange(27, 30), ZodiacName.Aquarius }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 3), ZodiacName.Libra },
                { new DegreeRange(3, 6), ZodiacName.Scorpio },
                { new DegreeRange(6, 9), ZodiacName.Sagittarius },
                { new DegreeRange(9, 12), ZodiacName.Capricorn },
                { new DegreeRange(12, 15), ZodiacName.Aquarius },
                { new DegreeRange(15, 18), ZodiacName.Pisces },
                { new DegreeRange(18, 21), ZodiacName.Aries },
                { new DegreeRange(21, 24), ZodiacName.Taurus },
                { new DegreeRange(24, 27), ZodiacName.Gemini },
                { new DegreeRange(27, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 3), ZodiacName.Cancer },
                { new DegreeRange(3, 6), ZodiacName.Leo },
                { new DegreeRange(6, 9), ZodiacName.Virgo },
                { new DegreeRange(9, 12), ZodiacName.Libra },
                { new DegreeRange(12, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 18), ZodiacName.Sagittarius },
                { new DegreeRange(18, 21), ZodiacName.Capricorn },
                { new DegreeRange(21, 24), ZodiacName.Aquarius },
                { new DegreeRange(24, 27), ZodiacName.Pisces },
                { new DegreeRange(27, 30), ZodiacName.Aries }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 3), ZodiacName.Sagittarius },
                { new DegreeRange(3, 6), ZodiacName.Capricorn },
                { new DegreeRange(6, 9), ZodiacName.Aquarius },
                { new DegreeRange(9, 12), ZodiacName.Pisces },
                { new DegreeRange(12, 15), ZodiacName.Aries },
                { new DegreeRange(15, 18), ZodiacName.Taurus },
                { new DegreeRange(18, 21), ZodiacName.Gemini },
                { new DegreeRange(21, 24), ZodiacName.Cancer },
                { new DegreeRange(24, 27), ZodiacName.Leo },
                { new DegreeRange(27, 30), ZodiacName.Virgo }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 3), ZodiacName.Virgo },
                { new DegreeRange(3, 6), ZodiacName.Libra },
                { new DegreeRange(6, 9), ZodiacName.Scorpio },
                { new DegreeRange(9, 12), ZodiacName.Sagittarius },
                { new DegreeRange(12, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 18), ZodiacName.Aquarius },
                { new DegreeRange(18, 21), ZodiacName.Pisces },
                { new DegreeRange(21, 24), ZodiacName.Aries },
                { new DegreeRange(24, 27), ZodiacName.Taurus },
                { new DegreeRange(27, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 3), ZodiacName.Aquarius },
                { new DegreeRange(3, 6), ZodiacName.Pisces },
                { new DegreeRange(6, 9), ZodiacName.Aries },
                { new DegreeRange(9, 12), ZodiacName.Taurus },
                { new DegreeRange(12, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 18), ZodiacName.Cancer },
                { new DegreeRange(18, 21), ZodiacName.Leo },
                { new DegreeRange(21, 24), ZodiacName.Virgo },
                { new DegreeRange(24, 27), ZodiacName.Libra },
                { new DegreeRange(27, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 3), ZodiacName.Scorpio },
                { new DegreeRange(3, 6), ZodiacName.Sagittarius },
                { new DegreeRange(6, 9), ZodiacName.Capricorn },
                { new DegreeRange(9, 12), ZodiacName.Aquarius },
                { new DegreeRange(12, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 18), ZodiacName.Aries },
                { new DegreeRange(18, 21), ZodiacName.Taurus },
                { new DegreeRange(21, 24), ZodiacName.Gemini },
                { new DegreeRange(24, 27), ZodiacName.Cancer },
                { new DegreeRange(27, 30), ZodiacName.Leo }
            }},
        };

        /// <summary>
        /// D12 : Dwadashamsha or one-twelfth of a sign (2°30').
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> DwadashamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Aries },
                { new DegreeRange(2.5, 5), ZodiacName.Taurus },
                { new DegreeRange(5, 7.5), ZodiacName.Gemini },
                { new DegreeRange(7.5, 10), ZodiacName.Cancer },
                { new DegreeRange(10, 12.5), ZodiacName.Leo },
                { new DegreeRange(12.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 17.5), ZodiacName.Libra },
                { new DegreeRange(17.5, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 25), ZodiacName.Capricorn },
                { new DegreeRange(25, 27.5), ZodiacName.Aquarius },
                { new DegreeRange(27.5, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Taurus },
                { new DegreeRange(2.5, 5), ZodiacName.Gemini },
                { new DegreeRange(5, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 10), ZodiacName.Leo },
                { new DegreeRange(10, 12.5), ZodiacName.Virgo },
                { new DegreeRange(12.5, 15), ZodiacName.Libra },
                { new DegreeRange(15, 17.5), ZodiacName.Scorpio },
                { new DegreeRange(17.5, 20), ZodiacName.Sagittarius },
                { new DegreeRange(20, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 27.5), ZodiacName.Pisces },
                { new DegreeRange(27.5, 30), ZodiacName.Aries }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Gemini },
                { new DegreeRange(2.5, 5), ZodiacName.Cancer },
                { new DegreeRange(5, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 10), ZodiacName.Virgo },
                { new DegreeRange(10, 12.5), ZodiacName.Libra },
                { new DegreeRange(12.5, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 17.5), ZodiacName.Sagittarius },
                { new DegreeRange(17.5, 20), ZodiacName.Capricorn },
                { new DegreeRange(20, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 30), ZodiacName.Taurus }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Cancer },
                { new DegreeRange(2.5, 5), ZodiacName.Leo },
                { new DegreeRange(5, 7.5), ZodiacName.Virgo },
                { new DegreeRange(7.5, 10), ZodiacName.Libra },
                { new DegreeRange(10, 12.5), ZodiacName.Scorpio },
                { new DegreeRange(12.5, 15), ZodiacName.Sagittarius },
                { new DegreeRange(15, 17.5), ZodiacName.Capricorn },
                { new DegreeRange(17.5, 20), ZodiacName.Aquarius },
                { new DegreeRange(20, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 25), ZodiacName.Aries },
                { new DegreeRange(25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 5), ZodiacName.Virgo },
                { new DegreeRange(5, 7.5), ZodiacName.Libra },
                { new DegreeRange(7.5, 10), ZodiacName.Scorpio },
                { new DegreeRange(10, 12.5), ZodiacName.Sagittarius },
                { new DegreeRange(12.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 17.5), ZodiacName.Aquarius },
                { new DegreeRange(17.5, 20), ZodiacName.Pisces },
                { new DegreeRange(20, 22.5), ZodiacName.Aries },
                { new DegreeRange(22.5, 25), ZodiacName.Taurus },
                { new DegreeRange(25, 27.5), ZodiacName.Gemini },
                { new DegreeRange(27.5, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 5), ZodiacName.Libra },
                { new DegreeRange(5, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 10), ZodiacName.Sagittarius },
                { new DegreeRange(10, 12.5), ZodiacName.Capricorn },
                { new DegreeRange(12.5, 15), ZodiacName.Aquarius },
                { new DegreeRange(15, 17.5), ZodiacName.Pisces },
                { new DegreeRange(17.5, 20), ZodiacName.Aries },
                { new DegreeRange(20, 22.5), ZodiacName.Taurus },
                { new DegreeRange(22.5, 25), ZodiacName.Gemini },
                { new DegreeRange(25, 27.5), ZodiacName.Cancer },
                { new DegreeRange(27.5, 30), ZodiacName.Leo }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Libra },
                { new DegreeRange(2.5, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 10), ZodiacName.Capricorn },
                { new DegreeRange(10, 12.5), ZodiacName.Aquarius },
                { new DegreeRange(12.5, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 17.5), ZodiacName.Aries },
                { new DegreeRange(17.5, 20), ZodiacName.Taurus },
                { new DegreeRange(20, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 25), ZodiacName.Cancer },
                { new DegreeRange(25, 27.5), ZodiacName.Leo },
                { new DegreeRange(27.5, 30), ZodiacName.Virgo }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Scorpio },
                { new DegreeRange(2.5, 5), ZodiacName.Sagittarius },
                { new DegreeRange(5, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 12.5), ZodiacName.Pisces },
                { new DegreeRange(12.5, 15), ZodiacName.Aries },
                { new DegreeRange(15, 17.5), ZodiacName.Taurus },
                { new DegreeRange(17.5, 20), ZodiacName.Gemini },
                { new DegreeRange(20, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 25), ZodiacName.Leo },
                { new DegreeRange(25, 27.5), ZodiacName.Virgo },
                { new DegreeRange(27.5, 30), ZodiacName.Libra }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Sagittarius },
                { new DegreeRange(2.5, 5), ZodiacName.Capricorn },
                { new DegreeRange(5, 7.5), ZodiacName.Aquarius },
                { new DegreeRange(7.5, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 17.5), ZodiacName.Gemini },
                { new DegreeRange(17.5, 20), ZodiacName.Cancer },
                { new DegreeRange(20, 22.5), ZodiacName.Leo },
                { new DegreeRange(22.5, 25), ZodiacName.Virgo },
                { new DegreeRange(25, 27.5), ZodiacName.Libra },
                { new DegreeRange(27.5, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Capricorn },
                { new DegreeRange(2.5, 5), ZodiacName.Aquarius },
                { new DegreeRange(5, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 10), ZodiacName.Aries },
                { new DegreeRange(10, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 17.5), ZodiacName.Cancer },
                { new DegreeRange(17.5, 20), ZodiacName.Leo },
                { new DegreeRange(20, 22.5), ZodiacName.Virgo },
                { new DegreeRange(22.5, 25), ZodiacName.Libra },
                { new DegreeRange(25, 27.5), ZodiacName.Scorpio },
                { new DegreeRange(27.5, 30), ZodiacName.Sagittarius }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Aquarius },
                { new DegreeRange(2.5, 5), ZodiacName.Pisces },
                { new DegreeRange(5, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 10), ZodiacName.Taurus },
                { new DegreeRange(10, 12.5), ZodiacName.Gemini },
                { new DegreeRange(12.5, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 20), ZodiacName.Virgo },
                { new DegreeRange(20, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 25), ZodiacName.Scorpio },
                { new DegreeRange(25, 27.5), ZodiacName.Sagittarius },
                { new DegreeRange(27.5, 30), ZodiacName.Capricorn }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 2.5), ZodiacName.Pisces },
                { new DegreeRange(2.5, 5), ZodiacName.Aries },
                { new DegreeRange(5, 7.5), ZodiacName.Taurus },
                { new DegreeRange(7.5, 10), ZodiacName.Gemini },
                { new DegreeRange(10, 12.5), ZodiacName.Cancer },
                { new DegreeRange(12.5, 15), ZodiacName.Leo },
                { new DegreeRange(15, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 20), ZodiacName.Libra },
                { new DegreeRange(20, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 25), ZodiacName.Sagittarius },
                { new DegreeRange(25, 27.5), ZodiacName.Capricorn },
                { new DegreeRange(27.5, 30), ZodiacName.Aquarius }
            }},
        };

        /// <summary>
        /// D16 : Shodashamsha or one-sixteenth of a sign (1°52'30").
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> ShodashamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Aries },
                { new DegreeRange(1.875, 3.75), ZodiacName.Taurus },
                { new DegreeRange(3.75, 5.625), ZodiacName.Gemini },
                { new DegreeRange(5.625, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 9.375), ZodiacName.Leo },
                { new DegreeRange(9.375, 11.25), ZodiacName.Virgo },
                { new DegreeRange(11.25, 13.125), ZodiacName.Libra },
                { new DegreeRange(13.125, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 16.875), ZodiacName.Sagittarius },
                { new DegreeRange(16.875, 18.75), ZodiacName.Capricorn },
                { new DegreeRange(18.75, 20.625), ZodiacName.Aquarius },
                { new DegreeRange(20.625, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 24.375), ZodiacName.Aries },
                { new DegreeRange(24.375, 26.25), ZodiacName.Taurus },
                { new DegreeRange(26.25, 28.125), ZodiacName.Gemini },
                { new DegreeRange(28.125, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Leo },
                { new DegreeRange(1.875, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5.625), ZodiacName.Libra },
                { new DegreeRange(5.625, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 9.375), ZodiacName.Sagittarius },
                { new DegreeRange(9.375, 11.25), ZodiacName.Capricorn },
                { new DegreeRange(11.25, 13.125), ZodiacName.Aquarius },
                { new DegreeRange(13.125, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 16.875), ZodiacName.Aries },
                { new DegreeRange(16.875, 18.75), ZodiacName.Taurus },
                { new DegreeRange(18.75, 20.625), ZodiacName.Gemini },
                { new DegreeRange(20.625, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 24.375), ZodiacName.Leo },
                { new DegreeRange(24.375, 26.25), ZodiacName.Virgo },
                { new DegreeRange(26.25, 28.125), ZodiacName.Libra },
                { new DegreeRange(28.125, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Sagittarius },
                { new DegreeRange(1.875, 3.75), ZodiacName.Capricorn },
                { new DegreeRange(3.75, 5.625), ZodiacName.Aquarius },
                { new DegreeRange(5.625, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 9.375), ZodiacName.Aries },
                { new DegreeRange(9.375, 11.25), ZodiacName.Taurus },
                { new DegreeRange(11.25, 13.125), ZodiacName.Gemini },
                { new DegreeRange(13.125, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.875), ZodiacName.Leo },
                { new DegreeRange(16.875, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20.625), ZodiacName.Libra },
                { new DegreeRange(20.625, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 24.375), ZodiacName.Sagittarius },
                { new DegreeRange(24.375, 26.25), ZodiacName.Capricorn },
                { new DegreeRange(26.25, 28.125), ZodiacName.Aquarius },
                { new DegreeRange(28.125, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Aries },
                { new DegreeRange(1.875, 3.75), ZodiacName.Taurus },
                { new DegreeRange(3.75, 5.625), ZodiacName.Gemini },
                { new DegreeRange(5.625, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 9.375), ZodiacName.Leo },
                { new DegreeRange(9.375, 11.25), ZodiacName.Virgo },
                { new DegreeRange(11.25, 13.125), ZodiacName.Libra },
                { new DegreeRange(13.125, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 16.875), ZodiacName.Sagittarius },
                { new DegreeRange(16.875, 18.75), ZodiacName.Capricorn },
                { new DegreeRange(18.75, 20.625), ZodiacName.Aquarius },
                { new DegreeRange(20.625, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 24.375), ZodiacName.Aries },
                { new DegreeRange(24.375, 26.25), ZodiacName.Taurus },
                { new DegreeRange(26.25, 28.125), ZodiacName.Gemini },
                { new DegreeRange(28.125, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Leo },
                { new DegreeRange(1.875, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5.625), ZodiacName.Libra },
                { new DegreeRange(5.625, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 9.375), ZodiacName.Sagittarius },
                { new DegreeRange(9.375, 11.25), ZodiacName.Capricorn },
                { new DegreeRange(11.25, 13.125), ZodiacName.Aquarius },
                { new DegreeRange(13.125, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 16.875), ZodiacName.Aries },
                { new DegreeRange(16.875, 18.75), ZodiacName.Taurus },
                { new DegreeRange(18.75, 20.625), ZodiacName.Gemini },
                { new DegreeRange(20.625, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 24.375), ZodiacName.Leo },
                { new DegreeRange(24.375, 26.25), ZodiacName.Virgo },
                { new DegreeRange(26.25, 28.125), ZodiacName.Libra },
                { new DegreeRange(28.125, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Sagittarius },
                { new DegreeRange(1.875, 3.75), ZodiacName.Capricorn },
                { new DegreeRange(3.75, 5.625), ZodiacName.Aquarius },
                { new DegreeRange(5.625, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 9.375), ZodiacName.Aries },
                { new DegreeRange(9.375, 11.25), ZodiacName.Taurus },
                { new DegreeRange(11.25, 13.125), ZodiacName.Gemini },
                { new DegreeRange(13.125, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.875), ZodiacName.Leo },
                { new DegreeRange(16.875, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20.625), ZodiacName.Libra },
                { new DegreeRange(20.625, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 24.375), ZodiacName.Sagittarius },
                { new DegreeRange(24.375, 26.25), ZodiacName.Capricorn },
                { new DegreeRange(26.25, 28.125), ZodiacName.Aquarius },
                { new DegreeRange(28.125, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Aries },
                { new DegreeRange(1.875, 3.75), ZodiacName.Taurus },
                { new DegreeRange(3.75, 5.625), ZodiacName.Gemini },
                { new DegreeRange(5.625, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 9.375), ZodiacName.Leo },
                { new DegreeRange(9.375, 11.25), ZodiacName.Virgo },
                { new DegreeRange(11.25, 13.125), ZodiacName.Libra },
                { new DegreeRange(13.125, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 16.875), ZodiacName.Sagittarius },
                { new DegreeRange(16.875, 18.75), ZodiacName.Capricorn },
                { new DegreeRange(18.75, 20.625), ZodiacName.Aquarius },
                { new DegreeRange(20.625, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 24.375), ZodiacName.Aries },
                { new DegreeRange(24.375, 26.25), ZodiacName.Taurus },
                { new DegreeRange(26.25, 28.125), ZodiacName.Gemini },
                { new DegreeRange(28.125, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Leo },
                { new DegreeRange(1.875, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5.625), ZodiacName.Libra },
                { new DegreeRange(5.625, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 9.375), ZodiacName.Sagittarius },
                { new DegreeRange(9.375, 11.25), ZodiacName.Capricorn },
                { new DegreeRange(11.25, 13.125), ZodiacName.Aquarius },
                { new DegreeRange(13.125, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 16.875), ZodiacName.Aries },
                { new DegreeRange(16.875, 18.75), ZodiacName.Taurus },
                { new DegreeRange(18.75, 20.625), ZodiacName.Gemini },
                { new DegreeRange(20.625, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 24.375), ZodiacName.Leo },
                { new DegreeRange(24.375, 26.25), ZodiacName.Virgo },
                { new DegreeRange(26.25, 28.125), ZodiacName.Libra },
                { new DegreeRange(28.125, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Sagittarius },
                { new DegreeRange(1.875, 3.75), ZodiacName.Capricorn },
                { new DegreeRange(3.75, 5.625), ZodiacName.Aquarius },
                { new DegreeRange(5.625, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 9.375), ZodiacName.Aries },
                { new DegreeRange(9.375, 11.25), ZodiacName.Taurus },
                { new DegreeRange(11.25, 13.125), ZodiacName.Gemini },
                { new DegreeRange(13.125, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.875), ZodiacName.Leo },
                { new DegreeRange(16.875, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20.625), ZodiacName.Libra },
                { new DegreeRange(20.625, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 24.375), ZodiacName.Sagittarius },
                { new DegreeRange(24.375, 26.25), ZodiacName.Capricorn },
                { new DegreeRange(26.25, 28.125), ZodiacName.Aquarius },
                { new DegreeRange(28.125, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Aries },
                { new DegreeRange(1.875, 3.75), ZodiacName.Taurus },
                { new DegreeRange(3.75, 5.625), ZodiacName.Gemini },
                { new DegreeRange(5.625, 7.5), ZodiacName.Cancer },
                { new DegreeRange(7.5, 9.375), ZodiacName.Leo },
                { new DegreeRange(9.375, 11.25), ZodiacName.Virgo },
                { new DegreeRange(11.25, 13.125), ZodiacName.Libra },
                { new DegreeRange(13.125, 15), ZodiacName.Scorpio },
                { new DegreeRange(15, 16.875), ZodiacName.Sagittarius },
                { new DegreeRange(16.875, 18.75), ZodiacName.Capricorn },
                { new DegreeRange(18.75, 20.625), ZodiacName.Aquarius },
                { new DegreeRange(20.625, 22.5), ZodiacName.Pisces },
                { new DegreeRange(22.5, 24.375), ZodiacName.Aries },
                { new DegreeRange(24.375, 26.25), ZodiacName.Taurus },
                { new DegreeRange(26.25, 28.125), ZodiacName.Gemini },
                { new DegreeRange(28.125, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Leo },
                { new DegreeRange(1.875, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5.625), ZodiacName.Libra },
                { new DegreeRange(5.625, 7.5), ZodiacName.Scorpio },
                { new DegreeRange(7.5, 9.375), ZodiacName.Sagittarius },
                { new DegreeRange(9.375, 11.25), ZodiacName.Capricorn },
                { new DegreeRange(11.25, 13.125), ZodiacName.Aquarius },
                { new DegreeRange(13.125, 15), ZodiacName.Pisces },
                { new DegreeRange(15, 16.875), ZodiacName.Aries },
                { new DegreeRange(16.875, 18.75), ZodiacName.Taurus },
                { new DegreeRange(18.75, 20.625), ZodiacName.Gemini },
                { new DegreeRange(20.625, 22.5), ZodiacName.Cancer },
                { new DegreeRange(22.5, 24.375), ZodiacName.Leo },
                { new DegreeRange(24.375, 26.25), ZodiacName.Virgo },
                { new DegreeRange(26.25, 28.125), ZodiacName.Libra },
                { new DegreeRange(28.125, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 1.875), ZodiacName.Sagittarius },
                { new DegreeRange(1.875, 3.75), ZodiacName.Capricorn },
                { new DegreeRange(3.75, 5.625), ZodiacName.Aquarius },
                { new DegreeRange(5.625, 7.5), ZodiacName.Pisces },
                { new DegreeRange(7.5, 9.375), ZodiacName.Aries },
                { new DegreeRange(9.375, 11.25), ZodiacName.Taurus },
                { new DegreeRange(11.25, 13.125), ZodiacName.Gemini },
                { new DegreeRange(13.125, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.875), ZodiacName.Leo },
                { new DegreeRange(16.875, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20.625), ZodiacName.Libra },
                { new DegreeRange(20.625, 22.5), ZodiacName.Scorpio },
                { new DegreeRange(22.5, 24.375), ZodiacName.Sagittarius },
                { new DegreeRange(24.375, 26.25), ZodiacName.Capricorn },
                { new DegreeRange(26.25, 28.125), ZodiacName.Aquarius },
                { new DegreeRange(28.125, 30), ZodiacName.Pisces }
            }},
        };

        /// <summary>
        /// D20 : Vimshamsha or one-twentieth of a sign (1°30').
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> VimshamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Aries },
                { new DegreeRange(1.5, 3), ZodiacName.Taurus },
                { new DegreeRange(3, 4.5), ZodiacName.Gemini },
                { new DegreeRange(4.5, 6), ZodiacName.Cancer },
                { new DegreeRange(6, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 9), ZodiacName.Virgo },
                { new DegreeRange(9, 10.5), ZodiacName.Libra },
                { new DegreeRange(10.5, 12), ZodiacName.Scorpio },
                { new DegreeRange(12, 13.5), ZodiacName.Sagittarius },
                { new DegreeRange(13.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 16.5), ZodiacName.Aquarius },
                { new DegreeRange(16.5, 18), ZodiacName.Pisces },
                { new DegreeRange(18, 19.5), ZodiacName.Aries },
                { new DegreeRange(19.5, 21), ZodiacName.Taurus },
                { new DegreeRange(21, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 24), ZodiacName.Cancer },
                { new DegreeRange(24, 25.5), ZodiacName.Leo },
                { new DegreeRange(25.5, 27), ZodiacName.Virgo },
                { new DegreeRange(27, 28.5), ZodiacName.Libra },
                { new DegreeRange(28.5, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Sagittarius },
                { new DegreeRange(1.5, 3), ZodiacName.Capricorn },
                { new DegreeRange(3, 4.5), ZodiacName.Aquarius },
                { new DegreeRange(4.5, 6), ZodiacName.Pisces },
                { new DegreeRange(6, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 9), ZodiacName.Taurus },
                { new DegreeRange(9, 10.5), ZodiacName.Gemini },
                { new DegreeRange(10.5, 12), ZodiacName.Cancer },
                { new DegreeRange(12, 13.5), ZodiacName.Leo },
                { new DegreeRange(13.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 16.5), ZodiacName.Libra },
                { new DegreeRange(16.5, 18), ZodiacName.Scorpio },
                { new DegreeRange(18, 19.5), ZodiacName.Sagittarius },
                { new DegreeRange(19.5, 21), ZodiacName.Capricorn },
                { new DegreeRange(21, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 24), ZodiacName.Pisces },
                { new DegreeRange(24, 25.5), ZodiacName.Aries },
                { new DegreeRange(25.5, 27), ZodiacName.Taurus },
                { new DegreeRange(27, 28.5), ZodiacName.Gemini },
                { new DegreeRange(28.5, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Leo },
                { new DegreeRange(1.5, 3), ZodiacName.Virgo },
                { new DegreeRange(3, 4.5), ZodiacName.Libra },
                { new DegreeRange(4.5, 6), ZodiacName.Scorpio },
                { new DegreeRange(6, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 9), ZodiacName.Capricorn },
                { new DegreeRange(9, 10.5), ZodiacName.Aquarius },
                { new DegreeRange(10.5, 12), ZodiacName.Pisces },
                { new DegreeRange(12, 13.5), ZodiacName.Aries },
                { new DegreeRange(13.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 16.5), ZodiacName.Gemini },
                { new DegreeRange(16.5, 18), ZodiacName.Cancer },
                { new DegreeRange(18, 19.5), ZodiacName.Leo },
                { new DegreeRange(19.5, 21), ZodiacName.Virgo },
                { new DegreeRange(21, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 24), ZodiacName.Scorpio },
                { new DegreeRange(24, 25.5), ZodiacName.Sagittarius },
                { new DegreeRange(25.5, 27), ZodiacName.Capricorn },
                { new DegreeRange(27, 28.5), ZodiacName.Aquarius },
                { new DegreeRange(28.5, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Aries },
                { new DegreeRange(1.5, 3), ZodiacName.Taurus },
                { new DegreeRange(3, 4.5), ZodiacName.Gemini },
                { new DegreeRange(4.5, 6), ZodiacName.Cancer },
                { new DegreeRange(6, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 9), ZodiacName.Virgo },
                { new DegreeRange(9, 10.5), ZodiacName.Libra },
                { new DegreeRange(10.5, 12), ZodiacName.Scorpio },
                { new DegreeRange(12, 13.5), ZodiacName.Sagittarius },
                { new DegreeRange(13.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 16.5), ZodiacName.Aquarius },
                { new DegreeRange(16.5, 18), ZodiacName.Pisces },
                { new DegreeRange(18, 19.5), ZodiacName.Aries },
                { new DegreeRange(19.5, 21), ZodiacName.Taurus },
                { new DegreeRange(21, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 24), ZodiacName.Cancer },
                { new DegreeRange(24, 25.5), ZodiacName.Leo },
                { new DegreeRange(25.5, 27), ZodiacName.Virgo },
                { new DegreeRange(27, 28.5), ZodiacName.Libra },
                { new DegreeRange(28.5, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Sagittarius },
                { new DegreeRange(1.5, 3), ZodiacName.Capricorn },
                { new DegreeRange(3, 4.5), ZodiacName.Aquarius },
                { new DegreeRange(4.5, 6), ZodiacName.Pisces },
                { new DegreeRange(6, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 9), ZodiacName.Taurus },
                { new DegreeRange(9, 10.5), ZodiacName.Gemini },
                { new DegreeRange(10.5, 12), ZodiacName.Cancer },
                { new DegreeRange(12, 13.5), ZodiacName.Leo },
                { new DegreeRange(13.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 16.5), ZodiacName.Libra },
                { new DegreeRange(16.5, 18), ZodiacName.Scorpio },
                { new DegreeRange(18, 19.5), ZodiacName.Sagittarius },
                { new DegreeRange(19.5, 21), ZodiacName.Capricorn },
                { new DegreeRange(21, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 24), ZodiacName.Pisces },
                { new DegreeRange(24, 25.5), ZodiacName.Aries },
                { new DegreeRange(25.5, 27), ZodiacName.Taurus },
                { new DegreeRange(27, 28.5), ZodiacName.Gemini },
                { new DegreeRange(28.5, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Leo },
                { new DegreeRange(1.5, 3), ZodiacName.Virgo },
                { new DegreeRange(3, 4.5), ZodiacName.Libra },
                { new DegreeRange(4.5, 6), ZodiacName.Scorpio },
                { new DegreeRange(6, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 9), ZodiacName.Capricorn },
                { new DegreeRange(9, 10.5), ZodiacName.Aquarius },
                { new DegreeRange(10.5, 12), ZodiacName.Pisces },
                { new DegreeRange(12, 13.5), ZodiacName.Aries },
                { new DegreeRange(13.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 16.5), ZodiacName.Gemini },
                { new DegreeRange(16.5, 18), ZodiacName.Cancer },
                { new DegreeRange(18, 19.5), ZodiacName.Leo },
                { new DegreeRange(19.5, 21), ZodiacName.Virgo },
                { new DegreeRange(21, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 24), ZodiacName.Scorpio },
                { new DegreeRange(24, 25.5), ZodiacName.Sagittarius },
                { new DegreeRange(25.5, 27), ZodiacName.Capricorn },
                { new DegreeRange(27, 28.5), ZodiacName.Aquarius },
                { new DegreeRange(28.5, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Aries },
                { new DegreeRange(1.5, 3), ZodiacName.Taurus },
                { new DegreeRange(3, 4.5), ZodiacName.Gemini },
                { new DegreeRange(4.5, 6), ZodiacName.Cancer },
                { new DegreeRange(6, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 9), ZodiacName.Virgo },
                { new DegreeRange(9, 10.5), ZodiacName.Libra },
                { new DegreeRange(10.5, 12), ZodiacName.Scorpio },
                { new DegreeRange(12, 13.5), ZodiacName.Sagittarius },
                { new DegreeRange(13.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 16.5), ZodiacName.Aquarius },
                { new DegreeRange(16.5, 18), ZodiacName.Pisces },
                { new DegreeRange(18, 19.5), ZodiacName.Aries },
                { new DegreeRange(19.5, 21), ZodiacName.Taurus },
                { new DegreeRange(21, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 24), ZodiacName.Cancer },
                { new DegreeRange(24, 25.5), ZodiacName.Leo },
                { new DegreeRange(25.5, 27), ZodiacName.Virgo },
                { new DegreeRange(27, 28.5), ZodiacName.Libra },
                { new DegreeRange(28.5, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Sagittarius },
                { new DegreeRange(1.5, 3), ZodiacName.Capricorn },
                { new DegreeRange(3, 4.5), ZodiacName.Aquarius },
                { new DegreeRange(4.5, 6), ZodiacName.Pisces },
                { new DegreeRange(6, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 9), ZodiacName.Taurus },
                { new DegreeRange(9, 10.5), ZodiacName.Gemini },
                { new DegreeRange(10.5, 12), ZodiacName.Cancer },
                { new DegreeRange(12, 13.5), ZodiacName.Leo },
                { new DegreeRange(13.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 16.5), ZodiacName.Libra },
                { new DegreeRange(16.5, 18), ZodiacName.Scorpio },
                { new DegreeRange(18, 19.5), ZodiacName.Sagittarius },
                { new DegreeRange(19.5, 21), ZodiacName.Capricorn },
                { new DegreeRange(21, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 24), ZodiacName.Pisces },
                { new DegreeRange(24, 25.5), ZodiacName.Aries },
                { new DegreeRange(25.5, 27), ZodiacName.Taurus },
                { new DegreeRange(27, 28.5), ZodiacName.Gemini },
                { new DegreeRange(28.5, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Leo },
                { new DegreeRange(1.5, 3), ZodiacName.Virgo },
                { new DegreeRange(3, 4.5), ZodiacName.Libra },
                { new DegreeRange(4.5, 6), ZodiacName.Scorpio },
                { new DegreeRange(6, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 9), ZodiacName.Capricorn },
                { new DegreeRange(9, 10.5), ZodiacName.Aquarius },
                { new DegreeRange(10.5, 12), ZodiacName.Pisces },
                { new DegreeRange(12, 13.5), ZodiacName.Aries },
                { new DegreeRange(13.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 16.5), ZodiacName.Gemini },
                { new DegreeRange(16.5, 18), ZodiacName.Cancer },
                { new DegreeRange(18, 19.5), ZodiacName.Leo },
                { new DegreeRange(19.5, 21), ZodiacName.Virgo },
                { new DegreeRange(21, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 24), ZodiacName.Scorpio },
                { new DegreeRange(24, 25.5), ZodiacName.Sagittarius },
                { new DegreeRange(25.5, 27), ZodiacName.Capricorn },
                { new DegreeRange(27, 28.5), ZodiacName.Aquarius },
                { new DegreeRange(28.5, 30), ZodiacName.Pisces }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Aries },
                { new DegreeRange(1.5, 3), ZodiacName.Taurus },
                { new DegreeRange(3, 4.5), ZodiacName.Gemini },
                { new DegreeRange(4.5, 6), ZodiacName.Cancer },
                { new DegreeRange(6, 7.5), ZodiacName.Leo },
                { new DegreeRange(7.5, 9), ZodiacName.Virgo },
                { new DegreeRange(9, 10.5), ZodiacName.Libra },
                { new DegreeRange(10.5, 12), ZodiacName.Scorpio },
                { new DegreeRange(12, 13.5), ZodiacName.Sagittarius },
                { new DegreeRange(13.5, 15), ZodiacName.Capricorn },
                { new DegreeRange(15, 16.5), ZodiacName.Aquarius },
                { new DegreeRange(16.5, 18), ZodiacName.Pisces },
                { new DegreeRange(18, 19.5), ZodiacName.Aries },
                { new DegreeRange(19.5, 21), ZodiacName.Taurus },
                { new DegreeRange(21, 22.5), ZodiacName.Gemini },
                { new DegreeRange(22.5, 24), ZodiacName.Cancer },
                { new DegreeRange(24, 25.5), ZodiacName.Leo },
                { new DegreeRange(25.5, 27), ZodiacName.Virgo },
                { new DegreeRange(27, 28.5), ZodiacName.Libra },
                { new DegreeRange(28.5, 30), ZodiacName.Scorpio }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Sagittarius },
                { new DegreeRange(1.5, 3), ZodiacName.Capricorn },
                { new DegreeRange(3, 4.5), ZodiacName.Aquarius },
                { new DegreeRange(4.5, 6), ZodiacName.Pisces },
                { new DegreeRange(6, 7.5), ZodiacName.Aries },
                { new DegreeRange(7.5, 9), ZodiacName.Taurus },
                { new DegreeRange(9, 10.5), ZodiacName.Gemini },
                { new DegreeRange(10.5, 12), ZodiacName.Cancer },
                { new DegreeRange(12, 13.5), ZodiacName.Leo },
                { new DegreeRange(13.5, 15), ZodiacName.Virgo },
                { new DegreeRange(15, 16.5), ZodiacName.Libra },
                { new DegreeRange(16.5, 18), ZodiacName.Scorpio },
                { new DegreeRange(18, 19.5), ZodiacName.Sagittarius },
                { new DegreeRange(19.5, 21), ZodiacName.Capricorn },
                { new DegreeRange(21, 22.5), ZodiacName.Aquarius },
                { new DegreeRange(22.5, 24), ZodiacName.Pisces },
                { new DegreeRange(24, 25.5), ZodiacName.Aries },
                { new DegreeRange(25.5, 27), ZodiacName.Taurus },
                { new DegreeRange(27, 28.5), ZodiacName.Gemini },
                { new DegreeRange(28.5, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 1.5), ZodiacName.Leo },
                { new DegreeRange(1.5, 3), ZodiacName.Virgo },
                { new DegreeRange(3, 4.5), ZodiacName.Libra },
                { new DegreeRange(4.5, 6), ZodiacName.Scorpio },
                { new DegreeRange(6, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 9), ZodiacName.Capricorn },
                { new DegreeRange(9, 10.5), ZodiacName.Aquarius },
                { new DegreeRange(10.5, 12), ZodiacName.Pisces },
                { new DegreeRange(12, 13.5), ZodiacName.Aries },
                { new DegreeRange(13.5, 15), ZodiacName.Taurus },
                { new DegreeRange(15, 16.5), ZodiacName.Gemini },
                { new DegreeRange(16.5, 18), ZodiacName.Cancer },
                { new DegreeRange(18, 19.5), ZodiacName.Leo },
                { new DegreeRange(19.5, 21), ZodiacName.Virgo },
                { new DegreeRange(21, 22.5), ZodiacName.Libra },
                { new DegreeRange(22.5, 24), ZodiacName.Scorpio },
                { new DegreeRange(24, 25.5), ZodiacName.Sagittarius },
                { new DegreeRange(25.5, 27), ZodiacName.Capricorn },
                { new DegreeRange(27, 28.5), ZodiacName.Aquarius },
                { new DegreeRange(28.5, 30), ZodiacName.Pisces }
            }},
        };

        /// <summary>
        /// D24 : Chaturvimshamsha or one-twenty fourth of a sign (1°15').
        /// </summary>
        public static Dictionary<ZodiacName, Dictionary<DegreeRange, ZodiacName>> ChaturvimshamshaTable = new()
        {
            { ZodiacName.Aries, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Taurus, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Gemini, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Cancer, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Leo, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Virgo, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Libra, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Scorpio, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Sagittarius, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Capricorn, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
            { ZodiacName.Aquarius, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Leo },
                { new DegreeRange(1.25, 2.5), ZodiacName.Virgo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Libra },
                { new DegreeRange(3.75, 5), ZodiacName.Scorpio },
                { new DegreeRange(5, 6.25), ZodiacName.Sagittarius },
                { new DegreeRange(6.25, 7.5), ZodiacName.Capricorn },
                { new DegreeRange(7.5, 8.75), ZodiacName.Aquarius },
                { new DegreeRange(8.75, 10), ZodiacName.Pisces },
                { new DegreeRange(10, 11.25), ZodiacName.Aries },
                { new DegreeRange(11.25, 12.5), ZodiacName.Taurus },
                { new DegreeRange(12.5, 13.75), ZodiacName.Gemini },
                { new DegreeRange(13.75, 15), ZodiacName.Cancer },
                { new DegreeRange(15, 16.25), ZodiacName.Leo },
                { new DegreeRange(16.25, 17.5), ZodiacName.Virgo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Libra },
                { new DegreeRange(18.75, 20), ZodiacName.Scorpio },
                { new DegreeRange(20, 21.25), ZodiacName.Sagittarius },
                { new DegreeRange(21.25, 22.5), ZodiacName.Capricorn },
                { new DegreeRange(22.5, 23.75), ZodiacName.Aquarius },
                { new DegreeRange(23.75, 25), ZodiacName.Pisces },
                { new DegreeRange(25, 26.25), ZodiacName.Aries },
                { new DegreeRange(26.25, 27.5), ZodiacName.Taurus },
                { new DegreeRange(27.5, 28.75), ZodiacName.Gemini },
                { new DegreeRange(28.75, 30), ZodiacName.Cancer }
            }},
            { ZodiacName.Pisces, new() {
                { new DegreeRange(0, 1.25), ZodiacName.Cancer },
                { new DegreeRange(1.25, 2.5), ZodiacName.Leo },
                { new DegreeRange(2.5, 3.75), ZodiacName.Virgo },
                { new DegreeRange(3.75, 5), ZodiacName.Libra },
                { new DegreeRange(5, 6.25), ZodiacName.Scorpio },
                { new DegreeRange(6.25, 7.5), ZodiacName.Sagittarius },
                { new DegreeRange(7.5, 8.75), ZodiacName.Capricorn },
                { new DegreeRange(8.75, 10), ZodiacName.Aquarius },
                { new DegreeRange(10, 11.25), ZodiacName.Pisces },
                { new DegreeRange(11.25, 12.5), ZodiacName.Aries },
                { new DegreeRange(12.5, 13.75), ZodiacName.Taurus },
                { new DegreeRange(13.75, 15), ZodiacName.Gemini },
                { new DegreeRange(15, 16.25), ZodiacName.Cancer },
                { new DegreeRange(16.25, 17.5), ZodiacName.Leo },
                { new DegreeRange(17.5, 18.75), ZodiacName.Virgo },
                { new DegreeRange(18.75, 20), ZodiacName.Libra },
                { new DegreeRange(20, 21.25), ZodiacName.Scorpio },
                { new DegreeRange(21.25, 22.5), ZodiacName.Sagittarius },
                { new DegreeRange(22.5, 23.75), ZodiacName.Capricorn },
                { new DegreeRange(23.75, 25), ZodiacName.Aquarius },
                { new DegreeRange(25, 26.25), ZodiacName.Pisces },
                { new DegreeRange(26.25, 27.5), ZodiacName.Aries },
                { new DegreeRange(27.5, 28.75), ZodiacName.Taurus },
                { new DegreeRange(28.75, 30), ZodiacName.Gemini }
            }},
        };

        /// <summary>
        /// D27 : Bhamsha (Sapta-vimshamsha) or one-twentyseventh of a sign (1°6'40").
        /// </summary>

        /// <summary>
        /// D30 : Trimshamsha or one-thirtieth of a sign (1°00').
        /// </summary>

        /// <summary>
        /// D40 : Khavedamsha or one-fortieth of a sign (0°45').
        /// </summary>

        /// <summary>
        /// D45 : Aksha-vedamsha or one-fortyfifth of a sign (0°40').
        /// </summary>

        /// <summary>
        /// D60 : Shashtyamsha or one-sixtieth of a sign (0°30').
        /// </summary>


    }
}