using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TwoPropertyBaseCollectionsEventData : EventData
{
	public virtual IReadOnlyList<IPropertyBase> FirstPropertyCollection { get; }

	public virtual IReadOnlyList<IPropertyBase> SecondPropertyCollection { get; }

	public TwoPropertyBaseCollectionsEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> firstPropertyCollection, [JetBrains.Annotations.NotNull] IReadOnlyList<IPropertyBase> secondPropertyCollection)
		: base(eventDefinition, messageGenerator)
	{
		FirstPropertyCollection = firstPropertyCollection;
		SecondPropertyCollection = secondPropertyCollection;
	}
}
