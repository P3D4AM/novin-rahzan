using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigratorEventData : EventData
{
	public virtual IMigrator Migrator { get; }

	public MigratorEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IMigrator migrator)
		: base(eventDefinition, messageGenerator)
	{
		Migrator = migrator;
	}
}
