using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class DbContextTypeEventData : EventData
{
	public virtual Type ContextType { get; }

	public DbContextTypeEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Type contextType)
		: base(eventDefinition, messageGenerator)
	{
		ContextType = contextType;
	}
}
