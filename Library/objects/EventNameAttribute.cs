using System;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Class that attaches attributes to event calculator methods
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class EventCalculatorAttribute : Attribute
    {
        private readonly EventName _eventName;

        public EventCalculatorAttribute(EventName eventName)
        {
            _eventName = eventName;
        }

        public EventName GetEventName()
        {
            return _eventName;
        }

    }

}