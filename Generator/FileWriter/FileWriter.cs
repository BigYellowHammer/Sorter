using System.Text;

namespace Altium.Generator
{
    public class FileWriter : IDisposable
    {
        private string path;
        private ulong bytesWritten;

        private StreamWriter writer;

        public ulong BytesWritten
        {
            get
            {
                return bytesWritten;
            }
        }

        public FileWriter(string path)
        {
            this.path = path;
            bytesWritten = 0;
            writer = new StreamWriter(path, true);
        }

        public void Write(string text)
        {
            try
            {
                writer.WriteLine(text);
                bytesWritten += (ulong)text.Length;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

        }

        public void Dispose()
        {
            writer.Dispose();
        }
    }
}