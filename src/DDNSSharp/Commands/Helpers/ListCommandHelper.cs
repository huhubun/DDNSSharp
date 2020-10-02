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
            const int MARGIN = 1;

            var domainColWidh = configs.Max(c => c.Domain.Length) + MARGIN;
            var typeColWidh = configs.Max(c => c.Type.ToString().Length) + MARGIN;
            var providerColWidh = configs.Max(c => c.Provider.Length) + MARGIN;

            foreach (var item in configs.OrderBy(c => c))
            {
                output.WriteLine(
                    "{1}|{0}{2}|{0}{3}",
                    String.Empty.PadRight(MARGIN),
                    item.Domain.PadRight(domainColWidh),
                    item.Type.ToString().PadRight(typeColWidh),
                    item.Provider.PadRight(providerColWidh)
                );
            }

            output.WriteLine();
        }
    }
}
