using System;
using System.Collections;
using System.Collections.Generic;


namespace Genso.Astrology.Library
{
    public class MuhurthaTimePeriod
    {
        //FIELDS
        private readonly Time _startTime;
        private readonly Time _endTime;
        private readonly Person _person;
        private readonly List<Event> _eventList;

        //CTOR
        public MuhurthaTimePeriod(Time startTime, Time endTime, Person person, List<Event> eventList)
        {
            //store input values as fields
            _startTime = startTime;
            _endTime = endTime;
            _person = person;
            _eventList = eventList;
        }

        public List<Event> GetEventList()
        {
            return _eventList;
        }



        //OVERRIDE METHODS
        public override string ToString()
        {
            return $"{_person} - {_startTime} - {_endTime}";
        }

        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(MuhurthaTimePeriod))
            {
                //cast to type
                var parsedValue = (MuhurthaTimePeriod)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //return false if value is null
                return false;
            }
        }

        /// <summary>
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = _startTime.GetHashCode();
            var hash2 = _endTime.GetHashCode();
            var hash3 = _person.GetHashCode();

            return hash1 + hash2 + hash3;
        }

    }
}