using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class IdentityMapFactoryFactory : IdentityMapFactoryFactoryBase
{
	public virtual Func<bool, IIdentityMap> Create([JetBrains.Annotations.NotNull] IKey key)
	{
		return (Func<bool, IIdentityMap>)typeof(IdentityMapFactoryFactory).GetTypeInfo().GetDeclaredMethod("CreateFactory").MakeGenericMethod(GetKeyType(key))
			.Invoke(null, new object[1] { key });
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static Func<bool, IIdentityMap> CreateFactory<TKey>(IKey key)
	{
		IPrincipalKeyValueFactory<TKey> factory = key.GetPrincipalKeyValueFactory<TKey>();
		if (!typeof(TKey).IsNullableType())
		{
			return (bool sensitiveLoggingEnabled) => new IdentityMap<TKey>(key, factory, sensitiveLoggingEnabled);
		}
		return (bool sensitiveLoggingEnabled) => new NullableKeyIdentityMap<TKey>(key, factory, sensitiveLoggingEnabled);
	}
}
