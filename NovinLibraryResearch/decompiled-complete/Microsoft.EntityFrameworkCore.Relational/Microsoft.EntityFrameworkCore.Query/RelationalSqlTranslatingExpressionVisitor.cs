using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalSqlTranslatingExpressionVisitor : ExpressionVisitor
{
	private class SqlTypeMappingVerifyingExpressionVisitor : ExpressionVisitor
	{
		protected override Expression VisitExtension(Expression node)
		{
			if (node is SqlExpression sqlExpression && !(node is SqlFragmentExpression) && sqlExpression.TypeMapping == null)
			{
				throw new InvalidOperationException("Null TypeMapping in Sql Tree");
			}
			return base.VisitExtension(node);
		}
	}

	private readonly IModel _model;

	private readonly QueryableMethodTranslatingExpressionVisitor _queryableMethodTranslatingExpressionVisitor;

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private readonly SqlTypeMappingVerifyingExpressionVisitor _sqlTypeMappingVerifyingExpressionVisitor;

	protected virtual RelationalSqlTranslatingExpressionVisitorDependencies Dependencies { get; }

	public RelationalSqlTranslatingExpressionVisitor(RelationalSqlTranslatingExpressionVisitorDependencies dependencies, IModel model, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
	{
		Dependencies = dependencies;
		_model = model;
		_queryableMethodTranslatingExpressionVisitor = queryableMethodTranslatingExpressionVisitor;
		_sqlExpressionFactory = dependencies.SqlExpressionFactory;
		_sqlTypeMappingVerifyingExpressionVisitor = new SqlTypeMappingVerifyingExpressionVisitor();
	}

	public virtual SqlExpression Translate(Expression expression)
	{
		SqlExpression sqlExpression = Visit(expression) as SqlExpression;
		if (sqlExpression != null)
		{
			if (sqlExpression is SqlUnaryExpression { OperatorType: ExpressionType.Convert } sqlUnaryExpression && sqlUnaryExpression.Type == typeof(object))
			{
				sqlExpression = sqlUnaryExpression.Operand;
			}
			sqlExpression = _sqlExpressionFactory.ApplyDefaultTypeMapping(sqlExpression);
			if ((sqlExpression is SqlConstantExpression || sqlExpression is SqlParameterExpression) && sqlExpression.TypeMapping == null)
			{
				return null;
			}
			_sqlTypeMappingVerifyingExpressionVisitor.Visit(sqlExpression);
			return sqlExpression;
		}
		return null;
	}

	public virtual SqlExpression TranslateAverage(Expression expression)
	{
		SqlExpression sqlExpression = expression as SqlExpression;
		if (sqlExpression == null)
		{
			sqlExpression = Translate(expression);
		}
		if (sqlExpression == null)
		{
			throw new InvalidOperationException(CoreStrings.TranslationFailed(expression.Print()));
		}
		Type type = sqlExpression.Type.UnwrapNullableType();
		if (type == typeof(int) || type == typeof(long))
		{
			sqlExpression = _sqlExpressionFactory.ApplyDefaultTypeMapping(_sqlExpressionFactory.Convert(sqlExpression, typeof(double)));
		}
		if (!(type == typeof(float)))
		{
			return _sqlExpressionFactory.Function("AVG", new SqlExpression[1] { sqlExpression }, sqlExpression.Type, sqlExpression.TypeMapping);
		}
		return _sqlExpressionFactory.Convert(_sqlExpressionFactory.Function("AVG", new SqlExpression[1] { sqlExpression }, typeof(double)), sqlExpression.Type, sqlExpression.TypeMapping);
	}

	public virtual SqlExpression TranslateCount(Expression expression = null)
	{
		if (expression != null)
		{
			return null;
		}
		return _sqlExpressionFactory.ApplyDefaultTypeMapping(_sqlExpressionFactory.Function("COUNT", new SqlFragmentExpression[1] { _sqlExpressionFactory.Fragment("*") }, typeof(int)));
	}

	public virtual SqlExpression TranslateLongCount(Expression expression = null)
	{
		if (expression != null)
		{
			return null;
		}
		return _sqlExpressionFactory.ApplyDefaultTypeMapping(_sqlExpressionFactory.Function("COUNT", new SqlFragmentExpression[1] { _sqlExpressionFactory.Fragment("*") }, typeof(long)));
	}

	public virtual SqlExpression TranslateMax(Expression expression)
	{
		SqlExpression sqlExpression = expression as SqlExpression;
		if (sqlExpression == null)
		{
			sqlExpression = Translate(expression);
		}
		if (sqlExpression == null)
		{
			return null;
		}
		return _sqlExpressionFactory.Function("MAX", new SqlExpression[1] { sqlExpression }, sqlExpression.Type, sqlExpression.TypeMapping);
	}

	public virtual SqlExpression TranslateMin(Expression expression)
	{
		SqlExpression sqlExpression = expression as SqlExpression;
		if (sqlExpression == null)
		{
			sqlExpression = Translate(expression);
		}
		if (sqlExpression == null)
		{
			return null;
		}
		return _sqlExpressionFactory.Function("MIN", new SqlExpression[1] { sqlExpression }, sqlExpression.Type, sqlExpression.TypeMapping);
	}

	public virtual SqlExpression TranslateSum(Expression expression)
	{
		SqlExpression sqlExpression = expression as SqlExpression;
		if (sqlExpression == null)
		{
			sqlExpression = Translate(expression);
		}
		if (sqlExpression == null)
		{
			throw new InvalidOperationException(CoreStrings.TranslationFailed(expression.Print()));
		}
		Type type = sqlExpression.Type.UnwrapNullableType();
		if (!(type == typeof(float)))
		{
			return _sqlExpressionFactory.Function("SUM", new SqlExpression[1] { sqlExpression }, type, sqlExpression.TypeMapping);
		}
		return _sqlExpressionFactory.Convert(_sqlExpressionFactory.Function("SUM", new SqlExpression[1] { sqlExpression }, typeof(double)), type, sqlExpression.TypeMapping);
	}

	protected override Expression VisitMember(MemberExpression memberExpression)
	{
		if (!TryBindMember(memberExpression.Expression, MemberIdentity.Create(memberExpression.Member), out var expression))
		{
			if (!TranslationFailed(memberExpression.Expression, base.Visit(memberExpression.Expression), out var castTranslation))
			{
				return Dependencies.MemberTranslatorProvider.Translate(castTranslation, memberExpression.Member, memberExpression.Type);
			}
			return null;
		}
		return expression;
	}

	private bool TryBindMember(Expression source, MemberIdentity member, out Expression expression)
	{
		source = source.UnwrapTypeConversion(out var convertedType);
		expression = null;
		if (source is EntityShaperExpression { EntityType: var entityType } entityShaperExpression)
		{
			if (convertedType != null)
			{
				entityType = entityType.GetRootType().GetDerivedTypesInclusive().FirstOrDefault((IEntityType et) => et.ClrType == convertedType);
				if (entityType == null)
				{
					return false;
				}
			}
			IProperty property = ((member.MemberInfo != null) ? entityType.FindProperty(member.MemberInfo) : entityType.FindProperty(member.Name));
			if (property != null && Visit(entityShaperExpression.ValueBufferExpression) is EntityProjectionExpression entityProjectionExpression && (entityProjectionExpression.EntityType.IsAssignableFrom(property.DeclaringEntityType) || property.DeclaringEntityType.IsAssignableFrom(entityProjectionExpression.EntityType)))
			{
				expression = entityProjectionExpression.BindProperty(property);
				return true;
			}
		}
		return false;
	}

	protected override Expression VisitTypeBinary(TypeBinaryExpression typeBinaryExpression)
	{
		if (typeBinaryExpression.NodeType == ExpressionType.TypeIs && Visit(typeBinaryExpression.Expression) is EntityProjectionExpression { EntityType: var entityType } entityProjectionExpression)
		{
			if (entityType.GetAllBaseTypesInclusive().Any((IEntityType et) => et.ClrType == typeBinaryExpression.TypeOperand))
			{
				return _sqlExpressionFactory.Constant(true);
			}
			IEntityType entityType2 = entityType.GetDerivedTypes().SingleOrDefault((IEntityType et) => et.ClrType == typeBinaryExpression.TypeOperand);
			if (entityType2 != null)
			{
				List<IEntityType> list = entityType2.GetConcreteDerivedTypesInclusive().ToList();
				ColumnExpression columnExpression = entityProjectionExpression.BindProperty(entityType.GetDiscriminatorProperty());
				if (list.Count != 1)
				{
					return _sqlExpressionFactory.In(columnExpression, _sqlExpressionFactory.Constant(list.Select((IEntityType et) => et.GetDiscriminatorValue()).ToList()), negated: false);
				}
				return _sqlExpressionFactory.Equal(columnExpression, _sqlExpressionFactory.Constant(list[0].GetDiscriminatorValue()));
			}
			return _sqlExpressionFactory.Constant(false);
		}
		return null;
	}

	private Expression GetSelector(MethodCallExpression methodCallExpression, GroupByShaperExpression groupByShaperExpression)
	{
		if (methodCallExpression.Arguments.Count == 1)
		{
			return groupByShaperExpression.ElementSelector;
		}
		if (methodCallExpression.Arguments.Count == 2)
		{
			LambdaExpression lambdaExpression = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();
			return ReplacingExpressionVisitor.Replace(lambdaExpression.Parameters[0], groupByShaperExpression.ElementSelector, lambdaExpression.Body);
		}
		throw new InvalidOperationException(CoreStrings.TranslationFailed(methodCallExpression.Print()));
	}

	private Expression GetPredicate(MethodCallExpression methodCallExpression, GroupByShaperExpression groupByShaperExpression)
	{
		if (methodCallExpression.Arguments.Count == 1)
		{
			return null;
		}
		if (methodCallExpression.Arguments.Count == 2)
		{
			LambdaExpression lambdaExpression = methodCallExpression.Arguments[1].UnwrapLambdaFromQuote();
			return ReplacingExpressionVisitor.Replace(lambdaExpression.Parameters[0], groupByShaperExpression.ElementSelector, lambdaExpression.Body);
		}
		throw new InvalidOperationException(CoreStrings.TranslationFailed(methodCallExpression.Print()));
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.TryGetEFPropertyArguments(out var entityExpression, out var propertyName))
		{
			if (TryBindMember(entityExpression, MemberIdentity.Create(propertyName), out var expression))
			{
				return expression;
			}
			throw new InvalidOperationException("EF.Property called with wrong property name.");
		}
		if (methodCallExpression.Object == null && methodCallExpression.Method.DeclaringType == typeof(Enumerable) && methodCallExpression.Arguments.Count > 0 && methodCallExpression.Arguments[0] is GroupByShaperExpression groupByShaperExpression)
		{
			return (Expression)((methodCallExpression.Method.Name switch
			{
				"Average" => TranslateAverage(GetSelector(methodCallExpression, groupByShaperExpression)), 
				"Count" => TranslateCount(GetPredicate(methodCallExpression, groupByShaperExpression)), 
				"LongCount" => TranslateLongCount(GetPredicate(methodCallExpression, groupByShaperExpression)), 
				"Max" => TranslateMax(GetSelector(methodCallExpression, groupByShaperExpression)), 
				"Min" => TranslateMin(GetSelector(methodCallExpression, groupByShaperExpression)), 
				"Sum" => TranslateSum(GetSelector(methodCallExpression, groupByShaperExpression)), 
				_ => null, 
			}) ?? throw new InvalidOperationException(CoreStrings.TranslationFailed(methodCallExpression.Print())));
		}
		ShapedQueryExpression shapedQueryExpression = _queryableMethodTranslatingExpressionVisitor.TranslateSubquery(methodCallExpression);
		if (shapedQueryExpression != null)
		{
			if (shapedQueryExpression.ResultCardinality == ResultCardinality.Enumerable)
			{
				return null;
			}
			SelectExpression selectExpression = (SelectExpression)shapedQueryExpression.QueryExpression;
			selectExpression.ApplyProjection();
			if (!(shapedQueryExpression.ShaperExpression is ProjectionBindingExpression) && !IsAggregateResultWithCustomShaper(methodCallExpression.Method))
			{
				return null;
			}
			if (selectExpression.Tables.Count == 0 && methodCallExpression.Method.IsGenericMethod)
			{
				MethodInfo genericMethodDefinition = methodCallExpression.Method.GetGenericMethodDefinition();
				if ((object)genericMethodDefinition != null && (genericMethodDefinition == QueryableMethods.AnyWithoutPredicate || genericMethodDefinition == QueryableMethods.AnyWithPredicate || genericMethodDefinition == QueryableMethods.All || genericMethodDefinition == QueryableMethods.Contains))
				{
					return selectExpression.Projection[0].Expression;
				}
			}
			return new ScalarSubqueryExpression(selectExpression);
		}
		if (TranslationFailed(methodCallExpression.Object, Visit(methodCallExpression.Object), out var castTranslation))
		{
			return null;
		}
		SqlExpression[] array = new SqlExpression[methodCallExpression.Arguments.Count];
		for (int i = 0; i < array.Length; i++)
		{
			Expression expression2 = methodCallExpression.Arguments[i];
			if (TranslationFailed(expression2, Visit(expression2), out var castTranslation2))
			{
				return null;
			}
			array[i] = castTranslation2;
		}
		return Dependencies.MethodCallTranslatorProvider.Translate(_model, castTranslation, methodCallExpression.Method, array);
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

	private static Expression TryRemoveImplicitConvert(Expression expression)
	{
		if (expression is UnaryExpression unaryExpression && (unaryExpression.NodeType == ExpressionType.Convert || unaryExpression.NodeType == ExpressionType.ConvertChecked))
		{
			Type type = unaryExpression.Operand.Type.UnwrapNullableType();
			if (type.IsEnum)
			{
				type = Enum.GetUnderlyingType(type);
			}
			Type type2 = unaryExpression.Type.UnwrapNullableType();
			if (type == type2 || (type2 == typeof(int) && (type == typeof(byte) || type == typeof(sbyte) || type == typeof(char) || type == typeof(short) || type == typeof(ushort))))
			{
				return TryRemoveImplicitConvert(unaryExpression.Operand);
			}
		}
		return expression;
	}

	private Expression ConvertAnonymousObjectEqualityComparison(BinaryExpression binaryExpression)
	{
		ReadOnlyCollection<Expression> expressions = ((NewArrayExpression)((NewExpression)binaryExpression.Left).Arguments[0]).Expressions;
		ReadOnlyCollection<Expression> expressions2 = ((NewArrayExpression)((NewExpression)binaryExpression.Right).Arguments[0]).Expressions;
		return expressions.Zip(expressions2, delegate(Expression l, Expression r)
		{
			l = RemoveObjectConvert(l);
			r = RemoveObjectConvert(r);
			if (l.Type.IsNullableType())
			{
				r = (r.Type.IsNullableType() ? r : Expression.Convert(r, l.Type));
			}
			else if (r.Type.IsNullableType())
			{
				l = (l.Type.IsNullableType() ? l : Expression.Convert(l, r.Type));
			}
			return Expression.Equal(l, r);
		}).Aggregate((BinaryExpression a, BinaryExpression b) => Expression.AndAlso(a, b));
		static Expression RemoveObjectConvert(Expression expression)
		{
			if (!(expression is UnaryExpression unaryExpression) || !(expression.Type == typeof(object)) || expression.NodeType != ExpressionType.Convert)
			{
				return expression;
			}
			return unaryExpression.Operand;
		}
	}

	protected override Expression VisitBinary(BinaryExpression binaryExpression)
	{
		if (binaryExpression.Left.Type == typeof(AnonymousObject) && binaryExpression.NodeType == ExpressionType.Equal)
		{
			return Visit(ConvertAnonymousObjectEqualityComparison(binaryExpression));
		}
		Expression node = TryRemoveImplicitConvert(binaryExpression.Left);
		Expression node2 = TryRemoveImplicitConvert(binaryExpression.Right);
		if (!TranslationFailed(binaryExpression.Left, Visit(node), out var castTranslation) && !TranslationFailed(binaryExpression.Right, Visit(node2), out var castTranslation2))
		{
			return _sqlExpressionFactory.MakeBinary(binaryExpression.NodeType, castTranslation, castTranslation2, null);
		}
		return null;
	}

	private SqlConstantExpression GetConstantOrNull(Expression expression)
	{
		if (CanEvaluate(expression))
		{
			return new SqlConstantExpression(Expression.Constant(Expression.Lambda<Func<object>>(Expression.Convert(expression, typeof(object)), Array.Empty<ParameterExpression>()).Compile()(), expression.Type), null);
		}
		return null;
	}

	private static bool CanEvaluate(Expression expression)
	{
		if (!(expression is ConstantExpression))
		{
			if (!(expression is NewExpression newExpression))
			{
				if (expression is MemberInitExpression memberInitExpression)
				{
					if (CanEvaluate(memberInitExpression.NewExpression))
					{
						return memberInitExpression.Bindings.All((MemberBinding mb) => mb is MemberAssignment memberAssignment && CanEvaluate(memberAssignment.Expression));
					}
					return false;
				}
				return false;
			}
			return newExpression.Arguments.All((Expression e) => CanEvaluate(e));
		}
		return true;
	}

	protected override Expression VisitNew(NewExpression node)
	{
		return GetConstantOrNull(node);
	}

	protected override Expression VisitMemberInit(MemberInitExpression node)
	{
		return GetConstantOrNull(node);
	}

	protected override Expression VisitNewArray(NewArrayExpression node)
	{
		return null;
	}

	protected override Expression VisitListInit(ListInitExpression node)
	{
		return null;
	}

	protected override Expression VisitInvocation(InvocationExpression node)
	{
		return null;
	}

	protected override Expression VisitLambda<T>(Expression<T> node)
	{
		return null;
	}

	protected override Expression VisitConstant(ConstantExpression constantExpression)
	{
		return new SqlConstantExpression(constantExpression, null);
	}

	protected override Expression VisitParameter(ParameterExpression parameterExpression)
	{
		return new SqlParameterExpression(parameterExpression, null);
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (!(extensionExpression is EntityProjectionExpression) && !(extensionExpression is SqlExpression))
		{
			if (!(extensionExpression is EntityShaperExpression entityShaperExpression))
			{
				if (extensionExpression is ProjectionBindingExpression projectionBindingExpression)
				{
					if (projectionBindingExpression.ProjectionMember == null)
					{
						return null;
					}
					return ((SelectExpression)projectionBindingExpression.QueryExpression).GetMappedProjection(projectionBindingExpression.ProjectionMember);
				}
				return null;
			}
			return Visit(entityShaperExpression.ValueBufferExpression);
		}
		return extensionExpression;
	}

	protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
	{
		Expression translation = Visit(conditionalExpression.Test);
		Expression translation2 = Visit(conditionalExpression.IfTrue);
		Expression translation3 = Visit(conditionalExpression.IfFalse);
		if (!TranslationFailed(conditionalExpression.Test, translation, out var castTranslation) && !TranslationFailed(conditionalExpression.IfTrue, translation2, out var castTranslation2) && !TranslationFailed(conditionalExpression.IfFalse, translation3, out var castTranslation3))
		{
			return _sqlExpressionFactory.Case(new CaseWhenClause[1]
			{
				new CaseWhenClause(castTranslation, castTranslation2)
			}, castTranslation3);
		}
		return null;
	}

	protected override Expression VisitUnary(UnaryExpression unaryExpression)
	{
		Expression operand = Visit(unaryExpression.Operand);
		if (TranslationFailed(unaryExpression.Operand, operand, out var castTranslation))
		{
			return null;
		}
		switch (unaryExpression.NodeType)
		{
		case ExpressionType.Not:
			return _sqlExpressionFactory.Not(castTranslation);
		case ExpressionType.Negate:
			return _sqlExpressionFactory.Negate(castTranslation);
		case ExpressionType.Convert:
			if ((operand.Type.IsInterface && unaryExpression.Type.GetInterfaces().Any((Type e) => e == operand.Type)) || unaryExpression.Type.UnwrapNullableType() == operand.Type.UnwrapNullableType() || unaryExpression.Type.UnwrapNullableType() == typeof(Enum))
			{
				return castTranslation;
			}
			if (unaryExpression.Type == typeof(object) || _sqlExpressionFactory.FindMapping(unaryExpression.Type) != null)
			{
				castTranslation = _sqlExpressionFactory.ApplyDefaultTypeMapping(castTranslation);
				return _sqlExpressionFactory.Convert(castTranslation, unaryExpression.Type);
			}
			break;
		}
		return null;
	}

	[DebuggerStepThrough]
	private bool TranslationFailed(Expression original, Expression translation, out SqlExpression castTranslation)
	{
		if (original != null && !(translation is SqlExpression))
		{
			castTranslation = null;
			return true;
		}
		castTranslation = translation as SqlExpression;
		return false;
	}
}
