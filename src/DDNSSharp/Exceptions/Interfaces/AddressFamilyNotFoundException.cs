using System;

namespace DDNSSharp.Exceptions.Interfaces
{
    class AddressFamilyNotFoundException : Exception
    {
        public AddressFamilyNotFoundException() : base() { }

        public AddressFamilyNotFoundException(string message) : base(message) { }

        public AddressFamilyNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}