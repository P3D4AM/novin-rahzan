using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class SqlServerStoreGenerationConvention : StoreGenerationConvention
{
	public SqlServerStoreGenerationConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies, relationalDependencies)
	{
	}

	public override void ProcessPropertyAnnotationChanged(IConventionPropertyBuilder propertyBuilder, string name, IConventionAnnotation annotation, IConventionAnnotation oldAnnotation, IConventionContext<IConventionAnnotation> context)
	{
		if (annotation == null || (oldAnnotation != null && oldAnnotation.Value != null))
		{
			return;
		}
		bool fromDataAnnotation = annotation.GetConfigurationSource() != ConfigurationSource.Convention;
		switch (name)
		{
		case "Relational:DefaultValue":
			if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasDefaultValue(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
				return;
			}
			break;
		case "Relational:DefaultValueSql":
			if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
				return;
			}
			break;
		case "Relational:ComputedColumnSql":
			if (propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) == null && propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
				return;
			}
			break;
		case "SqlServer:ValueGenerationStrategy":
			if (((propertyBuilder.HasDefaultValue(null, fromDataAnnotation) == null) | (propertyBuilder.HasDefaultValueSql(null, fromDataAnnotation) == null) | (propertyBuilder.HasComputedColumnSql(null, fromDataAnnotation) == null)) && propertyBuilder.HasValueGenerationStrategy(null, fromDataAnnotation) != null)
			{
				context.StopProcessing();
				return;
			}
			break;
		}
		base.ProcessPropertyAnnotationChanged(propertyBuilder, name, annotation, oldAnnotation, context);
	}

	protected override void Validate(IConventionProperty property)
	{
		if (property.GetValueGenerationStrategyConfigurationSource().HasValue && property.GetValueGenerationStrategy() != 0)
		{
			if (property.GetDefaultValue() != null)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("SqlServerValueGenerationStrategy", property.Name, "DefaultValue"));
			}
			if (property.GetDefaultValueSql() != null)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("SqlServerValueGenerationStrategy", property.Name, "DefaultValueSql"));
			}
			if (property.GetComputedColumnSql() != null)
			{
				throw new InvalidOperationException(RelationalStrings.ConflictingColumnServerGeneration("SqlServerValueGenerationStrategy", property.Name, "ComputedColumnSql"));
			}
		}
		base.Validate(property);
	}
}
