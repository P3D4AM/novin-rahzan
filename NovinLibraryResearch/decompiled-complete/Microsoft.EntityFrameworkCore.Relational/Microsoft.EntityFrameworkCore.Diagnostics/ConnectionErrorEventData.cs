using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ConnectionErrorEventData : ConnectionEndEventData, IErrorEventData
{
	public virtual Exception Exception { get; }

	public ConnectionErrorEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] DbContext context, Guid connectionId, [JetBrains.Annotations.NotNull] Exception exception, bool async, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, connection, context, connectionId, async, startTime, duration)
	{
		Exception = exception;
	}
}
