using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("CREATE TABLE {Name}")]
public class CreateTableOperation : TableOperation
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

	public virtual AddPrimaryKeyOperation PrimaryKey
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual List<AddColumnOperation> Columns { get; } = new List<AddColumnOperation>();


	public virtual List<AddForeignKeyOperation> ForeignKeys { get; } = new List<AddForeignKeyOperation>();


	public virtual List<AddUniqueConstraintOperation> UniqueConstraints { get; } = new List<AddUniqueConstraintOperation>();


	public virtual List<CreateCheckConstraintOperation> CheckConstraints { get; } = new List<CreateCheckConstraintOperation>();

}
