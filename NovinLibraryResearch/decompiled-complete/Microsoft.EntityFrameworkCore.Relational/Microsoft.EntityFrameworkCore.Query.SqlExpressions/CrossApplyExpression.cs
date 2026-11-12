using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class CrossApplyExpression : JoinExpressionBase
{
	public CrossApplyExpression(TableExpressionBase table)
		: base(table)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((TableExpressionBase)visitor.Visit(Table));
	}

	public virtual CrossApplyExpression Update(TableExpressionBase table)
	{
		if (table == Table)
		{
			return this;
		}
		return new CrossApplyExpression(table);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("CROSS APPLY ");
		expressionPrinter.Visit(Table);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is CrossApplyExpression crossApplyExpression)
				{
					return Equals(crossApplyExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(CrossApplyExpression crossApplyExpression)
	{
		return base.Equals((object)crossApplyExpression);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
