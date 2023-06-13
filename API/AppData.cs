using VedAstro.Library;

namespace API
{
    internal static class AppData
    {

         static AppData()
        {
            //load data at startup
            URL = new URL(GetIsBetaRuntime(), false);
        }

        public static bool GetIsBetaRuntime() => ThisAssembly.BranchName.Contains("beta");


        public static URL URL { get; set; }
    }
}
