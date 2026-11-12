using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Migrations;

public interface IMigrationsAssembly
{
	IReadOnlyDictionary<string, TypeInfo> Migrations { get; }

	ModelSnapshot ModelSnapshot { get; }

	Assembly Assembly { get; }

	string FindMigrationId([JetBrains.Annotations.NotNull] string nameOrId);

	Migration CreateMigration([JetBrains.Annotations.NotNull] TypeInfo migrationClass, [JetBrains.Annotations.NotNull] string activeProvider);
}
