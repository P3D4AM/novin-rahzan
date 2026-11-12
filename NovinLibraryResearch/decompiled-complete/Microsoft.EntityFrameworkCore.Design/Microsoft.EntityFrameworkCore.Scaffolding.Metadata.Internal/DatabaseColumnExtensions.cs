using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata.Internal;

public static class DatabaseColumnExtensions
{
	public static string DisplayName([JetBrains.Annotations.NotNull] this DatabaseColumn column)
	{
		string text = column.Table?.DisplayName();
		return ((!string.IsNullOrEmpty(text)) ? (text + ".") : "") + column.Name;
	}

	public static bool IsKeyOrIndex([JetBrains.Annotations.NotNull] this DatabaseColumn column)
	{
		DatabaseTable table = column.Table;
		DatabasePrimaryKey primaryKey = table.PrimaryKey;
		if (primaryKey != null && primaryKey.Columns.Contains(column))
		{
			return true;
		}
		if (table.UniqueConstraints.Any((DatabaseUniqueConstraint uc) => uc.Columns.Contains(column)))
		{
			return true;
		}
		return table.Indexes.Any((DatabaseIndex uc) => uc.Columns.Contains(column));
	}

	public static bool IsRowVersion([JetBrains.Annotations.NotNull] this DatabaseColumn column)
	{
		if (column.ValueGenerated == ValueGenerated.OnAddOrUpdate)
		{
			return (bool?)column["ConcurrencyToken"] == true;
		}
		return false;
	}
}
