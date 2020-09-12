using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNSSharp.Providers
{
    abstract class ProviderBase
    {
        public abstract IEnumerable<CommandOption> GetOptions();
    }
}
