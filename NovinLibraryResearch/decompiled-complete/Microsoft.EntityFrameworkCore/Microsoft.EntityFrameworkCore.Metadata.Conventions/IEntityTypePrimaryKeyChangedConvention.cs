using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IEntityTypePrimaryKeyChangedConvention : IConvention
{
	void ProcessEntityTypePrimaryKeyChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionKey newPrimaryKey, [JetBrains.Annotations.CanBeNull] IConventionKey previousPrimaryKey, [JetBrains.Annotations.NotNull] IConventionContext<IConventionKey> context);
}
