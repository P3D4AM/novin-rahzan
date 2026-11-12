using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class IntersectExpression : BinaryExpression
{
	public const string JsonName = "intersect";

	public IntersectExpression()
	{
	}

	public IntersectExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected override object EvaluateCore(object arg1, object arg2)
	{
		if (arg1 is IEnumerable<object> first)
		{
			if (arg2 is IEnumerable<object> second)
			{
				return first.Intersect(second);
			}
			throw new SqlAssessmentTypeMismatchException(typeof(IEnumerable), arg2?.GetType(), arg2);
		}
		throw new SqlAssessmentTypeMismatchException(typeof(IEnumerable), arg1?.GetType(), arg1);
	}

	protected override CompositeExpression Copy()
	{
		return new IntersectExpression();
	}
}
