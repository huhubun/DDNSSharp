using DDNSSharp.Commands.ProviderCommands;
using McMaster.Extensions.CommandLineUtils;
using static DDNSSharp.Commands.Helpers.ProviderCommandHelper;

namespace DDNSSharp.Commands
{
    [Subcommand(
        typeof(SetCommand),
        typeof(ProviderCommands.DeleteCommand)
    )]
    class ProviderCommand
    {
        int OnExecute(CommandLineApplication app, IConsole console)
        {
            WriteSupportedProviderListToConsole(console.Out, endWithNewLine: true);
            WriteAlreadyConfiguredProviderListToConsole(console.Out, endWithNewLine: true);

            return 1;
        }
    }
}
