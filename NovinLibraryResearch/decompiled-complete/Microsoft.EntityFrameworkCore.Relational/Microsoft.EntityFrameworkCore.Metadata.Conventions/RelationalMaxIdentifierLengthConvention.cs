using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalMaxIdentifierLengthConvention : IModelInitializedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public virtual int MaxIdentifierLength { get; }

	public RelationalMaxIdentifierLengthConvention(int maxIdentifierLength, [JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		MaxIdentifierLength = maxIdentifierLength;
		Dependencies = dependencies;
	}

	public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		modelBuilder.Metadata.Builder.HasMaxIdentifierLength(MaxIdentifierLength);
	}
}
