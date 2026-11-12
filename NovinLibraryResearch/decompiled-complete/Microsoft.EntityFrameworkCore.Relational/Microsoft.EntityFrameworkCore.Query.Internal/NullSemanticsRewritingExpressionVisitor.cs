using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class NullSemanticsRewritingExpressionVisitor : SqlExpressionVisitor
{
	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private bool _isNullable;

	private bool _canOptimize;

	private readonly List<ColumnExpression> _nonNullableColumns = new List<ColumnExpression>();

	public NullSemanticsRewritingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
		_canOptimize = true;
	}

	protected override Expression VisitCase(CaseExpression caseExpression)
	{
		_isNullable = false;
		bool flag = caseExpression.ElseResult == null;
		bool canOptimize = _canOptimize;
		bool canOptimize2 = caseExpression.Operand == null;
		_canOptimize = false;
		SqlExpression operand = (SqlExpression)Visit(caseExpression.Operand);
		List<CaseWhenClause> list = new List<CaseWhenClause>();
		foreach (CaseWhenClause whenClause in caseExpression.WhenClauses)
		{
			_canOptimize = canOptimize2;
			SqlExpression test = (SqlExpression)Visit(whenClause.Test);
			_canOptimize = false;
			_isNullable = false;
			SqlExpression result = (SqlExpression)Visit(whenClause.Result);
			flag |= _isNullable;
			list.Add(new CaseWhenClause(test, result));
		}
		_canOptimize = false;
		SqlExpression elseResult = (SqlExpression)Visit(caseExpression.ElseResult);
		_isNullable |= flag;
		_canOptimize = canOptimize;
		return caseExpression.Update(operand, list, elseResult);
	}

	protected override Expression VisitColumn(ColumnExpression columnExpression)
	{
		_isNullable = !_nonNullableColumns.Contains(columnExpression) && columnExpression.IsNullable;
		return columnExpression;
	}

	protected override Expression VisitCrossApply(CrossApplyExpression crossApplyExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		TableExpressionBase table = (TableExpressionBase)Visit(crossApplyExpression.Table);
		_canOptimize = canOptimize;
		return crossApplyExpression.Update(table);
	}

	protected override Expression VisitCrossJoin(CrossJoinExpression crossJoinExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		TableExpressionBase table = (TableExpressionBase)Visit(crossJoinExpression.Table);
		_canOptimize = canOptimize;
		return crossJoinExpression.Update(table);
	}

	protected override Expression VisitExcept(ExceptExpression exceptExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SelectExpression source = (SelectExpression)Visit(exceptExpression.Source1);
		SelectExpression source2 = (SelectExpression)Visit(exceptExpression.Source2);
		_canOptimize = canOptimize;
		return exceptExpression.Update(source, source2);
	}

	protected override Expression VisitExists(ExistsExpression existsExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SelectExpression subquery = (SelectExpression)Visit(existsExpression.Subquery);
		_canOptimize = canOptimize;
		return existsExpression.Update(subquery);
	}

	protected override Expression VisitFromSql(FromSqlExpression fromSqlExpression)
	{
		return fromSqlExpression;
	}

	protected override Expression VisitIn(InExpression inExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		_isNullable = false;
		SqlExpression item = (SqlExpression)Visit(inExpression.Item);
		bool isNullable = _isNullable;
		_isNullable = false;
		SelectExpression subquery = (SelectExpression)Visit(inExpression.Subquery);
		isNullable |= _isNullable;
		_isNullable = false;
		SqlExpression values = (SqlExpression)Visit(inExpression.Values);
		_isNullable |= isNullable;
		_canOptimize = canOptimize;
		return inExpression.Update(item, values, subquery);
	}

	protected override Expression VisitIntersect(IntersectExpression intersectExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SelectExpression source = (SelectExpression)Visit(intersectExpression.Source1);
		SelectExpression source2 = (SelectExpression)Visit(intersectExpression.Source2);
		_canOptimize = canOptimize;
		return intersectExpression.Update(source, source2);
	}

	protected override Expression VisitLike(LikeExpression likeExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		_isNullable = false;
		SqlExpression match = (SqlExpression)Visit(likeExpression.Match);
		bool isNullable = _isNullable;
		_isNullable = false;
		SqlExpression pattern = (SqlExpression)Visit(likeExpression.Pattern);
		isNullable |= _isNullable;
		_isNullable = false;
		SqlExpression escapeChar = (SqlExpression)Visit(likeExpression.EscapeChar);
		_isNullable |= isNullable;
		_canOptimize = canOptimize;
		return likeExpression.Update(match, pattern, escapeChar);
	}

	protected override Expression VisitInnerJoin(InnerJoinExpression innerJoinExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		TableExpressionBase table = (TableExpressionBase)Visit(innerJoinExpression.Table);
		SqlExpression joinPredicate = VisitJoinPredicate((SqlBinaryExpression)innerJoinExpression.JoinPredicate);
		_canOptimize = canOptimize;
		return innerJoinExpression.Update(table, joinPredicate);
	}

	protected override Expression VisitLeftJoin(LeftJoinExpression leftJoinExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		TableExpressionBase table = (TableExpressionBase)Visit(leftJoinExpression.Table);
		SqlExpression joinPredicate = VisitJoinPredicate((SqlBinaryExpression)leftJoinExpression.JoinPredicate);
		_canOptimize = canOptimize;
		return leftJoinExpression.Update(table, joinPredicate);
	}

	private SqlExpression VisitJoinPredicate(SqlBinaryExpression predicate)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = true;
		if (predicate.OperatorType == ExpressionType.Equal)
		{
			SqlExpression left = (SqlExpression)Visit(predicate.Left);
			SqlExpression right = (SqlExpression)Visit(predicate.Right);
			_canOptimize = canOptimize;
			return predicate.Update(left, right);
		}
		if (predicate.OperatorType == ExpressionType.AndAlso)
		{
			SqlExpression result = (SqlExpression)VisitSqlBinary(predicate);
			_canOptimize = canOptimize;
			return result;
		}
		throw new InvalidOperationException("Unexpected join predicate shape: " + predicate);
	}

	protected override Expression VisitOrdering(OrderingExpression orderingExpression)
	{
		SqlExpression expression = (SqlExpression)Visit(orderingExpression.Expression);
		return orderingExpression.Update(expression);
	}

	protected override Expression VisitOuterApply(OuterApplyExpression outerApplyExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		TableExpressionBase table = (TableExpressionBase)Visit(outerApplyExpression.Table);
		_canOptimize = canOptimize;
		return outerApplyExpression.Update(table);
	}

	protected override Expression VisitProjection(ProjectionExpression projectionExpression)
	{
		SqlExpression expression = (SqlExpression)Visit(projectionExpression.Expression);
		return projectionExpression.Update(expression);
	}

	protected override Expression VisitRowNumber(RowNumberExpression rowNumberExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		bool flag = false;
		List<SqlExpression> list = new List<SqlExpression>();
		foreach (SqlExpression partition in rowNumberExpression.Partitions)
		{
			SqlExpression sqlExpression = (SqlExpression)Visit(partition);
			flag = flag || sqlExpression != partition;
			list.Add(sqlExpression);
		}
		List<OrderingExpression> list2 = new List<OrderingExpression>();
		foreach (OrderingExpression ordering in rowNumberExpression.Orderings)
		{
			OrderingExpression orderingExpression = (OrderingExpression)Visit(ordering);
			flag = flag || orderingExpression != ordering;
			list2.Add(orderingExpression);
		}
		_canOptimize = canOptimize;
		return rowNumberExpression.Update(list, list2);
	}

	protected override Expression VisitSelect(SelectExpression selectExpression)
	{
		bool flag = false;
		bool canOptimize = _canOptimize;
		List<ProjectionExpression> list = new List<ProjectionExpression>();
		_canOptimize = false;
		foreach (ProjectionExpression item in selectExpression.Projection)
		{
			ProjectionExpression projectionExpression = (ProjectionExpression)Visit(item);
			list.Add(projectionExpression);
			flag = flag || projectionExpression != item;
		}
		List<TableExpressionBase> list2 = new List<TableExpressionBase>();
		foreach (TableExpressionBase table in selectExpression.Tables)
		{
			TableExpressionBase tableExpressionBase = (TableExpressionBase)Visit(table);
			flag = flag || tableExpressionBase != table;
			list2.Add(tableExpressionBase);
		}
		_canOptimize = true;
		SqlExpression sqlExpression = (SqlExpression)Visit(selectExpression.Predicate);
		flag |= sqlExpression != selectExpression.Predicate;
		List<SqlExpression> list3 = new List<SqlExpression>();
		_canOptimize = false;
		foreach (SqlExpression item2 in selectExpression.GroupBy)
		{
			SqlExpression sqlExpression2 = (SqlExpression)Visit(item2);
			flag = flag || sqlExpression2 != item2;
			list3.Add(sqlExpression2);
		}
		_canOptimize = true;
		SqlExpression sqlExpression3 = (SqlExpression)Visit(selectExpression.Having);
		flag |= sqlExpression3 != selectExpression.Having;
		List<OrderingExpression> list4 = new List<OrderingExpression>();
		_canOptimize = false;
		foreach (OrderingExpression ordering in selectExpression.Orderings)
		{
			SqlExpression sqlExpression4 = (SqlExpression)Visit(ordering.Expression);
			flag |= sqlExpression4 != ordering.Expression;
			list4.Add(ordering.Update(sqlExpression4));
		}
		SqlExpression sqlExpression5 = (SqlExpression)Visit(selectExpression.Offset);
		flag |= sqlExpression5 != selectExpression.Offset;
		SqlExpression sqlExpression6 = (SqlExpression)Visit(selectExpression.Limit);
		flag |= sqlExpression6 != selectExpression.Limit;
		_canOptimize = canOptimize;
		_isNullable = true;
		if (!flag)
		{
			return selectExpression;
		}
		return selectExpression.Update(list, list2, sqlExpression, list3, sqlExpression3, list4, sqlExpression6, sqlExpression5, selectExpression.IsDistinct, selectExpression.Alias);
	}

	protected override Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression)
	{
		_isNullable = false;
		bool canOptimize = _canOptimize;
		_canOptimize = _canOptimize && (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso || sqlBinaryExpression.OperatorType == ExpressionType.OrElse);
		List<ColumnExpression> list = new List<ColumnExpression>();
		if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso)
		{
			list = FindNonNullableColumns(sqlBinaryExpression.Left);
		}
		SqlExpression sqlExpression = (SqlExpression)Visit(sqlBinaryExpression.Left);
		bool isNullable = _isNullable;
		_isNullable = false;
		if (list.Count > 0)
		{
			_nonNullableColumns.AddRange(list);
		}
		SqlExpression sqlExpression2 = (SqlExpression)Visit(sqlBinaryExpression.Right);
		bool isNullable2 = _isNullable;
		foreach (ColumnExpression item in list)
		{
			_nonNullableColumns.Remove(item);
		}
		if (sqlBinaryExpression.OperatorType == ExpressionType.Coalesce)
		{
			_isNullable = isNullable && isNullable2;
			_canOptimize = canOptimize;
			return sqlBinaryExpression.Update(sqlExpression, sqlExpression2);
		}
		if (sqlBinaryExpression.OperatorType == ExpressionType.Equal || sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
		{
			bool flag = sqlExpression is SqlConstantExpression sqlConstantExpression && sqlConstantExpression.Value == null;
			if (sqlExpression2 is SqlConstantExpression { Value: null })
			{
				_isNullable = false;
				_canOptimize = canOptimize;
				if (sqlBinaryExpression.OperatorType != ExpressionType.Equal)
				{
					return _sqlExpressionFactory.IsNotNull(sqlExpression);
				}
				return _sqlExpressionFactory.IsNull(sqlExpression);
			}
			if (flag)
			{
				_isNullable = false;
				_canOptimize = canOptimize;
				if (sqlBinaryExpression.OperatorType != ExpressionType.Equal)
				{
					return _sqlExpressionFactory.IsNotNull(sqlExpression2);
				}
				return _sqlExpressionFactory.IsNull(sqlExpression2);
			}
			SqlUnaryExpression sqlUnaryExpression = sqlExpression as SqlUnaryExpression;
			SqlUnaryExpression sqlUnaryExpression2 = sqlExpression2 as SqlUnaryExpression;
			bool flag2 = sqlUnaryExpression != null && sqlUnaryExpression.OperatorType == ExpressionType.Not;
			bool flag3 = sqlUnaryExpression2 != null && sqlUnaryExpression2.OperatorType == ExpressionType.Not;
			if (flag2)
			{
				sqlExpression = sqlUnaryExpression.Operand;
			}
			if (flag3)
			{
				sqlExpression2 = sqlUnaryExpression2.Operand;
			}
			SqlUnaryExpression sqlUnaryExpression3 = _sqlExpressionFactory.IsNull(sqlExpression);
			SqlUnaryExpression sqlUnaryExpression4 = _sqlExpressionFactory.IsNull(sqlExpression2);
			if (canOptimize && sqlBinaryExpression.OperatorType == ExpressionType.Equal && !flag2 && !flag3)
			{
				if (isNullable && isNullable2)
				{
					_isNullable = true;
					_canOptimize = canOptimize;
					return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(sqlExpression, sqlExpression2), _sqlExpressionFactory.AndAlso(sqlUnaryExpression3, sqlUnaryExpression4));
				}
				if ((isNullable && !isNullable2) || (!isNullable && isNullable2))
				{
					_isNullable = true;
					_canOptimize = canOptimize;
					return _sqlExpressionFactory.Equal(sqlExpression, sqlExpression2);
				}
			}
			_isNullable = false;
			_canOptimize = canOptimize;
			if (sqlBinaryExpression.OperatorType == ExpressionType.Equal)
			{
				if (!isNullable && !isNullable2)
				{
					if (flag2 != flag3)
					{
						return _sqlExpressionFactory.NotEqual(sqlExpression, sqlExpression2);
					}
					return _sqlExpressionFactory.Equal(sqlExpression, sqlExpression2);
				}
				if (isNullable && isNullable2)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableEqualNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3, sqlUnaryExpression4);
					}
					return ExpandNullableEqualNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3, sqlUnaryExpression4);
				}
				if (isNullable && !isNullable2)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3);
					}
					return ExpandNullableEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3);
				}
				if (isNullable2 && !isNullable)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression4);
					}
					return ExpandNullableEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression4);
				}
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				if (!isNullable && !isNullable2)
				{
					if (flag2 != flag3)
					{
						return _sqlExpressionFactory.Equal(sqlExpression, sqlExpression2);
					}
					return _sqlExpressionFactory.NotEqual(sqlExpression, sqlExpression2);
				}
				if (isNullable && isNullable2)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableNotEqualNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3, sqlUnaryExpression4);
					}
					return ExpandNullableNotEqualNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3, sqlUnaryExpression4);
				}
				if (isNullable)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableNotEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3);
					}
					return ExpandNullableNotEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression3);
				}
				if (isNullable2)
				{
					if (flag2 != flag3)
					{
						return ExpandNegatedNullableNotEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression4);
					}
					return ExpandNullableNotEqualNonNullable(sqlExpression, sqlExpression2, sqlUnaryExpression4);
				}
			}
		}
		_isNullable = isNullable || isNullable2;
		_canOptimize = canOptimize;
		return sqlBinaryExpression.Update(sqlExpression, sqlExpression2);
	}

	protected override Expression VisitSqlConstant(SqlConstantExpression sqlConstantExpression)
	{
		_isNullable = sqlConstantExpression.Value == null;
		return sqlConstantExpression;
	}

	protected override Expression VisitSqlFragment(SqlFragmentExpression sqlFragmentExpression)
	{
		return sqlFragmentExpression;
	}

	protected override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SqlExpression instance = (SqlExpression)Visit(sqlFunctionExpression.Instance);
		SqlExpression[] array = new SqlExpression[sqlFunctionExpression.Arguments.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (SqlExpression)Visit(sqlFunctionExpression.Arguments[i]);
		}
		_canOptimize = canOptimize;
		_isNullable = true;
		return sqlFunctionExpression.Update(instance, array);
	}

	protected override Expression VisitSqlParameter(SqlParameterExpression sqlParameterExpression)
	{
		_isNullable = true;
		return sqlParameterExpression;
	}

	protected override Expression VisitSqlUnary(SqlUnaryExpression sqlCastExpression)
	{
		_isNullable = false;
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SqlExpression operand = (SqlExpression)Visit(sqlCastExpression.Operand);
		if (sqlCastExpression.OperatorType == ExpressionType.Equal || sqlCastExpression.OperatorType == ExpressionType.NotEqual)
		{
			_isNullable = false;
		}
		_canOptimize = canOptimize;
		return sqlCastExpression.Update(operand);
	}

	protected override Expression VisitSubSelect(ScalarSubqueryExpression scalarSubqueryExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SelectExpression subquery = (SelectExpression)Visit(scalarSubqueryExpression.Subquery);
		_canOptimize = canOptimize;
		return scalarSubqueryExpression.Update(subquery);
	}

	protected override Expression VisitTable(TableExpression tableExpression)
	{
		return tableExpression;
	}

	protected override Expression VisitUnion(UnionExpression unionExpression)
	{
		bool canOptimize = _canOptimize;
		_canOptimize = false;
		SelectExpression source = (SelectExpression)Visit(unionExpression.Source1);
		SelectExpression source2 = (SelectExpression)Visit(unionExpression.Source2);
		_canOptimize = canOptimize;
		return unionExpression.Update(source, source2);
	}

	private List<ColumnExpression> FindNonNullableColumns(SqlExpression sqlExpression)
	{
		List<ColumnExpression> list = new List<ColumnExpression>();
		if (sqlExpression is SqlBinaryExpression sqlBinaryExpression)
		{
			if (sqlBinaryExpression.OperatorType == ExpressionType.NotEqual)
			{
				if (sqlBinaryExpression.Left is ColumnExpression { IsNullable: not false } columnExpression && sqlBinaryExpression.Right is SqlConstantExpression { Value: null })
				{
					list.Add(columnExpression);
				}
				if (sqlBinaryExpression.Right is ColumnExpression { IsNullable: not false } columnExpression2 && sqlBinaryExpression.Left is SqlConstantExpression { Value: null })
				{
					list.Add(columnExpression2);
				}
			}
			if (sqlBinaryExpression.OperatorType == ExpressionType.AndAlso)
			{
				list.AddRange(FindNonNullableColumns(sqlBinaryExpression.Left));
				list.AddRange(FindNonNullableColumns(sqlBinaryExpression.Right));
			}
		}
		return list;
	}

	private SqlBinaryExpression ExpandNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression rightIsNull)
	{
		return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Equal(left, right), _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Not(leftIsNull), _sqlExpressionFactory.Not(rightIsNull))), _sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull));
	}

	private SqlBinaryExpression ExpandNegatedNullableEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression rightIsNull)
	{
		return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.AndAlso(_sqlExpressionFactory.NotEqual(left, right), _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Not(leftIsNull), _sqlExpressionFactory.Not(rightIsNull))), _sqlExpressionFactory.AndAlso(leftIsNull, rightIsNull));
	}

	private SqlBinaryExpression ExpandNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
	{
		return _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.Equal(left, right), _sqlExpressionFactory.Not(leftIsNull));
	}

	private SqlBinaryExpression ExpandNegatedNullableEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
	{
		return _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.NotEqual(left, right), _sqlExpressionFactory.Not(leftIsNull));
	}

	private SqlBinaryExpression ExpandNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression rightIsNull)
	{
		return _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.NotEqual(left, right), _sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)), _sqlExpressionFactory.OrElse(_sqlExpressionFactory.Not(leftIsNull), _sqlExpressionFactory.Not(rightIsNull)));
	}

	private SqlBinaryExpression ExpandNegatedNullableNotEqualNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull, SqlExpression rightIsNull)
	{
		return _sqlExpressionFactory.AndAlso(_sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(left, right), _sqlExpressionFactory.OrElse(leftIsNull, rightIsNull)), _sqlExpressionFactory.OrElse(_sqlExpressionFactory.Not(leftIsNull), _sqlExpressionFactory.Not(rightIsNull)));
	}

	private SqlBinaryExpression ExpandNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
	{
		return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.NotEqual(left, right), leftIsNull);
	}

	private SqlBinaryExpression ExpandNegatedNullableNotEqualNonNullable(SqlExpression left, SqlExpression right, SqlExpression leftIsNull)
	{
		return _sqlExpressionFactory.OrElse(_sqlExpressionFactory.Equal(left, right), leftIsNull);
	}
}
