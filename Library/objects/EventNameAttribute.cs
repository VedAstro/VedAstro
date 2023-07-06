using System;

namespace VedAstro.Library
{

    /// <summary>
    /// Class that attaches attributes to event calculator methods
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class EventCalculatorAttribute : Attribute
    {
        public EventName EventName { get; set; }

        public EventCalculatorAttribute(EventName eventName)
        {
            EventName = eventName;
        }

    }

    /// <summary>
    /// Class that attaches attributes to horoscope calculator methods
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class HoroscopeCalculatorAttribute : Attribute
    {
        public HoroscopeName HoroscopeName { get; set; }

        public HoroscopeCalculatorAttribute(HoroscopeName horoscopeName)
        {
            HoroscopeName = horoscopeName;
        }

    }

}