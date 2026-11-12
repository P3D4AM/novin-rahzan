using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CommandExecutedEventData : CommandEndEventData
{
	public virtual object Result { get; }

	public CommandExecutedEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, [JetBrains.Annotations.CanBeNull] object result, bool async, bool logParameterValues, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, connection, command, context, executeMethod, commandId, connectionId, async, logParameterValues, startTime, duration)
	{
		Result = result;
	}
}
