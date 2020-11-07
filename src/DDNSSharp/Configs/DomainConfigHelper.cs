using DDNSSharp.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DDNSSharp.Configs
{
    public static class DomainConfigHelper
    {
        private const string DOMAIN_CONFIG_FILE_NAME = "domain.json";

        /// <summary>
        /// 应使用 <see cref="DefaultOptions"/> 属性
        /// </summary>
        private static JsonSerializerOptions _defaultOptions = null;

        private static JsonSerializerOptions DefaultOptions
        {
            get
            {
                if (_defaultOptions == null)
                {
                    _defaultOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    _defaultOptions.Converters.Add(new JsonStringEnumConverter());
                }

                return _defaultOptions;
            }
        }

        /// <summary>
        /// 获取域名配置内容。
        /// </summary>
        /// <returns>配置的内容。注意：如果配置文件不存在或没有内容，将返回长度为 0 的 <see cref="List"/> 而非 null。</returns>
        public static List<DomainConfigItem> GetConfigs()
        {
            using var file = File.Open(DOMAIN_CONFIG_FILE_NAME, FileMode.OpenOrCreate, FileAccess.Read);
            if (file.Length == 0)
            {
                return new List<DomainConfigItem>();
            }

            var bytes = new byte[file.Length];
            file.Read(bytes, 0, bytes.Length);
            file.Seek(0, SeekOrigin.Begin);

            return JsonSerializer.Deserialize<List<DomainConfigItem>>(bytes, DefaultOptions);
        }

        public static T GetProviderInfo<T>(DomainConfigItem configItem) where T : class, IProviderConfig
        {
            var json = JsonSerializer.Serialize(configItem.ProviderInfo);
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// 根据传入的条件精确查询域名配置。
        /// </summary>
        /// <param name="query">可以不填写 <see cref="DomainConfigItem.Provider"/>，仅通过 <see cref="DomainConfigItem.Domain"/> 和 <see cref="DomainConfigItem.Type"/> 就能确定唯一。一旦填写了 <see cref="DomainConfigItem.Provider"/>，那它也将进行判断。</param>
        /// <returns>匹配的记录或 null。</returns>
        public static DomainConfigItem GetSingle(DomainConfigItem query)
        {
            return GetConfigs().SingleOrDefault(c => c.Equals(query, ignoreProvider: String.IsNullOrEmpty(query.Provider)));
        }

        public static bool Exists(DomainConfigItem item, bool ignoreProvider = true)
        {
            return GetConfigs().Any(c => c.Equals(item, ignoreProvider));
        }

        public static void AddItem(DomainConfigItem item)
        {
            var configs = GetConfigs();
            configs.Add(item);

            File.WriteAllBytes(DOMAIN_CONFIG_FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(configs, DefaultOptions));
        }

        public static void UpdateItem(DomainConfigItem item)
        {
            var configs = GetConfigs() ?? new List<DomainConfigItem>();

            var i = configs.FindIndex(c => c.Equals(item));

            if (i == -1)
            {
                throw new KeyNotFoundException($"No record with domain name '{item.Domain}' and type '{item.Type}' was found.");
            }

            configs[i] = item;
            File.WriteAllBytes(DOMAIN_CONFIG_FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(configs, DefaultOptions));
        }

        public static int DeleteItem(DomainConfigItem item)
        {
            var configs = GetConfigs() ?? new List<DomainConfigItem>();
            var originalCount = configs.Count;

            // TODO 这里不可能查到多个相等并返回 List 的
            configs = configs.Where(c => !c.Equals(item)).ToList();
            var currentCount = configs.Count;

            var deletedCount = originalCount - currentCount;

            if (deletedCount > 0)
            {
                File.WriteAllBytes(DOMAIN_CONFIG_FILE_NAME, JsonSerializer.SerializeToUtf8Bytes(configs, DefaultOptions));
            }

            return deletedCount;
        }
    }
}
