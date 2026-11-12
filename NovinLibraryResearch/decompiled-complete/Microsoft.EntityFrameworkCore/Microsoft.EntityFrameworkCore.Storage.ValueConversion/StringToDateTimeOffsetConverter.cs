using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToDateTimeOffsetConverter : StringDateTimeOffsetConverter<string, DateTimeOffset>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(DateTimeOffset), (ValueConverterInfo i) => new StringToDateTimeOffsetConverter(i.MappingHints), StringDateTimeOffsetConverter<string, DateTimeOffset>._defaultHints);


	public StringToDateTimeOffsetConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringDateTimeOffsetConverter<string, DateTimeOffset>.ToDateTimeOffset(), StringDateTimeOffsetConverter<string, DateTimeOffset>.ToString(), StringDateTimeOffsetConverter<string, DateTimeOffset>._defaultHints.With(mappingHints))
	{
	}
}
