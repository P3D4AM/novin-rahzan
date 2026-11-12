using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class EqualsExpression : BinaryExpression
{
	public StringComparison StringComparison { get; } = StringComparison.Ordinal;


	public EqualsExpression()
	{
	}

	public EqualsExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	public EqualsExpression(params Expression[] source)
		: this((IEnumerable<Expression>)source)
	{
	}

	public EqualsExpression(StringComparison stringComparison)
	{
		StringComparison = stringComparison;
	}

	public EqualsExpression(StringComparison stringComparison, IEnumerable<Expression> source)
		: base(source)
	{
		StringComparison = stringComparison;
	}

	public EqualsExpression(StringComparison stringComparison, params Expression[] source)
		: this(stringComparison, (IEnumerable<Expression>)source)
	{
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		return Utils.AreEqual(arg1, arg2, StringComparison);
	}

	protected override CompositeExpression Copy()
	{
		return new EqualsExpression(StringComparison);
	}
}
