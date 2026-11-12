using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class MutableAnnotatableExtensions
{
	public static IAnnotation GetAnnotation([JetBrains.Annotations.NotNull] this IMutableAnnotatable annotatable, [JetBrains.Annotations.NotNull] string annotationName)
	{
		return AnnotatableExtensions.GetAnnotation(annotatable, annotationName);
	}

	public static void AddAnnotations([JetBrains.Annotations.NotNull] this IMutableAnnotatable annotatable, [JetBrains.Annotations.NotNull] IEnumerable<IAnnotation> annotations)
	{
		foreach (IAnnotation annotation in annotations)
		{
			annotatable.AddAnnotation(annotation.Name, annotation.Value);
		}
	}

	public static void SetOrRemoveAnnotation([JetBrains.Annotations.NotNull] this IMutableAnnotatable annotatable, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value)
	{
		((ConventionAnnotatable)annotatable).SetOrRemoveAnnotation(name, value, ConfigurationSource.Explicit);
	}
}
