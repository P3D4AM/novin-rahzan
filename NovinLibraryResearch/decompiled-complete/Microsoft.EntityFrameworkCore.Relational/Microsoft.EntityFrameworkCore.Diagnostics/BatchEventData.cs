using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class BatchEventData : EventData
{
	public virtual IEnumerable<IUpdateEntry> Entries { get; }

	public virtual int CommandCount { get; }

	public BatchEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEnumerable<IUpdateEntry> entries, int commandCount)
		: base(eventDefinition, messageGenerator)
	{
		Entries = entries;
		CommandCount = commandCount;
	}
}
