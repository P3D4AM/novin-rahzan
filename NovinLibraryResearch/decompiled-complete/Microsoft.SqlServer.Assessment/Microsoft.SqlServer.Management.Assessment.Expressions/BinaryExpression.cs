using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal abstract class BinaryExpression : SequentialCompositeExpression
{
	protected BinaryExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	protected BinaryExpression()
	{
	}

	protected override object EvaluateCore(IEnumerable<object> args)
	{
		object[] nArguments = CompositeExpression.GetNArguments(args, 2);
		return EvaluateCore(nArguments[0], nArguments[1]);
	}

	protected abstract object EvaluateCore(object arg1, object arg2);
}
