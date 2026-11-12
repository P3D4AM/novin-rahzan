using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public class InternalEntityEntryNotifier : IInternalEntityEntryNotifier
{
	private readonly ILocalViewListener _localViewListener;

	private readonly IChangeDetector _changeDetector;

	private readonly INavigationFixer _navigationFixer;

	public InternalEntityEntryNotifier([JetBrains.Annotations.CanBeNull] ILocalViewListener localViewListener, [JetBrains.Annotations.CanBeNull] IChangeDetector changeDetector, [JetBrains.Annotations.CanBeNull] INavigationFixer navigationFixer)
	{
		_localViewListener = localViewListener;
		_changeDetector = changeDetector;
		_navigationFixer = navigationFixer;
	}

	public virtual void StateChanging(InternalEntityEntry entry, EntityState newState)
	{
		_navigationFixer.StateChanging(entry, newState);
		_localViewListener.StateChanging(entry, newState);
	}

	public virtual void StateChanged(InternalEntityEntry entry, EntityState oldState, bool fromQuery)
	{
		_navigationFixer.StateChanged(entry, oldState, fromQuery);
		_localViewListener.StateChanged(entry, oldState, fromQuery);
	}

	public virtual void TrackedFromQuery(InternalEntityEntry entry)
	{
		_navigationFixer.TrackedFromQuery(entry);
	}

	public virtual void NavigationReferenceChanged(InternalEntityEntry entry, INavigation navigation, object oldValue, object newValue)
	{
		_navigationFixer.NavigationReferenceChanged(entry, navigation, oldValue, newValue);
	}

	public virtual void NavigationCollectionChanged(InternalEntityEntry entry, INavigation navigation, IEnumerable<object> added, IEnumerable<object> removed)
	{
		_navigationFixer.NavigationCollectionChanged(entry, navigation, added, removed);
	}

	public virtual void KeyPropertyChanged(InternalEntityEntry entry, IProperty property, IReadOnlyList<IKey> keys, IReadOnlyList<IForeignKey> foreignKeys, object oldValue, object newValue)
	{
		_navigationFixer.KeyPropertyChanged(entry, property, keys, foreignKeys, oldValue, newValue);
	}

	public virtual void PropertyChanged(InternalEntityEntry entry, IPropertyBase property, bool setModified)
	{
		_changeDetector.PropertyChanged(entry, property, setModified);
	}

	public virtual void PropertyChanging(InternalEntityEntry entry, IPropertyBase property)
	{
		_changeDetector.PropertyChanging(entry, property);
	}
}
