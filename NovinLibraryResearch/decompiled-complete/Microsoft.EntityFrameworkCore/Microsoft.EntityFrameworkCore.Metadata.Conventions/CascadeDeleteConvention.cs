using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class CascadeDeleteConvention : IForeignKeyAddedConvention, IConvention, IForeignKeyRequirednessChangedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public CascadeDeleteConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessForeignKeyAdded(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		IConventionRelationshipBuilder conventionRelationshipBuilder = relationshipBuilder.OnDelete(GetTargetDeleteBehavior(relationshipBuilder.Metadata));
		if (conventionRelationshipBuilder != null)
		{
			context.StopProcessingIfChanged(conventionRelationshipBuilder);
		}
	}

	public virtual void ProcessForeignKeyRequirednessChanged(IConventionRelationshipBuilder relationshipBuilder, IConventionContext<IConventionRelationshipBuilder> context)
	{
		ProcessForeignKeyAdded(relationshipBuilder, context);
	}

	protected virtual DeleteBehavior GetTargetDeleteBehavior([JetBrains.Annotations.NotNull] IConventionForeignKey foreignKey)
	{
		if (!foreignKey.IsRequired)
		{
			return DeleteBehavior.ClientSetNull;
		}
		return DeleteBehavior.Cascade;
	}
}
