using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public abstract class ValueGenerator
{
	public abstract bool GeneratesTemporaryValues { get; }

	public virtual object Next([JetBrains.Annotations.NotNull] EntityEntry entry)
	{
		return NextValue(entry);
	}

	protected abstract object NextValue([JetBrains.Annotations.NotNull] EntityEntry entry);

	public virtual ValueTask<object> NextAsync([JetBrains.Annotations.NotNull] EntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return NextValueAsync(entry, cancellationToken);
	}

	protected virtual ValueTask<object> NextValueAsync([JetBrains.Annotations.NotNull] EntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new ValueTask<object>(NextValue(entry));
	}
}
public abstract class ValueGenerator<TValue> : ValueGenerator
{
	public new abstract TValue Next([JetBrains.Annotations.NotNull] EntityEntry entry);

	public new virtual ValueTask<TValue> NextAsync([JetBrains.Annotations.NotNull] EntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new ValueTask<TValue>(Next(entry));
	}

	protected override object NextValue(EntityEntry entry)
	{
		return Next(entry);
	}

	protected override ValueTask<object> NextValueAsync(EntityEntry entry, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new ValueTask<object>(Next(entry));
	}
}
