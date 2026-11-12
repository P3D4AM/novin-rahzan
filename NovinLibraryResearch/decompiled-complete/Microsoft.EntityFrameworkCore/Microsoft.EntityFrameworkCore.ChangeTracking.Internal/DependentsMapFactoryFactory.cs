using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class DependentsMapFactoryFactory : IdentityMapFactoryFactoryBase
{
	public virtual Func<IDependentsMap> Create([JetBrains.Annotations.NotNull] IForeignKey foreignKey)
	{
		return (Func<IDependentsMap>)typeof(DependentsMapFactoryFactory).GetTypeInfo().GetDeclaredMethod("CreateFactory").MakeGenericMethod(GetKeyType(foreignKey.PrincipalKey))
			.Invoke(null, new object[1] { foreignKey });
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static Func<IDependentsMap> CreateFactory<TKey>(IForeignKey foreignKey)
	{
		IPrincipalKeyValueFactory<TKey> principalKeyValueFactory = foreignKey.PrincipalKey.GetPrincipalKeyValueFactory<TKey>();
		IDependentKeyValueFactory<TKey> dependentKeyValueFactory = foreignKey.GetDependentKeyValueFactory<TKey>();
		return () => new DependentsMap<TKey>(foreignKey, principalKeyValueFactory, dependentKeyValueFactory);
	}
}
