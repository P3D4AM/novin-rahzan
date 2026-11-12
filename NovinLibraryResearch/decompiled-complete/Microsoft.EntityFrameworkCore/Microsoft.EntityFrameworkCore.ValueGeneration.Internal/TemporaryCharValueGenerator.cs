using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

public class TemporaryCharValueGenerator : TemporaryNumberValueGenerator<char>
{
	private int _current = 65435;

	public override char Next(EntityEntry entry)
	{
		return (char)Interlocked.Decrement(ref _current);
	}
}
