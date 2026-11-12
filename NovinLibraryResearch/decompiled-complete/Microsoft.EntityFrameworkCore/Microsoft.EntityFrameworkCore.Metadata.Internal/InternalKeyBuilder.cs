using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalKeyBuilder : InternalModelItemBuilder<Key>, IConventionKeyBuilder, IConventionAnnotatableBuilder
{
	IConventionKey IConventionKeyBuilder.Metadata => Metadata;

	public InternalKeyBuilder([JetBrains.Annotations.NotNull] Key key, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(key, modelBuilder)
	{
	}

	public virtual InternalKeyBuilder Attach([JetBrains.Annotations.NotNull] InternalEntityTypeBuilder entityTypeBuilder, ConfigurationSource? primaryKeyConfigurationSource)
	{
		List<string> list = Metadata.Properties.Select((Property p) => p.Name).ToList();
		foreach (string item in list)
		{
			if (entityTypeBuilder.Metadata.FindProperty(item) == null)
			{
				return null;
			}
		}
		InternalKeyBuilder internalKeyBuilder = entityTypeBuilder.HasKey(list, Metadata.GetConfigurationSource());
		internalKeyBuilder?.MergeAnnotationsFrom(Metadata);
		if (primaryKeyConfigurationSource.HasValue && internalKeyBuilder != null)
		{
			ConfigurationSource? primaryKeyConfigurationSource2 = entityTypeBuilder.Metadata.GetPrimaryKeyConfigurationSource();
			if (!primaryKeyConfigurationSource2.HasValue || !primaryKeyConfigurationSource2.GetValueOrDefault().Overrides(primaryKeyConfigurationSource.Value))
			{
				entityTypeBuilder.PrimaryKey(internalKeyBuilder.Metadata.Properties, primaryKeyConfigurationSource.Value);
			}
		}
		return internalKeyBuilder;
	}
}
