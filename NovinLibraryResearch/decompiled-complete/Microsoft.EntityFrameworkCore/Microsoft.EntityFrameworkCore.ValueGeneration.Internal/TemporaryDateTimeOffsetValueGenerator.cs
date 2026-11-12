using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryDateTimeOffsetValueGenerator : ValueGenerator<DateTimeOffset>
{
	private long _current;

	public override bool GeneratesTemporaryValues => true;

	public override DateTimeOffset Next(EntityEntry entry)
	{
		return new DateTimeOffset(Interlocked.Increment(ref _current), TimeSpan.Zero);
	}
}
