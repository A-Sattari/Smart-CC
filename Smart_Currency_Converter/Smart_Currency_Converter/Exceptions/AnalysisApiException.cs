using System;

namespace Smart_Currency_Converter.Exceptions
{
    public class AnalysisApiException : Exception
    {
        public AnalysisApiException() {}

        public AnalysisApiException(string message) : base(message) {}

        public AnalysisApiException(string message, Exception inner) : base(message, inner) {}
    }
}