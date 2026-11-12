using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public abstract class SnapshotFactoryFactory
{
	private static readonly MethodInfo _snapshotCollectionMethod = typeof(SnapshotFactoryFactory).GetTypeInfo().GetDeclaredMethod("SnapshotCollection");

	protected virtual bool UseEntityVariable => true;

	public virtual Func<ISnapshot> CreateEmpty([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (GetPropertyCount(entityType) != 0)
		{
			return Expression.Lambda<Func<ISnapshot>>(CreateConstructorExpression(entityType, null), Array.Empty<ParameterExpression>()).Compile();
		}
		return () => Snapshot.Empty;
	}

	protected virtual Expression CreateConstructorExpression([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.CanBeNull] ParameterExpression parameter)
	{
		int propertyCount = GetPropertyCount(entityType);
		Type[] array = new Type[propertyCount];
		IPropertyBase[] array2 = new IPropertyBase[propertyCount];
		foreach (IPropertyBase propertiesAndNavigation in entityType.GetPropertiesAndNavigations())
		{
			int propertyIndex = GetPropertyIndex(propertiesAndNavigation);
			if (propertyIndex >= 0)
			{
				array[propertyIndex] = (propertiesAndNavigation as IProperty)?.ClrType ?? typeof(object);
				array2[propertyIndex] = propertiesAndNavigation;
			}
		}
		if (propertyCount > 30)
		{
			List<Expression> list = new List<Expression>();
			for (int i = 0; i < propertyCount; i += 30)
			{
				list.Add(CreateSnapshotExpression(entityType.ClrType, parameter, array.Skip(i).Take(30).ToArray(), array2.Skip(i).Take(30).ToList()));
			}
			return Expression.Convert(Expression.New(MultiSnapshot.Constructor, Expression.NewArrayInit(typeof(ISnapshot), list)), typeof(ISnapshot));
		}
		return CreateSnapshotExpression(entityType.ClrType, parameter, array, array2);
	}

	protected virtual Expression CreateSnapshotExpression(Type entityType, ParameterExpression parameter, Type[] types, IList<IPropertyBase> propertyBases)
	{
		int num = types.Length;
		Expression[] array = new Expression[num];
		ParameterExpression parameterExpression = ((entityType == null) ? null : Expression.Variable(entityType, "entity"));
		for (int i = 0; i < num; i++)
		{
			IPropertyBase propertyBase = propertyBases[i];
			if (propertyBase == null)
			{
				array[i] = Expression.Constant(null);
				types[i] = typeof(object);
				continue;
			}
			if (propertyBase is IProperty property && property.GetStoreGeneratedIndex() != -1)
			{
				array[i] = CreateReadValueExpression(parameter, property);
				continue;
			}
			if (propertyBase.IsShadowProperty())
			{
				array[i] = CreateSnapshotValueExpression(CreateReadShadowValueExpression(parameter, propertyBase), propertyBase);
				continue;
			}
			Expression expression = Expression.MakeMemberAccess(parameterExpression, propertyBase.GetMemberInfo(forMaterialization: false, forSet: false));
			if (expression.Type != propertyBase.ClrType)
			{
				expression = Expression.Convert(expression, propertyBase.ClrType);
			}
			int num2 = i;
			INavigation obj = propertyBase as INavigation;
			array[num2] = ((obj != null && obj.IsCollection()) ? Expression.Call(null, _snapshotCollectionMethod, expression) : CreateSnapshotValueExpression(expression, propertyBase));
		}
		UnaryExpression unaryExpression = Expression.Convert(Expression.New(Snapshot.CreateSnapshotType(types).GetDeclaredConstructor(types), array), typeof(ISnapshot));
		if (!UseEntityVariable || parameterExpression == null)
		{
			return unaryExpression;
		}
		return Expression.Block(new List<ParameterExpression> { parameterExpression }, new List<Expression>
		{
			Expression.Assign(parameterExpression, Expression.Convert(Expression.Property(parameter, "Entity"), entityType)),
			unaryExpression
		});
	}

	private Expression CreateSnapshotValueExpression(Expression expression, IPropertyBase propertyBase)
	{
		if (propertyBase is IProperty property)
		{
			ValueComparer valueComparer = GetValueComparer(property);
			if (valueComparer != null)
			{
				Expression expression2 = ReplacingExpressionVisitor.Replace(valueComparer.SnapshotExpression.Parameters.Single(), expression, valueComparer.SnapshotExpression.Body);
				expression = (propertyBase.ClrType.IsNullableType() ? Expression.Condition(Expression.Equal(expression, Expression.Constant(null, propertyBase.ClrType)), Expression.Constant(null, propertyBase.ClrType), expression2) : expression2);
			}
		}
		return expression;
	}

	protected abstract ValueComparer GetValueComparer([JetBrains.Annotations.NotNull] IProperty property);

	protected virtual Expression CreateReadShadowValueExpression([JetBrains.Annotations.CanBeNull] ParameterExpression parameter, [JetBrains.Annotations.NotNull] IPropertyBase property)
	{
		return Expression.Call(parameter, InternalEntityEntry.ReadShadowValueMethod.MakeGenericMethod(property.ClrType), Expression.Constant(property.GetShadowIndex()));
	}

	protected virtual Expression CreateReadValueExpression([JetBrains.Annotations.CanBeNull] ParameterExpression parameter, [JetBrains.Annotations.NotNull] IPropertyBase property)
	{
		return Expression.Call(parameter, InternalEntityEntry.GetCurrentValueMethod.MakeGenericMethod(property.ClrType), Expression.Constant(property, typeof(IProperty)));
	}

	protected abstract int GetPropertyIndex([JetBrains.Annotations.NotNull] IPropertyBase propertyBase);

	protected abstract int GetPropertyCount([JetBrains.Annotations.NotNull] IEntityType entityType);

	[JetBrains.Annotations.UsedImplicitly]
	private static HashSet<object> SnapshotCollection(IEnumerable<object> collection)
	{
		if (collection != null)
		{
			return new HashSet<object>(collection, Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
		}
		return null;
	}
}
public abstract class SnapshotFactoryFactory<TInput> : SnapshotFactoryFactory
{
	public virtual Func<TInput, ISnapshot> Create([JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (GetPropertyCount(entityType) == 0)
		{
			return (TInput e) => Snapshot.Empty;
		}
		ParameterExpression parameterExpression = Expression.Parameter(typeof(TInput), "source");
		return Expression.Lambda<Func<TInput, ISnapshot>>(CreateConstructorExpression(entityType, parameterExpression), new ParameterExpression[1] { parameterExpression }).Compile();
	}
}
