using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class TimestampAttributeConvention : PropertyAttributeConventionBase<TimestampAttribute>
{
	public TimestampAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, TimestampAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		propertyBuilder.ValueGenerated(ValueGenerated.OnAddOrUpdate, fromDataAnnotation: true);
		propertyBuilder.IsConcurrencyToken(true, fromDataAnnotation: true);
	}
}
