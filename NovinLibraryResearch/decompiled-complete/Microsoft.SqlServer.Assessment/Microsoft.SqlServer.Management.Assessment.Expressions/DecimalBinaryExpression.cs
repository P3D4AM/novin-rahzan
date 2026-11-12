using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class DecimalBinaryExpression : BinaryExpression
{
	private readonly Func<decimal, decimal, decimal> operation;

	public DecimalBinaryExpression(Func<decimal, decimal, decimal> func)
	{
		operation = func;
	}

	public DecimalBinaryExpression(Func<decimal, decimal, decimal> func, IEnumerable<Expression> source)
		: base(source)
	{
		operation = func;
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		return operation(arg1.Normalize<decimal>(), arg2.Normalize<decimal>());
	}

	protected override CompositeExpression Copy()
	{
		return new DecimalBinaryExpression(operation);
	}
}
