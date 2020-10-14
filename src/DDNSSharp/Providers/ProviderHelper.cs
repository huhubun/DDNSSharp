﻿using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

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

        public static string GetProviderName<T>() where T : ProviderBase
        {
            return GetProviderName(typeof(T));
        }

        public static IEnumerable<ProviderOption> GetProviderOptions(Type type)
        {
            var getOptionsMethod = type.GetMethod(nameof(ProviderBase.GetOptions));
            if (getOptionsMethod == null)
            {
                // Provider 类中没有实现 GetOptions() 方法，显示错误信息
                ProviderBase.GetOptions();
            }

            var options = getOptionsMethod.Invoke(null, null) as IEnumerable<ProviderOption>;

            return options;
        }

        public static IEnumerable<Type> GetProviderTypes()
        {
            return Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.BaseType == typeof(ProviderBase));
        }

        public static IEnumerable<string> GetConfiguredProviderNames()
        {
            if (!File.Exists(ProviderBase.PROVIDER_CONFIG_FILE_NAME))
            {
                yield break;
            }

            var configBytes = File.ReadAllBytes(ProviderBase.PROVIDER_CONFIG_FILE_NAME);
            if (configBytes.Length == 0)
            {
                yield break;
            }

            var providerNames = GetProviderNames();

            using var document = JsonDocument.Parse(configBytes);
            var root = document.RootElement;

            foreach (var jsonProperty in root.EnumerateObject())
            {
                if (providerNames.Contains(jsonProperty.Name) && jsonProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    yield return jsonProperty.Name;
                }
            }
        }

        public static byte[] GetConfigByName(string providerName)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException(nameof(providerName));
            }

            if (!File.Exists(ProviderBase.PROVIDER_CONFIG_FILE_NAME))
            {
                return null;
            }

            var configBytes = File.ReadAllBytes(ProviderBase.PROVIDER_CONFIG_FILE_NAME);
            if (configBytes.Length == 0)
            {
                return null;
            }

            using var document = JsonDocument.Parse(configBytes);
            var root = document.RootElement;

            foreach (var jsonProperty in root.EnumerateObject())
            {
                if (jsonProperty.Name == providerName && jsonProperty.Value.ValueKind == JsonValueKind.Object)
                {
                    using var ms = new MemoryStream();
                    using var writer = new Utf8JsonWriter(ms);

                    jsonProperty.Value.WriteTo(writer);

                    writer.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    return ms.ToArray();
                }
            }

            return null;
        }
    }
}
