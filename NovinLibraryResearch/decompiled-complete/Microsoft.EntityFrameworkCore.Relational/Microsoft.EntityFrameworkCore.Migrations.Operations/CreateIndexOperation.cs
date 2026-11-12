using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("CREATE INDEX {Name} ON {Table}")]
public class CreateIndexOperation : MigrationOperation
{
	public virtual bool IsUnique { get; set; }

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

	public virtual string[] Columns
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Filter
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
