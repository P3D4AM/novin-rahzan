using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CollectionChangedEventData : NavigationEventData
{
	public virtual EntityEntry EntityEntry { get; }

	public virtual IEnumerable<object> Added { get; }

	public virtual IEnumerable<object> Removed { get; }

	public CollectionChangedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.NotNull] IEnumerable<object> added, [JetBrains.Annotations.NotNull] IEnumerable<object> removed)
		: base(eventDefinition, messageGenerator, navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityEntry, "entityEntry");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(added, "added");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(removed, "removed");
		EntityEntry = entityEntry;
		Added = added;
		Removed = removed;
	}
}
