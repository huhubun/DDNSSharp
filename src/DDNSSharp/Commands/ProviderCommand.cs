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

            console.WriteLine("Currently supported providers:");

            foreach (var provider in providers)
            {
                console.WriteLine($"  {provider}");
            }

            return 0;
        }
    }
}
