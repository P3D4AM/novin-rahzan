using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryDecimalValueGenerator : TemporaryNumberValueGenerator<decimal>
{
	private int _current = -2147482648;

	public override decimal Next(EntityEntry entry)
	{
		return Interlocked.Increment(ref _current);
	}
}
