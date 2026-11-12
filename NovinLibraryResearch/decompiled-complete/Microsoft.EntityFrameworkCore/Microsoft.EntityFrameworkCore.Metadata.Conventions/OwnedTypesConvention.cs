using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class OwnedTypesConvention : IEntityTypeRemovedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public OwnedTypesConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeRemoved(IConventionModelBuilder modelBuilder, IConventionEntityType entityType, IConventionContext<IConventionEntityType> context)
	{
		if (!entityType.HasDefiningNavigation())
		{
			return;
		}
		IReadOnlyCollection<IConventionEntityType> entityTypes = modelBuilder.Metadata.GetEntityTypes(entityType.Name);
		IConventionEntityType conventionEntityType = entityTypes.FirstOrDefault();
		if (conventionEntityType == null || !conventionEntityType.HasDefiningNavigation() || entityTypes.Count != 1 || !(conventionEntityType.FindOwnership() is ForeignKey foreignKey))
		{
			return;
		}
		using (context.DelayConventions())
		{
			InternalEntityTypeBuilder.DetachRelationship(foreignKey).Attach(foreignKey.PrincipalEntityType.Builder);
		}
	}
}
