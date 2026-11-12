using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class ChangeDetector : IChangeDetector
{
	private readonly IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> _logger;

	private readonly ILoggingOptions _loggingOptions;

	public const string SkipDetectChangesAnnotation = "ChangeDetector.SkipDetectChanges";

	private bool _suspended;

	public ChangeDetector([JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.ChangeTracking> logger, [JetBrains.Annotations.NotNull] ILoggingOptions loggingOptions)
	{
		_logger = logger;
		_loggingOptions = loggingOptions;
	}

	public virtual void Suspend()
	{
		_suspended = true;
	}

	public virtual void Resume()
	{
		_suspended = false;
	}

	public virtual void PropertyChanged(InternalEntityEntry entry, IPropertyBase propertyBase, bool setModified)
	{
		if (_suspended || entry.EntityState == EntityState.Detached || propertyBase is IServiceProperty)
		{
			return;
		}
		if (propertyBase is IProperty property)
		{
			entry.SetPropertyModified(property, setModified);
			if (property.GetRelationshipIndex() != -1)
			{
				DetectKeyChange(entry, property);
			}
		}
		else if (propertyBase.GetRelationshipIndex() != -1 && propertyBase is INavigation navigation)
		{
			DetectNavigationChange(entry, navigation);
		}
	}

	public virtual void PropertyChanging(InternalEntityEntry entry, IPropertyBase propertyBase)
	{
		if (!_suspended && entry.EntityState != 0 && !(propertyBase is IServiceProperty) && !entry.EntityType.UseEagerSnapshots())
		{
			if (propertyBase is IProperty propertyBase2 && propertyBase2.GetOriginalValueIndex() != -1)
			{
				entry.EnsureOriginalValues();
			}
			if (propertyBase.GetRelationshipIndex() != -1)
			{
				entry.EnsureRelationshipSnapshot();
			}
		}
	}

	public virtual void DetectChanges(IStateManager stateManager)
	{
		_logger.DetectChangesStarting(stateManager.Context);
		foreach (InternalEntityEntry item in stateManager.ToList())
		{
			if (item.EntityType.GetChangeTrackingStrategy() == ChangeTrackingStrategy.Snapshot && item.EntityState != 0)
			{
				LocalDetectChanges(item);
			}
		}
		_logger.DetectChangesCompleted(stateManager.Context);
	}

	public virtual void DetectChanges(InternalEntityEntry entry)
	{
		DetectChanges(entry, new HashSet<InternalEntityEntry> { entry });
	}

	private void DetectChanges(InternalEntityEntry entry, HashSet<InternalEntityEntry> visited)
	{
		if (entry.EntityState == EntityState.Detached)
		{
			return;
		}
		foreach (IForeignKey foreignKey in entry.EntityType.GetForeignKeys())
		{
			InternalEntityEntry internalEntityEntry = entry.StateManager.FindPrincipal(entry, foreignKey);
			if (internalEntityEntry != null && !visited.Contains(internalEntityEntry))
			{
				visited.Add(internalEntityEntry);
				DetectChanges(internalEntityEntry, visited);
			}
		}
		LocalDetectChanges(entry);
	}

	private void LocalDetectChanges(InternalEntityEntry entry)
	{
		IEntityType entityType = entry.EntityType;
		foreach (IProperty property in entityType.GetProperties())
		{
			if (property.GetOriginalValueIndex() < 0 || entry.IsModified(property) || entry.IsConceptualNull(property))
			{
				continue;
			}
			object obj = entry[property];
			object originalValue = entry.GetOriginalValue(property);
			ValueComparer valueComparer = property.GetValueComparer() ?? property.FindTypeMapping()?.Comparer;
			if (valueComparer == null)
			{
				if (!object.Equals(obj, originalValue))
				{
					LogChangeDetected(entry, property, originalValue, obj);
					entry.SetPropertyModified(property);
				}
			}
			else if (!valueComparer.Equals(obj, originalValue))
			{
				LogChangeDetected(entry, property, originalValue, obj);
				entry.SetPropertyModified(property);
			}
		}
		foreach (IProperty property2 in entityType.GetProperties())
		{
			DetectKeyChange(entry, property2);
		}
		if (!entry.HasRelationshipSnapshot)
		{
			return;
		}
		foreach (INavigation navigation in entityType.GetNavigations())
		{
			DetectNavigationChange(entry, navigation);
		}
	}

	private void LogChangeDetected(InternalEntityEntry entry, IProperty property, object original, object current)
	{
		if (_loggingOptions.IsSensitiveDataLoggingEnabled)
		{
			_logger.PropertyChangeDetectedSensitive(entry, property, original, current);
		}
		else
		{
			_logger.PropertyChangeDetected(entry, property, original, current);
		}
	}

	private void DetectKeyChange(InternalEntityEntry entry, IProperty property)
	{
		if (property.GetRelationshipIndex() < 0)
		{
			return;
		}
		object relationshipSnapshotValue = entry.GetRelationshipSnapshotValue(property);
		object obj = entry[property];
		if (!((property.GetKeyValueComparer() ?? property.FindTypeMapping()?.KeyComparer)?.Equals(obj, relationshipSnapshotValue) ?? StructuralComparisons.StructuralEqualityComparer.Equals(obj, relationshipSnapshotValue)))
		{
			List<IKey> keys = property.GetContainingKeys().ToList();
			List<IForeignKey> foreignKeys = (from fk in property.GetContainingForeignKeys()
				where fk.DeclaringEntityType.IsAssignableFrom(entry.EntityType)
				select fk).ToList();
			if (_loggingOptions.IsSensitiveDataLoggingEnabled)
			{
				_logger.ForeignKeyChangeDetectedSensitive(entry, property, relationshipSnapshotValue, obj);
			}
			else
			{
				_logger.ForeignKeyChangeDetected(entry, property, relationshipSnapshotValue, obj);
			}
			entry.StateManager.InternalEntityEntryNotifier.KeyPropertyChanged(entry, property, keys, foreignKeys, relationshipSnapshotValue, obj);
		}
	}

	private void DetectNavigationChange(InternalEntityEntry entry, INavigation navigation)
	{
		object relationshipSnapshotValue = entry.GetRelationshipSnapshotValue(navigation);
		object obj = entry[navigation];
		IStateManager stateManager = entry.StateManager;
		if (navigation.IsCollection())
		{
			IEnumerable enumerable = (IEnumerable)relationshipSnapshotValue;
			IEnumerable enumerable2 = (IEnumerable)obj;
			HashSet<object> hashSet = new HashSet<object>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			if (enumerable != null)
			{
				foreach (object item in enumerable)
				{
					hashSet.Add(item);
				}
			}
			HashSet<object> hashSet2 = new HashSet<object>(Microsoft.EntityFrameworkCore.Internal.ReferenceEqualityComparer.Instance);
			if (enumerable2 != null)
			{
				foreach (object item2 in enumerable2)
				{
					if (!hashSet.Remove(item2))
					{
						hashSet2.Add(item2);
					}
				}
			}
			if (hashSet2.Count > 0 || hashSet.Count > 0)
			{
				if (_loggingOptions.IsSensitiveDataLoggingEnabled)
				{
					_logger.CollectionChangeDetectedSensitive(entry, navigation, hashSet2, hashSet);
				}
				else
				{
					_logger.CollectionChangeDetected(entry, navigation, hashSet2, hashSet);
				}
				stateManager.InternalEntityEntryNotifier.NavigationCollectionChanged(entry, navigation, hashSet2, hashSet);
			}
		}
		else if (obj != relationshipSnapshotValue && (!navigation.ForeignKey.IsOwnership || !navigation.IsDependentToPrincipal()))
		{
			if (_loggingOptions.IsSensitiveDataLoggingEnabled)
			{
				_logger.ReferenceChangeDetectedSensitive(entry, navigation, relationshipSnapshotValue, obj);
			}
			else
			{
				_logger.ReferenceChangeDetected(entry, navigation, relationshipSnapshotValue, obj);
			}
			stateManager.InternalEntityEntryNotifier.NavigationReferenceChanged(entry, navigation, relationshipSnapshotValue, obj);
		}
	}
}
