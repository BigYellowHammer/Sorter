using Spectre.Console;
using System.Diagnostics.CodeAnalysis;


namespace Altium.GenSort.Logger
{
	[ExcludeFromCodeCoverage]
	internal class PerformanceLogger : IDisposable
	{
		private readonly DateTime _executionStart;
		private bool _disposed;

		public PerformanceLogger(string action)
		{
			_executionStart = DateTime.UtcNow;
			AnsiConsole.Markup($"[olive]{action} [/]");
		}

		private void DisposeInternal()
		{
			if (_disposed)
				return;

			AnsiConsole.MarkupLine($"[lime]{(DateTime.UtcNow - _executionStart).TotalSeconds:N}s[/]");

			_disposed = true;
		}

		public void Dispose()
		{
			DisposeInternal();
			GC.SuppressFinalize(this);
		}

		~PerformanceLogger()
		{
			DisposeInternal();
		}
	}
}
