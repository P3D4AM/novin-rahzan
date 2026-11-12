using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public sealed class PropertyAccessors
{
	public Delegate CurrentValueGetter { get; }

	public Delegate PreStoreGeneratedCurrentValueGetter { get; }

	public Delegate OriginalValueGetter { get; }

	public Delegate RelationshipSnapshotGetter { get; }

	public Func<ValueBuffer, object> ValueBufferGetter { get; }

	public PropertyAccessors([JetBrains.Annotations.NotNull] Delegate currentValueGetter, [JetBrains.Annotations.NotNull] Delegate preStoreGeneratedCurrentValueGetter, [JetBrains.Annotations.CanBeNull] Delegate originalValueGetter, [JetBrains.Annotations.NotNull] Delegate relationshipSnapshotGetter, [JetBrains.Annotations.CanBeNull] Func<ValueBuffer, object> valueBufferGetter)
	{
		CurrentValueGetter = currentValueGetter;
		PreStoreGeneratedCurrentValueGetter = preStoreGeneratedCurrentValueGetter;
		OriginalValueGetter = originalValueGetter;
		RelationshipSnapshotGetter = relationshipSnapshotGetter;
		ValueBufferGetter = valueBufferGetter;
	}
}
