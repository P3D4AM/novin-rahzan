using System;
using System.Data;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TransactionStartingEventData : DbContextEventData
{
	public virtual IsolationLevel IsolationLevel { get; }

	public virtual Guid TransactionId { get; }

	public virtual Guid ConnectionId { get; }

	public virtual bool IsAsync { get; }

	public virtual DateTimeOffset StartTime { get; }

	public TransactionStartingEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.CanBeNull] DbContext context, IsolationLevel isolationLevel, Guid transactionId, Guid connectionId, bool async, DateTimeOffset startTime)
		: base(eventDefinition, messageGenerator, context)
	{
		IsolationLevel = isolationLevel;
		TransactionId = transactionId;
		ConnectionId = connectionId;
		IsAsync = async;
		StartTime = startTime;
	}
}
