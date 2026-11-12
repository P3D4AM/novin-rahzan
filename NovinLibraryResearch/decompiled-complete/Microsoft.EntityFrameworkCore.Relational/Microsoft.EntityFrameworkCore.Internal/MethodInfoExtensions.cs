using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public static class MethodInfoExtensions
{
	public static string DisplayName([JetBrains.Annotations.NotNull] this MethodInfo methodInfo)
	{
		return methodInfo.DeclaringType.ShortDisplayName() + "." + methodInfo.Name;
	}
}
