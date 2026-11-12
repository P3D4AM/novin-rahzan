using System;
using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class DataReaderDisposingEventData : DbContextEventData
{
	public virtual DbCommand Command { get; }

	public virtual DbDataReader DataReader { get; }

	public virtual Guid CommandId { get; }

	public virtual Guid ConnectionId { get; }

	public virtual int RecordsAffected { get; }

	public virtual int ReadCount { get; }

	public virtual DateTimeOffset StartTime { get; }

	public virtual TimeSpan Duration { get; }

	public DataReaderDisposingEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] DbDataReader dataReader, [JetBrains.Annotations.CanBeNull] DbContext context, Guid commandId, Guid connectionId, int recordsAffected, int readCount, DateTimeOffset startTime, TimeSpan duration)
		: base(eventDefinition, messageGenerator, context)
	{
		Command = command;
		DataReader = dataReader;
		CommandId = commandId;
		ConnectionId = connectionId;
		RecordsAffected = recordsAffected;
		ReadCount = readCount;
		StartTime = startTime;
		Duration = duration;
	}
}
