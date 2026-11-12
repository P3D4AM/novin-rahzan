using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.AspNetCore.SignalR.Internal;

public static class HubReflectionHelper
{
	private static readonly Type[] _excludeInterfaces = new Type[1] { typeof(IDisposable) };

	public static IEnumerable<MethodInfo> GetHubMethods(Type hubType)
	{
		MethodInfo[] methods = hubType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
		IEnumerable<MethodInfo> second = _excludeInterfaces.SelectMany((Type i) => GetInterfaceMethods(hubType, i));
		return from m in methods.Except(second)
			where IsHubMethod(m)
			select m;
	}

	private static IEnumerable<MethodInfo> GetInterfaceMethods(Type type, Type iface)
	{
		if (!iface.IsAssignableFrom(type))
		{
			return Enumerable.Empty<MethodInfo>();
		}
		return type.GetInterfaceMap(iface).TargetMethods;
	}

	private static bool IsHubMethod(MethodInfo methodInfo)
	{
		Type declaringType = methodInfo.GetBaseDefinition().DeclaringType;
		if (typeof(object) == declaringType || methodInfo.IsSpecialName)
		{
			return false;
		}
		Type type = (declaringType.GetTypeInfo().IsGenericType ? declaringType.GetGenericTypeDefinition() : declaringType);
		return typeof(Hub) != type;
	}
}
