using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IStateManager : IResettableService
{
	StateManagerDependencies Dependencies { get; }

	CascadeTiming DeleteOrphansTiming { get; set; }

	CascadeTiming CascadeDeleteTiming { get; set; }

	bool SavingChanges { get; }

	IEnumerable<InternalEntityEntry> Entries { get; }

	int Count { get; }

	int ChangedCount { get; set; }

	IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }

	IValueGenerationManager ValueGenerationManager { get; }

	DbContext Context { get; }

	IModel Model { get; }

	IEntityMaterializerSource EntityMaterializerSource { get; }

	bool SensitiveLoggingEnabled { get; }

	IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	event EventHandler<EntityTrackedEventArgs> Tracked;

	event EventHandler<EntityStateChangedEventArgs> StateChanged;

	InternalEntityEntry GetOrCreateEntry([JetBrains.Annotations.NotNull] object entity);

	InternalEntityEntry GetOrCreateEntry([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] IEntityType entityType);

	InternalEntityEntry CreateEntry([JetBrains.Annotations.NotNull] IDictionary<string, object> values, [JetBrains.Annotations.NotNull] IEntityType entityType);

	InternalEntityEntry StartTrackingFromQuery([JetBrains.Annotations.NotNull] IEntityType baseEntityType, [JetBrains.Annotations.NotNull] object entity, in ValueBuffer valueBuffer);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] object[] keyValues);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] IKey key, object[] keyValues, bool throwOnNullKey, out bool hasNullKey);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] object entity, bool throwOnNonUniqueness = true);

	InternalEntityEntry TryGetEntry([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] IEntityType type, bool throwOnTypeMismatch = true);

	IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false);

	int GetCountForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false);

	IEnumerable<TEntity> GetNonDeletedEntities<TEntity>() where TEntity : class;

	void StateChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState newState);

	InternalEntityEntry StartTracking([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void StopTracking([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState oldState);

	void RecordReferencedUntrackedEntity([JetBrains.Annotations.NotNull] object referencedEntity, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] InternalEntityEntry referencedFromEntry);

	IEnumerable<Tuple<INavigation, InternalEntityEntry>> GetRecordedReferrers([JetBrains.Annotations.NotNull] object referencedEntity, bool clear);

	InternalEntityEntry FindPrincipal([JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	InternalEntityEntry FindPrincipalUsingPreStoreGeneratedValues([JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	InternalEntityEntry FindPrincipalUsingRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry dependentEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	void UpdateIdentityMap([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IKey principalKey);

	void UpdateDependentMap([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<InternalEntityEntry> GetDependentsFromNavigation([JetBrains.Annotations.NotNull] InternalEntityEntry principalEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<InternalEntityEntry> GetDependents([JetBrains.Annotations.NotNull] InternalEntityEntry principalEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot([JetBrains.Annotations.NotNull] InternalEntityEntry principalEntry, [JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IList<IUpdateEntry> GetEntriesToSave(bool cascadeChanges);

	int SaveChanges(bool acceptAllChangesOnSuccess);

	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

	void AcceptAllChanges();

	IEntityFinder CreateEntityFinder([JetBrains.Annotations.NotNull] IEntityType entityType);

	void Unsubscribe();

	void OnTracked([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, bool fromQuery);

	void OnStateChanged([JetBrains.Annotations.NotNull] InternalEntityEntry internalEntityEntry, EntityState oldState);

	void CascadeChanges(bool force);

	void CascadeDelete([JetBrains.Annotations.NotNull] InternalEntityEntry entry, bool force, IEnumerable<IForeignKey> foreignKeys = null);
}
