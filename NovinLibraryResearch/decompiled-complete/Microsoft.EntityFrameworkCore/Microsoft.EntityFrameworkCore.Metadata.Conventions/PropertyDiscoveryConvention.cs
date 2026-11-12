using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class PropertyDiscoveryConvention : IEntityTypeAddedConvention, IConvention, IEntityTypeBaseTypeChangedConvention
{
	protected virtual ProviderConventionSetBuilderDependencies Dependencies { get; }

	public PropertyDiscoveryConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
	{
		Dependencies = dependencies;
	}

	public virtual void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		Process(entityTypeBuilder);
	}

	public virtual void ProcessEntityTypeBaseTypeChanged(IConventionEntityTypeBuilder entityTypeBuilder, IConventionEntityType newBaseType, IConventionEntityType oldBaseType, IConventionContext<IConventionEntityType> context)
	{
		if ((newBaseType == null || oldBaseType != null) && entityTypeBuilder.Metadata.BaseType == newBaseType)
		{
			Process(entityTypeBuilder);
		}
	}

	private void Process(IConventionEntityTypeBuilder entityTypeBuilder)
	{
		IConventionEntityType metadata = entityTypeBuilder.Metadata;
		if (!metadata.HasClrType())
		{
			return;
		}
		foreach (PropertyInfo value in metadata.GetRuntimeProperties().Values)
		{
			if (IsCandidatePrimitiveProperty(value))
			{
				entityTypeBuilder.Property(value);
			}
		}
	}

	private bool IsCandidatePrimitiveProperty([JetBrains.Annotations.NotNull] PropertyInfo propertyInfo)
	{
		if (propertyInfo.IsCandidateProperty())
		{
			return Dependencies.TypeMappingSource.FindMapping(propertyInfo) != null;
		}
		return false;
	}
}
