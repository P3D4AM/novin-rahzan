using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class SharedDependentEntityEventData : EventData
{
	public virtual IEntityType FirstEntityType { get; }

	public virtual IEntityType SecondEntityType { get; }

	public SharedDependentEntityEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEntityType firstEntityType, [JetBrains.Annotations.NotNull] IEntityType secondEntityType)
		: base(eventDefinition, messageGenerator)
	{
		FirstEntityType = firstEntityType;
		SecondEntityType = secondEntityType;
	}
}
