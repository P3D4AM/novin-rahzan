using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public interface IMigrationsCodeGenerator : ILanguageBasedService
{
	string FileExtension { get; }

	string GenerateMetadata([JetBrains.Annotations.NotNull] string migrationNamespace, [JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.NotNull] string migrationName, [JetBrains.Annotations.NotNull] string migrationId, [JetBrains.Annotations.NotNull] IModel targetModel);

	string GenerateMigration([JetBrains.Annotations.NotNull] string migrationNamespace, [JetBrains.Annotations.NotNull] string migrationName, [JetBrains.Annotations.NotNull] IReadOnlyList<MigrationOperation> upOperations, [JetBrains.Annotations.NotNull] IReadOnlyList<MigrationOperation> downOperations);

	string GenerateSnapshot([JetBrains.Annotations.NotNull] string modelSnapshotNamespace, [JetBrains.Annotations.NotNull] Type contextType, [JetBrains.Annotations.NotNull] string modelSnapshotName, [JetBrains.Annotations.NotNull] IModel model);
}
