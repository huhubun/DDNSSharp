using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Commands.ProviderCommands
{
    [Command(AllowArgumentSeparator = true, UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    class DeleteCommand
    {
        [Argument(0, Description = "Provider name")]
        public string Name { get; }

        int OnExecute(CommandLineApplication app)
        {
            if (String.IsNullOrEmpty(Name))
            {
                Console.WriteLine("List of already configured providers:");

                var names = GetConfiguredProviderNames();

                if (names.Any())
                {
                    foreach (var providerName in names)
                    {
                        Console.WriteLine($"  {providerName}");
                    }
                }
                else
                {
                    Console.WriteLine("(There are no providers that have been set up. Use `provider set` command to set one.)");
                }

                return 1;
            }

            var provider = GetInstanceByName(Name, app);

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
