using DDNSSharp.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDNSSharp.Commands.Helpers
{
    public static class ListCommandHelper
    {
        public static void WriteDomainConfigItemListToConsole(List<DomainConfigItem> configs, TextWriter output)
        {
            if (configs.Any())
            {
                const int MARGIN = 1;

                var domainColWidh = configs.Max(c => c.Domain.Length) + MARGIN;
                var typeColWidh = configs.Max(c => c.Type.ToString().Length) + MARGIN;
                var interfaceColWidh = configs.Max(c => c.Interface.Length) + MARGIN;
                var providerColWidh = configs.Max(c => c.Provider.Length) + MARGIN;
                var lastSyncStatusColWidh = configs.Max(c => c.LastSyncStatus.ToString().Length) + MARGIN;
                var lastSyncTimeColWidh = configs.Max(c => c.LastSyncTime.ToString().Length) + MARGIN;

                foreach (var item in configs.OrderBy(c => c))
                {
                    output.WriteLine(
                        "{1}|{0}{2}|{0}{3}|{0}{4}|{0}{5}|{0}{6}",
                        String.Empty.PadRight(MARGIN),
                        item.Domain.PadRight(domainColWidh),
                        item.Type.ToString().PadRight(typeColWidh),
                        item.Interface.PadRight(interfaceColWidh),
                        item.Provider.PadRight(providerColWidh),
                        item.LastSyncStatus.ToString().PadRight(lastSyncStatusColWidh),
                        item.LastSyncTime.ToString().PadRight(lastSyncTimeColWidh)
                    );
                }
            }
            else
            {
                output.WriteLine("No domain has been added yet, please use the `add` command to add one.");
            }

            output.WriteLine();
        }
    }
}
