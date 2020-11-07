using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDNSSharp.Providers
{
    static class ProviderHelper
    {
        public static ProviderBase GetInstanceByName(string providerName, CommandLineApplication app)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException(nameof(providerName));
            }

            foreach (var type in GetProviderTypes())
            {
                var providerAttribute = type.GetCustomAttribute<ProviderAttribute>();
                if (providerAttribute == null)
                {
                    throw new Exception("Classes derived from ProviderBase must set ProviderAttribute.");
                }

                if (String.Equals(providerName, providerAttribute.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Activator.CreateInstance(type, app) as ProviderBase;
                }
            }

            throw new NotSupportedException($"Provider '{providerName}' is not supported.");
        }

        public static IEnumerable<string> GetProviderNames()
        {
            return GetProviderTypes().Select(t => GetProviderName(t));
        }

        /// <summary>
        /// 判断指定的 DNS 提供商是否支持
        /// </summary>
        /// <param name="providerName">DNS 提供商名称</param>
        /// <returns></returns>
        public static bool CheckSupportability(string providerName)
        {
            return GetProviderNames().Any(p => p == providerName);
        }

        public static string GetProviderName(Type type)
        {
            return (Attribute.GetCustomAttribute(type, typeof(ProviderAttribute)) as ProviderAttribute)?.Name;
        }

        public static IEnumerable<ProviderOptionAttribute> GetProviderOptions(Type type)
        {
            return ProviderBase.GetOptions(type).OrderBy(po => po.LongName);
        }

        public static IEnumerable<Type> GetProviderTypes()
        {
            return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(ProviderBase))
                    .OrderBy(pt => pt.Name);
        }
    }
}
