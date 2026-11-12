using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace Ninject.Infrastructure.Language;

internal static class ExtensionsForTargetInvocationException
{
	public static void RethrowInnerException(this TargetInvocationException exception)
	{
		Exception? innerException = exception.InnerException;
		ExceptionDispatchInfo.Capture(innerException).Throw();
		throw innerException;
	}
}
