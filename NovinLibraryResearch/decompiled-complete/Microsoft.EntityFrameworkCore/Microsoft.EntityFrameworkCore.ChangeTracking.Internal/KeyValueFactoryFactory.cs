using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class KeyValueFactoryFactory
{
	public virtual IPrincipalKeyValueFactory<TKey> Create<TKey>([JetBrains.Annotations.NotNull] IKey key)
	{
		if (key.Properties.Count != 1)
		{
			return (IPrincipalKeyValueFactory<TKey>)CreateCompositeFactory(key);
		}
		return CreateSimpleFactory<TKey>(key);
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static SimplePrincipalKeyValueFactory<TKey> CreateSimpleFactory<TKey>(IKey key)
	{
		DependentKeyValueFactoryFactory dependentKeyValueFactoryFactory = new DependentKeyValueFactoryFactory();
		SimplePrincipalKeyValueFactory<TKey> principalKeyValueFactory = new SimplePrincipalKeyValueFactory<TKey>(key.Properties.Single());
		foreach (IForeignKey foreignKey in key.GetReferencingForeignKeys())
		{
			IDependentKeyValueFactory<TKey> dependentKeyValueFactory = dependentKeyValueFactoryFactory.CreateSimple<TKey>(foreignKey);
			SetFactories(foreignKey, dependentKeyValueFactory, () => new DependentsMap<TKey>(foreignKey, principalKeyValueFactory, dependentKeyValueFactory));
		}
		return principalKeyValueFactory;
	}

	private static CompositePrincipalKeyValueFactory CreateCompositeFactory(IKey key)
	{
		DependentKeyValueFactoryFactory dependentKeyValueFactoryFactory = new DependentKeyValueFactoryFactory();
		CompositePrincipalKeyValueFactory principalKeyValueFactory = new CompositePrincipalKeyValueFactory(key);
		foreach (IForeignKey foreignKey in key.GetReferencingForeignKeys())
		{
			IDependentKeyValueFactory<object[]> dependentKeyValueFactory = dependentKeyValueFactoryFactory.CreateComposite(foreignKey);
			SetFactories(foreignKey, dependentKeyValueFactory, () => new DependentsMap<object[]>(foreignKey, principalKeyValueFactory, dependentKeyValueFactory));
		}
		return principalKeyValueFactory;
	}

	private static void SetFactories(IForeignKey foreignKey, object dependentKeyValueFactory, Func<IDependentsMap> dependentsMapFactory)
	{
		ForeignKey foreignKey2 = foreignKey.AsForeignKey("SetFactories");
		foreignKey2.DependentKeyValueFactory = dependentKeyValueFactory;
		foreignKey2.DependentsMapFactory = dependentsMapFactory;
	}
}
