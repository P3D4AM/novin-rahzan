using System;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlConstantExpression : SqlExpression
{
	private readonly ConstantExpression _constantExpression;

	public virtual object Value => _constantExpression.Value;

	public SqlConstantExpression(ConstantExpression constantExpression, RelationalTypeMapping typeMapping)
		: base(constantExpression.Type, typeMapping)
	{
		_constantExpression = constantExpression;
	}

	public virtual SqlExpression ApplyTypeMapping(RelationalTypeMapping typeMapping)
	{
		return new SqlConstantExpression(_constantExpression, typeMapping);
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return this;
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		Print(Value, expressionPrinter);
	}

	private void Print(object value, ExpressionPrinter expressionPrinter)
	{
		expressionPrinter.Append(TypeMapping?.GenerateSqlLiteral(value) ?? Value?.ToString() ?? "NULL");
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlConstantExpression sqlConstantExpression)
				{
					return Equals(sqlConstantExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlConstantExpression sqlConstantExpression)
	{
		if (base.Equals((object)sqlConstantExpression))
		{
			return ValueEquals(Value, sqlConstantExpression.Value);
		}
		return false;
	}

	private bool ValueEquals(object value1, object value2)
	{
		if (value1 == null)
		{
			return value2 == null;
		}
		if (value1 is IList list && value2 is IList list2)
		{
			if (list.Count != list2.Count)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!ValueEquals(list[i], list2[i]))
				{
					return false;
				}
			}
			return true;
		}
		return value1.Equals(value2);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), Value);
	}
}
