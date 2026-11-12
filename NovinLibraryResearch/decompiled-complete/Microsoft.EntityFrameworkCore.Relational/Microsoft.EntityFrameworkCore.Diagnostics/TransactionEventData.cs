using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TransactionEventData : DbContextEventData
{
	public virtual DbTransaction Transaction { get; }

	public virtual Guid TransactionId { get; }

	public virtual Guid ConnectionId { get; }

	public virtual bool IsAsync { get; }

	public virtual DateTimeOffset StartTime { get; }

	public TransactionEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.CanBeNull] DbContext context, Guid transactionId, Guid connectionId, bool async, DateTimeOffset startTime)
		: base(eventDefinition, messageGenerator, context)
	{
		Transaction = transaction;
		TransactionId = transactionId;
		ConnectionId = connectionId;
		IsAsync = async;
		StartTime = startTime;
	}
}
