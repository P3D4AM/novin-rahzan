using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class RelationalForeignKeyExtensions
{
	public static bool AreCompatible([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IForeignKey duplicateForeignKey, bool shouldThrow)
	{
		IEntityType principalEntityType = foreignKey.PrincipalEntityType;
		IEntityType principalEntityType2 = duplicateForeignKey.PrincipalEntityType;
		if (!string.Equals(principalEntityType.GetSchema(), principalEntityType2.GetSchema(), StringComparison.OrdinalIgnoreCase) || !string.Equals(principalEntityType.GetTableName(), principalEntityType2.GetTableName(), StringComparison.OrdinalIgnoreCase))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateForeignKeyPrincipalTableMismatch(foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName(), duplicateForeignKey.Properties.Format(), duplicateForeignKey.DeclaringEntityType.DisplayName(), Format(foreignKey.DeclaringEntityType), foreignKey.GetConstraintName(), Format(principalEntityType), Format(principalEntityType2)));
			}
			return false;
		}
		if (!foreignKey.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(duplicateForeignKey.Properties.Select((IProperty p) => p.GetColumnName())))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateForeignKeyColumnMismatch(foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName(), duplicateForeignKey.Properties.Format(), duplicateForeignKey.DeclaringEntityType.DisplayName(), Format(foreignKey.DeclaringEntityType), foreignKey.GetConstraintName(), foreignKey.Properties.FormatColumns(), duplicateForeignKey.Properties.FormatColumns()));
			}
			return false;
		}
		if (!foreignKey.PrincipalKey.Properties.Select((IProperty p) => p.GetColumnName()).SequenceEqual(duplicateForeignKey.PrincipalKey.Properties.Select((IProperty p) => p.GetColumnName())))
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateForeignKeyPrincipalColumnMismatch(foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName(), duplicateForeignKey.Properties.Format(), duplicateForeignKey.DeclaringEntityType.DisplayName(), Format(foreignKey.DeclaringEntityType), foreignKey.GetConstraintName(), foreignKey.PrincipalKey.Properties.FormatColumns(), duplicateForeignKey.PrincipalKey.Properties.FormatColumns()));
			}
			return false;
		}
		if (foreignKey.IsUnique != duplicateForeignKey.IsUnique)
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateForeignKeyUniquenessMismatch(foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName(), duplicateForeignKey.Properties.Format(), duplicateForeignKey.DeclaringEntityType.DisplayName(), Format(foreignKey.DeclaringEntityType), foreignKey.GetConstraintName()));
			}
			return false;
		}
		if (foreignKey.DeleteBehavior != duplicateForeignKey.DeleteBehavior)
		{
			if (shouldThrow)
			{
				throw new InvalidOperationException(RelationalStrings.DuplicateForeignKeyDeleteBehaviorMismatch(foreignKey.Properties.Format(), foreignKey.DeclaringEntityType.DisplayName(), duplicateForeignKey.Properties.Format(), duplicateForeignKey.DeclaringEntityType.DisplayName(), Format(foreignKey.DeclaringEntityType), foreignKey.GetConstraintName(), foreignKey.DeleteBehavior, duplicateForeignKey.DeleteBehavior));
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
