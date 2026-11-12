using System;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class InExpression : SqlExpression
{
	public virtual SqlExpression Item { get; }

	public virtual bool IsNegated { get; }

	public virtual SqlExpression Values { get; }

	public virtual SelectExpression Subquery { get; }

	public InExpression(SqlExpression item, bool negated, SelectExpression subquery, RelationalTypeMapping typeMapping)
		: this(item, negated, null, subquery, typeMapping)
	{
	}

	public InExpression(SqlExpression item, bool negated, SqlExpression values, RelationalTypeMapping typeMapping)
		: this(item, negated, values, null, typeMapping)
	{
	}

	private InExpression(SqlExpression item, bool negated, SqlExpression values, SelectExpression subquery, RelationalTypeMapping typeMapping)
		: base(typeof(bool), typeMapping)
	{
		Item = item;
		IsNegated = negated;
		Subquery = subquery;
		Values = values;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		SqlExpression item = (SqlExpression)visitor.Visit(Item);
		SelectExpression subquery = (SelectExpression)visitor.Visit(Subquery);
		SqlExpression values = (SqlExpression)visitor.Visit(Values);
		return Update(item, values, subquery);
	}

	public virtual InExpression Negate()
	{
		return new InExpression(Item, !IsNegated, Values, Subquery, TypeMapping);
	}

	public virtual InExpression Update(SqlExpression item, SqlExpression values, SelectExpression subquery)
	{
		if (item == Item && subquery == Subquery && values == Values)
		{
			return this;
		}
		return new InExpression(item, IsNegated, values, subquery, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Visit(Item);
		expressionPrinter.Append(IsNegated ? " NOT IN " : " IN ");
		expressionPrinter.Append("(");
		if (Values is SqlConstantExpression { Value: IEnumerable value } sqlConstantExpression)
		{
			bool flag = true;
			foreach (object item in value)
			{
				if (!flag)
				{
					expressionPrinter.Append(", ");
				}
				flag = false;
				expressionPrinter.Append(sqlConstantExpression.TypeMapping?.GenerateSqlLiteral(item) ?? item?.ToString() ?? "NULL");
			}
		}
		else
		{
			expressionPrinter.Visit(Values);
		}
		expressionPrinter.Append(")");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is InExpression inExpression)
				{
					return Equals(inExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(InExpression inExpression)
	{
		if (base.Equals((object)inExpression) && Item.Equals(inExpression.Item) && IsNegated.Equals(inExpression.IsNegated) && ((Values == null) ? (inExpression.Values == null) : Values.Equals(inExpression.Values)))
		{
			if (Subquery != null)
			{
				return Subquery.Equals(inExpression.Subquery);
			}
			return inExpression.Subquery == null;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Item, IsNegated, Values, Subquery);
	}
}
