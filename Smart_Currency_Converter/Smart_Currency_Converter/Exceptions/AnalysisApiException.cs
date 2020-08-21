using System;

namespace Smart_Currency_Converter.Exceptions
{
    public class AnalysisApiException : Exception
    {
        private const string DEFAULT_EXCEPTION_MESSAGE = "Something went wrong while analyzing the taken photo";

        public AnalysisApiException() : this(DEFAULT_EXCEPTION_MESSAGE) {}

        public AnalysisApiException(string message) : base(message) {}

        public AnalysisApiException(string message, Exception inner) : base(message, inner) {}
    }
}