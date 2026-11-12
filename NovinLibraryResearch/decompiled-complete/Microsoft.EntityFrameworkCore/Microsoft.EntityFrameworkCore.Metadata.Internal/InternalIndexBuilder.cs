using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalIndexBuilder : InternalModelItemBuilder<Index>, IConventionIndexBuilder, IConventionAnnotatableBuilder
{
	IConventionIndex IConventionIndexBuilder.Metadata => Metadata;

	public InternalIndexBuilder([JetBrains.Annotations.NotNull] Index index, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(index, modelBuilder)
	{
	}

	public virtual InternalIndexBuilder IsUnique(bool? unique, ConfigurationSource configurationSource)
	{
		if (!CanSetIsUnique(unique, configurationSource))
		{
			return null;
		}
		return Metadata.SetIsUnique(unique, configurationSource)?.Builder;
	}

	public virtual bool CanSetIsUnique(bool? unique, ConfigurationSource? configurationSource)
	{
		if (Metadata.IsUnique != unique)
		{
			return configurationSource.Overrides(Metadata.GetIsUniqueConfigurationSource());
		}
		return true;
	}

	public virtual InternalIndexBuilder Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder)
	{
		IReadOnlyList<Property> actualProperties = entityTypeBuilder.GetActualProperties(Metadata.Properties, null);
		if (actualProperties == null)
		{
			return null;
		}
		InternalIndexBuilder internalIndexBuilder = entityTypeBuilder.HasIndex(actualProperties, Metadata.GetConfigurationSource());
		internalIndexBuilder?.MergeAnnotationsFrom(Metadata);
		ConfigurationSource? isUniqueConfigurationSource = Metadata.GetIsUniqueConfigurationSource();
		if (isUniqueConfigurationSource.HasValue)
		{
			internalIndexBuilder?.IsUnique(Metadata.IsUnique, isUniqueConfigurationSource.Value);
		}
		return internalIndexBuilder;
	}

	IConventionIndexBuilder IConventionIndexBuilder.IsUnique(bool? unique, bool fromDataAnnotation)
	{
		return IsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}

	bool IConventionIndexBuilder.CanSetIsUnique(bool? unique, bool fromDataAnnotation)
	{
		return CanSetIsUnique(unique, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
