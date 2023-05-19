using System.Web;

namespace VedAstro.Library
{
    /// <summary>
    /// increase efficiency by using struct
    /// note : the lack of GetDurationMinutes compared to the other Event class
    /// struct shaves a few seconds vs classes
    /// </summary>
    public class EventSlice 
    {
        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static EventSlice Empty = new EventSlice(EventName.Empty, EventNature.Empty, "", Time.Empty, false);

        //FIELDS
        private readonly string _description;


        //CTOR
        public EventSlice(EventName name, EventNature nature, string description, Time time, bool isOccuring)
        {
            //initialize fields
            Name = name;
            Nature = nature;
            _description = HttpUtility.HtmlEncode(description); //HTML character safe
            Time = time;
            IsOccuring = isOccuring;
        }


        //PROPERTIES
        //Note: Created mainly for ease of use with WPF binding
        public EventName Name { get; }

        public string FormattedName => Format.FormatName(this);
        public string Description => HttpUtility.HtmlDecode(_description);
        public EventNature Nature { get; }

        public Time Time { get; }
        public bool IsOccuring { get; init; }


        //PRIVATE METHODS


        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(EventSlice))
            {
                //cast to type
                var parsedValue = (EventSlice)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }


        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Name.GetHashCode();
            var hash2 = Tools.GetStringHashCode(_description);
            var hash3 = Nature.GetHashCode();
            var hash4 = Time.GetHashCode();

            return hash1 + hash2 + hash3 + hash4;
        }

        public override string ToString()
        {
            return $"{Name} - {Nature} - {Time}";
        }

        

        /// <summary>
        /// Checks if this event occurred at the inputed time
        /// </summary>
        public bool IsOccurredAtTime(Time inputTime)
        {
            //input time is after (more than) event start time
            var isEqual = inputTime == this.Time;

            return isEqual;

        }
    }
}
