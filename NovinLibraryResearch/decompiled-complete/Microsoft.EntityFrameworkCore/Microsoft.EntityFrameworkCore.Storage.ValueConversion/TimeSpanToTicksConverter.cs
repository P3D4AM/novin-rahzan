using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class TimeSpanToTicksConverter : ValueConverter<TimeSpan, long>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(TimeSpan), typeof(long), (ValueConverterInfo i) => new TimeSpanToTicksConverter(i.MappingHints));


	public TimeSpanToTicksConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<TimeSpan, long>>)((TimeSpan v) => v.Ticks), (Expression<Func<long, TimeSpan>>)((long v) => new TimeSpan(v)), mappingHints)
	{
	}
}
