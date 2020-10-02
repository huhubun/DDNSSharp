using DDNSSharp.Attributes;
using DDNSSharp.Configs;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Providers
{
    abstract class ProviderBase
    {
        public const string PROVIDER_CONFIG_FILE_NAME = "provider.json";
        private bool _isDeleteCommand = false;

        public string Name => (Attribute.GetCustomAttribute(this.GetType(), typeof(ProviderAttribute)) as ProviderAttribute)?.Name;

        protected CommandLineApplication App { get; set; }

        public static IEnumerable<ProviderOption> GetOptions()
        {
            throw new NotImplementedException("Implement this method in your class.");
        }

        /// <summary>
        /// 向程序声明 Options。<br />
        /// Provider 的 Options 为动态声明的，不同 Provider 的 Options 可以不同。
        /// </summary>
        public virtual void SetOptionsToApp()
        {
            var options = GetProviderOptions(this.GetType());

            foreach (var option in options)
            {
                App.Option(option.Template, option.Description, option.OptionType);
            }
        }

        /// <summary>
        /// 保存新设置的 Options 到配置文件中。
        /// </summary>
        public virtual void SaveOptions()
        {
            SaveConfigFile();
        }

        public virtual void DeleteOptions()
        {
            _isDeleteCommand = true;

            SaveConfigFile();
        }

        public virtual T GetConfig<T>() where T : class, IProviderConfig
        {
            var currentProviderConfigBytes = GetConfigByName(Name);

            if (currentProviderConfigBytes == null || currentProviderConfigBytes.Length == 0)
            {
                return null;
            }

            return JsonSerializer.Deserialize<T>(currentProviderConfigBytes, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public void GetConfigFileNode()
        {
            var configBytes = File.ReadAllBytes(PROVIDER_CONFIG_FILE_NAME);
            if (configBytes.Length == 0)
            {
                return;
            }

            using var document = JsonDocument.Parse(configBytes);
            var root = document.RootElement;

            foreach (var jsonProperty in root.EnumerateObject())
            {
                if (jsonProperty.Name == Name && jsonProperty.Value.ValueKind == JsonValueKind.Object)
                {

                }
            }
        }

        public abstract void Sync(IEnumerable<DomainConfigItem> domainConfigItems);

        /// <summary>
        /// 将当前 Provider 配置的更改写入配置文件中。
        /// </summary>
        /// <param name="writer"></param>
        private void SaveCurrentProvider(Utf8JsonWriter writer)
        {
            if (_isDeleteCommand)
            {
                return;
            }

            writer.WritePropertyName(Name);
            writer.WriteStartObject();

            foreach (var option in App.Options)
            {
                if (option.HasValue())
                {
                    if (option.Values.Count > 1)
                    {
                        throw new NotSupportedException($"Multiple value of option '{option.LongName}' are not supported.");
                    }

                    var name = option.LongName ?? option.ShortName;

                    if (String.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException($"Option '{option.SymbolName}' has neither LongName nor ShortName.");
                    }

                    writer.WriteString(name, option.Value());
                }
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
