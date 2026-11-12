using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeToBinaryConverter : ValueConverter<DateTime, long>
{
	public static ValueConverterInfo DefaultInfo { get; } = new ValueConverterInfo(typeof(DateTime), typeof(long), (ValueConverterInfo i) => new DateTimeToBinaryConverter(i.MappingHints));


	public DateTimeToBinaryConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<DateTime, long>>)((DateTime v) => v.ToBinary()), (Expression<Func<long, DateTime>>)((long v) => DateTime.FromBinary(v)), mappingHints)
	{
	}
}
