using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class NavigationEagerLoadingConvention : IForeignKeyOwnershipChangedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public NavigationEagerLoadingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyOwnershipChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		relationshipBuilder.Metadata.PrincipalToDependent?.SetIsEagerLoaded(relationshipBuilder.Metadata.IsOwnership);
	}
}
