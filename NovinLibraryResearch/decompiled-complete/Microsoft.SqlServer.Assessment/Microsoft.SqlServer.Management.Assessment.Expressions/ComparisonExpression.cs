using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal abstract class ComparisonExpression<T> : BinaryExpression where T : IComparable<T>
{
	protected ComparisonExpression()
	{
	}

	protected ComparisonExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		return EvaluateCore(arg1.Normalize<T>().CompareTo(arg2.Normalize<T>()));
	}

	protected abstract bool EvaluateCore(int comparisonResult);
}
