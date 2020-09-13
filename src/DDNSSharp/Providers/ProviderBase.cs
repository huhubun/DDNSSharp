using DDNSSharp.Attributes;
using System;

namespace DDNSSharp.Providers
{
    abstract class ProviderBase
    {
        public string Name => (Attribute.GetCustomAttribute(this.GetType(), typeof(ProviderAttribute)) as ProviderAttribute)?.Name;

        public abstract void SetOptions();

        public void SaveOptions()
        {
            Console.WriteLine("save successed!");
        }
    }
}
