using System;
using System.Collections.Generic;
using System.Reflection;


namespace VedAstro.Library
{

    /// <summary>
    /// OBS SECURITY PROTOCOL
    /// </summary>
    public static partial class Secrets
    {
        /// <summary>
        /// OBS SECURITY PROTOCOL
        /// </summary>
        public static string Get(string key)
        {
            //keys are expected to be in private mode, accessed only via this method
            var field = typeof(Secrets).GetField(key, BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                return (string)field.GetValue(null);
            }

            Console.WriteLine($"The key --> '{key}' is missing sweetheart! Contact us for a testing Key --> vedastro.org/Contact");
            // give nice message to caller if missing 
            //throw new Exception($"The key --> '{key}' is missing sweetheart! Contact us for a testing Key --> vedastro.org/Contact");
            return "";
        }
    }
}
