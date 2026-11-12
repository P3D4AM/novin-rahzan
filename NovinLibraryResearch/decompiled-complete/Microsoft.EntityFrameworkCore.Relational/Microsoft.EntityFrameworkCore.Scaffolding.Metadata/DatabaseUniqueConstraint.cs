using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

public class DatabaseUniqueConstraint : Annotatable
{
	public virtual DatabaseTable Table
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string Name
	{
		get; [param: JetBrains.Annotations.NotNull]
		set;
	}

	public virtual IList<DatabaseColumn> Columns { get; } = new List<DatabaseColumn>();

}
