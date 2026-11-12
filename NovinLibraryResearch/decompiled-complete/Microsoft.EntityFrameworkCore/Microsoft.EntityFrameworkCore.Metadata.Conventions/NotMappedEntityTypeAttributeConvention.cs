using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class NotMappedEntityTypeAttributeConvention : EntityTypeAttributeConventionBase<NotMappedAttribute>
{
	public NotMappedEntityTypeAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, NotMappedAttribute attribute, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		if (entityTypeBuilder.ModelBuilder.Ignore(entityTypeBuilder.Metadata.Name, fromDataAnnotation: true) != null)
		{
			context.StopProcessing();
		}
	}
}
