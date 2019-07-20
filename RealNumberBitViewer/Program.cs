using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RealNumberBitViewer
{


	[StructLayout(LayoutKind.Explicit)]
	public struct Union
	{
		public const ulong SignMask = 0x8000_0000_0000_0000 ^ ulong.MaxValue;

		[FieldOffset(0)] public double RealNumber;
		[FieldOffset(0)] public ulong AsInteger;
	}

	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Math.Sin(2 / Math.PI).ToString("g17"));
			Console.WriteLine(Math.Cos(2 / Math.PI).ToString("g17"));

			var view = new BitView(Math.PI);

			Console.WriteLine(view.ToString());
		}
	}
}
