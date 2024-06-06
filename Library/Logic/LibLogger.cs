using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Simple logger to log events, errors directly in side library
    /// WebLogger & ApiLogger can't access the level of detail this logger gets
    /// </summary>
    public static class LibLogger
    {
        

        /// <summary>
        /// Makes a debug log entry
        /// </summary>
        public static async Task Debug(string message = "")
        {
            
        }

        public static void Debug(Exception message, string s)
        {
            
        }


        public static void Debug(Exception message)
        {
            
        }
    }
}
