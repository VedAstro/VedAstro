namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to enclose a prediction.
    /// Prediction = did it occur & the strength 
    /// </summary>
    public class Prediction
    {
        /// <summary>
        /// Indication if prediction is occurring
        /// </summary>
        public bool Occuring { get; set; }
        public string Strength { get; set; }

        /// <summary>
        /// Defaults set here
        /// </summary>
        public Prediction()
        {
            Strength = "0";
        }

        /// <summary>
        /// Return an Not Occuring Prediction
        /// </summary>
        public static Prediction NotOccuring()
        {
            var prediction = new Prediction()
            {
                Occuring = false,
                Strength = "0"
            };

            return prediction;
        }
        /// <summary>
        /// Return an Occuring Prediction
        /// </summary>
        public static Prediction IsOccuring()
        {
            var prediction = new Prediction()
            {
                Occuring = true,
                Strength = "-"
            };

            return prediction;
        }
    }
}
