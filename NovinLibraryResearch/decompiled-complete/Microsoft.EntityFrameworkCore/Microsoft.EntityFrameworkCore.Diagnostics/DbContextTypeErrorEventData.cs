using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class DbContextTypeErrorEventData : DbContextTypeEventData, IErrorEventData
{
	public virtual Exception Exception { get; }

	public DbContextTypeErrorEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.NotNull] Exception exception)
		: base(eventDefinition, messageGenerator, contextType)
	{
		Exception = exception;
	}
}
