using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigrationTypeEventData : EventData
{
	public virtual TypeInfo MigrationType { get; }

	public MigrationTypeEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] TypeInfo migrationType)
		: base(eventDefinition, messageGenerator)
	{
		MigrationType = migrationType;
	}
}
