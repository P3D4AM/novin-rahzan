using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SqlServerMemoryOptimizedTablesConvention : IEntityTypeAnnotationChangedConvention, IConvention, IKeyAddedConvention, IIndexAddedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public SqlServerMemoryOptimizedTablesConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAnnotationChanged(IConventionEntityTypeBuilder entityTypeBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (!(name == "SqlServer:MemoryOptimized"))
		{
			return;
		}
		bool flag = annotation?.Value as bool? == true;
		foreach (IConventionKey declaredKey in entityTypeBuilder.Metadata.GetDeclaredKeys())
		{
			declaredKey.Builder.IsClustered(flag ? new bool?(false) : null);
		}
		foreach (IConventionIndex item in entityTypeBuilder.Metadata.GetDerivedTypesInclusive().SelectMany((IConventionEntityType et) => et.GetDeclaredIndexes()))
		{
			item.Builder.IsClustered(flag ? new bool?(false) : null);
		}
	}

	public virtual void ProcessKeyAdded(IConventionKeyBuilder keyBuilder, IConventionContext<IConventionKeyBuilder> context)
	{
		if (keyBuilder.Metadata.DeclaringEntityType.IsMemoryOptimized())
		{
			keyBuilder.IsClustered(false);
		}
	}

	public virtual void ProcessIndexAdded(IConventionIndexBuilder indexBuilder, IConventionContext<IConventionIndexBuilder> context)
	{
		if (indexBuilder.Metadata.DeclaringEntityType.GetAllBaseTypesInclusive().Any((IConventionEntityType et) => et.IsMemoryOptimized()))
		{
			indexBuilder.IsClustered(false);
		}
	}
}
