using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ScalarSubqueryExpression : SqlExpression
{
	public virtual SelectExpression Subquery { get; }

	public ScalarSubqueryExpression(SelectExpression subquery)
		: base(Verify(subquery).Projection[0].Type, subquery.Projection[0].Expression.TypeMapping)
	{
		Subquery = subquery;
	}

	private static SelectExpression Verify(SelectExpression selectExpression)
	{
		if (selectExpression.Projection.Count != 1)
		{
			throw new InvalidOperationException(CoreStrings.TranslationFailed(selectExpression.Print()));
		}
		return selectExpression;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((SelectExpression)visitor.Visit(Subquery));
	}

	public virtual ScalarSubqueryExpression Update(SelectExpression subquery)
	{
		if (subquery == Subquery)
		{
			return this;
		}
		return new ScalarSubqueryExpression(subquery);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("(");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.Visit(Subquery);
		}
		expressionPrinter.Append(")");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is ScalarSubqueryExpression scalarSubqueryExpression)
				{
					return Equals(scalarSubqueryExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(ScalarSubqueryExpression scalarSubqueryExpression)
	{
		if (base.Equals((object)scalarSubqueryExpression))
		{
			return Subquery.Equals(scalarSubqueryExpression.Subquery);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Subquery);
	}
}
