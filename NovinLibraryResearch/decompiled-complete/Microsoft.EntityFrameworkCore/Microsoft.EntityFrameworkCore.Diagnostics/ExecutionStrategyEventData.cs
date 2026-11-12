using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ExecutionStrategyEventData : EventData
{
	public virtual IReadOnlyList<Exception> ExceptionsEncountered { get; }

	public virtual TimeSpan Delay { get; }

	public virtual bool IsAsync { get; }

	public ExecutionStrategyEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IReadOnlyList<Exception> exceptionsEncountered, TimeSpan delay, bool async)
		: base(eventDefinition, messageGenerator)
	{
		ExceptionsEncountered = exceptionsEncountered;
		Delay = delay;
		IsAsync = async;
	}
}
