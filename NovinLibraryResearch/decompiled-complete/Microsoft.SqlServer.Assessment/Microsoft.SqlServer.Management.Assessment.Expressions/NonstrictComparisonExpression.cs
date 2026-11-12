using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class NonstrictComparisonExpression<T> : ComparisonExpression<T> where T : IComparable<T>
{
	private readonly ComparisonExpression<T> strictComparisonExpr;

	public NonstrictComparisonExpression(ComparisonExpression<T> strictComparisonExpression)
		: base((IEnumerable<Expression>)strictComparisonExpression.Children)
	{
		strictComparisonExpr = strictComparisonExpression;
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		return (bool)base.EvaluateCore(data) || (bool)strictComparisonExpr.Evaluate(data);
	}

	protected override CompositeExpression Copy()
	{
		return new NonstrictComparisonExpression<T>(strictComparisonExpr);
	}

	protected override bool EvaluateCore(int comparisonResult)
	{
		return comparisonResult == 0;
	}
}
