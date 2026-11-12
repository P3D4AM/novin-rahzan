using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CascadeDeleteOrphanEventData : EntityEntryEventData
{
	public virtual EntityState State { get; }

	public virtual IEntityType ParentEntityType { get; }

	public CascadeDeleteOrphanEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, [JetBrains.Annotations.NotNull] IEntityType parentEntityTypes, EntityState state)
		: base(eventDefinition, messageGenerator, entityEntry)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(parentEntityTypes, "parentEntityTypes");
		ParentEntityType = parentEntityTypes;
		State = state;
	}
}
