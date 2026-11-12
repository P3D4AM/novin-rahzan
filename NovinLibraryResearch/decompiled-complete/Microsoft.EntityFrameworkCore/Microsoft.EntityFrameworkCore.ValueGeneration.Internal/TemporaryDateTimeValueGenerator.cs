using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryDateTimeValueGenerator : ValueGenerator<DateTime>
{
	private long _current;

	public override bool GeneratesTemporaryValues => true;

	public override DateTime Next(EntityEntry entry)
	{
		return new DateTime(Interlocked.Increment(ref _current));
	}
}
