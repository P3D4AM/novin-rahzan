using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query;

public abstract class ShapedQueryCompilingExpressionVisitor : ExpressionVisitor
{
	private class ConstantVerifyingExpressionVisitor : ExpressionVisitor
	{
		private readonly ITypeMappingSource _typeMappingSource;

		public ConstantVerifyingExpressionVisitor(ITypeMappingSource typeMappingSource)
		{
			_typeMappingSource = typeMappingSource;
		}

		private bool ValidConstant(ConstantExpression constantExpression)
		{
			if (constantExpression.Value != null)
			{
				return _typeMappingSource.FindMapping(constantExpression.Type) != null;
			}
			return true;
		}

		protected override Expression VisitConstant(ConstantExpression constantExpression)
		{
			if (!ValidConstant(constantExpression))
			{
				throw new InvalidOperationException(CoreStrings.ClientProjectionCapturingConstantInTree(constantExpression.Type.DisplayName()));
			}
			return constantExpression;
		}

		protected override Expression VisitMethodCall(MethodCallExpression methodCallExpression)
		{
			if (RemoveConvert(methodCallExpression.Object) is ConstantExpression constantExpression && !ValidConstant(constantExpression))
			{
				throw new InvalidOperationException(CoreStrings.ClientProjectionCapturingConstantInMethodInstance(constantExpression.Type.DisplayName(), methodCallExpression.Method.Name));
			}
			foreach (Expression argument in methodCallExpression.Arguments)
			{
				if (RemoveConvert(argument) is ConstantExpression constantExpression2 && !ValidConstant(constantExpression2))
				{
					throw new InvalidOperationException(CoreStrings.ClientProjectionCapturingConstantInMethodArgument(constantExpression2.Type.DisplayName(), methodCallExpression.Method.Name));
				}
			}
			return base.VisitMethodCall(methodCallExpression);
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is EntityShaperExpression) && !(extensionExpression is ProjectionBindingExpression))
			{
				return base.VisitExtension(extensionExpression);
			}
			return extensionExpression;
		}

		private static Expression RemoveConvert(Expression expression)
		{
			while (expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
			{
				expression = RemoveConvert(((UnaryExpression)expression).Operand);
			}
			return expression;
		}
	}

	private class EntityMaterializerInjectingExpressionVisitor : ExpressionVisitor
	{
		private static readonly ConstructorInfo _materializationContextConstructor = typeof(MaterializationContext).GetConstructors().Single((ConstructorInfo ci) => ci.GetParameters().Length == 2);

		private static readonly ConstructorInfo _valueBufferConstructor = typeof(ValueBuffer).GetTypeInfo().DeclaredConstructors.Single((ConstructorInfo ci) => ci.GetParameters().Length == 1);

		private static readonly PropertyInfo _dbContextMemberInfo = typeof(QueryContext).GetProperty("Context");

		private static readonly PropertyInfo _stateManagerMemberInfo = typeof(QueryContext).GetProperty("StateManager");

		private static readonly PropertyInfo _entityMemberInfo = typeof(InternalEntityEntry).GetProperty("Entity");

		private static readonly PropertyInfo _entityTypeMemberInfo = typeof(InternalEntityEntry).GetProperty("EntityType");

		private static readonly MethodInfo _tryGetEntryMethodInfo = typeof(IStateManager).GetTypeInfo().GetDeclaredMethods("TryGetEntry").Single((MethodInfo mi) => mi.GetParameters().Length == 4);

		private static readonly MethodInfo _startTrackingMethodInfo = typeof(QueryContext).GetMethod("StartTracking", new Type[3]
		{
			typeof(IEntityType),
			typeof(object),
			typeof(ValueBuffer)
		});

		private readonly IEntityMaterializerSource _entityMaterializerSource;

		private readonly bool _trackQueryResults;

		private readonly ISet<IEntityType> _visitedEntityTypes = new HashSet<IEntityType>();

		private int _currentEntityIndex;

		private static readonly MethodInfo _createUnableToDiscriminateException = typeof(EntityMaterializerInjectingExpressionVisitor).GetTypeInfo().GetDeclaredMethod("CreateUnableToDiscriminateException");

		public EntityMaterializerInjectingExpressionVisitor(IEntityMaterializerSource entityMaterializerSource, bool trackQueryResults)
		{
			_entityMaterializerSource = entityMaterializerSource;
			_trackQueryResults = trackQueryResults;
		}

		public Expression Inject(Expression expression)
		{
			Expression result = Visit(expression);
			if (_trackQueryResults)
			{
				foreach (IEntityType visitedEntityType in _visitedEntityTypes)
				{
					IForeignKey foreignKey = visitedEntityType.FindOwnership();
					if (foreignKey != null && !ContainsOwner(foreignKey.PrincipalEntityType))
					{
						throw new InvalidOperationException("A tracking query projects owned entity without corresponding owner in result. Owned entities cannot be tracked without their owner. Either include the owner entity in the result or make query non-tracking using AsNoTracking().");
					}
				}
			}
			return result;
			bool ContainsOwner(IEntityType owner)
			{
				if (owner != null)
				{
					if (!_visitedEntityTypes.Contains(owner))
					{
						return ContainsOwner(owner.BaseType);
					}
					return true;
				}
				return false;
			}
		}

		protected override Expression VisitExtension(Expression extensionExpression)
		{
			if (!(extensionExpression is EntityShaperExpression entityShaperExpression))
			{
				return base.VisitExtension(extensionExpression);
			}
			return ProcessEntityShaper(entityShaperExpression);
		}

		private Expression ProcessEntityShaper(EntityShaperExpression entityShaperExpression)
		{
			_currentEntityIndex++;
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			IEntityType entityType = entityShaperExpression.EntityType;
			ParameterExpression parameterExpression = Expression.Variable(typeof(MaterializationContext), "materializationContext" + _currentEntityIndex);
			list2.Add(parameterExpression);
			list.Add(Expression.Assign(parameterExpression, Expression.New(_materializationContextConstructor, entityShaperExpression.ValueBufferExpression, Expression.MakeMemberAccess(QueryCompilationContext.QueryContextParameter, _dbContextMemberInfo))));
			MethodCallExpression valueBufferExpression = Expression.Call(parameterExpression, MaterializationContext.GetValueBufferMethod);
			IKey key = entityType.FindPrimaryKey();
			ParameterExpression parameterExpression2 = Expression.Variable(typeof(IEntityType), "entityType" + _currentEntityIndex);
			list2.Add(parameterExpression2);
			ParameterExpression parameterExpression3 = Expression.Variable(entityType.ClrType, "instance" + _currentEntityIndex);
			list2.Add(parameterExpression3);
			list.Add(Expression.Assign(parameterExpression3, Expression.Constant(null, entityType.ClrType)));
			if (_trackQueryResults && key != null)
			{
				ParameterExpression parameterExpression4 = Expression.Variable(typeof(InternalEntityEntry), "entry" + _currentEntityIndex);
				ParameterExpression parameterExpression5 = Expression.Variable(typeof(bool), "hasNullKey" + _currentEntityIndex);
				list2.Add(parameterExpression4);
				list2.Add(parameterExpression5);
				list.Add(Expression.Assign(parameterExpression4, Expression.Call(Expression.MakeMemberAccess(QueryCompilationContext.QueryContextParameter, _stateManagerMemberInfo), _tryGetEntryMethodInfo, Expression.Constant(key), Expression.NewArrayInit(typeof(object), key.Properties.Select((IProperty p) => _entityMaterializerSource.CreateReadValueExpression(valueBufferExpression, typeof(object), p.GetIndex(), p))), Expression.Constant(!entityShaperExpression.IsNullable), parameterExpression5)));
				list.Add(Expression.IfThen(Expression.Not(parameterExpression5), Expression.IfThenElse(Expression.NotEqual(parameterExpression4, Expression.Constant(null, typeof(InternalEntityEntry))), Expression.Block(Expression.Assign(parameterExpression2, Expression.MakeMemberAccess(parameterExpression4, _entityTypeMemberInfo)), Expression.Assign(parameterExpression3, Expression.Convert(Expression.MakeMemberAccess(parameterExpression4, _entityMemberInfo), entityType.ClrType))), MaterializeEntity(entityType, parameterExpression, parameterExpression2, parameterExpression3, parameterExpression4))));
			}
			else
			{
				list.Add(Expression.IfThen((key != null) ? key.Properties.Select((IProperty p) => Expression.NotEqual(_entityMaterializerSource.CreateReadValueExpression(valueBufferExpression, typeof(object), p.GetIndex(), p), Expression.Constant(null))).Aggregate((BinaryExpression a, BinaryExpression b) => Expression.AndAlso(a, b)) : (from p in entityType.GetProperties()
					select Expression.NotEqual(_entityMaterializerSource.CreateReadValueExpression(valueBufferExpression, typeof(object), p.GetIndex(), p), Expression.Constant(null))).Aggregate((BinaryExpression a, BinaryExpression b) => Expression.OrElse(a, b)), MaterializeEntity(entityType, parameterExpression, parameterExpression2, parameterExpression3, null)));
			}
			list.Add(parameterExpression3);
			return Expression.Block(list2, list);
		}

		private Expression MaterializeEntity(IEntityType entityType, ParameterExpression materializationContextVariable, ParameterExpression concreteEntityTypeVariable, ParameterExpression instanceVariable, ParameterExpression entryVariable)
		{
			List<Expression> list = new List<Expression>();
			List<ParameterExpression> list2 = new List<ParameterExpression>();
			ParameterExpression parameterExpression = Expression.Variable(typeof(ValueBuffer), "shadowValueBuffer" + _currentEntityIndex);
			list2.Add(parameterExpression);
			list.Add(Expression.Assign(parameterExpression, Expression.Constant(ValueBuffer.Empty)));
			Type clrType = entityType.ClrType;
			MethodCallExpression valueBuffer = Expression.Call(materializationContextVariable, MaterializationContext.GetValueBufferMethod);
			(Type, ParameterExpression, ParameterExpression, ParameterExpression) materializeExpressionContext = (clrType, materializationContextVariable, concreteEntityTypeVariable, parameterExpression);
			List<IEntityType> list3 = entityType.GetConcreteDerivedTypesInclusive().ToList();
			IEntityType entityType2 = list3[0];
			Expression expression;
			if (list3.Count == 1)
			{
				expression = CreateFullMaterializeExpression(entityType2, in materializeExpressionContext);
			}
			else
			{
				IProperty discriminatorProperty = entityType2.GetDiscriminatorProperty();
				ParameterExpression parameterExpression2 = Expression.Variable(discriminatorProperty.ClrType, "discriminator" + _currentEntityIndex);
				list2.Add(parameterExpression2);
				list.Add(Expression.Assign(parameterExpression2, _entityMaterializerSource.CreateReadValueExpression(valueBuffer, discriminatorProperty.ClrType, discriminatorProperty.GetIndex(), discriminatorProperty)));
				expression = Expression.Block(Expression.Throw(Expression.Call(_createUnableToDiscriminateException, Expression.Constant(entityType), Expression.Convert(parameterExpression2, typeof(object)))), Expression.Constant(null, clrType));
				foreach (IEntityType item in list3)
				{
					ConstantExpression right = Expression.Constant(item.GetDiscriminatorValue(), discriminatorProperty.ClrType);
					expression = Expression.Condition(Expression.Equal(parameterExpression2, right), CreateFullMaterializeExpression(item, in materializeExpressionContext), expression);
				}
			}
			list.Add(Expression.Assign(instanceVariable, expression));
			if (_trackQueryResults && entityType.FindPrimaryKey() != null)
			{
				foreach (IEntityType item2 in entityType.GetTypesInHierarchy())
				{
					_visitedEntityTypes.Add(item2);
				}
				list.Add(Expression.Assign(entryVariable, Expression.Call(QueryCompilationContext.QueryContextParameter, _startTrackingMethodInfo, concreteEntityTypeVariable, instanceVariable, parameterExpression)));
			}
			list.Add(instanceVariable);
			return Expression.Block(clrType, list2, list);
		}

		private BlockExpression CreateFullMaterializeExpression(IEntityType concreteEntityType, in (Type ReturnType, ParameterExpression MaterializationContextVariable, ParameterExpression ConcreteEntityTypeVariable, ParameterExpression ShadowValuesVariable) materializeExpressionContext)
		{
			(Type ReturnType, ParameterExpression MaterializationContextVariable, ParameterExpression ConcreteEntityTypeVariable, ParameterExpression ShadowValuesVariable) tuple = materializeExpressionContext;
			Type item = tuple.ReturnType;
			ParameterExpression item2 = tuple.MaterializationContextVariable;
			ParameterExpression item3 = tuple.ConcreteEntityTypeVariable;
			ParameterExpression item4 = tuple.ShadowValuesVariable;
			List<Expression> list = new List<Expression>(3) { Expression.Assign(item3, Expression.Constant(concreteEntityType)) };
			Expression expression = _entityMaterializerSource.CreateMaterializeExpression(concreteEntityType, "instance", item2);
			if (_trackQueryResults && concreteEntityType.ShadowPropertyCount() > 0)
			{
				MethodCallExpression valueBufferExpression = Expression.Call(item2, MaterializationContext.GetValueBufferMethod);
				IEnumerable<IProperty> source = from p in concreteEntityType.GetProperties()
					where p.IsShadowProperty()
					select p;
				list.Add(Expression.Assign(item4, Expression.New(_valueBufferConstructor, Expression.NewArrayInit(typeof(object), source.Select((IProperty p) => _entityMaterializerSource.CreateReadValueExpression(valueBufferExpression, typeof(object), p.GetIndex(), p))))));
			}
			expression = ((expression.Type == item) ? expression : Expression.Convert(expression, item));
			list.Add(expression);
			return Expression.Block(list);
		}

		[JetBrains.Annotations.UsedImplicitly]
		private static Exception CreateUnableToDiscriminateException(IEntityType entityType, object discriminator)
		{
			return new InvalidOperationException(CoreStrings.UnableToDiscriminate(entityType.DisplayName(), discriminator?.ToString()));
		}
	}

	private static readonly PropertyInfo _cancellationTokenMemberInfo = typeof(QueryContext).GetProperty("CancellationToken");

	private readonly Expression _cancellationTokenParameter;

	private readonly EntityMaterializerInjectingExpressionVisitor _entityMaterializerInjectingExpressionVisitor;

	private readonly ConstantVerifyingExpressionVisitor _constantVerifyingExpressionVisitor;

	private static readonly MethodInfo _singleAsyncMethodInfo = typeof(ShapedQueryCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethods("SingleAsync").Single((MethodInfo mi) => mi.GetParameters().Length == 2);

	private static readonly MethodInfo _singleOrDefaultAsyncMethodInfo = typeof(ShapedQueryCompilingExpressionVisitor).GetTypeInfo().GetDeclaredMethods("SingleOrDefaultAsync").Single((MethodInfo mi) => mi.GetParameters().Length == 2);

	protected virtual ShapedQueryCompilingExpressionVisitorDependencies Dependencies { get; }

	protected virtual bool IsTracking { get; }

	public virtual bool IsBuffering { get; internal set; }

	protected virtual bool IsAsync { get; }

	protected ShapedQueryCompilingExpressionVisitor(ShapedQueryCompilingExpressionVisitorDependencies dependencies, QueryCompilationContext queryCompilationContext)
	{
		Dependencies = dependencies;
		IsTracking = queryCompilationContext.IsTracking;
		_entityMaterializerInjectingExpressionVisitor = new EntityMaterializerInjectingExpressionVisitor(dependencies.EntityMaterializerSource, queryCompilationContext.IsTracking);
		_constantVerifyingExpressionVisitor = new ConstantVerifyingExpressionVisitor(dependencies.TypeMappingSource);
		IsBuffering = queryCompilationContext.IsBuffering;
		IsAsync = queryCompilationContext.IsAsync;
		if (queryCompilationContext.IsAsync)
		{
			_cancellationTokenParameter = Expression.MakeMemberAccess(QueryCompilationContext.QueryContextParameter, _cancellationTokenMemberInfo);
		}
	}

	protected override Expression VisitExtension(Expression extensionExpression)
	{
		if (extensionExpression is ShapedQueryExpression shapedQueryExpression)
		{
			Expression expression = VisitShapedQueryExpression(shapedQueryExpression);
			switch (shapedQueryExpression.ResultCardinality)
			{
			case ResultCardinality.Enumerable:
				return expression;
			case ResultCardinality.Single:
				if (!IsAsync)
				{
					return Expression.Call(EnumerableMethods.SingleWithoutPredicate.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression);
				}
				return Expression.Call(_singleAsyncMethodInfo.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression, _cancellationTokenParameter);
			case ResultCardinality.SingleOrDefault:
				if (!IsAsync)
				{
					return Expression.Call(EnumerableMethods.SingleOrDefaultWithoutPredicate.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression);
				}
				return Expression.Call(_singleOrDefaultAsyncMethodInfo.MakeGenericMethod(expression.Type.TryGetSequenceType()), expression, _cancellationTokenParameter);
			}
		}
		return base.VisitExtension(extensionExpression);
	}

	private static async Task<TSource> SingleAsync<TSource>(IAsyncEnumerable<TSource> asyncEnumerable, CancellationToken cancellationToken = default(CancellationToken))
	{
		TSource result2;
		await using (IAsyncEnumerator<TSource> enumerator = asyncEnumerable.GetAsyncEnumerator(cancellationToken))
		{
			if (!(await enumerator.MoveNextAsync()))
			{
				throw new InvalidOperationException("Enumerator failed to MoveNextAsync.");
			}
			TSource result = enumerator.Current;
			if (await enumerator.MoveNextAsync())
			{
				throw new InvalidOperationException("Enumerator failed to MoveNextAsync.");
			}
			result2 = result;
		}
		return result2;
	}

	private static async Task<TSource> SingleOrDefaultAsync<TSource>(IAsyncEnumerable<TSource> asyncEnumerable, CancellationToken cancellationToken = default(CancellationToken))
	{
		TSource result2;
		await using (IAsyncEnumerator<TSource> enumerator = asyncEnumerable.GetAsyncEnumerator(cancellationToken))
		{
			if (!(await enumerator.MoveNextAsync()))
			{
				result2 = default(TSource);
			}
			else
			{
				TSource result = enumerator.Current;
				if (await enumerator.MoveNextAsync())
				{
					throw new InvalidOperationException("Enumerator failed to MoveNextAsync.");
				}
				result2 = result;
			}
		}
		return result2;
	}

	protected abstract Expression VisitShapedQueryExpression(ShapedQueryExpression shapedQueryExpression);

	protected virtual Expression InjectEntityMaterializers(Expression expression)
	{
		_constantVerifyingExpressionVisitor.Visit(expression);
		return _entityMaterializerInjectingExpressionVisitor.Inject(expression);
	}
}
