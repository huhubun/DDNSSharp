using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDNSSharp.Providers.Aliyun
{
    [Provider(Name = "Aliyun")]
    class AliyunProvider : ProviderBase
    {
        private CommandLineApplication _app;

        public AliyunProvider(CommandLineApplication app)
        {
            _app = app;
        }

        public CommandOption<string> Id { get; private set; }

        public CommandOption<string> Secret { get; private set; }

        public override void SetOptions()
        {
            Id = _app.Option<string>("--id", "aliyun accessKeyId", CommandOptionType.SingleValue);
            Secret = _app.Option<string>("--secret", "aliyun accessSecret", CommandOptionType.SingleValue);
        }


    }
}
