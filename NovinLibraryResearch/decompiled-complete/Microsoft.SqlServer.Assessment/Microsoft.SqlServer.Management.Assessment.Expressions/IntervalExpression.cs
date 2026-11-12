using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class IntervalExpression : CompositeExpression
{
	public IntervalExpression()
	{
	}

	public IntervalExpression(IEnumerable<Expression> source)
		: base(source)
	{
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		if (base.Children.Count < 2 || base.Children.Count % 2 != 0)
		{
			throw new SqlAssessmentArityException(2, base.Children.Count);
		}
		decimal num = base.Children[0].Evaluate(data).Normalize<decimal>();
		for (int i = 2; i < base.Children.Count; i += 2)
		{
			decimal num2 = base.Children[i].Evaluate(data).Normalize<decimal>();
			if (num <= num2)
			{
				return base.Children[i - 1].Evaluate(data);
			}
		}
		return base.Children.Last().Evaluate(data);
	}

	protected override CompositeExpression Copy()
	{
		return new IntersectExpression();
	}
}
