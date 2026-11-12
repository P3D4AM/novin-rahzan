using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class NavigationFixer : INavigationFixer
{
	private readonly IChangeDetector _changeDetector;

	private readonly IEntityGraphAttacher _attacher;

	private bool _inFixup;

	public NavigationFixer([JetBrains.Annotations.NotNull] IChangeDetector changeDetector, [JetBrains.Annotations.NotNull] IEntityGraphAttacher attacher)
	{
		_changeDetector = changeDetector;
		_attacher = attacher;
	}

	public virtual void NavigationReferenceChanged(InternalEntityEntry entry, INavigation navigation, object oldValue, object newValue)
	{
		if (_inFixup)
		{
			return;
		}
		IForeignKey foreignKey = navigation.ForeignKey;
		IStateManager stateManager = entry.StateManager;
		INavigation navigation2 = navigation.FindInverse();
		IEntityType targetType = navigation.GetTargetType();
		InternalEntityEntry oldTargetEntry = ((oldValue == null) ? null : stateManager.TryGetEntry(oldValue, targetType));
		InternalEntityEntry internalEntityEntry = oldTargetEntry;
		if (internalEntityEntry != null && internalEntityEntry.EntityState == EntityState.Detached)
		{
			oldTargetEntry = null;
		}
		InternalEntityEntry internalEntityEntry2 = ((newValue == null) ? null : stateManager.TryGetEntry(newValue, targetType));
		if (internalEntityEntry2 != null && internalEntityEntry2.EntityState == EntityState.Detached)
		{
			internalEntityEntry2 = null;
		}
		try
		{
			_inFixup = true;
			if (navigation.IsDependentToPrincipal())
			{
				if (newValue != null)
				{
					if (internalEntityEntry2 != null)
					{
						if (foreignKey.IsUnique)
						{
							InternalEntityEntry internalEntityEntry3 = stateManager.GetDependents(internalEntityEntry2, foreignKey).FirstOrDefault();
							if (internalEntityEntry3 != null && internalEntityEntry3 != entry)
							{
								ConditionallyNullForeignKeyProperties(internalEntityEntry3, internalEntityEntry2, foreignKey);
								if (internalEntityEntry3[navigation] == internalEntityEntry2.Entity && internalEntityEntry3.StateManager.TryGetEntry(internalEntityEntry3.Entity, navigation.DeclaringEntityType) != null)
								{
									SetNavigation(internalEntityEntry3, navigation, null, fromQuery: false);
								}
							}
						}
						SetForeignKeyProperties(entry, internalEntityEntry2, foreignKey, setModified: true, fromQuery: false);
					}
				}
				else
				{
					ConditionallyNullForeignKeyProperties(entry, oldTargetEntry, foreignKey);
					entry.SetRelationshipSnapshotValue(navigation, null);
				}
				if (navigation2 != null)
				{
					if (internalEntityEntry2 != null)
					{
						SetReferenceOrAddToCollection(internalEntityEntry2, navigation2, entry, fromQuery: false);
					}
					if (oldTargetEntry != null && oldTargetEntry.EntityState != EntityState.Deleted)
					{
						ResetReferenceOrRemoveCollection(oldTargetEntry, navigation2, entry, fromQuery: false);
					}
				}
			}
			else
			{
				if (oldTargetEntry != null)
				{
					ConditionallyNullForeignKeyProperties(oldTargetEntry, entry, foreignKey);
					if (navigation2 != null && oldTargetEntry[navigation2] == entry.Entity && (!oldTargetEntry.EntityType.HasDefiningNavigation() || entry.EntityType.GetNavigations().All((INavigation n) => n == navigation || oldTargetEntry.Entity != entry[n])))
					{
						SetNavigation(oldTargetEntry, navigation2, null, fromQuery: false);
					}
				}
				if (internalEntityEntry2 != null)
				{
					InternalEntityEntry internalEntityEntry4 = stateManager.FindPrincipal(internalEntityEntry2, foreignKey);
					if (internalEntityEntry4 != null && internalEntityEntry4 != entry && internalEntityEntry4[navigation] == internalEntityEntry2.Entity)
					{
						SetNavigation(internalEntityEntry4, navigation, null, fromQuery: false);
					}
					SetForeignKeyProperties(internalEntityEntry2, entry, foreignKey, setModified: true, fromQuery: false);
					SetNavigation(internalEntityEntry2, navigation2, entry, fromQuery: false);
				}
			}
			if (newValue == null)
			{
				entry.SetIsLoaded(navigation, loaded: false);
			}
		}
		finally
		{
			_inFixup = false;
		}
		if (newValue != null && internalEntityEntry2 == null)
		{
			stateManager.RecordReferencedUntrackedEntity(newValue, navigation, entry);
			entry.SetRelationshipSnapshotValue(navigation, newValue);
			internalEntityEntry2 = stateManager.GetOrCreateEntry(newValue, targetType);
			_attacher.AttachGraph(internalEntityEntry2, EntityState.Added, EntityState.Modified, forceStateWhenUnknownKey: false);
		}
	}

	public virtual void NavigationCollectionChanged(InternalEntityEntry entry, INavigation navigation, IEnumerable<object> added, IEnumerable<object> removed)
	{
		if (_inFixup)
		{
			return;
		}
		IForeignKey foreignKey = navigation.ForeignKey;
		IStateManager stateManager = entry.StateManager;
		INavigation navigation2 = navigation.FindInverse();
		IEntityType targetType = navigation.GetTargetType();
		foreach (object item in removed)
		{
			InternalEntityEntry internalEntityEntry = stateManager.TryGetEntry(item, targetType);
			if (internalEntityEntry == null || internalEntityEntry.EntityState == EntityState.Detached)
			{
				continue;
			}
			try
			{
				_inFixup = true;
				ConditionallyNullForeignKeyProperties(internalEntityEntry, entry, foreignKey);
				if (navigation2 != null && internalEntityEntry[navigation2] == entry.Entity && (!foreignKey.IsOwnership || (internalEntityEntry.EntityState != EntityState.Deleted && internalEntityEntry.EntityState != 0)))
				{
					SetNavigation(internalEntityEntry, navigation2, null, fromQuery: false);
				}
				entry.RemoveFromCollectionSnapshot(navigation, item);
			}
			finally
			{
				_inFixup = false;
			}
		}
		foreach (object item2 in added)
		{
			InternalEntityEntry orCreateEntry = stateManager.GetOrCreateEntry(item2, targetType);
			if (orCreateEntry.EntityState != 0)
			{
				try
				{
					_inFixup = true;
					InternalEntityEntry internalEntityEntry2 = stateManager.FindPrincipalUsingRelationshipSnapshot(orCreateEntry, foreignKey);
					if (internalEntityEntry2 != null && internalEntityEntry2 != entry)
					{
						RemoveFromCollection(internalEntityEntry2, navigation, orCreateEntry);
					}
					SetForeignKeyProperties(orCreateEntry, entry, foreignKey, setModified: true, fromQuery: false);
					SetNavigation(orCreateEntry, navigation2, entry, fromQuery: false);
				}
				finally
				{
					_inFixup = false;
				}
			}
			else
			{
				stateManager.RecordReferencedUntrackedEntity(item2, navigation, entry);
				_attacher.AttachGraph(orCreateEntry, EntityState.Added, EntityState.Modified, forceStateWhenUnknownKey: false);
			}
			entry.AddToCollectionSnapshot(navigation, item2);
		}
	}

	public virtual void KeyPropertyChanged(InternalEntityEntry entry, IProperty property, IReadOnlyList<IKey> containingPrincipalKeys, IReadOnlyList<IForeignKey> containingForeignKeys, object oldValue, object newValue)
	{
		if (entry.EntityState == EntityState.Detached)
		{
			return;
		}
		try
		{
			_inFixup = true;
			IStateManager stateManager = entry.StateManager;
			foreach (IForeignKey containingForeignKey in containingForeignKeys)
			{
				InternalEntityEntry internalEntityEntry = stateManager.FindPrincipal(entry, containingForeignKey) ?? stateManager.FindPrincipalUsingPreStoreGeneratedValues(entry, containingForeignKey);
				InternalEntityEntry internalEntityEntry2 = stateManager.FindPrincipalUsingRelationshipSnapshot(entry, containingForeignKey);
				INavigation principalToDependent = containingForeignKey.PrincipalToDependent;
				if (principalToDependent != null)
				{
					if (internalEntityEntry2 != null && internalEntityEntry2.EntityState != EntityState.Deleted)
					{
						ResetReferenceOrRemoveCollection(internalEntityEntry2, principalToDependent, entry, fromQuery: false);
					}
					if (internalEntityEntry != null && !entry.IsConceptualNull(property))
					{
						SetReferenceOrAddToCollection(internalEntityEntry, principalToDependent, entry, fromQuery: false);
					}
				}
				INavigation dependentToPrincipal = containingForeignKey.DependentToPrincipal;
				if (dependentToPrincipal != null)
				{
					if (internalEntityEntry != null)
					{
						if (containingForeignKey.IsUnique)
						{
							InternalEntityEntry internalEntityEntry3 = stateManager.GetDependentsUsingRelationshipSnapshot(internalEntityEntry, containingForeignKey).FirstOrDefault();
							if (internalEntityEntry3 != null && internalEntityEntry3 != entry)
							{
								ConditionallyNullForeignKeyProperties(internalEntityEntry3, internalEntityEntry, containingForeignKey);
								if (internalEntityEntry3[dependentToPrincipal] == internalEntityEntry.Entity && internalEntityEntry3.StateManager.TryGetEntry(internalEntityEntry3.Entity, containingForeignKey.DeclaringEntityType) != null)
								{
									SetNavigation(internalEntityEntry3, dependentToPrincipal, null, fromQuery: false);
								}
							}
						}
						if (!entry.IsConceptualNull(property))
						{
							SetNavigation(entry, dependentToPrincipal, internalEntityEntry, fromQuery: false);
						}
					}
					else if (internalEntityEntry2 != null)
					{
						if (entry[dependentToPrincipal] == internalEntityEntry2.Entity && entry.StateManager.TryGetEntry(entry.Entity, containingForeignKey.DeclaringEntityType) != null)
						{
							SetNavigation(entry, dependentToPrincipal, null, fromQuery: false);
						}
					}
					else if (entry[dependentToPrincipal] == null && entry.StateManager.TryGetEntry(entry.Entity, containingForeignKey.DeclaringEntityType) != null)
					{
						entry.SetIsLoaded(dependentToPrincipal, loaded: false);
					}
				}
				stateManager.UpdateDependentMap(entry, containingForeignKey);
			}
			foreach (IKey containingPrincipalKey in containingPrincipalKeys)
			{
				stateManager.UpdateIdentityMap(entry, containingPrincipalKey);
				foreach (IForeignKey referencingForeignKey in containingPrincipalKey.GetReferencingForeignKeys())
				{
					foreach (InternalEntityEntry item in stateManager.GetDependentsUsingRelationshipSnapshot(entry, referencingForeignKey).ToList())
					{
						SetForeignKeyProperties(item, entry, referencingForeignKey, setModified: true, fromQuery: false);
					}
					if (referencingForeignKey.IsOwnership)
					{
						continue;
					}
					foreach (InternalEntityEntry item2 in stateManager.GetDependents(entry, referencingForeignKey).ToList())
					{
						INavigation principalToDependent2 = referencingForeignKey.PrincipalToDependent;
						if (principalToDependent2 != null && !entry.IsConceptualNull(property))
						{
							SetReferenceOrAddToCollection(entry, principalToDependent2, item2, fromQuery: false);
						}
						INavigation dependentToPrincipal2 = referencingForeignKey.DependentToPrincipal;
						if (dependentToPrincipal2 != null && !entry.IsConceptualNull(property))
						{
							SetNavigation(item2, dependentToPrincipal2, entry, fromQuery: false);
						}
					}
				}
			}
			entry.SetRelationshipSnapshotValue(property, newValue);
		}
		finally
		{
			_inFixup = false;
		}
	}

	public virtual void StateChanging(InternalEntityEntry entry, EntityState newState)
	{
	}

	public virtual void TrackedFromQuery(InternalEntityEntry entry)
	{
		try
		{
			_inFixup = true;
			InitialFixup(entry, fromQuery: true);
		}
		finally
		{
			_inFixup = false;
		}
	}

	public virtual void StateChanged(InternalEntityEntry entry, EntityState oldState, bool fromQuery)
	{
		if (fromQuery || _inFixup)
		{
			return;
		}
		try
		{
			_inFixup = true;
			switch (oldState)
			{
			case EntityState.Detached:
				InitialFixup(entry, fromQuery: false);
				break;
			case EntityState.Deleted:
				if (entry.EntityState == EntityState.Detached)
				{
					DeleteFixup(entry);
				}
				break;
			}
		}
		finally
		{
			_inFixup = false;
		}
	}

	private void DeleteFixup(InternalEntityEntry entry)
	{
		IEntityType entityType = entry.EntityType;
		IStateManager stateManager = entry.StateManager;
		foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
		{
			INavigation principalToDependent = foreignKey.PrincipalToDependent;
			if (principalToDependent != null)
			{
				InternalEntityEntry internalEntityEntry = stateManager.FindPrincipal(entry, foreignKey);
				if (internalEntityEntry != null && internalEntityEntry.EntityState != EntityState.Deleted)
				{
					ResetReferenceOrRemoveCollection(internalEntityEntry, principalToDependent, entry, fromQuery: false);
				}
			}
		}
		foreach (IForeignKey referencingForeignKey in entityType.GetReferencingForeignKeys())
		{
			INavigation dependentToPrincipal = referencingForeignKey.DependentToPrincipal;
			if (dependentToPrincipal == null && !referencingForeignKey.IsOwnership)
			{
				continue;
			}
			foreach (InternalEntityEntry item in stateManager.GetDependents(entry, referencingForeignKey).ToList())
			{
				if (referencingForeignKey.IsOwnership)
				{
					ConditionallyNullForeignKeyProperties(item, entry, referencingForeignKey);
				}
				if (dependentToPrincipal != null && (!referencingForeignKey.IsOwnership || (entry.EntityState != EntityState.Deleted && entry.EntityState != 0)) && item[dependentToPrincipal] == entry.Entity)
				{
					SetNavigation(item, dependentToPrincipal, null, fromQuery: false);
				}
			}
		}
	}

	private void InitialFixup(InternalEntityEntry entry, bool fromQuery)
	{
		IEntityType entityType = entry.EntityType;
		IStateManager stateManager = entry.StateManager;
		foreach (IForeignKey foreignKey in entityType.GetForeignKeys())
		{
			InternalEntityEntry internalEntityEntry = stateManager.FindPrincipal(entry, foreignKey);
			if (internalEntityEntry != null)
			{
				INavigation dependentToPrincipal = foreignKey.DependentToPrincipal;
				object obj = ((dependentToPrincipal == null) ? null : entry[dependentToPrincipal]);
				if (obj == null || obj == internalEntityEntry.Entity)
				{
					SetNavigation(entry, dependentToPrincipal, internalEntityEntry, fromQuery);
					ToDependentFixup(entry, internalEntityEntry, foreignKey, fromQuery);
				}
			}
		}
		foreach (IForeignKey referencingForeignKey in entityType.GetReferencingForeignKeys())
		{
			if (referencingForeignKey.DeclaringEntityType.FindPrimaryKey() == null)
			{
				continue;
			}
			IEnumerable<InternalEntityEntry> dependents = stateManager.GetDependents(entry, referencingForeignKey);
			if (referencingForeignKey.IsUnique)
			{
				InternalEntityEntry internalEntityEntry2 = dependents.FirstOrDefault();
				if (internalEntityEntry2 != null && (!referencingForeignKey.IsOwnership || (internalEntityEntry2.EntityState != EntityState.Deleted && internalEntityEntry2.EntityState != 0)) && (referencingForeignKey.PrincipalToDependent == null || entry[referencingForeignKey.PrincipalToDependent] == null || entry[referencingForeignKey.PrincipalToDependent] == internalEntityEntry2.Entity))
				{
					SetNavigation(entry, referencingForeignKey.PrincipalToDependent, internalEntityEntry2, fromQuery);
					SetNavigation(internalEntityEntry2, referencingForeignKey.DependentToPrincipal, entry, fromQuery);
				}
				continue;
			}
			foreach (InternalEntityEntry item in dependents)
			{
				if (!referencingForeignKey.IsOwnership || (item.EntityState != EntityState.Deleted && item.EntityState != 0))
				{
					AddToCollection(entry, referencingForeignKey.PrincipalToDependent, item, fromQuery);
					SetNavigation(item, referencingForeignKey.DependentToPrincipal, entry, fromQuery);
				}
			}
		}
		if (fromQuery)
		{
			return;
		}
		bool setModified = entry.EntityState != EntityState.Unchanged;
		foreach (IForeignKey referencingForeignKey2 in entityType.GetReferencingForeignKeys())
		{
			if (referencingForeignKey2.DeclaringEntityType.FindPrimaryKey() == null)
			{
				continue;
			}
			INavigation principalToDependent = referencingForeignKey2.PrincipalToDependent;
			if (principalToDependent == null)
			{
				continue;
			}
			object obj2 = entry[principalToDependent];
			if (obj2 == null)
			{
				continue;
			}
			if (principalToDependent.IsCollection())
			{
				foreach (object item2 in ((IEnumerable)obj2).Cast<object>().ToList())
				{
					InternalEntityEntry internalEntityEntry3 = stateManager.TryGetEntry(item2, referencingForeignKey2.DeclaringEntityType);
					if (internalEntityEntry3 == null || internalEntityEntry3.EntityState == EntityState.Detached)
					{
						stateManager.RecordReferencedUntrackedEntity(item2, principalToDependent, entry);
					}
					else
					{
						FixupToDependent(entry, internalEntityEntry3, referencingForeignKey2, setModified, fromQuery);
					}
				}
			}
			else
			{
				IEntityType targetType = principalToDependent.GetTargetType();
				InternalEntityEntry internalEntityEntry4 = stateManager.TryGetEntry(obj2, targetType);
				if (internalEntityEntry4 == null || internalEntityEntry4.EntityState == EntityState.Detached)
				{
					stateManager.RecordReferencedUntrackedEntity(obj2, principalToDependent, entry);
				}
				else
				{
					FixupToDependent(entry, internalEntityEntry4, referencingForeignKey2, setModified, fromQuery);
				}
			}
		}
		foreach (IForeignKey foreignKey2 in entityType.GetForeignKeys())
		{
			INavigation dependentToPrincipal2 = foreignKey2.DependentToPrincipal;
			if (dependentToPrincipal2 == null)
			{
				continue;
			}
			object obj3 = entry[dependentToPrincipal2];
			if (obj3 != null)
			{
				IEntityType targetType2 = dependentToPrincipal2.GetTargetType();
				InternalEntityEntry internalEntityEntry5 = stateManager.TryGetEntry(obj3, targetType2);
				if (internalEntityEntry5 == null || internalEntityEntry5.EntityState == EntityState.Detached)
				{
					stateManager.RecordReferencedUntrackedEntity(obj3, dependentToPrincipal2, entry);
				}
				else
				{
					FixupToPrincipal(entry, internalEntityEntry5, foreignKey2, setModified, fromQuery);
				}
			}
		}
		foreach (Tuple<INavigation, InternalEntityEntry> recordedReferrer in stateManager.GetRecordedReferrers(entry.Entity, clear: true))
		{
			DelayedFixup(recordedReferrer.Item2, recordedReferrer.Item1, entry, fromQuery);
		}
	}

	private void DelayedFixup(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry referencedEntry, bool fromQuery)
	{
		object obj = entry[navigation];
		if (obj == null)
		{
			return;
		}
		bool setModified = referencedEntry.EntityState != EntityState.Unchanged;
		if (!navigation.IsDependentToPrincipal())
		{
			if (navigation.IsCollection())
			{
				if (entry.CollectionContains(navigation, referencedEntry))
				{
					FixupToDependent(entry, referencedEntry, navigation.ForeignKey, setModified, fromQuery);
				}
			}
			else if (referencedEntry.Entity == obj)
			{
				FixupToDependent(entry, referencedEntry, navigation.ForeignKey, setModified, fromQuery);
			}
		}
		else if (referencedEntry.Entity == obj)
		{
			FixupToPrincipal(entry, referencedEntry, navigation.ForeignKey, setModified, fromQuery);
		}
	}

	private void FixupToDependent(InternalEntityEntry principalEntry, InternalEntityEntry dependentEntry, IForeignKey foreignKey, bool setModified, bool fromQuery)
	{
		SetForeignKeyProperties(dependentEntry, principalEntry, foreignKey, setModified, fromQuery);
		SetNavigation(dependentEntry, foreignKey.DependentToPrincipal, principalEntry, fromQuery);
	}

	private void FixupToPrincipal(InternalEntityEntry dependentEntry, InternalEntityEntry principalEntry, IForeignKey foreignKey, bool setModified, bool fromQuery)
	{
		SetForeignKeyProperties(dependentEntry, principalEntry, foreignKey, setModified, fromQuery);
		ToDependentFixup(dependentEntry, principalEntry, foreignKey, fromQuery);
	}

	private void ToDependentFixup(InternalEntityEntry dependentEntry, InternalEntityEntry principalEntry, IForeignKey foreignKey, bool fromQuery)
	{
		INavigation principalToDependent = foreignKey.PrincipalToDependent;
		if (foreignKey.IsUnique)
		{
			object obj = ((principalToDependent == null) ? null : principalEntry[principalToDependent]);
			InternalEntityEntry internalEntityEntry = ((obj != null && dependentEntry.Entity != obj) ? dependentEntry.StateManager.TryGetEntry(obj, foreignKey.DeclaringEntityType) : dependentEntry.StateManager.GetDependentsUsingRelationshipSnapshot(principalEntry, foreignKey).FirstOrDefault());
			if (internalEntityEntry != null && dependentEntry.Entity != internalEntityEntry.Entity && internalEntityEntry.EntityState != 0)
			{
				ConditionallyNullForeignKeyProperties(internalEntityEntry, principalEntry, foreignKey);
				INavigation dependentToPrincipal = foreignKey.DependentToPrincipal;
				if (dependentToPrincipal != null && internalEntityEntry[dependentToPrincipal] == principalEntry.Entity && internalEntityEntry.StateManager.TryGetEntry(internalEntityEntry.Entity, foreignKey.DeclaringEntityType) != null)
				{
					SetNavigation(internalEntityEntry, dependentToPrincipal, null, fromQuery);
				}
			}
		}
		if (principalToDependent != null)
		{
			SetReferenceOrAddToCollection(principalEntry, principalToDependent, dependentEntry, fromQuery);
		}
	}

	private static void SetForeignKeyProperties(InternalEntityEntry dependentEntry, InternalEntityEntry principalEntry, IForeignKey foreignKey, bool setModified, bool fromQuery)
	{
		IReadOnlyList<IProperty> properties = foreignKey.PrincipalKey.Properties;
		IReadOnlyList<IProperty> properties2 = foreignKey.Properties;
		for (int i = 0; i < foreignKey.Properties.Count; i++)
		{
			IProperty property = properties[i];
			IProperty property2 = properties2[i];
			object obj = principalEntry[property];
			object dependentValue = dependentEntry[property2];
			if (!PrincipalValueEqualsDependentValue(property, dependentValue, obj) || (dependentEntry.IsConceptualNull(property2) && obj != null))
			{
				if (principalEntry.HasTemporaryValue(property))
				{
					dependentEntry.SetTemporaryValue(property2, obj, setModified);
				}
				else
				{
					dependentEntry.SetProperty(property2, obj, fromQuery, setModified);
				}
				dependentEntry.StateManager.UpdateDependentMap(dependentEntry, foreignKey);
				dependentEntry.SetRelationshipSnapshotValue(property2, obj);
			}
		}
	}

	private static bool PrincipalValueEqualsDependentValue(IProperty principalProperty, object dependentValue, object principalValue)
	{
		return (principalProperty.GetKeyValueComparer() ?? principalProperty.GetTypeMapping().KeyComparer)?.Equals(dependentValue, principalValue) ?? StructuralComparisons.StructuralEqualityComparer.Equals(dependentValue, principalValue);
	}

	private void ConditionallyNullForeignKeyProperties(InternalEntityEntry dependentEntry, InternalEntityEntry principalEntry, IForeignKey foreignKey)
	{
		IReadOnlyList<IProperty> properties = foreignKey.PrincipalKey.Properties;
		IReadOnlyList<IProperty> properties2 = foreignKey.Properties;
		bool flag = true;
		InternalEntityEntry internalEntityEntry = dependentEntry.StateManager.FindPrincipal(dependentEntry, foreignKey);
		if (internalEntityEntry != null && internalEntityEntry != principalEntry)
		{
			return;
		}
		if (principalEntry != null && principalEntry.EntityState != 0)
		{
			for (int i = 0; i < foreignKey.Properties.Count; i++)
			{
				if (!PrincipalValueEqualsDependentValue(properties[i], dependentEntry[properties2[i]], principalEntry[properties[i]]))
				{
					return;
				}
				if (!properties2[i].IsKey())
				{
					flag = false;
				}
			}
		}
		for (int j = 0; j < foreignKey.Properties.Count; j++)
		{
			if (!properties2[j].IsKey())
			{
				dependentEntry[properties2[j]] = null;
				dependentEntry.StateManager.UpdateDependentMap(dependentEntry, foreignKey);
				dependentEntry.SetRelationshipSnapshotValue(properties2[j], null);
			}
		}
		if (foreignKey.IsRequired && flag && dependentEntry.EntityState != 0)
		{
			switch (dependentEntry.EntityState)
			{
			case EntityState.Added:
				dependentEntry.SetEntityState(EntityState.Detached);
				DeleteFixup(dependentEntry);
				break;
			case EntityState.Unchanged:
			case EntityState.Modified:
				dependentEntry.SetEntityState(EntityState.Deleted);
				DeleteFixup(dependentEntry);
				break;
			case EntityState.Deleted:
				break;
			}
		}
	}

	private void SetNavigation(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry value, bool fromQuery)
	{
		if (navigation != null)
		{
			_changeDetector.Suspend();
			object value2 = value?.Entity;
			try
			{
				entry.SetProperty(navigation, value2, fromQuery);
			}
			finally
			{
				_changeDetector.Resume();
			}
			entry.SetRelationshipSnapshotValue(navigation, value2);
		}
	}

	private void AddToCollection(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry value, bool fromQuery)
	{
		if (navigation == null)
		{
			return;
		}
		_changeDetector.Suspend();
		try
		{
			if (entry.AddToCollection(navigation, value, fromQuery))
			{
				entry.AddToCollectionSnapshot(navigation, value.Entity);
			}
		}
		finally
		{
			_changeDetector.Resume();
		}
	}

	private void RemoveFromCollection(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry value)
	{
		_changeDetector.Suspend();
		try
		{
			if (entry.RemoveFromCollection(navigation, value))
			{
				entry.RemoveFromCollectionSnapshot(navigation, value.Entity);
			}
		}
		finally
		{
			_changeDetector.Resume();
		}
	}

	private void SetReferenceOrAddToCollection(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry value, bool fromQuery)
	{
		if (navigation.IsCollection())
		{
			AddToCollection(entry, navigation, value, fromQuery);
		}
		else
		{
			SetNavigation(entry, navigation, value, fromQuery);
		}
	}

	private void ResetReferenceOrRemoveCollection(InternalEntityEntry entry, INavigation navigation, InternalEntityEntry value, bool fromQuery)
	{
		if (navigation.IsCollection())
		{
			RemoveFromCollection(entry, navigation, value);
		}
		else if (entry[navigation] == value.Entity)
		{
			SetNavigation(entry, navigation, null, fromQuery);
		}
	}
}
