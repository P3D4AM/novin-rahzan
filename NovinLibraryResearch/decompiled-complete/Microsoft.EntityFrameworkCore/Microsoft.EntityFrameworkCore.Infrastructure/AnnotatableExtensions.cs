using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class AnnotatableExtensions
{
	public static IAnnotation GetAnnotation([JetBrains.Annotations.NotNull] this IAnnotatable annotatable, [JetBrains.Annotations.NotNull] string annotationName)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(annotatable, "annotatable");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(annotationName, "annotationName");
		return annotatable.FindAnnotation(annotationName) ?? throw new InvalidOperationException(CoreStrings.AnnotationNotFound(annotationName));
	}
}
