using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigrationScriptingEventData : MigrationEventData
{
	public virtual string FromMigration { get; }

	public virtual string ToMigration { get; }

	public virtual bool IsIdempotent { get; }

	public MigrationScriptingEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] Migration migration, [JetBrains.Annotations.CanBeNull] string fromMigration, [JetBrains.Annotations.CanBeNull] string toMigration, bool idempotent)
		: base(eventDefinition, messageGenerator, migrator, migration)
	{
		FromMigration = fromMigration;
		ToMigration = toMigration;
		IsIdempotent = idempotent;
	}
}
