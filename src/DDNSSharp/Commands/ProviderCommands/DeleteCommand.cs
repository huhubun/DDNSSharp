using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

namespace DDNSSharp.Commands.ProviderCommands
{
    [Command(AllowArgumentSeparator = true, UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    class DeleteCommand
    {
        [Argument(0, Description = "Provider name")]
        [Required]
        public string Name { get; }

        // int OnExecute(CommandLineApplication app)
        //{
        //    if (String.IsNullOrEmpty(Name))
        //    {
        //        app.ShowHelp();
        //        return 1;
        //    }

        //    var provider = ProviderHelper.GetInstanceByName(Name, app);

        //    Console.WriteLine($"Now start to delete the configuration of '{provider.Name}' provider.");

        //    provider.DeleteOptions();

        //    Console.WriteLine("Deleted.");

        //    return 0;
        //}
    }
}
