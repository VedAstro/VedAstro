using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jothidam
{
    public static class DateFilterMethod
    {
        public static List<DateTimeOffset> LunarFilter(List<DateTimeOffset> dateList, Criteria criteria, SearchInput searchInput)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            Tools.SearchLunarDay(criteria.lunarDayList, returnDates);

            return returnDates;
        }

        public static List<DateTimeOffset> ConstellationFilter(List<DateTimeOffset> dateList, Criteria criteria, SearchInput searchInput)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            returnDates = Tools.RulingConstellationSearch(dateList, criteria.rulingConstellationList);

            return returnDates;
        }

        public static List<DateTimeOffset> DayOfWeekFilter(List<DateTimeOffset> dateList, Criteria criteria, SearchInput searchInput)
        {
            List<DateTimeOffset> returnDates = new List<DateTimeOffset> { };

            returnDates = Tools.DayOfWeekSearch(dateList, criteria.dayOfWeekList);

            return returnDates;
        }


    }
}
