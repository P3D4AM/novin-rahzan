using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CommandEndEventData : CommandEventData
{
	public virtual TimeSpan Duration { get; }

	public CommandEndEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, bool logParameterValues, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, connection, command, context, executeMethod, commandId, connectionId, async, logParameterValues, startTime)
	{
		Duration = duration;
	}
}
