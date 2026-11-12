using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public class DateTimeToTicksConverter : ValueConverter<DateTime, long>
{
	public DateTimeToTicksConverter([JetBrains.Annotations.CanBeNull] ConverterMappingHints mappingHints = null)
		: base((Expression<Func<DateTime, long>>)((DateTime v) => v.Ticks), (Expression<Func<long, DateTime>>)((long v) => new DateTime(v)), mappingHints)
	{
	}
}
