using System;
using System.Data.Common;
using System.Transactions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TransactionEnlistedEventData : EventData
{
	public virtual Transaction Transaction { get; }

	public virtual DbConnection Connection { get; }

	public virtual Guid ConnectionId { get; }

	public TransactionEnlistedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Transaction transaction, [JetBrains.Annotations.NotNull] DbConnection connection, Guid connectionId)
		: base(eventDefinition, messageGenerator)
	{
		Transaction = transaction;
		Connection = connection;
		ConnectionId = connectionId;
	}
}
