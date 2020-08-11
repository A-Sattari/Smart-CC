using System;

namespace Smart_Currency_Converter.Exceptions
{
    public class CameraAccessException : Exception
    {
        public CameraAccessException() { }

        public CameraAccessException(string message) : base(message) { }

        public CameraAccessException(string message, Exception inner) : base(message, inner) { }
    }
}