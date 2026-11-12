using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

public class DatabaseSequence : Annotatable
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

	public virtual string StoreType
	{
		get; [param: JetBrains.Annotations.CanBeNull]
		set;
	}

	public virtual long? StartValue { get; set; }

	public virtual int? IncrementBy { get; set; }

	public virtual long? MinValue { get; set; }

	public virtual long? MaxValue { get; set; }

	public virtual bool? IsCyclic { get; set; }
}
