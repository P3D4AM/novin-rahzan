using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

[Serializable]
public class DbUpdateException : Exception
{
	[NonSerialized]
	private IReadOnlyList<EntityEntry> _entries;

	public virtual IReadOnlyList<EntityEntry> Entries => _entries ?? (_entries = new List<EntityEntry>());

	public DbUpdateException()
	{
	}

	public DbUpdateException([JetBrains.Annotations.NotNull] string message)
		: base(message)
	{
	}

	public DbUpdateException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.CanBeNull] Exception innerException)
		: base(message, innerException)
	{
	}

	public DbUpdateException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.NotNull] IReadOnlyList<IUpdateEntry> entries)
		: this(message, null, entries)
	{
	}

	public DbUpdateException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.CanBeNull] Exception innerException, [JetBrains.Annotations.NotNull] IReadOnlyList<IUpdateEntry> entries)
		: base(message, innerException)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(entries, "entries");
		_entries = (from e in entries
			where e.EntityState != EntityState.Unchanged
			select e.ToEntityEntry()).ToList();
	}

	public DbUpdateException([JetBrains.Annotations.NotNull] SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
