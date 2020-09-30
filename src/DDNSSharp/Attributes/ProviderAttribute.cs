using System;

namespace DDNSSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]

    class ProviderAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
