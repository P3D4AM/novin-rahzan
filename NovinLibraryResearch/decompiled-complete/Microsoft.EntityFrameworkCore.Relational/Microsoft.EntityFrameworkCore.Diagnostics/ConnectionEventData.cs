using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ConnectionEventData : DbContextEventData
{
	public virtual DbConnection Connection { get; }

	public virtual Guid ConnectionId { get; }

	public virtual bool IsAsync { get; }

	public virtual DateTimeOffset StartTime { get; }

	public ConnectionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] DbContext context, Guid connectionId, bool async, DateTimeOffset startTime)
		: base(eventDefinition, messageGenerator, context)
	{
		Connection = connection;
		ConnectionId = connectionId;
		IsAsync = async;
		StartTime = startTime;
	}
}
