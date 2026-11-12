using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public interface IMigrationsScaffolder
{
	ScaffoldedMigration ScaffoldMigration([JetBrains.Annotations.NotNull] string migrationName, [JetBrains.Annotations.NotNull] string rootNamespace, [JetBrains.Annotations.CanBeNull] string subNamespace = null, [JetBrains.Annotations.CanBeNull] string language = null);

	MigrationFiles RemoveMigration([JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] string rootNamespace, bool force, [JetBrains.Annotations.CanBeNull] string language);

	MigrationFiles Save([JetBrains.Annotations.NotNull] string projectDir, [JetBrains.Annotations.NotNull] ScaffoldedMigration migration, [JetBrains.Annotations.CanBeNull] string outputDir);
}
