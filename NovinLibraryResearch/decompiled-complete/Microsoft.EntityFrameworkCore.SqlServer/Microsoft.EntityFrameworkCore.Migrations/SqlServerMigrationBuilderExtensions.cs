using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Migrations;

public static class SqlServerMigrationBuilderExtensions
{
	public static bool IsSqlServer([JetBrains.Annotations.NotNull] this MigrationBuilder migrationBuilder)
	{
		return string.Equals(migrationBuilder.ActiveProvider, typeof(SqlServerOptionsExtension).GetTypeInfo().Assembly.GetName().Name, StringComparison.Ordinal);
	}
}
