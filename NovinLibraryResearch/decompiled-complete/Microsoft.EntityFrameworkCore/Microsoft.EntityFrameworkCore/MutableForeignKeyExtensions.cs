using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class MutableForeignKeyExtensions
{
	public static IMutableEntityType GetRelatedEntityType([JetBrains.Annotations.NotNull] this IMutableForeignKey foreignKey, [JetBrains.Annotations.NotNull] IMutableEntityType entityType)
	{
		return (IMutableEntityType)ForeignKeyExtensions.GetRelatedEntityType(foreignKey, entityType);
	}

	public static IMutableNavigation GetNavigation([JetBrains.Annotations.NotNull] this IMutableForeignKey foreignKey, bool pointsToPrincipal)
	{
		if (!pointsToPrincipal)
		{
			return foreignKey.PrincipalToDependent;
		}
		return foreignKey.DependentToPrincipal;
	}
}
