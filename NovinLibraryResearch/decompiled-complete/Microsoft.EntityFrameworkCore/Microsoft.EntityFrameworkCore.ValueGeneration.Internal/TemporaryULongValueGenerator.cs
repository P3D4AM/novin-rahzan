using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryULongValueGenerator : TemporaryNumberValueGenerator<ulong>
{
	private long _current = -9223372036854774808L;

	public override ulong Next(EntityEntry entry)
	{
		return (ulong)Interlocked.Increment(ref _current);
	}
}
