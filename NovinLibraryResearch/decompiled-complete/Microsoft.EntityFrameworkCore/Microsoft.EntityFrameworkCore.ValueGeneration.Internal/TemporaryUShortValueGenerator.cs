using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryUShortValueGenerator : TemporaryNumberValueGenerator<ushort>
{
	private int _current = -32668;

	public override ushort Next(EntityEntry entry)
	{
		return (ushort)Interlocked.Increment(ref _current);
	}
}
