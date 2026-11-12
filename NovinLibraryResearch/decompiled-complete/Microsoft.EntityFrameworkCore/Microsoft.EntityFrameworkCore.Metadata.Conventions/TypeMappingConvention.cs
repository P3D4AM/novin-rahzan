using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class TypeMappingConvention : IModelFinalizedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public TypeMappingConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionProperty item in modelBuilder.Metadata.GetEntityTypes().SelectMany((IConventionEntityType e) => e.GetDeclaredProperties()))
		{
			item.Builder.HasAnnotation("TypeMapping", Dependencies.TypeMappingSource.FindMapping(item));
		}
	}
}
