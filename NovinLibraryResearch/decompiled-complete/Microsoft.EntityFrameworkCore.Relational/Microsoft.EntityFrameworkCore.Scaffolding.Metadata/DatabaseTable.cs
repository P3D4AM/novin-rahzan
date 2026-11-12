using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

public class DatabaseTable : Annotatable
{
	public virtual DatabaseModel Database
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

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

	public virtual string Comment
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual DatabasePrimaryKey PrimaryKey
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual IList<DatabaseColumn> Columns { get; } = new List<DatabaseColumn>();


	public virtual IList<DatabaseUniqueConstraint> UniqueConstraints { get; } = new List<DatabaseUniqueConstraint>();


	public virtual IList<DatabaseIndex> Indexes { get; } = new List<DatabaseIndex>();


	public virtual IList<DatabaseForeignKey> ForeignKeys { get; } = new List<DatabaseForeignKey>();

}
