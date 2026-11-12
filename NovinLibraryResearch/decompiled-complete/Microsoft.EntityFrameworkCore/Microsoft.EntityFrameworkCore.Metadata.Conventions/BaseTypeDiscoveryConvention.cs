using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class BaseTypeDiscoveryConvention : InheritanceDiscoveryConventionBase, IEntityTypeAddedConvention, IConvention
{
	public BaseTypeDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		Type clrType = metadata.ClrType;
		if (clrType == null || metadata.HasDefiningNavigation() || metadata.FindDeclaredOwnership() != null || metadata.Model.FindIsOwnedConfigurationSource(clrType).HasValue)
		{
			return;
		}
		IConventionEntityType conventionEntityType = FindClosestBaseType(metadata);
		if (conventionEntityType != null && !conventionEntityType.HasDefiningNavigation())
		{
			entityTypeBuilder = entityTypeBuilder.HasBaseType(conventionEntityType);
			if (entityTypeBuilder != null)
			{
				context.StopProcessingIfChanged(entityTypeBuilder);
			}
		}
	}
}
