namespace Website
{


    /// <summary>
    /// Table to maintain interop function names
    /// Blazor -> JS
    /// - Interop global var made available by App.js
    /// - actual functions reside in Interop.js
    /// </summary>
    public static class JS
    {
        public const string PlaySoundFromUrl = "Interop.PlaySoundFromUrl";
        public const string addOptionToSelectDropdown = "Interop.addOptionToSelectDropdown";
        public const string getProperty = "Interop.getProperty";
        public const string removeProperty = "Interop.removeProperty";
        public const string setProperty = "Interop.setProperty";
        public const string removeClassWrapper = "Interop.removeClassWrapper";
        public const string addClassWrapper = "Interop.addClassWrapper";
        public const string toggleClassWrapper = "Interop.toggleClassWrapper";
        public const string getTextWrapper = "Interop.getTextWrapper";
        public const string setTextWrapper = "Interop.setTextWrapper";
        public const string getValueWrapper = "Interop.getValueWrapper";
        public const string setValueWrapper = "Interop.setValueWrapper";
        public const string IsOnline = "Interop.IsOnline";
        public const string getUrl = "Interop.getUrl";
        public const string ShowLeaveEmailAlert = "Interop.ShowLeaveEmailAlert";
        public const string PopupTextInput = "Interop.PopupTextInput";
        public const string loadJs = "Interop.loadJs";
        public const string getOriginUrl = "Interop.getOriginUrl";
        public const string GoBackReload = "Interop.GoBackReload";
        public const string InitErrorCatcher = "Interop.InitErrorCatcher";
        public const string watchProperty = "Interop.watchProperty";
        public const string getElementWidth = "Interop.getElementWidth";
        public const string addWidthToEveryChild = "Interop.addWidthToEveryChild";
        public const string getPropWrapper = "Interop.getPropWrapper";
        public const string setPropWrapper = "Interop.setPropWrapper";
        public const string setAttrWrapper = "Interop.setAttrWrapper";
        public const string setCssWrapper = "Interop.setCssWrapper";
        public const string showWrapper = "Interop.showWrapper";
        public const string showListWrapper = "Interop.showListWrapper";
        public const string InitializeSearchForAPICallList = "Interop.InitializeSearchForAPICallList";
        public const string InitializeSearchForAPISelector = "Interop.InitializeSearchForAPISelector";
        public const string hideWrapper = "Interop.hideWrapper";
        public const string FunFeaturePopUp = "Interop.FunFeaturePopUp";
        public const string getRandomText = "Interop.getRandomText";
        public const string InitializeInPageNav = "Interop.InitializeInPageNav";
        public const string addEventListenerWrapper = "Interop.addEventListenerWrapper";
        public const string addEventListenerByClass = "Interop.addEventListenerByClass";
        public const string InjectIntoElement = "Interop.InjectIntoElement";
        public const string getVisitorData = "Interop.getVisitorData";
        public const string CopyToClipboard = "Interop.CopyToClipboard";
        public const string AddNewBookmark = "Interop.AddNewBookmark";
        public const string generateLifeEventListTable = "Interop.generateLifeEventListTable";
        public const string generatePlanetDataTable = "Interop.generatePlanetDataTable";
        public const string generatePlanetDataInfoTable = "Interop.generatePlanetDataInfoTable";
        public const string generateHouseDataTable = "Interop.generateHouseDataTable";
        public const string generateHouseDataInfoTable = "Interop.generateHouseDataInfoTable";
        public const string scrollToDiv = "Interop.scrollToDiv";
        public const string animateHighlightElement = "Interop.animateHighlightElement";
        public const string smoothSlideToggle = "Interop.smoothSlideToggle";
        public const string htmlToPdfAutoDownload = "Interop.htmlToPdfAutoDownload";
        public const string pdfToEmail = "Interop.pdfToEmail";
        public const string htmlToEmail = "Interop.htmlToEmail";
        public const string openPDFNewTab = "Interop.openPDFNewTab";
        public const string htmlToPdfBlob = "Interop.htmlToPdfBlob";
        public const string DrawHouseStrengthChart = "Interop.DrawHouseStrengthChart";
        public const string DrawPlanetStrengthChart = "Interop.DrawPlanetStrengthChart";
        public const string generateWebsiteTaskListTable = "Interop.generateWebsiteTaskListTable";
        public const string addNewLifeEventToTable = "Interop.addNewLifeEventToTable";
        public const string getLifeEventsListTableData = "Interop.getLifeEventsListTableData";
        public const string showAccordion = "Interop.showAccordion";
        public const string toggleAccordion = "Interop.toggleAccordion";
        public const string scrollIntoView = "Interop.scrollIntoView";
        public const string getAllLocalStorageKeys = "Interop.getAllLocalStorageKeys";
        public const string postWrapper = "Interop.postWrapper";
        public const string SkyChartAnimate = "Interop.SkyChartAnimate";
        public const string SkyChartInit = "Interop.SkyChartInit";
        public const string SaveAsFile = "Interop.SaveAsFile";
        public const string highlightByEventName = "Interop.highlightByEventName";
        public const string AddEventsToGoogleCalendar = "Interop.AddEventsToGoogleCalendar";
        public const string unhighlightByEventName = "Interop.unhighlightByEventName";
        public const string getScreenData = "Interop.getScreenData";
        public const string shareDialogFacebook = "Interop.shareDialogFacebook";


        //
        public const string ReadOnlyIfPassJson = "Interop.ReadOnlyIfPassJson";
        public const string ReadOnlyIfPassString = "Interop.ReadOnlyIfPassString";



        //CHART

        public const string Swal_fire = "Swal.fire";
        public const string Swal_close = "Swal.close";
        public const string Swal_showLoading = "Swal.showLoading";

        //this works after APP.js assigns dark mode to window
        public const string DarkMode_toggle = "window.DarkMode.toggle";
        public const string DarkMode_isActivated = "window.DarkMode.isActivated";
        public const string LogThread_postMessage = "window.LogThread.postMessage";


        public const string tippy = "tippy";

        //TIMEINPUT.JS
        public const string LoadCalendar = "Interop.LoadCalendar";
        public const string InitCalendarPicker = "Interop.InitCalendarPicker";


        //BOOTSTRAP JS

        //SPECIAL SIGN IN FUNCS RESIDE IN index.html
        public const string facebookLogin = "window.facebookLogin";
        public const string SetSignInButtonInstance = "window.SetSignInButtonInstance";
        public const string ResizeGoogleLoginButton = "window.ResizeGoogleLoginButton";

        /// <summary>
        /// can't use window. needs direct method name
        /// declared in index.html, no dots just direct method name
        /// </summary>
        public const string OnGoogleSignInSuccessHandler = "OnGoogleSignInSuccessHandler";

        //SPECIAL APP.JS FUNCS
        public const string GetInteropFuncList = "window.GetInteropFuncList";



        //native browser
        public const string window_location_assign = "window.location.assign";
        public const string window_location_reload = "window.location.reload";
        public const string history_back = "history.back";
        public const string open = "open";
        public const string import = "import";


        public const string ChartFromSVG = "window.ChartFromSVG";

    }
}
