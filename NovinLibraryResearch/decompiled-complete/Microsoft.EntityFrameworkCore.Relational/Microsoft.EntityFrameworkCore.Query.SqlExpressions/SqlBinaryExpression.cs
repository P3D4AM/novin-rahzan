using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query.SqlExpressions;

public class SqlBinaryExpression : SqlExpression
{
	private static readonly ISet<ExpressionType> _allowedOperators = new HashSet<ExpressionType>
	{
		ExpressionType.Add,
		ExpressionType.Subtract,
		ExpressionType.Multiply,
		ExpressionType.Divide,
		ExpressionType.Modulo,
		ExpressionType.And,
		ExpressionType.AndAlso,
		ExpressionType.Or,
		ExpressionType.OrElse,
		ExpressionType.LessThan,
		ExpressionType.LessThanOrEqual,
		ExpressionType.GreaterThan,
		ExpressionType.GreaterThanOrEqual,
		ExpressionType.Equal,
		ExpressionType.NotEqual,
		ExpressionType.Coalesce
	};

	public virtual ExpressionType OperatorType { get; }

	public virtual SqlExpression Left { get; }

	public virtual SqlExpression Right { get; }

	private static ExpressionType VerifyOperator(ExpressionType operatorType)
	{
		if (!_allowedOperators.Contains(operatorType))
		{
			throw new InvalidOperationException("Unsupported Binary operator type specified.");
		}
		return operatorType;
	}

	public SqlBinaryExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, Type type, RelationalTypeMapping typeMapping)
		: base(type, typeMapping)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(left, "left");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(right, "right");
		OperatorType = VerifyOperator(operatorType);
		Left = left;
		Right = right;
	}

	protected override Expression VisitChildren(ExpressionVisitor visitor)
	{
		SqlExpression left = (SqlExpression)visitor.Visit(Left);
		SqlExpression right = (SqlExpression)visitor.Visit(Right);
		return Update(left, right);
	}

	public virtual SqlBinaryExpression Update(SqlExpression left, SqlExpression right)
	{
		if (left == Left && right == Right)
		{
			return this;
		}
		return new SqlBinaryExpression(OperatorType, left, right, Type, TypeMapping);
	}

	public override void Print(ExpressionPrinter expressionPrinter)
	{
		bool num = RequiresBrackets(Left);
		if (num)
		{
			expressionPrinter.Append("(");
		}
		expressionPrinter.Visit(Left);
		if (num)
		{
			expressionPrinter.Append(")");
		}
		expressionPrinter.Append(expressionPrinter.GenerateBinaryOperator(OperatorType));
		bool num2 = RequiresBrackets(Right);
		if (num2)
		{
			expressionPrinter.Append("(");
		}
		expressionPrinter.Visit(Right);
		if (num2)
		{
			expressionPrinter.Append(")");
		}
	}

	private bool RequiresBrackets(SqlExpression expression)
	{
		if (!(expression is SqlBinaryExpression { OperatorType: not ExpressionType.Coalesce }))
		{
			return expression is LikeExpression;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (this != obj)
			{
				if (obj is SqlBinaryExpression sqlBinaryExpression)
				{
					return Equals(sqlBinaryExpression);
				}
				return false;
			}
			return true;
		}
		return false;
	}

	private bool Equals(SqlBinaryExpression sqlBinaryExpression)
	{
		if (base.Equals((object)sqlBinaryExpression) && OperatorType == sqlBinaryExpression.OperatorType && Left.Equals(sqlBinaryExpression.Left))
		{
			return Right.Equals(sqlBinaryExpression.Right);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), OperatorType, Left, Right);
	}
}
