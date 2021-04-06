using System;
using System.Globalization;

namespace Genso.Astrology.Library
{
    //Time : Instance of Time

    public interface ITime
    {
        //string ToStdTimeFormat();
        //string ToLmtTimeFormat();//11:59 30/12/2018 +02:00
        Time AddHours(double granularityHours);
        Time SubtractHours(double granularityHours);
        DateTimeOffset GetLmtDateTimeOffset();
        string GetStdDateTimeOffsetText();
        DateTimeOffset GetStdDateTimeOffset();
    }


    /// <summary>
    /// A single representation of time, contains both standard time (STD) & local mean time (LMT)
    /// LMT is actual time based on the location of the place
    /// IMMUTABLE CLASS
    /// </summary>
    [Serializable()]
    public struct Time : ITime
    {
        //FIELDS
        private readonly DateTimeOffset _stdTime;
        private readonly GeoLocation _geoLocation;

        //CONSTANT FIELDS
        private static readonly DateTimeFormatInfo FormatInfo = GetDateTimeFormatInfo();
        private const string DateTimeFormat = "HH:mm dd/MM/yyyy zzz"; //define date time format
        private const string DateTimeFormat2 = "HH:mm:ss dd/MM/yyyy zzz"; //define date time format include seconds for unit testing



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
        /// Creates a new instance of time from LMT
        public Time(DateTime lmtDateTime, TimeSpan stdOffset, GeoLocation geoLocation)
        {
            //get lmt time
            var lmtTime = new DateTimeOffset(lmtDateTime, GetLocalTimeOffset(geoLocation.GetLongitude()));

            //convert lmt to std & store it
            _stdTime = lmtTime.ToOffset(stdOffset);

            //store geo location for later use
            _geoLocation = geoLocation;

        }



        //PUBLIC METHODS

        public Time AddHours(double granularityHours)
        {
            //increment time by hours
            var stdTime = _stdTime.AddHours(granularityHours);

            //create new instance of incremented time
            var newTime = new Time(stdTime, _geoLocation);

            //return time to caller
            return newTime;

        }

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
            var longitudeDeg = _geoLocation.GetLongitude();

            //convert internal STD time to LMT
            var lmtTime = StdToLmt(_stdTime, longitudeDeg);

            //return value to caller
            return lmtTime;
        }

        public string GetStdDateTimeOffsetText()
        {
            //format time with formatting info
            var stdTimeString = _stdTime.ToString(FormatInfo);

            //return formatted time
            return stdTimeString;
        }

        public DateTimeOffset GetStdDateTimeOffset()
        {
            //return internal std time
            return _stdTime;
        }

        /// <summary>
        /// Formating used for parsing date time,  HH:mm dd/MM/yyyy zzz 
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeFormat() => DateTimeFormat;

        /// <summary>
        /// Formating used for parsing date time,  HH:mm:ss dd/MM/yyyy zzz (includes seconds)
        /// Note: Created for acurate unit testing 
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeFormat2() => DateTimeFormat2;

        public static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            //NOTE: custom time format is standardized here
            //Example : 11:59 30/12/2018 +02:00

            //declare return value
            var formatInfo = new DateTimeFormatInfo();

            //define format pattern
            formatInfo.FullDateTimePattern = GetDateTimeFormat();


            //return format info to caller
            return formatInfo;
        }

        /// <summary>
        /// Subtracts a Time value with current Time,
        /// to get the time interval in between.
        /// </summary>
        public TimeSpan Subtract(Time time)
        {
            //get difference
            var difference = _stdTime.Subtract(time._stdTime);

            return difference;
        }
        /// <summary>
        /// Get the geo location at place of time
        /// </summary>
        public GeoLocation GetGeoLocation()
        {
            return _geoLocation;
        }




        //PRIVATE METHODS
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

        private static TimeSpan GetLocalTimeOffset(double longitudeDeg)
        {
            //declare +0:00 offset at first
            TimeSpan offsetToReturn = TimeSpan.Zero;

            //calculate offset based on longitude
            if (longitudeDeg == 0)
            {
                offsetToReturn = TimeSpan.FromHours(longitudeDeg / 15.0);
            }
            else if (longitudeDeg < 0)
            {
                offsetToReturn = TimeSpan.FromHours(-(longitudeDeg / 15.0));
            }
            else if (longitudeDeg > 0)
            {
                offsetToReturn = TimeSpan.FromHours(longitudeDeg / 15.0);
            }

            //round off offset to full minutes (because datetime doesnt accept fractional minutes in offsets)
            var offsetMinutes = Math.Round(offsetToReturn.TotalMinutes);

            //get new offset from rounded minutes
            offsetToReturn = TimeSpan.FromMinutes(offsetMinutes);

            //return offset to caller
            return offsetToReturn;
        }

        private string GetLmtDateTimeOffsetText()
        {
            //convert internal STD time to LMT
            var lmtTime = StdToLmt(_stdTime, _geoLocation.GetLongitude());

            //create LMT time string based on formatting info
            var lmtTimeString = lmtTime.ToString(FormatInfo);

            //return time string caller
            return lmtTimeString;
        }

        /// <summary>
        /// Converts time back to longitude, it is the reverse of GetLocalTimeOffset in Time
        /// Exp :  5h. 10m. 20s. E. Long. to 77° 35' E. Long
        /// </summary>
        private static Angle TimeToLongitude(TimeSpan time)
        {
            //degrees is equivelant to hours
            var totalDegrees = time.TotalHours * 15;

            return Angle.FromDegrees(totalDegrees);
        }


        //OVERRIDES
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
            else { return false; }

        }

        /// <summary>
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = _stdTime.GetHashCode();
            var hash2 = _geoLocation.GetHashCode();

            return hash1 + hash2;
        }

        public override string ToString()
        {
            return GetStdDateTimeOffsetText();
        }



        //OPERATOR METHODS
        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !(left == right);
        }


    }
}