using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryUIntValueGenerator : TemporaryNumberValueGenerator<uint>
{
	private int _current = -2147482648;

	public override uint Next(EntityEntry entry)
	{
		return (uint)Interlocked.Increment(ref _current);
	}
}
