using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class DecimalAssociativeExpression : SequentialCompositeExpression
{
	private readonly Func<decimal, decimal, decimal> operation;

	public DecimalAssociativeExpression(Func<decimal, decimal, decimal> func)
	{
		operation = func;
	}

	public DecimalAssociativeExpression(Func<decimal, decimal, decimal> func, IEnumerable<Expression> source)
		: base(source)
	{
		operation = func;
	}

	public DecimalAssociativeExpression(Func<decimal, decimal, decimal> func, params Expression[] source)
		: this(func, (IEnumerable<Expression>)source)
	{
	}

	protected override object EvaluateCore(IEnumerable<object> args)
	{
		return args.Select(Utils.Normalize<decimal>).Aggregate(operation);
	}

	protected override CompositeExpression Copy()
	{
		return new DecimalAssociativeExpression(operation);
	}
}
