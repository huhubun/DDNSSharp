using DDNSSharp.Configs;
using DDNSSharp.Enums;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.NetworkInformation;
using static DDNSSharp.Commands.Helpers.AddCommandHelper;
using static DDNSSharp.Commands.Helpers.ProviderCommandHelper;
using static DDNSSharp.Helpers.IPHelper;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Commands
{
    [Command(UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    class AddCommand
    {
        /// <summary>
        /// 域名
        /// </summary>
        [Required]
        [Argument(0, Description = "Domain name")]
        public string Domain { get; }

        /// <summary>
        /// 域名记录类型
        /// </summary>
        [Option(CommandOptionType.SingleValue, Description = "Domain record type. Default is 'A'.\nAllowed values are: A, AAAA.")]
        public DomainRecordType? Type { get; set; }

        /// <summary>
        /// DNS 提供商名称
        /// </summary>
        [Required]
        [Option(CommandOptionType.SingleValue, Description = "DNS provider name. Use `provider` command to view the list of supported providers.")]
        public string Provider { get; set; }

        [Option(CommandOptionType.SingleValue, Description = "Network interface name.")]
        public string Interface { get; set; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (!ProviderHelper.CheckSupportability(Provider))
            {
                console.Error.WriteLine($"Provider '{Provider}' is not supported.");
                return 1;
            }



            ProviderBase provider;

            try
            {
                provider = GetInstanceByName(Provider, app);
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



            return 0;

            var newConfigItem = new DomainConfigItem
            {
                Domain = Domain,
                Type = Type ?? DomainRecordType.A
                // 为了便于下面 GetSingle() 模糊查询，这里先不对 Provider 赋值
            };

            // 根据 域名+类型 判断，是否有重复的
            var item = DomainConfigHelper.GetSingle(newConfigItem);
            if (item != null)
            {
                console.Error.WriteLine("Failed to add, because the same configuration already exists: ");
                WriteConfigInfoToConsole(item, console.Error);
                console.Error.WriteLine("Only one record can be added for the same domain name and type, even if the provider is different.");

                return 1;
            }

            // 获取网卡信息
            // 如果只有一个网卡，并且用户没有设置 interface 选项，那么该域名就与该网卡绑定，使用该网卡的 IP 地址
            // 如果只有一个网卡，并且用户设置了 interface 选项，先判断可用网卡和用户设置的是否一致，如果一致则使用；不一致则报错
            // 如果有多个网卡，并且用户没有设置 interface 选项，报错
            // 如果有多个网卡，并且用户设置了 interface 选项，判断用户设置的网卡是否可用，如果不可用则报错
            NetworkInterface iface;
            var interfaces = GetAvailableNetworkInterfaces();

            if (!interfaces.Any())
            {
                return FailedWithAvailableNetworkInterface(console);
            }

            if (!String.IsNullOrEmpty(Interface))
            {
                iface = interfaces.SingleOrDefault(i => i.Name == Interface);

                if (iface == null)
                {
                    return FailedWithInterfaceNameNotFound(console);
                }
            }
            else if (interfaces.Count == 1)
            {
                iface = interfaces.Single();

                if (iface.Name != Interface)
                {
                    return FailedWithInterfaceNameNotFound(console);
                }
            }
            else
            {
                return FailedWithMultipleInterfaceNameFoundButNoSpecify(console);
            }

            // 准备保存数据了，为 Provider 等内容赋值
            newConfigItem.Provider = Provider;
            newConfigItem.Interface = iface.Name;

            DomainConfigHelper.AddItem(newConfigItem);
            console.Out.WriteLine($"Added successfully!");

            return 1;
        }

        int FailedWithAvailableNetworkInterface(IConsole console)
        {
            console.Error.WriteLine($"Failed to add, no available network interface.");
            return 0;
        }

        int FailedWithInterfaceNameNotFound(IConsole console)
        {
            console.Error.WriteLine($"Failed to add, the specified interface name was not found.");
            return 0;
        }

        int FailedWithMultipleInterfaceNameFoundButNoSpecify(IConsole console)
        {
            console.Error.WriteLine($"Failed to add, multiple interface found.");
            console.Error.WriteLine($"Use `ip` command to get available interface name list and use `--interface` option to specify one.");
            return 0;
        }
    }
}
