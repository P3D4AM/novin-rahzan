using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

internal static class Guard
{
	public static T AgainstNull<T>([NoEnumeration][NotNull] T? argument, [CallerArgumentExpression("argument")] string? paramName = null) where T : class
	{
		if (argument == null)
		{
			throw new ArgumentNullException(paramName);
		}
		return argument;
	}
}
