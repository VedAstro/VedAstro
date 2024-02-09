using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{

    public interface IAngle
    {
        long Degrees { get; }
        long Minutes { get; }
        long Seconds { get; }
        double TotalDegrees { get; }
        double TotalMinutes { get; }
    }

    [Serializable()]
    public class Angle : IAngle, IToJson, IFromUrl
    {
        //CONST FIELDS
        private const long SecondsPerDegree = SecondsPerMinute * 60; //3600
        private const long SecondsPerMinute = 60;
        private const double MinutesPerSecond = 1.0 / SecondsPerMinute; //0.01666666
        private const double DegreesPerSecond = 1.0 / SecondsPerDegree;

        public static readonly Angle Zero = new Angle(0, 0, 0);
        public static readonly Angle Degrees45 = new Angle(45, 0, 0);
        public static readonly Angle Degrees90 = new Angle(90, 0, 0);
        public static readonly Angle Degrees180 = new Angle(180, 0, 0);
        public static readonly Angle Degrees360 = new Angle(360, 0, 0);


        //DATA FIELD
        private readonly long _seconds;



        //CTOR
        public Angle(double degrees = 0, double minutes = 0, long seconds = 0)
        {
            _seconds = DegreesToSeconds(degrees);
            _seconds += MinutesToSeconds(minutes);
            _seconds += seconds;
        }

        /// <summary>
        /// NOTE: decimal seconds is chopped off
        /// used for easy test case code creation
        /// </summary>
        public Angle(double degrees = 0, double minutes = 0, double seconds = 0)
        {
            _seconds = DegreesToSeconds(degrees);
            _seconds += MinutesToSeconds(minutes);
            _seconds += (long)seconds;
        }



        //PROPERTIES
        public long Degrees => _seconds / SecondsPerDegree;
        public long Minutes => (_seconds % SecondsPerDegree) / SecondsPerMinute;
        public long Seconds => (_seconds % SecondsPerDegree) % SecondsPerMinute;
        public double TotalDegrees => SecondsToDegrees(_seconds);
        public double TotalMinutes => SecondsToMinutes(_seconds);

        /// <summary>
        /// Total degrees rounded nicely for human eyes, used to show in site
        /// </summary>
        public double Rounded => Math.Round(TotalDegrees, 4);


        //METHODS

        private static long DegreesToSeconds(double degrees)
        {
            return (long)(degrees * SecondsPerDegree);
        }

        private static long MinutesToSeconds(double minutes)
        {
            return (long)(minutes * SecondsPerMinute);
        }

        private static double SecondsToDegrees(long seconds)
        {
            return seconds * DegreesPerSecond;
        }

        private static double SecondsToMinutes(long seconds)
        {
            return seconds * MinutesPerSecond; ;
        }

        public static Angle FromDegrees(double value)
        {
            return new Angle(value, 0, 0);
        }

        /// <summary>
        /// Convert from degrees and minutes like Long. 77' 34"E.
        /// </summary>
        public static double ConvertDegreeMinuteToTotalDegrees(double degree, double minute) => new Angle(degree, minute, 0).TotalDegrees;

        /// <summary>
        /// If total degrees is more than 360°,
        /// minus 360° from total degrees.
        /// returns new modified instance
        /// </summary>
        public Angle Expunge360()
        {
            //if total degrees is more 360
            if (this.TotalDegrees > 360)
            {
                //minus 360 from total
                var expungedValue = this - Angle.Degrees360;

                //return expunged value
                return expungedValue;
            }
            //else return original value
            else
            {
                return this;
            }

        }

        /// <summary>
        /// Divide total degrees by integer.
        /// </summary>
        public Angle Divide(double divisor)
        {
            //get division result
            var result = this.TotalDegrees / divisor;

            //return division result as degrees
            return Angle.FromDegrees(result);
        }

        /// <summary>
        /// Gets positive value of difference from input value,
        /// like subtract but always returns positive value.
        /// Note:
        /// Gets raw difference! Does not account for planet 0-360° longitudes
        /// </summary>
        public Angle GetDifference(Angle value)
        {
            //subtract like normal
            var subtractedValue = this - value;

            //remove negative sign if there
            var differenceInDegrees = Math.Abs(subtractedValue.TotalDegrees);

            //convert degrees to angle
            var differenceInAngle = Angle.FromDegrees(differenceInDegrees);

            return differenceInAngle;
        }



        //OPERATOR OVERLOADS
        public static Angle operator +(Angle a1, Angle a2)
        {

            return new Angle(seconds: a1._seconds + a2._seconds);
        }
        public static Angle operator -(Angle a1, Angle a2)
        {
            return new Angle(seconds: a1._seconds - a2._seconds);
        }
        public static bool operator ==(Angle left, Angle right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(Angle left, Angle right)
        {
            return !(left == right);
        }
        public static bool operator >(Angle left, Angle right)
        {
            //return
            return (left._seconds > right._seconds);
        }
        public static bool operator <(Angle left, Angle right)
        {
            var leftSeconds = left?._seconds ?? 0;
            var rightSeconds = right?._seconds ?? 0;
            return leftSeconds < rightSeconds;
        }
        public static bool operator >=(Angle left, Angle right)
        {
            var leftSeconds = left?._seconds ?? 0;
            var rightSeconds = right?._seconds ?? 0;
            return leftSeconds >= rightSeconds;
        }
        public static bool operator <=(Angle left, Angle right)
        {
            return (left._seconds <= right._seconds);
        }



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {
            //if null auto false
            if (value == null) { return false; }

            if (value.GetType() == typeof(Angle))
            {
                //cast to angle
                var valueAngle = (Angle)value;

                //Check equality
                bool returnValue = (this.GetHashCode() == valueAngle.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }
        }

        public override string ToString()
        {
            //prepare string
            var degreeAll = this.TotalDegrees.ToString();//Only degrees is in negative

            //return string to caller
            return $"{degreeAll}";
        }

        /// <summary>
        /// Given Angle instance in URL form will convert to instance
        /// /Angle/23.555/
        /// </summary>
        public static async Task<dynamic> FromUrl(string url)
        {
            // INPUT -> "/Person/JesusHChrist0000/"
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var parsedAngle = Angle.FromDegrees(double.Parse(parts[1]));

            return parsedAngle;
        }

        public JObject ToJson()
        {
            var returnVal = new JObject();
            //classical notation (DMS)
            returnVal["DegreeMinuteSecond"] = this.DegreesMinutesSecondsText;
            returnVal["TotalDegrees"] = this.TotalDegrees.ToString();//Only degrees is in negative

            return returnVal;

        }

        /// <summary>
        /// Return angle as Degree Minute Seconds --> 23° 14' 12" as text
        /// Only degrees is in negative
        /// </summary>
        public string DegreesMinutesSecondsText => $"{this.Degrees}° {Math.Abs(this.Minutes)}' {Math.Abs(this.Seconds)}";

        /// <summary>
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = _seconds.GetHashCode();

            return hash1;
        }
    }
}
