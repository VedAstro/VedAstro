


namespace Website
{


    /// <summary>
    /// Central place where text shown in alerts are stored
    /// Helps to maintain a uniform style of text.
    /// </summary>
    public class AlertText
    {
        public const string ValidationError = $"Something went wrong, refresh page and try again!";
        public const string NoName = $"Name field cannot be empty!";
        public const string ErrorWillRefresh = "Something went wrong. Please wait page will auto refresh.";
        public const string NoInternet = "Please check your Internet connection.";
        public const string SorryNeedRefreshToHome = "Sorry! App just crashed.\nWe are fixing this error.\nPlease try again later.";
        public const string UpdatePersonFail = "Error failed to update person!\nPlease try again later.";
        public const string AskAstrologer = "Thank you, our astrologer will contact you soon!";
    }
}
