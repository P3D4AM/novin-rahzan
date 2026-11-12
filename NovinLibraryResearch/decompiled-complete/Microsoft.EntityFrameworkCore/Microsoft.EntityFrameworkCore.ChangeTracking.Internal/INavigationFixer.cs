using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

public interface INavigationFixer
{
	void NavigationReferenceChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue);

	void NavigationCollectionChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] IEnumerable<object> added, [JetBrains.Annotations.NotNull] IEnumerable<object> removed);

	void TrackedFromQuery([JetBrains.Annotations.NotNull] InternalEntityEntry entry);

	void KeyPropertyChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IReadOnlyList<IKey> containingPrincipalKeys, [JetBrains.Annotations.NotNull] IReadOnlyList<IForeignKey> containingForeignKeys, [JetBrains.Annotations.CanBeNull] object oldValue, [JetBrains.Annotations.CanBeNull] object newValue);

	void StateChanging([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState newState);

	void StateChanged([JetBrains.Annotations.NotNull] InternalEntityEntry entry, EntityState oldState, bool fromQuery);
}
