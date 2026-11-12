using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ChangeTrackingStrategyConvention : IModelFinalizedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public ChangeTrackingStrategyConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			if (entityType.GetChangeTrackingStrategy() == ChangeTrackingStrategy.Snapshot)
			{
				return;
			}
		}
		modelBuilder.HasAnnotation("ChangeDetector.SkipDetectChanges", "true");
	}
}
