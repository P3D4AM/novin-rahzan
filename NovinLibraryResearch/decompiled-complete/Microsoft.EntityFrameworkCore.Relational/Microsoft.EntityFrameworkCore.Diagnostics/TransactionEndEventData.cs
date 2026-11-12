using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TransactionEndEventData : TransactionEventData
{
	public virtual TimeSpan Duration { get; }

	public TransactionEndEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.CanBeNull] DbContext context, Guid transactionId, Guid connectionId, bool async, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, transaction, context, transactionId, connectionId, async, startTime)
	{
		Duration = duration;
	}
}
