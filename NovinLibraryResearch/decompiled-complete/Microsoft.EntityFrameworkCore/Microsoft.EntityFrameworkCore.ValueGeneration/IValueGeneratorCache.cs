using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public interface IValueGeneratorCache
{
	ValueGenerator GetOrAdd([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] Func<IProperty, IEntityType, ValueGenerator> factory);
}
