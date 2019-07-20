using SurveyGrocery;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace RealSurveyTest
{
	public class TrigonometricTests
	{
		private readonly ITestOutputHelper _output;

		public TrigonometricTests(ITestOutputHelper output)
		{
			_output = output;
		}

		private static IEnumerable<double[]> Load() => File.ReadLines("./ExpectedSamples/Expected.txt")
			.Select(str => str.Split('\t').Select(x => double.Parse(x)).ToArray());

		private static void ParseEqual(string[] source)
		{
			foreach (var s in source)
			{
				var d = double.Parse(s);
				d.ToString("g17").Is(s);
			}
		}

		[Fact]
		public void ParseEqualityTest()
		{
			using (var rdr = new StreamReader("./ExpectedSamples/Expected.txt"))
			{
				string cursor;

				while ((cursor = rdr.ReadLine()) != null)
				{
					var tmp = cursor.Split('\t');
					ParseEqual(tmp);
				}
			}
		}

		[Fact]
		public void SinTest()
		{
			foreach (var sample in Load())
			{
				var act = Trigonometric.Sin(sample[0]);
				act.Is(sample[1]);
			}
		}

		[Fact]
		public void CosTest()
		{
			foreach (var sample in Load())
			{
				var act = Trigonometric.Cos(sample[0]);
				act.Is(sample[2]);
			}
		}

		[Fact]
		public void TanTest()
		{
			foreach (var sample in Load())
			{
				var act = Trigonometric.Tan(sample[0]);
				act.Is(sample[3]);
			}
		}
	}
}
