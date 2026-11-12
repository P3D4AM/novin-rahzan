using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public abstract class ValueGeneratorFactory
{
	public abstract ValueGenerator Create([JetBrains.Annotations.NotNull] IProperty property);
}
