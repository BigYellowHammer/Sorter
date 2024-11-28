using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Altium.Generator
{
	[ExcludeFromCodeCoverage]
	internal class FileHandler : IDisposable, IFileHandler
	{
		private string path;
		private ulong bytesWritten;
		private string fullPath;

		private StreamWriter writer;

		public ulong BytesWritten
		{
			get
			{
				return bytesWritten;
			}
		}

		public string FullPath
		{
			get
			{
				return fullPath;
			}
		}

		public void Configure(string path)
		{
			this.path = path;
			this.fullPath = Path.GetFullPath(path);
			bytesWritten = 0;
			writer = new StreamWriter(path, false);
		}

		public void Write(string text)
		{
			try
			{
				writer.WriteLine(text);
				bytesWritten += (ulong)System.Text.Encoding.UTF8.GetBytes(text).Length;

			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}

		}

		public string ReadAllText(string inputPath)
		{
			return File.ReadAllText(inputPath);
		}

		public void Dispose()
		{
			writer.Dispose();
		}
	}
}