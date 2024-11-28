namespace Altium.Generator
{
	public interface IFileHandler
	{
		ulong BytesWritten { get; }
		string FullPath { get; }

		void Configure(string path);
		void Dispose();
		void Write(string text);
		string ReadAllText(string inputPath);
	}
}