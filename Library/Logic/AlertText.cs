


namespace VedAstro.Library
{


    /// <summary>
    /// Central place where text shown in alerts are stored
    /// Helps to maintain a uniform style of text.
    /// </summary>
    public class AlertText
    {
        //public const string ValidationError = $"Something went wrong, refresh page and try again!";
        public const string FailedNameList = $"Failed to fetch name list.";
        public const string FailedFileProcess = $"Failed to process file, try again.";
        public const string InvalidBirthTime = $"Birth time is invalid, check date time format!";
        public const string UnderMaintenance = $"Sorry, under maintenance.\nPlease try later";
        public const string NewFeatures = $"New features are being added.\nPlease try later";
        public const string ImproveWebsite = $"We're improving the website.\nPlease try later";
        public const string EnterName = $"Please enter Name!";
        public const string SelectGender = $"Please select gender, male or female!";
        public const string InvalidLocation = $"Wrong location, try different place name.";
        public const string SelectName = $"Please select Name!";
        public const string ErrorWillRefresh = "Something went wrong.\nPlease wait page will auto refresh.";
        public const string SlowUnstableInternet = "Problem with internet connetion,\nPlease try again later.";
        public const string NoInternet = "Please check your Internet connection.";
        public const string SorryNeedRefreshToHome = "Sorry! App just crashed.\nWe are fixing this error.\nPlease try again later.";
        //public const string SorryNeedRefreshToHome = UnderMaintenance;
        public const string UpdatePersonFail = "Error when update person!\nPlease try again later.";
        public const string DeletePersonFail = "Error when delete person!\nPlease try again later.";
        public const string DeleteChartFail = "Error when delete chart!\nPlease try again later.";
        public const string AskAstrologer = "Thank you\nOur astrologer will contact you soon!";
        public const string AskAstrologerEmail = "Please give email for astrologer to contact you!";
        public const string SelectEventType = "Select at least 1 Event Type!";
        public const string LoginFailed = "Could not login.\nDon't give up, try again";
        public const string FacebookLoginFail = "Error in OnFacebookSignInSuccessHandler where authResponse is null";
        public const string NoSavedCharts = "No saved charts, calculate a chart and save it to view here.";
        public const string NoPersonFound = "Person profile not found,\nrefresh or check profile share link";
        public const string PersonProfileNoExist = "Person profile no longer exists, could not make chart.";
        public const string Unexpected = "Unexpected error, we are working on the fix.\nPlease try again later.";
        public const string PleaseLogin1 = "If you don't login how can we save your data?";
        public const string PleaseLogin2 = "Don't worry we won't spam you. Our servers got better things todo.";
        public const string PleaseLogin3 = "Login to proof you're not a program from the Machine World.";


        /// <summary>
        /// random select because server talking related problems, can only be caused by INTERNET (slow) or BAD CODE (new features)
        /// and since it's hard to detect during failure, for now select on random, to tell user both possible related errors info
        /// </summary>
        /// <returns></returns>
        public static string ServerConnectionProblem() => Tools.RandomSelect(new[]
        {
            SlowUnstableInternet, NewFeatures, ImproveWebsite, Unexpected
        });

        /// <summary>
        /// Random selection of errors messages for unexpected errors 
        /// </summary>
        public static string ObliviousErrors() => Tools.RandomSelect(new[] { Unexpected, SlowUnstableInternet, NewFeatures, ImproveWebsite });
        public static string FunnyLoginText() => Tools.RandomSelect(new[] { PleaseLogin1, PleaseLogin2, PleaseLogin3 });
        public static string FunnyPleaseSelectText(string subject = "")
        {
            //list messages here for faster access
            var msgList = new[]
            {
                $"We are not mind readers. Please select {subject} to continue.",
            };

            var isDoneText = Tools.RandomSelect(msgList);
            return isDoneText;
        }

        /// <summary>
        /// How was your day, darling?
        /// </summary>
        /// <returns></returns>
        public static string RandomNoun()
        {
            //word below should nicely ryme with My darling, my dear,
            //exp: if you don't login, how are we supposed get your data, darling?
            //note: comma before question
            //How was your day, darling? 
            var msgList = new[]
            {
                "dear",
                "madam",
                "sir",
                "darling",
                "friend",
                "sweetheart",
            };

            var isDoneText = Tools.RandomSelect(msgList);
            return isDoneText;

        }

