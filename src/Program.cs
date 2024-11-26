using Altium.Generator.CommandOptions;
using Spectre.Console.Cli;
using Spectre.Console;

namespace Altium.Generator
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var startTime = DateTime.UtcNow;

            AnsiConsole.Write(new FigletText("GenSort").Color(Color.Purple));
            var app = new CommandApp();
            
            app.Configure(x => x
                .AddCommand<GenerateCommand>("generate")
                .WithExample( new[]
                {
                    "generate",
                    "1024"
                }));

            var exitCode = await app.RunAsync(args);

            AnsiConsole.MarkupLine($"[lime]Execution time {(DateTime.UtcNow - startTime).TotalSeconds:N}s[/]");

            return exitCode;
        }
    }
}