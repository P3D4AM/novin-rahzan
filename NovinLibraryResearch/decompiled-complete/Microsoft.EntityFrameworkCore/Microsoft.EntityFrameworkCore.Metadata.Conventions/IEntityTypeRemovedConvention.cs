using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IEntityTypeRemovedConvention : IConvention
{
	void ProcessEntityTypeRemoved([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.NotNull] IConventionContext<IConventionEntityType> context);
}
