using DDNSSharp.Configs;
using McMaster.Extensions.CommandLineUtils;
using static DDNSSharp.Commands.Helpers.ListCommandHelper;

namespace DDNSSharp.Commands
{
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
