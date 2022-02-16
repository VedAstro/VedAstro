using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Data structure to encapsulate an event before it's calculated
    /// In other words an object instance of the event data as stored in file
    /// </summary>
    public struct EventData : IHasName
    {
        /** FIELDS **/


        private readonly EventCalculator _eventCalculator;


        /** CTOR **/
        public EventData(EventName name, EventNature nature, string description, List<EventTag> eventTags, EventCalculator eventCalculator)
        {
            Name = name;
            Nature = nature;
            Description = description;
            Strength = "";//strength is only gotten after calculator is run
            EventTags = eventTags;
            _eventCalculator = eventCalculator;
        }


        /** PROPERTIES **/
        //mainly created for access from WPF binding
        public EventName Name { get; }
        public string FormattedName => Format.FormatName(this);
        public EventNature Nature { get; }
        public string Description { get; }
        public string Strength { get; set; }
        public List<EventTag> EventTags { get; }



        /** PUBLIC METHODS **/
        public bool IsEventOccuring(Time time, Person person)
        {
            //calculate prediction
            var prediction = _eventCalculator(time, person);

            //is prediction occuring
            bool isEventOccuring = prediction.Occuring;

            //store prediction strength for later
            Strength = prediction.Strength;

            //let caller know if occuring
            return isEventOccuring;
        }

        public EventName GetName() => Name;

        public EventNature GetNature() => Nature;

        public string GetDescription() => Description;
        public string GetStrength() => Strength;
    

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
            var hash1 = Name.GetHashCode();
            var hash2 = Nature.GetHashCode();
            var hash3 = Description.GetHashCode();

            return hash1 + hash2 + hash3;
        }

        public override string ToString()
        {
            return $"{Name} - {Nature} - {Description}";
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