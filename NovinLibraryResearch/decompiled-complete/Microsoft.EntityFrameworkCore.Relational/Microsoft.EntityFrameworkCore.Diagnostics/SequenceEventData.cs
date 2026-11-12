using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class SequenceEventData : EventData
{
	public virtual ISequence Sequence { get; }

	public SequenceEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] ISequence sequence)
		: base(eventDefinition, messageGenerator)
	{
		Sequence = sequence;
	}
}
