﻿using DDNSSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DDNSSharp.Providers
{
    static class ProviderHelper
    {
        public static ProviderBase GetByName(string providerName)
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
                    return Activator.CreateInstance(type) as ProviderBase;
                }
            }

            throw new NotSupportedException($"Provider '{providerName}' is not supported.");
        }

        public static IEnumerable<Type> GetProviderTypes()
        {
            return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(ProviderBase));
        }
    }
}
