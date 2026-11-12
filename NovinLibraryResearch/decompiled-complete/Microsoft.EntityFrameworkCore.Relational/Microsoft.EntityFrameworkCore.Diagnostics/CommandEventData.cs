using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class CommandEventData : CommandCorrelatedEventData
{
	public virtual DbCommand Command { get; }

	public virtual bool LogParameterValues { get; }

	public CommandEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.CanBeNull] DbContext context, DbCommandMethod executeMethod, Guid commandId, Guid connectionId, bool async, bool logParameterValues, DateTimeOffset startTime)
		: base(eventDefinition, messageGenerator, connection, context, executeMethod, commandId, connectionId, async, startTime)
	{
		Command = command;
		LogParameterValues = logParameterValues;
	}
}
