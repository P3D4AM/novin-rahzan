using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;

public interface IConventionAnnotatableBuilder
{
	IConventionAnnotatable Metadata { get; }

	IConventionModelBuilder ModelBuilder { get; }

	IConventionAnnotatableBuilder HasAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	IConventionAnnotatableBuilder SetOrRemoveAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	bool CanSetAnnotation([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false);

	IConventionAnnotatableBuilder RemoveAnnotation([JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false);

	bool CanRemoveAnnotation([JetBrains.Annotations.NotNull] string name, bool fromDataAnnotation = false);
}
