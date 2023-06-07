using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class EventChartTools
    {

        /// <summary>
        /// given a nice human time range will generate start and end times
        /// input user's current timezone, could be different from birth
        /// </summary>
        /// <param name="outputTimezone">"+08:00"</param>
        public static TimeRange AutoCalculateTimeRange(Person inputPerson, string timePreset, TimeSpan outputTimezone)
        {
            var birthDateMonthYear = inputPerson.BirthDateMonthYear;

            //note the use of system timezone and not birth timezone
            //birth time is adjusted to show for clients timezone
            var birthTimeStr = $"00:00 {birthDateMonthYear} {Tools.GetSystemTimezoneStr()}";
            var birthTimeClient = new Time(birthTimeStr, inputPerson.GetBirthLocation());

            var birthLocation = inputPerson.GetBirthLocation();

            Time start, end;
            //use the inputed user's timezone
            DateTimeOffset now = DateTimeOffset.Now.ToOffset(outputTimezone);
            var today = now.ToString("dd/MM/yyyy zzz");

            var yesterday = now.AddDays(-1).ToString("dd/MM/yyyy zzz");
            var timePresetString = timePreset.ToLower(); //so that all cases are accepted


            //NOTE:
            //two possible name types for 6months and "thismonth"
            //so if got number infront then different handle
            //assume input is "3days", number + date type
            //so split by number
            var split = Tools.SplitAlpha(timePresetString);
            var result = int.TryParse(split[0], out int number);
            number = number < 1 ? 1 : number; //min 1, so user can in put just, "year" and except 1 year
            //if no number, than data type in 1st place
            var dateType = result ? split[1] : split[0];


            //process accordingly
            int days;
            double hoursToAdd;
            string _2MonthsAgo;
            var timeNow = Time.Now(birthLocation);
            switch (dateType.ToLower())
            {
                case "hour":
                case "hours":
                    var startHour = now.AddHours(-1); //back 1 hour
                    var endHour = now.AddHours(number); //front by input
                    start = new Time(startHour, birthLocation);
                    end = new Time(endHour, birthLocation);
                    return new TimeRange(start, end);
                case "today":
                case "day":
                case "days":
                    start = new Time($"00:00 {today}", birthLocation);
                    end = timeNow.AddHours(Tools.DaysToHours(number));
                    return new TimeRange(start, end);
                case "week":
                case "weeks":
                    days = number * 7;
                    hoursToAdd = Tools.DaysToHours(days);
                    start = new Time($"00:00 {yesterday}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new TimeRange(start, end);
                case "month":
                case "months":
                    days = number * 30;
                    hoursToAdd = Tools.DaysToHours(days);
                    var _1WeekAgo = now.AddDays(-7).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_1WeekAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new TimeRange(start, end);
                case "year":
                case "years":
                    days = number * 365;
                    hoursToAdd = Tools.DaysToHours(days);
                    _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2MonthsAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new TimeRange(start, end);
                case "decades":
                case "decade":
                    days = number * 3652;
                    hoursToAdd = Tools.DaysToHours(days);
                    _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                    start = new Time($"00:00 {_2MonthsAgo}", birthLocation);
                    end = timeNow.AddHours(hoursToAdd);
                    return new TimeRange(start, end);
                case "age1to50":
                    start = birthTimeClient;
                    end = birthTimeClient.AddYears(50);
                    return new TimeRange(start, end);
                case "age1to25":
                    start = birthTimeClient;
                    end = birthTimeClient.AddYears(25);
                    return new TimeRange(start, end);
                case "age10to35":
                    start = birthTimeClient.AddYears(10);
                    end = birthTimeClient.AddYears(35);
                    return new TimeRange(start, end);
                case "age25to50":
                    start = birthTimeClient.AddYears(25);
                    end = birthTimeClient.AddYears(50);
                    return new TimeRange(start, end);
                case "age35to60":
                    start = birthTimeClient.AddYears(35);
                    end = birthTimeClient.AddYears(60);
                    return new TimeRange(start, end);
                case "age50to75":
                    start = birthTimeClient.AddYears(50);
                    end = birthTimeClient.AddYears(75);
                    return new TimeRange(start, end);
                case "age60to85":
                    start = birthTimeClient.AddYears(60);
                    end = birthTimeClient.AddYears(85);
                    return new TimeRange(start, end);
                case "age75to100":
                    start = birthTimeClient.AddYears(75);
                    end = birthTimeClient.AddYears(100);
                    return new TimeRange(start, end);
                case "age50to100":
                    start = birthTimeClient.AddYears(50);
                    end = birthTimeClient.AddYears(100);
                    return new TimeRange(start, end);
                case "fulllife":
                    start = birthTimeClient;
                    end = birthTimeClient.AddYears(100);
                    return new TimeRange(start, end);
                default:
                    return new TimeRange(Time.Empty, Time.Empty);

            }
        }




    }

    public record TimeRange(Time start, Time end)
    {
        public static TimeRange Empty = new TimeRange(Time.Empty, Time.Empty);

        /// <summary>
        /// Gets the number of days between start and end time
        /// </summary>
        public double daysBetween => this.end.Subtract(this.start).TotalDays;
    }
}
