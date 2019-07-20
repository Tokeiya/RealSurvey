using System;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;
using Dapper;

namespace Analyze
{


	public static class Comparator
	{
		static bool RelativeCompare((Result[] results, Session session) first,
			(Result[] results, Session session) second, Expression<Func<Result, double>> selector, ReportWriter writer)
		{
			var firstEnv = $"{first.session.ProcessArchitecture} {first.session.CPU}({first.session.Generation})";
			var secondEnv = $"{second.session.ProcessArchitecture} {second.session.CPU}({second.session.Generation})";

			var category = NameBuilder.BuildName(selector);
			var compiled = selector.Compile();

			if (first.results.Length != second.results.Length)
			{
				writer.WriteLine(
					$"Length mismatch! first:{first.results.Length} second:{second.results.Length}");
				return false;
			}


			for (int i = 0; i < first.results.Length; i++)
			{

				if (first.results[i].InputRadians != second.results[i].InputRadians ||
				    first.results[i].InputDegrees != second.results[i].InputDegrees)
				{
					writer.WriteLine($"Input is mismatch!");
					writer.WriteLine(
						$"FirstInputDegrees:{first.results[i].InputDegrees} FirstInputRadians:{first.results[i].InputRadians}");
					writer.WriteLine(
						$" SecondInputDegrees{second.results[i].InputDegrees} SecondInputRadians{second.results[i].InputRadians}");

					return false;
				}

				var f = compiled(first.results[i]);
				var s = compiled(second.results[i]);



				if (f != s)
				{
					var diff = Math.Abs(f - s);

					writer.WriteLine(
						$"{firstEnv}\t{secondEnv}\t{category}\t{first.results[i].InputDegrees:G17}\t{first.results[i].InputRadians:G17}\t{f:G17}\t{s:G17}\t{diff:G17}");
				}

			}

			return true;
		}


		public static void RelativeCompare(long firstSessionId, long secondSessionId, SQLiteConnection connection, string outputPath,bool append,bool writeHeader,params Expression<Func<Result,double>>[] selectors)
		{
			var first = DataStrageManager.Load(firstSessionId, connection);
			var second = DataStrageManager.Load(secondSessionId, connection);


			using (var writer = new ReportWriter(outputPath,append))
			{
				writer.WriteLine("First");
				writer.WriteLine($"Cpu={first.session.CPU}");
				writer.WriteLine($"Os={first.session.OS}");
				writer.WriteLine("");

				writer.WriteLine("Second");
				writer.WriteLine($"Cpu={second.session.CPU}");
				writer.WriteLine($"Os={second.session.OS}");


				if (writeHeader)
				{
					writer.WriteLine(
						"FirstEnvironment\tSecondEnvironment\tCategory\tInputDegrees\tInputRadians\tFirstValue\tSecondValue\tRelativeDiff",
						WriteOn.Stream);
				}

				foreach (var expression in selectors)
				{
					RelativeCompare(first, second, expression, writer);
				}
			}

		}


	}

}
