using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionDiscriminatorBuilder
{
	IConventionDiscriminatorBuilder HasValue([JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	IConventionDiscriminatorBuilder HasValue([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	bool CanSetValue([JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	bool CanSetValue([JetBrains.Annotations.NotNull] IConventionEntityType entityType, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);
}
