using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace DDNSSharp.Providers.Aliyun
{
    [Provider(Name = "aliyun")]
    class AliyunProvider : ProviderBase
    {
        public AliyunProvider(CommandLineApplication app)
        {
            App = app;
        }

        public static new IEnumerable<ProviderOption> GetOptions()
        {
            yield return new ProviderOption("--id", "aliyun accessKeyId", CommandOptionType.SingleValue);
            yield return new ProviderOption("--secret", "aliyun accessSecret", CommandOptionType.SingleValue);
        }
    }
}
