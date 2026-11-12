using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToNumberConverter<TNumber> : StringNumberConverter<string, TNumber, TNumber>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(TNumber), (ValueConverterInfo i) => new StringToNumberConverter<TNumber>(i.MappingHints), StringNumberConverter<string, TNumber, TNumber>._defaultHints);


	public StringToNumberConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringNumberConverter<string, TNumber, TNumber>.ToNumber(), StringNumberConverter<string, TNumber, TNumber>.ToString(), StringNumberConverter<string, TNumber, TNumber>._defaultHints.With(mappingHints))
	{
	}
}
