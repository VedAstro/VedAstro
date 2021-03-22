using System;

namespace Genso.Astrology.Library
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    /// <summary>
    /// Class that attaches attributes to event calculator methods
    /// </summary>
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