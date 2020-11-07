using McMaster.Extensions.CommandLineUtils;
using System;

namespace DDNSSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ProviderOptionAttribute : Attribute
    {
        /// <summary>
        /// 长名称，用于生成 Options 的名称。例如设置为 `id`，则会生成 --id 参数。
        /// 如果属性设置了 `JsonPropertyName` Attribute，则将 `JsonPropertyName` 设为长名称。
        /// </summary>
        public string LongName { get; set; }

        /// <summary>
        /// 描述，用于生成 Options 的描述。
        /// </summary>
        public string Description { get; set; }

        public CommandOptionType OptionType { get; set; } = CommandOptionType.SingleValue;
    }
}
