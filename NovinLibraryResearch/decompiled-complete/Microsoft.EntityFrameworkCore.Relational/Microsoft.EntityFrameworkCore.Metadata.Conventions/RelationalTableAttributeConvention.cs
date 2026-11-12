using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class RelationalTableAttributeConvention : EntityTypeAttributeConventionBase<TableAttribute>
{
	public RelationalTableAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies, [JetBrains.Annotations.NotNull] RelationalConventionSetBuilderDependencies relationalDependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, TableAttribute attribute, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		if (!string.IsNullOrWhiteSpace(attribute.Schema))
		{
			entityTypeBuilder.ToTable(attribute.Name, attribute.Schema, fromDataAnnotation: true);
		}
		else if (!string.IsNullOrWhiteSpace(attribute.Name))
		{
			entityTypeBuilder.ToTable(attribute.Name, fromDataAnnotation: true);
		}
	}
}
