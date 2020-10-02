using DDNSSharp.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDNSSharp.Commands.Helpers
{
    public static class AddCommandHelper
    {
        public static void WriteConfigInfoToConsole(DomainConfigItem item, TextWriter output)
        {
            output.WriteLine($"  Domain: {item.Domain}");
            output.WriteLine($"  Type: {item.Type}");
            output.WriteLine($"  Provider: {item.Provider}");
        }
    }
}
