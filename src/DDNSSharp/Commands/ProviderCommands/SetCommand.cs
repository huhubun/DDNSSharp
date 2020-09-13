using DDNSSharp.Providers;
using DDNSSharp.Providers.Aliyun;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DDNSSharp.Commands.ProviderCommands
{
    [Command(AllowArgumentSeparator = true, UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
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


            var provider = ProviderHelper.GetInstanceByName(Name, app);

            Console.WriteLine(provider);

            provider.SetOptions();

            app.Parse(app.RemainingArguments.ToArray());

            //if (provider is AliyunProvider)
            //{
            //    var aliyun = provider as AliyunProvider;

            //    Console.WriteLine($"id = {aliyun.Id?.Value()}, secret = {aliyun.Secret?.Value()}");
            //}

            provider.SaveOptions();

            return 0;
        }

    }
}
