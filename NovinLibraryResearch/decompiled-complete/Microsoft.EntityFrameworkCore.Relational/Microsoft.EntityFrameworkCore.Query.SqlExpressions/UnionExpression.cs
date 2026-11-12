using System;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class UnionExpression : SetOperationBase
{
	public UnionExpression(string alias, SelectExpression source1, SelectExpression source2, bool distinct)
		: base(alias, source1, source2, distinct)
	{
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		SelectExpression source = (SelectExpression)visitor.Visit(Source1);
		SelectExpression source2 = (SelectExpression)visitor.Visit(Source2);
		return Update(source, source2);
	}

	public virtual UnionExpression Update(SelectExpression source1, SelectExpression source2)
	{
		if (source1 == Source1 && source2 == Source2)
		{
			return this;
		}
		return new UnionExpression(Alias, source1, source2, IsDistinct);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("(");
		using (expressionPrinter.Indent())
		{
			expressionPrinter.Visit(Source1);
			expressionPrinter.AppendLine();
			expressionPrinter.Append("UNION");
			if (!IsDistinct)
			{
				expressionPrinter.AppendLine(" ALL");
			}
			expressionPrinter.Visit(Source2);
		}
		expressionPrinter.AppendLine().AppendLine(") AS " + Alias);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is UnionExpression unionExpression)
				{
					return Equals(unionExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(UnionExpression unionExpression)
	{
		return base.Equals((object)unionExpression);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), GetType());
	}
}
