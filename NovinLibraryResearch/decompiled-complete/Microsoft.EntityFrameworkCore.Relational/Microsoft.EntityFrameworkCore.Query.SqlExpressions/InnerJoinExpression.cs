using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class InnerJoinExpression : PredicateJoinExpressionBase
{
	public InnerJoinExpression(TableExpressionBase table, SqlExpression joinPredicate)
		: base(table, joinPredicate)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		TableExpressionBase table = (TableExpressionBase)visitor.Visit(Table);
		SqlExpression joinPredicate = (SqlExpression)visitor.Visit(JoinPredicate);
		return Update(table, joinPredicate);
	}

	public virtual InnerJoinExpression Update(TableExpressionBase table, SqlExpression joinPredicate)
	{
		if (table == Table && joinPredicate == JoinPredicate)
		{
			return this;
		}
		return new InnerJoinExpression(table, joinPredicate);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("INNER JOIN ");
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
				if (obj is InnerJoinExpression innerJoinExpression)
				{
					return Equals(innerJoinExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(InnerJoinExpression innerJoinExpression)
	{
		return base.Equals((object)innerJoinExpression);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
