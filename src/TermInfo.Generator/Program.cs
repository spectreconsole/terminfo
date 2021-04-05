using Generator.Commands;
using Spectre.Console.Cli;

namespace Generator
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandApp<GenerateCommand>();
            return app.Run(args);
        }
    }
}
