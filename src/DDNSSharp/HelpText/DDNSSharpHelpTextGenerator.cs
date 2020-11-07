using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using System;
using System.Collections.Generic;
using System.IO;
using static DDNSSharp.Commands.Helpers.ProviderCommandHelper;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.HelpText
{
    class DDNSSharpHelpTextGenerator : DefaultHelpTextGenerator
    {
        protected override void GenerateArguments(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandArgument> visibleArguments, int firstColumnWidth)
        {
            base.GenerateArguments(application, output, visibleArguments, firstColumnWidth);

            if (application is CommandLineApplication<Commands.ProviderCommands.SetCommand>)
            {
                output.WriteLine();

                WriteSupportedProviderListToConsole(output, endWithNewLine: false);
            }

            if (application is CommandLineApplication<Commands.ProviderCommands.DeleteCommand>)
            {
                output.WriteLine();

                WriteAlreadyConfiguredProviderListToConsole(output, endWithNewLine: false);
            }
        }

        protected override void GenerateOptions(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandOption> visibleOptions, int firstColumnWidth)
        {
            base.GenerateOptions(application, output, visibleOptions, firstColumnWidth);

            if (application is CommandLineApplication<Commands.AddCommand>)
            {
                output.WriteLine();
                output.WriteLine("[Note] Different providers have different options");

                var outputFormat = string.Format("  {{0, -{0}}}{{1}}", 12 /* first column width */);

                foreach (var providerType in GetProviderTypes())
                {
                    var providerName = GetProviderName(providerType);
                    var options = GetProviderOptions(providerType);

                    Console.WriteLine($"Provider '{providerName}' supports:");

                    foreach (var option in options)
                    {
                        var wrappedDescription = IndentWriter?.Write(option.Description);
                        var message = string.Format(outputFormat, Format(new CommandOption(option.Template, option.OptionType)), wrappedDescription);
                        output.WriteLine(message);
                    }
                }
            }
        }
    }
}