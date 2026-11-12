using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class ForeignKeyExtensions
{
	public static IEntityType GetRelatedEntityType([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, [JetBrains.Annotations.NotNull] IEntityType entityType)
	{
		if (foreignKey.DeclaringEntityType != entityType && foreignKey.PrincipalEntityType != entityType)
		{
			throw new InvalidOperationException(CoreStrings.EntityTypeNotInRelationshipStrict(entityType.DisplayName(), foreignKey.DeclaringEntityType.DisplayName(), foreignKey.PrincipalEntityType.DisplayName()));
		}
		if (foreignKey.DeclaringEntityType != entityType)
		{
			return foreignKey.DeclaringEntityType;
		}
		return foreignKey.PrincipalEntityType;
	}

	public static INavigation GetNavigation([JetBrains.Annotations.NotNull] this IForeignKey foreignKey, bool pointsToPrincipal)
	{
		if (!pointsToPrincipal)
		{
			return foreignKey.PrincipalToDependent;
		}
		return foreignKey.DependentToPrincipal;
	}
}
