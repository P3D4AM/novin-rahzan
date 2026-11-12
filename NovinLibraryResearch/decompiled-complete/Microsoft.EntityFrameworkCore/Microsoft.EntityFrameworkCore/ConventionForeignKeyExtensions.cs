using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class ConventionForeignKeyExtensions
{
	public static IConventionEntityType GetRelatedEntityType([JetBrains.Annotations.NotNull] this IConventionForeignKey foreignKey, [JetBrains.Annotations.NotNull] IConventionEntityType entityType)
	{
		return (IConventionEntityType)ForeignKeyExtensions.GetRelatedEntityType(foreignKey, entityType);
	}

	public static IConventionNavigation GetNavigation([JetBrains.Annotations.NotNull] this IConventionForeignKey foreignKey, bool pointsToPrincipal)
	{
		if (!pointsToPrincipal)
		{
			return foreignKey.PrincipalToDependent;
		}
		return foreignKey.DependentToPrincipal;
	}
}
