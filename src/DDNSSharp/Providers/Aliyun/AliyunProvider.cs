using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNSSharp.Providers.Aliyun
{
    [Provider(Name = "Aliyun")]
    class AliyunProvider : ProviderBase
    {
        public override IEnumerable<CommandOption> GetOptions()
        {
            yield return new CommandOption("id", CommandOptionType.SingleValue)
            {
                Description = "aliyun accessKeyId",
                LongName = "access key id"
            };

            yield return new CommandOption("secret", CommandOptionType.SingleValue)
            {
                Description = "aliyun accessSecret",
                LongName = "access secret"
            };
        }
    }
}
