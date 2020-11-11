using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;

namespace DDNSSharp.Commands
{
    [Command(Description = "查看支持的域名解析提供商名称")]
    class ProviderCommand
    {
        int OnExecute(CommandLineApplication app, IConsole console)
        {
            var providers = ProviderHelper.GetProviderNames();

            foreach (var provider in providers)
            {
                console.Out.WriteLine(provider);
            }

            return 0;
        }
    }
}
