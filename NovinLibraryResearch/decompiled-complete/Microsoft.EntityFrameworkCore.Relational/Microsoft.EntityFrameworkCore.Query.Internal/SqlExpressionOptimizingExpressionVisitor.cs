using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class SqlExpressionOptimizingExpressionVisitor : ExpressionVisitor
{
	private readonly bool _useRelationalNulls;

	protected virtual ISqlExpressionFactory SqlExpressionFactory { get; }

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

	public SqlExpressionOptimizingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory, bool useRelationalNulls)
	{
		SqlExpressionFactory = sqlExpressionFactory;
		_useRelationalNulls = useRelationalNulls;
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (!(extensionExpression is SqlUnaryExpression sqlUnaryExpression))
		{
			if (!(extensionExpression is SqlBinaryExpression sqlBinaryExpression))
			{
				if (extensionExpression is SelectExpression selectExpression)
				{
					return VisitSelectExpression(selectExpression);
				}
				return base.VisitExtension(extensionExpression);
			}
			return VisitSqlBinaryExpression(sqlBinaryExpression);
		}
		return VisitSqlUnaryExpression(sqlUnaryExpression);
	}

	private Expression VisitSelectExpression(SelectExpression selectExpression)
	{
		Expression expression = base.VisitExtension(selectExpression);
		if (expression is SelectExpression selectExpression2)
		{
			bool flag = false;
			SqlExpression predicate = selectExpression2.Predicate;
			SqlExpression havingExpression = selectExpression2.Having;
			bool flag2 = default(bool);
			int num;
			if (selectExpression2.Predicate is SqlConstantExpression { Value: var value })
			{
				if (value is bool)
				{
					flag2 = (bool)value;
					num = 1;
				}
				else
				{
					num = 0;
				}
			}
			else
			{
				num = 0;
			}
			if (((uint)num & (flag2 ? 1u : 0u)) != 0)
			{
				predicate = null;
				flag = true;
			}
			bool flag3 = default(bool);
			int num2;
			if (selectExpression2.Having is SqlConstantExpression { Value: var value2 })
			{
				if (value2 is bool)
				{
					flag3 = (bool)value2;
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
			}
			else
			{
				num2 = 0;
			}
			if (((uint)num2 & (flag3 ? 1u : 0u)) != 0)
			{
				havingExpression = null;
				flag = true;
			}
			if (!flag)
			{
				return selectExpression2;
			}
			return selectExpression2.Update(selectExpression2.Projection.ToList(), selectExpression2.Tables.ToList(), predicate, selectExpression2.GroupBy.ToList(), havingExpression, selectExpression2.Orderings.ToList(), selectExpression2.Limit, selectExpression2.Offset, selectExpression2.IsDistinct, selectExpression2.Alias);
		}
		return expression;
	}

	protected virtual Expression VisitSqlUnaryExpression(SqlUnaryExpression sqlUnaryExpression)
	{
		SqlExpression operand = (SqlExpression)Visit(sqlUnaryExpression.Operand);
		return SimplifyUnaryExpression(sqlUnaryExpression.OperatorType, operand, sqlUnaryExpression.Type, sqlUnaryExpression.TypeMapping);
	}

	private SqlExpression SimplifyUnaryExpression(ExpressionType operatorType, SqlExpression operand, Type type, RelationalTypeMapping typeMapping)
	{
		switch (operatorType)
		{
		case ExpressionType.Not:
			if (!(operand is SqlConstantExpression { Value: var value }))
			{
				if (operand is InExpression inExpression)
				{
					return inExpression.Negate();
				}
				if (!(operand is SqlUnaryExpression { OperatorType: var operatorType2 } sqlUnaryExpression))
				{
					if (operand is SqlBinaryExpression sqlBinaryExpression)
					{
						if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse)
						{
							SqlExpression left = SimplifyUnaryExpression(ExpressionType.Not, sqlBinaryExpression.Left, type, typeMapping);
							SqlExpression right = SimplifyUnaryExpression(ExpressionType.Not, sqlBinaryExpression.Right, type, typeMapping);
							return SimplifyLogicalSqlBinaryExpression((sqlBinaryExpression.OperatorType == ExpressionType.AndAlso) ? ExpressionType.OrElse : ExpressionType.AndAlso, left, right, sqlBinaryExpression.TypeMapping);
						}
						if (!_useRelationalNulls && TryNegate(sqlBinaryExpression.OperatorType, out var result))
						{
							return SimplifyBinaryExpression(result, sqlBinaryExpression.Left, sqlBinaryExpression.Right, sqlBinaryExpression.TypeMapping);
						}
					}
				}
				else
				{
					switch (operatorType2)
					{
					case ExpressionType.Not:
						return sqlUnaryExpression.Operand;
					case ExpressionType.Equal:
						return SqlExpressionFactory.IsNotNull(sqlUnaryExpression.Operand);
					case ExpressionType.NotEqual:
						return SqlExpressionFactory.IsNull(sqlUnaryExpression.Operand);
					}
				}
			}
			else if (value is bool flag)
			{
				return SqlExpressionFactory.Constant(!flag, typeMapping);
			}
			break;
		case ExpressionType.Equal:
		case ExpressionType.NotEqual:
			return SimplifyNullNotNullExpression(operatorType, operand, type, typeMapping);
		}
		return SqlExpressionFactory.MakeUnary(operatorType, operand, type, typeMapping);
	}

	private SqlExpression SimplifyNullNotNullExpression(ExpressionType operatorType, SqlExpression operand, Type type, RelationalTypeMapping typeMapping)
	{
		if (operatorType == ExpressionType.Equal || operatorType == ExpressionType.NotEqual)
		{
			if (operand is SqlConstantExpression sqlConstantExpression)
			{
				return SqlExpressionFactory.Constant((operatorType == ExpressionType.Equal) ? (sqlConstantExpression.Value == null) : (sqlConstantExpression.Value != null), typeMapping);
			}
			if (!(operand is ColumnExpression columnExpression))
			{
				if (!(operand is SqlUnaryExpression sqlUnaryExpression))
				{
					if (operand is SqlBinaryExpression { OperatorType: not ExpressionType.AndAlso, OperatorType: not ExpressionType.OrElse, OperatorType: not ExpressionType.And, OperatorType: not ExpressionType.Or } sqlBinaryExpression)
					{
						SqlExpression left = SimplifyNullNotNullExpression(operatorType, sqlBinaryExpression.Left, typeof(bool), typeMapping);
						SqlExpression right = SimplifyNullNotNullExpression(operatorType, sqlBinaryExpression.Right, typeof(bool), typeMapping);
						if (sqlBinaryExpression.OperatorType != ExpressionType.Coalesce)
						{
							return SimplifyLogicalSqlBinaryExpression((operatorType == ExpressionType.Equal) ? ExpressionType.OrElse : ExpressionType.AndAlso, left, right, typeMapping);
						}
						return SimplifyLogicalSqlBinaryExpression((operatorType == ExpressionType.Equal) ? ExpressionType.AndAlso : ExpressionType.OrElse, left, right, typeMapping);
					}
				}
				else
				{
					if (sqlUnaryExpression.OperatorType == ExpressionType.Convert || sqlUnaryExpression.OperatorType == ExpressionType.Not || sqlUnaryExpression.OperatorType == ExpressionType.Negate)
					{
						return SimplifyNullNotNullExpression(operatorType, sqlUnaryExpression.Operand, type, typeMapping);
					}
					if (sqlUnaryExpression.OperatorType == ExpressionType.Equal || sqlUnaryExpression.OperatorType == ExpressionType.NotEqual)
					{
						return SqlExpressionFactory.Constant(operatorType == ExpressionType.NotEqual, typeMapping);
					}
				}
			}
			else if (!columnExpression.IsNullable)
			{
				return SqlExpressionFactory.Constant(operatorType == ExpressionType.NotEqual, typeMapping);
			}
		}
		return SqlExpressionFactory.MakeUnary(operatorType, operand, type, typeMapping);
	}

	protected virtual Expression VisitSqlBinaryExpression(SqlBinaryExpression sqlBinaryExpression)
	{
		SqlExpression left = (SqlExpression)Visit(sqlBinaryExpression.Left);
		SqlExpression right = (SqlExpression)Visit(sqlBinaryExpression.Right);
		return SimplifyBinaryExpression(sqlBinaryExpression.OperatorType, left, right, sqlBinaryExpression.TypeMapping);
	}

	private SqlExpression SimplifyBinaryExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping)
	{
		switch (operatorType)
		{
		case ExpressionType.AndAlso:
		case ExpressionType.OrElse:
		{
			SqlUnaryExpression sqlUnaryExpression = left as SqlUnaryExpression;
			SqlUnaryExpression sqlUnaryExpression2 = right as SqlUnaryExpression;
			if (sqlUnaryExpression != null && sqlUnaryExpression2 != null && (sqlUnaryExpression.OperatorType == ExpressionType.Equal || sqlUnaryExpression.OperatorType == ExpressionType.NotEqual) && (sqlUnaryExpression2.OperatorType == ExpressionType.Equal || sqlUnaryExpression2.OperatorType == ExpressionType.NotEqual) && sqlUnaryExpression.Operand.Equals(sqlUnaryExpression2.Operand))
			{
				if (sqlUnaryExpression.OperatorType != sqlUnaryExpression2.OperatorType)
				{
					return SqlExpressionFactory.Constant(operatorType == ExpressionType.OrElse, typeMapping);
				}
				return sqlUnaryExpression;
			}
			return SimplifyLogicalSqlBinaryExpression(operatorType, left, right, typeMapping);
		}
		case ExpressionType.Equal:
		case ExpressionType.NotEqual:
		{
			SqlConstantExpression sqlConstantExpression = left as SqlConstantExpression;
			SqlConstantExpression sqlConstantExpression2 = right as SqlConstantExpression;
			bool flag = sqlConstantExpression != null && sqlConstantExpression.Value == null;
			bool flag2 = sqlConstantExpression2 != null && sqlConstantExpression2.Value == null;
			if (flag || flag2)
			{
				return SimplifyNullComparisonExpression(operatorType, left, right, flag, flag2, typeMapping);
			}
			SqlConstantExpression sqlConstantExpression3 = ((left.Type == typeof(bool)) ? sqlConstantExpression : null);
			SqlConstantExpression sqlConstantExpression4 = ((right.Type == typeof(bool)) ? sqlConstantExpression2 : null);
			if (sqlConstantExpression3 != null || sqlConstantExpression4 != null)
			{
				return SimplifyBoolConstantComparisonExpression(operatorType, left, right, sqlConstantExpression3, sqlConstantExpression4, typeMapping);
			}
			if ((left is LikeExpression || left is ColumnExpression { IsNullable: false }) && left.Equals(right))
			{
				return SqlExpressionFactory.Constant(operatorType == ExpressionType.Equal, typeMapping);
			}
			break;
		}
		}
		return SqlExpressionFactory.MakeBinary(operatorType, left, right, typeMapping);
	}

	protected virtual SqlExpression SimplifyNullComparisonExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, bool leftNull, bool rightNull, RelationalTypeMapping typeMapping)
	{
		if ((operatorType == ExpressionType.Equal || operatorType == ExpressionType.NotEqual) && (leftNull || rightNull))
		{
			if (leftNull && rightNull)
			{
				return SqlExpressionFactory.Constant(operatorType == ExpressionType.Equal, typeMapping);
			}
			if (leftNull)
			{
				return SimplifyNullNotNullExpression(operatorType, right, typeof(bool), typeMapping);
			}
			if (rightNull)
			{
				return SimplifyNullNotNullExpression(operatorType, left, typeof(bool), typeMapping);
			}
		}
		return SqlExpressionFactory.MakeBinary(operatorType, left, right, typeMapping);
	}

	private SqlExpression SimplifyBoolConstantComparisonExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, SqlConstantExpression leftBoolConstant, SqlConstantExpression rightBoolConstant, RelationalTypeMapping typeMapping)
	{
		if (leftBoolConstant != null && rightBoolConstant != null)
		{
			return SqlExpressionFactory.MakeBinary(operatorType, left, right, typeMapping);
		}
		if (rightBoolConstant != null && CanOptimize(left))
		{
			if (operatorType != ExpressionType.Equal)
			{
				if (!(bool)rightBoolConstant.Value)
				{
					return left;
				}
				return SimplifyUnaryExpression(ExpressionType.Not, left, typeof(bool), typeMapping);
			}
			if (!(bool)rightBoolConstant.Value)
			{
				return SimplifyUnaryExpression(ExpressionType.Not, left, typeof(bool), typeMapping);
			}
			return left;
		}
		if (leftBoolConstant != null && CanOptimize(right))
		{
			if (operatorType != ExpressionType.Equal)
			{
				if (!(bool)leftBoolConstant.Value)
				{
					return right;
				}
				return SimplifyUnaryExpression(ExpressionType.Not, right, typeof(bool), typeMapping);
			}
			if (!(bool)leftBoolConstant.Value)
			{
				return SimplifyUnaryExpression(ExpressionType.Not, right, typeof(bool), typeMapping);
			}
			return right;
		}
		return SqlExpressionFactory.MakeBinary(operatorType, left, right, typeMapping);
		static bool CanOptimize(SqlExpression operand)
		{
			if (!(operand is LikeExpression))
			{
				if (operand is SqlUnaryExpression sqlUnaryExpression)
				{
					if (sqlUnaryExpression.OperatorType != ExpressionType.Equal)
					{
						return sqlUnaryExpression.OperatorType == ExpressionType.NotEqual;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}

	private SqlExpression SimplifyLogicalSqlBinaryExpression(ExpressionType operatorType, SqlExpression left, SqlExpression right, RelationalTypeMapping typeMapping)
	{
		if (left is SqlConstantExpression sqlConstantExpression)
		{
			if (operatorType != ExpressionType.AndAlso)
			{
				if (!(bool)sqlConstantExpression.Value)
				{
					return right;
				}
				return sqlConstantExpression;
			}
			if (!(bool)sqlConstantExpression.Value)
			{
				return sqlConstantExpression;
			}
			return right;
		}
		if (right is SqlConstantExpression sqlConstantExpression2)
		{
			if (operatorType != ExpressionType.AndAlso)
			{
				if (!(bool)sqlConstantExpression2.Value)
				{
					return left;
				}
				return sqlConstantExpression2;
			}
			if (!(bool)sqlConstantExpression2.Value)
			{
				return sqlConstantExpression2;
			}
			return left;
		}
		return SqlExpressionFactory.MakeBinary(operatorType, left, right, typeMapping);
	}
}
