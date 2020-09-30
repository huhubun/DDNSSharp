using McMaster.Extensions.CommandLineUtils;

namespace DDNSSharp.Providers
{
    class ProviderOption
    {
        /// <summary>
        /// 设置 Provider 的 Option 内容。
        /// </summary>
        /// <param name="template">模板，确保至少设置一个长名称（--xxx）或短名称（-x），该名称将用作 Provider 配置文件的节点名称。</param>
        /// <param name="description"></param>
        /// <param name="optionType"></param>
        public ProviderOption(string template, string description, CommandOptionType optionType)
        {
            Template = template;
            Description = description;
            OptionType = optionType;
        }

        public string Template { get; set; }

        public string Description { get; set; }

        public CommandOptionType OptionType { get; set; }
    }
}
