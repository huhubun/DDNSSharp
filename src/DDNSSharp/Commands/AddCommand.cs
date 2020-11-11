using DDNSSharp.Commands.AddCommands;
using DDNSSharp.Commands.Helpers;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;

namespace DDNSSharp
{
    static class AddCommand
    {
        public static Action<CommandLineApplication<AddCommandModel>> Command => (addCmd) =>
        {
            addCmd.Description = "添加域名信息";
            addCmd.UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw;

            // 设置支持的 Provider 名称
            var providerOption = addCmd.Options
                                    .Where(o => o.LongName.Equals(nameof(AddCommandModel.Provider), StringComparison.InvariantCultureIgnoreCase))
                                    .Single();

            providerOption.Description += $"\nSupported: {String.Join(", ", ProviderHelper.GetProviderNames())}.";

            // 设置每个 Provider 需要用到的 Options
            // 在这里设置才能让框架正常在 --help 下展示，以及无论 Argument 和 Options 的顺序如何，都能正常解析
            foreach (var providerType in ProviderHelper.GetProviderTypes())
            {
                var providerOptions = ProviderHelper.GetProviderOptions(providerType);

                foreach (var option in providerOptions)
                {
                    addCmd.Option($"--{option.LongName}", option.Description, option.OptionType);
                }
            }

            addCmd.OnExecute(() =>
            {
                var provider = addCmd.Model.Provider;

                if (!ProviderHelper.CheckSupportability(provider))
                {
                    addCmd.Error.WriteLine($"Provider '{provider}' is not supported.");
                    return 1;
                }

                ProviderBase providerBase;

                try
                {
                    providerBase = ProviderHelper.GetInstanceByName(provider, addCmd);
                }
                catch (NotSupportedException ex)
                {
                    addCmd.Error.WriteLine(ex.Message);
                    addCmd.Error.WriteLine("Use `add --help` to view supported provider names");

                    return 0;
                }

                return AddCommandHelper.DoAdd(addCmd, providerBase);
            });

        };
    }
}