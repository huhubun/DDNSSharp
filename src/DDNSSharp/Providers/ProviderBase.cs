using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace DDNSSharp.Providers
{
    abstract class ProviderBase
    {
        private const string PROVIDER_CONFIG_FILE_NAME = "provider.json";

        public string Name => (Attribute.GetCustomAttribute(this.GetType(), typeof(ProviderAttribute)) as ProviderAttribute)?.Name;

        /// <summary>
        /// 向程序声明 Options。<br />
        /// Provider 的 Options 为动态声明的，不同 Provider 的 Options 可以不同。
        /// </summary>
        public abstract void SetOptionsToApp();

        /// <summary>
        /// 保存新设置的 Options 到配置文件中。
        /// </summary>
        public virtual void SaveOptions()
        {
            SaveConfigFile();

            Console.WriteLine("save successed!");
        }

        /// <summary>
        /// 获取与 Options 关联的属性信息。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PropertyInfo> GetOptionProperties()
        {
            foreach (var property in this.GetType().GetProperties())
            {
                var propertyType = property.PropertyType;

                if (
                    propertyType == typeof(CommandOption) ||
                    (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(CommandOption<>))
                )
                {
                    yield return property;
                }
            }
        }

        /// <summary>
        /// 获取与 Options 关联的属性的名称。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetOptionPropertyNames()
        {
            foreach (var property in GetOptionProperties())
            {
                yield return property.Name;
            }
        }

        /// <summary>
        /// 从当前 Provider 实例中获取指定的属性名称对应 Option 的值。
        /// </summary>
        /// <param name="optionPropertyName">要获取 Option 值的属性名称。</param>
        /// <returns>对应 Option 的值。</returns>
        public string GetOptionPropertyValue(string optionPropertyName)
        {
            var property = GetOptionProperties().SingleOrDefault(p => p.Name == optionPropertyName);

            if (property == null)
            {
                throw new ArgumentException($"Provider '{Name}' has no property named '{optionPropertyName}'", optionPropertyName);
            }

            var commandOption = property.GetValue(this);
            return typeof(CommandOption).GetMethod(nameof(CommandOption.Value)).Invoke(commandOption, null)?.ToString();
        }

        /// <summary>
        /// 将当前 Provider 配置的更改写入配置文件中。
        /// </summary>
        /// <param name="writer"></param>
        private void SaveCurrentProvider(Utf8JsonWriter writer)
        {
            writer.WritePropertyName(Name);
            writer.WriteStartObject();

            foreach (var name in GetOptionPropertyNames())
            {
                writer.WriteString(name, GetOptionPropertyValue(name));
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// 保存配置文件。
        /// </summary>
        private void SaveConfigFile()
        {
            var configBytes = File.Exists(PROVIDER_CONFIG_FILE_NAME) ? File.ReadAllBytes(PROVIDER_CONFIG_FILE_NAME) : new byte[0];
            var saved = false;

            using var ms = new MemoryStream();
            using var writer = new Utf8JsonWriter(ms, new JsonWriterOptions
            {
                // 让保存下来的配置文件有可读性
                Indented = true
            });

            writer.WriteStartObject();

            if (configBytes.Length > 0)
            {
                using var document = JsonDocument.Parse(configBytes);
                var root = document.RootElement;

                foreach (var jsonProperty in root.EnumerateObject())
                {
                    if (jsonProperty.Name == Name && jsonProperty.Value.ValueKind == JsonValueKind.Object)
                    {
                        SaveCurrentProvider(writer);
                        saved = true;
                    }
                    else
                    {
                        jsonProperty.WriteTo(writer);
                    }
                }
            }

            // jsonProperties is null --> no provider.json
            // or
            // saved == false --> current provider not in the provider.json 
            if (!saved)
            {
                SaveCurrentProvider(writer);
            }

            writer.WriteEndObject();
            writer.Flush();

            ms.Seek(0, SeekOrigin.Begin);

            // 最后再从内存流里将内容写入文件，避免处理到一半报错了，文件内容被破坏的问题
            using var fs = File.OpenWrite(PROVIDER_CONFIG_FILE_NAME);
            // 清空文件内容。如果不清空的话，当新内容的长度小于原来内容的长度，这部分内容会被保留下来
            fs.SetLength(0);
            fs.Write(ms.ToArray());
        }
    }
}
