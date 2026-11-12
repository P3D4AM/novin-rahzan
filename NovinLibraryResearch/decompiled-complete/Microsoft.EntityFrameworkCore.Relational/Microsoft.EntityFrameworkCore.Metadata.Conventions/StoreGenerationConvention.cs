using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class StoreGenerationConvention : IPropertyAnnotationChangedConvention, IConvention, IModelFinalizedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public StoreGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (annotation == null || (oldAnnotation != null && oldAnnotation.Value != null))
		{
			return;
		}
		bool fromDataAnnotation = annotation.GetConfigurationSource() != ConfigurationSource.Convention;
		switch (name)
		{
		case "Relational:DefaultValue":
			if (((propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) == null) | (propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) == null)) && propertyBuilder.HasDefaultValue(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
			}
			break;
		case "Relational:DefaultValueSql":
			if (((propertyBuilder.HasDefaultValue(null, fromDataAnnotation) == null) | (propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) == null)) && propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
			}
			break;
		case "Relational:ComputedColumnSql":
			if (((propertyBuilder.HasDefaultValue(null, fromDataAnnotation) == null) | (propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) == null)) && propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
			}
			break;
		}
	}

	public virtual void ProcessModelFinalized(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
	{
		foreach (IConventionEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
		{
			foreach (IConventionProperty declaredProperty in entityType.GetDeclaredProperties())
			{
				Validate(declaredProperty);
			}
		}
	}

	protected virtual void Validate([JetBrains.Annotations.NotNull] IConventionProperty property)
	{
		if (property.GetDefaultValue() != null)
		{
			if (property.GetDefaultValueSql() != null)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("DefaultValue", property.Name, "DefaultValueSql"));
			}
			if (property.GetComputedColumnSql() != null)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("DefaultValue", property.Name, "ComputedColumnSql"));
			}
		}
		else if (property.GetDefaultValueSql() != null && property.GetComputedColumnSql() != null)
		{
			throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("DefaultValueSql", property.Name, "ComputedColumnSql"));
		}
	}
}
