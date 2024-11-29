using Spectre.Console;
using Spectre.Console.Cli;


namespace Altium.Generator.CommandOptions
{
    internal class GenerateCommand : Command<GenerateCommandOptions>
    {
        private IFileHandler _fileHandler;
        private IRandomnessGenerator _randomnessGenerator; 

        public GenerateCommand(IFileHandler fileHandler, IRandomnessGenerator randomnessGenerator) 
        { 
            _fileHandler = fileHandler;
            _randomnessGenerator = randomnessGenerator;
        }
        
        public override int Execute(CommandContext context, GenerateCommandOptions settings)
        {
            try
            {
                var avaliableWords = GetAvaliableWords(settings.InputPath);

				_randomnessGenerator.Configure(avaliableWords);

                _fileHandler.Configure(settings.OutputPath);

				AnsiConsole.MarkupLine($"[lime]Generation started[/]");

				while (_fileHandler.BytesWritten < settings.Size)
                {
					_fileHandler.Write(_randomnessGenerator.GenerateRow());
                    
                    if(settings.ShowProgress.HasValue && settings.ShowProgress.Value)
					    Task.Run(() => Console.Write($"\r Progress:   {(double)_fileHandler.BytesWritten / settings.Size:P}"));
				}

				AnsiConsole.MarkupLine($"[olive]{_fileHandler.BytesWritten} bytes written[/]");
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

        private List<string> GetAvaliableWords(string inputPath)
        {
			List<string> avaliableWords = [];
			if (!string.IsNullOrEmpty(inputPath))
			{
				string fileContent = _fileHandler.ReadAllText(inputPath);
				avaliableWords = fileContent.Split([' ', '\t', '\r', '\n', ',', '.', ';', '!', '?'], StringSplitOptions.RemoveEmptyEntries).ToList();
                AnsiConsole.MarkupLine($"[lime]Words from {Path.GetFullPath(inputPath)} loaded[/]");
            }

            return avaliableWords;
		}
    }
}