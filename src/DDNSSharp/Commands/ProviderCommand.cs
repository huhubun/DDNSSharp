using DDNSSharp.Commands.ProviderCommands;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDNSSharp.Commands
{
    [Subcommand(
        typeof(SetCommand),
        typeof(ClearCommand)
    )]
    class ProviderCommand
    {
    }
}
