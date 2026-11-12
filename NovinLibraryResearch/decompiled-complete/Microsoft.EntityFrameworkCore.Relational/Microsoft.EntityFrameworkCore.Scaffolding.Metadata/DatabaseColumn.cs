using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

public class DatabaseColumn : Annotatable
{
	public virtual DatabaseTable Table
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual bool IsNullable { get; set; }

	public virtual string StoreType
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

	public virtual ValueGenerated? ValueGenerated { get; set; }
}
