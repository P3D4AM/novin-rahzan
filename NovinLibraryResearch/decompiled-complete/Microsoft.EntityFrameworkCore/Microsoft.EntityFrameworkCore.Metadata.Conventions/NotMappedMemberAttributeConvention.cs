using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class NotMappedMemberAttributeConvention : IEntityTypeAddedConvention, IConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public NotMappedMemberAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityTypeBuilder, "entityTypeBuilder");
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (!metadata.HasClrType())
		{
			return;
		}
		foreach (MemberInfo item in metadata.GetRuntimeProperties().Values.Cast<MemberInfo>().Concat(metadata.GetRuntimeFields().Values))
		{
			if (Attribute.IsDefined(item, typeof(NotMappedAttribute), inherit: true))
			{
				entityTypeBuilder.Ignore(item.GetSimpleMemberName(), fromDataAnnotation: true);
			}
		}
	}
}
