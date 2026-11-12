using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IDependentKeyValueFactory<TKey>
{
	[JetBrains.Annotations.ContractAnnotation("=>true, key:notnull; =>false, key:null")]
	bool TryCreateFromBuffer(in ValueBuffer valueBuffer, out TKey key);

	[JetBrains.Annotations.ContractAnnotation("=>true, key:notnull; =>false, key:null")]
	bool TryCreateFromCurrentValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry, out TKey key);

	[JetBrains.Annotations.ContractAnnotation("=>true, key:notnull; =>false, key:null")]
	bool TryCreateFromPreStoreGeneratedCurrentValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry, out TKey key);

	[JetBrains.Annotations.ContractAnnotation("=>true, key:notnull; =>false, key:null")]
	bool TryCreateFromOriginalValues([JetBrains.Annotations.NotNull] InternalEntityEntry entry, out TKey key);

	[JetBrains.Annotations.ContractAnnotation("=>true, key:notnull; =>false, key:null")]
	bool TryCreateFromRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry entry, out TKey key);
}
