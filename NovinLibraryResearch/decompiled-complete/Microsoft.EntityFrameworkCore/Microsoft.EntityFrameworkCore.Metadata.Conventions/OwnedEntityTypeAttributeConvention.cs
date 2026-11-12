using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public class OwnedEntityTypeAttributeConvention : EntityTypeAttributeConventionBase<OwnedAttribute>
{
	public OwnedEntityTypeAttributeConvention([JetBrains.Annotations.NotNull] ProviderConventionSetBuilderDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override void ProcessEntityTypeAdded(IConventionEntityTypeBuilder entityTypeBuilder, OwnedAttribute attribute, IConventionContext<IConventionEntityTypeBuilder> context)
	{
		if (entityTypeBuilder.Metadata.HasClrType())
		{
			entityTypeBuilder.ModelBuilder.Owned(entityTypeBuilder.Metadata.ClrType, fromDataAnnotation: true);
		}
	}
}
