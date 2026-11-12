using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal abstract class UnaryExpression : SequentialCompositeExpression
{
	protected UnaryExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected UnaryExpression()
	{
	}

	protected override object EvaluateCore(IEnumerable<object> args)
	{
		object[] nArguments = CompositeExpression.GetNArguments(args, 1);
		return EvaluateCore(nArguments[0]);
	}

	protected abstract object EvaluateCore(object arg);
}
