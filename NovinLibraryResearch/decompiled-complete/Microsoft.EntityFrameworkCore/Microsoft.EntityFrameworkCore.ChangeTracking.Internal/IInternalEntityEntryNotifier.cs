using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface IInternalEntityEntryNotifier
{
	void StateChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState newState);

	void StateChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState oldState, bool fromQuery);

	void TrackedFromQuery([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void NavigationReferenceChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue);

	void NavigationCollectionChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] IEnumerable<object> added, [JetBrains.Annotations.NotNull] IEnumerable<object> removed);

	void KeyPropertyChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IReadOnlyList<IKey> keys, [JetBrains.Annotations.NotNull] IReadOnlyList<IForeignKey> foreignKeys, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue);

	void PropertyChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IPropertyBase property, bool setModified);

	void PropertyChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IPropertyBase property);
}
