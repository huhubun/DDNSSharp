using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;
using static DDNSSharp.Commands.Helpers.ProviderCommandHelper;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Commands.ProviderCommands
{
    [Command(AllowArgumentSeparator = true, UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    class DeleteCommand
    {
        [Argument(0, Description = "Provider name")]
        public string Name { get; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (String.IsNullOrEmpty(Name))
            {
                app.ShowHelp();
                return 1;
            }

            ProviderBase provider;

            try
            {
                provider = GetInstanceByName(Name, app);
            }
            catch (NotSupportedException ex)
            {
                console.Error.WriteLine(ex.Message);

                console.Out.WriteLine();

                WriteAlreadyConfiguredProviderListToConsole(console.Out);

                return 0;
            }

            Console.WriteLine($"Now start to delete the configuration of '{provider.Name}' provider.");

            if (GetConfiguredProviderNames().Contains(Name))
            {
                provider.DeleteOptions();

                Console.WriteLine("Deleted.");
            }
            else
            {
                Console.WriteLine("This provider does not have any configuration.");
            }

            return 0;
        }
    }
}
