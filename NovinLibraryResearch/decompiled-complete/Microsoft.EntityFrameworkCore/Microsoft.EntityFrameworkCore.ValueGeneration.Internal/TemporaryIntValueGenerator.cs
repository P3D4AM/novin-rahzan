using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryIntValueGenerator : TemporaryNumberValueGenerator<int>
{
	private int _current = -2147482648;

	public override int Next(EntityEntry entry)
	{
		return Interlocked.Increment(ref _current);
	}
}
