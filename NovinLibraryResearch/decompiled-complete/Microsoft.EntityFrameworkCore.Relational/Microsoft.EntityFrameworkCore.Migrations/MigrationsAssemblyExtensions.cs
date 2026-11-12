using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations;

public static class MigrationsAssemblyExtensions
{
	public static string GetMigrationId([JetBrains.Annotations.NotNull] this IMigrationsAssembly assembly, [JetBrains.Annotations.NotNull] string nameOrId)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(assembly, "assembly");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(nameOrId, "nameOrId");
		return assembly.FindMigrationId(nameOrId) ?? throw new InvalidOperationException(RelationalStrings.MigrationNotFound(nameOrId));
	}
}
