
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
                await AppData.JsRuntime.ShowAlert("warning", AlertText.ErrorWillRefresh, false, timeToReadMsg);
                await AppData.JsRuntime.LoadPage(await AppData.CurrentUrlJS);
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
