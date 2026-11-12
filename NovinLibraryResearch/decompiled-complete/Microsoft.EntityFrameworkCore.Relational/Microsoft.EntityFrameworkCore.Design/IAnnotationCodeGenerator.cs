using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IAnnotationCodeGenerator
{
	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	bool IsHandledByConvention([JetBrains.Annotations.NotNull] IIndex index, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IEntityType entityType, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IKey key, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IAnnotation annotation);

	MethodCallCodeFragment GenerateFluentApi([JetBrains.Annotations.NotNull] IIndex index, [JetBrains.Annotations.NotNull] IAnnotation annotation);
}
