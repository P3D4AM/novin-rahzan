using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CommandCorrelatedEventData : DbContextEventData
{
	public virtual DbConnection Connection { get; }

	public virtual Guid CommandId { get; }

	public virtual Guid ConnectionId { get; }

	public virtual DbCommandMethod ExecuteMethod { get; }

	public virtual bool IsAsync { get; }

	public virtual DateTimeOffset StartTime { get; }

	public CommandCorrelatedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, DateTimeOffset startTime)
		: base(eventDefinition, messageGenerator, context)
	{
		Connection = connection;
		CommandId = commandId;
		ConnectionId = connectionId;
		ExecuteMethod = executeMethod;
		IsAsync = async;
		StartTime = startTime;
	}
}
