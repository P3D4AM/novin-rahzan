using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlUnaryExpression : SqlExpression
{
	private static readonly ISet<ExpressionType> _allowedOperators = new HashSet<ExpressionType>
	{
		ExpressionType.Equal,
		ExpressionType.NotEqual,
		ExpressionType.Convert,
		ExpressionType.Not,
		ExpressionType.Negate
	};

	public virtual ExpressionType OperatorType { get; }

	public virtual SqlExpression Operand { get; }

	private static ExpressionType VerifyOperator(ExpressionType operatorType)
	{
		if (!_allowedOperators.Contains(operatorType))
		{
			throw new InvalidOperationException("Unsupported Unary operator type specified.");
		}
		return operatorType;
	}

	public SqlUnaryExpression(ExpressionType operatorType, SqlExpression operand, Type type, RelationalTypeMapping typeMapping)
		: base(type, typeMapping)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(operand, "operand");
		OperatorType = VerifyOperator(operatorType);
		Operand = operand;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		return Update((SqlExpression)visitor.Visit(Operand));
	}

	public virtual SqlUnaryExpression Update(SqlExpression operand)
	{
		if (operand == Operand)
		{
			return this;
		}
		return new SqlUnaryExpression(OperatorType, operand, Type, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		if (OperatorType == ExpressionType.Convert)
		{
			expressionPrinter.Append("CAST(");
			expressionPrinter.Visit(Operand);
			expressionPrinter.Append(")");
			expressionPrinter.Append(" AS ");
			expressionPrinter.Append(TypeMapping.StoreType);
			expressionPrinter.Append(")");
		}
		else
		{
			expressionPrinter.Append(OperatorType);
			expressionPrinter.Append("(");
			expressionPrinter.Visit(Operand);
			expressionPrinter.Append(")");
		}
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlUnaryExpression sqlUnaryExpression)
				{
					return Equals(sqlUnaryExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlUnaryExpression sqlUnaryExpression)
	{
		if (base.Equals((object)sqlUnaryExpression) && OperatorType == sqlUnaryExpression.OperatorType)
		{
			return Operand.Equals(sqlUnaryExpression.Operand);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), OperatorType, Operand);
	}
}
