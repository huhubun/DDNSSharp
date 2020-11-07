using DDNSSharp.Attributes;
using System.Text.Json.Serialization;

namespace DDNSSharp.Providers.Cloudflare
{
    class CloudflareConfig : IProviderConfig
    {
        /// <summary>
        /// AccessKeyId
        /// </summary>
        [ProviderOption(Description = "Cloudflare Account_ID")]
        [JsonPropertyName("cf-account-id")]
        public string AccountId { get; set; }

        [ProviderOption(Description = "Cloudflare Token")]
        [JsonPropertyName("cf-token")]
        public string Token { get; set; }


        [ProviderOption(Description = "Cloudflare Zone_ID")]
        [JsonPropertyName("cf-zone-id")]
        public string ZoneId { get; set; }
    }
}
