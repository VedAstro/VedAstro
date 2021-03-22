using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library
{
    public struct EventData : IHasName
    {
        //FIELDS
        private readonly int _id;
        private readonly EventName _name;
        private readonly EventNature _nature;
        private readonly string _description;
        private readonly List<EventTag> _eventTags;
        private readonly EventCalculator _eventCalculator;


        //CTOR
        public EventData(int id, EventName name, EventNature nature, string description, List<EventTag> eventTags, EventCalculator eventCalculator)
        {
            _name = name;
            _nature = nature;
            _description = description;
            _eventTags = eventTags;
            _eventCalculator = eventCalculator;
            _id = id;
        }



        //METHODS
        public bool IsEventOccuring(Time time, Person person)
        {
            //call event calculator to check if event is occuring
            bool isEventOccuring = _eventCalculator(time, person);

            return isEventOccuring;
        }

        public EventName GetName() => _name;

        public EventNature GetNature() => _nature;

        public string GetDescription() => _description;

        public List<EventTag> GetEventTags() => _eventTags;



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(EventData))
            {
                //cast to type
                var parsedValue = (EventData)value;

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
            var hash1 = _id.GetHashCode();
            var hash2 = _name.GetHashCode();
            var hash3 = _nature.GetHashCode();
            var hash4 = _description.GetHashCode();

            return hash1 + hash2 + hash3 + hash4;
        }

        public override string ToString()
        {
            return $"{_id} - {_name} - {_nature} - {_description}";
        }


    }
}