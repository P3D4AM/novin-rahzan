using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class ConcurrencyCheckAttributeConvention : PropertyAttributeConventionBase<ConcurrencyCheckAttribute>
{
	public ConcurrencyCheckAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, ConcurrencyCheckAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		propertyBuilder.IsConcurrencyToken(true, fromDataAnnotation: true);
	}
}
