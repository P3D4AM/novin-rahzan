using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigrationAssemblyEventData : MigratorEventData
{
	public virtual IMigrationsAssembly MigrationsAssembly { get; }

	public MigrationAssemblyEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] IMigrationsAssembly migrationsAssembly)
		: base(eventDefinition, messageGenerator, migrator)
	{
		MigrationsAssembly = migrationsAssembly;
	}
}
