using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class GuidToStringConverter : StringGuidConverter<Guid, string>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(Guid), typeof(string), (ValueConverterInfo i) => new GuidToStringConverter(i.MappingHints), StringGuidConverter<Guid, string>._defaultHints);


	public GuidToStringConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base(StringGuidConverter<Guid, string>.ToString(), StringGuidConverter<Guid, string>.ToGuid(), StringGuidConverter<Guid, string>._defaultHints.With(mappingHints))
	{
	}
}
