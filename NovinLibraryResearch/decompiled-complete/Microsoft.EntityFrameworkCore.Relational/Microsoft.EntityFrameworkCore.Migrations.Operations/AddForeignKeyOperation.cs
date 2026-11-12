using System.Diagnostics;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

[DebuggerDisplay("ALTER TABLE {Table} ADD CONSTRAINT {Name} FOREIGN KEY")]
public class AddForeignKeyOperation : MigrationOperation
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

	public virtual string[] Columns
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string PrincipalSchema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string PrincipalTable
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string[] PrincipalColumns
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual ReferentialAction OnUpdate { get; set; }

	public virtual ReferentialAction OnDelete { get; set; }
}
