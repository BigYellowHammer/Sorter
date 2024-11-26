using Spectre.Console;
using Spectre.Console.Cli;


namespace Altium.Generator.CommandOptions
{
    public class GenerateCommand : Command<GenerateCommandOptions>
    {
        public override int Execute(CommandContext context, GenerateCommandOptions settings)
        {
            try
            {
                List<string> avaliableWords = new List<string>();
                if (!string.IsNullOrEmpty(settings.InputPath))
                {
                    string fileContent = File.ReadAllText(settings.InputPath);
                    avaliableWords = fileContent.Split([' ', '\t', '\r', '\n', ',', '.', ';', '!', '?'], StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                var random = new RandomnessGenerator(avaliableWords);

                using var writer = new FileWriter(settings.OutputPath);

                while (writer.BytesWritten < settings.Size)
                {
                    writer.Write(random.GenerateRow());                    
                }


            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Specified file was not found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to the file is denied.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return -1;
            }

            return 0;
        }
    }
}