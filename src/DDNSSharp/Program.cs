using DDNSSharp.Commands;
using DDNSSharp.Commands.AddCommands;
using McMaster.Extensions.CommandLineUtils;
using System.Reflection;

namespace DDNSSharp
{
    [Command("ddnssharp")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ListCommand),
        typeof(DeleteCommand),
        typeof(SyncCommand),
        typeof(IpCommand)
    )]

    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication<Program>();
            app.Conventions.UseDefaultConventions();

            // 由于需要动态加载 Provider 的 Options，所以 Add 命令通过 Builder API 创建
            app.Command<AddCommandModel>("add", AddCommand.Command);

            return app.Execute(args);
        }

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
