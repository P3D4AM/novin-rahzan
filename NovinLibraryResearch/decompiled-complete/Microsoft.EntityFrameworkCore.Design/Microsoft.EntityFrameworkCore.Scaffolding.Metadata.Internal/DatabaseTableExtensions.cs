using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;

public static class DatabaseTableExtensions
{
	public static string DisplayName([JetBrains.Annotations.NotNull] this DatabaseTable table)
	{
		if (string.IsNullOrEmpty(table.Schema))
		{
			return table.Name;
		}
		return table.Schema + "." + table.Name;
	}
}
