using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class ValueConverterEventData : EventData
{
	public virtual Type MappingClrType { get; }

	public virtual ValueConverter ValueConverter { get; }

	public ValueConverterEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] Type mappingClrType, [JetBrains.Annotations.NotNull] ValueConverter valueConverter)
		: base(eventDefinition, messageGenerator)
	{
		MappingClrType = mappingClrType;
		ValueConverter = valueConverter;
	}
}
