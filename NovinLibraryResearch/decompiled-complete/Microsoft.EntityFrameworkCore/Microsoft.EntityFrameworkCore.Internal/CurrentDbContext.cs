using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public sealed class CurrentDbContext : ICurrentDbContext
{
	public DbContext Context
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
	}

	public CurrentDbContext([JetBrains.Annotations.NotNull] DbContext context)
	{
		Context = context;
	}
}
