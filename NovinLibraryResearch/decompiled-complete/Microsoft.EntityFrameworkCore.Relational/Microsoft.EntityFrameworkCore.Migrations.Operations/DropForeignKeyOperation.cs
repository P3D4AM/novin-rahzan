using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER TABLE {Table} DROP CONSTRAINT {Name}")]
public class DropForeignKeyOperation : MigrationOperation
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

	public virtual string Table
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}
}
