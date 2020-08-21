using System;

namespace Smart_Currency_Converter.Exceptions
{
    public class CameraAccessException : Exception
    {
        private const string DEFAULT_EXCEPTION_MESSAGE = "Camera Unavailable";

        public CameraAccessException() : this(DEFAULT_EXCEPTION_MESSAGE) { }

        public CameraAccessException(string message) : base(message) { }

        public CameraAccessException(string message, Exception inner) : base(message, inner) { }
    }
}