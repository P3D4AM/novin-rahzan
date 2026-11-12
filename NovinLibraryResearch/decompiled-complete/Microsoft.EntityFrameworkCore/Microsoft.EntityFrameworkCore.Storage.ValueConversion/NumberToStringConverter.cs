using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class NumberToStringConverter<TNumber> : StringNumberConverter<TNumber, string, TNumber>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TNumber), typeof(string), (ValueConverterInfo i) => new NumberToStringConverter<TNumber>(i.MappingHints), StringNumberConverter<TNumber, string, TNumber>._defaultHints);


	public NumberToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringNumberConverter<TNumber, string, TNumber>.ToString(), StringNumberConverter<TNumber, string, TNumber>.ToNumber(), StringNumberConverter<TNumber, string, TNumber>._defaultHints.With(mappingHints))
	{
	}
}
