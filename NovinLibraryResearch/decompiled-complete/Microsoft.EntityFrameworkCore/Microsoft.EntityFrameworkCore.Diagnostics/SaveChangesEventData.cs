using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class SaveChangesEventData : EventData
{
	public virtual IEnumerable<IUpdateEntry> Entries { get; }

	public virtual int RowsAffected { get; }

	public SaveChangesEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEnumerable<IUpdateEntry> entries, int rowsAffected)
		: base(eventDefinition, messageGenerator)
	{
		Entries = entries;
		RowsAffected = rowsAffected;
	}
}
