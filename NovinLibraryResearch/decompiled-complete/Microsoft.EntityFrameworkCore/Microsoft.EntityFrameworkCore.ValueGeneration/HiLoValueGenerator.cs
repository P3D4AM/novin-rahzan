using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public abstract class HiLoValueGenerator<TValue> : ValueGenerator<TValue>
{
	private readonly HiLoValueGeneratorState _generatorState;

	protected HiLoValueGenerator([JetBrains.Annotations.NotNull] HiLoValueGeneratorState generatorState)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(generatorState, "generatorState");
		_generatorState = generatorState;
	}

	public override TValue Next(EntityEntry entry)
	{
		return _generatorState.Next<TValue>(GetNewLowValue);
	}

	public override ValueTask<TValue> NextAsync(EntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return _generatorState.NextAsync<TValue>(GetNewLowValueAsync);
	}

	protected abstract long GetNewLowValue();

	protected virtual Task<long> GetNewLowValueAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(GetNewLowValue());
	}
}
