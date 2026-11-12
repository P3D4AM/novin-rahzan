using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MigratorConnectionEventData : MigratorEventData
{
	public virtual DbConnection Connection { get; }

	public virtual Guid ConnectionId { get; }

	public MigratorConnectionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IMigrator migrator, [JetBrains.Annotations.NotNull] DbConnection connection, Guid connectionId)
		: base(eventDefinition, messageGenerator, migrator)
	{
		Connection = connection;
		ConnectionId = connectionId;
	}
}
