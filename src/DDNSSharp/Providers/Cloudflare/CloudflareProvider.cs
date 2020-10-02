using DDNSSharp.Attributes;
using DDNSSharp.Configs;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace DDNSSharp.Providers.Cloudflare
{
    [Provider(Name = "cloudflare")]
    class CloudflareProvider : ProviderBase
    {
        public CloudflareProvider(CommandLineApplication app)
        {
            App = app;
        }

        public static new IEnumerable<ProviderOption> GetOptions()
        {
            yield return new ProviderOption("--id", "Cloudflare Account_ID", CommandOptionType.SingleValue);
            yield return new ProviderOption("--token", "Cloudflare Token", CommandOptionType.SingleValue);
        }

        public override void Sync(IEnumerable<DomainConfigItem> domainConfigItems)
        {
            throw new System.NotImplementedException();
        }
    }
}
