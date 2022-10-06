


namespace Website
{


    /// <summary>
    /// Central place where text shown in alerts are stored
    /// Helps to maintain a uniform style of text.
    /// </summary>
    public class AlertText
    {
        //public const string ValidationError = $"Something went wrong, refresh page and try again!";
        public const string ValidationError = UnderMaintenance;
        public const string UnderMaintenance = $"Sorry, under maintenance. Please try later";
        public const string NoName = $"Please enter Name!";
        public const string ErrorWillRefresh = "Something went wrong. Please wait page will auto refresh.";
        //public const string ServerConnectionProblem = "Problem talking to Server\nSlow or unstable internet\ncan cause this";
        public const string ServerConnectionProblem = UnderMaintenance;
        public const string NoInternet = "Please check your Internet connection.";
        //public const string SorryNeedRefreshToHome = "Sorry! App just crashed.\nWe are fixing this error.\nPlease try again later.";
        public const string SorryNeedRefreshToHome = UnderMaintenance;
        public const string UpdatePersonFail = "Error failed to update person!\nPlease try again later.";
        public const string AskAstrologer = "Thank you, our astrologer will contact you soon!";
        public const string AskAstrologerEmail = "Please give email for astrologer to contact you!";
        public const string SelectEventType = "Select at least 1 Event Type!";
        public const string LoginFailed = "Login failed\nPlease try again";
        public const string FacebookLoginFail = "Error in OnFacebookSignInSuccessHandler where authResponse is null";
        public const string NoSavedCharts = "No saved charts, calculate a chart and save it to view here.";
    }
}
