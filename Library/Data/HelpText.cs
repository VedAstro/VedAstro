namespace VedAstro.Library
{

    /// <summary>
    /// Place where Text is stored to be shown in Help Box
    /// </summary>
    public static class HelpText
    {
        public const string PrecisionHelp = @"The number of days in a pixel, more days in 1 pixel equals less precision. If the number is too low, the chart will take too long and will not generate. Change in small steps. For very high precision use Desktop App. Linked to time range, this number will auto update when time range is changed.";
        public const string GeoLocationName = @"Enter city or state name, will auto detect location";
        public const string DasaTimeRange = @"Start and end time for chart";
        public const string Gender = @"Gender is used in predicting marriage compatibility and horoscope";
        public const string PrivateProfileName = @"Use unique names for easy sorting later";
        public const string PersonAdvanced = @"This is optional";
        public const string DateInput = @"Format Example: 5th January 2020 is 05/01/2020";
        public const string TimezoneInput = @"Standard timezone used at location (UTC/GMT)";
        public const string MatchReportName = @"Sanskrit name of the Kutas or Pooruththam which are astrological methods used for calculation";
        public const string MatchReportResult = @"Good or bad prediction for an aspect of the relationship";
        public const string MatchReportInfo = @"Extra astrological info for the given result";
        public const string MatchReportMale = @"Extra astrological info regarding male horoscope";
        public const string MatchReportFemale = @"Extra astrological info regarding female horoscope";
        public const string AskAstrologerQuestion = @"Common questions to ask the astrologer";
        public const string AskAstrologerDetails = @"Give extra info regarding your question";
        public const string DasaChart = @"This chart shows accurate Dasa, Bhukti, Antaram and Gochara for person's life";
        public const string MuhurthaChart = @"This chart shows when good and bad Muhurtha events will happen";
        public const string SearchReference = @"Astrological facts used for quick reference";
        public const string SearchPredictions = @"These are horoscope predictions that have been programmed into VedAstro";
        public const string Column1MLData = @"Data in 1st column of table, can be birth time list or time list from range";
        public const string Column2MLData = @"Astro data in each column after Time, column read by ML/AI for learning";
        public const string SearchEvents = @"These are Muhurtha events that have been programmed into VedAstro";
        public const string HouseStrength = @"Good and bad aspects of life, higher score is better. Also known as Bhava Bala.";
        public const string PlanetStrength = @"Degree of planet's effect on you, higher score is stronger positive influence. Also known as Shadbala.";
    }
}
