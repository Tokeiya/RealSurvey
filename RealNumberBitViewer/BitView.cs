using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace RealNumberBitViewer
{
	public enum SignTypes
	{
		Positive = 1,
		Negative = 2
	}

	public class BitView
	{
		private static readonly ulong[] BitwiseMasks = new ulong[]
		{
			0x0000000000000001,
			0x0000000000000002,
			0x0000000000000004,
			0x0000000000000008,
			0x0000000000000010,
			0x0000000000000020,
			0x0000000000000040,
			0x0000000000000080,
			0x0000000000000100,
			0x0000000000000200,
			0x0000000000000400,
			0x0000000000000800,
			0x0000000000001000,
			0x0000000000002000,
			0x0000000000004000,
			0x0000000000008000,
			0x0000000000010000,
			0x0000000000020000,
			0x0000000000040000,
			0x0000000000080000,
			0x0000000000100000,
			0x0000000000200000,
			0x0000000000400000,
			0x0000000000800000,
			0x0000000001000000,
			0x0000000002000000,
			0x0000000004000000,
			0x0000000008000000,
			0x0000000010000000,
			0x0000000020000000,
			0x0000000040000000,
			0x0000000080000000,
			0x0000000100000000,
			0x0000000200000000,
			0x0000000400000000,
			0x0000000800000000,
			0x0000001000000000,
			0x0000002000000000,
			0x0000004000000000,
			0x0000008000000000,
			0x0000010000000000,
			0x0000020000000000,
			0x0000040000000000,
			0x0000080000000000,
			0x0000100000000000,
			0x0000200000000000,
			0x0000400000000000,
			0x0000800000000000,
			0x0001000000000000,
			0x0002000000000000,
			0x0004000000000000,
			0x0008000000000000,
			0x0010000000000000,
			0x0020000000000000,
			0x0040000000000000,
			0x0080000000000000,
			0x0100000000000000,
			0x0200000000000000,
			0x0400000000000000,
			0x0800000000000000,
			0x1000000000000000,
			0x2000000000000000,
			0x4000000000000000,
			0x8000000000000000
		};

		private const ulong SignMask = 0x8000_0000_0000_0000;
		private const ulong ExponentMask = 0x7ff0000000000000;



		[StructLayout(LayoutKind.Explicit)]
		readonly struct Union
		{
			public Union(double value):this()
			{
				Value = value;
			}

			public Union(ulong integerExpression) : this()
			{
				IntegerExpression = integerExpression;
			}

			[FieldOffset(0)] public readonly double Value;
			[FieldOffset(0)] public readonly ulong IntegerExpression;
		}

		private readonly Union _value;

		public static BitView CreateFromIntegerExpression(ulong integerExpression) =>
			new BitView(new Union(integerExpression));

		private BitView(Union value)
		{
			_value = value;
		}

		public BitView(double value) : this(new Union(value))
		{

		}


		public double Value => _value.Value;
		public ulong IntegerExpression => _value.IntegerExpression;

		public SignTypes Sign => (_value.IntegerExpression & SignMask) != 0 ? SignTypes.Negative : SignTypes.Positive;

		public int Exponent
		{
			get
			{
				var ret = _value.IntegerExpression & ExponentMask;
				ret >>= 52;
				return (short) (ret - 0x3FF);
			}
		}

		public bool[] Mantissa
		{
			get
			{
				var ret = new bool[53];

				for (int i = 0; i < 52; i++)
				{
					ret[i] = (_value.IntegerExpression & BitwiseMasks[i]) != 0;
				}

				ret[52] = true;

				return ret;
			}
		}

		public override string ToString()
		{
			var bld = new StringBuilder();

			bld.Append($"Value:{Value:g17}\n");
			bld.Append($"IntegerExpression:0x{IntegerExpression:x}\n\n");


			bld.Append($"Sign:{(Sign)}\n");
			bld.Append($"Exponent:{Exponent}\n\n");

			var mantissa = Mantissa;

			//Does not consider Subnormal state
			bld.Append("Mantissa:\n53    52\n");
			bld.Append(" 1     ");
			for (int i = 51; i >= 48; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append('\n');

			bld.Append("48    44\n ");
			for (int i = 47; i >= 44; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 43; i >= 40; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");

			bld.Append("40    36\n ");
			for (int i = 39; i >= 36; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 35; i >= 32; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");


			bld.Append("32    28\n ");
			for (int i = 31; i >= 28; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 27; i >= 24; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");

			bld.Append("24    20\n ");
			for (int i = 23; i >= 20; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 19; i >= 16; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");

			bld.Append("16    12\n ");
			for (int i = 15; i >= 12; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 11; i >= 8; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");

			bld.Append("08    04\n ");
			for (int i = 7; i >= 4; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("  ");
			for (int i = 3; i >= 0; i--) bld.Append(mantissa[i] ? "1" : "0");
			bld.Append("\n");


			return bld.ToString();
		}
	}
}
