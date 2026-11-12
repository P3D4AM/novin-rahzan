using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporarySByteValueGenerator : TemporaryNumberValueGenerator<sbyte>
{
	private int _current = -128;

	public override sbyte Next(EntityEntry entry)
	{
		return (sbyte)Interlocked.Increment(ref _current);
	}
}
