using System.Diagnostics;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

[DebuggerStepThrough]
public static class MethodInfoExtensions
{
	public static bool MethodIsClosedFormOf([JetBrains.Annotations.NotNull] this MethodInfo methodInfo, [JetBrains.Annotations.NotNull] MethodInfo genericMethod)
	{
		if (methodInfo.IsGenericMethod)
		{
			return object.Equals(methodInfo.GetGenericMethodDefinition(), genericMethod);
		}
		return false;
	}
}
