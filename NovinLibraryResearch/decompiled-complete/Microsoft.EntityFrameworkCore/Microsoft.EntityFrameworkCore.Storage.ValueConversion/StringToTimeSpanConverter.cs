using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToTimeSpanConverter : StringTimeSpanConverter<string, TimeSpan>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(TimeSpan), (ValueConverterInfo i) => new StringToTimeSpanConverter(i.MappingHints), StringTimeSpanConverter<string, TimeSpan>._defaultHints);


	public StringToTimeSpanConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringTimeSpanConverter<string, TimeSpan>.ToTimeSpan(), StringTimeSpanConverter<string, TimeSpan>.ToString(), StringTimeSpanConverter<string, TimeSpan>._defaultHints.With(mappingHints))
	{
	}
}
