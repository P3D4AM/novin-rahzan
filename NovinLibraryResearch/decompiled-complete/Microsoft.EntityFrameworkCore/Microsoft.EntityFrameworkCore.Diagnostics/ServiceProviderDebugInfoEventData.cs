using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ServiceProviderDebugInfoEventData : EventData
{
	public virtual IDictionary<string, string> NewDebugInfo { get; }

	public virtual IList<IDictionary<string, string>> CachedDebugInfos { get; }

	public ServiceProviderDebugInfoEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IDictionary<string, string> newDebugInfo, [JetBrains.Annotations.NotNull] IList<IDictionary<string, string>> cachedDebugInfos)
		: base(eventDefinition, messageGenerator)
	{
		NewDebugInfo = newDebugInfo;
		CachedDebugInfos = cachedDebugInfos;
	}
}
