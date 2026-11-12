using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class ScaffoldingEntityTypeAnnotations
{
	public static string GetDbSetName(this IEntityType entityType)
	{
		return ((string)entityType["Scaffolding:DbSetName"]) ?? entityType.Name;
	}

	public static void SetDbSetName(this IMutableEntityType entityType, string value)
	{
		entityType.SetAnnotation("Scaffolding:DbSetName", Microsoft.EntityFrameworkCore.Utilities.Check.NullButNotEmpty(value, "value"));
	}
}
