using System.Linq;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;

public static class DatabaseForeignKeyExtensions
{
	public static string DisplayName([JetBrains.Annotations.NotNull] this DatabaseForeignKey foreignKey)
	{
		return foreignKey.Table?.DisplayName() + "(" + string.Join(",", foreignKey.Columns.Select((DatabaseColumn f) => f.Name)) + ")";
	}
}
