using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public class TableOperation : MigrationOperation
{
	public virtual string Comment
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
