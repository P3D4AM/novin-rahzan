using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyPrincipalEndChangedConvention : IConvention
{
	void ProcessForeignKeyPrincipalEndChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionRelationshipBuilder> context);
}
