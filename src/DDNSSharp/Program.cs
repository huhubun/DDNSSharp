using DDNSSharp.Commands;
using DDNSSharp.HelpText;
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
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication<Program>();
            app.HelpTextGenerator = new DDNSSharpHelpTextGenerator();
            app.Conventions.UseDefaultConventions();

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
