namespace Analyze
{
	public class Result
	{
		public long? Id { get; set; }
		public long SessionId { get; set; }
		public double InputDegrees { get; set; }
		public double InputRadians { get; set; }

		public double Radians { get; set; }
		public double HighPrecisionRadians { get; set; }
		public double Sin { get; set; }
		public double Cos { get; set; }
		public double SinCos { get; set; }
		public double Tan { get; set; }
		public double PayneHanekSin { get; set; }
		public double PayneHanekCos { get; set; }
		public double PayneHanekTan { get; set; }

	}
}