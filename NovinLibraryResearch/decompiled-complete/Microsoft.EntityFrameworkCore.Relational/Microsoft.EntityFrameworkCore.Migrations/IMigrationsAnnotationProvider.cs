using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationsAnnotationProvider
{
	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IModel model);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IIndex index);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IProperty property);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IKey key);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] IEntityType entityType);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] ISequence sequence);

	IEnumerable<IAnnotation> For([JetBrains.Annotations.NotNull] ICheckConstraint checkConstraint);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IModel model);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IIndex index);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IProperty property);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IKey key);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IForeignKey foreignKey);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] IEntityType entityType);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] ISequence sequence);

	IEnumerable<IAnnotation> ForRemove([JetBrains.Annotations.NotNull] ICheckConstraint checkConstraint);
}
