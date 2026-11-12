using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER SEQUENCE {Name} RENAME TO {NewName}")]
public class RenameSequenceOperation : MigrationOperation
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

	public virtual string NewName
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string NewSchema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
