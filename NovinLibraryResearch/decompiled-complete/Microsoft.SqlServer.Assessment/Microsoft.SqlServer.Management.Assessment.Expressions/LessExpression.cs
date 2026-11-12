using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class LessExpression : ComparisonExpression<decimal>
{
	public LessExpression()
	{
	}

	public LessExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected override bool EvaluateCore(int comparisonResult)
	{
		return comparisonResult < 0;
	}

	protected override CompositeExpression Copy()
	{
		return new LessExpression();
	}
}
