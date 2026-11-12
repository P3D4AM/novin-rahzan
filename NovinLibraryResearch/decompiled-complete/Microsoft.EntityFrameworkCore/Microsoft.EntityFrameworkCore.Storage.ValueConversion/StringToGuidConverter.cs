using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class StringToGuidConverter : StringGuidConverter<string, Guid>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(string), typeof(Guid), (ValueConverterInfo i) => new StringToGuidConverter(i.MappingHints), StringGuidConverter<string, Guid>._defaultHints);


	public StringToGuidConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringGuidConverter<string, Guid>.ToGuid(), StringGuidConverter<string, Guid>.ToString(), StringGuidConverter<string, Guid>._defaultHints.With(mappingHints))
	{
	}
}
