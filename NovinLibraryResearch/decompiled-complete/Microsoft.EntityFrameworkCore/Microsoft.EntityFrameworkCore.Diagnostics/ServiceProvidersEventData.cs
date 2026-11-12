using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ServiceProvidersEventData : EventData
{
	public virtual ICollection<IServiceProvider> ServiceProviders { get; }

	public ServiceProvidersEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] ICollection<IServiceProvider> serviceProviders)
		: base(eventDefinition, messageGenerator)
	{
		ServiceProviders = serviceProviders;
	}
}
