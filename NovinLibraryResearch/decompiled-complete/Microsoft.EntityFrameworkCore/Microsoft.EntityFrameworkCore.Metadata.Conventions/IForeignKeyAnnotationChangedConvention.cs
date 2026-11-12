using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IForeignKeyAnnotationChangedConvention : IConvention
{
	void ProcessForeignKeyAnnotationChanged([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] IConventionAnnotation annotation, [JetBrains.Annotations.CanBeNull] IConventionAnnotation oldAnnotation, [JetBrains.Annotations.NotNull] IConventionContext<IConventionAnnotation> context);
}
