using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// A single representation of time, contains both standard time (STD) & local mean time (LMT)
    /// LMT is actual time based on the location of the place
    /// IMMUTABLE CLASS
    /// Exists as a merger/wrapper between Standard Time at a place and the coordinates of that place,
    /// to generate the Local Mean Time needed for astrological calculation
    /// </summary>
    [Serializable()]
    public struct Time : IToXml, IFromUrl, IToJson
    {
        //FIELDS

        /// <summary>
        /// The number of pieces the URL version of this instance needs to be cut for processing
        /// EXP -> Location/Singapore/Time/23:59/31/12/2000/+08:00/ == 8 PIECES
        /// </summary>
        public static int OpenAPILength = 8;

        private readonly DateTimeOffset _stdTime;
        private readonly GeoLocation _geoLocation;

        //CONSTANT FIELDS
        private static readonly DateTimeFormatInfo FormatInfo = GetDateTimeFormatInfo();
        /// <summary>
        /// HH:mm dd/MM/yyyy zzz
        /// </summary>
        public const string DateTimeFormat = "HH:mm dd/MM/yyyy zzz"; //define date time format

        public const string DateTimeFormatTimezone = "zzz";

        /// <summary>
        /// HH:mm dd/MM/yyyy
        /// </summary>
        public const string DateTimeFormatNoTimezone = "HH:mm dd/MM/yyyy"; //define date time format

        /// <summary>
        /// HH:mm:ss dd/MM/yyyy zzz
        /// </summary>
        public const string DateTimeFormatSeconds = "HH:mm:ss dd/MM/yyyy zzz"; //used in logging

        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static Time Empty = new("00:00 01/01/2000 +08:00", GeoLocation.Empty);

        /// <summary>
        /// Creates a new instance of time from STD & Geo location
        /// </summary>
        public Time(DateTimeOffset stdDateTime, GeoLocation geoLocation)
        {
            //store std time
            _stdTime = stdDateTime;

            //store geo location for later use
            _geoLocation = geoLocation;
        }

        /// <summary>
        /// Creates a new instance of time from STD string (HH:mm dd/MM/yyyy zzz)
        /// & Geo location
        /// </summary>
        public Time(string stdDateTimeText, GeoLocation geoLocation)
        {
            try
            {
                var stdDateTime = DateTimeOffset.ParseExact(stdDateTimeText, Time.DateTimeFormat, null);

                //store std time
                _stdTime = stdDateTime;

                //store geolocation for later use
                _geoLocation = geoLocation;

            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw e;

                //if can't parse just make empty instance
                _stdTime = DateTimeOffset.MinValue;
                _geoLocation = GeoLocation.Empty;
            }
        }

        /// <summary>
        /// Creates a new instance of time from LMT
        /// </summary>
        public Time(DateTime lmtDateTime, TimeSpan stdOffset, GeoLocation geoLocation)
        {
            //get lmt time
            var lmtTime = new DateTimeOffset(lmtDateTime, GetLocalTimeOffset(geoLocation.Longitude()));

            //convert lmt to std & store it
            _stdTime = lmtTime.ToOffset(stdOffset);

            //store geo location for later use
            _geoLocation = geoLocation;

        }

        /// <summary>
        /// Gets now time (STD) at this time's location/offset
        /// SYSTEM NOW TIME
        /// </summary>
        public DateTimeOffset StdTimeNowAtOffset => DateTimeOffset.Now.ToOffset(this.GetStdDateTimeOffset().Offset);

        /// <summary>
        /// Gets now time (STD) at this time's location/offset
        /// SYSTEM NOW TIME
        /// </summary>
        public Time TimeNowAtOffset => new Time(this.StdTimeNowAtOffset, this.GetGeoLocation());

        /// <summary>
        /// Sample method used for Open API example metadata StandardHoroscope
        /// </summary>
        public static Time StandardHoroscope() => new("14:20 16/10/1918 +05:30", GeoLocation.Bangalore);



        //█▀█ █░█ █▄▄ █░░ █ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▄█ █▄█ █▄▄ █ █▄▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█



        /// <summary>
        /// Gets the Time now in current system, needs location
        /// Note: Offset of system is used not location
        /// </summary>
        public static Time NowSystem(GeoLocation geoLocation)
        {
            //get current date time
            var dateTimeOffset = DateTimeOffset.Now;

            return new Time(dateTimeOffset, geoLocation);
        }


        /// <summary>
        /// Gets the Time now at given location uses api to get correct offset
        /// </summary>
        public static async Task<Time> Now(GeoLocation geoLocation)
        {
            //get current date time
            var dateTimeOffset = DateTimeOffset.UtcNow;

            //get standard offset at location
            var locationOffset = await Tools.GetTimezoneOffsetApi(geoLocation, dateTimeOffset);

            DateTimeOffset temp = DateTimeOffset.ParseExact(locationOffset.Payload, Time.DateTimeFormatTimezone, null);
            TimeSpan timespan = temp.Offset;

            //var timeSpan = TimeSpan.Parse(locationOffset.Payload);
            var modifiedOffset = dateTimeOffset.ToOffset(timespan);

            return new Time(modifiedOffset, geoLocation);
        }

        public int StdYear() => this.GetStdDateTimeOffset().Year;

        public int StdMonth() => this.GetStdDateTimeOffset().Month;

        /// <summary>
        /// Will return month number as text with leading 0
        /// </summary>
        public string StdMonthText() => this.GetStdDateTimeOffset().Month.ToString("D2");

        /// <summary>
        /// Gets date in month 1-31
        /// </summary>
        public int StdDate() => this.GetStdDateTimeOffset().Day;

        /// <summary>
        /// Gets date in month 1-31
        /// Will return month number as text with leading 0
        /// </summary>
        public string StdDateText() => this.GetStdDateTimeOffset().Day.ToString("D2");

        /// <summary>
        /// Gets hour in 0 - 23
        /// </summary>
        public int StdHour() => this.GetStdDateTimeOffset().Hour;

        /// <summary>
        /// Get the geo location at place of time
        /// </summary>
        public GeoLocation GetGeoLocation() => _geoLocation;

        /// <summary>
        /// Slices time range into pieces by inputed hours
        /// Given a start time and end time, it will add precision hours to start time until reaching end time.
        /// Note: number of slices returned != precision hours
        /// </summary>
        public static List<Time> GetTimeListFromRange(Time startTime, Time endTime, double precisionInHours)
        {
            //declare return value
            var timeList = new List<Time>();

            //create list
            for (var day = startTime; day.GetStdDateTimeOffset() <= endTime.GetStdDateTimeOffset(); day = day.AddHours(precisionInHours))
            {
                timeList.Add(day);
            }

            //return value
            return timeList;
        }

        /// <summary>
        /// Returns a new instance of the modified time.
        /// Only input positive numbers
        /// </summary>
        public Time AddHours(double granularityHours)
        {
            //increment time by hours
            var stdTime = _stdTime.AddHours(granularityHours);

            //create new instance of incremented time
            var newTime = new Time(stdTime, _geoLocation);

            //return time to caller
            return newTime;

        }

        /// <summary>
        /// Returns a new instance of the modified time.
        /// Only input positive numbers
        /// </summary>
        public Time RemoveHours(double granularityHours)
        {
            //increment time by hours
            var stdTime = _stdTime.RemoveHours(granularityHours);

            //create new instance of incremented time
            var newTime = new Time(stdTime, _geoLocation);

            //return time to caller
            return newTime;

        }

        /// <summary>
        /// Returns new instance.
        /// Moves current date to exactly next day, without altering time (hh:ss)
        /// </summary>
        public Time MoveToNextDay()
        {
            //move to next day
            var stdTime = _stdTime.AddDays(1).AddTicks(-_stdTime.TimeOfDay.Ticks);

            //create new instance of incremented time
            var newTime = new Time(stdTime, _geoLocation);

            //return time to caller
            return newTime;
        }

        /// <summary>
        /// Returns a new instance with the added years.
        /// Only input positive numbers 
        /// </summary>
        public Time AddYears(int years)
        {
            const int hoursInAYear = 8760;

            //create new same as current one
            var newTime = this;

            //convert years to hours and together
            for (var i = 0; i < years; i++)
            {
                newTime = newTime.AddHours(hoursInAYear);
            }

            return newTime;
        }

        /// <summary>
        /// Returns a new instance of the modified time.
        /// Only positive numbers
        /// </summary>
        public Time SubtractHours(double granularityHours)
        {
            //convert hours to negative number for subtraction
            var negativeGranularityHours = System.Math.Abs(granularityHours) * (-1);

            //subtract time by hours
            var stdTime = _stdTime.AddHours(negativeGranularityHours);

            //create new instance of subtracted time
            var newTime = new Time(stdTime, _geoLocation);

            //return time to caller
            return newTime;
        }

        /// <summary>
        /// Local mean time (LMT) is the actual time based on the location of the place
        /// </summary>
        public DateTimeOffset GetLmtDateTimeOffset()
        {
            //get location longitude
            var longitudeDeg = _geoLocation.Longitude();

            //convert internal STD time to LMT
            var lmtTime = StdToLmt(_stdTime, longitudeDeg);

            //return value to caller
            return lmtTime;
        }

        /// <summary>
        /// Returns STD time in string HH:mm dd/MM/yyyy zzz
        /// </summary>
        public string GetStdDateTimeOffsetText()
        {
            //format time with formatting info
            //note: only explicit statement of format as below works
            var stdDateTimeString = _stdTime.ToString("HH:mm dd/MM/yyyy");
            var stdTimeZoneString = _stdTime.ToString("zzz"); //timezone separate so can clean date time

            //god knows why, in some time zones date comes with "." instead of "/" (despite above formatting)
            stdDateTimeString = stdDateTimeString.Replace('.', '/');

            //god knows why, in some time zones date comes with "-" instead of "/" (despite above formatting)
            stdDateTimeString = stdDateTimeString.Replace('-', '/');

            //recombine
            var final = $"{stdDateTimeString} {stdTimeZoneString}";

            //return formatted time
            return final;
        }

        /// <summary>
        /// Returns STD time in string HH:mm dd/MM/yyyy zzz
        /// </summary>
        public string GetStdDateTimeSecondsOffsetText()
        {
            //format time with formatting info
            //note: only explicit statement of format as below works
            var stdDateTimeString = _stdTime.ToString("HH:mm:ss dd/MM/yyyy");
            var stdTimeZoneString = _stdTime.ToString("zzz"); //timezone separate so can clean date time

            //god knows why, in some time zones date comes with "." instead of "/" (despite above formatting)
            stdDateTimeString = stdDateTimeString.Replace('.', '/');

            //god knows why, in some time zones date comes with "-" instead of "/" (despite above formatting)
            stdDateTimeString = stdDateTimeString.Replace('-', '/');

            //recombine
            var final = $"{stdDateTimeString} {stdTimeZoneString}";

            //return formatted time
            return final;
        }

        /// <summary>
        /// Returns STD time in string dd/MM/yyyy
        /// </summary>
        public readonly string StdDateMonthYearText => _stdTime.ToString("dd/MM/yyyy");

        /// <summary>
        /// returns year as 1995
        /// </summary>
        public readonly string StdYearText => _stdTime.ToString("yyyy");

        /// <summary>
        /// Returns STD time zone as text "+08:00"
        /// </summary>
        /// <returns></returns>
        public readonly string StdTimezoneText => _stdTime.ToString("zzz");

        /// <summary>
        /// STD Hour and Minute exp : 14:18
        /// </summary>
        public readonly string StdHourMinuteText => _stdTime.ToString("HH:mm");


        /// <summary>
        /// return internal std time
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset GetStdDateTimeOffset() => _stdTime;

        /// <summary>
        /// NOTE: custom time format is standardized here
        /// Example : 11:59 30/12/2018 +02:00
        /// </summary>
        public static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            //NOTE: custom time format is standardized here
            //Example : 11:59 30/12/2018 +02:00

            //declare return value
            var formatInfo = new DateTimeFormatInfo();

            //define format pattern
            formatInfo.FullDateTimePattern = DateTimeFormat;

            //return format info to caller
            return formatInfo;
        }

        /// <summary>
        /// Subtracts a Time value with current Time,
        /// to get the time interval in between.
        /// Note: Inputed time has to be older (smaller), else return value will be negative
        /// </summary>
        public TimeSpan Subtract(Time time)
        {
            //get difference
            var difference = _stdTime.Subtract(time._stdTime);

            return difference;
        }

        public string GetLmtDateTimeOffsetText() => this.GetLmtDateTimeOffset().ToString(Time.DateTimeFormat);


        /// <summary>
        /// Check if an inputed STD time string is valid,
        /// returns default time if not parseable
        /// </summary>
        public static bool TryParseStd(string stdDateTimeText, out DateTimeOffset parsed)
        {
            try
            {
                parsed = DateTimeOffset.ParseExact(stdDateTimeText, Time.DateTimeFormat, null);
                return true;
            }
            catch (Exception)
            {
                //failure for any reason, return false
                parsed = new DateTimeOffset();
                return false;
            }
        }


        #region CONVERTERS

        /// <summary>
        /// Note root element is "Time"
        /// </summary>
        public XElement ToXml()
        {
            var timeHolder = new XElement("Time");
            var timeString = this.GetStdDateTimeOffsetText();
            var timeValue = new XElement("StdTime", timeString);
            var location = this.GetGeoLocation().ToXml();

            timeHolder.Add(timeValue, location);

            return timeHolder;
        }

        public JObject ToJson()
        {
            var temp = new JObject();
            temp["StdTime"] = this.GetStdDateTimeOffsetText();
            temp["Location"] = this.GetGeoLocation().ToJson();

            //compile into an JSON array
            return temp;
        }

        /// <summary>
        /// The root element is expected to be name of Type
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// Note: Root element must be named Time
        /// </summary>
        public static Time FromXml(XElement timeXmlElement)
        {
            try
            {
                var timeString = timeXmlElement.Element("StdTime")?.Value ?? "00:00 01/01/2000 +08:00";

                //know issue to have "." instead of "/" for date separator, so change it here if at all
                timeString = timeString.Replace('.', '/');

                var locationXml = timeXmlElement.Element("Location");
                var geoLocation = GeoLocation.FromXml(locationXml);

                var parsedTime = new Time(timeString, geoLocation);

                return parsedTime;
            }
            catch (Exception e)
            {
                //log it
                LibLogger.Debug(e, $"Time.FromXml FAIL! : {timeXmlElement}");

                //return empty time to stop keep things running
                return Time.Empty;
            }
        }

        /// <summary>
        /// Parse list of XML directly
        /// </summary>
        public static List<Time> FromXml(IEnumerable<XElement> xmlList) => xmlList.Select(timeXml => Time.FromXml(timeXml)).ToList();

        public static Time FromJson(JToken timeJson)
        {
            try
            {
                var timeString = timeJson["StdTime"].Value<string>();// ?.Value ?? "00:00 01/01/2000 +08:00";

                //know issue to have "." instead of "/" for date separator, so change it here if at all
                timeString = timeString.Replace('.', '/');

                var locationJson = timeJson["Location"];
                GeoLocation geoLocation = GeoLocation.FromJson(locationJson);

                var parsedTime = new Time(timeString, geoLocation);

                return parsedTime;

            }
            catch (Exception e)
            {
                Console.WriteLine($"JSON PARSE FAIL:\n{timeJson}\n{e}");
                return Empty;
            }
        }


        /// <summary>
        /// Given a list of Time wrapped in json will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<Time> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<Time>(); }

            var returnList = new List<Time>();

            foreach (var personJson in personList)
            {
                returnList.Add(Time.FromJson(personJson));
            }

            return returnList;
        }

        /// <summary>
        /// Given Time instance in URL form will convert to instance
        /// EXP 1 :-> Location/Singapore/Time/23:59/31/12/2000/
        /// EXP 2 :-> Location/8.716,77.55/Time/07:30/20/07/1978/
        /// Offset auto set based on location & time
        /// </summary>
        public static Task<dynamic> FromUrl(string url)
        {

            //CACHE MECHANISM
            return CacheManager.GetCache(new CacheKey("Time.FromUrl", url), fromUrl);

            Task<dynamic> fromUrl()
            {
                try
                {
                    // INPUT -> "Location/Singapore/Time/23:59/31/12/2000/"
                    string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                    //parse time range from caller (possible to fail)
                    var parsedTime = Tools.ParseTime(
                        locationName: parts[1], //note skip "Location"
                        hhmmStr: parts[3], //note skip "Time"
                        dateStr: parts[4],
                        monthStr: parts[5],
                        yearStr: parts[6]).Result;

                    return Task.FromResult<dynamic>(parsedTime);

                }
                catch (Exception e)
                {
                    //TODO log to server
                    Console.WriteLine(e);
                    return Task.FromResult<dynamic>(Time.Empty);
                }

            }
        }

       

        /// <summary>
        /// Output TIME only for URL format
        /// time converted to the format used in OPEN API url
        /// /Location/London/Time/00:00/01/01/2011/+08:00
        /// </summary>
        public string ToUrl()
        {
            var stringWithSpace = this.GetGeoLocation().Name();
            var locationName = Tools.RemoveWhiteSpace(stringWithSpace);

            //reconstruct into URL pattern
            //00:00/22/05/2023/+08:00
            var returnVal = this.GetStdDateTimeOffsetText(); //date time with space

            //replace spacing between to slash, presto done!
            var formattedTime = returnVal.Replace(" ", "/");

            var finalUrl = $"/Location/{locationName}/Time/{formattedTime}";

            return finalUrl;
        }


        #endregion



        //█▀█ █░█ █▀▀ █▀█ █▀█ █ █▀▄ █▀▀ █▀
        //█▄█ ▀▄▀ ██▄ █▀▄ █▀▄ █ █▄▀ ██▄ ▄█

        public override bool Equals(object obj)
        {
            //if type is correct
            if (obj.GetType() == typeof(Time))
            {
                //hard cast inputed value to time
                Time inputTime = (Time)obj;

                //check equality with hash code
                return this.GetHashCode() == inputTime.GetHashCode();
            }

            //not correct type, return not equal
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = (int)_stdTime.Ticks;
            var hash2 = _geoLocation?.GetHashCode() ?? 0;

            return hash1 + hash2;
        }

        /// <summary>
        /// Returns STD time in string HH:mm dd/MM/yyyy zzz
        /// </summary>
        public override string ToString()
        {
            return GetStdDateTimeOffsetText();
        }



        //█▀█ █▀█ █▀▀ █▀█ ▄▀█ ▀█▀ █▀█ █▀█   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▄█ █▀▀ ██▄ █▀▄ █▀█ ░█░ █▄█ █▀▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        public static bool operator ==(Time left, Time right) => left.Equals(right);

        public static bool operator !=(Time left, Time right) => !(left == right);

        public static bool operator >(Time a, Time b) => a.GetStdDateTimeOffset() > b.GetStdDateTimeOffset();

        public static bool operator <(Time a, Time b) => a.GetStdDateTimeOffset() < b.GetStdDateTimeOffset();

        public static bool operator >=(Time a, Time b) => a.GetStdDateTimeOffset() >= b.GetStdDateTimeOffset();

        public static bool operator <=(Time a, Time b) => a.GetStdDateTimeOffset() <= b.GetStdDateTimeOffset();



        //█▀█ █▀█ █ █░█ ▄▀█ ▀█▀ █▀▀   █▀▄▀█ █▀▀ ▀█▀ █░█ █▀█ █▀▄ █▀
        //█▀▀ █▀▄ █ ▀▄▀ █▀█ ░█░ ██▄   █░▀░█ ██▄ ░█░ █▀█ █▄█ █▄▀ ▄█

        private static DateTimeOffset StdToLmt(DateTimeOffset stdDateTime, double longitudeDeg)
        {
            //NOTE: LMT = STD + LMT offset

            //get LMT offset
            var lmtOffset = GetLocalTimeOffset(longitudeDeg);

            //create LMT from offsetting STD time
            var lmtDateTime = stdDateTime.ToOffset(lmtOffset);

            //return lmt to caller
            return lmtDateTime;
        }

        /// <summary>
        /// Convert longitude to LMT offset
        /// input longitude range : -180 to 180 
        /// </summary>
        public static TimeSpan GetLocalTimeOffset(double longitudeDeg)
        {
            var failCount = 0;
            var failTryLimit = 3;


            try
            {
            TryAgain:
                //raise alarm if longitude is out of range
                var outOfRange = !(longitudeDeg >= -180 && longitudeDeg <= 180);
                if (outOfRange)
                {
                    if (failCount < failTryLimit)
                    {
                        var oldLongitude = longitudeDeg; //back up for logging

                        //instead of giving up, lets take a go at correcting it
                        //assume input is 48401 but should be 48.401, so divide 1000
                        longitudeDeg = longitudeDeg / 1000;

                        failCount++; //keep track so not fall into rabbit hole

                        LibLogger.Debug($"Longitude out of range : {oldLongitude} > Auto correct to : {longitudeDeg}"); //log it for debug research

                        goto TryAgain;
                    }

                    //if control reaches here than raise exception,
                    //control should not reach here under any good call condition
                    throw new Exception($"Longitude out of range : {longitudeDeg} > Auto correct failed!");
                }

                //calculate offset based on longitude
                var offsetToReturn = TimeSpan.FromHours(longitudeDeg / 15.0);

                //round off offset to full minutes (because datetime doesnt accept fractional minutes in offsets)
                var offsetMinutes = Math.Round(offsetToReturn.TotalMinutes);

                //get new offset from rounded minutes
                offsetToReturn = TimeSpan.FromMinutes(offsetMinutes);

                //return offset to caller
                return offsetToReturn;

            }
            catch (Exception e)
            {
                //let caller know failure silently
                LibLogger.Debug(e);

                //return empty LMT for controlled failure
                return TimeSpan.Zero;
            }

        }

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        public static Angle TimeToLongitude(TimeSpan time)
        {
            //degrees is equivelant to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }


        public static bool TryParse(string cellValue, out object o)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Given an LMT date time in string, will convert to full Time instance
        /// with STD support
        /// </summary>
        public static Time FromLMT(string lmtDateTime, GeoLocation geoLocation)
        {
            // Parse the LMT string into a DateTime instance
            if (!DateTime.TryParseExact(lmtDateTime, "HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lmtParsed))
            {
                throw new ArgumentException($"Invalid LMT format. Expected format is 'HH:mm MM/dd/yyyy'.", nameof(lmtDateTime));
            }

            //Create new instance of time 
            var returnVal = new Time(lmtParsed, geoLocation);

            return returnVal;
        }
    }


}