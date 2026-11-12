using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class OuterApplyExpression : JoinExpressionBase
{
	public OuterApplyExpression(TableExpressionBase table)
		: base(table)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((TableExpressionBase)visitor.Visit(Table));
	}

	public virtual OuterApplyExpression Update(TableExpressionBase table)
	{
		if (table == Table)
		{
			return this;
		}
		return new OuterApplyExpression(table);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("OUTER APPLY ");
		expressionPrinter.Visit(Table);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is OuterApplyExpression outerApplyExpression)
				{
					return Equals(outerApplyExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(OuterApplyExpression outerApplyExpression)
	{
		return base.Equals((object)outerApplyExpression);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
