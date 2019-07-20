using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Analyze
{
	[DebuggerStepThrough]
	public class NameBuilder
	{
		// ReSharper disable once InconsistentNaming
		private static readonly NameBuildVisitor _nameBuilder = new NameBuildVisitor();


		public static string BuildName(LambdaExpression expression)
		{
			if (expression == null) throw new ArgumentNullException(nameof(expression));

			return _nameBuilder.BuildName_(expression);
		}

		private class NameBuildVisitor : ExpressionVisitor
		{
			private Type _bodyType;

			private StringBuilder _builder;

			private bool _hasDot;
			private bool _isComplex;

			private Type _returnType;

			public string BuildName_(LambdaExpression expression)
			{
				_builder = new StringBuilder();
				_isComplex = false;
				_returnType = null;
				_bodyType = expression.Body.Type;
				_hasDot = false;

				Visit(expression.Body);


				if (_isComplex) return $"Complex {_returnType?.Name ?? _bodyType.Name}";

				return _builder.ToString();
			}

			public override Expression Visit(Expression node)
			{
				switch (node.NodeType)
				{
					case ExpressionType.Block:
					case ExpressionType.Dynamic:
					case ExpressionType.Assign:
					case ExpressionType.DebugInfo:
					case ExpressionType.Goto:
					case ExpressionType.Label:
					case ExpressionType.ListInit:
					case ExpressionType.Quote:
					case ExpressionType.Unbox:
					case ExpressionType.RuntimeVariables:
					case ExpressionType.Throw:
					case ExpressionType.Lambda:
						_isComplex = true;
						return null;
				}

				return base.Visit(node);
			}

			protected override Expression VisitUnary(UnaryExpression node)
			{
				switch (node.NodeType)
				{
					case ExpressionType.ArrayLength:
						_builder.Append(".Length");
						break;

					case ExpressionType.Negate:
						_builder.Append('-');
						break;

					case ExpressionType.UnaryPlus:
						_builder.Append('+');
						break;

					case ExpressionType.Convert:
						if (_returnType == null) _returnType = node.Operand.Type;
						break;

					default:
						_isComplex = true;
						break;
				}
				Visit(node.Operand);

				return node;
			}

			protected override Expression VisitBinary(BinaryExpression node)
			{
				_hasDot = false;
				Visit(node.Left);

				switch (node.NodeType)
				{
					case ExpressionType.Add:
					case ExpressionType.AddChecked:
						_builder.Append('+');
						break;

					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
						_builder.Append('-');
						break;

					case ExpressionType.Multiply:
					case ExpressionType.MultiplyChecked:
						_builder.Append('*');
						break;

					case ExpressionType.Divide:
						_builder.Append('/');
						break;

					case ExpressionType.Modulo:
						_builder.Append('%');
						break;

					case ExpressionType.Power:
						_builder.Append('^');
						break;
				}

				_hasDot = false;
				return Visit(node.Right);
			}

			protected override Expression VisitParameter(ParameterExpression node)
			{
				return node;
			}

			protected override Expression VisitMember(MemberExpression node)
			{
				Visit(node.Expression);

				if (_hasDot)
				{
					_builder.Append($".{node.Member.Name}");
				}
				else
				{
					_builder.Append($"{node.Member.Name}");
					_hasDot = true;
				}


				return node;
			}

			protected override Expression VisitMethodCall(MethodCallExpression node)
			{
				if (node.Method.IsStatic) _builder.Append(node?.Method?.DeclaringType?.Name ?? "UnknownType");
				else Visit(node.Object);

				_builder.Append("." + node.Method.Name + "(");

				foreach (var expr in node.Arguments)
				{
					Visit(expr);
					_builder.Append('.');
				}


				if (node.Arguments.Count != 0) _builder.Remove(_builder.Length - 1, 1);


				_builder.Append(')');

				return node;
			}

			protected override Expression VisitConstant(ConstantExpression node)
			{
				if (node.Value is string) _builder.Append('"');
				_builder.Append(node.Value?.ToString() ?? "NULL");
				if (node.Value is string) _builder.Append('"');

				return base.VisitConstant(node);
			}
		}
	}
}