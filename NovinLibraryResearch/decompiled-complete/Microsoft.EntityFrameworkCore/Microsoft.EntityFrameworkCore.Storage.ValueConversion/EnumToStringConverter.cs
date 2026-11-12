using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class EnumToStringConverter<TEnum> : StringEnumConverter<TEnum, string, TEnum> where TEnum : struct
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TEnum), typeof(string), (ValueConverterInfo i) => new EnumToStringConverter<TEnum>(i.MappingHints));


	public EnumToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringEnumConverter<TEnum, string, TEnum>.ToString(), StringEnumConverter<TEnum, string, TEnum>.ToEnum(), mappingHints)
	{
	}
}
