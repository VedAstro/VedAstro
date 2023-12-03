using System;

namespace VedAstro.Library
{

    /// <summary>
    /// If specified above a field, it is for Pro users and can be used to differentiate in GUI
    /// It means is is an advanced option, made for use in Ayanamsa
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AdvancedOptionAttribute : Attribute
    {
    }
}
