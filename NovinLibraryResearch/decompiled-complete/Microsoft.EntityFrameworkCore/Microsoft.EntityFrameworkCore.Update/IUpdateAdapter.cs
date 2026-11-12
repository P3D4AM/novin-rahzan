using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Update;

public interface IUpdateAdapter
{
	CascadeTiming DeleteOrphansTiming { get; set; }

	CascadeTiming CascadeDeleteTiming { get; set; }

	IEnumerable<IUpdateEntry> Entries { get; }

	IModel Model { get; }

	IUpdateEntry FindPrincipal([JetBrains.Annotations.NotNull] IUpdateEntry dependentEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<IUpdateEntry> GetDependents([JetBrains.Annotations.NotNull] IUpdateEntry principalEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IUpdateEntry TryGetEntry([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] object[] keyValues);

	void DetectChanges();

	void CascadeChanges();

	void CascadeDelete([JetBrains.Annotations.NotNull] IUpdateEntry entry, [JetBrains.Annotations.CanBeNull] IEnumerable<IForeignKey> foreignKeys = null);

	IList<IUpdateEntry> GetEntriesToSave();

	IUpdateEntry CreateEntry([JetBrains.Annotations.NotNull] IDictionary<string, object> values, [JetBrains.Annotations.NotNull] IEntityType entityType);
}
