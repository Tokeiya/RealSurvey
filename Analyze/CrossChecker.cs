using System;
using System.Data.SQLite;
using System.Linq;
using System.Linq.Expressions;

namespace Analyze
{
	public static class CrossChecker
	{

		static string Format(Session first, Session second, string result) =>
			$"{first.OutputOrder:00}\t{second.OutputOrder:00}\t{first.CPU}({first.Generation})\t{second.CPU}({second.Generation})\t{result}";

		public static void Check(SQLiteConnection connection, string outputPath, params long[] target) =>
			Check(connection, outputPath, DataStrageManager.AllTrigonometric, target);

		public static void Check(SQLiteConnection connection, string outputPath,
			Expression<Func<Result, double>>[] selector, params long[] target)
		{
			using (var wtr = new ReportWriter(outputPath, false))
			{
				wtr.WriteLine($"first_order\tsecond_order\tfirst_environment\tsecond_environment\tresult", WriteOn.Stream);

				foreach (var first in target)
				{
					foreach (var second in target)
					{
						Check(first, second, connection, wtr, selector.Select(expr => expr.Compile()).ToArray());

					}
				}
			}
		}
		private static void Check(long first, long second, SQLiteConnection connection, ReportWriter writer,Func<Result,double>[] selector)
		{

			var (fResults, fSession) = DataStrageManager.Load(first, connection);
			var (sResults, sSession) = DataStrageManager.Load(second, connection);

			if (first == second)
			{
				writer.WriteLine(Format(fSession, sSession, "ー"));
				return;
			}

			if (fResults.Length != sResults.Length)
			{
				writer.WriteLine(Format(fSession, sSession, "LengthMismatch!"));
				return;
			}

			var diffResult = true;

			for (int i = 0; i < fResults.Length; i++)
			{
				var f = fResults[i];
				var s = sResults[i];

				foreach (var func in selector)
				{
					if (func(f) != func(s))
					{
						diffResult = false;
						break;
					}
				}
			}

			writer.WriteLine(Format(fSession, sSession, diffResult ? "○" : "×"));

		}

	}


}
