using DDNSSharp.Enums;
using McMaster.Extensions.CommandLineUtils;
using System.ComponentModel.DataAnnotations;

namespace DDNSSharp.Commands.AddCommands
{
    public class AddCommandModel
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
        [Option(CommandOptionType.SingleValue, Description = "DNS provider name.")]
        public string Provider { get; set; }

        /// <summary>
        /// 网卡名称
        /// </summary>
        [Option(CommandOptionType.SingleValue, Description = "Network interface name.")]
        public string Interface { get; set; }
    }
}
