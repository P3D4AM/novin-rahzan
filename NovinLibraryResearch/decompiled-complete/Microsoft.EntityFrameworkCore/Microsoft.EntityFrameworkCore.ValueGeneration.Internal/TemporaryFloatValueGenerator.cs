using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryFloatValueGenerator : TemporaryNumberValueGenerator<float>
{
	private int _current = -2147482648;

	public override float Next(EntityEntry entry)
	{
		return Interlocked.Increment(ref _current);
	}
}
