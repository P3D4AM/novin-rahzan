using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public class RelationalQueryableMethodTranslatingExpressionVisitor : QueryableMethodTranslatingExpressionVisitor
{
	private class CorrelationFindingExpressionVisitor : ExpressionVisitor
	{
		private ParameterExpression _outerParameter;

		private bool _correlated;

		private bool _defaultIfEmpty;

		public (LambdaExpression, bool, bool) IsCorrelated(LambdaExpression lambdaExpression)
		{
			_correlated = false;
			_defaultIfEmpty = false;
			_outerParameter = lambdaExpression.Parameters[0];
			return (Expression.Lambda(Visit(lambdaExpression.Body), _outerParameter), _correlated, _defaultIfEmpty);
		}

		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			if (parameterExpression == _outerParameter)
			{
				_correlated = true;
			}
			return base.VisitParameter(parameterExpression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.Method.IsGenericMethod && methodCallExpression.Method.GetGenericMethodDefinition() == QueryableMethods.DefaultIfEmptyWithoutArgument)
			{
				_defaultIfEmpty = true;
				return Visit(methodCallExpression.Arguments[0]);
			}
			return base.VisitMethodCall(methodCallExpression);
		}
	}

	private class WeakEntityExpandingExpressionVisitor : ExpressionVisitor
	{
		private SelectExpression _selectExpression;

		private readonly RelationalSqlTranslatingExpressionVisitor _sqlTranslator;

		private readonly ISqlExpressionFactory _sqlExpressionFactory;

		public WeakEntityExpandingExpressionVisitor(RelationalSqlTranslatingExpressionVisitor sqlTranslator, ISqlExpressionFactory sqlExpressionFactory)
		{
			_sqlTranslator = sqlTranslator;
			_sqlExpressionFactory = sqlExpressionFactory;
		}

		public virtual Expression Expand(SelectExpression selectExpression, Expression lambdaBody)
		{
			_selectExpression = selectExpression;
			return Visit(lambdaBody);
		}

		protected override Expression VisitMember(MemberExpression memberExpression)
		{
			Expression expression = Visit(memberExpression.Expression);
			return TryExpand(expression, MemberIdentity.Create(memberExpression.Member)) ?? memberExpression.Update(expression);
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (methodCallExpression.TryGetEFPropertyArguments(out var entityExpression, out var propertyName))
			{
				entityExpression = Visit(entityExpression);
				return TryExpand(entityExpression, MemberIdentity.Create(propertyName)) ?? methodCallExpression.Update(null, new Expression[2]
				{
					entityExpression,
					methodCallExpression.Arguments[1]
				});
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is EntityShaperExpression))
			{
				return base.VisitExtension(extensionExpression);
			}
			return extensionExpression;
		}

		private Expression TryExpand(Expression source, MemberIdentity member)
		{
			source = source.UnwrapTypeConversion(out var convertedType);
			if (!(source is EntityShaperExpression { EntityType: var entityType } entityShaperExpression))
			{
				return null;
			}
			if (convertedType != null)
			{
				entityType = entityType.GetRootType().GetDerivedTypesInclusive().FirstOrDefault((IEntityType et) => et.ClrType == convertedType);
				if (entityType == null)
				{
					return null;
				}
			}
			INavigation navigation = ((member.MemberInfo != null) ? entityType.FindNavigation(member.MemberInfo) : entityType.FindNavigation(member.Name));
			if (navigation == null)
			{
				return null;
			}
			IEntityType targetType = navigation.GetTargetType();
			if (targetType == null || (!targetType.HasDefiningNavigation() && !targetType.IsOwned()))
			{
				return null;
			}
			IForeignKey foreignKey = navigation.ForeignKey;
			if (navigation.IsCollection())
			{
				ShapedQueryExpression shapedQueryExpression = CreateShapedQueryExpression(targetType, _sqlExpressionFactory.Select(targetType));
				bool makeNullable = (from p in foreignKey.PrincipalKey.Properties.Concat(foreignKey.Properties)
					select p.ClrType).Any((Type t) => t.IsNullableType());
				Type type = shapedQueryExpression.Type.TryGetSequenceType();
				ParameterExpression parameterExpression = Expression.Parameter(type);
				Expression expression = entityShaperExpression.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? foreignKey.Properties : foreignKey.PrincipalKey.Properties, makeNullable);
				Expression right = parameterExpression.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? foreignKey.PrincipalKey.Properties : foreignKey.Properties, makeNullable);
				Expression expression2 = ((expression is NewExpression newExpression) ? ((UnaryExpression)((NewArrayExpression)newExpression.Arguments[0]).Expressions[0]).Operand : expression);
				LambdaExpression expression3 = Expression.Lambda(expression2.Type.IsNullableType() ? Expression.AndAlso(Expression.NotEqual(expression2, Expression.Constant(null, expression2.Type)), Expression.Equal(expression, right)) : Expression.Equal(expression, right), parameterExpression);
				return Expression.Call(QueryableMethods.Where.MakeGenericMethod(type), shapedQueryExpression, Expression.Quote(expression3));
			}
			EntityProjectionExpression entityProjectionExpression = (EntityProjectionExpression)((entityShaperExpression.ValueBufferExpression is ProjectionBindingExpression projectionBindingExpression) ? _selectExpression.GetMappedProjection(projectionBindingExpression.ProjectionMember) : entityShaperExpression.ValueBufferExpression);
			EntityShaperExpression entityShaperExpression2 = entityProjectionExpression.BindNavigation(navigation);
			if (entityShaperExpression2 == null)
			{
				SelectExpression selectExpression = _sqlExpressionFactory.Select(targetType);
				ShapedQueryExpression shapedQueryExpression2 = CreateShapedQueryExpression(targetType, selectExpression);
				bool makeNullable2 = (from p in foreignKey.PrincipalKey.Properties.Concat(foreignKey.Properties)
					select p.ClrType).Any((Type t) => t.IsNullableType());
				Expression left = entityShaperExpression.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? foreignKey.Properties : foreignKey.PrincipalKey.Properties, makeNullable2);
				Expression right2 = shapedQueryExpression2.ShaperExpression.CreateKeyAccessExpression(navigation.IsDependentToPrincipal() ? foreignKey.PrincipalKey.Properties : foreignKey.Properties, makeNullable2);
				SqlExpression joinPredicate = _sqlTranslator.Translate(Expression.Equal(left, right2));
				_selectExpression.AddLeftJoin(selectExpression, joinPredicate, null);
				TableExpressionBase table = ((LeftJoinExpression)_selectExpression.Tables.Last()).Table;
				entityShaperExpression2 = new EntityShaperExpression(targetType, new EntityProjectionExpression(targetType, table, nullable: true), nullable: true);
				entityProjectionExpression.AddNavigationBinding(navigation, entityShaperExpression2);
			}
			return entityShaperExpression2;
		}
	}

	private readonly RelationalSqlTranslatingExpressionVisitor _sqlTranslator;

	private readonly WeakEntityExpandingExpressionVisitor _weakEntityExpandingExpressionVisitor;

	private readonly RelationalProjectionBindingExpressionVisitor _projectionBindingExpressionVisitor;

	private readonly IModel _model;

	private readonly ISqlExpressionFactory _sqlExpressionFactory;

	private readonly bool _subquery;

	protected virtual RelationalQueryableMethodTranslatingExpressionVisitorDependencies RelationalDependencies { get; }

	public RelationalQueryableMethodTranslatingExpressionVisitor(QueryableMethodTranslatingExpressionVisitorDependencies dependencies, RelationalQueryableMethodTranslatingExpressionVisitorDependencies relationalDependencies, IModel model)
		: base(dependencies, subquery: false)
	{
		RelationalDependencies = relationalDependencies;
		ISqlExpressionFactory sqlExpressionFactory = relationalDependencies.SqlExpressionFactory;
		_sqlTranslator = relationalDependencies.RelationalSqlTranslatingExpressionVisitorFactory.Create(model, this);
		_weakEntityExpandingExpressionVisitor = new WeakEntityExpandingExpressionVisitor(_sqlTranslator, sqlExpressionFactory);
		_projectionBindingExpressionVisitor = new RelationalProjectionBindingExpressionVisitor(this, _sqlTranslator);
		_model = model;
		_sqlExpressionFactory = sqlExpressionFactory;
		_subquery = false;
	}

	protected RelationalQueryableMethodTranslatingExpressionVisitor(RelationalQueryableMethodTranslatingExpressionVisitor parentVisitor)
		: base(parentVisitor.Dependencies, subquery: true)
	{
		RelationalDependencies = parentVisitor.RelationalDependencies;
		_model = parentVisitor._model;
		_sqlTranslator = parentVisitor._sqlTranslator;
		_weakEntityExpandingExpressionVisitor = parentVisitor._weakEntityExpandingExpressionVisitor;
		_projectionBindingExpressionVisitor = new RelationalProjectionBindingExpressionVisitor(this, _sqlTranslator);
		_sqlExpressionFactory = parentVisitor._sqlExpressionFactory;
		_subquery = true;
	}

	protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
	{
		if (methodCallExpression.Method.DeclaringType == typeof(RelationalQueryableExtensions) && methodCallExpression.Method.Name == "FromSqlOnQueryable")
		{
			string sql = (string)((ConstantExpression)methodCallExpression.Arguments[1]).Value;
			IQueryable queryable = (IQueryable)((ConstantExpression)methodCallExpression.Arguments[0]).Value;
			return CreateShapedQueryExpression(queryable.ElementType, sql, methodCallExpression.Arguments[2]);
		}
		return base.VisitMethodCall(methodCallExpression);
	}

	protected override QueryableMethodTranslatingExpressionVisitor CreateSubqueryVisitor()
	{
		return new RelationalQueryableMethodTranslatingExpressionVisitor(this);
	}

	protected override ShapedQueryExpression CreateShapedQueryExpression(Type elementType)
	{
		IEntityType entityType = _model.FindEntityType(elementType);
		SelectExpression selectExpression = _sqlExpressionFactory.Select(entityType);
		return CreateShapedQueryExpression(entityType, selectExpression);
	}

	private ShapedQueryExpression CreateShapedQueryExpression(Type elementType, string sql, Expression arguments)
	{
		IEntityType entityType = _model.FindEntityType(elementType);
		SelectExpression selectExpression = _sqlExpressionFactory.Select(entityType, sql, arguments);
		return CreateShapedQueryExpression(entityType, selectExpression);
	}

	private static ShapedQueryExpression CreateShapedQueryExpression(IEntityType entityType, SelectExpression selectExpression)
	{
		return new ShapedQueryExpression(selectExpression, new EntityShaperExpression(entityType, new ProjectionBindingExpression(selectExpression, new ProjectionMember(), typeof(ValueBuffer)), nullable: false));
	}

	protected override ShapedQueryExpression TranslateAll(ShapedQueryExpression source, LambdaExpression predicate)
	{
		SqlExpression sqlExpression = TranslateLambdaExpression(source, predicate);
		if (sqlExpression == null)
		{
			return null;
		}
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.ApplyPredicate(_sqlExpressionFactory.Not(sqlExpression));
		selectExpression.ReplaceProjectionMapping(new Dictionary<ProjectionMember, Expression>());
		if (selectExpression.Limit == null && selectExpression.Offset == null)
		{
			selectExpression.ClearOrdering();
		}
		sqlExpression = _sqlExpressionFactory.Exists(selectExpression, negated: true);
		source.QueryExpression = _sqlExpressionFactory.Select(sqlExpression);
		source.ShaperExpression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), typeof(bool));
		return source;
	}

	protected override ShapedQueryExpression TranslateAny(ShapedQueryExpression source, LambdaExpression predicate)
	{
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.ReplaceProjectionMapping(new Dictionary<ProjectionMember, Expression>());
		if (selectExpression.Limit == null && selectExpression.Offset == null)
		{
			selectExpression.ClearOrdering();
		}
		ExistsExpression projection = _sqlExpressionFactory.Exists(selectExpression, negated: false);
		source.QueryExpression = _sqlExpressionFactory.Select(projection);
		source.ShaperExpression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), typeof(bool));
		return source;
	}

	protected override ShapedQueryExpression TranslateAverage(ShapedQueryExpression source, LambdaExpression selector, Type resultType)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		Expression expression = ((selector == null || selector.Body == selector.Parameters[0]) ? selectExpression.GetMappedProjection(new ProjectionMember()) : RemapLambdaBody(source, selector));
		SqlExpression sqlExpression = _sqlTranslator.TranslateAverage(expression);
		if (sqlExpression == null)
		{
			return null;
		}
		return AggregateResultShaper(source, sqlExpression, throwOnNullResult: true, resultType);
	}

	protected override ShapedQueryExpression TranslateCast(ShapedQueryExpression source, Type resultType)
	{
		if (source.ShaperExpression.Type != resultType)
		{
			source.ShaperExpression = Expression.Convert(source.ShaperExpression, resultType);
		}
		return source;
	}

	protected override ShapedQueryExpression TranslateConcat(ShapedQueryExpression source1, ShapedQueryExpression source2)
	{
		((SelectExpression)source1.QueryExpression).ApplyUnion((SelectExpression)source2.QueryExpression, distinct: false);
		return source1;
	}

	protected override ShapedQueryExpression TranslateContains(ShapedQueryExpression source, Expression item)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		SqlExpression sqlExpression = TranslateExpression(item);
		if (sqlExpression == null)
		{
			return null;
		}
		if (selectExpression.Limit == null && selectExpression.Offset == null)
		{
			selectExpression.ClearOrdering();
		}
		selectExpression.ApplyProjection();
		sqlExpression = _sqlExpressionFactory.In(sqlExpression, selectExpression, negated: false);
		source.QueryExpression = _sqlExpressionFactory.Select(sqlExpression);
		source.ShaperExpression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), typeof(bool));
		return source;
	}

	protected override ShapedQueryExpression TranslateCount(ShapedQueryExpression source, LambdaExpression predicate)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		SqlExpression sqlExpression = _sqlTranslator.TranslateCount();
		if (sqlExpression == null)
		{
			return null;
		}
		Dictionary<ProjectionMember, Expression> projectionMapping = new Dictionary<ProjectionMember, Expression> { 
		{
			new ProjectionMember(),
			sqlExpression
		} };
		selectExpression.ClearOrdering();
		selectExpression.ReplaceProjectionMapping(projectionMapping);
		source.ShaperExpression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), typeof(int));
		return source;
	}

	protected override ShapedQueryExpression TranslateDefaultIfEmpty(ShapedQueryExpression source, Expression defaultValue)
	{
		if (defaultValue == null)
		{
			((SelectExpression)source.QueryExpression).ApplyDefaultIfEmpty(_sqlExpressionFactory);
			source.ShaperExpression = MarkShaperNullable(source.ShaperExpression);
			return source;
		}
		return null;
	}

	protected override ShapedQueryExpression TranslateDistinct(ShapedQueryExpression source)
	{
		((SelectExpression)source.QueryExpression).ApplyDistinct();
		return source;
	}

	protected override ShapedQueryExpression TranslateElementAtOrDefault(ShapedQueryExpression source, Expression index, bool returnDefault)
	{
		return null;
	}

	protected override ShapedQueryExpression TranslateExcept(ShapedQueryExpression source1, ShapedQueryExpression source2)
	{
		((SelectExpression)source1.QueryExpression).ApplyExcept((SelectExpression)source2.QueryExpression, distinct: true);
		return source1;
	}

	protected override ShapedQueryExpression TranslateFirstOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault)
	{
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		((SelectExpression)source.QueryExpression).ApplyLimit(TranslateExpression(Expression.Constant(1)));
		if (source.ShaperExpression.Type != returnType)
		{
			source.ShaperExpression = Expression.Convert(source.ShaperExpression, returnType);
		}
		return source;
	}

	protected override ShapedQueryExpression TranslateGroupBy(ShapedQueryExpression source, LambdaExpression keySelector, LambdaExpression elementSelector, LambdaExpression resultSelector)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		Expression expression = RemapLambdaBody(source, keySelector);
		Expression expression2 = TranslateGroupingKey(expression);
		if (expression2 != null)
		{
			if (elementSelector != null)
			{
				source = TranslateSelect(source, elementSelector);
			}
			selectExpression.ApplyGrouping(expression2);
			source.ShaperExpression = new GroupByShaperExpression(expression2, source.ShaperExpression);
			if (resultSelector == null)
			{
				return source;
			}
			ParameterExpression parameterExpression = resultSelector.Parameters[0];
			ParameterExpression parameterExpression2 = resultSelector.Parameters[1];
			Expression lambdaBody = new ReplacingExpressionVisitor(new Expression[2] { parameterExpression, parameterExpression2 }, new Expression[2] { expression2, source.ShaperExpression }).Visit(resultSelector.Body);
			lambdaBody = ExpandWeakEntities(selectExpression, lambdaBody);
			source.ShaperExpression = _projectionBindingExpressionVisitor.Translate(selectExpression, lambdaBody);
			return source;
		}
		return null;
	}

	private Expression TranslateGroupingKey(Expression expression)
	{
		if (!(expression is NewExpression newExpression))
		{
			if (expression is MemberInitExpression memberInitExpression)
			{
				NewExpression newExpression2 = (NewExpression)TranslateGroupingKey(memberInitExpression.NewExpression);
				if (newExpression2 == null)
				{
					return null;
				}
				MemberAssignment[] array = new MemberAssignment[memberInitExpression.Bindings.Count];
				for (int i = 0; i < array.Length; i++)
				{
					MemberAssignment memberAssignment = (MemberAssignment)memberInitExpression.Bindings[i];
					Expression expression2 = TranslateGroupingKey(memberAssignment.Expression);
					if (expression2 == null)
					{
						return null;
					}
					array[i] = memberAssignment.Update(expression2);
				}
				return memberInitExpression.Update(newExpression2, array);
			}
			SqlExpression sqlExpression = _sqlTranslator.Translate(expression);
			if (sqlExpression == null)
			{
				return null;
			}
			if (!(sqlExpression.Type == expression.Type))
			{
				return Expression.Convert(sqlExpression, expression.Type);
			}
			return sqlExpression;
		}
		if (newExpression.Constructor == null)
		{
			return newExpression;
		}
		if (newExpression.Arguments.Count == 0)
		{
			return newExpression;
		}
		Expression[] array2 = new Expression[newExpression.Arguments.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = TranslateGroupingKey(newExpression.Arguments[j]);
			if (array2[j] == null)
			{
				return null;
			}
		}
		return newExpression.Update(array2);
	}

	protected override ShapedQueryExpression TranslateGroupJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector)
	{
		return null;
	}

	protected override ShapedQueryExpression TranslateIntersect(ShapedQueryExpression source1, ShapedQueryExpression source2)
	{
		((SelectExpression)source1.QueryExpression).ApplyIntersect((SelectExpression)source2.QueryExpression, distinct: true);
		return source1;
	}

	protected override ShapedQueryExpression TranslateJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector)
	{
		SqlExpression sqlExpression = CreateJoinPredicate(outer, outerKeySelector, inner, innerKeySelector);
		if (sqlExpression != null)
		{
			Type transparentIdentifierType = TransparentIdentifierFactory.Create(resultSelector.Parameters[0].Type, resultSelector.Parameters[1].Type);
			((SelectExpression)outer.QueryExpression).AddInnerJoin((SelectExpression)inner.QueryExpression, sqlExpression, transparentIdentifierType);
			return TranslateResultSelectorForJoin(outer, resultSelector, inner.ShaperExpression, transparentIdentifierType);
		}
		return null;
	}

	protected override ShapedQueryExpression TranslateLeftJoin(ShapedQueryExpression outer, ShapedQueryExpression inner, LambdaExpression outerKeySelector, LambdaExpression innerKeySelector, LambdaExpression resultSelector)
	{
		SqlExpression sqlExpression = CreateJoinPredicate(outer, outerKeySelector, inner, innerKeySelector);
		if (sqlExpression != null)
		{
			Type transparentIdentifierType = TransparentIdentifierFactory.Create(resultSelector.Parameters[0].Type, resultSelector.Parameters[1].Type);
			((SelectExpression)outer.QueryExpression).AddLeftJoin((SelectExpression)inner.QueryExpression, sqlExpression, transparentIdentifierType);
			return TranslateResultSelectorForJoin(outer, resultSelector, MarkShaperNullable(inner.ShaperExpression), transparentIdentifierType);
		}
		return null;
	}

	private SqlExpression CreateJoinPredicate(ShapedQueryExpression outer, LambdaExpression outerKeySelector, ShapedQueryExpression inner, LambdaExpression innerKeySelector)
	{
		Expression expression = RemapLambdaBody(outer, outerKeySelector);
		Expression expression2 = RemapLambdaBody(inner, innerKeySelector);
		if (expression is NewExpression newExpression && newExpression.Type != typeof(AnonymousObject))
		{
			NewExpression newExpression2 = (NewExpression)expression2;
			SqlExpression sqlExpression = null;
			for (int i = 0; i < newExpression.Arguments.Count; i++)
			{
				SqlExpression sqlExpression2 = CreateJoinPredicate(newExpression.Arguments[i], newExpression2.Arguments[i]);
				sqlExpression = ((sqlExpression == null) ? sqlExpression2 : _sqlExpressionFactory.AndAlso(sqlExpression, sqlExpression2));
			}
			return sqlExpression;
		}
		return CreateJoinPredicate(expression, expression2);
	}

	private SqlExpression CreateJoinPredicate(Expression outerKey, Expression innerKey)
	{
		return TranslateExpression(Expression.Equal(outerKey, innerKey));
	}

	protected override ShapedQueryExpression TranslateLastOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		if (selectExpression.Orderings.Count == 0)
		{
			return null;
		}
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		selectExpression.ReverseOrderings();
		selectExpression.ApplyLimit(TranslateExpression(Expression.Constant(1)));
		if (source.ShaperExpression.Type != returnType)
		{
			source.ShaperExpression = Expression.Convert(source.ShaperExpression, returnType);
		}
		return source;
	}

	protected override ShapedQueryExpression TranslateLongCount(ShapedQueryExpression source, LambdaExpression predicate)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		SqlExpression sqlExpression = _sqlTranslator.TranslateLongCount();
		if (sqlExpression == null)
		{
			return null;
		}
		Dictionary<ProjectionMember, Expression> projectionMapping = new Dictionary<ProjectionMember, Expression> { 
		{
			new ProjectionMember(),
			sqlExpression
		} };
		selectExpression.ClearOrdering();
		selectExpression.ReplaceProjectionMapping(projectionMapping);
		source.ShaperExpression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), typeof(long));
		return source;
	}

	protected override ShapedQueryExpression TranslateMax(ShapedQueryExpression source, LambdaExpression selector, Type resultType)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		Expression expression = ((selector == null || selector.Body == selector.Parameters[0]) ? selectExpression.GetMappedProjection(new ProjectionMember()) : RemapLambdaBody(source, selector));
		SqlExpression projection = _sqlTranslator.TranslateMax(expression);
		return AggregateResultShaper(source, projection, throwOnNullResult: true, resultType);
	}

	protected override ShapedQueryExpression TranslateMin(ShapedQueryExpression source, LambdaExpression selector, Type resultType)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		Expression expression = ((selector == null || selector.Body == selector.Parameters[0]) ? selectExpression.GetMappedProjection(new ProjectionMember()) : RemapLambdaBody(source, selector));
		SqlExpression projection = _sqlTranslator.TranslateMin(expression);
		return AggregateResultShaper(source, projection, throwOnNullResult: true, resultType);
	}

	protected override ShapedQueryExpression TranslateOfType(ShapedQueryExpression source, Type resultType)
	{
		if (source.ShaperExpression is EntityShaperExpression { EntityType: var entityType } entityShaperExpression)
		{
			if (entityType.ClrType == resultType)
			{
				return source;
			}
			IEntityType entityType2 = entityType.GetAllBaseTypes().SingleOrDefault((IEntityType et) => et.ClrType == resultType);
			if (entityType2 != null)
			{
				source.ShaperExpression = entityShaperExpression.WithEntityType(entityType2);
				return source;
			}
			IEntityType entityType3 = entityType.GetDerivedTypes().SingleOrDefault((IEntityType et) => et.ClrType == resultType);
			if (entityType3 != null)
			{
				SelectExpression obj = (SelectExpression)source.QueryExpression;
				List<IEntityType> list = entityType3.GetConcreteDerivedTypesInclusive().ToList();
				ProjectionBindingExpression projectionBindingExpression = (ProjectionBindingExpression)entityShaperExpression.ValueBufferExpression;
				ColumnExpression columnExpression = ((EntityProjectionExpression)obj.GetMappedProjection(projectionBindingExpression.ProjectionMember)).BindProperty(entityType.GetDiscriminatorProperty());
				SqlExpression expression = ((list.Count == 1) ? ((SqlExpression)_sqlExpressionFactory.Equal(columnExpression, _sqlExpressionFactory.Constant(list[0].GetDiscriminatorValue()))) : ((SqlExpression)_sqlExpressionFactory.In(columnExpression, _sqlExpressionFactory.Constant(list.Select((IEntityType et) => et.GetDiscriminatorValue())), negated: false)));
				obj.ApplyPredicate(expression);
				ProjectionMember projectionMember = projectionBindingExpression.ProjectionMember;
				EntityProjectionExpression entityProjectionExpression = (EntityProjectionExpression)obj.GetMappedProjection(projectionMember);
				obj.ReplaceProjectionMapping(new Dictionary<ProjectionMember, Expression> { 
				{
					projectionMember,
					entityProjectionExpression.UpdateEntityType(entityType3)
				} });
				source.ShaperExpression = entityShaperExpression.WithEntityType(entityType3);
				return source;
			}
		}
		return null;
	}

	protected override ShapedQueryExpression TranslateOrderBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending)
	{
		SqlExpression sqlExpression = TranslateLambdaExpression(source, keySelector);
		if (sqlExpression == null)
		{
			return null;
		}
		((SelectExpression)source.QueryExpression).ApplyOrdering(new OrderingExpression(sqlExpression, ascending));
		return source;
	}

	protected override ShapedQueryExpression TranslateReverse(ShapedQueryExpression source)
	{
		return null;
	}

	protected override ShapedQueryExpression TranslateSelect(ShapedQueryExpression source, LambdaExpression selector)
	{
		if (selector.Body == selector.Parameters[0])
		{
			return source;
		}
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		if (selectExpression.IsDistinct)
		{
			selectExpression.PushdownIntoSubquery();
		}
		Expression expression = ReplacingExpressionVisitor.Replace(selector.Parameters.Single(), source.ShaperExpression, selector.Body);
		source.ShaperExpression = _projectionBindingExpressionVisitor.Translate(selectExpression, expression);
		return source;
	}

	protected override ShapedQueryExpression TranslateSelectMany(ShapedQueryExpression source, LambdaExpression collectionSelector, LambdaExpression resultSelector)
	{
		var (lambdaExpression, flag, flag2) = new CorrelationFindingExpressionVisitor().IsCorrelated(collectionSelector);
		if (flag)
		{
			Expression node = RemapLambdaBody(source, lambdaExpression);
			if (Visit(node) is ShapedQueryExpression shapedQueryExpression)
			{
				Type transparentIdentifierType = TransparentIdentifierFactory.Create(resultSelector.Parameters[0].Type, resultSelector.Parameters[1].Type);
				Expression expression = shapedQueryExpression.ShaperExpression;
				if (flag2)
				{
					((SelectExpression)source.QueryExpression).AddOuterApply((SelectExpression)shapedQueryExpression.QueryExpression, transparentIdentifierType);
					expression = MarkShaperNullable(expression);
				}
				else
				{
					((SelectExpression)source.QueryExpression).AddCrossApply((SelectExpression)shapedQueryExpression.QueryExpression, transparentIdentifierType);
				}
				return TranslateResultSelectorForJoin(source, resultSelector, expression, transparentIdentifierType);
			}
		}
		else
		{
			ShapedQueryExpression shapedQueryExpression2 = Visit(lambdaExpression.Body) as ShapedQueryExpression;
			if (shapedQueryExpression2 != null)
			{
				if (flag2)
				{
					shapedQueryExpression2 = TranslateDefaultIfEmpty(shapedQueryExpression2, null);
					if (shapedQueryExpression2 == null)
					{
						return null;
					}
				}
				Type transparentIdentifierType2 = TransparentIdentifierFactory.Create(resultSelector.Parameters[0].Type, resultSelector.Parameters[1].Type);
				((SelectExpression)source.QueryExpression).AddCrossJoin((SelectExpression)shapedQueryExpression2.QueryExpression, transparentIdentifierType2);
				return TranslateResultSelectorForJoin(source, resultSelector, shapedQueryExpression2.ShaperExpression, transparentIdentifierType2);
			}
		}
		return null;
	}

	protected override ShapedQueryExpression TranslateSelectMany(ShapedQueryExpression source, LambdaExpression selector)
	{
		ParameterExpression parameterExpression = Expression.Parameter(selector.ReturnType.TryGetSequenceType(), "i");
		LambdaExpression resultSelector = Expression.Lambda(parameterExpression, Expression.Parameter(source.Type.TryGetSequenceType()), parameterExpression);
		return TranslateSelectMany(source, selector, resultSelector);
	}

	protected override ShapedQueryExpression TranslateSingleOrDefault(ShapedQueryExpression source, LambdaExpression predicate, Type returnType, bool returnDefault)
	{
		if (predicate != null)
		{
			source = TranslateWhere(source, predicate);
			if (source == null)
			{
				return null;
			}
		}
		((SelectExpression)source.QueryExpression).ApplyLimit(TranslateExpression(Expression.Constant(_subquery ? 1 : 2)));
		if (source.ShaperExpression.Type != returnType)
		{
			source.ShaperExpression = Expression.Convert(source.ShaperExpression, returnType);
		}
		return source;
	}

	protected override ShapedQueryExpression TranslateSkip(ShapedQueryExpression source, Expression count)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		SqlExpression sqlExpression = TranslateExpression(count);
		if (sqlExpression == null)
		{
			return null;
		}
		selectExpression.ApplyOffset(sqlExpression);
		return source;
	}

	protected override ShapedQueryExpression TranslateSkipWhile(ShapedQueryExpression source, LambdaExpression predicate)
	{
		return null;
	}

	protected override ShapedQueryExpression TranslateSum(ShapedQueryExpression source, LambdaExpression selector, Type resultType)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		selectExpression.PrepareForAggregate();
		Expression expression = ((selector == null || selector.Body == selector.Parameters[0]) ? selectExpression.GetMappedProjection(new ProjectionMember()) : RemapLambdaBody(source, selector));
		SqlExpression sqlExpression = _sqlTranslator.TranslateSum(expression);
		if (sqlExpression == null)
		{
			return null;
		}
		return AggregateResultShaper(source, sqlExpression, throwOnNullResult: false, resultType);
	}

	protected override ShapedQueryExpression TranslateTake(ShapedQueryExpression source, Expression count)
	{
		SelectExpression selectExpression = (SelectExpression)source.QueryExpression;
		SqlExpression sqlExpression = TranslateExpression(count);
		if (sqlExpression == null)
		{
			return null;
		}
		selectExpression.ApplyLimit(sqlExpression);
		return source;
	}

	protected override ShapedQueryExpression TranslateTakeWhile(ShapedQueryExpression source, LambdaExpression predicate)
	{
		return null;
	}

	protected override ShapedQueryExpression TranslateThenBy(ShapedQueryExpression source, LambdaExpression keySelector, bool ascending)
	{
		SqlExpression sqlExpression = TranslateLambdaExpression(source, keySelector);
		if (sqlExpression == null)
		{
			return null;
		}
		((SelectExpression)source.QueryExpression).AppendOrdering(new OrderingExpression(sqlExpression, ascending));
		return source;
	}

	protected override ShapedQueryExpression TranslateUnion(ShapedQueryExpression source1, ShapedQueryExpression source2)
	{
		((SelectExpression)source1.QueryExpression).ApplyUnion((SelectExpression)source2.QueryExpression, distinct: true);
		return source1;
	}

	protected override ShapedQueryExpression TranslateWhere(ShapedQueryExpression source, LambdaExpression predicate)
	{
		SqlExpression sqlExpression = TranslateLambdaExpression(source, predicate);
		if (sqlExpression == null)
		{
			return null;
		}
		((SelectExpression)source.QueryExpression).ApplyPredicate(sqlExpression);
		return source;
	}

	private SqlExpression TranslateExpression(Expression expression)
	{
		return _sqlTranslator.Translate(expression);
	}

	private SqlExpression TranslateLambdaExpression(ShapedQueryExpression shapedQueryExpression, LambdaExpression lambdaExpression)
	{
		return TranslateExpression(RemapLambdaBody(shapedQueryExpression, lambdaExpression));
	}

	private Expression RemapLambdaBody(ShapedQueryExpression shapedQueryExpression, LambdaExpression lambdaExpression)
	{
		Expression lambdaBody = ReplacingExpressionVisitor.Replace(lambdaExpression.Parameters.Single(), shapedQueryExpression.ShaperExpression, lambdaExpression.Body);
		return ExpandWeakEntities((SelectExpression)shapedQueryExpression.QueryExpression, lambdaBody);
	}

	internal Expression ExpandWeakEntities(SelectExpression selectExpression, Expression lambdaBody)
	{
		return _weakEntityExpandingExpressionVisitor.Expand(selectExpression, lambdaBody);
	}

	private ShapedQueryExpression AggregateResultShaper(ShapedQueryExpression source, Expression projection, bool throwOnNullResult, Type resultType)
	{
		if (projection == null)
		{
			return null;
		}
		SelectExpression obj = (SelectExpression)source.QueryExpression;
		obj.ReplaceProjectionMapping(new Dictionary<ProjectionMember, Expression> { 
		{
			new ProjectionMember(),
			projection
		} });
		obj.ClearOrdering();
		Type type = resultType.MakeNullable();
		Expression expression = new ProjectionBindingExpression(source.QueryExpression, new ProjectionMember(), throwOnNullResult ? type : projection.Type);
		if (throwOnNullResult)
		{
			ParameterExpression parameterExpression = Expression.Variable(type, "result");
			Expression ifTrue = (resultType.IsNullableType() ? ((Expression)Expression.Constant(null, resultType)) : ((Expression)Expression.Throw(Expression.New(typeof(InvalidOperationException).GetConstructors().Single((ConstructorInfo ci) => ci.GetParameters().Length == 1), Expression.Constant(CoreStrings.NoElements)), resultType)));
			expression = Expression.Block(new ParameterExpression[1] { parameterExpression }, Expression.Assign(parameterExpression, expression), Expression.Condition(Expression.Equal(parameterExpression, Expression.Default(type)), ifTrue, (resultType != parameterExpression.Type) ? ((Expression)Expression.Convert(parameterExpression, resultType)) : ((Expression)parameterExpression)));
		}
		else if (resultType != expression.Type)
		{
			expression = Expression.Convert(expression, resultType);
		}
		source.ShaperExpression = expression;
		return source;
	}
}
