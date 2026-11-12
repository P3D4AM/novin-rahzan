using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyOwnershipChangedConvention : IConvention
{
	void ProcessForeignKeyOwnershipChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionRelationshipBuilder> context);
}