        public static string IsDoneText()
        {
            //list messages here for faster access
            var msgList = new[]
            {
                "Your pizza is ready!",
                "I will donate!",
                "Mission accomplished!",
                "Ready, captain!",
                "Shells away!",
                "I will donate!",
                "Fire in the hole!",
                "Your cake is baked!",
                "Generated nicely!",
                "Bingo done!",
                "Oh yeah, its done!",
                "Ready to sail, captain!",
                "I'm donating!",
                "We have liftoff!",
                "Houston, we have liftoff!",
                "Your order is up!",
                "I will donate!",
                "Success, commander!",
                "Bombs away!",
                "I'm donating!",
                "Your pie is cooked!",
                "Generated smoothly!",
                "Jackpot hit!",
                "Yes, it's complete!",
                "Anchors aweigh, captain!",
                "We are airborne!",
                "I'm donating!",
                "Houston, we are in orbit!",
            };

            var isDoneText = Tools.RandomSelect(msgList);
            return isDoneText;
        }
    }
}

//                WHAT DO YOU SEE?
//           THAT'S WHAT'S IN YOUR MIND!
//     THERE IS NO VICE NOR VIRTUE IN THIS WORLD
//          ALL IS BUT A REFLECTION OF YOU!
//
//              SO, WHAT DO YOU SEE? -- Walter Kovacs AKA Rorschach 
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣠⠤⠶⠶⠶⠤⠤⣄⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⡤⠚⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠳⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣠⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⢦⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⣠⠞⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⡼⢣⡶⠀⠀⠀⠀⠀⣠⠖⠉⣀⡴⠶⠊⢷⡆⠀⠀⠀⠀⠀⠀⠀⠀⢹⡄⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⢀⣼⠃⡼⠁⠀⠀⠀⣠⠞⣡⡴⠋⠁⠀⠀⠀⠘⣿⣧⡀⠀⠀⠀⣇⠀⠀⠸⡇⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⣼⡏⢠⡇⠀⠀⠀⣼⣣⠞⠃⠀⠀⠀⣀⣠⣤⣄⣿⡇⢧⡀⠀⠀⢻⣄⡀⠀⡗⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⢸⡇⢸⡇⠀⢠⣼⣿⣏⡀⠀⠀⢠⢾⠛⠉⠁⢀⣿⡉⠛⣷⣄⠀⢸⣿⠻⣶⣧⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠘⡇⠀⣇⡴⢿⣿⡉⠉⠉⠁⠀⠀⠀⣀⣶⠾⢿⡛⠟⢶⣼⡇⠀⢸⢻⡀⣹⣿⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⣧⠀⢸⡆⣿⣛⠩⣿⣷⠀⠀⠀⠀⠙⠉⣷⢾⣿⡇⠾⠀⢻⠀⢸⠇⣽⠛⢹⡇⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⢸⡄⠀⠻⣏⠛⠳⡿⢿⡞⠀⠀⠀⠀⠀⠾⠿⠿⠇⠆⠀⠀⣆⣼⣱⡇⠀⠈⣧⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⢳⠀⠀⠙⡄⠀⠙⠉⢹⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⡿⠁⢿⡀⠀⢸⣇⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠈⢧⠀⠀⢻⡄⠀⠀⠈⡷⠂⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣸⠇⢰⠘⡇⠀⠈⢿⣆⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠈⢷⡀⠘⡟⠦⣀⠀⠀⠒⠒⠋⠉⠁⠀⠀⢀⣠⠆⠀⣹⠀⣈⡇⢹⡄⠀⠸⣿⡄⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠱⣄⣷⠀⢹⡳⣄⡀⠀⠀⠀⠀⣀⡴⠚⠁⠀⠀⣿⠀⣟⢿⠈⡇⠀⠀⣹⣻⡀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⠀⣼⠻⣧⣿⠇⢰⡍⠳⠤⣴⠛⠁⠀⠀⠀⠀⢰⣿⠀⡿⢸⠀⢸⡀⢀⣿⡇⣿⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣠⠇⢀⡼⠋⢀⣮⡇⣀⠴⡟⠀⠀⠀⠀⠀⠀⣿⠃⠻⣅⣼⠀⢸⡇⢸⣹⠀⣿⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢀⠏⢀⡞⢀⣀⡸⠟⠋⠁⠘⠁⠀⠀⠀⠀⠀⠀⣿⠀⠀⠉⠳⢄⣸⣧⣧⣟⡼⠉⠀⠀⠀
//⠀⠀⠀⠀⢀⡤⠿⠤⠚⢲⣟⣁⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢻⡀⡀⠀⢀⣀⠙⠛⠿⢾⣄⠀⠀⠀⠀
//⠀⠀⢀⡴⠋⠀⠀⠀⠀⠀⠀⠀⠉⠉⠉⠛⠓⠀⠴⠒⠒⠚⠒⠛⠛⣿⡉⠉⠉⠀⠀⠀⠀⠀⠀⠙⢢⡀⠀
//⠀⠀⣸⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠹⣿⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢿⣄
//⠀⢸⢹⠀⠀⠀⢀⡤⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⠓⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿
//⠀⠀⢸⠀⠀⢀⣾⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣶⡄⠀⠀⠀⠀⠀⠀⠀⣼⠛
//⠀⠀⠸⣇⣰⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢹⡇⠀⠀⠀⠀⠀⠀⢀⡏⠀
//⠀⠀⣰⠟⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢷⠀⠀⠀⠀⠀⠀⣼⠀⠀
//⠀⡞⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡶⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⡇⠀⠀⠀⠀⣰⠃⠀⠀
//⣼⢃⣴⡃⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⠀⠀⠀⢳⠀⠀⠀⢠⠏⠀⠀⠀
//⣿⠈⠿⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢰⣹⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢤⣶⠀⠀⢸⠀⠀⠀⣾⠀⠀⠀⠀
//⢹⡆⠀⠀⠐⠊⠀⠀⠀⠀⠀⠀⠀⠀⠈⢻⡀⠀⠀⠀⠀⠀⠀⠀⣄⠀⠉⠈⠀⠀⡼⠀⠀⢰⡇⠀⠀⠀⠀
//⠀⠹⣆⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡄⠀⠀⢳⡀⠀⠀⠀⠀⠀⠀⠀⠁⠀⠀⠀⣸⠃⠀⠀⣼⠀⠀⠀⠀⠀
//⠀⠀⠈⠳⢤⣀⣀⣀⣀⣀⡴⠖⠋⠀⠀⠀⠀⠙⠦⣀⠀⠀⠀⠀⠀⠀⢀⣠⠾⠁⠀⠀⢰⡏⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⣇⢻⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠓⠒⠒⠒⠋⠉⣿⠀⠀⠀⠀⣸⠁⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢽⣼⠀⠀⠄⠀⠀⠀⠀⠀⠀⠲⣄⠀⠀⠀⠀⠀⠀⠀⠀⢀⡇⠀⠀⠀⢠⡇⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢸⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⠀⠀⣸⠁⠀⠀⠀⣸⠁⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⣿⡇⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠈⠳⣄⠀⠀⠀⠀⡿⠀⠀⠀⢰⡇⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢿⣿⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⠃⠀⠀⠀⢸⠃⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢸⢹⠀⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⠀⠀⠀⠀⢼⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢸⡼⣇⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢘⣿⠀⠀⠀⠀⣸⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠘⣧⣿⠀⠀⠀⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⠀⠀⠀⢠⡏⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⠀⣹⠃⠀⠀⠀⣧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣄⠀⠀⣾⠃⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⠀⢠⡟⠀⠀⠀⠀⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢦⣴⠃⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⢀⡾⠀⠀⠀⠀⠀⣽⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠳⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⠀⡼⠁⠀⠀⠀⠀⠈⠿⠛⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠳⡄⠀⠀⠀⠀⠀⠀⠀
//⠀⠀⠀⠀⣸⠇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢦⣀⠀⠀⠀⠀⠀
//       I SEE BEAUTY, I SEE SWEETNESS
//I SEE THE GENIUS OF MAN TO CARVE NUMBERS INTO EMOTION
//          I SEE GOODNESS IN ALL