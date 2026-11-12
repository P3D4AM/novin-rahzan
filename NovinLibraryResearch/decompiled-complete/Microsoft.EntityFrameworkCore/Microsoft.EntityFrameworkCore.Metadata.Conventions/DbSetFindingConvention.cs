using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DbSetFindingConvention : IModelInitializedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public DbSetFindingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelInitialized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (DbSetProperty item in Dependencies.SetFinder.FindSets(Dependencies.ContextType))
		{
			if (item.IsKeyless)
			{
				modelBuilder.Entity(item.ClrType, false, fromDataAnnotation: true).HasNoKey(fromDataAnnotation: true);
			}
			else
			{
				modelBuilder.Entity(item.ClrType, false, fromDataAnnotation: true);
			}
		}
	}
}
