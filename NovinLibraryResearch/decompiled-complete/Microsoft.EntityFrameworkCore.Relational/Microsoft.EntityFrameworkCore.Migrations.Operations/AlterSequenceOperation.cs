using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER SEQUENCE {Name}")]
public class AlterSequenceOperation : SequenceOperation, IAlterMigrationOperation
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

	public virtual SequenceOperation OldSequence
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	} = new SequenceOperation();


	IMutableAnnotatable IAlterMigrationOperation.OldAnnotations => OldSequence;
}
