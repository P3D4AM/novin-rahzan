using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToUriConverter : StringUriConverter<string, Uri>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(Uri), (ValueConverterInfo i) => new StringToUriConverter(i.MappingHints));


	public StringToUriConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringUriConverter<string, Uri>.ToUri(), StringUriConverter<string, Uri>.ToString(), mappingHints)
	{
	}
}
