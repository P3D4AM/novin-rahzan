using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class MaxLengthAttributeConvention : PropertyAttributeConventionBase<MaxLengthAttribute>
{
	public MaxLengthAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, MaxLengthAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		if (attribute.Length > 0)
		{
			propertyBuilder.HasMaxLength(attribute.Length, fromDataAnnotation: true);
		}
	}
}
