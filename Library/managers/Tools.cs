using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// A collection of general functions that don't have a home yet, so they live here for now.
    /// You're allowed to move them somewhere you see fit, not copy, move!
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Converts xml element instance to string properly
        /// </summary>
        public static string XmlToString(XElement xml)
        {
            //remove all formatting, for clean xml as string
            return xml.ToString(SaveOptions.DisableFormatting);
        }


        /// <summary>
        /// Converts any type to XML
        /// Will use Type's own ToXml() converter if available
        /// Note, used to transfer data via internet Client to API Server
        /// Example:
        /// <TypeName>
        ///     DataValue
        /// </TypeName>
        /// </summary>
        public static XElement AnyTypeToXml<T>(T value)
        {
            //check if type has own ToXml method
            //use the Type's own converter if available
            if (value is IToXml hasToXml)
            {
                var betterXml = hasToXml.ToXml();
                return betterXml;
            }

            //gets enum value as string to place inside XML
            var enumValueStr = value.ToString();

            //get the name of the Enum
            //Note: This is the name that will be used
            //later to instantiate the class from string
            var typeName = typeof(T).FullName;

            return new XElement(typeName, enumValueStr);
        }

        ///// <summary>
        ///// Converts only Enums in XML as form to it's instance
        ///// Note, used to transfer data via internet Client to API Server
        ///// </summary>
        //public static T XmlToEnum<T>(XElement xml) where T : struct, Enum
        //{
        //    //get the name of the Enum
        //    var typeName = nameof(T);

        //    //make sure the XML enclosing type has the same name
        //    if (xml.Name == typeName)
        //    {
        //        //get the value and parse it
        //        var rawVal = xml.Value;
        //        //TODO CLEAN UP
        //        var x = Enum.Parse<T>(rawVal);
        //        var y = (T)Enum.Parse(typeof(T), rawVal);
        //        return x;
        //    }

        //    //raise error since not XML type and Input type mismatch
        //    throw new Exception($"Can't parse XML to {typeName}");
        //}

        /// <summary>
        /// - Type is a value typ
        /// - Enum
        /// </summary>
        public static dynamic XmlToAnyType<T>(XElement xml)// where T : //IToXml, new()
        {
            //get the name of the Enum
            var typeNameFullName = typeof(T).FullName;
            var typeNameShortName = typeof(T).FullName;

            Console.WriteLine(xml.ToString());
            //type name inside XML
            var xmlElementName = xml?.Name;

            //get the value for parsing later
            var rawVal = xml.Value;


            //make sure the XML enclosing type has the same name
            //check both full class name, and short class name
            var isSameName = xmlElementName == typeNameFullName || xmlElementName == typeof(T).GetShortTypeName();

            //if not same name raise error
            if (!isSameName) { throw new Exception($"Can't parse XML {xmlElementName} to {typeNameFullName}"); }

            //implements ToXml()
            var typeImplementsToXml = typeof(T).GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IToXml));

            //type has owm ToXml method
            if (typeImplementsToXml)
            {
                dynamic inputTypeInstance = GetInstance(typeof(T).FullName);

                return inputTypeInstance.FromXml(xml);

            }

            //if type is an Enum process differently
            if (typeof(T).IsEnum)
            {
                var parsedEnum = (T)Enum.Parse(typeof(T), rawVal);

                return parsedEnum;
            }

            //else it is a value type
            if (typeof(T) == typeof(string)) { return rawVal; }
            if (typeof(T) == typeof(double)) { return Double.Parse(rawVal); }
            if (typeof(T) == typeof(int)) { return int.Parse(rawVal); }

            //raise error since converter not implemented
            throw new NotImplementedException($"XML converter for {typeNameFullName}, not implemented!");
        }

        /// <summary>
        /// Gets only the name of the Class, without assembly
        /// </summary>
        public static string GetShortTypeName(this Type type)
        {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                .Select(t => t.GetShortTypeName())));
            sb.Append(">");
            return sb.ToString();
        }

        public static bool Implements<I>(this Type type, I @interface) where I : class
        {
            if (((@interface as Type) == null) || !(@interface as Type).IsInterface)
                throw new ArgumentException("Only interfaces can be 'implemented'.");

            return (@interface as Type).IsAssignableFrom(type);
        }

        /// <summary>
        /// For converting value types, String, Double, etc.
        /// </summary>
        //public static dynamic XmlToValueType<T>(XElement xml) 
        //{
        //    //get the name of the Enum
        //    var typeName = nameof(T);


        //    //raise error since not XML type and Input type mismatch
        //    throw new Exception($"Can't parse XML to {typeName}");
        //}


        /// <summary>
        /// Gets an instance of Class from string name
        /// </summary>
        public static object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }


        /// <summary>
        /// Gets a list of tags in string form & changes it a structed list of tags
        /// </summary>
        public static List<EventTag> StringToEventTagList(string tagString)
        {
            //create a place to store the parsed tags
            var returnTags = new List<EventTag>();

            //split the string by comma "," (tag seperator)
            var splittedRawTags = tagString.Split(',');

            //parse each raw tag
            foreach (var rawTag in splittedRawTags)
            {
                //parse
                var result = Enum.TryParse(rawTag, out EventTag eventTag);
                //raise error if could not parse
                if (!result) throw new Exception("Event tag not found!");

                //add the parsed tag to the return list
                returnTags.Add(eventTag);
            }

            return returnTags;
        }


        /// <summary>
        /// Converts days to hours
        /// </summary>
        /// <returns></returns>
        public static double DaysToHours(double days) => days * 24.0;

        public static double MinutesToHours(double minutes) => minutes / 60.0;

        public static double MinutesToYears(double minutes) => minutes / 525600.0;

        public static double MinutesToDays(double minutes) => minutes / 1440.0;

        /// <summary>
        /// Given a date it will count the days to the end of that year
        /// </summary>
        public static double GetDaysToNextYear(Time getBirthDateTime)
        {
            //get start of next year
            var standardTime = getBirthDateTime.GetStdDateTimeOffset();
            var nextYear = standardTime.Year + 1;
            var startOfNextYear = new DateTimeOffset(nextYear, 1, 1, 0, 0, 0, 0, standardTime.Offset);

            //calculate difference of days between 2 dates
            var diffDays = (startOfNextYear - standardTime).TotalDays;

            return diffDays;
        }
    }
}
