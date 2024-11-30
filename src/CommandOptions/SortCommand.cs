using Altium.GenSort.Logger;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Altium.GenSort.CommandOptions
{
    internal class SortCommand : Command<SortCommandOptions>
    {
        private readonly IFileHandler _fileHandler;

        public SortCommand(IFileHandler fileHandler) 
        { 
            _fileHandler = fileHandler;
        }
        
        public override int Execute(CommandContext context, SortCommandOptions settings)
        {
            try
            {

	            _fileHandler.Configure(settings.OutputPath);

				using (new PerformanceLogger("Reading input"))
				{
					foreach (var line in _fileHandler.ReadLines(settings.InputPath!))
					{
						var parts = line.Split(['.'],  2);

						_fileHandler.SaveLineIntoChunk(parts[1].Substring(1, 3).ToLower(), line);
					}
					_fileHandler.CloseAllChunks();
				}

				using (new PerformanceLogger("Sorting chunks"))
				{
					int totalChunks = _fileHandler.ChunkNames.Length;
					int completedChunks = 0;

					if (settings.ShowProgress.HasValue && settings.ShowProgress.Value)
					{
						Console.WriteLine();
					}
					
					Parallel.ForEach(_fileHandler.ChunkNames, chunk =>
					{
						var lines = _fileHandler.ReadChunkLines(chunk);
						var sortedLines = CustomSort(lines);
						_fileHandler.SaveLinesIntoChunk(chunk, sortedLines);

						if (settings.ShowProgress.HasValue && settings.ShowProgress.Value)
						{
							var progress = Interlocked.Increment(ref completedChunks);
							Console.Write($"\r Progress:   {(double)progress / totalChunks:P}");
						}
					});

					if (settings.ShowProgress.HasValue && settings.ShowProgress.Value)
					{
						AnsiConsole.Markup($"[olive]\nSorting chunks [/]");
					}
				}

				using (new PerformanceLogger("Merging files"))
				{
					var sortedChunks = _fileHandler.ChunkNames.OrderBy(c => c);
					foreach (var chunk in sortedChunks)
					{
						var lines = _fileHandler.ReadChunkLines(chunk);
						_fileHandler.WriteLines(lines);
					}
				}


				AnsiConsole.MarkupLine($"[grey]{_fileHandler.FullPath} created[/]");

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

		internal string[] CustomSort(string[] lines)
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