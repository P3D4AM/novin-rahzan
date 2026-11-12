using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class TimeSpanToStringConverter : StringTimeSpanConverter<TimeSpan, string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TimeSpan), typeof(string), (ValueConverterInfo i) => new TimeSpanToStringConverter(i.MappingHints), StringTimeSpanConverter<TimeSpan, string>._defaultHints);


	public TimeSpanToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringTimeSpanConverter<TimeSpan, string>.ToString(), StringTimeSpanConverter<TimeSpan, string>.ToTimeSpan(), StringTimeSpanConverter<TimeSpan, string>._defaultHints.With(mappingHints))
	{
	}
}
