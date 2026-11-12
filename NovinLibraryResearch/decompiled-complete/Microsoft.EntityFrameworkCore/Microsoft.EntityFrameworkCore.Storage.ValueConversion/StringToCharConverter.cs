using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToCharConverter : StringCharConverter<string, char>
{
	private static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(1);

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(char), (ValueConverterInfo i) => new StringToCharConverter(i.MappingHints), _defaultHints);


	public StringToCharConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringCharConverter<string, char>.ToChar(), StringCharConverter<string, char>.ToString(), _defaultHints.With(mappingHints))
	{
	}
}
