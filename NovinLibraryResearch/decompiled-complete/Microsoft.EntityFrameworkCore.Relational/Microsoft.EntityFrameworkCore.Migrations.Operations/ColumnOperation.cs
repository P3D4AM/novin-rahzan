using System;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations;

public class ColumnOperation : MigrationOperation
{
	public virtual Type ClrType
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string ColumnType
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual bool? IsUnicode { get; set; }

	public virtual bool? IsFixedLength { get; set; }

	public virtual int? MaxLength { get; set; }

	public virtual bool IsRowVersion { get; set; }

	public virtual bool IsNullable { get; set; }

	public virtual object DefaultValue
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string DefaultValueSql
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string ComputedColumnSql
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string Comment
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}
}
