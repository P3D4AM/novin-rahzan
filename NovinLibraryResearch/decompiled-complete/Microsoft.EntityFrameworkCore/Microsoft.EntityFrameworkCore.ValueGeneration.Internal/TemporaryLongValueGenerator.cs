using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryLongValueGenerator : TemporaryNumberValueGenerator<long>
{
	private long _current = -9223372036854774808L;

	public override long Next(EntityEntry entry)
	{
		return Interlocked.Increment(ref _current);
	}
}
