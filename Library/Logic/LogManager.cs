using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// TODO MARKED FOR DEPRECATION, REMOVE IF SEE FIT
    /// Manages the internal logging of the app
    /// Logs can be of types, error, debug
    /// NOTE : 
    ///     - a log file is not required for LogManager to work,
    ///        if a log file is supplied log data will be auto saved there
    ///        otherwise log will only exist as an instance variable in text form.
    ///     - the formatting used for logs in file & in text form is different
    ///       the time & log type is an xml element but, in text form it is just tab separated
    ///       todo maybe if needed this can be unified so, text form can go xml and vice versa
    /// </summary>
    public static class LogManager
    {

        /** INTERNAL TYPES **/

        /// <summary>
        /// Simple internal type to differentiate logs
        /// </summary>
        private enum LogType { Error, Debug, Failure }

        /// <summary>
        /// event handlers for log manager events
        /// todo add change name to "LogEventHandler"
        /// </summary>
        public delegate void LogEvent(string message = "");



        /** FIELDS **/

        private static Data _logFile = null;
        private static bool _logSet;



        /** EVENTS **/

        //Fired when a log, debug or error is added
        public static event LogEvent LogUpdated;
        public static event LogEvent ErrorOccurred;




        /** PROPERTIES **/

        /// <summary>
        /// All logs in text form
        /// </summary>
        public static string LogText { get; set; }

        /// <summary>
        /// Files where logs are saved.
        /// If not set logs will be saved to "LogText"
        /// Note: Set only once before any logging begins by Initialize()
        /// </summary>
        public static Data LogFile
        {
            get => _logFile;
            private set => _logFile = value;
        }




        /** PUBLIC METHODS **/


        /// <summary>
        /// Sets the log file,call once before using other methods
        ///  NOTE: No need to call this method if not using file/stream for logs
        /// </summary>
        public static void Initialize(Data log)
        {
            LogFile = log;

            //log file can only be set once,if set again raise alarm
            if (_logSet) { Error("Log file set more than once!"); }

            //mark log file as set
            _logSet = true;
        }

        /// <summary>
        /// Log an error from the exception
        /// </summary>
        public static void Error(Exception e)
        {
            //get the exception that started it all
            var originalException = e.GetBaseException();

            //add to main log text
            AddToLog(originalException.Message, LogType.Error);

            //save to file
            SaveToFile();


            //fire event that log has been updated
            LogUpdated?.Invoke();//null check in-case nobody is listening
            ErrorOccurred?.Invoke(e.Message);//null check in-case nobody is listening


            //----------------------FUNCTIONS---------------------------
            void SaveToFile()
            {
                //place to store the exception data
                string fileName;
                string methodName;
                int line;
                int columnNumber;
                string message;
                string source;

                //if no log file specified end here
                if (LogFile == null) return;

                //get the data from the exception
                //this fills the fields declared above
                ExtractDataFromException();

                //put together the new error record
                var newRecord = new XElement("Error",
                    new XElement("Message", message),
                    new XElement("Source", source),
                    new XElement("FileName", fileName),
                    new XElement("SourceLineNumber", line),
                    new XElement("SourceColNumber", columnNumber),
                    new XElement("MethodName", methodName),
                    new XElement("Time", GetNow())
                );

                //place new record into the log list
                LogFile.InsertRecord(newRecord);

                void ExtractDataFromException()
                {

                    //extract the data from the error
                    StackTrace st = new StackTrace(e, true);

                    //Get the first stack frame
                    StackFrame frame = st.GetFrame(st.FrameCount - 1);

                    //Get the file name
                    fileName = frame.GetFileName();

                    //Get the method name
                    methodName = frame.GetMethod().Name;

                    //Get the line number from the stack frame
                    line = frame.GetFileLineNumber();

                    //Get the column number
                    columnNumber = frame.GetFileColumnNumber();

                    message = originalException.ToString();

                    source = originalException.Source;
                }

            }
        }

        /// <summary>
        /// Log an error with the inputted text
        /// </summary>
        public static void Error(string errorMessage)
        {
            //add to main log text
            AddToLog(errorMessage, LogType.Error);

            //save to file
            SaveToFile();

            //fire event that log has been updated
            LogUpdated?.Invoke();//null check in-case nobody is listening
            ErrorOccurred?.Invoke(errorMessage);//null check in-case nobody is listening


            //-------------------FUNCTIONS--------------------------

            void SaveToFile()
            {
                //if no log file specified end here
                if (LogFile == null) return;

                var newRecord = new XElement("Error",
                    new XElement("Message", errorMessage),
                    new XElement("Time", GetNow())
                );

                //place new record into the log list
                LogFile.InsertRecord(newRecord);

            }
        }

        /// <summary>
        /// Log a debug message with the inputted text
        /// </summary>
        public static void Debug(string message)
        {
            //add to main log text
            AddToLog(message, LogType.Debug);

            //save to file
            SaveToFile();


            //fire event that log has been updated
            LogUpdated?.Invoke();//null check in-case nobody is listening

            //----------------FUNCTIONS------

            void SaveToFile()
            {
                //if no log file specified end here
                if (LogFile == null) return;

                //put together the new error record
                var newRecord = new XElement("Debug",
                    new XElement("Message", message),
                    new XElement("Time", GetNow())
                );

                //place new record into the log list
                LogFile.InsertRecord(newRecord);
            }
        }



        /** PRIVATE METHODS **/

        /// <summary>
        /// Adds message to the main log at a new line
        /// </summary>
        private static void AddToLog(string message, LogType debug) => LogText += $"\n{GetNow()}:\t{debug}:\t{message}";

        public static void Initialize(object data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets now time in UTC +8:00
        /// </summary>
        /// <returns></returns>
        private static DateTimeOffset GetNow()
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
