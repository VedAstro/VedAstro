using System;

namespace VedAstro.Library
{
    public static class EventChartTools
    {

        /// <summary>
        /// supports dynamic 3 types of preset
        /// - age1to10
        /// - 3weeks, 3months, 3years, fulllife
        /// - 1990-2000
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

            //PRESET A = 3days,
            //PRESET B = 1990 - 2000
            //get type of preset
            var isPresetB = timePresetString.Contains("-"); //if has hyphen than must be time range
            var isPresetC = timePresetString.Contains("to"); //age5to10

            TimeRange returnValue;

            //process 1990-2000
            if (isPresetB) { returnValue = ProcessPresetTypeB(); }

            //when preset is age1to50
            else if (isPresetC) { returnValue = ProcessPresetTypeC(); }

            //A type is default processing, if not B or C must A then
            //3days, 2years, this week, full life
            else { returnValue = ProcessPresetTypeA(); }


            return returnValue;


            //process 3days, 2years, this week, full life
            TimeRange ProcessPresetTypeA()
            {

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
                string _1WeekAgo = now.AddDays(-7).ToString("dd/MM/yyyy zzz");
                string _2MonthsAgo = now.AddDays(-60).ToString("dd/MM/yyyy zzz");
                string _3MonthsAgo = now.AddDays(-90).ToString("dd/MM/yyyy zzz");
                string _6MonthsAgo = now.AddDays(-182).ToString("dd/MM/yyyy zzz");
                string _1YearAgo = now.AddDays(-365).ToString("dd/MM/yyyy zzz");
                var timeNow = Time.NowSystem(birthLocation);
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
                        hoursToAdd = Tools.DaysToHours(number); //convert DAYS to HOURS
                        var startDays = now.RemoveHours(hoursToAdd); //back by input
                        var endDays = now.AddHours(hoursToAdd); //front by input
                        start = new Time(startDays, birthLocation);
                        end = new Time(endDays, birthLocation);
                        return new TimeRange(start, end);
                    case "week":
                    case "weeks":
                        hoursToAdd = Tools.WeeksToHours(number);
                        start = timeNow.RemoveHours(hoursToAdd);
                        end = timeNow.AddHours(hoursToAdd); //+the days
                        return new TimeRange(start, end);
                    case "month":
                    case "months":
                        hoursToAdd = Tools.MonthsToHours(number);
                        start = new Time($"00:00 {_1WeekAgo}", birthLocation);
                        end = timeNow.AddHours(hoursToAdd);
                        return new TimeRange(start, end);
                    case "year":
                    case "years":
                        hoursToAdd = Tools.YearsToHours(number);
                        start = new Time($"00:00 {_6MonthsAgo}", birthLocation);
                        end = timeNow.AddHours(hoursToAdd);
                        return new TimeRange(start, end);
                    case "decades":
                    case "decade":
                        hoursToAdd = Tools.DecadesToHours(number);
                        start = new Time($"00:00 {_1YearAgo}", birthLocation);
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
                        end = birthTimeClient.AddYears(75);
                        return new TimeRange(start, end);
                    default:
                        return new TimeRange(Time.Empty, Time.Empty);

                }
            }

            //process age1to50
            TimeRange ProcessPresetTypeC()
            {

                //age 1 to 50
                var split = Tools.SplitAlpha(timePresetString);

                var startAge = int.Parse(split[1]);
                var endAge = int.Parse(split[3]);

                //if age 1 set to 0, because in common talk age 1 is same as birth year, nobody says age 0
                startAge = startAge == 1 ? 0 : startAge;

                //add to birth time to get final time range
                start = birthTimeClient.AddYears(startAge);
                end = birthTimeClient.AddYears(endAge);
                return new TimeRange(start, end);

            }

            //process 1990-2000
            TimeRange ProcessPresetTypeB()
            {
                //break into start & end year
                var splited = timePresetString.Split('-');

                //get year
                var startYear = splited[0];
                var endYear = splited[1];

                //timezone to construct new time for client time
                var timeZone = now.ToString("zzz");

                //create time at start and end of year
                var startTime = new Time($"00:00 01/01/{startYear} {timeZone}", birthLocation);
                var endTime = new Time($"00:00 31/12/{endYear} {timeZone}", birthLocation);

                return new TimeRange(startTime, endTime);

            }
        }




    }

   
}
