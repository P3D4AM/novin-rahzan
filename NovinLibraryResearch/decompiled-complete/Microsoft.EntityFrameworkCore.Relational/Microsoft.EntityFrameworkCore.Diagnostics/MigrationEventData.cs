using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigrationEventData : MigratorEventData
{
	public virtual Migration Migration { get; }

	public MigrationEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration)
		: base(eventDefinition, messageGenerator, migrator)
	{
		Migration = migration;
	}
}
