using System;

namespace Smart_Currency_Converter.Exceptions
{
    public class InternetAccessException : Exception
    {
        private const string DEFAULT_EXCEPTION_MESSAGE = "Internet Connection is Not Available";

        public InternetAccessException() : this(DEFAULT_EXCEPTION_MESSAGE) { }

        public InternetAccessException(string message) : base(message) { }

        public InternetAccessException(string message, Exception inner) : base(message, inner) { }
    }
}