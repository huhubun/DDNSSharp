using DDNSSharp.Configs;
using DDNSSharp.Enums;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;
using static DDNSSharp.Commands.Helpers.AddCommandHelper;

namespace DDNSSharp.Commands
{
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
        [Option(CommandOptionType.SingleValue, Description = "Domain record type. Default is 'A'.")]
        public DomainRecordType? Type { get; set; }

        /// <summary>
        /// DNS 提供商名称
        /// </summary>
        [Required]
        [Option(CommandOptionType.SingleValue, Description = "DNS provider name. Use `provider` command to view the list of supported providers.")]
        public string Provider { get; set; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (!ProviderHelper.CheckSupportability(Provider))
            {
                console.Error.WriteLine($"Provider '{Provider}' is not supported.");
                return 1;
            }

            var newConfigItem = new DomainConfigItem
            {
                Domain = Domain,
                Type = Type ?? DomainRecordType.A
                // 为了便于下面 GetSingle() 模糊查询，这里先不对 Provider 赋值
            };

            var item = DomainConfigHelper.GetSingle(newConfigItem);
            if (item != null)
            {
                console.Error.WriteLine("Failed to add, because the same configuration already exists: ");
                WriteConfigInfoToConsole(item, console.Error);
                console.Error.WriteLine("Only one record can be added for the same domain name and type, even if the provider is different.");

                return 1;
            }

            // 准备保存数据了，为 Provider 赋值
            newConfigItem.Provider = Provider;

            DomainConfigHelper.AddItem(newConfigItem);
            console.Out.WriteLine($"Added successfully!");

            return 1;
        }
    }
}
