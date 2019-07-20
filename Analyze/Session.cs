using System;

// ReSharper disable UnusedMember.Global

namespace Analyze
{
	public class Session
	{
		public long? Id { get; set; }
		public DateTimeOffset TimeStamp { get; set; }
		public string CPU { get; set; }
		public string OS { get; set; }
		public string Framework { get; set; }
		public string ProcessArchitecture { get; set; }

		public string Generation { get; set; }

		public int? OutputOrder { get; set; }

		public string Description { get; set; }
	}
}
