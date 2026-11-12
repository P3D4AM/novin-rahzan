using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ReferenceChangedEventData : NavigationEventData
{
	public virtual EntityEntry EntityEntry { get; }

	public virtual object OldReferencedEntity { get; }

	public virtual object NewReferencedEntity { get; }

	public ReferenceChangedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, [JetBrains.Annotations.NotNull] INavigation navigation, [JetBrains.Annotations.CanBeNull] object oldReferencedEntity, [JetBrains.Annotations.CanBeNull] object newReferencedEntity)
		: base(eventDefinition, messageGenerator, navigation)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityEntry, "entityEntry");
		EntityEntry = entityEntry;
		OldReferencedEntity = oldReferencedEntity;
		NewReferencedEntity = newReferencedEntity;
	}
}
