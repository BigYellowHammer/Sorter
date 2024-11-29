using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.RegularExpressions;


namespace Altium.Generator.CommandOptions
{
    internal class SortCommand : Command<SortCommandOptions>
    {
        private IFileHandler _fileHandler;

        public SortCommand(IFileHandler fileHandler) 
        { 
            _fileHandler = fileHandler;
        }
        
        public override int Execute(CommandContext context, SortCommandOptions settings)
        {
            try
            {
				AnsiConsole.MarkupLine($"[olive]Reading input[/]");

				_fileHandler.Configure(settings.OutputPath);

				var pattern = @"^\d+\.\s(\w.).*$";
				Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

				foreach (var line in _fileHandler.ReadLines(settings.InputPath))
                {
					Match m = r.Match(line);
                    _fileHandler.SaveLineIntoChunk(m.Groups[1].Value.ToLower(), line);
				}

                _fileHandler.CloseAllChunks();

				AnsiConsole.MarkupLine($"[olive]Sorting chunks[/]");

                Parallel.ForEach(_fileHandler.ChunkNames, chunk =>
                {
                    var lines = _fileHandler.ReadChunkLines(chunk);
					var sortedLines = CustomSort(lines);
                    _fileHandler.SaveLinesIntoChunk(chunk, sortedLines);

				});

				AnsiConsole.MarkupLine($"[olive]Merging files[/]");

                var sortedChunks = _fileHandler.ChunkNames.OrderBy(c => c);
                foreach(var chunk in sortedChunks)
                {
                    var lines = _fileHandler.ReadChunkLines(chunk);
                    _fileHandler.WriteLines(lines);
                }

				AnsiConsole.MarkupLine($"[olive]{_fileHandler.FullPath} created[/]");

			}
            catch (FileNotFoundException)
            {
                Console.WriteLine("Specified file was not found.");
                return -3;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to the file is denied.");
                return -2;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return -1;
            }
            finally
            {
                _fileHandler.Dispose();
			}

            return 0;
        }

		static string[] CustomSort(string[] lines)
		{
			var lineList = new List<(ulong Number, string Text)>();

			foreach (var line in lines)
			{
				var parts = line.Split(['.'], 2);
                lineList.Add((ulong.Parse(parts[0]), parts[1].Trim()));
			}

            var sortedLines = lineList
                .OrderBy(l => l.Text)
                .ThenBy(l => l.Number)
                .Select(l => $"{l.Number}. {l.Text}")
                .ToArray();

			return sortedLines;
		}
	}
}