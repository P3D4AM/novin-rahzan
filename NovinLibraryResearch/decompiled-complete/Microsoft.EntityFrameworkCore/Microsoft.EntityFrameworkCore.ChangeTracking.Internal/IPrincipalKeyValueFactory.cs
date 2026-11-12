using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IPrincipalKeyValueFactory<TKey>
{
	IEqualityComparer<TKey> EqualityComparer { get; }

	object CreateFromKeyValues([JetBrains.Annotations.NotNull] object[] keyValues);

	object CreateFromBuffer(ValueBuffer valueBuffer);

	IProperty FindNullPropertyInKeyValues(object[] keyValues);

	IProperty FindNullPropertyInValueBuffer(ValueBuffer valueBuffer);

	TKey CreateFromCurrentValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	IProperty FindNullPropertyInCurrentValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	TKey CreateFromOriginalValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	TKey CreateFromRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry entry);
}
