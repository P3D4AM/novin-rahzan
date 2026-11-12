using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class ExistsExpression : SqlExpression
{
	public virtual SelectExpression Subquery { get; }

	public virtual bool IsNegated { get; }

	public ExistsExpression(SelectExpression subquery, bool negated, RelationalTypeMapping typeMapping)
		: base(typeof(bool), typeMapping)
	{
		Subquery = subquery;
		IsNegated = negated;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((SelectExpression)visitor.Visit(Subquery));
	}

	public virtual ExistsExpression Update(SelectExpression subquery)
	{
		if (subquery == Subquery)
		{
			return this;
		}
		return new ExistsExpression(subquery, IsNegated, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		if (IsNegated)
		{
			expressionPrinter.Append("NOT ");
		}
		expressionPrinter.AppendLine("EXISTS (");
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
				if (obj is ExistsExpression existsExpression)
				{
					return Equals(existsExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(ExistsExpression existsExpression)
	{
		if (base.Equals((object)existsExpression) && Subquery.Equals(existsExpression.Subquery))
		{
			return IsNegated == existsExpression.IsNegated;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Subquery, IsNegated);
	}
}
