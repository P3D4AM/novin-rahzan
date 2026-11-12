using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class StringLengthAttributeConvention : PropertyAttributeConventionBase<StringLengthAttribute>
{
	public StringLengthAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, StringLengthAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		if (attribute.MaximumLength > 0)
		{
			propertyBuilder.HasMaxLength(attribute.MaximumLength, fromDataAnnotation: true);
		}
	}
}
