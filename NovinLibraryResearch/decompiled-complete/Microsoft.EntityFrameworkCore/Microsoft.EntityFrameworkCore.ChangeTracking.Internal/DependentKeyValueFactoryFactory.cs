using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class DependentKeyValueFactoryFactory
{
	public virtual IDependentKeyValueFactory<TKey> Create<TKey>([JetBrains.Annotations.NotNull] IForeignKey foreignKey)
	{
		if (foreignKey.Properties.Count != 1)
		{
			return (IDependentKeyValueFactory<TKey>)CreateComposite(foreignKey);
		}
		return CreateSimple<TKey>(foreignKey);
	}

	public virtual IDependentKeyValueFactory<TKey> CreateSimple<TKey>([JetBrains.Annotations.NotNull] IForeignKey foreignKey)
	{
		IProperty property = foreignKey.Properties.Single();
		Type clrType = foreignKey.PrincipalKey.Properties.Single().ClrType;
		PropertyAccessors propertyAccessors = property.GetPropertyAccessors();
		if (property.ClrType.IsNullableType() && clrType.IsNullableType())
		{
			return new SimpleFullyNullableDependentKeyValueFactory<TKey>(propertyAccessors);
		}
		if (property.ClrType.IsNullableType())
		{
			return (IDependentKeyValueFactory<TKey>)Activator.CreateInstance(typeof(SimpleNullableDependentKeyValueFactory<>).MakeGenericType(typeof(TKey)), propertyAccessors);
		}
		if (!clrType.IsNullableType())
		{
			return new SimpleNonNullableDependentKeyValueFactory<TKey>(propertyAccessors);
		}
		return (IDependentKeyValueFactory<TKey>)Activator.CreateInstance(typeof(SimpleNullablePrincipalDependentKeyValueFactory<, >).MakeGenericType(typeof(TKey), typeof(TKey).UnwrapNullableType()), propertyAccessors);
	}

	public virtual IDependentKeyValueFactory<object[]> CreateComposite([JetBrains.Annotations.NotNull] IForeignKey foreignKey)
	{
		return new CompositeValueFactory(foreignKey.Properties);
	}
}
