using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IPropertyAnnotationChangedConvention : IConvention
{
	void ProcessPropertyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation, [JetBrains.Annotations.NotNull] IConventionContext<IConventionAnnotation> context);
}
