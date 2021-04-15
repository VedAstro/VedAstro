using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Data structure to encapsulat an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct EventData : IHasName
    {
        /** FIELDS **/

        private readonly int _id;

        private readonly EventCalculator _eventCalculator;


        /** CTOR **/
        public EventData(int id, EventName name, EventNature nature, string description, List<EventTag> eventTags, EventCalculator eventCalculator)
        {
            Name = name;
            Nature = nature;
            Description = description;
            EventTags = eventTags;
            _eventCalculator = eventCalculator;
            _id = id;
        }


        /** PROPERTIES **/
        //mainly created for access from WPF binding
        public EventName Name { get; }
        public string FormattedName => Format.FormatName(this);
        public EventNature Nature { get; }
        public string Description { get; }
        public List<EventTag> EventTags { get; }



        /** PUBLIC METHODS **/
        public bool IsEventOccuring(Time time, Person person)
        {
            //call event calculator to check if event is occuring
            bool isEventOccuring = _eventCalculator(time, person);

            return isEventOccuring;
        }

        public EventName GetName() => Name;

        public EventNature GetNature() => Nature;

        public string GetDescription() => Description;

        public List<EventTag> GetEventTags() => EventTags;



        /** METHOD OVERRIDES **/
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
            var hash2 = Name.GetHashCode();
            var hash3 = Nature.GetHashCode();
            var hash4 = Description.GetHashCode();

            return hash1 + hash2 + hash3 + hash4;
        }

        public override string ToString()
        {
            return $"{_id} - {Name} - {Nature} - {Description}";
        }

        public static bool operator ==(EventData left, EventData right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EventData left, EventData right)
        {
            return !(left == right);
        }

    }
}