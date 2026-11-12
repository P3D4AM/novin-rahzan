using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore;

[Serializable]
public class DbUpdateConcurrencyException : DbUpdateException
{
	public DbUpdateConcurrencyException()
	{
	}

	public DbUpdateConcurrencyException([JetBrains.Annotations.NotNull] string message)
		: base(message)
	{
	}

	public DbUpdateConcurrencyException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.CanBeNull] Exception innerException)
		: base(message, innerException)
	{
	}

	public DbUpdateConcurrencyException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.CanBeNull] Exception innerException, [JetBrains.Annotations.NotNull] IReadOnlyList<IUpdateEntry> entries)
		: base(message, innerException, entries)
	{
	}

	public DbUpdateConcurrencyException([JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.NotNull] IReadOnlyList<IUpdateEntry> entries)
		: base(message, entries)
	{
	}

	public DbUpdateConcurrencyException([JetBrains.Annotations.NotNull] SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
