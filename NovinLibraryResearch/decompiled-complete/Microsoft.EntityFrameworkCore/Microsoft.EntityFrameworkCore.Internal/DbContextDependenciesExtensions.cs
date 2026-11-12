using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class DbContextDependenciesExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDbContextDependencies GetDependencies([JetBrains.Annotations.NotNull] this IDbContextDependencies context)
	{
		return context;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDbContextDependencies GetDependencies([JetBrains.Annotations.NotNull] this ICurrentDbContext currentContext)
	{
		return currentContext.Context;
	}
}
