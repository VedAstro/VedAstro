using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Genso.Framework
{
    /// <summary>
    /// A collection of tools used for various straightforward computation
    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// Gets the SHA256 of the inputed string
        /// </summary>
        public static string StringToHash(string input)
        {
            //change text to stream
            var textStream = GenerateStreamFromString(input);

            //get hash of this combined text
            SHA256 mySHA256 = SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(textStream);

            //return hex of hash as string
            return getByteString(hashValue);

            // Display the byte array in a readable format.
            void PrintByteArray(byte[] array)
            {
                //for each byte in array
                for (int i = 0; i < array.Length; i++)
                {
                    //prints byte as 2 digit hexadecimal
                    Console.Write($"{array[i]:X2}");
                    //create a space every 4 bytes
                    if ((i % 4) == 3) Console.Write(" ");
                }
                Console.WriteLine();
            }

            // converts an array of bytes to hex in string
            string getByteString(byte[] array)
            {
                string returnString = "";

                //for each byte in array
                for (int i = 0; i < array.Length; i++)
                {
                    //add byte as 2 digit hexadecimal to string
                    returnString = returnString + $"{array[i]:X2}";
                }

                //return compiled string to caller
                return returnString;
            }

            MemoryStream GenerateStreamFromString(string value)
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
            }
        }

        /// <summary>
        /// Generates KEY1 from the inputed username & password
        /// The key1 generated is solely local, it does not mean key1 exist in server
        /// </summary>
        public static string GenerateKey1(string username, string password)
        {
            //combine username & password
            var combined = username + password;

            //get the has of the combined text
            var hash = StringToHash(combined);

            //return hash as KEY1
            return hash;

        }


        /// <summary>
        /// Gets now time in UTC +8:00
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset GetNow()
        {
            //create utc 8
            var utc8 = new TimeSpan(8, 0, 0);
            //get now time in utc 0
            var nowTime = DateTimeOffset.Now.ToUniversalTime();
            //convert time utc 0 to utc 8
            var utc8Time = nowTime.ToOffset(utc8);

            //return converted time to caller
            return utc8Time;
        }


    }
}
