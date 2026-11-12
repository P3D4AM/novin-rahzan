using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public class ScaffoldedMigration
{
	public virtual string FileExtension { get; }

	public virtual string PreviousMigrationId { get; }

	public virtual string MigrationCode { get; }

	public virtual string MigrationId { get; }

	public virtual string MetadataCode { get; }

	public virtual string MigrationSubNamespace { get; }

	public virtual string SnapshotCode { get; }

	public virtual string SnapshotName { get; }

	public virtual string SnapshotSubnamespace { get; }

	public ScaffoldedMigration([JetBrains.Annotations.NotNull] string fileExtension, [JetBrains.Annotations.CanBeNull] string previousMigrationId, [JetBrains.Annotations.NotNull] string migrationCode, [JetBrains.Annotations.NotNull] string migrationId, [JetBrains.Annotations.NotNull] string metadataCode, [JetBrains.Annotations.NotNull] string migrationSubNamespace, [JetBrains.Annotations.NotNull] string snapshotCode, [JetBrains.Annotations.NotNull] string snapshotName, [JetBrains.Annotations.NotNull] string snapshotSubNamespace)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(fileExtension, "fileExtension");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationCode, "migrationCode");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(migrationId, "migrationId");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(metadataCode, "metadataCode");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(migrationSubNamespace, "migrationSubNamespace");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(snapshotCode, "snapshotCode");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(snapshotName, "snapshotName");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(snapshotSubNamespace, "snapshotSubNamespace");
		FileExtension = fileExtension;
		PreviousMigrationId = previousMigrationId;
		MigrationCode = migrationCode;
		MigrationId = migrationId;
		MetadataCode = metadataCode;
		MigrationSubNamespace = migrationSubNamespace;
		SnapshotCode = snapshotCode;
		SnapshotName = snapshotName;
		SnapshotSubnamespace = snapshotSubNamespace;
	}
}
