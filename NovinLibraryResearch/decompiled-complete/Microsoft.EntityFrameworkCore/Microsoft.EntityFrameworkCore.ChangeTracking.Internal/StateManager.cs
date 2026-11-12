using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class StateManager : IStateManager, IResettableService
{
	private readonly EntityReferenceMap _entityReferenceMap = new EntityReferenceMap(hasSubMap: true);

	private IDictionary<object, IList<Tuple<INavigation, InternalEntityEntry>>> _referencedUntrackedEntities;

	private IIdentityMap _identityMap0;

	private IIdentityMap _identityMap1;

	private Dictionary<IKey, IIdentityMap> _identityMaps;

	private bool _needsUnsubscribe;

	private readonly IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> _changeTrackingLogger;

	private readonly IInternalEntityEntryFactory _internalEntityEntryFactory;

	private readonly IInternalEntityEntrySubscriber _internalEntityEntrySubscriber;

	private readonly IModel _model;

	private readonly IDatabase _database;

	private readonly IConcurrencyDetector _concurrencyDetector;

	public virtual StateManagerDependencies Dependencies { get; }

	public virtual bool SensitiveLoggingEnabled { get; }

	public virtual IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	public virtual CascadeTiming DeleteOrphansTiming { get; set; }

	public virtual CascadeTiming CascadeDeleteTiming { get; set; }

	public virtual bool SavingChanges { get; set; }

	public virtual IInternalEntityEntryNotifier InternalEntityEntryNotifier { get; }

	public virtual IValueGenerationManager ValueGenerationManager { get; }

	public virtual DbContext Context { get; }

	public virtual IModel Model => _model;

	public virtual IEntityFinderFactory EntityFinderFactory { get; }

	public virtual IEntityMaterializerSource EntityMaterializerSource { get; }

	public virtual int Count => GetCountForState(added: true, modified: true, deleted: true, unchanged: true);

	public virtual IEnumerable<InternalEntityEntry> Entries => GetEntriesForState(added: true, modified: true, deleted: true, unchanged: true);

	public virtual int ChangedCount { get; set; }

	public event EventHandler<EntityTrackedEventArgs> Tracked;

	public event EventHandler<EntityStateChangedEventArgs> StateChanged;

	public StateManager([JetBrains.Annotations.NotNull] StateManagerDependencies dependencies)
	{
		Dependencies = dependencies;
		_internalEntityEntryFactory = dependencies.InternalEntityEntryFactory;
		_internalEntityEntrySubscriber = dependencies.InternalEntityEntrySubscriber;
		InternalEntityEntryNotifier = dependencies.InternalEntityEntryNotifier;
		ValueGenerationManager = dependencies.ValueGenerationManager;
		_model = dependencies.Model;
		_database = dependencies.Database;
		_concurrencyDetector = dependencies.ConcurrencyDetector;
		Context = dependencies.CurrentContext.Context;
		EntityFinderFactory = new EntityFinderFactory(dependencies.EntityFinderSource, this, dependencies.SetSource, dependencies.CurrentContext.Context);
		EntityMaterializerSource = dependencies.EntityMaterializerSource;
		if (dependencies.LoggingOptions.IsSensitiveDataLoggingEnabled)
		{
			SensitiveLoggingEnabled = true;
		}
		UpdateLogger = dependencies.UpdateLogger;
		_changeTrackingLogger = dependencies.ChangeTrackingLogger;
	}

	public virtual void StateChanging(InternalEntityEntry entry, EntityState newState)
	{
		InternalEntityEntryNotifier.StateChanging(entry, newState);
		UpdateReferenceMaps(entry, newState, entry.EntityState);
	}

	public virtual InternalEntityEntry GetOrCreateEntry(object entity)
	{
		InternalEntityEntry internalEntityEntry = TryGetEntry(entity);
		if (internalEntityEntry == null)
		{
			IEntityType entityType = _model.FindRuntimeEntityType(entity.GetType());
			if (entityType == null)
			{
				if (_model.HasEntityTypeWithDefiningNavigation(entity.GetType()))
				{
					throw new InvalidOperationException(CoreStrings.UntrackedDependentEntity(entity.GetType().ShortDisplayName(), ".Reference().TargetEntry", ".Collection().FindEntry()"));
				}
				throw new InvalidOperationException(CoreStrings.EntityTypeNotFound(entity.GetType().ShortDisplayName()));
			}
			if (entityType.FindPrimaryKey() == null)
			{
				throw new InvalidOperationException(CoreStrings.KeylessTypeTracked(entityType.DisplayName()));
			}
			internalEntityEntry = _internalEntityEntryFactory.Create(this, entityType, entity);
			UpdateReferenceMaps(internalEntityEntry, EntityState.Detached, null);
		}
		return internalEntityEntry;
	}

	public virtual InternalEntityEntry GetOrCreateEntry(object entity, IEntityType entityType)
	{
		InternalEntityEntry internalEntityEntry = TryGetEntry(entity, entityType);
		if (internalEntityEntry == null)
		{
			IEntityType entityType2 = _model.FindRuntimeEntityType(entity.GetType());
			if (entityType2 != null)
			{
				if (!entityType.IsAssignableFrom(entityType2))
				{
					throw new InvalidOperationException(CoreStrings.TrackingTypeMismatch(entityType2.DisplayName(), entityType.DisplayName()));
				}
				entityType = entityType2;
			}
			if (entityType.FindPrimaryKey() == null)
			{
				throw new InvalidOperationException(CoreStrings.KeylessTypeTracked(entityType.DisplayName()));
			}
			internalEntityEntry = _internalEntityEntryFactory.Create(this, entityType, entity);
			UpdateReferenceMaps(internalEntityEntry, EntityState.Detached, null);
		}
		return internalEntityEntry;
	}

	public virtual InternalEntityEntry CreateEntry(IDictionary<string, object> values, IEntityType entityType)
	{
		int num = 0;
		object[] array = new object[entityType.PropertyCount()];
		object[] array2 = new object[entityType.ShadowPropertyCount()];
		foreach (IProperty property in entityType.GetProperties())
		{
			array[num++] = (values.TryGetValue(property.Name, out var value) ? value : property.ClrType.GetDefaultValue());
			if (property.IsShadowProperty())
			{
				array2[property.GetShadowIndex()] = (values.TryGetValue(property.Name, out var value2) ? value2 : property.ClrType.GetDefaultValue());
			}
		}
		ValueBuffer valueBuffer = new ValueBuffer(array);
		object entity = (entityType.HasClrType() ? EntityMaterializerSource.GetMaterializer(entityType)(new MaterializationContext(in valueBuffer, Context)) : null);
		ValueBuffer valueBuffer2 = new ValueBuffer(array2);
		InternalEntityEntry internalEntityEntry = _internalEntityEntryFactory.Create(this, entityType, entity, in valueBuffer2);
		UpdateReferenceMaps(internalEntityEntry, EntityState.Detached, null);
		return internalEntityEntry;
	}

	private void UpdateReferenceMaps(InternalEntityEntry entry, EntityState state, EntityState? oldState)
	{
		IEntityType entityType = entry.EntityType;
		object mapKey = entry.Entity ?? entry;
		if (entityType.HasDefiningNavigation())
		{
			foreach (IEntityType item in from et in _model.GetEntityTypes(entityType.Name)
				where et != entityType && TryGetEntry(mapKey, et) != null
				select et)
			{
				UpdateLogger.DuplicateDependentEntityTypeInstanceWarning(entityType, item);
			}
		}
		_entityReferenceMap.Update(entry, state, oldState);
	}

	public virtual InternalEntityEntry StartTrackingFromQuery(IEntityType baseEntityType, object entity, in ValueBuffer valueBuffer)
	{
		InternalEntityEntry internalEntityEntry = TryGetEntry(entity);
		if (internalEntityEntry != null)
		{
			return internalEntityEntry;
		}
		Type type = entity.GetType();
		IEntityType entityType = ((baseEntityType.ClrType == type || baseEntityType.HasDefiningNavigation()) ? baseEntityType : _model.FindRuntimeEntityType(type));
		InternalEntityEntry internalEntityEntry2 = (valueBuffer.IsEmpty ? _internalEntityEntryFactory.Create(this, entityType, entity) : _internalEntityEntryFactory.Create(this, entityType, entity, in valueBuffer));
		foreach (IKey key in baseEntityType.GetKeys())
		{
			GetOrCreateIdentityMap(key).AddOrUpdate(internalEntityEntry2);
		}
		UpdateReferenceMaps(internalEntityEntry2, EntityState.Unchanged, null);
		internalEntityEntry2.MarkUnchangedFromQuery();
		if (_internalEntityEntrySubscriber.SnapshotAndSubscribe(internalEntityEntry2))
		{
			_needsUnsubscribe = true;
		}
		return internalEntityEntry2;
	}

	public virtual InternalEntityEntry TryGetEntry(IKey key, object[] keyValues)
	{
		return FindIdentityMap(key)?.TryGetEntry(keyValues);
	}

	public virtual InternalEntityEntry TryGetEntry(IKey key, object[] keyValues, bool throwOnNullKey, out bool hasNullKey)
	{
		return GetOrCreateIdentityMap(key).TryGetEntry(keyValues, throwOnNullKey, out hasNullKey);
	}

	public virtual InternalEntityEntry TryGetEntry(object entity, bool throwOnNonUniqueness = true)
	{
		if (!_entityReferenceMap.TryGet(entity, null, out var entry, throwOnNonUniqueness))
		{
			return null;
		}
		return entry;
	}

	public virtual InternalEntityEntry TryGetEntry(object entity, IEntityType entityType, bool throwOnTypeMismatch = true)
	{
		InternalEntityEntry entry;
		bool flag = _entityReferenceMap.TryGet(entity, entityType, out entry, throwOnNonUniqueness: false);
		if (flag && !entityType.IsAssignableFrom(entry.EntityType))
		{
			if (throwOnTypeMismatch)
			{
				throw new InvalidOperationException(CoreStrings.TrackingTypeMismatch(entry.EntityType.DisplayName(), entityType.DisplayName()));
			}
			return null;
		}
		if (!flag)
		{
			return null;
		}
		return entry;
	}

	private IIdentityMap GetOrCreateIdentityMap(IKey key)
	{
		if (_identityMap0 == null)
		{
			_identityMap0 = key.GetIdentityMapFactory()(SensitiveLoggingEnabled);
			return _identityMap0;
		}
		if (_identityMap0.Key == key)
		{
			return _identityMap0;
		}
		if (_identityMap1 == null)
		{
			_identityMap1 = key.GetIdentityMapFactory()(SensitiveLoggingEnabled);
			return _identityMap1;
		}
		if (_identityMap1.Key == key)
		{
			return _identityMap1;
		}
		if (_identityMaps == null)
		{
			_identityMaps = new Dictionary<IKey, IIdentityMap>();
		}
		if (!_identityMaps.TryGetValue(key, out var value))
		{
			value = key.GetIdentityMapFactory()(SensitiveLoggingEnabled);
			_identityMaps[key] = value;
		}
		return value;
	}

	private IIdentityMap FindIdentityMap(IKey key)
	{
		if (_identityMap0 == null || key == null)
		{
			return null;
		}
		if (_identityMap0.Key == key)
		{
			return _identityMap0;
		}
		if (_identityMap1 == null)
		{
			return null;
		}
		if (_identityMap1.Key == key)
		{
			return _identityMap1;
		}
		if (_identityMaps != null && _identityMaps.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	public virtual int GetCountForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
	{
		return _entityReferenceMap.GetCountForState(added, modified, deleted, unchanged);
	}

	public virtual IEnumerable<InternalEntityEntry> GetEntriesForState(bool added = false, bool modified = false, bool deleted = false, bool unchanged = false)
	{
		return _entityReferenceMap.GetEntriesForState(added, modified, deleted, unchanged);
	}

	public virtual IEnumerable<TEntity> GetNonDeletedEntities<TEntity>() where TEntity : class
	{
		return _entityReferenceMap.GetNonDeletedEntities<TEntity>();
	}

	public virtual InternalEntityEntry StartTracking(InternalEntityEntry entry)
	{
		EntityType entityType = (EntityType)entry.EntityType;
		if (entry.StateManager != this)
		{
			throw new InvalidOperationException(CoreStrings.WrongStateManager(entityType.DisplayName()));
		}
		object entity = entry.Entity ?? entry;
		InternalEntityEntry internalEntityEntry = TryGetEntry(entity, entityType);
		if (internalEntityEntry != null && internalEntityEntry != entry)
		{
			throw new InvalidOperationException(CoreStrings.MultipleEntries(entityType.DisplayName()));
		}
		foreach (Key key in entityType.GetKeys())
		{
			GetOrCreateIdentityMap(key).Add(entry);
		}
		if (_internalEntityEntrySubscriber.SnapshotAndSubscribe(entry))
		{
			_needsUnsubscribe = true;
		}
		return entry;
	}

	public virtual void StopTracking(InternalEntityEntry entry, EntityState oldState)
	{
		if (_needsUnsubscribe)
		{
			_internalEntityEntrySubscriber.Unsubscribe(entry);
		}
		foreach (IKey key in entry.EntityType.GetKeys())
		{
			FindIdentityMap(key)?.Remove(entry);
		}
		if (_referencedUntrackedEntities == null)
		{
			return;
		}
		foreach (KeyValuePair<object, IList<Tuple<INavigation, InternalEntityEntry>>> item in _referencedUntrackedEntities.ToList())
		{
			if (item.Value.Any((Tuple<INavigation, InternalEntityEntry> t) => t.Item2 == entry))
			{
				_referencedUntrackedEntities.Remove(item.Key);
				List<Tuple<INavigation, InternalEntityEntry>> list = item.Value.Where((Tuple<INavigation, InternalEntityEntry> tuple) => tuple.Item2 != entry).ToList();
				if (list.Count > 0)
				{
					_referencedUntrackedEntities.Add(item.Key, list);
				}
			}
		}
	}

	public virtual void Unsubscribe()
	{
		if (!_needsUnsubscribe)
		{
			return;
		}
		foreach (InternalEntityEntry entry in Entries)
		{
			_internalEntityEntrySubscriber.Unsubscribe(entry);
		}
	}

	public virtual void ResetState()
	{
		Unsubscribe();
		ChangedCount = 0;
		_entityReferenceMap.Clear();
		_referencedUntrackedEntities = null;
		_identityMaps?.Clear();
		_identityMap0?.Clear();
		_identityMap1?.Clear();
		_needsUnsubscribe = false;
		this.Tracked = null;
		this.StateChanged = null;
		SavingChanges = false;
	}

	public virtual Task ResetStateAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		ResetState();
		return null;
	}

	public virtual void RecordReferencedUntrackedEntity(object referencedEntity, INavigation navigation, InternalEntityEntry referencedFromEntry)
	{
		if (_referencedUntrackedEntities == null)
		{
			_referencedUntrackedEntities = new Dictionary<object, IList<Tuple<INavigation, InternalEntityEntry>>>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
		}
		if (!_referencedUntrackedEntities.TryGetValue(referencedEntity, out var value))
		{
			value = new List<Tuple<INavigation, InternalEntityEntry>>();
			_referencedUntrackedEntities.Add(referencedEntity, value);
		}
		value.Add(Tuple.Create(navigation, referencedFromEntry));
	}

	public virtual IEnumerable<Tuple<INavigation, InternalEntityEntry>> GetRecordedReferrers(object referencedEntity, bool clear)
	{
		if (_referencedUntrackedEntities != null && _referencedUntrackedEntities.TryGetValue(referencedEntity, out var value))
		{
			if (clear)
			{
				_referencedUntrackedEntities.Remove(referencedEntity);
			}
			return value;
		}
		return Enumerable.Empty<Tuple<INavigation, InternalEntityEntry>>();
	}

	public virtual InternalEntityEntry FindPrincipal(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
	{
		return FilterIncompatiblePrincipal(foreignKey, FindIdentityMap(foreignKey.PrincipalKey)?.TryGetEntry(foreignKey, dependentEntry));
	}

	public virtual InternalEntityEntry FindPrincipalUsingPreStoreGeneratedValues(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
	{
		return FilterIncompatiblePrincipal(foreignKey, FindIdentityMap(foreignKey.PrincipalKey)?.TryGetEntryUsingPreStoreGeneratedValues(foreignKey, dependentEntry));
	}

	public virtual InternalEntityEntry FindPrincipalUsingRelationshipSnapshot(InternalEntityEntry dependentEntry, IForeignKey foreignKey)
	{
		return FilterIncompatiblePrincipal(foreignKey, FindIdentityMap(foreignKey.PrincipalKey)?.TryGetEntryUsingRelationshipSnapshot(foreignKey, dependentEntry));
	}

	private static InternalEntityEntry FilterIncompatiblePrincipal(IForeignKey foreignKey, InternalEntityEntry principalEntry)
	{
		if (principalEntry == null || !foreignKey.PrincipalEntityType.IsAssignableFrom(principalEntry.EntityType))
		{
			return null;
		}
		return principalEntry;
	}

	public virtual void UpdateIdentityMap(InternalEntityEntry entry, IKey key)
	{
		if (entry.EntityState != 0)
		{
			IIdentityMap identityMap = FindIdentityMap(key);
			if (identityMap != null)
			{
				identityMap.RemoveUsingRelationshipSnapshot(entry);
				identityMap.Add(entry);
			}
		}
	}

	public virtual void UpdateDependentMap(InternalEntityEntry entry, IForeignKey foreignKey)
	{
		if (entry.EntityState != 0)
		{
			FindIdentityMap(foreignKey.DeclaringEntityType.FindPrimaryKey())?.FindDependentsMap(foreignKey)?.Update(entry);
		}
	}

	public virtual IEnumerable<InternalEntityEntry> GetDependents(InternalEntityEntry principalEntry, IForeignKey foreignKey)
	{
		IIdentityMap identityMap = FindIdentityMap(foreignKey.DeclaringEntityType.FindPrimaryKey());
		if (identityMap == null)
		{
			return Enumerable.Empty<InternalEntityEntry>();
		}
		return identityMap.GetDependentsMap(foreignKey).GetDependents(principalEntry);
	}

	public virtual IEnumerable<InternalEntityEntry> GetDependentsUsingRelationshipSnapshot(InternalEntityEntry principalEntry, IForeignKey foreignKey)
	{
		IIdentityMap identityMap = FindIdentityMap(foreignKey.DeclaringEntityType.FindPrimaryKey());
		if (identityMap == null)
		{
			return Enumerable.Empty<InternalEntityEntry>();
		}
		return identityMap.GetDependentsMap(foreignKey).GetDependentsUsingRelationshipSnapshot(principalEntry);
	}

	public virtual IEnumerable<InternalEntityEntry> GetDependentsFromNavigation(InternalEntityEntry principalEntry, IForeignKey foreignKey)
	{
		INavigation principalToDependent = foreignKey.PrincipalToDependent;
		if (principalToDependent == null || principalToDependent.IsShadowProperty())
		{
			return null;
		}
		object obj = principalEntry[principalToDependent];
		if (obj == null)
		{
			return Enumerable.Empty<InternalEntityEntry>();
		}
		if (foreignKey.IsUnique)
		{
			InternalEntityEntry internalEntityEntry = TryGetEntry(obj, foreignKey.DeclaringEntityType);
			if (internalEntityEntry == null)
			{
				return Enumerable.Empty<InternalEntityEntry>();
			}
			return new InternalEntityEntry[1] { internalEntityEntry };
		}
		return from v in (IEnumerable<object>)obj
			select TryGetEntry(v, foreignKey.DeclaringEntityType) into e
			where e != null
			select e;
	}

	public virtual IEntityFinder CreateEntityFinder(IEntityType entityType)
	{
		return EntityFinderFactory.Create(entityType);
	}

	public virtual IList<IUpdateEntry> GetEntriesToSave(bool cascadeChanges)
	{
		if (cascadeChanges)
		{
			CascadeChanges(force: false);
		}
		List<IUpdateEntry> list = new List<IUpdateEntry>(GetCountForState(added: true, modified: true, deleted: true));
		foreach (InternalEntityEntry item in GetEntriesForState(added: true, modified: true, deleted: true))
		{
			list.Add(item.PrepareToSave());
		}
		return list;
	}

	public virtual void CascadeChanges(bool force)
	{
		List<InternalEntityEntry> list = new List<InternalEntityEntry>();
		foreach (InternalEntityEntry item in GetEntriesForState(added: true, modified: true))
		{
			if (item.HasConceptualNull)
			{
				list.Add(item);
			}
		}
		foreach (InternalEntityEntry item2 in list)
		{
			item2.HandleConceptualNulls(SensitiveLoggingEnabled, force, isCascadeDelete: false);
		}
		foreach (InternalEntityEntry item3 in this.ToListForState(added: false, modified: false, deleted: true))
		{
			CascadeDelete(item3, force);
		}
	}

	public virtual void CascadeDelete(InternalEntityEntry entry, bool force, IEnumerable<IForeignKey> foreignKeys = null)
	{
		bool flag = force || CascadeDeleteTiming != CascadeTiming.Never;
		bool flag2 = entry.EntityState == EntityState.Detached;
		bool isEnabled;
		bool flag3 = !AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue18982", out isEnabled) || !isEnabled;
		if (foreignKeys == null)
		{
			foreignKeys = entry.EntityType.GetReferencingForeignKeys();
		}
		foreach (IForeignKey foreignKey in foreignKeys)
		{
			if (foreignKey.DeleteBehavior == DeleteBehavior.ClientNoAction)
			{
				continue;
			}
			foreach (InternalEntityEntry item in (GetDependentsFromNavigation(entry, foreignKey) ?? GetDependents(entry, foreignKey)).ToList())
			{
				if (item.EntityState == EntityState.Deleted || item.EntityState == EntityState.Detached || (item.EntityState != EntityState.Added && !KeysEqual(entry, foreignKey, item)))
				{
					continue;
				}
				if ((foreignKey.DeleteBehavior == DeleteBehavior.Cascade || foreignKey.DeleteBehavior == DeleteBehavior.ClientCascade) && flag)
				{
					EntityState entityState = ((!flag3) ? ((item.EntityState != EntityState.Added) ? EntityState.Deleted : EntityState.Detached) : ((!flag2 && item.EntityState != EntityState.Added) ? EntityState.Deleted : EntityState.Detached));
					if (SensitiveLoggingEnabled)
					{
						UpdateLogger.CascadeDeleteSensitive(item, entry, entityState);
					}
					else
					{
						UpdateLogger.CascadeDelete(item, entry, entityState);
					}
					item.SetEntityState(entityState);
					CascadeDelete(item, force);
				}
				else
				{
					if (flag3 && flag2)
					{
						continue;
					}
					foreach (IProperty property in foreignKey.Properties)
					{
						item.SetProperty(property, null, isMaterialization: false, setModified: true, isCascadeDelete: true);
					}
					if (item.HasConceptualNull)
					{
						item.HandleConceptualNulls(SensitiveLoggingEnabled, force, isCascadeDelete: true);
					}
				}
			}
		}
	}

	private static bool KeysEqual(InternalEntityEntry entry, IForeignKey fk, InternalEntityEntry dependent)
	{
		for (int i = 0; i < fk.Properties.Count; i++)
		{
			IProperty property = fk.PrincipalKey.Properties[i];
			IProperty propertyBase = fk.Properties[i];
			if (!KeyValuesEqual(property, entry[property], dependent[propertyBase]))
			{
				return false;
			}
		}
		return true;
	}

	private static bool KeyValuesEqual(IProperty property, object value, object currentValue)
	{
		return (property.GetKeyValueComparer() ?? property.FindTypeMapping()?.KeyComparer)?.Equals(currentValue, value) ?? object.Equals(currentValue, value);
	}

	protected virtual int SaveChanges([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entriesToSave)
	{
		using (_concurrencyDetector.EnterCriticalSection())
		{
			return _database.SaveChanges(entriesToSave);
		}
	}

	protected virtual async Task<int> SaveChangesAsync([JetBrains.Annotations.NotNull] IList<IUpdateEntry> entriesToSave, CancellationToken cancellationToken = default(CancellationToken))
	{
		using (_concurrencyDetector.EnterCriticalSection())
		{
			return await _database.SaveChangesAsync(entriesToSave, cancellationToken);
		}
	}

	public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		if (!Context.Database.AutoTransactionsEnabled)
		{
			return SaveChanges(Context, acceptAllChangesOnSuccess);
		}
		return Dependencies.ExecutionStrategyFactory.Create().Execute(acceptAllChangesOnSuccess, SaveChanges, null);
	}

	private int SaveChanges(DbContext _, bool acceptAllChangesOnSuccess)
	{
		if (ChangedCount == 0)
		{
			return 0;
		}
		IList<IUpdateEntry> entriesToSave = GetEntriesToSave(cascadeChanges: true);
		if (entriesToSave.Count == 0)
		{
			return 0;
		}
		try
		{
			SavingChanges = true;
			int result = SaveChanges(entriesToSave);
			if (acceptAllChangesOnSuccess)
			{
				AcceptAllChanges((IReadOnlyList<IUpdateEntry>)entriesToSave);
			}
			return result;
		}
		catch
		{
			foreach (InternalEntityEntry item in entriesToSave)
			{
				item.DiscardStoreGeneratedValues();
			}
			throw;
		}
		finally
		{
			SavingChanges = false;
		}
	}

	public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!Context.Database.AutoTransactionsEnabled)
		{
			return SaveChangesAsync(Context, acceptAllChangesOnSuccess, cancellationToken);
		}
		return Dependencies.ExecutionStrategyFactory.Create().ExecuteAsync(acceptAllChangesOnSuccess, SaveChangesAsync, null, cancellationToken);
	}

	private async Task<int> SaveChangesAsync(DbContext _, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
	{
		if (ChangedCount == 0)
		{
			return 0;
		}
		IList<IUpdateEntry> entriesToSave = GetEntriesToSave(cascadeChanges: true);
		if (entriesToSave.Count == 0)
		{
			return 0;
		}
		try
		{
			SavingChanges = true;
			int result = await SaveChangesAsync(entriesToSave, cancellationToken);
			if (acceptAllChangesOnSuccess)
			{
				AcceptAllChanges((IReadOnlyList<IUpdateEntry>)entriesToSave);
			}
			return result;
		}
		catch
		{
			foreach (InternalEntityEntry item in entriesToSave)
			{
				item.DiscardStoreGeneratedValues();
			}
			throw;
		}
		finally
		{
			SavingChanges = false;
		}
	}

	public virtual void AcceptAllChanges()
	{
		AcceptAllChanges(this.ToListForState(added: true, modified: true, deleted: true));
	}

	private static void AcceptAllChanges(IReadOnlyList<IUpdateEntry> changedEntries)
	{
		for (int i = 0; i < changedEntries.Count; i++)
		{
			((InternalEntityEntry)changedEntries[i]).AcceptChanges();
		}
	}

	public virtual void OnTracked(InternalEntityEntry internalEntityEntry, bool fromQuery)
	{
		EventHandler<EntityTrackedEventArgs> tracked = this.Tracked;
		if (SensitiveLoggingEnabled)
		{
			_changeTrackingLogger.StartedTrackingSensitive(internalEntityEntry);
		}
		else
		{
			_changeTrackingLogger.StartedTracking(internalEntityEntry);
		}
		tracked?.Invoke(Context.ChangeTracker, new EntityTrackedEventArgs(internalEntityEntry, fromQuery));
	}

	public virtual void OnStateChanged(InternalEntityEntry internalEntityEntry, EntityState oldState)
	{
		EventHandler<EntityStateChangedEventArgs> stateChanged = this.StateChanged;
		EntityState entityState = internalEntityEntry.EntityState;
		if (SensitiveLoggingEnabled)
		{
			_changeTrackingLogger.StateChangedSensitive(internalEntityEntry, oldState, entityState);
		}
		else
		{
			_changeTrackingLogger.StateChanged(internalEntityEntry, oldState, entityState);
		}
		stateChanged?.Invoke(Context.ChangeTracker, new EntityStateChangedEventArgs(internalEntityEntry, oldState, entityState));
	}
}
