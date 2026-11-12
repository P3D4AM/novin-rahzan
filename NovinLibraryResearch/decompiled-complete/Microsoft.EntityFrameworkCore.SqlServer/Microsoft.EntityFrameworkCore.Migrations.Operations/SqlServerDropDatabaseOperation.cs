using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public class SqlServerDropDatabaseOperation : MigrationOperation
{
	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}
}
