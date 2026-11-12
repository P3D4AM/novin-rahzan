using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IModelAnnotationChangedConvention
{
	void ProcessModelAnnotationChanged([JetBrains.Annotations.NotNull] IConventionModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation, [JetBrains.Annotations.NotNull] IConventionContext<IConventionAnnotation> context);
}
