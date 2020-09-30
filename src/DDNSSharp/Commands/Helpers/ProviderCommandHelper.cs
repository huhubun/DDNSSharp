using System.IO;
using System.Linq;
using static DDNSSharp.Providers.ProviderHelper;

namespace DDNSSharp.Commands.Helpers
{
    public static class ProviderCommandHelper
    {
        /// <summary>
        /// 打印已经配置过的 Provider 列表
        /// </summary>
        /// <param name="output"></param>
        /// <param name="endWithNewLine"></param>
        public static void WriteAlreadyConfiguredProviderListToConsole(TextWriter output, bool endWithNewLine = true)
        {
            output.WriteLine("List of already configured providers:");

            var names = GetConfiguredProviderNames();

            if (names.Any())
            {
                foreach (var providerName in names)
                {
                    output.WriteLine($"  {providerName}");
                }
            }
            else
            {
                output.WriteLine("(There are no providers that have been set up. Use `provider set` command to set one.)");
            }

            WriteLine(output, endWithNewLine);
        }

        /// <summary>
        /// 打印当前支持配置的 Provider 列表
        /// </summary>
        /// <param name="output"></param>
        /// <param name="endWithNewLine"></param>
        public static void WriteSupportedProviderListToConsole(TextWriter output, bool endWithNewLine = true)
        {
            output.WriteLine("List of supported providers:");

            foreach (var name in GetProviderNames())
            {
                output.WriteLine($"  {name}");
            }

            WriteLine(output, endWithNewLine);
        }

        private static void WriteLine(TextWriter output, bool endWithNewLine = true)
        {
            if (endWithNewLine)
            {
                output.WriteLine();
            }
        }
    }
}
