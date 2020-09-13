using System;

namespace DDNSSharp.Core.Providers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]

    public class ProviderAttribute : Attribute
    {
        public string Name { get; set; }
    }
}

