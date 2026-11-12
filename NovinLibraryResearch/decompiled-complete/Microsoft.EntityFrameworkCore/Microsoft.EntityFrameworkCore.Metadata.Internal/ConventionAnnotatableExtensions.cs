using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ConventionAnnotatableExtensions
{
	public static void SetOrRemoveAnnotation([JetBrains.Annotations.NotNull] this ConventionAnnotatable annotatable, [JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.CanBeNull] object value, ConfigurationSource configurationSource)
	{
		if (value == null)
		{
			annotatable.RemoveAnnotation(name);
		}
		else
		{
			annotatable.SetAnnotation(name, value, configurationSource);
		}
	}
}
