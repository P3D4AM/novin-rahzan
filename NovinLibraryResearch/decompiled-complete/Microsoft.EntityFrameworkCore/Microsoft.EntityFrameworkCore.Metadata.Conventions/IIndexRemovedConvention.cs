using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IIndexRemovedConvention : IConvention
{
	void ProcessIndexRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionIndex index, [JetBrains.Annotations.NotNull] IConventionContext<IConventionIndex> context);
}
