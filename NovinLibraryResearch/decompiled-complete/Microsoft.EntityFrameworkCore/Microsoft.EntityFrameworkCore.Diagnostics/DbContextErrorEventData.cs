using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class DbContextErrorEventData : DbContextEventData, IErrorEventData
{
	public virtual Exception Exception { get; }

	public DbContextErrorEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] Exception exception)
		: base(eventDefinition, messageGenerator, context)
	{
		Exception = exception;
	}
}
