using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;

namespace Analyze
{
	public static class DifferenceAnalyzer
	{
		public static Expression<Func<Result, double>>[] SystemMath { get; } =
		{
			x => x.Radians,
			x => x.Sin,
			x => x.Cos,
			x => x.Tan,
			x => x.SinCos
		};

		public static Expression<Func<Result, double>>[] PayneHanec { get; } =
		{
			x=>x.HighPrecisionRadians,
			x=>x.PayneHanekSin,
			x=>x.PayneHanekCos,
			x=>x.PayneHanekTan
		};

		public static  Expression<Func<Result, double>>[] All { get; }

		static DifferenceAnalyzer() => All = SystemMath.Concat(PayneHanec).ToArray();
		
		public static void GetDifference(long first, long second, string outputPath)
			=> GetDifference(first, second, outputPath, All);

		public static void GetDifference(long firstId, long secondId, string outputDirectory,
			params Expression<Func<Result, double>>[] selector)
		{
			(Result[], Session) first;
			(Result[], Session) second;


			using (var con=DataStrageManager.CreateConnection())
			{
				first = DataStrageManager.Load(firstId, con);
				second = DataStrageManager.Load(secondId, con);
			}

			using (var wtr =
				new ReportWriter($"{outputDirectory}/{first.Item2.Description}_{second.Item2.Description}.tsv",
					false)) 
			{
				wtr.WriteLine(
					"first_description\tsecond_description\tmethod\tdegrees\tradians\tfirst_result\tsecond_result\tdifference",
					WriteOn.Stream);


				foreach (var impl in selector.Select(x => (method: x.Compile(), name: NameBuilder.BuildName(x))))
				{
					wtr.WriteLine(impl.name, WriteOn.Console);
					for (int i = 0; i < first.Item1.Length; i++)
					{
						var f = first.Item1[i];
						var s = second.Item1[i];

						wtr.WriteLine(
							$"{first.Item2.Description}\t{second.Item2.Description}\t{impl.name}\t{f.InputDegrees:g17}\t{f.InputRadians:g17}\t{impl.method(f):g17}\t{impl.method(s):g17}\t{Math.Abs(impl.method(f) - impl.method(s)):g17}",
							WriteOn.Stream);
					}
				}
			}


		}




	}
}
