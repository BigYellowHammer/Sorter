using Altium.Generator.CommandOptions;
using Spectre.Console.Cli;
using Spectre.Console;
using Microsoft.Extensions.DependencyInjection;
using GenSort.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Altium.Generator
{
	[ExcludeFromCodeCoverage]
	public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var startTime = DateTime.UtcNow;

            AnsiConsole.Write(new FigletText("GenSort").Color(Color.Purple));

            var app = new CommandApp(TypeRegistrations());
            
            app.Configure(x => x.SetApplicationName("GenSort")
                .AddCommand<GenerateCommand>("generate")
                .WithExample( new[]
                {
                    "generate",
                    "1024"
                })
                .WithExample( new[]
                {
                    "generate",
                    "1024",
                    "-i ../samples/words.txt"
                })
                .WithExample( new[]
                {
                    "generate",
                    "1024",
                    "-i ../samples/words.txt",
                    "-o customOutputFile.txt"
                })
				.WithExample(new[]
				{
					"generate",
					"1024",
					"-i ../samples/words.txt",
					"-o customOutputFile.txt",
                    "--progress"
				})
                .WithDescription("Generate file for sorting"));
            
            var exitCode = await app.RunAsync(args);

            AnsiConsole.MarkupLine($"[lime]Execution time {(DateTime.UtcNow - startTime).TotalSeconds:N}s[/]");

            return exitCode;
        }

        private static TypeRegistrar TypeRegistrations()
        {
			var services = new ServiceCollection();

			services.AddTransient<IFileHandler, FileHandler>();
			services.AddTransient<IRandomnessGenerator, RandomnessGenerator>();

			return new TypeRegistrar(services);
		}

	}
}