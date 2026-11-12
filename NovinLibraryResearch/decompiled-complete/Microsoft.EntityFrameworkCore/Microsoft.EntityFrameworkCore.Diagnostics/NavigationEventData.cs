using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class NavigationEventData : EventData
{
	public virtual INavigation Navigation { get; }

	public NavigationEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] INavigation navigation)
		: base(eventDefinition, messageGenerator)
	{
		Navigation = navigation;
	}
}
