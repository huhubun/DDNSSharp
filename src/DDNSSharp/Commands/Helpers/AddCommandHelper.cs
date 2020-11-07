using DDNSSharp.Commands.AddCommands;
using DDNSSharp.Configs;
using DDNSSharp.Enums;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using static DDNSSharp.Helpers.IPHelper;

namespace DDNSSharp.Commands.Helpers
{
    public static class AddCommandHelper
    {
        public static int DoAdd(CommandLineApplication<AddCommandModel> addCmd, ProviderBase providerBase)
        {
            var newConfigItem = new DomainConfigItem
            {
                Domain = addCmd.Model.Domain,
                Type = addCmd.Model.Type ?? DomainRecordType.A
                // 为了便于下面 GetSingle() 模糊查询，这里先不对 Provider 赋值
            };

            // 根据 域名+类型 判断，是否有重复的
            var item = DomainConfigHelper.GetSingle(newConfigItem);
            if (item != null)
            {
                addCmd.Error.WriteLine("Failed to add, because the same configuration already exists: ");
                WriteConfigInfoToConsole(item, addCmd.Error);
                addCmd.Error.WriteLine("Only one record can be added for the same domain name and type, even if the provider is different.");

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
                return FailedWithAvailableNetworkInterface(addCmd.Error);
            }

            if (!String.IsNullOrEmpty(addCmd.Model.Interface))
            {
                iface = interfaces.SingleOrDefault(i => i.Name == addCmd.Model.Interface);

                if (iface == null)
                {
                    return FailedWithInterfaceNameNotFound(addCmd.Error);
                }
            }
            else if (interfaces.Count == 1)
            {
                iface = interfaces.Single();

                if (iface.Name != addCmd.Model.Interface)
                {
                    return FailedWithInterfaceNameNotFound(addCmd.Error);
                }
            }
            else
            {
                return FailedWithMultipleInterfaceNameFoundButNoSpecify(addCmd.Error);
            }

            // 准备保存数据了，为 Provider 等内容赋值
            newConfigItem.Provider = addCmd.Model.Provider;
            newConfigItem.Interface = iface.Name;

            // 保存访问 DNS 提供商 API 时需要用到的参数信息
            var providerOptionNames = ProviderHelper.GetProviderOptions(providerBase.GetType()).Select(o => o.LongName);
            newConfigItem.ProviderInfo = addCmd.Options
                                            .Where(o => providerOptionNames.Contains(o.LongName))
                                            .ToDictionary(k => k.LongName, v => String.Join('|', v.Values));

            DomainConfigHelper.AddItem(newConfigItem);
            addCmd.Out.WriteLine($"Added successfully!");

            return 1;
        }

        static int FailedWithAvailableNetworkInterface(TextWriter writer)
        {
            writer.WriteLine($"Failed to add, no available network interface.");
            return 0;
        }

        static int FailedWithInterfaceNameNotFound(TextWriter writer)
        {
            writer.WriteLine($"Failed to add, the specified interface name was not found.");
            return 0;
        }

        static int FailedWithMultipleInterfaceNameFoundButNoSpecify(TextWriter writer)
        {
            writer.WriteLine($"Failed to add, multiple interface found.");
            writer.WriteLine($"Use `ip` command to get available interface name list and use `--interface` option to specify one.");
            return 0;
        }

        public static void WriteConfigInfoToConsole(DomainConfigItem item, TextWriter output)
        {
            output.WriteLine($"  Domain: {item.Domain}");
            output.WriteLine($"  Type: {item.Type}");
            output.WriteLine($"  Provider: {item.Provider}");
        }
    }
}
