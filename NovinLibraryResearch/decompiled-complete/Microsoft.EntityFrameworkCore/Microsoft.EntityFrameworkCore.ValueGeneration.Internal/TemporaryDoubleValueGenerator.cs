using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryDoubleValueGenerator : TemporaryNumberValueGenerator<double>
{
	private int _current = -2147482648;

	public override double Next(EntityEntry entry)
	{
		return Interlocked.Increment(ref _current);
	}
}
