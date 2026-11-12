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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Internal;

public class EntityFinder<TEntity> : IEntityFinder<TEntity>, IEntityFinder where TEntity : class
{
	private readonly IStateManager _stateManager;

	private readonly IDbSetSource _setSource;

	private readonly IDbSetCache _setCache;

	private readonly IModel _model;

	private readonly IQueryable<TEntity> _queryRoot;

	private static readonly MethodInfo _selectMethod = typeof(EntityFinder<TEntity>).GetTypeInfo().GetDeclaredMethods("Select").Single((MethodInfo mi) => mi.IsGenericMethodDefinition);

	public EntityFinder([JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IDbSetSource setSource, [JetBrains.Annotations.NotNull] IDbSetCache setCache, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		_stateManager = stateManager;
		_setSource = setSource;
		_setCache = setCache;
		_model = entityType.Model;
		_queryRoot = (IQueryable<TEntity>)BuildQueryRoot(entityType);
	}

	public virtual TEntity Find(object[] keyValues)
	{
		object obj;
		if (keyValues != null && !keyValues.Any((object v) => v == null))
		{
			obj = FindTracked(keyValues, out var keyProperties);
			if (obj == null)
			{
				return _queryRoot.FirstOrDefault(BuildLambda(keyProperties, new ValueBuffer(keyValues)));
			}
		}
		else
		{
			obj = null;
		}
		return (TEntity)obj;
	}

	object IEntityFinder.Find(object[] keyValues)
	{
		return Find(keyValues);
	}

	public virtual ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (keyValues == null || keyValues.Any((object v) => v == null))
		{
			return new ValueTask<TEntity>((TEntity)null);
		}
		IReadOnlyList<IProperty> keyProperties;
		TEntity val = FindTracked(keyValues, out keyProperties);
		if (val == null)
		{
			return new ValueTask<TEntity>(_queryRoot.FirstOrDefaultAsync(BuildLambda(keyProperties, new ValueBuffer(keyValues)), cancellationToken));
		}
		return new ValueTask<TEntity>(val);
	}

	ValueTask<object> IEntityFinder.FindAsync(object[] keyValues, CancellationToken cancellationToken)
	{
		if (keyValues == null || keyValues.Any((object v) => v == null))
		{
			return new ValueTask<object>((object)null);
		}
		IReadOnlyList<IProperty> keyProperties;
		TEntity val = FindTracked(keyValues, out keyProperties);
		if (val == null)
		{
			return new ValueTask<object>(_queryRoot.FirstOrDefaultAsync(BuildObjectLambda(keyProperties, new ValueBuffer(keyValues)), cancellationToken));
		}
		return new ValueTask<object>(val);
	}

	public virtual void Load(INavigation navigation, InternalEntityEntry entry)
	{
		if (entry.EntityState == EntityState.Detached)
		{
			throw new InvalidOperationException(CoreStrings.CannotLoadDetached(navigation.Name, entry.EntityType.DisplayName()));
		}
		object[] loadValues = GetLoadValues(navigation, entry);
		if (loadValues != null)
		{
			Query(navigation, loadValues).Load();
		}
		entry.SetIsLoaded(navigation);
	}

	public virtual async Task LoadAsync(INavigation navigation, InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (entry.EntityState == EntityState.Detached)
		{
			throw new InvalidOperationException(CoreStrings.CannotLoadDetached(navigation.Name, entry.EntityType.DisplayName()));
		}
		object[] loadValues = GetLoadValues(navigation, entry);
		if (loadValues != null)
		{
			await Query(navigation, loadValues).LoadAsync(cancellationToken);
		}
		entry.SetIsLoaded(navigation);
	}

	public virtual IQueryable<TEntity> Query(INavigation navigation, InternalEntityEntry entry)
	{
		if (entry.EntityState == EntityState.Detached)
		{
			throw new InvalidOperationException(CoreStrings.CannotLoadDetached(navigation.Name, entry.EntityType.DisplayName()));
		}
		object[] loadValues = GetLoadValues(navigation, entry);
		if (loadValues == null)
		{
			return _queryRoot.Where((TEntity e) => false);
		}
		return Query(navigation, loadValues);
	}

	public virtual object[] GetDatabaseValues(InternalEntityEntry entry)
	{
		return GetDatabaseValuesQuery(entry)?.FirstOrDefault();
	}

	public virtual Task<object[]> GetDatabaseValuesAsync(InternalEntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetDatabaseValuesQuery(entry)?.FirstOrDefaultAsync(cancellationToken);
	}

	private IQueryable<object[]> GetDatabaseValuesQuery(InternalEntityEntry entry)
	{
		IEntityType entityType = entry.EntityType;
		IReadOnlyList<IProperty> properties = entityType.FindPrimaryKey().Properties;
		object[] array = new object[properties.Count];
		for (int i = 0; i < array.Length; i++)
		{
			object obj = entry[properties[i]];
			if (obj == null)
			{
				return null;
			}
			array[i] = obj;
		}
		return _queryRoot.AsNoTracking().IgnoreQueryFilters().Where(BuildObjectLambda(properties, new ValueBuffer(array)))
			.Select(BuildProjection(entityType));
	}

	private IQueryable<TEntity> Query(INavigation navigation, object[] keyValues)
	{
		return _queryRoot.Where(BuildLambda(GetLoadProperties(navigation), new ValueBuffer(keyValues))).AsTracking();
	}

	IQueryable IEntityFinder.Query(INavigation navigation, InternalEntityEntry entry)
	{
		return Query(navigation, entry);
	}

