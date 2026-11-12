using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class LeftJoinExpression : PredicateJoinExpressionBase
{
	public LeftJoinExpression(TableExpressionBase table, SqlExpression joinPredicate)
		: base(table, joinPredicate)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		TableExpressionBase table = (TableExpressionBase)visitor.Visit(Table);
		SqlExpression joinPredicate = (SqlExpression)visitor.Visit(JoinPredicate);
		return Update(table, joinPredicate);
	}

	public virtual LeftJoinExpression Update(TableExpressionBase table, SqlExpression joinPredicate)
	{
		if (table == Table && joinPredicate == JoinPredicate)
		{
			return this;
		}
		return new LeftJoinExpression(table, joinPredicate);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("LEFT JOIN ");
		expressionPrinter.Visit(Table);
		expressionPrinter.Append(" ON ");
		expressionPrinter.Visit(JoinPredicate);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is LeftJoinExpression leftJoinExpression)
				{
					return Equals(leftJoinExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(LeftJoinExpression leftJoinExpression)
	{
		return base.Equals((object)leftJoinExpression);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
