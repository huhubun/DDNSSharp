using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using static DDNSSharp.Commands.Helpers.ProviderCommandHelper;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Commands.ProviderCommands
{
    [Command(AllowArgumentSeparator = true, UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    class SetCommand
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

                WriteSupportedProviderListToConsole(console.Out);

                return 0;
            }

            // 加载 Provider 的 Options
            provider.SetOptionsToApp();

            // 应用 Options，经过这一步，Option 对应的属性才能获取到 Option 的值
            app.Parse(app.RemainingArguments.ToArray());

            Console.WriteLine($"Now start to set up '{provider.Name}' provider.");

            provider.SaveOptions();

            Console.WriteLine("Saved.");

            return 0;
        }
    }
}
