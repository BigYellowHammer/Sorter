using Altium.Generator.CommandOptions;
using Spectre.Console.Cli;

namespace Altium.Generator
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var startTime = DateTime.UtcNow;

            var app = new CommandApp<GenerateCommand>();

            var exitCode = await app.RunAsync(args);

            Console.WriteLine($"Execution time: {(DateTime.UtcNow - startTime).TotalSeconds:N}s");

            return exitCode;
        }
    }
}