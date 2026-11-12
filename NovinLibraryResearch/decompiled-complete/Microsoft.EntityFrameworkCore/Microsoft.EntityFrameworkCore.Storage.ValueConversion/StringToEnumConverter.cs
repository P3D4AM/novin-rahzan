using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToEnumConverter<TEnum> : StringEnumConverter<string, TEnum, TEnum> where TEnum : struct
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(TEnum), (ValueConverterInfo i) => new StringToEnumConverter<TEnum>(i.MappingHints));


	public StringToEnumConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringEnumConverter<string, TEnum, TEnum>.ToEnum(), StringEnumConverter<string, TEnum, TEnum>.ToString(), mappingHints)
	{
	}
}
