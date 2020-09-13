using DDNSSharp.Core.Providers;
using DDNSSharp.Core.Providers.Attributes;
using McMaster.Extensions.CommandLineUtils;

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

        public override void SetOptionsToApp()
        {
            Id = _app.Option<string>("--id", "aliyun accessKeyId", CommandOptionType.SingleValue);
            Secret = _app.Option<string>("--secret", "aliyun accessSecret", CommandOptionType.SingleValue);
        }
    }
}