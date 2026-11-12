using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IDependentsMap
{
	void Add([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Update([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void Remove([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	IEnumerable<InternalEntityEntry> GetDependents([JetBrains.Annotations.NotNull] InternalEntityEntry principalEntry);

	IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry principalEntry);
}
