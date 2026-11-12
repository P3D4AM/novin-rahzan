using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER TABLE {Table}")]
public class AlterTableOperation : TableOperation, IAlterMigrationOperation
{
	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Schema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual TableOperation OldTable
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	} = new TableOperation();


	IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldTable;
}
