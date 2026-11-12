using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class RelationalPropertyExtensions
{
	public static IForeignKey FindSharedTableLink([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return property.FindSharedTableLink(property.DeclaringEntityType.GetTableName(), property.DeclaringEntityType.GetSchema());
	}

	public static IForeignKey FindSharedTableLink([JetBrains.Annotations.NotNull] this IProperty property, string table, string schema)
	{
		IKey key = property.FindContainingPrimaryKey();
		if (key == null)
		{
			return null;
		}
		foreach (IForeignKey item in property.DeclaringEntityType.FindForeignKeys(key.Properties))
		{
			if (item.PrincipalKey.IsPrimaryKey() && item.PrincipalEntityType != item.DeclaringEntityType)
			{
				IEntityType principalEntityType = item.PrincipalEntityType;
				_ = item.DeclaringEntityType;
				if (table == principalEntityType.GetTableName() && schema == principalEntityType.GetSchema())
				{
					return item;
				}
			}
		}
		return null;
	}

	[DebuggerStepThrough]
	public static string GetConfiguredColumnType([JetBrains.Annotations.NotNull] this IProperty property)
	{
		return (string)property["Relational:ColumnType"];
	}
}
