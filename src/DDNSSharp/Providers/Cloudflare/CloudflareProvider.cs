using DDNSSharp.Attributes;
using McMaster.Extensions.CommandLineUtils;

namespace DDNSSharp.Providers.Cloudflare
{
    [Provider(Name = "Cloudflare")]
    class CloudflareProvider : ProviderBase
    {
        private CommandLineApplication _app;

        public CloudflareProvider(CommandLineApplication app)
        {
            _app = app;
        }

        public CommandOption<string> Id { get; private set; }

        public CommandOption<string> Token { get; private set; }

        public override void SetOptionsToApp()
        {
            Id = _app.Option<string>("--id", "Cloudflare Account_ID", CommandOptionType.SingleValue);
            Token = _app.Option<string>("--token", "Cloudflare Token", CommandOptionType.SingleValue);
        }
    }
}
