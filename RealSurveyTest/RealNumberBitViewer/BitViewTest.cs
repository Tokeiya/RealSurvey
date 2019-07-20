using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using RealNumberBitViewer;
using Xunit;
using Xunit.Abstractions;

namespace RealSurveyTest.RealNumberBitViewer
{
	public class BitViewTest
	{
		private readonly ITestOutputHelper _output;

		public BitViewTest(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void ValueTest()
		{
			var target = new BitView(Math.PI);
			target.Value.Is(Math.PI);

			target = new BitView(Math.E);
			target.Value.Is(Math.E);

			target = new BitView(0.0);
			target.Value.Is(0.0);
		}

		[Fact]
		public void SignTest()
		{
			var target = new BitView(-1.0);
			target.Sign.Is(SignTypes.Negative);

			target = new BitView(1.0);
			target.Sign.Is(SignTypes.Positive);
		}

		[Fact]
		public void CrateFromIntegerExpressionTest()
		{
			var target = BitView.CreateFromIntegerExpression(0x3ff0_0000_0000_0001);
			target.Value.Is(1.0000000000000002);

			target = BitView.CreateFromIntegerExpression(0x3ff0_0000_0000_0002);
			target.Value.Is(1.0000000000000004);

			BitView.CreateFromIntegerExpression(0x4000_0000_0000_0000).Value.Is(2.0);
			BitView.CreateFromIntegerExpression(0xc000_0000_0000_0000).Value.Is(-2.0);
		}

		[Fact]
		public void ExponentTest()
		{
			var target = new BitView(0.75);
			target.Exponent.Is(-1);

			new BitView(1).Exponent.Is(0);
		}

		[Fact]
		public void MantissaTest()
		{
			var target = new BitView(1.0).Mantissa;

			target.Take(52).All(x=>!x).IsTrue();
			target[52].IsTrue();

			target = new BitView(0.5).Mantissa;

			target.Take(52).All(x => !x).IsTrue();
			target[52].IsTrue();


			target = new BitView(.75).Mantissa;
			target.Take(51).All(x => !x).IsTrue();
			target[51].IsTrue();
			target[52].IsTrue();



		}





	}

}
