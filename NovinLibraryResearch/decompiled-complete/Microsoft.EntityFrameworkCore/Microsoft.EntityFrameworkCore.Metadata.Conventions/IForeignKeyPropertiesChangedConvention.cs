using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyPropertiesChangedConvention : IConvention
{
	void ProcessForeignKeyPropertiesChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IReadOnlyList<IConventionProperty> oldDependentProperties, [JetBrains.Annotations.NotNull] IConventionKey oldPrincipalKey, [JetBrains.Annotations.NotNull] IConventionContext<IConventionRelationshipBuilder> context);
}
