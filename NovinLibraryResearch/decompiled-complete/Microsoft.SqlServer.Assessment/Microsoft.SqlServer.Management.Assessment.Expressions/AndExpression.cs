using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class AndExpression : SequentialCompositeExpression
{
	public AndExpression()
	{
	}

	public AndExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	public AndExpression(params Expression[] source)
		: this((IEnumerable<Expression>)source)
	{
	}

	protected override object EvaluateCore(IEnumerable<object> args)
	{
		return args.All(Utils.Normalize<bool>);
	}

	protected override CompositeExpression Copy()
	{
		return new AndExpression();
	}
}
