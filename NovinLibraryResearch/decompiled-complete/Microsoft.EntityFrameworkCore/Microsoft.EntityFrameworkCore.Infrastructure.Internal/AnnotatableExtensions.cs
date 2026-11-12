using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure.Internal;

public static class AnnotatableExtensions
{
	public static string AnnotationsToDebugString([JetBrains.Annotations.NotNull] this IAnnotatable annotatable, [JetBrains.Annotations.NotNull] string indent = "")
	{
		List<IAnnotation> list = annotatable.GetAnnotations().ToList();
		if (list.Count == 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine().Append(indent).Append("Annotations: ");
		foreach (IAnnotation item in list)
		{
			stringBuilder.AppendLine().Append(indent).Append("  ")
				.Append(item.Name)
				.Append(": ")
				.Append(item.Value);
		}
		return stringBuilder.ToString();
	}
}
