using System;

namespace VedAstro.Library
{
    [Serializable]
    public class ApiCommunicationFailed : Exception
    {
        public ApiCommunicationFailed() : base() { }
        public ApiCommunicationFailed(string message) : base(message) { }
        public ApiCommunicationFailed(string message, Exception inner) : base(message, inner) { }
    }
    public class NoInternetError : Exception
    {
        public NoInternetError() : base() { }
        public NoInternetError(string message) : base(message) { }
        public NoInternetError(string message, Exception inner) : base(message, inner) { }
    }
}
