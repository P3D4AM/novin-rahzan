using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class ParameterValueBasedSelectExpressionOptimizer
{
	private class ParameterNullabilityBasedSqlExpressionOptimizingExpressionVisitor : SqlExpressionOptimizingExpressionVisitor
	{
		private readonly IReadOnlyDictionary<string, object> _parametersValues;

		public ParameterNullabilityBasedSqlExpressionOptimizingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory, bool useRelationalNulls, IReadOnlyDictionary<string, object> parametersValues)
			: base(sqlExpressionFactory, useRelationalNulls)
		{
			_parametersValues = parametersValues;
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			Expression expression = base.VisitExtension(extensionExpression);
			if (expression is SelectExpression selectExpression)
			{
				bool flag = false;
				SqlExpression predicate = selectExpression.Predicate;
				SqlExpression havingExpression = selectExpression.Having;
				if (selectExpression.Predicate is SqlConstantExpression { Value: var value } sqlConstantExpression && value is bool flag2)
				{
					flag = true;
					predicate = (flag2 ? null : GenerateEqualTrueExpression(sqlConstantExpression));
				}
				if (selectExpression.Having is SqlConstantExpression { Value: var value2 } sqlConstantExpression2 && value2 is bool flag3)
				{
					flag = true;
					havingExpression = (flag3 ? null : GenerateEqualTrueExpression(sqlConstantExpression2));
				}
				if (!flag)
				{
					return selectExpression;
				}
				return selectExpression.Update(selectExpression.Projection.ToList(), selectExpression.Tables.ToList(), predicate, selectExpression.GroupBy.ToList(), havingExpression, selectExpression.Orderings.ToList(), selectExpression.Limit, selectExpression.Offset, selectExpression.IsDistinct, selectExpression.Alias);
			}
			if (expression is CaseExpression { Operand: null } caseExpression)
			{
				bool flag4 = false;
				List<CaseWhenClause> list = new List<CaseWhenClause>();
				for (int i = 0; i < caseExpression.WhenClauses.Count; i++)
				{
					if (caseExpression.WhenClauses[i].Test is SqlConstantExpression sqlConstantExpression3 && sqlConstantExpression3.Value is bool)
					{
						flag4 = true;
						list.Add(new CaseWhenClause(GenerateEqualTrueExpression(sqlConstantExpression3), caseExpression.WhenClauses[i].Result));
					}
					else
					{
						list.Add(caseExpression.WhenClauses[i]);
					}
				}
				if (!flag4)
				{
					return caseExpression;
				}
				return caseExpression.Update(caseExpression.Operand, list, caseExpression.ElseResult);
			}
			return expression;
			SqlExpression GenerateEqualTrueExpression(SqlExpression argument)
			{
				return SqlExpressionFactory.Equal(argument, SqlExpressionFactory.Constant(true, argument.TypeMapping));
			}
		}

		protected override Expression VisitSqlUnaryExpression(SqlUnaryExpression sqlUnaryExpression)
		{
			Expression expression = base.VisitSqlUnaryExpression(sqlUnaryExpression);
			if (expression is SqlUnaryExpression { Operand: SqlParameterExpression operand })
			{
				object obj = _parametersValues[operand.Name];
				if (sqlUnaryExpression.OperatorType == ExpressionType.Equal)
				{
					return SqlExpressionFactory.Constant(obj == null, sqlUnaryExpression.TypeMapping);
				}
				if (sqlUnaryExpression.OperatorType == ExpressionType.NotEqual)
				{
					return SqlExpressionFactory.Constant(obj != null, sqlUnaryExpression.TypeMapping);
				}
			}
			return expression;
		}

		protected override Expression VisitSqlBinaryExpression(SqlBinaryExpression sqlBinaryExpression)
		{
			Expression expression = base.VisitSqlBinaryExpression(sqlBinaryExpression);
			if (expression is SqlBinaryExpression sqlBinaryExpression2)
			{
				bool flag = sqlBinaryExpression2.Left is SqlParameterExpression sqlParameterExpression && _parametersValues[sqlParameterExpression.Name] == null;
				bool flag2 = sqlBinaryExpression2.Right is SqlParameterExpression sqlParameterExpression2 && _parametersValues[sqlParameterExpression2.Name] == null;
				if ((sqlBinaryExpression2.OperatorType == ExpressionType.Equal || sqlBinaryExpression2.OperatorType == ExpressionType.NotEqual) && (flag || flag2))
				{
					return SimplifyNullComparisonExpression(sqlBinaryExpression2.OperatorType, sqlBinaryExpression2.Left, sqlBinaryExpression2.Right, flag, flag2, sqlBinaryExpression2.TypeMapping);
				}
			}
			return expression;
		}
	}

	private class InExpressionValuesExpandingExpressionVisitor : ExpressionVisitor
	{
		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		private readonly IReadOnlyDictionary<string, object> _parametersValues;

		public InExpressionValuesExpandingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory, IReadOnlyDictionary<string, object> parametersValues)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
			_parametersValues = parametersValues;
		}

		public override Expression Visit(Expression expression)
		{
			if (expression is InExpression { Values: not null } inExpression)
			{
				List<object> list = new List<object>();
				bool flag = false;
				RelationalTypeMapping typeMapping = null;
				SqlExpression values = inExpression.Values;
				if (!(values is SqlConstantExpression sqlConstantExpression))
				{
					if (values is SqlParameterExpression sqlParameterExpression)
					{
						typeMapping = sqlParameterExpression.TypeMapping;
						foreach (object item in (IEnumerable)_parametersValues[sqlParameterExpression.Name])
						{
							if (item == null)
							{
								flag = true;
							}
							else
							{
								list.Add(item);
							}
						}
					}
				}
				else
				{
					typeMapping = sqlConstantExpression.TypeMapping;
					foreach (object item2 in (IEnumerable)sqlConstantExpression.Value)
					{
						if (item2 == null)
						{
							flag = true;
						}
						else
						{
							list.Add(item2);
						}
					}
				}
				InExpression inExpression2 = ((list.Count > 0) ? _sqlExpressionFactory.In((SqlExpression)Visit(inExpression.Item), _sqlExpressionFactory.Constant(list, typeMapping), inExpression.IsNegated) : null);
				SqlUnaryExpression sqlUnaryExpression = ((!flag) ? null : (inExpression.IsNegated ? _sqlExpressionFactory.IsNotNull(inExpression.Item) : _sqlExpressionFactory.IsNull(inExpression.Item)));
				if (inExpression2 != null && sqlUnaryExpression != null)
				{
					if (!inExpression.IsNegated)
					{
						return _sqlExpressionFactory.OrElse(inExpression2, sqlUnaryExpression);
					}
					return _sqlExpressionFactory.AndAlso(inExpression2, sqlUnaryExpression);
				}
				if (inExpression2 == null && sqlUnaryExpression == null)
				{
					return _sqlExpressionFactory.Equal(_sqlExpressionFactory.Constant(true), _sqlExpressionFactory.Constant(inExpression.IsNegated));
				}
				return (Expression)(((object)inExpression2) ?? ((object)sqlUnaryExpression));
			}
			return base.Visit(expression);
		}
	}

	private class FromSqlParameterApplyingExpressionVisitor : ExpressionVisitor
	{
		private readonly IDictionary<FromSqlExpression, Expression> _visitedFromSqlExpressions = new Dictionary<FromSqlExpression, Expression>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		private readonly ParameterNameGenerator _parameterNameGenerator;

		private readonly IReadOnlyDictionary<string, object> _parametersValues;

		public FromSqlParameterApplyingExpressionVisitor(ISqlExpressionFactory sqlExpressionFactory, ParameterNameGenerator parameterNameGenerator, IReadOnlyDictionary<string, object> parametersValues)
		{
			_sqlExpressionFactory = sqlExpressionFactory;
			_parameterNameGenerator = parameterNameGenerator;
			_parametersValues = parametersValues;
		}

		public override Expression Visit(Expression expression)
		{
			if (expression is FromSqlExpression fromSqlExpression)
			{
				if (!_visitedFromSqlExpressions.TryGetValue(fromSqlExpression, out var value))
				{
					Expression arguments = fromSqlExpression.Arguments;
					if (!(arguments is ParameterExpression parameterExpression))
					{
						if (arguments is ConstantExpression constantExpression)
						{
							object[] array = (object[])constantExpression.Value;
							object[] array2 = new object[array.Length];
							for (int i = 0; i < array.Length; i++)
							{
								object obj = array[i];
								if (obj is DbParameter dbParameter)
								{
									string text = _parameterNameGenerator.GenerateNext();
									if (string.IsNullOrEmpty(dbParameter.ParameterName))
									{
										dbParameter.ParameterName = text;
									}
									else
									{
										text = dbParameter.ParameterName;
									}
									array2[i] = new RawRelationalParameter(text, dbParameter);
								}
								else
								{
									array2[i] = _sqlExpressionFactory.Constant(obj, _sqlExpressionFactory.GetTypeMappingForValue(obj));
								}
							}
							value = new FromSqlExpression(fromSqlExpression.Sql, Expression.Constant(array2, typeof(object[])), fromSqlExpression.Alias);
							_visitedFromSqlExpressions[fromSqlExpression] = value;
						}
					}
					else
					{
						object[] array3 = (object[])_parametersValues[parameterExpression.Name];
						List<IRelationalParameter> list = new List<IRelationalParameter>(array3.Length);
						for (int j = 0; j < array3.Length; j++)
						{
							string text2 = _parameterNameGenerator.GenerateNext();
							if (array3[j] is DbParameter dbParameter2)
							{
								if (string.IsNullOrEmpty(dbParameter2.ParameterName))
								{
									dbParameter2.ParameterName = text2;
								}
								else
								{
									text2 = dbParameter2.ParameterName;
								}
								list.Add(new RawRelationalParameter(text2, dbParameter2));
							}
							else
							{
								list.Add(new TypeMappedRelationalParameter(text2, text2, _sqlExpressionFactory.GetTypeMappingForValue(array3[j]), array3[j]?.GetType().IsNullableType()));
							}
						}
						value = new FromSqlExpression(fromSqlExpression.Sql, Expression.Constant(new CompositeRelationalParameter(parameterExpression.Name, list)), fromSqlExpression.Alias);
						_visitedFromSqlExpressions[fromSqlExpression] = value;
					}
				}
				return value;
			}
			return base.Visit(expression);
		}
	}

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private readonly IParameterNameGeneratorFactory _parameterNameGeneratorFactory;

	private readonly bool _useRelationalNulls;

	public ParameterValueBasedSelectExpressionOptimizer(ISqlExpressionFactory sqlExpressionFactory, IParameterNameGeneratorFactory parameterNameGeneratorFactory, bool useRelationalNulls)
	{
		_sqlExpressionFactory = sqlExpressionFactory;
		_parameterNameGeneratorFactory = parameterNameGeneratorFactory;
		_useRelationalNulls = useRelationalNulls;
	}

	public virtual (SelectExpression selectExpression, bool canCache) Optimize(SelectExpression selectExpression, IReadOnlyDictionary<string, object> parametersValues)
	{
		bool item = true;
		Expression expression = new InExpressionValuesExpandingExpressionVisitor(_sqlExpressionFactory, parametersValues).Visit(selectExpression);
		if (selectExpression != expression)
		{
			item = false;
		}
		Expression expression2 = new ParameterNullabilityBasedSqlExpressionOptimizingExpressionVisitor(_sqlExpressionFactory, _useRelationalNulls, parametersValues).Visit(expression);
		Expression expression3 = new FromSqlParameterApplyingExpressionVisitor(_sqlExpressionFactory, _parameterNameGeneratorFactory.Create(), parametersValues).Visit(expression2);
		if (expression2 != expression3)
		{
			item = false;
		}
		return (selectExpression: (SelectExpression)expression3, canCache: item);
	}
}
