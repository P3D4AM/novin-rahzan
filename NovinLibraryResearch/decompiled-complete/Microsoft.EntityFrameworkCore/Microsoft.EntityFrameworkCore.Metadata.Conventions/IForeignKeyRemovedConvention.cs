using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyRemovedConvention : IConvention
{
	void ProcessForeignKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey, [JetBrains.Annotations.NotNull] IConventionContext<IConventionForeignKey> context);
}
