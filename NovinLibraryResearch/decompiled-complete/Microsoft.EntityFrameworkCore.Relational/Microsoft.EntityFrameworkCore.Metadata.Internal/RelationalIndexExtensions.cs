using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class RelationalIndexExtensions
{
	public static bool AreCompatible([JetBrains.Annotations.NotNull] this IIndex index, [JetBrains.Annotations.NotNull] IIndex duplicateIndex, bool shouldThrow)
	{
		if (!index.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(duplicateIndex.Properties.Select((IProperty p) => p.GetColumnName())))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateIndexColumnMismatch(index.Properties.Format(), index.DeclaringEntityType.DisplayName(), duplicateIndex.Properties.Format(), duplicateIndex.DeclaringEntityType.DisplayName(), Format(index.DeclaringEntityType), index.GetName(), index.Properties.FormatColumns(), duplicateIndex.Properties.FormatColumns()));
			}
			return false;
		}
		if (index.IsUnique != duplicateIndex.IsUnique)
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateIndexUniquenessMismatch(index.Properties.Format(), index.DeclaringEntityType.DisplayName(), duplicateIndex.Properties.Format(), duplicateIndex.DeclaringEntityType.DisplayName(), Format(index.DeclaringEntityType), index.GetName()));
			}
			return false;
		}
		return true;
	}

	private static string Format(IEntityType entityType)
	{
		return (string.IsNullOrEmpty(entityType.GetSchema()) ? "" : (entityType.GetSchema() + ".")) + entityType.GetTableName();
	}
}
