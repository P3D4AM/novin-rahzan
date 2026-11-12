using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public sealed class ValueGeneratorSelectorDependencies
{
	public IValueGeneratorCache Cache { get; }

	[EntityFrameworkInternal]
	public ValueGeneratorSelectorDependencies([JetBrains.Annotations.NotNull] IValueGeneratorCache cache)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(cache, "cache");
		Cache = cache;
	}

	public ValueGeneratorSelectorDependencies With([JetBrains.Annotations.NotNull] IValueGeneratorCache cache)
	{
		return new ValueGeneratorSelectorDependencies(cache);
	}
}
