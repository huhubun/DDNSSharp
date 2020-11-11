using DDNSSharp.Commands.SyncCommands;
using DDNSSharp.Configs;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Linq;
using static DDNSSharp.Configs.DomainConfigHelper;

namespace DDNSSharp.Commands
{
    [Command(Description = "执行同步，将本机 IP 信息更新到域名解析提供商")]
    class SyncCommand
    {
        /// <summary>
        /// 强制刷新
        /// </summary>
        [Option(CommandOptionType.NoValue, Description = "Force synchronization, even if the IP address has not changed.")]
        public bool Force { get; set; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            var configs = GetConfigs();

            if (!configs.Any())
            {
                console.Out.WriteLine("No domain name is currently configured, please add domain name information via the `add` command.");
                return 0;
            }

            foreach (var item in configs)
            {
                var provider = ProviderHelper.GetInstanceByName(item.Provider, app);
                console.Out.WriteLine($"Current domain: {item.Domain}");

                // 非强制刷新的情况下，只将 IP 地址变化的内容传给域名解析提供商
                if (item.IsIPChanged() || Force)
                {
                    try
                    {
                        provider.Sync(new SyncContext
                        {
                            DomainConfigItem = item
                        });
                    }
                    catch (Exception ex)
                    {
                        item.LastSyncStatus = SyncStatus.Failure;

                        console.Error.WriteLine($"{item.Domain} update failed");
                        console.Error.WriteLine(ex);
                    }
                }
                else
                {
                    console.Out.WriteLine($"This domain names will not be synchronized because their IP addresses have not changed. Its current record is {item.LastSyncSuccessCurrentIP}. If there is a difference with the provider, please use `--force` to force synchronization.");

                    item.LastSyncStatus = SyncStatus.Ignore;
                }

                UpdateItem(item);

                console.Out.WriteLine("--------------------");
            }

            console.Out.WriteLine();

            var results = GetConfigs().OrderBy(c => c.Provider);

            console.Out.WriteLine($"Synchronization is complete. Total = {results.Count()} (Success = {results.Count(i => i.LastSyncStatus == SyncStatus.Success)}, Failure = {results.Count(i => i.LastSyncStatus == SyncStatus.Failure)}, Ignore = {results.Count(i => i.LastSyncStatus == SyncStatus.Ignore)}).");

            foreach (var item in results)
            {
                var changes = item.LastSyncStatus == SyncStatus.Success ? $"{item.LastSyncSuccessOriginalIP} -> {item.LastSyncSuccessCurrentIP}" : "--";

                console.Out.WriteLine($"{item.Domain} | {item.Type} | {item.Provider} | {item.LastSyncStatus} | {changes}");
            }

            return 0;
        }
    }
}
