using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DDNSSharp.Commands.ProviderCommands
{
    class SetCommand
    {
        [Argument(0, Description = "Provider name")]
        [Required]
        public string Name { get; }

        int OnExecute(CommandLineApplication app)
        {
            if (String.IsNullOrEmpty(Name))
            {
                app.ShowHelp();
                return 1;
            }

            Console.WriteLine($"input name is {Name}");

            var provider = ProviderHelper.GetByName(Name);
            Console.WriteLine(provider);


            return 0;
        }

    }
}
