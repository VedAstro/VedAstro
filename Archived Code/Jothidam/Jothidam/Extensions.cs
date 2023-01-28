using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Jothidam
{
    public static class Extensions
    {

        public static Constellation Parse(this Constellation type, string constellationName)
        {
            FieldInfo constNameFieldInfo = typeof(Constellation).GetField(constellationName); 

            return (Constellation)constNameFieldInfo.GetValue(new Constellation());

        }

        public static DayOfWeek Parse(this DayOfWeek type, string dayName)
        {
            FieldInfo dayNameFieldInfo = typeof(DayOfWeek).GetField(dayName);

            return (DayOfWeek)dayNameFieldInfo.GetValue(new DayOfWeek());
        }

        public static Zodiac Parse(this Zodiac type, string zodiacName)
        {
            FieldInfo zodNameFieldInfo = typeof(Zodiac).GetField(zodiacName);

            return (Zodiac)zodNameFieldInfo.GetValue(new Zodiac());

        }

        public static int Difference(this Zodiac first, Zodiac last)
        {
            int x = (int)first;
            int y = (int)last;
            int temp;

            if (y < x)
                temp = (y + 12) - x;
            else
                temp = x - y;

            temp++;

            return temp;

        }


    }
}
