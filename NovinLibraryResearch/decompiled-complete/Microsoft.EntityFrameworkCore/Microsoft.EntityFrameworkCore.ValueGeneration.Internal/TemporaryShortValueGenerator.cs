using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryShortValueGenerator : TemporaryNumberValueGenerator<short>
{
	private int _current = -32668;

	public override short Next(EntityEntry entry)
	{
		return (short)Interlocked.Increment(ref _current);
	}
}
