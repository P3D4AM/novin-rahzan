using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlParameterExpression : SqlExpression
{
	private readonly ParameterExpression _parameterExpression;

	public string Name => _parameterExpression.Name;

	internal SqlParameterExpression(ParameterExpression parameterExpression, RelationalTypeMapping typeMapping)
		: base(parameterExpression.Type, typeMapping)
	{
		_parameterExpression = parameterExpression;
	}

	public SqlExpression ApplyTypeMapping(RelationalTypeMapping typeMapping)
	{
		return new SqlParameterExpression(_parameterExpression, typeMapping);
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append("@" + _parameterExpression.Name);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlParameterExpression sqlParameterExpression)
				{
					return Equals(sqlParameterExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlParameterExpression sqlParameterExpression)
	{
		if (base.Equals((object)sqlParameterExpression))
		{
			return string.Equals(Name, sqlParameterExpression.Name);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Name);
	}
}
