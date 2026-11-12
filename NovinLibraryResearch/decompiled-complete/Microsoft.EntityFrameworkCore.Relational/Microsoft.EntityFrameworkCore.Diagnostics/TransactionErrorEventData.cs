using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TransactionErrorEventData : TransactionEndEventData, IErrorEventData
{
	public virtual string Action { get; }

	public virtual Exception Exception { get; }

	public TransactionErrorEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbTransaction transaction, [JetBrains.Annotations.CanBeNull] DbContext context, Guid transactionId, Guid connectionId, bool async, [JetBrains.Annotations.NotNull] string action, [JetBrains.Annotations.NotNull] Exception exception, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, transaction, context, transactionId, connectionId, async, startTime, duration)
	{
		Action = action;
		Exception = exception;
	}
}
