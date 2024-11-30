using System.ComponentModel;
using Spectre.Console.Cli;

namespace Altium.GenSort.CommandOptions
{
    [Description("Sort input file using multi-key sorting algorithm ")]
    internal class SortCommandOptions : CommandSettings
    {
        [Description("Input file that needs to be sorted")]
        [CommandArgument(0, "<input>")]
        public string? InputPath { get; set; }

        [Description("Output file")]
        [CommandOption("-o|--output")]
        [DefaultValue("sorted.txt")]
        public string OutputPath { get; set; } = null!;

        [CommandOption("--progress")]
		[Description("Shows generation progress -> not recommended for large generations as its slows down execution")]
		public bool? ShowProgress { get; set; }

	}
}