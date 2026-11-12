using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class EntityTypeSchemaEventData : EventData
{
	public virtual IEntityType EntityType { get; }

	public virtual string Schema { get; }

	public EntityTypeSchemaEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] string schema)
		: base(eventDefinition, messageGenerator)
	{
		EntityType = entityType;
		Schema = schema;
	}
}
