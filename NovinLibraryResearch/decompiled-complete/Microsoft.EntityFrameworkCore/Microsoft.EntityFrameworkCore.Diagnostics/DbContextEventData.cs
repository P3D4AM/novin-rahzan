using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class DbContextEventData : EventData
{
	public virtual DbContext Context { get; }

	public DbContextEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.CanBeNull] DbContext context)
		: base(eventDefinition, messageGenerator)
	{
		Context = context;
	}
}
