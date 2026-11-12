using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class UnaryFuncExpression : UnaryExpression
{
	private readonly Func<decimal, decimal> roundFunc;

	public UnaryFuncExpression(Func<decimal, decimal> roundingFunction)
	{
		roundFunc = roundingFunction;
	}

	public UnaryFuncExpression(Func<decimal, decimal> roundingFunction, IEnumerable<Expression> source)
		: base(source)
	{
		roundFunc = roundingFunction;
	}

	protected override CompositeExpression Copy()
	{
		return new UnaryFuncExpression(roundFunc);
	}

	protected override object EvaluateCore(object arg)
	{
		return roundFunc(arg.Normalize<decimal>());
	}
}
