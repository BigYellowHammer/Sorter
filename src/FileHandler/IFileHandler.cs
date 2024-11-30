namespace Altium.GenSort
{
	public interface IFileHandler
	{
		ulong BytesWritten { get; }
		string[] ChunkNames { get; }
		string FullPath { get; }

		void Configure(string path);
		void Dispose();
		string ReadAllText(string inputPath);
		string[] ReadChunkLines(string chunkName);
		IEnumerable<string> ReadLines(string inputPath);
		void SaveLineIntoChunk(string chunkName, string line);
		void SaveLinesIntoChunk(string chunkName, string[] sortedLines);
		void Write(string text);
		void WriteLines(string[] lines);
		void CloseAllChunks();
	}
}