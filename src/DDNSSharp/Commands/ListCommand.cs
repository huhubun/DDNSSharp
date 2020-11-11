using DDNSSharp.Configs;
using McMaster.Extensions.CommandLineUtils;
using static DDNSSharp.Commands.Helpers.ListCommandHelper;

namespace DDNSSharp.Commands
{
    [Command(Description = "查看已配置的域名信息")]
    class ListCommand
    {
        int OnExecute(CommandLineApplication app, IConsole console)
        {
            var configs = DomainConfigHelper.GetConfigs();

            WriteDomainConfigItemListToConsole(configs, console.Out);

            return 0;
        }
    }
}
