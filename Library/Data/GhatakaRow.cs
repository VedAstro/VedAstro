using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{

    /// <summary>
    /// Represents 1 row in a Ghataka Chakra table
    /// </summary>
    public record struct GhatakaRow(ZodiacName MoonSign, LunarDayGroup TithiGroup, DayOfWeek WeekDay, ConstellationName MoonConstellation, ZodiacName LagnaSameSex, ZodiacName LagnaOppositeSex);

}
