using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SqlServerValueGenerationStrategyConvention : IModelInitializedConvention, IConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public SqlServerValueGenerationStrategyConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		modelBuilder.HasValueGenerationStrategy(SqlServerValueGenerationStrategy.IdentityColumn);
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				SqlServerValueGenerationStrategy valueGenerationStrategy = declaredProperty.GetValueGenerationStrategy();
				if (valueGenerationStrategy != 0)
				{
					declaredProperty.Builder.HasValueGenerationStrategy(valueGenerationStrategy);
				}
			}
		}
	}
}
