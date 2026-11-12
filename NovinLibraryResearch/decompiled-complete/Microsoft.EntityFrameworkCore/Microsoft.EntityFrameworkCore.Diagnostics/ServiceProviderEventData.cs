using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ServiceProviderEventData : EventData
{
	public virtual IServiceProvider ServiceProvider { get; }

	public ServiceProviderEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IServiceProvider serviceProvider)
		: base(eventDefinition, messageGenerator)
	{
		ServiceProvider = serviceProvider;
	}
}
