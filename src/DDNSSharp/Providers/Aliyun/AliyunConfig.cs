using DDNSSharp.Attributes;
using System.Text.Json.Serialization;

namespace DDNSSharp.Providers.Aliyun
{
    class AliyunConfig : IProviderConfig
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        [ProviderOptionAttribute(Description = "aliyun accessKeyId")]
        [JsonPropertyName("ali-access-key-id")]
        public string Id { get; set; }

        [ProviderOptionAttribute(Description = "aliyun secret")]
        [JsonPropertyName("ali-secret")]
        public string Secret { get; set; }
    }
}
