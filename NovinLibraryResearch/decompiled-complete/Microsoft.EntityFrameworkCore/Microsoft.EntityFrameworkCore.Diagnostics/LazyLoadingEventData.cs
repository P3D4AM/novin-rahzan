using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class LazyLoadingEventData : DbContextEventData
{
	public virtual object Entity { get; }

	public virtual string NavigationPropertyName { get; }

	public LazyLoadingEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] DbContext context, [JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] string navigationPropertyName)
		: base(eventDefinition, messageGenerator, context)
	{
		Entity = entity;
		NavigationPropertyName = navigationPropertyName;
	}
}
