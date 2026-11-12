using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public class RelationalProjectionBindingExpressionVisitor : ExpressionVisitor
{
	private readonly RelationalQueryableMethodTranslatingExpressionVisitor _queryableMethodTranslatingExpressionVisitor;

	private readonly RelationalSqlTranslatingExpressionVisitor _sqlTranslator;

	private SelectExpression _selectExpression;

	private bool _clientEval;

	private readonly IDictionary<ProjectionMember, Expression> _projectionMapping = new Dictionary<ProjectionMember, Expression>();

	private readonly Stack<ProjectionMember> _projectionMembers = new Stack<ProjectionMember>();

	private static readonly MethodInfo _getParameterValueMethodInfo = typeof(RelationalProjectionBindingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("GetParameterValue");

	public RelationalProjectionBindingExpressionVisitor(RelationalQueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor, RelationalSqlTranslatingExpressionVisitor sqlTranslatingExpressionVisitor)
	{
		_queryableMethodTranslatingExpressionVisitor = queryableMethodTranslatingExpressionVisitor;
		_sqlTranslator = sqlTranslatingExpressionVisitor;
	}

	public virtual Expression Translate(SelectExpression selectExpression, Expression expression)
	{
		_selectExpression = selectExpression;
		_clientEval = false;
		_projectionMembers.Push(new ProjectionMember());
		Expression node = _queryableMethodTranslatingExpressionVisitor.ExpandWeakEntities(_selectExpression, expression);
		Expression expression2 = Visit(node);
		if (expression2 == null)
		{
			_clientEval = true;
			node = _queryableMethodTranslatingExpressionVisitor.ExpandWeakEntities(_selectExpression, expression);
			expression2 = Visit(node);
			_projectionMapping.Clear();
		}
		_selectExpression.ReplaceProjectionMapping(_projectionMapping);
		_selectExpression = null;
		_projectionMembers.Clear();
		_projectionMapping.Clear();
		return expression2;
	}

	public override Expression Visit(Expression expression)
	{
		if (expression == null)
		{
			return null;
		}
		if (!(expression is NewExpression) && !(expression is MemberInitExpression) && !(expression is EntityShaperExpression) && !(expression is IncludeExpression))
		{
			if (expression is ParameterExpression parameterExpression && parameterExpression.Type.IsGenericType && parameterExpression.Type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				return parameterExpression;
			}
			if (_clientEval)
			{
				if (!(expression is ConstantExpression))
				{
					if (!(expression is ParameterExpression parameterExpression2))
					{
						if (!(expression is MaterializeCollectionNavigationExpression materializeCollectionNavigationExpression))
						{
							if (expression is MethodCallExpression methodCallExpression)
							{
								if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.DeclaringType == typeof(Enumerable) && methodCallExpression.Method.Name == "ToList")
								{
									Type elementType = methodCallExpression.Method.GetGenericArguments()[0];
									ShapedQueryExpression shapedQueryExpression = _queryableMethodTranslatingExpressionVisitor.TranslateSubquery(methodCallExpression.Arguments[0]);
									return _selectExpression.AddCollectionProjection(shapedQueryExpression, null, elementType);
								}
								ShapedQueryExpression shapedQueryExpression2 = _queryableMethodTranslatingExpressionVisitor.TranslateSubquery(methodCallExpression);
								if (shapedQueryExpression2 != null)
								{
									if (shapedQueryExpression2.ResultCardinality == ResultCardinality.Enumerable)
									{
										return _selectExpression.AddCollectionProjection(shapedQueryExpression2, null, shapedQueryExpression2.ShaperExpression.Type);
									}
									if (!(shapedQueryExpression2.ShaperExpression is ProjectionBindingExpression) && !IsAggregateResultWithCustomShaper(methodCallExpression.Method))
									{
										return _selectExpression.AddSingleProjection(shapedQueryExpression2);
									}
								}
							}
							SqlExpression sqlExpression = _sqlTranslator.Translate(expression);
							if (sqlExpression != null)
							{
								return new ProjectionBindingExpression(_selectExpression, _selectExpression.AddToProjection(sqlExpression), expression.Type);
							}
							return base.Visit(expression);
						}
						return _selectExpression.AddCollectionProjection(_queryableMethodTranslatingExpressionVisitor.TranslateSubquery(materializeCollectionNavigationExpression.Subquery), materializeCollectionNavigationExpression.Navigation, null);
					}
					return Expression.Call(_getParameterValueMethodInfo.MakeGenericMethod(parameterExpression2.Type), QueryCompilationContext.QueryContextParameter, Expression.Constant(parameterExpression2.Name));
				}
				return expression;
			}
			SqlExpression sqlExpression2 = _sqlTranslator.Translate(expression);
			if (sqlExpression2 == null)
			{
				return null;
			}
			_projectionMapping[_projectionMembers.Peek()] = sqlExpression2;
			return new ProjectionBindingExpression(_selectExpression, _projectionMembers.Peek(), expression.Type);
		}
		return base.Visit(expression);
		static bool IsAggregateResultWithCustomShaper(MethodInfo method)
		{
			if (method.IsGenericMethod)
			{
				method = method.GetGenericMethodDefinition();
			}
			if (!QueryableMethods.IsAverageWithoutSelector(method) && !QueryableMethods.IsAverageWithSelector(method) && !(method == QueryableMethods.MaxWithoutSelector) && !(method == QueryableMethods.MaxWithSelector) && !(method == QueryableMethods.MinWithoutSelector) && !(method == QueryableMethods.MinWithSelector) && !QueryableMethods.IsSumWithoutSelector(method))
			{
				return QueryableMethods.IsSumWithSelector(method);
			}
			return true;
		}
	}

	private static T GetParameterValue<T>(QueryContext queryContext, string parameterName)
	{
		return (T)queryContext.ParameterValues[parameterName];
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (extensionExpression is EntityShaperExpression entityShaperExpression)
		{
			EntityProjectionExpression entityProjectionExpression;
			if (entityShaperExpression.ValueBufferExpression is ProjectionBindingExpression projectionBindingExpression)
			{
				VerifySelectExpression(projectionBindingExpression);
				entityProjectionExpression = (EntityProjectionExpression)_selectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember);
			}
			else
			{
				entityProjectionExpression = (EntityProjectionExpression)entityShaperExpression.ValueBufferExpression;
			}
			if (_clientEval)
			{
				return entityShaperExpression.Update(new ProjectionBindingExpression(_selectExpression, _selectExpression.AddToProjection(entityProjectionExpression)));
			}
			_projectionMapping[_projectionMembers.Peek()] = entityProjectionExpression;
			return entityShaperExpression.Update(new ProjectionBindingExpression(_selectExpression, _projectionMembers.Peek(), typeof(ValueBuffer)));
		}
		if (extensionExpression is IncludeExpression node)
		{
			if (!_clientEval)
			{
				return null;
			}
			return base.VisitExtension(node);
		}
		throw new InvalidOperationException(CoreStrings.QueryFailed(extensionExpression.Print(), GetType().Name));
	}

	protected override Expression VisitNew(NewExpression newExpression)
	{
		if (newExpression.Constructor == null)
		{
			return newExpression;
		}
		if (newExpression.Arguments.Count == 0)
		{
			return newExpression;
		}
		if (!_clientEval && newExpression.Members == null)
		{
			return null;
		}
		Expression[] array = new Expression[newExpression.Arguments.Count];
		for (int i = 0; i < array.Length; i++)
		{
			if (_clientEval)
			{
				array[i] = Visit(newExpression.Arguments[i]);
				continue;
			}
			ProjectionMember item = _projectionMembers.Peek().Append(newExpression.Members[i]);
			_projectionMembers.Push(item);
			array[i] = Visit(newExpression.Arguments[i]);
			if (array[i] == null)
			{
				return null;
			}
			_projectionMembers.Pop();
		}
		return newExpression.Update(array);
	}

	protected override Expression VisitMemberInit(MemberInitExpression memberInitExpression)
	{
		Expression expression = Visit(memberInitExpression.NewExpression);
		if (expression == null)
		{
			return null;
		}
		MemberBinding[] array = new MemberBinding[memberInitExpression.Bindings.Count];
		for (int i = 0; i < array.Length; i++)
		{
			if (memberInitExpression.Bindings[i].BindingType != 0)
			{
				return null;
			}
			array[i] = VisitMemberBinding(memberInitExpression.Bindings[i]);
			if (array[i] == null)
			{
				return null;
			}
		}
		return memberInitExpression.Update((NewExpression)expression, array);
	}

	protected override MemberAssignment VisitMemberAssignment(MemberAssignment memberAssignment)
	{
		if (_clientEval)
		{
			return memberAssignment.Update(Visit(memberAssignment.Expression));
		}
		ProjectionMember item = _projectionMembers.Peek().Append(memberAssignment.Member);
		_projectionMembers.Push(item);
		Expression expression = Visit(memberAssignment.Expression);
		if (expression == null)
		{
			return null;
		}
		_projectionMembers.Pop();
		return memberAssignment.Update(expression);
	}

	private void VerifySelectExpression(ProjectionBindingExpression projectionBindingExpression)
	{
		if (projectionBindingExpression.QueryExpression != _selectExpression)
		{
			throw new InvalidOperationException(CoreStrings.QueryFailed(projectionBindingExpression.Print(), GetType().Name));
		}
	}
}
