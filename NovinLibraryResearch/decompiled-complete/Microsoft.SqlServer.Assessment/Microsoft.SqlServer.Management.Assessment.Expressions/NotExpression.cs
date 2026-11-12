using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class NotExpression : UnaryExpression
{
	public NotExpression()
	{
	}

	public NotExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected override object EvaluateCore(object arg)
	{
		return !arg.Normalize<bool>();
	}

	protected override CompositeExpression Copy()
	{
		return new NotExpression();
	}
}
