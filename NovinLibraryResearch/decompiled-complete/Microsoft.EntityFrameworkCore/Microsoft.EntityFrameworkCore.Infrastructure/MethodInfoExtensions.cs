using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public static class MethodInfoExtensions
{
	private static readonly string _efTypeName = typeof(EF).FullName;

	public static bool IsEFPropertyMethod([JetBrains.Annotations.CanBeNull] this MethodInfo methodInfo)
	{
		if (!object.Equals(methodInfo, EF.PropertyMethod))
		{
			if ((object)methodInfo != null && methodInfo.IsGenericMethod && methodInfo.Name == "Property")
			{
				return methodInfo.DeclaringType?.FullName == _efTypeName;
			}
			return false;
		}
		return true;
	}
}
