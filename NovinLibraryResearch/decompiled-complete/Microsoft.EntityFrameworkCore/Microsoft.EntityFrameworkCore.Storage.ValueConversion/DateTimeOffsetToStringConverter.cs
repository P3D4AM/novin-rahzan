using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeOffsetToStringConverter : StringDateTimeOffsetConverter<DateTimeOffset, string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(DateTimeOffset), typeof(string), (ValueConverterInfo i) => new DateTimeOffsetToStringConverter(i.MappingHints), StringDateTimeOffsetConverter<DateTimeOffset, string>._defaultHints);


	public DateTimeOffsetToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringDateTimeOffsetConverter<DateTimeOffset, string>.ToString(), StringDateTimeOffsetConverter<DateTimeOffset, string>.ToDateTimeOffset(), StringDateTimeOffsetConverter<DateTimeOffset, string>._defaultHints.With(mappingHints))
	{
	}
}
