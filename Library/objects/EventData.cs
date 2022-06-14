using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
        public EventNature Nature { get; private set; }
        public string Description { get; }
        public string Strength { get; set; }
        public List<EventTag> EventTags { get; }



        /** PUBLIC METHODS **/
        public bool IsEventOccuring(Time time, Person person)
        {
            //do calculation for this event to get prediction data
            var predictionData = _eventCalculator(time, person);

            //extract the data out and store it for later use
            //is prediction occuring
            bool isEventOccuring = predictionData.Occuring;

            //prediction strength
            Strength = predictionData.Strength;

            //override even nature from xml if specified
            Nature = predictionData.NatureOverride == EventNature.Empty ? Nature : predictionData.NatureOverride;

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
            var hash3 = Tools.GetHashCode(Description);

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

        public static EventData ToXml(XElement eventData)
        {
            //extract the individual data out & convert it to the correct type
            var nameString = eventData.Element("Name").Value;
            Enum.TryParse(nameString, out EventName name);
            var natureString = eventData.Element("Nature").Value;
            Enum.TryParse(natureString, out EventNature nature);
            var description = eventData.Element("Description").Value;
            var tagString = eventData.Element("Tag").Value;
            var tagList = Tools.StringToEventTagList(tagString);
            var calculatorMethod = EventManager.GetEventCalculatorMethod(name);

            //place the data into an event data structure
            var eventX = new EventData(name, nature, description, tagList, calculatorMethod);

            return eventX;
        }
    }
}