using DDNSSharp.Attributes;
using DDNSSharp.Commands.SyncCommands;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace DDNSSharp.Providers.Cloudflare
{
    ////[Provider(Name = "cloudflare")]
    //class CloudflareProvider : ProviderBase
    //{
    //    public CloudflareProvider(CommandLineApplication app)
    //    {
    //        App = app;
    //    }

    //    public static new IEnumerable<ProviderOption> GetOptions()
    //    {
    //        yield return new ProviderOption("--id", "Cloudflare Account_ID", CommandOptionType.SingleValue);
    //        yield return new ProviderOption("--token", "Cloudflare Token", CommandOptionType.SingleValue);
    //    }

    //    public override void BeforeSync()
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public override void Sync(SyncContext context)
    //    {
    //        throw new System.NotImplementedException();
    //    }
    //}
}
