using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Update.Internal;

public class KeyValueIndexFactory<TKey> : IKeyValueIndexFactory
{
	private readonly IPrincipalKeyValueFactory<TKey> _principalKeyValueFactory;

	public KeyValueIndexFactory([JetBrains.Annotations.NotNull] IPrincipalKeyValueFactory<TKey> principalKeyValueFactory)
	{
		_principalKeyValueFactory = principalKeyValueFactory;
	}

	public virtual IKeyValueIndex CreatePrincipalKeyValue(InternalEntityEntry entry, IForeignKey foreignKey)
	{
		return new KeyValueIndex<TKey>(foreignKey, _principalKeyValueFactory.CreateFromCurrentValues(entry), _principalKeyValueFactory.EqualityComparer, fromOriginalValues: false);
	}

	public virtual IKeyValueIndex CreatePrincipalKeyValueFromOriginalValues(InternalEntityEntry entry, IForeignKey foreignKey)
	{
		return new KeyValueIndex<TKey>(foreignKey, _principalKeyValueFactory.CreateFromOriginalValues(entry), _principalKeyValueFactory.EqualityComparer, fromOriginalValues: true);
	}

	public virtual IKeyValueIndex CreateDependentKeyValue(InternalEntityEntry entry, IForeignKey foreignKey)
	{
		if (!foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromCurrentValues(entry, out var key))
		{
			return null;
		}
		return new KeyValueIndex<TKey>(foreignKey, key, _principalKeyValueFactory.EqualityComparer, fromOriginalValues: false);
	}

	public virtual IKeyValueIndex CreateDependentKeyValueFromOriginalValues(InternalEntityEntry entry, IForeignKey foreignKey)
	{
		if (!foreignKey.GetDependentKeyValueFactory<TKey>().TryCreateFromOriginalValues(entry, out var key))
		{
			return null;
		}
		return new KeyValueIndex<TKey>(foreignKey, key, _principalKeyValueFactory.EqualityComparer, fromOriginalValues: true);
	}
}
