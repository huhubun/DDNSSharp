using DDNSSharp.Attributes;
using DDNSSharp.Commands.SyncCommands;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace DDNSSharp.Providers
{
    public abstract class ProviderBase
    {
        public string Name => (Attribute.GetCustomAttribute(this.GetType(), typeof(ProviderAttribute)) as ProviderAttribute)?.Name;

        protected CommandLineApplication App { get; set; }

        public static IEnumerable<ProviderOptionAttribute> GetOptions(Type providerType)
        {
            var providerName = providerType.GetCustomAttribute<ProviderAttribute>().Name;

            var providerConfigType = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.GetInterfaces().Contains(typeof(IProviderConfig)))
                    .Where(t => t.Name.Equals($"{providerName}Config", StringComparison.InvariantCultureIgnoreCase))
                    .SingleOrDefault();

            if (providerConfigType == null)
            {
                throw new Exception($"必须为 Provider '{providerName}' 定义 Options");
            }

            foreach (var propertyInfo in providerConfigType.GetProperties())
            {
                var providerOptionAttribute = propertyInfo.GetCustomAttribute<ProviderOptionAttribute>();

                if (providerOptionAttribute != null)
                {
                    var jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
                    if (jsonPropertyNameAttribute != null)
                    {
                        providerOptionAttribute.LongName = jsonPropertyNameAttribute.Name;
                    }

                    if (String.IsNullOrEmpty(providerOptionAttribute.LongName))
                    {
                        throw new Exception($"必须为属性 '{propertyInfo.Name}' 设置 JsonPropertyName 或 LongName。");
                    }

                    yield return providerOptionAttribute;
                }
            }
        }

        public abstract void Sync(SyncContext context);
    }
}
