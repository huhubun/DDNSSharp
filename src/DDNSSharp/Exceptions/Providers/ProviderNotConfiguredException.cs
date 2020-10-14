using System;

namespace DDNSSharp.Exceptions.Providers
{
    class ProviderNotConfiguredException : Exception
    {
        public ProviderNotConfiguredException() : base() { }

        public ProviderNotConfiguredException(string message) : base(message) { }

        public ProviderNotConfiguredException(string message, Exception innerException) : base(message, innerException) { }
    }
}