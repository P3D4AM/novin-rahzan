using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.ValueGeneration;

public interface IValueGeneratorSelector
{
	ValueGenerator Select([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IEntityType entityType);
}
