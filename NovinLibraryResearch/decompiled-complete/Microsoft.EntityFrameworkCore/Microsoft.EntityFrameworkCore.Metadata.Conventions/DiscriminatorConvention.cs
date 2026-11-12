using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DiscriminatorConvention : IEntityTypeBaseTypeChangedConvention, IConvention, IEntityTypeRemovedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public DiscriminatorConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if (oldBaseType != null && oldBaseType.BaseType == null && !oldBaseType.GetDirectlyDerivedTypes().Any())
		{
			oldBaseType.Builder?.HasNoDeclaredDiscriminator();
		}
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		List<IConventionEntityType> list = metadata.GetDerivedTypes().ToList();
		IConventionDiscriminatorBuilder conventionDiscriminatorBuilder;
		if (newBaseType == null)
		{
			if (list.Count == 0)
			{
				entityTypeBuilder.HasNoDeclaredDiscriminator();
				return;
			}
			conventionDiscriminatorBuilder = entityTypeBuilder.HasDiscriminator(typeof(string));
		}
		else
		{
			if (entityTypeBuilder.HasNoDeclaredDiscriminator() == null)
			{
				return;
			}
			conventionDiscriminatorBuilder = metadata.GetRootType().Builder?.HasDiscriminator(typeof(string));
			if (newBaseType.BaseType == null)
			{
				conventionDiscriminatorBuilder?.HasValue(newBaseType, newBaseType.ShortName());
			}
		}
		if (conventionDiscriminatorBuilder != null)
		{
			conventionDiscriminatorBuilder.HasValue(entityTypeBuilder.Metadata, entityTypeBuilder.Metadata.ShortName());
			SetDefaultDiscriminatorValues(list, conventionDiscriminatorBuilder);
		}
	}

	public virtual void ProcessEntityTypeRemoved(IConventionModelBuilder modelBuilder, IConventionEntityType entityType, IConventionContext<IConventionEntityType> context)
	{
		IConventionEntityType baseType = entityType.BaseType;
		if (baseType != null && baseType.BaseType == null && !baseType.GetDirectlyDerivedTypes().Any())
		{
			baseType.Builder?.HasNoDeclaredDiscriminator();
		}
	}

	protected virtual void SetDefaultDiscriminatorValues([JetBrains.Annotations.NotNull] IEnumerable<IConventionEntityType> entityTypes, [JetBrains.Annotations.NotNull] IConventionDiscriminatorBuilder discriminatorBuilder)
	{
		foreach (IConventionEntityType entityType in entityTypes)
		{
			discriminatorBuilder.HasValue(entityType, entityType.ShortName());
		}
	}
}
