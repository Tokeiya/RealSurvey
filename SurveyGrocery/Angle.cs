using System;
using System.IO;
using System.Transactions;

namespace SurveyGrocery
{
	public static class RealNumberHelper
	{
		public static string G17(this double value) => value.ToString("g17");
	}

	public struct TestAngle
	{
		public static string Header = string.Join('\t',"SessionId", "InputDegrees", "InputRadians","Radians", "HighPrecisionRadians", "Sin", "Cos",
			"Tan", "Sin/Cos", "PayneHanekSin", "PayneHanekCos", "PayneHanekTan");

		public TestAngle(double degrees, double radians)
		{
			InputDegrees = degrees;
			InputRadians = radians;
		}

		public double InputDegrees { get; }
		public double InputRadians { get; }

		public double Radians => InputDegrees * (Math.PI / 180.0);
		public double HpRadians => Trigonometric.ToRadians(InputDegrees);

		public double Sin => Math.Sin(InputRadians);
		public double Cos => Math.Cos(InputRadians);
		public double Tan => Math.Tan(InputRadians);

		public double SinCos => Sin / Cos;

		public double PayneHanekSin => Trigonometric.Sin(InputRadians);
		public double PayneHanekCos => Trigonometric.Cos(InputRadians);
		public double PayneHanekTan => Trigonometric.Tan(InputRadians);

		public void WriteTsv(TextWriter writer,long sessionId)
		{
			writer.WriteLine(string.Join('\t', sessionId, InputDegrees.G17(), InputRadians.G17(), Radians.G17(),
				HpRadians.G17(), Sin.G17(), Cos.G17(), Tan.G17(), SinCos.G17(),
				PayneHanekSin.G17(), PayneHanekCos.G17(), PayneHanekTan.G17()));
		}
	}

}