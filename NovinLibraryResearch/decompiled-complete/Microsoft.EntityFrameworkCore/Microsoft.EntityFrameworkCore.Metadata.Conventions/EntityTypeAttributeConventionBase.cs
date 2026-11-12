using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public abstract class EntityTypeAttributeConventionBase<TAttribute> : IEntityTypeAddedConvention, IConvention where TAttribute : Attribute
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	protected EntityTypeAttributeConventionBase([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		Type clrType = entityTypeBuilder.Metadata.ClrType;
		if (clrType == null || !Attribute.IsDefined(clrType, typeof(TAttribute), inherit: true))
		{
			return;
		}
		foreach (TAttribute customAttribute in clrType.GetTypeInfo().GetCustomAttributes<TAttribute>(inherit: true))
		{
			ProcessEntityTypeAdded(entityTypeBuilder, customAttribute, context);
			if (((IReadableConventionContext)context).ShouldStopProcessing())
			{
				break;
			}
		}
	}

	protected abstract void ProcessEntityTypeAdded([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] TAttribute attribute, [JetBrains.Annotations.NotNull] IConventionContext<IConventionEntityTypeBuilder> context);
}
