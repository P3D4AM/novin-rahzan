using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class CrossJoinExpression : JoinExpressionBase
{
	public CrossJoinExpression(TableExpressionBase table)
		: base(table)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((TableExpressionBase)visitor.Visit(Table));
	}

	public virtual CrossJoinExpression Update(TableExpressionBase table)
	{
		if (table == Table)
		{
			return this;
		}
		return new CrossJoinExpression(table);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("CROSS JOIN ");
		expressionPrinter.Visit(Table);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is CrossJoinExpression crossJoinExpression)
				{
					return Equals(crossJoinExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(CrossJoinExpression crossJoinExpression)
	{
		return base.Equals((object)crossJoinExpression);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
