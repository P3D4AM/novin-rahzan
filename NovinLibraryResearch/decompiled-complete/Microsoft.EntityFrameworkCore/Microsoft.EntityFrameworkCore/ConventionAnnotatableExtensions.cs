using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionAnnotatableExtensions
{
	public static IConventionAnnotation GetAnnotation([JetBrains.Annotations.NotNull] this IConventionAnnotatable annotatable, [JetBrains.Annotations.NotNull] string annotationName)
	{
		return (IConventionAnnotation)AnnotatableExtensions.GetAnnotation(annotatable, annotationName);
	}

	public static void AddAnnotations([JetBrains.Annotations.NotNull] this IConventionAnnotatable annotatable, [JetBrains.Annotations.NotNull] IEnumerable<IConventionAnnotation> annotations, bool fromDataAnnotation = false)
	{
		foreach (IConventionAnnotation annotation in annotations)
		{
			annotatable.AddAnnotation(annotation.Name, annotation.Value, fromDataAnnotation);
		}
	}

	public static void SetOrRemoveAnnotation([JetBrains.Annotations.NotNull] this IConventionAnnotatable annotatable, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, bool fromDataAnnotation = false)
	{
		((ConventionAnnotatable)annotatable).SetOrRemoveAnnotation(name, value, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
