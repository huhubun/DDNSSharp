using DDNSSharp.Configs;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Linq;
using static DDNSSharp.Configs.DomainConfigHelper;

namespace DDNSSharp.Commands
{
    class SyncCommand
    {
        /// <summary>
        /// 强制刷新
        /// </summary>
        [Option(CommandOptionType.NoValue, Description = "Force synchronization, even if the IP address has not changed.")]
        public bool Force { get; set; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            var group = GetConfigs().GroupBy(c => c.Provider);

            if (!group.Any())
            {
                console.Out.WriteLine("No domain name is currently configured, please add domain name information via the `add` command.");
            }

            foreach (var domainConfigItems in group)
            {
                var provider = ProviderHelper.GetInstanceByName(domainConfigItems.Key, app);

                var items = domainConfigItems.AsEnumerable();

                if (!Force)
                {
                    var ignores = items.Where(i => i.IsIPChanged() == false);
                    OutputIgnoreDomainList(ignores, console);

                    // 非强制刷新的情况下，只将 IP 地址变化的内容传给域名解析服务商
                    items = items.Where(i => i.IsIPChanged());
                }

                provider.Sync(items);
            }

            if (group.Any())
            {
                // TODO 考虑在这里收集同步信息，展示成功、失败的数量
                console.Out.WriteLine("Synchronization is complete.");
            }

            return 0;
        }

        void OutputIgnoreDomainList(IEnumerable<DomainConfigItem> domainConfigItems, IConsole console)
        {
            if (domainConfigItems.Any())
            {
                console.Out.WriteLine("The following domain names will not be synchronized because their IP addresses have not changed.");
            }

            foreach (var item in domainConfigItems)
            {
                // TODO 输出内容美化
                console.Out.WriteLine($"{item.Domain} | {item.Type}");
            }

            if (domainConfigItems.Any())
            {
                console.Out.WriteLine();
            }
        }
    }
}
