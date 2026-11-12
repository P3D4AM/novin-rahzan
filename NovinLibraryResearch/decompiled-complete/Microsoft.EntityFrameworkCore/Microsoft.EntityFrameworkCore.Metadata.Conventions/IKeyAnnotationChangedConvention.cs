using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IKeyAnnotationChangedConvention : IConvention
{
	void ProcessKeyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionKeyBuilder keyBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation, [JetBrains.Annotations.NotNull] IConventionContext<IConventionAnnotation> context);
}
