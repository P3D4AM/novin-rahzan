using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ContextInitializedEventData : EventData
{
	public virtual DbContext Context { get; }

	public virtual DbContextOptions ContextOptions { get; }

	public ContextInitializedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] DbContextOptions contextOptions)
		: base(eventDefinition, messageGenerator)
	{
		Context = context;
		ContextOptions = contextOptions;
	}
}
