using System.Collections.Concurrent;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class KeyValueIndexFactorySource : IdentityMapFactoryFactoryBase, IKeyValueIndexFactorySource
{
	private readonly ConcurrentDictionary<IKey, IKeyValueIndexFactory> _factories = new ConcurrentDictionary<IKey, IKeyValueIndexFactory>();

	public virtual IKeyValueIndexFactory GetKeyValueIndexFactory(IKey key)
	{
		return _factories.GetOrAdd(key, Create);
	}

	public virtual IKeyValueIndexFactory Create([JetBrains.Annotations.NotNull] IKey key)
	{
		return (IKeyValueIndexFactory)typeof(KeyValueIndexFactorySource).GetTypeInfo().GetDeclaredMethod("CreateFactory").MakeGenericMethod(GetKeyType(key))
			.Invoke(null, new object[1] { key });
	}

	[JetBrains.Annotations.UsedImplicitly]
	private static IKeyValueIndexFactory CreateFactory<TKey>(IKey key)
	{
		return new KeyValueIndexFactory<TKey>(key.GetPrincipalKeyValueFactory<TKey>());
	}
}
