using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public abstract class SqlExpressionVisitor : ExpressionVisitor
{
	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (!(extensionExpression is CaseExpression caseExpression))
		{
			if (!(extensionExpression is ColumnExpression columnExpression))
			{
				if (!(extensionExpression is CrossApplyExpression crossApplyExpression))
				{
					if (!(extensionExpression is CrossJoinExpression crossJoinExpression))
					{
						if (!(extensionExpression is ExceptExpression exceptExpression))
						{
							if (!(extensionExpression is ExistsExpression existsExpression))
							{
								if (!(extensionExpression is FromSqlExpression fromSqlExpression))
								{
									if (!(extensionExpression is InExpression inExpression))
									{
										if (!(extensionExpression is IntersectExpression intersectExpression))
										{
											if (!(extensionExpression is InnerJoinExpression innerJoinExpression))
											{
												if (!(extensionExpression is LeftJoinExpression leftJoinExpression))
												{
													if (!(extensionExpression is LikeExpression likeExpression))
													{
														if (!(extensionExpression is OrderingExpression orderingExpression))
														{
															if (!(extensionExpression is OuterApplyExpression outerApplyExpression))
															{
																if (!(extensionExpression is ProjectionExpression projectionExpression))
																{
																	if (!(extensionExpression is RowNumberExpression rowNumberExpression))
																	{
																		if (!(extensionExpression is SelectExpression selectExpression))
																		{
																			if (!(extensionExpression is SqlBinaryExpression sqlBinaryExpression))
																			{
																				if (!(extensionExpression is SqlUnaryExpression sqlCastExpression))
																				{
																					if (!(extensionExpression is SqlConstantExpression sqlConstantExpression))
																					{
																						if (!(extensionExpression is SqlFragmentExpression sqlFragmentExpression))
																						{
																							if (!(extensionExpression is SqlFunctionExpression sqlFunctionExpression))
																							{
																								if (!(extensionExpression is SqlParameterExpression sqlParameterExpression))
																								{
																									if (!(extensionExpression is ScalarSubqueryExpression scalarSubqueryExpression))
																									{
																										if (!(extensionExpression is TableExpression tableExpression))
																										{
																											if (extensionExpression is UnionExpression unionExpression)
																											{
																												return VisitUnion(unionExpression);
																											}
																											return base.VisitExtension(extensionExpression);
																										}
																										return VisitTable(tableExpression);
																									}
																									return VisitSubSelect(scalarSubqueryExpression);
																								}
																								return VisitSqlParameter(sqlParameterExpression);
																							}
																							return VisitSqlFunction(sqlFunctionExpression);
																						}
																						return VisitSqlFragment(sqlFragmentExpression);
																					}
																					return VisitSqlConstant(sqlConstantExpression);
																				}
																				return VisitSqlUnary(sqlCastExpression);
																			}
																			return VisitSqlBinary(sqlBinaryExpression);
																		}
																		return VisitSelect(selectExpression);
																	}
																	return VisitRowNumber(rowNumberExpression);
																}
																return VisitProjection(projectionExpression);
															}
															return VisitOuterApply(outerApplyExpression);
														}
														return VisitOrdering(orderingExpression);
													}
													return VisitLike(likeExpression);
												}
												return VisitLeftJoin(leftJoinExpression);
											}
											return VisitInnerJoin(innerJoinExpression);
										}
										return VisitIntersect(intersectExpression);
									}
									return VisitIn(inExpression);
								}
								return VisitFromSql(fromSqlExpression);
							}
							return VisitExists(existsExpression);
						}
						return VisitExcept(exceptExpression);
					}
					return VisitCrossJoin(crossJoinExpression);
				}
				return VisitCrossApply(crossApplyExpression);
			}
			return VisitColumn(columnExpression);
		}
		return VisitCase(caseExpression);
	}

	protected abstract Expression VisitCase(CaseExpression caseExpression);

	protected abstract Expression VisitColumn(ColumnExpression columnExpression);

	protected abstract Expression VisitCrossApply(CrossApplyExpression crossApplyExpression);

	protected abstract Expression VisitCrossJoin(CrossJoinExpression crossJoinExpression);

	protected abstract Expression VisitExcept(ExceptExpression exceptExpression);

	protected abstract Expression VisitExists(ExistsExpression existsExpression);

	protected abstract Expression VisitFromSql(FromSqlExpression fromSqlExpression);

	protected abstract Expression VisitIn(InExpression inExpression);

	protected abstract Expression VisitIntersect(IntersectExpression intersectExpression);

	protected abstract Expression VisitLike(LikeExpression likeExpression);

	protected abstract Expression VisitInnerJoin(InnerJoinExpression innerJoinExpression);

	protected abstract Expression VisitLeftJoin(LeftJoinExpression leftJoinExpression);

	protected abstract Expression VisitOrdering(OrderingExpression orderingExpression);

	protected abstract Expression VisitOuterApply(OuterApplyExpression outerApplyExpression);

	protected abstract Expression VisitProjection(ProjectionExpression projectionExpression);

	protected abstract Expression VisitRowNumber(RowNumberExpression rowNumberExpression);

	protected abstract Expression VisitSelect(SelectExpression selectExpression);

	protected abstract Expression VisitSqlBinary(SqlBinaryExpression sqlBinaryExpression);

	protected abstract Expression VisitSqlConstant(SqlConstantExpression sqlConstantExpression);

	protected abstract Expression VisitSqlFragment(SqlFragmentExpression sqlFragmentExpression);

	protected abstract Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression);

	protected abstract Expression VisitSqlParameter(SqlParameterExpression sqlParameterExpression);

	protected abstract Expression VisitSqlUnary(SqlUnaryExpression sqlCastExpression);

	protected abstract Expression VisitSubSelect(ScalarSubqueryExpression scalarSubqueryExpression);

	protected abstract Expression VisitTable(TableExpression tableExpression);

	protected abstract Expression VisitUnion(UnionExpression unionExpression);
}
