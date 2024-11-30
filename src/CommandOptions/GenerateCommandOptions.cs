using System.ComponentModel;
using Spectre.Console.Cli;

namespace Altium.GenSort.CommandOptions
{
    [Description("Generates example file for testing purposes")]
    internal class GenerateCommandOptions : CommandSettings
    {
        [Description("Size of the file that should be generated (in bytes)")]
        [CommandArgument(0, "<size>")]
        public ulong Size { get; set; }

        [Description("Input dictionary used for string generation (if nothing specified will generate random strings)")]
        [CommandOption("-i|--input")]
        public string? InputPath { get; set; }

        [Description("Output file")]
        [CommandOption("-o|--output")]
        [DefaultValue("output.txt")]
        public string OutputPath { get; set; } = null!;

        [CommandOption("--progress")]
		[Description("Shows generation progress -> not recommended for large generations as its slows down execution")]
		public bool? ShowProgress { get; set; }

	}
}