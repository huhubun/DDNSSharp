using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;

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

            Console.WriteLine($"Now start to set up '{Name}' provider.");

            var provider = ProviderHelper.GetInstanceByName(Name, app);

            // 加载 Provider 的 Options
            provider.SetOptionsToApp();

            // 应用 Options，经过这一步，Option 对应的属性才能获取到 Option 的值
            app.Parse(app.RemainingArguments.ToArray());

            provider.SaveOptions();

            return 0;
        }

    }
}
