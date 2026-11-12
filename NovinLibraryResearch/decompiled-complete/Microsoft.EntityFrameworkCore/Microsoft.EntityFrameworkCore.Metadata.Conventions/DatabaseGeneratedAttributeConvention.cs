using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class DatabaseGeneratedAttributeConvention : PropertyAttributeConventionBase<DatabaseGeneratedAttribute>
{
	public DatabaseGeneratedAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, DatabaseGeneratedAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		ValueGenerated value = (((int)attribute.DatabaseGeneratedOption == 1) ? ValueGenerated.OnAdd : (((int)attribute.DatabaseGeneratedOption == 2) ? ValueGenerated.OnAddOrUpdate : ValueGenerated.Never));
		propertyBuilder.ValueGenerated(value, fromDataAnnotation: true);
	}
}
