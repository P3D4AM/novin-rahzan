using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class NullableKeyIdentityMap<TKey> : IdentityMap<TKey>
{
	public NullableKeyIdentityMap([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] IPrincipalKeyValueFactory<TKey> principalKeyValueFactory, bool sensitiveLoggingEnabled)
		: base(key, principalKeyValueFactory, sensitiveLoggingEnabled)
	{
	}

	public override void Add(InternalEntityEntry entry)
	{
		TKey val = PrincipalKeyValueFactory.CreateFromCurrentValues(entry);
		if (val == null)
		{
			if (Key.IsPrimaryKey())
			{
				throw new InvalidOperationException(CoreStrings.InvalidKeyValue(entry.EntityType.DisplayName(), PrincipalKeyValueFactory.FindNullPropertyInCurrentValues(entry).Name));
			}
			throw new InvalidOperationException(CoreStrings.InvalidAlternateKeyValue(entry.EntityType.DisplayName(), PrincipalKeyValueFactory.FindNullPropertyInCurrentValues(entry).Name));
		}
		Add(val, entry);
	}

	public override void RemoveUsingRelationshipSnapshot(InternalEntityEntry entry)
	{
		TKey val = PrincipalKeyValueFactory.CreateFromRelationshipSnapshot(entry);
		if (val != null)
		{
			Remove(val, entry);
		}
	}
}
