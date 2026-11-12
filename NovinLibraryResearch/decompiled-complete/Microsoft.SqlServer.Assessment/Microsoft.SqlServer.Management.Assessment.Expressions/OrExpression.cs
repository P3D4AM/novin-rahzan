using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class OrExpression : SequentialCompositeExpression
{
	public OrExpression()
	{
	}

	public OrExpression(params Expression[] source)
		: this((IEnumerable<Expression>)source)
	{
	}

	public OrExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected override object EvaluateCore(IEnumerable<object> args)
	{
		return args.Any(Utils.Normalize<bool>);
	}

	protected override CompositeExpression Copy()
	{
		return new OrExpression();
	}
}
