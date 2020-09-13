using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNSSharp.Providers.Cloudflare
{
    [Provider(Name = "Cloudflare")]
    class CloudflareProvider : ProviderBase
    {
        //public override IEnumerable<CommandOption> GetOptions()
        //{
        //    yield return new CommandOption("id", CommandOptionType.SingleValue)
        //    {
        //        Description = "Cloudflare Account_ID",
        //        LongName = "account ID"
        //    };

        //    yield return new CommandOption("token", CommandOptionType.SingleValue)
        //    {
        //        Description = "Cloudflare Token",
        //        LongName = "token"
        //    };
        //}


        public override void SetOptions()
        {
            throw new NotImplementedException();
        }
    }
}
