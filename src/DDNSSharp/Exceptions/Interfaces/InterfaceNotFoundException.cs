using System;

namespace DDNSSharp.Exceptions.Interfaces
{
    public class InterfaceNotFoundException : Exception
    {
        public InterfaceNotFoundException() : base() { }

        public InterfaceNotFoundException(string message) : base(message) { }

        public InterfaceNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
