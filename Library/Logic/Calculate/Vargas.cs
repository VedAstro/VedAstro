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
    public struct DegreeRange(double startDegree, double endDegree)
    {
        public double StartDegree { get; set; } = startDegree;
        public double EndDegree { get; set; } = endDegree;

        /// <summary>
        /// returns true if given number is within start and end range
        /// </summary>
        public bool IsWithinRange(double degree) => degree >= StartDegree && degree <= EndDegree;
    }

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
    public class Vargas
    {

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

    }
}
