using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalColumnAttributeConvention : PropertyAttributeConventionBase<ColumnAttribute>
{
	public RelationalColumnAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, ColumnAttribute attribute, MemberInfo clrMember, IConventionContext context)
	{
		if (!string.IsNullOrWhiteSpace(attribute.Name))
		{
			propertyBuilder.HasColumnName(attribute.Name, fromDataAnnotation: true);
		}
		if (!string.IsNullOrWhiteSpace(attribute.TypeName))
		{
			propertyBuilder.HasColumnType(attribute.TypeName, fromDataAnnotation: true);
		}
	}
}
