using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER TABLE {Name} RENAME TO {NewName}")]
public class RenameTableOperation : MigrationOperation
{
	public virtual string Schema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string NewSchema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string NewName
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
