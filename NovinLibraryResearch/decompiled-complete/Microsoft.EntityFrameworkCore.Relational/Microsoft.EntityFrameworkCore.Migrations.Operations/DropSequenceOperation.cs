using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("DROP SEQUENCE {Name}")]
public class DropSequenceOperation : MigrationOperation
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
}
