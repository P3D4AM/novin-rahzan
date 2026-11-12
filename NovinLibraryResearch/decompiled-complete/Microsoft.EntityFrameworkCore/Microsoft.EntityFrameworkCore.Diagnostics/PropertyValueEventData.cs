using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class PropertyValueEventData : PropertyEventData
{
	public virtual EntityEntry EntityEntry { get; }

	public virtual object Value { get; }

	public PropertyValueEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] EntityEntry entityEntry, [JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.CanBeNull] object value)
		: base(eventDefinition, messageGenerator, property)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityEntry, "entityEntry");
		EntityEntry = entityEntry;
		Value = value;
	}
}
