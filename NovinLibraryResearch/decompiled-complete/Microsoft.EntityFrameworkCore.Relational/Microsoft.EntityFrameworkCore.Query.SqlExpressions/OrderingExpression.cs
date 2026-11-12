using System;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class OrderingExpression : Expression, IPrintableExpression
{
	public virtual SqlExpression Expression { get; }

	public virtual bool IsAscending { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	public override Type Type => Expression.Type;

	public OrderingExpression(SqlExpression expression, bool ascending)
	{
		Expression = expression;
		IsAscending = ascending;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((SqlExpression)visitor.Visit(Expression));
	}

	public virtual OrderingExpression Update(SqlExpression expression)
	{
		if (expression == Expression)
		{
			return this;
		}
		return new OrderingExpression(expression, IsAscending);
	}

	public virtual void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Visit(Expression);
		expressionPrinter.Append(IsAscending ? " ASC" : " DESC");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is OrderingExpression orderingExpression)
				{
					return Equals(orderingExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(OrderingExpression orderingExpression)
	{
		if (Expression.Equals(orderingExpression.Expression))
		{
			return IsAscending == orderingExpression.IsAscending;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Expression, IsAscending);
	}
}
