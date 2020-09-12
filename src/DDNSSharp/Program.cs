using DDNSSharp.Commands;
using McMaster.Extensions.CommandLineUtils;
using System.Reflection;

namespace DDNSSharp
{
    [Command("ddnssharp")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ListCommand),
        typeof(AddCommand),
        typeof(DeleteCommand),
        typeof(ProviderCommand)
    )]

    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        protected int OnExecute(CommandLineApplication app)
        {
            //// 光执行程序但什么指令都不输入时，返回帮助信息
            //app.ShowHelp();

            return 1;
        }

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
