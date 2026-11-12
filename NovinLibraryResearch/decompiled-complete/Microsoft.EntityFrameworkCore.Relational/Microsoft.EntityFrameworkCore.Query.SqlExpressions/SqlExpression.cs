using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public abstract class SqlExpression : Expression, IPrintableExpression
{
	public override Type Type { get; }

	public virtual RelationalTypeMapping TypeMapping { get; }

	public sealed override ExpressionType NodeType => ExpressionType.Extension;

	protected SqlExpression(Type type, RelationalTypeMapping typeMapping)
	{
		Type = type;
		TypeMapping = typeMapping;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		throw new InvalidOperationException("VisitChildren must be overridden in class deriving from SqlExpression");
	}

	public abstract void Print(ExpressionPrinter expressionPrinter);

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlExpression sqlExpression)
				{
					return Equals(sqlExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlExpression sqlExpression)
	{
		if (Type == sqlExpression.Type)
		{
			if (TypeMapping != null || sqlExpression.TypeMapping != null)
			{
				return TypeMapping?.Equals(sqlExpression.TypeMapping) ?? false;
			}
			return true;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Type, TypeMapping);
	}
}
