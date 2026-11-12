using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class TypeExtensions
{
	public static string ShortDisplayName([JetBrains.Annotations.NotNull] this Type type)
	{
		return type.DisplayName(fullName: false);
	}
}
