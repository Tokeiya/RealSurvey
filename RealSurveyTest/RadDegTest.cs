using SurveyGrocery;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace RealSurveyTest
{
	public class RadDegTest
	{
		private readonly ITestOutputHelper _output;

		public RadDegTest(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void ToDegTest()
		{
			foreach (var line in File.ReadLines("./ExpectedSamples/ExpectedRadDeg.txt")
				.Select(str => str.Split('\t').Select(x => double.Parse(x)).ToArray()))
			{
				var act = Trigonometric.ToRadians(line[0]);
				line[1].Is(act);
			}
		}

		[Fact]
		public void ToRadTest()
		{
			foreach (var line in File.ReadLines("./ExpectedSamples/ExpectedRadDeg.txt")
				.Select(str => str.Split('\t').Select(x => double.Parse(x)).ToArray()))
			{
				var act = Trigonometric.ToDegrees(line[1]);
				line[2].Is(act);
			}
		}

		[Fact]
		public void ConvertBackTest()
		{
			foreach (var line in File.ReadLines("./ExpectedSamples/ExpectedRadDeg.txt")
				.Select(str => str.Split('\t').Select(x => double.Parse(x)).ToArray()))
			{
				var act = Trigonometric.ToRadians(line[0]);
				act = Trigonometric.ToDegrees(act);
				act.Is(line[2]);

				act = Trigonometric.ToRadians(act);
				act.Is(line[3]);
			}


		}



	}

}
