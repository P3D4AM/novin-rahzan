using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RequiredPropertyAttributeConvention : PropertyAttributeConventionBase<RequiredAttribute>
{
	public RequiredPropertyAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, RequiredAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		propertyBuilder.IsRequired(true, fromDataAnnotation: true);
	}
}
