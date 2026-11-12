using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeOffsetToBinaryConverter : ValueConverter<DateTimeOffset, long>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(DateTimeOffset), typeof(long), (ValueConverterInfo i) => new DateTimeOffsetToBinaryConverter(i.MappingHints));


	public DateTimeOffsetToBinaryConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<DateTimeOffset, long>>)((DateTimeOffset v) => (v.Ticks / 1000 << 11) | ((long)v.Offset.TotalMinutes & 0x7FF)), (Expression<Func<long, DateTimeOffset>>)((long v) => new DateTimeOffset(new DateTime((v >> 11) * 1000), new TimeSpan(0, (int)(v << 53 >> 53), 0))), mappingHints)
	{
	}
}
