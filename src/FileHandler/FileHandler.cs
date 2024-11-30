using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Altium.GenSort
{
	[ExcludeFromCodeCoverage]
	internal class FileHandler : IDisposable, IFileHandler
	{
		private ulong bytesWritten = 0;
		private string fullPath = string.Empty;
		private StreamWriter writer = default!;
		private Dictionary<string, StreamWriter> chunks = new Dictionary<string, StreamWriter>();
		private bool _disposed = false;

		private const string TEMP_DIRECTORY = "temp";

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

		public string[] ChunkNames
		{
			get
			{
				return chunks.Keys.ToArray();
			}
		}

		public void Configure(string path)
		{
			this.fullPath = Path.GetFullPath(path);
			bytesWritten = 0;
			writer = new StreamWriter(path, false);
			chunks = new Dictionary<string, StreamWriter>();

			Directory.CreateDirectory(TEMP_DIRECTORY);
		}

		public void SaveLineIntoChunk(string chunkName, string line)
		{
			if (chunks.ContainsKey(chunkName))
			{
				chunks[chunkName].WriteLine(line);
			}
			else
			{
				var sw = new StreamWriter($"{TEMP_DIRECTORY}/{chunkName}.txt", false);
				sw.WriteLine(line);
				chunks[chunkName] = sw;
			}
		}

		public void CloseAllChunks()
		{
			foreach (var chunk in chunks.Keys)
			{
				chunks[chunk].Close();
			}
		}

		public string[] ReadChunkLines(string chunkName)
		{
			return File.ReadAllLines($"{TEMP_DIRECTORY}/{chunkName}.txt");
		}

		public void SaveLinesIntoChunk(string chunkName, string[] sortedLines)
		{
			File.WriteAllLines($"{TEMP_DIRECTORY}/{chunkName}.txt", sortedLines);
		}


		public void WriteLines(string[] lines)
		{
			foreach (var line in lines)
			{
				writer.WriteLine(line);
			}
		}

		public void Write(string text)
		{
			try
			{
				writer.WriteLine(text);
				bytesWritten += (ulong)Encoding.UTF8.GetBytes(text).Length;

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

		public IEnumerable<string> ReadLines(string inputPath)
		{
			return File.ReadLines(inputPath);
		}

		public void DisposeInternal()
		{
			if (_disposed)
				return;


			writer.Dispose();
			foreach (var key in chunks.Keys)
			{
				chunks[key].Dispose();
			}

			if (Directory.Exists(TEMP_DIRECTORY))
			{
				DirectoryInfo di = new DirectoryInfo(TEMP_DIRECTORY);
				di.Delete(true);
			}

			_disposed = true;
		}

		public void Dispose()
		{
			DisposeInternal();
			GC.SuppressFinalize(this);
		}

		~FileHandler()
		{
			DisposeInternal();
		}
	}
}