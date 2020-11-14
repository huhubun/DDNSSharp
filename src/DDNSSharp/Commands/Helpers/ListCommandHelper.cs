using ConsoleTables;
using DDNSSharp.Configs;
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
                var table = new ConsoleTable(
                    nameof(DomainConfigItem.Domain),
                    nameof(DomainConfigItem.Type),
                    nameof(DomainConfigItem.Interface),
                    nameof(DomainConfigItem.Provider),
                    nameof(DomainConfigItem.LastSyncStatus),
                    nameof(DomainConfigItem.LastSyncTime),
                    nameof(DomainConfigItem.LastSyncSuccessTime));

                foreach (var item in configs.OrderBy(c => c))
                {
                    table.AddRow(
                        item.Domain,
                        item.Type,
                        item.Interface,
                        item.Provider,
                        item.LastSyncStatus,
                        item.LastSyncTime,
                        item.LastSyncSuccessTime);
                }

                table.Write(Format.Minimal);
            }
            else
            {
                output.WriteLine("No domain has been added yet, please use the `add` command to add one.");
            }

            output.WriteLine();
        }
    }
}
