using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IIdentityMap
{
	IKey Key { get; }

	bool Contains(in ValueBuffer valueBuffer);

	bool Contains([JetBrains.Annotations.NotNull] IForeignKey foreignKey, in ValueBuffer valueBuffer);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] object[] keyValues);

	InternalEntityEntry TryGetEntry(object[] keyValues, bool throwOnNullKey, out bool hasNullKey);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry);

	InternalEntityEntry TryGetEntryUsingPreStoreGeneratedValues([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry);

	InternalEntityEntry TryGetEntryUsingRelationshipSnapshot([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry);

	void AddOrUpdate([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Add([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Add([JetBrains.Annotations.NotNull] object[] keyValues, [JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Remove([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void RemoveUsingRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	IDependentsMap GetDependentsMap([JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IDependentsMap FindDependentsMap([JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	void Clear();
}
