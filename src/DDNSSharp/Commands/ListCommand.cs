using DDNSSharp.Configs;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
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
