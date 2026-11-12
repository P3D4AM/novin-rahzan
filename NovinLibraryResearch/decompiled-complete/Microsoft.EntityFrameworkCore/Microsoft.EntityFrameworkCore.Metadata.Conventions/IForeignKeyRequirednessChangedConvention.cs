using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyRequirednessChangedConvention : IConvention
{
	void ProcessForeignKeyRequirednessChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionRelationshipBuilder> context);
}
