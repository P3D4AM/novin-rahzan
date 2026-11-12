using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

public class DatabaseModel : Annotatable
{
	public virtual string DatabaseName
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual string DefaultSchema
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual IList<DatabaseTable> Tables { get; } = new List<DatabaseTable>();


	public virtual IList<DatabaseSequence> Sequences { get; } = new List<DatabaseSequence>();

}
