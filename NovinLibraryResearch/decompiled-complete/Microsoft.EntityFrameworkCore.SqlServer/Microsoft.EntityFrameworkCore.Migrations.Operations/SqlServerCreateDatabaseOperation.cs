using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public class SqlServerCreateDatabaseOperation : MigrationOperation
{
	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string FileName
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
