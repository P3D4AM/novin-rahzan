using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class UriToStringConverter : StringUriConverter<Uri, string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(Uri), typeof(string), (ValueConverterInfo i) => new UriToStringConverter(i.MappingHints));


	public UriToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringUriConverter<Uri, string>.ToString(), StringUriConverter<Uri, string>.ToUri(), mappingHints)
	{
	}
}
