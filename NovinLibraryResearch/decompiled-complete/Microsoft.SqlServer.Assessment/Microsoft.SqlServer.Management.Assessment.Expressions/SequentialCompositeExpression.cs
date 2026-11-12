using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal abstract class SequentialCompositeExpression : CompositeExpression
{
	protected SequentialCompositeExpression(IEnumerable<Expression> source)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		base.Children.AddRange(source);
	}

	protected SequentialCompositeExpression()
	{
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		return EvaluateCore(base.Children.Select((Expression ex) => ex.Evaluate(data)));
	}

	protected abstract object EvaluateCore(IEnumerable<object> args);
}
