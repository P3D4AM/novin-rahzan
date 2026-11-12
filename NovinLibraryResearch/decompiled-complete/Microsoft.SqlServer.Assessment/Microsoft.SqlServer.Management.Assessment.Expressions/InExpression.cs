using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class InExpression : BinaryExpression
{
	public StringComparison StringComparison { get; } = StringComparison.Ordinal;


	public InExpression()
	{
	}

	public InExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	public InExpression(StringComparison stringComparison)
	{
		StringComparison = stringComparison;
	}

	public InExpression(StringComparison stringComparison, IEnumerable<Expression> source)
		: base(source)
	{
		StringComparison = stringComparison;
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		if (!(arg2 is IEnumerable source))
		{
			if (arg2 == null)
			{
				return false;
			}
			throw new SqlAssessmentTypeMismatchException(typeof(IEnumerable), arg2.GetType(), arg2);
		}
		return source.Cast<object>().Any((object o) => Utils.AreEqual(arg1, o, StringComparison));
	}

	protected override CompositeExpression Copy()
	{
		return new InExpression(StringComparison);
	}
}
