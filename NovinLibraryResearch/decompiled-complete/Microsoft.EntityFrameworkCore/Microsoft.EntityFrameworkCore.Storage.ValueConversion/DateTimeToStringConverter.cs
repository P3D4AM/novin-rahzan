using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeToStringConverter : StringDateTimeConverter<DateTime, string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(DateTime), typeof(string), (ValueConverterInfo i) => new DateTimeToStringConverter(i.MappingHints), StringDateTimeConverter<DateTime, string>._defaultHints);


	public DateTimeToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringDateTimeConverter<DateTime, string>.ToString(), StringDateTimeConverter<DateTime, string>.ToDateTime(), StringDateTimeConverter<DateTime, string>._defaultHints.With(mappingHints))
	{
	}
}
