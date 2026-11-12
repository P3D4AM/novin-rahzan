using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class AccessorExtensions
{
	[DebuggerStepThrough]
	public static TService GetService<TService>([JetBrains.Annotations.NotNull] this IInfrastructure<IServiceProvider> accessor)
	{
		return InfrastructureExtensions.GetService<TService>(Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(accessor, "accessor"));
	}

	[DebuggerStepThrough]
	public static T GetInfrastructure<T>([JetBrains.Annotations.NotNull] this IInfrastructure<T> accessor)
	{
		return Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(accessor, "accessor").Instance;
	}
}
