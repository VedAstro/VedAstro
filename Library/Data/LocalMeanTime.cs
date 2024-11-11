using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data class to hold LMT data before conversion to STD
    /// Sample input : "05:45 03/05/1932" &  longitude 75°
    /// </summary>
    public class LocalMeanTime : IFromUrl
    {
        /// <summary>
        /// The number of pieces the URL version of this instance needs to be cut for processing
        /// EXP -> Time/05:45/03/05/1932/Longitude/75 == 7 PIECES
        /// </summary>
        public static int OpenAPILength = 7;

        /// <summary>
        /// Gets or sets the date of the Local Mean Time
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Initializes a new instance of the LocalMeanTime class
        /// </summary>
        /// <param name="date">The date of the Local Mean Time</param>
        /// <param name="longitude">The longitude of the location</param>
        public LocalMeanTime(DateTime date, double longitude)
        {
            Date = date;
            Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new instance of the LocalMeanTime class
        /// </summary>
        /// <param name="timeString">The time string in the format "HH:mm dd/MM/yyyy"</param>
        /// <param name="longitude">The longitude of the location</param>
        public LocalMeanTime(string lmtTimeString, double longitude)
        {
            // Parse the LMT string into a DateTime instance
            if (!DateTime.TryParseExact(lmtTimeString, "HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lmtParsed))
            {
                throw new ArgumentException($"Invalid LMT format. Expected format is 'HH:mm MM/dd/yyyy'.", nameof(lmtTimeString));
            }
            Date = lmtParsed;

            Longitude = longitude;
        }

        /// <summary>
        /// Returns a string representation of the LocalMeanTime object
        /// </summary>
        /// <returns>A string in the format "HH:mm dd/MM/yyyy Longitude: xx°"</returns>
        public override string ToString()
        {
            return $"{Date.ToString("HH:mm dd/MM/yyyy")} Longitude: {Longitude}°";
        }


        /// <summary>
        /// Given LMT instance in URL form will convert to instance
        /// EXP 1 :-> Time/05:45/03/05/1932/Longitude/75
        /// </summary>
        public static Task<dynamic> FromUrl(string url)
        {
            // INPUT -> "Time/05:45/03/05/1932/Longitude/75"
            // Check if the URL has the correct number of parts
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != OpenAPILength)
            {
                throw new ArgumentException($"Invalid URL format. Expected {OpenAPILength} parts.", nameof(url));
            }

            // Check if the URL starts with "Time" and contains "Longitude"
            if (parts[0] != "Time" || parts[parts.Length - 2] != "Longitude")
            {
                throw new ArgumentException($"Invalid URL format. Expected 'Time' and 'Longitude' parts.", nameof(url));
            }

            // Extract the date and time parts
            string time = parts[1];
            string day = parts[2];
            string month = parts[3];
            string year = parts[4];

            // Combine the date and time parts into a single string
            string dateTimeString = $"{time} {day}/{month}/{year}";


            // Extract the longitude part
            if (!double.TryParse(parts[parts.Length - 1], out double longitude))
            {
                throw new ArgumentException($"Invalid longitude format. Expected a double value.", nameof(url));
            }

            // Create a new LocalMeanTime instance
            LocalMeanTime localMeanTime = new LocalMeanTime(dateTimeString, longitude);

            // Return the LocalMeanTime instance as a Task
            return Task.FromResult<dynamic>(localMeanTime);

        }

    }

}
