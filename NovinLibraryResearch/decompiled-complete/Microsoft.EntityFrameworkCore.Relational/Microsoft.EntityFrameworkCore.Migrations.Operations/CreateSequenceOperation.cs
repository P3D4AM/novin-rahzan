using System;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("CREATE SEQUENCE {Name}")]
public class CreateSequenceOperation : SequenceOperation
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

	public virtual Type ClrType
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual long StartValue { get; set; } = 1L;

}
