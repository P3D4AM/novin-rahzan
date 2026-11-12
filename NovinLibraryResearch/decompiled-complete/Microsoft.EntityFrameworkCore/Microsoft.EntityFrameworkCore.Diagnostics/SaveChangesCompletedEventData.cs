using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class SaveChangesCompletedEventData : DbContextEventData
{
	public virtual int EntitiesSavedCount { get; }

	public SaveChangesCompletedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbContext context, int entitiesSavedCount)
		: base(eventDefinition, messageGenerator, context)
	{
		EntitiesSavedCount = entitiesSavedCount;
	}
}
