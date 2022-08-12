using Genso.Astrology.Library;

namespace Website
{
    /// <summary>
    /// Static class to hold global site data
    /// </summary>
    public static class AppData
    {
        /// <summary>
        /// Represents the currently logged in User
        /// If null then nobody logged in
        /// </summary>
        public static UserData? CurrentUser { get; set; } = UserData.Empty;

        /// <summary>
        /// A copy of this visitor's ID, set when logging new visit
        /// Different from User Id in current user
        /// Used to log user flow
        /// </summary>
        public static string? VisitorId { get; set; }
    }
}