using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

// ReSharper disable InconsistentNaming

namespace SurveyGrocery
{
	public static class RuntimeEnvironment
	{
		public static string CPU { get; } = CpuInfo.GetCpuName();
		public static string ProcessArchitecture { get; } = RuntimeInformation.ProcessArchitecture.ToString();
		public static string OSArchitecture { get; } = RuntimeInformation.OSArchitecture.ToString();
		public static string OSDescription { get; } = RuntimeInformation.OSDescription;

		public static string FrameworkDescription { get; } = RuntimeInformation.FrameworkDescription;

		public static void WriteIniInMarkdown(TextWriter writer,long sessionId)
		{
			writer.WriteLine("```ini");
			writer.WriteLine($"SessionId={sessionId}");
			writer.WriteLine($"CPU={CPU}");
			writer.WriteLine($"OS={OSDescription}");
			writer.WriteLine($"OSArchitecture={OSArchitecture}");
			writer.WriteLine($"Framework={FrameworkDescription}");
			writer.WriteLine($"ProcessArchitecture={ProcessArchitecture}");
			writer.WriteLine("```");
		}


	}
}
