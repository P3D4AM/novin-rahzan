using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class CharToStringConverter : StringCharConverter<char, string>
{
	private static readonly ConverterMappingHints _defaultHints = new ConverterMappingHints(1);

	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(char), typeof(string), (ValueConverterInfo i) => new CharToStringConverter(i.MappingHints), _defaultHints);


	public CharToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringCharConverter<char, string>.ToString(), StringCharConverter<char, string>.ToChar(), _defaultHints.With(mappingHints))
	{
	}
}
