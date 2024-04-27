
using Microsoft.Extensions.Logging;
using VedAstro.Library;

namespace Website
{

    public interface IUnhandledExceptionSender
    {
        event EventHandler<Exception> UnhandledExceptionThrown;
    }

    /// <summary>
    /// TODO Explanation needed
    /// </summary>
    public class UnhandledExceptionSender : ILogger, IUnhandledExceptionSender
    {

        public event EventHandler<Exception> UnhandledExceptionThrown;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            if (exception != null)
            {
                UnhandledExceptionThrown?.Invoke(this, exception);
            }
        }
    }

    public class UnhandledExceptionProvider : ILoggerProvider
    {
        UnhandledExceptionSender _unhandledExceptionSender;


        public UnhandledExceptionProvider(UnhandledExceptionSender unhandledExceptionSender)
        {
            _unhandledExceptionSender = unhandledExceptionSender;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new UnhandledExceptionLogger(categoryName, _unhandledExceptionSender);
        }

        public void Dispose()
        {
        }

        public class UnhandledExceptionLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly UnhandledExceptionSender _unhandeledExceptionSender;

            public UnhandledExceptionLogger(string categoryName, UnhandledExceptionSender unhandledExceptionSender)
            {
                _unhandeledExceptionSender = unhandledExceptionSender;
                _categoryName = categoryName;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            /// <summary>
            /// All unhandled exceptions will call this method
            /// Here exceptions are logged to API server
            /// </summary>
            public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {

#if DEBUG
                //in debug mode just rethrow exception
                throw exception;
#endif
                //console log everything we got to ID this error,
                //so that it never happens again!
                var extraInfo = "UNEXPECTED ERROR > STATUS CRITICAL \n";
                extraInfo += ($"{eventId.Id} {eventId.Name} \n");
                extraInfo += ($"{state} \n");
                extraInfo += ($"{formatter.Method.Name} \n");
                extraInfo += ($"{formatter.Method.MemberType} \n");
                extraInfo += ($"{formatter.Method.ReturnParameter} \n");
                extraInfo += ($"{formatter.Method.ReturnType} \n");
                extraInfo += ($"{formatter.Method.Module.FullyQualifiedName} \n");
                extraInfo += ($"{formatter.Target} \n");

                Console.WriteLine(extraInfo);

                //make a log to server
                await WebLogger.Error(exception, extraInfo);

                //now refresh page, only best option we have at the moment
                //otherwise user is left standing in broken page
                //note: below refresh method works well to recover, though it should not happen

                var timeToReadMsg = 4000; //give time for user to see the message

                //get debug mode, most likely cause for errors
                //if debug mode enabled tell user it could be the cause of errors
                if (await WebsiteTools.IsLocalServerDebugMode())
                {
                    await AppData.JsRuntime.ShowAlert(
                        icon: "warning",
                        title: "App has crashed!",
                        descriptionText: "Debug Mode is <strong>Enabled</strong>. It could be causing the error!"
                        );
                }
                //normal error message with redirect to home page
                else
                {
                    //prepare question to ask user
                    var alertData = new
                    {
                        icon = "warning",
                        title = "App has crashed!",
                        html = "Go <strong>Home</strong> page and press <kbd>CTRL + SHIFT + R</kbd> to <strong>restart</strong> app",
                        showCancelButton = true,
                        confirmButtonColor = "#3085d6",
                        cancelButtonColor = "#d33",
                        cancelButtonText = "Restart",
                        confirmButtonText = "Ignore"
                    };

                    var sweetAlertResult = await AppData.JsRuntime.ShowAlertResult(alertData);

                    //if user clicked "Ignore", then continue like nothing
                    var continueClicked = sweetAlertResult.GetProperty("isConfirmed").GetBoolean();

                    //user clicked "Restart"
                    if (!continueClicked)
                    {
                        //send user to home page to restart
                        await AppData.JsRuntime.LoadPage(AppData.URL.WebUrl);
                    }

                }

            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return new NoopDisposable();
            }

            private class NoopDisposable : IDisposable
            {
                public void Dispose()
                {
                }
            }
        }
    }

}
