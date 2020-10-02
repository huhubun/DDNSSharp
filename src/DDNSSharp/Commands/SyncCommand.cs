using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DDNSSharp.Configs.DomainConfigHelper;

namespace DDNSSharp.Commands
{
    class SyncCommand
    {
        int OnExecute(CommandLineApplication app, IConsole console)
        {
            var group = GetConfigs().GroupBy(c => c.Provider);
            foreach (var domainConfigItems in group)
            {
                var provider = ProviderHelper.GetInstanceByName(domainConfigItems.Key, app);
                provider.Sync(domainConfigItems);
            }

            return 0;
        }
    }
}
