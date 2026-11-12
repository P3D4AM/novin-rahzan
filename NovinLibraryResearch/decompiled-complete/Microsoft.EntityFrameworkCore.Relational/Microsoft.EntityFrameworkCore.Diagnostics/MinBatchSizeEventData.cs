using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class MinBatchSizeEventData : BatchEventData
{
	public virtual int MinBatchSize { get; }

	public MinBatchSizeEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEnumerable<IUpdateEntry> entries, int commandCount, int minBatchSize)
		: base(eventDefinition, messageGenerator, entries, commandCount)
	{
		MinBatchSize = minBatchSize;
	}
}
