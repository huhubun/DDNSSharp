using DDNSSharp.Commands;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.HelpText;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DDNSSharp
{
    [Command("ddnssharp")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(ListCommand),
        typeof(AddCommand),
        typeof(DeleteCommand)
        //typeof(ProviderCommand)
    )]
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication<Program>();
            app.HelpTextGenerator = new MyHelpTextGenerator();
            app.Conventions.UseDefaultConventions();

            app.Command("provider", providerConfig =>
            {
                providerConfig.Command("set", setConfig =>
                {

                });

                providerConfig.Command("delete", setConfig =>
                {

                });
            });

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

    class MyHelpTextGenerator : DefaultHelpTextGenerator
    {
        //public override void Generate(CommandLineApplication application, TextWriter output)
        //{
        //    base.Generate(application, output);
        //}

        //protected override void GenerateBody(CommandLineApplication application, TextWriter output)
        //{
        //    base.GenerateBody(application, output);
        //}

        protected override void GenerateOptions(CommandLineApplication application, TextWriter output, IReadOnlyList<CommandOption> visibleOptions, int firstColumnWidth)
        {
            base.GenerateOptions(application, output, visibleOptions, firstColumnWidth);

            if(application is CommandLineApplication<Commands.ProviderCommands.SetCommand>)
            {
                output.WriteLine("aaa");
                output.WriteLine("bbbcccccccccccccc");
                output.WriteLine("bbbdddd");
            }
        }
    }

}
