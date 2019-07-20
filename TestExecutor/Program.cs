using SurveyGrocery;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using env = SurveyGrocery.RuntimeEnvironment;

namespace TestExecutor
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var ver = Assembly.GetExecutingAssembly().GetName().Version;

			Console.WriteLine($"TestExecutor ver{ver.Major}.{ver.Minor}.{ver.Revision}");



			var now = DateTimeOffset.Now;
			var dir = Directory.CreateDirectory($"{Path.GetTempPath()}/{now:yyMMdd_HHmmss}");

			try
			{
				using (var writer = new StreamWriter($"{dir.FullName}/Environment.txt"))
				{
					env.WriteIniInMarkdown(writer, now.Ticks);
				}

				using (var writer = new StreamWriter($"{dir.FullName}/Result.tsv"))
				{
					writer.WriteLine(TestAngle.Header);

					foreach (var angle in FixedAngles.Angles.Select(x => new TestAngle(x.degrees, x.radians)))
					{
						angle.WriteTsv(writer, now.Ticks);
					}
				}

				ZipFile.CreateFromDirectory(dir.FullName, $"{now:yyMMdd_HHmmss}.zip", CompressionLevel.Optimal,
					false);

				var repDir = new DirectoryInfo($"{now:yyMMdd_HHmmss}.zip");

				Console.WriteLine($"{repDir.FullName} を作成しました。");
				Console.WriteLine("恐れ入りますが、作成されたZipファイルを、https://github.com/Tokeiya/RealSurvey/issues　までご提供ください。");
			}
			finally
			{
				dir.Delete(true);
			}
		}


	}
}