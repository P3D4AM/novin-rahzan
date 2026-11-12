using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalEntityEntrySubscriber : IInternalEntityEntrySubscriber
{
	public virtual bool SnapshotAndSubscribe(InternalEntityEntry entry)
	{
		IEntityType entityType = entry.EntityType;
		if (entityType.UseEagerSnapshots())
		{
			entry.EnsureOriginalValues();
			entry.EnsureRelationshipSnapshot();
		}
		ChangeTrackingStrategy changeTrackingStrategy = entityType.GetChangeTrackingStrategy();
		if (changeTrackingStrategy == ChangeTrackingStrategy.Snapshot)
		{
			return false;
		}
		foreach (INavigation item in from n in entityType.GetNavigations()
			where n.IsCollection()
			select n)
		{
			AsINotifyCollectionChanged(entry, item, entityType, changeTrackingStrategy).CollectionChanged += entry.HandleINotifyCollectionChanged;
		}
		if (changeTrackingStrategy != ChangeTrackingStrategy.ChangedNotifications)
		{
			AsINotifyPropertyChanging(entry, entityType, changeTrackingStrategy).PropertyChanging += entry.HandleINotifyPropertyChanging;
		}
		AsINotifyPropertyChanged(entry, entityType, changeTrackingStrategy).PropertyChanged += entry.HandleINotifyPropertyChanged;
		return true;
	}

	public virtual void Unsubscribe(InternalEntityEntry entry)
	{
		IEntityType entityType = entry.EntityType;
		ChangeTrackingStrategy changeTrackingStrategy = entityType.GetChangeTrackingStrategy();
		if (changeTrackingStrategy == ChangeTrackingStrategy.Snapshot)
		{
			return;
		}
		foreach (INavigation item in from n in entityType.GetNavigations()
			where n.IsCollection()
			select n)
		{
			AsINotifyCollectionChanged(entry, item, entityType, changeTrackingStrategy).CollectionChanged -= entry.HandleINotifyCollectionChanged;
		}
		if (changeTrackingStrategy != ChangeTrackingStrategy.ChangedNotifications)
		{
			AsINotifyPropertyChanging(entry, entityType, changeTrackingStrategy).PropertyChanging -= entry.HandleINotifyPropertyChanging;
		}
		AsINotifyPropertyChanged(entry, entityType, changeTrackingStrategy).PropertyChanged -= entry.HandleINotifyPropertyChanged;
	}

	private static INotifyCollectionChanged AsINotifyCollectionChanged(InternalEntityEntry entry, INavigation navigation, IEntityType entityType, ChangeTrackingStrategy changeTrackingStrategy)
	{
		return (navigation.GetCollectionAccessor()?.GetOrCreate(entry.Entity, forMaterialization: false) as INotifyCollectionChanged) ?? throw new InvalidOperationException(CoreStrings.NonNotifyingCollection(navigation.Name, entityType.DisplayName(), changeTrackingStrategy));
	}

	private static INotifyPropertyChanged AsINotifyPropertyChanged(InternalEntityEntry entry, IEntityType entityType, ChangeTrackingStrategy changeTrackingStrategy)
	{
		return (entry.Entity as INotifyPropertyChanged) ?? throw new InvalidOperationException(CoreStrings.ChangeTrackingInterfaceMissing(entityType.DisplayName(), changeTrackingStrategy, "INotifyPropertyChanged"));
	}

	private static INotifyPropertyChanging AsINotifyPropertyChanging(InternalEntityEntry entry, IEntityType entityType, ChangeTrackingStrategy changeTrackingStrategy)
	{
		return (entry.Entity as INotifyPropertyChanging) ?? throw new InvalidOperationException(CoreStrings.ChangeTrackingInterfaceMissing(entityType.DisplayName(), changeTrackingStrategy, "INotifyPropertyChanging"));
	}
}
