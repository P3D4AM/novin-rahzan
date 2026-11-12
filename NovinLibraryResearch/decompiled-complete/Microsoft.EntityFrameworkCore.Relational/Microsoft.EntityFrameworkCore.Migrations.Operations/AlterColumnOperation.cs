using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER TABLE {Table} ALTER COLUMN {Name}")]
public class AlterColumnOperation : ColumnOperation, IAlterMigrationOperation
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

	public virtual ColumnOperation OldColumn
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	} = new ColumnOperation();


	IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldColumn;
}
