using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal;

public static class MigrationExtensions
{
	public static string GetId([JetBrains.Annotations.NotNull] this Migration migration)
	{
		return migration.GetType().GetTypeInfo().GetCustomAttribute<MigrationAttribute>()?.Id;
	}
}