	private static object[] GetLoadValues(INavigation navigation, InternalEntityEntry entry)
	{
		IReadOnlyList<IProperty> readOnlyList = (navigation.IsDependentToPrincipal() ? navigation.ForeignKey.Properties : navigation.ForeignKey.PrincipalKey.Properties);
		object[] array = new object[readOnlyList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			object obj = entry[readOnlyList[i]];
			if (obj == null)
			{
				return null;
			}
			array[i] = obj;
		}
		return array;
	}

	private static IReadOnlyList<IProperty> GetLoadProperties(INavigation navigation)
	{
		if (!navigation.IsDependentToPrincipal())
		{
			return navigation.ForeignKey.Properties;
		}
		return navigation.ForeignKey.PrincipalKey.Properties;
	}

	private TEntity FindTracked(object[] keyValues, out IReadOnlyList<IProperty> keyProperties)
	{
		IKey key = _model.FindEntityType(typeof(TEntity)).FindPrimaryKey();
		keyProperties = key.Properties;
		if (keyProperties.Count != keyValues.Length)
		{
			if (keyProperties.Count == 1)
			{
				throw new ArgumentException(CoreStrings.FindNotCompositeKey(typeof(TEntity).ShortDisplayName(), keyValues.Length));
			}
			throw new ArgumentException(CoreStrings.FindValueCountMismatch(typeof(TEntity).ShortDisplayName(), keyProperties.Count, keyValues.Length));
		}
		for (int i = 0; i < keyValues.Length; i++)
		{
			Type type = keyValues[i].GetType();
			Type clrType = keyProperties[i].ClrType;
			if (type != clrType.UnwrapNullableType())
			{
				throw new ArgumentException(CoreStrings.FindValueTypeMismatch(i, typeof(TEntity).ShortDisplayName(), type.ShortDisplayName(), clrType.ShortDisplayName()));
			}
		}
		return _stateManager.TryGetEntry(key, keyValues)?.Entity as TEntity;
	}

	private static Expression<Func<TEntity, bool>> BuildLambda(IReadOnlyList<IProperty> keyProperties, ValueBuffer keyValues)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TEntity), "e");
		return Expression.Lambda<Func<TEntity, bool>>(BuildPredicate(keyProperties, keyValues, parameterExpression), new ParameterExpression[1] { parameterExpression });
	}

	private static Expression<Func<object, bool>> BuildObjectLambda(IReadOnlyList<IProperty> keyProperties, ValueBuffer keyValues)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "e");
		return Expression.Lambda<Func<object, bool>>(BuildPredicate(keyProperties, keyValues, parameterExpression), new ParameterExpression[1] { parameterExpression });
	}

	private IQueryable BuildQueryRoot(IEntityType entityType)
	{
		IEntityType definingEntityType = entityType.DefiningEntityType;
		if (definingEntityType == null)
		{
			IForeignKey foreignKey = entityType.FindOwnership();
			if (foreignKey == null)
			{
				return (IQueryable)_setCache.GetOrAddSet(_setSource, entityType.ClrType);
			}
			return BuildQueryRoot(foreignKey.PrincipalEntityType, entityType, foreignKey.PrincipalToDependent.Name);
		}
		return BuildQueryRoot(definingEntityType, entityType, entityType.DefiningNavigationName);
	}

	private IQueryable BuildQueryRoot(IEntityType ownerOrDefiningEntityType, IEntityType entityType, string navigationName)
	{
		IQueryable queryable = BuildQueryRoot(ownerOrDefiningEntityType);
		return (IQueryable)_selectMethod.MakeGenericMethod(ownerOrDefiningEntityType.ClrType, entityType.ClrType).Invoke(null, new object[2] { queryable, navigationName });
	}

	private static IQueryable<TResult> Select<TSource, TResult>([JetBrains.Annotations.NotNull] IQueryable<TSource> source, [JetBrains.Annotations.NotNull] string propertyName) where TSource : class where TResult : class
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TSource), "e");
		return source.Select(Expression.Lambda<Func<TSource, TResult>>(Expression.MakeMemberAccess(parameterExpression, typeof(TSource).GetAnyProperty(propertyName)), new ParameterExpression[1] { parameterExpression }));
	}

	private static BinaryExpression BuildPredicate(IReadOnlyList<IProperty> keyProperties, ValueBuffer keyValues, ParameterExpression entityParameter)
	{
		ConstantExpression keyValuesConstant = Expression.Constant(keyValues);
		BinaryExpression binaryExpression = GenerateEqualExpression(keyProperties[0], 0);
		for (int j = 1; j < keyProperties.Count; j++)
		{
			binaryExpression = Expression.AndAlso(binaryExpression, GenerateEqualExpression(keyProperties[j], j));
		}
		return binaryExpression;
		BinaryExpression GenerateEqualExpression(IProperty property, int i)
		{
			return Expression.Equal(Expression.Call(EF.PropertyMethod.MakeGenericMethod(property.ClrType), entityParameter, Expression.Constant(property.Name, typeof(string))), Expression.Convert(Expression.Call(keyValuesConstant, ValueBuffer.GetValueMethod, Expression.Constant(i)), property.ClrType));
		}
	}

	private static Expression<Func<object, object[]>> BuildProjection(IEntityType entityType)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "e");
		List<Expression> list = new List<Expression>();
		foreach (IProperty property in entityType.GetProperties())
		{
			list.Add(Expression.Convert(Expression.Convert(Expression.Call(EF.PropertyMethod.MakeGenericMethod(property.ClrType), parameterExpression, Expression.Constant(property.Name, typeof(string))), property.ClrType), typeof(object)));
		}
		return Expression.Lambda<Func<object, object[]>>(Expression.NewArrayInit(typeof(object), list), new ParameterExpression[1] { parameterExpression });
	}
}
