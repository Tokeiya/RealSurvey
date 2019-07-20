using System;
using System.IO;

namespace Analyze
{
	[Flags]
	internal enum WriteOn
	{
		None = 0x00,
		Console = 0x01,
		Stream = 0x02,
		Both = 0x03
	}


	internal class ReportWriter : IDisposable
	{
		private readonly StreamWriter _writer;

		public ReportWriter(string path, bool append) =>
			_writer = new StreamWriter(path ?? throw new ArgumentNullException(nameof(path)), append);


		public void WriteLine(string value, WriteOn writeOn = WriteOn.Both)
		{
			if ((writeOn & WriteOn.Console) == WriteOn.Console) Console.WriteLine(value);
			if ((writeOn & WriteOn.Stream) == WriteOn.Stream) _writer.WriteLine(value);
		}

		public void Write(string value, WriteOn writeOn = WriteOn.Both)
		{
			if ((writeOn & WriteOn.Console) == WriteOn.Console) Console.Write(value);
			if ((writeOn & WriteOn.Stream) == WriteOn.Stream) _writer.Write(value);
		}

		public void Dispose()
		{
			_writer?.Dispose();
		}
	}
}