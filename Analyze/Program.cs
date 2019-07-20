using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace Analyze
{
	class Program
	{

		static IEnumerable<(long first, long second)> CombinationGenerator(long[] id)
		{
			foreach (var first in id)
			foreach (var second in id)
			{
				if(first==second) continue;
				yield return (first, second);
			}
		}


		static void Main()
		{
			using (var wtr = new StreamWriter("../../../Gitignore/merge.txt"))
			{
				wtr.WriteLine("id\tfirst_environment\tsecond_environment\tmethod\tdegrees\tradians\tfirst_value\tsecond_value\tdifference");
				var cnt = -1;
				foreach (var file in Directory.GetFiles("../../../GitIgnore", "*.tsv"))
				{
					Console.WriteLine(file);
					var isHeader = true;

					foreach (var line in File.ReadLines(file))
					{
						if (isHeader)
						{
							isHeader = false;
							continue;
						}

						var cursor = line.Split('\t');

						wtr.WriteLine(
							$"{++cnt}\t{cursor[0]}\t{cursor[1]}\t{cursor[2]}\t{cursor[3]}\t{cursor[4]}\t{cursor[5]}\t{cursor[6]}\t{cursor[7]}");


					}

				}
			}
		}

		private static void NewMethod()
		{
			var id = new[]
			{
				636983493474057592, //WinWithoutFMA3
				636983533905992017, //WinWithFMA3
				636983495478558317, //WinWithX87
				636984750609976297, //Linux
				636983853402321720 //macOS
			};

			foreach (var elem in CombinationGenerator(id))
			{
				DifferenceAnalyzer.GetDifference(elem.first, elem.second,
					$"../../../GitIgnore");
			}
		}
	}
}
