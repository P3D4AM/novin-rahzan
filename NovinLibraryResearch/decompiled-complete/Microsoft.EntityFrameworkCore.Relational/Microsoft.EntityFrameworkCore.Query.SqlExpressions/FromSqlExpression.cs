using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class FromSqlExpression : TableExpressionBase
{
	public virtual string Sql { get; }

	public virtual Expression Arguments { get; }

	public FromSqlExpression([JetBrains.Annotations.NotNull] string sql, Expression arguments, [JetBrains.Annotations.NotNull] string alias)
		: base(alias)
	{
		Sql = sql;
		Arguments = arguments;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append(Sql);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is FromSqlExpression fromSqlExpression)
				{
					return Equals(fromSqlExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(FromSqlExpression fromSqlExpression)
	{
		if (base.Equals((object)fromSqlExpression))
		{
			return string.Equals(Sql, fromSqlExpression.Sql);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Sql);
	}
}
