using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToDateTimeConverter : StringDateTimeConverter<string, DateTime>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(DateTime), (ValueConverterInfo i) => new StringToDateTimeConverter(i.MappingHints), StringDateTimeConverter<string, DateTime>._defaultHints);


	public StringToDateTimeConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringDateTimeConverter<string, DateTime>.ToDateTime(), StringDateTimeConverter<string, DateTime>.ToString(), StringDateTimeConverter<string, DateTime>._defaultHints.With(mappingHints))
	{
	}
}
