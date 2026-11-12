using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ConnectionEndEventData : ConnectionEventData
{
	public virtual TimeSpan Duration { get; }

	public ConnectionEndEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] DbContext context, Guid connectionId, bool async, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, connection, context, connectionId, async, startTime)
	{
		Duration = duration;
	}
}
