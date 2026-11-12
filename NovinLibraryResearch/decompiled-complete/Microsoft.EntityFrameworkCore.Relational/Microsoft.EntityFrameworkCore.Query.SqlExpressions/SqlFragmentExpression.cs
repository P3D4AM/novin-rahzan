using System;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlFragmentExpression : SqlExpression
{
	public virtual string Sql { get; }

	public SqlFragmentExpression(string sql)
		: base(typeof(string), null)
	{
		Sql = sql;
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
				if (obj is SqlFragmentExpression sqlFragmentExpression)
				{
					return Equals(sqlFragmentExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlFragmentExpression sqlFragmentExpression)
	{
		if (base.Equals((object)sqlFragmentExpression))
		{
			return string.Equals(Sql, sqlFragmentExpression.Sql);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Sql);
	}
}
