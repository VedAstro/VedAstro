
using System;
using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library
{
    public class Event : IHasName
    {
        //FIELDS
        private readonly EventName _name;
        private readonly string _description;
        private readonly EventNature _nature;
        private readonly Time _startTime;
        private readonly Time _endTime;


        //CTOR
        public Event(EventName name, EventNature nature, string description, Time startTime, Time endTime)
        {
            //initialize fields
            _name = name;
            _nature = nature;
            _description = description;
            _startTime = startTime;
            _endTime = endTime;
        }


        //PUBLIC METHODS
        public EventName GetName()
        {
            return _name;
        }

        public EventNature GetNature()
        {
            return _nature;
        }

        public Time GetStartTime()
        {
            return _startTime;
        }

        public string GetDescription()
        {
            return _description;
        }

        public Time GetEndTime()
        {
            return _endTime;
        }


        //PRIVATE METHODS
        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }


        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(Event))
            {
                //cast to type
                var parsedValue = (Event)value;

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
            var hash1 = _name.GetHashCode();
            var hash2 = _description.GetHashCode();
            var hash3 = _nature.GetHashCode();
            var hash4 = _startTime.GetHashCode();
            var hash5 = _endTime.GetHashCode();

            return hash1 + hash2 + hash3 + hash4 + hash5;
        }

        public override string ToString()
        {
            return $"{GetName()} - {_nature} - {_startTime} - {_endTime} - {GetDurationMinutes()}";
        }


        /// <summary>
        /// Gets the duration of the event from start to end time
        /// </summary>
        public int GetDurationMinutes()
        {
            var difference = GetEndTime().GetStdDateTimeOffset() - GetStartTime().GetStdDateTimeOffset();

            return (int)difference.TotalMinutes;
        }
    }
}