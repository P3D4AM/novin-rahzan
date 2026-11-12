using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Design;

public class MigrationFiles
{
	public virtual string MigrationFile
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string MetadataFile
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string SnapshotFile
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
