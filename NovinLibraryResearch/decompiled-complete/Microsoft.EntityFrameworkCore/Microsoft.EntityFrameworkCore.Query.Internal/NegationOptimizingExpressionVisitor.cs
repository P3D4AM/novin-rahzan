using System.Collections.Generic;
using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NegationOptimizingExpressionVisitor : ExpressionVisitor
{
	private static bool TryNegate(ExpressionType expressionType, out ExpressionType result)
	{
		ExpressionType? expressionType2 = expressionType switch
		{
			ExpressionType.AndAlso => ExpressionType.OrElse, 
			ExpressionType.OrElse => ExpressionType.AndAlso, 
			ExpressionType.Equal => ExpressionType.NotEqual, 
			ExpressionType.NotEqual => ExpressionType.Equal, 
			ExpressionType.GreaterThan => ExpressionType.LessThanOrEqual, 
			ExpressionType.GreaterThanOrEqual => ExpressionType.LessThan, 
			ExpressionType.LessThan => ExpressionType.GreaterThanOrEqual, 
			ExpressionType.LessThanOrEqual => ExpressionType.GreaterThan, 
			_ => null, 
		};
		result = expressionType2.GetValueOrDefault();
		return expressionType2.HasValue;
	}

	private static ExpressionType Negate(ExpressionType expressionType)
	{
		if (!TryNegate(expressionType, out var result))
		{
			throw new KeyNotFoundException();
		}
		return result;
	}

	protected override Expression VisitUnary(UnaryExpression unaryExpression)
	{
		if (unaryExpression.NodeType == ExpressionType.Not)
		{
			if (unaryExpression.Operand is ConstantExpression constantExpression && constantExpression.Value is bool flag)
			{
				return Expression.Constant(!flag);
			}
			if (unaryExpression.Operand is UnaryExpression { NodeType: ExpressionType.Not } unaryExpression2)
			{
				return Visit(unaryExpression2.Operand);
			}
			if (unaryExpression.Operand is BinaryExpression binaryExpression)
			{
				if (binaryExpression.NodeType == ExpressionType.AndAlso || binaryExpression.NodeType == ExpressionType.OrElse)
				{
					return Visit(Expression.MakeBinary(Negate(binaryExpression.NodeType), Expression.Not(binaryExpression.Left), Expression.Not(binaryExpression.Right)));
				}
				if (TryNegate(binaryExpression.NodeType, out var result))
				{
					return Visit(Expression.MakeBinary(result, binaryExpression.Left, binaryExpression.Right));
				}
			}
		}
		return base.VisitUnary(unaryExpression);
	}
}
