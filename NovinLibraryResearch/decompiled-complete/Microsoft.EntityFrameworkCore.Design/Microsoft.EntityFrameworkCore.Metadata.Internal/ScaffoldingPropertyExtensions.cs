using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ScaffoldingPropertyExtensions
{
	public static int GetColumnOrdinal([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return ((int?)property["Scaffolding:ColumnOrdinal"]) ?? (-1);
	}

	public static void SetColumnOrdinal([JetBrains.Annotations.NotNull] this IMutableProperty property, int? ordinal)
	{
		property.SetOrRemoveAnnotation("Scaffolding:ColumnOrdinal", ordinal);
	}
}
