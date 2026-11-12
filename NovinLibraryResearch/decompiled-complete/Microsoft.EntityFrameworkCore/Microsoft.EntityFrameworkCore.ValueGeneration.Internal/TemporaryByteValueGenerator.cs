using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryByteValueGenerator : TemporaryNumberValueGenerator<byte>
{
	private int _current;

	public override byte Next(EntityEntry entry)
	{
		return (byte)Interlocked.Decrement(ref _current);
	}
}
