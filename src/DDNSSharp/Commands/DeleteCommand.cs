using DDNSSharp.Configs;
using DDNSSharp.Enums;
using DDNSSharp.Providers;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DDNSSharp.Commands
{
    class DeleteCommand
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
        [Option(CommandOptionType.SingleValue, Description = "DNS provider name. Use `provider` command to view the list of supported providers.")]
        public string Provider { get; set; }

        int OnExecute(CommandLineApplication app, IConsole console)
        {
            if (!String.IsNullOrEmpty(Provider) && !ProviderHelper.CheckSupportability(Provider))
            {
                console.Error.WriteLine($"Provider '{Provider}' is not supported.");
                return 1;
            }

            var item = DomainConfigHelper.GetSingle(new DomainConfigItem
            {
                Domain = Domain,
                Type = Type ?? DomainRecordType.A,
                Provider = Provider
            });

            if (item == null)
            {
                console.Error.WriteLine("Failed to delete, because there is no such configuration.");
                return 1;
            }

            var confirmMessage = new StringBuilder("Please confirm that you want to delete this record:");
            confirmMessage.AppendLine();
            confirmMessage.AppendLine($"  Domain: {item.Domain}");
            confirmMessage.AppendLine($"  Type: {item.Type}");
            confirmMessage.AppendLine($"  Provider: {item.Provider}");

            if (!Prompt.GetYesNo(confirmMessage.ToString(), defaultAnswer: false))
            {
                console.Out.WriteLine("Operation cancelled.");
                return 1;
            }

            DomainConfigHelper.DeleteItem(item);
            console.Out.WriteLine($"Deleted successfully!");

            return 1;
        }
    }
}
